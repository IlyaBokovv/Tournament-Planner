using Microsoft.AspNetCore.Mvc;
using TournamentPlanner.DTOs;
using TournamentPlanner.Services.Interface;

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
        public async Task<ActionResult<IEnumerable<MatchDTO>>> GetMatches()
        {
            var matches = await _matchService.GetAllMatchesAsync();
            return Ok(matches);
        }

        [HttpGet]
        [Route("open")]
        public async Task<ActionResult<IEnumerable<MatchDTO>>> GetIncompleteMatches()
        {
            var matches = await _matchService.GetIncompleteMatches();
            return Ok(matches);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MatchDTO>> GetMatch(int id)
        {
            var match = await _matchService.GetMatchByIdAsync(id);
            var matchDTO = new MatchDTO(match.Id, match.RoundNumber);

            return Ok(matchDTO);
        }

        [HttpPost]
        public async Task<ActionResult<MatchDTO>> CreateMatch(MatchForCreateDto match)
        {
            if (match is null)
                return BadRequest();

            var createdCompany = await _matchService.CreateMatchAsync(match);
            return CreatedAtAction(nameof(GetMatch), new { id = createdCompany.Id }, createdCompany);
        }

        [HttpPost]
        [Route("/generate")]
        public async Task GenerateRound()
        {
            await _matchService.GenerateMatchesForNextRound();
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
