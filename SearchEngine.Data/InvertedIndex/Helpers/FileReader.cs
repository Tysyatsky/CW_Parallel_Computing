namespace SearchEngineData.InvertedIndex.Helpers;

public static class FileReader
{
    private static readonly char[] TextExtras = { ' ', '\t', '\n', '\r', '.', ',', ';', ':', '!', '?', '>', '<', '"', '/', '(', ')' };

    public static IEnumerable<string> ReadWordsFromFile(string path)
    {
        var text = File.ReadAllText(path);
        return TextToWordsConvertor(text);
    }

    private static IEnumerable<string> TextToWordsConvertor(string text) =>
        text.Split(TextExtras, StringSplitOptions.RemoveEmptyEntries);
}
