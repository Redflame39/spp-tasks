using System;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;

namespace MPP7
{

    public delegate void Task();

    public static class Program
    {

        public static void Main(string[] args)
        {
            Test1();
        }

        static public void Test1()
        {
            var counter = 0;

            Program.WaitAll(new Task[]
            {
                () =>
                {
                    counter++;
                },
                () =>
                {
                    Thread.Sleep(300);
                    counter++;
                },
                () =>
                {
                    Thread.Sleep(300);
                    counter++;
                    Thread.Sleep(100);
                }
            });


        }

        
        public static void WaitAll(Task[] tasks)//принимает массив делегатов
        {
            //ManualResetEvent представляет событие синхронизации потока, которое при получении сигнала необходимо сбросить вручную.
            var signal = new ManualResetEvent(false);
            var numberOfTasks = tasks.Length;


            for (var i = 0; i < tasks.Length; i++)
            {
                var index = i;
                //ThreadPool.QueueUserWorkItem помещает метод в очередь на выполнение. Метод выполняется, когда становится доступен поток из пула потоков.
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    Console.WriteLine("Executing task " + index);
                    tasks[index]();

                    MarkTaskExecuted(ref numberOfTasks, signal);//метод уменьшает numberOfTasks на 1, 
                });
            }

            Console.WriteLine("Waiting for " + tasks.Length + " tasks");
            //WaitHandle.WaitOne блокирует текущий поток до получения сигнала объектом WaitHandle, то есть ожидание вызова метода Set()
            signal.WaitOne();

            Console.WriteLine("Tasks executed");
        }
        private static void MarkTaskExecuted(ref int numberOfTasks, EventWaitHandle signal)
        {
            //Interlocked.Decrement уменьшает значение заданной переменной и сохраняет результат в виде атомарной операции.
            if (Interlocked.Decrement(ref numberOfTasks) == 0)
            {
                //EventWaitHandle.Set задает сигнальное состояние события, позволяя одному или нескольким ожидающим потокам продолжить.
                signal.Set();
            }
        }
    }


}