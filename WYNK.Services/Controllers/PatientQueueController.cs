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
    public class PatientQueueController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public PatientQueueController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpGet("GetQueueDate/{CompanyID}")]
        public PatientQueueViewModel GetQueueDate(int CompanyID)
        {
            return _repoWrapper.PatientQueue.GetQueueDate(CompanyID);
        }
        

    }
}


