
using System;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository.Implementation
{
    class GstTaxSummaryRepository : RepositoryBase<GstTaxSummaryViewM>, IGstTaxSummaryRepository
    {

        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        
        public GstTaxSummaryRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }


        public GstTaxSummaryViewM getTaxSummary(DateTime Date, int CompanyID)
        {


            var M_PatientAcc = WYNKContext.PatientAccount.ToList();
            var M_PatientAccDetTran = WYNKContext.PatientAccountDetail.ToList();
            var M_PatientAccDet = WYNKContext.PatientAccountDetailTax.ToList();
            var M_TaxMaster = CMPSContext.TaxMaster.ToList();


            var date = Convert.ToDateTime(Date).ToString("dd-MM-yyyy");

            var date1 = Convert.ToDateTime(Date).ToString("MM");

            var date2 = Convert.ToDateTime(Date).ToString("yyyy");

            int month = Convert.ToInt32(date1);
            int year = Convert.ToInt32(date2);

            DateTime first = new DateTime(year, month, 1);
            DateTime last = first.AddMonths(1).AddSeconds(-1);



            var lastdate = Convert.ToDateTime(last).ToString("dd-MM-yyyy");
           
            var getData = new GstTaxSummaryViewM();
            getData.getGSTSummary1 = (from M_PADT in M_PatientAccDet.Where(x => Convert.ToDateTime(x.CreatedUTC).Date >= Convert.ToDateTime(date) && Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(lastdate))

                                      join M_PAt in M_PatientAccDetTran
                                      on M_PADT.PAccDetailID equals M_PAt.PAccDetailID
                                      join M_PA in M_PatientAcc.Where(x => x.CMPID == CompanyID)
                                      on M_PAt.PAID equals M_PA.PAID

                                      join M_TM in M_TaxMaster.Where(x => x.TaxGroupId == 1)
                                      on M_PADT.TaxID equals M_TM.ID


                                      select new getGSTSummary1
                                      {
                                          TaxPercentage = M_PADT.TaxPercentage,
                                          TotalTurnOver = M_PADT.TotalProductValue - M_PADT.TotalDiscountValue,
                                          TotalGSTTaxValue = M_PADT.TotalTaxValue,
                                          TotalCGSTTaxValue = M_PADT.TotalCGSTTaxValue,
                                          TotalSGSTTaxValue = M_PADT.TotalSGSTTaxValue

                                      }).ToList();

            getData.getGSTSummary = (from val in getData.getGSTSummary1.GroupBy(x => x.TaxPercentage)

                                     select new getGSTSummary
                                     {
                                         Tax1 = val.Key,
                                         TotTurnOver = val.Sum(x => x.TotalTurnOver),
                                         TotGST = val.Sum(x => x.TotalGSTTaxValue),
                                         TotCGST = val.Sum(x => x.TotalCGSTTaxValue),
                                         TotSGST = val.Sum(x => x.TotalSGSTTaxValue),


                                     }).ToList();




            getData.getIGSTsummary1 = (from M_PADT in M_PatientAccDet.Where(x => Convert.ToDateTime(x.CreatedUTC).Date >= Convert.ToDateTime(date) && Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(lastdate))

                                       join M_TM in M_TaxMaster.Where(x => x.TaxGroupId == 2)
                                       on M_PADT.TaxID equals M_TM.ID


                                       select new getIGSTsummary1
                                       {
                                           TaxPercentage = M_PADT.TaxPercentage,
                                           TotalTurnOver = M_PADT.TotalProductValue - M_PADT.TotalDiscountValue,
                                           TotalIGSTTaxValue = M_PADT.TotalTaxValue

                                       }).ToList();


            getData.getIGSTsummary = (from val1 in getData.getIGSTsummary1.GroupBy(x => x.TaxPercentage)

                                      select new getIGSTsummary
                                      {
                                          Tax2 = val1.Key,
                                          TotTurnOver1 = val1.Sum(x => x.TotalTurnOver),
                                          TotalIGST = val1.Sum(x => x.TotalIGSTTaxValue)


                                      }).ToList();


            return getData;
        }


    }
}
