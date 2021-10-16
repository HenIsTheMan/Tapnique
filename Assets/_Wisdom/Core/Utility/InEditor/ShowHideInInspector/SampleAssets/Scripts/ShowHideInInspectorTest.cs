using UnityEngine;

namespace Genesis.Wisdom {
    internal sealed partial class ShowHideInInspectorTest: MonoBehaviour {
		[SerializeField]
		[ShowHideInInspector(true, nameof(flag), true)]
		private int test0;

		[field: SerializeField]
		[field: ShowHideInInspector(true, "<Val>k__BackingField", 4)]
		private int Test1 {
			get;
			set;
		}

		[field: SerializeField]
		[field: ShowHideInInspector(true, "<Str>k__BackingField", nameof(Str))]
		private int Test2 {
			get;
			set;
		}

		[SerializeField]
		[ShowHideInInspector(true, nameof(testEnum), TestEnum.Test4)]
		private int test3;

		public bool flag;

		[field: SerializeField]
		private int Val {
			get;
			set;
		}

		[field: SerializeField]
		private string Str {
			get;
			set;
		}

		[SerializeField]
		internal TestEnum testEnum;

		private void Reset() {
			flag = true;
			Val = 4;
			Str = nameof(Str);
			testEnum = TestEnum.Test4;
		}
    }
}