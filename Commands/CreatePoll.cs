using Telegram.Bot.Types;
using Telegram.Bot;

namespace PadelTennisKrdBot.Commands
{
    internal class CreatePoll : TgBotCommand.TgBotCommand
    {
        public override string Command => "/createpoll";

        public override async Task Handle(ITelegramBotClient botClient, Message message)
        {
            if (CheckCommand(botClient, message))
            {
                ChatMember[] chatMembers = await botClient.GetChatAdministratorsAsync(AppData.ChatId);
                if (chatMembers.Select(x => x.User.Id).Contains(message.From!.Id))
                {
                    TgBot.CreatePoll();
                    botClient.SendTextMessageAsync(message.Chat.Id, "Опрос создан");
                }
                else botClient.SendTextMessageAsync(message.Chat.Id, "Только администратор чата может создать опрос");
            }
            else Succsessor?.Handle(botClient, message);
        }
    }
}
