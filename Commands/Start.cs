using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace PadelTennisKrdBot.Commands
{
    internal class Start : TgBotCommand.TgBotCommand
    {
        public override string Command => "/start";

        public override Task Handle(ITelegramBotClient botClient, Message message)
        {
            if (CheckCommand(botClient, message)) botClient.SendTextMessageAsync(message.Chat.Id, "Приветствую! Смотри команды в меню");
            else Succsessor?.Handle(botClient, message);
            return Task.CompletedTask;
        }
    }
}
