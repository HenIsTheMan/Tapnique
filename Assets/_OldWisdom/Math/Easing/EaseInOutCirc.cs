namespace IWP.Math {
	internal static partial class Easing {
		#region Fields
		#endregion

		#region Properties
		#endregion

		public static float EaseInOutCirc(float x) {
			return x < 0.5f
				? (1.0f - UnityEngine.Mathf.Sqrt(1.0f - 2.0f * x * 2.0f * x)) * 0.5f
				: (UnityEngine.Mathf.Sqrt(1.0f - UnityEngine.Mathf.Pow(-2.0f * x + 2.0f, 2.0f)) + 1.0f) * 0.5f;
		}
	}
}