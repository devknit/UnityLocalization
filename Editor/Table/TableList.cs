
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Knit.Localization.Editor
{
	internal sealed class TableList : TreeView, System.IDisposable
	{
		internal void Setup( List<Table> tables)
		{
			m_Tables = tables;
			Refresh();
		}
		internal TableList( TreeViewState treeViewState, 
			System.Action<Table> onUpdateTable, System.Action<Table> onSelectTable)
			: base( treeViewState, new TableListColumnHeader()) 
		{
			m_OnUpdateTable = onUpdateTable;
			m_OnSelectTable = onSelectTable;
			m_SearchField = new SearchField();
			m_SearchField.downOrUpArrowKeyPressed += SetFocusAndEnsureSelectedItem;
			Undo.undoRedoPerformed += UndoRedoPerformed;
			
			showBorder = true;
			showAlternatingRowBackgrounds = true;
			multiColumnHeader.sortingChanged += header => Refresh();
			multiColumnHeader.ResizeToFit();
		}
		~TableList()
		{
			Dispose();
		}
		public void Dispose()
		{
			Undo.undoRedoPerformed -= UndoRedoPerformed;
		}
		void UndoRedoPerformed()
		{
			Refresh();
		}
		internal void Refresh()
		{
			Reload();
		}
		protected override TreeViewItem BuildRoot()
		{
			var root = new TreeViewItem
			{
				id = 0,
				depth = -1,
				displayName = string.Empty
			};
			var items = new Dictionary<string, TreeViewItem>();
			int maxDepth = 0;
			int id = 1;
			
			for( int i0 = 0; i0 <= maxDepth; ++i0)
			{
				foreach( var table in m_Tables)
				{
					string[] keys = table.Key.Split( '/');
					int depth = keys.Length - 1;
					
					if( i0 == 0)
					{
						if( maxDepth < depth)
						{
							maxDepth = depth;
						}
					}
					if( depth == i0)
					{
						var parent = GetParent( root, items, keys, depth, ref id);
						var item = new TableListItem( 
							table, id++, depth, keys[ depth]);
						parent.AddChild( item);
						items.Add( table.Key, item);
					}
				}
			}
			if( multiColumnHeader.sortedColumnIndex == 0
			&&	multiColumnHeader.IsSortedAscending( multiColumnHeader.sortedColumnIndex) == false)
			{
				OrderByDescending( root);
			}
			else
			{
				OrderBy( root);
			}
			return root;
		}
		TreeViewItem GetParent( TreeViewItem root, Dictionary<string, TreeViewItem> items, string[] keys, int depth, ref int id)
		{
			if( depth <= 0)
			{
				return root;
			}
			string parentKey = string.Join( "/", keys, 0, depth);
			
            if( items.TryGetValue( parentKey, out TreeViewItem item) == false)
            {
                var parent = GetParent( root, items, keys, depth - 1, ref id);
				
                item = new TreeViewItem( id++, depth - 1, keys[depth - 1])
                {
                    icon = EditorGUIUtility.Load( "d_FolderEmpty Icon") as Texture2D
                };
                items.Add( parentKey, item);
                parent.AddChild( item);
            }
            return item;
		}
		void OrderBy( TreeViewItem root)
		{
			root.children = root.children.OrderBy( x => x.displayName).ToList();
			
			foreach( var child in root.children)
			{
				if( child is not TableListItem)
				{
					OrderBy( child);
				}
			}
		}
		void OrderByDescending( TreeViewItem root)
		{
			root.children = root.children.OrderByDescending( x => x.displayName).ToList();
			
			foreach( var child in root.children)
			{
				if( child is not TableListItem)
				{
					OrderByDescending( child);
				}
			}
		}
		public override void OnGUI( Rect rect)
		{
			var searchRect = new Rect( rect.x + 4, rect.y + 3, 
				rect.width - 8, EditorGUIUtility.singleLineHeight);
				
			EditorGUI.BeginChangeCheck();
			string searchValue = m_SearchField.OnToolbarGUI( searchRect, searchString);
			if( EditorGUI.EndChangeCheck() != false)
			{
				searchString = searchValue;
			}
			rect.yMin += searchRect.height + EditorGUIUtility.standardVerticalSpacing * 2;
			
			base.OnGUI( rect);
			OnEvent( Event.current, treeViewRect);
		}
		void OnEvent( Event ev, Rect rect)
		{
			if( ev.type == EventType.MouseUp && ev.button == 1)
			{
				if( rect.Contains( ev.mousePosition) != false)
				{
					int selectedCount = state.selectedIDs.Count;
					
					if( selectedCount > 0)
					{
						List<int> ids = state.selectedIDs;
						var items = GetRows().Where( x => ids.Contains( x.id)).Cast<TableListItem>();
						var contextMenu = new GenericMenu();
						
						if( selectedCount == 1)
						{
							contextMenu.AddItem( new GUIContent( "Select"), false,
							() =>
							{
								m_OnSelectTable?.Invoke( items.First().Table);
							});
							contextMenu.AddItem( new GUIContent( "Rename"), false,
							() =>
							{
								BeginRename( items.First());
							});
							contextMenu.AddItem( new GUIContent( "Ping"), false,
							() =>
							{
								EditorGUIUtility.PingObject( items.First().Table);
							});
							contextMenu.AddItem( new GUIContent( "Show in Explorer"), false, () =>
							{
								EditorUtility.RevealInFinder( AssetDatabase.GetAssetPath( items.First().Table));
							});
						}
						contextMenu.AddItem( new GUIContent( "Copy Path"), false, () =>
						{
							var builder = new System.Text.StringBuilder();
							
							foreach( var item in items)
							{
								builder.AppendLine( item.AssetPath);
							}
							EditorGUIUtility.systemCopyBuffer = builder.ToString();
						});
						contextMenu.AddItem( new GUIContent( "Copy Guid"), false, () =>
						{
							var builder = new System.Text.StringBuilder();
							
							foreach( var item in items)
							{
								builder.AppendLine( item.AssetGUID);
							}
							EditorGUIUtility.systemCopyBuffer = builder.ToString();
						});
						contextMenu.ShowAsContext();
						ev.Use();
					}
				}
			}
		}
		protected override void RowGUI( RowGUIArgs args)
		{
			if( args.item is TableListItem node)
			{
				var rect = args.rowRect;
				rect.xMin += rect.width - 20;
				
				EditorGUI.BeginChangeCheck();
				bool isRemote = EditorGUI.Toggle( rect, node.IsRemote);
				if( EditorGUI.EndChangeCheck() != false)
				{
					IList<int> selection = GetSelection();
					
					if( selection.Contains( node.id) == false)
					{
						node.IsRemote = isRemote;
					}
					else
					{
						bool propagate = selection.Count == 1 && Event.current.alt;
						
						foreach( var id in selection)
						{
							if( FindItem( id, rootItem) is TableListItem item)
							{
								item.IsRemote = isRemote;
							}
						}
					}
				}
			}
			base.RowGUI( args);
		}
		protected override void DoubleClickedItem( int id)
		{
			if( FindItem( id, rootItem) is TableListItem item)
			{
				m_OnSelectTable?.Invoke( item.Table);
			}
			base.DoubleClickedItem( id);
		}
		protected override bool CanRename( TreeViewItem item)
		{
			return true;
		}
		protected override void RenameEnded( RenameEndedArgs args)
		{
			if( args.acceptedRename != false)
			{
				TreeViewItem item = FindItem( args.itemID, rootItem);
				
				if( RenameKeys( item, args.originalName, args.newName) != false)
				{
					m_OnUpdateTable?.Invoke( (item as TableListItem)?.Table);
				}
			}
		}
		bool RenameKeys( TreeViewItem item, string originalName, string newName)
		{
			bool result = false;
			
			if( item is TableListItem tableItem)
			{
				if( tableItem.Table.RenameKey( originalName, newName) != false)
				{
					result = true;
				}
			}
			if( item.children != null)
			{
				if( originalName.EndsWith( "/") == false)
				{
					originalName += "/";
				}
				if( string.IsNullOrEmpty( newName) == false)
				{
					if( newName.EndsWith( "/") == false)
					{
						newName += "/";
					}
				}
				foreach( var child in item.children)
				{
					if( RenameKeys( child, originalName, newName) != false)
					{
						result = true;
					}
				}
			}
			return result;
		}
		List<Table> m_Tables;
        readonly SearchField m_SearchField;
        readonly System.Action<Table> m_OnUpdateTable;
        readonly System.Action<Table> m_OnSelectTable;
	}
}
