
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IDrugStockSummaryRepository : IRepositoryBase<DrugStockSummaryDataView>
    {       
        dynamic GetStockSummary(DrugStockSummaryDataView stocksummary, string From, string To, int CompanyID, string Time);
        dynamic GetStockLedger(DrugStockSummaryDataView stockledger, string From, string To, int CompanyID, string Time);
    }
}
