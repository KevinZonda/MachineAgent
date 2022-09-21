namespace KevinZonda.MachineAgent.ConsoleApp.Factory;

internal class HttpClientFactory
{
    private static readonly HttpClient _hc;
    public static HttpClient GetOne()
    {
        return _hc;
    }

    static HttpClientFactory()
    {
        _hc = new HttpClient()
        {
            Timeout = TimeSpan.FromSeconds(5)
        };
    }
}
