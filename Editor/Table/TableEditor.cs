
using UnityEditor;
using UnityEngine;

namespace Knit.Localization.Editor
{
	[CustomEditor( typeof( Table), true)]
	sealed class TableEditor : UnityEditor.Editor
	{
		void OnEnable()
		{
			m_KeyProperty = serializedObject.FindProperty( "m_Key");
			m_RemoteProperty = serializedObject.FindProperty( "m_Remote");
			m_RecordsProperty = serializedObject.FindProperty( "m_Records");
		}
		public override void OnInspectorGUI()
		{
			serializedObject.UpdateIfRequiredOrScript();
			
			if( GUILayout.Button( "Localization Window") != false)
			{
				TableWindow.Open( m_KeyProperty.stringValue, string.Empty);
			}
			using( new EditorGUI.DisabledGroupScope( true))
			{
				EditorGUILayout.PropertyField( m_KeyProperty);
				EditorGUILayout.PropertyField( m_RemoteProperty);
				EditorGUILayout.PropertyField( m_RecordsProperty);
			}
			serializedObject.ApplyModifiedProperties();
		}
		SerializedProperty m_KeyProperty;
		SerializedProperty m_RemoteProperty;
		SerializedProperty m_RecordsProperty;
	}
}
