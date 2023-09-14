using TournamentPlanner.API.Data.Sql.Models;
using TournamentPlanner.DTOs;

namespace TournamentPlanner.Services.Interface
{
    public interface IMatchService
    {
        Task<IEnumerable<MatchDTO>> GetAllMatchesAsync();
        Task<MatchDTO> GetMatchByIdAsync(int id);
        Task<MatchDTO> CreateMatchAsync(MatchForCreateDto match);
        Task UpdateMatchAsync(int id, MatchForUpdateDTO match);
        Task DeleteMatchAsync(int id);
        Task<IEnumerable<MatchDTO>> GetIncompleteMatches();
        Task GenerateMatchesForNextRound();

    }
}