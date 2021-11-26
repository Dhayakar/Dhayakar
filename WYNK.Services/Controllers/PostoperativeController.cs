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
    public class PostoperativeController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public PostoperativeController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpPost("Insertpostoperative")]
        public dynamic Insertpostoperative([FromBody]Postoperativeview postop)
        {
            return _repoWrapper.Postoperative.Insertpostoperative(postop);
        }


        [HttpPost("Updatepostoperative/{ID}")]
        public dynamic Updatepostoperative([FromBody] Postoperativeview postopup, int ID)
        {
            return _repoWrapper.Postoperative.Updatepostoperative(postopup, ID);
        }


    }

}




