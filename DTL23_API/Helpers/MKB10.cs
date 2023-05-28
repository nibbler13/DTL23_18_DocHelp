namespace DTL23_API.Helpers {
	internal static class MKB10 {
		public static bool IsMkb10StringCorrect(string mkbCode) {
			if (string.IsNullOrWhiteSpace(mkbCode) ||
				mkbCode.Length < 3 || 
				mkbCode.Length > 8 ||
				!char.IsLetter(mkbCode[0]) ||
				!char.IsDigit(mkbCode[1]) ||
				!char.IsDigit(mkbCode[2]))
				return false;

			return true;
		}
	}
}
