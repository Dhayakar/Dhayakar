
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class FundusController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public FundusController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpGet("GetDesc")]
        public dynamic GetDesc()
        {
            return _repoWrapper.FundusViewM.GetDesc();
        }

        [HttpGet("GetDescfif")]
        public dynamic GetDescfif()
        {
            return _repoWrapper.FundusViewM.GetDescfif();
        }

        [HttpGet("GetDescfn")]
        public dynamic GetDescfn()
        {
            return _repoWrapper.FundusViewM.GetDescfn();
        }

        [HttpPost("InsertDesc")]
        public dynamic InsertDesc([FromBody]FundusViewM Fundus)
        {
            return _repoWrapper.FundusViewM.InsertDesc(Fundus);
        }
        [HttpGet("GetSubDesc/{ID}")]
        public dynamic GetSubDesc(int ID)
        {
            return _repoWrapper.FundusViewM.GetSubDesc(ID);
        }

        [HttpPost("InsertSubdesc")]
        public dynamic InsertSubdesc([FromBody]FundusViewM Fundus)
        {
            return _repoWrapper.FundusViewM.InsertSubdesc(Fundus);
        }

        [HttpGet("GetadDesc/{ID}")]
        public dynamic GetadDesc(int ID)
        {
            return _repoWrapper.FundusViewM.GetadDesc(ID);
        }

        [HttpGet("GetfullDesc/{ID}")]
        public dynamic GetfullDesc(int ID)
        {
            return _repoWrapper.FundusViewM.GetfullDesc(ID);
        }

        [HttpPost("InsertAddesc")]
        public dynamic InsertAddesc([FromBody]FundusViewM Fundus)
        {
            return _repoWrapper.FundusViewM.InsertAddesc(Fundus);
        }


        [HttpPost("UpdateDesc/{ID}")]
        public dynamic UpdateDesc([FromBody] FundusViewM Fundus, int ID)
        {
            return _repoWrapper.FundusViewM.UpdateDesc(Fundus, ID);
        }

        [HttpPost("UpdateSubDesc/{ID}")]
        public dynamic UpdateSubDesc([FromBody] FundusViewM Fundus, int ID)
        {
            return _repoWrapper.FundusViewM.UpdateSubDesc(Fundus, ID);
        }

        [HttpPost("UpdateaddDesc/{ID}")]
        public dynamic UpdateaddDesc([FromBody] FundusViewM Fundus, int ID)
        {
            return _repoWrapper.FundusViewM.UpdateaddDesc(Fundus, ID);
        }

        [HttpPost("Deletedes/{ID}")]
        public dynamic Deletedes([FromBody] FundusViewM Fundus, int ID)
        {
            return _repoWrapper.FundusViewM.Deletedes(Fundus, ID);
        }

        [HttpGet("GetSubDescfi/{ID}")]
        public dynamic GetSubDescfi(int ID)
        {
            return _repoWrapper.FundusViewM.GetSubDescfi(ID);
        }

        [HttpGet("GetaddDescfi/{ID}")]
        public dynamic GetaddDescfi(int ID)
        {
            return _repoWrapper.FundusViewM.GetaddDescfi(ID);
        }
        [HttpPost("Deletedess/{ID}")]
        public dynamic Deletedess([FromBody] FundusViewM Fundus, int ID)
        {
            return _repoWrapper.FundusViewM.Deletedess(Fundus, ID);
        }

        [HttpPost("Deletedesss/{ID}")]
        public dynamic Deletedesss([FromBody] FundusViewM Fundus, int ID)
        {
            return _repoWrapper.FundusViewM.Deletedesss(Fundus, ID);
        }
    }
}