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
    public class OpticalDashboardController : Controller
    {
        private IRepositoryWrapper _repoWrapper;

        public OpticalDashboardController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpGet("GetPeriodSearch/{FromDate}/{ToDate}/{Cmpid}")]
        public dynamic GetPeriodSearch(string FromDate, string ToDate, int Cmpid)
        {
            return _repoWrapper.OpticalDashboard.GetPeriodSearch(FromDate, ToDate, Cmpid);
        }


        [HttpPost("GetSalesTypeWiseSearch/{Cmpid}")]
        public dynamic GetSalesTypeWiseSearch([FromBody] OpticalDashboardViewModel OpticalDashboard, int Cmpid)
        {
            return _repoWrapper.OpticalDashboard.GetSalesTypeWiseSearch(OpticalDashboard,Cmpid);
        }


        [HttpGet("GetOpbill/{FromDate}/{ToDate}/{Cmpid}/{LensOrFrameId}")]
        public dynamic GetOpbill(string FromDate, string ToDate, int Cmpid,int LensOrFrameId)
        {
            return _repoWrapper.OpticalDashboard.GetOpbill(FromDate, ToDate, Cmpid, LensOrFrameId);
        }

        [HttpGet("GetAdvanceAmount/{FromDate}/{ToDate}/{Cmpid}/{Tc}")]
        public dynamic GetAdvanceAmount(string FromDate, string ToDate, int Cmpid,int Tc)
        {
            return _repoWrapper.OpticalDashboard.GetAdvanceAmount(FromDate, ToDate, Cmpid,Tc);
        }

    }
}