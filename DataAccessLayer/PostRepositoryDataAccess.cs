using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace DataAccessLayer
{
    public class PostRepositoryDataAccess
    {





     
     
        private readonly AppDbContext _context;

        public PostRepositoryDataAccess(AppDbContext context)
        {
            _context = context;
            
        }
 
        private readonly SignInManager<ApplicationUser> _signInManager;

        public class PostProjectDTO
        {
            public int ProjectID { get; set; }
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
            public int UserId { get; set; }

            public PostProjectDTO() { }

            // Constructor
            public PostProjectDTO(
                int projectID,
                string name,
                string? descriptions,
                string? rating,
                bool isGraduationProject,
                DateTime? endDate,
                string? skills,
                int? availableSeats,
                string? projectLocation,
                string? teamType,
                int? numberOfAvailableSeats)
            {
                ProjectID = projectID;
                Name = name;
                Descriptions = descriptions;
                Rating = rating;
                IsGraduationProject = isGraduationProject;
                EndDate = endDate;
                Skills = skills;
                AvailableSeats = availableSeats;
                ProjectLocation = projectLocation;
                TeamType = teamType;
                NumberOfAvailableSeats = numberOfAvailableSeats;
            }
        }


        public class CreateParticipationDto
        {
            public int ProjectId { get; set; }
            public int userID { get; set; }
        }
        public class UpdateParticipationStatusDto
        {
            public string Status { get; set; } // "Approved" أو "Rejected"

            public int ProjectId { get; set; }


        }

        public class ParticipationDto
        {
            public int Id { get; set; }

            public int UserId { get; set; }
            public string UserName { get; set; }

            public int ProjectId { get; set; }
            public string ProjectName { get; set; }

            public string Status { get; set; }

            public DateTime CreatedAt { get; set; }
        }

        public async Task<bool> AddProject(PostProjectDTO dto)
        {
            var entity = new EnrollProject
            {
                ProjectID = dto.ProjectID,
                Name = dto.Name,
                Descriptions = dto.Descriptions,
                Rating = dto.Rating,
                isGraduationProject = dto.IsGraduationProject,
                EndDate = dto.EndDate,
                Skills = dto.Skills,
                AvailableSeats = dto.AvailableSeats,
                ProjectLocation = dto.ProjectLocation,
                TeamType = dto.TeamType,
                numberOFAvailableSeats = dto.NumberOfAvailableSeats
            };


       

            var AddProject = await _context.EnrollProjects.AddAsync(entity);

            var Result = await _context.SaveChangesAsync();

              if (Result > 0) {


                var entity2 = new Team
                {
                    Name = dto.TeamType,
                    ProjectId = entity.ProjectID
                };

                var AddTeam = await _context.Teams.AddAsync(entity2);
                await _context.SaveChangesAsync();


            }


                if (Result >0) {



                entity.EnrollUsers = new List<EnrollProjectsUsers>
                    {
                          new EnrollProjectsUsers
                          {
                              porjectID = entity.ProjectID,
                              userID = dto.UserId 
                              ,PojectRole = "Manger"

                          }
                    };


            

                await _context.EnrollProjectsUsers.AddRangeAsync(entity.EnrollUsers);
                await _context.SaveChangesAsync();
              


            }



           

            return Result > 0;

        }

        public async Task<bool> DeleteProject(int ProjectID)
        {

            var Result = await _context.EnrollProjects.
                Where(x => x.ProjectID == ProjectID).
                ExecuteDeleteAsync();    

           return Result >0;
        }


        public async Task<bool> update(PostProjectDTO dto )
        {

            var Result = await _context.EnrollProjects
              .Where(p => p.ProjectID == dto.ProjectID)
              .ExecuteUpdateAsync(setters => setters
                  .SetProperty(p => p.Name, dto.Name)
                  .SetProperty(p => p.Descriptions, dto.Descriptions)
                  .SetProperty(p => p.Rating, dto.Rating)
                  .SetProperty(p => p.isGraduationProject, dto.IsGraduationProject)
                  .SetProperty(p => p.EndDate, dto.EndDate)
                  .SetProperty(p => p.Skills, dto.Skills)
                  .SetProperty(p => p.AvailableSeats, dto.AvailableSeats)
                  .SetProperty(p => p.ProjectLocation, dto.ProjectLocation)
                  .SetProperty(p => p.TeamType, dto.TeamType)
                  .SetProperty(p => p.numberOFAvailableSeats, dto.NumberOfAvailableSeats)
              );


           

            return Result > 0;

        }



        public async Task<PostProjectDTO> GetProjectById(int ProjectID) {



            var Project = await _context.EnrollProjects.FindAsync(ProjectID);


            PostProjectDTO postProjectDTO = new PostProjectDTO
            {
                ProjectID = Project.ProjectID,
                Name = Project.Name,
                Descriptions = Project.Descriptions,
                Rating = Project.Rating,
                IsGraduationProject = Project.isGraduationProject,
                EndDate = Project.EndDate,
                Skills = Project.Skills,
                AvailableSeats = Project.AvailableSeats,
                ProjectLocation = Project.ProjectLocation,
                TeamType = Project.TeamType,
                NumberOfAvailableSeats = Project.numberOFAvailableSeats
            };



            return postProjectDTO;
        }


        public async Task <List<PostProjectDTO>> GetAllProject_Post()
        {



            var Project = await _context.EnrollProjects.ToListAsync();

            var result = Project.Select(x => new PostProjectDTO
            {
                ProjectID =x.ProjectID,
                Name = x.Name,
                Descriptions = x.Descriptions,
                Rating = x.Rating,
                IsGraduationProject = x.isGraduationProject,
                EndDate = x.EndDate,
                Skills = x.Skills,
                ProjectLocation = x.ProjectLocation,
                TeamType = x.TeamType,
                AvailableSeats = x.AvailableSeats,
                NumberOfAvailableSeats = x.numberOFAvailableSeats
            }).ToList();



            return result;
        }


        public async Task<UserDTO> GetUserByProjectID(int ProjectID)
        {



            var USer = await _context.EnrollProjectsUsers.
                Where(x => x.porjectID == ProjectID&&x.PojectRole== "Manger").
                Select(x => x.userID).ToListAsync();


            UserDataAccess UserDA = new UserDataAccess(_context);



            var userID = USer.FirstOrDefault();

              return   await  UserDA.getUserByid(userID);




        }



    







      

    }
}
