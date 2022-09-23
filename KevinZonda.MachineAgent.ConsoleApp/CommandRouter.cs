using KevinZonda.MachineAgent.ConsoleApp.Middleware;
using System.Collections.Concurrent;

namespace KevinZonda.MachineAgent.ConsoleApp;

public class CommandRouter
{
    // command -> args -> uid -> rst
    private static ConcurrentDictionary<string, Func<string?, long?, string>> _dic = new();

    public static async Task<string> GetRouteResult(string command, string? args, long? from)
    {
        if (_dic.TryGetValue(command, out var action))
        {
            try
            {
                return action(args, from);
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
        _dic.TryAdd("info", new((_, _) => Controllers.SystemInfoController.GetSysInfoMessage()));
        _dic.TryAdd("about", new((_, _) => Controllers.AboutController.GetAboutMessage()));
        _dic.TryAdd("ip", new((_, _) => Controllers.IPController.GetIPLocation()));
        _dic.TryAdd("exec", new((args, from) =>
        {
            if (!AuthMiddleware.IsAllowedTgUser(from)) return "Error: Permission denied.";
            if (args == null) return "Error: Please enter code block";
            return Controllers.ScriptEngineController.Exec(args.Trim().Split('\n')).Result.ToString();
        }));
    }

    static CommandRouter()
    {
        InitRouter();
    }
}
