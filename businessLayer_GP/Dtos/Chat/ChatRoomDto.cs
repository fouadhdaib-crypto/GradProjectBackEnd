using BusinessLayer.Dtos.User;

namespace BusinessLayer.Dtos.Chat
{
    public class ChatRoomDto
    {
      
        public string? Name { get; set; }
        public bool IsGroup { get; set; }
        public string? RoomKey { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class ChatRoomDetailDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsGroup { get; set; }
        public string? RoomKey { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<UserDto2> Members { get; set; } = new List<UserDto2>();
    }
}