
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;
using System;

namespace WYNK.Services.Controllers
{

    [Route("[controller]")]
    public class MedservController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public MedservController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }
        [HttpGet("getDetails/{FromDate}/{Todate}/{companyid}")]
        public Medservviewmodel getDetails(DateTime FromDate, DateTime Todate, int companyid)
        {
            return _repoWrapper.medserv.getDetails(FromDate, Todate, companyid);
        }
        [HttpGet("getDetailsIn/{FromDate}/{Todate}/{service}")]
        public Medservviewmodel getDetailsIn(DateTime FromDate, DateTime Todate,string service)
        {
            return _repoWrapper.medserv.getDetailsIn(FromDate, Todate,service);
        }

    }
}
