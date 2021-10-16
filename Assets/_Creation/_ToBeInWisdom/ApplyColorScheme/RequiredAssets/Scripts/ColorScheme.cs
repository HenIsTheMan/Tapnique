using UnityEngine;

namespace Genesis.Creation {
	[CreateAssetMenu(
		fileName = nameof(ColorScheme),
		menuName = "ScriptableObjs/" + nameof(ColorScheme),
		order = 0
	)]
	internal sealed class ColorScheme: ScriptableObject { //POD
		private const float defaultR = 1.0f;
		private const float defaultG = 1.0f;
		private const float defaultB = 1.0f;
		private const float defaultIntensity = 4.0f;
		private static readonly float defaultColorMultiplier = Mathf.Pow(2.0f, defaultIntensity);

		[field: ColorUsage(true, true)]
		[field: SerializeField]
		internal Color[] ColorArr {
			get;
			private set;
		} = new Color[1] {
			new Color(
				defaultR * defaultColorMultiplier,
				defaultG * defaultColorMultiplier,
				defaultB * defaultColorMultiplier,
				1.0f
			)
		};
    }
}