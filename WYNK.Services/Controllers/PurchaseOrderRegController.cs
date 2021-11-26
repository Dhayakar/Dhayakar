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
    public class PurchaseOrderRegController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public PurchaseOrderRegController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }


        [HttpGet("getPO_Reg/{FromDate}/{Todate}/{CompanyID}/{TID}")]
        public PurchaseOrderRegView getPO_Reg(DateTime FromDate, DateTime Todate, int CompanyID, int TID)
        {
            return _repoWrapper.PurchaseOrderRegView.getPO_Reg(FromDate, Todate, CompanyID, TID);
        }


        [HttpGet("getPO_Reg1/{FromDate}/{Todate}/{CompanyID}/{TID1}")]
        public PurchaseOrderRegView getPO_Reg1(DateTime FromDate, DateTime Todate, int CompanyID, int TID1)
        {
            return _repoWrapper.PurchaseOrderRegView.getPO_Reg1(FromDate, Todate, CompanyID, TID1);
        }



    }
}