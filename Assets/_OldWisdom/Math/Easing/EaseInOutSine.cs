namespace IWP.Math {
	internal static partial class Easing {
		#region Fields
		#endregion

		#region Properties
		#endregion

		public static float EaseInOutSine(float x) {
			return -(Trigo.Cos(UnityEngine.Mathf.PI * x) - 1.0f) * 0.5f;
		}
	}
}