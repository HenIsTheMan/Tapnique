using UnityEngine;

namespace Genesis.Wisdom {
    internal sealed class StrTest: MonoBehaviour {
		private void Awake() {
			string myStr0 = "DIST";
			string myStr1 = new string("DIST".ToCharArray());

			Debug.Log("myStr0: " + myStr0, gameObject);
			Debug.Log("myStr1: " + myStr1, gameObject);
			Debug.Log("ReferenceEquals(myStr0, myStr1): " + ReferenceEquals(myStr0, myStr1), gameObject);

			string myStr2 = "DIST";
			string myStr3 = "DIST";

			Debug.Log("myStr2: " + myStr2, gameObject);
			Debug.Log("myStr3: " + myStr3, gameObject);
			Debug.Log("ReferenceEquals(myStr2, myStr3): " + ReferenceEquals(myStr2, myStr3), gameObject);
		}
    }
}