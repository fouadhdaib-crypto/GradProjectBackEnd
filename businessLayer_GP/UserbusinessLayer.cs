using DataAccessLayer;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using static DataAccessLayer.UserDataAccess;

namespace businessLayer_GP
{
    public class UserbusinessLayer
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;


       
        public string FullName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public bool isITStudent { get; set; }
        private  string password { get; set; }
        public string? Image { get; set; }

       

        private readonly AppDbContext _context;
        private readonly UserDataAccess _userDataAccess;
        public UserbusinessLayer(UserDataAccess userDataAccess)
        {
            _userDataAccess = userDataAccess;
          
        }
        public UserbusinessLayer(AppDbContext _context)
        {

          _userDataAccess = new UserDataAccess(_context);
        }

        public  async Task<List<UserDTO>> GetAllUser() {

            return await _userDataAccess.GetAllUsersUsingEntityFramework();
            

        }



        public async  Task<UserDTO> getUserByid(int? id)
        {


            return await _userDataAccess.getUserByid(id);


        }

        public async Task<List<UserDTO>> GetAllPendingRequestsByProjectID(int ProjectID)
        {



            return await _userDataAccess.GetAllPendingRequestsByProjectID(ProjectID);





        }

        public async Task<bool> UpdateUserInfo(UserDTO Dto) { 
        
        
        
            return await _userDataAccess.UpdateUserInfo(Dto);
        }

    }
}
