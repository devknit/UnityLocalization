
using System.Text;
using UnityEditor.IMGUI.Controls;

namespace Knit.Localization.Editor
{
	public sealed class TableViewItem : TreeViewItem
	{
		public string Key
		{
			get{ return m_Key; }
		}
		public Table Table
		{
			get{ return m_Table; }
		}
		public TableViewItem( string key, Table table, int id) : base( id, 0)
		{
			m_Key = key;
			m_Table = table;
			UpdateString();
		}
		public string GetValue( string column)
		{
			if( column == "Key")
			{
				return m_Key;
			}
			return m_Table.FindRecord( m_Key)?.GetString( column) ?? string.Empty;
		}
		public void SetValue( string column, string value)
		{
			if( column == "Key")
			{
				if( m_Table.ChangeKey( m_Key, value) != false)
				{
					m_Key = value;
				}
			}
			else
			{
				m_Table.SetValue( m_Key, column, value);
			}
			UpdateString();
		}
		void UpdateString()
		{
			var builder = new StringBuilder();
			
			builder.AppendLine( m_Key);
			
			Record record = m_Table.FindRecord( m_Key);
			if( record != null)
			{
				builder.AppendLine( record.Ja);
				builder.AppendLine( record.En);
			#if LOCALE_LANGUAGE_DE
				builder.AppendLine( record.De);
			#endif
			#if LOCALE_LANGUAGE_ES
				builder.AppendLine( record.Es);
			#endif
			#if LOCALE_LANGUAGE_FR
				builder.AppendLine( record.Fr);
			#endif
			#if LOCALE_LANGUAGE_IT
				builder.AppendLine( record.It);
			#endif
			#if LOCALE_LANGUAGE_KO
				builder.AppendLine( record.Ko);
			#endif
			#if LOCALE_LANGUAGE_ZH_HANS
				builder.AppendLine( record.ZhHans);
			#endif
			#if LOCALE_LANGUAGE_ZH_HANT
				builder.AppendLine( record.ZhHant);
			#endif
			}
			displayName = builder.ToString();
		}
		string m_Key;
        readonly Table m_Table;
	}
}
