
using System;
using System.Globalization;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;


namespace WYNK.Data.Repository.Implementation
{
    class MedicalBillRegisterRepository : RepositoryBase<BillingPharmacy>, IMedicalBillRegisterRepository
    {

        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        

        public MedicalBillRegisterRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public dynamic getMedBillDet(string FromDate, string Todate, int CompanyID)
        {
            try
            {
                var getMedBillReg = new MedicalBillRegisterDetails();

                var regs = WYNKContext.Registration.Where(x=>x.CMPID == CompanyID).ToList();
                var drug = WYNKContext.DrugMaster.Where(x => x.Cmpid == CompanyID).ToList();
                var mbill = WYNKContext.MedicalBillMaster.Where(x => x.CMPID == CompanyID).ToList();
                var mtran = WYNKContext.MedicalBillTran.ToList();
                var Tax = CMPSContext.TaxMaster.ToList();

                DateTime DT;
                var appdate = DateTime.TryParseExact(FromDate.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DT);
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

                getMedBillReg.getRegisterDetail = (from REG in regs
                                                   join MB in mbill
                                                   .Where(x => Convert.ToDateTime(x.CreatedUTC).Date >= fdate && Convert.ToDateTime(x.CreatedUTC).Date <= tdate && x.CMPID == CompanyID)
                                                   on REG.UIN equals MB.UIN
                                                   join MedTan in mtran on MB.ID equals MedTan.MedicalBillID
                                                   orderby MB.CreatedUTC

                                                   select new getRegDet
                                                   {
                                                       BillNo = MB.BillNo,
                                                       BillDate = MB.CreatedUTC.ToString("dd-MM-yyyy"),
                                                       PatientName = REG.Name,
                                                       Drug = drug.Where(x => x.ID == MedTan.DrugID).Select(x => x.Brand).FirstOrDefault(),
                                                       UOM = drug.Where(x => x.ID == MedTan.DrugID).Select(x => x.UOM).FirstOrDefault(),
                                                       Quantity = Convert.ToDecimal(MedTan.Quantity),
                                                       Amount = Convert.ToDecimal(MedTan.ItemValue),
                                                       DiscountPerc = Convert.ToDecimal(MedTan.DiscountPercentage),
                                                       DiscountAmount = Convert.ToDecimal(MedTan.DiscountAmount),
                                                       TaxDescription = Tax.Where(t => t.ID == drug.Where(x => x.ID == MedTan.DrugID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.TaxDescription).FirstOrDefault(),
                                                       CessDescription = Tax.Where(t => t.ID == drug.Where(x => x.ID == MedTan.DrugID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.CESSDescription).FirstOrDefault(),
                                                       AddCessDescription = Tax.Where(t => t.ID == drug.Where(x => x.ID == MedTan.DrugID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.AdditionalCESSDescription).FirstOrDefault(),
                                                       TaxPerc = MedTan.GSTPercentage,
                                                       CessPerc = MedTan.CESSPercentage,
                                                       AddCessPerc = MedTan.AdditionalCESSPercentage,
                                                       TaxValue = MedTan.GSTTaxValue,
                                                       CessValue = MedTan.CESSValue,
                                                       AddCessValue = MedTan.AdditionalCESSValue,
                                                       GrossAmount = Convert.ToDecimal(MedTan.ItemValue) - (MedTan.DiscountAmount!= null ? Convert.ToDecimal(MedTan.DiscountAmount) : 0),
                                                       NetAmount= MB.TotalBillValue,
                                                   }).ToList();


                          var MedTaxSummary =    (from opim in mbill.Where(x => Convert.ToDateTime(x.CreatedUTC).Date >= fdate
                                                  && Convert.ToDateTime(x.CreatedUTC).Date <= tdate && x.CMPID == CompanyID)
                                                  join opT in mtran on opim.ID equals opT.MedicalBillID
                                                  group opT by opT.TaxID into g

                                                       select new 
                                                       {
                                                           TaxID = g.Key,
                                                           TaxDescription = Tax.Where(x => x.ID == g.Select(s => s.TaxID).FirstOrDefault()).Select(x => x.TaxDescription).FirstOrDefault(),
                                                           TotalProductValue = g.Select(x => (x.ItemValue + x.GSTTaxValue + x.CESSValue + x.AdditionalCESSValue) - (x.DiscountAmount)).Sum(),
                                                           GSTTaxValue = g.Select(x => x.GSTTaxValue).Sum(),
                                                           CESSAmount = g.Select(x => x.CESSValue).Sum(),
                                                           AddCESSAmount = g.Select(x => x.AdditionalCESSValue).Sum(),
                                                           TaxPayable = g.Select(x => x.GSTTaxValue + x.CESSValue + x.AdditionalCESSValue).Sum(),
                                                           TaxableTurnover = g.Select(x => (x.ItemValue) - (x.DiscountAmount)).Sum(),
                                                       }).ToList();



                return new { Success = true, RegisterDetail = getMedBillReg.getRegisterDetail , MedTaxSummary = MedTaxSummary }; 
            }
            catch (Exception ex)
            {

                return new { Success = false, Message = "Something Went Wrong" };
            }
        }







    }

















}
