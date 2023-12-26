using System.Text;

namespace SearchEngineData.ThreadPool.Data.Models
{
    public class SearchTask
    {
        private readonly string _searchString;
        private readonly InvertedIndex.Instance.InvertedIndex _invertedIndex;
        private readonly StringBuilder _stringBuilder = new();

        public SearchTask(string searchString, InvertedIndex.Instance.InvertedIndex invertedIndex)
        {
            _searchString = searchString;
            _invertedIndex = invertedIndex;
        }

        public string Execute()
        {
            var result = _invertedIndex.Search(_searchString);
            
            foreach (var docId in result)
            {
                _stringBuilder.Append($"Doc #{docId}\n");
            }

            return _stringBuilder.ToString();
        }
    }
}