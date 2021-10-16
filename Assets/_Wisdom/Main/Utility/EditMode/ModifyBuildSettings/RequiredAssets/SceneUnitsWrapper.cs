#if UNITY_EDITOR

using UnityEngine;

namespace Genesis.Wisdom {
	internal sealed partial class BuildSettingsModifier {
		[System.Serializable]
		internal sealed class SceneUnitsWrapper { //Wrapper Class
			[SerializeField]
			internal SceneUnit[] sceneUnits;
		}
	}
}

#endif