namespace IWP.Math {
	internal static partial class Easing {
		#region Fields
		#endregion

		#region Properties
		#endregion

		public static float EaseOutElastic(float x) {
			return UnityEngine.Mathf.Approximately(x, 0.0f)
				? 0.0f
				: (UnityEngine.Mathf.Approximately(x, 1.0f)
				? 1.0f
				: UnityEngine.Mathf.Pow(2.0f, -10.0f * x) * Trigo.Sin((x * 10.0f - 0.75f) * c4)) + 1.0f;
		}
	}
}