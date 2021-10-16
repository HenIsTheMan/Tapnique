namespace Genesis.Wisdom {
	internal static partial class RateOfChange {
		public static float EaseOutExpo(float x) {
			return UnityEngine.Mathf.Approximately(x, 1.0f) ? 1.0f : 1.0f - UnityEngine.Mathf.Pow(2.0f, -10.0f * x);
		}
	}
}