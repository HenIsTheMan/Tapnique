namespace Genesis.Wisdom {
    internal static partial class RateOfChange {
		private static System.Type Type {
			get;
		} = typeof(RateOfChange);

		private static readonly float c1 = 1.70158f;
		private static readonly float c2 = c1 * 1.525f;
		private static readonly float c3 = c1 + 1.0f;
		private static readonly float c4 = 2.0f * UnityEngine.Mathf.PI / 3.0f;
		private static readonly float c5 = 2.0f * UnityEngine.Mathf.PI / 4.5f;
		private static readonly float d1 = 2.75f;
		private static readonly float n1 = 7.5625f;
    }
}