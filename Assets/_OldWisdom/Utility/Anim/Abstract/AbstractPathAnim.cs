using UnityEngine;

namespace IWP.Anim {
	internal abstract class AbstractPathAnim: AbstractAnim {
		#region Fields

		protected float currSpd;

		[HideInInspector, SerializeField]
		internal float startSpd;

		[HideInInspector, SerializeField]
		internal float endSpd;

		[HideInInspector, SerializeField]
		internal uint currWayptIndex;

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal AbstractPathAnim(): base() {
			currSpd = 0.0f;
			startSpd = 0.0f;
			endSpd = 0.0f;

			currWayptIndex = 0u;
		}

        static AbstractPathAnim() {
        }

        #endregion

        #region Unity User Callback Event Funcs
        #endregion
    }
}