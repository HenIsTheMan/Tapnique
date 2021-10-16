using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace Genesis.Wisdom {
    internal sealed class LocalTranslateAnim: AbstractAnim {
		private Vector3 pos;

		[SerializeField]
		private Transform myTransform;

		[SerializeField]
		private TranslateAnimSupport translateAnimSupport;

		#if UNITY_EDITOR

		protected override void OnValidate() {
			translateAnimSupport ??= new TranslateAnimSupport();

			base.OnValidate();

			if(!EditorApplication.isPlayingOrWillChangePlaymode) {
				translateAnimSupport.MyOnValidate(); //Can be below base.OnValidate due to EditorApplication.delayCall
			}
		}

		#endif

		protected override void Awake() {
			translateAnimSupport.MyAwake();

			base.Awake();

			pos = myTransform.localPosition;
		}

		protected override void UpdateAnim(float myLerpFactor) {
			translateAnimSupport.calcComponentValDelegate(ref pos, myLerpFactor);
			myTransform.localPosition = pos;
		}
	}
}