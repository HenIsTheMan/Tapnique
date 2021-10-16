#if UNITY_EDITOR

using UnityEditor;

namespace IWP.Anim {
	[CustomEditor(typeof(ImgFadeAnim)), CanEditMultipleObjects]
	internal sealed class ImgFadeAnimEditor: AbstractAnimEditor {
		#region Fields
		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal ImgFadeAnimEditor(): base() {
		}

		static ImgFadeAnimEditor() {
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
				"img"
			};
		}
	}
}

#endif