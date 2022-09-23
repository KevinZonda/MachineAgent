using System.Runtime.InteropServices;
using Telegram.Bot.Types;

namespace KevinZonda.MachineAgent.ConsoleApp;

internal static class Extensions
{
    public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> doSth)
    {
        foreach (var t in source) doSth(t);
    }

    public static T Null<T>()
    {
        return default;
    }

    public static bool IsUnix()
    {
        return RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ||
               RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
    }

    public static long? GetUid(this Message msg)
    {
        if (msg == null || msg.From == null) return null;
        return msg.From.Id;
    }
}
