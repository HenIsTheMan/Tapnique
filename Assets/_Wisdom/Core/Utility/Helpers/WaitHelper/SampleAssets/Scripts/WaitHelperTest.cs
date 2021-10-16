using System.Collections;
using UnityEngine;

namespace Genesis.Wisdom {
	internal sealed class WaitHelperTest: MonoBehaviour {
		[Min(0.0f)]
		[SerializeField]
		private float timeScale;

		[SerializeField]
		private float delay;

		[SerializeField]
		private float elapsedTimeThreshold;

		private float elapsedTime = 0.0f;

		private int count = 0;

		private void Reset() {
			timeScale = 1.0f;
		}

		private void Awake() {
			Time.timeScale = timeScale;
			WaitHelper.AddWaitForSeconds(delay);
		}

		private IEnumerator Start() {
			yield return new WaitUntil(() => {
				return elapsedTime > elapsedTimeThreshold;
			});

			while(true) {
				DebugHelper.ClearConsole();
				Debug.Log(++count, gameObject);

				yield return WaitHelper.GetWaitForSeconds(delay);
			}
		}

		private void Update() {
			DebugHelper.ClearConsole();
			Debug.Log(elapsedTime, gameObject);

			elapsedTime += Time.deltaTime;

			if(elapsedTime > elapsedTimeThreshold) {
				enabled = false;
			}
		}
    }
}