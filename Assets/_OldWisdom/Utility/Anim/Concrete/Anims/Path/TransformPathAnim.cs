using IWP.Math;
using UnityEngine;

namespace IWP.Anim {
	internal sealed class TransformPathAnim: AbstractPathAnim {
		#region Fields

		[HideInInspector, SerializeField]
		internal Transform myTransform;

		[HideInInspector, SerializeField]
		internal Transform[] wayptTransforms;

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal TransformPathAnim(): base() {
			myTransform = null;

			wayptTransforms = System.Array.Empty<Transform>();
		}

		static TransformPathAnim() {
		}

		#endregion

		#region Unity User Callback Event Funcs
		#endregion

		protected override void InitVals() {
			myTransform.localPosition = wayptTransforms[currWayptIndex].localPosition;
		}

		protected override void UpdateAnim() {
			currSpd = Val.Lerp(startSpd, endSpd, Mathf.Min(1.0f, animTime / animDuration));

			myTransform.localPosition = Vector3.MoveTowards(
				myTransform.localPosition,
				wayptTransforms[currWayptIndex].localPosition,
				currSpd * Time.deltaTime
			);
		}

		protected override System.Collections.IEnumerator CheckAnim() {
			if(myTransform.localPosition == wayptTransforms[currWayptIndex].localPosition) {
				if(Mathf.Approximately(currSpd, endSpd)) {
					if(countThreshold != 0 && ++count == countThreshold) {
						IsUpdating = false;
						yield break;
					}

					animTime = 0.0f;

					animPrePeriodicDelegate?.Invoke();

					if(periodicWaitForSeconds != null) {
						yield return periodicWaitForSeconds;
					}

					animPostPeriodicDelegate?.Invoke();
				}

				currWayptIndex = (currWayptIndex + 1u) % (uint)wayptTransforms.Length;
			}
		}
	}
}