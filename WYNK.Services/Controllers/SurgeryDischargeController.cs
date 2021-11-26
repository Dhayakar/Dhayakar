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
    public class SurgeryDischargeController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public SurgeryDischargeController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }


        [HttpGet("getSystemicOcularDetails/{UIN}/{RegistrationTranId}/{CMPID}")]
        public SurgeryDischarge_Master getSystemicOcularDetails(string UIN, int RegistrationTranId, int CMPID)
        {
            return _repoWrapper.surgeryDischarge.getSystemicOcularDetails(UIN, RegistrationTranId, CMPID);
        }

        [HttpGet("getSurgeryIDDischargeDetails/{UIN}/{RegistrationTranId}/{CMPID}/{TC}")]
        public SurgeryDischarge_Master getSurgeryIDDischargeDetails(string UIN,int RegistrationTranId,int CMPID,int TC)
        {
            return _repoWrapper.surgeryDischarge.getSurgeryIDDischargeDetails(UIN, RegistrationTranId, CMPID, TC);
        }


        [HttpPost("SurgerDischargeDetails/{Tc}")]
        public dynamic SurgerDischargeDetails([FromBody] SurgeryDischarge_Master SurgerDischargeDetails,int Tc)
        {
            SurgerDischargeDetails.RunningNo = _repoWrapper.Common.GenerateRunningCtrlNoo(Tc, SurgerDischargeDetails.SurgeryDischargeMaster.CMPID, "GetRunningNo");
            if (SurgerDischargeDetails.RunningNo == "Running Number Does'nt Exist")
            {
                return new
                {
                    Success = false,
                    Message = "Running Number Does'nt Exist"
                };
            }
            return _repoWrapper.surgeryDischarge.SurgerDischargeDetails(SurgerDischargeDetails,Tc);
        }


        [HttpGet("IsSurgeryComplete/{RegistrationTranId}")]
        public dynamic IsSurgeryComplete(int RegistrationTranId)
        {
            return _repoWrapper.surgeryDischarge.IsSurgeryComplete(RegistrationTranId);
        }


        [HttpGet("Isbilled/{AdmissionID}")]
        public dynamic Isbilled(string AdmissionID)
        {
            return _repoWrapper.surgeryDischarge.Isbilled(AdmissionID);
        }

        [HttpPost("SaveTemplate")]
        public dynamic SaveTemplate([FromBody] SaveTemplate SaveTemplate)
        {
            return _repoWrapper.surgeryDischarge.SaveTemplate(SaveTemplate);
        }

        [HttpGet("TemplateDetails/{DoctorId}/{cmpid}")]
        public dynamic TemplateDetails(int DoctorId, int cmpid)
        {
            return _repoWrapper.surgeryDischarge.TemplateDetails(DoctorId, cmpid);
        }

        [HttpGet("ViewTempDetails/{Desc}/{DoctorID}/{cmpid}")]
        public dynamic ViewTemplateDetails(string Desc, int DoctorID, int cmpid)
        {
            return _repoWrapper.surgeryDischarge.ViewTemplateDetails(Desc, DoctorID, cmpid);
        }

        [HttpGet("ViewTempDetailsm/{Desc}/{DoctorID}/{cmpid}")]
        public dynamic ViewTemplateDetailsm(string Desc, int DoctorID, int cmpid)
        {
            return _repoWrapper.surgeryDischarge.ViewTemplateDetailsm(Desc, DoctorID, cmpid);
        }

        [HttpGet("getTemplateDescription/{Desc}")]
        public dynamic TemplateDescription(string Desc)
        {
            return _repoWrapper.surgeryDischarge.TemplateDescription(Desc);
        }

    }
}
