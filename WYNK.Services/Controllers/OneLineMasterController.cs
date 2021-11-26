using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Model.ViewModel;
using WYNK.Data.Repository;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class OneLineMasterController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public OneLineMasterController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpPost("InsertSlamp/{userroleID}/{CMPID}")]
        public dynamic InsertSlamp([FromBody] OneLineMasterViewModel OneLineMaster, int userroleID, int CMPID)
        {
            return _repoWrapper.OneLineMaster.InsertSlamp(OneLineMaster, userroleID, CMPID);
        }


        [HttpPost("UdateSlampwithvalue/{OLMID}/{userroleID}/{CMPID}/{modvalue}")]
        public dynamic UdateSlampwithvalue([FromBody] OneLineMasterViewModel OneLineMaster, int OLMID, int userroleID, int CMPID, int modvalue)
        {
            return _repoWrapper.OneLineMaster.UdateSlampwithvalue(OneLineMaster, OLMID, userroleID, CMPID, modvalue);
        }

        [HttpPost("InsertSlampwithvalue/{userroleID}/{CMPID}/{modvalue}")]
        public dynamic InsertSlampwithvalue([FromBody] OneLineMasterViewModel OneLineMaster, int userroleID, int CMPID, int modvalue)
        {
            return _repoWrapper.OneLineMaster.InsertSlampwithvalue(OneLineMaster, userroleID, CMPID, modvalue);
        }


        [HttpPost("UdateSlamp/{OLMID}/{userroleID}/{CMPID}")]
        public dynamic UdateSlamp([FromBody] OneLineMasterViewModel OneLineMaster, int OLMID, int userroleID, int CMPID)
        {
            return _repoWrapper.OneLineMaster.UdateSlamp(OneLineMaster, OLMID, userroleID, CMPID);
        }
        [HttpPost("DeleteSlamp/{OLMID}/{CMPID}")]
        public dynamic DeleteSlamp([FromBody] OneLineMasterViewModel OneLineMaster, int OLMID, int CMPID)
        {
            return _repoWrapper.OneLineMaster.DeleteSlamp(OneLineMaster, OLMID, CMPID);
        }

        [HttpPost("DeleteSlampwithvalue/{OLMID}/{CMPID}/{modvalue}")]
        public dynamic DeleteSlampwithvalue([FromBody] OneLineMasterViewModel OneLineMaster, int OLMID, int CMPID, int modvalue)
        {
            return _repoWrapper.OneLineMaster.DeleteSlampwithvalue(OneLineMaster, OLMID, CMPID, modvalue);
        }


        [HttpGet("GetDetails/{MasterName}/{CMPID}")]
        public OneLineMasterViewModel GetDetails(string MasterName, int CMPID)
        {
            return _repoWrapper.OneLineMaster.GetDetails(MasterName, CMPID);
        }

    }

}



