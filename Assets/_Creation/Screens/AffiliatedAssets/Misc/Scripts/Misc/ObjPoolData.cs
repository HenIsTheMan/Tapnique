using UnityEngine;

namespace Genesis.Creation {
	[CreateAssetMenu(
		fileName = nameof(ObjPoolData),
		menuName = "ScriptableObjs/" + nameof(ObjPoolData)
	)]
	internal sealed class ObjPoolData: ScriptableObject {
		[SerializeField]
		private int size;

		[SerializeField]
		private GameObject prefab;

		[SerializeField]
		private Transform parentTransform;

		[SerializeField]
		private string instanceName;
	}
}