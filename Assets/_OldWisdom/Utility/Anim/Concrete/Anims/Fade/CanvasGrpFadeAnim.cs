using IWP.Math;
using UnityEngine;

namespace IWP.Anim {
    internal sealed class CanvasGrpFadeAnim: AbstractFadeAnim {
		#region Fields

		[HideInInspector, SerializeField]
		internal CanvasGroup canvasGrp;

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal CanvasGrpFadeAnim(): base() {
			canvasGrp = null;
		}

        static CanvasGrpFadeAnim() {
        }

		#endregion

		#region Unity User Callback Event Funcs
		#endregion

		protected override void UpdateAnim() {
			canvasGrp.alpha = Val.Lerp(startAlpha, endAlpha, easingDelegate(x: Mathf.Min(1.0f, animTime / animDuration)));
		}
	}
}