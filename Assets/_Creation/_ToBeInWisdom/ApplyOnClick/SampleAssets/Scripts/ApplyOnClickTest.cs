using UnityEngine;

namespace Genesis.Creation {
	internal sealed class ApplyOnClickTest: MonoBehaviour {
		public void TestFunc() {
			Debug.Log(nameof(TestFunc), gameObject);
		}

		public void TestFunc(bool flag) {
			Debug.Log(flag, gameObject);
		}

		public void TestFunc(int val) {
			Debug.Log(val, gameObject);
		}

		public void TestFunc(string str) {
			Debug.Log(str, gameObject);
		}
    }
}