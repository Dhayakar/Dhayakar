
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class ModuleMasterController : Controller
    {
        private IRepositoryWrapper _repoWrapper;

        public ModuleMasterController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpGet("getParentModuleName/{CMPID}/{Type}")]
        public ModuleMasterViewM getParentModuleName(string CMPID,string Type)
        {
            return _repoWrapper.ModuleMasterViewM.getParentModuleName(CMPID,Type);
        }


        [HttpGet("Getavilablemodules")]
        public dynamic Getavilablemodules()
        {
            return _repoWrapper.ModuleMasterViewM.Getavilablemodules();
        }
        

        [HttpPost("insertdata")]
        public dynamic insertdata([FromBody]ModuleMasterViewM moduleMaster)
        {
            return _repoWrapper.ModuleMasterViewM.insertdata(moduleMaster);
        }

        [HttpPost("updatedata/{ID}")]
        public dynamic updatedata([FromBody]ModuleMasterViewM moduleMaster, int ID)
        {
            return _repoWrapper.ModuleMasterViewM.updatedata(moduleMaster, ID);
        }





    }
}