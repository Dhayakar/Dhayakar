using Microsoft.EntityFrameworkCore;
using CommonMailService.cs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using WYNK.Data.Common;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Helpers;

namespace WYNK.Data.Repository.Implementation
{
    class OpticalBillRegisterRepository : RepositoryBase<OpticalBillRegisterViewModel>, IOpticalBillRegisterRepository
    {

        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        

        public OpticalBillRegisterRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }
        public decimal? getvalue(decimal? data)
        {
            var a = 0;
            var b = data;
            if (b != null)
            {
                return b;
            }
            return a;
        }

        public OpticalBillRegisterViewModel getOpticalBillDet(string Fromdate, string Todate, int CMPID)
        {
            var getData = new OpticalBillRegisterViewModel();
            var cusM = WYNKContext.CustomerMaster.ToList();
            var lens = WYNKContext.Lenstrans.ToList();
            var cusO = WYNKContext.CustomerOrder.ToList();
            var opivM = WYNKContext.OpticalInvoiceMaster.ToList();
            var opivT = WYNKContext.OpticalInvoiceTran.ToList();
            var BRD = WYNKContext.Brand.ToList();
            var Tax = CMPSContext.TaxMaster.ToList();
            var uom = CMPSContext.uommaster.ToList();
            var CompanyMaster = CMPSContext.Company.ToList();

            DateTime DT;
            var appdate = DateTime.TryParseExact(Fromdate.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DT);
            {
                DT.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            }
            var fdate = DT;

            DateTime DT1;
            var appdate1 = DateTime.TryParseExact(Todate.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DT1);
            {
                DT1.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            }
            var tdate = DT1;

            getData.getOpticalBillRegisterDetail = (from opim in opivM.Where(x => x.CreatedUTC.Date >= fdate
                                                    && x.CreatedUTC.Date <= tdate && x.CMPID == CMPID)
                                                    join opimt in opivT on opim.RandomUniqueID equals opimt.OID
                                                    orderby opim.CreatedUTC
                                                    select new getOpticalBillRegDet
                                                    
                                                    {
                                                        BillNo         = opim.InvoiceNumber,
                                                        BillDate       = opim.InvoiceDate,
                                                        PatientName    = cusM.Where(x=>x.ID == cusO.Where(s=>s.RandomUniqueID== opimt.COID).Select(s=>s.CusID).FirstOrDefault()).Select(x=>x.Name+' '+x.MiddleName+' '+x.LastName).FirstOrDefault(),
                                                        Brand          = BRD.Where(x=>x.ID == lens.Where(s => s.ID == opimt.LensID).Select(s => s.Brand).FirstOrDefault()).Select(x=>x.Description).FirstOrDefault(),

                                                        UOM            = uom.Where(x => x.id == opimt.UOMID).Select(x => x.Description).FirstOrDefault(),
                                                      
                                                        Quantity       = Convert.ToDecimal(opimt.Quantity),
                                                        Amount         = Convert.ToDecimal(opimt.itemValue),
                                                        DiscountPerc   = Convert.ToDecimal(getvalue(opimt.DiscountPercentage)),
                                                        DiscountAmount = Convert.ToDecimal(getvalue(opimt.DiscountAmount)),
                                                       
                                                        TaxDescription = Tax.Where(x => x.ID == opimt.TaxID).Select(s => s.TaxDescription).FirstOrDefault(),
                                                        CessDescription = Tax.Where(x => x.ID == opimt.TaxID).Select(s => s.CESSDescription).FirstOrDefault(),
                                                        AddCessDescription = Tax.Where(x => x.ID == opimt.TaxID).Select(s => s.AdditionalCESSDescription).FirstOrDefault(),
                                                      
                                                        TaxPerc = getvalue(opimt.GSTPercentage),
                                                        CessPerc = getvalue(opimt.CESSPercentage),
                                                        AddCessPerc = getvalue(opimt.AdditionalCESSPercentage),
                                                       
                                                        TaxValue = getvalue(opimt.GSTTaxValue),
                                                        CessValue = getvalue(opimt.CESSAmount),
                                                        AddCessValue = getvalue(opimt.AdditionalCESSAmount),
                                                      
                                                        GrossAmount = Convert.ToDecimal(getvalue(opimt.itemValue) - getvalue(opimt.DiscountAmount)),
                                                        NetAmount = getvalue(opim.NetAmount),
                                                        
                                                        CompanyName = CompanyMaster.Where(x => x.CmpID == CMPID).Select(x => x.CompanyName).FirstOrDefault(),
                                                        CAddress1 = CompanyMaster.Where(x => x.CmpID == CMPID).Select(x => x.Address1).FirstOrDefault(),
                                                    }).ToList();
            getData.TTAXAMOUNT = getData.getOpticalBillRegisterDetail.Select(x => x.TaxValue + x.CessValue + x.AddCessValue).Sum();
            getData.TotalsumAmount = getData.getOpticalBillRegisterDetail.Select(x => x.Amount).Sum();
            getData.TotalsumDiscountAmount = getData.getOpticalBillRegisterDetail.Select(x => x.DiscountAmount).Sum();
            getData.TotalsumGrossAmount = getData.getOpticalBillRegisterDetail.Select(x => x.GrossAmount).Sum();
            getData.getOpticalBillRegTaxsummary = (from opim in opivM.Where(x => Convert.ToDateTime(x.CreatedUTC).Date >= Convert.ToDateTime(fdate)
                                                    && Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(tdate) && x.CMPID == CMPID)
                                                   join opT in opivT on opim.RandomUniqueID equals opT.OID group opT by opT.TaxID into g
                                                   select new getOpticalBillRegtaxsummary
                                                    {
                                                       TaxID = g.Key,                 
                                                       TaxDescription = Tax.Where(x=>x.ID==g.Select(s=>s.TaxID).FirstOrDefault()).Select(x=>x.TaxDescription).FirstOrDefault(),
                                                       CESSDescription = Tax.Where(x => x.ID == g.Select(s => s.TaxID).FirstOrDefault()).Select(x => x.CESSDescription).FirstOrDefault(),
                                                       AdditionalCESSDescription = Tax.Where(x => x.ID == g.Select(s => s.TaxID).FirstOrDefault()).Select(x => x.AdditionalCESSDescription).FirstOrDefault(),
                                                       TotalProductValue = g.Select(x => (x.itemValue + x.GSTTaxValue + x.CESSAmount + x.AdditionalCESSAmount)-(x.DiscountAmount)).Sum(),
                                                       GSTTaxValue = g.Select(x => x.GSTTaxValue).Sum(),
                                                       CESSAmount = g.Select(x => x.CESSAmount).Sum(),
                                                       AddCESSAmount = g.Select(x => x.AdditionalCESSAmount).Sum(),
                                                       TaxPayable = g.Select(x => x.GSTTaxValue + x.CESSAmount + x.AdditionalCESSAmount).Sum(),
                                                       TaxableTurnover = g.Select(x => (x.itemValue)-(x.DiscountAmount)).Sum(),
                                                   }).ToList();
            getData.TAXTotalProductValue = getData.getOpticalBillRegTaxsummary.Select(x => x.TotalProductValue).Sum();
            getData.TAXCESSAmount = getData.getOpticalBillRegTaxsummary.Select(x => x.CESSAmount).Sum();
            getData.TAXAddCESSAmount = getData.getOpticalBillRegTaxsummary.Select(x => x.AddCESSAmount).Sum();
            getData.TAXTaxPayable = getData.getOpticalBillRegTaxsummary.Select(x => x.TaxPayable).Sum();
            getData.TAXGSTTaxValue = getData.getOpticalBillRegTaxsummary.Select(x => x.GSTTaxValue).Sum();
            getData.TAXTaxableTurnover = getData.getOpticalBillRegTaxsummary.Select(x => x.TaxableTurnover).Sum();

            return getData;
        }
        }
}
