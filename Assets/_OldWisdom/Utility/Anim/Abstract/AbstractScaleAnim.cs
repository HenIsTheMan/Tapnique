using UnityEngine;

namespace IWP.Anim {
    internal abstract class AbstractScaleAnim: AbstractAnim {
		#region Fields

		protected float lerpFactor;
		protected Vector3 myScale;

		[HideInInspector]
		public bool shldAnimateX;

		[HideInInspector]
		public bool shldAnimateY;

		[HideInInspector]
		public bool shldAnimateZ;

		[HideInInspector, SerializeField]
		internal Vector3 startScale;

		[HideInInspector, SerializeField]
		internal Vector3 endScale;

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal AbstractScaleAnim(): base() {
			lerpFactor = 0.0f;
			myScale = Vector3.one;

			shldAnimateX = true;
			shldAnimateY = true;
			shldAnimateZ = true;

			startScale = Vector3.one;
			endScale = Vector3.one;
		}

        static AbstractScaleAnim() {
        }

        #endregion

        #region Unity User Callback Event Funcs
        #endregion
    }
}