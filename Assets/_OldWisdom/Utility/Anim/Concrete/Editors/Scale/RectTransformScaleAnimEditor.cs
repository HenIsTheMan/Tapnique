#if UNITY_EDITOR

using UnityEditor;

namespace IWP.Anim {
	[CustomEditor(typeof(RectTransformScaleAnim)), CanEditMultipleObjects]
	internal sealed class RectTransformScaleAnimEditor: AbstractAnimEditor {
		#region Fields
		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal RectTransformScaleAnimEditor(): base() {
		}

		static RectTransformScaleAnimEditor() {
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
				"startScale",
				"endScale",
				"myRectTransform"
			};
		}
	}
}

#endif