using UnityEngine;

namespace Genesis.Wisdom {
    internal abstract class EditModeTaskPerformer: MonoBehaviour {
		protected virtual void Reset() {
			enabled = false;
		}

		protected virtual void Awake() {
			enabled = false;
		}

		protected abstract void OnEnable();

		protected void TaskPerformanceOutcome(string msg) {
			Debug.Log(msg, gameObject);
			enabled = false;
		}
    }
}