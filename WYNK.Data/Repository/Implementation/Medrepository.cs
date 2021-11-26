using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Data.Repository.Operation;

namespace WYNK.Data.Repository.Implementation
{
    class Medrepository : RepositoryBase<Medical_Prescription>, IMed
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
       
        public Medrepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;
        }
        public dynamic getDetails(DateTime FromDate, DateTime Todate, int companyid,string GMT)
        {
            try
            {
                TimeSpan ts = TimeSpan.Parse(GMT);
                var MedicalPres = WYNKContext.MedicalPrescriptionTran.ToList();
                var DrugMaster =  WYNKContext.DrugMaster.ToList();

                var res = (from MedPres in WYNKContext.MedicalPrescription.Where(x=> (x.PrescribedDate + ts).Date >= Convert.ToDateTime(FromDate.ToString("yyyy-MM-dd")) && (x.PrescribedDate + ts).Date <= Convert.ToDateTime(Todate.ToString("yyyy-MM-dd")) && x.CmpID == companyid)
                           join MBT in WYNKContext.MedicalBillTran
                           on MedPres.RandomUniqueID equals MBT.MedicalPrescriptionID
                           into JoinData
                           from NewRes in JoinData.DefaultIfEmpty()
                           join MB in WYNKContext.MedicalBillMaster on NewRes.MedicalBillID equals MB.ID  into ss
                           from sss in ss.DefaultIfEmpty()
                           select new
                           {
                               PrescriptionNo = MedPres.MedicalPrescriptionNo,
                               Date = MedPres.PrescribedDate,
                               PrescribedBy = MedPres.PrescribedByName,
                               MedPresAmount = (from Med in MedicalPres.Where(x => x.MedicalPrescriptionID == MedPres.RandomUniqueID)
                                                join Drug in DrugMaster.Where(x => x.Cmpid == companyid) on Med.DrugId equals Drug.ID
                                                select new
                                                {
                                                    Amount = Med.Quantity * Drug.Rate
                                                }).Sum(x=>x.Amount),
                               BillNo = sss.BillNo != null ? sss.BillNo : "",
                               BillAmount = sss.GrossProductValue != null ? sss.GrossProductValue : (decimal?)null,
                               BillDate = sss.CreatedUTC != null ? sss.CreatedUTC : (DateTime?) null,
                           }).AsQueryable().AsNoTracking().ToList();
                return new 
                {
                    Success = true,
                    res= res
                };
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    ErrorLog oErrorLog = new ErrorLog();
                    oErrorLog.WriteErrorLog("Error", ex.InnerException.Message.ToString());
                    return new { Success = false };
                }
                else {
                    ErrorLog oErrorLog = new ErrorLog();
                    oErrorLog.WriteErrorLog("Error", ex.Message.ToString());
                    return new { Success = false };
                }
            }
        }
        public Medical_Prescription getDetailsbill(DateTime FromDate, DateTime Todate)
        {

            var bill = new Medical_Prescription();
            var regs = WYNKContext.Registration.ToList();
            var mper = WYNKContext.MedicalPrescription.ToList();
            var mtran = WYNKContext.MedicalPrescriptionTran.ToList();
            var drugmas = WYNKContext.DrugMaster.ToList();
            var fdate = Convert.ToDateTime(FromDate).ToString("dd-MMM-yyyy");
            var tdate = Convert.ToDateTime(Todate).ToString("dd-MMM-yyyy");

            bill.getdetailsbill = (from REG in regs
                                   join MB in mper.Where(x => Convert.ToDateTime(x.CreatedUTC).Date >= Convert.ToDateTime(fdate) && Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(tdate) && x.Status == "Closed" && x.IsDeleted == false)

                                   on REG.UIN equals MB.UIN
                                   join MTRAN in mtran
                                   on MB.RandomUniqueID equals MTRAN.MedicalPrescriptionID
                                   join DM in drugmas
                                   on MTRAN.DrugId equals DM.ID
                                   select new billed
                                   {
                                       date = MB.CreatedUTC,
                                       uin = MB.UIN,
                                       Patientname = REG.Name,
                                       gender = REG.Gender,
                                       PrescriptionNo = MB.MedicalPrescriptionNo,
                                       prescribedby = MB.PrescribedByName,
                                       Drugname = DM.Brand
                                   }).ToList();
            bill.getss = (from res in bill.getdetailsbill.GroupBy(x => x.date)
                          select new getbill
                          {
                              date = res.Key,
                              PrescriptionNo = res.Select(x => (x.PrescriptionNo)).FirstOrDefault(),
                              Patientname = res.Select(x => (x.Patientname)).FirstOrDefault(),
                              prescribedby = res.Select(x => (x.prescribedby)).FirstOrDefault()
                          }).ToList();

            bill.count = bill.getss.Count();
            return bill;
        }
        public Medical_Prescription getDetailspres(DateTime FromDate, DateTime Todate)
        {

            var regs = WYNKContext.Registration.ToList();
            var mper = WYNKContext.MedicalPrescription.ToList();
            var mtran = WYNKContext.MedicalPrescriptionTran.ToList();
            var drugmas = WYNKContext.DrugMaster.ToList();
            var fdate = Convert.ToDateTime(FromDate).ToString("dd-MMM-yyyy");
            var tdate = Convert.ToDateTime(Todate).ToString("dd-MMM-yyyy");
            var getpres = new Medical_Prescription();

            getpres.getpres = (from REG in regs
                               join MB in mper.Where(x => Convert.ToDateTime(x.CreatedUTC).Date >= Convert.ToDateTime(fdate) && Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(tdate))

                               on REG.UIN equals MB.UIN
                               join MTRAN in mtran
                               on MB.RandomUniqueID equals MTRAN.MedicalPrescriptionID
                               join DM in drugmas
                               on MTRAN.DrugId equals DM.ID
                               select new getpres
                               {
                                   date = MB.CreatedUTC,
                                   uin = MB.UIN,
                                   Patientname = REG.Name,
                                   gender = REG.Gender,
                                   PrescriptionNo = MB.MedicalPrescriptionNo,
                                   prescribedby = MB.PrescribedByName,                                   
                               }).ToList();

            getpres.getpress = (from res in getpres.getpres.GroupBy(x => x.date)
                                select new getpress
                                {

                                    date = res.Key,
                                    PrescriptionNo = res.Select(x => (x.PrescriptionNo)).FirstOrDefault(),
                                    Patientname = res.Select(x => (x.Patientname)).FirstOrDefault(),
                                    prescribedby = res.Select(x => (x.prescribedby)).FirstOrDefault()

                                }).ToList();
            getpres.count = getpres.getpress.Count();

            return getpres;


        }

    }
}


