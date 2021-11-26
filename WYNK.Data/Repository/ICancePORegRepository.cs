using System;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface ICancePORegRepository : IRepositoryBase<CancelPORegViewModel>
    {
        dynamic getC_PO_Reg(DateTime FromDate, DateTime Todate, int TID, int CompanyID);

    }
}
