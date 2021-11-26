using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading;
using WYNK.Data.Repository;
using WYNK.Data.Common;
using WYNK.Data.Model.ViewModel;


namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class SetupMasterController : Controller
    {
        private IRepositoryWrapper _repoWrapper;

        public SetupMasterController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;

        }
        [HttpPost("InsertSetupdata")]
        public dynamic InsertSetupdata([FromBody]SetupMasterViewModel Con)
        {

            return _repoWrapper.GetSetup.InsertSetupdata(Con);
        }


        [HttpGet("Getcountrycurrency/{countryvalue}")]
        public dynamic Getcountrycurrency(string countryvalue)
        {

            return _repoWrapper.GetSetup.Getcountrycurrency(countryvalue);
        }


        [HttpGet("GetcountryTimesoffsetwitjstring/{countryvalue}")]
        public dynamic GetcountryTimesoffsetwitjstring(string countryvalue)
        {

            return _repoWrapper.GetSetup.GetcountryTimesoffsetwitjstring(countryvalue);
        }


        [HttpGet("GetcountryTimesoffset/{countryvalue}")]
        public dynamic GetcountryTimesoffset(string countryvalue)
        {

            return _repoWrapper.GetSetup.GetcountryTimesoffset(countryvalue);
        }






        [HttpPost("uploadImag/{CompanyID}")]
        public bool uploadImag(string CompanyID)
        {
            var file = Request.Form.Files[0];
            return _repoWrapper.GetSetup.uploadImag(file, CompanyID);
        }


        [HttpGet("getsetupdata/{Cmpid}")]
        public dynamic getsetupdata(string Cmpid)
        {

            return _repoWrapper.GetSetup.getsetupdata(Cmpid);
        }


        [HttpPost("UpdateSetupdata")]
        public dynamic UpdateSetupdata([FromBody]SetupMasterViewModel Con)
        {

            return _repoWrapper.GetSetup.UpdateSetupdata(Con);
        }



    }
}


