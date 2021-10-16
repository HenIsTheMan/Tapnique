using UnityEngine;

namespace Genesis.Creation {
	internal sealed class EqualSignTest: MonoBehaviour {
		[SerializeField]
		private int val0;

		[SerializeField]
		private int val1;

		[SerializeField]
		private int val2;

		private int saveVal0;

		private int saveVal1;

		private int saveVal2;

		private void Awake() {
			saveVal0 = val0;
			saveVal1 = val1;
			saveVal2 = val2;

			PrintVals();
			Debug.Log("val0 = val1: " + (val0 = val1), gameObject);
			PrintVals();

			RestoreSaves();

			PrintVals();
			Debug.Log("val0 = val1 = val2: " + (val0 = val1 = val2), gameObject);
			PrintVals();

			RestoreSaves();

			PrintVals();
			Debug.Log("val1 += val2: " + (val1 += val2), gameObject);
			PrintVals();

			RestoreSaves();

			int? val3 = null;

			Debug.Log(val3, gameObject);
			PrintVals();
			Debug.Log("val3 ??= val2: " + (val3 ??= val2), gameObject);
			Debug.Log(val3, gameObject);
			PrintVals();

			RestoreSaves();
		}

		private void PrintVals() {
			Debug.Log($"{val0}, {val1}, {val2}", gameObject);
		}

		private void RestoreSaves() {
			val0 = saveVal0;
			val1 = saveVal1;
			val2 = saveVal2;
		}
    }
}