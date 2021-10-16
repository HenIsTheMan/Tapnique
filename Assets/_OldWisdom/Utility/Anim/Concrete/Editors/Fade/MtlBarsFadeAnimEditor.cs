#if UNITY_EDITOR

using UnityEditor;

namespace IWP.Anim {
	[CustomEditor(typeof(MtlBarsFadeAnim)), CanEditMultipleObjects]
	internal sealed class MtlBarsFadeAnimEditor: AbstractAnimEditor {
		#region Fields
		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal MtlBarsFadeAnimEditor(): base() {
		}

		static MtlBarsFadeAnimEditor() {
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
				"mtl",
				"barCount",
				"barOrientation"
			};
		}
	}
}

#endif