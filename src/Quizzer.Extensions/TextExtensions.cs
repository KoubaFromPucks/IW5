namespace Quizzer.Extensions; 
public static class TextExtensions {
    public static string TrimWhiteAndUnprintableChars(this string input) {
        int startIndex = -1;
        int endIndex = -1;

        var forbiddenChars = new HashSet<char>() { '\b', '\a', '\0' };

        for (int i = 0; i < input.Length; i++) {
            if (!Char.IsWhiteSpace(input[i]) && !forbiddenChars.Contains(input[i])) {
                startIndex = i;
                break;
            }
        }

        for (int i = input.Length - 1; i >= 0; i--) {
            if (!Char.IsWhiteSpace(input[i]) && !forbiddenChars.Contains(input[i])) {
                endIndex = i;
                break;
            }
        }

        if (startIndex == -1) {
            return "";
        }

        return input.Substring(startIndex, endIndex - startIndex + 1);
    }
}
