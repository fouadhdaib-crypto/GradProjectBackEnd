using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace DataAccessLayer
{
    public class ClsProjectTeamsDataAaccess
    {


        private readonly AppDbContext _context;

        public ClsProjectTeamsDataAaccess(AppDbContext con)
        {


            _context = con;


        }

        public class UserTeamDTO
        {
            public int ProjectId { get; set; }
            public string Name { get; set; }
            public string Rating { get; set; }
            public string Status { get; set; }
            public string ProjectType { get; set; }
            public int AvailableSeats { get; set; }
            public DateTime UpdatedAt { get; set; }
            public DateTime CreatedAt { get; set; }
            public string Role { get; set; }
            public int UserId { get; set; }
            public string description { get; set; }
        }
        public class TeamTaskDto
        {

            public int ProjectID { get; set; }

            public string TaskName { get; set; }

            public string? Description { get; set; }

            public bool? IsDone { get; set; }

           
        }
        public class TeamTaskDtoForset
        {
            public int taskid { get; set; }

            public int ProjectID { get; set; }

            public string TaskName { get; set; }

            public string? Description { get; set; }

            public bool? IsDone { get; set; }


        }

        public async Task<List<UserTeamDTO>> GetUserTeams(int userId)
        {
            var result = await (
                from p in _context.Participations
                join epu in _context.EnrollProjectsUsers
                    on p.ProjectId equals epu.porjectID
                join ep in _context.EnrollProjects
                    on epu.porjectID equals ep.ProjectID
                
                where epu.userID == userId
        && (epu.PojectRole == "Manger" || p.Status == "approved")
                select new UserTeamDTO
                {
                    ProjectId = ep.ProjectID,
                    Name = ep.Name,
                    Rating = ep.Rating,
                    Status = ep.status,
                    ProjectType = ep.ProjectType,
                    AvailableSeats = ep.numberOFAvailableSeats ?? 0, 
                    Role = epu.PojectRole,
                    UserId = epu.userID ?? 0,
                    description = ep.Descriptions
                    
                }
            ).Distinct().ToListAsync();

            return result;
        }



        public async Task<List<UserDTO>> GetAllTeamMembersByProjectId(int projectId) {



                          var result = await (
                            from epu in _context.EnrollProjectsUsers
                            join user in _context.Users
                                on epu.userID equals user.Id
                            where epu.porjectID == projectId
                            select new UserDTO
                            {
                                Id = user.Id,
                                FullName = user.FullName,
                                imagePath = user.ImagePath,
                                Role = epu.PojectRole
                            }).Distinct().ToListAsync();


                return result;



        }



        public async Task<bool> updateTeamMemberRate(int userId  , int projectID,  int? Rating) {




            var entity = await _context.EnrollProjectsUsers.
                FirstOrDefaultAsync(E => E.porjectID == projectID && E.userID == userId);


            if (entity == null)
                return false;


            entity.Rating = Rating;

            var result = await _context.SaveChangesAsync();

            return result > 0;



        }

        public async Task<bool> addTaskByTeamID(TeamTaskDto  Model) {



            var entity = new TeamTask {
            
            CreatedAt  = DateTime.UtcNow,
            TeamId = Model.ProjectID,
            TaskName = Model.TaskName,
            Description = Model.Description,
            IsDone = false,
   


            };



         await  _context.TeamTasks.AddAsync(entity);
             
            var Result =  await _context.SaveChangesAsync();

            return (Result>0);



        }

        public async Task<int> GetTeamIDByProjectID(int ProjectId) {

            var result = await _context.Teams
        .Where(x => x.ProjectId == ProjectId)
        .Select(x => x.TeamId)
        .FirstOrDefaultAsync();

            return result;


        }

        public async Task<List<TeamTaskDtoForset>> GetAllTaskName(int TeamID)
        {
            var result = await _context.TeamTasks
                .Where(x => x.TeamId == TeamID)
                .Select(x => new TeamTaskDtoForset
                {
                    Description = x.Description,
                    TaskName = x.TaskName,
                    IsDone = x.IsDone,
                    taskid = x.TaskId
                })
                .ToListAsync();

            return result;
        }


        public async Task<bool> DeleteTaskByTaskId(int TaskID)
        {
            var result = await _context.TeamTasks
                .Where(x => x.TaskId == TaskID).ExecuteDeleteAsync();
           
            return result >0;
        }
    }
}
