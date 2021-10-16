using UnityEngine;

namespace Genesis.Wisdom {
    internal sealed class ValHelperTest: MonoBehaviour {
		[SerializeField]
		private int numberToReverse;

		private void Awake() {
			Debug.Log(numberToReverse.Reverse(), gameObject);
		}
    }
}