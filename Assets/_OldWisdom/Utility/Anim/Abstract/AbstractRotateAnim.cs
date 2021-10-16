using UnityEngine;

namespace IWP.Anim {
    internal abstract class AbstractRotateAnim: AbstractAnim {
		#region Fields

		protected float lerpFactor;
		protected Vector3 myEulerAngles;

		[HideInInspector]
		public bool shldAnimateX;

		[HideInInspector]
		public bool shldAnimateY;

		[HideInInspector]
		public bool shldAnimateZ;

		[HideInInspector, SerializeField]
		internal Vector3 startEulerAngles;

		[HideInInspector, SerializeField]
		internal Vector3 endEulerAngles;

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal AbstractRotateAnim(): base() {
			lerpFactor = 0.0f;
			myEulerAngles = Vector3.zero;

			shldAnimateX = true;
			shldAnimateY = true;
			shldAnimateZ = true;

			startEulerAngles = Vector3.zero;
			endEulerAngles = Vector3.zero;
		}

        static AbstractRotateAnim() {
        }

        #endregion

        #region Unity User Callback Event Funcs
        #endregion
    }
}