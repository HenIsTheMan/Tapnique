using IWP.Math;
using UnityEngine;

namespace IWP.Anim {
	internal sealed class TransformShakeAnim: AbstractShakeAnim {
		#region Fields

		[HideInInspector, SerializeField]
		internal Transform myTransform;

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal TransformShakeAnim(): base() {
			myTransform = null;
		}

		static TransformShakeAnim() {
		}

		#endregion

		#region Unity User Callback Event Funcs
		#endregion

		protected override void InitCore() {
			myPos = myTransform.localPosition;
		}

		protected override void UpdateAnim() {
			val = Val.Lerp(0.0f, maxMoveCount, Mathf.Min(1.0f, animTime / animDuration));

			if((int)val != prevVal) {
				if(shldAnimateX) {
					pos1.x = startPos.x + Random.Range(minOffset.x, maxOffset.x);
				}
				if(shldAnimateY) {
					pos1.y = startPos.y + Random.Range(minOffset.y, maxOffset.y);
				}
				if(shldAnimateZ) {
					pos1.z = startPos.z + Random.Range(minOffset.z, maxOffset.z);
				}

				pos0 = myTransform.localPosition;

				prevVal = (int)val;
			}

			lerpFactor = easingDelegate(x: val - Mathf.Floor(val));

			if(val >= maxMoveCount - 1) {
				if(shldAnimateX) {
					myPos.x = Val.Lerp(pos0.x, startPos.x, lerpFactor);
				}
				if(shldAnimateY) {
					myPos.y = Val.Lerp(pos0.y, startPos.y, lerpFactor);
				}
				if(shldAnimateZ) {
					myPos.z = Val.Lerp(pos0.z, startPos.z, lerpFactor);
				}
			} else {
				if(shldAnimateX) {
					myPos.x = Val.Lerp(pos0.x, pos1.x, lerpFactor);
				}
				if(shldAnimateY) {
					myPos.y = Val.Lerp(pos0.y, pos1.y, lerpFactor);
				}
				if(shldAnimateZ) {
					myPos.z = Val.Lerp(pos0.z, pos1.z, lerpFactor);
				}
			}

			myTransform.localPosition = myPos;
		}
	}
}