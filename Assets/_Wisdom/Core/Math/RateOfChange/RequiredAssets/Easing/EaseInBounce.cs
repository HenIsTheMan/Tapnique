namespace Genesis.Wisdom {
	internal static partial class RateOfChange {
		public static float EaseInBounce(float x) {
			return 1.0f - EaseOutBounce(1.0f - x);
		}
	}
}