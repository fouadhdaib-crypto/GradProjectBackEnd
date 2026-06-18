using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class ClsProfileDataAccsessLayer
    {


        private readonly AppDbContext _context;

        public ClsProfileDataAccsessLayer(AppDbContext context) {


            _context = context;





        }


          public async Task<string> GetSpecialistNameByUserId(int userId) {


            var SpecialistName = await _context.Users.
            Where(u => u.Id == userId).Select(x => x.Specialization.Name).FirstOrDefaultAsync();


            if (SpecialistName==null) {

                return "Not found";
            
            }

            return SpecialistName;

        }

        public async Task<List<string>> GetCurrentProjcts(int userId) {



            var Count = await _context.EnrollProjectsUsers.
             Where(x => x.userID == userId && x.Project.EndDate > DateTime.UtcNow)
             .Select(x => x.Project.Name).ToListAsync();


            return Count;


        }


        public async Task<List<string>> GetprevProjcts(int userId)
        {

            var Count = await _context.EnrollProjectsUsers.
             Where(x => x.userID == userId && x.Project.EndDate < DateTime.UtcNow)
             .Select(x=>x.Project.Name).ToListAsync();


            return Count;
        }

      
        public async Task<int> GetCountOfFinshedProjects(int userId)
        {

            var Count = await _context.EnrollProjectsUsers.
                Where(x => x.userID == userId && x.Project.EndDate < DateTime.UtcNow).CountAsync();


            return Count;
        }

        public async Task<int> GetCountOfAllProjects(int userId)
        {
            
         var Count =  await _context.EnrollProjectsUsers.
                Where(x => x.userID == userId).CountAsync();

            
            return Count;

        }
    }
}
