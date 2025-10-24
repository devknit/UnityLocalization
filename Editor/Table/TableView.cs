
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Knit.Serializer.Editor;

namespace Knit.Localization.Editor
{
	internal class TableView : TreeView, System.IDisposable
	{
		internal void Setup( Table table, Dictionary<string, Table> tables)
		{
			m_Table = table;
			m_Tables = tables;
			Reload();
		}
		internal void SetSelectAndFocus( string entryKey)
		{
			m_FocusItemKey = entryKey;
		}
		internal void AddEntry()
		{
			if( m_Table != null)
			{
				m_Table.AddEntry();
				Reload();
			}
		}
		internal void ClearEntries()
		{
			if( m_Table != null)
			{
				m_Table.ClearEntries();
				Reload();
			}
		}
		internal void ExportCSV( object arg)
		{
			if( m_Table != null)
			{
				string exportPath = EditorUtility.SaveFilePanel( "Export CSV", 
					SessionState.GetString( GetType().ToString(), Application.dataPath), m_Table.name, "csv");
				
				if( string.IsNullOrEmpty( exportPath) == false)
				{
					SessionState.SetString( GetType().ToString(), Path.GetDirectoryName( exportPath));
					
					if( Utility.ExportCSV( m_Table, exportPath) != false)
					{
						EditorUtility.RevealInFinder( exportPath);
					}
				}
			}
		}
		internal void ImportCSV( object arg)
		{
			if( m_Table != null)
			{
				string importPath = EditorUtility.OpenFilePanel( "Import CSV", 
					SessionState.GetString( GetType().ToString(), Application.dataPath), "csv");
				
				if( string.IsNullOrEmpty( importPath) == false)
				{
					SessionState.SetString( GetType().ToString(), Path.GetDirectoryName( importPath));
					if( Utility.ImportCSV( m_Table, importPath) != false)
					{
						Reload();
					}
				}
			}
		}
		internal TableView( TreeViewState treeViewState, IEnumerable<string> locales)
			: base( treeViewState, new TableViewColumnHeader( locales)) 
		{
			m_SearchField = new SearchField();
			m_SearchField.downOrUpArrowKeyPressed += SetFocusAndEnsureSelectedItem;
			Undo.undoRedoPerformed += UndoRedoPerformed;
			
			showBorder = true;
			showAlternatingRowBackgrounds = true;
			rowHeight = 80;//EditorStyles.textArea.lineHeight;
			multiColumnHeader.sortingChanged += header => Reload();
			multiColumnHeader.ResizeToFit();
			(multiColumnHeader as TableViewColumnHeader).SetCallbacks( 
				AddEntry,
				ClearEntries,
				ExportCSV,
				ImportCSV);
		}
		~TableView()
		{
			Dispose();
		}
		public void Dispose()
		{
			Undo.undoRedoPerformed -= UndoRedoPerformed;
		}
		protected void UndoRedoPerformed()
		{
			Reload();
		}
		protected override TreeViewItem BuildRoot()
		{
			var root = new TreeViewItem{ id = -1, depth = -1, displayName = string.Empty };
			var items = new List<TreeViewItem>();
			
			foreach( var key in m_Table.Keys)
			{
				items.Add( new TableViewItem( key, m_Table, items.Count));
			}
			if( multiColumnHeader.sortedColumnIndex == 0)
			{
				if( multiColumnHeader.IsSortedAscending( multiColumnHeader.sortedColumnIndex) != false)
				{
					items = items.OrderBy( x => (x as TableViewItem).Key).ToList();
				}
				else
				{
					items = items.OrderByDescending( x => (x as TableViewItem).Key).ToList();
				}
			}
			SetupParentsAndChildrenFromDepths( root, items);
			return root;
		}
		public override void OnGUI( Rect rect)
		{
			var ev = Event.current;
			
			if( ev.type == EventType.KeyDown)
			{
				if( ev.shift != false)
				{
					if( ev.keyCode == KeyCode.C)
					{
					#if UNITY_EDITOR_WIN
						if( ev.control != false)
					#else
						if( ev.command != false)
					#endif
						{
							CopyKeyToClipBoard();
						}
						else
						{
							// CopyRecordToClipBoard();
						}
						ev.Use();
					}
					else if( ev.keyCode == KeyCode.V)
					{
						// PasteRecordToClipBoard();
						ev.Use();
					}
				}
			}
			if( ev.type == EventType.Layout)
			{
				if( m_FocusItem != null)
				{
					SetSelection( new List<int>{ m_FocusItem.id});
					FrameItem( m_FocusItem.id);
					SetFocus();
					m_FocusItem = null;
				}
				if( string.IsNullOrEmpty( m_FocusItemKey) == false)
				{
					m_FocusItem = rootItem.children.FirstOrDefault( x =>
					{
						if( x is TableViewItem item)
						{
							return item.Key == m_FocusItemKey;
						}
						return false;
					});
					m_FocusItemKey = null;
				}
			}
			var createRect = new Rect( rect.x + 0, rect.y + 2, 
				30, EditorGUIUtility.singleLineHeight);
			var exportRect = new Rect( rect.xMax - 30, rect.y + 2,
				30, EditorGUIUtility.singleLineHeight);
			var importRect = new Rect( rect.xMax - 60, rect.y + 2,
				30, EditorGUIUtility.singleLineHeight);
			var searchRect = new Rect( rect.x + 34, rect.y + 3, 
				rect.width - 96, EditorGUIUtility.singleLineHeight);
				
			if( GUI.Button( createRect, EditorGUIUtility.IconContent( "d_CreateAddNew@2x", "Add entry"), EditorStyles.toolbarButton) != false)
			{
				AddEntry();
			}
			if( GUI.Button( exportRect, EditorGUIUtility.IconContent( "d_SaveAs@2x", "Export"), EditorStyles.toolbarButton) != false)
			{
				ExportCSV( null);
			}
			if( GUI.Button( importRect, EditorGUIUtility.IconContent( "d_Import@2x", "Import"), EditorStyles.toolbarButton) != false)
			{
				ImportCSV( null);
			}
			EditorGUI.BeginChangeCheck();
			string searchValue = m_SearchField.OnToolbarGUI( searchRect, searchString);
			if( EditorGUI.EndChangeCheck() != false)
			{
				searchString = searchValue;
			}
			rect.yMin += searchRect.height + EditorGUIUtility.standardVerticalSpacing * 2;
			
			base.OnGUI( rect);
		}
		protected override void RowGUI( RowGUIArgs args)
		{
			m_TextAreaStyle ??= new GUIStyle( "TextField")
			{
				wordWrap = true
			};
			if( args.item is TableViewItem item)
			{
				int columnCount = args.GetNumVisibleColumns();
				
				for( int i0 = 0; i0 < columnCount; ++i0)
				{
					var cellRect = args.GetCellRect( i0);
					var columnIndex = args.GetColumn( i0);
					var column = multiColumnHeader.GetColumn( columnIndex);
					var columnName = column.headerContent.tooltip;
					string value = item.GetValue( columnName);
					
					cellRect.yMin += EditorGUIUtility.standardVerticalSpacing / 2;
					cellRect.yMax -= EditorGUIUtility.standardVerticalSpacing / 2;
					
					if( columnName == "Key")
					{
						Rect buttonRect = cellRect;
						buttonRect.xMax -= buttonRect.width - 20;
						buttonRect.x += 2;
						
						if( GUI.Button( buttonRect, "-") != false)
						{
							m_Table.RemoveEntry( value);
							Reload();
							break;
						}
						Rect eventRect = cellRect;
						eventRect.xMin += buttonRect.width;
						eventRect.yMin += EditorGUIUtility.singleLineHeight;
						OnEvent( Event.current, eventRect);
						
						cellRect.xMin += 23;
						cellRect.xMax -= 2;
						cellRect.height = EditorGUIUtility.singleLineHeight;
						
						EditorGUI.BeginChangeCheck();
						value = EditorGUI.TextField( cellRect, value);
					}
					else
					{
						EditorGUI.BeginChangeCheck();
						value = EditorGUI.TextArea( cellRect, value, m_TextAreaStyle);
					}
					if( EditorGUI.EndChangeCheck() != false)
					{
						item.SetValue( columnName, value);
					}
				}
			}
		}
		void OnEvent( Event ev, Rect rect)
		{
			if( ev.type == EventType.MouseUp && ev.button == 1)
			{
				if( rect.Contains( ev.mousePosition) != false)
				{
					var contextMenu = new GenericMenu();
					
					contextMenu.AddItem( new GUIContent( "Clipboard/Copy key only %#c"), false, CopyKeyToClipBoard);
					// contextMenu.AddItem( new GUIContent( "Clipboard/Copy #c"), false, CopyRecordToClipBoard);
					// contextMenu.AddItem( new GUIContent( "Clipboard/Paste #v"), false, PasteRecordToClipBoard);
					contextMenu.AddItem( new GUIContent( "Add"), false, () =>
					{
						AddEntry();
					});
					foreach( var table in m_Tables)
					{
						if( table.Key != m_Table.Key)
						{
							// contextMenu.AddItem( new GUIContent( $"Copy/{table.Key}"), false, 
							// 	(arg) =>
							// 	{
							// 		if( arg is Table table)
							// 		{
							// 			IList<int> selection = GetSelection();
										
							// 			if( selection.Count > 0)
							// 			{
							// 				foreach( var id in selection)
							// 				{
							// 					if( FindItem( id, rootItem) is TableViewItem item)
							// 					{
							// 						table.CopyEntry( m_Table, item.key);
							// 					}
							// 				}
							// 				Reload();
							// 			}
							// 		}
							// 	}, table.Value);
							// contextMenu.AddItem( new GUIContent( $"Move/{table.Key}"), false, 
							// 	(arg) =>
							// 	{
							// 		if( arg is Table table)
							// 		{
							// 			IList<int> selection = GetSelection();
										
							// 			if( selection.Count > 0)
							// 			{
							// 				foreach( var id in selection)
							// 				{
							// 					if( FindItem( id, rootItem) is TableViewItem item)
							// 					{
							// 						table.CopyEntry( m_Table, item.key);
							// 						m_Table.RemoveEntry( item.key);
							// 					}
							// 				}
							// 				Reload();
							// 			}
							// 		}
							// 	}, table.Value);
						}
					}
					contextMenu.AddItem( new GUIContent( "Remove"), false, () =>
					{
						IList<int> selection = GetSelection();
						
						if( selection.Count > 0)
						{
							foreach( var id in selection)
							{
								if( FindItem( id, rootItem) is TableViewItem item)
								{
									m_Table.RemoveEntry( item.Key);
								}
							}
							SetSelection( new List<int>());
							Reload();
						}
					});
					contextMenu.ShowAsContext();
					ev.Use();
				}
			}
		}
		void CopyKeyToClipBoard()
		{
			IList<int> selection = GetSelection();
			
			if( selection.Count > 0)
			{
				if( FindItem( selection[ 0], rootItem) is TableViewItem item)
				{
					GUIUtility.systemCopyBuffer = $"{m_Table.Key}@{item.Key}";
				}
			}
		}
		// void CopyRecordToClipBoard()
		// {
		// 	IList<int> selection = GetSelection();
			
		// 	if( selection.Count > 0)
		// 	{
		// 		if( FindItem( selection[ 0], rootItem) is TableViewItem item)
		// 		{
		// 			var json = new Dictionary<string, object>();
					
		// 			json.Add( "Table", m_Table.key);
		// 			json.Add( "Entry", item.key);
					
		// 			foreach( var table in m_Table.tables)
		// 			{
		// 				var record = table.records.FirstOrDefault( x => x.key == item.key);
		// 				string value = record?.value ?? string.Empty;
		// 				json.Add( table.locale, value);
		// 			}
		// 			GUIUtility.systemCopyBuffer = MiniJSON.Json.Serialize( json);
		// 		}
		// 	}
		// }
		// void PasteRecordToClipBoard()
		// {
		// 	IList<int> selection = GetSelection();
			
		// 	if( selection.Count > 0)
		// 	{
		// 		if( MiniJSON.Json.Deserialize( GUIUtility.systemCopyBuffer) is Dictionary<string, object> json)
		// 		{
		// 			foreach( var id in selection)
		// 			{
		// 				if( FindItem( id, rootItem) is TableViewItem item)
		// 				{
		// 					foreach( var table in m_Table.tables)
		// 					{
		// 						foreach( var element in json)
		// 						{
		// 							if( element.Value is string value)
		// 							{
		// 								m_Table.SetValue( item.key, element.Key, value);
		// 							}
		// 						}
		// 					}
		// 				}
		// 			}
		// 		}
		// 	}
		// }
        readonly SearchField m_SearchField;
		GUIStyle m_TextAreaStyle;
		Table m_Table;
		Dictionary<string, Table> m_Tables;
		TreeViewItem m_FocusItem;
		string m_FocusItemKey;
	}
}
