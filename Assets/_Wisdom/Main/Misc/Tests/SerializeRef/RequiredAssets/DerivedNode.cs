using UnityEngine;

namespace Genesis.Wisdom {
	internal sealed partial class SerializeRefTest {
		[System.Serializable]
		internal sealed class DerivedNode: BaseNode {
			[SerializeField]
			internal string myOtherStr = nameof(DerivedNode);
		}
	}
}