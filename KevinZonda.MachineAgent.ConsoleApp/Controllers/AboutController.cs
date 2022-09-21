using System.Diagnostics;
using System.Reflection;

namespace KevinZonda.MachineAgent.ConsoleApp.Controllers;

internal class AboutController
{
    public static string GetAboutMessage()
    {
        using var proc = Process.GetCurrentProcess();

        return
            $"KevinZonda.MachineAgent\n" +
            $"=======================\n" +
            $"Version  : {Assembly.GetExecutingAssembly().GetName().Version}\n" +
            $"Runtime  : .NET {Environment.Version}\n" +
            $"OS       : {Environment.OSVersion}\n" +
            $"CPU Count: {Environment.ProcessorCount}\n" +
            $"Memory   : {1.0 * proc.PrivateMemorySize64 / 1024 / 1024} MiB";
    }
}
