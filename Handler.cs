using PadelTennisKrdBot.Commands;
using Telegram.Bot;
using Telegram.Bot.Types;


namespace PadelTennisKrdBot
{
    internal static class Handler
    {
        private static Dictionary<string, TgBotCommand.TgBotCommand> _userHandlers = new Dictionary<string, TgBotCommand.TgBotCommand>();
        internal static Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            string userId = update.Type == Telegram.Bot.Types.Enums.UpdateType.Message ? update.Message!.From!.Id.ToString() : update.CallbackQuery!.From.Id.ToString();

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
