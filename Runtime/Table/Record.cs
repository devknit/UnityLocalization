
using UnityEngine;

namespace Knit.Localization
{
	[System.Serializable]
	public sealed class Record
	{
		public Record( string key)
		{
			m_Key = key;
			m_Ja = string.Empty;
			m_En = string.Empty;
		#if LOCALE_LANGUAGE_DE
			m_De = string.Empty;
		#endif
		#if LOCALE_LANGUAGE_ES
			m_Es = string.Empty;
		#endif
		#if LOCALE_LANGUAGE_FR
			m_Fr = string.Empty;
		#endif
		#if LOCALE_LANGUAGE_IT
			m_It = string.Empty;
		#endif
		#if LOCALE_LANGUAGE_KO
			m_Ko = string.Empty;
		#endif
		#if LOCALE_LANGUAGE_ZH_HANS
			m_ZhHans = string.Empty;
		#endif
		#if LOCALE_LANGUAGE_ZH_HANT
			m_ZhHant = string.Empty;
		#endif
		}
		public string GetString( Locale locale)
		{
			return locale switch
			{
				Locale.Ja => m_Ja,
				Locale.En => m_En,
			#if LOCALE_LANGUAGE_DE
				Locale.De => m_De,
			#endif
			#if LOCALE_LANGUAGE_ES
				Locale.Es => m_Es,
			#endif
			#if LOCALE_LANGUAGE_FR
				Locale.Fr => m_Fr,
			#endif
			#if LOCALE_LANGUAGE_IT
				Locale.It => m_It,
			#endif
			#if LOCALE_LANGUAGE_KO
				Locale.Ko => m_Ko,
			#endif
			#if LOCALE_LANGUAGE_ZH_HANS
				Locale.ZhHans => m_ZhHans,
			#endif
			#if LOCALE_LANGUAGE_ZH_HANT
				Locale.ZhHant => m_ZhHant,
			#endif
				_ => string.Empty
			};
		}
		public string GetString( string locale)
		{
			return locale switch
			{
				Localize.kLocaleCodeJa => m_Ja,
				Localize.kLocaleCodeEn => m_En,
			#if LOCALE_LANGUAGE_DE
				Localize.kLocaleCodeDe => m_De,
			#endif
			#if LOCALE_LANGUAGE_ES
				Localize.kLocaleCodeEs => m_Es,
			#endif
			#if LOCALE_LANGUAGE_FR
				Localize.kLocaleCodeFr => m_Fr,
			#endif
			#if LOCALE_LANGUAGE_IT
				Localize.kLocaleCodeIt => m_It,
			#endif
			#if LOCALE_LANGUAGE_KO
				Localize.kLocaleCodeKo => m_Ko,
			#endif
			#if LOCALE_LANGUAGE_ZH_HANS
				Localize.kLocaleCodeZhHans => m_ZhHans,
			#endif
			#if LOCALE_LANGUAGE_ZH_HANT
				Localize.kLocaleCodeZhHant => m_ZhHant,
			#endif
				_ => string.Empty
			};
		}
	#if UNITY_EDITOR
		public static bool Clean( string source, out string result)
		{
			string cleanValue = source.Trim().Replace( "\r", string.Empty);
			bool change = cleanValue != source;
			result = cleanValue;
			return change;
		}
		public bool Clean()
		{
			bool change = false;
			
			if( Clean( m_Ja, out m_Ja) != false)
			{
				change = true;
			}
			if( Clean( m_En, out m_En) != false)
			{
				change = true;
			}
		#if LOCALE_LANGUAGE_DE
			if( Clean( m_De, out m_De) != false)
			{
				change = true;
			}
		#endif
		#if LOCALE_LANGUAGE_ES
			if( Clean( m_Es, out m_Es) != false)
			{
				change = true;
			}
		#endif
		#if LOCALE_LANGUAGE_FR
			if( Clean( m_Fr, out m_Fr) != false)
			{
				change = true;
			}
		#endif
		#if LOCALE_LANGUAGE_IT
			if( Clean( m_It, out m_It) != false)
			{
				change = true;
			}
		#endif
		#if LOCALE_LANGUAGE_KO
			if( Clean( m_Ko, out m_Ko) != false)
			{
				change = true;
			}
		#endif
		#if LOCALE_LANGUAGE_ZH_HANS
			if( Clean( m_ZhHans, out m_ZhHans) != false)
			{
				change = true;
			}
		#endif
		#if LOCALE_LANGUAGE_ZH_HANT
			if( Clean( m_ZhHant, out m_ZhHant) != false)
			{
				change = true;
			}
		#endif
			return change;
		}
	#endif
		public string Key
		{
			get{ return m_Key; }
			internal set{ m_Key = value; }
		}
		public string Ja
		{
			get{ return m_Ja; }
			internal set{ m_Ja = value; }
		}
		public string En
		{
			get{ return m_En; }
			internal set{ m_En = value; }
		}
	#if LOCALE_LANGUAGE_DE
		public string De
		{
			get{ return m_De; }
			internal set{ m_De = value; }
		}
	#endif
	#if LOCALE_LANGUAGE_ES
		public string Es
		{
			get{ return m_Es; }
			internal set{ m_Es = value; }
		}
	#endif
	#if LOCALE_LANGUAGE_FR
		public string Fr
		{
			get{ return m_Fr; }
			internal set{ m_Fr = value; }
		}
	#endif
	#if LOCALE_LANGUAGE_IT
		public string It
		{
			get{ return m_It; }
			internal set{ m_It = value; }
		}
	#endif
	#if LOCALE_LANGUAGE_KO
		public string Ko
		{
			get{ return m_Ko; }
			internal set{ m_Ko = value; }
		}
	#endif
	#if LOCALE_LANGUAGE_ZH_HANS
		public string ZhHans
		{
			get{ return m_ZhHans; }
			internal set{ m_ZhHans = value; }
		}
	#endif
	#if LOCALE_LANGUAGE_ZH_HANT
		public string ZhHant
		{
			get{ return m_ZhHant; }
			internal set{ m_ZhHant = value; }
		}
	#endif
		[SerializeField]
		string m_Key;
		[SerializeField, Multiline( 4)]
		string m_Ja;
		[SerializeField, Multiline( 4)]
		string m_En;
	#if LOCALE_LANGUAGE_DE
		[SerializeField, Multiline( 4)]
		string m_De;
	#endif
	#if LOCALE_LANGUAGE_ES
		[SerializeField, Multiline( 4)]
		string m_Es;
	#endif
	#if LOCALE_LANGUAGE_FR
		[SerializeField, Multiline( 4)]
		string m_Fr;
	#endif
	#if LOCALE_LANGUAGE_IT
		[SerializeField, Multiline( 4)]
		string m_It;
	#endif
	#if LOCALE_LANGUAGE_KO
		[SerializeField, Multiline( 4)]
		string m_Ko;
	#endif
	#if LOCALE_LANGUAGE_ZH_HANS
		[SerializeField, Multiline( 4)]
		string m_ZhHans;
	#endif
	#if LOCALE_LANGUAGE_ZH_HANT
		[SerializeField, Multiline( 4)]
		string m_ZhHant;
	#endif
	}
}
