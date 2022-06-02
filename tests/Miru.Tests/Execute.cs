using System;
using System.Threading;

namespace Miru.Tests;

public static class Execute
{
    public static bool Until(Func<bool> task, TimeSpan timeOut)
    {
        var success = false;
        var elapsed = 0;
            
        while (!success && elapsed < timeOut.TotalMilliseconds)
        {
            Thread.Sleep(25);
            elapsed += 25;
            success = task();
        }

        Console.WriteLine($"Finished in {elapsed} ms");
        return success;
    }
}