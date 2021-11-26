
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;


namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class FinancialController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public FinancialController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }
        [HttpPost("InsertFart")]
        public dynamic InsertFart([FromBody]Financialview Fe)
        {
            return _repoWrapper.Financial.InsertFart(Fe);
        }
        [HttpPost("UpdateFart/{ID}/{FID}")]
        public dynamic UpdateFart([FromBody]Financialview Fe1, int ID, int FID)
        {
            return _repoWrapper.Financial.UpdateFart(Fe1, ID, FID);
        }

        [HttpGet("GetFpartDetail/{cmpid}")]
        public Financialview GetFpartDetail(int cmpid)
        {
            return _repoWrapper.Financial.GetFpartDetail(cmpid);
        }
        [HttpGet("GetStsDetail/{M_ID}/{docotorid}")]
        public Financialview GetStsDetail(int M_ID, int docotorid)
        {
            return _repoWrapper.Financial.GetStsDetail(M_ID, docotorid);
        }
        [HttpGet("GetStatusDetails/{docotorid}/{FYD}")]
        public Financialview GetStatusDetails(int docotorid, int FYD)
        {
            return _repoWrapper.Financial.GetStatusDetails(docotorid, FYD);
        }
    


    }
}


