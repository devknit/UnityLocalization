
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

namespace Knit.Localization
{
	sealed class LocalizeTextMixerBehaviour : PlayableBehaviour
	{
		public override void ProcessFrame( Playable playable, FrameData info, object playerData)
		{
			if( playerData is TMP_Text component)
			{
				if( m_Component == null)
				{
					m_Component = component;
					m_DefaultText = component.text;
				}
				if( m_Component != null)
				{
					int inputCount = playable.GetInputCount();
					string text = m_DefaultText;
					string locale = Localize.GetLocaleCode();
					
				#if UNITY_EDITOR
					if( Application.isPlaying == false && string.IsNullOrEmpty( m_Locale) == false)
					{
						locale = m_Locale;
					}
				#endif
					for( int i0 = 0; i0 < inputCount; ++i0)
					{
						if( playable.GetInputWeight( i0) > 0.0f)
						{
							var inputPlayable = (ScriptPlayable<LocalizeTextBehaviour>)playable.GetInput( i0);
							var behaviour = inputPlayable.GetBehaviour();
							text = behaviour.GetString( locale) ?? string.Empty;
							break;
						}
					}
					if( m_CurrentText != text)
					{
						m_CurrentText = text;
						m_Component.SetText( text);
					}
				}
			}
		}
		public override void OnPlayableDestroy( Playable playable)
		{
			if( m_Component != null)
			{
				m_Component.SetText( m_DefaultText);
				m_Component = null;
			}
		}
	#if UNITY_EDITOR
		internal void SetLocale( string locale)
		{
			m_Locale = locale ?? Localize.kLocaleCodeJa;
		}
		string m_Locale = Localize.kLocaleCodeJa;
	#endif
		string m_DefaultText;
		string m_CurrentText;
		TMP_Text m_Component;
	}
}
