using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace BasicMemoryMonitor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Process name: "); 
            string processName = Console.ReadLine();
            Console.Write("Read interval (samples / min): "); 
            int readInterval = int.Parse(Console.ReadLine());

            using (StreamWriter sw = new StreamWriter($"{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}_{Path.GetFileNameWithoutExtension(processName)}.csv"))
            {
                Process[] procs = Process.GetProcessesByName(processName);
                if(procs.Length == 0)
                {
                    Console.WriteLine($"{processName} not found, continue?");
                    if(Console.ReadLine().ToLower() == "n")
                    {
                        return; 
                    }
                }

                for (; ; )
                {
                    int sleepInMs = (1000 * 60) / readInterval; 
                    procs = Process.GetProcessesByName(processName);
                    if (procs.Length >= 1)
                    {
                        long numBytes = procs[0].PrivateMemorySize64;
                        sw.WriteLine($"{DateTime.Now.Ticks},{numBytes}");
                        sw.Flush();

                        Thread.Sleep(TimeSpan.FromMilliseconds(sleepInMs)); 
                    }
                }
            }
        }
    }
}
