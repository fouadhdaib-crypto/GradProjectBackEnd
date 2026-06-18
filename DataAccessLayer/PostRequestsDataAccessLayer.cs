using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using static DataAccessLayer.PostRepositoryDataAccess;

namespace DataAccessLayer
{
    public class PostRequestsDataAccessLayer
    {


        private readonly AppDbContext _context;
     
         private readonly UserDataAccess _UserDataAccess ;

        public PostRequestsDataAccessLayer(AppDbContext context)
        {
            _context = context;

            _UserDataAccess = new UserDataAccess(context);


        }


        public async Task<bool> UpdateStatus(int participationId, string  status)
        {


            var request = await _context.Participations.FirstOrDefaultAsync(x=>x.Id== participationId);


            if (request == null) { 
            
            return false;
            }

            request.Status = status;
            request.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> subscribeToProject(int UserID, int ProjectID)
        {



            var entity = new EnrollProjectsUsers
            {
                porjectID = ProjectID,
                userID = UserID
                 ,
                PojectRole = "subscriber"

            };


            await _context.EnrollProjectsUsers.AddAsync(entity);
            var Result = await _context.SaveChangesAsync();

            if (Result > 0)
            {


                return true;


            }


            return false;

        }
        public async Task<bool> UnsubscribeToProject(int UserID, int Projectid)
        {






            var Result  =   await _context.EnrollProjectsUsers
                .Where(c=>c.userID== UserID && c.porjectID == Projectid).
                ExecuteDeleteAsync();
       

            if (Result > 0)
            {


                return true;


            }


            return false;

        }

        public async Task<bool> SendPostRequestToManagerAsync(CreateParticipationDto Dto)
        {



            var Entity = new Participation
            {

                ProjectId = Dto.ProjectId,
                Status = "Pending",
                UserId = Dto.userID,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,


            };



            await _context.Participations.AddAsync(Entity);


            var RequestT = await _context.SaveChangesAsync();

            if (RequestT > 0)
            {

                return true;

            }
            return false;




        }



        public async Task<List<UserDTO>> GetAllsubscribersByProjectID(int ID) {



            List<UserDTO> Users = new List<UserDTO> ();
            var subscribersIds = await _context.Participations.
                Where(x=>x.ProjectId== ID)
                .Select(x=>x.UserId)
                .Distinct().ToListAsync();


            foreach (var item in subscribersIds)
            {
                Users.Add(await _UserDataAccess.getUserByid(item));
            }


            return Users;


        }
        public async Task<int?> GetUserMangerIdByProjectID(int ProjectID)
        {


            var userId = await _context.EnrollProjectsUsers
                .Where(x => x.porjectID == ProjectID && x.PojectRole == "Manger")
                .Select(x => x.userID)
                .FirstOrDefaultAsync();





            return userId;






        }

        public async Task<bool> DeleteProjectByIdAsync(int projectId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var project = await _context.EnrollProjects
                    .Include(p => p.EnrollUsers)       // ⚠️ Restrict — must delete manually
                    .Include(p => p.Team)
                        .ThenInclude(t => t.Tasks)     // Cascade but included for safety
                    .FirstOrDefaultAsync(p => p.ProjectID == projectId);

                if (project == null)
                    return false;

                // 1️⃣ Delete EnrollProjectsUsers FIRST (Restrict = won't auto-cascade)
                if (project.EnrollUsers != null && project.EnrollUsers.Any())
                    _context.EnrollProjectsUsers.RemoveRange(project.EnrollUsers);

                // 2️⃣ Delete Notifications that reference this project
                var notifications = await _context.Notifications
                    .Where(n => n.ProjectId == projectId)
                    .ToListAsync();

                if (notifications.Any())
                    _context.Notifications.RemoveRange(notifications);

                // 3️⃣ Team + Tasks are Cascade — but if you want to be explicit:
                if (project.Team != null)
                {
                    if (project.Team.Tasks != null && project.Team.Tasks.Any())
                        _context.TeamTasks.RemoveRange(project.Team.Tasks);

                    _context.Teams.Remove(project.Team);
                }

                // 4️⃣ Participations are Cascade — explicit for clarity:
                var participations = await _context.Participations
                    .Where(p => p.ProjectId == projectId)
                    .ToListAsync();

                if (participations.Any())
                    _context.Participations.RemoveRange(participations);

                // 5️⃣ Finally delete the project itself
                _context.EnrollProjects.Remove(project);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Delete failed for project {projectId}: {ex.Message}");
            }
        }
    }


}

