using System.ComponentModel.DataAnnotations;

namespace Championship_Internal_Front.Models
{
    public class NewChampionship
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? StartDate { get; set; }
        public int TotalPhases { get; set; }
    }
}
