using IWP.Math;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static IWP.Anim.BarOrientations;

namespace IWP.Anim {
    internal sealed class TextMtlBarsFadeAnim: AbstractFadeAnim {
		#region Fields

		private float part;
		private ValSet[] valSets;
		private List<float> decreasingVals;
		private List<float> increasingVals;

		private bool isSet;
		private int charCount;
		private int mtlIndex;
		private Vector3 vertex0;
		private Vector3 vertex1;
		private Vector3[] vertices;

		[HideInInspector, SerializeField]
		internal bool shldResetToOG;

		[HideInInspector, SerializeField]
		internal Material mtl;

		[HideInInspector, SerializeField]
		internal int barCount;

		[HideInInspector, SerializeField]
		internal BarOrientation barOrientation;

		[HideInInspector, SerializeField]
		internal TMP_Text tmpTextComponent;

		[HideInInspector, SerializeField]
		internal Transform myTransform;

		[HideInInspector, SerializeField]
		internal float extraXOffsetFromOrigin;

		[HideInInspector, SerializeField]
		internal float extraYOffsetFromOrigin;

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal TextMtlBarsFadeAnim(): base() {
			part = 0.0f;
			valSets = System.Array.Empty<ValSet>();
			decreasingVals = null;
			increasingVals = null;

			isSet = false;
			charCount = -1;
			mtlIndex = -1;
			vertex0 = Vector3.zero;
			vertex1 = Vector3.zero;
			vertices = System.Array.Empty<Vector3>();

			shldResetToOG = true;

			mtl = null;

			barCount = 0;
			barOrientation = BarOrientation.Amt;

			tmpTextComponent = null;

			myTransform = null;

			extraXOffsetFromOrigin = 0.0f;
			extraYOffsetFromOrigin = 0.0f;
		}

        static TextMtlBarsFadeAnim() {
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

			tmpTextComponent.ForceMeshUpdate();
			charCount = tmpTextComponent.textInfo.characterCount;
		}

		protected override void UpdateAnim() {
			for(int i = 0; i < charCount; ++i) {
				mtlIndex = tmpTextComponent.textInfo.characterInfo[i].materialReferenceIndex;
				vertices = tmpTextComponent.textInfo.meshInfo[mtlIndex].vertices;

				foreach(Vector3 vertex in vertices) {
					if(!isSet) {
						vertex0 = vertex;
						vertex1 = vertex;
						isSet = true;
					} else {
						if(barOrientation == BarOrientation.Vert) {
							if(vertex.x < vertex0.x) {
								vertex0 = vertex;
							}
							if(vertex.x > vertex1.x) {
								vertex1 = vertex;
							}
						} else {
							if(vertex.y < vertex0.y) {
								vertex0 = vertex;
							}
							if(vertex.y > vertex1.y) {
								vertex1 = vertex;
							}
						}
					}
				}
			}

			mtl.SetFloat("len", barOrientation == BarOrientation.Vert
				? (vertex1.x - vertex0.x) * myTransform.localScale.x
				: (vertex1.y - vertex0.y) * myTransform.localScale.y
			);

			mtl.SetFloat("offset", barOrientation == BarOrientation.Vert
				? myTransform.localPosition.x + extraXOffsetFromOrigin
				: myTransform.localPosition.y + extraYOffsetFromOrigin
			);

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