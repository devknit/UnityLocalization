
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Knit.Localization.Editor
{
	public class TableWindow : EditorWindow
	{
		[MenuItem( "Tools/Localization/Table")]
		internal static void Open()
		{
			GetWindow<TableWindow>( false, "Localization Tables", true).Show();
		}
		internal static void Open( string tableSelectName, string entryKey)
		{
			var window = GetWindow<TableWindow>( false, "Localization Tables", true);
			window.UpdateTable( tableSelectName);
			window.SetSelectAndFocus( entryKey);
			window.Show();
		}
		void OnEnable()
		{
			minSize = new Vector2( 900, 520);
			m_TableList = new TableList( m_TableListState, UpdateTable, SelectTable);
			m_TableView = new TableView( m_TableViewState, Localize.GetLocales());
			UpdateTable( SessionState.GetString( kTableSelectName, string.Empty));
		}
		void OnGUI()
		{
			if( m_TableKeys.Length > 0)
			{
				EditorGUILayout.BeginHorizontal();
				{
					EditorGUI.BeginChangeCheck();
					int tableSelectIndex = EditorGUILayout.Popup( m_TableSelectIndex, m_TableKeys);
					if( EditorGUI.EndChangeCheck() != false)
					{
						if( m_Tables.TryGetValue( m_TableKeys[ tableSelectIndex], out Table table) != false)
						{
							m_TableView.Setup( table, m_Tables);
						}
						SessionState.SetString( kTableSelectName, m_TableKeys[ tableSelectIndex]);
						m_TableSelectIndex = tableSelectIndex;
					}
					m_TableShow = EditorGUILayout.Toggle( m_TableShow, GUILayout.Width( 20));
					
					if( GUILayout.Button( EditorGUIUtility.IconContent( "d_CreateAddNew@2x"), EditorStyles.toolbarButton, GUILayout.Width( 30)) != false)
					{
						CreateTable();
					}
				}
				EditorGUILayout.EndHorizontal();
			}
			else
			{
				GUILayout.FlexibleSpace();
				EditorGUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				
				if( GUILayout.Button( "Create New Table") != false)
				{
					CreateTable();
				}
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
			}
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.BeginVertical();
				{
					if( m_TableKeys.Length > 0)
					{
						using( var scope = new EditorGUILayout.VerticalScope( GUILayout.ExpandHeight( true)))
						{
							m_TableView.OnGUI( scope.rect);
						}
					}
				}
				EditorGUILayout.EndVertical();
				
				if( m_TableShow != false && (m_TableKeys?.Length ?? 0) > 0)
				{
					EditorGUILayout.BeginVertical( GUILayout.Width( 200));
					{
						using( var scope = new EditorGUILayout.VerticalScope( GUILayout.ExpandHeight( true)))
						{
							m_TableList.OnGUI( scope.rect);
						}
					}
					EditorGUILayout.EndVertical();
				}
			}
			EditorGUILayout.EndHorizontal();
		}
		void CreateTable()
		{
			string tablePath = EditorUtility.SaveFilePanel( "Create Table Table", 
				SessionState.GetString( GetType().ToString(), Application.dataPath), string.Empty, "asset");
			
			if( tablePath.StartsWith( Application.dataPath) != false)
			{
				SessionState.SetString( GetType().ToString(), Path.GetDirectoryName( tablePath));
				tablePath = tablePath.Replace( Application.dataPath, "Assets");
				UpdateTable( Table.CreateAsset( tablePath));
			}
		}
		void SelectTable( Table table)
		{
			if( m_TableKeys.Length > 0)
			{
				for( int i0 = 0; i0 < m_TableKeys.Length; ++i0)
				{
					if( m_TableKeys[ i0] == table.Key)
					{
						m_TableView.Setup( table, m_Tables);
						SessionState.SetString( kTableSelectName, m_TableKeys[ i0]);
						m_TableSelectIndex = i0;
						break;
					}
				}
			}
		}
		void UpdateTable( Table table)
		{
			UpdateTable( table?.Key ?? SessionState.GetString( kTableSelectName, string.Empty));
		}
		void UpdateTable( string tableName)
		{
			m_Tables = Table.GetOrCreateTables();
			m_TableKeys = m_Tables.Keys.ToArray();
			
			if( m_TableKeys.Length > 0)
			{
				int i0;
				
				for( i0 = 0; i0 < m_TableKeys.Length; ++i0)
				{
					if( m_TableKeys[ i0] == tableName)
					{
						break;
					}
				}
				if( i0 >= m_TableKeys.Length)
				{
					i0 = 0;
				}
				m_TableList?.Setup( m_Tables.Values.ToList());
				
				if( m_TableView != null)
				{
					if( m_Tables.TryGetValue( m_TableKeys[ i0], out Table table) != false)
					{
						m_TableView.Setup( table, m_Tables);
					}
				}
				SessionState.SetString( kTableSelectName, m_TableKeys[ i0]);
				m_TableSelectIndex = i0;
			}
		}
		void SetSelectAndFocus( string entryKey)
		{
			m_TableView?.SetSelectAndFocus( entryKey);
		}
		const string kTableSelectName = "Knit.Localization.Editor.TableWindow.TableSelectName";
		
		[SerializeField]
		bool m_TableShow = true;
		[SerializeField]
		TreeViewState m_TableViewState = new();
		[SerializeField]
		TreeViewState m_TableListState = new();
		
		[System.NonSerialized]
		TableView m_TableView;
		[System.NonSerialized]
		TableList m_TableList;
		[System.NonSerialized]
		int m_TableSelectIndex;
		[System.NonSerialized]
		string[] m_TableKeys;
		[System.NonSerialized]
		Dictionary<string, Table> m_Tables;
	}
}
