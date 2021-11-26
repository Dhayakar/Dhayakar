
using System;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository.Implementation
{
    class Medservrepository : RepositoryBase<Medservviewmodel>, IMedservRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        
        public Medservrepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }
        public Medservviewmodel getDetails(DateTime FromDate, DateTime Todate,int companyid)
        {
           
            var patientaccount = WYNKContext.PatientAccount.ToList();
            var Medserv = new Medservviewmodel();
            var om = CMPSContext.OneLineMaster.ToList();
            var pad = WYNKContext.PatientAccountDetail.ToList();
            var fdate = Convert.ToDateTime(FromDate).ToString("dd-MMM-yyyy");
            var tdate = Convert.ToDateTime(Todate).ToString("dd-MMM-yyyy");

            Medserv.getDetail = (from PAD in pad.Where(x => Convert.ToDateTime(x.CreatedUTC).Date >= Convert.ToDateTime(fdate) && Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(tdate))  
                                 join OLM in om
                                 on PAD.OLMID equals OLM.OLMID
                                 join patientaccountt in patientaccount.Where(x=>x.CMPID== companyid)
                                 on PAD.PAID equals patientaccountt.PAID



                                 select new getDetail
                                 {
                                     services = OLM.ParentDescription,
                                     servicedes = PAD.ServiceDescription,
                                     totalamount = PAD.TotalBillValue
                                 }).ToList();



            Medserv.getSummary = (from res in Medserv.getDetail.GroupBy(x => x.services)
                                  select new getSummary
                                  {
                                      servicess = res.Key,
                                      totalamountt = res.Sum(x => (x.totalamount))



                                  }).ToList();





            return Medserv;
        }

        public Medservviewmodel getDetailsIn(DateTime FromDate, DateTime Todate,string service)
        {            
            var Medserve = new Medservviewmodel();
            var om = CMPSContext.OneLineMaster.ToList().Where(x => x.ParentDescription==service);
            var pad = WYNKContext.PatientAccountDetail.ToList();
            var fdate = Convert.ToDateTime(FromDate).ToString("dd-MMM-yyyy");
            var tdate = Convert.ToDateTime(Todate).ToString("dd-MMM-yyyy");

            Medserve.getDetailss = (from PAD in pad.Where(x => Convert.ToDateTime(x.CreatedUTC).Date >= Convert.ToDateTime(fdate) && Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(tdate))
                                       
                                    join OLM in om
                                    on PAD.OLMID equals OLM.OLMID
                                  

                                    select new getDetailss
                                    {

                                        services = OLM.ParentDescription,                                        
                                        servicedes = PAD.ServiceDescription,
                                        totalamount = PAD.TotalBillValue,
                                        datee=PAD.CreatedUTC

                                    }).ToList();
            Medserve.getsummaryy = (from res in Medserve.getDetailss.GroupBy(x => x.servicedes)
                                    select new getsummaryy
                                    {


                                        servicedess = res.Key,
                                        totalamountt = res.Sum(x => (x.totalamount)),
                                        date=res.Select(x=>(x.datee)).FirstOrDefault()
                                    }).ToList();


            return Medserve;
        }


    }
}

