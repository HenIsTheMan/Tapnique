using IWP.Math;
using System.Collections.Generic;
using UnityEngine;
using static IWP.Anim.BarOrientations;

namespace IWP.Anim {
    internal sealed class MtlBarsFadeAnim: AbstractFadeAnim {
		#region Fields

		private float part;
		private ValSet[] valSets;
		private List<float> decreasingVals;
		private List<float> increasingVals;

		[HideInInspector, SerializeField]
		internal bool shldResetToOG;

		[HideInInspector, SerializeField]
		internal Material mtl;

		[HideInInspector, SerializeField]
		internal int barCount;

		[HideInInspector, SerializeField]
		internal BarOrientation barOrientation;

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal MtlBarsFadeAnim(): base() {
			part = 0.0f;
			valSets = System.Array.Empty<ValSet>();
			decreasingVals = null;
			increasingVals = null;

			shldResetToOG = true;

			mtl = null;

			barCount = 0;
			barOrientation = BarOrientation.Amt;
		}

        static MtlBarsFadeAnim() {
        }

		#endregion

		#region Unity User Callback Event Funcs

		protected override void OnDisable() {
			base.OnDisable();

			if(shldResetToOG && mtl != null && decreasingVals != null && increasingVals != null) {
				for(int i = 0; i < barCount; ++i) {
					decreasingVals[i] = -1.0f;
					increasingVals[i] = -1.0f;
				}

				mtl.SetFloatArray("decreasingVals", decreasingVals);
				mtl.SetFloatArray("increasingVals", increasingVals);
			}
		}

		#endregion

		protected override void MyOnValidate() {
			UnityEngine.Assertions.Assert.AreNotEqual(
				barOrientation, BarOrientation.Amt,
				"barOrientation, BarOrientation.Amt"
			);

			if(mtl != null) {
				mtl.SetFloat("startAlpha", startAlpha);
			}
		}

		protected override void InitCore() {
			part = 1.0f / barCount;
			mtl.SetFloat("part", part);

			mtl.SetFloat("startAlpha", startAlpha);
			mtl.SetFloat("endAlpha", endAlpha);
			mtl.SetInt("barOrientation", (int)barOrientation);

			valSets = new ValSet[barCount];
			decreasingVals = new List<float>(barCount);
			increasingVals = new List<float>(barCount);

			for(int i = 0; i < barCount; ++i) {
				valSets[i].startVal = Random.Range(part * i, part * (i + 1));

				valSets[i].minVal = part * i;
				valSets[i].maxVal = part * (i + 1);

				decreasingVals.Add(valSets[i].startVal);
				increasingVals.Add(valSets[i].startVal);
			}

			mtl.SetFloatArray("decreasingVals", decreasingVals);
			mtl.SetFloatArray("increasingVals", increasingVals);
		}

		protected override void UpdateAnim() {
			for(int i = 0; i < barCount; ++i) {
				decreasingVals[i] = Val.Lerp(
					valSets[i].startVal,
					valSets[i].minVal,
					easingDelegate(x: Mathf.Min(1.0f, animTime / animDuration))
				);
				increasingVals[i] = Val.Lerp(
					valSets[i].startVal,
					valSets[i].maxVal,
					easingDelegate(x: Mathf.Min(1.0f, animTime / animDuration))
				);
			}

			mtl.SetFloatArray("decreasingVals", decreasingVals);
			mtl.SetFloatArray("increasingVals", increasingVals);
		}
	}
}