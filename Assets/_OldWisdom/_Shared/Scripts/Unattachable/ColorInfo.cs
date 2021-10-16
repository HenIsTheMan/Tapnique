using UnityEngine;

namespace IWP.General {
	[System.Serializable]
	internal struct ColorInfo {
		internal float r;
		internal float g;
		internal float b;
		internal float a;

		public static explicit operator Color(ColorInfo colorInfo) {
			return new Color(
				colorInfo.r,
				colorInfo.g,
				colorInfo.b,
				colorInfo.a
			);
		}

		public static explicit operator ColorInfo(Color color) {
			return new ColorInfo {
				r = color.r,
				g = color.g,
				b = color.b,
				a = color.a,
			};
		}
	};
}