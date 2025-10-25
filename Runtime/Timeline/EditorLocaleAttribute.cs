
using UnityEngine;
using System.Diagnostics;

namespace Knit.TimelineExtension
{
	[Conditional( "UNITY_EDITOR")]
	public sealed class EditorLocaleAttribute : PropertyAttribute 
	{
		public EditorLocaleAttribute()
		{
		}
	}
}
