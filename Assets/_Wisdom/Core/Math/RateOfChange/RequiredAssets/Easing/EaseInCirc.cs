namespace Genesis.Wisdom {
	internal static partial class RateOfChange {
		public static float EaseInCirc(float x) {
			return 1.0f - UnityEngine.Mathf.Sqrt(1.0f - x * x);
		}
	}
}