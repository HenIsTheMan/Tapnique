using System.Threading.Tasks;
using UnityEngine;

namespace Genesis.Wisdom {
	internal sealed class OnScreenConsoleTest: MonoBehaviour {
		[SerializeField]
		private string msg0;

		[SerializeField]
		private string msg1;

		[SerializeField]
		private int taskDelayInMilliseconds;

		private void Reset() {
			msg0 = nameof(msg0);
			msg1 = nameof(msg1);

			taskDelayInMilliseconds = 1400;
		}

		private async void Awake() {
			Debug.Log(msg0, gameObject);

			await Task.Delay(taskDelayInMilliseconds);

			Debug.Log(msg1, gameObject);
		}
    }
}