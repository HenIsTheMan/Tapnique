using UnityEngine;

namespace Genesis.Wisdom {
	internal sealed partial class ExistenceEnforcer {
		[System.Serializable]
		internal struct ExistenceInfo {
			[SerializeField]
			internal GameObject prefabGameObj;

			[SerializeField]
			internal string myName;

			[SerializeField]
			internal Vector3 pos;

			[SerializeField]
			internal Quaternion rotation;

			[SerializeField]
			internal Transform parentTransform;
		};
	}
}