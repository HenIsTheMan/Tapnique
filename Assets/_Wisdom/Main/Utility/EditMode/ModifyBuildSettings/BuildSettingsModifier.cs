#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Genesis.Wisdom {
	[ExecuteInEditMode]
	internal sealed partial class BuildSettingsModifier: EditModeTaskPerformer {
		[SerializeField]
		private int index;

		[SerializeField]
		private SceneUnitsWrapper[] allSceneUnits;

		private void OnValidate() {
			if(!EditorApplication.isPlayingOrWillChangePlaymode
				&& allSceneUnits != null
				&& allSceneUnits.Length > 0
			) {
				index = Mathf.Clamp(index, 0, allSceneUnits.Length - 1);
			}
		}

		protected override void OnEnable() {
			SceneUnit[] selectedSceneUnits = allSceneUnits[index].sceneUnits;
			int selectedSceneUnitsLen = selectedSceneUnits.Length;

			List<string> sceneNames = new List<string>(selectedSceneUnitsLen);
			string sceneName;

			for(int i = 0; i < selectedSceneUnitsLen; ++i) {
				sceneName = selectedSceneUnits[i].sceneAsset.name;

				if(sceneNames.Contains(sceneName)) {
					TaskPerformanceOutcome("ModifyBuildSettings Failure!");
					return;
				}

				sceneNames.Add(sceneName);
			}

			SceneUnit sceneUnit;
			List<EditorBuildSettingsScene> editorBuildSettingsSceneContainer
				= new List<EditorBuildSettingsScene>(selectedSceneUnitsLen);

			for(int i = 0; i < selectedSceneUnitsLen; ++i) {
				sceneUnit = selectedSceneUnits[i];

				editorBuildSettingsSceneContainer.Add(new EditorBuildSettingsScene(
					AssetDatabase.GetAssetPath(sceneUnit.sceneAsset),
					sceneUnit.isEnabled
				));
			}

			EditorBuildSettings.scenes = editorBuildSettingsSceneContainer.ToArray();

			TaskPerformanceOutcome("ModifyBuildSettings Success!");
		}
	}
}

#endif