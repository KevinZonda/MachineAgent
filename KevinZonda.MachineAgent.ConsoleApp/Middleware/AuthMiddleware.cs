using Telegram.Bot.Types;

namespace KevinZonda.MachineAgent.ConsoleApp.Middleware;

internal class AuthMiddleware
{
    public static bool IsAllowedTgUser(Message msg, long uid)
    {
        if (msg.From == null) return false;
        return msg.From.Id == uid;
    }
}
