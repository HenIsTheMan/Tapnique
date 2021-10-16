namespace Genesis.Wisdom {
	internal static partial class RateOfChange {
		public static float EaseInOutBack(float x) {
			return x < 0.5f
				? UnityEngine.Mathf.Pow(2.0f * x, 2.0f) * ((c2 + 1.0f) * 2.0f * x - c2) * 0.5f
				: (UnityEngine.Mathf.Pow(2.0f * x - 2.0f, 2.0f) * ((c2 + 1.0f) * (x * 2.0f - 2.0f) + c2) + 2.0f) * 0.5f;
		}
	}
}