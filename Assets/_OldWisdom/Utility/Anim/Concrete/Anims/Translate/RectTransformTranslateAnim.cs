using IWP.Math;
using UnityEngine;

namespace IWP.Anim {
    internal sealed class RectTransformTranslateAnim: AbstractTranslateAnim {
		#region Fields

		[HideInInspector, SerializeField]
		internal RectTransform myRectTransform;

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal RectTransformTranslateAnim(): base() {
			myRectTransform = null;
		}

        static RectTransformTranslateAnim() {
        }

		#endregion

		#region Unity User Callback Event Funcs
		#endregion

		protected override void InitCore() {
			myPos = myRectTransform.anchoredPosition;
		}

		protected override void UpdateAnim() {
			lerpFactor = easingDelegate(x: Mathf.Min(1.0f, animTime / animDuration));

			if(shldAnimateX) {
				myPos.x = Val.Lerp(startPos.x, endPos.x, lerpFactor);
			}
			if(shldAnimateY) {
				myPos.y = Val.Lerp(startPos.y, endPos.y, lerpFactor);
			}
			if(shldAnimateZ) {
				myPos.z = Val.Lerp(startPos.z, endPos.z, lerpFactor);
			}

			myRectTransform.anchoredPosition = myPos;
		}
	}
}