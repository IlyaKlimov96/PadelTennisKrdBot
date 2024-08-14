using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace PadelTennisKrdBot.Commands
{
    internal class Nothing : TgBotCommand.TgBotCommand
    {
        public override string Command => "/nothing";
        public override Task Handle(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            if (CheckCommand(botClient, callbackQuery)) botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
            else Succsessor?.Handle(botClient, callbackQuery);
            return Task.CompletedTask;
        }
    }
}
