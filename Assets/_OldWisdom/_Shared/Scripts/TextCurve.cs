using TMPro;
using UnityEngine;
using static IWP.General.InitIDs;

namespace IWP.General {
	internal sealed class TextCurve: MonoBehaviour {
		#region Fields

		[SerializeField]
		private InitControl initControl;
		
		[SerializeField]
		private bool shldInit;

		[SerializeField]
		private bool shldUpdate;

		[SerializeField]
		private TMP_Text tmpTextComponent;

		[SerializeField]
		private AnimationCurve animCurve;

		[SerializeField]
		private float scaleFactor;

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal TextCurve(): base() {
			initControl = null;

			shldInit = true;
			shldUpdate = false;

			tmpTextComponent = null;

			animCurve = null;
			scaleFactor = 1.0f;
		}

		static TextCurve() {
		}

		#endregion

		#region Unity User Callback Event Funcs

		private void OnValidate() {
			UnityEngine.Assertions.Assert.IsTrue(animCurve.length > 0, "animCurve.length > 0");
		}

		private void OnEnable() {
			initControl.AddMethod((uint)InitID.TextCurve, Init);
		}

		private void Update() {
			if(shldUpdate) {
				CurveText();
			}
		}

		private void OnDisable() {
			initControl.RemoveMethod((uint)InitID.TextCurve, Init);
		}

		#endregion

		private void Init() {
			if(shldInit) {
				CurveText();
			}
		}

		internal void CurveText() {
			int vertexIndex;
			int mtlIndex;
			float x0;
			float x1;
			float y0;
			float y1;

			Vector3[] vertices;
			Matrix4x4 mat;
			Vector3 offsetToMidBaseline;
			offsetToMidBaseline.z = 0.0f;

			tmpTextComponent.ForceMeshUpdate();
			TMP_TextInfo textInfo = tmpTextComponent.textInfo;
			int charCount = textInfo.characterCount;
			if(charCount == 0) {
				return;
			}

			float boundsMinX = tmpTextComponent.bounds.min.x;
			float boundsMaxX = tmpTextComponent.bounds.max.x;

			Vector3 horizontal = Vector3.right;
			Vector3 tangent;
			tangent.z = 0.0f;
			float dot;
			Vector3 cross;
			Vector3 translation = Vector3.zero;

			for(int i = 0; i < charCount; ++i) {
				if(!textInfo.characterInfo[i].isVisible) {
					continue;
				}

				vertexIndex = textInfo.characterInfo[i].vertexIndex;
				mtlIndex = textInfo.characterInfo[i].materialReferenceIndex;
				vertices = textInfo.meshInfo[mtlIndex].vertices;

				//* Adjust pivot pt
				offsetToMidBaseline.x = (vertices[vertexIndex + 0].x + vertices[vertexIndex + 2].x) * 0.5f;
				offsetToMidBaseline.y = textInfo.characterInfo[i].baseLine;

				vertices[vertexIndex + 0] -= offsetToMidBaseline;
				vertices[vertexIndex + 1] -= offsetToMidBaseline;
				vertices[vertexIndex + 2] -= offsetToMidBaseline;
				vertices[vertexIndex + 3] -= offsetToMidBaseline;
				//*/

				x0 = (offsetToMidBaseline.x - boundsMinX) / (boundsMaxX - boundsMinX);
				x1 = x0 + 0.0001f;
				y0 = animCurve.Evaluate(x0) * scaleFactor;
				y1 = animCurve.Evaluate(x1) * scaleFactor;

				tangent.x = x1 * (boundsMaxX - boundsMinX) + boundsMinX - offsetToMidBaseline.x;
				tangent.y = y1 - y0;
				dot = Mathf.Acos(Vector3.Dot(horizontal, tangent.normalized)) * 57.2957795f;
				cross = Vector3.Cross(horizontal, tangent);
				translation.y = y0;
				mat = Matrix4x4.TRS(translation, Quaternion.Euler(0.0f, 0.0f, cross.z > 0.0f ? dot : 360.0f - dot), Vector3.one);

				vertices[vertexIndex + 0] = mat.MultiplyPoint3x4(vertices[vertexIndex + 0]) + offsetToMidBaseline;
				vertices[vertexIndex + 1] = mat.MultiplyPoint3x4(vertices[vertexIndex + 1]) + offsetToMidBaseline;
				vertices[vertexIndex + 2] = mat.MultiplyPoint3x4(vertices[vertexIndex + 2]) + offsetToMidBaseline;
				vertices[vertexIndex + 3] = mat.MultiplyPoint3x4(vertices[vertexIndex + 3]) + offsetToMidBaseline;
			}

			tmpTextComponent.UpdateVertexData();
		}
	}
}