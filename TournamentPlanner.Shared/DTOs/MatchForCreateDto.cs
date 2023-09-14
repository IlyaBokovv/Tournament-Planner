namespace TournamentPlanner.DTOs
{
    public record MatchForCreateDto
    {
        public int RoundNumber { get; set; }
        public int Player1Id { get; set; }
        public int Player2Id { get; set; }
        public int? WinnerId { get; set; }
    }

}
