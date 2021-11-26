
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IFinalBillingRepository : IRepositoryBase<FinalBillingMaster>

    {
              
        FinalBillingMaster Getcmpdtls(int cmpid);//, string billtype
        FinalBillingMaster Getunbilleddtls(int cmpid, int tranid);//Getprintpackage
        FinalBillingMaster Getbillingdtls(int paid,int cmpid);
        FinalBillingMaster Getrepbillingdtls(int paid, int cmpid, string gmt);
        FinalBillingMaster GetReBillingDetails(int id, string gmt);
        FinalBillingMaster Getbreakupdtls(int padid, int cmpid);
        FinalBillingMaster Getbreakupdtlspackage(int padid, int cmpid);
        FinalBillingMaster Getprint(int paid, string bill, int cid, int tid, string gmt);
        FinalBillingMaster Getprintpackage(int paid, string bill, int cid, int tid, string gmt);
        dynamic InsertBilling(FinalBillingMaster payment, int cmpid, string UIN, int TransactionTypeid, int paid);//UpdateBilling
        dynamic InsertBillingpackage(FinalBillingMaster payment, int cmpid, string UIN, int TransactionTypeid, int paid, decimal netamount, decimal discamount);
        FinalBillingMaster Getunbilleddtlspackage(int cmpid, string tranid);
        dynamic Getbilleddtlspackage(int cmpid, int tranid);
        FinalBillingMaster Getbillingdtlspackage(int paid, int cmpid, string billtype);
        dynamic UpdateBilling(FinalBillingMaster payment, int cmpid, int padtid);
        dynamic UpdatePackageBilling(FinalBillingMaster payment, int cmpid, int padtid);
        dynamic Getbreakupbilling(int cmpid, int padtid);

        dynamic GetPrintbillingdtlspackage(int paid, int cmpid, string billtype);
    }

}


