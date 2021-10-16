namespace Genesis.Wisdom {
	internal static partial class RateOfChange {
		public static float EaseInOutBounce(float x) {
			return x < 0.5f ? (1.0f - EaseOutBounce(1.0f - 2.0f * x)) * 0.5f : (1.0f + EaseOutBounce(2.0f * x - 1.0f)) * 0.5f;
		}
	}
}