#if UNITY_EDITOR

using UnityEngine;

namespace Genesis.Wisdom {
    internal abstract class SingletonEditModeTaskPerformer<T>: Singleton<T> where T:
		MonoBehaviour
	{
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

#endif