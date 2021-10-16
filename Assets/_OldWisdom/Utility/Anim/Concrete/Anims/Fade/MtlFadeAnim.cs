using IWP.Math;
using UnityEngine;

namespace IWP.Anim {
    internal sealed class MtlFadeAnim: AbstractFadeAnim {
		#region Fields

		[HideInInspector, SerializeField]
		internal bool shldResetToOG;

		private Color color;
		private Color colorOG;

		[HideInInspector, SerializeField]
		internal Material mtl;

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal MtlFadeAnim(): base() {
			shldResetToOG = true;

			color = Color.white;
			colorOG = Color.white;

			mtl = null;
		}

        static MtlFadeAnim() {
        }

		#endregion

		#region Unity User Callback Event Funcs

		protected override void OnDisable() {
			base.OnDisable();

			if(shldResetToOG && mtl != null) {
				mtl.color = colorOG;
			}
		}

		#endregion

		protected override void InitCore() {
			colorOG = mtl.color;
		}

		protected override void UpdateAnim() {
			color = mtl.color; //In case color gets changed elsewhere
			color.a = Val.Lerp(startAlpha, endAlpha, easingDelegate(x: Mathf.Min(1.0f, animTime / animDuration)));
			mtl.color = color;
		}
	}
}