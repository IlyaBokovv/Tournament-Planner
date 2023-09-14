using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TournamentPlanner.API.Data;
using TournamentPlanner.API.Data.Sql.Models;
using TournamentPlanner.API.Services;
using TournamentPlanner.DTOs;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TournamentPlanner.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private readonly IMatchService _matchService;

        public MatchesController(IMatchService matchService)
        {
            _matchService = matchService;
        }
        [HttpGet]
        [Route("open")]
        public async Task<ActionResult<IEnumerable<Match>>> GetIncompleteMatches()
        {
            var matches = await _matchService.GetIncompleteMatches();
            return Ok(matches);
        }

        [HttpPost]
        [Route("/generate")]
        public async Task GenerateRound()
        {
            await _matchService.GenerateMatchesForNextRound();
        }
        ь
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Match>>> GetMatches()
        {
            var matches = await _matchService.GetAllMatchesAsync();
            return Ok(matches);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Match>> GetMatch(int id)
        {
            var match = await _matchService.GetMatchByIdAsync(id);

            if (match == null)
            {
                return NotFound();
            }
            var matchDTO = new MatchDTO(match.Id, match.RoundNumber);

            return Ok(matchDTO);
        }

        [HttpPost]
        public async Task<ActionResult<Match>> CreateMatch(MatchForCreationDto match)
        {
            if (match is null)
                return BadRequest();

            var createdCompany = await _matchService.CreateMatchAsync(match);
            return CreatedAtAction(nameof(GetMatch), new { id = createdCompany.Id }, createdCompany);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMatch(int id, MatchForUpdateDTO match)
        {
            if (match is null)
                return BadRequest("CompanyForUpdateDto object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            await _matchService.UpdateMatchAsync(id, match);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatch(int id)
        {
            await _matchService.DeleteMatchAsync(id);
            return NoContent();
        }
    }
}
