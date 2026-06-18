using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;


namespace DataAccessLayer
{
    public class LoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public LoginDTO() { }


        public LoginDTO(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }

    public class UserDTO
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? imagePath { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Role { get; set; }
        public int ParticipationID { get; set; }
        public string? skills { get; set; }
        public string? url { get; set; }
        public string? bio { get; set; }

        public IFormFile? ProfileImage { get; set; }
        public UserDTO() { }


        public UserDTO(int id, string fullName, string email, string userName,
                       string phoneNumber, string role)
        {
            Id = id;
            FullName = fullName;
            Email = email;
            UserName = userName;
            PhoneNumber = phoneNumber;
            Role = role;

        }
    }






    public class UserDataAccess
    {

        public UserDataAccess(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public UserDataAccess(AppDbContext context)
        {
            _context = context;

        }
        public string _connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=GradProjectsHubDB2;Trusted_Connection=True;TrustServerCertificate=True;";
        private readonly UserManager<ApplicationUser> _userManager;




        private readonly AppDbContext _context;

        private readonly SignInManager<ApplicationUser> _signInManager;
        public List<UserDTO> GetAllUsersuSINGDapper()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var users = conn.Query<UserDTO>(
                    "SPGetAllUser",
                    commandType: CommandType.StoredProcedure
                ).ToList();

                return users;
            }
        }


        public async Task<List<UserDTO>> GetAllUsersUsingEntityFramework()
        {


            var Useres = _context.Users.ToList();

            var UserList = new List<UserDTO>();


            foreach (var user in Useres)
            {

                var Roles = await _userManager.GetRolesAsync(user);

                UserList.Add(new UserDTO
                { Id = user.Id, Email = user.Email, FullName = user.FullName, PhoneNumber = user.PhoneNumber, UserName = user.UserName, Role = Roles.FirstOrDefault() });



            }

            return UserList;

        }









        public class RegisterDTO
        {
            public string FullName { get; set; }
            public string Email { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }

            public IFormFile? ProfileImage { get; set; }
            public string? imagePAth { get; set; }
            public string? skills { get; set; }
            public string? githubUrl { get; set; }
            public string? workField { get; set; }
            public int? SpecializationId { get; set; }
            public string? Description { get; set; }
            public bool Role { get; set; }

        }




        public async Task<bool> AddNewUser(RegisterDTO dto)
        {



            var User = new ApplicationUser

            {
                FullName = dto.FullName,
                Email = dto.Email
                    ,
                UserName = dto.UserName
                    ,
                ImagePath = dto.imagePAth
                    ,
                SpecializationId = dto.SpecializationId
                    ,
                githubUrl = dto.githubUrl
                    ,
                skills = dto.skills
                    ,
                Description = dto.Description


            };


            var Result = await _userManager.CreateAsync(User, dto.Password);






            if (Result.Succeeded)
            {
                if (dto.Role)
                    await _userManager.AddToRoleAsync(User, "UserSt");
                else
                    await _userManager.AddToRoleAsync(User, "UserDr");

                return true;
            }

            else
            {
                foreach (var error in Result.Errors)
                {
                    Console.WriteLine(error.Description);
                }
                return false;
            }







        }





        public async Task<bool> addNewProfile(ProfileInfo dto)
        {
            var user = await _context.Users
     .OrderByDescending(u => u.Id)
     .FirstOrDefaultAsync();


            var profile = new ProfileInfo
            {
                Description = dto.Description ?? "no Description ",
                skills = dto.skills ?? "no skills",
                rating = dto.rating ?? 0,
                UserId = user.Id,
                RoleName = "Student"

            };


            await _context.ProfileInfos.AddAsync(profile);

            await _context.SaveChangesAsync();


            return true;
        }


        public async Task<UserDTO> getUserByid(int? id)
        {


            var userData = await _context.Users.Include(u => u.Specialization).FirstOrDefaultAsync(u => u.Id == id);


            if (userData == null)
                return null;


            return new UserDTO
            {
                Id = userData.Id,
                Email = userData.Email,
                UserName = userData.UserName,
                imagePath = userData.ImagePath,
                FullName = userData.FullName,
                Role = userData.Specialization?.Name,
                skills = userData.skills,
                bio = userData.Specialization?.Description,
                url = userData.githubUrl


            };


        }


        public async Task<List<UserDTO>> GetAllPendingRequestsByProjectID(int ID)
        {

            var User = await (from p in _context.Participations
                              join Users in _context.Users
                               on p.UserId equals Users.Id
                              where p.ProjectId == ID && p.Status == "Pending"
                              select new UserDTO
                              {

                                  Id = Users.Id,
                                  FullName = Users.FullName,
                                  Email = Users.Email,
                                  imagePath = Users.ImagePath,
                                  ParticipationID = p.Id

                              }).ToListAsync();



            return User;


        }

        public async Task<bool> UpdateUserInfo(UserDTO dto)
        {
            var user = await _context.Users.FindAsync(dto.Id);
            if (user == null) return false;

            if (dto.FullName != null) user.FullName = dto.FullName;
            if (dto.Email != null) user.Email = dto.Email;
            if (dto.UserName != null) user.UserName = dto.UserName;
            if (dto.imagePath != null) user.ImagePath = dto.imagePath;
            if (dto.PhoneNumber != null) user.PhoneNumber = dto.PhoneNumber;
            if (dto.url != null) user.githubUrl = dto.url;
            if (dto.bio != null) user.Description = dto.bio;
            if (dto.skills != null) user.skills = dto.skills;

            var rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }




    }
}