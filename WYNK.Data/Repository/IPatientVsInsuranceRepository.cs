using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IPatientVsInsuranceRepository : IRepositoryBase<PatientVsInsuranceViewModel>
    {
        PatientVsInsuranceViewModel getRegData( int CmpID,string Name);
        PatientVsInsuranceViewModel getJointPolicyData(int CmpID, string UIN);
        dynamic InsertPatientVsInsurance(PatientVsInsuranceViewModel AddPatientVsInsurance, int UserID, int CmpID);
        dynamic UpdatePatientVsInsurance(PatientVsInsuranceViewModel PatientVsInsuranceUpdate, int PAINSID);
        
        PatientVsInsuranceViewModel getPatientVsInsuranceData(int CmpID);

        
             bool uploadinsimage(IFormFile file, string desc, string uin, string id);
    }
}
