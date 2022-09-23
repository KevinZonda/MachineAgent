
using KevinZonda.MachineAgent.ConsoleApp;
using KevinZonda.MachineAgent.ConsoleApp.Controllers;


using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

Console.WriteLine(AboutController.GetAboutMessage());
Console.WriteLine("=======================");

Console.WriteLine("Connecting Telegram...");
await Shared.Init(Config.TgBotToken);

Console.WriteLine($"Connected. Bot ID={Shared.Me.Id}, Username={Shared.Me.Username}");

using var cts = new CancellationTokenSource();

var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = Array.Empty<UpdateType>()
};

Shared.Bot.StartReceiving(
    updateHandler: BotHandler.HandleUpdateAsync,
    pollingErrorHandler: BotHandler.HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);

bool keepRunning = true;

Console.CancelKeyPress += delegate {
    Console.WriteLine("Exiting...");
    keepRunning = false;
};

while (keepRunning) { }
cts.Cancel();
