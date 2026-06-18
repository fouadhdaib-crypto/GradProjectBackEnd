using businessLayer_GP;
using DataAccessLayer;
using GradProject.Global;
using GradProject.Models;
using GradProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using static DataAccessLayer.UserDataAccess;
namespace GradProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {


        private readonly LoginBusinessLayer logBL;
        private readonly JwtService _jwtService;
        private readonly UserDataAccess _userDataAccess;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly UserbusinessLayer _userbusiness;


        public LoginController(
            LoginBusinessLayer _logBL,
            UserDataAccess userDataAccess,
            UserManager<ApplicationUser> userManager
            , JwtService jwtService)
        {
            logBL = _logBL;
            _userDataAccess = userDataAccess;
            _userManager = userManager;
            _jwtService = jwtService;
            _userbusiness = new UserbusinessLayer(_userDataAccess);
        }



        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {







            if (login == null || string.IsNullOrEmpty(login.Identifier) || string.IsNullOrEmpty(login.Password))
            {
                return BadRequest(new { message = "الرجاء إدخال بيانات صحيحة" });
            }

            var LoginDto = new DataAccessLayer.LoginDTO(login.Identifier, login.Password );


            var user = await _userManager.FindByEmailAsync(login.Identifier);

            if (user != null && !user.EmailConfirmed)
            {
                return Unauthorized(new { message = "يجب تأكيد الإيميل أولاً" });
            }



            var success = await logBL.Login(LoginDto);



            if (success && user != null)
            {
                var token = _jwtService.GenerateToken(user.Id.ToString(), user.Email);


             

                return Ok(new
                {
                    message = "تم تسجيل الدخول بنجاح",
                    user = new
                    {
                        identifier = login.Identifier,
                        fullName = user.FullName,
                        id = user.Id
                    },
                    token = token


                


                });

               
            }
            else
            {
                return Unauthorized(new { message = "اسم المستخدم أو كلمة المرور غير صحيحة" });
            }




        }

        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> Register([FromForm] RegisterDTO Register)
        {

            var passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$";

            if (!Regex.IsMatch(Register.Password, passwordPattern))
            {
                return BadRequest("كلمة المرور ضعيفة (يجب أن تحتوي على 8 أحرف على الأقل، حرف كبير، حرف صغير، ورقم)");
            }


            if (string.IsNullOrEmpty(Register.UserName) ||
              string.IsNullOrEmpty(Register.Password) ||
              string.IsNullOrEmpty(Register.FullName) ||
              string.IsNullOrEmpty(Register.Email) ||
              Register.ProfileImage == null ||
                  string.IsNullOrEmpty(Register.skills) ||
              string.IsNullOrEmpty(Register.ProfileImage.FileName))
            {
                return BadRequest("البيانات غير مكتملة");
            }

            // إنشاء الـ DTO وربطه بالـ Business Layer
            RegisterBusinessLayer registerBusiness = new RegisterBusinessLayer(_userDataAccess);

            

            if (Register.ProfileImage.FileName != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Register.ProfileImage.FileName);
                var path = Path.Combine("wwwroot/images", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await Register.ProfileImage.CopyToAsync(stream);
                }

                Register.imagePAth = "images/" + fileName;
            }


            var user = await _userManager.FindByEmailAsync(Register.Email);

            registerBusiness.RDto = Register;

            if (user != null)
            {
                // توليد رمز تأكيد الإيميل
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                // توليد رابط التأكيد
                var confirmationLink = Url.Action(
                    "ConfirmEmail",
                    "Login",
                    new { userId = user.Id, token = encodedToken },
                    Request.Scheme
                );

                // إرسال الإيميل
                await SendEmail(user.Email, "Confirm your email",
                    $@"
            <h2>Welcome 🎉</h2>
            <p>Please confirm your email by clicking the button below:</p>
            <a href='{confirmationLink}' 
               style='padding:10px 20px; background:#007bff; color:white; text-decoration:none; border-radius:5px;'>
               Confirm Email
            </a>
            ");
            }


            if (await registerBusiness.Save())
            {



                return Ok(new { message = "تم إنشاء الحساب بنجاح", userName = Register.UserName });




            }

            return BadRequest("البيانات غير مكتملة");

        }




        private async Task SendEmail(string toEmail, string subject, string body)
        {
            var smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("fouad.hdaib@gmail.com", "guuoitgneullzyak"),
                EnableSsl = true
            };

            var message = new MailMessage
            {
                From = new MailAddress("fouad.hdaib@gmail.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            message.To.Add(toEmail);

            await smtp.SendMailAsync(message);
        }


        [HttpGet("confirm")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return BadRequest("User not found");

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (!result.Succeeded)
                return BadRequest("Invalid token");

            return Ok("Email confirmed ✅");
        }




        [HttpGet("UserName")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> getuserNameByUserID() {


           var user = await _userManager.GetUserAsync(User);

            if (user == null) {
            
            

                return NotFound();
            
            
            }

            return Ok( new
            {
             
                user.FullName
            });

        }


        [HttpGet("GetUserInfo/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserInfo(int? id) {
            
           


             var Result  =  await _userDataAccess.getUserByid(id);

            

            if (Result == null) {
              



                return NotFound(Result);
            }
          
            return Ok(Result);
         

        }



        [HttpPut("UpdateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUser([FromForm] UserDTO dto)  // ← add [FromBody]
        {
            if (!ModelState.IsValid)  // ← add validation check
                return BadRequest(ModelState);


            if (dto.ProfileImage != null) {
                if (dto.ProfileImage.FileName != null)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.ProfileImage.FileName);
                    var path = Path.Combine("wwwroot/images", fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await dto.ProfileImage.CopyToAsync(stream);
                    }

                    dto.imagePath = "images/" + fileName;
                }
            }



            var result = await _userbusiness.UpdateUserInfo(dto);

            if (result)
                return Ok("User Updated");

            return NotFound("User not found or update failed");
        }
    }
  

}
