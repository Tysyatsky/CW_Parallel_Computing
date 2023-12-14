namespace Data.Models
{
    public class FakeTask
    {
        private readonly int _id;
        private readonly string _title;
        private readonly int _executionTime;
        private readonly string _result;

        public FakeTask(int id, string title, int executionTime, string result)
        {
            _id = id;
            _title = title;
            _executionTime = executionTime;
            _result = result;
        }

        public int Id { get { return _id;} }
        public string Title { get { return _title;} }
        public int ExecutionTime { get { return _executionTime; } }
        public string Result { get { return _result;} }

        public void Print()
        {
            Console.WriteLine($"Task id: {_id}");
            Console.WriteLine($"Title: {_title}");
            Console.WriteLine($"Ex. time: {_executionTime}");
            Console.WriteLine($"Result: {_result}");
        }

        public string Execute()
        {   
            Thread.Sleep(ExecutionTime * 1000);
            Console.WriteLine(Result);
            return Result;
        }
    }
}