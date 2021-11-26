using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IOpticalStockSummaryRepository : IRepositoryBase<OpticalStockSummaryDataView>
    {
        dynamic GetStockSummary(OpticalStockSummaryDataView stocksummary, string From, string To, int CompanyID, string Time);
    }
}
