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
    public class ServiceMasterController : Controller
    {
        private IRepositoryWrapper _repoWrapper;

        public ServiceMasterController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpPost("Saveservicemasterdata")]
        public dynamic Saveservicemasterdata([FromBody]ServicesViewModel BMI)
        {
            return _repoWrapper.service.Saveservicemasterdata(BMI);
        }

        [HttpGet("Deleteservicemasterdata/{Pid}/{childid}/{docid}")]
        public dynamic Deleteservicemasterdata(string Pid, string childid, string docid)
        {
            return _repoWrapper.service.Deleteservicemasterdata(Pid, childid, docid);
        }

        [HttpPost("InsertRoleVsService/{CMPID}/{userroleID}/{M_Role}")]
        public dynamic InsertRoleVsService([FromBody] ServicesViewModel AddRoleVsService, int CMPID, int userroleID, int M_Role)
        {
            return _repoWrapper.service.InsertRoleVsService(AddRoleVsService, CMPID, userroleID, M_Role);
        }

        [HttpGet("GetServiceDetails/{RoleName}/{CMPID}")]
        public ServicesViewModel GetServiceDetails(int RoleName,int CMPID)

        {
            return _repoWrapper.service.GetServiceDetails(RoleName, CMPID);
        }

    }
}