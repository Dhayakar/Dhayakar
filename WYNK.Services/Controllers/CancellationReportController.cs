
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;
using System;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class CancellationReportController : Controller
    {
        private IRepositoryWrapper _repoWrapper;

        public CancellationReportController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

       
        [HttpGet("Todaysearch/{FromDate}/{Todate}/{CompanyID}/{arrvalue}")]
        public dynamic Todaysearch(DateTime FromDate, DateTime Todate, int CompanyID,int arrvalue)
        {
            return _repoWrapper.CancellationViewM.Todaysearch(FromDate, Todate, CompanyID, arrvalue);
        }



    }
}