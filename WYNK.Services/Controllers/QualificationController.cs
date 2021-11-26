using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WYNK.Data.Common;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Data.Repository;
using WYNK.Data.Repository.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class QualificationController : Controller
    {
        private IRepositoryWrapper _repoWrapper;

        public QualificationController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }



        [HttpPost("AddValues")]
        public dynamic AddValues([FromBody]QualificationMasterViewModel add)
        {
            return _repoWrapper.QualifyMaster.AddValues(add);
        }
        [HttpGet("getDetails")]
        public dynamic getDetails()
        {
            return _repoWrapper.QualifyMaster.getDetails();
        }

        [HttpPost("Update/{id}")]
        public dynamic Update([FromBody] QualificationMasterViewModel upd, int id)
        {
            return _repoWrapper.QualifyMaster.Update(upd, id);
        }
        [HttpPost("Delete/{id}/{idd}")]
        public dynamic Delete([FromBody] QualificationMasterViewModel del, string id,int idd)
        {
            return _repoWrapper.QualifyMaster.Delete(del, id,idd);
        }
        [HttpGet("getDetailsAll")]
        public dynamic getDetailsAll()
        {
            return _repoWrapper.QualifyMaster.getDetailsAll();
        }
        [HttpGet("getDetailsdegree/{id}")]
        public dynamic getDetailsdegree(int id)
        {
            return _repoWrapper.QualifyMaster.getDetailsdegree(id);
        }
        [HttpGet("getDetailsspec/{val}")]
        public dynamic getDetailsspec(string val)
        {
            return _repoWrapper.QualifyMaster.getDetailsspec(val);
        }
        [HttpPost("Addvaluess/{id}")]
        public dynamic Addvaluess([FromBody]QualificationMasterViewModel add,int id)
        {
            return _repoWrapper.QualifyMaster.Addvaluess(add,id);
        }
        [HttpPost("AddvaluessS/{id}")]
        public dynamic AddvaluessS([FromBody]QualificationMasterViewModel addd, int id)
        {
            return _repoWrapper.QualifyMaster.AddvaluessS(addd, id);
        }
    }
}