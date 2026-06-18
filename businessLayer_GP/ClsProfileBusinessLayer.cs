using DataAccessLayer;
using GradProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace businessLayer_GP
{
    public class ClsProfileBusinessLayer
    {

      

        private readonly ClsProfileDataAccsessLayer _ProfileData;

        public ClsProfileBusinessLayer( ClsProfileDataAccsessLayer ProfileData) {


        
            _ProfileData = ProfileData;



        }







        public async Task<string> GetSpecialistNameByUserId(int id)
        {



            return  await _ProfileData.GetSpecialistNameByUserId(id);


        }

        public async Task<List<string>> GetCurrentProjcts(int id)
        {



            return await _ProfileData.GetCurrentProjcts(id);


        }
        public async Task<List<string>> GetprevProjcts(int id)
        {



            return await _ProfileData.GetprevProjcts(id);


        }
        public async Task<int> GetCountOfAllProjects(int id)
        {



            return await _ProfileData.GetCountOfAllProjects(id);


        }

        public async Task<int> GetCountOfFinshedProjects(int id)
        {



            return await _ProfileData.GetCountOfFinshedProjects(id);


        }
    }
}
