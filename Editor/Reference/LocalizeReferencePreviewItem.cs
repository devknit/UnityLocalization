
using UnityEditor.IMGUI.Controls;

namespace Knit.Localization.Editor
{
	public class LocalizeReferencePreviewItem : TreeViewItem
	{
		public string TableKey
		{
			get{ return m_TableKey; }
		}
		public string EntryKey
		{
			get{ return m_EntryKey; }
		}
		public string Preview
		{
			get{ return m_Preview; }
		}
		public LocalizeReferencePreviewItem( string tableKey, string entryKey, int id, int depth, string preview) : base( id, depth)
		{
			m_TableKey = tableKey;
			m_EntryKey = entryKey;
			m_Preview = preview;
		}
        readonly string m_TableKey;
        readonly string m_EntryKey;
        readonly string m_Preview;
	}
}
