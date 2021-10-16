namespace Genesis.Wisdom {
	internal sealed partial class FlagsEnumTest {
		[System.Flags]
		[System.Serializable]
		internal enum TestFlagsEnum: int { //Does not work for uint
			None,
			Test0 = 1 << 0,
			Test1 = 1 << 1,
			Test2 = 1 << 2,
			Test3 = 1 << 3,
			Test4 = 1 << 4,
			All = ~0
		}
	}
}