#if UNITY_EDITOR

using System.IO;
using UnityEditor;

namespace Genesis.Wisdom {
    internal static partial class CreateCustom {
		private static void CreateCustomScript(string scriptTemplateFolderPath, string scriptTemplateName, string scriptTemplateFileExtension) {
			string scriptTemplateFilePath = scriptTemplateFolderPath + scriptTemplateName + '.' + scriptTemplateFileExtension;
			string folderPath;

			if(Selection.activeObject == null) {
				folderPath = "Assets" + Path.AltDirectorySeparatorChar;
			} else {
				string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);

				if(File.Exists(assetPath)) {
					folderPath = Path.GetDirectoryName(assetPath) + Path.DirectorySeparatorChar;
				} else {
					folderPath = assetPath + Path.AltDirectorySeparatorChar;
				}
			}

			File.Copy(scriptTemplateFilePath, folderPath + scriptTemplateName + ".cs");
			AssetDatabase.Refresh();
		}
	}
}

#endif