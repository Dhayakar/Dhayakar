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
    public class UomMasterController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public UomMasterController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }


        [HttpPost("InsertUom/{UOMdep}/{userID}")]
        public dynamic InsertUom(string UOMdep,int userID)
        {
            return _repoWrapper.UomMaster.InsertUom(UOMdep, userID);
        }

        [HttpPost("UpdateUom/{UOMID}/{UOMdep}/{userID}")]
        public dynamic UpdateUom(int UOMID,string UOMdep, int userID)
        {
            return _repoWrapper.UomMaster.UpdateUom(UOMID,UOMdep, userID);
        }
        [HttpPost("DeleteUom/{UOMID}")]
        public dynamic DeleteUom( int UOMID)
        {
            return _repoWrapper.UomMaster.DeleteUom(UOMID);
        }

        [HttpGet("GetUomDetails")]
        public UomMasterViewModel GetUomDetails()
        {
            return _repoWrapper.UomMaster.GetUomDetails();
        }
    }
}



