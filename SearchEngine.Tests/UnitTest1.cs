using SearchEngineData;
using SearchEngineData.InvertedIndex.Instance;
using SearchEngineServer;

namespace SearchEngineTests
{
    public class Tests
    {
        private static readonly string[] Doc1 = { "apple", "banana", "orange" };
        private static readonly string[] Doc2 = { "apple", "pear", "grape" };
        private static readonly string[] Doc3 = { "banana", "kiwi", "orange" };
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}