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
    public class PatientVsInsuranceController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public PatientVsInsuranceController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpGet("getRegData/{CmpID}/{Name}")]
        public PatientVsInsuranceViewModel getRegData(int CmpID, string Name)
        {
            return _repoWrapper.PatientVsInsurance.getRegData(CmpID, Name);
        }



        [HttpGet("getJointPolicyData/{CmpID}/{UIN}")]
        public PatientVsInsuranceViewModel getJointPolicyData(int CmpID, string UIN)
        {
            return _repoWrapper.PatientVsInsurance.getJointPolicyData(CmpID, UIN);
        }
        [HttpPost("InsertPatientVsInsurance/{UserID}/{CmpID}")]
        public dynamic InsertPatientVsInsurance([FromBody]PatientVsInsuranceViewModel AddPatientVsInsurance,int UserID, int CmpID)
        {
            return _repoWrapper.PatientVsInsurance.InsertPatientVsInsurance(AddPatientVsInsurance, UserID, CmpID);
        }

        [HttpGet("getPatientVsInsuranceData/{CmpID}")]
        public PatientVsInsuranceViewModel getPatientVsInsuranceData(int CmpID)
        {
            return _repoWrapper.PatientVsInsurance.getPatientVsInsuranceData(CmpID);
        }
        [HttpPost("UpdatePatientVsInsurance/{PAINSID}")]
        public dynamic UpdatePatientVsInsurance([FromBody]PatientVsInsuranceViewModel PatientVsInsuranceUpdate, int PAINSID)
        {
            return _repoWrapper.PatientVsInsurance.UpdatePatientVsInsurance(PatientVsInsuranceUpdate, PAINSID);
        }




        [HttpPost("uploadinsimage/{id}/{desc}/{uin}")]
        public bool uploadinsimage(string id, string desc, string uin)
        {
            var file1 = Request.Form.Files[0];
            return _repoWrapper.PatientVsInsurance.uploadinsimage(file1, uin, desc, id);
        }



    }
}