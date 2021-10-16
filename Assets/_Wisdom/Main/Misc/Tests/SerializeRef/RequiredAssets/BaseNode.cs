using UnityEngine;

namespace Genesis.Wisdom {
	internal sealed partial class SerializeRefTest {
		[System.Serializable]
		internal class BaseNode: INode {
			[field: SerializeField] //Must have
			public string MyStr {
				get;
				set;
			} = nameof(BaseNode);
		}
	}
}