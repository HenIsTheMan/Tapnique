using IWP.Math;
using UnityEngine;
using static IWP.Anim.FlashAcrossDirs;

namespace IWP.Anim {
    internal sealed class MtlFlashAcrossAnim: AbstractAnim {
		#region Fields

		private float startCoord;
		private float endCoord;

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

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal MtlFlashAcrossAnim(): base() {
			startCoord = 0.0f;
			endCoord = 0.0f;

			shldResetToOG = true;

			mtl = null;

			thickness = 0.0f;
			color = Color.white;
			dir = FlashAcrossDir.Amt;
		}

        static MtlFlashAcrossAnim() {
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
		}

		protected override void UpdateAnim() {
			mtl.SetFloat("coord", Val.Lerp(
				startCoord,
				endCoord,
				easingDelegate(x: Mathf.Min(1.0f, animTime / animDuration))
			));
		}
	}
}