
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Text.RegularExpressions;

namespace Knit.Localization.Editor
{
	[CustomPropertyDrawer( typeof( LocalizeReference), true)]
	public sealed class LocalizeReferenceDrawer : PropertyDrawer
	{
		public static float GetPropertyHeight( SerializedProperty property)
		{
			float height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			
			if( property.isExpanded != false)
			{
				height += (Localize.GetLocales().Count() + 1) * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
			}
			return height;
		}
		public static void OnGUI( LocalizeReferenceDrawerContext context, Rect position, SerializedProperty property, GUIContent label)
		{
			context.UpdateProperty( property, Table.GetOrCreateTables());
			
			var rowPosition = new Rect( position.x, position.y, 
				position.width, EditorGUIUtility.singleLineHeight);
			var foldoutRect = new Rect( rowPosition.x, rowPosition.y, 
				EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);
			property.isExpanded = EditorGUI.Foldout( foldoutRect, property.isExpanded, label, true);
			
			/* ドロップダウンメニュー */
			var dropDownPosition = new Rect( foldoutRect.xMax, rowPosition.y, 
				rowPosition.width - EditorGUIUtility.labelWidth - 30, EditorGUIUtility.singleLineHeight);
			
			if( EditorGUI.DropdownButton( dropDownPosition, context.Content, FocusType.Passive) != false)
			{
				var referenceView = new LocalizeReferencePreview( 
					context.TableKey, context.EntryKey,
					Table.GetOrCreateTables(), (tableKey, entryKey) =>
				{
					context.SetProperty( tableKey, entryKey);
				});
				PopupWindow.Show( dropDownPosition, new LocalizeReferencePreviewContent( referenceView, dropDownPosition.width));
			}
			/* テーブルウィンドウ展開ボタン */
			var buttonPosition = new Rect( dropDownPosition.xMax, 
				rowPosition.y, 30, EditorGUIUtility.singleLineHeight);
			if( GUI.Button( buttonPosition, kIconTexture) != false)
			{
				TableWindow.Open( context.TableKey, context.EntryKey);
			}
			/* ロケール毎の値を表示 */
			if( property.isExpanded != false)
			{
				rowPosition.xMin += 15;
				rowPosition.y += rowPosition.height + EditorGUIUtility.standardVerticalSpacing;
				
				EditorGUI.BeginChangeCheck();
				string tableEntry = EditorGUI.DelayedTextField( rowPosition, $"{context.TableKey}@{context.EntryKey}");
				if( EditorGUI.EndChangeCheck() != false)
				{
					string[] table0Entry1 = tableEntry.Split( '@');
					
					if( table0Entry1.Length != 2)
					{
						Debug.LogError( "Input formatting is incorrect.");
					}
					else if( string.IsNullOrEmpty( table0Entry1[ 0]) != false)
					{
						context.SetProperty( table0Entry1[ 0], Regex.Replace( table0Entry1[ 1], @"[^\x20-\x7e]", string.Empty));
					}
					else if( Table.GetOrCreateTables().TryGetValue( table0Entry1[ 0], out Table table) == false)
					{
						Debug.LogError( $"The table {table0Entry1[ 0]} was not found.");
					}
					else if( table.Keys.Contains( table0Entry1[ 1]) == false)
					{
						Debug.LogError( $"The entry {table0Entry1[ 1]} was not found.");
					}
					else
					{
						context.SetProperty( table0Entry1[ 0], table0Entry1[ 1]);
					}
				}
				rowPosition.y += rowPosition.height + EditorGUIUtility.standardVerticalSpacing;
				
				if( (context.locales?.Count ?? 0) > 0)
				{
					foreach( var locale in context.locales)
					{
						EditorGUI.LabelField( rowPosition, locale.value, locale.label);
						rowPosition.y += rowPosition.height + EditorGUIUtility.standardVerticalSpacing;
					}
				}
				else
				{
					foreach( var locale in Localize.GetLocales())
					{
						EditorGUI.LabelField( rowPosition, locale, "<empty>");
						rowPosition.y += rowPosition.height + EditorGUIUtility.standardVerticalSpacing;
					}
				}
			}
		}
		public override void OnGUI( Rect position, SerializedProperty property, GUIContent label)
		{
			OnGUI( m_Context, position, property, label);
		}
		public override float GetPropertyHeight( SerializedProperty property, GUIContent label)
		{
			return GetPropertyHeight( property);
		}
		static LocalizeReferenceDrawer()
		{
			kIconTexture = new Texture2D( 1, 1, TextureFormat.ARGB32, false, false)
			{
				hideFlags = HideFlags.HideAndDontSave
			};
			kIconTexture.LoadImage( System.Convert.FromBase64String( kIconBase64));
		}
		const string kIconBase64 = 
			"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";
		static readonly Texture2D kIconTexture;
        readonly LocalizeReferenceDrawerContext m_Context = new();
	}
}
