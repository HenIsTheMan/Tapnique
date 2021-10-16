using UnityEngine;

namespace Genesis.Wisdom {
    internal sealed class InterfaceTest: MonoBehaviour, IFoobar {
		public int Test {
			get;
			set;
		} = 4;

		public void TestFunc() {
			Debug.Log(Test, gameObject);
		}

		private void Awake() {
			TestFunc();
		}
	}
}