using System.Collections.Concurrent;

namespace KevinZonda.MachineAgent.ConsoleApp;

public class CommandRouter
{
    private static ConcurrentDictionary<string, Func<string?, string>> _dic = new();

    public static async Task<string> GetRouteResult(string command, string? args)
    {
        if (_dic.TryGetValue(command, out var action))
        {
            try
            {
                return action(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Catched: Command: {command}; Args: {args}\nDetails: {ex}");
                return "Exception detected. Command failed.";
            }
        }

        return "";
    }

    private static void InitRouter()
    {
        _dic.TryAdd("info", new(_ => Controllers.SystemInfoController.GetSysInfoMessage()));
        _dic.TryAdd("about", new(_ => Controllers.AboutController.GetAboutMessage()));
        _dic.TryAdd("ip", new(_ => Controllers.IPController.GetIPLocation()));
    }

    static CommandRouter()
    {
        InitRouter();
    }
}
