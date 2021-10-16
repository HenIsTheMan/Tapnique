namespace IWP.Math {
	internal static partial class Easing {
		#region Fields
		#endregion

		#region Properties
		#endregion

		public static float EaseInSine(float x) {
			return 1.0f - Trigo.Cos(x * UnityEngine.Mathf.PI * 0.5f);
		}
	}
}