using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace Genesis.Wisdom {
    internal sealed class LocalScaleAnim: AbstractAnim {
		private Vector3 scale;

		[SerializeField]
		private Transform myTransform;

		[SerializeField]
		private ScaleAnimSupport scaleAnimSupport;

		#if UNITY_EDITOR

		protected override void OnValidate() {
			scaleAnimSupport ??= new ScaleAnimSupport();

			base.OnValidate();

			if(!EditorApplication.isPlayingOrWillChangePlaymode) {
				scaleAnimSupport.MyOnValidate(); //Can be below base.OnValidate due to EditorApplication.delayCall
			}
		}

		#endif

		protected override void Awake() {
			scaleAnimSupport.MyAwake();

			base.Awake();

			scale = myTransform.localScale;
		}

		protected override void UpdateAnim(float myLerpFactor) {
			scaleAnimSupport.calcComponentValDelegate(ref scale, myLerpFactor);
			myTransform.localScale = scale;
		}
	}
}