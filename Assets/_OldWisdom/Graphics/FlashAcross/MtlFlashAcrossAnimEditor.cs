#if UNITY_EDITOR

using UnityEditor;

namespace IWP.Anim {
	[CustomEditor(typeof(MtlFlashAcrossAnim)), CanEditMultipleObjects]
	internal sealed class MtlFlashAcrossAnimEditor: AbstractAnimEditor {
		#region Fields
		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal MtlFlashAcrossAnimEditor(): base() {
		}

		static MtlFlashAcrossAnimEditor() {
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
				"mtl",
				"thickness",
				"color",
				"dir"
			};
		}
	}
}

#endif