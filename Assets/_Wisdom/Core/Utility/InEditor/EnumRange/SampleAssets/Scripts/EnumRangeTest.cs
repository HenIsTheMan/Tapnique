using UnityEngine;

namespace Genesis.Wisdom {
    internal sealed partial class EnumRangeTest: MonoBehaviour {
		[EnumRange(typeof(TestEnum), (int)TestEnum.Test1, (int)TestEnum.Amt)]
		[SerializeField]
		private TestEnum testEnum;

		private void OnValidate() {
			if(!Application.isPlaying) {
				DebugHelper.ClearConsole();
				Debug.Log(testEnum.ToString(), gameObject);
			}
		}
    }
}