using Championship_Internal_Front.Enums;

namespace Championship_Internal_Front.Models
{
    public class Match
    {
        public Guid IdMatch { get; set; }
        public string? Name { get; set; }
        public int PhaseNumber { get; set; }
        public string? StartDate { get; set; }
        public int TotalTickets { get; set; }
        public int SoldTickets { get; set; }
        public string? RefereeName { get; set; }
        public Guid IdTeamA { get; set; }
        public string? TeamAName { get; set; }
        public Guid IdTeamB { get; set; }
        public string? TeamBName { get; set; }
        public MatchStatusEnum Status { get; set; }
    }
}
