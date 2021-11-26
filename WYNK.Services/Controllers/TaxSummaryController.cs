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
    public class TaxSummaryController : Controller
    {
        private IRepositoryWrapper _repoWrapper;

        public TaxSummaryController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }


        [HttpGet("getTaxSummaryDet/{Fromdate}/{Todate}/{CMPID}")]
        public TaxSummaryViewM getTaxSummaryDet(string Fromdate, string Todate, int CMPID)
        {
            return _repoWrapper.TaxSummaryViewM.getTaxSummaryDet(Fromdate, Todate, CMPID);
        }



    }
}