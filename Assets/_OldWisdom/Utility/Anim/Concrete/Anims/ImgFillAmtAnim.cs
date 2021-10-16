using IWP.Math;
using UnityEngine;
using UnityEngine.UI;

namespace IWP.Anim {
    internal sealed class ImgFillAmtAnim: AbstractAnim {
		#region Fields

		[HideInInspector, SerializeField]
		internal Image img;

		[HideInInspector, SerializeField]
		internal float startFillAmt;

		[HideInInspector, SerializeField]
		internal float endFillAmt;

        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        internal ImgFillAmtAnim(): base() {
			img = null;

			startFillAmt = 0.0f;
			endFillAmt = 0.0f;
        }

        static ImgFillAmtAnim() {
        }

		#endregion

		#region Unity User Callback Event Funcs
		#endregion

		protected override void MyOnValidate() {
			startFillAmt = Mathf.Max(0.0f, startFillAmt);
			endFillAmt = Mathf.Max(0.0f, endFillAmt);
		}

		protected override void UpdateAnim() {
			img.fillAmount = Val.Lerp(startFillAmt, endFillAmt, easingDelegate(x: Mathf.Min(1.0f, animTime / animDuration)));
		}
	}
}