//using TournamentPlanner.API.Data.Repositories;
//using TournamentPlanner.API.Data;

//namespace TournamentPlanner.API.Services
//{
//    public class PlayerService : IPlayerService
//    {
//        private readonly IPlayerRepository _playerRepository;

//        public PlayerService(IPlayerRepository playerRepository)
//        {
//            _playerRepository = playerRepository;
//        }

//        public async Task<IEnumerable<Player>> GetAllPlayersAsync()
//        {
//            return await _playerRepository.GetAllPlayersAsync();
//        }

//        public async Task<Player> GetPlayerByIdAsync(int id)
//        {
//            return await _playerRepository.GetPlayerByIdAsync(id);
//        }

//        public async Task CreatePlayerAsync(Player player)
//        {
//            await _playerRepository.CreatePlayerAsync(player);
//        }

//        public async Task UpdatePlayerAsync(Player player)
//        {
//            await _playerRepository.UpdatePlayerAsync(player);
//        }

//        public async Task DeletePlayerAsync(int id)
//        {
//            await _playerRepository.DeletePlayerAsync(id);
//        }
//    }
//}
