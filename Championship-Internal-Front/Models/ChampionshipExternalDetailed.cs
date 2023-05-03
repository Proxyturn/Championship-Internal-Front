using Championship_Internal_Front.Enums;

namespace Championship_Internal_Front.Models
{
    public class ChampionshipExternalDetailed
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set;}
        public int Subscription { get; set; }
        public int TotalPhases { get; set; }
        public ChampionshipStatusEnum Status { get; set; }
        public List<Team>? Ranking { get; set; }
        public List<Match>? Matchs { get; set; }
    }
}
