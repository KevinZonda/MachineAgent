namespace KevinZonda.MachineAgent.ConsoleApp;

internal class ConfigModel
{
    public string TgBotToken { get; set; }
    public bool EnableAuth { get; set; }
    public long[] Admin { get; set; }
}