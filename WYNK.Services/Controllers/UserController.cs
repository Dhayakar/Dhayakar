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
    public class UserController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public UserController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpGet("GetRoledetails/{roletext}/{CompanyID}")]
        public dynamic GetRoledetails(string roletext,int CompanyID)
        {
            return _repoWrapper.User.GetRoledetails(roletext, CompanyID);
        }


        [HttpGet("GetModuletransactiondetails/{ModuleDescription}/{CompanyID}")]
        public dynamic GetModuletransactiondetails(string ModuleDescription, int CompanyID)
        {
            return _repoWrapper.User.GetModuletransactiondetails(ModuleDescription, CompanyID);
        }

        [HttpGet("GetModuletransactiondetailsstring/{ModuleDescription}/{suffix}/{CompanyID}")]
        public dynamic GetModuletransactiondetailsstring(string ModuleDescription, string suffix, int CompanyID)
        {
            return _repoWrapper.User.GetModuletransactiondetailsstring(ModuleDescription, suffix, CompanyID);
        }

        [HttpGet("Getconsulttranid/{ModuleDescription}/{CompanyID}")]
        public dynamic Getconsulttranid(string ModuleDescription, int CompanyID)
        {
            return _repoWrapper.User.Getconsulttranid(ModuleDescription, CompanyID);
        }

        [HttpGet("GetWorkflodata/{res1}")]
        public dynamic GetWorkflodata(string res1)
        {
            return _repoWrapper.User.GetWorkflodata(res1);
        }

        [HttpPost("Postinternaluserdetails")]
        public dynamic Postinternaluserdetails([FromBody] User AddEmp)
        {
            return _repoWrapper.User.Postinternaluserdetails(AddEmp);
        }


        [HttpPost("PostExternaluserdetails")]
        public dynamic PostExternaluserdetails([FromBody] User AddEmp)
        {
            return _repoWrapper.User.PostExternaluserdetails(AddEmp);
        }      

        [HttpPost("updateUserDet/{ID}/{IsActive}/{DoctorID}")]
        public dynamic updateUserDet([FromBody] User User, int ID, Boolean IsActive, int DoctorID)
        {
            return _repoWrapper.User.updateUserDet(User, ID, IsActive, DoctorID);


        }


    }
}
