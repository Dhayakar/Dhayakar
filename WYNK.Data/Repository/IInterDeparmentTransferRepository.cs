
using System.Collections.Generic;

using WYNK.Data.Common;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IInterDeparmentTransferRepository : IRepositoryBase<InterDepartmentTransfer>
    {
        dynamic GetStoreDetails(int ID);
        dynamic GetdrugDetails(int ID);
        IEnumerable<Dropdown> GetDrugvalues(int storeID, int Cmpid);       

        dynamic AddstockDetails(SubmitTransferdetails AddTransfer, string GMT);
        dynamic InterDepartmentTransferDetails(int transactionCode, string GMT);
        dynamic StockTransferDetails(string StockTransferNo, int CmpId, string GMT);

        dynamic AddstockDetails(SubmitTransferdetails AddTransfer);
        dynamic InterDepartmentTransferDetails(int transactionCode);
        dynamic StockTransferDetails(string StockTransferNo, int CmpId);
        dynamic GetMixedCompanyDetails(GetCompanyIds GetCompanyIds, int CmpId);
        dynamic InterBranchIssueDetailed(string StockTransferNo, int CmpId);
        dynamic AddInterBranchTransfer(SubmitInterBanchdetails AddTransfer);
        dynamic InterBranchIsuueDetails(int transactionCode, int Cmpid, string GMT);
        dynamic GetItemListDetails(GetItemListDetails GetItemListDetail, string FromDate, string Todate, int CmpId, string GMT);
        dynamic IssueStatusUpdate(IssueStatusDetails IssueStatusDetail, int Cmpid);

    }
}
