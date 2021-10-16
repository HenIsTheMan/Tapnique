using UnityEngine;

namespace IWP.Anim {
	internal abstract class AbstractTranslateAnim: AbstractAnim {
		#region Fields

		protected float lerpFactor;
		protected Vector3 myPos;

		[HideInInspector]
		public bool shldAnimateX;

		[HideInInspector]
		public bool shldAnimateY;

		[HideInInspector]
		public bool shldAnimateZ;

		[HideInInspector, SerializeField]
		internal Vector3 startPos;

		[HideInInspector, SerializeField]
		internal Vector3 endPos;

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal AbstractTranslateAnim(): base() {
			lerpFactor = 0.0f;
			myPos = Vector3.zero;

			shldAnimateX = true;
			shldAnimateY = true;
			shldAnimateZ = true;

			startPos = Vector3.zero;
			endPos = Vector3.zero;
		}

        static AbstractTranslateAnim() {
        }

        #endregion

        #region Unity User Callback Event Funcs
        #endregion
    }
}