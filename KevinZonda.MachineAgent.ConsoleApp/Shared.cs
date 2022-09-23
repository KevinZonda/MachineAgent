using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using File = System.IO.File;

namespace KevinZonda.MachineAgent.ConsoleApp;

internal class Shared
{
    public static TelegramBotClient Bot;
    public static User Me;
    public static ConfigModel Config;

    public const string CONFIG_PATH = "config.json";
    public static async Task Init()
    {
        if (File.Exists(CONFIG_PATH))
        {
            var conf = File.ReadAllText(CONFIG_PATH);
            Config = JsonSerializer.Deserialize<ConfigModel>(conf);
            if (Config == null)
            {
                Console.WriteLine("Error: Cannot read config. Exiting...");
            }
        }
        Bot = new TelegramBotClient(Config!.TgBotToken);
        Me = await Bot.GetMeAsync();
    }
}
