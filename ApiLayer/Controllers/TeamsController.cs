using businessLayer_GP;
using DataAccessLayer;
using GradProject.Model;
using GradProject.Services;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using static DataAccessLayer.ClsProjectTeamsDataAaccess;
namespace GradProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {



        private readonly ClsProjectTeamsbusinessLayer _ClsProjectTeamsbusinessLayer;

        private readonly ICurrentUserService _CurrentUserService;
        public TeamsController(ClsProjectTeamsbusinessLayer ClsProjectTeamsbusinessLayer , ICurrentUserService ICurrentUserService)
        {

            _ClsProjectTeamsbusinessLayer = ClsProjectTeamsbusinessLayer;
            _CurrentUserService = ICurrentUserService;
        }




        [HttpGet("GetUserTeams")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public async Task<IActionResult> GetUserTeams() {

            int userid =  _CurrentUserService.UserId;
         var Result =  await  _ClsProjectTeamsbusinessLayer.GetUserTeams(userid);


            if (Result != null) {


               return Ok(Result);



            }

            return BadRequest("User not Found ");

        }

        [HttpGet("GetAllTeamMembersByProjectId")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<IActionResult> GetAllTeamMembersByProjectId(int ProjectId)
        {

            var Result = await _ClsProjectTeamsbusinessLayer.GetAllTeamMembersByProjectId(ProjectId);




            if (ProjectId < 1) {


                return BadRequest(" Project ID is not valid ");
            }


            if (Result != null)
            {


                return Ok(Result);



            }

            return NotFound("no Project Found ");

        }
        [HttpPut("updateTeamMemberRate")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<IActionResult> updateTeamMemberRate(int userId, int projectID, int? Rating)
        {

            var Result = await _ClsProjectTeamsbusinessLayer.updateTeamMemberRate(userId, projectID, Rating);




            if (userId < 1 || projectID <1 || Rating ==null || Rating <1)
            {


                return BadRequest(" data is not valid ");
            }


            if (Result != null)
            {


                return Ok(Result);



            }

            return NotFound("no Project Found ");

        }
        [HttpPost("addTaskByTeamID")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> addTaskByProjectId(TeamTaskDto Model)
        {


            var Teamid = await _ClsProjectTeamsbusinessLayer.GetTeamIDByProjectID(Model.ProjectID);


            Model.ProjectID = Teamid;

            var Result = await _ClsProjectTeamsbusinessLayer.addTaskByTeamID(Model);



            if (Model.ProjectID < 1 ) {

                return BadRequest();
            
            }


            if (Result) { 
            
            return Ok(Result);
            
            }

            return NotFound("No Task For This Team");

        }


        [HttpGet("GetAllTaskByProjectId")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllTaskByProjectId( int ProjectId) {


            var Teamid = await _ClsProjectTeamsbusinessLayer.GetTeamIDByProjectID(ProjectId);


            if (Teamid == 0) { 
            
            return BadRequest($" ProjectId is not Valid ");
            
            }


            var Result = await _ClsProjectTeamsbusinessLayer.GetAllTaskName(Teamid);


            if (Result.Count > 0) {

                return Ok(Result);
            
            }


            return NotFound( "No Task For This Project");
        }


        [HttpDelete("deleteTaskByTaskID")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> deleteTaskByTaskID(int taskId)
        {


       


            if (taskId == 0)
            {

                return BadRequest($" taskId is not Valid ");

            }


            var Result = await _ClsProjectTeamsbusinessLayer.DeleteTaskByTaskId(taskId);


            if (Result)
            {

                return Ok("Task Deleted");

            }


            return NotFound("can'nt Delete Task Try Again Later");
        }


    }
}
