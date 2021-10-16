using System.Collections.Generic;
using UnityEngine;

namespace Genesis.Wisdom {
    internal sealed class ShuffleElementsTest: MonoBehaviour {
		[SerializeField]
		private int[] arrOfVals;

		[SerializeField]
		private List<int> listOfVals;

		private void Awake() {
			string str;

			str = string.Empty;
			foreach(int val in arrOfVals) {
				str += val + ", ";
			}
			str = str.Substring(0, str.Length - 2);

			Debug.Log(str, gameObject);

			ShuffleElements.Shuffle(arrOfVals);

			str = string.Empty;
			foreach(int val in arrOfVals) {
				str += val + ", ";
			}
			str = str.Substring(0, str.Length - 2);

			Debug.Log(str, gameObject);

			listOfVals.ForEach((val) => {
				Debug.Log(val, gameObject);
			});

			listOfVals.Shuffle();

			listOfVals.ForEach((val) => {
				Debug.Log(val, gameObject);
			});
		}
    }
}