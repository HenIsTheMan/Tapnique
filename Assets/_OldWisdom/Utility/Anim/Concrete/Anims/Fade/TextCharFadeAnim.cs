using IWP.Math;
using TMPro;
using UnityEngine;

namespace IWP.Anim {
    internal sealed class TextCharFadeAnim: AbstractFadeAnim {
		#region Fields

		[HideInInspector, SerializeField]
		internal TextMeshProUGUI tmpComponent;

		private float val;
		private uint charCount;
		private int validCharCount;

		private byte alpha;
		private Color32[] vertexColors;
		private int meshIndex;
		private int vertexIndex;
		private uint actualCharIndex;
		private uint proxyCharIndex;
		private int prevActualCharIndex;
		private uint indexOffset;
		private TMP_CharacterInfo charInfoAtIndex;

		private TMP_TextInfo textInfo;
		private TMP_CharacterInfo[] charInfo;

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal TextCharFadeAnim(): base() {
			tmpComponent = null;

			val = 0.0f;
			charCount = 0u;
			validCharCount = 0;

			alpha = 0;
			vertexColors = System.Array.Empty<Color32>();
			meshIndex = 0;
			vertexIndex = 0;
			actualCharIndex = 0u;
			proxyCharIndex = 0u;
			prevActualCharIndex = -1;
			indexOffset = 0u;
			//charInfoAtIndex;

			textInfo = null;
			charInfo = System.Array.Empty<TMP_CharacterInfo>();
		}

        static TextCharFadeAnim() {
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
				if(charInfo[i].character != (char)KeyCode.Space) { //If char is not a space...
					SubUpdateAnim(i);
				}
			}
		}

		private void SubUpdateAnim(uint actualCharIndexParam) {
			if((int)actualCharIndexParam != prevActualCharIndex) {
				charInfoAtIndex = charInfo[actualCharIndexParam];

				meshIndex = charInfoAtIndex.materialReferenceIndex;
				vertexIndex = charInfoAtIndex.vertexIndex;

				vertexColors = textInfo.meshInfo[meshIndex].colors32;
			}

			alpha = Mathf.Approximately(val, validCharCount)
				? (byte)(endAlpha * 255.0f)
				: (byte)Mathf.Ceil(Val.Lerp(startAlpha, endAlpha, easingDelegate(x: val - Mathf.Floor(val))) * 255.0f);

			vertexColors[vertexIndex + 0].a = alpha;
			vertexColors[vertexIndex + 1].a = alpha;
			vertexColors[vertexIndex + 2].a = alpha;
			vertexColors[vertexIndex + 3].a = alpha;

			tmpComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

			prevActualCharIndex = (int)actualCharIndexParam;
		}

		protected override void InitCore() {
			tmpComponent.ForceMeshUpdate();

			textInfo = tmpComponent.textInfo;
			charInfo = textInfo.characterInfo;

			charCount = (uint)textInfo.characterCount;

			for(uint i = 0; i < charCount; ++i) {
				if(charInfo[i].character != (char)KeyCode.Space) { //If char is not a space...
					_ = ++validCharCount;
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
			General.Console.Log("here");

			val = Val.Lerp(0.0f, validCharCount, Mathf.Min(1.0f, animTime / animDuration));
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