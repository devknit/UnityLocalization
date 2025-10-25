
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Knit.Localization.Editor
{
	public class TableExporter
	{
		[MenuItem( "Tools/Localization/Export Character/Ja")]
		internal static void ExportCharacterJa()
		{
			ExportCharacter( Locale.Ja);
		}
		[MenuItem( "Tools/Localization/Export Character/En")]
		internal static void ExportCharacterEn()
		{
			ExportCharacter( Locale.En);
		}
	#if LOCALE_LANGUAGE_ZH_HANS
		[MenuItem( "Tools/Localization/Export Character/ZhHans")]
		internal static void ExportCharacterZhHans()
		{
			ExportCharacter( Locale.ZhHans);
		}
	#endif
	#if LOCALE_LANGUAGE_ZH_HANT
		[MenuItem( "Tools/Localization/Export Character/ZhHant")]
		internal static void ExportCharacterZhHant()
		{
			ExportCharacter( Locale.ZhHant);
		}
	#endif
		static void ExportCharacter( Locale locale)
		{
			string exportPath = EditorUtility.SaveFilePanel( "Create Table Table", 
				SessionState.GetString( typeof( TableExporter).ToString(), Application.dataPath), locale.ToString(), "txt");
			
			if( exportPath.StartsWith( Application.dataPath) != false)
			{
				SessionState.SetString( typeof( TableExporter).ToString(), Path.GetDirectoryName( exportPath));
				Dictionary<string, Table> tables = Table.GetOrCreateTables();
				var characters = new HashSet<char>();
				
				foreach( Table table in tables.Values)
				{
					foreach( Record record in table.Records)
					{
						foreach( char character in record.GetString( locale))
						{
							characters.Add( character);
						}
					}
				}
				var builder = new StringBuilder();
				
				builder.Append( '\uFEFF');
				
				foreach( char character in characters.OrderBy( x => x))
				{
					if( character <= 128)
					{
						continue;
					}
					builder.Append( character);
				}
				File.WriteAllText( exportPath, builder.ToString());
				AssetDatabase.ImportAsset( exportPath);
				AssetDatabase.Refresh();
			}
		}
	}
}
