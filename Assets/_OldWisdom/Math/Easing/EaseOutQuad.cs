namespace IWP.Math {
	internal static partial class Easing {
		#region Fields
		#endregion

		#region Properties
		#endregion

		public static float EaseOutQuad(float x) {
			return 1.0f - UnityEngine.Mathf.Pow(1.0f - x, 2.0f);
		}
	}
}