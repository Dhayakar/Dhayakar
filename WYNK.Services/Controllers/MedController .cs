
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;
using System;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class MedController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public MedController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }
        [HttpGet("getDetails/{FromDate}/{Todate}/{companyid}/{GMT}")]
        public dynamic getDetails(DateTime FromDate, DateTime Todate, int companyid,string GMT)
        {
            return _repoWrapper.medeg.getDetails(FromDate, Todate, companyid, GMT);
        }
        [HttpGet("getDetailsbill/{FromDate}/{Todate}")]
        public Medical_Prescription getDetailsbill(DateTime FromDate, DateTime Todate)
        {
            return _repoWrapper.medeg.getDetailsbill(FromDate, Todate);
        }
        [HttpGet("getDetailspres/{FromDate}/{Todate}")]
        public Medical_Prescription getDetailspres(DateTime FromDate, DateTime Todate)
        {
            return _repoWrapper.medeg.getDetailspres(FromDate, Todate);
        }
    }
}
