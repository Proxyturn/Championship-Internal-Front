using Championship_Internal_Front.Enums;
using System.ComponentModel.DataAnnotations;

namespace Championship_Internal_Front.Models
{
    public class ChampionshipInternal
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? StartDate { get; set; }
        public int TotalPhases { get; set; }
        public Guid WinnerTeam { get; set; }
        public Guid SecondTeam { get; set; }
        public Guid ThirdTeam { get; set; }
        public ChampionshipStatusEnum Status { get; set; }
    }
}
