using IWP.Math;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace IWP.Anim {
    internal sealed class BloomAnim: AbstractAnim {
		#region Fields

		private Bloom bloom;

		private float intensityOG;
		private float scatterOG;

		[HideInInspector, SerializeField]
		internal bool shldResetToOG;

		[HideInInspector, SerializeField]
		internal Volume myVol;

		[HideInInspector, SerializeField]
		internal float startIntensity;

		[HideInInspector, SerializeField]
		internal float endIntensity;

		[HideInInspector, SerializeField]
		internal float startScatter;

		[HideInInspector, SerializeField]
		internal float endScatter;

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal BloomAnim(): base() {
			bloom = null;

			intensityOG = 0.0f;
			scatterOG = 0.0f;

			shldResetToOG = true;

			myVol = null;

			startIntensity = 0.0f;
			endIntensity = 0.0f;
			startScatter = 0.0f;
			endScatter = 0.0f;
		}

        static BloomAnim() {
        }

		#endregion

		#region Unity User Callback Event Funcs

		protected override void OnDisable() {
			base.OnDisable();

			if(shldResetToOG && bloom != null) {
				bloom.intensity.value = intensityOG;
				bloom.scatter.value = scatterOG;
			}
		}

		#endregion

		protected override void InitCore() {
			_ = myVol.sharedProfile.TryGet(out bloom);

			intensityOG = bloom.intensity.value;
			scatterOG = bloom.scatter.value;
		}

		protected override void UpdateAnim() {
			bloom.intensity.value = Val.Lerp(startIntensity, endIntensity, easingDelegate(x: Mathf.Min(1.0f, animTime / animDuration)));
			bloom.scatter.value = Val.Lerp(startScatter, endScatter, easingDelegate(x: Mathf.Min(1.0f, animTime / animDuration)));
		}
	}
}