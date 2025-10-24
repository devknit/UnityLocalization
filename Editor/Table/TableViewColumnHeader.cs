
using UnityEditor;
using UnityEngine;
using UnityEditor.IMGUI.Controls;
using System.Linq;
using System.Collections.Generic;

namespace Knit.Localization.Editor
{
	internal sealed class TableViewColumnHeader : MultiColumnHeader
	{
		static readonly Dictionary<string, string> kLocaleLanguageCodeToNames = new()
		{
			{ "ja", "Japanese" },
			{ "en", "English" },
			{ "de", "German" },
			{ "es", "Spanish" },
			{ "fr", "French" },
			{ "it", "Italian" },
			{ "ko", "Korean" },
			{ "zh-Hans", "Chinese Simplified" },
			{ "zh-Hant", "Chinese Traditional" },
		};
		static MultiColumnHeaderState CreateState( IEnumerable<string> locales)
		{
			var columns = new List<MultiColumnHeaderState.Column>
            {
                new()
				{
                    headerContent = new GUIContent( "Key", "Key"),
                    headerTextAlignment = TextAlignment.Center,
                    canSort = true,
                    width = 250,
                    minWidth = 100,
                    autoResize = false,
                    allowToggleVisibility = false,
                }
            };
			foreach( var locale in locales)
			{
				if( kLocaleLanguageCodeToNames.TryGetValue( locale, out string name) == false)
				{
					name = locale;
				}
				columns.Add( new MultiColumnHeaderState.Column
				{
					headerContent = new GUIContent( name, locale),
					headerTextAlignment = TextAlignment.Center,
					canSort = false,
					width = 100, 
					minWidth = 100,
					autoResize = true,
					allowToggleVisibility = true
				});
			}
			return new MultiColumnHeaderState( columns.ToArray());
		}
		internal TableViewColumnHeader( IEnumerable<string> locales) : base( CreateState( locales))
		{
		}
		internal void SetCallbacks(
			GenericMenu.MenuFunction addEntry,
			GenericMenu.MenuFunction clearEntries,
			GenericMenu.MenuFunction2 export,
			GenericMenu.MenuFunction2 import)
		{
			m_AddEntry = addEntry;
			m_ClearEntries = clearEntries;
			m_Export = export;
			m_Import = import;
		}
		protected override void AddColumnHeaderContextMenuItems( GenericMenu menu)
		{
			menu.AddItem( EditorGUIUtility.TrTextContent( "Resize to Fit"), false, ResizeToFit);
			
			if( sortedColumnIndex >= 0)
			{
				menu.AddItem( EditorGUIUtility.TrTextContent( "Default Order"), false, () => { sortedColumnIndex = -1; });
			}
			else
			{
				menu.AddDisabledItem( EditorGUIUtility.TrTextContent( "Default Order"));
			}
			menu.AddSeparator( string.Empty);
			menu.AddItem( EditorGUIUtility.TrTextContent( "Add New Entry"), false, m_AddEntry);
			menu.AddItem( EditorGUIUtility.TrTextContent( "Clear Entries"), false, m_ClearEntries);
			menu.AddSeparator( string.Empty);
			menu.AddItem( EditorGUIUtility.TrTextContent( "Export"), false, m_Export, null);
			menu.AddItem( EditorGUIUtility.TrTextContent( "Import"), false, m_Import, null);
			menu.AddSeparator( string.Empty);
			
			for( int i0 = 0; i0 < state.columns.Length; ++i0)
			{
				MultiColumnHeaderState.Column column = state.columns[ i0];
				string menuText = !string.IsNullOrEmpty( column.contextMenuText)?
					column.contextMenuText : column.headerContent.text;
				
				if( column.allowToggleVisibility != false)
				{
					menu.AddItem( EditorGUIUtility.TrTextContent( $"Visibility/{menuText}"), state.visibleColumns.Contains( i0), ToggleVisibility, i0);
				}
				else
				{
					menu.AddDisabledItem( EditorGUIUtility.TrTextContent( $"Visibility/{menuText}"));
				}
			}
		}
		void ToggleVisibility( object userData)
		{
			base.ToggleVisibility( (int)userData);
		}
		GenericMenu.MenuFunction m_AddEntry;
		GenericMenu.MenuFunction m_ClearEntries;
		GenericMenu.MenuFunction2 m_Export;
		GenericMenu.MenuFunction2 m_Import;
	}
}
