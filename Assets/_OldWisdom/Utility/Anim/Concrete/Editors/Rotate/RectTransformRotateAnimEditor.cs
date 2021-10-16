#if UNITY_EDITOR

using UnityEditor;

namespace IWP.Anim {
	[CustomEditor(typeof(RectTransformRotateAnim)), CanEditMultipleObjects]
	internal sealed class RectTransformRotateAnimEditor: AbstractAnimEditor {
		#region Fields
		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal RectTransformRotateAnimEditor(): base() {
		}

		static RectTransformRotateAnimEditor() {
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
				"shldAnimateX",
				"shldAnimateY",
				"shldAnimateZ",
				"startEulerAngles",
				"endEulerAngles",
				"myRectTransform"
			};
		}
	}
}

#endif