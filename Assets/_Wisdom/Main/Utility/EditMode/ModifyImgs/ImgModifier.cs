#if UNITY_EDITOR

using System.IO;
using UnityEditor;
using UnityEngine;

namespace Genesis.Wisdom {
	[ExecuteInEditMode]
	internal sealed class ImgModifier: EditModeTaskPerformer {
		[SerializeField]
		private string folderPath;

		[SerializeField]
		private bool shldChangeTexImporterType;

		[SerializeField]
		private TextureImporterType texImporterType;

		[SerializeField]
		private bool shldChangeDefaultPlatTexSettings;
		
		[SerializeField]
		private TextureImporterFormat defaultTexImporterFormat;

		[SerializeField]
		private bool shldChangeStandalonePlatTexSettings;

		[SerializeField]
		private TextureImporterFormat standaloneTexImporterFormat;

		[SerializeField]
		private bool shldChangeAndroidPlatTexSettings;

		[SerializeField]
		private TextureImporterFormat androidTexImporterFormat;

		protected override void OnEnable() {
			TextureImporter texImporter;
			TextureImporterPlatformSettings texImporterPlatSettings;
			string[] filePaths = Directory.GetFiles(folderPath);

			foreach(string filePath in filePaths) {
				if(AssetDatabase.GetMainAssetTypeAtPath(filePath) == typeof(Texture2D)) {
					texImporter = AssetImporter.GetAtPath(filePath) as TextureImporter;

					if(shldChangeTexImporterType) {
						texImporter.textureType = texImporterType;
					}

					if(shldChangeDefaultPlatTexSettings) {
						texImporterPlatSettings = texImporter.GetDefaultPlatformTextureSettings();
						texImporterPlatSettings.format = defaultTexImporterFormat;
						texImporter.SetPlatformTextureSettings(texImporterPlatSettings);
					}

					if(shldChangeStandalonePlatTexSettings) {
						texImporterPlatSettings = texImporter.GetPlatformTextureSettings("Standalone");
						texImporterPlatSettings.overridden = true;
						texImporterPlatSettings.format = standaloneTexImporterFormat;
						texImporter.SetPlatformTextureSettings(texImporterPlatSettings);
					}

					if(shldChangeAndroidPlatTexSettings) {
						texImporterPlatSettings = texImporter.GetPlatformTextureSettings("Android");
						texImporterPlatSettings.overridden = true;
						texImporterPlatSettings.format = androidTexImporterFormat;
						texImporter.SetPlatformTextureSettings(texImporterPlatSettings);
					}

					AssetDatabase.ImportAsset(filePath);
				}
			}

			TaskPerformanceOutcome("ModifyImgs Success!");
		}
    }
}

#endif