#if UNITY_EDITOR

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace Genesis.Wisdom {
    internal sealed class SimpleSceneChanger: MonoBehaviour {
		[SerializeField]
		private SceneAsset sceneAsset;

		[UnmodifiableInInspector]
		[SerializeField]
		private string sceneName;

		private void OnValidate() {
			if(!EditorApplication.isPlayingOrWillChangePlaymode) {
				sceneName = sceneAsset.name;
			}
		}

		private void Awake() {
			SceneManager.LoadScene(sceneName);
		}
    }
}

#endif