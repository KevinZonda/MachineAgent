using System.Runtime.InteropServices;

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
}
