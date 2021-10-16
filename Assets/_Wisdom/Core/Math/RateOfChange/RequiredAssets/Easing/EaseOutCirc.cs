namespace Genesis.Wisdom {
	internal static partial class RateOfChange {
		public static float EaseOutCirc(float x) {
			return UnityEngine.Mathf.Sqrt(1.0f - (x - 1.0f) * (x - 1.0f));
		}
	}
}