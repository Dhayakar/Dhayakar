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
    public class SurgerySafetyChecklistController : Controller
    {
        private IRepositoryWrapper _repoWrapper;

        public SurgerySafetyChecklistController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }


        [HttpPost("SubmitSurgerySafetyChecklist")]
        public dynamic SubmitSurgerySafetyChecklist([FromBody] SurgeryCheckListViewModel SubmitSurgerySafetyChecklist)
        {
            return _repoWrapper.SurgerySafetyCheckList.SubmitSurgerySafetyChecklist(SubmitSurgerySafetyChecklist);
        }

        [HttpGet("GetSurgerySafetyChecklist/{CMPID}")]
        public dynamic GetSurgerySafetyChecklist(int CMPID)
        {
            return _repoWrapper.SurgerySafetyCheckList.GetSurgerySafetyChecklist(CMPID);
        }


        [HttpGet("DeleteSSCdetail/{CmpId}/{id}")]
        public dynamic DeleteSSCdetail(int CMPID,int id)
        {
            return _repoWrapper.SurgerySafetyCheckList.DeleteSSCdetail(CMPID, id);
        }


        [HttpPost("UpdateSScDetails/{id}")]
        public dynamic UpdateSScDetails([FromBody] SurgeryCheckListViewModel SubmitSurgerySafetyChecklist, int id)
        {
            return _repoWrapper.SurgerySafetyCheckList.UpdateSScDetails(SubmitSurgerySafetyChecklist, id);
        }



        [HttpGet("PreviousSSCGroupdesc/{CmpId}/{value}")]
        public dynamic PreviousSSCGroupdesc(int CMPID, int value)
        {
            return _repoWrapper.SurgerySafetyCheckList.PreviousSSCGroupdesc(CMPID, value);
        }

    }
}



