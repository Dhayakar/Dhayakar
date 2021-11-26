using System;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Helpers;

namespace WYNK.Data.Repository.Implementation
{
    public class PatientQueueRepository : RepositoryBase<PatientQueueViewModel>, IPatientQueueRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;


        public PatientQueueRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }


        public PatientQueueViewModel GetQueueDate(int CompanyID)
        {
            var Queuedata = new PatientQueueViewModel();
            var date = DateTime.UtcNow.Date;
            var utctime = CMPSContext.Setup.Where(x => x.CMPID == CompanyID).Select(x => x.UTCTime).FirstOrDefault();
            TimeSpan ts = TimeSpan.Parse(utctime);
            var wm = WYNKContext.Workflowmaster.Where(x => x.CMPID == CompanyID).ToList();
            var wt = WYNKContext.Workflowtran.Where(x => x.CMPID == CompanyID && x.Date.Date == date && x.status == true).ToList();
            var swt = WYNKContext.Workflowtran.Where(x => x.CMPID == CompanyID && x.Date.Date == date).ToList();
            var olm = CMPSContext.OneLineMaster.ToList();
            var rt = (from rm in WYNKContext.RegistrationTran.Where(x => x.CmpID == CompanyID)
                      join rrt in WYNKContext.Registration on rm.UIN equals rrt.UIN
                      select new
                      {
                          Regtran = rm.RegistrationTranID,
                          UIN = rrt.UIN,
                          name = rrt.Name,
                          mname = rrt.MiddleName,
                          lname = rrt.LastName,
                          regdatetime = rrt.DateofRegistration + ts,
                          dateofvisit = rm.DateofVisit + ts,
                          status = olm.Where(x => x.OLMID == Convert.ToInt32(rm.PatientVisitType)).Select(x => x.ParentDescription).FirstOrDefault(),
                          fees = rm.ConsulationFees,
                          gender = rrt.Gender,
                          age = PasswordEncodeandDecode.ToAgeString(rrt.DateofBirth)
                      }).ToList();

            Queuedata.Patientassignquedata = (from cc in wm.OrderBy(x => x.TrayOrder)
                                              select new Patientassignquedata
                                              {
                                                  Description = cc.Description,
                                                  subforms = (from wwm in wt.Where(x => x.Workflowmasterid == cc.ID)
                                                              select new subforms
                                                              {
                                                                  UIN = rt.Where(x => x.Regtran == wwm.Regtranid).Select(x => x.UIN).FirstOrDefault(),
                                                                  Name = rt.Where(x => x.Regtran == wwm.Regtranid).Select(x => x.name).FirstOrDefault(),
                                                                  Lname = rt.Where(x => x.Regtran == wwm.Regtranid).Select(x => x.lname).FirstOrDefault(),
                                                                  Reception = wwm.Reception,
                                                                  optometrist = wwm.Optometrist,
                                                                  checintime = wwm.Checkintime,
                                                                  checouttime = wwm.Checkouttime,
                                                                  regdatetime = rt.Where(x => x.Regtran == wwm.Regtranid).Select(x => x.dateofvisit).FirstOrDefault(),
                                                                  Patientstatus = rt.Where(x => x.Regtran == wwm.Regtranid).Select(x => x.status).FirstOrDefault(),
                                                                  Doctorname = CMPSContext.DoctorMaster.Where(x =>x.DoctorID == wwm.Doctorid).Select(x =>x.Firstname + " " + x.LastName).FirstOrDefault(),
                                                                  fees = rt.Where(x => x.Regtran == wwm.Regtranid).Select(x => x.fees).FirstOrDefault(),
                                                                  Age = rt.Where(x => x.Regtran == wwm.Regtranid).Select(x => x.age).FirstOrDefault(),
                                                                  Gender = rt.Where(x => x.Regtran == wwm.Regtranid).Select(x => x.gender).FirstOrDefault(),
                                                                  Patientcheckdetails = (from gg in swt.Where(x => x.Regtranid == wwm.Regtranid)
                                                                                         select new Patientcheckdetails
                                                                                         {
                                                                                             checkintime = gg.Checkintime,
                                                                                             checouttime = gg.Checkouttime,
                                                                                             waitingtime = Gettimediff(gg.Checkintime, gg.Checkouttime),
                                                                                             Role = WYNKContext.Workflowmaster.Where(x => x.ID == gg.Workflowmasterid).Select(x => x.Description).FirstOrDefault(),
                                                                                             date = gg.Date.ToString("dd-MMM-yyyy"),
                                                                                             Createdutc = gg.Createdutc,
                                                                                         }).OrderByDescending(x =>x.Createdutc).ToList(),
                                                              }).ToList(),

                                              }).ToList();
            return Queuedata;
        }
        public string Gettimediff(string checkin, string checkout)
        {
            var id = "";
            if (checkin != "" && checkout != "" && checkin != null && checkout != null)
            {
                var ts1 = TimeSpan.Parse(checkin);
                var ts2 = TimeSpan.Parse(checkout);
               var ids = (ts2 - ts1).Minutes;
                id = Convert.ToString(ids);
            }
            else
            {
                id = "";
            }
            return id;
        }

        public dynamic getdocname(int docid)
        {
            var id = "";

            var Referencetag = CMPSContext.Users.Where(x => x.Userid == docid).Select(x => x.ReferenceTag).FirstOrDefault();
            var Emailid = CMPSContext.Users.Where(x => x.Userid == docid).Select(x => x.Username).FirstOrDefault();
            if (Referencetag == "A")
            {
                id = Emailid;
            }
            else if (Referencetag == "D" || Referencetag == "V" || Referencetag == "O")
            {
                id = CMPSContext.DoctorMaster.Where(x => x.EmailID == Emailid).Select(x => x.LastName).FirstOrDefault();
            }
            else if (Referencetag == "E" || Referencetag == "R")
            {
                var EMpid = CMPSContext.EmployeeCommunication.Where(x => x.EmailID == Emailid).Select(x => x.EmpID).FirstOrDefault();
                id = CMPSContext.Employee.Where(x => x.EmployeeID == EMpid).Select(x => x.LastName).FirstOrDefault();
            }
            return id;
        }

    }
}


