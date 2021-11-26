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
    public class UniversityController : Controller
    {
        private IRepositoryWrapper _repoWrapper;

        public UniversityController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }
        [HttpGet("getDetailsAll")]
        public dynamic getDetailsAll()
        {
            return _repoWrapper.UniversityMaster.getDetailsAll();
        }
        [HttpGet("getDetailscollege/{id}")]
        public dynamic getDetailscollege(int id)
        {
            return _repoWrapper.UniversityMaster.getDetailscollege(id);
        }
        [HttpGet("getDetailsLoc")]
        public dynamic getDetailsLoc()
        {
            return _repoWrapper.UniversityMaster.getDetailsLoc();
        }
        [HttpPost("Addvalues/{id}/{companyid}")]
        public dynamic Addvalues([FromBody]UniversityViewModel add,int id,int companyid)
        {
            return _repoWrapper.UniversityMaster.Addvalues(add,id, companyid);
        }
        [HttpPost("Addvalues1/{id}/{id1}")]
        public dynamic Addvalues1([FromBody]UniversityViewModel add, int id,int id1)
        {
            return _repoWrapper.UniversityMaster.Addvalues1(add, id,id1);
        }
        [HttpGet("getDetailsuniv")]
        public dynamic getDetailsuniv()
        {
            return _repoWrapper.UniversityMaster.getDetailsuniv();
        }
        [HttpPost("Delete/{id}")]
        public dynamic Delete([FromBody] UniversityViewModel del, string id)
        {
            return _repoWrapper.UniversityMaster.Delete(del, id);
        }
    }
}
