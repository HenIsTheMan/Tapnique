#if UNITY_EDITOR

using UnityEditor;

namespace IWP.Anim {
	[CustomEditor(typeof(TransformRotateAnim)), CanEditMultipleObjects]
	internal sealed class TransformRotateAnimEditor: AbstractAnimEditor {
		#region Fields
		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal TransformRotateAnimEditor(): base() {
		}

		static TransformRotateAnimEditor() {
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
				"myTransform"
			};
		}
	}
}

#endif