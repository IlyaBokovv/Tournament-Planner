using Contracts;
using Microsoft.EntityFrameworkCore;
using TournamentPlanner.API.Data.Sql.Models;

namespace TournamentPlanner.API.Data.Repositories
{
    public interface IMatchRepository : IRepositoryBase<Match>
    {
        Task<IEnumerable<Match>> GetAllMatchesAsync();
        Task<IEnumerable<Match>> GetIncompleteMatchesAsync();
        Task GenerateMatchesForNextRoundAsync();
        Task AddSubsequentRound(DbSet<Match> matches);
    }
}