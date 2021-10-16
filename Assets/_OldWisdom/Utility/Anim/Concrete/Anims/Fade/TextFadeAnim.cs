using IWP.Math;
using TMPro;
using UnityEngine;

namespace IWP.Anim {
    internal sealed class TextFadeAnim: AbstractFadeAnim {
		#region Fields

		[HideInInspector, SerializeField]
		internal TextMeshProUGUI tmpComponent;

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal TextFadeAnim(): base() {
			tmpComponent = null;
		}

        static TextFadeAnim() {
        }

        #endregion

        #region Unity User Callback Event Funcs
		#endregion

		protected override void UpdateAnim() {
			tmpComponent.alpha = Val.Lerp(startAlpha, endAlpha, easingDelegate(x: Mathf.Min(1.0f, animTime / animDuration)));
		}
	}
}