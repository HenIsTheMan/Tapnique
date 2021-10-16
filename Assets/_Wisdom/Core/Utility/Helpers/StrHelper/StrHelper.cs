using System.Linq;

namespace Genesis.Wisdom {
    internal static class StrHelper {
		internal static bool IsEmail(this string myStr) {
			int atIndex = myStr.IndexOf('@');
			int dotIndex = myStr.IndexOf('.');
			int myStrLen = myStr.Length;

			return !(myStr.Count(myChar => myChar == '@') != 1
				|| myStr.Count(myChar => myChar == '.') != 1
				|| atIndex < 1
				|| dotIndex < 3
				|| atIndex > myStrLen - 4
				|| dotIndex > myStrLen - 2
				|| (atIndex >= dotIndex - 1)
				|| !myStr.Substring(0, atIndex).All(char.IsLetterOrDigit)
				|| !myStr.Substring(atIndex + 1, dotIndex - atIndex - 1).All(char.IsLetterOrDigit)
				|| !myStr.Substring(dotIndex + 1, myStrLen - dotIndex - 1).All(char.IsLetterOrDigit));
		}
	}
}