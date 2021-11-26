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
    public class OTConsumptionController : Controller
    {
        private IRepositoryWrapper _repoWrapper;

        public OTConsumptionController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }


        [HttpGet("SurgeryAssignedList/{CMPID}/{GMTTIME}")]
        public dynamic SurgeryAssignedList(int CMPID , string GMTTIME)
        {
            return _repoWrapper.OTConsumption.SurgeryAssignedList(CMPID,GMTTIME);
        }

        [HttpGet("UnbilledList/{CMPID}/{GMTTIME}")]
        public dynamic UnbilledList(int CMPID, string GMTTIME)
        {
            return _repoWrapper.OTConsumption.UnbilledList(CMPID, GMTTIME);
        }

        [HttpGet("AssignedDoctors/{SAID}/{CMPID}")]
        public dynamic AssignedDoctors(string SAID,  int CMPID)
        {
            return _repoWrapper.OTConsumption.AssignedDoctors(SAID, CMPID);
        }




        [HttpGet("GetdrugDetails/{ID}/{StoreID}/{CmpID}")]
        public dynamic GetdrugDetails(int ID,int StoreID, int CmpID)
        {
            return _repoWrapper.OTConsumption.GetdrugDetails(ID, StoreID, CmpID);
        }


        [HttpGet("GetSurgeryPackageCost/{BillingType}/{AdmRandomId}/{CmpID}")]
        public dynamic GetSurgeryPackageCost(string BillingType, string AdmRandomId, int CmpID)
        {
            return _repoWrapper.OTConsumption.GetSurgeryPackageCost(BillingType, AdmRandomId, CmpID);
        }

        [HttpPost("SubmitOT")]
        public dynamic SubmitOT([FromBody] OTConsumptionDetails OTConsumptionDetails)
        {
            OTConsumptionDetails.RunningNo = _repoWrapper.Common.GenerateRunningCtrlNoo(OTConsumptionDetails.Tc, OTConsumptionDetails.Cmpid, "GetRunningNo");
            if (OTConsumptionDetails.RunningNo == "Running Number Does'nt Exist")
            {
                return new
                {
                    Success = false,
                    Message = "Running Number Does'nt Exist"
                };
            }
            return _repoWrapper.OTConsumption.SubmitOT(OTConsumptionDetails);
        }



        [HttpPost("UpdateOT")]
        public dynamic UpdateOT([FromBody] UpdateOTConsumptionDetails UpdateOTConsumptionDetails)
        {
            return _repoWrapper.OTConsumption.UpdateOT(UpdateOTConsumptionDetails);
        }

        [HttpPost("uploadImagslod/{SurgeryID}/{ImageCount}")]
        public bool uploadImagslod(string SurgeryID, int ImageCount)
        {
            var file1 = Request.Form.Files[0];
            return _repoWrapper.OTConsumption.uploadImagslod(file1, SurgeryID, ImageCount);
        }




        [HttpGet("BeforeSkinIncisionDetails/{CMPID}")]
        public dynamic BeforeSkinIncisionDetails(int CMPID)
        {
            return _repoWrapper.OTConsumption.BeforeSkinIncisionDetails(CMPID);
        }


        [HttpGet("BeforePatientLeavesOperating/{CMPID}")]
        public dynamic BeforePatientLeavesOperating(int CMPID)
        {
            return _repoWrapper.OTConsumption.BeforePatientLeavesOperating(CMPID);
        }


        [HttpPost("OtNoteslist/{value}")]
        public dynamic OtNoteslist([FromBody] OTConsumptionDetails otSurgeryDesc,int value)
        {
            return _repoWrapper.OTConsumption.OtNoteslist(otSurgeryDesc,value);
        }


        [HttpGet("GetconsentDetails/{CMPID}")]
        public dynamic OtNotGetconsentDetailseslist(int CMPID)
        {
            return _repoWrapper.OTConsumption.GetconsentDetails(CMPID);
        }


        [HttpGet("IoDrugDetails/{SAID}/{CMPID}")]
        public dynamic IoDrugDetails(string SAID, int CMPID)
        {
            return _repoWrapper.OTConsumption.IoDrugDetails(SAID, CMPID);
        }


    }
}



