
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace Knit.Localization.Editor
{
	internal sealed class TableListItem : TreeViewItem
	{
		internal Table Table
		{
			get{ return m_Table; }
		}
		internal string AssetPath
		{
			get{ return m_AssetPath; }
		}
		internal string AssetGUID
		{
			get{ return m_AssetGUID; }
		}
		internal bool IsRemote
		{
			get{ return m_Table.IsRemote; }
			set{ m_Table.IsRemote = value; }
		}
		internal TableListItem( Table table, int id, int depth, string displayName) : base( id, depth, displayName)
		{
			m_Table = table;
			m_AssetPath = AssetDatabase.GetAssetPath( table);
			m_AssetGUID = AssetDatabase.AssetPathToGUID( m_AssetPath);
			icon = AssetDatabase.GetCachedIcon( m_AssetPath) as Texture2D;
		}
        readonly Table m_Table;
		readonly string m_AssetPath;
		readonly string m_AssetGUID;
	}
}
