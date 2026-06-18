using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace businessLayer_GP.Dtos.Notification
{
    public class CreateNotificationDto
    {
        public int? UserId { get; set; }
        public int? SenderId { get; set; }

        public string Type { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }

        public int? ProjectId { get; set; }
    }

}
