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
    public class TaxMasterController : Controller
    {
        private IRepositoryWrapper _repoWrapper;

        public TaxMasterController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpPost("insertTaxMaster/{CMPID}")]
        public dynamic insertTaxMaster([FromBody]TaxMasterViewM taxMaster,int CMPID)
        {
            return _repoWrapper.TaxMasterViewM.insertTaxMaster(taxMaster, CMPID);
        }


        [HttpPost("updateTaxMaster/{ID}/{CMPID}")]
        public dynamic updatesampleReg([FromBody] TaxMasterViewM taxMaster, int ID, int CMPID)
        {
            return _repoWrapper.TaxMasterViewM.updateTaxMaster(taxMaster, ID,CMPID);
        }
    }
}