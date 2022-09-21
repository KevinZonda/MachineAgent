using KevinZonda.MachineAgent.ConsoleApp.Factory;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KevinZonda.MachineAgent.ConsoleApp.Controllers;

internal class IPController
{
    private static readonly HttpClient _hc;

    static IPController()
    {
        _hc = HttpClientFactory.GetOne();
    }
    public static async Task<IPResult> GetIPInfo()
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

    private class IPSBModel
    {
        [JsonPropertyName("city")]
        public string City { get; set; }
        [JsonPropertyName("region")]
        public string Region { get; set; }
        [JsonPropertyName("country")]
        public string Country { get; set; }
        [JsonPropertyName("ip")]
        public string IP { get; set; }

    }

    public class IPResult
    {
        public bool IsOk { get; init; }
        public string Addr { get; set; } = "";
        public string Location { get; set; } = "";
    }
}
