using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace businessLayer_GP.Dtos.User
{
    public class ProfileInfoDTO
    {
        public string? Description { get; set; }
        public string? Skills { get; set; }
        public int? Rating { get; set; }
        public int UserId { get; set; }
        public string? RoleName { get; set; }
        public string? getHubUrl { get; set; }
        public int collageID { get; set; }
        public string? workField { get; set; }
        public ProfileInfoDTO() { }

        public ProfileInfoDTO(string? description, string? skills, int? rating, int userId, string? roleName, int _collageID, string? workField)
        {
            Description = description;
            Skills = skills;
            Rating = rating;
            UserId = userId;
            RoleName = roleName;
          
            this.workField = workField;
        }
    }
}
