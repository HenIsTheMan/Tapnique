using IWP.Math;
using TMPro;
using UnityEngine;

namespace IWP.Anim {
    internal sealed class TextRevealAnim: AbstractAnim {
		#region Fields

		[HideInInspector, SerializeField]
		internal TextMeshProUGUI tmpComponent;

		[HideInInspector, SerializeField]
		internal int offsetFromStart;

		[HideInInspector, SerializeField]
		internal int offsetFromEnd;

		private float start;
		private float end;

        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        internal TextRevealAnim(): base() {
			tmpComponent = null;

			offsetFromStart = 0;
			offsetFromEnd = 0;

			start = 0.0f;
			end = 0.0f;
        }

        static TextRevealAnim() {
        }

		#endregion

		#region Unity User Callback Event Funcs
		#endregion

		protected override void InitCore() {
			tmpComponent.ForceMeshUpdate();

			start += offsetFromStart;
			end = tmpComponent.textInfo.characterCount - offsetFromEnd;

			end = Mathf.Max(0.0f, end);
			start = Mathf.Min(end, start);
		}

		protected override void UpdateAnim() {
			tmpComponent.maxVisibleCharacters = (int)Mathf.Floor(Val.Lerp(start, end, easingDelegate(x: Mathf.Min(1.0f, animTime / animDuration))));
		}
	}
}