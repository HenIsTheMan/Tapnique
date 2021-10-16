using Genesis.Wisdom;
using UnityEngine;

namespace Genesis.Creation {
	internal sealed partial class IslandControl {
		[System.Serializable]
		internal partial struct IslandDifficultyLvl {
			internal string MyName {
				get => type.ToString();
			}

			internal Color MyColor {
				get => colors[(int)type];
			}

			[SerializeField]
			private IslandDifficultyLvlType type;

			[EnumIndicesForColor(true, true, true, typeof(IslandDifficultyLvlColor))]
			[SerializeField]
			private Color[] colors;
		};
	}
}