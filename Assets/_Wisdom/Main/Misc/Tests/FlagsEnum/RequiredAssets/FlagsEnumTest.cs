using UnityEngine;

namespace Genesis.Wisdom {
	internal sealed partial class FlagsEnumTest: MonoBehaviour {
		[SerializeField]
		private TestFlagsEnum testFlagsEnum;

		[SerializeField]
		private TestFlagsEnum[] testFlagsEnumArr;

		private void OnValidate() {
			if(!Application.isPlaying) {
				DebugHelper.ClearConsole();
				Debug.Log(testFlagsEnum.ToString(), gameObject);

				if(testFlagsEnumArr == null) {
					return;
				}

				string myStr = string.Empty;
				foreach(TestFlagsEnum myTestFlagsEnum in testFlagsEnumArr) {
					myStr += myTestFlagsEnum.ToString() + '\n';
				}

				if(!string.IsNullOrEmpty(myStr)) {
					myStr = myStr.Remove(myStr.Length - 1);
					Debug.Log(myStr, gameObject);
				}
			}
		}
	}
}