namespace Genesis.Wisdom {
	internal sealed partial class LineEndingChanger {
		private enum LineEndingType: byte {
			CRLF,
			CR,
			LF,
			Mixed,
			Amt
		}
	}
}