using System;

namespace SPP_Exam_task1
{
    class Program
    {
        static void Main(string[] args)
        {
            TaskQueue queue = new TaskQueue(10);
            for(int i=0; i< 10; i++)
            {
                queue.EnqueueTask(ExampleTask);
            }
        }

        static void ExampleTask()
        {
            for(int i=0; i< 15; i++)
            {
                Console.Error.WriteLine($"Number: {i}");
            }
        }
    }
}
