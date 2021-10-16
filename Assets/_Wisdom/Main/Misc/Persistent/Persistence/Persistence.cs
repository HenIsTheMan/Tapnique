using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace Genesis.Wisdom {
	internal sealed class Persistence: MonoBehaviour {
		[SerializeField]
		private Transform myTransform;

		#if UNITY_EDITOR

		private void OnValidate() {
			if(!EditorApplication.isPlayingOrWillChangePlaymode) {
				myTransform ??= transform;
			}
		}

		#endif

		private void Awake() {
			if(myTransform.parent != null) {
				UnityEngine.Assertions.Assert.IsTrue(false, "myTransform.parent != null");
				return;
			}

			DontDestroyOnLoad(myTransform.gameObject);
		}
	}
}