using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TournamentPlanner.API.Data.Sql.Models
{
    public class Match
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(1, 5)]
        public int RoundNumber { get; set; }

        [Required]
        public int Player1Id { get; set; }

        [Required]
        public int Player2Id { get; set; }

        public int? WinnerId { get; set; }

        public Player Player1 { get; set; } = null!;

        public Player Player2 { get; set; } = null!;

        public Player? Winner { get; set; }
    }
}
