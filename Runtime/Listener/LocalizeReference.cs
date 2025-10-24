
using UnityEngine;

namespace Knit.Localization
{
	[System.Serializable]
	public sealed class LocalizeReference
	{
		public bool IsValid
		{
			get{ return string.IsNullOrEmpty( m_TableKey) == false && string.IsNullOrEmpty( m_EntryKey) == false; }
		}
		public string GetString()
		{
			if( Localize.TryGetString( m_TableKey, m_EntryKey, out string value) == false)
			{
			#if UNITY_EDITOR || DEVELOPMENT_BUILD
				if( string.IsNullOrEmpty( m_TableKey) == false)
				{
					Debug.LogError( $"Localization not found.\nTable={m_TableKey}, Entry={m_EntryKey}");
				}
			#endif
				value = m_EntryKey ?? string.Empty;
			}
			return value;
		}
		public bool TryGetString( out string value)
		{
			return Localize.TryGetString( m_TableKey, m_EntryKey, out value);
		}
		public string GetString( params object[] args)
		{
			if( Localize.TryGetString( m_TableKey, m_EntryKey, out string value) == false)
			{
			#if UNITY_EDITOR || DEVELOPMENT_BUILD
				if( string.IsNullOrEmpty( m_TableKey) == false)
				{
					Debug.LogError( $"Localization not found.\nTable={m_TableKey}, Entry={m_EntryKey}");
				}
			#endif
				value = m_EntryKey ?? string.Empty;
			}
			else
			{
				value = string.Format( value, args);
			}
			return value;
		}
		public bool TryGetString( out string value, params object[] args)
		{
			if( Localize.TryGetString( m_TableKey, m_EntryKey, out value) != false)
			{
				value = string.Format( value, args);
				return true;
			}
			return false;
		}
		internal void SetReference( string tableKey, string entryKey)
		{
			m_TableKey = tableKey;
			m_EntryKey = entryKey;
		}
	#if UNITY_EDITOR
		public string TableKey
		{
			get{ return m_TableKey; }
		}
		public string EntryKey
		{
			get{ return m_EntryKey; }
		}
		public void OnVerify( System.Action<string, string> onError)
		{
			if( Localize.TryGetString( m_TableKey, m_EntryKey, out string value) == false)
			{
				if( string.IsNullOrEmpty( m_TableKey) != false)
				{
					if( string.IsNullOrEmpty( m_EntryKey) == false
					&&	m_EntryKey.Contains( "DEBUG") == false)
					{
						onError?.Invoke( m_TableKey, m_EntryKey);
					}
				}
				else
				{
					onError?.Invoke( m_TableKey, m_EntryKey);
				}
			}
		}
		public string GetStringJa()
		{
			if( Localize.TryGetStringJa( m_TableKey, m_EntryKey, out string value) == false)
			{
			#if UNITY_EDITOR || DEVELOPMENT_BUILD
				if( string.IsNullOrEmpty( m_TableKey) == false)
				{
					Debug.LogError( $"Localization not found.\nTable={m_TableKey}, Entry={m_EntryKey}");
				}
			#endif
				value = m_EntryKey ?? string.Empty;
			}
			return value;
		}
		public string GetStringEn()
		{
			if( Localize.TryGetStringJa( m_TableKey, m_EntryKey, out string value) == false)
			{
			#if UNITY_EDITOR || DEVELOPMENT_BUILD
				if( string.IsNullOrEmpty( m_TableKey) == false)
				{
					Debug.LogError( $"Localization not found.\nTable={m_TableKey}, Entry={m_EntryKey}");
				}
			#endif
				value = m_EntryKey ?? string.Empty;
			}
			return value;
		}
	#endif
		[SerializeField]
		string m_TableKey;
		[SerializeField]
		string m_EntryKey;
	}
}
