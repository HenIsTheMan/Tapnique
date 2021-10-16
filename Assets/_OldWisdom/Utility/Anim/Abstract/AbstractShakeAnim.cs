using UnityEngine;

namespace IWP.Anim {
    internal abstract class AbstractShakeAnim: AbstractAnim {
		#region Fields

		protected float val;
		protected int prevVal;

		protected float lerpFactor;
		protected Vector3 myPos;

		protected Vector3 pos0;
		protected Vector3 pos1;

		[HideInInspector]
		public bool shldAnimateX;

		[HideInInspector]
		public bool shldAnimateY;

		[HideInInspector]
		public bool shldAnimateZ;

		[HideInInspector, SerializeField]
		internal Vector3 startPos;

		[HideInInspector, SerializeField]
		internal Vector3 minOffset;

		[HideInInspector, SerializeField]
		internal Vector3 maxOffset;

		[HideInInspector, Min(2), SerializeField]
		internal int maxMoveCount;

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal AbstractShakeAnim(): base() {
			val = 0.0f;
			prevVal = -1;

			lerpFactor = 0.0f;
			myPos = Vector3.zero;

			pos0 = Vector3.zero;
			pos1 = Vector3.zero;

			shldAnimateX = true;
			shldAnimateY = true;
			shldAnimateZ = true;

			startPos = Vector3.zero;
			minOffset = Vector3.zero;
			maxOffset = Vector3.zero;

			maxMoveCount = 2;
		}

        static AbstractShakeAnim() {
        }

		#endregion

		#region Unity User Callback Event Funcs
		#endregion

		protected override void ResetMe() {
			base.ResetMe();

			prevVal = 0;
		}
    }
}