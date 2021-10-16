using System.Collections.Generic;
using UnityEngine;

namespace Genesis.Wisdom {
	internal sealed partial class SerializeRefTest: MonoBehaviour {
		[SerializeField]
		private List<INode> nodeList0;

		[SerializeReference]
		private List<INode> nodeList1 = new List<INode>() {
			new BaseNode(),
			new DerivedNode()
		};
	}
}