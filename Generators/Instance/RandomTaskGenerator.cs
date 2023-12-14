using Data.Models;
using Generators.Math;

namespace Generators.Instance
{
    public static class RandomTaskGenerator
    {
        private static volatile int _taskCounter = 0;
        private static Random _random = new Random();
        public static FakeTask GetTask()
        {
            int id = _taskCounter;
            _taskCounter++;
            return new(
                id,
                $"Task #{id}",
                RandomNumberGenerator.GetTaskExecutionTime(),
                $"Task #{id} done executing!"
            );
        }

    }
}
