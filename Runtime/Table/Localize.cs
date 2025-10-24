
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Knit.Localization
{
	public enum Locale
	{
		OS = 0,
		Ja = 1,
		En = 2,
	#if LOCALE_LANGUAGE_DE
		De = 3,
	#endif
	#if LOCALE_LANGUAGE_ES
		Es = 4,
	#endif
	#if LOCALE_LANGUAGE_FR
		Fr = 5,
	#endif
	#if LOCALE_LANGUAGE_IT
		It = 6,
	#endif
	#if LOCALE_LANGUAGE_KO
		Ko = 7,
	#endif
	#if LOCALE_LANGUAGE_ZH_HANS
		/// <summary>
		/// 中国簡体字
		/// </summary>
		ZhHans = 8,
	#endif
	#if LOCALE_LANGUAGE_ZH_HANT
		/// <summary>
		/// 中国繁体字
		/// </summary>
		ZhHant = 9,
	#endif
	}
	public static class Localize
	{
		public const string kLocaleCodeJa = "ja";
		public const string kLocaleCodeEn = "en";
	#if LOCALE_LANGUAGE_DE
		public const string kLocaleCodeDe = "de";
	#endif
	#if LOCALE_LANGUAGE_ES
		public const string kLocaleCodeEs = "es";
	#endif
	#if LOCALE_LANGUAGE_FR
		public const string kLocaleCodeFr = "fr";
	#endif
	#if LOCALE_LANGUAGE_IT
		public const string kLocaleCodeIt = "it";
	#endif
	#if LOCALE_LANGUAGE_KO
		public const string kLocaleCodeKo = "ko";
	#endif
	#if LOCALE_LANGUAGE_ZH_HANS
		/// <summary>
		/// 中国簡体字
		/// </summary>
		public const string kLocaleCodeZhHans = "zh-Hans";
	#endif
	#if LOCALE_LANGUAGE_ZH_HANT
		/// <summary>
		/// 中国繁体字
		/// </summary>
		public const string kLocaleCodeZhHant = "zh-Hant";
	#endif
		public enum ListenerCallbackMode
		{
			None,
			NewListenerOnly,
			ExistingListeners,
			All,
		}
		public static IEnumerable<string> GetLocales()
		{
			yield return kLocaleCodeJa;
			yield return kLocaleCodeEn;
		#if LOCALE_LANGUAGE_DE
			yield return kLocaleCodeDe;
		#endif
		#if LOCALE_LANGUAGE_ES
			yield return kLocaleCodeEs;
		#endif
		#if LOCALE_LANGUAGE_FR
			yield return kLocaleCodeFr;
		#endif
		#if LOCALE_LANGUAGE_IT
			yield return kLocaleCodeIt;
		#endif
		#if LOCALE_LANGUAGE_KO
			yield return kLocaleCodeKo;
		#endif
		#if LOCALE_LANGUAGE_ZH_HANS
			yield return kLocaleCodeZhHans;
		#endif
		#if LOCALE_LANGUAGE_ZH_HANT
			yield return kLocaleCodeZhHant;
		#endif
		}
		public static void SetLocale( Locale locale)
		{
			SetLocaleCode( locale switch
			{
				Locale.OS =>  GetOperationSystemLocaleCode(),
				Locale.Ja => kLocaleCodeJa,
			#if LOCALE_LANGUAGE_DE
				Locale.De => kLocaleCodeDe,
			#endif
			#if LOCALE_LANGUAGE_ES
				Locale.Es => kLocaleCodeEs,
			#endif
			#if LOCALE_LANGUAGE_FR
				Locale.Fr => kLocaleCodeFr,
			#endif
			#if LOCALE_LANGUAGE_IT
				Locale.It => kLocaleCodeIt,
			#endif
			#if LOCALE_LANGUAGE_KO
				Locale.Ko => kLocaleCodeKo,
			#endif
			#if LOCALE_LANGUAGE_ZH_HANS
				Locale.ZhHans => kLocaleCodeZhHans,
			#endif
			#if LOCALE_LANGUAGE_ZH_HANT
				Locale.ZhHant => kLocaleCodeZhHant,
			#endif
				_ => kLocaleCodeEn
			});
		}
		public static Locale GetLocale()
		{
			return GetLocaleCode() switch
			{
				kLocaleCodeJa => Locale.Ja,
			#if LOCALE_LANGUAGE_DE
				kLocaleCodeDe => Locale.De,
			#endif
			#if LOCALE_LANGUAGE_ES
				kLocaleCodeEs => Locale.Es,
			#endif
			#if LOCALE_LANGUAGE_FR
				kLocaleCodeFr => Locale.Fr,
			#endif
			#if LOCALE_LANGUAGE_IT
				kLocaleCodeIt => Locale.It,
			#endif
			#if LOCALE_LANGUAGE_KO
				kLocaleCodeKo => Locale.Ko,
			#endif
			#if LOCALE_LANGUAGE_ZH_HANS
				kLocaleCodeZhHans => Locale.ZhHans,
			#endif
			#if LOCALE_LANGUAGE_ZH_HANT
				kLocaleCodeZhHant => Locale.ZhHant,
			#endif
				_ => Locale.En
			};
		}
		public static void SetLocaleCode( string localeCode)
		{
	#if LOCALE_LANGUAGE_FIXED
		#if LOCALE_LANGUAGE_JA_DEFAULT
			localeCode = kLocaleCodeJa;
		#elif LOCALE_LANGUAGE_EN_DEFAULT
			localeCode = kLocaleCodeEn;
		#elif LOCALE_LANGUAGE_DE_DEFAULT
			localeCode = kLocaleCodeDe;
		#elif LOCALE_LANGUAGE_ES_DEFAULT
			localeCode = kLocaleCodeEs;
		#elif LOCALE_LANGUAGE_FR_DEFAULT
			localeCode = kLocaleCodeFr;
		#elif LOCALE_LANGUAGE_IT_DEFAULT
			localeCode = kLocaleCodeIt;
		#elif LOCALE_LANGUAGE_KO_DEFAULT
			localeCode = kLocaleCodeKo;
		#elif LOCALE_LANGUAGE_ZH_HANS_DEFAULT
			localeCode = kLocaleCodeZhHans;
		#elif LOCALE_LANGUAGE_ZH_HANT_DEFAULT
			localeCode = kLocaleCodeZhHant;
		#else
			#error "If the language is fixed, a default language must be specified."
		#endif
	#endif
			if( GetLocaleCode() != localeCode)
			{
				s_Locale = localeCode;
				
				foreach( var listener in s_Listeners.Values.OrderBy( x => x.GetLocaleOrder()))
				{
					listener.OnLocaleChanged();
				}
			}
		}
		public static string GetLocaleCode()
		{
			if( s_Locale == null)
			{
			#if UNITY_EDITOR
				if( Application.isPlaying == false)
				{
					s_Locale = kLocaleCodeEn;
				}
				else
			#endif
				{
				#if LOCALE_LANGUAGE_JA_DEFAULT
					s_Locale = kLocaleCodeJa;
				#elif LOCALE_LANGUAGE_EN_DEFAULT
					s_Locale = kLocaleCodeEn;
				#elif LOCALE_LANGUAGE_DE_DEFAULT
					s_Locale = kLocaleCodeDe;
				#elif LOCALE_LANGUAGE_ES_DEFAULT
					s_Locale = kLocaleCodeEs;
				#elif LOCALE_LANGUAGE_FR_DEFAULT
					s_Locale = kLocaleCodeFr;
				#elif LOCALE_LANGUAGE_IT_DEFAULT
					s_Locale = kLocaleCodeIt;
				#elif LOCALE_LANGUAGE_KO_DEFAULT
					s_Locale = kLocaleCodeKo;
				#elif LOCALE_LANGUAGE_ZH_HANS_DEFAULT
					s_Locale = kLocaleCodeZhHans;
				#elif LOCALE_LANGUAGE_ZH_HANT_DEFAULT
					s_Locale = kLocaleCodeZhHant;
				#else
					s_Locale = GetOperationSystemLocaleCode();
				#endif
				}
			}
			return s_Locale;
		}
		public static string GetOperationSystemLocaleCode()
		{
			return Application.systemLanguage switch
			{
				SystemLanguage.Japanese => kLocaleCodeJa,
			#if LOCALE_LANGUAGE_DE
				SystemLanguage.German => kLocaleCodeDe,
			#endif
			#if LOCALE_LANGUAGE_ES
				SystemLanguage.Spanish => kLocaleCodeEs,
			#endif
			#if LOCALE_LANGUAGE_FR
				SystemLanguage.French => kLocaleCodeFr,
			#endif
			#if LOCALE_LANGUAGE_IT
				SystemLanguage.Italian => kLocaleCodeIt,
			#endif
			#if LOCALE_LANGUAGE_KO
				SystemLanguage.Korean => kLocaleCodeKo,
			#endif
			#if LOCALE_LANGUAGE_ZH_HANS
				SystemLanguage.ChineseSimplified => kLocaleCodeZhHans,
			#endif
			#if LOCALE_LANGUAGE_ZH_HANT
				SystemLanguage.ChineseTraditional => kLocaleCodeZhHant,
			#endif
				_ => kLocaleCodeEn
			};
		}
		public static string GetString( string keys)
		{
			string[] key = keys.Split( '@');
			
			if( key.Length != 2 || TryGetString( key[ 0], key[ 1], out string value) == false)
			{
				value = string.Empty;
			}
			return value;
		}
		public static string GetString( string tableKey, string entryKey)
		{
			if( TryGetString( tableKey, entryKey, out string value) == false)
			{
				value = string.Empty;
			}
			return value;
		}
		public static bool TryGetString( string tableKey, string entryKey, out string value)
		{
			if( s_Tables.TryGetValue( tableKey ?? string.Empty, out Table table) != false)
			{
				Record record = table.GetRecord( entryKey);
				
				if( record != null)
				{
					value = record.GetString( GetLocaleCode());
					return true;
				}
			#if UNITY_EDITOR || DEVELOPMENT_BUILD
				else
				{
					value = $"Not found EntryKey`{tableKey}@{entryKey}`.";
				}
			#endif
			}
		#if UNITY_EDITOR || DEVELOPMENT_BUILD
			else
			{
				value = $"Not found TableKey`{tableKey}@{entryKey}`.";
			}
		#else
			value = string.Empty;
		#endif
			return false;
		}
	#if UNITY_EDITOR
		public static string GetString( string locale, LocalizeReference reference)
		{
			if( string.IsNullOrEmpty( reference?.TableKey) == false)
			{
				if( Table.GetOrCreateTables().TryGetValue( reference.TableKey, out Table table) != false)
				{
					return table.FindRecord( reference.EntryKey)?.GetString( locale) ?? string.Empty;
				}
			}
			return string.Empty;
		}
		public static bool TryGetStringJa( string tableKey, string entryKey, out string value)
		{
			return TryGetStringLocale( kLocaleCodeJa, tableKey, entryKey, out value);
		}
		public static bool TryGetStringEn( string tableKey, string entryKey, out string value)
		{
			return TryGetStringLocale( kLocaleCodeEn, tableKey, entryKey, out value);
		}
		static bool TryGetStringLocale( string locale, string tableKey, string entryKey, out string value)
		{
			if( s_Tables.TryGetValue( tableKey ?? string.Empty, out Table table) != false)
			{
				Record record = table.GetRecord( entryKey);
				
				if( record != null)
				{
					value = record.GetString( locale);
					return true;
				}
			#if UNITY_EDITOR || DEVELOPMENT_BUILD
				else
				{
					value = $"Not found EntryKey`{tableKey}@{entryKey}`.";
				}
			#endif
			}
		#if UNITY_EDITOR || DEVELOPMENT_BUILD
			else
			{
				value = $"Not found TableKey`{tableKey}@{entryKey}`.";
			}
		#else
			value = string.Empty;
		#endif
			return false;
		}
	#endif
		public static void AddEventListener( ILocalizeListener newListener, ListenerCallbackMode mode=ListenerCallbackMode.NewListenerOnly)
		{
			int id = newListener.GetInstanceID();
			
			if( s_Listeners.ContainsKey( id) == false)
			{
				s_Listeners.Add( id, newListener);
				
				switch( mode)
				{
					case ListenerCallbackMode.NewListenerOnly:
					{
						newListener.OnLocaleChanged();
						break;
					}
					case ListenerCallbackMode.ExistingListeners:
					{
						foreach( var listener in s_Listeners.Values.OrderBy( x => x.GetLocaleOrder()))
						{
							if( listener != newListener)
							{
								listener.OnLocaleChanged();
							}
						}
						break;
					}
					case ListenerCallbackMode.All:
					{
						foreach( var listener in s_Listeners.Values.OrderBy( x => x.GetLocaleOrder()))
						{
							listener.OnLocaleChanged();
						}
						break;
					}
				}
			}
		}
		public static void RemoveEventListener( ILocalizeListener listener)
		{
			int id = listener.GetInstanceID();
			
			if( s_Listeners.ContainsKey( id) != false)
			{
				s_Listeners.Remove( id);
			}
		}
		internal static void AddTable( Table table)
		{
			if( string.IsNullOrEmpty( table?.Key) == false)
			{
				s_Tables.TryAdd( table.Key, table);
			}
		}
		internal static void RemoveTable( Table table)
		{
			if( s_Tables.ContainsKey( table.Key) != false)
			{
				s_Tables.Remove( table.Key);
			}
		}
		static string s_Locale = null;
		static readonly Dictionary<string, Table> s_Tables = new();
		static readonly Dictionary<int, ILocalizeListener> s_Listeners = new();
	}
}
