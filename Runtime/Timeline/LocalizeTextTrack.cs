
using TMPro;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using System.ComponentModel; 

namespace Knit.Localization
{
	[TrackColor( 0.0f, 0.7f, 0.9f)]
	[TrackClipType( typeof( LocalizeTextClip))]
	[TrackBindingType( typeof( TMP_Text))]
	[DisplayName( "Knit.Timeline/Localize Text Track")]
	internal sealed class LocalizeTextTrack : TrackAsset
	{
		public override Playable CreateTrackMixer( PlayableGraph graph, GameObject go, int inputCount)
		{
			foreach( var clip in GetClips())
			{
				if( clip.asset is LocalizeTextClip textClip)
				{
					textClip.Initialize( clip);
				}
			}
			var playable = ScriptPlayable<LocalizeTextMixerBehaviour>.Create( graph, inputCount);
		#if UNITY_EDITOR
			playable.GetBehaviour().SetLocale( m_EditorLocale);
		#endif
			return playable;
		}
		public override void GatherProperties( PlayableDirector director, IPropertyCollector driver)
		{
	#if UNITY_EDITOR
			var text = director.GetGenericBinding( this) as TMP_Text;
			if( text == null)
			{
				return;
			}
			driver.AddFromName<TMP_Text>( text.gameObject, "m_text");
	#endif
			base.GatherProperties( director, driver);
		}
	#if UNITY_EDITOR
		[SerializeField, EditorLocale]
		string m_EditorLocale;
	#endif
	}
}
