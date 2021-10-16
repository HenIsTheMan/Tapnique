using UnityEngine;

namespace Genesis.Wisdom {
    internal sealed class CoroutineTest: MonoBehaviour {
		private Coroutine myCoroutine;

		private void Awake() {
			_ = StartCoroutine(nameof(Routine));
			//_ = StartCoroutine(Routine());

			myCoroutine = StartCoroutine(Test(4, 4));
		}

		private void Update() {
			if(Input.GetMouseButtonDown(0)) {
				StopCoroutine(nameof(Routine));
				//StopCoroutine(Routine());

				//StopCoroutine(nameof(Test));
				//myCoroutine = null;
				StopCoroutine(myCoroutine);
			}
		}

		private System.Collections.IEnumerator Routine() {
			for(;;) {
				Debug.Log("Running", gameObject);
				yield return null;
			}
		}

		private System.Collections.IEnumerator Test(int a, int b) {
			while(true) {
				Debug.Log(a + b, gameObject);
				yield return null;
			}
		}
	}
}