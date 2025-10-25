
using System;
using UnityEngine;
using UnityEngine.Playables;
using Knit.Localization;

namespace Knit.TimelineExtension
{
	[Serializable]
	sealed class LocalizeTextBehaviour : PlayableBehaviour
	{
		internal string GetString( string locale)
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
		[SerializeField, TextArea( 1, 6)]
		string m_Ja;
		[SerializeField, TextArea( 1, 6)]
		string m_En;
	#if LOCALE_LANGUAGE_DE
		[SerializeField, TextArea( 1, 6)]
		string m_De;
	#endif
	#if LOCALE_LANGUAGE_ES
		[SerializeField, TextArea( 1, 6)]
		string m_Es;
	#endif
	#if LOCALE_LANGUAGE_FR
		[SerializeField, TextArea( 1, 6)]
		string m_Fr;
	#endif
	#if LOCALE_LANGUAGE_IT
		[SerializeField, TextArea( 1, 6)]
		string m_It;
	#endif
	#if LOCALE_LANGUAGE_KO
		[SerializeField, TextArea( 1, 6)]
		string m_Ko;
	#endif
	#if LOCALE_LANGUAGE_ZH_HANS
		[SerializeField, TextArea( 1, 6)]
		string m_ZhHans;
	#endif
	#if LOCALE_LANGUAGE_ZH_HANT
		[SerializeField, TextArea( 1, 6)]
		string m_ZhHant;
	#endif
	}
}
