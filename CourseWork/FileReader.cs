namespace CourseWork
{
    public static class FileReader
    {
        private static readonly char[] textExtras = { ' ', '\t', '\n', '\r', '.', ',', ';', ':', '!', '?', '>', '<', '"', '/', '(', ')' };

        public static string[] ReadWordsFromFile(string path)
        {
            string text = File.ReadAllText(path);
            return TextToWordsConvertor(text);
        }

        private static string[] TextToWordsConvertor(string text) =>
            text.Split(textExtras, StringSplitOptions.RemoveEmptyEntries);
    }
}
