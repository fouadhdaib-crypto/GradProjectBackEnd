using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataAccessLayer.PostRepositoryDataAccess;
using static DataAccessLayer.PostRequestsDataAccessLayer;

namespace businessLayer_GP
{
    public class PostRequestsBusinessLayer
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode { get; set; }

        public enum enStatus { Pending, rejected, approved };

        public enStatus Status { get; set; }

        PostRequestsDataAccessLayer _DataAccsees;

        private readonly AppDbContext _context;
        public PostRequestsBusinessLayer(AppDbContext con)
        {

            _context = con;

            _DataAccsees = new PostRequestsDataAccessLayer(con);

        }

        public async Task<bool> subscribeToProject(int UserID, int ProjectID)
        {





            return await _DataAccsees.subscribeToProject(UserID, ProjectID);


        }
        public async Task<bool> UnsubscribeToProject(int UserID, int Projectid) {



            return await _DataAccsees.UnsubscribeToProject(UserID, Projectid);


        }
        public async Task<bool> SendPostRequestToManagerAsync(CreateParticipationDto Dto)
        {


            return await _DataAccsees.SendPostRequestToManagerAsync(Dto);
        }



        public async Task<bool> UpdateStatus(int participationId)
        {

            switch (Status)
            {
                case enStatus.Pending:

                    if (await _DataAccsees.UpdateStatus(participationId, "Pending"))
                        return true;
                    break;
                case enStatus.rejected:

                    if (await _DataAccsees.UpdateStatus(participationId, "rejected"))
                        return true;
                    break;
                case enStatus.approved:

                    if (await _DataAccsees.UpdateStatus(participationId, "approved"))
                        return true;
                    break;

            }
            return false;
        }


        public async Task<int?> GetUserMangerIdByProjectID(int ProjectID)
        {







            return await _DataAccsees.GetUserMangerIdByProjectID(ProjectID);






        }


        public async Task<bool> DeleteProjectByIdAsync(int projectId)
        {


            return await _DataAccsees.DeleteProjectByIdAsync(projectId);



        }




    }
}
