namespace BusinessLayer.Dtos.User
{

    public class UserDto2
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string? imagePath { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Role { get; set; }
        public int ParticipationID { get; set; }

    }
}
