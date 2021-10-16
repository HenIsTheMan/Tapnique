namespace Genesis.Wisdom {
	internal static partial class RateOfChange {
		public static float EaseInExpo(float x) {
			return UnityEngine.Mathf.Approximately(x, 0.0f) ? 0.0f : UnityEngine.Mathf.Pow(2.0f, 10.0f * x - 10.0f);
		}
	}
}