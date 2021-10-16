using IWP.Math;
using UnityEngine;
using UnityEngine.UI;

namespace IWP.Anim {
    internal sealed class ImgFadeAnim: AbstractFadeAnim {
		#region Fields

		private Color color;

		[HideInInspector, SerializeField]
		internal Image img;

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal ImgFadeAnim(): base() {
			color = Color.white;

			img = null;
		}

        static ImgFadeAnim() {
        }

		#endregion

		#region Unity User Callback Event Funcs
		#endregion

		protected override void InitCore() {
			color = img.color;
		}

		protected override void UpdateAnim() {
			color = img.color; //In case color gets changed elsewhere
			color.a = Val.Lerp(startAlpha, endAlpha, easingDelegate(x: Mathf.Min(1.0f, animTime / animDuration)));
			img.color = color;
		}
	}
}