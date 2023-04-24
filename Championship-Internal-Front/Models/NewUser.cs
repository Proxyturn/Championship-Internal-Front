using Championship_Internal_Front.Enums;

namespace Championship_Internal_Front.Models
{
    public class NewUser
    {
        public string? Name { get; set; }
        public int Age { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public readonly UserEnum UserType = UserEnum.Organizer;
    }
}
