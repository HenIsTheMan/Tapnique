#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Genesis.Wisdom {
    internal sealed class UnmodifiableInInspectorTest: MonoBehaviour {
		[UnmodifiableInInspector(AttribWorkingPeriod.Always)]
		[SerializeField]
		private int val;

		[UnmodifiableInInspector(AttribWorkingPeriod.PlayMode)]
		[SerializeField]
		private int minVal;

		[UnmodifiableInInspector(AttribWorkingPeriod.PlayMode)]
		[SerializeField]
		private int maxVal;

		[field: UnmodifiableInInspector(AttribWorkingPeriod.EditMode)]
		[field: SerializeField]
		private int OtherVal {
			get;
			set;
		}

		private void OnValidate() {
			if(!EditorApplication.isPlayingOrWillChangePlaymode) {
				val = Random.Range(minVal, maxVal + 1);
				EditorSceneManager.SaveScene(gameObject.scene);
			}
		}
	}
}

#endif