using UnityEngine;

namespace Genesis.Wisdom {
    internal sealed class VibrationMasterTest: MonoBehaviour {
		public void OnClick0() {
			VibrationMaster.StartVibration();
		}

		public void OnClick1() {
			VibrationMaster.StartVibration(1000);
		}

		public void OnClick2() {
			VibrationMaster.StartVibration(new long[] {0, 100, 1000, 200, 2000}, -1);
		}

		public void OnClick3() {
			VibrationMaster.StopVibration();
		}
	}
}