#if UNITY_EDITOR

using UnityEditor;

namespace IWP.Anim {
	[CustomEditor(typeof(RectTransformShakeAnim)), CanEditMultipleObjects]
	internal sealed class RectTransformShakeAnimEditor: AbstractAnimEditor {
		#region Fields
		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal RectTransformShakeAnimEditor(): base() {
		}

		static RectTransformShakeAnimEditor() {
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
				"startPos",
				"minOffset",
				"maxOffset",
				"maxMoveCount",
				"myRectTransform"
			};
		}
	}
}

#endif