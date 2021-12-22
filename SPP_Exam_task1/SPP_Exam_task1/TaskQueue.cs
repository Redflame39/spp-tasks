using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SPP_Exam_task1
{
    class TaskQueue: IDisposable
    {
        //delegate of method performed
        public delegate void TaskDelegate();


        private readonly Queue<TaskDelegate> tasksQueue = new Queue<TaskDelegate>();
        private readonly Thread[] threads;
        private readonly object locker = new object(); //locker for memory access control
        private bool isRun = true; //?queue is running


        /**
         * Create new tread poll with fixed treads count
         */
        public TaskQueue(int threadsCount)
        {
            threads = new Thread[threadsCount];

            for (var i = 0; i < threadsCount; i++)
            {
                threads[i] = new Thread(QueueConsumeTask) { Name = "Thread " + i }; //new thread performing method shown below and with name set
                threads[i].Start();
            }
        }

        /**
         * Function which run in each tread. It check queue and proceed task
         */
        private void QueueConsumeTask()
        {
            while (isRun)
            {
                TaskDelegate taskDelegate;
                bool isExecuting;

                lock (locker) //queue access only for 1 process in each moment
                {
                    if (tasksQueue.Count == 0)
                    {
                        Console.WriteLine("Thread " + Thread.CurrentThread.Name + " in wait mode ");
                        Monitor.Wait(locker); //waiting other processes
                        Console.WriteLine("Thread awake " + Thread.CurrentThread.Name);
                    }

                    isExecuting = tasksQueue.TryDequeue(out taskDelegate); //if dequeue succeded - execution starts
                }

                if (isExecuting)
                {
                    Console.WriteLine("Executing task in " + Thread.CurrentThread.Name);
                    taskDelegate(); //run method
                }

            }

            Console.WriteLine(Thread.CurrentThread.Name + " die.");
        }

        /**
         * Add task to queue.
         */
        public virtual void EnqueueTask(TaskDelegate taskDelegate)
        {
            lock (locker)
            {
                tasksQueue.Enqueue(taskDelegate);
                Monitor.Pulse(locker); // Notify slept tread that there is new task
            }

        }



        /**
         * Wait until all active tasks ends and kill all treads 
         */
        public void Dispose()
        {

            while (true)
            {
                Thread.Sleep(2000);

                lock (locker) //waiting for all processes to be dequeued
                {
                    if (tasksQueue.Count == 0) break;
                }
            }

            lock (locker)
            {
                isRun = false;
                Monitor.PulseAll(locker);
            }
        }
    }
}
