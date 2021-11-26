
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IAdvancePaymentRepository : IRepositoryBase<AdvancePaymentViewModel>
    {

        dynamic getADVRePrint(string UIN,int cmpid);
        dynamic getADVRePrintEdit(string ReceiptNumber);
        dynamic getADVRePrintF(string ReceiptNumber,int cmpid);
        
        dynamic AddAdvance(AdvancePaymentViewModel AddBill, int CompanyID, int TransactionTypeid);
       
    }
}
