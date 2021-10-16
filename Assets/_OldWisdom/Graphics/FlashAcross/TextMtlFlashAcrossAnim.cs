using IWP.Math;
using TMPro;
using UnityEngine;
using static IWP.Anim.FlashAcrossDirs;

namespace IWP.Anim {
    internal sealed class TextMtlFlashAcrossAnim: AbstractAnim {
		#region Fields

		private float startCoord;
		private float endCoord;

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

		[HideInInspector, Range(0.0f, 1.0f), SerializeField]
		internal float thickness;

		[ColorUsage(false, true), HideInInspector, SerializeField]
		internal Color color;

		[HideInInspector, SerializeField]
		internal FlashAcrossDir dir;

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

		internal TextMtlFlashAcrossAnim(): base() {
			startCoord = 0.0f;
			endCoord = 0.0f;

			isSet = false;
			charCount = -1;
			mtlIndex = -1;
			vertex0 = Vector3.zero;
			vertex1 = Vector3.zero;
			vertices = System.Array.Empty<Vector3>();

			shldResetToOG = true;

			mtl = null;

			thickness = 0.0f;
			color = Color.white;
			dir = FlashAcrossDir.Amt;

			tmpTextComponent = null;

			myTransform = null;

			extraXOffsetFromOrigin = 0.0f;
			extraYOffsetFromOrigin = 0.0f;
		}

        static TextMtlFlashAcrossAnim() {
        }

		#endregion

		#region Unity User Callback Event Funcs

		protected override void OnDisable() {
			base.OnDisable();

			if(shldResetToOG && mtl != null) {
				mtl.SetFloat("coord", startCoord);
			}
		}

		#endregion

		protected override void MyOnValidate() {
			UnityEngine.Assertions.Assert.AreNotEqual(
				dir, FlashAcrossDir.Amt,
				"dir, FlashAcrossDir.Amt"
			);
		}

		protected override void InitCore() {
			startCoord = ((int)dir & 1) == 1 ? 1.0f : -thickness;
			endCoord = ((int)dir & 1) == 1 ? -thickness : 1.0f;

			mtl.SetFloat("thickness", thickness);
			mtl.SetColor("color", color);

			mtl.SetInt("dir", (int)dir);

			tmpTextComponent.ForceMeshUpdate();
			charCount = tmpTextComponent.textInfo.characterCount;
		}

		protected override void InitVals() {
			if(Mathf.Approximately(extraYOffsetFromOrigin, 0.0f)) {
				base.InitVals();
			} else {
				_ = StartCoroutine(nameof(DelayedInitVals));
			}
		}

		private System.Collections.IEnumerator DelayedInitVals() {
			yield return new WaitForEndOfFrame();

			base.InitVals();
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
						if((int)dir < 2) {
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

			mtl.SetFloat("len", (int)dir < 2
				? (vertex1.x - vertex0.x) * myTransform.localScale.x
				: (vertex1.y - vertex0.y) * myTransform.localScale.y
			);

			mtl.SetFloat("offset", (int)dir < 2
				? myTransform.localPosition.x + extraXOffsetFromOrigin
				: myTransform.localPosition.y + extraYOffsetFromOrigin
			);

			mtl.SetFloat("coord", Val.Lerp(
				startCoord,
				endCoord,
				easingDelegate(x: Mathf.Min(1.0f, animTime / animDuration))
			));
		}
	}
}