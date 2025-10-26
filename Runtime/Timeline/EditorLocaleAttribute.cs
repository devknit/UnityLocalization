
using UnityEngine;
using System.Diagnostics;

namespace Knit.Localization
{
	[Conditional( "UNITY_EDITOR")]
	public sealed class EditorLocaleAttribute : PropertyAttribute 
	{
		public EditorLocaleAttribute()
		{
		}
	}
}
