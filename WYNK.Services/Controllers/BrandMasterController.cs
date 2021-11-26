
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;


namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class BrandMasterController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public BrandMasterController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }


        [HttpPost("Insertbrand")]
        public dynamic Insertbrand([FromBody] BrandView Addbrand)
        {
            return _repoWrapper.BrandMaster.Insertbrand(Addbrand);

        }

        [HttpGet("Fullbrandlist/{cmpid}")]
        public dynamic Fullbrandlist(int cmpid)
        {
            return _repoWrapper.BrandMaster.Fullbrandlist(cmpid);
        }


        [HttpPost("updatebrand/{ID}")]
        public dynamic updatebrand([FromBody] BrandView Upbrand, int ID)
        {
            return _repoWrapper.BrandMaster.updatebrand(Upbrand, ID);
        }

        [HttpPost("Deletebrand/{ID}")]
        public dynamic Deletebrand(int ID)
        {
            return _repoWrapper.BrandMaster.Deletebrand(ID);
        }







    }
}