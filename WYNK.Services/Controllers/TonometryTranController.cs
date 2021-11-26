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
    public class TonometryTranController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public TonometryTranController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }


        [HttpPost("InsertTonoTrans")]
        public dynamic InsertTonoTrans([FromBody]TonometryTranViewmodel Addtono)
        {
            return _repoWrapper.TonometryTran.InsertTonoTrans(Addtono);
        }

        [HttpGet("Gettonometry/{UIN}/{cmpid}")]
        public dynamic Gettonometry(string UIN, int cmpid)
        {
            return _repoWrapper.TonometryTran.Gettonometry(UIN, cmpid);
        }

        [HttpPost("updateTonoTrans/{uin}")]
        public dynamic updateTonoTrans([FromBody] TonometryTranViewmodel uptono, string uin)
        {
            return _repoWrapper.TonometryTran.updateTonoTrans(uptono, uin);
        }

        [HttpGet("GetHistoryiopDetails/{UIN}/{cmpid}")]
        public dynamic GetHistoryiopDetails(string UIN, int cmpid)
        {
            return _repoWrapper.TonometryTran.GetHistoryiopDetails(UIN, cmpid);
        }


    }
}