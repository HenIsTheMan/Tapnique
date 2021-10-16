using IWP.Math;
using TMPro;
using UnityEngine;

namespace IWP.Anim {
    internal sealed class TextCharScaleAnim: AbstractScaleAnim {
		#region Fields

		[HideInInspector, SerializeField]
		internal TMP_Text tmpTextComponent;

		private float val;
		private int mtlIndex;
		private int vertexIndex;
		private Vector3 offsetToMidBaseline;

		private uint actualCharIndex;
		private uint proxyCharIndex;
		private int prevActualCharIndex;
		private uint indexOffset;
		private uint charCount;
		private int visibleCharCount;

		private TMP_TextInfo textInfo;
		private TMP_CharacterInfo charInfoAtIndex;
		private TMP_CharacterInfo[] charInfo;

		private Matrix4x4 mat;
		private TMP_MeshInfo[] meshInfoVertexData;
		private Vector3[] srcVertices;
		private Vector3[] dstVertices;

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal TextCharScaleAnim(): base() {
			tmpTextComponent = null;

			val = 0.0f;
			mtlIndex = 0;
			vertexIndex = 0;
			offsetToMidBaseline = Vector3.zero;

			actualCharIndex = 0u;
			proxyCharIndex = 0u;
			prevActualCharIndex = -1;
			indexOffset = 0u;
			charCount = 0u;
			visibleCharCount = 0;
			textInfo = null;
			//charInfoAtIndex;
			charInfo = System.Array.Empty<TMP_CharacterInfo>();

			mat = Matrix4x4.identity;
			meshInfoVertexData = System.Array.Empty<TMP_MeshInfo>();
			srcVertices = System.Array.Empty<Vector3>();
			dstVertices = System.Array.Empty<Vector3>();
		}

        static TextCharScaleAnim() {
        }

		#endregion

		#region Unity User Callback Event Funcs

		protected override void OnEnable() {
			base.OnEnable();

			animPostPeriodicDelegate += ResetStuff;
		}

		protected override void OnDisable() {
			base.OnDisable();

			animPostPeriodicDelegate -= ResetStuff;
		}

		#endregion

		private void ResetStuff() {
			indexOffset = 0u;
			val = 0.0f;

			for(uint i = 0; i < charCount; ++i) {
				if(charInfo[i].isVisible) {
					SubUpdateAnim(i);
				}
			}
		}

		private void SubUpdateAnim(uint actualCharIndexParam) {
			if((int)actualCharIndexParam != prevActualCharIndex) {
				charInfoAtIndex = charInfo[actualCharIndexParam];

				mtlIndex = charInfoAtIndex.materialReferenceIndex;
				vertexIndex = charInfoAtIndex.vertexIndex;

				srcVertices = meshInfoVertexData[mtlIndex].vertices;
				dstVertices = textInfo.meshInfo[mtlIndex].vertices;
				offsetToMidBaseline.x = (srcVertices[vertexIndex + 0].x + srcVertices[vertexIndex + 2].x) * 0.5f;
				offsetToMidBaseline.y = charInfoAtIndex.baseLine;
			}

			dstVertices[vertexIndex + 0] = srcVertices[vertexIndex + 0] - offsetToMidBaseline;
			dstVertices[vertexIndex + 1] = srcVertices[vertexIndex + 1] - offsetToMidBaseline;
			dstVertices[vertexIndex + 2] = srcVertices[vertexIndex + 2] - offsetToMidBaseline;
			dstVertices[vertexIndex + 3] = srcVertices[vertexIndex + 3] - offsetToMidBaseline;

			lerpFactor = Mathf.Approximately(val, visibleCharCount) ? 1.0f : easingDelegate(val - Mathf.Floor(val));

			if(shldAnimateX) {
				myScale.x = Val.Lerp(startScale.x, endScale.x, lerpFactor);
			}
			if(shldAnimateY) {
				myScale.y = Val.Lerp(startScale.y, endScale.y, lerpFactor);
			}
			if(shldAnimateZ) {
				myScale.z = Val.Lerp(startScale.z, endScale.z, lerpFactor);
			}

			mat = Matrix4x4.TRS(
				Vector3.zero,
				Quaternion.identity,
				myScale
			);

			dstVertices[vertexIndex + 0] = mat.MultiplyPoint3x4(dstVertices[vertexIndex + 0]) + offsetToMidBaseline;
			dstVertices[vertexIndex + 1] = mat.MultiplyPoint3x4(dstVertices[vertexIndex + 1]) + offsetToMidBaseline;
			dstVertices[vertexIndex + 2] = mat.MultiplyPoint3x4(dstVertices[vertexIndex + 2]) + offsetToMidBaseline;
			dstVertices[vertexIndex + 3] = mat.MultiplyPoint3x4(dstVertices[vertexIndex + 3]) + offsetToMidBaseline;

			textInfo.meshInfo[mtlIndex].mesh.vertices = textInfo.meshInfo[mtlIndex].vertices;
			tmpTextComponent.UpdateGeometry(textInfo.meshInfo[mtlIndex].mesh, mtlIndex);

			prevActualCharIndex = (int)actualCharIndexParam;
		}

		protected override void InitCore() {
			if(!tmpTextComponent.enabled) {
				return;
			}

			tmpTextComponent.ForceMeshUpdate();

			textInfo = tmpTextComponent.textInfo;
			charInfo = textInfo.characterInfo;

			meshInfoVertexData = textInfo.CopyMeshInfoVertexData();

			charCount = (uint)textInfo.characterCount;

			for(uint i = 0; i < charCount; ++i) {
				if(charInfo[i].character != (char)KeyCode.Space) { //If char is not a space... 
					_ = ++visibleCharCount;
				}
			}
		}

		protected override void InitVals() {
			for(uint i = 0; i < charCount; ++i) {
				if(charInfo[i].character != (char)KeyCode.Space) { //If char is not a space... 
					SubUpdateAnim(i);
				}
			}
		}

		protected override void UpdateAnim() {
			val = Val.Lerp(0.0f, visibleCharCount, Mathf.Min(1.0f, animTime / animDuration));
			proxyCharIndex = (uint)Mathf.Max(0.0f, Mathf.Ceil(val) - 1.0f);

			actualCharIndex = proxyCharIndex + indexOffset;

			while(charInfo[actualCharIndex].character == (char)KeyCode.Space) { //While char is a space...
				_ = ++actualCharIndex;
				_ = ++indexOffset;
			}

			SubUpdateAnim(actualCharIndex);
		}
	}
}