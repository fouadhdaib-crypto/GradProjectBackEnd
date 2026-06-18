using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DataAccessLayer.PostRepositoryDataAccess;
namespace businessLayer_GP
{
    public class PostServiceBusinessLayer
    {

        private readonly AppDbContext _context;

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        int ProjectID { get; set; }

        public PostProjectDTO  Postdto { get; set; }

        PostRepositoryDataAccess _DataAccsees;


        public PostServiceBusinessLayer(PostProjectDTO PostDTO , AppDbContext con) {

            Postdto=PostDTO;
            _context = con;

            _DataAccsees = new PostRepositoryDataAccess(_context);

        }


        public class UpdateParticipationStatusDto
        {
            public string Status { get; set; } // "Approved" أو "Rejected"
        }
        private async Task<bool> _addNewPost() {


          return   await  _DataAccsees.AddProject(Postdto);



        }

        private async Task<bool> _UpdatePost()
        {


            return await _DataAccsees.update(Postdto);



        }

        public async Task<bool> Save()
        {


            switch (Mode) { 
            
            case enMode.AddNew:

                  if(  await _addNewPost())
                    return true;
                  break;

             case enMode.Update:


                    if (await _UpdatePost())
                        return true;
                    break;



            }

            return false;

        }

        public async Task<List<PostProjectDTO>> GetAllProject_Post()
        {
            





            return await _DataAccsees.GetAllProject_Post();




          }

        public async Task<PostProjectDTO> GetProjectById(int id)
        {






            return await _DataAccsees.GetProjectById(id);




        }
        public async Task<UserDTO> getUSerByProjectID(int id)
        {






            return await _DataAccsees.GetUserByProjectID(id);




        }

      
    



    }
    }
