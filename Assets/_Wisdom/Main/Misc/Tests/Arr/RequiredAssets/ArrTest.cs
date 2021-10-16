using System.Linq;
using UnityEngine;

namespace Genesis.Wisdom {
    internal sealed class ArrTest: MonoBehaviour {
		[SerializeField]
		private int[] valContainer;

		private void Reset() {
			valContainer = new int[] {
				7,
				70,
				700
			};

			int[] otherValContainer = {
				4,
				40,
				400
			};

			valContainer = otherValContainer;

			otherValContainer[2] = 0;
		}

		private void OnValidate() {
			if(!Application.isPlaying) {
				DebugHelper.ClearConsole();
				Debug.Log($"Min: {valContainer.Min()}", gameObject);
				Debug.Log($"Max: {valContainer.Max()}", gameObject);
				Debug.Log($"Sum: {valContainer.Sum()}", gameObject);
			}
		}
    }
}