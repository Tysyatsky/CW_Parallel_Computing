namespace CourseWork
{
    class InvertedIndex
    {
        private Dictionary<string, List<int>> index;

        public InvertedIndex()
        {
            index = new Dictionary<string, List<int>>();
        }

        public void AddDocument(int documentId, string[] terms)
        {
            foreach (var term in terms)
            {
                if (!index.ContainsKey(term))
                {
                    index[term] = new List<int>();
                }

                if (!index[term].Contains(documentId))
                {
                    index[term].Add(documentId);
                }
            }
        }

        public List<int> Search(string term)
        {
            if (index.ContainsKey(term))
            {
                return index[term];
            }
            else
            {
                return new List<int>();
            }
        }

        public void PrintIndex()
        {
            foreach (var term in index.Keys)
            {
                Console.Write($"{term}: ");
                foreach (var docId in index[term])
                {
                    Console.Write($"{docId} ");
                }
                Console.WriteLine();
            }
        }
    }
}
