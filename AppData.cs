using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TimeHelper;

namespace PadelTennisKrdBot
{
    internal static class AppData
    {
        private static string _botName = null!;

        internal static string ChatId { get; } = ConfigurationManager.AppSettings.Get("Chat")!;
        internal static DayOfWeek DayOfPoll { get; } = (DayOfWeek)int.Parse(ConfigurationManager.AppSettings.Get("DayOfPoll")!);
        internal static string AdminId { get; } = ConfigurationManager.AppSettings.Get("Admin")!;
        internal static ITelegramBotClient BotClient { get; } = new TelegramBotClient(ConfigurationManager.AppSettings.Get("BotToken")!);
#if DEBUG
        internal static string ConnectionString { get; } = "Data Source=D:\\SQLite\\PadelTennis.db;Mode=ReadWrite;";
#else
        internal static string ConnectionString { get; } = ConfigurationManager.AppSettings.Get("ConnectionString")!;
#endif
        internal static PooledDbContextFactory<PadelTennisDbContext> PadelDbContextFactoty { get; }
        internal static string BotName
        {
            get
            {
                if (_botName is null) _botName = BotClient.GetMeAsync().Result.Username!;
                return _botName;
            }
        }

        static AppData()
        {
            DbContextOptions<PadelTennisDbContext> options = new DbContextOptionsBuilder<PadelTennisDbContext>().UseSqlite(ConnectionString).Options;
            PadelDbContextFactoty = new PooledDbContextFactory<PadelTennisDbContext>(options, 10);
        }
    }
}
