using UnityEngine;

namespace Genesis.Creation {
    internal sealed class ChangeSceneInAwake: MonoBehaviour {
		[SerializeField]
		private SceneChangeUnit sceneChangeUnit;

		private void Awake() {
			sceneChangeUnit.ChangeScene();
		}
    }
}