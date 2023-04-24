using System.ComponentModel.DataAnnotations;

namespace Championship_Internal_Front.Models
{
    public class NewChampionship
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }
        public int TotalPhases { get; set; }
    }
}
