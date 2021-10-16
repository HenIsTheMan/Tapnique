using UnityEngine;

namespace Genesis.Wisdom {
	internal sealed class SingletonTest: MonoBehaviour {
		private void Awake() {
			Debug.Log(SampleSingleton.GlobalObj.Val, gameObject);
		}
	}
}