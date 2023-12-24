namespace SearchEngineData;

public static class FileReader
{
    private static readonly char[] TextExtras = { ' ', '\t', '\n', '\r', '.', ',', ';', ':', '!', '?', '>', '<', '"', '/', '(', ')' };

    public static string[] ReadWordsFromFile(string path)
    {
        var text = File.ReadAllText(path);
        return TextToWordsConvertor(text);
    }

    private static string[] TextToWordsConvertor(string text) =>
        text.Split(TextExtras, StringSplitOptions.RemoveEmptyEntries);
}
