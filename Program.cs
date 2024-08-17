using Telegram.Bot;
using PadelTennisKrdBot;
using System.Configuration;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Polling;
using TimeHelper;
using PadelTennisKrdBot.Commands;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using System.Globalization;

class Program
{
    static async Task Main()
    {
        CultureInfo.CurrentCulture = new CultureInfo("ru-RU");

        Alarm alarm = new Alarm(DateTime.Now.GetNextDayOfWeek(AppData.DayOfPoll).AddHours(12), null, TimeSpan.FromDays(7));
        alarm.TimeHasCome += (object sender, DateTime date, object? state) => TgBot.CreatePoll();

        using (PadelTennisDbContext context = await AppData.PadelDbContextFactoty.CreateDbContextAsync())
            foreach (PadelTennisKrdBot.Game game in context.Games.AsNoTracking().Include(x => x.Players))
                AlarmManager.AddAlarm(game);

        ReceiverOptions options = new ReceiverOptions()
        {
            ThrowPendingUpdates = true,
            AllowedUpdates = new[]
            {
                UpdateType.CallbackQuery,
                UpdateType.Message
            }
        };
        await AppData.BotClient.ReceiveAsync(Handler.UpdateHandler, Handler.ErrorHandler, options);
    }
}

