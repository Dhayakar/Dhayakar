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
    public class UserListController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public UserListController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpGet("GetUsersList/{CompanyID}")]
        public UsersListView GetUsersList(int CompanyID)
        {
            return _repoWrapper.UserList.GetUsersList(CompanyID);
        }

        [HttpPost("UpdateStatus/{UID}")]
        public dynamic UpdateStatus([FromBody] UsersListView UserList, int UID)
        {
            return _repoWrapper.UserList.UpdateStatus(UserList, UID);
        }



    }
}
