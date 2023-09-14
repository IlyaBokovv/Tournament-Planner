using Contracts;
using Microsoft.EntityFrameworkCore;
using Repository;
using TournamentPlanner.API.Data.Sql.Models;

namespace TournamentPlanner.API.Data.Repositories
{
    public class MatchRepository : RepositoryBase<Match>, IMatchRepository
    {
        private readonly TournamentPlannerDbContext _db;

        public MatchRepository(TournamentPlannerDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Match>> GetIncompleteMatchesAsync()
        {
            return await FindByCondition(x => x.Winner == null).ToListAsync();
        }

        public async Task<IEnumerable<Match>> GetAllMatchesAsync()
        {
            return await _db.Matches.ToListAsync();
        }

        public async Task GenerateMatchesForNextRoundAsync()
        {
            using var transaction = await _db.Database.BeginTransactionAsync();

            if ((await GetIncompleteMatchesAsync()).Any())
                throw new InvalidOperationException("Every match MUST have a winner before generating next round matches");

            var players = await _db.Players.ToListAsync();
            if (players.Count != 32) throw new InvalidOperationException("Incorrect number of players");

            var numberOfMatches = await _db.Matches.CountAsync();
            switch (numberOfMatches)
            {
                case 0:
                    AddFirstRound(_db.Matches, players);
                    break;
                case 16 or 24 or 28 or 30:
                    await AddSubsequentRound(_db.Matches);
                    break;
                default:
                    throw new InvalidOperationException("Invalid number of rounds");
            }

            await _db.SaveChangesAsync();
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
        }
        public async Task AddSubsequentRound(DbSet<Match> matches)
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
                    Player1 = match1.Winner!,
                    Player2 = match2.Winner!,
                    RoundNumber = nextRound
                });
            }
        }
    }
}
