namespace Genesis.Wisdom {
	internal static partial class RateOfChange {
		public static float EaseInOutSine(float x) {
			return -(UnityEngine.Mathf.Cos(UnityEngine.Mathf.PI * x) - 1.0f) * 0.5f;
		}
	}
}