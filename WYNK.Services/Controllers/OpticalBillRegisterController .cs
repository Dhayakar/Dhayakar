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
    public class OpticalBillRegisterController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public OpticalBillRegisterController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpGet("getOpticalBillDet/{Fromdate}/{Todate}/{CMPID}")]
        public OpticalBillRegisterViewModel getOpticalBillDet(string Fromdate, string Todate, int CMPID)
        {
            return _repoWrapper.OpticalBillRegister.getOpticalBillDet(Fromdate, Todate, CMPID);
        }



    }
}