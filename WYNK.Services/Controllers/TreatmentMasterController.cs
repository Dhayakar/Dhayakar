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
    public class TreatmentMasterController:Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public TreatmentMasterController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }



        [HttpPost("InsertTreatment")]
        public dynamic InsertTreatment([FromBody]TreatmentMasterDataView Con)
        {
            return _repoWrapper.TreatmentMaster.InsertTreatment(Con);
        }

        [HttpGet("PopupSearch")]
        public TreatmentMasterDataView PopupSearch()
        {
            return _repoWrapper.TreatmentMaster.PopupSearch();
        }
        [HttpPost("UpdateTratment")]
        public dynamic UpdateTratment([FromBody]TreatmentMasterDataView Cons)
        {
            return _repoWrapper.TreatmentMaster.UpdateTratment(Cons);
        }
        [HttpPost("DeleteTreatment/{ID}")]
        public dynamic DeleteTreatment([FromBody]TreatmentMasterDataView Cons1, int ID)
        {
            return _repoWrapper.TreatmentMaster.DeleteTreatment(Cons1,ID);
        }
    }
}
