namespace Genesis.Wisdom {
	internal static partial class RateOfChange {
		public static float EaseInQuint(float x) {
			return x * x * x * x * x;
		}
	}
}