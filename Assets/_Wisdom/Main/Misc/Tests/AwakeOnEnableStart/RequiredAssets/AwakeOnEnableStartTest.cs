using UnityEngine;

namespace Genesis.Wisdom {
	internal sealed class AwakeOnEnableStartTest: MonoBehaviour {
		private void Awake() {
			Debug.Log(name + ' ' + nameof(Awake), gameObject);
		}

		private void OnEnable() {
			Debug.Log(name + ' ' + nameof(OnEnable), gameObject);
		}

		private void Start() {
			Debug.Log(name + ' ' + nameof(Start), gameObject);
		}
    }
}