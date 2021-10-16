using IWP.Math;
using UnityEngine;

namespace IWP.Anim {
    internal sealed class RectTransformScaleAnim: AbstractScaleAnim {
		#region Fields

		[HideInInspector, SerializeField]
		internal RectTransform myRectTransform;

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal RectTransformScaleAnim(): base() {
			myRectTransform = null;
		}

        static RectTransformScaleAnim() {
        }

		#endregion

		#region Unity User Callback Event Funcs
		#endregion

		protected override void InitCore() {
			myScale = myRectTransform.localScale;
		}

		protected override void UpdateAnim() {
			lerpFactor = easingDelegate(x: Mathf.Min(1.0f, animTime / animDuration));

			if(shldAnimateX) {
				myScale.x = Val.Lerp(startScale.x, endScale.x, lerpFactor);
			}
			if(shldAnimateY) {
				myScale.y = Val.Lerp(startScale.y, endScale.y, lerpFactor);
			}
			if(shldAnimateZ) {
				myScale.z = Val.Lerp(startScale.z, endScale.z, lerpFactor);
			}

			myRectTransform.localScale = myScale;
		}
	}
}