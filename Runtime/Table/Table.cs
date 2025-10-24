#if !UNITY_EDITOR
	#define WITH_TO_DICTIONARY
#endif

using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Knit.Localization
{
	public sealed partial class Table : ScriptableObject
	{
		public void OnEnable()
		{
		#if WITH_TO_DICTIONARY
			m_Dictionary = new Dictionary<string, Record>();
			
			foreach( var record in m_Records)
			{
				m_Dictionary.TryAdd( record.Key, record);
			}
		#elif UNITY_EDITOR
			var records = new HashSet<string>();
			bool isError = false;
			
			foreach( var record in m_Records)
			{
				if( records.Contains( record.Key) == false)
				{
					records.Add( record.Key);
				}
				else
				{
					Debug.LogError( $"ローカライズキーが重複しています\nTableKey = {m_Key}, EntryKey = {record.Key}", this);
					isError = true;
				}
			}
			if( isError != false)
			{
				UnityEditor.EditorApplication.isPlaying = false;
			}
		#endif
			Localize.AddTable( this);
		#if UNITY_EDITOR
			OnEnableEditor();
		#endif
		}
		void OnDisable()
		{
			Localize.RemoveTable( this);
		#if UNITY_EDITOR
			OnDisableEditor();
		#endif
		}
		public string Key
		{
			get{ return m_Key; }
		}
		public Record GetRecord( string recordKey)
		{
		#if WITH_TO_DICTIONARY
			m_Dictionary.TryGetValue( recordKey, out Record record);
			return record;
		#else
			return m_Records.FirstOrDefault( x => x.Key == recordKey);
		#endif
		}
		[SerializeField]
		string m_Key;
		[SerializeField, TableList]
		List<Record> m_Records = new();
	#if WITH_TO_DICTIONARY
		Dictionary<string, Record> m_Dictionary;
	#endif
	}
}
