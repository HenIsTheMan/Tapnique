using UnityEngine;

namespace Genesis.Wisdom {
	internal sealed class ObjCopierTest: MonoBehaviour {
		[System.Serializable]
		private sealed class IntWrapperClass {
			internal int val;
		};

		[SerializeField]
		private int initialVal;

		[SerializeField]
		private int changedVal;

		private void Awake() {
			IntWrapperClass instance0 = new IntWrapperClass() {
				val = initialVal
			};
			IntWrapperClass instance1;

			instance1 = instance0; //Shallow copy
			Debug.Log(instance1.val, gameObject);
			instance0.val = changedVal;
			Debug.Log(instance1.val, gameObject);

			instance0.val = initialVal;

			instance1 = instance0.DeepCopy(); //Deep copy
			Debug.Log(instance1.val, gameObject);
			instance0.val = changedVal;
			Debug.Log(instance1.val, gameObject);
		}
    }
}