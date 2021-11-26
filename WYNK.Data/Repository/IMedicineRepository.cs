using System;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IMedicineRepository : IRepositoryBase<Vmmedicine>
    {
        Vmmedicine ManualSearch(DateTime GivenDate, int CompanyID);
        Vmmedicine MonthSearch(DateTime FromDate, DateTime ToDate, int CompanyID);
    }

}
