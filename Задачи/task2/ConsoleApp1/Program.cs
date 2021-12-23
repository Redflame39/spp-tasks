using System.Threading;


namespace ConsoleApplication1.mutex.tests
{

    public class Program


    {

        public static void Main(string[] args)
        {
            //TestTwoThreadInSameTime();
            //TryToUnlockAlienMutex();
            int a = 1;
            Console.WriteLine(Interlocked.CompareExchange(ref a,0,1));
            Console.WriteLine(a);
        }

        static public void TestTwoThreadInSameTime()
        {
            const int threadsCount = 10;

            var mutex = new Mutex();
            var counter = 0;
            
            var arr = new bool[threadsCount];
            var threads = new Thread[threadsCount];

            var isInvalidValue = false;
            
            
            for (var i = 0; i < threadsCount; i++)
            {
                var currIndex = i;
                threads[i] = new Thread(o =>
                {
                    mutex.Lock();
                    counter++;
                    Thread.Sleep(50);
                    if (!isInvalidValue && counter != 1)
                    {
                        isInvalidValue = true;
                    }
                    Thread.Sleep(50);
                    counter--;
                    arr[currIndex] = true;
                    mutex.Unlock();
                }) {Name = "Thread " + i};
                threads[i].Start();
            }

            for (var i = 0; i < threadsCount; i++) 
                threads[i].Join();
            
            for (var i = 0; i < threadsCount; i++)
            {
                
            }
            
        }


         static public void TryToUnlockAlienMutex()
        {
            var mutex = new Mutex();
            
            var counter = 0;
            bool isInvalid = false;

            var thread1 = new Thread(o =>
            {
                mutex.Lock();
                counter++;
                Thread.Sleep(400);
                counter--;
                mutex.Unlock();
            }) { Name = "Thread 1" };
            
            thread1.Start();
            
            Thread.Sleep(100);
            mutex.Unlock();
            mutex.Lock();
           
            mutex.Unlock();
        }
    }


    public class Mutex
    {

        private const int SleepTime = 20;

        private int BusyByCurrentTread
        {
            get { return Thread.CurrentThread.ManagedThreadId; }//возвращает уникальный идентификатор текущего управляемого потока.
            //Значение свойства потока служит для уникальной идентификации этого потока в процессе.
        }

        private const int Free = 0;

        /**
         * Variable which can contain 0 if mutex is free or THREAD_ID when mutex is busy.
         */
        private int _lockVariable;//хранит 0, если mutex свободен, или THREAD_ID, если занят

        public Mutex()
        {
           
        }

        public void Lock()//метод блокировки памяти
        {
            // метод Interlocked.CompareExchange сравнивает два значения на равенство и, если они равны, заменяет первое на третье.
            //этот while работает пока в переменной _lockVariable не станет 0 значение, то есть, пока поток не осводит mutex
            while (Interlocked.CompareExchange(ref _lockVariable,
                       BusyByCurrentTread, Free) != Free)
            {
                Thread.Sleep(SleepTime);
                Console.WriteLine("Tread " + Thread.CurrentThread.Name + " is waiting when mutex will be unlocked "+_lockVariable+" "+BusyByCurrentTread);

            }
            Console.WriteLine("Mutex has been blocked by " + Thread.CurrentThread.Name);

        }

        public void Unlock()//освобождение Mutex
        {
            // метод Interlocked.CompareExchange сравнивает два значения на равенство и, если они равны, заменяет первое.
            //установка id текущего потока в _lockVariable
            if (Interlocked.CompareExchange(ref _lockVariable, Free,
                    BusyByCurrentTread) == BusyByCurrentTread)
            {
               Console.WriteLine("Mutex has been unlocked by " + Thread.CurrentThread.Name);
            }
            else
            {
                Console.WriteLine(Thread.CurrentThread.Name + " tried to unlock alien mutex");
            }
        }
    }
}