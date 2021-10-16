namespace IWP.Math {
	internal static partial class Easing {
		#region Fields
		#endregion

		#region Properties
		#endregion

		public static float EaseInBack(float x) {
			return c3 * x * x * x - c1 * x * x;
		}
	}
}