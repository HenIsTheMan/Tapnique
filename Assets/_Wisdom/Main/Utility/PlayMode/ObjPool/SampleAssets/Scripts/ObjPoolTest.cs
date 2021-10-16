using UnityEngine;

namespace Genesis.Wisdom {
    internal sealed class ObjPoolTest: MonoBehaviour {
		[SerializeField]
		private ObjPool cubePool;

		[SerializeField]
		private int size;

		[SerializeField]
		private GameObject prefab;

		[SerializeField]
		private Transform parentTransform;

		[SerializeField]
		private string instanceName;

		private void Awake() {
			cubePool.InitMe(size, prefab, parentTransform, instanceName);
		}
    }
}