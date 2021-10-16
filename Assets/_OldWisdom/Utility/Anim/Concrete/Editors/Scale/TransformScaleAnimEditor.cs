#if UNITY_EDITOR

using UnityEditor;

namespace IWP.Anim {
	[CustomEditor(typeof(TransformScaleAnim)), CanEditMultipleObjects]
	internal sealed class TransformScaleAnimEditor: AbstractAnimEditor {
		#region Fields
		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal TransformScaleAnimEditor(): base() {
		}

		static TransformScaleAnimEditor() {
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
				"myTransform"
			};
		}
	}
}

#endif