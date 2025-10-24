
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using System.Collections.Generic;

namespace Knit.Localization.Editor
{
	internal class LocalizeReferencePreview : TreeView
	{
		internal LocalizeReferencePreview( 
			string tableName, string entryName,
			Dictionary<string, Table> tables, 
			System.Action<string, string> callback) : base( new TreeViewState())
		{
			string locale = Localize.kLocaleCodeJa;
			
			if( string.IsNullOrEmpty( locale) == false)
			{
				multiColumnHeader = new LocalizeReferencePreviewColumnHeader( locale);
				multiColumnHeader.ResizeToFit();
			}
			m_Tables = tables;
			m_Callback = callback;
			showAlternatingRowBackgrounds = true;
			showBorder = true;
			
			if( string.IsNullOrEmpty( tableName) == false
			&&	string.IsNullOrEmpty( entryName) == false)
			{
				m_FocusItemKey = $"{tableName}@{entryName}";
			}
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
			int id = 1;
			
			root.AddChild( new LocalizeReferencePreviewItem( string.Empty, string.Empty, id++, 0, string.Empty)
			{
				displayName = "None"
			});
			m_Items = new Dictionary<string, TreeViewItem>();
			
			foreach( var table in m_Tables)
			{
				var item = new TreeViewItem( id++, 0)
				{
					displayName = table.Key,
					icon = AssetDatabase.GetCachedIcon( AssetDatabase.GetAssetPath( table.Value)) as Texture2D
				};
				root.AddChild( item);
				
				foreach( var record in table.Value.Records)
				{
					var entry = new LocalizeReferencePreviewItem( table.Key, record.Key, id++, 1, record.Ja.Replace( "\n", "\\n"))
					{
						displayName = record.Key
					};
					item.AddChild( entry);
					m_Items.Add( $"{table.Key}@{record.Key}", entry);
				}
			}
			SetupDepthsFromParentsAndChildren( root);
			
			return root;
		}
		public override void OnGUI( Rect rect)
		{
			if( Event.current.type == EventType.Layout)
			{
				if( m_FocusItem != null)
				{
					FrameItem( m_FocusItem.id);
					m_FocusItem = null;
				}
				if( string.IsNullOrEmpty( m_FocusItemKey) == false)
				{
					m_Items.TryGetValue( m_FocusItemKey, out m_FocusItem);
					m_FocusItemKey = null;
				}
			}
			base.OnGUI( rect);
		}
		protected override void RowGUI( RowGUIArgs args)
		{
			if( multiColumnHeader == null)
			{
				base.RowGUI( args);
				return;
			}
			int columnCount = args.GetNumVisibleColumns();
			
			for( int i0 = 0; i0 < columnCount; ++i0)
			{
				var cellRect = args.GetCellRect( i0);
				var columnIndex = args.GetColumn( i0);
				
				switch( columnIndex)
				{
					case 1:
					{
						if( args.item is LocalizeReferencePreviewItem item)
						{
							EditorGUI.LabelField( cellRect, item.Preview);
						}
						break;
					}
					default:
					{
						base.RowGUI( args);
						break;
					}
				}
			}
		}
		protected override void SelectionChanged( IList<int> selectedIds)
		{
			if( selectedIds.Count > 0)
			{
				var selected = FindItem( selectedIds[ 0], rootItem);
				
				if( selected is LocalizeReferencePreviewItem item)
				{
					m_Callback?.Invoke( item.TableKey, item.EntryKey);
					return;
				}
				if( selected.hasChildren != false)
				{
					SetExpanded( selected.id, !IsExpanded( selected.id));
				}
				SetSelection( new int[]{});
			}
		}
		protected override bool CanMultiSelect( TreeViewItem item)
		{
			return false;
		}
        readonly System.Action<string, string> m_Callback;
        readonly Dictionary<string, Table> m_Tables;
		Dictionary<string, TreeViewItem> m_Items;
		TreeViewItem m_FocusItem;
		string m_FocusItemKey;
	}
}
