using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface ISurgeryDischargeRepository : IRepositoryBase<SurgeryDischarge_Master>
    {

        SurgeryDischarge_Master getSurgeryIDDischargeDetails(string UIN, int RegistrationTranId,int CMPID,int TC);
        SurgeryDischarge_Master getSystemicOcularDetails(string UIN, int RegistrationTranId,int CMPID);
        dynamic SurgerDischargeDetails(SurgeryDischarge_Master SurgerDischargeDetails, int Tc);
        dynamic IsSurgeryComplete(int RegistrationTranId);
        dynamic Isbilled(string AdmissionID);
        dynamic SaveTemplate(SaveTemplate SaveTemplate);
        dynamic TemplateDetails(int DoctorId, int cmpid);
        dynamic ViewTemplateDetails(string Desc, int DoctorID, int cmpid);
        dynamic ViewTemplateDetailsm(string Desc, int DoctorID, int cmpid);
        dynamic TemplateDescription(string Desc);
    }
}
