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
    public class SlitLampController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public SlitLampController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }


        [HttpGet("GetDesc")]
        public dynamic GetDesc()
        {
            return _repoWrapper.SlitLampViewM.GetDesc();
        }

        [HttpGet("GetDescfi")]
        public dynamic GetDescfi()
        {
            return _repoWrapper.SlitLampViewM.GetDescfi();
        }

        [HttpGet("GetDesctab")]
        public dynamic GetDesctab()
        {
            return _repoWrapper.SlitLampViewM.GetDesctab();
        }

        [HttpPost("InsertDesc")]
        public dynamic InsertDesc([FromBody]SlitLampViewM SlitLamp)
        {
            return _repoWrapper.SlitLampViewM.InsertDesc(SlitLamp);
        }

        [HttpPost("UpdateDesc/{ID}")]
        public dynamic UpdateDesc([FromBody] SlitLampViewM SlitLamp, int ID)
        {
            return _repoWrapper.SlitLampViewM.UpdateDesc(SlitLamp, ID);
        }

        [HttpPost("Deletedes/{ID}")]
        public dynamic Deletedes([FromBody] SlitLampViewM SlitLamp, int ID)
        {
            return _repoWrapper.SlitLampViewM.Deletedes(SlitLamp, ID);
        }

        [HttpPost("Deletedess/{ID}")]
        public dynamic Deletedess([FromBody] SlitLampViewM SlitLamp, int ID)
        {
            return _repoWrapper.SlitLampViewM.Deletedess(SlitLamp, ID);
        }

        [HttpPost("Deletedesss/{ID}")]
        public dynamic Deletedesss([FromBody] SlitLampViewM SlitLamp, int ID)
        {
            return _repoWrapper.SlitLampViewM.Deletedesss(SlitLamp, ID);
        }
        [HttpPost("UpdateSubDesc/{ID}")]
        public dynamic UpdateSubDesc([FromBody] SlitLampViewM SlitLamp, int ID)
        {
            return _repoWrapper.SlitLampViewM.UpdateSubDesc(SlitLamp, ID);
        }


        [HttpPost("UpdateaddDesc/{ID}")]
        public dynamic UpdateaddDesc([FromBody] SlitLampViewM SlitLamp, int ID)
        {
            return _repoWrapper.SlitLampViewM.UpdateaddDesc(SlitLamp, ID);
        }

        [HttpPost("InsertSubdesc")]
        public dynamic InsertSubdesc([FromBody]SlitLampViewM SlitLamp)
        {
            return _repoWrapper.SlitLampViewM.InsertSubdesc(SlitLamp);
        }

        [HttpPost("InsertAddesc")]
        public dynamic InsertAddesc([FromBody]SlitLampViewM SlitLamp)
        {
            return _repoWrapper.SlitLampViewM.InsertAddesc(SlitLamp);
        }

        [HttpGet("GetSubDesc/{ID}")]
        public dynamic GetSubDesc(int ID)
        {
            return _repoWrapper.SlitLampViewM.GetSubDesc(ID);
        }

        [HttpGet("GetSubDescfi/{ID}")]
        public dynamic GetSubDescfi(int ID)
        {
            return _repoWrapper.SlitLampViewM.GetSubDescfi(ID);
        }

        [HttpGet("GetaddDescfi/{ID}")]
        public dynamic GetaddDescfi(int ID)
        {
            return _repoWrapper.SlitLampViewM.GetaddDescfi(ID);
        }

        [HttpGet("GetfullDesc/{ID}")]
        public dynamic GetfullDesc(int ID)
        {
            return _repoWrapper.SlitLampViewM.GetfullDesc(ID);
        }

        [HttpGet("GetadDesc/{ID}")]
        public dynamic GetadDesc(int ID)
        {
            return _repoWrapper.SlitLampViewM.GetadDesc(ID);
        }


    }
}