using PadelTennisKrdBot.Commands;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;


namespace PadelTennisKrdBot
{
    internal static class Handler
    {
        private static Dictionary<long, TgBotCommand.TgBotCommand> _userHandlers = new Dictionary<long, TgBotCommand.TgBotCommand>();
        internal static Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            long userId = update.Type == UpdateType.Message ?
                update.Message!.From!.Id : update.CallbackQuery!.From.Id;
#if DEBUG
            if (userId.ToString() != AppData.AdminId)
            {
                if (update.Type == UpdateType.CallbackQuery)
                    botClient.AnswerCallbackQueryAsync(update.CallbackQuery!.Id);
                botClient.SendTextMessageAsync(userId, "Бот на техобслуживании");
                return Task.CompletedTask;
            }
#endif
            if (!_userHandlers.TryGetValue(userId, out var handler))
            {
                handler = new CreatePoll();
                handler.AddSuccsessor(new Start());
                handler.AddSuccsessor(new CreateGame());
                handler.AddSuccsessor(new DeleteGame());
                handler.AddSuccsessor(new GetGames());
                handler.AddSuccsessor(new Confirm());
                handler.AddSuccsessor(new Nothing());
                _userHandlers.Add(userId, handler);
            }
            handler.Handle(botClient, update);
            return Task.CompletedTask;
        }

        internal static Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
