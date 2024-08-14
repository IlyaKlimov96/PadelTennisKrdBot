using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using System.Linq.Expressions;
using Telegram.Bot.Types.ReplyMarkups;
using System.Globalization;

namespace PadelTennisKrdBot.Commands
{
    internal class CreateGame : TgBotCommand.TgBotCommand
    {
        private bool _isWaitingDate = false;
        private bool _isWaitingTime = false;
        private bool _isWaitingCourtNumber = false;
        private Game? _game;
        private DateTime? _date;

        public override string Command => "/creategame";

        public override async Task Handle(ITelegramBotClient botClient, Message message)
        {
            if (CheckCommand(botClient, message))
            {
                botClient.SendTextMessageAsync(message.Chat.Id, "Пришлите дату (в формате дд.мм)");
                _isWaitingDate = true;
            }
            else if (_isWaitingDate)
            {
                _isWaitingDate = false;
                if (DateTime.TryParse(message.Text + "." + DateTime.Now.Year, CultureInfo.CreateSpecificCulture("ru-RU"), out DateTime date))
                {
                    botClient.SendTextMessageAsync(message.Chat.Id, "Пришлите время (в формате чч:мм)");
                    _date = date;
                    _isWaitingTime = true;
                }
                else botClient.SendTextMessageAsync(message.Chat.Id, "Не удалось распознать дату"); 
            }
            else if (_isWaitingTime)
            {
                _isWaitingTime = false;
                if (TimeOnly.TryParse(message.Text, out TimeOnly time))
                {
                    _date = _date!.Value.Add(time.ToTimeSpan());
                    _game = new Game(_date.Value);
                    botClient.SendTextMessageAsync(message.Chat.Id, "Пришлите номер корта");
                    _isWaitingCourtNumber = true;
                }
                else botClient.SendTextMessageAsync(message.Chat.Id, "Не удалось распознать время");
            }
            else if (_isWaitingCourtNumber)
            {
                _isWaitingCourtNumber = false;
                if (byte.TryParse(message.Text, out byte courtNumber))
                {
                    _game!.CourtNumber = courtNumber;
                    using (PadelTennisDbContext context = await AppData.PadelDbContextFactoty.CreateDbContextAsync())
                    {
                        context.Games.Add(_game);
                        try
                        {
                            await context.SaveChangesAsync();
                        }
                        catch
                        {
                            botClient.SendTextMessageAsync(message.Chat.Id, "Ошибка БД");
                            Reset();
                            return;
                        }
                    }
                    AlarmManager.AddAlarm(_game);
                    InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup
                        (InlineKeyboardButton.WithCallbackData("Иду!", "/confirm" + _game.Id));
                    botClient.SendTextMessageAsync(message.Chat.Id, "Игра создана");
                    botClient.SendTextMessageAsync(AppData.ChatId,
                        $"Корт #{courtNumber} забронирован на {_date!.Value.ToString("dd.MM HH:mm")}. Жми \"Иду\", чтобы подтвердить своё участие",
                        replyMarkup: inlineKeyboard);
                }
                else botClient.SendTextMessageAsync(message.Chat.Id, "Не удалось распознать номер корта");
                Reset();
            }
            else Succsessor?.Handle(botClient, message);
        }

        private void Reset()
        {
            _isWaitingDate = false;
            _isWaitingTime = false;
            _isWaitingCourtNumber = false;
            _game = null;
            _date = null;
        }
    }
}
