using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataAccessLayer.ClsProjectTeamsDataAaccess;

namespace businessLayer_GP
{
    public class ClsProjectTeamsbusinessLayer
    {
     

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        private readonly ClsProjectTeamsDataAaccess _ClsProjctTeamsDataAaccess;

        public ClsProjectTeamsbusinessLayer(ClsProjectTeamsDataAaccess ClsProjctTeamsDataAaccess) {

            _ClsProjctTeamsDataAaccess = ClsProjctTeamsDataAaccess;




        }
       
        public async Task<List<UserTeamDTO>> GetUserTeams(int userId)
        { 
        
        
        
            return await _ClsProjctTeamsDataAaccess.GetUserTeams(userId);





        }

        public async Task<List<UserDTO>> GetAllTeamMembersByProjectId(int projectId) {



            return await _ClsProjctTeamsDataAaccess.GetAllTeamMembersByProjectId(projectId);


        }

        public async Task<bool> updateTeamMemberRate(int userId, int projectID, int? Rating) {




            return await _ClsProjctTeamsDataAaccess.updateTeamMemberRate(userId,projectID,Rating);

        }

        public async Task<bool> addTaskByTeamID(TeamTaskDto Model) {







            return await _ClsProjctTeamsDataAaccess.addTaskByTeamID(Model);
        }
        public async Task<List<TeamTaskDtoForset>> GetAllTaskName(int TeamID)
        {







            return await _ClsProjctTeamsDataAaccess.GetAllTaskName(TeamID);
        }



        public async Task<int> GetTeamIDByProjectID(int ProjectId)
        {

            return await _ClsProjctTeamsDataAaccess.GetTeamIDByProjectID(ProjectId);

        }

        public async Task<bool> DeleteTaskByTaskId(int TaskID) {

            return await _ClsProjctTeamsDataAaccess.DeleteTaskByTaskId(TaskID);


        }
    }
}
