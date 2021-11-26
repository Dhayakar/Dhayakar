
using System;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Helpers;

namespace WYNK.Data.Repository.Implementation
{
    class IpOpticalBillingRepository : RepositoryBase<OpticalBilling>, IIpOpticalBillingRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        
        public IpOpticalBillingRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public OpticalBilling GetPatientDetails(int id)

        {

            var op = new OpticalBilling();
           
            return op;

        }

        public OpticalBilling GetOpticalDetails(int id, int rid)

        {

            var op = new OpticalBilling();
           
            return op;

        }

        public OpticalBilling GetLensDetails()

        {

            var ld = new OpticalBilling();
           
            return ld;

        }


        public dynamic UpdateOPBilling(OpticalBilling OpticalBilling, string UIN)
        {


            var opmas = new OpticalMaster();
           
            opmas.CreatedUTC = DateTime.UtcNow;

            WYNKContext.OpticalMaster.Add(opmas);

            WYNKContext.SaveChanges();

            var crby = opmas.CreatedBy;
            var upby = opmas.UpdatedBy;

            var res = (from im in WYNKContext.PatientAccount.Where(x => x.UIN == UIN && x.InvoiceNumber == null)
                       select new
                       {
                           rt = im.UIN.Count(),

                       }).ToList();

            var provalue = opmas.GrossProductValue;
            var disvalue = opmas.TotalDiscountValue;
            var taxvalue = opmas.TotalTaxValue;
            var billvalue = opmas.NetAmount;
            var cid = opmas.CMPID;

            if (res.Count() == 0)
            {

                var pa = new PatientAccount();

                pa.CMPID = cid;
                pa.UIN = UIN;
                pa.TotalProductValue = provalue;
                pa.TotalDiscountValue = disvalue;
                pa.TotalTaxValue = taxvalue;
                pa.TotalCGSTTaxValue= taxvalue/2;
                pa.TotalSGSTTaxValue = taxvalue / 2;
                pa.TotalBillValue = billvalue;
                pa.CreatedUTC = DateTime.UtcNow;
                pa.CreatedBy = crby;
                pa.UpdatedBy = upby;
                WYNKContext.Add(pa);
                WYNKContext.SaveChanges();

            }

            else
            {
                var masters = WYNKContext.PatientAccount.Where(x => x.UIN == UIN).LastOrDefault();
                if (masters != null)
                {

                    masters.TotalProductValue += provalue;
                    masters.TotalDiscountValue += disvalue;
                    masters.TotalTaxValue += taxvalue;
                    masters.TotalCGSTTaxValue += taxvalue/2;
                    masters.TotalSGSTTaxValue += taxvalue/2;
                    masters.TotalBillValue += billvalue;
                    WYNKContext.PatientAccount.UpdateRange(masters);

                }

            }



            var res1 = WYNKContext.PatientAccount.Where(x => x.UIN == UIN).Select(x => x.PAID).LastOrDefault();
            var res2 = WYNKContext.PatientAccountDetail.Where(x => x.PAID == res1 && x.ServiceDescription == "Opticalcharges").ToList();


            if (res2.Count() == 0)
            {

                var pat = new PatientAccountDetail();
                var rs1 = WYNKContext.PatientAccount.Where(x => x.UIN == UIN).Select(x => x.PAID).LastOrDefault();

                pat.PAID = rs1;
                pat.OLMID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Opticalcharges" && x.ParentTag == "Services").Select(x => x.OLMID).FirstOrDefault();
                pat.ServiceTypeID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Opticalcharges" && x.ParentTag == "Services").Select(x => x.OLMID).FirstOrDefault();
                pat.ServiceDescription = "Opticalcharges";
                pat.TotalProductValue = provalue;
                pat.TotalDiscountValue = disvalue;
                pat.TotalTaxValue = taxvalue;
                pat.TotalCGSTTaxValue = taxvalue/2;
                pat.TotalSGSTTaxValue = taxvalue/2;
                pat.TotalBillValue = billvalue;
                pat.CreatedUTC = DateTime.UtcNow;
                pat.CreatedBy = crby;
                pat.UpdatedBy = upby;
                WYNKContext.Add(pat);
                WYNKContext.SaveChanges();

            }


            else
            {

                var mast = WYNKContext.PatientAccount.Where(x => x.UIN == UIN).LastOrDefault();
                var resul = WYNKContext.PatientAccountDetail.Where(x => x.PAID == mast.PAID && x.ServiceDescription == "Opticalcharges").ToList();

                if (resul != null)
                {

                    resul.All(x => {
                        x.TotalProductValue += provalue;
                        x.TotalDiscountValue += disvalue;
                        x.TotalTaxValue += taxvalue;
                        x.TotalCGSTTaxValue += taxvalue / 2;
                        x.TotalSGSTTaxValue += taxvalue / 2;
                        x.TotalBillValue += billvalue;
                        return true;
                    });
                    WYNKContext.PatientAccountDetail.UpdateRange(resul);
                }


            }

            var serid = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Opticalcharges" && x.ParentTag == "Services").Select(x => x.OLMID).FirstOrDefault();


            var rs2 = WYNKContext.PatientAccount.Where(x => x.UIN == UIN).Select(x => x.PAID).LastOrDefault();
            var rs3 = WYNKContext.PatientAccountDetail.Where(x => x.PAID == rs2 && x.ServiceTypeID == serid).Select(x => x.PAccDetailID).LastOrDefault();
            

            try
            {
                if (WYNKContext.SaveChanges() >= 0)


                    return new
                    {

                        Success = true,
                        Message = CommonMessage.saved,
                    };
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Success = false,
                Message = CommonMessage.Missing,
            };

        }




    }
}






