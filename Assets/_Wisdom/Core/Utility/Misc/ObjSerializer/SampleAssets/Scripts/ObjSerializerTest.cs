using System.Collections.Generic;
using UnityEngine;

namespace Genesis.Wisdom {
    internal sealed class ObjSerializerTest: MonoBehaviour {
		[SerializeField]
		private int[] arrOfVals;

		[SerializeField]
		private List<int> listOfVals;

		private void Awake() {
			byte[] serialized;
			string str;

			str = string.Empty;
			foreach(int val in arrOfVals) {
				str += val + ", ";
			}
			str = str.Substring(0, str.Length - 2);

			Debug.Log(str, gameObject);

			serialized = ObjSerializer.Serialize(arrOfVals);
			arrOfVals = ObjSerializer.Deserialize<int[]>(serialized);

			str = string.Empty;
			foreach(int val in arrOfVals) {
				str += val + ", ";
			}
			str = str.Substring(0, str.Length - 2);

			Debug.Log(str, gameObject);

			listOfVals.ForEach((val) => {
				Debug.Log(val, gameObject);
			});

			serialized = listOfVals.Serialize();
			listOfVals = serialized.Deserialize<List<int>>();

			listOfVals.ForEach((val) => {
				Debug.Log(val, gameObject);
			});
		}
    }
}