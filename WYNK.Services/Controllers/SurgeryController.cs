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
    public class SurgeryController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public SurgeryController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpPost("InsertSurgeryMas")]
        public dynamic InsertSurgeryMas([FromBody]SurgeryViewModel SurgeryMaster)
        {
            return _repoWrapper.SurgeryMaster.InsertSurgeryMas(SurgeryMaster);
        }

        [HttpPost("InsertIntraoperative")]
        public dynamic InsertIntraoperative([FromBody]SurgeryViewModel Intraoperative)
        {
            return _repoWrapper.SurgeryMaster.InsertIntraoperative(Intraoperative);
        }
        [HttpPost("InsertPreoperative")]
        public dynamic InsertPreoperative([FromBody]SurgeryViewModel Preoperative)
        {
            return _repoWrapper.SurgeryMaster.InsertPreoperative(Preoperative);
        }




        [HttpPost("UpdateSurgeryMas/{M_AdmId}/{M_surId}")]
        public dynamic UpdateSurgeryMas([FromBody] SurgeryViewModel vendorMaster, int M_AdmId, int M_surId)
        {
            return _repoWrapper.SurgeryMaster.UpdateSurgeryMas(vendorMaster, M_AdmId, M_surId);
        }



    }

}



