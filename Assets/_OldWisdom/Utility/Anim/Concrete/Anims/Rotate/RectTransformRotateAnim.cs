using IWP.Math;
using UnityEngine;

namespace IWP.Anim {
    internal sealed class RectTransformRotateAnim: AbstractRotateAnim {
		#region Fields

		[HideInInspector, SerializeField]
		internal RectTransform myRectTransform;

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal RectTransformRotateAnim(): base() {
			myRectTransform = null;
		}

        static RectTransformRotateAnim() {
        }

		#endregion

		#region Unity User Callback Event Funcs
		#endregion

		protected override void InitCore() {
			myEulerAngles = myRectTransform.localRotation.eulerAngles;
		}

		protected override void UpdateAnim() {
			lerpFactor = easingDelegate(x: Mathf.Min(1.0f, animTime / animDuration));

			if(shldAnimateX) {
				myEulerAngles.x = Val.Lerp(startEulerAngles.x, endEulerAngles.x, lerpFactor);
			}
			if(shldAnimateY) {
				myEulerAngles.y = Val.Lerp(startEulerAngles.y, endEulerAngles.y, lerpFactor);
			}
			if(shldAnimateZ) {
				myEulerAngles.z = Val.Lerp(startEulerAngles.z, endEulerAngles.z, lerpFactor);
			}

			myRectTransform.localRotation = Quaternion.Euler(myEulerAngles);
		}
	}
}