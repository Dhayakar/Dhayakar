
using System;
using System.Collections.Generic;
using System.Linq;

using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Helpers;

namespace WYNK.Data.Repository.Implementation
{
    class CancellationReportRepository : RepositoryBase<CancellationViewM>, ICancellationReportRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        
        public CancellationReportRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public string GetConcatName(string Name)
        {
            var S = " ";
            var G = Name;
            if (G != null)
            {
                S = G;
            }

            return S;
        }
        public dynamic Todaysearch(DateTime FromDate, DateTime Todate, int CompanyID,int arrvalue)
        {
            var getData = new CancellationViewM();
            var getcancelled = new List<getPatientcancelledDet>();

            if(arrvalue == 1)
            {
                var Cndate = (from c in WYNKContext.Registration.Where(x => x.CMPID == CompanyID)
                                                  join rg in WYNKContext.RegistrationTran.Where(x => x.IsCancelled == true && Convert.ToDateTime(x.Cancelleddate).Date >= FromDate.Date
                                                  && Convert.ToDateTime(x.Cancelleddate).Date <= Todate.Date) on c.UIN equals rg.UIN
                                                  select new 
                                                  {
                                                      UIN = rg.UIN,
                                                      FName = GetConcatName(c.Name),
                                                      MName = GetConcatName(c.MiddleName),
                                                      lName = GetConcatName(c.LastName),
                                                      Gender = c.Gender,
                                                      Age = PasswordEncodeandDecode.ToAgeString(c.DateofBirth),
                                                      CancelledDateTime = Convert.ToDateTime(rg.Cancelleddate),
                                                      CancelledReasons = rg.CancellationReasons,
                                                      doctorid = rg.DoctorID,
                                                  }).ToList();



                foreach(var item in Cndate)
                {
                    var Newdate = new getPatientcancelledDet();
                    Newdate.UIN = item.UIN;
                    Newdate.FName = item.FName;
                    Newdate.MName = item.MName;
                    Newdate.lName = item.lName;
                    Newdate.Age = item.Age;
                    Newdate.Gender = item.Gender;
                    Newdate.CancelledReasons = item.CancelledReasons;
                    Newdate.CancelledDateTime = item.CancelledDateTime;

                    var Docname = CMPSContext.Users.Where(x => x.Userid == item.doctorid).Select(x => x.ReferenceTag).FirstOrDefault();
                    var Empname = CMPSContext.Users.Where(x => x.Userid == item.doctorid).Select(x => x.Username).FirstOrDefault();
                    if (Docname == "A")
                    {
                        Newdate.Doctorname = "Admin";
                        getcancelled.Add(Newdate);
                    }
                    else if (Docname == "R")
                    {
                        var empid = CMPSContext.EmployeeCommunication.Where(x => x.EmailID == Empname).Select(x => x.EmpID).FirstOrDefault();

                        var employeenamefirst = CMPSContext.Employee.Where(x => x.EmployeeID == empid).Select(x => x.FirstName).FirstOrDefault();
                        var employeenameMiddle = CMPSContext.Employee.Where(x => x.EmployeeID == empid).Select(x => x.MiddleName).FirstOrDefault();
                        var employeenameLast = CMPSContext.Employee.Where(x => x.EmployeeID == empid).Select(x => x.LastName).FirstOrDefault();

                        Newdate.Doctorname = employeenamefirst + ' ' + employeenameMiddle + ' ' + employeenameLast;
                        getcancelled.Add(Newdate);
                    }
                    
                }

         
            }
            else if (arrvalue == 2)
            {

                var Pa = (from pp in WYNKContext.PatientAssign.Where(x => Convert.ToDateTime(x.Cancelleddate).Date >= FromDate.Date
                          && Convert.ToDateTime(x.Cancelleddate).Date <= Todate.Date && x.IsCancelled == true)
                          join rt in WYNKContext.RegistrationTran on pp.RegistrationTranID equals rt.RegistrationTranID
                          join r in WYNKContext.Registration.Where(x =>x.CMPID == CompanyID) on rt.UIN equals r.UIN
                          select new
                          {
                              UIN = r.UIN,
                              FName = GetConcatName(r.Name),
                              MName = GetConcatName(r.MiddleName),
                              lName = GetConcatName(r.LastName),
                              Gender = r.Gender,
                              Age = PasswordEncodeandDecode.ToAgeString(r.DateofBirth),
                              CancelledDateTime = Convert.ToDateTime(pp.Cancelleddate),
                              CancelledReasons = pp.CancellationReasons,
                              doctorid = pp.DoctorID,
                          }).ToList();

                foreach(var item in Pa)
                {
                    var Newdate = new getPatientcancelledDet();
                    Newdate.UIN = item.UIN;
                    Newdate.FName = item.FName;
                    Newdate.MName = item.MName;
                    Newdate.lName = item.lName;
                    Newdate.Age = item.Age;
                    Newdate.Gender = item.Gender;
                    Newdate.CancelledReasons = item.CancelledReasons;
                    Newdate.CancelledDateTime = item.CancelledDateTime;

                    var Docname = CMPSContext.Users.Where(x => x.Userid == item.doctorid).Select(x => x.ReferenceTag).FirstOrDefault();
                    var Empname = CMPSContext.Users.Where(x => x.Userid == item.doctorid).Select(x => x.Username).FirstOrDefault();
                    if (Docname == "D")
                    {

                        var fname = CMPSContext.DoctorMaster.Where(x => x.EmailID == Empname).Select(x => x.Firstname).FirstOrDefault();
                        var Mname = CMPSContext.DoctorMaster.Where(x => x.EmailID == Empname).Select(x => x.MiddleName).FirstOrDefault();
                        var Lname = CMPSContext.DoctorMaster.Where(x => x.EmailID == Empname).Select(x => x.LastName).FirstOrDefault();

                        Newdate.Doctorname = fname + ' ' + Mname + ' ' + Lname;
                        getcancelled.Add(Newdate);
                    }
                }


            }
            else if (arrvalue == 3)
            {
                var Pa = (from pp in WYNKContext.PatientAssign.Where(x => Convert.ToDateTime(x.Cancelleddate).Date >= FromDate.Date
                         && Convert.ToDateTime(x.Cancelleddate).Date <= Todate.Date && x.IsCancelled == true)
                          join rt in WYNKContext.RegistrationTran on pp.RegistrationTranID equals rt.RegistrationTranID
                          join r in WYNKContext.Registration.Where(x => x.CMPID == CompanyID) on rt.UIN equals r.UIN
                          select new
                          {
                              UIN = r.UIN,
                              FName = GetConcatName(r.Name),
                              MName = GetConcatName(r.MiddleName),
                              lName = GetConcatName(r.LastName),
                              Gender = r.Gender,
                              Age = PasswordEncodeandDecode.ToAgeString(r.DateofBirth),
                              CancelledDateTime = Convert.ToDateTime(pp.Cancelleddate),
                              CancelledReasons = pp.CancellationReasons,
                              doctorid = pp.DoctorID,
                          }).ToList();

                foreach (var item in Pa)
                {
                    var Newdate = new getPatientcancelledDet();
                    Newdate.UIN = item.UIN;
                    Newdate.FName = item.FName;
                    Newdate.MName = item.MName;
                    Newdate.lName = item.lName;
                    Newdate.Age = item.Age;
                    Newdate.Gender = item.Gender;
                    Newdate.CancelledReasons = item.CancelledReasons;
                    Newdate.CancelledDateTime = item.CancelledDateTime;

                    var Docname = CMPSContext.Users.Where(x => x.Userid == item.doctorid).Select(x => x.ReferenceTag).FirstOrDefault();
                    var Empname = CMPSContext.Users.Where(x => x.Userid == item.doctorid).Select(x => x.Username).FirstOrDefault();
                    if (Docname == "O")
                    {

                        var fname = CMPSContext.DoctorMaster.Where(x => x.EmailID == Empname).Select(x => x.Firstname).FirstOrDefault();
                        var Mname = CMPSContext.DoctorMaster.Where(x => x.EmailID == Empname).Select(x => x.MiddleName).FirstOrDefault();
                        var Lname = CMPSContext.DoctorMaster.Where(x => x.EmailID == Empname).Select(x => x.LastName).FirstOrDefault();

                        Newdate.Doctorname = fname + ' ' + Mname + ' ' + Lname;
                        getcancelled.Add(Newdate);
                    }
                }

            }
            else if (arrvalue == 4)
            {
                var Pa = (from pp in WYNKContext.PatientAssign.Where(x => Convert.ToDateTime(x.Cancelleddate).Date >= FromDate.Date
                         && Convert.ToDateTime(x.Cancelleddate).Date <= Todate.Date && x.IsCancelled == true)
                          join rt in WYNKContext.RegistrationTran on pp.RegistrationTranID equals rt.RegistrationTranID
                          join r in WYNKContext.Registration.Where(x => x.CMPID == CompanyID) on rt.UIN equals r.UIN
                          select new
                          {
                              UIN = r.UIN,
                              FName = GetConcatName(r.Name),
                              MName = GetConcatName(r.MiddleName),
                              lName = GetConcatName(r.LastName),
                              Gender = r.Gender,
                              Age = PasswordEncodeandDecode.ToAgeString(r.DateofBirth),
                              CancelledDateTime = Convert.ToDateTime(pp.Cancelleddate),
                              CancelledReasons = pp.CancellationReasons,
                              doctorid = pp.DoctorID,
                          }).ToList();

                foreach (var item in Pa)
                {
                    var Newdate = new getPatientcancelledDet();
                    Newdate.UIN = item.UIN;
                    Newdate.FName = item.FName;
                    Newdate.MName = item.MName;
                    Newdate.lName = item.lName;
                    Newdate.Age = item.Age;
                    Newdate.Gender = item.Gender;
                    Newdate.CancelledReasons = item.CancelledReasons;
                    Newdate.CancelledDateTime = item.CancelledDateTime;

                    var Docname = CMPSContext.Users.Where(x => x.Userid == item.doctorid).Select(x => x.ReferenceTag).FirstOrDefault();
                    var Empname = CMPSContext.Users.Where(x => x.Userid == item.doctorid).Select(x => x.Username).FirstOrDefault();
                    if (Docname == "V")
                    {

                        var fname = CMPSContext.DoctorMaster.Where(x => x.EmailID == Empname).Select(x => x.Firstname).FirstOrDefault();
                        var Mname = CMPSContext.DoctorMaster.Where(x => x.EmailID == Empname).Select(x => x.MiddleName).FirstOrDefault();
                        var Lname = CMPSContext.DoctorMaster.Where(x => x.EmailID == Empname).Select(x => x.LastName).FirstOrDefault();

                        Newdate.Doctorname = fname + ' ' + Mname + ' ' + Lname;
                        getcancelled.Add(Newdate);
                    }
                }

            }

            getData.Companyname = CMPSContext.Company.Where(x => x.CmpID == CompanyID).Select(x => x.CompanyName).FirstOrDefault();
            getData.Fromdates = FromDate.Date.ToString("dd-MMM-yyyy");
            getData.Todated = Todate.Date.ToString("dd-MMM-yyyy");

            getData.getPatientcancelledDet = getcancelled;

            return getData;

        }


        }
}
