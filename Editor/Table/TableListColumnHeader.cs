
using UnityEngine;
using UnityEditor.IMGUI.Controls;
using System.Collections.Generic;

namespace Knit.Localization.Editor
{
	internal class TableListColumnHeader : MultiColumnHeader
	{
		static MultiColumnHeaderState CreateState()
		{
			var columns = new List<MultiColumnHeaderState.Column>
            {
                new()
				{
                    headerContent = new GUIContent( "Name"),
                    headerTextAlignment = TextAlignment.Center,
                    canSort = true,
                    width = 100,
                    minWidth = 100,
                    autoResize = true,
                    allowToggleVisibility = false
                }
            };
			return new MultiColumnHeaderState( columns.ToArray());
		}
		internal TableListColumnHeader() : base( CreateState())
		{
		}
	}
}
