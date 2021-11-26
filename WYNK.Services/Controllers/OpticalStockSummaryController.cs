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
    public class OpticalStockSummaryController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public OpticalStockSummaryController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpPost("GetStockSummary/{From}/{To}/{CompanyID}/{Time}")]
        public dynamic GetStockSummary([FromBody] OpticalStockSummaryDataView stocksummary, string From, string To, int CompanyID, string Time)
        {
            return _repoWrapper.OpticalStockSummary.GetStockSummary(stocksummary, From, To, CompanyID, Time);
        }

    }
}
