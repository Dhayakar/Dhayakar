using System;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using System.Globalization;

namespace WYNK.Data.Repository.Implementation
{
    class Form3cRepository : RepositoryBase<Form3cViewModel>, IForm3cRepository
    {

        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        public Form3cRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;
        }
        public dynamic getConsultationSummary(Form3cViewModel Form3cViewModel, string FromDate, string Todate, int companyid, string GMT)
        {
            try
            {
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

                var reg = WYNKContext.Registration.Where(x => x.CMPID == companyid).ToList();
                var DocMast = CMPSContext.DoctorMaster.Where(x => x.CMPID == companyid).ToList();
                var PatientAssign = WYNKContext.PatientAssign.ToList();
                var RegistrationTrans = WYNKContext.RegistrationTran.Where(x => x.CmpID == companyid && Convert.ToDateTime(x.DateofVisit).Date >= fdate && Convert.ToDateTime(x.DateofVisit).Date <= tdate && x.ConsulationFees != null).ToList();
                var res = (from Cs in RegistrationTrans
                           join PA in PatientAssign.Where(x => x.DoctorID == Form3cViewModel.DoctorIds) on Cs.RegistrationTranID equals PA.RegistrationTranID
                           orderby Cs.DoctorID
                           select new
                           {
                               DoctorID = PA.DoctorID,
                               ReceiptDate = Cs.DateofVisit,
                               ReceiptNo = WYNKContext.PaymentMaster.Where(x => x.PaymentType == "O" && Convert.ToDateTime(x.InVoiceDate).Date == Cs.DateofVisit.Date && x.UIN == Cs.UIN).Select(x => x.ReceiptNumber).FirstOrDefault(),
                               PatientName = reg.Where(x => x.UIN == Cs.UIN).Select(x => String.Concat(x.Name, ' ', x.MiddleName, ' ', x.LastName)).FirstOrDefault(),
                               DoctorName = DocMast.Where(x => x.DoctorID == PA.DoctorID).Select(x => String.Concat(x.Firstname, ' ', x.MiddleName, ' ', x.LastName)).FirstOrDefault(),
                               FeesRecd = Cs.ConsulationFees,
                               UIN = Cs.UIN,
                               Service = "Consultation",
                               TotalAmount = (from TA in RegistrationTrans.Where(x => x.DoctorID == PA.DoctorID)
                                              group TA by TA.DoctorID into TotalAmountRes
                                              select new
                                              {
                                                  Consultation = TotalAmountRes.Select(x => x.ConsulationFees).ToList(),
                                              }).ToList().First().Consultation.Sum(x => x.Value),
                           }).ToList();

                return new
                {
                    Success = true,
                    res = res
                };

            }
            catch (Exception ex)
            {
                return new { Success = false };
            }

        }
    }
}
