
using UnityEngine;
using System.Diagnostics;

namespace Knit.Timeline
{
	[Conditional( "UNITY_EDITOR")]
	public sealed class EditorLocaleAttribute : PropertyAttribute 
	{
		public EditorLocaleAttribute()
		{
		}
	}
}
