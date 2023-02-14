using UserListBackend.Models.DataModels.Enum;

namespace UserListBackend.Models.DataModels
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public Gender Gender { get; set; }
    }
}

