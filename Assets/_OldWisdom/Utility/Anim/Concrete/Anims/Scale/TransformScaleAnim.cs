using IWP.Math;
using UnityEngine;

namespace IWP.Anim {
    internal sealed class TransformScaleAnim: AbstractScaleAnim {
		#region Fields

		[HideInInspector, SerializeField]
		internal Transform myTransform;

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal TransformScaleAnim(): base() {
			myTransform = null;
		}

        static TransformScaleAnim() {
        }

		#endregion

		#region Unity User Callback Event Funcs
		#endregion

		protected override void InitCore() {
			myScale = myTransform.localScale;
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

			myTransform.localScale = myScale;
		}
	}
}