namespace IWP.Math {
	internal static partial class Easing {
		#region Fields

		private static readonly float c1;
		private static readonly float c2;
		private static readonly float c3;
		private static readonly float c4;
		private static readonly float c5;
		private static readonly float d1;
		private static readonly float n1;

		#endregion

		#region Properties

		internal static System.Type Type {
			get;
			private set;
		}

		#endregion

		#region Ctors and Dtor

		static Easing() {
			c1 = 1.70158f;
			c2 = c1 * 1.525f;
			c3 = c1 + 1.0f;
			c4 = 2.0f * UnityEngine.Mathf.PI / 3.0f;
			c5 = 2.0f * UnityEngine.Mathf.PI / 4.5f;
			d1 = 2.75f;
			n1 = 7.5625f;

			Type = typeof(Easing);
		}

		#endregion
	}
}