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
    public class SquintExtnMasterController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public SquintExtnMasterController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpPost("InsertSquintExtnM/{userroleID}/{CmpID}")]
        public dynamic InsertSquintExtnM([FromBody] SquintExtnMasterViewModel InsertSquintExtnMaster, int userroleID,int CmpID)
        {
            return _repoWrapper.SquintExtnMaster.InsertSquintExtnM(InsertSquintExtnMaster, userroleID, CmpID);
        }
        [HttpPost("UpdateSquintExtnM/{ID}/{userroleID}/{CmpID}")]
        public dynamic UpdateSquintExtnM([FromBody] SquintExtnMasterViewModel updateMaster, int ID, int userroleID,int CmpID)
        {
            return _repoWrapper.SquintExtnMaster.UpdateSquintExtnM(updateMaster, ID, userroleID, CmpID);
        }
        [HttpPost("DeleteSQEM/{ID}/{CmpID}")]
        public dynamic DeleteSQEM([FromBody] SquintExtnMasterViewModel DeleteMaster, int ID, int CmpID)
        {
            return _repoWrapper.SquintExtnMaster.DeleteSQEM(DeleteMaster, ID, CmpID);
        }
        [HttpGet("GetDetails/{MasterName}/{CmpID}")]
        public SquintExtnMasterViewModel GetDetails(string MasterName,int CmpID)
        {
            return _repoWrapper.SquintExtnMaster.GetDetails(MasterName, CmpID);
        }


    }

}



