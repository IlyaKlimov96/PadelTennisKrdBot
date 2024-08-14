using Telegram.Bot.Types;
using Telegram.Bot;

namespace PadelTennisKrdBot.Commands
{
    internal class CreatePoll : TgBotCommand.TgBotCommand
    {
        public override string Command => "/createpoll";

        public override Task Handle(ITelegramBotClient botClient, Message message)
        {
            if (CheckCommand(botClient, message)) TgBot.CreatePoll();
            else Succsessor?.Handle(botClient, message);
            return Task.CompletedTask;
        }
    }
}
