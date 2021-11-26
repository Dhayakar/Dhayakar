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
    public class OfferController : Controller
    {

        private IRepositoryWrapper _repoWrapper;
        public OfferController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }


        [HttpGet("GetModels/{ID}")]
        public dynamic GetModels(int ID)
        {
            return _repoWrapper.OfferViewM.GetModels(ID);
        }

        [HttpPost("Insertdata")]
        public dynamic Insertdata([FromBody]OfferViewM offerviewm)
        {
            return _repoWrapper.OfferViewM.Insertdata(offerviewm);
        }

        [HttpGet("getOneplusmodelvalue/{ID}")]
        public dynamic getOneplusmodelvalue(int ID)

        {
            return _repoWrapper.OfferViewM.getOneplusmodelvalue(ID);
        }
    }
}