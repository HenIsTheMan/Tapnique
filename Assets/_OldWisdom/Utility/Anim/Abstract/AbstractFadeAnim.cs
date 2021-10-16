using UnityEngine;

namespace IWP.Anim {
	internal abstract class AbstractFadeAnim: AbstractAnim {
		#region Fields

		[HideInInspector, Range(0.0f, 1.0f), SerializeField]
		internal float startAlpha;

		[HideInInspector, Range(0.0f, 1.0f), SerializeField]
		internal float endAlpha;

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal AbstractFadeAnim(): base() {
			startAlpha = 1.0f;
			endAlpha = 1.0f;
		}

        static AbstractFadeAnim() {
        }

        #endregion

        #region Unity User Callback Event Funcs
        #endregion
    }
}