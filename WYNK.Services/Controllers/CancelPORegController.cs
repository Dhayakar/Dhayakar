
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;
using System;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]

    public class CancelPORegController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public CancelPORegController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpGet("getC_PO_Reg/{FromDate}/{Todate}/{TID}/{CompanyID}")]
        public CancelPORegViewModel getC_PO_Reg(DateTime FromDate, DateTime Todate, int TID, int CompanyID)
        {
            return _repoWrapper.CancelPORegViewModel.getC_PO_Reg(FromDate, Todate, TID, CompanyID);
        }


    }

}