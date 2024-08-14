using TimeHelper;

namespace PadelTennisKrdBot
{
    internal static class AlarmManager
    {
        internal static List<Alarm> Alarms { get; } = new List<Alarm>();

        internal static void AddAlarm(Game game)
        {
            if (game.Date.AddDays(-1) >= DateTime.Now) return;
            Alarm alarm = new Alarm(game.Date.AddDays(-1), game.Id);
            alarm.TimeHasCome += (object sender, DateTime date, object? state) =>
            {
                TgBot.SendReminder((int)state!);
                Alarms.Remove(alarm);
            };
            Alarms.Add(alarm);
        }
    }
}
