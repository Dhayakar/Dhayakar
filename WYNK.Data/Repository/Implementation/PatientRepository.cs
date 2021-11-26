using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository.Implementation
{
    class PatientRepository : RepositoryBase<PatientViewModel>, IPatientRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        

        public PatientRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public dynamic GetDetails(DateTime FromDate, DateTime ToDate, int CMPID)
        {

            var regisrtattran = WYNKContext.RegistrationTran.Where(x => x.CmpID == CMPID).ToList();
            var register = WYNKContext.Registration.Where(x =>x.CMPID == CMPID).ToList();

            var OneLine = CMPSContext.OneLineMaster.ToList();
            var tables = new PatientViewModel();

            var grtran = regisrtattran.GroupBy(x => x.UIN).Where(grp => grp.Count() > 1).Select(grp => grp.Key).ToList();

            tables.PatientList = (from e in regisrtattran
                                  where !grtran.Contains(e.UIN) && !(e.DateofVisit.Date >= FromDate.Date && e.DateofVisit.Date <= ToDate.Date)
                                  orderby e.DateofVisit.Date descending
                                  select new PatientList
                                  {
                                      UIN = e.UIN,
                                      Name = register.Where(x => x.UIN == e.UIN).Select(x => x.Name).FirstOrDefault(),
                                      Gender = register.Where(x => x.UIN == e.UIN).Select(x => x.Gender).FirstOrDefault(),
                                      DateofVisit = e.DateofVisit.Date,
                                      Address1 = register.Where(x => x.UIN == e.UIN).Select(x => x.Address1).FirstOrDefault(),
                                      Phone = register.Where(x => x.UIN == e.UIN).Select(x => x.Phone).FirstOrDefault(),
                                  }).ToList();

            tables.total = tables.PatientList.Count();


            return tables;

        }
    }
}


