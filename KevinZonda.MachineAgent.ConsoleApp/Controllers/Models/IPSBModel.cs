using System.Text.Json.Serialization;

namespace KevinZonda.MachineAgent.ConsoleApp.Controllers.Models;

internal class IPSBModel
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
