#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Genesis.Wisdom {
	[ExecuteInEditMode]
	internal sealed class OnValidateTest: MonoBehaviour {
		[ContextMenu("Refresh")]
		private void OnValidate() {
			Debug.Log(nameof(OnValidate), gameObject);

			Debug.Log($"{nameof(Application.isEditor)}: {Application.isEditor}", gameObject);
			Debug.Log($"{nameof(Application.isPlaying)}: {Application.isPlaying}", gameObject);

			Debug.Log(
				$"{nameof(EditorApplication.isPlayingOrWillChangePlaymode)}: {EditorApplication.isPlayingOrWillChangePlaymode}",
				gameObject
			);
		}

		private void OnEnable() {
			enabled = false;
		}
    }
}

#endif