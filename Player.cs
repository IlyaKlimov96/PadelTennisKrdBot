using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace PadelTennisKrdBot
{
    public class Player
    {
        public long Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public List<Game> Games { get; set; } = new List<Game>();
        public string? UserName { get; set; }

        public Player(long id) => Id = id;

        public Player(User user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            UserName = user.Username;
        }

        public string FullName => FirstName + (LastName is null ? string.Empty : " " + LastName);

        public string ToTgHtmlLink() => $"<a href=\"tg://user?id={Id}\">{FullName}</a>";
    }
}
