namespace IWP.Math {
	internal static partial class Easing {
		#region Fields
		#endregion

		#region Properties
		#endregion

		public static float EaseInCirc(float x) {
			return 1.0f - UnityEngine.Mathf.Sqrt(1.0f - x * x);
		}
	}
}