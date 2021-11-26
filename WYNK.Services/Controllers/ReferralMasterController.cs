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
    public class ReferralMasterController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public ReferralMasterController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }
        [HttpPost("UpdateRefMaster")]
        public dynamic UpdateRefMaster([FromBody] Referral_Master ReferralMaster)
        {
            return _repoWrapper.ReferralMaster.UpdateRefMaster(ReferralMaster);
        }
    }
}
