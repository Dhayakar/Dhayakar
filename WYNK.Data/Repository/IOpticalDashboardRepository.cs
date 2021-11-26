using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IOpticalDashboardRepository : IRepositoryBase<OpticalDashboardViewModel>
    {
        dynamic GetPeriodSearch(string FromDate, string ToDate, int Cmpid);
        dynamic GetSalesTypeWiseSearch(OpticalDashboardViewModel OpticalDashboard, int Cmpid);
        dynamic GetOpbill(string FromDate, string ToDate, int Cmpid,int LensOrFrameId);
        dynamic GetAdvanceAmount(string FromDate, string ToDate, int Cmpid,int Tc);
    }
}
