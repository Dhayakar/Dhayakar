using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository.Implementation
{
    public interface IPatientHistoryRepository : IRepositoryBase<PatientHistoryViewModel>
    {
        dynamic InsertPatientHistory(PatientHistoryViewModel InsertPatientHistory);
        dynamic InsertAllergy(PatientHistoryViewModel InsertAllergy, string uin);
        dynamic InsertPAC(PatientHistoryViewModel InsertPAC);
        dynamic InsertBH(PatientHistoryViewModel InsertBH);
        dynamic GetallergyDetailsrecent(string uin);
        dynamic GetallergyDetailshistory(string uin);
        PatientHistoryViewModel Getphdetails(string Getuin);
        dynamic getpacxam(string uin, int CmpID);
        dynamic getbh(string uin, int CmpID);
        dynamic gethistorypacxam(string uin, int CmpID, string gmt);
        dynamic gethistorybh(string uin, int CmpID, string gmt);
        dynamic gethistoryal(string uin, int CmpID, string gmt);
        PatientHistoryViewModel GetMedicalPrescriptiondetails(string Getuin);
        PatientHistoryViewModel GetMedicaldetails(string Getuin, string Defineddata);
        PatientHistoryViewModel GetInvestigationPrescriptiondetails(string Getuin);
        PatientHistoryViewModel GetInvestigationdetails(string Getuin, int CID);
        dynamic GetInvestigationImagedetails(DateTime Dataid, string Getuin);
        PatientHistoryViewModel Getrefractiondetails(string Getuin);
        dynamic Getrefractiondetails1(int RID);
        dynamic Insertfamilyhistory(PatientHistoryViewModel Insertfamilyhistory);
        PatientHistoryViewModel Getopitcaldetails(string Getuin);
        dynamic Getopticalprescription(int RID);

        dynamic updateSystemicConditions(PatientHistoryViewModels PatientHistoryViewModels, int ID);
        dynamic DeletesystemicCondition(int ID, string Reasons);


        dynamic SubmitCurrentMedication(PatientCurrentMed PatientCurrentMedication, string UIN, int cmpid);
        dynamic GetCurrentMedication(string UIN, List<ListCompanyIds> cmpid);
        dynamic InsertSurgeryHistory(SurgeryHistoryViewModel SurgeryHistoryViewModel);
        dynamic DeleteSurgeryHistory(SurgeryHistoryViewModel DeleteSurgeryHistory,int SurHisID);

        dynamic getSurgeryHistory(List<ListCompanyIds> cmpid, string UIN);
        dynamic GetSurgeryOtNotes(string SurgeryId, int Cmpid);
        dynamic DeleteFDDTSyringe(string uin, int ID, int Cmpid);
        dynamic DeleteAllergy(string uin, int ID, int Cmpid);
        dynamic GetRemovedAllergyDetails(string uin);

    }
}
