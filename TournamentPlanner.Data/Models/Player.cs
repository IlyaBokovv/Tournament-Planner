using System.ComponentModel.DataAnnotations;

namespace TournamentPlanner.API.Data
{
    public class Player
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;
    }
}
