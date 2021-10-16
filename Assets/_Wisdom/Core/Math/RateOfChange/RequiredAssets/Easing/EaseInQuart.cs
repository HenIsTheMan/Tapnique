namespace Genesis.Wisdom {
	internal static partial class RateOfChange {
		public static float EaseInQuart(float x) {
			return x * x * x * x;
		}
	}
}