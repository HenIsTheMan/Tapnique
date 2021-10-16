#if UNITY_EDITOR

using UnityEditor;

namespace IWP.Anim {
	[CustomEditor(typeof(MtlFadeAnim)), CanEditMultipleObjects]
	internal sealed class MtlFadeAnimEditor: AbstractAnimEditor {
		#region Fields
		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal MtlFadeAnimEditor(): base() {
		}

		static MtlFadeAnimEditor() {
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
				"startAlpha",
				"endAlpha",
				"shldResetToOG",
				"mtl"
			};
		}
	}
}

#endif