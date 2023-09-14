using Microsoft.EntityFrameworkCore;
using TournamentPlanner.API.Data.Sql.Models;

namespace TournamentPlanner.Data.IRepository
{
    public interface IMatchRepository : IRepositoryBase<Match>
    {
        Task<IEnumerable<Match>> GetAllMatchesAsync();
        Task<IEnumerable<Match>> GetIncompleteMatchesAsync();
        Task GenerateMatchesForNextRoundAsync();
        Task AddSubsequentRound(DbSet<Match> matches);
        Task<int> GetMatchesCountAsync();
    }
}