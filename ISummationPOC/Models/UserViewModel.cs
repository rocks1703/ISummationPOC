using ISummationPOC.Entity;

namespace ISummationPOC.Models
{
    public class UserViewModel : User
    {
        public int Id { get; set; }
        public string UserType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string ProfileImage { get; set; } = "noimage.jpg";
    }
}
