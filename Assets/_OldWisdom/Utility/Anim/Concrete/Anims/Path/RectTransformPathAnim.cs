using IWP.Math;
using UnityEngine;

namespace IWP.Anim {
	internal sealed class RectTransformPathAnim: AbstractPathAnim {
		#region Fields

		[HideInInspector, SerializeField]
		internal RectTransform myRectTransform;

		[HideInInspector, SerializeField]
		internal RectTransform[] wayptRectTransforms;

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal RectTransformPathAnim(): base() {
			myRectTransform = null;

			wayptRectTransforms = System.Array.Empty<RectTransform>();
		}

		static RectTransformPathAnim() {
		}

		#endregion

		#region Unity User Callback Event Funcs
		#endregion

		protected override void InitVals() {
			myRectTransform.localPosition = wayptRectTransforms[currWayptIndex].localPosition;
		}

		protected override void UpdateAnim() {
			currSpd = Val.Lerp(startSpd, endSpd, Mathf.Min(1.0f, animTime / animDuration));

			myRectTransform.localPosition = Vector3.MoveTowards(
				myRectTransform.localPosition,
				wayptRectTransforms[currWayptIndex].localPosition,
				currSpd * Time.deltaTime
			);
		}

		protected override System.Collections.IEnumerator CheckAnim() {
			if(myRectTransform.localPosition == wayptRectTransforms[currWayptIndex].localPosition) {
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

				currWayptIndex = (currWayptIndex + 1u) % (uint)wayptRectTransforms.Length;
			}
		}
	}
}