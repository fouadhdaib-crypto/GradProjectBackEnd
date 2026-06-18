using BusinessLayer.Dtos.Chat;
using businessLayer_GP;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GradProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {

         private readonly ClsChatBusinesssLayer _chatBL;
        public ChatController(ClsChatBusinesssLayer chatBL) {


            _chatBL = chatBL;




        }


        [HttpPost("CreateChat")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateChat([FromBody] ChatRoomDto crd)
        {
            var result = await _chatBL.CreateChatAsync(crd);
            if (!result)
                return BadRequest("فشل إنشاء غرفة الدردشة");

            return Ok("تم إنشاء غرفة الدردشة بنجاح");
        }

        [HttpPost("JoinChat/{roomId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> JoinChat(int roomId)
        {
            var result = await _chatBL.JoinChatRoom(roomId);
            if (!result)
                return BadRequest("فشل الانضمام للغرفة");

            return Ok("تم الانضمام بنجاح");
        }

        [HttpPost("SendMessage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SendMessage([FromBody] MessageDto mdto)
        {
            var result = await _chatBL.SendMessageAsync(mdto);
            if (!result)
                return BadRequest("فشل إرسال الرسالة");

            return Ok("تم إرسال الرسالة بنجاح");
        }

        [HttpGet("GetMessages/{roomId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MessageDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMessages(int roomId)
        {
            var messages = await _chatBL.GetAllMessageByRoomIdAsync(roomId);

            if (messages == null || !messages.Any())
                return BadRequest("لا توجد رسائل لهذه الغرفة");

            return Ok(messages);
        }

        //[HttpGet("GetAllUserBySpecializationName/{SpecializationName}")]
        //public async Task<IActionResult> GetAllUserBySpecializationName(string SpecializationName)
        //{
        //    try
        //    {
        //        var result = await _chatBL.GetAllUserBySpecializationName(SpecializationName);

        //        if (result == null || !result.Any())
        //            return NotFound($"No users found with specialization: {SpecializationName}");

        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}


    }
}
