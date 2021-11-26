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
    public class ServiceExtensionController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public ServiceExtensionController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }


     
        [HttpGet("GetInpatientBilleddtls/{cmpid}")]
        public ServiceExtensionViewModel GetInpatientBilleddtls(int cmpid)
        {
            return _repoWrapper.ServiceExtension.GetInpatientBilleddtls(cmpid);
        }
        [HttpGet("GetOutpatientBilleddtls/{cmpid}")]
        public ServiceExtensionViewModel GetOutpatientBilleddtls(int cmpid)
        {
            return _repoWrapper.ServiceExtension.GetOutpatientBilleddtls(cmpid);
        }


        //[HttpGet("GetServicedtls/{cmpid}")]
        //public ServiceExtensionViewModel GetServicedtls(int cmpid)
        //{
        //    return _repoWrapper.ServiceExtension.GetServicedtls(cmpid);
        //}


    }
}


