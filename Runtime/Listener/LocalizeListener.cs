
using UnityEngine;
using UnityEngine.Events;

namespace Knit.Localization
{
	public sealed class LocalizeListener : MonoBehaviour, ILocalizeListener
	{
		public LocalizeReference Reference
		{
			get{ return m_Reference; }
		}
		public event UnityAction<string> OnUpdateString
		{
			add{ m_UpdateString.AddListener( value); }
			remove{ m_UpdateString.RemoveListener( value); }
		}
		/// <summary>
		/// 書式を設定
		/// </summary>
		/// <param name="format">{0}を含まれた文字列</param>
		public void SetFormat( string format)
		{
			m_Format = format;
			OnLocaleChanged();
		}
		public void SetArguments( params object[] args)
		{
			m_Args = args;
			OnLocaleChanged();
		}
		public void SetReference( string tableKey, string entryKey)
		{
			m_Reference.SetReference( tableKey, entryKey);
			OnLocaleChanged();
		}
		public void SetReference( string tableKey, string entryKey, params object[] args)
		{
			m_Reference.SetReference( tableKey, entryKey);
			m_Args = args;
			OnLocaleChanged();
		}
		void OnEnable()
		{
			Localize.AddEventListener( this);
		}
		void OnDisable()
		{
			Localize.RemoveEventListener( this);
		}
		public int GetLocaleOrder()
		{
			return 0;
		}
		public void OnLocaleChanged()
		{
			string str = m_Reference.GetString();
			
			if( (m_Args?.Length ?? 0) > 0)
			{
				str = string.Format( str, m_Args);
			}
			if( string.IsNullOrEmpty( m_Format) == false)
			{
				str = string.Format( m_Format, str);
			}
			m_UpdateString.Invoke( str);
		}
	#if UNITY_EDITOR
		void Reset()
		{
			while( m_UpdateString.GetPersistentEventCount() > 0)
			{
				UnityEditor.Events.UnityEventTools.RemovePersistentListener( m_UpdateString, 0);
			}
			if( TryGetComponent<TMPro.TMP_Text>( out var tmpProText) != false)
			{
				var methodInfo = tmpProText.GetType().GetMethod( "SetText",
					System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance,
					null, System.Reflection.CallingConventions.Any, new[]{ typeof(string) }, null);
				var methodDelegate = System.Delegate.CreateDelegate( typeof( UnityAction<string>), tmpProText, methodInfo) as UnityAction<string>;
				UnityEditor.Events.UnityEventTools.AddPersistentListener<string>( m_UpdateString, methodDelegate);
				m_UpdateString.SetPersistentListenerState( 0, UnityEventCallState.EditorAndRuntime);
			}
			else if( TryGetComponent<UnityEngine.UI.Text>( out var uiText) != false)
			{
				var methodInfo = uiText.GetType().GetProperty( "text").GetSetMethod();
				var methodDelegate = System.Delegate.CreateDelegate( typeof( UnityAction<string>), uiText, methodInfo) as UnityAction<string>;
				UnityEditor.Events.UnityEventTools.AddPersistentListener<string>( m_UpdateString, methodDelegate);
				m_UpdateString.SetPersistentListenerState( 0, UnityEventCallState.EditorAndRuntime);
			}
			else if( TryGetComponent<TextMesh>( out var textMesh) != false)
			{
				var methodInfo = textMesh.GetType().GetProperty( "text").GetSetMethod();
				var methodDelegate = System.Delegate.CreateDelegate( typeof( UnityAction<string>), textMesh, methodInfo) as UnityAction<string>;
				UnityEditor.Events.UnityEventTools.AddPersistentListener<string>( m_UpdateString, methodDelegate);
				m_UpdateString.SetPersistentListenerState( 0, UnityEventCallState.EditorAndRuntime);
			}
		}
	#endif
		[System.Serializable]
		sealed class UnityEventString : UnityEvent<string>{};
		[SerializeField]
		LocalizeReference m_Reference = new();
		[SerializeField]
		UnityEventString m_UpdateString = new();
		[System.NonSerialized]
		string m_Format;
		[System.NonSerialized]
		object[] m_Args;
	}
}
