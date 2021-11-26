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
        public class ProfController : Controller
        {
            private IRepositoryWrapper _repoWrapper;
            public ProfController(IRepositoryWrapper repoWrapper)
            {
                _repoWrapper = repoWrapper;
            }
            [HttpPost("InsertPros")]
            public dynamic InsertPros([FromBody]ProfessionalView pos)
            {
                return _repoWrapper.Prof.InsertPros(pos);
            }
            [HttpGet("GetDetails")]
            public ProfessionalView GetDetails()
            {
            return _repoWrapper.Prof.GetDetails();
            }
            [HttpPost("deleteProfes/{ID}")]
            public dynamic deleteProfes(int? ID)
            {
            return _repoWrapper.Prof.deleteProfes(ID);
            }
            [HttpGet("Gettrans/{ID}")]
            public ProfessionalView Gettrans(int ID)
            {
             return _repoWrapper.Prof.Gettrans(ID);
            }
            [HttpGet("getData")]
            public ProfessionalView getData()
            {
            return _repoWrapper.Prof.getData();
            }
       
           [HttpPost("UpdatePart/{ID}/{pfID}")]
           public dynamic UpdatePart([FromBody]ProfessionalView De1, int ID,int pfID)
           {
            return _repoWrapper.Prof.UpdatePart(De1, ID, pfID);
           }
        

    }
}