#if UNITY_EDITOR

using UnityEditor;

namespace IWP.Anim {
	[CustomEditor(typeof(ImgFillAmtAnim)), CanEditMultipleObjects]
	internal sealed class ImgFillAmtAnimEditor: AbstractAnimEditor {
		#region Fields
		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal ImgFillAmtAnimEditor(): base() {
		}

		static ImgFillAmtAnimEditor() {
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
				"img",
				"startFillAmt",
				"endFillAmt"
			};
		}
	}
}

#endif