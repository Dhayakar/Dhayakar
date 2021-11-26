using Microsoft.EntityFrameworkCore;
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
    class TaxSummaryRepository : RepositoryBase<TaxSummaryViewM>, ITaxSummaryRepository
    {

        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
       
        public TaxSummaryRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public TaxSummaryViewM getTaxSummaryDet(string Fromdate, string Todate, int CMPID)
        {
            var getData = new TaxSummaryViewM();
           

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


            var TaxSum = WYNKContext.TaxSummary.ToList();
            var CompanyMaster = CMPSContext.Company.ToList();
            getData.getTaxSummaryDet =(from TxSm in TaxSum.Where(x => x.TransactionDate.Date >= fdate
                                       && x.TransactionDate.Date <= tdate
                                       && x.CMPID == CMPID && x.BillingType == "B")
                                       group TxSm by TxSm.TaxID into g
                                       select new getTaxSummaryDet
                                       {
                                        TaxID = g.Key,
                                        TaxDescription = g.Select(x=>x.TaxDescription).FirstOrDefault(),
                                        TransactionDate = g.Select(x => x.TransactionDate).FirstOrDefault(),
                                        TaxPercentage = g.Select(x => x.TaxPercentage).FirstOrDefault(),
                                        TaxValue = g.Select(x => x.TaxValue).Sum(),
                                        GrossAmount = g.Select(x => x.GrossAmount).Sum(),
                                        TaxPayable = g.Select(x => x.TaxValue +  x.CESSValue + x.AddlCESSValue).Sum(),
                                        Tax1Percentage = g.Select(x => x.Tax1Percentage).FirstOrDefault(),
                                        Tax1Value = g.Select(x => x.Tax1Value).Sum(),
                                        Tax2Percentage = g.Select(x => x.Tax2Percentage).FirstOrDefault(),
                                        Tax2Value = g.Select(x => x.Tax2Value).Sum(),
                                        CESSPercentage = g.Select(x => x.CESSPercentage).FirstOrDefault(),
                                        CESSValue = g.Select(x => x.CESSValue).Sum(),
                                        AddlCESSPercentage = g.Select(x => x.AddlCESSPercentage).FirstOrDefault(),
                                        AddlCESSValue = g.Select(x => x.AddlCESSValue).Sum(),
                                        CESSDescription = g.Select(x => x.CESSDescription).FirstOrDefault(),
                                        AddlCESSDescription = g.Select(x => x.AddlCESSDescription).FirstOrDefault(),
                                        CompanyName = CompanyMaster.Where(x => x.CmpID == CMPID).Select(x => x.CompanyName).FirstOrDefault(),
                                       }).ToList();
            getData.TTaxPayable = getData.getTaxSummaryDet.Select(x => x.TaxPayable).Sum();
            getData.TGrossAmount = getData.getTaxSummaryDet.Select(x => x.GrossAmount).Sum();
            getData.TTaxValue = getData.getTaxSummaryDet.Select(x => x.TaxValue).Sum();
            getData.TTax1Value = getData.getTaxSummaryDet.Select(x => x.Tax1Value).Sum();
            getData.TTax2Value = getData.getTaxSummaryDet.Select(x => x.Tax2Value).Sum();
            getData.TCESSValue = getData.getTaxSummaryDet.Select(x => x.CESSValue).Sum();
            getData.TAddlCESSValue = getData.getTaxSummaryDet.Select(x => x.AddlCESSValue).Sum();
            getData.getTaxSummaryDet1 = (from TxSm in TaxSum.Where(x => Convert.ToDateTime(x.TransactionDate.Date.ToString("yyyy-MM")) >= Convert.ToDateTime(fdate.ToString("yyyy-MM"))
                                       && Convert.ToDateTime(x.TransactionDate.Date.ToString("yyyy-MM")) <= Convert.ToDateTime(tdate.ToString("yyyy-MM"))
                                       && x.CMPID == CMPID && x.BillingType=="R")
                                        group TxSm by TxSm.TaxID into g
                                        select new getTaxSummaryDet1
                                        {
                                            TaxID = g.Key,
                                            TaxDescription = g.Select(x => x.TaxDescription).FirstOrDefault(),
                                            TransactionDate = g.Select(x => x.TransactionDate).FirstOrDefault(),
                                            GrossAmount = g.Select(x => x.GrossAmount).Sum(),
                                            TaxPayable = g.Select(x => x.TaxValue + x.CESSValue + x.AddlCESSValue).Sum(),
                                            TaxPercentage = g.Select(x => x.TaxPercentage).FirstOrDefault(),
                                            TaxValue = g.Select(x => x.TaxValue).Sum(),
                                            Tax1Percentage = g.Select(x => x.Tax1Percentage).FirstOrDefault(),
                                            Tax1Value = g.Select(x => x.Tax1Value).Sum(),
                                            Tax2Percentage = g.Select(x => x.Tax2Percentage).FirstOrDefault(),
                                            Tax2Value = g.Select(x => x.Tax2Value).Sum(),
                                            CESSPercentage = g.Select(x => x.CESSPercentage).FirstOrDefault(),
                                            CESSValue = g.Select(x => x.CESSValue).Sum(),
                                            AddlCESSPercentage = g.Select(x => x.AddlCESSPercentage).FirstOrDefault(),
                                            AddlCESSValue = g.Select(x => x.AddlCESSValue).Sum(),
                                            CESSDescription = g.Select(x => x.CESSDescription).FirstOrDefault(),
                                            AddlCESSDescription = g.Select(x => x.AddlCESSDescription).FirstOrDefault(),
                                        }).ToList();
            getData.TTaxPayableR = getData.getTaxSummaryDet1.Select(x => x.TaxPayable).Sum();
            getData.TGrossAmountR = getData.getTaxSummaryDet1.Select(x => x.GrossAmount).Sum();
            getData.TTaxValueR = getData.getTaxSummaryDet1.Select(x => x.TaxValue).Sum();
            getData.TTax1ValueR = getData.getTaxSummaryDet1.Select(x => x.Tax1Value).Sum();
            getData.TTax2ValueR = getData.getTaxSummaryDet1.Select(x => x.Tax2Value).Sum();
            getData.TCESSValueR = getData.getTaxSummaryDet1.Select(x => x.CESSValue).Sum();
            getData.TAddlCESSValueR = getData.getTaxSummaryDet1.Select(x => x.AddlCESSValue).Sum();
            return getData;
        }


    }
}
