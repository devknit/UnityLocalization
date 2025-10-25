
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using System.Linq;
using Knit.Localization;

namespace Knit.TimelineExtension
{
	[System.Serializable]
	sealed class LocalizeTextClip : PlayableAsset, ITimelineClipAsset
	{
		public ClipCaps clipCaps
		{
			get { return ClipCaps.Extrapolation; }
		}
		public override Playable CreatePlayable( PlayableGraph graph, GameObject owner)
		{
			return ScriptPlayable<LocalizeTextBehaviour>.Create( graph, m_Source);
		}
		internal void Initialize( TimelineClip timelineClip)
		{
			m_TimelineClip = timelineClip;
		#if UNITY_EDITOR
			string displayName = m_Source?.GetString( Localize.kLocaleCodeJa)?.Trim().Split( '\n').FirstOrDefault() ?? string.Empty;
			
			if( displayName.Length > 16)
			{
				displayName = displayName[..16];
			}
			m_TimelineClip.displayName = displayName;
		#endif
		}
		[SerializeField]
		internal LocalizeTextBehaviour m_Source = new();
		[System.NonSerialized]
		TimelineClip m_TimelineClip;
	}
}
