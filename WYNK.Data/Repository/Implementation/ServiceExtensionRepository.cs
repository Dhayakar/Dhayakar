using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Helpers;

namespace WYNK.Data.Repository.Implementation
{
    public class ServiceExtensionRepository : RepositoryBase<ServiceExtensionViewModel>, IServiceExtensionRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;


        public ServiceExtensionRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }


        public ServiceExtensionViewModel GetInpatientBilleddtls(int cmpid)
        {


            var FinalBilling = new ServiceExtensionViewModel();

            var patientaccount = WYNKContext.PatientAccount.ToList();
            var registration = WYNKContext.Registration.ToList();
            var customermaster = WYNKContext.CustomerMaster.ToList();

            var reg = (from PA in patientaccount.Where(u => u.CMPID == cmpid && u.InvoiceNumber == null && u.InvoiceDate == null ).OrderByDescending(x => x.CreatedUTC)
                       join REG in registration on PA.UIN equals REG.UIN

                       select new
                       {
                           paid = PA.PAID,
                           uin = PA.UIN,
                           name = REG.Name + " " + REG.MiddleName + " " + REG.LastName,
                           gender = REG.Gender,
                           age = PasswordEncodeandDecode.ToAgeString(REG.DateofBirth),
                           address = REG.Address1,
                           phone = REG.Phone,

                       }).ToList();

            var Creg = (from PA in patientaccount.Where(u => u.CMPID == cmpid && u.InvoiceNumber == null && u.InvoiceDate == null ).OrderByDescending(x => x.CreatedUTC)
                        join CM in customermaster on PA.UIN equals Convert.ToString(CM.ID)
                        select new
                        {
                            paid = PA.PAID,
                            uin = PA.UIN,
                            name = CM.Name + " " + CM.MiddleName + " " + CM.LastName,
                            gender = "",
                            age = "",
                            address = CM.Address1,
                            phone = CM.PhoneNo,

                        }).ToList();

            var res = reg.Concat(Creg);

            FinalBilling.patientDtls = (from R in res.GroupBy(x => x.uin)
                                        select new patientDtls
                                        {
                                            paid = R.Select(x => x.paid).FirstOrDefault(),
                                            uin = R.Select(x => x.uin).FirstOrDefault(),
                                            name = R.Select(x => x.name).FirstOrDefault(),
                                            gender = R.Select(x => x.gender).FirstOrDefault(),
                                            age = R.Select(x => x.age).FirstOrDefault(),
                                            address = R.Select(x => x.address).FirstOrDefault(),
                                            phoneno = R.Select(x => x.phone).FirstOrDefault(),

                                        }).ToList();



            return FinalBilling;

        }


        public ServiceExtensionViewModel GetOutpatientBilleddtls(int cmpid)
        {
            var OutpatientBilleddtls = new ServiceExtensionViewModel();

            var onelinemaster = CMPSContext.OneLineMaster.AsNoTracking().ToList();
            var registration = WYNKContext.Registration.AsNoTracking().ToList();
            var regisrtattran = WYNKContext.RegistrationTran.AsNoTracking().ToList();
            var LocationMaster = CMPSContext.LocationMaster.AsNoTracking().ToList();
            var Company = CMPSContext.Company.AsNoTracking().ToList();
            var VisitType = CMPSContext.OneLineMaster.AsNoTracking().Where(c => c.ParentDescription == "New" && c.ParentTag == "TOV").Select(c => c.OLMID).FirstOrDefault();
           

            OutpatientBilleddtls.outpatient = (from R in registration.Where(x => x.CMPID==cmpid)
                                                                  join REGT in regisrtattran on R.UIN equals REGT.UIN
                                                                  join cmp in Company.Where(x => x.ParentID == cmpid || x.CmpID == cmpid) on R.CMPID equals cmp.CmpID
                                                                  where REGT.PatientVisitType == Convert.ToString(VisitType) && R.IsDeleted == false
                                                                  select new outpatient
                                                                  {
                                                                  paid = 0,
                                                                  uin = R.UIN,
                                                                  name = R.Name + " " + R.MiddleName + " " + R.LastName,
                                                                  gender = R.Gender,
                                                                  age = PasswordEncodeandDecode.ToAgeString(R.DateofBirth),
                                                                  address = R.Address1,
                                                                  phoneno = R.Phone,

                                        }).ToList();



            return OutpatientBilleddtls;

        }
   
    
    }



}





