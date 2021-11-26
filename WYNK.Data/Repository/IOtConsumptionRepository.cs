using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;
using Microsoft.AspNetCore.Http;


namespace WYNK.Data.Repository
{
    public interface IOTConsumptionRepository : IRepositoryBase<OTConsumptionViewModel>
    {
        dynamic SurgeryAssignedList(int cmpid, string GMTTIME);
        dynamic UnbilledList(int cmpid, string GMTTIME);
        dynamic AssignedDoctors(string SAID, int cmpid);
      
        dynamic GetdrugDetails(int ID,int StoreID, int CmpID);
        dynamic GetSurgeryPackageCost(string BillingType, string AdmRandomId, int CmpID);
        dynamic SubmitOT(OTConsumptionDetails OTConsumptionDetails);
        dynamic UpdateOT(UpdateOTConsumptionDetails OTConsumptionDetails);
        dynamic BeforeSkinIncisionDetails(int CMPID);
        dynamic BeforePatientLeavesOperating(int CMPID);
        dynamic OtNoteslist( OTConsumptionDetails Text, int value);

        bool uploadImagslod(IFormFile file, string SurgeryID, int ImageCount);

        dynamic GetconsentDetails(int cmpid);
        dynamic IoDrugDetails(string SAID, int cmpid);
    }
}
