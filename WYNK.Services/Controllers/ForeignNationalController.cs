
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class ForeignNationalController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public ForeignNationalController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }
        [HttpPost("InsertForeignNational")]
        public dynamic InsertForeignNational([FromBody]ForeignNationalViewModel AddForeignNational)
        {
            return _repoWrapper.ForeignNational.InsertForeignNational(AddForeignNational);
        }

        [HttpGet("getForeignNational/{CmpID}")]
        public ForeignNationalViewModel getForeignNational(int CmpID)

        {
            return _repoWrapper.ForeignNational.getForeignNational(CmpID);
        }


        [HttpPost("UpdateForeignNational/{FNID}/{Cmpid}")]
        public dynamic UpdateForeignNational([FromBody] ForeignNationalViewModel ForeignNationalUpdate, int FNID,int Cmpid)
        {
            return _repoWrapper.ForeignNational.UpdateForeignNational(ForeignNationalUpdate, FNID, Cmpid);
        }



    }
}