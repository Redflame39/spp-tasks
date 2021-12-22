using System;
using System.Runtime.InteropServices;

public class NativeBuffer : IDisposable
{
    // Дескриптор выделенного блока. Только отдаем, устанавливать запрещаем
    public IntPtr Handle { get; }

    // sizeBytes - размер выделяемого блока
    public NativeBuffer(int sizeBytes)
    {
        try
        {
            Handle = Marshal.AllocHGlobal(sizeBytes); // выделение
        }
        catch (OutOfMemoryException) // нету свободной памяти
        { 
            Console.WriteLine("There is insufficient memory to satisfy the request.");
        }
    }

    // деструктор на этапе компиляции превратится в Finalize
    ~NativeBuffer()
    {
        Marshal.FreeHGlobal(Handle); // освобождаем
    }

    // для ручной очистки
    public void Dispose()
    {
        Marshal.FreeHGlobal(Handle); // освобождаем
    }
}