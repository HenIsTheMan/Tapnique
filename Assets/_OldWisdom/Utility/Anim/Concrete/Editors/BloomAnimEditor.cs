#if UNITY_EDITOR

using UnityEditor;

namespace IWP.Anim {
	[CustomEditor(typeof(BloomAnim)), CanEditMultipleObjects]
	internal sealed class BloomAnimEditor: AbstractAnimEditor {
		#region Fields
		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal BloomAnimEditor(): base() {
		}

		static BloomAnimEditor() {
		}

		#endregion

		#region Unity User Callback Event Funcs
		#endregion

		protected override void InitNames() {
			names = new string[]{
				"initControl",
				"isUpdating",
				"shldInitVals",
				"periodicDelay",
				"startTimeOffset",
				"animDuration",
				"countThreshold",
				"easingType",
				"shldResetToOG",
				"myVol",
				"startIntensity",
				"endIntensity",
				"startScatter",
				"endScatter"
			};
		}
	}
}

#endif