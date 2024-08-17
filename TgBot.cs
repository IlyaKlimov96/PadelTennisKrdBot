using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace PadelTennisKrdBot
{
    internal static class TgBot
    {
        internal static void CreatePoll()
        {
            DateTime monday = DateTime.Now.GetNextDayOfWeek(DayOfWeek.Monday);
            List<string> days = new List<string>();
            for (int i = 0; i < 7; i++)
            {
                DateTime day = monday.AddDays(i);
                if (day.DayOfWeek == DayOfWeek.Saturday || day.DayOfWeek == DayOfWeek.Sunday)
                {
                    days.Add($"{day.ToString("dd.MM")} {day:dddd)} - утро");
                    days.Add($"{day.ToString("dd.MM")} {day:dddd)} - вечер");
                }
                else days.Add($"{day.ToString("dd.MM")} {day:dddd)}");
            }

            AppData.BotClient.SendPollAsync(AppData.ChatId, "Когда играем?", options: days, allowsMultipleAnswers: true, isAnonymous: false);
        }

        internal static async Task SendReminder(int gameId)
        {
            using PadelTennisDbContext context = await AppData.PadelDbContextFactoty.CreateDbContextAsync();
            Game? game = await context.Games.AsNoTracking().Include(x => x.Players).FirstOrDefaultAsync(x => x.Id == gameId);
            if (game != null) SendReminder(game);
        }

        internal static void SendReminder(Game game)
        {
            foreach (Player player in game.Players)
                AppData.BotClient.SendTextMessageAsync(player.Id, 
                    $"Завтра в {game.Date.ToString("HH:mm")} состоится игра на корте #{game.CourtNumber}");
        }
    }
}
