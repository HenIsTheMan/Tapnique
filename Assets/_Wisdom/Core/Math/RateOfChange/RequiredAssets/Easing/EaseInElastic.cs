namespace Genesis.Wisdom {
	internal static partial class RateOfChange {
		public static float EaseInElastic(float x) {
			return UnityEngine.Mathf.Approximately(x, 0.0f)
				? 0.0f
				: (UnityEngine.Mathf.Approximately(x, 1.0f)
				? 1.0f
				: -UnityEngine.Mathf.Pow(2.0f, 10.0f * x - 10.0f) * UnityEngine.Mathf.Sin((x * 10.0f - 10.75f) * c4));
		}
	}
}