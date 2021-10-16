namespace IWP.Math {
	internal static partial class Easing {
		#region Fields
		#endregion

		#region Properties
		#endregion

		public static float EaseInOutExpo(float x) {
			return UnityEngine.Mathf.Approximately(x, 0.0f)
				? 0.0f
				: (UnityEngine.Mathf.Approximately(x, 1.0f)
				? 1.0f
				: x < 0.5f ? UnityEngine.Mathf.Pow(2.0f, 20.0f * x - 10.0f) * 0.5f
				: (2.0f - UnityEngine.Mathf.Pow(2.0f, -20.0f * x + 10.0f)) * 0.5f);
		}
	}
}