namespace Genesis.Wisdom {
	internal static partial class RateOfChange {
		public static float EaseInOutQuint(float x) {
			return x < 0.5f ? 16.0f * x * x * x * x * x : 1.0f - UnityEngine.Mathf.Pow(-2.0f * x + 2.0f, 5.0f) * 0.5f;
		}
	}
}