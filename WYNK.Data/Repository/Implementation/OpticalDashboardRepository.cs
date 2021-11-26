using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository.Implementation
{
    class OpticalDashboardRepository : RepositoryBase<OpticalDashboardViewModel>, IOpticalDashboardRepository
    {

        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        public OpticalDashboardRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public dynamic GetPeriodSearch(string FromDate, string ToDate, int Cmpid)
        {
            try
            {

                var ModifiedFromMonth = "01-" + Convert.ToDateTime(FromDate).ToString("MMM-yyyy");
                var Tomonth = Convert.ToDateTime(ToDate).ToString("MMM-yyyy");
                var ActualToMonth = Convert.ToDateTime(Tomonth).AddMonths(1).AddDays(-1).ToString("dd-MMM-yyyy");

                DateTime DT;
                var appdate = DateTime.TryParseExact(ModifiedFromMonth.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DT);
                {
                    DT.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
                }
                var fdate = DT;

                DateTime DT1;
                var appdate1 = DateTime.TryParseExact(ActualToMonth.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DT1);
                {
                    DT1.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
                }
                var tdate = DT1;

                var LensMas = WYNKContext.Lensmaster.Where(x=>x.CMPID == Cmpid).ToList();
                var OpticalSummary = WYNKContext.OpticalSummary.Where(x=>x.CmpID == Cmpid).ToList();

                var res = (from OPticalSummary in OpticalSummary.Where(x => x.Date >= fdate && x.Date <= tdate && x.CmpID == Cmpid )
                           group OPticalSummary by OPticalSummary.FrameLensType into OPticalSummaryRes
                           select new
                           {
                              OpsumRandomUniquieIDs = OPticalSummaryRes.Select(x => x.RandomUniqueID).ToList(),
                              Type = OPticalSummaryRes.Select(op => op.FrameLensType).FirstOrDefault() !=null ? LensMas.Where(x=>x.ID == OPticalSummaryRes.Select(op => op.FrameLensType).FirstOrDefault()).Select(x=>x.LensType).FirstOrDefault() : "Advance Amount",
                              SalesNos = OPticalSummaryRes.Select(x => x.BilledNumbers).Sum(),
                              SalesAmount = OPticalSummaryRes.Select(x => x.BilledAmount).Sum(),
                              Collections = OPticalSummaryRes.Select(x => x.CollectedAmount).Sum(),
                              LensOrFrameId = OPticalSummaryRes.Select(op => op.FrameLensType).FirstOrDefault(),
                           }).ToList();

                return new
                {
                    Success = true,
                    res = res,
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    Success = false,
                };
            }
        }

        public dynamic GetSalesTypeWiseSearch(OpticalDashboardViewModel OpticalDashboard, int Cmpid)
        {
            try
            {

                var OpticalSummary = (from OpticalSummarys in WYNKContext.OpticalSummary.Where(x => x.CmpID == Cmpid)
                                    where OpticalDashboard.OpsumRandomUniquieIDs.Contains(OpticalSummarys.RandomUniqueID)
                                    select OpticalSummarys).ToList();

                var Brand = WYNKContext.Brand.Where(x => x.cmpID == Cmpid).ToList();

                var res = (from OPticalSummary in OpticalSummary
                           group OPticalSummary by OPticalSummary.Brand into OPticalSummaryRes
                           select new
                           {
                               Brand = Brand.Where(x => x.ID == OPticalSummaryRes.Key).Select(x => x.Description).FirstOrDefault(),
                               SalesNos = OPticalSummaryRes.Select(x => x.BilledNumbers).Sum(),
                               SalesAmount = OPticalSummaryRes.Select(x => x.BilledAmount).Sum(),
                               Collections = OPticalSummaryRes.Select(x => x.CollectedAmount).Sum(),
                           }).ToList();

                return new
                {
                    Success = true,
                    res = res,
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    Success = false,
                };
            }
        }

        public dynamic GetOpbill(string FromDate, string ToDate, int Cmpid, int LensOrFrameId)
        {

            try
            {
                var LenMasRandomuniqueID = WYNKContext.Lensmaster.Where(x => x.CMPID == Cmpid && x.ID == LensOrFrameId).Select(x => x.RandomUniqueID).FirstOrDefault();
                var LtIDs = WYNKContext.Lenstrans.Where(x => x.LMID == LenMasRandomuniqueID).Select(x => x.ID).ToList();


                var ModifiedFromMonth = "01-" + Convert.ToDateTime(FromDate).ToString("MMM-yyyy");
                var Tomonth = Convert.ToDateTime(ToDate).ToString("MMM-yyyy");
                var ActualToMonth = Convert.ToDateTime(Tomonth).AddMonths(1).AddDays(-1).ToString("dd-MMM-yyyy");

                DateTime DT;
                var appdate = DateTime.TryParseExact(ModifiedFromMonth.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DT);
                {
                    DT.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
                }
                var fdate = DT;

                DateTime DT1;
                var appdate1 = DateTime.TryParseExact(ActualToMonth.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DT1);
                {
                    DT1.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
                }
                var tdate = DT1;

                var OpticalIn = WYNKContext.OpticalInvoiceMaster.Where(x => x.CMPID == Cmpid).ToList();
                var OpticalInvoiceTran = WYNKContext.OpticalInvoiceTran.ToList();
                var Lenstrans = WYNKContext.Lenstrans.ToList();
                var uommaster = CMPSContext.uommaster.ToList();
                var Brand = WYNKContext.Brand.Where(x=>x.cmpID == Cmpid).ToList();
                var cusM = WYNKContext.CustomerMaster.Where(x=>x.CmpID == Cmpid).ToList();
                var cusO = WYNKContext.CustomerOrder.Where(x => x.CmpID == Cmpid).ToList();
                var TaxMaster = CMPSContext.TaxMaster.ToList();

                var res = (from OpticalInvoiceMasters in OpticalIn.Where(x => x.CreatedUTC.Date >= fdate && x.CreatedUTC.Date <= tdate)
                           join OpticalInvoiceTrans in OpticalInvoiceTran on OpticalInvoiceMasters.RandomUniqueID equals  OpticalInvoiceTrans.OID
                           where LtIDs.Contains(OpticalInvoiceTrans.LensID)
                           select new
                           {
                            BillNo = OpticalInvoiceMasters.InvoiceNumber,
                            BillDate = OpticalInvoiceMasters.InvoiceDate,
                            CustomerName = cusM.Where(x => x.ID == cusO.Where(s => s.RandomUniqueID == OpticalInvoiceTrans.COID).Select(s => s.CusID).FirstOrDefault()).Select(x => String.Concat(x.Name + " " + (x.MiddleName != null ? x.MiddleName : " ") + " " + x.LastName)).FirstOrDefault(),
                            Description = OpticalInvoiceTrans.Description == null ? " " : OpticalInvoiceTrans.Description,
                            Brand = Brand.Where(br => br.ID == Lenstrans.Where(x => x.ID == OpticalInvoiceTrans.LensID).Select(x => x.Brand).FirstOrDefault()).Select(br=>br.Description).FirstOrDefault(),
                            UOM   = uommaster.Where(x=>x.id == OpticalInvoiceTrans.UOMID).Select(x=>x.Description).FirstOrDefault(),
                            Qty = OpticalInvoiceTrans.Quantity,
                            Rate = OpticalInvoiceTrans.Amount,
                            Amount = OpticalInvoiceTrans.Quantity * OpticalInvoiceTrans.Amount,
                            DisPerc = OpticalInvoiceTrans.DiscountPercentage,
                            DisAmount = OpticalInvoiceTrans.DiscountAmount,
                            GrossAmount = (OpticalInvoiceTrans.Quantity * OpticalInvoiceTrans.Amount) - (OpticalInvoiceTrans.DiscountAmount !=null ? OpticalInvoiceTrans.DiscountAmount : 0),
                            NetAmount = OpticalInvoiceTrans.itemValue,
                            GST = OpticalInvoiceTrans.GSTPercentage,
                            CESS = OpticalInvoiceTrans.CESSPercentage,
                            AddCess = OpticalInvoiceTrans.AdditionalCESSPercentage,
                            GSTValue = OpticalInvoiceTrans.GSTTaxValue,
                            CESSValue = OpticalInvoiceTrans.CESSAmount,
                            AddCessValue = OpticalInvoiceTrans.AdditionalCESSAmount,
                            GSTDesc = TaxMaster.Where(tax => tax.ID == OpticalInvoiceTrans.TaxID).Select(x => x.TaxDescription).FirstOrDefault(),
                            CESSDesc = TaxMaster.Where(tax => tax.ID == OpticalInvoiceTrans.TaxID).Select(x => x.CESSDescription).FirstOrDefault(),
                            AddCessDesc = TaxMaster.Where(tax => tax.ID == OpticalInvoiceTrans.TaxID).Select(x => x.AdditionalCESSDescription).FirstOrDefault(),
                           }).ToList();

                return new
                {
                    Success = true,
                    res = res,
                };
            }
            catch (Exception ex)
            {

                return new
                {
                    Success = false,
                };
            }
        }

        public dynamic GetAdvanceAmount(string FromDate, string ToDate, int Cmpid,int Tc)
        {

            try
            {
                var ModifiedFromMonth = "01-" + Convert.ToDateTime(FromDate).ToString("MMM-yyyy");
                var Tomonth = Convert.ToDateTime(ToDate).ToString("MMM-yyyy");
                var ActualToMonth = Convert.ToDateTime(Tomonth).AddMonths(1).AddDays(-1).ToString("dd-MMM-yyyy");

                DateTime DT;
                var appdate = DateTime.TryParseExact(ModifiedFromMonth.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DT);
                {
                    DT.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
                }
                var fdate = DT;

                DateTime DT1;
                var appdate1 = DateTime.TryParseExact(ActualToMonth.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DT1);
                {
                    DT1.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
                }
                var tdate = DT1;

                var CustomerOrder = WYNKContext.CustomerOrder.Where(x => x.CmpID == Cmpid).ToList();
                var CustomerMaster = WYNKContext.CustomerMaster.Where(x => x.CmpID == Cmpid).ToList();
                var PaymentMaster = WYNKContext.PaymentMaster.Where(x => x.CmpID == Cmpid).ToList();


                var res = (from Pay in PaymentMaster.Where(x => Convert.ToDateTime(x.InVoiceDate).Date >= fdate && Convert.ToDateTime(x.InVoiceDate).Date <= tdate && x.TransactionID == Tc)
                           select new 
                           {
                               ReceiptNo = Pay.ReceiptNumber,
                               ReceiptDate = Pay.ReceiptDate,
                               CustomerName = CustomerMaster.Where(Cm=>Cm.ID == CustomerOrder.Where(x => x.OrderNo == Pay.InVoiceNumber).Select(x => x.CusID).FirstOrDefault()).Select(cm => String.Concat(cm.Name + " " + (cm.MiddleName != null ? cm.MiddleName : " ") + " " + cm.LastName)).FirstOrDefault(),
                               SaleAmount = CustomerOrder.Where(x => x.OrderNo == Pay.InVoiceNumber).Select(x => x.TotalProductValue).FirstOrDefault(),
                               CollectedAmount = (from Pa in PaymentMaster.Where(x => x.InVoiceNumber == Pay.InVoiceNumber)
                                                  select new
                                                  { 
                                                  Amount =Pa.Amount
                                                  }).ToList().Sum(x=>x.Amount),
                               PayMode =Pay.PaymentMode,
                               InstrumentNo =Pay.InstrumentNumber,
                               InstrumentDate = Pay.Instrumentdate,
                               BankName = Pay.BankName,
                               Expirydate = Pay.Expirydate,
                               Amount = Pay.Amount,
                           }).ToList();



                return new
                {
                    Success = true,
                    res = res,
                };
            }
            catch (Exception ex)
            {

                return new
                {
                    Success = false,
                };
            }



           
        }
    }
}
