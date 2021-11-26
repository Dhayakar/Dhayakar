
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;


namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class DrugStockSummaryController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public DrugStockSummaryController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        
        [HttpPost("GetStockSummary/{From}/{To}/{CompanyID}/{Time}")]
        public dynamic GetStockSummary([FromBody] DrugStockSummaryDataView stocksummary, string From, string To, int CompanyID, string Time)
        {
            return _repoWrapper.DrugStockSummary.GetStockSummary(stocksummary, From, To, CompanyID, Time);
        }

        //////////////////////////////////ledger//////////////////////////////////////////////////////
        [HttpPost("GetStockLedger/{From}/{To}/{CompanyID}/{Time}")]
        public dynamic GetStockLedger([FromBody] DrugStockSummaryDataView stockledger, string From, string To, int CompanyID, string Time)
        {
            return _repoWrapper.DrugStockSummary.GetStockLedger(stockledger, From, To, CompanyID, Time);
        }

      

    }
}
