using IWP.Math;
using UnityEngine;

namespace IWP.Anim {
    internal sealed class TransformRotateAnim: AbstractRotateAnim {
		#region Fields

		[HideInInspector, SerializeField]
		internal Transform myTransform;

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal TransformRotateAnim(): base() {
			myTransform = null;
		}

        static TransformRotateAnim() {
        }

		#endregion

		#region Unity User Callback Event Funcs
		#endregion

		protected override void InitCore() {
			myEulerAngles = myTransform.localRotation.eulerAngles;
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

			myTransform.localRotation = Quaternion.Euler(myEulerAngles);
		}
	}
}