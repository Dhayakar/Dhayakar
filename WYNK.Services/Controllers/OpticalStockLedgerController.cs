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
    public class OpticalStockLedgerController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public OpticalStockLedgerController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpPost("GetStockLedger/{From}/{To}/{CompanyID}/{Time}")]
        public dynamic GetStockLedger([FromBody] OpticalStockLedgerDataView stockledger, string From, string To, int CompanyID, string Time)
        {
            return _repoWrapper.OpticalStockLedger.GetStockLedger(stockledger, From, To, CompanyID, Time);
        }
    }
}
