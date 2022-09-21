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
}
