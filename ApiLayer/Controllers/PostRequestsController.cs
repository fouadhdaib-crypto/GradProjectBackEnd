using businessLayer_GP;
using businessLayer_GP.Dtos.Notification;
using businessLayer_GP.Service;
using DataAccessLayer;
using GradProject.Global;
using GradProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using static businessLayer_GP.PostRequestsBusinessLayer;
using static DataAccessLayer.PostRepositoryDataAccess;
using static DataAccessLayer.UserDataAccess;

namespace GradProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostRequestsController : ControllerBase
    {

        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly PostProjectDTO Dto = new PostProjectDTO();
        private readonly UserbusinessLayer _userB;
        private readonly ICurrentUserService _currentUserService;
        private PostRequestsBusinessLayer _PostRequestsBusinessLayer;

        private readonly INotificationService _notificationService;

        public PostRequestsController(
            AppDbContext context,
            UserManager<ApplicationUser> userManager,
            ICurrentUserService currentUserService,
            INotificationService notificationService)
        {
            _context = context;
            _userManager = userManager;

            _PostRequestsBusinessLayer = new PostRequestsBusinessLayer(context);
            _currentUserService = currentUserService;

            _userB = new UserbusinessLayer(context);
            _notificationService = notificationService;
        }




        [HttpPost("subscribeToProject")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> subscribeToProject(int Projectid ,int userId)
        {
          
        

            bool Result = await _PostRequestsBusinessLayer.subscribeToProject(userId, Projectid);

            // This For send  Notification
            var createNotificationDto = new CreateNotificationDto
            {
                UserId = userId,
                ProjectId = Projectid,
                Message = $"You subscribed To Project Number {Projectid} successfully ",
                SenderId = null,
                Type = "Notification",
                Title = "subscribe"
            };

            if (Result)
            {


                // This For send  Notification
                await _notificationService.AddAsync(createNotificationDto);

                return Ok("User  subscribed successfully");

            }
            return BadRequest("Server erorr");


        }

        [HttpDelete("UnsubscribeToProject")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public async Task<IActionResult> UnsubscribeToProject(int Projectid)
        {
            var USerId = _currentUserService.UserId;


            if (USerId <1 || Projectid  < 1) {


                return BadRequest("User or Project ID are Not found");
            
            }

            bool Result = await _PostRequestsBusinessLayer.UnsubscribeToProject(USerId , Projectid);

            // This For send  Notification
            var createNotificationDto = new CreateNotificationDto
            {
                UserId = USerId,
                ProjectId = Projectid,
                Message = $"You Unsubscribe To Project Number {Projectid} successfully ",
                SenderId = null,
                Type = "Notification",
                Title = "Unsubscribe"
            };

            if (Result)
            {


                // This For send  Notification
                await _notificationService.AddAsync(createNotificationDto);

                return Ok("User  subscribed successfully");

            }
            return BadRequest("Server erorr");


        }
        [Authorize]
        [HttpPost("SendPostRequestToManager")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SendPostRequestToManagerAsync(CreateParticipationDto Model)
        {

            var userId = _currentUserService.UserId;

            Model.userID = userId;

            CreateNotificationDto _CreateNotificationDto = new CreateNotificationDto();

            bool Result = await _PostRequestsBusinessLayer.SendPostRequestToManagerAsync(Model);

             var UserMangerId =  await _PostRequestsBusinessLayer.GetUserMangerIdByProjectID(Model.ProjectId);







            // This For send  Notification
            var createNotificationDto = new CreateNotificationDto
            {
                UserId = UserMangerId,
                ProjectId = Model.ProjectId,
                Message = $"the user {_userB.FullName} wants to subscribe to your project",
                SenderId = userId,
                Type = "ParticipationRequest",
                Title = "New Request"
            };


            if (Result)
            {

                // This For send  Notification
                await _notificationService.AddAsync(createNotificationDto);

                return Ok("Send successful");


            }
            return BadRequest("Server error");


        }

        //PostRequestsBusinessLayer





        [HttpPost("UpdateStatus")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<IActionResult> UpdateStatus([FromBody] UpdateStatusDto model) {


            _PostRequestsBusinessLayer = new PostRequestsBusinessLayer(_context);
            _PostRequestsBusinessLayer.Status = model.Status;



      

            var Result = await _PostRequestsBusinessLayer.UpdateStatus(model.ParticipationID);


            if (Result) {


                return Ok("Status Updated");
            

            }
            return NotFound($"ParticipationID : NotFound ");





        }

        [HttpGet("GetAllRequestsByProjectID")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<IActionResult> GetAllPendingRequestsByProjectID(int ProjectID) {

           

            if (ProjectID <1 || ProjectID== null) {


                ProjectID = ClsGlobal.ProjectID;


            }


            var Users = await _userB.GetAllPendingRequestsByProjectID(ProjectID);


            if (Users != null) {

               
                return Ok(Users);

            }

            return NotFound(" no  subscribers");

        }


        [HttpDelete("DeleteProjectByID/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProject(int id)
        {

            var result = await _PostRequestsBusinessLayer.DeleteProjectByIdAsync(id);


            return result
                ? Ok(new { message = $"Project {id} deleted successfully." })
                : NotFound(new { message = $"Project {id} not found." });
        }
    }
}
