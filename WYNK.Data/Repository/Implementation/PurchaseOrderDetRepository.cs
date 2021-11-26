using System;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository.Implementation
{
    class PurchaseOrderDetRepository : RepositoryBase<PurchaseOrdDetView>, IPurchaseorderDetRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        

        public PurchaseOrderDetRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public dynamic getPODet(DateTime FromDate, DateTime Todate)
        {
            var getPoDet = new PurchaseOrdDetView();

            var M_Drug = WYNKContext.DrugMaster.ToList();
            var M_PO = WYNKContext.PurchaseOrder.ToList();
            var M_Vendor = CMPSContext.VendorMaster.ToList();
            var M_POT = WYNKContext.PurchaseOrderTrans.Where(x => x.ItemQty != x.PORecdQty).ToList();


            var Fdate = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd");
            var Tdate = Convert.ToDateTime(Todate).ToString("yyyy-MM-dd");

          

            return getPoDet;
        }






        public dynamic getP_D_Det(string P_Po_No, DateTime P_Po_Date)
        {
            var getP_Det = new PurchaseOrdDetView();

            var M_Drug1 = WYNKContext.DrugMaster.ToList();
            var M_PO1 = WYNKContext.PurchaseOrder.ToList();

            var M_POT1 = WYNKContext.PurchaseOrderTrans.Where(x => x.ItemQty != x.PORecdQty).ToList();

            

            return getP_Det;
        }

    }
}
