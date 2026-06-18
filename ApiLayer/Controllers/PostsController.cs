using businessLayer_GP;
using businessLayer_GP.Dtos;
using businessLayer_GP.Dtos.Post;
using DataAccessLayer;
using GradProject.Global;
using GradProject.Model;
using GradProject.Services;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using static DataAccessLayer.PostRepositoryDataAccess;

namespace GradProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly PostProjectDTO Dto = new PostProjectDTO();

        private readonly ICurrentUserService _CurrentUserService;
        private  PostServiceBusinessLayer PostServiceBusinessLayer;

        public PostsController(AppDbContext context, UserManager<ApplicationUser> userManager , ICurrentUserService CurrentUserService)
        {
            _context = context;
            _userManager = userManager;

            PostServiceBusinessLayer = new PostServiceBusinessLayer(Dto, context);
            _CurrentUserService = CurrentUserService;
        }

        




        [HttpPost("Create")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public async Task<IActionResult> CreatePost(AddPostDto Model)
        {


            Dto.ProjectLocation = Model.ProjectLocation;
            Dto.EndDate = Model.EndDate;
            Dto.Descriptions = Model.Descriptions;
            Dto.Rating = Model.Rating;
            Dto.AvailableSeats = Model.AvailableSeats;
            Dto.Skills = Model.Skills;
            Dto.Name = Model.Name;
            Dto.IsGraduationProject = Model.IsGraduationProject;
            Dto.TeamType = Model.TeamType;
            Dto.NumberOfAvailableSeats = Model.NumberOfAvailableSeats;
            Dto.UserId =   _CurrentUserService.UserId;

            if (Dto.UserId == null|| Dto.UserId ==0) {


                return BadRequest("User ID Not found");
            
            
            }



            PostServiceBusinessLayer PostServiceBusinessLayer = new PostServiceBusinessLayer(Dto, _context);


            if (await PostServiceBusinessLayer.Save())
                return Ok();
            else { 
            
            
                return BadRequest("invalid Data");
            
            }



        }




        [HttpPut("UpdatePost")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
       
        public async Task<IActionResult> UpdatePost(PostProjectDTO Model)
        {




            PostServiceBusinessLayer PostServiceBusinessLayer = new PostServiceBusinessLayer(Dto, _context);
            PostServiceBusinessLayer.Mode = PostServiceBusinessLayer.enMode.Update;

            PostServiceBusinessLayer.Postdto = Model;

            if (await PostServiceBusinessLayer.Save())
                return Ok();
            else
            {


                return BadRequest("invalid Data");

            }



        }


        [HttpGet("GetAllProject")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllProject() {



             PostServiceBusinessLayer = new PostServiceBusinessLayer(Dto, _context);



            var Resutl = await PostServiceBusinessLayer.GetAllProject_Post();


            if (Resutl != null)
            {




                return Ok(Resutl);

            }
            else {


                return BadRequest("Data Problem");


            }



        }



        [HttpGet("GetProjectById")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProjectById(int id)
        {


            if (id <= 0)
            {
                return BadRequest("Invalid ID");
            }

            ClsGlobal.ProjectID= id;

            PostServiceBusinessLayer =new PostServiceBusinessLayer(Dto, _context);

            var Project =    await   PostServiceBusinessLayer.GetProjectById(id);


                if (Project != null) { 
            
            
            
                    return Ok(Project);
            
               }

                return NotFound(Project);

        }

        [HttpGet("GetUserByProjectId")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserByProjectId(int id)
        {


            if (id <= 0)
            {
                return BadRequest("Invalid ID");
            }

            PostServiceBusinessLayer = new PostServiceBusinessLayer(Dto, _context);

            var user = await PostServiceBusinessLayer.getUSerByProjectID(id);


            if (user != null)
            {



                return Ok(user);

            }

            return NotFound(user);

        }



    }




    }

