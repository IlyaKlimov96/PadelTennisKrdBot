using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace PadelTennisKrdBot.Commands
{
    internal class GetGames : TgBotCommand.TgBotCommand
    {
        public override string Command => "/getcurrentgames";

        public override async Task Handle(ITelegramBotClient botClient, Message message)
        {
            if (!CheckCommand(botClient, message))
            {
                Succsessor?.Handle(botClient, message);
                return;
            }

            using PadelTennisDbContext context = await AppData.PadelDbContextFactoty.CreateDbContextAsync();
            List<Game> games = context.Games.AsNoTracking().Include(x => x.Players).OrderBy(x => x.Date).ToList();
            if (games.Count > 0)
            {
                int i = 0;
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("Предстоящие игры:");
                foreach (Game game in games)
                {
                    stringBuilder.AppendLine($"{++i}) {game.ToString()}");
                    foreach (Player player in game.Players) stringBuilder.AppendLine("     " + player.ToTgHtmlLink());
                }
                botClient.SendTextMessageAsync(message.Chat.Id, stringBuilder.ToString(), parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
            }
            else botClient.SendTextMessageAsync(message.Chat.Id, "Нет предстоящих игр");
        }
    }
}
