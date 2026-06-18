using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace businessLayer_GP.Dtos.Notification
{
    public class GetNotificationDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? SenderId { get; set; }
        public string? SenderName { get; set; }

        public string Type { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }

        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }

        public int? ProjectId { get; set; }
        public string? ProjectName { get; set; }
    }




}
