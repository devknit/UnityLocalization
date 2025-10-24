
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Knit.Localization.Editor
{
	public sealed class LocalizeReferenceDrawerContext
	{
		internal GUIContent Content
		{
			get{ return m_Content; }
		}
		internal string TableKey
		{
			get{ return m_TableKey; }
		}
		internal string EntryKey
		{
			get{ return m_EntryKey; }
		}
		internal List<(GUIContent value, GUIContent label)> locales
		{
			get{ return m_Locales; }
		}
		internal void SetProperty( string tableKey, string entryKey)
		{
			if( m_Property != null)
			{
				m_TableKey = tableKey;
				m_EntryKey = entryKey;
				
				if( m_TableKeyProperty != null && m_EntryKeyProperty != null)
				{
					m_TableKeyProperty.stringValue = tableKey;
					m_EntryKeyProperty.stringValue = entryKey;
				}
				else if( m_Property.propertyType == SerializedPropertyType.String)
				{
					m_Property.stringValue = $"{tableKey}@{entryKey}";
				}
				m_Property.serializedObject.ApplyModifiedProperties();
			}
		}
		internal void UpdateProperty( SerializedProperty property, Dictionary<string, Table> tables)
		{
			if( m_Property != property)
			{
				List<(GUIContent, GUIContent)> locales = null;
				
				if( property.propertyType == SerializedPropertyType.Generic)
				{
					m_TableKeyProperty = property.FindPropertyRelative( "m_TableKey");
					m_EntryKeyProperty = property.FindPropertyRelative( "m_EntryKey");
				}
				else
				{
					m_TableKeyProperty = null;
					m_EntryKeyProperty = null;
				}
				m_TableKey = string.Empty;
				m_EntryKey = string.Empty;
				
				if( m_TableKeyProperty != null && m_EntryKeyProperty != null)
				{
					m_TableKey = m_TableKeyProperty.stringValue;
					m_EntryKey = m_EntryKeyProperty.stringValue;
				}
				else if( property.propertyType == SerializedPropertyType.String)
				{
					string[] split = property.stringValue.Split( '@');
					
					if( split.Length > 0)
					{
						m_TableKey = split[ 0];
					}
					if( split.Length > 1)
					{
						m_EntryKey = split[ 1];
					}
				}
				if( string.IsNullOrEmpty( m_TableKey) == false
				&&	string.IsNullOrEmpty( m_EntryKey) == false)
				{
					if( tables.TryGetValue( m_TableKey, out m_Table) != false)
					{
						Record record = m_Table.FindRecord( m_EntryKey);
						locales = new List<(GUIContent, GUIContent)>();
						
						if( record != null)
						{
							string rawLabel;
							string label;
							
							label = rawLabel = record.Ja;
							label = (string.IsNullOrEmpty( label) != false)? "<empty>":
								label.Replace( "\r\n", "\n").Replace( "\n", "\\n");
							locales.Add( (new GUIContent( Localize.kLocaleCodeJa), new GUIContent( label, rawLabel)));
							
							label = rawLabel = record.En;
							label = (string.IsNullOrEmpty( label) != false)? "<empty>":
								label.Replace( "\r\n", "\n").Replace( "\n", "\\n");
							locales.Add( (new GUIContent( Localize.kLocaleCodeEn), new GUIContent( label, rawLabel)));
						#if LOCALE_LANGUAGE_DE
							label = rawLabel = record.De;
							label = (string.IsNullOrEmpty( label) != false)? "<empty>":
								label.Replace( "\r\n", "\n").Replace( "\n", "\\n");
							locales.Add( (new GUIContent( Localize.kLocaleCodeDe), new GUIContent( label, rawLabel)));
						#endif
						#if LOCALE_LANGUAGE_ES
							label = rawLabel = record.Es;
							label = (string.IsNullOrEmpty( label) != false)? "<empty>":
								label.Replace( "\r\n", "\n").Replace( "\n", "\\n");
							locales.Add( (new GUIContent( Localize.kLocaleCodeEs), new GUIContent( label, rawLabel)));
						#endif
						#if LOCALE_LANGUAGE_FR
							label = rawLabel = record.Fr;
							label = (string.IsNullOrEmpty( label) != false)? "<empty>":
								label.Replace( "\r\n", "\n").Replace( "\n", "\\n");
							locales.Add( (new GUIContent( Localize.kLocaleCodeFr), new GUIContent( label, rawLabel)));
						#endif
						#if LOCALE_LANGUAGE_IT
							label = rawLabel = record.It;
							label = (string.IsNullOrEmpty( label) != false)? "<empty>":
								label.Replace( "\r\n", "\n").Replace( "\n", "\\n");
							locales.Add( (new GUIContent( Localize.kLocaleCodeIt), new GUIContent( label, rawLabel)));
						#endif
						#if LOCALE_LANGUAGE_KO
							label = rawLabel = record.Ko;
							label = (string.IsNullOrEmpty( label) != false)? "<empty>":
								label.Replace( "\r\n", "\n").Replace( "\n", "\\n");
							locales.Add( (new GUIContent( Localize.kLocaleCodeKo), new GUIContent( label, rawLabel)));
						#endif
						#if LOCALE_LANGUAGE_ZH_HANS
							label = rawLabel = record.ZhHans;
							label = (string.IsNullOrEmpty( label) != false)? "<empty>":
								label.Replace( "\r\n", "\n").Replace( "\n", "\\n");
							locales.Add( (new GUIContent( Localize.kLocaleCodeZhHans), new GUIContent( label, rawLabel)));
						#endif
						#if LOCALE_LANGUAGE_ZH_HANT
							label = rawLabel = record.ZhHant;
							label = (string.IsNullOrEmpty( label) != false)? "<empty>":
								label.Replace( "\r\n", "\n").Replace( "\n", "\\n");
							locales.Add( (new GUIContent( Localize.kLocaleCodeZhHant), new GUIContent( label, rawLabel)));
						#endif
							m_Content = new GUIContent( $"{m_TableKey}@{m_EntryKey}");
						}
						else
						{
							m_Content = new GUIContent( $"Missing Entry<{m_TableKey}@{m_EntryKey}>");
						}
					}
					else
					{
						m_Content = new GUIContent( $"Missing Table<{m_TableKey}@{m_EntryKey}>");
					}
				}
				else if( string.IsNullOrEmpty( m_EntryKey) == false)
				{
					m_Content = new GUIContent( $"Raw: {m_EntryKey}>");
				}
				else
				{
					m_Content = new GUIContent( "None (String)");
				}
				m_Locales = locales;
				m_Property = property;
			}
		}
		Table m_Table;
		GUIContent m_Content;
		SerializedProperty m_Property;
		SerializedProperty m_TableKeyProperty;
		SerializedProperty m_EntryKeyProperty;
		List<(GUIContent, GUIContent)> m_Locales;
		string m_TableKey;
		string m_EntryKey;
	}
}
