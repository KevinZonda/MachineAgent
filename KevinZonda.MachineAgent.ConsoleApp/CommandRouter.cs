using System.Collections.Concurrent;

namespace KevinZonda.MachineAgent.ConsoleApp;

public class CommandRouter
{
    private static ConcurrentDictionary<string, Func<string, string>> _dic = new();

    public static string GetRouteResult(string command, string args)
    {
        if (_dic.TryGetValue(command, out var action)) return action(args);
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
