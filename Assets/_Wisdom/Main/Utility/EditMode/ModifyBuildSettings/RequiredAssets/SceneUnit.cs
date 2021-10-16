#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Genesis.Wisdom {
	internal sealed partial class BuildSettingsModifier {
		[System.Serializable]
		internal struct SceneUnit {
			[SerializeField]
			internal bool isEnabled;

			[SerializeField]
			internal SceneAsset sceneAsset;
		};
	}
}

#endif