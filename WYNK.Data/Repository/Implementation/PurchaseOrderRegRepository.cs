using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
namespace WYNK.Data.Repository.Implementation
{
    class PurchaseOrderRegRepository : RepositoryBase<PurchaseOrderRegView>, IPurchaseOrderRegRepository
    {

        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        

        public PurchaseOrderRegRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public dynamic getPO_Reg(DateTime FromDate, DateTime Todate, int CompanyID, int TID)
        {
            var getData = new PurchaseOrderRegView();
            var M_Drug = WYNKContext.DrugMaster.ToList();
            var M_PO = WYNKContext.PurchaseOrder.ToList();
            var M_Vendor = CMPSContext.VendorMaster.ToList();
            var M_POT = WYNKContext.PurchaseOrderTrans.ToList();
            var M_OLM = CMPSContext.OneLineMaster.ToList();
            var Fdate = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd");
            var Tdate = Convert.ToDateTime(Todate).ToString("yyyy-MM-dd");


            getData.getPO_Reg = (from PO in M_PO.Where(x => Convert.ToDateTime(x.CreatedUTC).Date >= Convert.ToDateTime(Fdate) && Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(Tdate)
                                 && x.TransactionID == TID && x.CMPID == CompanyID
                                 )
                                 join Mvendor in M_Vendor
                                 on PO.VendorID equals Mvendor.ID
                                 join OLM in M_OLM
                                 on PO.DeliveryLocationID equals OLM.OLMID

                                 select new getPO_Reg
                                 {
                                     PoNo = PO.PONumber,
                                     PoDate = PO.PODate,
                                     Q_No = PO.QuotationNumber,
                                     Q_Date = PO.QuotationDate,
                                     VendorName = Mvendor.Name,
                                     Dlvry_Name = PO.DeliveryName,
                                     Dlvry_Addrss = PO.DeliveryAddress1,
                                     Location = OLM.ParentDescription,
                                     Gross_Prod_val = PO.GrossProductValue,
                                     Tot_Dis_val = PO.TotalDiscountValue,
                                     Tot_Tax_val = PO.TotalTaxValue,
                                     Tot_Po_val = PO.TotalPOValue,
                                     PO_status = PO.POStatus,
                                 }).ToList();
            return getData;
        }

        public dynamic getPO_Reg1(DateTime FromDate, DateTime Todate, int CompanyID, int TID1)
        {
            var getData1 = new PurchaseOrderRegView();
            var M_Drug = WYNKContext.DrugMaster.ToList();
            var M_PO = WYNKContext.PurchaseOrder.ToList();
            var M_Vendor = CMPSContext.VendorMaster.ToList();
            var M_POT = WYNKContext.PurchaseOrderTrans.ToList();
            var M_OLM = CMPSContext.OneLineMaster.ToList();
            var Fdate = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd");
            var Tdate = Convert.ToDateTime(Todate).ToString("yyyy-MM-dd");


            getData1.getPO_Reg1 = (from PO in M_PO.Where(x => Convert.ToDateTime(x.CreatedUTC).Date >= Convert.ToDateTime(Fdate) && Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(Tdate) && x.CMPID == CompanyID
                                   && x.TransactionID == TID1)


                                   join Mvendor in M_Vendor
                                   on PO.VendorID equals Mvendor.ID
                                   join OLM in M_OLM
                                   on PO.DeliveryLocationID equals OLM.OLMID

                                   select new getPO_Reg1
                                   {
                                       PoNo1 = PO.PONumber,
                                       PoDate1 = PO.PODate,
                                       VendorName1 = Mvendor.Name,
                                       Dlvry_Name1 = PO.DeliveryName,
                                       Dlvry_Addrss1 = PO.DeliveryAddress1,
                                       Location1 = OLM.ParentDescription,
                                       Gross_Prod_val1 = PO.GrossProductValue,
                                       Tot_Dis_val1 = PO.TotalDiscountValue,
                                       Tot_Tax_val1 = PO.TotalTaxValue,
                                       Tot_Po_val1 = PO.TotalPOValue,
                                       PO_status1 = PO.POStatus,

                                   }).ToList();


            return getData1;
        }

    }
}
