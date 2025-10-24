
using UnityEngine;
using UnityEditor.IMGUI.Controls;
using System.Collections.Generic;

namespace Knit.Localization.Editor
{
	internal sealed class LocalizeReferencePreviewColumnHeader : MultiColumnHeader
	{
		static MultiColumnHeaderState CreateState( string locale)
		{
			var columns = new List<MultiColumnHeaderState.Column>
            {
                new()
				{
                    headerContent = new GUIContent( "Key"),
                    headerTextAlignment = TextAlignment.Center,
                    canSort = false,
                    width = 100,
                    minWidth = 100,
                    autoResize = true,
                    allowToggleVisibility = false
                }
            };
			if( string.IsNullOrEmpty( locale) == false)
			{
				columns.Add( new MultiColumnHeaderState.Column
				{
					headerContent = new GUIContent( locale),
					headerTextAlignment = TextAlignment.Center,
					canSort = false,
					width = 60, 
					minWidth = 60,
					autoResize = true,
					allowToggleVisibility = false
				});
			}
			return new MultiColumnHeaderState( columns.ToArray());
		}
		internal LocalizeReferencePreviewColumnHeader( string locale) : base( CreateState( locale))
		{
			height = 20;
		}
	}
}
