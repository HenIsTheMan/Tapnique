using UnityEngine;

namespace Genesis.Wisdom {
    internal sealed class DebugHelperTest: MonoBehaviour {
		[SerializeField]
		private float waitTime;

		private void Awake() {
			_ = StartCoroutine(nameof(MyCoroutine));
		}

		private System.Collections.IEnumerator MyCoroutine() {
			Debug.Log("Test", gameObject);

			yield return new WaitForSeconds(waitTime);

			DebugHelper.ClearConsole();
		}
    }
}