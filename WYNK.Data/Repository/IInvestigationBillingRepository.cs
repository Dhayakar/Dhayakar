
using WYNK.Data.Model.ViewModel;


namespace WYNK.Data.Repository
{
    public interface IInvestigationBillingRepository : IRepositoryBase<InvestigationBilling>
    {

        InvestigationBilling GetPatDetails(string uin);//GetTaxDetails
        InvestigationBilling GetBillingDetails(string ipid,int TaxID);
        InvestigationBilling GetTaxDetails();

        dynamic UpdateInvBilling(InvestigationBilling InvestigationBilling, int cmpPid, int TransactionTypeid);
        InvestigationBilling GetInvesprint(int id, int rid, int cid, int tid);//Getprint
        InvestigationBilling Getprint(string billno, int cid, int tid);
        InvestigationBilling GetReBillingDetails(int id);

        InvestigationBilling Getreprint(int oid, int rid, int cid, int tid);
        InvestigationBilling GetBillingTaxDetails(string ipid, int TaxID, int iptrid);

    }
}
