using businessLayer_GP;
using businessLayer_GP.Dtos.Notification;
using businessLayer_GP.Service;
using GradProject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static businessLayer_GP.NotificationService;
using static DataAccessLayer.ClsProjectTeamsDataAaccess;

namespace GradProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {






        private readonly INotificationService _notificationService;
        private readonly ICurrentUserService _currentUserService;

        public NotificationController(
            INotificationService notificationService,
            ICurrentUserService currentUserService)
        {
            _notificationService = notificationService;
            _currentUserService = currentUserService;
        }



        [HttpPost("addNotification")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> Notification(CreateNotificationDto model)
        {






            if (model == null)
                return BadRequest();

           
            model.SenderId = _currentUserService.UserId;

            var result = await _notificationService.AddAsync(model);

            if (result)
                return Ok("Notification Saved");

            return NotFound("User ID Or Sender Id is wrong");
        }


        [HttpGet("GetNotification")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> GetNotification()
        {

          

            var result = await _notificationService.GetAsync();

            if (result.Count>0)
                return Ok(result);





            return NotFound("no notification Found");
        }

    }
    }
