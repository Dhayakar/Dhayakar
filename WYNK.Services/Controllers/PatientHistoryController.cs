using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Model.ViewModel;
using WYNK.Data.Repository;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]


    public class PatientHistoryController : Controller
    {
        private IRepositoryWrapper _repoWrapper;

        public PatientHistoryController(IRepositoryWrapper repowrapper)
        {
            _repoWrapper = repowrapper;
        }

        [HttpPost("Insertfamilyhistory")]
        public dynamic Insertfamilyhistory([FromBody] PatientHistoryViewModel patientHistoryViewModel)
        {
            return _repoWrapper.PatientHistory.Insertfamilyhistory(patientHistoryViewModel);
        }


        [HttpPost("InsertPatientHistory")]
        public dynamic InsertPatientHistory([FromBody] PatientHistoryViewModel patientHistoryViewModel)
        {
            return _repoWrapper.PatientHistory.InsertPatientHistory(patientHistoryViewModel);
        }


        [HttpGet("Getphdetails/{Getuin}")]
        public PatientHistoryViewModel Getphdetails(string Getuin)
        {
            return _repoWrapper.PatientHistory.Getphdetails(Getuin);
        }

       

        [HttpGet("GetMedicalPrescriptiondetails/{Getuin}")]
        public PatientHistoryViewModel GetMedicalPrescriptiondetails(string Getuin)
        {
            return _repoWrapper.PatientHistory.GetMedicalPrescriptiondetails(Getuin);
        }


        [HttpGet("GetMedicaldetails/{Getuin}/{Defineddata}")]
        public PatientHistoryViewModel GetMedicaldetails(string Getuin, string Defineddata)
        {
            return _repoWrapper.PatientHistory.GetMedicaldetails(Getuin, Defineddata);
        }


        [HttpGet("GetInvestigationPrescriptiondetails/{Getuin}")]
        public PatientHistoryViewModel GetInvestigationPrescriptiondetails(string Getuin)
        {
            return _repoWrapper.PatientHistory.GetInvestigationPrescriptiondetails(Getuin);
        }


        [HttpGet("GetInvestigationdetails/{Getuin}/{CID}")]
        public PatientHistoryViewModel GetInvestigationdetails(string Getuin, int CID)
        {
            return _repoWrapper.PatientHistory.GetInvestigationdetails(Getuin, CID);
        }

        [HttpGet("GetInvestigationImagedetails/{Dataid}/{Getuin}")]
        public dynamic GetInvestigationImagedetails(DateTime Dataid, string Getuin)
        {
            return _repoWrapper.PatientHistory.GetInvestigationImagedetails(Dataid, Getuin);
        }

        [HttpGet("Getrefractiondetails/{Getuin}")]
        public PatientHistoryViewModel Getrefractiondetails(string Getuin)
        {
            return _repoWrapper.PatientHistory.Getrefractiondetails(Getuin);
        }

        [HttpGet("Getrefractiondetails1/{RID}")]
        public PatientHistoryViewModel Getrefractiondetails1(int RID)
        {
            return _repoWrapper.PatientHistory.Getrefractiondetails1(RID);
        }

        [HttpGet("Getopitcaldetails/{Getuin}")]
        public PatientHistoryViewModel Getopitcaldetails(string Getuin)
        {
            return _repoWrapper.PatientHistory.Getopitcaldetails(Getuin);
        }
        [HttpGet("Getopticalprescription/{RID}")]
        public dynamic Getopticalprescription(int RID)
        {
            return _repoWrapper.PatientHistory.Getopticalprescription(RID);
        }


        [HttpPost("DeletesystemicCondition/{ID}/{Reasons}")]
        public dynamic DeletesystemicCondition(int ID, string Reasons)
        {
            return _repoWrapper.PatientHistory.DeletesystemicCondition(ID, Reasons);
        }


        [HttpPost("updateSystemicConditions/{ID}")]
        public dynamic updateSystemicConditions([FromBody] PatientHistoryViewModels PatientHistoryViewModels, int ID)
        {
            return _repoWrapper.PatientHistory.updateSystemicConditions(PatientHistoryViewModels, ID);
        }




        [HttpPost("SubmitCurrentMedication/{UIN}/{cmpid}")]
        public dynamic SubmitCurrentMedication([FromBody] PatientCurrentMed PatientCurrentMedication, string UIN, int cmpid)
        {
            return _repoWrapper.PatientHistory.SubmitCurrentMedication(PatientCurrentMedication, UIN, cmpid);
        }

        [HttpPost("GetCurrentMedication/{UIN}")]
        public dynamic GetCurrentMedication([FromBody] List<ListCompanyIds> cmpid, string UIN)
        {
            return _repoWrapper.PatientHistory.GetCurrentMedication(UIN, cmpid);
        }


        [HttpPost("InsertSurgeryHistory")]
        public dynamic InsertSurgeryHistory([FromBody] SurgeryHistoryViewModel SurgeryHistoryViewModel)
        {
            return _repoWrapper.PatientHistory.InsertSurgeryHistory(SurgeryHistoryViewModel);
        }


        [HttpPost("DeleteSurgeryHistory/{SurHisID}")]
        public dynamic DeleteSurgeryHistory([FromBody] SurgeryHistoryViewModel SurgeryHistoryViewModel,int SurHisID)
        {
            return _repoWrapper.PatientHistory.DeleteSurgeryHistory(SurgeryHistoryViewModel, SurHisID);
        }

        [HttpPost("getSurgeryHistory/{UIN}")]
        public dynamic getSurgeryHistory([FromBody] List<ListCompanyIds> cmpid, string UIN)
        {
            return _repoWrapper.PatientHistory.getSurgeryHistory(cmpid, UIN);
        }


        [HttpGet("GetSurgeryOtNotes/{SurgeryId}/{Cmpid}")]
        public dynamic GetSurgeryOtNotes(string SurgeryId, int Cmpid)
        {
            return _repoWrapper.PatientHistory.GetSurgeryOtNotes(SurgeryId, Cmpid);
        }

        [HttpPost("InsertAllergy/{uin}")]
        public dynamic InsertAllergy([FromBody] PatientHistoryViewModel InsertAllergy, string uin)
        {
            return _repoWrapper.PatientHistory.InsertAllergy(InsertAllergy, uin);
        }


        [HttpPost("InsertPAC")]
        public dynamic InsertPAC([FromBody] PatientHistoryViewModel InsertPAC)
        {
            return _repoWrapper.PatientHistory.InsertPAC(InsertPAC);
        }

        [HttpPost("InsertBH")]
        public dynamic InsertBH([FromBody] PatientHistoryViewModel InsertBH)
        {
            return _repoWrapper.PatientHistory.InsertBH(InsertBH);
        }


        [HttpGet("GetallergyDetailsrecent/{uin}")]
        public dynamic GetallergyDetailsrecent(string uin)
        {
            return _repoWrapper.PatientHistory.GetallergyDetailsrecent(uin);
        }

        [HttpGet("getpacxam/{uin}/{CmpID}")]
        public dynamic getpacxam(string uin, int CmpID)
        {
            return _repoWrapper.PatientHistory.getpacxam(uin, CmpID);
        }

        [HttpGet("getbh/{uin}/{CmpID}")]
        public dynamic getbh(string uin, int CmpID)
        {
            return _repoWrapper.PatientHistory.getbh(uin, CmpID);
        }

        [HttpGet("gethistorypacxam/{uin}/{CmpID}/{gmt}")]
        public dynamic gethistorypacxam(string uin, int CmpID, string gmt)
        {
            return _repoWrapper.PatientHistory.gethistorypacxam(uin, CmpID, gmt);
        }
        [HttpGet("gethistorybh/{uin}/{CmpID}/{gmt}")]
        public dynamic gethistorybh(string uin, int CmpID, string gmt)
        {
            return _repoWrapper.PatientHistory.gethistorybh(uin, CmpID, gmt);
        }

        [HttpGet("gethistoryal/{uin}/{CmpID}/{gmt}")]
        public dynamic gethistoryal(string uin, int CmpID, string gmt)
        {
            return _repoWrapper.PatientHistory.gethistoryal(uin, CmpID, gmt);
        }

        [HttpGet("GetallergyDetailshistory/{uin}")]
        public dynamic GetallergyDetailshistory(string uin)
        {
            return _repoWrapper.PatientHistory.GetallergyDetailshistory(uin);
        }


        [HttpPost("DeleteAllergy/{uin}/{ID}/{Cmpid}")]
        public dynamic DeleteAllergy(string uin, int ID, int Cmpid)
        {
            return _repoWrapper.PatientHistory.DeleteAllergy(uin, ID, Cmpid);
        }

        [HttpGet("GetRemovedAllergyDetails/{uin}")]
        public dynamic GetRemovedAllergyDetails(string uin)
        {
            return _repoWrapper.PatientHistory.GetRemovedAllergyDetails(uin);
        }

    }
}