using Telegram.Bot.Types;

namespace KevinZonda.MachineAgent.ConsoleApp.Middleware;

internal class AuthMiddleware
{
    public static bool IsAllowedTgUser(Message msg)
    {
        return IsAllowedTgUser(msg.GetUid());
    }

    public static bool IsAllowedTgUser(long? fid)
    {
        if (!Shared.Config.EnableAuth) return true;
        if (Shared.Config.Admin.Length < 1) return false;
        if (fid == null) return false;
        return Shared.Config.Admin.Contains(fid.Value);
    }
}
