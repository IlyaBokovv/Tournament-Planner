using Microsoft.AspNetCore.Mvc;
using TournamentPlanner.API.Data.Sql.Models;
using TournamentPlanner.DTOs;

namespace TournamentPlanner.API.Services
{
    public interface IMatchService
    {
        Task<IEnumerable<MatchDTO>> GetAllMatchesAsync();
        Task<MatchDTO> GetMatchByIdAsync(int id);
        Task<MatchDTO> CreateMatchAsync(MatchForCreationDto match);
        Task<IActionResult> UpdateMatchAsync(int id, MatchForUpdateDTO match);
        Task DeleteMatchAsync(int id);
        Task<IEnumerable<MatchDTO>> GetIncompleteMatches();
        Task GenerateMatchesForNextRound();

    }
}