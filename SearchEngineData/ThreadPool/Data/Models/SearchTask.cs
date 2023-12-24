using System.Text;

namespace SearchEngineData.ThreadPool.Data.Models
{
    public class SearchTask
    {
        private readonly int _minIndex;
        private readonly int _maxIndex;
        private readonly string _searchString;
        private readonly string _directoryPath;
        private readonly InvertedIndex _invertedIndex;

        public SearchTask(int minIndex, int maxIndex, string searchString, string directoryPath, InvertedIndex invertedIndex)
        {
            _minIndex = minIndex;
            _maxIndex = maxIndex;
            _searchString = searchString;
            _directoryPath = directoryPath;
            _invertedIndex = invertedIndex;
        }

        public string Execute()
        {
            var stringBuilder = new StringBuilder();
            var result = _invertedIndex.Search(_searchString);
            Console.WriteLine($"Documents containing '{_searchString}':");

            Console.WriteLine($"Received: {_searchString}");
            foreach (var docId in result)
            {
                stringBuilder.Append($"Doc #{docId}\n");
            }

            return stringBuilder.ToString();
        }
    }
}