using UnityEngine;

namespace Genesis.Wisdom {
	internal sealed class StructTest: MonoBehaviour {
		private void Awake() {
			Foobar foobar0;
			foobar0.val = 4;
			Debug.Log(foobar0.val, gameObject);

			Foobar foobar1 = new Foobar(4);
			Debug.Log(foobar1.val, gameObject);
		}
    }
}