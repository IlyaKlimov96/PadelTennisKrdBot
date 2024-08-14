using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace PadelTennisKrdBot
{
    internal class PadelTennisDbContext : DbContext
    {
        public virtual DbSet<Game> Games { get; set; } = null!;
        public virtual DbSet<Player> Players { get; set; } = null!;

        public PadelTennisDbContext (DbContextOptions options) : base (options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>().ToTable("Games").ToView("CurrentGames");
        }
    }
}
