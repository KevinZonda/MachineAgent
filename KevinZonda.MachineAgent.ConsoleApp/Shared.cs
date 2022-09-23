using Telegram.Bot;
using Telegram.Bot.Types;

namespace KevinZonda.MachineAgent.ConsoleApp;

internal class Shared
{
    public static TelegramBotClient Bot;
    public static User Me;

    public static async Task Init(string botToken)
    {
        Bot = new TelegramBotClient(botToken);
        Me = await Bot.GetMeAsync();
    }
}
