namespace IWP.Math {
	internal static partial class Easing {
		#region Fields
		#endregion

		#region Properties
		#endregion

		public static float EaseInOutElastic(float x) {
			return UnityEngine.Mathf.Approximately(x, 0.0f)
				? 0.0f
				: (UnityEngine.Mathf.Approximately(x, 1.0f)
				? 1.0f
				: (x < 0.5f
				? -(UnityEngine.Mathf.Pow(2.0f, 20.0f * x - 10.0f) * Trigo.Sin((20.0f * x - 11.125f) * c5)) * 0.5f
				: UnityEngine.Mathf.Pow(2.0f, -20.0f * x + 10.0f) * Trigo.Sin((20.0f * x - 11.125f) * c5) * 0.5f + 1));
		}
	}
}