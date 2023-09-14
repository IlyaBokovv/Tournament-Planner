using Microsoft.EntityFrameworkCore;
using TournamentPlanner.API.Data.Sql.Models;

namespace TournamentPlanner.API.Data
{
    public class TournamentPlannerDbContext : DbContext
    {
        public TournamentPlannerDbContext(DbContextOptions<TournamentPlannerDbContext> optinons)
            : base(optinons)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Player1)
                .WithMany()
                .HasForeignKey(m => m.Player1Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Player2)
                .WithMany()
                .HasForeignKey(m => m.Player2Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Winner)
                .WithMany()
                .HasForeignKey(m => m.WinnerId)
                .OnDelete(DeleteBehavior.NoAction);

        }

        public DbSet<Match> Matches { get; set; }
        public DbSet<Player> Players { get; set; }
        public async Task<Player> AddPlayer(Player newPlayer)
        {
            Players.Add(newPlayer);
            await SaveChangesAsync();
            return newPlayer;
        }
        public async Task<Match> AddMatch(int player1Id, int player2Id, int roundNumber)
        {
            var match = new Match
            {
                Player1Id = player1Id,
                Player2Id = player2Id,
                RoundNumber = roundNumber
            };
            Matches.Add(match);
            await SaveChangesAsync();
            return match;
        }

        public async Task<Match> SetWinner(int matchId, int playerNumber)
        {
            var match = Matches.Single(m => m.Id == matchId);
            match.WinnerId = playerNumber switch
            {
                1 => match.Player1Id,
                2 => match.Player2Id,
                _ => throw new ArgumentOutOfRangeException(nameof(playerNumber))
            };
            await SaveChangesAsync();
            return match;
        }

        public async Task<IList<Match>> GetIncompleteMatchesAsync()
        {
            return await Matches.Where(m => m.Winner == null).ToListAsync();
        }

        public async Task DeleteEverything()
        {
            using var transaction = await Database.BeginTransactionAsync();
            await Database.ExecuteSqlRawAsync("DELETE FROM dbo.Matches");
            await Database.ExecuteSqlRawAsync("DELETE FROM dbo.Players");
            await transaction.CommitAsync();
        }

        public async Task<IList<Player>> GetFilteredPlayers(string filter = null)
        {
            return await Players.Where(p => filter == null
            || p.Name.Contains(filter)).ToListAsync();
        }
        public async Task GenerateMatchesForNextRoundAsync()
        {
            using var transaction = await Database.BeginTransactionAsync();

            if ((await GetIncompleteMatchesAsync()).Any()) throw new InvalidOperationException("Incomplete matches");

            var players = await GetFilteredPlayers();
            if (players.Count != 32) throw new InvalidOperationException("Incorrect number of players");

            var numberOfMatches = await Matches.CountAsync();
            switch (numberOfMatches)
            {
                case 0:
                    AddFirstRound(Matches, players);
                    break;
                case 16 or 24 or 28 or 30:
                    await AddSubsequentRound(Matches);
                    break;
                default:
                    throw new InvalidOperationException("Invalid number of rounds");
            }

            await SaveChangesAsync();
            await transaction.CommitAsync();

        static void AddFirstRound(DbSet<Match> matches, IList<Player> players)
        {
            var rand = new Random();
            for (var i = 0; i < 16; i++)
            {
                var player1 = players[rand.Next(players.Count)];
                players.Remove(player1);
                var player2 = players[rand.Next(players.Count)];
                players.Remove(player2);
                matches.Add(new Match
                {
                    Player1 = player1,
                    Player2 = player2,
                    RoundNumber = 1
                });
            }
        }

        static async Task AddSubsequentRound(DbSet<Match> matches)
        {
            var rand = new Random();

            var prevRound = await matches.MaxAsync(m => m.RoundNumber);
            var prevRoundMatches = await matches.Where(m => m.RoundNumber == prevRound).ToListAsync();
            var nextRound = prevRound + 1;
            for (var i = prevRoundMatches.Count / 2; i > 0; i--)
            {
                var match1 = prevRoundMatches[rand.Next(prevRoundMatches.Count)];
                prevRoundMatches.Remove(match1);
                var match2 = prevRoundMatches[rand.Next(prevRoundMatches.Count)];
                prevRoundMatches.Remove(match2);
                matches.Add(new Match
                {
                    Player1 = match1.Winner,
                    Player2 = match2.Winner,
                    RoundNumber = nextRound
                });
            }
        }
        }
    }
}
