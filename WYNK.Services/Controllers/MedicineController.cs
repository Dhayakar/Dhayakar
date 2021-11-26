
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;
using System;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class MedicineController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public MedicineController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpGet("ManualSearch/{GivenDate}/{CompanyID}")]
        public Vmmedicine ManualSearch(DateTime GivenDate, int CompanyID)
        {
            return _repoWrapper.Medicine.ManualSearch(GivenDate, CompanyID);
        }
        [HttpGet("MonthSearch/{FromDate}/{ToDate}/{CompanyID}")]
        public Vmmedicine MonthSearch(DateTime FromDate, DateTime ToDate, int CompanyID)
        {
            return _repoWrapper.Medicine.MonthSearch(FromDate, ToDate, CompanyID);
        }
     
    }
}









