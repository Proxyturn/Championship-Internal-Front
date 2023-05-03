using Championship_Internal_Front.Enums;

namespace Championship_Internal_Front.Models
{
    public class EditedUser
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int Age { get; set; }
        public string? Email { get; set; }
    }
}
