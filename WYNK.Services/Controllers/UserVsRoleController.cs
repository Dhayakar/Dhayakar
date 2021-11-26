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
    public class UserVsRoleController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public UserVsRoleController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }
        [HttpGet("GetUsersDetails/{CmpID}/{RoleID}")]
        public UserVsRoleViewModel GetUsersDetails(int CmpID,int RoleID)

        {
            return _repoWrapper.UsersVsRole.GetUsersDetails(CmpID, RoleID);
        }

        [HttpPost("InsertUserVsRole/{RoleID}/{userroleID}/{CmpID}/{Dtag}")]
        public dynamic InsertUserVsRole([FromBody] UserVsRoleViewModel UserVsRoleInsert, int RoleID, int userroleID, int CmpID,string Dtag)
        {
            return _repoWrapper.UsersVsRole.InsertUserVsRole(UserVsRoleInsert, RoleID, userroleID, CmpID, Dtag);
        }

    }
}