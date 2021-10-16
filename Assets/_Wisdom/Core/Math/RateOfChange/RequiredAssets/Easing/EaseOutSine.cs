namespace Genesis.Wisdom {
	internal static partial class RateOfChange {
		public static float EaseOutSine(float x) {
			return UnityEngine.Mathf.Sin(x * UnityEngine.Mathf.PI * 0.5f);
		}
	}
}