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
    public class RoleVsAccessController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public RoleVsAccessController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }


        [HttpGet("GetAccessPrivilegesDetails/{RefID}")]
        public dynamic GetAccessPrivilegesDetails(int RefID)
        {
            return _repoWrapper.RoleVsAccess.GetAccessPrivilegesDetails(RefID);
        }

        [HttpGet("GetUserDetails/{Roleuser}/{CompanyID}")]
        public dynamic GetUserDetails(string Roleuser,string CompanyID)
        {
            return _repoWrapper.RoleVsAccess.GetUserDetails(Roleuser, CompanyID);
        }
        

        [HttpGet("GetAccessModule/{CMPID}")]
        public dynamic GetAccessModule(int CMPID)
        {
            return _repoWrapper.RoleVsAccess.GetAccessModule(CMPID);
        }


        [HttpGet("GetAccessModulebasedonuser/{Role}/{CMPID}/{MViewaccess}")]
        public dynamic GetAccessModulebasedonuser(string Role, int CMPID, string MViewaccess)
        {
            return _repoWrapper.RoleVsAccess.GetAccessModulebasedonuser(Role,CMPID, MViewaccess);
        }


        


        [HttpPost("InsertAccessPrivileges")]
        public dynamic InsertAccessPrivileges([FromBody] Rolevsaccesscontrol Rolevsaccesscontrol)
        {
            return _repoWrapper.RoleVsAccess.InsertAccessPrivileges(Rolevsaccesscontrol);
        }


        [HttpGet("GetMenuaccess/{userDoctorID}/{userroleID}/{CompanyID}/{Description}")]
        public dynamic GetMenuaccess(int userDoctorID, int userroleID, string CompanyID, string Description)
        {
            return _repoWrapper.RoleVsAccess.GetMenuaccess(userDoctorID, userroleID, CompanyID, Description);
        }

        

    }
}
