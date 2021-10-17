using UnityEngine;

namespace Genesis.Creation {
	[CreateAssetMenu(
		fileName = nameof(ObjPoolData),
		menuName = "ScriptableObjs/" + nameof(ObjPoolData)
	)]
	internal sealed class ObjPoolData: ScriptableObject {
		[field: SerializeField]
		internal int Size {
			get;
			private set;
		}

		[field: SerializeField]
		internal GameObject Prefab {
			get;
			private set;
		}

		[field: SerializeField]
		internal string InstanceName {
			get;
			private set;
		}
	}
}