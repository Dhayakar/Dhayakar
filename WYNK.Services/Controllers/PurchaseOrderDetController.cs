
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
    public class PurchaseOrderDetController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public PurchaseOrderDetController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }


        [HttpGet("getPODet/{FromDate}/{Todate}")]
        public PurchaseOrdDetView getPODet(DateTime FromDate, DateTime Todate)
        {
            return _repoWrapper.PurchaseOrdDet.getPODet(FromDate, Todate);
        }

        [HttpGet("getP_D_Det/{P_Po_No}/{P_Po_Date}")]
        public PurchaseOrdDetView getP_D_Det(string P_Po_No, DateTime P_Po_Date)
        {
            return _repoWrapper.PurchaseOrdDet.getP_D_Det(P_Po_No, P_Po_Date);

        }
    }
}

