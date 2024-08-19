using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;
using Microsoft.EntityFrameworkCore;

namespace PadelTennisKrdBot.Commands
{
    internal class DeleteGame : TgBotCommand.TgBotCommand
    {
        private bool _isWaitingNumber = false;

        public override string Command => "/deletegame";

        public override async Task Handle(ITelegramBotClient botClient, Message message)
        {
            if (_isWaitingNumber)
            {
                if (int.TryParse(message.Text, out int id))
                {
                    using PadelTennisDbContext context = await AppData.PadelDbContextFactoty.CreateDbContextAsync();
                    Game? game = context.Games.Find(id);
                    if (game != null)
                    {
                        context.Games.Remove(game);
                        await context.SaveChangesAsync();
                        botClient.SendTextMessageAsync(message.Chat.Id, "Игра удалена");
                    }
                    else botClient.SendTextMessageAsync(message.Chat.Id, "Игра не найдена");
                }
                else botClient.SendTextMessageAsync(message.Chat.Id, "Не удалось распознать id игры");
                _isWaitingNumber = false;
            }
            else if (CheckCommand(botClient, message))
            {
                using PadelTennisDbContext context = await AppData.PadelDbContextFactoty.CreateDbContextAsync();
                List<Game> games = context.Games.AsNoTracking().Where(x => x.Date >= DateTime.Now.Date).OrderBy(x => x.Date).ToList();
                if (games.Count > 0)
                {
                    botClient.SendTextMessageAsync(message.Chat.Id, "Пришлите id игры");
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.AppendLine("Предстоящие игры:");
                    foreach (Game game in games)
                    {
                        stringBuilder.AppendLine($"{game.Id}) {game.ToString()}");
                    }
                    botClient.SendTextMessageAsync(message.Chat.Id, stringBuilder.ToString());
                    _isWaitingNumber = true;
                }
                else botClient.SendTextMessageAsync(message.Chat.Id, "Нет предстоящих игр");
            }
            else Succsessor?.Handle(botClient, message);
        }
    }
}
