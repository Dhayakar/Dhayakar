using System;

using WYNK.Data.Model.ViewModel;


namespace WYNK.Data.Repository
{
    public interface ICancellationReportRepository : IRepositoryBase<CancellationViewM>
    {

        dynamic Todaysearch(DateTime FromDate, DateTime Todate, int CompanyID,int arrvalue);






    }
}
