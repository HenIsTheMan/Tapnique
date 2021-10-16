#if UNITY_EDITOR

using UnityEditor;

namespace IWP.Anim {
	[CustomEditor(typeof(TextMtlFlashAcrossAnim)), CanEditMultipleObjects]
	internal sealed class TextMtlFlashAcrossAnimEditor: AbstractAnimEditor {
		#region Fields
		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal TextMtlFlashAcrossAnimEditor(): base() {
		}

		static TextMtlFlashAcrossAnimEditor() {
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
				"dir",
				"tmpTextComponent",
				"myTransform",
				"extraXOffsetFromOrigin",
				"extraYOffsetFromOrigin"
			};
		}
	}
}

#endif