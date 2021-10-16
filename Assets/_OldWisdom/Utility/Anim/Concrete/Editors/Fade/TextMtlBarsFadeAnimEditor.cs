#if UNITY_EDITOR

using UnityEditor;

namespace IWP.Anim {
	[CustomEditor(typeof(TextMtlBarsFadeAnim)), CanEditMultipleObjects]
	internal sealed class TextMtlBarsFadeAnimEditor: AbstractAnimEditor {
		#region Fields
		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal TextMtlBarsFadeAnimEditor(): base() {
		}

		static TextMtlBarsFadeAnimEditor() {
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
				"barOrientation",
				"tmpTextComponent",
				"myTransform",
				"extraXOffsetFromOrigin",
				"extraYOffsetFromOrigin"
			};
		}
	}
}

#endif