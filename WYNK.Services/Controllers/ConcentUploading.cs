
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;



namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class ConcentUploading : Controller
    {
        private IRepositoryWrapper _repoWrapper;

        public ConcentUploading(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;

        }

        [HttpPost("InsertConcent")]
        public dynamic InsertConcent([FromBody]ConcentUploadingViewModel Con)
        {

            return _repoWrapper.ConcentUploading.InsertConcent(Con);
        }
        [HttpGet("Getallconnectionstring/{CMPID}/{Module}")]
        public dynamic Getallconnectionstring(int CMPID, string Module)
        {

            return _repoWrapper.ConcentUploading.Getallconnectionstring(CMPID, Module);
        }
        
    }
}



