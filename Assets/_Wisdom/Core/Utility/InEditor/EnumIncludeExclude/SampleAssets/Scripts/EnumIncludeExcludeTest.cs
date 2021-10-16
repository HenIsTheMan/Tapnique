using UnityEngine;

namespace Genesis.Wisdom {
    internal sealed partial class EnumIncludeExcludeTest: MonoBehaviour {
		[EnumIncludeExclude(
			typeof(TestEnum),
			true,
			nameof(TestEnum.Test3),
			nameof(TestEnum.Test4)
		)]
		[SerializeField]
		private TestEnum testEnumInclusion;

		[EnumIncludeExclude(
			typeof(TestEnum),
			false,
			nameof(TestEnum.Test2),
			nameof(TestEnum.Test3),
			nameof(TestEnum.Amt)
		)]
		[SerializeField]
		private TestEnum testEnumExclusion;

		private void OnValidate() {
			if(!Application.isPlaying) {
				DebugHelper.ClearConsole();
				Debug.Log(testEnumInclusion.ToString(), gameObject);
				Debug.Log(testEnumExclusion.ToString(), gameObject);
			}
		}
	}
}