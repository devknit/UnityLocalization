
using UnityEngine;
using UnityEditor;
using UnityEngine.Timeline;
using UnityEditor.Timeline;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Knit.Localization;

namespace Knit.Timeline.Editor
{
	[CustomPropertyDrawer( typeof( EditorLocaleAttribute))]
	sealed class EditorLocaleDrawer : PropertyDrawer 
	{
		public override float GetPropertyHeight( SerializedProperty property, GUIContent label)
		{
			float height = base.GetPropertyHeight( property, label);
			
			if( property.serializedObject.targetObjects.FirstOrDefault( x => x is TrackAsset) != null)
			{
				height *= 2;
			}
			return height;
		}
		public override void OnGUI( Rect position, SerializedProperty property, GUIContent label)
		{
			position.height = EditorGUIUtility.singleLineHeight;
			
			string[] locales = Localize.GetLocales().Prepend( "Default").ToArray();
			int index = Mathf.Max( 0, System.Array.IndexOf( locales, property.stringValue));
			
			EditorGUI.BeginChangeCheck();
			index = EditorGUI.Popup( position, property.displayName, index, locales);
			if( EditorGUI.EndChangeCheck() != false)
			{
				property.stringValue = (index > 0)? locales[ index] : string.Empty;
				TimelineEditor.Refresh( 
					RefreshReason.WindowNeedsRedraw|
					RefreshReason.SceneNeedsUpdate|
					RefreshReason.ContentsAddedOrRemoved|
					RefreshReason.ContentsModified);
			}
			Object[] targets = property.serializedObject.targetObjects;
			Object target = targets.FirstOrDefault( x => x is TrackAsset);
			
			if( target != false)
			{
				// using( new EditorGUILayout.HorizontalScope())
				{
					position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
					
					Rect importRect = position;
					Rect exportRect = position;
					
					exportRect.xMin = exportRect.xMax - 60;
					importRect.xMax = exportRect.xMin - 2;
					importRect.xMin = importRect.xMax - 60;
					
					EditorGUI.LabelField( position, "Serialize CSV");
					GUILayout.FlexibleSpace();
					
					using( new EditorGUI.DisabledGroupScope( targets.Length != 1))
					{
						if( GUI.Button( importRect, "Import") != false && target is TrackAsset track)
						{
							string assetPath = AssetDatabase.GetAssetPath( track);
							string directory = Path.GetDirectoryName( assetPath);
							string importPath = EditorUtility.OpenFilePanel( "Import Localize Text", directory, "csv");
							
							if( string.IsNullOrEmpty( importPath) == false)
							{
								SerializeTable table = null;
								try
								{
									string csv = File.ReadAllText( importPath) ?? string.Empty;
									string json = Serializer.Context.FromXSV( csv, ',').ToJson();
									table = JsonUtility.FromJson<SerializeTable>( json);
								}
								catch( System.Exception e)
								{
									Debug.LogException( e);
								}
								if( table != null)
								{
									Import( track, table.m_Records);
									EditorUtility.SetDirty( track);
									AssetDatabase.SaveAssetIfDirty( track);
									AssetDatabase.ImportAsset( assetPath);
									Debug.Log( $"Import: {importPath}", track);
								}
								GUIUtility.ExitGUI();
							}
						}
					}
					if( GUI.Button( exportRect, "Export") != false)
					{
						var exportAssets = new List<Object>();
						
						foreach( var track in targets.Cast<TrackAsset>())
						{
							EditorUtility.SetDirty( track);
							AssetDatabase.SaveAssetIfDirty( track);
							Export( track, exportAssets);
						}
						if( exportAssets.Count > 0)
						{
							Selection.objects = exportAssets.ToArray();
						}
						GUIUtility.ExitGUI();
					}
				}
			}
		}
		static string GetCsvPath( TrackAsset track, out string fileName)
		{
			string assetPath = AssetDatabase.GetAssetPath( track);
			string directory = Path.GetDirectoryName( assetPath);
			fileName = $"{Path.GetFileNameWithoutExtension( assetPath)}@{track.name}";
			return AssetDatabase.GenerateUniqueAssetPath( 
				Path.ChangeExtension( Path.Combine( directory, fileName), ".csv"));
		}
		void Import( TrackAsset track, List<Record> records)
		{
			int i0 = 0;
			
			foreach( var clip in track.GetClips())
			{
				if( clip.asset is LocalizeTextClip asset)
				{
					asset.m_Source = JsonUtility.FromJson<LocalizeTextBehaviour>( 
						JsonUtility.ToJson( records[ i0], false));
					asset.Initialize( clip);
				}
				++i0;
			}
			for( ; i0 < records.Count; ++i0)
			{
				var clip = track.CreateClip<LocalizeTextClip>();
				
				if( clip.asset is LocalizeTextClip asset)
				{
					asset.m_Source = JsonUtility.FromJson<LocalizeTextBehaviour>( 
						JsonUtility.ToJson( records[ i0], false));
					asset.Initialize( clip);
				}
			}
		}
		void Export( TrackAsset track, List<Object> exportObjects)
		{
			string exportPath = GetCsvPath( track, out string fileName);
			var records = new List<Record>();
			int i0 = 0;
			
			foreach( var clip in track.GetClips())
			{
				if( clip.asset is LocalizeTextClip asset)
				{
					var record = JsonUtility.FromJson<Record>( 
						JsonUtility.ToJson( asset.m_Source, false));
					record.Key = $"{i0}";
					records.Add( record);
				}
				++i0;
			}
			var csv = Serializer.Context.FromJson( JsonUtility.ToJson( 
				new SerializeTable( fileName, records))).ToXSV( ',');
			File.WriteAllText( exportPath, csv);
			AssetDatabase.ImportAsset( exportPath);
			var exportAsset = AssetDatabase.LoadAssetAtPath<Object>( exportPath);
			Debug.Log( $"Export: {exportPath}", exportAsset);
			exportObjects.Add( exportAsset);
		}
		sealed class SerializeTable
		{
			public SerializeTable( string key, List<Record> records)
			{
				m_Key = key;
				m_Records = records;
				ScriptGUID = "148cb35176397844f840df2b9ffd6fcc";
			}
			[SerializeField]
			internal string m_Key;
			[SerializeField]
			internal bool m_Remote;
			[SerializeField]
			internal List<Record> m_Records;
			[SerializeField]
			internal string ScriptGUID;
		}
	}
}
