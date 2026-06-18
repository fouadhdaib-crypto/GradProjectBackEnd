using businessLayer_GP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Layer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {





        private readonly ClsProfileBusinessLayer _profileBusinessLayer;
        public ProfileController(ClsProfileBusinessLayer profileBusinessLayer) {


            _profileBusinessLayer = profileBusinessLayer;


        }

    
        [HttpGet("GetSpecialistNameByUserId/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSpecialistNameByUserId(int id) { 
        





            var Result =  await _profileBusinessLayer.GetSpecialistNameByUserId(id);

            if (Result == null) { 
            

                return NotFound("this user does not have Specialist Yet ");
            
            }

            return Ok(Result);


        }

       
        [HttpGet("GetCurrentProjects/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCurrentProjects(int id)
        {






            var Result = await _profileBusinessLayer.GetCurrentProjcts(id);

            if (Result == null)
            {


                return NotFound("this user does not have Projects Yet ");

            }

            return Ok(Result);


        }
       
        [HttpGet("GetprevProjcts/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetprevProjcts(int id)
        {






            var Result = await _profileBusinessLayer.GetprevProjcts(id);

            if (Result == null)
            {


                return NotFound("this user does not have any prev Projects  Yet ");

            }

            return Ok(Result);


        }
   
        [HttpGet("GetCountOfAllProjects/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCountOfAllProjects(int id)
        {






            var Result = await _profileBusinessLayer.GetCountOfAllProjects(id);

            if (Result == null)
            {


                return Ok(0);

            }

            return Ok(Result);


        }
     
        [HttpGet("GetCountOfFinshedProjects/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCountOfFinshedProjects(int id)
        {






            var Result = await _profileBusinessLayer.GetCountOfFinshedProjects(id);

            if (Result == null)
            {


                return Ok(0);

            }

            return Ok(Result);


        }
    }
}
