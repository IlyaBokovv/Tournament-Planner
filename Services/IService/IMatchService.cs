using TournamentPlanner.DTOs;

namespace Services.IService
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
        void AGAW();

    }
}