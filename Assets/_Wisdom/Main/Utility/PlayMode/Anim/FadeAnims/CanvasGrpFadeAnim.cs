using UnityEngine;

namespace Genesis.Wisdom {
    internal sealed class CanvasGrpFadeAnim: AbstractAnim {
		[SerializeField]
		internal CanvasGroup canvasGrp;

		[SerializeField]
		internal FadeAnimSupport fadeAnimSupport;

		protected override void UpdateAnim(float myLerpFactor) {
			canvasGrp.alpha = Mathf.LerpUnclamped(
				fadeAnimSupport.startAlpha,
				fadeAnimSupport.endAlpha,
				myLerpFactor
			);
		}
	}
}