using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadelTennisKrdBot
{
    public class Game
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public byte CourtNumber { get; set; }
        public List<Player> Players { get; set; } = new List<Player>();

        public Game(DateTime date) => Date = date;

        public override string ToString() => $"{Date:dd.MM HH:mm (dddd)} - корт #{CourtNumber}";
    }
}
