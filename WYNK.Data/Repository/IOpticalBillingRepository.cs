using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Common;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IOpticalBillingRepository : IRepositoryBase<OpticalBilling>
    {

        dynamic Getorders(int val, string Time);
        dynamic GetGrntrnsdetails(string RandomUniqueID, int SID, int CmpID);
        dynamic stockavailable(int ID, int SID, int CmpID);
        dynamic Beforesubmitcheckstock(OpticalBilling GetOpticaldetailsfullcheck, int SID, int CmpID);
        dynamic InsertOptBilling(OpticalBilling OpticalBilling, int CmpID, int TID, int SID);
        dynamic Opticalbillingprint(string OpticalID, int CMPID, int TID, string Time);
        dynamic GetGrntrnsdetailsstore(int SID, int CmpID);
    }
}
