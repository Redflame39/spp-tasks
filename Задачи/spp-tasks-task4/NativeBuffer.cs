using System;
using System.Runtime.InteropServices;

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
        Dispose(false);
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
            Dispose(true);
            GC.SuppressFinalize(this);
            disposed = true;
        }
    }
    protected virtual
    void Dispose(bool disposing)
    {
        if (handle != IntPtr.Zero)
            Marshal.FreeHGlobal(handle);
    }
}
