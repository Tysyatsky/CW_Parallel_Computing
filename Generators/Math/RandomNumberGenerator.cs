namespace Generators.Math
{
    public static class RandomNumberGenerator
    {   
        private static readonly Random _random = new();
        private static readonly int _minTime = 6;
        private static readonly int _maxTime = 14;
        public static int GetTaskExecutionTime()
        {
            return _random.Next(_minTime, _maxTime);
        }
    }
}