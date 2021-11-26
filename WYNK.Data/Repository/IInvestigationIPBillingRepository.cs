
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IInvestigationIPBillingRepository : IRepositoryBase<InvestigationIPBilling>

    {
        InvestigationIPBilling GetRegINVIPBILLDetails(int Cmpid);//GetRegINVBILLDetails
        InvestigationIPBilling GetRegINVBILLDetails(int Cmpid);
        InvestigationIPBilling GetInvIPDetails(string uin, int cmpid);
        InvestigationIPBilling GetIPTaxBillingDetails(string ipid,int TaxID,int inpID);
        InvestigationIPBilling GetIPBillingDetails(string ipid, int TaxID);
        InvestigationIPBilling GetTaxDetails();
        
        dynamic insertInvIPBilling(InvestigationIPBilling InvIPBilling, int cmpPid, int TransactionTypeid,string UIN);

    }

}


