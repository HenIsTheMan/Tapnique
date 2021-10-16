namespace Genesis.Wisdom {
	internal static partial class RateOfChange {
		public static float EaseInBack(float x) {
			return c3 * x * x * x - c1 * x * x;
		}
	}
}