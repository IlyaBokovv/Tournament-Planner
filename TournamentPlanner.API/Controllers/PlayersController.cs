using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TournamentPlanner.API.Data;
using TournamentPlanner.Services.Interface;

namespace TournamentPlanner.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayerService _service;
        private readonly IMatchService _matchService;

        public PlayersController(IPlayerService service, IMatchService matchService)
        {
            _service = service;
            _matchService = matchService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
        {
            var players = await _service.GetAllPlayersAsync();
            return Ok(players);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetPlayer(int id)
        {
            var player = await _service.GetPlayerByIdAsync(id);

            if (player == null)
            {
                return NotFound();
            }

            return Ok(player);
        }

        [HttpPost]
        public async Task<ActionResult<Player>> CreatePlayer(Player player)
        {
            await _service.CreatePlayerAsync(player);

            return CreatedAtAction("GetPlayer", new { id = player.Id }, player);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlayer(int id, Player player)
        {
            var currentPlayer = await _service.GetPlayerByIdAsync(id);
            if (id != player.Id)
            {
                return BadRequest();
            }

            if(currentPlayer is null)
            {
                return NotFound();
            }
            await _service.UpdatePlayerAsync(player);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var player = _service.GetPlayerByIdAsync(id);
            if (player == null)
            {
                return NotFound();
            }

            await _service.DeletePlayerAsync(id);
            return NoContent();
            //TODO: FIX DELETE
        }

        [HttpGet("{matchId}/players")]
        public async Task<IActionResult> GetPlayersForMatch(int matchId, string filter = null)
        {
            return Ok();
        }
    }
}
