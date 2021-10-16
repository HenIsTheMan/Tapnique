namespace Genesis.Wisdom {
	internal static partial class RateOfChange {
		public static float EaseOutQuad(float x) {
			return 1.0f - UnityEngine.Mathf.Pow(1.0f - x, 2.0f);
		}
	}
}