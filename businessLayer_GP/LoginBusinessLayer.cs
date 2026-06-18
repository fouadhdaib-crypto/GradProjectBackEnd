using DataAccessLayer;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataAccessLayer.UserDTO;

namespace businessLayer_GP
{
    public class LoginBusinessLayer
    {
      


        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly UserDataAccess _userDataAccess;
    

        public LoginBusinessLayer(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public async Task<bool> Login(LoginDTO LDto)
        {
        

            var user = await _userManager.FindByEmailAsync(LDto.Email);


            if (user == null) {

                return false;
            }


            var result = await _signInManager.PasswordSignInAsync(user.UserName, LDto.Password,false,false);


            return result.Succeeded;

        }



   

    }
}
