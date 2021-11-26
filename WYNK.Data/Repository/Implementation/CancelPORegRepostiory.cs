
using System;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
namespace WYNK.Data.Repository.Implementation
{
    class CancelPORegRepostiory : RepositoryBase<CancelPORegViewModel>, ICancePORegRepository
    {


        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        

        public CancelPORegRepostiory(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }
        public dynamic getC_PO_Reg(DateTime FromDate, DateTime Todate, int TID, int CompanyID)
        {
            var getC_PoDetReg = new CancelPORegViewModel();


            var M_Drug = WYNKContext.DrugMaster.ToList();
            var M_PO = WYNKContext.PurchaseOrder.ToList();
            var M_Vendor = CMPSContext.VendorMaster.ToList();
            var M_POT = WYNKContext.PurchaseOrderTrans.ToList();
            var M_OLM = CMPSContext.OneLineMaster.ToList();

            var Fdate = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd");
            var Tdate = Convert.ToDateTime(Todate).ToString("yyyy-MM-dd");


            return getC_PoDetReg;
        }



    }
}
