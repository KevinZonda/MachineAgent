
using KevinZonda.MachineAgent.ConsoleApp;
using KevinZonda.MachineAgent.ConsoleApp.Controllers;


using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

Console.WriteLine(AboutController.GetAboutMessage());
Console.WriteLine("=======================");

Console.WriteLine("Connecting Telegram...");
var botClient = new TelegramBotClient(Config.TgBotToken);
var me = await botClient.GetMeAsync();
string username = me.Username;
Console.WriteLine($"Connected. Bot ID={me.Id}, Username={me.Username}");

using var cts = new CancellationTokenSource();

var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = Array.Empty<UpdateType>()
};
botClient.StartReceiving(
    updateHandler: HandleUpdateAsync,
    pollingErrorHandler: HandlePollingErrorAsync,
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


Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    if (update.Message is not { } message)
        return;
    if (message.Text is not { } messageText)
        return;

    var chatId = message.Chat.Id;

    Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");
    var (cmd, args) = ParseMsg(messageText, username);

    await botClient.SendTextMessageAsync(
        chatId: chatId,
        text: "You said:\n" + messageText,
        cancellationToken: cancellationToken);
}


static (string? command, string? argument) ParseMsg(ReadOnlySpan<char> text, string botUsername)
{
    if (text.IsEmpty)
        return (null, null);

    if (text[0] != '/' || text.Length < 2)
        return (null, null);

    // Remove the leading '/'
    text = text[1..];

    // Split command and argument
    ReadOnlySpan<char> command, argument;
    var spacePos = text.IndexOf(' ');
    if (spacePos == -1)
    {
        command = text;
        argument = ReadOnlySpan<char>.Empty;
    }
    else if (spacePos == text.Length - 1)
    {
        command = text[..spacePos];
        argument = ReadOnlySpan<char>.Empty;
    }
    else
    {
        command = text[..spacePos];
        argument = text[(spacePos + 1)..];
    }

    // Verify and remove trailing '@bot' from command
    var atSignIndex = command.IndexOf('@');
    if (atSignIndex != -1)
    {
        if (atSignIndex != command.Length - 1)
        {
            var atUsername = command[(atSignIndex + 1)..];
            if (!atUsername.SequenceEqual(botUsername))
            {
                return (null, null);
            }
        }

        command = command[..atSignIndex];
    }

    argument = argument.Trim();

    // Convert back to string
    string? commandString = null;
    string? argumentString = null;
    if (!command.IsEmpty)
    {
        commandString = command.ToString();

        if (!argument.IsEmpty)
        {
            argumentString = argument.ToString();
        }
    }

    return (commandString, argumentString);
}