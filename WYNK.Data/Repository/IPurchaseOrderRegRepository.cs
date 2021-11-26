using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;


namespace WYNK.Data.Repository
{
    public interface IPurchaseOrderRegRepository : IRepositoryBase<PurchaseOrderRegView>
    {

        dynamic getPO_Reg(DateTime FromDate, DateTime Todate, int CompanyID, int TID);

        dynamic getPO_Reg1(DateTime FromDate, DateTime Todate, int CompanyID, int TID1);
    }
}
