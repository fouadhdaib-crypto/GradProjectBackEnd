using businessLayer_GP.Dtos.User;
using DataAccessLayer;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataAccessLayer.UserDataAccess;

namespace businessLayer_GP
{
    public class RegisterBusinessLayer
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public RegisterDTO RDto { get; set; }

        public ProfileInfoDTO ProfileInfoDTO { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }


        public bool Role { get; set; }
        public string password { get; set; }
        public string? skills { get; set; }

        public string? Specialization { get; set; }
        public string imagePAth { get; set; }

        private readonly UserDataAccess _userDataAccess;

        public RegisterBusinessLayer(UserDataAccess userDataAccess)
        {
            _userDataAccess = userDataAccess;





        }

        private async Task<bool> _addNewUser()
        {

            if (RDto != null)
            {


                await _userDataAccess.AddNewUser(RDto);

                return true;
            }

            return false;

        }

        public async Task<bool> addNewProfile() {


            var ProfileInfo = new ProfileInfo
            {
                
                Description = ProfileInfoDTO.Description,
                RoleName = ProfileInfoDTO.RoleName,
                rating = ProfileInfoDTO.Rating,
                skills = ProfileInfoDTO.Skills,
                UserId = ProfileInfoDTO.UserId,
                

            };



            await _userDataAccess.addNewProfile(ProfileInfo);



            return true;
        }

        public async Task<bool> Save()
        {



            switch (Mode)
            {


                case enMode.AddNew:


                    if (await _addNewUser()) {


                        return true;


                    }


                    break;




                case enMode.Update:

                    //   _UpdateNewUser Will Be Here();

                    break;




            }

            return false;


        }



    
    }
}
