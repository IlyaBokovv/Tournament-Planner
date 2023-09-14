using Microsoft.EntityFrameworkCore;
using TournamentPlanner.Data.IRepository;

namespace TournamentPlanner.API.Data.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly TournamentPlannerDbContext _db;

        public PlayerRepository(TournamentPlannerDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Player>> GetAllPlayersAsync()
        {
            return await _db.Players.ToListAsync();
        }

        public async Task<Player> GetPlayerByIdAsync(int id)
        {
            return await _db.Players.FindAsync(id);
        }

        public async Task CreatePlayerAsync(Player player)
        {
            _db.Players.Add(player);
            await _db.SaveChangesAsync();
        }

        public async Task UpdatePlayerAsync(Player player)
        {
            _db.Players.Update(player);
            await _db.SaveChangesAsync();
        }

        public async Task DeletePlayerAsync(int id)
        {
            var player = await _db.Players.FindAsync(id);
            if (player != null)
            {
                _db.Players.Remove(player);
                await _db.SaveChangesAsync();
            }
        }
    }
}
