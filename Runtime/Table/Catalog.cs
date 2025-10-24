
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Knit.Localization
{
	public sealed class Catalog : ScriptableObject
	{
	#if UNITY_EDITOR
		public void Enable()
		{
			m_Tables.ForEach( x => x?.OnEnable());
		}
		internal void Search()
		{
			string[] GUIDs = UnityEditor.AssetDatabase.FindAssets( $"t:{typeof( Table)}");
			m_Tables.Clear();
			
			foreach( var GUID in GUIDs)
			{
				var table = UnityEditor.AssetDatabase.LoadAssetAtPath<Table>( 
					UnityEditor.AssetDatabase.GUIDToAssetPath( GUID));
				if( table != null && table.IsRemote == m_Remote)
				{
					m_Tables.Add( table);
				}
			}
			m_Tables = m_Tables.OrderBy( x => x.name).ToList();
		}
		[SerializeField]
		bool m_Remote;
	#endif
		[SerializeField]
		List<Table> m_Tables;
	}
}
