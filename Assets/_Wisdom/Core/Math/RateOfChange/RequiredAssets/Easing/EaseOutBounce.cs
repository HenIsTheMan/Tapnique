namespace Genesis.Wisdom {
	internal static partial class RateOfChange {
		public static float EaseOutBounce(float x) {
			if(x < 1.0f / d1) {
				return n1 * x * x;
			} else if(x < 2.0f / d1) {
				return n1 * (x -= 1.5f / d1) * x + 0.75f;
			} else if(x < 2.5f / d1) {
				return n1 * (x -= 2.25f / d1) * x + 0.9375f;
			} else {
				return n1 * (x -= 2.625f / d1) * x + 0.984375f;
			}
		}
	}
}