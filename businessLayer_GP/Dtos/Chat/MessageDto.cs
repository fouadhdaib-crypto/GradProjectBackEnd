using BusinessLayer.Dtos.User;
using DataAccessLayer;

namespace BusinessLayer.Dtos.Chat
{
    public class MessageDto
    {
        public int Id { get; set; }
        public int ChatRoomId { get; set; }
        public int SenderId { get; set; }
        public string? Content { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
    }

    public class CreateMessageDto
    {
        public int ChatRoomId { get; set; }
        public int SenderId { get; set; }
        public string? Content { get; set; }
    }

    public class MessageDetailDto
    {
        public int Id { get; set; }
        public int ChatRoomId { get; set; }
        public ChatRoomDto? ChatRoom { get; set; }      // reference من ملف ثاني
        public int SenderId { get; set; }
        public UserDto2? Sender { get; set; }            // reference من ملف ثاني
        public string? Content { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
    }
}