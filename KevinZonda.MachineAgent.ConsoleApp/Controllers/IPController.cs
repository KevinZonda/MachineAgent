using KevinZonda.MachineAgent.ConsoleApp.Controllers.Models;
using KevinZonda.MachineAgent.ConsoleApp.Factory;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace KevinZonda.MachineAgent.ConsoleApp.Controllers;

internal partial class IPController
{
    private static readonly HttpClient _hc;

    static IPController()
    {
        _hc = HttpClientFactory.GetOne();
    }

    public static string GetIPLocation()
    {
        var rst = GetIPInfoAsync().Result;
        if (rst == null) return "Request failed.";
        return $"Location: {rst.Location}";
    }
    public static async Task<IPResult> GetIPInfoAsync()
    {
        var resp = await _hc.GetAsync("https://api.ip.sb/geoip");
        if (!resp.IsSuccessStatusCode) return new IPResult() { IsOk = false };
        var content = await resp.Content.ReadAsStringAsync();
        var mod = JsonSerializer.Deserialize<IPSBModel>(content);
        if (mod == null) return new IPResult() { IsOk = false };
        return new IPResult()
        {
            IsOk = true,
            Addr = mod.IP,
            Location = EasyJoin(", ", mod.City, mod.Region, mod.Country)
        };
    }

    private static string EasyJoin(string sep, params string[] content)
    {
        if (content.Length < 1) return "";
        if (content.Length == 1) return content[0].Trim();

        var sb = new StringBuilder();
        foreach (var item in content)
        {
            if (string.IsNullOrWhiteSpace(item)) continue;
            sb.Append(item.Trim());
            sb.Append(sep);
        }
        sb.Remove(sb.Length - sep.Length, sep.Length);
        return sb.ToString();
    }
}
