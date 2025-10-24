
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace Knit.Localization.Editor
{
	public class LocalizeReferencePreviewContent : PopupWindowContent
	{
		public LocalizeReferencePreviewContent( TreeView treeView, float width)
		{
			m_SearchField = new SearchField();
            m_TreeView = treeView;
			m_Width = Mathf.Max( 600, width);
		}
		public override Vector2 GetWindowSize()
		{
			var size = base.GetWindowSize();
			size.x = m_Width;
			size.y = 400;
			return size;
		}
		public override void OnGUI( Rect rect)
        {
			const int border = 4;
            const int topPadding = 12;
            const int searchHeight = 20;
            const int remainTop = topPadding + searchHeight + border;
			
            var searchRect = new Rect( border, topPadding, rect.width - border * 2, searchHeight);
            var remainingRect = new Rect( border, topPadding + searchHeight + border, rect.width - border * 2, rect.height - remainTop - border);
			
            m_TreeView.searchString = m_SearchField.OnGUI( searchRect, m_TreeView.searchString);
            m_TreeView.OnGUI( remainingRect);
			
            if( m_ShouldClose != false)
            {
                GUIUtility.hotControl = 0;
                editorWindow.Close();
            }
            if( m_TreeView.HasSelection() != false)
			{
                ForceClose();
			}
		}
		public override void OnOpen()
        {
            m_SearchField.SetFocus();
            base.OnOpen();
        }
		public void ForceClose()
		{
			m_ShouldClose = true;
		}
		readonly SearchField m_SearchField;
        readonly TreeView m_TreeView;
		readonly float m_Width;
		bool m_ShouldClose;
	}
}
