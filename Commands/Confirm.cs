using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Microsoft.EntityFrameworkCore;

namespace PadelTennisKrdBot.Commands
{
    internal class Confirm : TgBotCommand.TgBotCommand
    {
        public override string Command => "/confirm";

        public override async Task Handle(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            if (!CheckCommand(botClient, callbackQuery))
            {
                Succsessor.Handle(botClient, callbackQuery);
                return;
            }

            using PadelTennisDbContext context = await AppData.PadelDbContextFactoty.CreateDbContextAsync();
            int gameId = int.Parse(callbackQuery.Data.Replace(Command, string.Empty));
            Game? game = context.Games.Include(x => x.Players).FirstOrDefault(x => x.Id == gameId);
            if (game != null)
            {
                Player? player = context.Players.FirstOrDefault(x => x.Id == callbackQuery.From.Id);
                if (player == null)
                {
                    player = new Player(callbackQuery.From);
                    context.Players.Add(player);
                }

                if (game.Players.Contains(player))
                {
                    game.Players.Remove(player);
                    botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Участие отменено");
                }
                else
                {
                    if (game.Players.Count < 4)
                    {
                        game.Players.Add(player);
                        botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Участие подтверждено");
                    }
                    else botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Нет мест");
                }
                context.SaveChangesAsync();
                InlineKeyboardButton button = callbackQuery.Message.ReplyMarkup.InlineKeyboard.First().First();
                InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup
                    (
                        new List<InlineKeyboardButton[]>
                        {
                                new[] { button },
                                new[] { InlineKeyboardButton.WithCallbackData(string.Join(", ", game.Players.Select(x => x.FirstName)), "/nothing") }
                        }
                    );
                botClient.EditMessageReplyMarkupAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId, replyMarkup: inlineKeyboard);
            }
            else botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Игра не найдена");
        }
    }
}
