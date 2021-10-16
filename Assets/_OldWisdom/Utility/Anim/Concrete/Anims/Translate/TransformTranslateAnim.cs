using IWP.Math;
using UnityEngine;

namespace IWP.Anim {
    internal sealed class TransformTranslateAnim: AbstractTranslateAnim {
		#region Fields

		[HideInInspector, SerializeField]
		internal Transform myTransform;

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal TransformTranslateAnim(): base() {
			myTransform = null;
		}

        static TransformTranslateAnim() {
        }

		#endregion

		#region Unity User Callback Event Funcs
		#endregion

		protected override void InitCore() {
			myPos = myTransform.position;
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

			myTransform.position = myPos;
		}
	}
}