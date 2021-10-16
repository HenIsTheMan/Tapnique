namespace Genesis.Wisdom {
	internal static partial class RateOfChange {
		public static float EaseInSine(float x) {
			return 1.0f - UnityEngine.Mathf.Cos(x * UnityEngine.Mathf.PI * 0.5f);
		}
	}
}