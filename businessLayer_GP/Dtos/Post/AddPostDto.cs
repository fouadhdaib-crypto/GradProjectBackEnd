using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace businessLayer_GP.Dtos.Post
{
    public class AddPostDto
    {

        public string Name { get; set; }
        public string? Descriptions { get; set; }
        public string? Rating { get; set; }
        public bool? IsGraduationProject { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Skills { get; set; }
        public int? AvailableSeats { get; set; }
        public string? ProjectLocation { get; set; }
        public string? TeamType { get; set; }
        public int? NumberOfAvailableSeats { get; set; }







    }
}
