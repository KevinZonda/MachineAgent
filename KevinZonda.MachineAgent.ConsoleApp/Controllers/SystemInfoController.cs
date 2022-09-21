using System.Diagnostics;
using System.Runtime.Versioning;

namespace KevinZonda.MachineAgent.ConsoleApp.Controllers;

internal class SystemInfoController
{
    [SupportedOSPlatform("linux")]
    internal static class SysInfoUtilsForLinux
    {
        private const int DigitsInResult = 2;
        private static long totalMemoryInKb;

        public static double GetOverallCpuUsagePercentage()
        {
            var startTime = DateTime.UtcNow;
            var startCpuUsage = Process.GetProcesses().Sum(a => a.TotalProcessorTime.TotalMilliseconds);

            Thread.Sleep(500);

            var endTime = DateTime.UtcNow;
            var endCpuUsage = Process.GetProcesses().Sum(a => a.TotalProcessorTime.TotalMilliseconds);

            var cpuUsedMs = endCpuUsage - startCpuUsage;
            var totalMsPassed = (endTime - startTime).TotalMilliseconds;
            var cpuUsageTotal = cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);

            return Math.Round(cpuUsageTotal * 100, DigitsInResult);
        }

        public static double GetOccupiedMemoryPercentage()
        {
            var totalMemory = GetTotalMemoryInKb();
            var usedMemory = GetUsedMemoryForAllProcessesInKb();

            var percentage = 100.0 * usedMemory / totalMemory;
            return Math.Round(percentage, DigitsInResult);
        }

        private static double GetUsedMemoryForAllProcessesInKb()
        {
            var totalAllocatedMemoryInBytes = Process.GetProcesses().Sum(a => a.PrivateMemorySize64);
            return totalAllocatedMemoryInBytes / 1024.0;
        }

        private const string meminfoPath = "/proc/meminfo";

        private static long GetTotalMemoryInKb()
        {
            if (totalMemoryInKb > 0)
                return totalMemoryInKb;

            if (!File.Exists(meminfoPath)) return -1;

            using var reader = new StreamReader(meminfoPath);
            string line = string.Empty;
            while (!string.IsNullOrWhiteSpace(line = reader.ReadLine()))
            {
                if (line.Contains("MemTotal", StringComparison.OrdinalIgnoreCase))
                {
                    // e.g. MemTotal:       16370152 kB
                    var parts = line.Split(':');
                    var valuePart = parts[1].Trim();
                    parts = valuePart.Split(' ');
                    var numberString = parts[0].Trim();

                    var result = long.TryParse(numberString, out totalMemoryInKb);
                    return result ? totalMemoryInKb : -1;
                }
            }
            return -1;
        }
    }

    [SupportedOSPlatform("linux")]
    public static string GetSysInfoMessage()
    {

        return
            $"Sys Info" +
            $"========" +
            $"CPU: {SysInfoUtilsForLinux.GetOverallCpuUsagePercentage()}%\n" +
            $"MEM: {SysInfoUtilsForLinux.GetOccupiedMemoryPercentage()}%";
    }
}
