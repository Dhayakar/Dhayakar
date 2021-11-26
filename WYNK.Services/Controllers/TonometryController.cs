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
    public class TonometryController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public TonometryController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }


        [HttpPost("Inserttonometry")]
        public dynamic Inserttonometry([FromBody]TonomentryViewmodel Addtonometry)
        {
            return _repoWrapper.Tonometry.Inserttonometry(Addtonometry);

        }


        [HttpGet("Fulltonometrylist")]
        public dynamic Fulltonometrylist()
        {
            return _repoWrapper.Tonometry.Fulltonometrylist();
        }

        [HttpPost("updatetonometry/{ID}")]
        public dynamic updatetonometry([FromBody] TonomentryViewmodel Uptonometry, int ID)
        {
            return _repoWrapper.Tonometry.updatetonometry(Uptonometry, ID);
        }

    }
}