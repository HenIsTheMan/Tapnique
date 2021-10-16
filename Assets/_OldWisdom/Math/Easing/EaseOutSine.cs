namespace IWP.Math {
	internal static partial class Easing {
		#region Fields
		#endregion

		#region Properties
		#endregion

		public static float EaseOutSine(float x) {
			return Trigo.Sin(x * UnityEngine.Mathf.PI * 0.5f);
		}
	}
}