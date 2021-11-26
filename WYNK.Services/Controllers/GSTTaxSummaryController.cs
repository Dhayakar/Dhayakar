
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class GSTTaxSummaryController : Controller
    {
        private IRepositoryWrapper _repoWrapper;

        public GSTTaxSummaryController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }


      

    }
}