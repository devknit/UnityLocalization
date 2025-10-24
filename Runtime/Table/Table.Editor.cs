#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace Knit.Localization
{
	public sealed partial class Table : ScriptableObject
	{
		public static Dictionary<string, Table> GetOrCreateTables()
		{
			string[] GUIDs = AssetDatabase.FindAssets( $"t:{typeof( Table)}");
			var tables = new Dictionary<string, Table>();
			Table table;
			
			foreach( var GUID in GUIDs)
			{
				table = 
					AssetDatabase.LoadAssetAtPath<Table>( 
						AssetDatabase.GUIDToAssetPath( GUID));
				
				if( table != null)
				{
					string tableName = table.Key;
					
					if( string.IsNullOrWhiteSpace( tableName) != false)
					{
						tableName = table.name;
					}
					if( tables.ContainsKey( tableName) == false)
					{
						// table.records.ForEach( x => 
						// {
						// 	if( x.Clean() != false)
						// 	{
						// 		EditorUtility.SetDirty( table);
						// 	}
						// });
						tables.Add( tableName, table);
					}
				}
			}
			s_Tables = tables;
			return s_Tables;
		}
		public static string CreateAsset( string tablePath)
		{
			string fileName = Path.GetFileNameWithoutExtension( tablePath);
			var table = ScriptableObject.CreateInstance<Table>();
			table.m_Key = fileName;
			AssetDatabase.CreateAsset( table, tablePath);
			s_Tables = null;
			
			return fileName;
		}
		public bool RenameKey( string originalName, string newName)
		{
			string newKey = m_Key.Replace( originalName, newName);
			
			while( newKey.Contains( "//") != false)
			{
				newKey = newKey.Replace( "//", "/");
			}
			List<string> newKeys = newKey.Split( '/').ToList();
			
			for( int i0 = 0; i0 < newKeys.Count; ++i0)
			{
				if( newKeys[ i0] == "..")
				{
					if( i0 > 0)
					{
						newKeys.RemoveAt( --i0);
					}
					newKeys.RemoveAt( i0--);
				}
				else if( newKeys[ i0] == ".")
				{
					newKeys.RemoveAt( i0--);
				}
			}
			newKey = string.Join( "/", newKeys.ToArray());
			
			if( string.IsNullOrWhiteSpace( newKey) == false)
			{
				m_Key = newKey;
				EditorUtility.SetDirty( this);
				s_Tables = null;
				return true;
			}
			return false;
		}
		public void ClearEntries()
		{
			RecordObjects( "Clear Entries");
			m_Records.Clear();
			EditorUtility.SetDirty( this);
		}
		public void AddEntry( string key = null)
		{
			RecordObjects( "Add Entry");
			
			if( string.IsNullOrEmpty( key) != false)
			{
				key = "New Entry";
			}
			string newKey = key;
			int count = 1;
			
			while( m_Records.Any( x => x.Key == newKey) != false)
			{
				newKey = $"{key} {count++}";
			}
			m_Records.Add( new Record( newKey));
			EditorUtility.SetDirty( this);
		}
		public void RemoveEntry( string key)
		{
			Record record = FindRecord( key);
			
			if( record != null)
			{
				RecordObjects( "Remove Entry");
				m_Records.Remove( record);
				EditorUtility.SetDirty( this);
			}
		}
		// public void CopyEntry( Table sourceTable, string key)
		// {
		// 	if( m_Keys.Contains( key) == false)
		// 	{
		// 		RecordObjects( "Copy Entry");
				
		// 		foreach( var sourceTable in sourceTable.tables)
		// 		{
		// 			Record_ sourceRecord = sourceTable.records.FirstOrDefault( x => x.key == key);
		// 			var table = m_Tables.FirstOrDefault( x => x.locale == sourceTable.locale);
		// 			if( sourceRecord != null & table != null)
		// 			{
		// 				table.records.Add( new Record_
		// 				{
		// 					key = sourceRecord.key,
		// 					value = sourceRecord.value
		// 				});
		// 				EditorUtility.SetDirty( table);
		// 			}
		// 		}
		// 		m_Keys.Add( key);
		// 		EditorUtility.SetDirty( this);
		// 	}
		// }
		public bool ChangeKey( string currentKey, string newKey)
		{
			Record record = FindRecord( currentKey);
			
			if( record != null)
			{
				if( m_Records.Any( x => x.Key == newKey) != false)
				{
					Debug.LogError( $"同名のキーが既に存在しています。\n{newKey}");
					return false;
				}
				RecordObjects( "Update Key");
				record.Key = newKey;
				EditorUtility.SetDirty( this);
				return true;
			}
			return false;
		}
		public void SetValue( string key, string locale, string value)
		{
			Record record = FindRecord( key);
			
			if( record != null)
			{
				RecordObjects( "Update Value");
				Record.Clean( value, out value);
				
				switch( locale)
				{
					case Localize.kLocaleCodeJa: record.Ja = value; break;
					case Localize.kLocaleCodeEn: record.En = value; break;
				#if LOCALE_LANGUAGE_DE
					case Localize.kLocaleCodeDe: record.De = value; break;
				#endif
				#if LOCALE_LANGUAGE_ES
					case Localize.kLocaleCodeEs: record.Es = value; break;
				#endif
				#if LOCALE_LANGUAGE_FR
					case Localize.kLocaleCodeFr: record.Fr = value; break;
				#endif
				#if LOCALE_LANGUAGE_IT
					case Localize.kLocaleCodeIt: record.It = value; break;
				#endif
				#if LOCALE_LANGUAGE_KO
					case Localize.kLocaleCodeKo: record.Ko = value; break;
				#endif
				#if LOCALE_LANGUAGE_ZH_HANS
					case Localize.kLocaleCodeZhHans: record.ZhHans = value; break;
				#endif
				#if LOCALE_LANGUAGE_ZH_HANT
					case Localize.kLocaleCodeZhHant: record.ZhHant = value; break;
				#endif
				}
				EditorUtility.SetDirty( this);
			}
		}
		void RecordObjects( string message)
		{
			Undo.RecordObject( this, message);
		}
		void OnEnableEditor()
		{
			s_Tables = null;
		}
		void OnDisableEditor()
		{
			s_Tables = null;
		}
		public Record FindRecord( string recordKey)
		{
			return m_Records.FirstOrDefault( x => x.Key == recordKey);
		}
		public List<Record> Records
		{
			get{ return m_Records; }
		}
		public IEnumerable<string> Keys
		{
			get{ return m_Records.Select( x => x.Key); }
		}
		public bool IsRemote
		{
			get{ return m_Remote; }
			set{ m_Remote = value; EditorUtility.SetDirty( this); }
		}
		[SerializeField]
		bool m_Remote;
		
		static Dictionary<string, Table> s_Tables;
	}
}
#endif
