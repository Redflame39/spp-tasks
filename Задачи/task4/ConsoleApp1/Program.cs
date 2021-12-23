using System;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Runtime.InteropServices;

namespace MPP7
{

    public delegate void Task();

    public static class Program
    {

        public static void Main(string[] args)
        {
           NativeBuffer buf=new NativeBuffer(1024);
           buf.Dispose();
            for (var i = 0; i < 10000000; i++)
            {
                NativeBuffer buf1 = new NativeBuffer(1024*40);
                

            }
        }

       
    }

    public class NativeBuffer : Object, IDisposable
    {
        private IntPtr handle;
        private bool disposed;
        public NativeBuffer(int size)
        {
            handle = Marshal.AllocHGlobal(size);
        }
        ~NativeBuffer()
        {
            Dispose();
        }
        public IntPtr Handle
        {
            get
            {
                if (!disposed)
                    return handle;
                else
                    throw new ObjectDisposedException(ToString());
            }
        }

        public void Dispose()
        {
            if (!disposed)
            {
                if (handle != IntPtr.Zero)
                    Marshal.FreeHGlobal(handle);

                GC.SuppressFinalize(this);
                disposed = true;
            }
        }
       
    }



}