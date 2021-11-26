using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Helpers;

namespace WYNK.Data.Repository.Implementation
{
    class ManagementDashboardRepository : RepositoryBase<RegistrationMasterViewModel>, IManagementDashRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;

        public ManagementDashboardRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }
        public RegistrationMasterViewModel GetUsernamepasswordWITHPARENTIDZERO(string username, string password, string CompanyID, string Roldedescrip)
        {
            var loginstatus = new RegistrationMasterViewModel();
            var passwordss = PasswordEncodeandDecode.EncodePasswordToBase64(password);
            loginstatus.Users = CMPSContext.Users.Where(x => x.Username == username && x.Password == passwordss && x.Isactive == true).FirstOrDefault();
            DateTime fulldate = Convert.ToDateTime(DateTime.Now.Date.ToString("yyyy-MM-dd"));
            var FFFDate = fulldate.Date.ToString("yyyy-MM-dd");
            var passencrypt = PasswordEncodeandDecode.EncodePasswordToBase64(password);
            var COMPAnydetails = CMPSContext.Company.Where(x => x.CmpID == Convert.ToInt32(CompanyID)).FirstOrDefault();
            loginstatus.USERSCompanyID = COMPAnydetails.CmpID;
            loginstatus.ParentID = COMPAnydetails.ParentID;
            loginstatus.SELECTALLCompany = CompanyID;
            var userid = loginstatus.Users.Userid;
            loginstatus.FYearDetails = WYNKContext.FinancialYear.Where(x => x.IsActive == true && x.CMPID == Convert.ToInt32(CompanyID)).ToList();
            var setupdetails = CMPSContext.Setup.Where(x => x.CMPID == Convert.ToInt32(CompanyID)).FirstOrDefault();
            loginstatus.AlertNotification = Convert.ToString(setupdetails.IsNotification).ToLower();
            loginstatus.countryname = CMPSContext.Country.Where(x => x.ID == Convert.ToInt32(setupdetails.Country)).Select(x => x.CountryName).FirstOrDefault();
            loginstatus.BillingType = setupdetails.BillingType;
            loginstatus.RegistrationDateManage = setupdetails.RegistrationDateManage;

            if (loginstatus.Users.ReferenceTag == "A")
            {
                loginstatus.doctorname = username;
                loginstatus.ReferrenceIC = 0;
                loginstatus.CompanyID = loginstatus.USERSCompanyID;
                loginstatus.CompanyName = COMPAnydetails.CompanyName;
                loginstatus.Roleid = Convert.ToString(loginstatus.Users.ReferenceID);
                loginstatus.RoleDescription = CMPSContext.Role.Where(x => x.RoleID == Convert.ToInt32(loginstatus.Roleid)).Select(x => x.RoleDescription).FirstOrDefault();

                loginstatus.Referrencetag = loginstatus.Users.ReferenceTag;
                loginstatus.GMTUTCTime = CMPSContext.Setup.Where(x => x.CMPID == loginstatus.CompanyID).Select(x => x.UTCTime).FirstOrDefault();
                loginstatus.Userid = loginstatus.Users.Userid;
                if (loginstatus.GMTUTCTime != null)
                {
                    TimeSpan ts = TimeSpan.Parse(loginstatus.GMTUTCTime);
                    var menudatetime = DateTime.UtcNow.Add(ts);
                    loginstatus.Menulogindateandtime = menudatetime.ToString("dd-MMM-yyyy HH:mm");
                }
                else
                {
                    loginstatus.Menulogindateandtime = DateTime.UtcNow.ToString("dd-MMM-yyyy HH:mm");
                }

                loginstatus.MenuDoctorname = " " + loginstatus.doctorname;
                loginstatus.MenecompanyBranch = COMPAnydetails.Address1;
                var Companycountry = CMPSContext.Setup.Where(x => x.CMPID == loginstatus.CompanyID).Select(x => x.Country).FirstOrDefault();
                if (Companycountry != null)
                {
                    loginstatus.Compnayflag = CMPSContext.Country.Where(x => x.ID == Convert.ToInt32(Companycountry)).Select(x => x.CountryCode).FirstOrDefault().Substring(0, 2).ToLower();

                }


            }
            else
            {
                if (loginstatus.Users.Usertype == "NMS")
                {

                    loginstatus.CompanyID = loginstatus.USERSCompanyID;
                    loginstatus.GMTUTCTime = CMPSContext.Setup.Where(x => x.CMPID == loginstatus.CompanyID).Select(x => x.UTCTime).FirstOrDefault();
                    var empidd = CMPSContext.EmployeeCommunication.Where(x => x.EmailID == loginstatus.Users.Username && x.CmpID == loginstatus.CompanyID).Select(x => x.EmpID).FirstOrDefault();
                    loginstatus.doctorname = CMPSContext.Employee.Where(x => x.EmployeeID == empidd).Select(x => x.FirstName).FirstOrDefault();
                    loginstatus.ReferrenceIC = empidd;
                    loginstatus.CompanyName = COMPAnydetails.CompanyName;
                    loginstatus.Roleid = CMPSContext.UserVsRole.Where(x => x.UserID == loginstatus.Users.Userid && x.CMPID == loginstatus.CompanyID).Select(x => Convert.ToString(x.RoleID)).FirstOrDefault();
                    loginstatus.RoleDescription = CMPSContext.Role.Where(x => x.RoleID == Convert.ToInt32(loginstatus.Roleid)).Select(x => x.RoleDescription).FirstOrDefault();
                    loginstatus.Referrencetag = CMPSContext.Users.Where(x => x.Userid == loginstatus.Users.Userid && x.CMPID == loginstatus.CompanyID).Select(x => x.ReferenceTag).FirstOrDefault();
                    loginstatus.MenuDoctorTitle = CMPSContext.Employee.Where(x => x.EmployeeID == empidd).Select(x => x.Title).FirstOrDefault();
                    loginstatus.MenuDoctorname = loginstatus.MenuDoctorTitle + " . " + loginstatus.doctorname;
                    if (loginstatus.GMTUTCTime != null)
                    {
                        TimeSpan ts = TimeSpan.Parse(loginstatus.GMTUTCTime);
                        var menudatetime = DateTime.UtcNow.Add(ts);
                        loginstatus.Menulogindateandtime = menudatetime.ToString("dd-MMM-yyyy HH:mm");
                    }
                    else
                    {
                        loginstatus.Menulogindateandtime = DateTime.UtcNow.ToString("dd-MMM-yyyy HH:mm");
                    }
                    loginstatus.MenecompanyBranch = COMPAnydetails.Address1;
                    var Companycountry = CMPSContext.Setup.Where(x => x.CMPID == loginstatus.CompanyID).Select(x => x.Country).FirstOrDefault();
                    if (Companycountry != null)
                    {
                        loginstatus.Compnayflag = CMPSContext.Country.Where(x => x.ID == Convert.ToInt32(Companycountry)).Select(x => x.CountryCode).FirstOrDefault().Substring(0, 2).ToLower();

                    }
                    loginstatus.Userid = loginstatus.Users.Userid;

                }

                else if (loginstatus.Users.Usertype == "MS")
                {
                    loginstatus.CompanyID = loginstatus.USERSCompanyID;
                    loginstatus.GMTUTCTime = CMPSContext.Setup.Where(x => x.CMPID == loginstatus.USERSCompanyID).Select(x => x.UTCTime).FirstOrDefault();

                    loginstatus.GMTUTCTime = setupdetails.UTCTime;
                    loginstatus.ReferrenceIC = CMPSContext.DoctorMaster.Where(x => x.EmailID == loginstatus.Users.Username && x.CMPID == loginstatus.CompanyID).Select(x => x.DoctorID).FirstOrDefault();
                    loginstatus.CompanyName = COMPAnydetails.CompanyName;
                    loginstatus.doctorname = CMPSContext.DoctorMaster.Where(x => x.EmailID == loginstatus.Users.Username && x.CMPID == loginstatus.CompanyID).Select(x => x.Firstname + " " + x.MiddleName + " " + x.LastName).FirstOrDefault();
                    loginstatus.Registrationnumber = CMPSContext.DoctorMaster.Where(x => x.DoctorID == loginstatus.Users.ReferenceID && x.CMPID == loginstatus.CompanyID).Select(x => x.RegistrationNumber).FirstOrDefault();
                    loginstatus.Roleid = CMPSContext.UserVsRole.Where(x => x.UserID == loginstatus.Users.Userid && x.CMPID == loginstatus.CompanyID).Select(x => Convert.ToString(x.RoleID)).FirstOrDefault();
                    loginstatus.RoleDescription = CMPSContext.Role.Where(x => x.RoleID == Convert.ToInt32(loginstatus.Roleid)).Select(x => x.RoleDescription).FirstOrDefault();
                    loginstatus.Referrencetag = loginstatus.Users.ReferenceTag;
                    loginstatus.MenuDoctorTitle = CMPSContext.DoctorMaster.Where(x => x.EmailID == loginstatus.Users.Username && x.CMPID == loginstatus.CompanyID).Select(x => x.Title).FirstOrDefault();
                    loginstatus.MenuDoctorname = loginstatus.MenuDoctorTitle + " . " + loginstatus.doctorname;
                    if (loginstatus.GMTUTCTime != null)
                    {
                        TimeSpan ts = TimeSpan.Parse(loginstatus.GMTUTCTime);
                        var menudatetime = DateTime.UtcNow.Add(ts);
                        loginstatus.Menulogindateandtime = menudatetime.ToString("dd-MMM-yyyy HH:mm");
                    }
                    else
                    {
                        loginstatus.Menulogindateandtime = DateTime.UtcNow.ToString("dd-MMM-yyyy HH:mm");
                    }
                    loginstatus.Userid = loginstatus.Users.Userid;

                    loginstatus.MenuDoctorname = loginstatus.MenuDoctorTitle + " . " + loginstatus.doctorname;
                    loginstatus.MenecompanyBranch = COMPAnydetails.Address1;

                    var Companycountry = CMPSContext.Setup.Where(x => x.CMPID == loginstatus.CompanyID).Select(x => x.Country).FirstOrDefault();

                    if (Companycountry != null)
                    {
                        loginstatus.Compnayflag = CMPSContext.Country.Where(x => x.ID == Convert.ToInt32(Companycountry)).Select(x => x.CountryCode).FirstOrDefault().Substring(0, 2).ToLower();

                    }
                }
            }
            return loginstatus;
        }



        public RegistrationMasterViewModel GetSpecificperiodPatientpopulationdetails(string FromDate, string ToDate, int CompanyID, string Phase)
        {
            var KEYPPD = new RegistrationMasterViewModel();


            DateTime TT;
            DateTime FT;
            var appdate = DateTime.TryParseExact(FromDate.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out TT);
            {
                TT.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            }
            var apptdate = DateTime.TryParseExact(ToDate.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out FT);
            {
                FT.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            }
            var KPIFFFDate = TT;
            var KPITODate = FT;
            var KPIDescriptionnew = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "New").Select(x => x.OLMID).FirstOrDefault();
            var KPIDescriptionReview = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Review").Select(x => x.OLMID).FirstOrDefault();
            var KPISUrgeryReviewDesc = WYNKContext.Admission.Where(x => x.IsSurgeryCompleted == true).FirstOrDefault();
            if (Phase == "ALL")
            {
                KEYPPD.POPUPPatientpopulationdetails = (from REGTRAN in WYNKContext.RegistrationTran.Where(x => x.DateofVisit.Date >= KPIFFFDate
                       && x.DateofVisit.Date <= KPITODate
                       && x.PatientVisitType == Convert.ToString(KPIDescriptionnew) && x.PatientVisitType != Convert.ToString(KPIDescriptionReview))
                                                        join Reg in WYNKContext.Registration.Where(x => x.CMPID == CompanyID) on REGTRAN.UIN equals Reg.UIN
                                                        select new POPUPPatientpopulationdetails
                                                        {
                                                            UIN = REGTRAN.UIN,
                                                            Name = Reg.Name,
                                                            Phone = Reg.Phone,
                                                            Address = Reg.Address1,
                                                            gender = Reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(Reg.DateofBirth),
                                                            email = Reg.EmailID,
                                                        }).ToList();
                var KPINewList = (from REGTRAN in WYNKContext.RegistrationTran.Where(x => x.DateofVisit.Date >= KPIFFFDate
                                  && x.DateofVisit.Date <= KPITODate
                                  && x.PatientVisitType == Convert.ToString(KPIDescriptionnew) && x.PatientVisitType != Convert.ToString(KPIDescriptionReview))
                                  join reg in WYNKContext.Registration.Where(x => x.CMPID == CompanyID) on REGTRAN.UIN equals reg.UIN
                                  select new
                                  {
                                      UIN = REGTRAN.UIN.Count(),
                                  }).ToList();
                KEYPPD.KPINEWFORTHEDAY = KPINewList.Count();
                KEYPPD.REVIEWPOPUPPatientpopulationdetails = (from REGTRAN in WYNKContext.RegistrationTran.Where(x => x.DateofVisit.Date >= KPIFFFDate
&& x.DateofVisit.Date <= KPITODate
&& x.PatientVisitType == Convert.ToString(KPIDescriptionReview) && x.PatientVisitType != Convert.ToString(KPIDescriptionnew))
                                                              join Reg in WYNKContext.Registration.Where(x => x.CMPID == CompanyID) on REGTRAN.UIN equals Reg.UIN
                                                              select new REVIEWPOPUPPatientpopulationdetails
                                                              {
                                                                  UIN = REGTRAN.UIN,
                                                                  Name = Reg.Name,
                                                                  Phone = Reg.Phone,
                                                                  Address = Reg.Address1,
                                                                  gender = Reg.Gender,
                                                                  Age = PasswordEncodeandDecode.ToAgeString(Reg.DateofBirth),
                                                                  email = Reg.EmailID,
                                                              }).ToList();
                var KPIReviewList = (from REGTRAN in WYNKContext.RegistrationTran.Where(x => x.DateofVisit.Date >= KPIFFFDate
                               && x.DateofVisit.Date <= KPITODate
                               && x.PatientVisitType == Convert.ToString(KPIDescriptionReview) && x.PatientVisitType != Convert.ToString(KPIDescriptionnew))
                                     join reg in WYNKContext.Registration.Where(x => x.CMPID == CompanyID) on REGTRAN.UIN equals reg.UIN
                                     select new
                                     {
                                         UIN = REGTRAN.UIN.Count(),
                                     }).ToList();
                KEYPPD.KPINEWMONTHTILLDATE = KPIReviewList.Count();
                KEYPPD.SURGERYPOPUPPatientpopulationdetails = (from ADM in WYNKContext.Admission.Where(x => x.AdmDate.Date >= KPIFFFDate
                            && x.AdmDate.Date <= KPITODate && x.IsSurgeryCompleted == true)
                                                               join reg in WYNKContext.Registration.Where(x => x.CMPID == CompanyID) on ADM.UIN equals reg.UIN
                                                               select new SURGERYPOPUPPatientpopulationdetails
                                                               {
                                                                   UIN = ADM.UIN,
                                                                   Name = reg.Name,
                                                                   Phone = reg.Phone,
                                                                   Address = reg.Address1,
                                                                   gender = reg.Gender,
                                                                   Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                                   email = reg.EmailID,
                                                               }).ToList();
                var KPISURGERYReviewList = (from ADM in WYNKContext.Admission.Where(x => x.AdmDate.Date >= KPIFFFDate
                              && x.AdmDate.Date <= KPITODate && x.IsSurgeryCompleted == true)
                                            join reg in WYNKContext.Registration.Where(x => x.CMPID == CompanyID) on ADM.UIN equals reg.UIN
                                            select new
                                            {
                                                UIN = ADM.UIN.Count(),
                                            }).ToList();
                KEYPPD.KPISURGERYREVIEWFORTHEDAY = KPISURGERYReviewList.Count();
            }
            else if (Phase == "New")
            {
                KEYPPD.POPUPPatientpopulationdetails = (from REGTRAN in WYNKContext.RegistrationTran.Where(x => x.DateofVisit.Date >= KPIFFFDate
                            && x.DateofVisit.Date <= KPITODate
                            && x.PatientVisitType == Convert.ToString(KPIDescriptionnew) && x.PatientVisitType != Convert.ToString(KPIDescriptionReview))
                                                        join Reg in WYNKContext.Registration.Where(x => x.CMPID == CompanyID) on REGTRAN.UIN equals Reg.UIN
                                                        select new POPUPPatientpopulationdetails
                                                        {
                                                            UIN = REGTRAN.UIN,
                                                            Name = Reg.Name,
                                                            Phone = Reg.Phone,
                                                            Address = Reg.Address1,
                                                            gender = Reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(Reg.DateofBirth),
                                                            email = Reg.EmailID,
                                                        }).ToList();
                var KPINewList = (from REGTRAN in WYNKContext.RegistrationTran.Where(x => x.DateofVisit.Date >= KPIFFFDate
                                  && x.DateofVisit.Date <= KPITODate
                                  && x.PatientVisitType == Convert.ToString(KPIDescriptionnew) && x.PatientVisitType != Convert.ToString(KPIDescriptionReview))
                                  join reg in WYNKContext.Registration.Where(x => x.CMPID == CompanyID) on REGTRAN.UIN equals reg.UIN
                                  select new
                                  {
                                      UIN = REGTRAN.UIN.Count(),
                                  }).ToList();
                KEYPPD.KPINEWFORTHEDAY = KPINewList.Count();
            }
            else if (Phase == "Review")
            {
                KEYPPD.REVIEWPOPUPPatientpopulationdetails = (from REGTRAN in WYNKContext.RegistrationTran.Where(x => x.DateofVisit.Date >= KPIFFFDate
          && x.DateofVisit.Date <= KPITODate
          && x.PatientVisitType == Convert.ToString(KPIDescriptionReview) && x.PatientVisitType != Convert.ToString(KPIDescriptionnew))
                                                              join Reg in WYNKContext.Registration.Where(x => x.CMPID == CompanyID) on REGTRAN.UIN equals Reg.UIN
                                                              select new REVIEWPOPUPPatientpopulationdetails
                                                              {
                                                                  UIN = REGTRAN.UIN,
                                                                  Name = Reg.Name,
                                                                  Phone = Reg.Phone,
                                                                  Address = Reg.Address1,
                                                                  gender = Reg.Gender,
                                                                  Age = PasswordEncodeandDecode.ToAgeString(Reg.DateofBirth),
                                                                  email = Reg.EmailID,
                                                              }).ToList();
                var KPIReviewList = (from REGTRAN in WYNKContext.RegistrationTran.Where(x => x.DateofVisit.Date >= KPIFFFDate
                               && x.DateofVisit.Date <= KPITODate
                               && x.PatientVisitType == Convert.ToString(KPIDescriptionReview) && x.PatientVisitType != Convert.ToString(KPIDescriptionnew))
                                     join reg in WYNKContext.Registration.Where(x => x.CMPID == CompanyID) on REGTRAN.UIN equals reg.UIN
                                     select new
                                     {
                                         UIN = REGTRAN.UIN.Count(),
                                     }).ToList();
                KEYPPD.KPINEWMONTHTILLDATE = KPIReviewList.Count();
            }
            else
            {
                KEYPPD.SURGERYPOPUPPatientpopulationdetails = (from ADM in WYNKContext.Admission.Where(x => x.AdmDate.Date >= KPIFFFDate
                              && x.AdmDate.Date <= KPITODate && x.IsSurgeryCompleted == true)
                                                               join reg in WYNKContext.Registration.Where(x => x.CMPID == CompanyID) on ADM.UIN equals reg.UIN
                                                               select new SURGERYPOPUPPatientpopulationdetails
                                                               {
                                                                   UIN = ADM.UIN,
                                                                   Name = reg.Name,
                                                                   Phone = reg.Phone,
                                                                   Address = reg.Address1,
                                                                   gender = reg.Gender,
                                                                   Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                                   email = reg.EmailID,
                                                               }).ToList();
                var KPISURGERYReviewList = (from ADM in WYNKContext.Admission.Where(x => x.AdmDate.Date >= KPIFFFDate
                              && x.AdmDate.Date <= KPITODate && x.IsSurgeryCompleted == true)
                                            join reg in WYNKContext.Registration.Where(x => x.CMPID == CompanyID) on ADM.UIN equals reg.UIN
                                            select new
                                            {
                                                UIN = ADM.UIN.Count(),
                                            }).ToList();
                KEYPPD.KPISURGERYREVIEWFORTHEDAY = KPISURGERYReviewList.Count();
            }
            KEYPPD.KPIDFDATE = FromDate;
            KEYPPD.KPITDDAY = ToDate;
            KEYPPD.KPIDATAPOPULATIONTOTAL = KEYPPD.KPINEWFORTHEDAY + KEYPPD.KPINEWMONTHTILLDATE + KEYPPD.KPISURGERYREVIEWFORTHEDAY;
            return KEYPPD;
        }
        public RegistrationMasterViewModel GetMonthPatientpopulationdetails(string FromDate, string ToDate, int CompanyID, string phase)
        {
            var KEYMonthPPD = new RegistrationMasterViewModel();

            DateTime FT;
            var appdate = DateTime.TryParseExact(FromDate.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out FT);
            {
                FT.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            }

            DateTime TT;
            var appTdate = DateTime.TryParseExact(ToDate.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out TT);
            {
                TT.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            }


            var monthFrmDate = FT;
            var monthTODate = TT;
            var KPIDescriptionnewMONTH = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "New").Select(x => x.OLMID).FirstOrDefault();
            var KPIDescriptionReviewMONTH = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Review").Select(x => x.OLMID).FirstOrDefault();
            var KPISUrgeryReviewDescMONTH = WYNKContext.Admission.Where(x => x.IsSurgeryCompleted == true).FirstOrDefault();
            if (phase == "ALL")
            {
                var MonthFromNew = (from REGTRAN in WYNKContext.RegistrationTran
                          .Where(x => Convert.ToDateTime(x.DateofVisit.Date.ToString("yyyy-MM")) == Convert.ToDateTime(monthFrmDate.ToString("yyyy-MM"))
                          && x.PatientVisitType == Convert.ToString(KPIDescriptionnewMONTH) && x.PatientVisitType != Convert.ToString(KPIDescriptionReviewMONTH))
                                    join reg in WYNKContext.Registration.Where(x => x.CMPID == CompanyID) on REGTRAN.UIN equals reg.UIN
                                    select new
                                    {
                                        UIN = REGTRAN.UIN,
                                    }).ToList();
                var MonthToNew = (from REGTRAN in WYNKContext.RegistrationTran
                         .Where(x => Convert.ToDateTime(x.DateofVisit.Date.ToString("yyyy-MM")) == Convert.ToDateTime(monthTODate.ToString("yyyy-MM"))
                          && x.PatientVisitType == Convert.ToString(KPIDescriptionnewMONTH) && x.PatientVisitType != Convert.ToString(KPIDescriptionReviewMONTH))
                                  join reg in WYNKContext.Registration.Where(x => x.CMPID == CompanyID) on REGTRAN.UIN equals reg.UIN
                                  select new
                                  {
                                      UIN = REGTRAN.UIN,
                                  }).ToList();

                var MonthFROMReview = (from REGTRAN in WYNKContext.RegistrationTran
                                .Where(x => Convert.ToDateTime(x.DateofVisit.Date.ToString("yyyy-MM")) == Convert.ToDateTime(monthFrmDate.ToString("yyyy-MM"))
                                 && x.PatientVisitType == Convert.ToString(KPIDescriptionReviewMONTH) && x.PatientVisitType != Convert.ToString(KPIDescriptionnewMONTH))
                                       join reg in WYNKContext.Registration.Where(x => x.CMPID == CompanyID) on REGTRAN.UIN equals reg.UIN
                                       select new
                                       {
                                           UIN = REGTRAN.UIN,
                                       }).ToList();
                var MonthTOReview = (from REGTRAN in WYNKContext.RegistrationTran
                               .Where(x => Convert.ToDateTime(x.DateofVisit.Date.ToString("yyyy-MM")) == Convert.ToDateTime(monthTODate.ToString("yyyy-MM"))
                                && x.PatientVisitType == Convert.ToString(KPIDescriptionReviewMONTH) && x.PatientVisitType != Convert.ToString(KPIDescriptionnewMONTH))
                                     join reg in WYNKContext.Registration.Where(x => x.CMPID == CompanyID) on REGTRAN.UIN equals reg.UIN
                                     select new
                                     {
                                         UIN = REGTRAN.UIN,
                                     }).ToList();

                var MonthFROMSurgery = (from ADM in WYNKContext.Admission.Where(x => Convert.ToDateTime(x.AdmDate.Date.ToString("yyyy-MM")) == Convert.ToDateTime(monthFrmDate.ToString("yyyy-MM"))
                                    && x.IsSurgeryCompleted == true)
                                        join reg in WYNKContext.Registration.Where(x => x.CMPID == CompanyID) on ADM.UIN equals reg.UIN
                                        select new
                                        {
                                            UIN = ADM.UIN,
                                        }).ToList();


                var MonthTOSurgery = (from ADM in WYNKContext.Admission
                                      .Where(x => Convert.ToDateTime(x.AdmDate.Date.ToString("yyyy-MM")) == Convert.ToDateTime(monthTODate.ToString("yyyy-MM"))
                                    && x.IsSurgeryCompleted == true)
                                      join reg in WYNKContext.Registration.Where(x => x.CMPID == CompanyID) on ADM.UIN equals reg.UIN
                                      select new
                                      {
                                          UIN = ADM.UIN,
                                      }).ToList();

                KEYMonthPPD.MonthNewFromDate = MonthFromNew.Count();
                KEYMonthPPD.MonthNewTODate = MonthToNew.Count();
                KEYMonthPPD.MonthReviewFromdate = MonthFROMReview.Count();
                KEYMonthPPD.MonthReviewToDate = MonthTOReview.Count();
                KEYMonthPPD.MonthSurgeryFromDate = MonthFROMSurgery.Count();
                KEYMonthPPD.MonthSurgeryToDate = MonthTOSurgery.Count();
                KEYMonthPPD.MonthKPIFromDate = FromDate;
                KEYMonthPPD.MonthKPIToDate = ToDate;

            }
            else if (phase == "New")
            {


                KEYMonthPPD.MonthFromPOPUPPatientpopulationdetails = (from REGTRAN in WYNKContext.RegistrationTran
                                .Where(x => Convert.ToDateTime(x.DateofVisit.Date.ToString("yyyy-MM"))
                                == Convert.ToDateTime(monthFrmDate.ToString("yyyy-MM"))
                                && x.PatientVisitType == Convert.ToString(KPIDescriptionnewMONTH) && x.PatientVisitType != Convert.ToString(KPIDescriptionReviewMONTH))
                                                                      join reg in WYNKContext.Registration.Where(x => x.CMPID == CompanyID) on REGTRAN.UIN equals reg.UIN
                                                                      select new MonthPOPUPPatientpopulationdetails
                                                                      {
                                                                          UIN = REGTRAN.UIN,
                                                                          Name = reg.Name,
                                                                          gender = reg.Gender,
                                                                          Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                                          Phone = reg.Phone,
                                                                          Address = reg.Address1,
                                                                          email = reg.EmailID
                                                                      }).ToList();
                KEYMonthPPD.MonthTOPOPUPPatientpopulationdetails = (from REGTRAN in WYNKContext.RegistrationTran
                       .Where(x => Convert.ToDateTime(x.DateofVisit.Date.ToString("yyyy-MM"))
                       == Convert.ToDateTime(monthTODate.ToString("yyyy-MM"))
                       && x.PatientVisitType == Convert.ToString(KPIDescriptionnewMONTH) && x.PatientVisitType != Convert.ToString(KPIDescriptionReviewMONTH))
                                                                    join reg in WYNKContext.Registration.Where(x => x.CMPID == CompanyID) on REGTRAN.UIN equals reg.UIN
                                                                    select new MonthTOPOPUPPatientpopulationdetails
                                                                    {
                                                                        UIN = REGTRAN.UIN,
                                                                        Name = reg.Name,
                                                                        gender = reg.Gender,
                                                                        Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                                        Phone = reg.Phone,
                                                                        Address = reg.Address1,
                                                                        email = reg.EmailID
                                                                    }).ToList();
            }
            else if (phase == "Review")
            {

                KEYMonthPPD.MonthFromREVIEWPOPUPPatientpopulationdetails = (from REGTRAN in WYNKContext.RegistrationTran
                         .Where(x => Convert.ToDateTime(x.DateofVisit.Date.ToString("yyyy-MM"))
                         == Convert.ToDateTime(monthFrmDate.ToString("yyyy-MM"))
                         && x.PatientVisitType == Convert.ToString(KPIDescriptionReviewMONTH) && x.PatientVisitType != Convert.ToString(KPIDescriptionnewMONTH))
                                                                            join reg in WYNKContext.Registration.Where(x => x.CMPID == CompanyID) on REGTRAN.UIN equals reg.UIN

                                                                            select new MonthREVIEWPOPUPPatientpopulationdetails
                                                                            {
                                                                                UIN = REGTRAN.UIN,
                                                                                Name = reg.Name,
                                                                                gender = reg.Gender,
                                                                                Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                                                Phone = reg.Phone,
                                                                                Address = reg.Address1,
                                                                                email = reg.EmailID
                                                                            }).ToList();

                KEYMonthPPD.MonthTOREVIEWPOPUPPatientpopulationdetails = (from REGTRAN in WYNKContext.RegistrationTran
             .Where(x => Convert.ToDateTime(x.DateofVisit.Date.ToString("yyyy-MM"))
             == Convert.ToDateTime(monthTODate.ToString("yyyy-MM"))
             && x.PatientVisitType == Convert.ToString(KPIDescriptionReviewMONTH) && x.PatientVisitType != Convert.ToString(KPIDescriptionnewMONTH))
                                                                          join reg in WYNKContext.Registration.Where(x => x.CMPID == CompanyID) on REGTRAN.UIN equals reg.UIN
                                                                          select new MonthTOREVIEWPOPUPPatientpopulationdetails
                                                                          {
                                                                              UIN = REGTRAN.UIN,
                                                                              Name = reg.Name,
                                                                              gender = reg.Gender,
                                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                                              Phone = reg.Phone,
                                                                              Address = reg.Address1,
                                                                              email = reg.EmailID
                                                                          }).ToList();
            }
            else
            {
                KEYMonthPPD.MonthFromSURGERYPOPUPPatientpopulationdetails = (from ADM in WYNKContext.Admission.Where(x => Convert.ToDateTime(x.AdmDate.Date.ToString("yyyy-MM")) == Convert.ToDateTime(monthFrmDate.ToString("yyyy-MM"))
                    && x.IsSurgeryCompleted == true)
                                                                             join reg in WYNKContext.Registration.Where(x => x.CMPID == CompanyID) on ADM.UIN equals reg.UIN
                                                                             select new MonthSURGERYPOPUPPatientpopulationdetails
                                                                             {
                                                                                 UIN = ADM.UIN,
                                                                                 Name = reg.Name,
                                                                                 gender = reg.Gender,
                                                                                 Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                                                 Phone = reg.Phone,
                                                                                 Address = reg.Address1,
                                                                                 email = reg.EmailID
                                                                             }).ToList();

                KEYMonthPPD.MonthTOSURGERYPOPUPPatientpopulationdetails = (from ADM in WYNKContext.Admission.
                Where(x => Convert.ToDateTime(x.AdmDate.Date.ToString("yyyy-MM")) == Convert.ToDateTime(monthTODate.ToString("yyyy-MM"))
                                    && x.IsSurgeryCompleted == true)
                                                                           join reg in WYNKContext.Registration.Where(x => x.CMPID == CompanyID) on ADM.UIN equals reg.UIN
                                                                           select new MonthTOSURGERYPOPUPPatientpopulationdetails
                                                                           {
                                                                               UIN = ADM.UIN,
                                                                               Name = reg.Name,
                                                                               gender = reg.Gender,
                                                                               Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                                               Phone = reg.Phone,
                                                                               Address = reg.Address1,
                                                                               email = reg.EmailID
                                                                           }).ToList();
            }
            KEYMonthPPD.MonthKPIFromDate = FromDate;
            KEYMonthPPD.MonthKPIToDate = ToDate;
            return KEYMonthPPD;
        }

        public ManagementDashboardViewModel GetHalfyearlycomparisionPatientpopulationdetails(string selecetdmonth, string selectedyear,
            string selectedtomonth, string selectedtoyear, int CompanyID)
        {
            var HalfyearlyPPD = new ManagementDashboardViewModel();
            var NewVisit = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "New").Select(x => x.OLMID).FirstOrDefault();
            var ReviewVisit = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Review").Select(x => x.OLMID).FirstOrDefault();
            var Qreg = WYNKContext.Registration.Where(x => x.CMPID == CompanyID).ToList();
            var Qregt = WYNKContext.RegistrationTran.ToList();
            var Qsur = WYNKContext.Admission.Where(x => x.CMPID == CompanyID).ToList();


            if (selecetdmonth == "Jan - Jun")
            {

                string mnthname = "January";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;


                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                HalfyearlyPPD.newcount = Newq.Count();
                HalfyearlyPPD.Reviewcount = Reviewq.Count();
                HalfyearlyPPD.SurgeryReviewcount = SurReviewq.Count();
            }
            else if (selecetdmonth == "Feb - Jul")
            {
                string mnthname = "February";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;


                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                HalfyearlyPPD.newcount = Newq.Count();
                HalfyearlyPPD.Reviewcount = Reviewq.Count();
                HalfyearlyPPD.SurgeryReviewcount = SurReviewq.Count();
            }
            else if (selecetdmonth == "Mar - Aug")
            {
                string mnthname = "March";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;


                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                HalfyearlyPPD.newcount = Newq.Count();
                HalfyearlyPPD.Reviewcount = Reviewq.Count();
                HalfyearlyPPD.SurgeryReviewcount = SurReviewq.Count();
            }
            else if (selecetdmonth == "Apr - Sept")
            {
                string mnthname = "April";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;


                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                HalfyearlyPPD.newcount = Newq.Count();
                HalfyearlyPPD.Reviewcount = Reviewq.Count();
                HalfyearlyPPD.SurgeryReviewcount = SurReviewq.Count();
            }
            else if (selecetdmonth == "May - Oct")
            {
                string mnthname = "May";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;


                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                HalfyearlyPPD.newcount = Newq.Count();
                HalfyearlyPPD.Reviewcount = Reviewq.Count();
                HalfyearlyPPD.SurgeryReviewcount = SurReviewq.Count();
            }
            else if (selecetdmonth == "Jun - Nov")
            {
                string mnthname = "June";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;


                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                HalfyearlyPPD.newcount = Newq.Count();
                HalfyearlyPPD.Reviewcount = Reviewq.Count();
                HalfyearlyPPD.SurgeryReviewcount = SurReviewq.Count();
            }
            else if (selecetdmonth == "Jul - Dec")
            {
                string mnthname = "July";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;


                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                HalfyearlyPPD.newcount = Newq.Count();
                HalfyearlyPPD.Reviewcount = Reviewq.Count();
                HalfyearlyPPD.SurgeryReviewcount = SurReviewq.Count();
            }
            else if (selecetdmonth == "Aug - Jan")
            {
                string mnthname = "August";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;


                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                HalfyearlyPPD.newcount = Newq.Count();
                HalfyearlyPPD.Reviewcount = Reviewq.Count();
                HalfyearlyPPD.SurgeryReviewcount = SurReviewq.Count();
            }
            else if (selecetdmonth == "Sept - Feb")
            {
                string mnthname = "September";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;


                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                HalfyearlyPPD.newcount = Newq.Count();
                HalfyearlyPPD.Reviewcount = Reviewq.Count();
                HalfyearlyPPD.SurgeryReviewcount = SurReviewq.Count();
            }
            else if (selecetdmonth == "Oct - Mar")
            {
                string mnthname = "October";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;


                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                HalfyearlyPPD.newcount = Newq.Count();
                HalfyearlyPPD.Reviewcount = Reviewq.Count();
                HalfyearlyPPD.SurgeryReviewcount = SurReviewq.Count();
            }
            else if (selecetdmonth == "Nov - Apr")
            {
                string mnthname = "November";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;


                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                HalfyearlyPPD.newcount = Newq.Count();
                HalfyearlyPPD.Reviewcount = Reviewq.Count();
                HalfyearlyPPD.SurgeryReviewcount = SurReviewq.Count();
            }
            else if (selecetdmonth == "Dec - May")
            {
                string mnthname = "December";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;


                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                HalfyearlyPPD.newcount = Newq.Count();
                HalfyearlyPPD.Reviewcount = Reviewq.Count();
                HalfyearlyPPD.SurgeryReviewcount = SurReviewq.Count();
            }

            if (selectedtomonth == "Jan - Jun")
            {
                //var FromHalf = Convert.ToDateTime("01/01/" + selectedyear);

                string mnthname = "January";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;


                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedtoyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                HalfyearlyPPD.TOnewcount = Newq.Count();
                HalfyearlyPPD.TOReviewcount = Reviewq.Count();
                HalfyearlyPPD.TOSurgeryReviewcount = SurReviewq.Count();
            }
            else if (selectedtomonth == "Feb - Jul")
            {
                string mnthname = "February";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;


                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedtoyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                HalfyearlyPPD.TOnewcount = Newq.Count();
                HalfyearlyPPD.TOReviewcount = Reviewq.Count();
                HalfyearlyPPD.TOSurgeryReviewcount = SurReviewq.Count();
            }
            else if (selectedtomonth == "Mar - Aug")
            {
                string mnthname = "March";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;


                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedtoyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                HalfyearlyPPD.TOnewcount = Newq.Count();
                HalfyearlyPPD.TOReviewcount = Reviewq.Count();
                HalfyearlyPPD.TOSurgeryReviewcount = SurReviewq.Count();
            }
            else if (selectedtomonth == "Apr - Sept")
            {
                string mnthname = "April";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;


                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedtoyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                HalfyearlyPPD.TOnewcount = Newq.Count();
                HalfyearlyPPD.TOReviewcount = Reviewq.Count();
                HalfyearlyPPD.TOSurgeryReviewcount = SurReviewq.Count();
            }
            else if (selectedtomonth == "May - Oct")
            {
                string mnthname = "May";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;


                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedtoyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                HalfyearlyPPD.TOnewcount = Newq.Count();
                HalfyearlyPPD.TOReviewcount = Reviewq.Count();
                HalfyearlyPPD.TOSurgeryReviewcount = SurReviewq.Count();
            }
            else if (selectedtomonth == "Jun - Nov")
            {
                string mnthname = "June";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;


                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedtoyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                HalfyearlyPPD.TOnewcount = Newq.Count();
                HalfyearlyPPD.TOReviewcount = Reviewq.Count();
                HalfyearlyPPD.TOSurgeryReviewcount = SurReviewq.Count();
            }
            else if (selectedtomonth == "Jul - Dec")
            {
                string mnthname = "July";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;


                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedtoyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                HalfyearlyPPD.TOnewcount = Newq.Count();
                HalfyearlyPPD.TOReviewcount = Reviewq.Count();
                HalfyearlyPPD.TOSurgeryReviewcount = SurReviewq.Count();
            }
            else if (selectedtomonth == "Aug - Jan")
            {
                string mnthname = "August";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;


                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedtoyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                HalfyearlyPPD.TOnewcount = Newq.Count();
                HalfyearlyPPD.TOReviewcount = Reviewq.Count();
                HalfyearlyPPD.TOSurgeryReviewcount = SurReviewq.Count();
            }
            else if (selectedtomonth == "Sept - Feb")
            {
                string mnthname = "September";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;


                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedtoyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                HalfyearlyPPD.TOnewcount = Newq.Count();
                HalfyearlyPPD.TOReviewcount = Reviewq.Count();
                HalfyearlyPPD.TOSurgeryReviewcount = SurReviewq.Count();
            }
            else if (selectedtomonth == "Oct - Mar")
            {
                string mnthname = "October";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;


                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedtoyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                HalfyearlyPPD.TOnewcount = Newq.Count();
                HalfyearlyPPD.TOReviewcount = Reviewq.Count();
                HalfyearlyPPD.TOSurgeryReviewcount = SurReviewq.Count();
            }
            else if (selectedtomonth == "Nov - Apr")
            {
                string mnthname = "November";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;


                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedtoyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                HalfyearlyPPD.TOnewcount = Newq.Count();
                HalfyearlyPPD.TOReviewcount = Reviewq.Count();
                HalfyearlyPPD.TOSurgeryReviewcount = SurReviewq.Count();
            }
            else if (selectedtomonth == "Dec - May")
            {
                string mnthname = "December";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;


                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedtoyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                HalfyearlyPPD.TOnewcount = Newq.Count();
                HalfyearlyPPD.TOReviewcount = Reviewq.Count();
                HalfyearlyPPD.TOSurgeryReviewcount = SurReviewq.Count();
            }

            return HalfyearlyPPD;
        }
        public ManagementDashboardViewModel GetAnnualcomparisionPatientpopulationdetails(string selecetdmonth, string selectedyear,
            string selectedtomonth, string selectedtoyear, int CompanyID)
        {
            var AnnualPPD = new ManagementDashboardViewModel();
            var NewVisit = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "New").Select(x => x.OLMID).FirstOrDefault();
            var ReviewVisit = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Review").Select(x => x.OLMID).FirstOrDefault();
            var Qreg = WYNKContext.Registration.Where(x => x.CMPID == CompanyID).ToList();
            var Qregt = WYNKContext.RegistrationTran.ToList();
            var Qsur = WYNKContext.Admission.Where(x => x.CMPID == CompanyID).ToList();


            if (selecetdmonth == "Jan to Dec")
            {
                // var FromHalf = Convert.ToDateTime("01/01/" + selectedyear);

                string mnthname = "January";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;
                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                AnnualPPD.newcount = Newq.Count();
                AnnualPPD.Reviewcount = Reviewq.Count();
                AnnualPPD.SurgeryReviewcount = SurReviewq.Count();
            }
            else if (selecetdmonth == "Feb to Jan")
            {
                string mnthname = "February";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;
                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                AnnualPPD.newcount = Newq.Count();
                AnnualPPD.Reviewcount = Reviewq.Count();
                AnnualPPD.SurgeryReviewcount = SurReviewq.Count();
            }
            else if (selecetdmonth == "Mar to Feb")
            {
                string mnthname = "March";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;
                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                AnnualPPD.newcount = Newq.Count();
                AnnualPPD.Reviewcount = Reviewq.Count();
                AnnualPPD.SurgeryReviewcount = SurReviewq.Count();
            }
            else if (selecetdmonth == "Apr to Mar")
            {
                string mnthname = "April";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;
                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                AnnualPPD.newcount = Newq.Count();
                AnnualPPD.Reviewcount = Reviewq.Count();
                AnnualPPD.SurgeryReviewcount = SurReviewq.Count();
            }
            else if (selecetdmonth == "May to Apr")
            {
                string mnthname = "May";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;
                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                AnnualPPD.newcount = Newq.Count();
                AnnualPPD.Reviewcount = Reviewq.Count();
                AnnualPPD.SurgeryReviewcount = SurReviewq.Count();
            }
            else if (selecetdmonth == "Jun to May")
            {
                string mnthname = "June";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;
                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                AnnualPPD.newcount = Newq.Count();
                AnnualPPD.Reviewcount = Reviewq.Count();
                AnnualPPD.SurgeryReviewcount = SurReviewq.Count();
            }
            else if (selecetdmonth == "Jul to Jun")
            {
                string mnthname = "July";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;
                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                AnnualPPD.newcount = Newq.Count();
                AnnualPPD.Reviewcount = Reviewq.Count();
                AnnualPPD.SurgeryReviewcount = SurReviewq.Count();
            }
            else if (selecetdmonth == "Aug to Jul")
            {
                string mnthname = "August";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;
                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                AnnualPPD.newcount = Newq.Count();
                AnnualPPD.Reviewcount = Reviewq.Count();
                AnnualPPD.SurgeryReviewcount = SurReviewq.Count();
            }
            else if (selecetdmonth == "Sept to Aug")
            {
                string mnthname = "September";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;
                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                AnnualPPD.newcount = Newq.Count();
                AnnualPPD.Reviewcount = Reviewq.Count();
                AnnualPPD.SurgeryReviewcount = SurReviewq.Count();
            }
            else if (selecetdmonth == "Oct to Sept")
            {
                string mnthname = "October";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;
                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                AnnualPPD.newcount = Newq.Count();
                AnnualPPD.Reviewcount = Reviewq.Count();
                AnnualPPD.SurgeryReviewcount = SurReviewq.Count();
            }
            else if (selecetdmonth == "Nov to Oct")
            {
                string mnthname = "November";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;
                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                AnnualPPD.newcount = Newq.Count();
                AnnualPPD.Reviewcount = Reviewq.Count();
                AnnualPPD.SurgeryReviewcount = SurReviewq.Count();
            }
            else if (selecetdmonth == "Dec to Nov")
            {
                string mnthname = "December";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;
                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                AnnualPPD.newcount = Newq.Count();
                AnnualPPD.Reviewcount = Reviewq.Count();
                AnnualPPD.SurgeryReviewcount = SurReviewq.Count();
            }


            if (selectedtomonth == "Jan to Dec")
            {
                // var FromHalf = Convert.ToDateTime("01/01/" + selectedyear);

                string mnthname = "January";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;
                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedtoyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                AnnualPPD.TOnewcount = Newq.Count();
                AnnualPPD.TOReviewcount = Reviewq.Count();
                AnnualPPD.TOSurgeryReviewcount = SurReviewq.Count();
            }
            else if (selectedtomonth == "Feb to Jan")
            {
                string mnthname = "February";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;
                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedtoyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                AnnualPPD.TOnewcount = Newq.Count();
                AnnualPPD.TOReviewcount = Reviewq.Count();
                AnnualPPD.TOSurgeryReviewcount = SurReviewq.Count();
            }
            else if (selectedtomonth == "Mar to Feb")
            {
                string mnthname = "March";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;
                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedtoyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                AnnualPPD.TOnewcount = Newq.Count();
                AnnualPPD.TOReviewcount = Reviewq.Count();
                AnnualPPD.TOSurgeryReviewcount = SurReviewq.Count();
            }
            else if (selectedtomonth == "Apr to Mar")
            {
                string mnthname = "April";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;
                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedtoyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                AnnualPPD.TOnewcount = Newq.Count();
                AnnualPPD.TOReviewcount = Reviewq.Count();
                AnnualPPD.TOSurgeryReviewcount = SurReviewq.Count();
            }
            else if (selectedtomonth == "May to Apr")
            {
                string mnthname = "May";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;
                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedtoyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                AnnualPPD.TOnewcount = Newq.Count();
                AnnualPPD.TOReviewcount = Reviewq.Count();
                AnnualPPD.TOSurgeryReviewcount = SurReviewq.Count();
            }
            else if (selectedtomonth == "Jun to May")
            {
                string mnthname = "June";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;
                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedtoyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                AnnualPPD.TOnewcount = Newq.Count();
                AnnualPPD.TOReviewcount = Reviewq.Count();
                AnnualPPD.TOSurgeryReviewcount = SurReviewq.Count();
            }
            else if (selectedtomonth == "Jul to Jun")
            {
                string mnthname = "July";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;
                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedtoyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                AnnualPPD.TOnewcount = Newq.Count();
                AnnualPPD.TOReviewcount = Reviewq.Count();
                AnnualPPD.TOSurgeryReviewcount = SurReviewq.Count();
            }
            else if (selectedtomonth == "Aug to Jul")
            {
                string mnthname = "August";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;
                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedtoyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                AnnualPPD.TOnewcount = Newq.Count();
                AnnualPPD.TOReviewcount = Reviewq.Count();
                AnnualPPD.TOSurgeryReviewcount = SurReviewq.Count();
            }
            else if (selectedtomonth == "Sept to Aug")
            {
                string mnthname = "September";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;
                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedtoyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                AnnualPPD.TOnewcount = Newq.Count();
                AnnualPPD.TOReviewcount = Reviewq.Count();
                AnnualPPD.TOSurgeryReviewcount = SurReviewq.Count();
            }
            else if (selectedtomonth == "Oct to Sept")
            {
                string mnthname = "October";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;
                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedtoyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                AnnualPPD.TOnewcount = Newq.Count();
                AnnualPPD.TOReviewcount = Reviewq.Count();
                AnnualPPD.TOSurgeryReviewcount = SurReviewq.Count();
            }
            else if (selectedtomonth == "Nov to Oct")
            {
                string mnthname = "November";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;
                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedtoyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                AnnualPPD.TOnewcount = Newq.Count();
                AnnualPPD.TOReviewcount = Reviewq.Count();
                AnnualPPD.TOSurgeryReviewcount = SurReviewq.Count();
            }
            else if (selectedtomonth == "Dec to Nov")
            {
                string mnthname = "December";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;
                var Newq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
&& x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                            join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();
                var Reviewq = (from Qregtt in Qregt
.Where(x => x.DateofVisit.Month >= FromHalf
&& x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
   && x.PatientVisitType == Convert.ToString(ReviewVisit) && x.PatientVisitType != Convert.ToString(NewVisit))
                               join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
.Where(x => x.AdmDate.Month >= FromHalf
&& x.AdmDate.Month <= TotalDays && x.IsSurgeryCompleted == true && x.AdmDate.Year == Convert.ToInt32(selectedtoyear))
                                  join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                AnnualPPD.TOnewcount = Newq.Count();
                AnnualPPD.TOReviewcount = Reviewq.Count();
                AnnualPPD.TOSurgeryReviewcount = SurReviewq.Count();
            }

            return AnnualPPD;
        }
        public ManagementDashboardViewModel GetHalfPiechartcomparisionPatientpopulationdetails(string selecetdmonth, string selectedyear,
        string selectedtomonth, string selectedtoyear, int CompanyID, string id)
        {
            var PiechartQuaerterly = new ManagementDashboardViewModel();
            var NewVisit = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "New").Select(x => x.OLMID).FirstOrDefault();
            var ReviewVisit = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Review").Select(x => x.OLMID).FirstOrDefault();
            var Qreg = WYNKContext.Registration.Where(x => x.CMPID == CompanyID).ToList();
            var Qregt = WYNKContext.RegistrationTran.ToList();
            var Qsur = WYNKContext.Admission.Where(x => x.CMPID == CompanyID).ToList();

            if (selecetdmonth == "Jan - Jun")
            {
                string mnthname = "January";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedyear) && x.IsSurgeryCompleted == true)
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
            }
            else if (selecetdmonth == "Feb - Jul")
            {
                string mnthname = "February";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedyear) && x.IsSurgeryCompleted == true)
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
            }
            else if (selecetdmonth == "Mar - Aug")
            {
                string mnthname = "March";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedyear) && x.IsSurgeryCompleted == true)
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
            }
            else if (selecetdmonth == "Apr - Sept")
            {
                string mnthname = "April";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedyear) && x.IsSurgeryCompleted == true)
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
            }
            else if (selecetdmonth == "May - Oct")
            {
                string mnthname = "May";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedyear) && x.IsSurgeryCompleted == true)
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
            }
            else if (selecetdmonth == "Jun - Nov")
            {
                string mnthname = "June";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedyear) && x.IsSurgeryCompleted == true)
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }

            }
            else if (selecetdmonth == "Jul - Dec")
            {
                string mnthname = "July";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedyear) && x.IsSurgeryCompleted == true)
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
            }
            else if (selecetdmonth == "Aug - Jan")
            {
                string mnthname = "August";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedyear) && x.IsSurgeryCompleted == true)
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
            }
            else if (selecetdmonth == "Sept - Feb")
            {
                string mnthname = "September";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedyear) && x.IsSurgeryCompleted == true)
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
            }
            else if (selecetdmonth == "Oct - Mar")
            {
                string mnthname = "October";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedyear) && x.IsSurgeryCompleted == true)
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
            }
            else if (selecetdmonth == "Nov - Apr")
            {
                string mnthname = "November";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedyear) && x.IsSurgeryCompleted == true)
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
            }
            else if (selecetdmonth == "Dec - May")
            {
                string mnthname = "December";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedyear) && x.IsSurgeryCompleted == true)
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }

            }

            //Quarterly count to

            if (selectedtomonth == "Jan - Jun")
            {
                string mnthname = "January";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedtoyear) && x.IsSurgeryCompleted == true)
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
            }
            else if (selectedtomonth == "Feb - Jul")
            {
                string mnthname = "February";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedtoyear) && x.IsSurgeryCompleted == true)
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
            }
            else if (selectedtomonth == "Mar - Aug")
            {
                string mnthname = "March";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedtoyear) && x.IsSurgeryCompleted == true)
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
            }
            else if (selectedtomonth == "Apr - Sept")
            {
                string mnthname = "April";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedtoyear) && x.IsSurgeryCompleted == true)
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
            }
            else if (selectedtomonth == "May - Oct")
            {
                string mnthname = "May";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedtoyear) && x.IsSurgeryCompleted == true)
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
            }
            else if (selectedtomonth == "Jun - Nov")
            {
                string mnthname = "June";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedtoyear) && x.IsSurgeryCompleted == true)
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
            }
            else if (selectedtomonth == "Jul - Dec")
            {
                string mnthname = "July";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedtoyear) && x.IsSurgeryCompleted == true)
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
            }
            else if (selectedtomonth == "Aug - Jan")
            {
                string mnthname = "August";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedtoyear) && x.IsSurgeryCompleted == true)
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
            }
            else if (selectedtomonth == "Sept - Feb")
            {
                string mnthname = "September";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedtoyear) && x.IsSurgeryCompleted == true)
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
            }
            else if (selectedtomonth == "Oct - Mar")
            {
                string mnthname = "October";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedtoyear) && x.IsSurgeryCompleted == true)
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
            }
            else if (selectedtomonth == "Nov - Apr")
            {
                string mnthname = "November";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedtoyear) && x.IsSurgeryCompleted == true)
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
            }
            else if (selectedtomonth == "Dec - May")
            {
                string mnthname = "December";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 5;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedtoyear) && x.IsSurgeryCompleted == true)
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
            }


            return PiechartQuaerterly;
        }
        public ManagementDashboardViewModel GetAnnualPiechartcomparisionPatientpopulationdetails(string selecetdmonth, string selectedyear,
        string selectedtomonth, string selectedtoyear, int CompanyID, string id)
        {
            var PiechartQuaerterly = new ManagementDashboardViewModel();
            var NewVisit = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "New").Select(x => x.OLMID).FirstOrDefault();
            var ReviewVisit = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Review").Select(x => x.OLMID).FirstOrDefault();
            var Qreg = WYNKContext.Registration.Where(x => x.CMPID == CompanyID).ToList();
            var Qregt = WYNKContext.RegistrationTran.ToList();
            var Qsur = WYNKContext.Admission.Where(x => x.CMPID == CompanyID).ToList();


            ///////ANnaually///////////////////////////////////////////////////////////////////////////////////

            if (selecetdmonth == "Jan to Dec")
            {
                string mnthname = "January";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedyear) && x.IsSurgeryCompleted == true)
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
            }
            else if (selecetdmonth == "Feb to Jan")
            {
                string mnthname = "February";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedyear) && x.IsSurgeryCompleted == true)
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
            }
            else if (selecetdmonth == "Mar to Feb")
            {
                string mnthname = "March";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedyear) && x.IsSurgeryCompleted == true)
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
            }
            else if (selecetdmonth == "Apr to Mar")
            {
                string mnthname = "April";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedyear) && x.IsSurgeryCompleted == true)
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
            }
            else if (selecetdmonth == "May to Apr")
            {
                string mnthname = "May";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedyear) && x.IsSurgeryCompleted == true)
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
            }
            else if (selecetdmonth == "Jun to May")
            {
                string mnthname = "June";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedyear) && x.IsSurgeryCompleted == true)
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }

            }
            else if (selecetdmonth == "Jul to Jun")
            {
                string mnthname = "July";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedyear) && x.IsSurgeryCompleted == true)
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
            }
            else if (selecetdmonth == "Aug to Jul")
            {
                string mnthname = "August";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedyear) && x.IsSurgeryCompleted == true)
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
            }
            else if (selecetdmonth == "Sept to Aug")
            {
                string mnthname = "September";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedyear) && x.IsSurgeryCompleted == true)
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
            }
            else if (selecetdmonth == "Oct to Sept")
            {
                string mnthname = "October";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedyear) && x.IsSurgeryCompleted == true)
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
            }
            else if (selecetdmonth == "Nov to Oct")
            {
                string mnthname = "November";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedyear) && x.IsSurgeryCompleted == true)
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
            }
            else if (selecetdmonth == "Dec to Nov")
            {
                string mnthname = "December";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedyear) && x.IsSurgeryCompleted == true)
                                                        join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }

            }

            //Quarterly count to

            if (selectedtomonth == "Jan to Dec")
            {
                string mnthname = "January";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedtoyear) && x.IsSurgeryCompleted == true)
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
            }
            else if (selectedtomonth == "Feb to Jan")
            {
                string mnthname = "February";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedtoyear) && x.IsSurgeryCompleted == true)
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
            }
            else if (selectedtomonth == "Mar to Feb")
            {
                string mnthname = "March";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedtoyear) && x.IsSurgeryCompleted == true)
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
            }
            else if (selectedtomonth == "Apr to Mar")
            {
                string mnthname = "April";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedtoyear) && x.IsSurgeryCompleted == true)
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
            }
            else if (selectedtomonth == "May to Apr")
            {
                string mnthname = "May";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedtoyear) && x.IsSurgeryCompleted == true)
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
            }
            else if (selectedtomonth == "Jun to May")
            {
                string mnthname = "June";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedtoyear) && x.IsSurgeryCompleted == true)
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
            }
            else if (selectedtomonth == "Jul to Jun")
            {
                string mnthname = "July";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedtoyear) && x.IsSurgeryCompleted == true)
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
            }
            else if (selectedtomonth == "Aug to Jul")
            {
                string mnthname = "August";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedtoyear) && x.IsSurgeryCompleted == true)
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
            }
            else if (selectedtomonth == "Sept to Aug")
            {
                string mnthname = "September";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedtoyear) && x.IsSurgeryCompleted == true)
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
            }
            else if (selectedtomonth == "Oct to Sept")
            {
                string mnthname = "October";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedtoyear) && x.IsSurgeryCompleted == true)
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
            }
            else if (selectedtomonth == "Nov to Oct")
            {
                string mnthname = "November";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedtoyear) && x.IsSurgeryCompleted == true)
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
            }
            else if (selectedtomonth == "Dec to Nov")
            {
                string mnthname = "December";
                int FromHalf = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = FromHalf + 11;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
        .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(NewVisit) && x.PatientVisitType != Convert.ToString(ReviewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
     .Where(x => x.DateofVisit.Month >= FromHalf
        && x.DateofVisit.Month <= TotalDays && x.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && x.PatientVisitType == Convert.ToString(ReviewVisit)
        && x.PatientVisitType != Convert.ToString(NewVisit))
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qregtt.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qsurs in Qsur
                .Where(x => x.AdmDate.Month >= FromHalf
        && x.AdmDate.Month <= TotalDays && x.AdmDate.Year == Convert.ToInt32(selectedtoyear) && x.IsSurgeryCompleted == true)
                                                          join reg in Qreg.Where(x => x.CMPID == CompanyID) on Qsurs.UIN equals reg.UIN
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
            }

            return PiechartQuaerterly;
        }
        public ManagementDashboardViewModel GetSurgerydetails(string Yearstring, string Monthstring, string Daystring, int CompanyID)
        {
            var RD = new ManagementDashboardViewModel();
            DateTime fulldate = Convert.ToDateTime(Yearstring + '/' + Monthstring + '/' + Daystring);
            var FFFMonthDate = fulldate.Date.ToString("MM");
            var FFFDate = fulldate.Date.ToString("yyyy-MM-dd");
            var ICDSpeciality = WYNKContext.ICDSpecialityCode.Where(x => x.IsActive == true).ToList();
            RD.SurgeryBreakUPs = new List<SurgeryBreakUP>();
            var summarylisdata = WYNKContext.RevenueSummary.Where(x => x.CmpID == CompanyID && x.ParentID == null && x.ServiceID == null && x.SpecialityID != null).ToList();

            foreach (var item in ICDSpeciality)
            {
                var Slist = new SurgeryBreakUP();
                //var ID = ICDSpeciality.Where(x => x.SpecialityDescription == item.SpecialityDescription).Select(x => x.ID).FirstOrDefault();
                Slist.SpecialityDescription = item.SpecialityDescription;

                var Mlist = (from ss in summarylisdata.Where(x => x.Date.Month == fulldate.Month && x.SpecialityID == item.ID)
                             select new
                             {
                                 amount = ss.Amount,
                                 monthpcount = ss.Numbers,
                             }).ToList();
                var list = (from ss in summarylisdata.Where(x => x.Date.Date == fulldate.Date && x.SpecialityID == item.ID)
                            select new
                            {
                                amount = ss.Amount,
                                daypcount = ss.Numbers,
                            }).ToList();
                Slist.CountForday = list.Sum(x => x.amount);
                Slist.CountForMonth = Mlist.Sum(x => x.amount);
                Slist.patientcountforday = list.Count();
                Slist.patientcountformonth = Mlist.Count();
                RD.SurgeryBreakUPs.Add(Slist);
            }


            RD.TotalSurgsumFortheDay = RD.SurgeryBreakUPs.Sum(x => x.CountForday);
            RD.TotalSurgsumFoirtheMonth = RD.SurgeryBreakUPs.Sum(x => x.CountForMonth);

            RD.patientcountforday = RD.SurgeryBreakUPs.Sum(x => x.patientcountforday);
            RD.patientcountformonth = RD.SurgeryBreakUPs.Sum(x => x.patientcountformonth);

            return RD;
        }

        public ManagementDashboardViewModel GetMonthsurgerydetails(DateTime FromDate, DateTime ToDate, int CompanyID)
        {
            var RD = new ManagementDashboardViewModel();
            var FFFMonthDate = FromDate.ToString("MM-yyyy");
            var Tomonth = ToDate.ToString("MM-yyyy");
            var ICDSpeciality = WYNKContext.ICDSpecialityCode.Where(x => x.IsActive == true).ToList();
            RD.SurgeryBreakUPs = new List<SurgeryBreakUP>();

            var mcompanydetails = CMPSContext.Company.ToList();
            var maincomany = mcompanydetails.Where(x => x.CmpID == CompanyID).Select(x => x.CmpID).ToList();
            var parentcomany = mcompanydetails.Where(x => x.ParentID == CompanyID).Select(x => x.CmpID).ToList();
            var jpintcompany = maincomany.Concat(parentcomany);

            var summarylisdata = WYNKContext.RevenueSummary.Where(x => jpintcompany.Contains(x.CmpID)).ToList();
            foreach (var item in ICDSpeciality)
            {
                var Slist = new SurgeryBreakUP();
                Slist.SpecialityDescription = item.SpecialityDescription;

                var Mlist = (from ss in summarylisdata.Where(x => x.Date.ToString("MM-yyyy") == FFFMonthDate && x.SpecialityID == item.ID)
                             select new
                             {
                                 amount = ss.Amount,
                                 monthpcount = ss.Numbers,
                             }).ToList();
                var list = (from ss in summarylisdata.Where(x => x.Date.ToString("MM-yyyy") == Tomonth && x.SpecialityID == item.ID)
                            select new
                            {
                                amount = ss.Amount,
                                daypcount = ss.Numbers,
                            }).ToList();
                Slist.CountForday = list.Sum(x => x.amount);
                Slist.CountForMonth = Mlist.Sum(x => x.amount);
                Slist.patientcountforday = list.Count();
                Slist.patientcountformonth = Mlist.Count();
                RD.SurgeryBreakUPs.Add(Slist);
            }

            RD.TotalSurgsumFortheDay = RD.SurgeryBreakUPs.Sum(x => x.CountForday);
            RD.TotalSurgsumFoirtheMonth = RD.SurgeryBreakUPs.Sum(x => x.CountForMonth);
            RD.patientcountforday = RD.SurgeryBreakUPs.Sum(x => x.patientcountforday);
            RD.patientcountformonth = RD.SurgeryBreakUPs.Sum(x => x.patientcountformonth);
            return RD;
        }

        public static int GetMonthNumber_From_MonthName(string monthname)
        {
            int monthNumber = 0;
            monthNumber = DateTime.ParseExact(monthname, "MMMM", CultureInfo.CurrentCulture).Month;
            return monthNumber;
        }
        public static DateTime CreateDateFromTime(int year, int month, int day, DateTime time)
        {
            return new DateTime(year, month, day, time.Hour, time.Minute, 0);
        }
        public ManagementDashboardViewModel GetQuarterlycomparisionsurgerydetails(string selecetdmonth, string selectedyear, string selectedtomonth, string selectedtoyear, int CompanyID)
        {
            var RD = new ManagementDashboardViewModel();
            int fromMonthNumber = GetMonthNumber_From_MonthName(selecetdmonth);
            int toMonthNumber = GetMonthNumber_From_MonthName(selectedtomonth);
            int fromyear = Convert.ToInt32(selectedyear);
            int toyear = Convert.ToInt32(selectedtoyear);
            DateTime time = DateTime.Now;
            DateTime fromnewDateTime = CreateDateFromTime(fromyear, fromMonthNumber, 01, time);
            DateTime tonewDateTime = CreateDateFromTime(toyear, toMonthNumber, 01, time);

            var fromdatevalue = fromnewDateTime;
            var todatevalue = tonewDateTime;

            var ICDSpeciality = WYNKContext.ICDSpecialityCode.Where(x => x.IsActive == true).ToList();
            RD.SurgeryBreakUPs = new List<SurgeryBreakUP>();

            var mcompanydetails = CMPSContext.Company.ToList();
            var maincomany = mcompanydetails.Where(x => x.CmpID == CompanyID).Select(x => x.CmpID).ToList();
            var parentcomany = mcompanydetails.Where(x => x.ParentID == CompanyID).Select(x => x.CmpID).ToList();
            var jpintcompany = maincomany.Concat(parentcomany);

            var addmonthvaluesfrom = fromdatevalue.AddMonths(2);
            var addmonthvaluesfto = todatevalue.AddMonths(2);

            var summarylisdata = WYNKContext.RevenueSummary.Where(x => jpintcompany.Contains(x.CmpID)).ToList();
            foreach (var item in ICDSpeciality)
            {
                var Slist = new SurgeryBreakUP();
                Slist.SpecialityDescription = item.SpecialityDescription;

                var fromlist = (from ss in summarylisdata.Where(x => (x.Date.Date >= fromdatevalue && x.Date.Date <= addmonthvaluesfrom) && x.SpecialityID == item.ID)
                                select new
                                {
                                    amount = ss.Amount,
                                    monthpcount = ss.Numbers,
                                }).ToList();
                var tolist = (from ss in summarylisdata.Where(x => (x.Date.Date >= todatevalue && x.Date.Date <= addmonthvaluesfto) && x.SpecialityID == item.ID)
                              select new
                              {
                                  amount = ss.Amount,
                                  daypcount = ss.Numbers,
                              }).ToList();
                Slist.CountForday = tolist.Sum(x => x.amount);
                Slist.CountForMonth = fromlist.Sum(x => x.amount);
                Slist.patientcountforday = tolist.Count();
                Slist.patientcountformonth = fromlist.Count();
                RD.SurgeryBreakUPs.Add(Slist);
            }

            RD.TotalSurgsumFortheDay = RD.SurgeryBreakUPs.Sum(x => x.CountForday);
            RD.TotalSurgsumFoirtheMonth = RD.SurgeryBreakUPs.Sum(x => x.CountForMonth);
            RD.patientcountforday = RD.SurgeryBreakUPs.Sum(x => x.patientcountforday);
            RD.patientcountformonth = RD.SurgeryBreakUPs.Sum(x => x.patientcountformonth);
            return RD;
        }

        public ManagementDashboardViewModel Gethalfyearlycomparisionsurgerydetails(string selecetdmonth, string selectedyear, string selectedtomonth, string selectedtoyear, int CompanyID)
        {
            var RD = new ManagementDashboardViewModel();
            int fromMonthNumber = GetMonthNumber_From_MonthName(selecetdmonth);
            int toMonthNumber = GetMonthNumber_From_MonthName(selectedtomonth);
            int fromyear = Convert.ToInt32(selectedyear);
            int toyear = Convert.ToInt32(selectedtoyear);
            DateTime time = DateTime.Now;
            DateTime fromnewDateTime = CreateDateFromTime(fromyear, fromMonthNumber, 01, time);
            DateTime tonewDateTime = CreateDateFromTime(toyear, toMonthNumber, 01, time);

            var fromdatevalue = fromnewDateTime;
            var todatevalue = tonewDateTime;

            var ICDSpeciality = WYNKContext.ICDSpecialityCode.Where(x => x.IsActive == true).ToList();
            RD.SurgeryBreakUPs = new List<SurgeryBreakUP>();

            var mcompanydetails = CMPSContext.Company.ToList();
            var maincomany = mcompanydetails.Where(x => x.CmpID == CompanyID).Select(x => x.CmpID).ToList();
            var parentcomany = mcompanydetails.Where(x => x.ParentID == CompanyID).Select(x => x.CmpID).ToList();
            var jpintcompany = maincomany.Concat(parentcomany);

            var addmonthvaluesfrom = fromdatevalue.AddMonths(5);
            var addmonthvaluesfto = todatevalue.AddMonths(5);

            var summarylisdata = WYNKContext.RevenueSummary.Where(x => jpintcompany.Contains(x.CmpID)).ToList();
            foreach (var item in ICDSpeciality)
            {
                var Slist = new SurgeryBreakUP();
                Slist.SpecialityDescription = item.SpecialityDescription;

                var fromlist = (from ss in summarylisdata.Where(x => (x.Date.Date >= fromdatevalue && x.Date.Date <= addmonthvaluesfrom) && x.SpecialityID == item.ID)
                                select new
                                {
                                    amount = ss.Amount,
                                    monthpcount = ss.Numbers,
                                }).ToList();
                var tolist = (from ss in summarylisdata.Where(x => (x.Date.Date >= todatevalue && x.Date.Date <= addmonthvaluesfto) && x.SpecialityID == item.ID)
                              select new
                              {
                                  amount = ss.Amount,
                                  daypcount = ss.Numbers,
                              }).ToList();
                Slist.CountForday = tolist.Sum(x => x.amount);
                Slist.CountForMonth = fromlist.Sum(x => x.amount);
                Slist.patientcountforday = tolist.Count();
                Slist.patientcountformonth = fromlist.Count();
                RD.SurgeryBreakUPs.Add(Slist);
            }

            RD.TotalSurgsumFortheDay = RD.SurgeryBreakUPs.Sum(x => x.CountForday);
            RD.TotalSurgsumFoirtheMonth = RD.SurgeryBreakUPs.Sum(x => x.CountForMonth);
            RD.patientcountforday = RD.SurgeryBreakUPs.Sum(x => x.patientcountforday);
            RD.patientcountformonth = RD.SurgeryBreakUPs.Sum(x => x.patientcountformonth);
            return RD;
        }

        public ManagementDashboardViewModel Getannualcomparisionsurgerydetails(string selecetdmonth, string selectedyear, string selectedtomonth, string selectedtoyear, int CompanyID)
        {
            var RD = new ManagementDashboardViewModel();
            int fromMonthNumber = GetMonthNumber_From_MonthName(selecetdmonth);
            int toMonthNumber = GetMonthNumber_From_MonthName(selectedtomonth);
            int fromyear = Convert.ToInt32(selectedyear);
            int toyear = Convert.ToInt32(selectedtoyear);
            DateTime time = DateTime.Now;
            DateTime fromnewDateTime = CreateDateFromTime(fromyear, fromMonthNumber, 01, time);
            DateTime tonewDateTime = CreateDateFromTime(toyear, toMonthNumber, 01, time);

            var fromdatevalue = fromnewDateTime;
            var todatevalue = tonewDateTime;

            var ICDSpeciality = WYNKContext.ICDSpecialityCode.Where(x => x.IsActive == true).ToList();
            RD.SurgeryBreakUPs = new List<SurgeryBreakUP>();

            var mcompanydetails = CMPSContext.Company.ToList();
            var maincomany = mcompanydetails.Where(x => x.CmpID == CompanyID).Select(x => x.CmpID).ToList();
            var parentcomany = mcompanydetails.Where(x => x.ParentID == CompanyID).Select(x => x.CmpID).ToList();
            var jpintcompany = maincomany.Concat(parentcomany);

            var addmonthvaluesfrom = fromdatevalue.AddYears(1);
            var addmonthvaluesfto = todatevalue.AddYears(1);

            var summarylisdata = WYNKContext.RevenueSummary.Where(x => jpintcompany.Contains(x.CmpID)).ToList();
            foreach (var item in ICDSpeciality)
            {
                var Slist = new SurgeryBreakUP();
                Slist.SpecialityDescription = item.SpecialityDescription;

                var fromlist = (from ss in summarylisdata.Where(x => (x.Date.Date >= fromdatevalue && x.Date.Date <= addmonthvaluesfrom) && x.SpecialityID == item.ID)
                                select new
                                {
                                    amount = ss.Amount,
                                    monthpcount = ss.Numbers,
                                }).ToList();
                var tolist = (from ss in summarylisdata.Where(x => (x.Date.Date >= todatevalue && x.Date.Date <= addmonthvaluesfto) && x.SpecialityID == item.ID)
                              select new
                              {
                                  amount = ss.Amount,
                                  daypcount = ss.Numbers,
                              }).ToList();
                Slist.CountForday = tolist.Sum(x => x.amount);
                Slist.CountForMonth = fromlist.Sum(x => x.amount);
                Slist.patientcountforday = tolist.Count();
                Slist.patientcountformonth = fromlist.Count();
                RD.SurgeryBreakUPs.Add(Slist);
            }

            RD.TotalSurgsumFortheDay = RD.SurgeryBreakUPs.Sum(x => x.CountForday);
            RD.TotalSurgsumFoirtheMonth = RD.SurgeryBreakUPs.Sum(x => x.CountForMonth);
            RD.patientcountforday = RD.SurgeryBreakUPs.Sum(x => x.patientcountforday);
            RD.patientcountformonth = RD.SurgeryBreakUPs.Sum(x => x.patientcountformonth);
            return RD;
        }




        public ManagementDashboardViewModel GetbranchSurgerydetails(string Yearstring, string Monthstring, string Daystring, int CompanyID, string branch)
        {
            var RD = new ManagementDashboardViewModel();
            DateTime fulldate = Convert.ToDateTime(Yearstring + '/' + Monthstring + '/' + Daystring);
            var FFFMonthDate = fulldate.Date.ToString("MM");
            var FFFDate = fulldate.Date.ToString("yyyy-MM-dd");
            var ICDSpeciality = WYNKContext.ICDSpecialityCode.Where(x => x.IsActive == true).ToList();
            RD.SurgeryBreakUPs = new List<SurgeryBreakUP>();

            if (branch == "Main")
            {
                var summarylisdata = WYNKContext.RevenueSummary.Where(x => x.CmpID == CompanyID).ToList();

                foreach (var item in ICDSpeciality)
                {
                    var Slist = new SurgeryBreakUP();
                    Slist.SpecialityDescription = item.SpecialityDescription;
                    var Mlist = (from ss in summarylisdata.Where(x => x.Date.Month == fulldate.Month && x.SpecialityID == item.ID)
                                 select new
                                 {
                                     amount = ss.Amount,
                                     monthpcount = ss.Numbers,
                                 }).ToList();
                    var list = (from ss in summarylisdata.Where(x => x.Date.Date == fulldate.Date && x.SpecialityID == item.ID)
                                select new
                                {
                                    amount = ss.Amount,
                                    daypcount = ss.Numbers,
                                }).ToList();
                    Slist.CountForday = list.Sum(x => x.amount);
                    Slist.CountForMonth = Mlist.Sum(x => x.amount);
                    Slist.patientcountforday = list.Count();
                    Slist.patientcountformonth = Mlist.Count();
                    RD.SurgeryBreakUPs.Add(Slist);
                }


                RD.TotalSurgsumFortheDay = RD.SurgeryBreakUPs.Sum(x => x.CountForday);
                RD.TotalSurgsumFoirtheMonth = RD.SurgeryBreakUPs.Sum(x => x.CountForMonth);

                RD.patientcountforday = RD.SurgeryBreakUPs.Sum(x => x.patientcountforday);
                RD.patientcountformonth = RD.SurgeryBreakUPs.Sum(x => x.patientcountformonth);
            }
            else
            {
                var mcompanydetails = CMPSContext.Company.ToList();
                var maincomany = mcompanydetails.Where(x => x.CmpID == CompanyID).Select(x => x.CmpID).ToList();
                var parentcomany = mcompanydetails.Where(x => x.ParentID == CompanyID).Select(x => x.CmpID).ToList();
                var jpintcompany = maincomany.Concat(parentcomany);
                var summarylisdata = WYNKContext.RevenueSummary.Where(x => jpintcompany.Contains(x.CmpID)).ToList();
                foreach (var item in ICDSpeciality)
                {
                    var Slist = new SurgeryBreakUP();
                    Slist.SpecialityDescription = item.SpecialityDescription;

                    var Mlist = (from ss in summarylisdata.Where(x => x.Date.Month == fulldate.Month && x.SpecialityID == item.ID)
                                 select new
                                 {
                                     amount = ss.Amount,
                                     monthpcount = ss.Numbers,
                                 }).ToList();
                    var list = (from ss in summarylisdata.Where(x => x.Date.Date == fulldate.Date && x.SpecialityID == item.ID)
                                select new
                                {
                                    amount = ss.Amount,
                                    daypcount = ss.Numbers,
                                }).ToList();
                    Slist.CountForday = list.Sum(x => x.amount);
                    Slist.CountForMonth = Mlist.Sum(x => x.amount);
                    Slist.patientcountforday = list.Count();
                    Slist.patientcountformonth = Mlist.Count();
                    RD.SurgeryBreakUPs.Add(Slist);
                }
                RD.TotalSurgsumFortheDay = RD.SurgeryBreakUPs.Sum(x => x.CountForday);
                RD.TotalSurgsumFoirtheMonth = RD.SurgeryBreakUPs.Sum(x => x.CountForMonth);
                RD.patientcountforday = RD.SurgeryBreakUPs.Sum(x => x.patientcountforday);
                RD.patientcountformonth = RD.SurgeryBreakUPs.Sum(x => x.patientcountformonth);

            }
            return RD;
        }

        public ManagementDashboardViewModel GetSurgeryMonthFulldetails(string Monthstring, int CompanyID)
        {
            var RD = new ManagementDashboardViewModel();
            var FFFMonthDate = Monthstring;
            var ICDSpeciality = WYNKContext.ICDSpecialityCode.Where(x => x.IsActive == true).ToList();
            var FindingsEXT = (from f in WYNKContext.FindingsExt.Where(x => x.CreatedUTC.Date.ToString("MM") == FFFMonthDate && x.SurgeryComplete == true)
                               join fd in WYNKContext.Findings.Where(x => x.CmpID == CompanyID) on f.FindingsID equals fd.RandomUniqueID
                               select new
                               {
                                   ID = f.FindingsID,
                                   ICD = f.ICDSpecialityid,
                                   FID = f.ID,
                               }).ToList();

            RD.SurgeryBreakUPTOtalDetailss = new List<SurgeryBreakUPTOtalDetails>();

            foreach (var List in FindingsEXT)
            {
                var Slist = new SurgeryBreakUPTOtalDetails();
                var UIN = WYNKContext.Findings.Where(x => x.RandomUniqueID == List.ID && x.CmpID == CompanyID).Select(x => x.UIN).FirstOrDefault();
                var ICDCODE = WYNKContext.FindingsExt.Where(x => x.ID == List.FID).Select(x => x.ICDCode).FirstOrDefault();
                var ICDDESC = WYNKContext.ICDMaster.Where(x => x.ICDCODE == ICDCODE).Select(x => x.ICDDESCRIPTION).FirstOrDefault();
                var SD = WYNKContext.ICDSpecialityCode.Where(x => x.ID == List.ICD).Select(x => x.SpecialityDescription).FirstOrDefault();
                var Fname = WYNKContext.Registration.Where(x => x.UIN == UIN && x.CMPID == CompanyID).Select(x => x.Name).FirstOrDefault();
                var Mname = WYNKContext.Registration.Where(x => x.UIN == UIN && x.CMPID == CompanyID).Select(x => x.MiddleName).FirstOrDefault();
                var Lname = WYNKContext.Registration.Where(x => x.UIN == UIN && x.CMPID == CompanyID).Select(x => x.LastName).FirstOrDefault();
                var DOB = WYNKContext.Registration.Where(x => x.UIN == UIN && x.CMPID == CompanyID).Select(x => x.DateofBirth).FirstOrDefault();
                var Middle = " ";
                if (Mname == null)
                {
                    Middle = " ";
                }
                else
                {
                    Middle = Mname;
                }

                Slist.UIN = UIN;
                Slist.Name = Fname + Middle + Lname;
                Slist.Age = PasswordEncodeandDecode.ToAgeString(DOB);
                Slist.Gender = WYNKContext.Registration.Where(x => x.UIN == UIN && x.CMPID == CompanyID).Select(x => x.Gender).FirstOrDefault();
                Slist.Address1 = WYNKContext.Registration.Where(x => x.UIN == UIN && x.CMPID == CompanyID).Select(x => x.Address1).FirstOrDefault();
                Slist.Phone = WYNKContext.Registration.Where(x => x.UIN == UIN && x.CMPID == CompanyID).Select(x => x.Phone).FirstOrDefault();
                Slist.Specilaitydescription = SD;
                Slist.ICDdescription = ICDDESC;

                RD.SurgeryBreakUPTOtalDetailss.Add(Slist);

            }

            //var Intnum = Convert.ToInt32(FFFMonthDate);

            //RD.Monthname = Intnum.ToString("MMMM");

            return RD;
        }
        public ManagementDashboardViewModel GetSurgeryDayFulldetails(string Daystring, int CompanyID)
        {
            var RD = new ManagementDashboardViewModel();
            var FFFMonthDate = DateTime.Now.Date.ToString("dd-MM-yyyy");
            var ICDSpeciality = WYNKContext.ICDSpecialityCode.Where(x => x.IsActive == true).ToList();
            var FindingsEXT = (from f in WYNKContext.FindingsExt.Where(x => x.CreatedUTC.Date.ToString("dd-MM-yyyy") == FFFMonthDate && x.SurgeryComplete == true)
                               join fd in WYNKContext.Findings.Where(x => x.CmpID == CompanyID) on f.FindingsID equals fd.RandomUniqueID
                               select new
                               {
                                   ID = f.FindingsID,
                                   ICD = f.ICDSpecialityid,
                                   FID = f.ID,
                               }).ToList();

            RD.SurgeryBreakUPTOtalDetailss = new List<SurgeryBreakUPTOtalDetails>();

            foreach (var List in FindingsEXT)
            {
                var Slist = new SurgeryBreakUPTOtalDetails();
                var UIN = WYNKContext.Findings.Where(x => x.RandomUniqueID == List.ID && x.CmpID == CompanyID).Select(x => x.UIN).FirstOrDefault();
                var ICDCODE = WYNKContext.FindingsExt.Where(x => x.ID == List.FID).Select(x => x.ICDCode).FirstOrDefault();
                var ICDDESC = WYNKContext.ICDMaster.Where(x => x.ICDCODE == ICDCODE).Select(x => x.ICDDESCRIPTION).FirstOrDefault();
                var SD = WYNKContext.ICDSpecialityCode.Where(x => x.ID == List.ICD).Select(x => x.SpecialityDescription).FirstOrDefault();
                var Fname = WYNKContext.Registration.Where(x => x.UIN == UIN && x.CMPID == CompanyID).Select(x => x.Name).FirstOrDefault();
                var Mname = WYNKContext.Registration.Where(x => x.UIN == UIN && x.CMPID == CompanyID).Select(x => x.MiddleName).FirstOrDefault();
                var Lname = WYNKContext.Registration.Where(x => x.UIN == UIN && x.CMPID == CompanyID).Select(x => x.LastName).FirstOrDefault();
                var DOB = WYNKContext.Registration.Where(x => x.UIN == UIN && x.CMPID == CompanyID).Select(x => x.DateofBirth).FirstOrDefault();
                var Middle = " ";
                if (Mname == null)
                {
                    Middle = " ";
                }
                else
                {
                    Middle = Mname;
                }

                Slist.UIN = UIN;
                Slist.Name = Fname + Middle + Lname;
                Slist.Age = PasswordEncodeandDecode.ToAgeString(DOB);
                Slist.Gender = WYNKContext.Registration.Where(x => x.UIN == UIN && x.CMPID == CompanyID).Select(x => x.Gender).FirstOrDefault();
                Slist.Address1 = WYNKContext.Registration.Where(x => x.UIN == UIN && x.CMPID == CompanyID).Select(x => x.Address1).FirstOrDefault();
                Slist.Phone = WYNKContext.Registration.Where(x => x.UIN == UIN && x.CMPID == CompanyID).Select(x => x.Phone).FirstOrDefault();
                Slist.Specilaitydescription = SD;
                Slist.ICDdescription = ICDDESC;
                RD.SurgeryBreakUPTOtalDetailss.Add(Slist);

            }
            return RD;
        }
        public ManagementDashboardViewModel GetCountSurgeryDayFulldetails(string Daystring, string Surgdescription, int CompanyID, string branch)
        {
            var RD = new ManagementDashboardViewModel();
            var FFFMonthDate = DateTime.Now.Date;

            var mcompanydetails = CMPSContext.Company.ToList();
            var maincomany = mcompanydetails.Where(x => x.CmpID == CompanyID).Select(x => x.CmpID).ToList();
            var parentcomany = mcompanydetails.Where(x => x.ParentID == CompanyID).Select(x => x.CmpID).ToList();
            var jpintcompany = maincomany.Concat(parentcomany);
            if (branch == "Main")
            {
                var ICDSpecilityID = WYNKContext.ICDSpecialityCode.Where(x => x.SpecialityDescription == Surgdescription).Select(x => x.ID).FirstOrDefault();
                var summarylistdata = WYNKContext.RevenueSummary.Where(x => x.CmpID == CompanyID && x.SpecialityDesc == Surgdescription && x.Date.Date == FFFMonthDate).ToList();
                var doctable = CMPSContext.DoctorMaster.Where(x => x.CMPID == CompanyID).ToList();
                if (summarylistdata.Count() != 0)
                {
                    RD.Surgerydoctorbreakupdetails = (from cc in summarylistdata
                                                      select new Surgerydoctorbreakupdetails
                                                      {

                                                          fDoctorname = doctable.Where(x => x.DoctorID == cc.DoctorID).Select(x => x.Firstname).FirstOrDefault(),
                                                          mDoctorname = doctable.Where(x => x.DoctorID == cc.DoctorID).Select(x => x.MiddleName).FirstOrDefault(),
                                                          lDoctorname = doctable.Where(x => x.DoctorID == cc.DoctorID).Select(x => x.LastName).FirstOrDefault(),
                                                          patientcount = cc.Numbers,
                                                          specialitydescription = cc.SpecialityDesc,
                                                          amount = cc.Amount,
                                                          companybranch = CMPSContext.Company.Where(x => x.CmpID == CompanyID).Select(x => x.Address1).FirstOrDefault(),
                                                      }).ToList();
                }

            }
            else
            {
                //var rs = WYNKContext.RevenueSummary.Where(x => jpintcompany.Contains(x.CmpID)).ToList();
                foreach (var item in jpintcompany)
                {
                    var ICDSpecilityID = WYNKContext.ICDSpecialityCode.Where(x => x.SpecialityDescription == Surgdescription).Select(x => x.ID).FirstOrDefault();
                    var summarylistdata = WYNKContext.RevenueSummary.Where(x => x.CmpID == item && x.SpecialityDesc == Surgdescription && x.Date.Date == FFFMonthDate).ToList();
                    var doctable = CMPSContext.DoctorMaster.Where(x => x.CMPID == item).ToList();
                    if (summarylistdata.Count() != 0)
                    {
                        RD.Surgerydoctorbreakupdetails = (from cc in summarylistdata
                                                          select new Surgerydoctorbreakupdetails
                                                          {

                                                              fDoctorname = doctable.Where(x => x.DoctorID == cc.DoctorID).Select(x => x.Firstname).FirstOrDefault(),
                                                              mDoctorname = doctable.Where(x => x.DoctorID == cc.DoctorID).Select(x => x.MiddleName).FirstOrDefault(),
                                                              lDoctorname = doctable.Where(x => x.DoctorID == cc.DoctorID).Select(x => x.LastName).FirstOrDefault(),
                                                              patientcount = cc.Numbers,
                                                              specialitydescription = cc.SpecialityDesc,
                                                              amount = cc.Amount,
                                                              companybranch = CMPSContext.Company.Where(x => x.CmpID == item).Select(x => x.Address1).FirstOrDefault(),
                                                          }).ToList();
                    }


                }

            }

            return RD;
        }
        public ManagementDashboardViewModel GetCountSurgeryMonthFulldetails(string Monthstring, string Surgdescription, int CompanyID, string branch)
        {
            var RD = new ManagementDashboardViewModel();
            var FFFMonthDate = DateTime.Now.Month;

            var mcompanydetails = CMPSContext.Company.ToList();
            var maincomany = mcompanydetails.Where(x => x.CmpID == CompanyID).Select(x => x.CmpID).ToList();
            var parentcomany = mcompanydetails.Where(x => x.ParentID == CompanyID).Select(x => x.CmpID).ToList();
            var jpintcompany = maincomany.Concat(parentcomany);
            if (branch == "Main")
            {
                var rs = WYNKContext.RevenueSummary.Where(x => x.CmpID == CompanyID).ToList();

                var ICDSpecilityID = WYNKContext.ICDSpecialityCode.Where(x => x.SpecialityDescription == Surgdescription).Select(x => x.ID).FirstOrDefault();
                var summarylistdata = rs.Where(x => x.SpecialityDesc == Surgdescription && x.Date.Month == FFFMonthDate).ToList();
                var doctable = CMPSContext.DoctorMaster.Where(x => x.CMPID == CompanyID).ToList();
                if (summarylistdata.Count() != 0)
                {
                    RD.Surgerydoctorbreakupdetails = (from cc in summarylistdata
                                                      select new Surgerydoctorbreakupdetails
                                                      {


                                                          fDoctorname = doctable.Where(x => x.DoctorID == cc.DoctorID).Select(x => x.Firstname).FirstOrDefault(),
                                                          mDoctorname = doctable.Where(x => x.DoctorID == cc.DoctorID).Select(x => x.MiddleName).FirstOrDefault(),
                                                          lDoctorname = doctable.Where(x => x.DoctorID == cc.DoctorID).Select(x => x.LastName).FirstOrDefault(),
                                                          patientcount = cc.Numbers,
                                                          specialitydescription = cc.SpecialityDesc,
                                                          amount = cc.Amount,
                                                          companybranch = CMPSContext.Company.Where(x => x.CmpID == CompanyID).Select(x => x.Address1).FirstOrDefault(),
                                                      }).ToList();
                }
            }
            else
            {
                RD.Surgerydoctorbreakupdetails = new List<Surgerydoctorbreakupdetails>();
                // var rs = WYNKContext.RevenueSummary.Where(x => jpintcompany.Contains(x.CmpID)).ToList();
                foreach (var item in jpintcompany)
                {
                    var ICDSpecilityID = WYNKContext.ICDSpecialityCode.Where(x => x.SpecialityDescription == Surgdescription).Select(x => x.ID).FirstOrDefault();
                    var summarylistdata = WYNKContext.RevenueSummary.Where(x => x.CmpID == item && x.SpecialityDesc == Surgdescription && x.Date.Month == FFFMonthDate).ToList();
                    var doctable = CMPSContext.DoctorMaster.Where(x => x.CMPID == item).ToList();
                    if (summarylistdata.Count() != 0)
                    {

                        foreach (var iitem in summarylistdata)
                        {
                            var data = new Surgerydoctorbreakupdetails();
                            data.fDoctorname = doctable.Where(x => x.DoctorID == iitem.DoctorID).Select(x => x.Firstname).FirstOrDefault();
                            data.mDoctorname = doctable.Where(x => x.DoctorID == iitem.DoctorID).Select(x => x.MiddleName).FirstOrDefault();
                            data.lDoctorname = doctable.Where(x => x.DoctorID == iitem.DoctorID).Select(x => x.LastName).FirstOrDefault();
                            data.patientcount = iitem.Numbers;
                            data.specialitydescription = iitem.SpecialityDesc;
                            data.amount = iitem.Amount;
                            data.companybranch = CMPSContext.Company.Where(x => x.CmpID == item).Select(x => x.Address1).FirstOrDefault();
                            RD.Surgerydoctorbreakupdetails.Add(data);
                        }
                    }
                }

            }

            return RD;
        }

        public RegistrationMasterViewModel GetPatientpopulationdetails(string Yearstring, string Monthstring, string Daystring, int CompanyID)
        {
            var PPD = new RegistrationMasterViewModel();
            var Regt = WYNKContext.RegistrationTran.ToList();
            var Reg = WYNKContext.Registration.Where(x => x.CMPID == CompanyID).ToList();
            DateTime fulldate = Convert.ToDateTime(Yearstring + '/' + Monthstring + '/' + Daystring);
            var FFFDate = fulldate.Date.ToString("yyyy-MM-dd");
            var Descriptionnew = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "New").Select(x => x.OLMID).FirstOrDefault();
            var DescriptionReview = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Review").Select(x => x.OLMID).FirstOrDefault();
            var SUrgeryReviewDesc = WYNKContext.Admission.Where(x => x.IsSurgeryCompleted == true).FirstOrDefault();
            var Managemnt = WYNKContext.PatientFootfall.ToList();
            var FFFMonthDate = fulldate.Date.ToString("MM");

            //PPD.NEWFORTHEDAY = WYNKContext.PatientFootfall.Where(x => x.Date.Date.ToString("yyyy-MM-dd") == FFFDate && x.CmpID == CompanyID).Select(x => x.NewPatients).Count();

            //PPD.REVIEWFORTHEDAY = WYNKContext.PatientFootfall.Where(x => x.Date.Date.ToString("yyyy-MM-dd") == FFFDate && x.CmpID == CompanyID).Select(x => x.ReviewPatients).Count();

            //PPD.SURGERYREVIEWFORTHEDAY = WYNKContext.PatientFootfall.Where(x => x.Date.Date.ToString("yyyy-MM-dd") == FFFDate && x.CmpID == CompanyID).Select(x => x.SurgeryReviewPatient).Count();


            var PMD = (from REGTRAN in Regt.Where(x => x.DateofVisit.Date.ToString("yyyy-MM-dd") == FFFDate
                                          && x.PatientVisitType == Convert.ToString(Descriptionnew) && x.PatientVisitType != Convert.ToString(DescriptionReview))
                       join RG in Reg.Where(x => x.CMPID == CompanyID) on REGTRAN.UIN equals RG.UIN
                       select new
                       {
                           UINCount = REGTRAN.UIN,
                       }).ToList();

            var RPMD = (from REGTRAN in Regt.Where(x => x.DateofVisit.Date.ToString("yyyy-MM-dd") == FFFDate
                                          && x.PatientVisitType == Convert.ToString(DescriptionReview) && x.PatientVisitType != Convert.ToString(Descriptionnew))
                        join RG in Reg.Where(x => x.CMPID == CompanyID) on REGTRAN.UIN equals RG.UIN
                        select new
                        {
                            UINCount = REGTRAN.UIN,
                        }).ToList();

            var SRPMD = (from ADM in WYNKContext.Admission.Where(x => x.AdmDate.Date.ToString("yyyy-MM-dd") == FFFDate
                                          && x.IsSurgeryCompleted == true)
                         join RG in Reg.Where(x => x.CMPID == CompanyID) on ADM.UIN equals RG.UIN
                         select new SURGERYREVIEWPatientMonthtilldatepopulationdetails
                         {
                             UINCount = ADM.UIN,
                         }).ToList();
            PPD.NEWFORTHEDAY = PMD.Count();
            PPD.REVIEWFORTHEDAY = RPMD.Count();
            PPD.SURGERYREVIEWFORTHEDAY = SRPMD.Count();

            var patientMonthtilldatepopulationdetails = (from REGTRAN in Regt.Where(x => x.DateofVisit.Date.ToString("MM") == FFFMonthDate
                                          && x.PatientVisitType == Convert.ToString(Descriptionnew) && x.PatientVisitType != Convert.ToString(DescriptionReview))
                                                         join RG in Reg.Where(x => x.CMPID == CompanyID) on REGTRAN.UIN equals RG.UIN
                                                         select new
                                                         {
                                                             UINCount = REGTRAN.UIN,
                                                         }).ToList();
            var rEVIEWPatientMonthtilldatepopulationdetails = (from REGTRAN in Regt.Where(x => x.DateofVisit.Date.ToString("MM") == FFFMonthDate
                                          && x.PatientVisitType == Convert.ToString(DescriptionReview) && x.PatientVisitType != Convert.ToString(Descriptionnew))
                                                               join RG in Reg.Where(x => x.CMPID == CompanyID) on REGTRAN.UIN equals RG.UIN
                                                               select new
                                                               {
                                                                   UINCount = REGTRAN.UIN,
                                                               }).ToList();
            var SURREVIEWPOPULATIONMONTHTILLDATE = (from ADM in WYNKContext.Admission.Where(x => x.AdmDate.Date.ToString("MM") == FFFMonthDate
                                          && x.IsSurgeryCompleted == true)
                                                    join RG in Reg.Where(x => x.CMPID == CompanyID) on ADM.UIN equals RG.UIN
                                                    select new SURGERYREVIEWPatientMonthtilldatepopulationdetails
                                                    {
                                                        UINCount = ADM.UIN,
                                                    }).ToList();


            PPD.NEWMONTHTILLDATE = patientMonthtilldatepopulationdetails.Count();
            PPD.REVIEWMONTHTILLDATE = rEVIEWPatientMonthtilldatepopulationdetails.Count();
            PPD.SURGERYREVIEWMONTHTILLDATE = SURREVIEWPOPULATIONMONTHTILLDATE.Count();

            return PPD;
        }



        public ManagementDashboardViewModel GetSpecificperiodsurgerydetailsdetails(DateTime FromDate, DateTime ToDate, int compoid)
        {
            var KEYRD = new ManagementDashboardViewModel();
            KEYRD.SurgeryBreakUPfromdate = new List<SurgeryBreakUPfromdate>();
            var mcompanydetails = CMPSContext.Company.ToList();
            var maincomany = mcompanydetails.Where(x => x.CmpID == compoid).Select(x => x.CmpID).ToList();
            var parentcomany = mcompanydetails.Where(x => x.ParentID == compoid).Select(x => x.CmpID).ToList();
            var jpintcompany = maincomany.Concat(parentcomany);
            var ICDSpeciality = WYNKContext.ICDSpecialityCode.Where(x => x.IsActive == true).ToList();
            var summarylisdata = WYNKContext.RevenueSummary.Where(x => jpintcompany.Contains(x.CmpID)).ToList();
            foreach (var item in ICDSpeciality)
            {
                var Slist = new SurgeryBreakUPfromdate();
                var ID = ICDSpeciality.Where(x => x.SpecialityDescription == item.SpecialityDescription).Select(x => x.ID).FirstOrDefault();
                Slist.Revenuedesc = item.SpecialityDescription;

                var Mlist = (from ss in summarylisdata.Where(x => (x.Date.Date >= FromDate.Date && x.Date.Date <= ToDate.Date) && x.SpecialityID == item.ID)
                             select new
                             {
                                 amount = ss.Amount,
                                 monthpcount = ss.Numbers,
                             }).ToList();
                Slist.RevenueAmount = Mlist.Sum(x => x.amount);
                Slist.patientcountformonth = Mlist.Count();
                KEYRD.SurgeryBreakUPfromdate.Add(Slist);
            }

            KEYRD.Totalrevenuesum = KEYRD.SurgeryBreakUPfromdate.Sum(x => x.RevenueAmount);
            KEYRD.patientcountforday = KEYRD.SurgeryBreakUPfromdate.Sum(x => x.patientcountformonth);
            return KEYRD;
        }

        public ManagementDashboardViewModel GetSpecificperiodRevenueBriefdetails(string FromDate, string ToDate, int CompanyID)
        {
            var KEYRD = new ManagementDashboardViewModel();


            var KPIFFFDate = Convert.ToDateTime(FromDate);
            var KPITODate = Convert.ToDateTime(ToDate);
            var oneline = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "Services").ToList();
            var onelineValue = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "INV").Select(x => x.ParentID).FirstOrDefault();
            var revenuefortheday = new List<RevenuespecificBreakUPTOtalDetails>();
            var Patientaccount = WYNKContext.PatientAccount.Where(x => x.CMPID == CompanyID).ToList();
            var PAdetail = WYNKContext.PatientAccountDetail.ToList();
            var Reg = WYNKContext.Registration.ToList();
            var Regtran = WYNKContext.RegistrationTran.ToList();
            var PAdetaillist = (from PDD in PAdetail.Where(x => x.OLMID == onelineValue && x.CreatedUTC.Date >= KPIFFFDate.Date
                                && x.CreatedUTC.Date <= KPITODate.Date)
                                join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                join rg in Reg on Pa.UIN equals rg.UIN
                                select new
                                {
                                    Revamt = PDD.TotalProductValue,
                                }).ToList();
            var TotalInvestigationSumForday = PAdetaillist.Sum(x => x.Revamt);

            foreach (var Item in oneline)
            {
                var RAMT = new RevenueAMT();
                var i = 0;
                var nonlistvale = oneline.Where(x => x.OLMID == Item.OLMID).Select(x => x.ParentDescription).FirstOrDefault();

                if (nonlistvale != "Investigation" || nonlistvale == null)
                {

                    if (Item.OLMID != 0)
                    {

                        var listAmtvalue = (from pd in PAdetail
                                            join p in Patientaccount on pd.PAID equals p.PAID
                                            join rg in Reg on p.UIN equals rg.UIN
                                            where pd.OLMID == Item.OLMID
                                            && (pd.CreatedUTC.Date >= KPIFFFDate.Date && pd.CreatedUTC.Date <= KPITODate.Date)
                                            select new
                                            {
                                                RP = pd.TotalProductValue,
                                                UINs = rg.UIN,
                                                Name = rg.Name + "  " + rg.MiddleName + "  " + rg.LastName,
                                                Dateofbirth = rg.DateofBirth,
                                                Genders = rg.Gender,
                                                Addrewss = rg.Address1,
                                                phone = rg.Phone,
                                                Servicedesc = Item.ParentDescription,
                                            }).ToList();


                        if (listAmtvalue.Count != 0)
                        {
                            foreach (var IItem in listAmtvalue)
                            {
                                var Newdata = new RevenuespecificBreakUPTOtalDetails();
                                Newdata.UIN = IItem.UINs;
                                Newdata.Name = IItem.Name;
                                Newdata.Age = PasswordEncodeandDecode.ToAgeString(IItem.Dateofbirth);
                                Newdata.Gender = IItem.Genders;
                                Newdata.Address1 = IItem.Addrewss;
                                Newdata.Phone = IItem.phone;
                                Newdata.Specilatyamount = IItem.RP;
                                Newdata.Specilaitydescription = IItem.Servicedesc;

                                if (Newdata.UIN != null)
                                {
                                    revenuefortheday.Add(Newdata);
                                }
                            }
                        }


                    }
                    //else
                    //{

                    //    Newdata.RevenueAmount = i;
                    //    Newdata.Revenuedesc = Item.ParentDescription;
                    //    Newdata.serviceid = Item.OLMID;
                    //    revenuefortheday.Add(Newdata);
                    //}
                }
                else
                {
                    //var Newdata = new Revenuefortheday();
                    //Newdata.RevenueAmount = TotalInvestigationSumForday;
                    //Newdata.Revenuedesc = Item.ParentDescription;
                    //Newdata.serviceid = Item.OLMID;
                    //revenuefortheday.Add(Newdata);
                    var Newdata = new RevenuespecificBreakUPTOtalDetails();

                    var listAmtvalue = (from pd in PAdetail
                                        join p in Patientaccount on pd.PAID equals p.PAID
                                        join rg in Reg on p.UIN equals rg.UIN
                                        where pd.OLMID == Item.OLMID
                                        && (pd.CreatedUTC.Date >= KPIFFFDate.Date && pd.CreatedUTC.Date <= KPITODate.Date)
                                        select new
                                        {
                                            RP = pd.TotalProductValue,
                                            UINs = rg.UIN,
                                            Name = rg.Name + "  " + rg.MiddleName + "  " + rg.LastName,
                                            Dateofbirth = rg.DateofBirth,
                                            Genders = rg.Gender,
                                            Addrewss = rg.Address1,
                                            phone = rg.Phone,
                                            Servicedesc = Item.ParentDescription,
                                        }).ToList();


                    Newdata.UIN = listAmtvalue.Select(x => x.UINs).FirstOrDefault();
                    Newdata.Name = listAmtvalue.Select(x => x.Name).FirstOrDefault();
                    Newdata.Age = PasswordEncodeandDecode.ToAgeString(listAmtvalue.Select(x => x.Dateofbirth).FirstOrDefault());
                    Newdata.Gender = listAmtvalue.Select(x => x.Genders).FirstOrDefault();
                    Newdata.Address1 = listAmtvalue.Select(x => x.Addrewss).FirstOrDefault();
                    Newdata.Phone = listAmtvalue.Select(x => x.phone).FirstOrDefault();
                    Newdata.Specilatyamount = TotalInvestigationSumForday;
                    Newdata.Specilaitydescription = listAmtvalue.Select(x => x.Servicedesc).FirstOrDefault();
                    if (Newdata.UIN != null)
                    {
                        revenuefortheday.Add(Newdata);
                    }


                }
            }


            KEYRD.RevenuespecificBreakUPTOtalDetailsss = revenuefortheday;
            KEYRD.RevenueFromdate = FromDate;
            KEYRD.RevenueTodate = ToDate;

            return KEYRD;
        }
        public ManagementDashboardViewModel GetSpecificperiodRevenueBriefBreakupdetails(string FromDate, string ToDate, int CompanyID, string Description, string OLMID)
        {
            var KEYRD = new ManagementDashboardViewModel();


            var KPIFFFDate = Convert.ToDateTime(FromDate);
            var KPITODate = Convert.ToDateTime(ToDate);
            var revenuefortheday = new List<RevenuespecificBreakUPTOtalDetails>();
            var Patientaccount = WYNKContext.PatientAccount.Where(x => x.CMPID == CompanyID).ToList();
            var PAdetail = WYNKContext.PatientAccountDetail.ToList();
            var Reg = WYNKContext.Registration.ToList();
            var Regtran = WYNKContext.RegistrationTran.ToList();

            var listAmtvalue = (from pd in PAdetail
                                join p in Patientaccount on pd.PAID equals p.PAID
                                join rg in Reg on p.UIN equals rg.UIN
                                where pd.OLMID == Convert.ToInt32(OLMID)
                                && (pd.CreatedUTC.Date >= KPIFFFDate.Date && pd.CreatedUTC.Date <= KPITODate.Date)
                                select new
                                {
                                    RP = pd.TotalProductValue,
                                    UINs = rg.UIN,
                                    Name = rg.Name + "  " + rg.MiddleName + "  " + rg.LastName,
                                    Dateofbirth = rg.DateofBirth,
                                    Genders = rg.Gender,
                                    Addrewss = rg.Address1,
                                    phone = rg.Phone,
                                    Servicedesc = Description,
                                }).ToList();


            if (listAmtvalue.Count != 0)
            {
                foreach (var IItem in listAmtvalue)
                {
                    var Newdata = new RevenuespecificBreakUPTOtalDetails();
                    Newdata.UIN = IItem.UINs;
                    Newdata.Name = IItem.Name;
                    Newdata.Age = PasswordEncodeandDecode.ToAgeString(IItem.Dateofbirth);
                    Newdata.Gender = IItem.Genders;
                    Newdata.Address1 = IItem.Addrewss;
                    Newdata.Phone = IItem.phone;
                    Newdata.Specilatyamount = IItem.RP;
                    Newdata.Specilaitydescription = IItem.Servicedesc;

                    if (Newdata.UIN != null)
                    {
                        revenuefortheday.Add(Newdata);
                    }
                }
            }


            KEYRD.RevenuespecificBreakUPTOtalDetailsss = revenuefortheday;
            KEYRD.RevenueFromdate = FromDate;
            KEYRD.RevenueTodate = ToDate;

            return KEYRD;
        }

        public ManagementDashboardViewModel GetQuarterlycomparisionPatientpopulationdetails(string selecetdmonth,
    string selectedyear, string selectedtomonth, string selectedtoyear, int CompanyID)
        {
            var QuarterlyPPD = new ManagementDashboardViewModel();
            var NewVisit = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "New").Select(x => x.OLMID).FirstOrDefault();
            var ReviewVisit = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Review").Select(x => x.OLMID).FirstOrDefault();
            var Qreg = WYNKContext.Registration.Where(x => x.CMPID == CompanyID).ToList();
            var Qregt = WYNKContext.RegistrationTran.ToList();
            var Qsur = WYNKContext.Admission.Where(x => x.CMPID == CompanyID).ToList();

            if (selecetdmonth == "Jan - Feb - Mar")
            {


                string mnthname = "January";
                int i = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = i + 2;

                // var Mm = Convert.ToDateTime("01/01/" + selectedyear);

                // var TotalDays = Mm.AddDays(90);

                var Newq = (from Qregtt in Qregt
                            join reg in Qreg on Qregtt.UIN equals reg.UIN
                            where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                             && Qregtt.PatientVisitType == Convert.ToString(NewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear)

                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();


                var Reviewq = (from Qregtt in Qregt
                               join reg in Qreg on Qregtt.UIN equals reg.UIN
                               where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                                && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear)
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
                                  join reg in Qreg on Qsurs.UIN equals reg.UIN
                                  where (Qsurs.AdmDate.Month >= i && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && reg.CMPID == CompanyID && Qsurs.AdmDate.Year == Convert.ToInt32(selectedyear)
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                QuarterlyPPD.newcount = Newq.Count();
                QuarterlyPPD.Reviewcount = Reviewq.Count();
                QuarterlyPPD.SurgeryReviewcount = SurReviewq.Count();
            }
            else if (selecetdmonth == "Feb - Mar - Apr")
            {
                string mnthname = "February";
                int i = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = i + 2;

                // var Mm = Convert.ToDateTime("01/01/" + selectedyear);

                // var TotalDays = Mm.AddDays(90);

                var Newq = (from Qregtt in Qregt
                            join reg in Qreg on Qregtt.UIN equals reg.UIN
                            where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                             && Qregtt.PatientVisitType == Convert.ToString(NewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear)

                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();


                var Reviewq = (from Qregtt in Qregt
                               join reg in Qreg on Qregtt.UIN equals reg.UIN
                               where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                                && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear)
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
                                  join reg in Qreg on Qsurs.UIN equals reg.UIN
                                  where (Qsurs.AdmDate.Month >= i && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && Qsurs.AdmDate.Year == Convert.ToInt32(selectedyear)
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                QuarterlyPPD.newcount = Newq.Count();
                QuarterlyPPD.Reviewcount = Reviewq.Count();
                QuarterlyPPD.SurgeryReviewcount = SurReviewq.Count();
            }
            else if (selecetdmonth == "Mar - Apr - May")
            {
                string mnthname = "March";
                int i = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = i + 2;

                // var Mm = Convert.ToDateTime("01/01/" + selectedyear);

                // var TotalDays = Mm.AddDays(90);

                var Newq = (from Qregtt in Qregt
                            join reg in Qreg on Qregtt.UIN equals reg.UIN
                            where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                             && Qregtt.PatientVisitType == Convert.ToString(NewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear)

                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();


                var Reviewq = (from Qregtt in Qregt
                               join reg in Qreg on Qregtt.UIN equals reg.UIN
                               where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                                && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear)
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
                                  join reg in Qreg on Qsurs.UIN equals reg.UIN
                                  where (Qsurs.AdmDate.Month >= i && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && Qsurs.AdmDate.Year == Convert.ToInt32(selectedyear)
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                QuarterlyPPD.newcount = Newq.Count();
                QuarterlyPPD.Reviewcount = Reviewq.Count();
                QuarterlyPPD.SurgeryReviewcount = SurReviewq.Count();
            }
            else if (selecetdmonth == "Apr- May - Jun")
            {
                string mnthname = "April";
                int i = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = i + 2;

                // var Mm = Convert.ToDateTime("01/01/" + selectedyear);

                // var TotalDays = Mm.AddDays(90);

                var Newq = (from Qregtt in Qregt
                            join reg in Qreg on Qregtt.UIN equals reg.UIN
                            where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                             && Qregtt.PatientVisitType == Convert.ToString(NewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear)

                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();


                var Reviewq = (from Qregtt in Qregt
                               join reg in Qreg on Qregtt.UIN equals reg.UIN
                               where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                                && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear)
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
                                  join reg in Qreg on Qsurs.UIN equals reg.UIN
                                  where (Qsurs.AdmDate.Month >= i && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && Qsurs.AdmDate.Year == Convert.ToInt32(selectedyear)
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                QuarterlyPPD.newcount = Newq.Count();
                QuarterlyPPD.Reviewcount = Reviewq.Count();
                QuarterlyPPD.SurgeryReviewcount = SurReviewq.Count();
            }
            else if (selecetdmonth == "May - Jun - Jul")
            {
                string mnthname = "May";
                int i = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = i + 2;

                // var Mm = Convert.ToDateTime("01/01/" + selectedyear);

                // var TotalDays = Mm.AddDays(90);

                var Newq = (from Qregtt in Qregt
                            join reg in Qreg on Qregtt.UIN equals reg.UIN
                            where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                             && Qregtt.PatientVisitType == Convert.ToString(NewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear)

                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();


                var Reviewq = (from Qregtt in Qregt
                               join reg in Qreg on Qregtt.UIN equals reg.UIN
                               where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                                && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear)
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
                                  join reg in Qreg on Qsurs.UIN equals reg.UIN
                                  where (Qsurs.AdmDate.Month >= i && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && Qsurs.AdmDate.Year == Convert.ToInt32(selectedyear)
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                QuarterlyPPD.newcount = Newq.Count();
                QuarterlyPPD.Reviewcount = Reviewq.Count();
                QuarterlyPPD.SurgeryReviewcount = SurReviewq.Count();


            }
            else if (selecetdmonth == "Jun - Jul - Aug")
            {
                string mnthname = "June";
                int i = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = i + 2;

                // var Mm = Convert.ToDateTime("01/01/" + selectedyear);

                // var TotalDays = Mm.AddDays(90);

                var Newq = (from Qregtt in Qregt
                            join reg in Qreg on Qregtt.UIN equals reg.UIN
                            where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                             && Qregtt.PatientVisitType == Convert.ToString(NewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear)

                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();


                var Reviewq = (from Qregtt in Qregt
                               join reg in Qreg on Qregtt.UIN equals reg.UIN
                               where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                                && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear)
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
                                  join reg in Qreg on Qsurs.UIN equals reg.UIN
                                  where (Qsurs.AdmDate.Month >= i && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && Qsurs.AdmDate.Year == Convert.ToInt32(selectedyear)
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                QuarterlyPPD.newcount = Newq.Count();
                QuarterlyPPD.Reviewcount = Reviewq.Count();
                QuarterlyPPD.SurgeryReviewcount = SurReviewq.Count();

            }
            else if (selecetdmonth == "Jul - Aug - Sep")
            {
                string mnthname = "July";
                int i = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = i + 2;

                // var Mm = Convert.ToDateTime("01/01/" + selectedyear);

                // var TotalDays = Mm.AddDays(90);

                var Newq = (from Qregtt in Qregt
                            join reg in Qreg on Qregtt.UIN equals reg.UIN
                            where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                             && Qregtt.PatientVisitType == Convert.ToString(NewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear)

                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();


                var Reviewq = (from Qregtt in Qregt
                               join reg in Qreg on Qregtt.UIN equals reg.UIN
                               where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                                && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear)
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
                                  join reg in Qreg on Qsurs.UIN equals reg.UIN
                                  where (Qsurs.AdmDate.Month >= i && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && reg.CMPID == CompanyID && Qsurs.AdmDate.Year == Convert.ToInt32(selectedyear)
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                QuarterlyPPD.newcount = Newq.Count();
                QuarterlyPPD.Reviewcount = Reviewq.Count();
                QuarterlyPPD.SurgeryReviewcount = SurReviewq.Count();
            }
            else if (selecetdmonth == "Aug - Sep - Oct")
            {
                string mnthname = "August";
                int i = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = i + 2;

                // var Mm = Convert.ToDateTime("01/01/" + selectedyear);

                // var TotalDays = Mm.AddDays(90);

                var Newq = (from Qregtt in Qregt
                            join reg in Qreg on Qregtt.UIN equals reg.UIN
                            where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                             && Qregtt.PatientVisitType == Convert.ToString(NewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear)

                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();


                var Reviewq = (from Qregtt in Qregt
                               join reg in Qreg on Qregtt.UIN equals reg.UIN
                               where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                                && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear)
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
                                  join reg in Qreg on Qsurs.UIN equals reg.UIN
                                  where (Qsurs.AdmDate.Month >= i && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && reg.CMPID == CompanyID && Qsurs.AdmDate.Year == Convert.ToInt32(selectedyear)
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                QuarterlyPPD.newcount = Newq.Count();
                QuarterlyPPD.Reviewcount = Reviewq.Count();
                QuarterlyPPD.SurgeryReviewcount = SurReviewq.Count();
            }
            else if (selecetdmonth == "Sep - Oct - Nov")
            {
                string mnthname = "September";
                int i = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = i + 2;

                // var Mm = Convert.ToDateTime("01/01/" + selectedyear);

                // var TotalDays = Mm.AddDays(90);

                var Newq = (from Qregtt in Qregt
                            join reg in Qreg on Qregtt.UIN equals reg.UIN
                            where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                             && Qregtt.PatientVisitType == Convert.ToString(NewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear)

                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();


                var Reviewq = (from Qregtt in Qregt
                               join reg in Qreg on Qregtt.UIN equals reg.UIN
                               where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                                && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear)
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
                                  join reg in Qreg on Qsurs.UIN equals reg.UIN
                                  where (Qsurs.AdmDate.Month >= i && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && reg.CMPID == CompanyID && Qsurs.AdmDate.Year == Convert.ToInt32(selectedyear)
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                QuarterlyPPD.newcount = Newq.Count();
                QuarterlyPPD.Reviewcount = Reviewq.Count();
                QuarterlyPPD.SurgeryReviewcount = SurReviewq.Count();
            }
            else if (selecetdmonth == "Oct - Nov - Dec")
            {
                string mnthname = "October";
                int i = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = i + 2;

                // var Mm = Convert.ToDateTime("01/01/" + selectedyear);

                // var TotalDays = Mm.AddDays(90);

                var Newq = (from Qregtt in Qregt
                            join reg in Qreg on Qregtt.UIN equals reg.UIN
                            where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                             && Qregtt.PatientVisitType == Convert.ToString(NewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear)

                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();


                var Reviewq = (from Qregtt in Qregt
                               join reg in Qreg on Qregtt.UIN equals reg.UIN
                               where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                                && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear)
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
                                  join reg in Qreg on Qsurs.UIN equals reg.UIN
                                  where (Qsurs.AdmDate.Month >= i && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && reg.CMPID == CompanyID && Qsurs.AdmDate.Year == Convert.ToInt32(selectedyear)
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                QuarterlyPPD.newcount = Newq.Count();
                QuarterlyPPD.Reviewcount = Reviewq.Count();
                QuarterlyPPD.SurgeryReviewcount = SurReviewq.Count();
            }
            else if (selecetdmonth == "Nov - Dec - Jan")
            {
                string mnthname = "November";
                int i = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = i + 2;

                // var Mm = Convert.ToDateTime("01/01/" + selectedyear);

                // var TotalDays = Mm.AddDays(90);

                var Newq = (from Qregtt in Qregt
                            join reg in Qreg on Qregtt.UIN equals reg.UIN
                            where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                             && Qregtt.PatientVisitType == Convert.ToString(NewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear)

                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();


                var Reviewq = (from Qregtt in Qregt
                               join reg in Qreg on Qregtt.UIN equals reg.UIN
                               where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                                && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear)
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
                                  join reg in Qreg on Qsurs.UIN equals reg.UIN
                                  where (Qsurs.AdmDate.Month >= i && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && reg.CMPID == CompanyID && Qsurs.AdmDate.Year == Convert.ToInt32(selectedyear)
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                QuarterlyPPD.newcount = Newq.Count();
                QuarterlyPPD.Reviewcount = Reviewq.Count();
                QuarterlyPPD.SurgeryReviewcount = SurReviewq.Count();

            }
            else if (selecetdmonth == "Dec - Jan - Feb")
            {
                string mnthname = "December";
                int i = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = i + 2;

                // var Mm = Convert.ToDateTime("01/01/" + selectedyear);

                // var TotalDays = Mm.AddDays(90);

                var Newq = (from Qregtt in Qregt
                            join reg in Qreg on Qregtt.UIN equals reg.UIN
                            where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                             && Qregtt.PatientVisitType == Convert.ToString(NewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear)

                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();


                var Reviewq = (from Qregtt in Qregt
                               join reg in Qreg on Qregtt.UIN equals reg.UIN
                               where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                                && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear)
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
                                  join reg in Qreg on Qsurs.UIN equals reg.UIN
                                  where (Qsurs.AdmDate.Month >= i && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && reg.CMPID == CompanyID && Qsurs.AdmDate.Year == Convert.ToInt32(selectedyear)
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                QuarterlyPPD.newcount = Newq.Count();
                QuarterlyPPD.Reviewcount = Reviewq.Count();
                QuarterlyPPD.SurgeryReviewcount = SurReviewq.Count();

            }

            //Quarterly count to

            if (selectedtomonth == "Jan - Feb - Mar")
            {
                string mnthname = "January";
                int i = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = i + 2;

                // var Mm = Convert.ToDateTime("01/01/" + selectedyear);

                // var TotalDays = Mm.AddDays(90);

                var Newq = (from Qregtt in Qregt
                            join reg in Qreg on Qregtt.UIN equals reg.UIN
                            where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                             && Qregtt.PatientVisitType == Convert.ToString(NewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)

                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();


                var Reviewq = (from Qregtt in Qregt
                               join reg in Qreg on Qregtt.UIN equals reg.UIN
                               where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                                && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
                                  join reg in Qreg on Qsurs.UIN equals reg.UIN
                                  where (Qsurs.AdmDate.Month >= i && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && reg.CMPID == CompanyID && Qsurs.AdmDate.Year == Convert.ToInt32(selectedtoyear)
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                QuarterlyPPD.TOnewcount = Newq.Count();
                QuarterlyPPD.TOReviewcount = Reviewq.Count();
                QuarterlyPPD.TOSurgeryReviewcount = SurReviewq.Count();
            }
            else if (selectedtomonth == "Feb - Mar - Apr")
            {
                string mnthname = "February";
                int i = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = i + 2;

                // var Mm = Convert.ToDateTime("01/01/" + selectedyear);

                // var TotalDays = Mm.AddDays(90);

                var Newq = (from Qregtt in Qregt
                            join reg in Qreg on Qregtt.UIN equals reg.UIN
                            where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                             && Qregtt.PatientVisitType == Convert.ToString(NewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)

                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();


                var Reviewq = (from Qregtt in Qregt
                               join reg in Qreg on Qregtt.UIN equals reg.UIN
                               where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                                && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
                                  join reg in Qreg on Qsurs.UIN equals reg.UIN
                                  where (Qsurs.AdmDate.Month >= i && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && reg.CMPID == CompanyID && Qsurs.AdmDate.Year == Convert.ToInt32(selectedtoyear)
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                QuarterlyPPD.TOnewcount = Newq.Count();
                QuarterlyPPD.TOReviewcount = Reviewq.Count();
                QuarterlyPPD.TOSurgeryReviewcount = SurReviewq.Count();
            }
            else if (selectedtomonth == "Mar - Apr - May")
            {
                string mnthname = "March";
                int i = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = i + 2;

                // var Mm = Convert.ToDateTime("01/01/" + selectedyear);

                // var TotalDays = Mm.AddDays(90);

                var Newq = (from Qregtt in Qregt
                            join reg in Qreg on Qregtt.UIN equals reg.UIN
                            where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                             && Qregtt.PatientVisitType == Convert.ToString(NewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)

                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();


                var Reviewq = (from Qregtt in Qregt
                               join reg in Qreg on Qregtt.UIN equals reg.UIN
                               where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                                && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
                                  join reg in Qreg on Qsurs.UIN equals reg.UIN
                                  where (Qsurs.AdmDate.Month >= i && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && reg.CMPID == CompanyID && Qsurs.AdmDate.Year == Convert.ToInt32(selectedtoyear)
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                QuarterlyPPD.TOnewcount = Newq.Count();
                QuarterlyPPD.TOReviewcount = Reviewq.Count();
                QuarterlyPPD.TOSurgeryReviewcount = SurReviewq.Count();
            }
            else if (selectedtomonth == "Apr- May - Jun")
            {
                // var tm = Convert.ToDateTime("01/04/" + selectedtoyear);

                //var Tdate = DateTime.Now.Date.ToString("yyyy-MM-dd");


                string mnthname = "April";
                int i = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = i + 2;
                //var ddf = Month.toInt("April");
                //var CC = Convert.ToDateTime("01/04/" + selectedtoyear).ToString("yyyy-MM-dd ");

                //var tm = Convert.ToDateTime(CC);


                var Newq = (from Qregtt in Qregt
                            join reg in Qreg on Qregtt.UIN equals reg.UIN
                            where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                             && Qregtt.PatientVisitType == Convert.ToString(NewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)

                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();


                var Reviewq = (from Qregtt in Qregt
                               join reg in Qreg on Qregtt.UIN equals reg.UIN
                               where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                                && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
                                  join reg in Qreg on Qsurs.UIN equals reg.UIN
                                  where (Qsurs.AdmDate.Month >= i && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && reg.CMPID == CompanyID && Qsurs.AdmDate.Year == Convert.ToInt32(selectedtoyear)
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                QuarterlyPPD.TOnewcount = Newq.Count();
                QuarterlyPPD.TOReviewcount = Reviewq.Count();
                QuarterlyPPD.TOSurgeryReviewcount = SurReviewq.Count();
            }
            else if (selectedtomonth == "May - Jun - Jul")
            {
                string mnthname = "May";
                int i = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = i + 2;
                //var ddf = Month.toInt("April");
                //var CC = Convert.ToDateTime("01/04/" + selectedtoyear).ToString("yyyy-MM-dd ");

                //var tm = Convert.ToDateTime(CC);


                var Newq = (from Qregtt in Qregt
                            join reg in Qreg on Qregtt.UIN equals reg.UIN
                            where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                             && Qregtt.PatientVisitType == Convert.ToString(NewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)

                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();


                var Reviewq = (from Qregtt in Qregt
                               join reg in Qreg on Qregtt.UIN equals reg.UIN
                               where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                                && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
                                  join reg in Qreg on Qsurs.UIN equals reg.UIN
                                  where (Qsurs.AdmDate.Month >= i && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && reg.CMPID == CompanyID && Qsurs.AdmDate.Year == Convert.ToInt32(selectedtoyear)
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                QuarterlyPPD.TOnewcount = Newq.Count();
                QuarterlyPPD.TOReviewcount = Reviewq.Count();
                QuarterlyPPD.TOSurgeryReviewcount = SurReviewq.Count();
            }
            else if (selectedtomonth == "Jun - Jul - Aug")
            {
                string mnthname = "June";
                int i = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = i + 2;
                //var ddf = Month.toInt("April");
                //var CC = Convert.ToDateTime("01/04/" + selectedtoyear).ToString("yyyy-MM-dd ");

                //var tm = Convert.ToDateTime(CC);


                var Newq = (from Qregtt in Qregt
                            join reg in Qreg on Qregtt.UIN equals reg.UIN
                            where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                             && Qregtt.PatientVisitType == Convert.ToString(NewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)

                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();


                var Reviewq = (from Qregtt in Qregt
                               join reg in Qreg on Qregtt.UIN equals reg.UIN
                               where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                                && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
                                  join reg in Qreg on Qsurs.UIN equals reg.UIN
                                  where (Qsurs.AdmDate.Month >= i && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && reg.CMPID == CompanyID && Qsurs.AdmDate.Year == Convert.ToInt32(selectedtoyear)
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                QuarterlyPPD.TOnewcount = Newq.Count();
                QuarterlyPPD.TOReviewcount = Reviewq.Count();
                QuarterlyPPD.TOSurgeryReviewcount = SurReviewq.Count();
            }
            else if (selectedtomonth == "Jul - Aug - Sep")
            {
                string mnthname = "July";
                int i = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = i + 2;
                //var ddf = Month.toInt("April");
                //var CC = Convert.ToDateTime("01/04/" + selectedtoyear).ToString("yyyy-MM-dd ");

                //var tm = Convert.ToDateTime(CC);


                var Newq = (from Qregtt in Qregt
                            join reg in Qreg on Qregtt.UIN equals reg.UIN
                            where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                             && Qregtt.PatientVisitType == Convert.ToString(NewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)

                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();


                var Reviewq = (from Qregtt in Qregt
                               join reg in Qreg on Qregtt.UIN equals reg.UIN
                               where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                                && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
                                  join reg in Qreg on Qsurs.UIN equals reg.UIN
                                  where (Qsurs.AdmDate.Month >= i && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && reg.CMPID == CompanyID && Qsurs.AdmDate.Year == Convert.ToInt32(selectedtoyear)
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                QuarterlyPPD.TOnewcount = Newq.Count();
                QuarterlyPPD.TOReviewcount = Reviewq.Count();
                QuarterlyPPD.TOSurgeryReviewcount = SurReviewq.Count();
            }
            else if (selectedtomonth == "Aug - Sep - Oct")
            {
                string mnthname = "August";
                int i = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = i + 2;
                //var ddf = Month.toInt("April");
                //var CC = Convert.ToDateTime("01/04/" + selectedtoyear).ToString("yyyy-MM-dd ");

                //var tm = Convert.ToDateTime(CC);


                var Newq = (from Qregtt in Qregt
                            join reg in Qreg on Qregtt.UIN equals reg.UIN
                            where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                             && Qregtt.PatientVisitType == Convert.ToString(NewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)

                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();


                var Reviewq = (from Qregtt in Qregt
                               join reg in Qreg on Qregtt.UIN equals reg.UIN
                               where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                                && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
                                  join reg in Qreg on Qsurs.UIN equals reg.UIN
                                  where (Qsurs.AdmDate.Month >= i && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && reg.CMPID == CompanyID && Qsurs.AdmDate.Year == Convert.ToInt32(selectedtoyear)
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                QuarterlyPPD.TOnewcount = Newq.Count();
                QuarterlyPPD.TOReviewcount = Reviewq.Count();
                QuarterlyPPD.TOSurgeryReviewcount = SurReviewq.Count();
            }
            else if (selectedtomonth == "Sep - Oct - Nov")
            {
                string mnthname = "September";
                int i = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = i + 2;
                //var ddf = Month.toInt("April");
                //var CC = Convert.ToDateTime("01/04/" + selectedtoyear).ToString("yyyy-MM-dd ");

                //var tm = Convert.ToDateTime(CC);


                var Newq = (from Qregtt in Qregt
                            join reg in Qreg on Qregtt.UIN equals reg.UIN
                            where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                             && Qregtt.PatientVisitType == Convert.ToString(NewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)

                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();


                var Reviewq = (from Qregtt in Qregt
                               join reg in Qreg on Qregtt.UIN equals reg.UIN
                               where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                                && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
                                  join reg in Qreg on Qsurs.UIN equals reg.UIN
                                  where (Qsurs.AdmDate.Month >= i && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && reg.CMPID == CompanyID && Qsurs.AdmDate.Year == Convert.ToInt32(selectedtoyear)
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                QuarterlyPPD.TOnewcount = Newq.Count();
                QuarterlyPPD.TOReviewcount = Reviewq.Count();
                QuarterlyPPD.TOSurgeryReviewcount = SurReviewq.Count();
            }
            else if (selectedtomonth == "Oct - Nov - Dec")
            {
                string mnthname = "October";
                int i = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = i + 2;
                //var ddf = Month.toInt("April");
                //var CC = Convert.ToDateTime("01/04/" + selectedtoyear).ToString("yyyy-MM-dd ");

                //var tm = Convert.ToDateTime(CC);


                var Newq = (from Qregtt in Qregt
                            join reg in Qreg on Qregtt.UIN equals reg.UIN
                            where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                             && Qregtt.PatientVisitType == Convert.ToString(NewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)

                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();


                var Reviewq = (from Qregtt in Qregt
                               join reg in Qreg on Qregtt.UIN equals reg.UIN
                               where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                                && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
                                  join reg in Qreg on Qsurs.UIN equals reg.UIN
                                  where (Qsurs.AdmDate.Month >= i && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && reg.CMPID == CompanyID && Qsurs.AdmDate.Year == Convert.ToInt32(selectedtoyear)
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                QuarterlyPPD.TOnewcount = Newq.Count();
                QuarterlyPPD.TOReviewcount = Reviewq.Count();
                QuarterlyPPD.TOSurgeryReviewcount = SurReviewq.Count();
            }
            else if (selectedtomonth == "Nov - Dec - Jan")
            {
                string mnthname = "November";
                int i = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = i + 2;
                //var ddf = Month.toInt("April");
                //var CC = Convert.ToDateTime("01/04/" + selectedtoyear).ToString("yyyy-MM-dd ");

                //var tm = Convert.ToDateTime(CC);


                var Newq = (from Qregtt in Qregt
                            join reg in Qreg on Qregtt.UIN equals reg.UIN
                            where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                             && Qregtt.PatientVisitType == Convert.ToString(NewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)

                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();


                var Reviewq = (from Qregtt in Qregt
                               join reg in Qreg on Qregtt.UIN equals reg.UIN
                               where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                                && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
                                  join reg in Qreg on Qsurs.UIN equals reg.UIN
                                  where (Qsurs.AdmDate.Month >= i && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && reg.CMPID == CompanyID && Qsurs.AdmDate.Year == Convert.ToInt32(selectedtoyear)
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                QuarterlyPPD.TOnewcount = Newq.Count();
                QuarterlyPPD.TOReviewcount = Reviewq.Count();
                QuarterlyPPD.TOSurgeryReviewcount = SurReviewq.Count();
            }
            else if (selectedtomonth == "Dec - Jan - Feb")
            {
                string mnthname = "December";
                int i = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = i + 2;
                //var ddf = Month.toInt("April");
                //var CC = Convert.ToDateTime("01/04/" + selectedtoyear).ToString("yyyy-MM-dd ");

                //var tm = Convert.ToDateTime(CC);


                var Newq = (from Qregtt in Qregt
                            join reg in Qreg on Qregtt.UIN equals reg.UIN
                            where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                             && Qregtt.PatientVisitType == Convert.ToString(NewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)

                            select new
                            {
                                uin = reg.UIN,
                            }).ToList();


                var Reviewq = (from Qregtt in Qregt
                               join reg in Qreg on Qregtt.UIN equals reg.UIN
                               where (Qregtt.DateofVisit.Month >= i && Qregtt.DateofVisit.Month <= TotalDays)
                                && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
                               select new
                               {
                                   uin = reg.UIN,
                               }).ToList();
                var SurReviewq = (from Qsurs in Qsur
                                  join reg in Qreg on Qsurs.UIN equals reg.UIN
                                  where (Qsurs.AdmDate.Month >= i && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && reg.CMPID == CompanyID && Qsurs.AdmDate.Year == Convert.ToInt32(selectedtoyear)
                                  select new
                                  {
                                      uin = reg.UIN,
                                  }).ToList();
                QuarterlyPPD.TOnewcount = Newq.Count();
                QuarterlyPPD.TOReviewcount = Reviewq.Count();
                QuarterlyPPD.TOSurgeryReviewcount = SurReviewq.Count();
            }

            return QuarterlyPPD;
        }
        public ManagementDashboardViewModel GetQuarterlyPiechartcomparisionPatientpopulationdetails(string selecetdmonth, string selectedyear,
            string selectedtomonth, string selectedtoyear, int CompanyID, string id)
        {
            var PiechartQuaerterly = new ManagementDashboardViewModel();
            var NewVisit = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "New").Select(x => x.OLMID).FirstOrDefault();
            var ReviewVisit = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Review").Select(x => x.OLMID).FirstOrDefault();
            var Qreg = WYNKContext.Registration.Where(x => x.CMPID == CompanyID).ToList();
            var Qregt = WYNKContext.RegistrationTran.ToList();
            var Qsur = WYNKContext.Admission.Where(x => x.CMPID == CompanyID).ToList();

            if (selecetdmonth == "Jan - Feb - Mar")
            {
                // var Mm = Convert.ToDateTime("01/01/" + selectedyear);
                //var TotalDays = Mm.AddDays(90);

                string mnthname = "January";
                int Mm = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = Mm + 2;


                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
                                                        join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                        where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                         && Qregtt.PatientVisitType == Convert.ToString(NewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear)
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
                                                        join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                        where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                         && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear)
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qsurs in Qsur
                                                        join reg in Qreg on Qsurs.UIN equals reg.UIN
                                                        where (Qsurs.AdmDate.Month >= Mm && Qsurs.AdmDate.Month <= TotalDays)
                                                        && Qsurs.IsSurgeryCompleted == true && reg.CMPID == CompanyID
                                                        && Qsurs.AdmDate.Year == Convert.ToInt32(selectedyear)

                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
            }
            else if (selecetdmonth == "Feb - Mar - Apr")
            {

                string mnthname = "February";
                int Mm = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = Mm + 2;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
                                                        join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                        where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                         && Qregtt.PatientVisitType == Convert.ToString(NewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear)
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
                                                        join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                        where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                         && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear)
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qsurs in Qsur
                                                        join reg in Qreg on Qsurs.UIN equals reg.UIN
                                                        where (Qsurs.AdmDate.Month >= Mm && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && reg.CMPID == CompanyID && Qsurs.AdmDate.Year == Convert.ToInt32(selectedyear)
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
            }
            else if (selecetdmonth == "Mar - Apr - May")
            {

                string mnthname = "March";
                int Mm = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = Mm + 2;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
                                                        join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                        where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                         && Qregtt.PatientVisitType == Convert.ToString(NewVisit)
                                                     && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear)
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
                                                        join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                        where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                       && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear) && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit)
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qsurs in Qsur
                                                        join reg in Qreg on Qsurs.UIN equals reg.UIN
                                                        where (Qsurs.AdmDate.Month >= Mm && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && reg.CMPID == CompanyID && Qsurs.AdmDate.Year == Convert.ToInt32(selectedyear)
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
            }
            else if (selecetdmonth == "Apr- May - Jun")
            {
                string mnthname = "April";
                int Mm = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = Mm + 2;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
                                                        join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                        where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                       && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear) && Qregtt.PatientVisitType == Convert.ToString(NewVisit)
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
                                                        join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                        where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                       && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear) && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit)
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qsurs in Qsur
                                                        join reg in Qreg on Qsurs.UIN equals reg.UIN
                                                        where (Qsurs.AdmDate.Month >= Mm && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && reg.CMPID == CompanyID && Qsurs.AdmDate.Year == Convert.ToInt32(selectedyear)
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
            }
            else if (selecetdmonth == "May - Jun - Jul")
            {
                string mnthname = "May";
                int Mm = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = Mm + 2;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
                                                        join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                        where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                       && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear) && Qregtt.PatientVisitType == Convert.ToString(NewVisit)
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
                                                        join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                        where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                       && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear) && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit)
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qsurs in Qsur
                                                        join reg in Qreg on Qsurs.UIN equals reg.UIN
                                                        where (Qsurs.AdmDate.Month >= Mm && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && reg.CMPID == CompanyID && Qsurs.AdmDate.Year == Convert.ToInt32(selectedyear)
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
            }
            else if (selecetdmonth == "Jun - Jul - Aug")
            {
                string mnthname = "June";
                int Mm = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = Mm + 2;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
                                                        join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                        where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                       && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear) && Qregtt.PatientVisitType == Convert.ToString(NewVisit)
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
                                                        join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                        where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                      && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear) && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit)
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qsurs in Qsur
                                                        join reg in Qreg on Qsurs.UIN equals reg.UIN
                                                        where (Qsurs.AdmDate.Month >= Mm && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && reg.CMPID == CompanyID && Qsurs.AdmDate.Year == Convert.ToInt32(selectedyear)
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }

            }
            else if (selecetdmonth == "Jul - Aug - Sep")
            {
                string mnthname = "July";
                int Mm = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = Mm + 2;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
                                                        join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                        where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                        && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear) && Qregtt.PatientVisitType == Convert.ToString(NewVisit)
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
                                                        join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                        where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                       && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear) && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit)
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qsurs in Qsur
                                                        join reg in Qreg on Qsurs.UIN equals reg.UIN
                                                        where (Qsurs.AdmDate.Month >= Mm && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && reg.CMPID == CompanyID && Qsurs.AdmDate.Year == Convert.ToInt32(selectedyear)
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
            }
            else if (selecetdmonth == "Aug - Sep - Oct")
            {
                string mnthname = "August";
                int Mm = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = Mm + 2;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
                                                        join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                        where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                        && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear) && Qregtt.PatientVisitType == Convert.ToString(NewVisit)
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
                                                        join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                        where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                       && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear) && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit)
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qsurs in Qsur
                                                        join reg in Qreg on Qsurs.UIN equals reg.UIN
                                                        where (Qsurs.AdmDate.Month >= Mm && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && reg.CMPID == CompanyID && Qsurs.AdmDate.Year == Convert.ToInt32(selectedyear)
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
            }
            else if (selecetdmonth == "Sep - Oct - Nov")
            {
                string mnthname = "September";
                int Mm = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = Mm + 2;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
                                                        join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                        where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                       && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear) && Qregtt.PatientVisitType == Convert.ToString(NewVisit)
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
                                                        join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                        where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                       && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear) && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit)
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qsurs in Qsur
                                                        join reg in Qreg on Qsurs.UIN equals reg.UIN
                                                        where (Qsurs.AdmDate.Month >= Mm && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && reg.CMPID == CompanyID && Qsurs.AdmDate.Year == Convert.ToInt32(selectedyear)
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
            }
            else if (selecetdmonth == "Oct - Nov - Dec")
            {
                string mnthname = "October";
                int Mm = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = Mm + 2;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
                                                        join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                        where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                       && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear) && Qregtt.PatientVisitType == Convert.ToString(NewVisit)
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
                                                        join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                        where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                      && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear) && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit)
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qsurs in Qsur
                                                        join reg in Qreg on Qsurs.UIN equals reg.UIN
                                                        where (Qsurs.AdmDate.Month >= Mm && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && reg.CMPID == CompanyID && Qsurs.AdmDate.Year == Convert.ToInt32(selectedyear)
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
            }
            else if (selecetdmonth == "Nov - Dec - Jan")
            {
                string mnthname = "November";
                int Mm = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = Mm + 2;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
                                                        join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                        where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                      && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear) && Qregtt.PatientVisitType == Convert.ToString(NewVisit)
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
                                                        join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                        where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                       && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear) && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit)
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qsurs in Qsur
                                                        join reg in Qreg on Qsurs.UIN equals reg.UIN
                                                        where (Qsurs.AdmDate.Month >= Mm && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && reg.CMPID == CompanyID && Qsurs.AdmDate.Year == Convert.ToInt32(selectedyear)
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
            }
            else if (selecetdmonth == "Dec - Jan - Feb")
            {
                string mnthname = "December";
                int Mm = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = Mm + 2;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
                                                        join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                        where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                       && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear) && Qregtt.PatientVisitType == Convert.ToString(NewVisit)
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qregtt in Qregt
                                                        join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                        where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                       && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedyear) && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit)
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlyList = (from Qsurs in Qsur
                                                        join reg in Qreg on Qsurs.UIN equals reg.UIN
                                                        where (Qsurs.AdmDate.Month >= Mm && Qsurs.AdmDate.Month <= TotalDays)
                                                        && Qsurs.IsSurgeryCompleted == true && reg.CMPID == CompanyID
                                                        && Qsurs.AdmDate.Year == Convert.ToInt32(selectedyear)
                                                        select new QuarterlyFromList
                                                        {
                                                            UIN = reg.UIN,
                                                            Name = reg.Name,
                                                            gender = reg.Gender,
                                                            Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                            Phone = reg.Phone,
                                                            Address = reg.Address1,
                                                            email = reg.EmailID
                                                        }).ToList();
                }

            }

            //Quarterly count to



            if (selectedtomonth == "Jan - Feb - Mar")
            {
                // var Mm = Convert.ToDateTime("01/01/" + selectedyear);
                //var TotalDays = Mm.AddDays(90);

                string mnthname = "January";
                int Mm = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = Mm + 2;


                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
                                                          join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                          where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                           && Qregtt.PatientVisitType == Convert.ToString(NewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
                                                          join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                          where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                           && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qsurs in Qsur
                                                          join reg in Qreg on Qsurs.UIN equals reg.UIN
                                                          where (Qsurs.AdmDate.Month >= Mm && Qsurs.AdmDate.Month <= TotalDays)
                                                          && Qsurs.IsSurgeryCompleted == true && reg.CMPID == CompanyID && Qsurs.AdmDate.Year == Convert.ToInt32(selectedtoyear)
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
            }
            else if (selectedtomonth == "Feb - Mar - Apr")
            {

                string mnthname = "February";
                int Mm = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = Mm + 2;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
                                                          join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                          where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                           && Qregtt.PatientVisitType == Convert.ToString(NewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
                                                          join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                          where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                           && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qsurs in Qsur
                                                          join reg in Qreg on Qsurs.UIN equals reg.UIN
                                                          where (Qsurs.AdmDate.Month >= Mm && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && reg.CMPID == CompanyID && Qsurs.AdmDate.Year == Convert.ToInt32(selectedtoyear)
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
            }
            else if (selectedtomonth == "Mar - Apr - May")
            {

                string mnthname = "March";
                int Mm = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = Mm + 2;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
                                                          join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                          where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                           && Qregtt.PatientVisitType == Convert.ToString(NewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
                                                          join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                          where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                           && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qsurs in Qsur
                                                          join reg in Qreg on Qsurs.UIN equals reg.UIN
                                                          where (Qsurs.AdmDate.Month >= Mm && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && reg.CMPID == CompanyID && Qsurs.AdmDate.Year == Convert.ToInt32(selectedtoyear)
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
            }
            else if (selectedtomonth == "Apr- May - Jun")
            {
                string mnthname = "April";
                int Mm = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = Mm + 2;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
                                                          join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                          where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                           && Qregtt.PatientVisitType == Convert.ToString(NewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
                                                          join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                          where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                           && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qsurs in Qsur
                                                          join reg in Qreg on Qsurs.UIN equals reg.UIN
                                                          where (Qsurs.AdmDate.Month >= Mm && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && reg.CMPID == CompanyID && Qsurs.AdmDate.Year == Convert.ToInt32(selectedtoyear)
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
            }
            else if (selectedtomonth == "May - Jun - Jul")
            {
                string mnthname = "May";
                int Mm = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = Mm + 2;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
                                                          join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                          where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                           && Qregtt.PatientVisitType == Convert.ToString(NewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
                                                          join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                          where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                           && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qsurs in Qsur
                                                          join reg in Qreg on Qsurs.UIN equals reg.UIN
                                                          where (Qsurs.AdmDate.Month >= Mm && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && reg.CMPID == CompanyID && Qsurs.AdmDate.Year == Convert.ToInt32(selectedtoyear)
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
            }
            else if (selectedtomonth == "Jun - Jul - Aug")
            {
                string mnthname = "June";
                int Mm = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = Mm + 2;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
                                                          join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                          where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                           && Qregtt.PatientVisitType == Convert.ToString(NewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
                                                          join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                          where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                           && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qsurs in Qsur
                                                          join reg in Qreg on Qsurs.UIN equals reg.UIN
                                                          where (Qsurs.AdmDate.Month >= Mm && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && reg.CMPID == CompanyID && Qsurs.AdmDate.Year == Convert.ToInt32(selectedtoyear)
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }

            }
            else if (selectedtomonth == "Jul - Aug - Sep")
            {
                string mnthname = "July";
                int Mm = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = Mm + 2;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
                                                          join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                          where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                           && Qregtt.PatientVisitType == Convert.ToString(NewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
                                                          join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                          where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                           && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qsurs in Qsur
                                                          join reg in Qreg on Qsurs.UIN equals reg.UIN
                                                          where (Qsurs.AdmDate.Month >= Mm && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && reg.CMPID == CompanyID && Qsurs.AdmDate.Year == Convert.ToInt32(selectedtoyear)
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
            }
            else if (selectedtomonth == "Aug - Sep - Oct")
            {
                string mnthname = "August";
                int Mm = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = Mm + 2;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
                                                          join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                          where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                           && Qregtt.PatientVisitType == Convert.ToString(NewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
                                                          join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                          where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                           && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qsurs in Qsur
                                                          join reg in Qreg on Qsurs.UIN equals reg.UIN
                                                          where (Qsurs.AdmDate.Month >= Mm && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && reg.CMPID == CompanyID && Qsurs.AdmDate.Year == Convert.ToInt32(selectedtoyear)
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
            }
            else if (selectedtomonth == "Sep - Oct - Nov")
            {
                string mnthname = "September";
                int Mm = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = Mm + 2;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
                                                          join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                          where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                           && Qregtt.PatientVisitType == Convert.ToString(NewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
                                                          join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                          where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                           && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qsurs in Qsur
                                                          join reg in Qreg on Qsurs.UIN equals reg.UIN
                                                          where (Qsurs.AdmDate.Month >= Mm && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && reg.CMPID == CompanyID && Qsurs.AdmDate.Year == Convert.ToInt32(selectedtoyear)
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
            }
            else if (selectedtomonth == "Oct - Nov - Dec")
            {
                string mnthname = "October";
                int Mm = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = Mm + 2;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
                                                          join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                          where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                           && Qregtt.PatientVisitType == Convert.ToString(NewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
                                                          join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                          where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                           && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qsurs in Qsur
                                                          join reg in Qreg on Qsurs.UIN equals reg.UIN
                                                          where (Qsurs.AdmDate.Month >= Mm && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && reg.CMPID == CompanyID && Qsurs.AdmDate.Year == Convert.ToInt32(selectedtoyear)
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
            }
            else if (selectedtomonth == "Nov - Dec - Jan")
            {
                string mnthname = "November";
                int Mm = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = Mm + 2;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
                                                          join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                          where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                           && Qregtt.PatientVisitType == Convert.ToString(NewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
                                                          join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                          where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                           && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qsurs in Qsur
                                                          join reg in Qreg on Qsurs.UIN equals reg.UIN
                                                          where (Qsurs.AdmDate.Month >= Mm && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && reg.CMPID == CompanyID && Qsurs.AdmDate.Year == Convert.ToInt32(selectedtoyear)
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
            }
            else if (selectedtomonth == "Dec - Jan - Feb")
            {
                string mnthname = "December";
                int Mm = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = Mm + 2;

                if (id == "QNew")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
                                                          join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                          where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                           && Qregtt.PatientVisitType == Convert.ToString(NewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();

                }
                else if (id == "QReview")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qregtt in Qregt
                                                          join reg in Qreg on Qregtt.UIN equals reg.UIN
                                                          where (Qregtt.DateofVisit.Month >= Mm && Qregtt.DateofVisit.Month <= TotalDays)
                                                           && Qregtt.PatientVisitType == Convert.ToString(ReviewVisit) && Qregtt.DateofVisit.Year == Convert.ToInt32(selectedtoyear)
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }
                else if (id == "QSurgery-Review")
                {
                    PiechartQuaerterly.QuarterlytoList = (from Qsurs in Qsur
                                                          join reg in Qreg on Qsurs.UIN equals reg.UIN
                                                          where (Qsurs.AdmDate.Month >= Mm && Qsurs.AdmDate.Month <= TotalDays) && Qsurs.IsSurgeryCompleted == true && reg.CMPID == CompanyID && Qsurs.AdmDate.Year == Convert.ToInt32(selectedtoyear)
                                                          select new QuarterlytoList
                                                          {
                                                              UIN = reg.UIN,
                                                              Name = reg.Name,
                                                              gender = reg.Gender,
                                                              Age = PasswordEncodeandDecode.ToAgeString(reg.DateofBirth),
                                                              Phone = reg.Phone,
                                                              Address = reg.Address1,
                                                              email = reg.EmailID
                                                          }).ToList();
                }

            }



            return PiechartQuaerterly;
        }

  
   

        public ManagementDashboardViewModel GetSRevenueMonthBriefdetails(DateTime FromDate, DateTime ToDate, int CompanyID)
        {
            var SRD = new ManagementDashboardViewModel();

            var KPIFFFDate = FromDate.Date;
            var KPITODate = ToDate.Date;
            var oneline = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "Services").ToList();
            var onelineValue = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "INV").Select(x => x.ParentID).FirstOrDefault();
            var Patientaccount = WYNKContext.PatientAccount.Where(x => x.CMPID == CompanyID).ToList();
            var PAdetail = WYNKContext.PatientAccountDetail.ToList();
            SRD.RevenuespecificBreakUPTOtalDetailsss = new List<RevenuespecificBreakUPTOtalDetails>();
            SRD.toRevenuespecificBreakUPTOtalDetailss = new List<toRevenuespecificBreakUPTOtalDetails>();
            var PAfromdetaillist = (from PDD in PAdetail.Where(x => x.OLMID == onelineValue && x.CreatedUTC.Month == KPIFFFDate.Month)
                                    join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                    join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                    select new
                                    {
                                        Revamt = PDD.TotalProductValue,
                                    }).ToList();
            var PAftodetaillist = (from PDD in PAdetail.Where(x => x.OLMID == onelineValue && x.CreatedUTC.Month == KPITODate.Month)
                                   join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                   join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                   select new
                                   {
                                       Revamt = PDD.TotalProductValue,
                                   }).ToList();
            var TotalInvestigationSumForday = PAfromdetaillist.Sum(x => x.Revamt);
            var TotalInvestigationSumFormonth = PAftodetaillist.Sum(x => x.Revamt);


            foreach (var Item in oneline)
            {
                var i = 0;
                var listvalue = PAdetail.Where(x => x.OLMID == Item.OLMID
                && x.CreatedUTC.Month == KPIFFFDate.Month).Select(x => x.OLMID).FirstOrDefault();
                var nonlistvale = oneline.Where(x => x.OLMID == Item.OLMID).Select(x => x.ParentDescription).FirstOrDefault();

                if (nonlistvale != "Investigation" || nonlistvale == null)
                {

                    if (listvalue != 0)
                    {


                        var listAmtvalue = (from pd in PAdetail
                                            join p in Patientaccount on pd.PAID equals p.PAID
                                            where pd.OLMID == Item.OLMID
                                            && (pd.CreatedUTC.Month == KPIFFFDate.Month)
                                            join Rg in WYNKContext.Registration on p.UIN equals Rg.UIN
                                            select new
                                            {
                                                UIN = Rg.UIN,
                                                Name = Rg.Name + ' ' + Rg.MiddleName + ' ' + Rg.LastName,
                                                Dateofbirth = Rg.DateofBirth,
                                                Genderss = Rg.Gender,
                                                Phonee = Rg.Phone,
                                                Addrees = Rg.Address1 + ' ' + Rg.Address2 + ' ' + Rg.Address3,
                                                description = Item.ParentDescription,
                                                RP = pd.TotalProductValue,
                                            }).ToList();

                        if (listAmtvalue.Count() != 0)
                        {
                            foreach (var items in listAmtvalue)
                            {
                                var Newdata = new RevenuespecificBreakUPTOtalDetails();
                                Newdata.UIN = items.UIN;
                                Newdata.Name = items.Name;
                                Newdata.Age = PasswordEncodeandDecode.ToAgeString(items.Dateofbirth);
                                Newdata.Gender = items.Genderss;
                                Newdata.Phone = items.Phonee;
                                Newdata.Address1 = items.Addrees;
                                Newdata.Specilaitydescription = items.description;
                                Newdata.Specilatyamount = items.RP;

                                if (Newdata.UIN != null)
                                {
                                    SRD.RevenuespecificBreakUPTOtalDetailsss.Add(Newdata);
                                }
                            }
                        }




                    }
                }
                else
                {


                    var listAmtvalue = (from pd in PAdetail
                                        join p in Patientaccount on pd.PAID equals p.PAID
                                        where pd.OLMID == Item.OLMID
                                        && (pd.CreatedUTC.Month == KPIFFFDate.Month)
                                        join Rg in WYNKContext.Registration on p.UIN equals Rg.UIN
                                        select new
                                        {
                                            UIN = Rg.UIN,
                                            Name = Rg.Name + ' ' + Rg.MiddleName + ' ' + Rg.LastName,
                                            Dateofbirth = Rg.DateofBirth,
                                            Genderss = Rg.Gender,
                                            Phonee = Rg.Phone,
                                            Addrees = Rg.Address1 + ' ' + Rg.Address2 + ' ' + Rg.Address3,
                                            description = Item.ParentDescription,
                                            RP = TotalInvestigationSumForday,
                                        }).ToList();

                    if (listAmtvalue.Count() != 0)
                    {
                        foreach (var items in listAmtvalue)
                        {
                            var Newdata = new RevenuespecificBreakUPTOtalDetails();
                            Newdata.UIN = items.UIN;
                            Newdata.Name = items.Name;
                            Newdata.Age = PasswordEncodeandDecode.ToAgeString(items.Dateofbirth);
                            Newdata.Gender = items.Genderss;
                            Newdata.Phone = items.Phonee;
                            Newdata.Address1 = items.Addrees;
                            Newdata.Specilaitydescription = items.description;
                            Newdata.Specilatyamount = items.RP;

                            if (Newdata.UIN != null)
                            {
                                SRD.RevenuespecificBreakUPTOtalDetailsss.Add(Newdata);
                            }
                        }
                    }



                }
            }


            foreach (var Item in oneline)
            {
                var i = 0;
                var listvalue = PAdetail.Where(x => x.OLMID == Item.OLMID
                && x.CreatedUTC.Month == KPITODate.Month).Select(x => x.OLMID).FirstOrDefault();
                var nonlistvale = oneline.Where(x => x.OLMID == Item.OLMID).Select(x => x.ParentDescription).FirstOrDefault();

                if (nonlistvale != "Investigation" || nonlistvale == null)
                {

                    if (listvalue != 0)
                    {


                        var listAmtvalue = (from pd in PAdetail
                                            join p in Patientaccount on pd.PAID equals p.PAID
                                            where pd.OLMID == Item.OLMID
                                            && (pd.CreatedUTC.Month == KPITODate.Month)
                                            join Rg in WYNKContext.Registration on p.UIN equals Rg.UIN
                                            select new
                                            {
                                                UIN = Rg.UIN,
                                                Name = Rg.Name + ' ' + Rg.MiddleName + ' ' + Rg.LastName,
                                                Dateofbirth = Rg.DateofBirth,
                                                Genderss = Rg.Gender,
                                                Phonee = Rg.Phone,
                                                Addrees = Rg.Address1 + ' ' + Rg.Address2 + ' ' + Rg.Address3,
                                                description = Item.ParentDescription,
                                                RP = pd.TotalProductValue,
                                            }).ToList();


                        if (listAmtvalue.Count() != 0)
                        {
                            foreach (var items in listAmtvalue)
                            {
                                var Newdata = new toRevenuespecificBreakUPTOtalDetails();
                                Newdata.UIN = items.UIN;
                                Newdata.Name = items.Name;
                                Newdata.Age = PasswordEncodeandDecode.ToAgeString(items.Dateofbirth);
                                Newdata.Gender = items.Genderss;
                                Newdata.Phone = items.Phonee;
                                Newdata.Address1 = items.Addrees;
                                Newdata.Specilaitydescription = items.description;
                                Newdata.Specilatyamount = items.RP;

                                if (Newdata.UIN != null)
                                {
                                    SRD.toRevenuespecificBreakUPTOtalDetailss.Add(Newdata);
                                }
                            }
                        }




                    }
                }
                else
                {


                    var listAmtvalue = (from pd in PAdetail
                                        join p in Patientaccount on pd.PAID equals p.PAID
                                        where pd.OLMID == Item.OLMID
                                        && (pd.CreatedUTC.Month == KPITODate.Month)
                                        join Rg in WYNKContext.Registration on p.UIN equals Rg.UIN
                                        select new
                                        {
                                            UIN = Rg.UIN,
                                            Name = Rg.Name + ' ' + Rg.MiddleName + ' ' + Rg.LastName,
                                            Dateofbirth = Rg.DateofBirth,
                                            Genderss = Rg.Gender,
                                            Phonee = Rg.Phone,
                                            Addrees = Rg.Address1 + ' ' + Rg.Address2 + ' ' + Rg.Address3,
                                            description = Item.ParentDescription,
                                            RP = TotalInvestigationSumFormonth,
                                        }).ToList();


                    if (listAmtvalue.Count() != 0)
                    {
                        foreach (var items in listAmtvalue)
                        {
                            var Newdata = new toRevenuespecificBreakUPTOtalDetails();
                            Newdata.UIN = items.UIN;
                            Newdata.Name = items.Name;
                            Newdata.Age = PasswordEncodeandDecode.ToAgeString(items.Dateofbirth);
                            Newdata.Gender = items.Genderss;
                            Newdata.Phone = items.Phonee;
                            Newdata.Address1 = items.Addrees;
                            Newdata.Specilaitydescription = items.description;
                            Newdata.Specilatyamount = items.RP;

                            if (Newdata.UIN != null)
                            {
                                SRD.toRevenuespecificBreakUPTOtalDetailss.Add(Newdata);
                            }
                        }
                    }

                }
            }


            SRD.RevenueFromdate = KPIFFFDate.Date.ToString("MMM");
            SRD.RevenueTodate = KPITODate.Date.ToString("MMM");
            return SRD;
        }

        public ManagementDashboardViewModel GetSRevenueMonthBriefservicedetails(string Desc, string OLMID, DateTime FromDate, DateTime ToDate, int CompanyID)
        {
            var SRD = new ManagementDashboardViewModel();

            var KPIFFFDate = FromDate.Date;
            var KPITODate = ToDate.Date;
            var onelineValue = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == Desc).Select(x => x.OLMID).FirstOrDefault();
            var Patientaccount = WYNKContext.PatientAccount.Where(x => x.CMPID == CompanyID).ToList();
            var PAdetail = WYNKContext.PatientAccountDetail.ToList();
            SRD.RevenuespecificBreakUPTOtalDetailsss = new List<RevenuespecificBreakUPTOtalDetails>();
            SRD.toRevenuespecificBreakUPTOtalDetailss = new List<toRevenuespecificBreakUPTOtalDetails>();
            var PAfromdetaillist = (from PDD in PAdetail.Where(x => x.OLMID == onelineValue && x.CreatedUTC.Month == KPIFFFDate.Month)
                                    join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                    join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                    select new
                                    {
                                        Revamt = PDD.TotalProductValue,
                                    }).ToList();
            var PAftodetaillist = (from PDD in PAdetail.Where(x => x.OLMID == onelineValue && x.CreatedUTC.Month == KPITODate.Month)
                                   join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                   join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                   select new
                                   {
                                       Revamt = PDD.TotalProductValue,
                                   }).ToList();
            var TotalInvestigationSumForday = PAfromdetaillist.Sum(x => x.Revamt);
            var TotalInvestigationSumFormonth = PAftodetaillist.Sum(x => x.Revamt);


            var fromlistAmtvalue = (from pd in PAdetail
                                    join p in Patientaccount on pd.PAID equals p.PAID
                                    where pd.OLMID == onelineValue
                                    && (pd.CreatedUTC.Month == KPIFFFDate.Month)
                                    join Rg in WYNKContext.Registration on p.UIN equals Rg.UIN
                                    select new
                                    {
                                        UIN = Rg.UIN,
                                        Name = Rg.Name + ' ' + Rg.MiddleName + ' ' + Rg.LastName,
                                        Dateofbirth = Rg.DateofBirth,
                                        Genderss = Rg.Gender,
                                        Phonee = Rg.Phone,
                                        Addrees = Rg.Address1 + ' ' + Rg.Address2 + ' ' + Rg.Address3,
                                        description = Desc,
                                        RP = pd.TotalProductValue,
                                    }).ToList();

            if (fromlistAmtvalue.Count() != 0)
            {
                foreach (var items in fromlistAmtvalue)
                {
                    var Newdata = new RevenuespecificBreakUPTOtalDetails();
                    Newdata.UIN = items.UIN;
                    Newdata.Name = items.Name;
                    Newdata.Age = PasswordEncodeandDecode.ToAgeString(items.Dateofbirth);
                    Newdata.Gender = items.Genderss;
                    Newdata.Phone = items.Phonee;
                    Newdata.Address1 = items.Addrees;
                    Newdata.Specilaitydescription = items.description;
                    Newdata.Specilatyamount = items.RP;

                    if (Newdata.UIN != null)
                    {
                        SRD.RevenuespecificBreakUPTOtalDetailsss.Add(Newdata);
                    }
                }
            }

            var TolistAmtvalue = (from pd in PAdetail
                                  join p in Patientaccount on pd.PAID equals p.PAID
                                  where pd.OLMID == onelineValue
                                  && (pd.CreatedUTC.Month == KPITODate.Month)
                                  join Rg in WYNKContext.Registration on p.UIN equals Rg.UIN
                                  select new
                                  {
                                      UIN = Rg.UIN,
                                      Name = Rg.Name + ' ' + Rg.MiddleName + ' ' + Rg.LastName,
                                      Dateofbirth = Rg.DateofBirth,
                                      Genderss = Rg.Gender,
                                      Phonee = Rg.Phone,
                                      Addrees = Rg.Address1 + ' ' + Rg.Address2 + ' ' + Rg.Address3,
                                      description = Desc,
                                      RP = pd.TotalProductValue,
                                  }).ToList();

            if (TolistAmtvalue.Count() != 0)
            {
                foreach (var items in TolistAmtvalue)
                {
                    var Newdata = new toRevenuespecificBreakUPTOtalDetails();
                    Newdata.UIN = items.UIN;
                    Newdata.Name = items.Name;
                    Newdata.Age = PasswordEncodeandDecode.ToAgeString(items.Dateofbirth);
                    Newdata.Gender = items.Genderss;
                    Newdata.Phone = items.Phonee;
                    Newdata.Address1 = items.Addrees;
                    Newdata.Specilaitydescription = items.description;
                    Newdata.Specilatyamount = items.RP;

                    if (Newdata.UIN != null)
                    {
                        SRD.toRevenuespecificBreakUPTOtalDetailss.Add(Newdata);
                    }
                }
            }


            SRD.RevenueFromdate = KPIFFFDate.Date.ToString("MMM");
            SRD.RevenueTodate = KPITODate.Date.ToString("MMM");
            return SRD;
        }


        public ManagementDashboardViewModel GetSRevenueQuarterlyBriefdetails(string FromDate, string fyear, string ToDate, string toyear, int CompanyID)
        {
            var RQD = new ManagementDashboardViewModel();
            var frommonth = FromDate;
            var selectedyear = fyear;
            var Tomonth = ToDate;
            var Toyear = toyear;
            var oneline = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "Services").ToList();
            var onelineValue = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "INV").Select(x => x.ParentID).FirstOrDefault();
            var revenuefortheday = new List<RevenuespecificBreakUPTOtalDetails>();
            var RevenuefortheMonth = new List<toRevenuespecificBreakUPTOtalDetails>();
            var Patientaccount = WYNKContext.PatientAccount.Where(x => x.CMPID == CompanyID).ToList();
            var PAdetail = WYNKContext.PatientAccountDetail.ToList();


            if (frommonth == "Jan - Feb - Mar")
            {
                string mnthname = "January";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var FromPAdetaillist = (from PDD in PAdetail.Where(x => x.OLMID == onelineValue
                                        && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                        join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                        join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                        select new
                                        {
                                            Revamt = PDD.TotalProductValue,
                                        }).ToList();
                var TotalInvestigationSumFromMonth = FromPAdetaillist.Sum(x => x.Revamt);

                foreach (var Item in oneline)
                {

                    var listvalue = PAdetail.Where(x => x.OLMID == Item.OLMID
                   && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear)).Select(x => x.OLMID).FirstOrDefault();
                    var nonlistvale = oneline.Where(x => x.OLMID == listvalue).Select(x => x.ParentDescription).FirstOrDefault();

                    if (nonlistvale != "Investigation" || nonlistvale == null)
                    {

                        if (listvalue != 0)
                        {
                            var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                       && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                                join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                                join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                                select new
                                                {
                                                    uin = rg.UIN,
                                                    name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                    age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                    gender = rg.Gender,
                                                    phone = rg.Phone,
                                                    address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                    Description = Item.ParentDescription,
                                                    Revamt = PDD.TotalProductValue,
                                                }).ToList();

                            if (listAmtvalue.Count() != 0)
                            {
                                foreach (var list in listAmtvalue)
                                {
                                    var Newdata = new RevenuespecificBreakUPTOtalDetails();
                                    Newdata.UIN = list.uin;
                                    Newdata.Name = list.name;
                                    Newdata.Age = list.age;
                                    Newdata.Gender = list.gender;
                                    Newdata.Phone = list.phone;
                                    Newdata.Address1 = list.address;
                                    Newdata.Specilaitydescription = list.Description;
                                    Newdata.Specilatyamount = list.Revamt;

                                    if (list.uin != null)
                                    {
                                        revenuefortheday.Add(Newdata);
                                    }

                                }
                            }
                        }

                    }
                    else
                    {

                        var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                      && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                            join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                            join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                            select new
                                            {
                                                uin = rg.UIN,
                                                name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                gender = rg.Gender,
                                                phone = rg.Phone,
                                                address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                Description = Item.ParentDescription,
                                                Revamt = TotalInvestigationSumFromMonth,
                                            }).ToList();

                        if (listAmtvalue.Count() != 0)
                        {
                            foreach (var list in listAmtvalue)
                            {
                                var Newdata = new RevenuespecificBreakUPTOtalDetails();
                                Newdata.UIN = list.uin;
                                Newdata.Name = list.name;
                                Newdata.Age = list.age;
                                Newdata.Gender = list.gender;
                                Newdata.Phone = list.phone;
                                Newdata.Address1 = list.address;
                                Newdata.Specilaitydescription = list.Description;
                                Newdata.Specilatyamount = list.Revamt;

                                if (list.uin != null)
                                {
                                    revenuefortheday.Add(Newdata);
                                }

                            }
                        }


                        //var Newdata = new Revenuefortheday();
                        //Newdata.RevenueAmount = TotalInvestigationSumFromMonth;
                        //Newdata.Revenuedesc = Item.ParentDescription;
                        //Newdata.serviceid = Item.OLMID;
                        //revenuefortheday.Add(Newdata);
                    }
                }
                RQD.RevenuespecificBreakUPTOtalDetailsss = revenuefortheday;
            }
            else if (frommonth == "Feb - Mar - Apr")
            {
                string mnthname = "February";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var FromPAdetaillist = (from PDD in PAdetail.Where(x => x.OLMID == onelineValue
                                        && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                        join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                        join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                        select new
                                        {
                                            Revamt = PDD.TotalProductValue,
                                        }).ToList();
                var TotalInvestigationSumFromMonth = FromPAdetaillist.Sum(x => x.Revamt);

                foreach (var Item in oneline)
                {

                    var listvalue = PAdetail.Where(x => x.OLMID == Item.OLMID
                   && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear)).Select(x => x.OLMID).FirstOrDefault();
                    var nonlistvale = oneline.Where(x => x.OLMID == listvalue).Select(x => x.ParentDescription).FirstOrDefault();

                    if (nonlistvale != "Investigation" || nonlistvale == null)
                    {

                        if (listvalue != 0)
                        {
                            var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                       && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                                join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                                join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                                select new
                                                {
                                                    uin = rg.UIN,
                                                    name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                    age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                    gender = rg.Gender,
                                                    phone = rg.Phone,
                                                    address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                    Description = Item.ParentDescription,
                                                    Revamt = PDD.TotalProductValue,
                                                }).ToList();

                            if (listAmtvalue.Count() != 0)
                            {
                                foreach (var list in listAmtvalue)
                                {
                                    var Newdata = new RevenuespecificBreakUPTOtalDetails();
                                    Newdata.UIN = list.uin;
                                    Newdata.Name = list.name;
                                    Newdata.Age = list.age;
                                    Newdata.Gender = list.gender;
                                    Newdata.Phone = list.phone;
                                    Newdata.Address1 = list.address;
                                    Newdata.Specilaitydescription = list.Description;
                                    Newdata.Specilatyamount = list.Revamt;

                                    if (list.uin != null)
                                    {
                                        revenuefortheday.Add(Newdata);
                                    }

                                }
                            }
                        }

                    }
                    else
                    {

                        var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                      && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                            join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                            join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                            select new
                                            {
                                                uin = rg.UIN,
                                                name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                gender = rg.Gender,
                                                phone = rg.Phone,
                                                address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                Description = Item.ParentDescription,
                                                Revamt = TotalInvestigationSumFromMonth,
                                            }).ToList();

                        if (listAmtvalue.Count() != 0)
                        {
                            foreach (var list in listAmtvalue)
                            {
                                var Newdata = new RevenuespecificBreakUPTOtalDetails();
                                Newdata.UIN = list.uin;
                                Newdata.Name = list.name;
                                Newdata.Age = list.age;
                                Newdata.Gender = list.gender;
                                Newdata.Phone = list.phone;
                                Newdata.Address1 = list.address;
                                Newdata.Specilaitydescription = list.Description;
                                Newdata.Specilatyamount = list.Revamt;

                                if (list.uin != null)
                                {
                                    revenuefortheday.Add(Newdata);
                                }

                            }
                        }


                        //var Newdata = new Revenuefortheday();
                        //Newdata.RevenueAmount = TotalInvestigationSumFromMonth;
                        //Newdata.Revenuedesc = Item.ParentDescription;
                        //Newdata.serviceid = Item.OLMID;
                        //revenuefortheday.Add(Newdata);
                    }
                }
                RQD.RevenuespecificBreakUPTOtalDetailsss = revenuefortheday;
            }
            else if (frommonth == "Mar - Apr - May")
            {
                string mnthname = "March";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var FromPAdetaillist = (from PDD in PAdetail.Where(x => x.OLMID == onelineValue
                                        && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                        join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                        join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                        select new
                                        {
                                            Revamt = PDD.TotalProductValue,
                                        }).ToList();
                var TotalInvestigationSumFromMonth = FromPAdetaillist.Sum(x => x.Revamt);

                foreach (var Item in oneline)
                {

                    var listvalue = PAdetail.Where(x => x.OLMID == Item.OLMID
                   && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear)).Select(x => x.OLMID).FirstOrDefault();
                    var nonlistvale = oneline.Where(x => x.OLMID == listvalue).Select(x => x.ParentDescription).FirstOrDefault();

                    if (nonlistvale != "Investigation" || nonlistvale == null)
                    {

                        if (listvalue != 0)
                        {
                            var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                       && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                                join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                                join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                                select new
                                                {
                                                    uin = rg.UIN,
                                                    name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                    age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                    gender = rg.Gender,
                                                    phone = rg.Phone,
                                                    address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                    Description = Item.ParentDescription,
                                                    Revamt = PDD.TotalProductValue,
                                                }).ToList();

                            if (listAmtvalue.Count() != 0)
                            {
                                foreach (var list in listAmtvalue)
                                {
                                    var Newdata = new RevenuespecificBreakUPTOtalDetails();
                                    Newdata.UIN = list.uin;
                                    Newdata.Name = list.name;
                                    Newdata.Age = list.age;
                                    Newdata.Gender = list.gender;
                                    Newdata.Phone = list.phone;
                                    Newdata.Address1 = list.address;
                                    Newdata.Specilaitydescription = list.Description;
                                    Newdata.Specilatyamount = list.Revamt;

                                    if (list.uin != null)
                                    {
                                        revenuefortheday.Add(Newdata);
                                    }

                                }
                            }
                        }

                    }
                    else
                    {

                        var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                      && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                            join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                            join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                            select new
                                            {
                                                uin = rg.UIN,
                                                name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                gender = rg.Gender,
                                                phone = rg.Phone,
                                                address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                Description = Item.ParentDescription,
                                                Revamt = TotalInvestigationSumFromMonth,
                                            }).ToList();

                        if (listAmtvalue.Count() != 0)
                        {
                            foreach (var list in listAmtvalue)
                            {
                                var Newdata = new RevenuespecificBreakUPTOtalDetails();
                                Newdata.UIN = list.uin;
                                Newdata.Name = list.name;
                                Newdata.Age = list.age;
                                Newdata.Gender = list.gender;
                                Newdata.Phone = list.phone;
                                Newdata.Address1 = list.address;
                                Newdata.Specilaitydescription = list.Description;
                                Newdata.Specilatyamount = list.Revamt;

                                if (list.uin != null)
                                {
                                    revenuefortheday.Add(Newdata);
                                }

                            }
                        }


                        //var Newdata = new Revenuefortheday();
                        //Newdata.RevenueAmount = TotalInvestigationSumFromMonth;
                        //Newdata.Revenuedesc = Item.ParentDescription;
                        //Newdata.serviceid = Item.OLMID;
                        //revenuefortheday.Add(Newdata);
                    }
                }
                RQD.RevenuespecificBreakUPTOtalDetailsss = revenuefortheday;
            }
            else if (frommonth == "Apr- May - Jun")
            {
                string mnthname = "April";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var FromPAdetaillist = (from PDD in PAdetail.Where(x => x.OLMID == onelineValue
                                        && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                        join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                        join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                        select new
                                        {
                                            Revamt = PDD.TotalProductValue,
                                        }).ToList();
                var TotalInvestigationSumFromMonth = FromPAdetaillist.Sum(x => x.Revamt);

                foreach (var Item in oneline)
                {

                    var listvalue = PAdetail.Where(x => x.OLMID == Item.OLMID
                   && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear)).Select(x => x.OLMID).FirstOrDefault();
                    var nonlistvale = oneline.Where(x => x.OLMID == listvalue).Select(x => x.ParentDescription).FirstOrDefault();

                    if (nonlistvale != "Investigation" || nonlistvale == null)
                    {

                        if (listvalue != 0)
                        {
                            var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                       && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                                join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                                join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                                select new
                                                {
                                                    uin = rg.UIN,
                                                    name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                    age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                    gender = rg.Gender,
                                                    phone = rg.Phone,
                                                    address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                    Description = Item.ParentDescription,
                                                    Revamt = PDD.TotalProductValue,
                                                }).ToList();

                            if (listAmtvalue.Count() != 0)
                            {
                                foreach (var list in listAmtvalue)
                                {
                                    var Newdata = new RevenuespecificBreakUPTOtalDetails();
                                    Newdata.UIN = list.uin;
                                    Newdata.Name = list.name;
                                    Newdata.Age = list.age;
                                    Newdata.Gender = list.gender;
                                    Newdata.Phone = list.phone;
                                    Newdata.Address1 = list.address;
                                    Newdata.Specilaitydescription = list.Description;
                                    Newdata.Specilatyamount = list.Revamt;

                                    if (list.uin != null)
                                    {
                                        revenuefortheday.Add(Newdata);
                                    }

                                }
                            }
                        }

                    }
                    else
                    {

                        var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                      && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                            join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                            join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                            select new
                                            {
                                                uin = rg.UIN,
                                                name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                gender = rg.Gender,
                                                phone = rg.Phone,
                                                address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                Description = Item.ParentDescription,
                                                Revamt = TotalInvestigationSumFromMonth,
                                            }).ToList();

                        if (listAmtvalue.Count() != 0)
                        {
                            foreach (var list in listAmtvalue)
                            {
                                var Newdata = new RevenuespecificBreakUPTOtalDetails();
                                Newdata.UIN = list.uin;
                                Newdata.Name = list.name;
                                Newdata.Age = list.age;
                                Newdata.Gender = list.gender;
                                Newdata.Phone = list.phone;
                                Newdata.Address1 = list.address;
                                Newdata.Specilaitydescription = list.Description;
                                Newdata.Specilatyamount = list.Revamt;

                                if (list.uin != null)
                                {
                                    revenuefortheday.Add(Newdata);
                                }

                            }
                        }


                        //var Newdata = new Revenuefortheday();
                        //Newdata.RevenueAmount = TotalInvestigationSumFromMonth;
                        //Newdata.Revenuedesc = Item.ParentDescription;
                        //Newdata.serviceid = Item.OLMID;
                        //revenuefortheday.Add(Newdata);
                    }
                }
                RQD.RevenuespecificBreakUPTOtalDetailsss = revenuefortheday;
            }
            else if (frommonth == "May - Jun - Jul")
            {
                string mnthname = "May";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var FromPAdetaillist = (from PDD in PAdetail.Where(x => x.OLMID == onelineValue
                                        && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                        join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                        join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                        select new
                                        {
                                            Revamt = PDD.TotalProductValue,
                                        }).ToList();
                var TotalInvestigationSumFromMonth = FromPAdetaillist.Sum(x => x.Revamt);

                foreach (var Item in oneline)
                {

                    var listvalue = PAdetail.Where(x => x.OLMID == Item.OLMID
                   && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear)).Select(x => x.OLMID).FirstOrDefault();
                    var nonlistvale = oneline.Where(x => x.OLMID == listvalue).Select(x => x.ParentDescription).FirstOrDefault();

                    if (nonlistvale != "Investigation" || nonlistvale == null)
                    {

                        if (listvalue != 0)
                        {
                            var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                       && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                                join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                                join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                                select new
                                                {
                                                    uin = rg.UIN,
                                                    name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                    age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                    gender = rg.Gender,
                                                    phone = rg.Phone,
                                                    address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                    Description = Item.ParentDescription,
                                                    Revamt = PDD.TotalProductValue,
                                                }).ToList();

                            if (listAmtvalue.Count() != 0)
                            {
                                foreach (var list in listAmtvalue)
                                {
                                    var Newdata = new RevenuespecificBreakUPTOtalDetails();
                                    Newdata.UIN = list.uin;
                                    Newdata.Name = list.name;
                                    Newdata.Age = list.age;
                                    Newdata.Gender = list.gender;
                                    Newdata.Phone = list.phone;
                                    Newdata.Address1 = list.address;
                                    Newdata.Specilaitydescription = list.Description;
                                    Newdata.Specilatyamount = list.Revamt;

                                    if (list.uin != null)
                                    {
                                        revenuefortheday.Add(Newdata);
                                    }

                                }
                            }
                        }

                    }
                    else
                    {

                        var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                      && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                            join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                            join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                            select new
                                            {
                                                uin = rg.UIN,
                                                name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                gender = rg.Gender,
                                                phone = rg.Phone,
                                                address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                Description = Item.ParentDescription,
                                                Revamt = TotalInvestigationSumFromMonth,
                                            }).ToList();

                        if (listAmtvalue.Count() != 0)
                        {
                            foreach (var list in listAmtvalue)
                            {
                                var Newdata = new RevenuespecificBreakUPTOtalDetails();
                                Newdata.UIN = list.uin;
                                Newdata.Name = list.name;
                                Newdata.Age = list.age;
                                Newdata.Gender = list.gender;
                                Newdata.Phone = list.phone;
                                Newdata.Address1 = list.address;
                                Newdata.Specilaitydescription = list.Description;
                                Newdata.Specilatyamount = list.Revamt;

                                if (list.uin != null)
                                {
                                    revenuefortheday.Add(Newdata);
                                }

                            }
                        }


                        //var Newdata = new Revenuefortheday();
                        //Newdata.RevenueAmount = TotalInvestigationSumFromMonth;
                        //Newdata.Revenuedesc = Item.ParentDescription;
                        //Newdata.serviceid = Item.OLMID;
                        //revenuefortheday.Add(Newdata);
                    }
                }
                RQD.RevenuespecificBreakUPTOtalDetailsss = revenuefortheday;
            }
            else if (frommonth == "Jun - Jul - Aug")
            {
                string mnthname = "June";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var FromPAdetaillist = (from PDD in PAdetail.Where(x => x.OLMID == onelineValue
                                        && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                        join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                        join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                        select new
                                        {
                                            Revamt = PDD.TotalProductValue,
                                        }).ToList();
                var TotalInvestigationSumFromMonth = FromPAdetaillist.Sum(x => x.Revamt);

                foreach (var Item in oneline)
                {

                    var listvalue = PAdetail.Where(x => x.OLMID == Item.OLMID
                   && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear)).Select(x => x.OLMID).FirstOrDefault();
                    var nonlistvale = oneline.Where(x => x.OLMID == listvalue).Select(x => x.ParentDescription).FirstOrDefault();

                    if (nonlistvale != "Investigation" || nonlistvale == null)
                    {

                        if (listvalue != 0)
                        {
                            var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                       && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                                join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                                join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                                select new
                                                {
                                                    uin = rg.UIN,
                                                    name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                    age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                    gender = rg.Gender,
                                                    phone = rg.Phone,
                                                    address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                    Description = Item.ParentDescription,
                                                    Revamt = PDD.TotalProductValue,
                                                }).ToList();

                            if (listAmtvalue.Count() != 0)
                            {
                                foreach (var list in listAmtvalue)
                                {
                                    var Newdata = new RevenuespecificBreakUPTOtalDetails();
                                    Newdata.UIN = list.uin;
                                    Newdata.Name = list.name;
                                    Newdata.Age = list.age;
                                    Newdata.Gender = list.gender;
                                    Newdata.Phone = list.phone;
                                    Newdata.Address1 = list.address;
                                    Newdata.Specilaitydescription = list.Description;
                                    Newdata.Specilatyamount = list.Revamt;

                                    if (list.uin != null)
                                    {
                                        revenuefortheday.Add(Newdata);
                                    }

                                }
                            }
                        }

                    }
                    else
                    {

                        var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                      && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                            join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                            join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                            select new
                                            {
                                                uin = rg.UIN,
                                                name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                gender = rg.Gender,
                                                phone = rg.Phone,
                                                address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                Description = Item.ParentDescription,
                                                Revamt = TotalInvestigationSumFromMonth,
                                            }).ToList();

                        if (listAmtvalue.Count() != 0)
                        {
                            foreach (var list in listAmtvalue)
                            {
                                var Newdata = new RevenuespecificBreakUPTOtalDetails();
                                Newdata.UIN = list.uin;
                                Newdata.Name = list.name;
                                Newdata.Age = list.age;
                                Newdata.Gender = list.gender;
                                Newdata.Phone = list.phone;
                                Newdata.Address1 = list.address;
                                Newdata.Specilaitydescription = list.Description;
                                Newdata.Specilatyamount = list.Revamt;

                                if (list.uin != null)
                                {
                                    revenuefortheday.Add(Newdata);
                                }

                            }
                        }


                        //var Newdata = new Revenuefortheday();
                        //Newdata.RevenueAmount = TotalInvestigationSumFromMonth;
                        //Newdata.Revenuedesc = Item.ParentDescription;
                        //Newdata.serviceid = Item.OLMID;
                        //revenuefortheday.Add(Newdata);
                    }
                }
                RQD.RevenuespecificBreakUPTOtalDetailsss = revenuefortheday;
            }
            else if (frommonth == "Jul - Aug - Sep")
            {
                string mnthname = "July";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var FromPAdetaillist = (from PDD in PAdetail.Where(x => x.OLMID == onelineValue
                                        && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                        join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                        join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                        select new
                                        {
                                            Revamt = PDD.TotalProductValue,
                                        }).ToList();
                var TotalInvestigationSumFromMonth = FromPAdetaillist.Sum(x => x.Revamt);

                foreach (var Item in oneline)
                {

                    var listvalue = PAdetail.Where(x => x.OLMID == Item.OLMID
                   && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear)).Select(x => x.OLMID).FirstOrDefault();
                    var nonlistvale = oneline.Where(x => x.OLMID == listvalue).Select(x => x.ParentDescription).FirstOrDefault();

                    if (nonlistvale != "Investigation" || nonlistvale == null)
                    {

                        if (listvalue != 0)
                        {
                            var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                       && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                                join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                                join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                                select new
                                                {
                                                    uin = rg.UIN,
                                                    name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                    age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                    gender = rg.Gender,
                                                    phone = rg.Phone,
                                                    address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                    Description = Item.ParentDescription,
                                                    Revamt = PDD.TotalProductValue,
                                                }).ToList();

                            if (listAmtvalue.Count() != 0)
                            {
                                foreach (var list in listAmtvalue)
                                {
                                    var Newdata = new RevenuespecificBreakUPTOtalDetails();
                                    Newdata.UIN = list.uin;
                                    Newdata.Name = list.name;
                                    Newdata.Age = list.age;
                                    Newdata.Gender = list.gender;
                                    Newdata.Phone = list.phone;
                                    Newdata.Address1 = list.address;
                                    Newdata.Specilaitydescription = list.Description;
                                    Newdata.Specilatyamount = list.Revamt;

                                    if (list.uin != null)
                                    {
                                        revenuefortheday.Add(Newdata);
                                    }

                                }
                            }
                        }

                    }
                    else
                    {

                        var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                      && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                            join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                            join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                            select new
                                            {
                                                uin = rg.UIN,
                                                name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                gender = rg.Gender,
                                                phone = rg.Phone,
                                                address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                Description = Item.ParentDescription,
                                                Revamt = TotalInvestigationSumFromMonth,
                                            }).ToList();

                        if (listAmtvalue.Count() != 0)
                        {
                            foreach (var list in listAmtvalue)
                            {
                                var Newdata = new RevenuespecificBreakUPTOtalDetails();
                                Newdata.UIN = list.uin;
                                Newdata.Name = list.name;
                                Newdata.Age = list.age;
                                Newdata.Gender = list.gender;
                                Newdata.Phone = list.phone;
                                Newdata.Address1 = list.address;
                                Newdata.Specilaitydescription = list.Description;
                                Newdata.Specilatyamount = list.Revamt;

                                if (list.uin != null)
                                {
                                    revenuefortheday.Add(Newdata);
                                }

                            }
                        }


                        //var Newdata = new Revenuefortheday();
                        //Newdata.RevenueAmount = TotalInvestigationSumFromMonth;
                        //Newdata.Revenuedesc = Item.ParentDescription;
                        //Newdata.serviceid = Item.OLMID;
                        //revenuefortheday.Add(Newdata);
                    }
                }
                RQD.RevenuespecificBreakUPTOtalDetailsss = revenuefortheday;
            }
            else if (frommonth == "Aug - Sep - Oct")
            {
                string mnthname = "August";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var FromPAdetaillist = (from PDD in PAdetail.Where(x => x.OLMID == onelineValue
                                        && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                        join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                        join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                        select new
                                        {
                                            Revamt = PDD.TotalProductValue,
                                        }).ToList();
                var TotalInvestigationSumFromMonth = FromPAdetaillist.Sum(x => x.Revamt);

                foreach (var Item in oneline)
                {

                    var listvalue = PAdetail.Where(x => x.OLMID == Item.OLMID
                   && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear)).Select(x => x.OLMID).FirstOrDefault();
                    var nonlistvale = oneline.Where(x => x.OLMID == listvalue).Select(x => x.ParentDescription).FirstOrDefault();

                    if (nonlistvale != "Investigation" || nonlistvale == null)
                    {

                        if (listvalue != 0)
                        {
                            var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                       && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                                join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                                join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                                select new
                                                {
                                                    uin = rg.UIN,
                                                    name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                    age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                    gender = rg.Gender,
                                                    phone = rg.Phone,
                                                    address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                    Description = Item.ParentDescription,
                                                    Revamt = PDD.TotalProductValue,
                                                }).ToList();

                            if (listAmtvalue.Count() != 0)
                            {
                                foreach (var list in listAmtvalue)
                                {
                                    var Newdata = new RevenuespecificBreakUPTOtalDetails();
                                    Newdata.UIN = list.uin;
                                    Newdata.Name = list.name;
                                    Newdata.Age = list.age;
                                    Newdata.Gender = list.gender;
                                    Newdata.Phone = list.phone;
                                    Newdata.Address1 = list.address;
                                    Newdata.Specilaitydescription = list.Description;
                                    Newdata.Specilatyamount = list.Revamt;

                                    if (list.uin != null)
                                    {
                                        revenuefortheday.Add(Newdata);
                                    }

                                }
                            }
                        }

                    }
                    else
                    {

                        var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                      && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                            join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                            join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                            select new
                                            {
                                                uin = rg.UIN,
                                                name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                gender = rg.Gender,
                                                phone = rg.Phone,
                                                address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                Description = Item.ParentDescription,
                                                Revamt = TotalInvestigationSumFromMonth,
                                            }).ToList();

                        if (listAmtvalue.Count() != 0)
                        {
                            foreach (var list in listAmtvalue)
                            {
                                var Newdata = new RevenuespecificBreakUPTOtalDetails();
                                Newdata.UIN = list.uin;
                                Newdata.Name = list.name;
                                Newdata.Age = list.age;
                                Newdata.Gender = list.gender;
                                Newdata.Phone = list.phone;
                                Newdata.Address1 = list.address;
                                Newdata.Specilaitydescription = list.Description;
                                Newdata.Specilatyamount = list.Revamt;

                                if (list.uin != null)
                                {
                                    revenuefortheday.Add(Newdata);
                                }

                            }
                        }


                        //var Newdata = new Revenuefortheday();
                        //Newdata.RevenueAmount = TotalInvestigationSumFromMonth;
                        //Newdata.Revenuedesc = Item.ParentDescription;
                        //Newdata.serviceid = Item.OLMID;
                        //revenuefortheday.Add(Newdata);
                    }
                }
                RQD.RevenuespecificBreakUPTOtalDetailsss = revenuefortheday;
            }
            else if (frommonth == "Sep - Oct - Nov")
            {
                string mnthname = "September";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var FromPAdetaillist = (from PDD in PAdetail.Where(x => x.OLMID == onelineValue
                                        && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                        join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                        join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                        select new
                                        {
                                            Revamt = PDD.TotalProductValue,
                                        }).ToList();
                var TotalInvestigationSumFromMonth = FromPAdetaillist.Sum(x => x.Revamt);

                foreach (var Item in oneline)
                {

                    var listvalue = PAdetail.Where(x => x.OLMID == Item.OLMID
                   && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear)).Select(x => x.OLMID).FirstOrDefault();
                    var nonlistvale = oneline.Where(x => x.OLMID == listvalue).Select(x => x.ParentDescription).FirstOrDefault();

                    if (nonlistvale != "Investigation" || nonlistvale == null)
                    {

                        if (listvalue != 0)
                        {
                            var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                       && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                                join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                                join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                                select new
                                                {
                                                    uin = rg.UIN,
                                                    name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                    age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                    gender = rg.Gender,
                                                    phone = rg.Phone,
                                                    address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                    Description = Item.ParentDescription,
                                                    Revamt = PDD.TotalProductValue,
                                                }).ToList();

                            if (listAmtvalue.Count() != 0)
                            {
                                foreach (var list in listAmtvalue)
                                {
                                    var Newdata = new RevenuespecificBreakUPTOtalDetails();
                                    Newdata.UIN = list.uin;
                                    Newdata.Name = list.name;
                                    Newdata.Age = list.age;
                                    Newdata.Gender = list.gender;
                                    Newdata.Phone = list.phone;
                                    Newdata.Address1 = list.address;
                                    Newdata.Specilaitydescription = list.Description;
                                    Newdata.Specilatyamount = list.Revamt;

                                    if (list.uin != null)
                                    {
                                        revenuefortheday.Add(Newdata);
                                    }

                                }
                            }
                        }

                    }
                    else
                    {

                        var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                      && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                            join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                            join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                            select new
                                            {
                                                uin = rg.UIN,
                                                name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                gender = rg.Gender,
                                                phone = rg.Phone,
                                                address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                Description = Item.ParentDescription,
                                                Revamt = TotalInvestigationSumFromMonth,
                                            }).ToList();

                        if (listAmtvalue.Count() != 0)
                        {
                            foreach (var list in listAmtvalue)
                            {
                                var Newdata = new RevenuespecificBreakUPTOtalDetails();
                                Newdata.UIN = list.uin;
                                Newdata.Name = list.name;
                                Newdata.Age = list.age;
                                Newdata.Gender = list.gender;
                                Newdata.Phone = list.phone;
                                Newdata.Address1 = list.address;
                                Newdata.Specilaitydescription = list.Description;
                                Newdata.Specilatyamount = list.Revamt;

                                if (list.uin != null)
                                {
                                    revenuefortheday.Add(Newdata);
                                }

                            }
                        }


                        //var Newdata = new Revenuefortheday();
                        //Newdata.RevenueAmount = TotalInvestigationSumFromMonth;
                        //Newdata.Revenuedesc = Item.ParentDescription;
                        //Newdata.serviceid = Item.OLMID;
                        //revenuefortheday.Add(Newdata);
                    }
                }
                RQD.RevenuespecificBreakUPTOtalDetailsss = revenuefortheday;
            }
            else if (frommonth == "Oct - Nov - Dec")
            {
                string mnthname = "October";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var FromPAdetaillist = (from PDD in PAdetail.Where(x => x.OLMID == onelineValue
                                        && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                        join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                        join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                        select new
                                        {
                                            Revamt = PDD.TotalProductValue,
                                        }).ToList();
                var TotalInvestigationSumFromMonth = FromPAdetaillist.Sum(x => x.Revamt);

                foreach (var Item in oneline)
                {

                    var listvalue = PAdetail.Where(x => x.OLMID == Item.OLMID
                   && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear)).Select(x => x.OLMID).FirstOrDefault();
                    var nonlistvale = oneline.Where(x => x.OLMID == listvalue).Select(x => x.ParentDescription).FirstOrDefault();

                    if (nonlistvale != "Investigation" || nonlistvale == null)
                    {

                        if (listvalue != 0)
                        {
                            var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                       && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                                join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                                join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                                select new
                                                {
                                                    uin = rg.UIN,
                                                    name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                    age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                    gender = rg.Gender,
                                                    phone = rg.Phone,
                                                    address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                    Description = Item.ParentDescription,
                                                    Revamt = PDD.TotalProductValue,
                                                }).ToList();

                            if (listAmtvalue.Count() != 0)
                            {
                                foreach (var list in listAmtvalue)
                                {
                                    var Newdata = new RevenuespecificBreakUPTOtalDetails();
                                    Newdata.UIN = list.uin;
                                    Newdata.Name = list.name;
                                    Newdata.Age = list.age;
                                    Newdata.Gender = list.gender;
                                    Newdata.Phone = list.phone;
                                    Newdata.Address1 = list.address;
                                    Newdata.Specilaitydescription = list.Description;
                                    Newdata.Specilatyamount = list.Revamt;

                                    if (list.uin != null)
                                    {
                                        revenuefortheday.Add(Newdata);
                                    }

                                }
                            }
                        }

                    }
                    else
                    {

                        var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                      && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                            join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                            join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                            select new
                                            {
                                                uin = rg.UIN,
                                                name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                gender = rg.Gender,
                                                phone = rg.Phone,
                                                address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                Description = Item.ParentDescription,
                                                Revamt = TotalInvestigationSumFromMonth,
                                            }).ToList();

                        if (listAmtvalue.Count() != 0)
                        {
                            foreach (var list in listAmtvalue)
                            {
                                var Newdata = new RevenuespecificBreakUPTOtalDetails();
                                Newdata.UIN = list.uin;
                                Newdata.Name = list.name;
                                Newdata.Age = list.age;
                                Newdata.Gender = list.gender;
                                Newdata.Phone = list.phone;
                                Newdata.Address1 = list.address;
                                Newdata.Specilaitydescription = list.Description;
                                Newdata.Specilatyamount = list.Revamt;

                                if (list.uin != null)
                                {
                                    revenuefortheday.Add(Newdata);
                                }

                            }
                        }


                        //var Newdata = new Revenuefortheday();
                        //Newdata.RevenueAmount = TotalInvestigationSumFromMonth;
                        //Newdata.Revenuedesc = Item.ParentDescription;
                        //Newdata.serviceid = Item.OLMID;
                        //revenuefortheday.Add(Newdata);
                    }
                }
                RQD.RevenuespecificBreakUPTOtalDetailsss = revenuefortheday;
            }
            else if (frommonth == "Nov - Dec - Jan")
            {
                string mnthname = "November";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var FromPAdetaillist = (from PDD in PAdetail.Where(x => x.OLMID == onelineValue
                                        && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                        join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                        join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                        select new
                                        {
                                            Revamt = PDD.TotalProductValue,
                                        }).ToList();
                var TotalInvestigationSumFromMonth = FromPAdetaillist.Sum(x => x.Revamt);

                foreach (var Item in oneline)
                {

                    var listvalue = PAdetail.Where(x => x.OLMID == Item.OLMID
                   && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear)).Select(x => x.OLMID).FirstOrDefault();
                    var nonlistvale = oneline.Where(x => x.OLMID == listvalue).Select(x => x.ParentDescription).FirstOrDefault();

                    if (nonlistvale != "Investigation" || nonlistvale == null)
                    {

                        if (listvalue != 0)
                        {
                            var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                       && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                                join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                                join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                                select new
                                                {
                                                    uin = rg.UIN,
                                                    name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                    age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                    gender = rg.Gender,
                                                    phone = rg.Phone,
                                                    address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                    Description = Item.ParentDescription,
                                                    Revamt = PDD.TotalProductValue,
                                                }).ToList();

                            if (listAmtvalue.Count() != 0)
                            {
                                foreach (var list in listAmtvalue)
                                {
                                    var Newdata = new RevenuespecificBreakUPTOtalDetails();
                                    Newdata.UIN = list.uin;
                                    Newdata.Name = list.name;
                                    Newdata.Age = list.age;
                                    Newdata.Gender = list.gender;
                                    Newdata.Phone = list.phone;
                                    Newdata.Address1 = list.address;
                                    Newdata.Specilaitydescription = list.Description;
                                    Newdata.Specilatyamount = list.Revamt;

                                    if (list.uin != null)
                                    {
                                        revenuefortheday.Add(Newdata);
                                    }

                                }
                            }
                        }

                    }
                    else
                    {

                        var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                      && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                            join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                            join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                            select new
                                            {
                                                uin = rg.UIN,
                                                name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                gender = rg.Gender,
                                                phone = rg.Phone,
                                                address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                Description = Item.ParentDescription,
                                                Revamt = TotalInvestigationSumFromMonth,
                                            }).ToList();

                        if (listAmtvalue.Count() != 0)
                        {
                            foreach (var list in listAmtvalue)
                            {
                                var Newdata = new RevenuespecificBreakUPTOtalDetails();
                                Newdata.UIN = list.uin;
                                Newdata.Name = list.name;
                                Newdata.Age = list.age;
                                Newdata.Gender = list.gender;
                                Newdata.Phone = list.phone;
                                Newdata.Address1 = list.address;
                                Newdata.Specilaitydescription = list.Description;
                                Newdata.Specilatyamount = list.Revamt;

                                if (list.uin != null)
                                {
                                    revenuefortheday.Add(Newdata);
                                }

                            }
                        }


                        //var Newdata = new Revenuefortheday();
                        //Newdata.RevenueAmount = TotalInvestigationSumFromMonth;
                        //Newdata.Revenuedesc = Item.ParentDescription;
                        //Newdata.serviceid = Item.OLMID;
                        //revenuefortheday.Add(Newdata);
                    }
                }
                RQD.RevenuespecificBreakUPTOtalDetailsss = revenuefortheday;
            }
            else if (frommonth == "Dec - Jan - Feb")
            {
                string mnthname = "December";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var FromPAdetaillist = (from PDD in PAdetail.Where(x => x.OLMID == onelineValue
                                        && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                        join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                        join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                        select new
                                        {
                                            Revamt = PDD.TotalProductValue,
                                        }).ToList();
                var TotalInvestigationSumFromMonth = FromPAdetaillist.Sum(x => x.Revamt);

                foreach (var Item in oneline)
                {

                    var listvalue = PAdetail.Where(x => x.OLMID == Item.OLMID
                   && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear)).Select(x => x.OLMID).FirstOrDefault();
                    var nonlistvale = oneline.Where(x => x.OLMID == listvalue).Select(x => x.ParentDescription).FirstOrDefault();

                    if (nonlistvale != "Investigation" || nonlistvale == null)
                    {

                        if (listvalue != 0)
                        {
                            var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                       && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                                join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                                join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                                select new
                                                {
                                                    uin = rg.UIN,
                                                    name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                    age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                    gender = rg.Gender,
                                                    phone = rg.Phone,
                                                    address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                    Description = Item.ParentDescription,
                                                    Revamt = PDD.TotalProductValue,
                                                }).ToList();

                            if (listAmtvalue.Count() != 0)
                            {
                                foreach (var list in listAmtvalue)
                                {
                                    var Newdata = new RevenuespecificBreakUPTOtalDetails();
                                    Newdata.UIN = list.uin;
                                    Newdata.Name = list.name;
                                    Newdata.Age = list.age;
                                    Newdata.Gender = list.gender;
                                    Newdata.Phone = list.phone;
                                    Newdata.Address1 = list.address;
                                    Newdata.Specilaitydescription = list.Description;
                                    Newdata.Specilatyamount = list.Revamt;

                                    if (list.uin != null)
                                    {
                                        revenuefortheday.Add(Newdata);
                                    }

                                }
                            }
                        }

                    }
                    else
                    {

                        var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                      && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                            join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                            join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                            select new
                                            {
                                                uin = rg.UIN,
                                                name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                gender = rg.Gender,
                                                phone = rg.Phone,
                                                address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                Description = Item.ParentDescription,
                                                Revamt = TotalInvestigationSumFromMonth,
                                            }).ToList();

                        if (listAmtvalue.Count() != 0)
                        {
                            foreach (var list in listAmtvalue)
                            {
                                var Newdata = new RevenuespecificBreakUPTOtalDetails();
                                Newdata.UIN = list.uin;
                                Newdata.Name = list.name;
                                Newdata.Age = list.age;
                                Newdata.Gender = list.gender;
                                Newdata.Phone = list.phone;
                                Newdata.Address1 = list.address;
                                Newdata.Specilaitydescription = list.Description;
                                Newdata.Specilatyamount = list.Revamt;

                                if (list.uin != null)
                                {
                                    revenuefortheday.Add(Newdata);
                                }

                            }
                        }


                        //var Newdata = new Revenuefortheday();
                        //Newdata.RevenueAmount = TotalInvestigationSumFromMonth;
                        //Newdata.Revenuedesc = Item.ParentDescription;
                        //Newdata.serviceid = Item.OLMID;
                        //revenuefortheday.Add(Newdata);
                    }
                }
                RQD.RevenuespecificBreakUPTOtalDetailsss = revenuefortheday;
            }



            //to revenuee aurterly


            if (Tomonth == "Jan - Feb - Mar")
            {
                string mnthname = "January";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var FromPAdetaillist = (from PDD in PAdetail.Where(x => x.OLMID == onelineValue
                                        && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                        join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                        join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                        select new
                                        {
                                            Revamt = PDD.TotalProductValue,
                                        }).ToList();
                var TotalInvestigationSumFromMonth = FromPAdetaillist.Sum(x => x.Revamt);

                foreach (var Item in oneline)
                {

                    var listvalue = PAdetail.Where(x => x.OLMID == Item.OLMID
                   && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear)).Select(x => x.OLMID).FirstOrDefault();
                    var nonlistvale = oneline.Where(x => x.OLMID == listvalue).Select(x => x.ParentDescription).FirstOrDefault();

                    if (nonlistvale != "Investigation" || nonlistvale == null)
                    {

                        if (listvalue != 0)
                        {
                            var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                       && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                                join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                                join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                                select new
                                                {
                                                    uin = rg.UIN,
                                                    name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                    age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                    gender = rg.Gender,
                                                    phone = rg.Phone,
                                                    address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                    Description = Item.ParentDescription,
                                                    Revamt = PDD.TotalProductValue,
                                                }).ToList();

                            if (listAmtvalue.Count() != 0)
                            {
                                foreach (var list in listAmtvalue)
                                {
                                    var Newdata = new toRevenuespecificBreakUPTOtalDetails();
                                    Newdata.UIN = list.uin;
                                    Newdata.Name = list.name;
                                    Newdata.Age = list.age;
                                    Newdata.Gender = list.gender;
                                    Newdata.Phone = list.phone;
                                    Newdata.Address1 = list.address;
                                    Newdata.Specilaitydescription = list.Description;
                                    Newdata.Specilatyamount = list.Revamt;

                                    if (list.uin != null)
                                    {
                                        RevenuefortheMonth.Add(Newdata);
                                    }

                                }
                            }
                        }

                    }
                    else
                    {

                        var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                      && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                            join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                            join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                            select new
                                            {
                                                uin = rg.UIN,
                                                name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                gender = rg.Gender,
                                                phone = rg.Phone,
                                                address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                Description = Item.ParentDescription,
                                                Revamt = TotalInvestigationSumFromMonth,
                                            }).ToList();

                        if (listAmtvalue.Count() != 0)
                        {
                            foreach (var list in listAmtvalue)
                            {
                                var Newdata = new toRevenuespecificBreakUPTOtalDetails();
                                Newdata.UIN = list.uin;
                                Newdata.Name = list.name;
                                Newdata.Age = list.age;
                                Newdata.Gender = list.gender;
                                Newdata.Phone = list.phone;
                                Newdata.Address1 = list.address;
                                Newdata.Specilaitydescription = list.Description;
                                Newdata.Specilatyamount = list.Revamt;

                                if (list.uin != null)
                                {
                                    RevenuefortheMonth.Add(Newdata);
                                }

                            }
                        }


                        //var Newdata = new Revenuefortheday();
                        //Newdata.RevenueAmount = TotalInvestigationSumFromMonth;
                        //Newdata.Revenuedesc = Item.ParentDescription;
                        //Newdata.serviceid = Item.OLMID;
                        //revenuefortheday.Add(Newdata);
                    }
                }
                RQD.toRevenuespecificBreakUPTOtalDetailss = RevenuefortheMonth;
            }
            else if (Tomonth == "Feb - Mar - Apr")
            {
                string mnthname = "February";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var FromPAdetaillist = (from PDD in PAdetail.Where(x => x.OLMID == onelineValue
                                        && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                        join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                        join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                        select new
                                        {
                                            Revamt = PDD.TotalProductValue,
                                        }).ToList();
                var TotalInvestigationSumFromMonth = FromPAdetaillist.Sum(x => x.Revamt);

                foreach (var Item in oneline)
                {

                    var listvalue = PAdetail.Where(x => x.OLMID == Item.OLMID
                   && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear)).Select(x => x.OLMID).FirstOrDefault();
                    var nonlistvale = oneline.Where(x => x.OLMID == listvalue).Select(x => x.ParentDescription).FirstOrDefault();

                    if (nonlistvale != "Investigation" || nonlistvale == null)
                    {

                        if (listvalue != 0)
                        {
                            var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                       && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                                join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                                join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                                select new
                                                {
                                                    uin = rg.UIN,
                                                    name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                    age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                    gender = rg.Gender,
                                                    phone = rg.Phone,
                                                    address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                    Description = Item.ParentDescription,
                                                    Revamt = PDD.TotalProductValue,
                                                }).ToList();

                            if (listAmtvalue.Count() != 0)
                            {
                                foreach (var list in listAmtvalue)
                                {
                                    var Newdata = new toRevenuespecificBreakUPTOtalDetails();
                                    Newdata.UIN = list.uin;
                                    Newdata.Name = list.name;
                                    Newdata.Age = list.age;
                                    Newdata.Gender = list.gender;
                                    Newdata.Phone = list.phone;
                                    Newdata.Address1 = list.address;
                                    Newdata.Specilaitydescription = list.Description;
                                    Newdata.Specilatyamount = list.Revamt;

                                    if (list.uin != null)
                                    {
                                        RevenuefortheMonth.Add(Newdata);
                                    }

                                }
                            }
                        }

                    }
                    else
                    {

                        var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                      && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                            join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                            join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                            select new
                                            {
                                                uin = rg.UIN,
                                                name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                gender = rg.Gender,
                                                phone = rg.Phone,
                                                address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                Description = Item.ParentDescription,
                                                Revamt = TotalInvestigationSumFromMonth,
                                            }).ToList();

                        if (listAmtvalue.Count() != 0)
                        {
                            foreach (var list in listAmtvalue)
                            {
                                var Newdata = new toRevenuespecificBreakUPTOtalDetails();
                                Newdata.UIN = list.uin;
                                Newdata.Name = list.name;
                                Newdata.Age = list.age;
                                Newdata.Gender = list.gender;
                                Newdata.Phone = list.phone;
                                Newdata.Address1 = list.address;
                                Newdata.Specilaitydescription = list.Description;
                                Newdata.Specilatyamount = list.Revamt;

                                if (list.uin != null)
                                {
                                    RevenuefortheMonth.Add(Newdata);
                                }

                            }
                        }


                        //var Newdata = new Revenuefortheday();
                        //Newdata.RevenueAmount = TotalInvestigationSumFromMonth;
                        //Newdata.Revenuedesc = Item.ParentDescription;
                        //Newdata.serviceid = Item.OLMID;
                        //revenuefortheday.Add(Newdata);
                    }
                }
                RQD.toRevenuespecificBreakUPTOtalDetailss = RevenuefortheMonth;
            }
            else if (Tomonth == "Mar - Apr - May")
            {
                string mnthname = "March";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var FromPAdetaillist = (from PDD in PAdetail.Where(x => x.OLMID == onelineValue
                                        && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                        join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                        join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                        select new
                                        {
                                            Revamt = PDD.TotalProductValue,
                                        }).ToList();
                var TotalInvestigationSumFromMonth = FromPAdetaillist.Sum(x => x.Revamt);

                foreach (var Item in oneline)
                {

                    var listvalue = PAdetail.Where(x => x.OLMID == Item.OLMID
                   && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear)).Select(x => x.OLMID).FirstOrDefault();
                    var nonlistvale = oneline.Where(x => x.OLMID == listvalue).Select(x => x.ParentDescription).FirstOrDefault();

                    if (nonlistvale != "Investigation" || nonlistvale == null)
                    {

                        if (listvalue != 0)
                        {
                            var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                       && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                                join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                                join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                                select new
                                                {
                                                    uin = rg.UIN,
                                                    name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                    age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                    gender = rg.Gender,
                                                    phone = rg.Phone,
                                                    address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                    Description = Item.ParentDescription,
                                                    Revamt = PDD.TotalProductValue,
                                                }).ToList();

                            if (listAmtvalue.Count() != 0)
                            {
                                foreach (var list in listAmtvalue)
                                {
                                    var Newdata = new toRevenuespecificBreakUPTOtalDetails();
                                    Newdata.UIN = list.uin;
                                    Newdata.Name = list.name;
                                    Newdata.Age = list.age;
                                    Newdata.Gender = list.gender;
                                    Newdata.Phone = list.phone;
                                    Newdata.Address1 = list.address;
                                    Newdata.Specilaitydescription = list.Description;
                                    Newdata.Specilatyamount = list.Revamt;

                                    if (list.uin != null)
                                    {
                                        RevenuefortheMonth.Add(Newdata);
                                    }

                                }
                            }
                        }

                    }
                    else
                    {

                        var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                      && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                            join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                            join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                            select new
                                            {
                                                uin = rg.UIN,
                                                name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                gender = rg.Gender,
                                                phone = rg.Phone,
                                                address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                Description = Item.ParentDescription,
                                                Revamt = TotalInvestigationSumFromMonth,
                                            }).ToList();

                        if (listAmtvalue.Count() != 0)
                        {
                            foreach (var list in listAmtvalue)
                            {
                                var Newdata = new toRevenuespecificBreakUPTOtalDetails();
                                Newdata.UIN = list.uin;
                                Newdata.Name = list.name;
                                Newdata.Age = list.age;
                                Newdata.Gender = list.gender;
                                Newdata.Phone = list.phone;
                                Newdata.Address1 = list.address;
                                Newdata.Specilaitydescription = list.Description;
                                Newdata.Specilatyamount = list.Revamt;

                                if (list.uin != null)
                                {
                                    RevenuefortheMonth.Add(Newdata);
                                }

                            }
                        }


                        //var Newdata = new Revenuefortheday();
                        //Newdata.RevenueAmount = TotalInvestigationSumFromMonth;
                        //Newdata.Revenuedesc = Item.ParentDescription;
                        //Newdata.serviceid = Item.OLMID;
                        //revenuefortheday.Add(Newdata);
                    }
                }
                RQD.toRevenuespecificBreakUPTOtalDetailss = RevenuefortheMonth;
            }
            else if (Tomonth == "Apr- May - Jun")
            {
                string mnthname = "April";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var FromPAdetaillist = (from PDD in PAdetail.Where(x => x.OLMID == onelineValue
                                        && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                        join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                        join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                        select new
                                        {
                                            Revamt = PDD.TotalProductValue,
                                        }).ToList();
                var TotalInvestigationSumFromMonth = FromPAdetaillist.Sum(x => x.Revamt);

                foreach (var Item in oneline)
                {

                    var listvalue = PAdetail.Where(x => x.OLMID == Item.OLMID
                   && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear)).Select(x => x.OLMID).FirstOrDefault();
                    var nonlistvale = oneline.Where(x => x.OLMID == listvalue).Select(x => x.ParentDescription).FirstOrDefault();

                    if (nonlistvale != "Investigation" || nonlistvale == null)
                    {

                        if (listvalue != 0)
                        {
                            var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                       && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                                join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                                join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                                select new
                                                {
                                                    uin = rg.UIN,
                                                    name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                    age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                    gender = rg.Gender,
                                                    phone = rg.Phone,
                                                    address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                    Description = Item.ParentDescription,
                                                    Revamt = PDD.TotalProductValue,
                                                }).ToList();

                            if (listAmtvalue.Count() != 0)
                            {
                                foreach (var list in listAmtvalue)
                                {
                                    var Newdata = new toRevenuespecificBreakUPTOtalDetails();
                                    Newdata.UIN = list.uin;
                                    Newdata.Name = list.name;
                                    Newdata.Age = list.age;
                                    Newdata.Gender = list.gender;
                                    Newdata.Phone = list.phone;
                                    Newdata.Address1 = list.address;
                                    Newdata.Specilaitydescription = list.Description;
                                    Newdata.Specilatyamount = list.Revamt;

                                    if (list.uin != null)
                                    {
                                        RevenuefortheMonth.Add(Newdata);
                                    }

                                }
                            }
                        }

                    }
                    else
                    {

                        var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                      && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                            join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                            join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                            select new
                                            {
                                                uin = rg.UIN,
                                                name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                gender = rg.Gender,
                                                phone = rg.Phone,
                                                address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                Description = Item.ParentDescription,
                                                Revamt = TotalInvestigationSumFromMonth,
                                            }).ToList();

                        if (listAmtvalue.Count() != 0)
                        {
                            foreach (var list in listAmtvalue)
                            {
                                var Newdata = new toRevenuespecificBreakUPTOtalDetails();
                                Newdata.UIN = list.uin;
                                Newdata.Name = list.name;
                                Newdata.Age = list.age;
                                Newdata.Gender = list.gender;
                                Newdata.Phone = list.phone;
                                Newdata.Address1 = list.address;
                                Newdata.Specilaitydescription = list.Description;
                                Newdata.Specilatyamount = list.Revamt;

                                if (list.uin != null)
                                {
                                    RevenuefortheMonth.Add(Newdata);
                                }

                            }
                        }


                        //var Newdata = new Revenuefortheday();
                        //Newdata.RevenueAmount = TotalInvestigationSumFromMonth;
                        //Newdata.Revenuedesc = Item.ParentDescription;
                        //Newdata.serviceid = Item.OLMID;
                        //revenuefortheday.Add(Newdata);
                    }
                }
                RQD.toRevenuespecificBreakUPTOtalDetailss = RevenuefortheMonth;
            }
            else if (Tomonth == "May - Jun - Jul")
            {
                string mnthname = "May";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var FromPAdetaillist = (from PDD in PAdetail.Where(x => x.OLMID == onelineValue
                                        && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                        join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                        join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                        select new
                                        {
                                            Revamt = PDD.TotalProductValue,
                                        }).ToList();
                var TotalInvestigationSumFromMonth = FromPAdetaillist.Sum(x => x.Revamt);

                foreach (var Item in oneline)
                {

                    var listvalue = PAdetail.Where(x => x.OLMID == Item.OLMID
                   && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear)).Select(x => x.OLMID).FirstOrDefault();
                    var nonlistvale = oneline.Where(x => x.OLMID == listvalue).Select(x => x.ParentDescription).FirstOrDefault();

                    if (nonlistvale != "Investigation" || nonlistvale == null)
                    {

                        if (listvalue != 0)
                        {
                            var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                       && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                                join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                                join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                                select new
                                                {
                                                    uin = rg.UIN,
                                                    name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                    age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                    gender = rg.Gender,
                                                    phone = rg.Phone,
                                                    address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                    Description = Item.ParentDescription,
                                                    Revamt = PDD.TotalProductValue,
                                                }).ToList();

                            if (listAmtvalue.Count() != 0)
                            {
                                foreach (var list in listAmtvalue)
                                {
                                    var Newdata = new toRevenuespecificBreakUPTOtalDetails();
                                    Newdata.UIN = list.uin;
                                    Newdata.Name = list.name;
                                    Newdata.Age = list.age;
                                    Newdata.Gender = list.gender;
                                    Newdata.Phone = list.phone;
                                    Newdata.Address1 = list.address;
                                    Newdata.Specilaitydescription = list.Description;
                                    Newdata.Specilatyamount = list.Revamt;

                                    if (list.uin != null)
                                    {
                                        RevenuefortheMonth.Add(Newdata);
                                    }

                                }
                            }
                        }

                    }
                    else
                    {

                        var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                      && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                            join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                            join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                            select new
                                            {
                                                uin = rg.UIN,
                                                name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                gender = rg.Gender,
                                                phone = rg.Phone,
                                                address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                Description = Item.ParentDescription,
                                                Revamt = TotalInvestigationSumFromMonth,
                                            }).ToList();

                        if (listAmtvalue.Count() != 0)
                        {
                            foreach (var list in listAmtvalue)
                            {
                                var Newdata = new toRevenuespecificBreakUPTOtalDetails();
                                Newdata.UIN = list.uin;
                                Newdata.Name = list.name;
                                Newdata.Age = list.age;
                                Newdata.Gender = list.gender;
                                Newdata.Phone = list.phone;
                                Newdata.Address1 = list.address;
                                Newdata.Specilaitydescription = list.Description;
                                Newdata.Specilatyamount = list.Revamt;

                                if (list.uin != null)
                                {
                                    RevenuefortheMonth.Add(Newdata);
                                }

                            }
                        }


                        //var Newdata = new Revenuefortheday();
                        //Newdata.RevenueAmount = TotalInvestigationSumFromMonth;
                        //Newdata.Revenuedesc = Item.ParentDescription;
                        //Newdata.serviceid = Item.OLMID;
                        //revenuefortheday.Add(Newdata);
                    }
                }
                RQD.toRevenuespecificBreakUPTOtalDetailss = RevenuefortheMonth;
            }
            else if (Tomonth == "Jun - Jul - Aug")
            {
                string mnthname = "June";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var FromPAdetaillist = (from PDD in PAdetail.Where(x => x.OLMID == onelineValue
                                        && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                        join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                        join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                        select new
                                        {
                                            Revamt = PDD.TotalProductValue,
                                        }).ToList();
                var TotalInvestigationSumFromMonth = FromPAdetaillist.Sum(x => x.Revamt);

                foreach (var Item in oneline)
                {

                    var listvalue = PAdetail.Where(x => x.OLMID == Item.OLMID
                   && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear)).Select(x => x.OLMID).FirstOrDefault();
                    var nonlistvale = oneline.Where(x => x.OLMID == listvalue).Select(x => x.ParentDescription).FirstOrDefault();

                    if (nonlistvale != "Investigation" || nonlistvale == null)
                    {

                        if (listvalue != 0)
                        {
                            var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                       && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                                join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                                join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                                select new
                                                {
                                                    uin = rg.UIN,
                                                    name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                    age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                    gender = rg.Gender,
                                                    phone = rg.Phone,
                                                    address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                    Description = Item.ParentDescription,
                                                    Revamt = PDD.TotalProductValue,
                                                }).ToList();

                            if (listAmtvalue.Count() != 0)
                            {
                                foreach (var list in listAmtvalue)
                                {
                                    var Newdata = new toRevenuespecificBreakUPTOtalDetails();
                                    Newdata.UIN = list.uin;
                                    Newdata.Name = list.name;
                                    Newdata.Age = list.age;
                                    Newdata.Gender = list.gender;
                                    Newdata.Phone = list.phone;
                                    Newdata.Address1 = list.address;
                                    Newdata.Specilaitydescription = list.Description;
                                    Newdata.Specilatyamount = list.Revamt;

                                    if (list.uin != null)
                                    {
                                        RevenuefortheMonth.Add(Newdata);
                                    }

                                }
                            }
                        }

                    }
                    else
                    {

                        var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                      && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                            join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                            join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                            select new
                                            {
                                                uin = rg.UIN,
                                                name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                gender = rg.Gender,
                                                phone = rg.Phone,
                                                address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                Description = Item.ParentDescription,
                                                Revamt = TotalInvestigationSumFromMonth,
                                            }).ToList();

                        if (listAmtvalue.Count() != 0)
                        {
                            foreach (var list in listAmtvalue)
                            {
                                var Newdata = new toRevenuespecificBreakUPTOtalDetails();
                                Newdata.UIN = list.uin;
                                Newdata.Name = list.name;
                                Newdata.Age = list.age;
                                Newdata.Gender = list.gender;
                                Newdata.Phone = list.phone;
                                Newdata.Address1 = list.address;
                                Newdata.Specilaitydescription = list.Description;
                                Newdata.Specilatyamount = list.Revamt;

                                if (list.uin != null)
                                {
                                    RevenuefortheMonth.Add(Newdata);
                                }

                            }
                        }


                        //var Newdata = new Revenuefortheday();
                        //Newdata.RevenueAmount = TotalInvestigationSumFromMonth;
                        //Newdata.Revenuedesc = Item.ParentDescription;
                        //Newdata.serviceid = Item.OLMID;
                        //revenuefortheday.Add(Newdata);
                    }
                }
                RQD.toRevenuespecificBreakUPTOtalDetailss = RevenuefortheMonth;
            }
            else if (Tomonth == "Jul - Aug - Sep")
            {
                string mnthname = "July";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var FromPAdetaillist = (from PDD in PAdetail.Where(x => x.OLMID == onelineValue
                                        && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                        join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                        join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                        select new
                                        {
                                            Revamt = PDD.TotalProductValue,
                                        }).ToList();
                var TotalInvestigationSumFromMonth = FromPAdetaillist.Sum(x => x.Revamt);

                foreach (var Item in oneline)
                {

                    var listvalue = PAdetail.Where(x => x.OLMID == Item.OLMID
                   && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear)).Select(x => x.OLMID).FirstOrDefault();
                    var nonlistvale = oneline.Where(x => x.OLMID == listvalue).Select(x => x.ParentDescription).FirstOrDefault();

                    if (nonlistvale != "Investigation" || nonlistvale == null)
                    {

                        if (listvalue != 0)
                        {
                            var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                       && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                                join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                                join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                                select new
                                                {
                                                    uin = rg.UIN,
                                                    name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                    age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                    gender = rg.Gender,
                                                    phone = rg.Phone,
                                                    address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                    Description = Item.ParentDescription,
                                                    Revamt = PDD.TotalProductValue,
                                                }).ToList();

                            if (listAmtvalue.Count() != 0)
                            {
                                foreach (var list in listAmtvalue)
                                {
                                    var Newdata = new toRevenuespecificBreakUPTOtalDetails();
                                    Newdata.UIN = list.uin;
                                    Newdata.Name = list.name;
                                    Newdata.Age = list.age;
                                    Newdata.Gender = list.gender;
                                    Newdata.Phone = list.phone;
                                    Newdata.Address1 = list.address;
                                    Newdata.Specilaitydescription = list.Description;
                                    Newdata.Specilatyamount = list.Revamt;

                                    if (list.uin != null)
                                    {
                                        RevenuefortheMonth.Add(Newdata);
                                    }

                                }
                            }
                        }

                    }
                    else
                    {

                        var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                      && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                            join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                            join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                            select new
                                            {
                                                uin = rg.UIN,
                                                name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                gender = rg.Gender,
                                                phone = rg.Phone,
                                                address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                Description = Item.ParentDescription,
                                                Revamt = TotalInvestigationSumFromMonth,
                                            }).ToList();

                        if (listAmtvalue.Count() != 0)
                        {
                            foreach (var list in listAmtvalue)
                            {
                                var Newdata = new toRevenuespecificBreakUPTOtalDetails();
                                Newdata.UIN = list.uin;
                                Newdata.Name = list.name;
                                Newdata.Age = list.age;
                                Newdata.Gender = list.gender;
                                Newdata.Phone = list.phone;
                                Newdata.Address1 = list.address;
                                Newdata.Specilaitydescription = list.Description;
                                Newdata.Specilatyamount = list.Revamt;

                                if (list.uin != null)
                                {
                                    RevenuefortheMonth.Add(Newdata);
                                }

                            }
                        }


                        //var Newdata = new Revenuefortheday();
                        //Newdata.RevenueAmount = TotalInvestigationSumFromMonth;
                        //Newdata.Revenuedesc = Item.ParentDescription;
                        //Newdata.serviceid = Item.OLMID;
                        //revenuefortheday.Add(Newdata);
                    }
                }
                RQD.toRevenuespecificBreakUPTOtalDetailss = RevenuefortheMonth;
            }
            else if (Tomonth == "Aug - Sep - Oct")
            {
                string mnthname = "August";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var FromPAdetaillist = (from PDD in PAdetail.Where(x => x.OLMID == onelineValue
                                        && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                        join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                        join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                        select new
                                        {
                                            Revamt = PDD.TotalProductValue,
                                        }).ToList();
                var TotalInvestigationSumFromMonth = FromPAdetaillist.Sum(x => x.Revamt);

                foreach (var Item in oneline)
                {

                    var listvalue = PAdetail.Where(x => x.OLMID == Item.OLMID
                   && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear)).Select(x => x.OLMID).FirstOrDefault();
                    var nonlistvale = oneline.Where(x => x.OLMID == listvalue).Select(x => x.ParentDescription).FirstOrDefault();

                    if (nonlistvale != "Investigation" || nonlistvale == null)
                    {

                        if (listvalue != 0)
                        {
                            var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                       && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                                join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                                join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                                select new
                                                {
                                                    uin = rg.UIN,
                                                    name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                    age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                    gender = rg.Gender,
                                                    phone = rg.Phone,
                                                    address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                    Description = Item.ParentDescription,
                                                    Revamt = PDD.TotalProductValue,
                                                }).ToList();

                            if (listAmtvalue.Count() != 0)
                            {
                                foreach (var list in listAmtvalue)
                                {
                                    var Newdata = new toRevenuespecificBreakUPTOtalDetails();
                                    Newdata.UIN = list.uin;
                                    Newdata.Name = list.name;
                                    Newdata.Age = list.age;
                                    Newdata.Gender = list.gender;
                                    Newdata.Phone = list.phone;
                                    Newdata.Address1 = list.address;
                                    Newdata.Specilaitydescription = list.Description;
                                    Newdata.Specilatyamount = list.Revamt;

                                    if (list.uin != null)
                                    {
                                        RevenuefortheMonth.Add(Newdata);
                                    }

                                }
                            }
                        }

                    }
                    else
                    {

                        var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                      && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                            join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                            join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                            select new
                                            {
                                                uin = rg.UIN,
                                                name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                gender = rg.Gender,
                                                phone = rg.Phone,
                                                address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                Description = Item.ParentDescription,
                                                Revamt = TotalInvestigationSumFromMonth,
                                            }).ToList();

                        if (listAmtvalue.Count() != 0)
                        {
                            foreach (var list in listAmtvalue)
                            {
                                var Newdata = new toRevenuespecificBreakUPTOtalDetails();
                                Newdata.UIN = list.uin;
                                Newdata.Name = list.name;
                                Newdata.Age = list.age;
                                Newdata.Gender = list.gender;
                                Newdata.Phone = list.phone;
                                Newdata.Address1 = list.address;
                                Newdata.Specilaitydescription = list.Description;
                                Newdata.Specilatyamount = list.Revamt;

                                if (list.uin != null)
                                {
                                    RevenuefortheMonth.Add(Newdata);
                                }

                            }
                        }


                        //var Newdata = new Revenuefortheday();
                        //Newdata.RevenueAmount = TotalInvestigationSumFromMonth;
                        //Newdata.Revenuedesc = Item.ParentDescription;
                        //Newdata.serviceid = Item.OLMID;
                        //revenuefortheday.Add(Newdata);
                    }
                }
                RQD.toRevenuespecificBreakUPTOtalDetailss = RevenuefortheMonth;
            }
            else if (Tomonth == "Sep - Oct - Nov")
            {
                string mnthname = "September";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var FromPAdetaillist = (from PDD in PAdetail.Where(x => x.OLMID == onelineValue
                                        && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                        join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                        join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                        select new
                                        {
                                            Revamt = PDD.TotalProductValue,
                                        }).ToList();
                var TotalInvestigationSumFromMonth = FromPAdetaillist.Sum(x => x.Revamt);

                foreach (var Item in oneline)
                {

                    var listvalue = PAdetail.Where(x => x.OLMID == Item.OLMID
                   && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear)).Select(x => x.OLMID).FirstOrDefault();
                    var nonlistvale = oneline.Where(x => x.OLMID == listvalue).Select(x => x.ParentDescription).FirstOrDefault();

                    if (nonlistvale != "Investigation" || nonlistvale == null)
                    {

                        if (listvalue != 0)
                        {
                            var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                       && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                                join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                                join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                                select new
                                                {
                                                    uin = rg.UIN,
                                                    name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                    age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                    gender = rg.Gender,
                                                    phone = rg.Phone,
                                                    address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                    Description = Item.ParentDescription,
                                                    Revamt = PDD.TotalProductValue,
                                                }).ToList();

                            if (listAmtvalue.Count() != 0)
                            {
                                foreach (var list in listAmtvalue)
                                {
                                    var Newdata = new toRevenuespecificBreakUPTOtalDetails();
                                    Newdata.UIN = list.uin;
                                    Newdata.Name = list.name;
                                    Newdata.Age = list.age;
                                    Newdata.Gender = list.gender;
                                    Newdata.Phone = list.phone;
                                    Newdata.Address1 = list.address;
                                    Newdata.Specilaitydescription = list.Description;
                                    Newdata.Specilatyamount = list.Revamt;

                                    if (list.uin != null)
                                    {
                                        RevenuefortheMonth.Add(Newdata);
                                    }

                                }
                            }
                        }

                    }
                    else
                    {

                        var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                      && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                            join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                            join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                            select new
                                            {
                                                uin = rg.UIN,
                                                name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                gender = rg.Gender,
                                                phone = rg.Phone,
                                                address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                Description = Item.ParentDescription,
                                                Revamt = TotalInvestigationSumFromMonth,
                                            }).ToList();

                        if (listAmtvalue.Count() != 0)
                        {
                            foreach (var list in listAmtvalue)
                            {
                                var Newdata = new toRevenuespecificBreakUPTOtalDetails();
                                Newdata.UIN = list.uin;
                                Newdata.Name = list.name;
                                Newdata.Age = list.age;
                                Newdata.Gender = list.gender;
                                Newdata.Phone = list.phone;
                                Newdata.Address1 = list.address;
                                Newdata.Specilaitydescription = list.Description;
                                Newdata.Specilatyamount = list.Revamt;

                                if (list.uin != null)
                                {
                                    RevenuefortheMonth.Add(Newdata);
                                }

                            }
                        }


                        //var Newdata = new Revenuefortheday();
                        //Newdata.RevenueAmount = TotalInvestigationSumFromMonth;
                        //Newdata.Revenuedesc = Item.ParentDescription;
                        //Newdata.serviceid = Item.OLMID;
                        //revenuefortheday.Add(Newdata);
                    }
                }
                RQD.toRevenuespecificBreakUPTOtalDetailss = RevenuefortheMonth;
            }
            else if (Tomonth == "Oct - Nov - Dec")
            {
                string mnthname = "October";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var FromPAdetaillist = (from PDD in PAdetail.Where(x => x.OLMID == onelineValue
                                        && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                        join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                        join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                        select new
                                        {
                                            Revamt = PDD.TotalProductValue,
                                        }).ToList();
                var TotalInvestigationSumFromMonth = FromPAdetaillist.Sum(x => x.Revamt);

                foreach (var Item in oneline)
                {

                    var listvalue = PAdetail.Where(x => x.OLMID == Item.OLMID
                   && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear)).Select(x => x.OLMID).FirstOrDefault();
                    var nonlistvale = oneline.Where(x => x.OLMID == listvalue).Select(x => x.ParentDescription).FirstOrDefault();

                    if (nonlistvale != "Investigation" || nonlistvale == null)
                    {

                        if (listvalue != 0)
                        {
                            var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                       && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                                join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                                join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                                select new
                                                {
                                                    uin = rg.UIN,
                                                    name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                    age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                    gender = rg.Gender,
                                                    phone = rg.Phone,
                                                    address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                    Description = Item.ParentDescription,
                                                    Revamt = PDD.TotalProductValue,
                                                }).ToList();

                            if (listAmtvalue.Count() != 0)
                            {
                                foreach (var list in listAmtvalue)
                                {
                                    var Newdata = new toRevenuespecificBreakUPTOtalDetails();
                                    Newdata.UIN = list.uin;
                                    Newdata.Name = list.name;
                                    Newdata.Age = list.age;
                                    Newdata.Gender = list.gender;
                                    Newdata.Phone = list.phone;
                                    Newdata.Address1 = list.address;
                                    Newdata.Specilaitydescription = list.Description;
                                    Newdata.Specilatyamount = list.Revamt;

                                    if (list.uin != null)
                                    {
                                        RevenuefortheMonth.Add(Newdata);
                                    }

                                }
                            }
                        }

                    }
                    else
                    {

                        var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                      && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                            join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                            join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                            select new
                                            {
                                                uin = rg.UIN,
                                                name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                gender = rg.Gender,
                                                phone = rg.Phone,
                                                address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                Description = Item.ParentDescription,
                                                Revamt = TotalInvestigationSumFromMonth,
                                            }).ToList();

                        if (listAmtvalue.Count() != 0)
                        {
                            foreach (var list in listAmtvalue)
                            {
                                var Newdata = new toRevenuespecificBreakUPTOtalDetails();
                                Newdata.UIN = list.uin;
                                Newdata.Name = list.name;
                                Newdata.Age = list.age;
                                Newdata.Gender = list.gender;
                                Newdata.Phone = list.phone;
                                Newdata.Address1 = list.address;
                                Newdata.Specilaitydescription = list.Description;
                                Newdata.Specilatyamount = list.Revamt;

                                if (list.uin != null)
                                {
                                    RevenuefortheMonth.Add(Newdata);
                                }

                            }
                        }


                        //var Newdata = new Revenuefortheday();
                        //Newdata.RevenueAmount = TotalInvestigationSumFromMonth;
                        //Newdata.Revenuedesc = Item.ParentDescription;
                        //Newdata.serviceid = Item.OLMID;
                        //revenuefortheday.Add(Newdata);
                    }
                }
                RQD.toRevenuespecificBreakUPTOtalDetailss = RevenuefortheMonth;
            }
            else if (Tomonth == "Nov - Dec - Jan")
            {
                string mnthname = "November";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var FromPAdetaillist = (from PDD in PAdetail.Where(x => x.OLMID == onelineValue
                                        && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                        join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                        join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                        select new
                                        {
                                            Revamt = PDD.TotalProductValue,
                                        }).ToList();
                var TotalInvestigationSumFromMonth = FromPAdetaillist.Sum(x => x.Revamt);

                foreach (var Item in oneline)
                {

                    var listvalue = PAdetail.Where(x => x.OLMID == Item.OLMID
                   && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear)).Select(x => x.OLMID).FirstOrDefault();
                    var nonlistvale = oneline.Where(x => x.OLMID == listvalue).Select(x => x.ParentDescription).FirstOrDefault();

                    if (nonlistvale != "Investigation" || nonlistvale == null)
                    {

                        if (listvalue != 0)
                        {
                            var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                       && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                                join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                                join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                                select new
                                                {
                                                    uin = rg.UIN,
                                                    name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                    age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                    gender = rg.Gender,
                                                    phone = rg.Phone,
                                                    address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                    Description = Item.ParentDescription,
                                                    Revamt = PDD.TotalProductValue,
                                                }).ToList();

                            if (listAmtvalue.Count() != 0)
                            {
                                foreach (var list in listAmtvalue)
                                {
                                    var Newdata = new toRevenuespecificBreakUPTOtalDetails();
                                    Newdata.UIN = list.uin;
                                    Newdata.Name = list.name;
                                    Newdata.Age = list.age;
                                    Newdata.Gender = list.gender;
                                    Newdata.Phone = list.phone;
                                    Newdata.Address1 = list.address;
                                    Newdata.Specilaitydescription = list.Description;
                                    Newdata.Specilatyamount = list.Revamt;

                                    if (list.uin != null)
                                    {
                                        RevenuefortheMonth.Add(Newdata);
                                    }

                                }
                            }
                        }

                    }
                    else
                    {

                        var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                      && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                            join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                            join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                            select new
                                            {
                                                uin = rg.UIN,
                                                name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                gender = rg.Gender,
                                                phone = rg.Phone,
                                                address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                Description = Item.ParentDescription,
                                                Revamt = TotalInvestigationSumFromMonth,
                                            }).ToList();

                        if (listAmtvalue.Count() != 0)
                        {
                            foreach (var list in listAmtvalue)
                            {
                                var Newdata = new toRevenuespecificBreakUPTOtalDetails();
                                Newdata.UIN = list.uin;
                                Newdata.Name = list.name;
                                Newdata.Age = list.age;
                                Newdata.Gender = list.gender;
                                Newdata.Phone = list.phone;
                                Newdata.Address1 = list.address;
                                Newdata.Specilaitydescription = list.Description;
                                Newdata.Specilatyamount = list.Revamt;

                                if (list.uin != null)
                                {
                                    RevenuefortheMonth.Add(Newdata);
                                }

                            }
                        }


                        //var Newdata = new Revenuefortheday();
                        //Newdata.RevenueAmount = TotalInvestigationSumFromMonth;
                        //Newdata.Revenuedesc = Item.ParentDescription;
                        //Newdata.serviceid = Item.OLMID;
                        //revenuefortheday.Add(Newdata);
                    }
                }
                RQD.toRevenuespecificBreakUPTOtalDetailss = RevenuefortheMonth;
            }
            else if (Tomonth == "Dec - Jan - Feb")
            {
                string mnthname = "December";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var FromPAdetaillist = (from PDD in PAdetail.Where(x => x.OLMID == onelineValue
                                        && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                        join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                        join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                        select new
                                        {
                                            Revamt = PDD.TotalProductValue,
                                        }).ToList();
                var TotalInvestigationSumFromMonth = FromPAdetaillist.Sum(x => x.Revamt);

                foreach (var Item in oneline)
                {

                    var listvalue = PAdetail.Where(x => x.OLMID == Item.OLMID
                   && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear)).Select(x => x.OLMID).FirstOrDefault();
                    var nonlistvale = oneline.Where(x => x.OLMID == listvalue).Select(x => x.ParentDescription).FirstOrDefault();

                    if (nonlistvale != "Investigation" || nonlistvale == null)
                    {

                        if (listvalue != 0)
                        {
                            var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                       && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                                join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                                join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                                select new
                                                {
                                                    uin = rg.UIN,
                                                    name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                    age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                    gender = rg.Gender,
                                                    phone = rg.Phone,
                                                    address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                    Description = Item.ParentDescription,
                                                    Revamt = PDD.TotalProductValue,
                                                }).ToList();

                            if (listAmtvalue.Count() != 0)
                            {
                                foreach (var list in listAmtvalue)
                                {
                                    var Newdata = new toRevenuespecificBreakUPTOtalDetails();
                                    Newdata.UIN = list.uin;
                                    Newdata.Name = list.name;
                                    Newdata.Age = list.age;
                                    Newdata.Gender = list.gender;
                                    Newdata.Phone = list.phone;
                                    Newdata.Address1 = list.address;
                                    Newdata.Specilaitydescription = list.Description;
                                    Newdata.Specilatyamount = list.Revamt;

                                    if (list.uin != null)
                                    {
                                        RevenuefortheMonth.Add(Newdata);
                                    }

                                }
                            }
                        }

                    }
                    else
                    {

                        var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == Item.OLMID
                                      && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                            join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                            join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                            select new
                                            {
                                                uin = rg.UIN,
                                                name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                                age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                                gender = rg.Gender,
                                                phone = rg.Phone,
                                                address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                                Description = Item.ParentDescription,
                                                Revamt = TotalInvestigationSumFromMonth,
                                            }).ToList();

                        if (listAmtvalue.Count() != 0)
                        {
                            foreach (var list in listAmtvalue)
                            {
                                var Newdata = new toRevenuespecificBreakUPTOtalDetails();
                                Newdata.UIN = list.uin;
                                Newdata.Name = list.name;
                                Newdata.Age = list.age;
                                Newdata.Gender = list.gender;
                                Newdata.Phone = list.phone;
                                Newdata.Address1 = list.address;
                                Newdata.Specilaitydescription = list.Description;
                                Newdata.Specilatyamount = list.Revamt;

                                if (list.uin != null)
                                {
                                    RevenuefortheMonth.Add(Newdata);
                                }

                            }
                        }


                        //var Newdata = new Revenuefortheday();
                        //Newdata.RevenueAmount = TotalInvestigationSumFromMonth;
                        //Newdata.Revenuedesc = Item.ParentDescription;
                        //Newdata.serviceid = Item.OLMID;
                        //revenuefortheday.Add(Newdata);
                    }
                }
                RQD.toRevenuespecificBreakUPTOtalDetailss = RevenuefortheMonth;
            }


            RQD.RevenueFromdate = frommonth + '-' + selectedyear;
            RQD.RevenueTodate = Tomonth + '-' + Toyear;


            return RQD;
        }



        public ManagementDashboardViewModel GetSRevenueQuartelyBriefservicedetails(string desc, string olmid, string FromDate, string fyear, string ToDate, string toyear, int CompanyID)
        {
            var RQD = new ManagementDashboardViewModel();
            var frommonth = FromDate;
            var selectedyear = fyear;
            var Tomonth = ToDate;
            var Toyear = toyear;
            var oneline = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "Services").ToList();
            var onelineValue = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "INV").Select(x => x.ParentID).FirstOrDefault();
            var revenuefortheday = new List<RevenuespecificBreakUPTOtalDetails>();
            var RevenuefortheMonth = new List<toRevenuespecificBreakUPTOtalDetails>();
            var Patientaccount = WYNKContext.PatientAccount.Where(x => x.CMPID == CompanyID).ToList();
            var PAdetail = WYNKContext.PatientAccountDetail.ToList();
            var OLMID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == desc).Select(x => x.OLMID).FirstOrDefault();

            if (frommonth == "Jan - Feb - Mar")
            {
                string mnthname = "January";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == OLMID
                           && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                    join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                    join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                    select new
                                    {
                                        uin = rg.UIN,
                                        name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                        age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                        gender = rg.Gender,
                                        phone = rg.Phone,
                                        address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                        Description = desc,
                                        Revamt = PDD.TotalProductValue,
                                    }).ToList();

                if (listAmtvalue.Count() != 0)
                {
                    foreach (var list in listAmtvalue)
                    {
                        var Newdata = new RevenuespecificBreakUPTOtalDetails();
                        Newdata.UIN = list.uin;
                        Newdata.Name = list.name;
                        Newdata.Age = list.age;
                        Newdata.Gender = list.gender;
                        Newdata.Phone = list.phone;
                        Newdata.Address1 = list.address;
                        Newdata.Specilaitydescription = list.Description;
                        Newdata.Specilatyamount = list.Revamt;

                        if (list.uin != null)
                        {
                            revenuefortheday.Add(Newdata);
                        }

                    }
                }


                RQD.RevenuespecificBreakUPTOtalDetailsss = revenuefortheday;
            }
            else if (frommonth == "Feb - Mar - Apr")
            {
                string mnthname = "February";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == OLMID
                           && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                    join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                    join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                    select new
                                    {
                                        uin = rg.UIN,
                                        name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                        age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                        gender = rg.Gender,
                                        phone = rg.Phone,
                                        address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                        Description = desc,
                                        Revamt = PDD.TotalProductValue,
                                    }).ToList();

                if (listAmtvalue.Count() != 0)
                {
                    foreach (var list in listAmtvalue)
                    {
                        var Newdata = new RevenuespecificBreakUPTOtalDetails();
                        Newdata.UIN = list.uin;
                        Newdata.Name = list.name;
                        Newdata.Age = list.age;
                        Newdata.Gender = list.gender;
                        Newdata.Phone = list.phone;
                        Newdata.Address1 = list.address;
                        Newdata.Specilaitydescription = list.Description;
                        Newdata.Specilatyamount = list.Revamt;

                        if (list.uin != null)
                        {
                            revenuefortheday.Add(Newdata);
                        }

                    }
                }


                RQD.RevenuespecificBreakUPTOtalDetailsss = revenuefortheday;
            }
            else if (frommonth == "Mar - Apr - May")
            {
                string mnthname = "March";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == OLMID
                           && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                    join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                    join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                    select new
                                    {
                                        uin = rg.UIN,
                                        name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                        age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                        gender = rg.Gender,
                                        phone = rg.Phone,
                                        address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                        Description = desc,
                                        Revamt = PDD.TotalProductValue,
                                    }).ToList();

                if (listAmtvalue.Count() != 0)
                {
                    foreach (var list in listAmtvalue)
                    {
                        var Newdata = new RevenuespecificBreakUPTOtalDetails();
                        Newdata.UIN = list.uin;
                        Newdata.Name = list.name;
                        Newdata.Age = list.age;
                        Newdata.Gender = list.gender;
                        Newdata.Phone = list.phone;
                        Newdata.Address1 = list.address;
                        Newdata.Specilaitydescription = list.Description;
                        Newdata.Specilatyamount = list.Revamt;

                        if (list.uin != null)
                        {
                            revenuefortheday.Add(Newdata);
                        }

                    }
                }


                RQD.RevenuespecificBreakUPTOtalDetailsss = revenuefortheday;
            }
            else if (frommonth == "Apr- May - Jun")
            {
                string mnthname = "April";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == OLMID
                           && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                    join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                    join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                    select new
                                    {
                                        uin = rg.UIN,
                                        name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                        age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                        gender = rg.Gender,
                                        phone = rg.Phone,
                                        address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                        Description = desc,
                                        Revamt = PDD.TotalProductValue,
                                    }).ToList();

                if (listAmtvalue.Count() != 0)
                {
                    foreach (var list in listAmtvalue)
                    {
                        var Newdata = new RevenuespecificBreakUPTOtalDetails();
                        Newdata.UIN = list.uin;
                        Newdata.Name = list.name;
                        Newdata.Age = list.age;
                        Newdata.Gender = list.gender;
                        Newdata.Phone = list.phone;
                        Newdata.Address1 = list.address;
                        Newdata.Specilaitydescription = list.Description;
                        Newdata.Specilatyamount = list.Revamt;

                        if (list.uin != null)
                        {
                            revenuefortheday.Add(Newdata);
                        }

                    }
                }


                RQD.RevenuespecificBreakUPTOtalDetailsss = revenuefortheday;
            }
            else if (frommonth == "May - Jun - Jul")
            {
                string mnthname = "May";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == OLMID
                           && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                    join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                    join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                    select new
                                    {
                                        uin = rg.UIN,
                                        name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                        age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                        gender = rg.Gender,
                                        phone = rg.Phone,
                                        address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                        Description = desc,
                                        Revamt = PDD.TotalProductValue,
                                    }).ToList();

                if (listAmtvalue.Count() != 0)
                {
                    foreach (var list in listAmtvalue)
                    {
                        var Newdata = new RevenuespecificBreakUPTOtalDetails();
                        Newdata.UIN = list.uin;
                        Newdata.Name = list.name;
                        Newdata.Age = list.age;
                        Newdata.Gender = list.gender;
                        Newdata.Phone = list.phone;
                        Newdata.Address1 = list.address;
                        Newdata.Specilaitydescription = list.Description;
                        Newdata.Specilatyamount = list.Revamt;

                        if (list.uin != null)
                        {
                            revenuefortheday.Add(Newdata);
                        }

                    }
                }


                RQD.RevenuespecificBreakUPTOtalDetailsss = revenuefortheday;
            }
            else if (frommonth == "Jun - Jul - Aug")
            {
                string mnthname = "June";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == OLMID
                           && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                    join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                    join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                    select new
                                    {
                                        uin = rg.UIN,
                                        name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                        age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                        gender = rg.Gender,
                                        phone = rg.Phone,
                                        address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                        Description = desc,
                                        Revamt = PDD.TotalProductValue,
                                    }).ToList();

                if (listAmtvalue.Count() != 0)
                {
                    foreach (var list in listAmtvalue)
                    {
                        var Newdata = new RevenuespecificBreakUPTOtalDetails();
                        Newdata.UIN = list.uin;
                        Newdata.Name = list.name;
                        Newdata.Age = list.age;
                        Newdata.Gender = list.gender;
                        Newdata.Phone = list.phone;
                        Newdata.Address1 = list.address;
                        Newdata.Specilaitydescription = list.Description;
                        Newdata.Specilatyamount = list.Revamt;

                        if (list.uin != null)
                        {
                            revenuefortheday.Add(Newdata);
                        }

                    }
                }


                RQD.RevenuespecificBreakUPTOtalDetailsss = revenuefortheday;
            }
            else if (frommonth == "Jul - Aug - Sep")
            {
                string mnthname = "July";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == OLMID
                           && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                    join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                    join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                    select new
                                    {
                                        uin = rg.UIN,
                                        name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                        age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                        gender = rg.Gender,
                                        phone = rg.Phone,
                                        address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                        Description = desc,
                                        Revamt = PDD.TotalProductValue,
                                    }).ToList();

                if (listAmtvalue.Count() != 0)
                {
                    foreach (var list in listAmtvalue)
                    {
                        var Newdata = new RevenuespecificBreakUPTOtalDetails();
                        Newdata.UIN = list.uin;
                        Newdata.Name = list.name;
                        Newdata.Age = list.age;
                        Newdata.Gender = list.gender;
                        Newdata.Phone = list.phone;
                        Newdata.Address1 = list.address;
                        Newdata.Specilaitydescription = list.Description;
                        Newdata.Specilatyamount = list.Revamt;

                        if (list.uin != null)
                        {
                            revenuefortheday.Add(Newdata);
                        }

                    }
                }


                RQD.RevenuespecificBreakUPTOtalDetailsss = revenuefortheday;
            }
            else if (frommonth == "Aug - Sep - Oct")
            {
                string mnthname = "August";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == OLMID
                           && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                    join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                    join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                    select new
                                    {
                                        uin = rg.UIN,
                                        name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                        age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                        gender = rg.Gender,
                                        phone = rg.Phone,
                                        address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                        Description = desc,
                                        Revamt = PDD.TotalProductValue,
                                    }).ToList();

                if (listAmtvalue.Count() != 0)
                {
                    foreach (var list in listAmtvalue)
                    {
                        var Newdata = new RevenuespecificBreakUPTOtalDetails();
                        Newdata.UIN = list.uin;
                        Newdata.Name = list.name;
                        Newdata.Age = list.age;
                        Newdata.Gender = list.gender;
                        Newdata.Phone = list.phone;
                        Newdata.Address1 = list.address;
                        Newdata.Specilaitydescription = list.Description;
                        Newdata.Specilatyamount = list.Revamt;

                        if (list.uin != null)
                        {
                            revenuefortheday.Add(Newdata);
                        }

                    }
                }


                RQD.RevenuespecificBreakUPTOtalDetailsss = revenuefortheday;
            }
            else if (frommonth == "Sep - Oct - Nov")
            {
                string mnthname = "September";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == OLMID
                           && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                    join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                    join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                    select new
                                    {
                                        uin = rg.UIN,
                                        name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                        age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                        gender = rg.Gender,
                                        phone = rg.Phone,
                                        address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                        Description = desc,
                                        Revamt = PDD.TotalProductValue,
                                    }).ToList();

                if (listAmtvalue.Count() != 0)
                {
                    foreach (var list in listAmtvalue)
                    {
                        var Newdata = new RevenuespecificBreakUPTOtalDetails();
                        Newdata.UIN = list.uin;
                        Newdata.Name = list.name;
                        Newdata.Age = list.age;
                        Newdata.Gender = list.gender;
                        Newdata.Phone = list.phone;
                        Newdata.Address1 = list.address;
                        Newdata.Specilaitydescription = list.Description;
                        Newdata.Specilatyamount = list.Revamt;

                        if (list.uin != null)
                        {
                            revenuefortheday.Add(Newdata);
                        }

                    }
                }


                RQD.RevenuespecificBreakUPTOtalDetailsss = revenuefortheday;
            }
            else if (frommonth == "Oct - Nov - Dec")
            {
                string mnthname = "October";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == OLMID
                           && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                    join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                    join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                    select new
                                    {
                                        uin = rg.UIN,
                                        name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                        age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                        gender = rg.Gender,
                                        phone = rg.Phone,
                                        address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                        Description = desc,
                                        Revamt = PDD.TotalProductValue,
                                    }).ToList();

                if (listAmtvalue.Count() != 0)
                {
                    foreach (var list in listAmtvalue)
                    {
                        var Newdata = new RevenuespecificBreakUPTOtalDetails();
                        Newdata.UIN = list.uin;
                        Newdata.Name = list.name;
                        Newdata.Age = list.age;
                        Newdata.Gender = list.gender;
                        Newdata.Phone = list.phone;
                        Newdata.Address1 = list.address;
                        Newdata.Specilaitydescription = list.Description;
                        Newdata.Specilatyamount = list.Revamt;

                        if (list.uin != null)
                        {
                            revenuefortheday.Add(Newdata);
                        }

                    }
                }


                RQD.RevenuespecificBreakUPTOtalDetailsss = revenuefortheday;
            }
            else if (frommonth == "Nov - Dec - Jan")
            {
                string mnthname = "November";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == OLMID
                           && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                    join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                    join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                    select new
                                    {
                                        uin = rg.UIN,
                                        name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                        age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                        gender = rg.Gender,
                                        phone = rg.Phone,
                                        address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                        Description = desc,
                                        Revamt = PDD.TotalProductValue,
                                    }).ToList();

                if (listAmtvalue.Count() != 0)
                {
                    foreach (var list in listAmtvalue)
                    {
                        var Newdata = new RevenuespecificBreakUPTOtalDetails();
                        Newdata.UIN = list.uin;
                        Newdata.Name = list.name;
                        Newdata.Age = list.age;
                        Newdata.Gender = list.gender;
                        Newdata.Phone = list.phone;
                        Newdata.Address1 = list.address;
                        Newdata.Specilaitydescription = list.Description;
                        Newdata.Specilatyamount = list.Revamt;

                        if (list.uin != null)
                        {
                            revenuefortheday.Add(Newdata);
                        }

                    }
                }


                RQD.RevenuespecificBreakUPTOtalDetailsss = revenuefortheday;
            }
            else if (frommonth == "Dec - Jan - Feb")
            {
                string mnthname = "December";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == OLMID
                           && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(selectedyear))
                                    join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                    join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                    select new
                                    {
                                        uin = rg.UIN,
                                        name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                        age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                        gender = rg.Gender,
                                        phone = rg.Phone,
                                        address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                        Description = desc,
                                        Revamt = PDD.TotalProductValue,
                                    }).ToList();

                if (listAmtvalue.Count() != 0)
                {
                    foreach (var list in listAmtvalue)
                    {
                        var Newdata = new RevenuespecificBreakUPTOtalDetails();
                        Newdata.UIN = list.uin;
                        Newdata.Name = list.name;
                        Newdata.Age = list.age;
                        Newdata.Gender = list.gender;
                        Newdata.Phone = list.phone;
                        Newdata.Address1 = list.address;
                        Newdata.Specilaitydescription = list.Description;
                        Newdata.Specilatyamount = list.Revamt;

                        if (list.uin != null)
                        {
                            revenuefortheday.Add(Newdata);
                        }

                    }
                }


                RQD.RevenuespecificBreakUPTOtalDetailsss = revenuefortheday;
            }
            //to revenuee aurterly


            if (Tomonth == "Jan - Feb - Mar")
            {
                string mnthname = "January";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == OLMID
                           && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                    join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                    join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                    select new
                                    {
                                        uin = rg.UIN,
                                        name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                        age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                        gender = rg.Gender,
                                        phone = rg.Phone,
                                        address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                        Description = desc,
                                        Revamt = PDD.TotalProductValue,
                                    }).ToList();

                if (listAmtvalue.Count() != 0)
                {
                    foreach (var list in listAmtvalue)
                    {
                        var Newdata = new toRevenuespecificBreakUPTOtalDetails();
                        Newdata.UIN = list.uin;
                        Newdata.Name = list.name;
                        Newdata.Age = list.age;
                        Newdata.Gender = list.gender;
                        Newdata.Phone = list.phone;
                        Newdata.Address1 = list.address;
                        Newdata.Specilaitydescription = list.Description;
                        Newdata.Specilatyamount = list.Revamt;

                        if (list.uin != null)
                        {
                            RevenuefortheMonth.Add(Newdata);
                        }

                    }
                }


                RQD.toRevenuespecificBreakUPTOtalDetailss = RevenuefortheMonth;
            }
            else if (Tomonth == "Feb - Mar - Apr")
            {
                string mnthname = "February";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == OLMID
                           && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                    join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                    join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                    select new
                                    {
                                        uin = rg.UIN,
                                        name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                        age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                        gender = rg.Gender,
                                        phone = rg.Phone,
                                        address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                        Description = desc,
                                        Revamt = PDD.TotalProductValue,
                                    }).ToList();

                if (listAmtvalue.Count() != 0)
                {
                    foreach (var list in listAmtvalue)
                    {
                        var Newdata = new toRevenuespecificBreakUPTOtalDetails();
                        Newdata.UIN = list.uin;
                        Newdata.Name = list.name;
                        Newdata.Age = list.age;
                        Newdata.Gender = list.gender;
                        Newdata.Phone = list.phone;
                        Newdata.Address1 = list.address;
                        Newdata.Specilaitydescription = list.Description;
                        Newdata.Specilatyamount = list.Revamt;

                        if (list.uin != null)
                        {
                            RevenuefortheMonth.Add(Newdata);
                        }

                    }
                }


                RQD.toRevenuespecificBreakUPTOtalDetailss = RevenuefortheMonth;
            }
            else if (Tomonth == "Mar - Apr - May")
            {
                string mnthname = "March";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == OLMID
                           && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                    join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                    join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                    select new
                                    {
                                        uin = rg.UIN,
                                        name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                        age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                        gender = rg.Gender,
                                        phone = rg.Phone,
                                        address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                        Description = desc,
                                        Revamt = PDD.TotalProductValue,
                                    }).ToList();

                if (listAmtvalue.Count() != 0)
                {
                    foreach (var list in listAmtvalue)
                    {
                        var Newdata = new toRevenuespecificBreakUPTOtalDetails();
                        Newdata.UIN = list.uin;
                        Newdata.Name = list.name;
                        Newdata.Age = list.age;
                        Newdata.Gender = list.gender;
                        Newdata.Phone = list.phone;
                        Newdata.Address1 = list.address;
                        Newdata.Specilaitydescription = list.Description;
                        Newdata.Specilatyamount = list.Revamt;

                        if (list.uin != null)
                        {
                            RevenuefortheMonth.Add(Newdata);
                        }

                    }
                }


                RQD.toRevenuespecificBreakUPTOtalDetailss = RevenuefortheMonth;
            }
            else if (Tomonth == "Apr- May - Jun")
            {
                string mnthname = "April";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == OLMID
                           && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                    join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                    join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                    select new
                                    {
                                        uin = rg.UIN,
                                        name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                        age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                        gender = rg.Gender,
                                        phone = rg.Phone,
                                        address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                        Description = desc,
                                        Revamt = PDD.TotalProductValue,
                                    }).ToList();

                if (listAmtvalue.Count() != 0)
                {
                    foreach (var list in listAmtvalue)
                    {
                        var Newdata = new toRevenuespecificBreakUPTOtalDetails();
                        Newdata.UIN = list.uin;
                        Newdata.Name = list.name;
                        Newdata.Age = list.age;
                        Newdata.Gender = list.gender;
                        Newdata.Phone = list.phone;
                        Newdata.Address1 = list.address;
                        Newdata.Specilaitydescription = list.Description;
                        Newdata.Specilatyamount = list.Revamt;

                        if (list.uin != null)
                        {
                            RevenuefortheMonth.Add(Newdata);
                        }

                    }
                }


                RQD.toRevenuespecificBreakUPTOtalDetailss = RevenuefortheMonth;
            }
            else if (Tomonth == "May - Jun - Jul")
            {
                string mnthname = "May";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == OLMID
                           && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                    join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                    join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                    select new
                                    {
                                        uin = rg.UIN,
                                        name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                        age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                        gender = rg.Gender,
                                        phone = rg.Phone,
                                        address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                        Description = desc,
                                        Revamt = PDD.TotalProductValue,
                                    }).ToList();

                if (listAmtvalue.Count() != 0)
                {
                    foreach (var list in listAmtvalue)
                    {
                        var Newdata = new toRevenuespecificBreakUPTOtalDetails();
                        Newdata.UIN = list.uin;
                        Newdata.Name = list.name;
                        Newdata.Age = list.age;
                        Newdata.Gender = list.gender;
                        Newdata.Phone = list.phone;
                        Newdata.Address1 = list.address;
                        Newdata.Specilaitydescription = list.Description;
                        Newdata.Specilatyamount = list.Revamt;

                        if (list.uin != null)
                        {
                            RevenuefortheMonth.Add(Newdata);
                        }

                    }
                }


                RQD.toRevenuespecificBreakUPTOtalDetailss = RevenuefortheMonth;
            }
            else if (Tomonth == "Jun - Jul - Aug")
            {
                string mnthname = "June";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == OLMID
                           && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                    join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                    join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                    select new
                                    {
                                        uin = rg.UIN,
                                        name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                        age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                        gender = rg.Gender,
                                        phone = rg.Phone,
                                        address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                        Description = desc,
                                        Revamt = PDD.TotalProductValue,
                                    }).ToList();

                if (listAmtvalue.Count() != 0)
                {
                    foreach (var list in listAmtvalue)
                    {
                        var Newdata = new toRevenuespecificBreakUPTOtalDetails();
                        Newdata.UIN = list.uin;
                        Newdata.Name = list.name;
                        Newdata.Age = list.age;
                        Newdata.Gender = list.gender;
                        Newdata.Phone = list.phone;
                        Newdata.Address1 = list.address;
                        Newdata.Specilaitydescription = list.Description;
                        Newdata.Specilatyamount = list.Revamt;

                        if (list.uin != null)
                        {
                            RevenuefortheMonth.Add(Newdata);
                        }

                    }
                }


                RQD.toRevenuespecificBreakUPTOtalDetailss = RevenuefortheMonth;
            }
            else if (Tomonth == "Jul - Aug - Sep")
            {
                string mnthname = "July";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == OLMID
                           && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                    join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                    join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                    select new
                                    {
                                        uin = rg.UIN,
                                        name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                        age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                        gender = rg.Gender,
                                        phone = rg.Phone,
                                        address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                        Description = desc,
                                        Revamt = PDD.TotalProductValue,
                                    }).ToList();

                if (listAmtvalue.Count() != 0)
                {
                    foreach (var list in listAmtvalue)
                    {
                        var Newdata = new toRevenuespecificBreakUPTOtalDetails();
                        Newdata.UIN = list.uin;
                        Newdata.Name = list.name;
                        Newdata.Age = list.age;
                        Newdata.Gender = list.gender;
                        Newdata.Phone = list.phone;
                        Newdata.Address1 = list.address;
                        Newdata.Specilaitydescription = list.Description;
                        Newdata.Specilatyamount = list.Revamt;
                        if (list.uin != null)
                        {
                            RevenuefortheMonth.Add(Newdata);
                        }

                    }
                }


                RQD.toRevenuespecificBreakUPTOtalDetailss = RevenuefortheMonth;
            }
            else if (Tomonth == "Aug - Sep - Oct")
            {
                string mnthname = "August";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == OLMID
                           && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                    join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                    join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                    select new
                                    {
                                        uin = rg.UIN,
                                        name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                        age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                        gender = rg.Gender,
                                        phone = rg.Phone,
                                        address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                        Description = desc,
                                        Revamt = PDD.TotalProductValue,
                                    }).ToList();

                if (listAmtvalue.Count() != 0)
                {
                    foreach (var list in listAmtvalue)
                    {
                        var Newdata = new toRevenuespecificBreakUPTOtalDetails();
                        Newdata.UIN = list.uin;
                        Newdata.Name = list.name;
                        Newdata.Age = list.age;
                        Newdata.Gender = list.gender;
                        Newdata.Phone = list.phone;
                        Newdata.Address1 = list.address;
                        Newdata.Specilaitydescription = list.Description;
                        Newdata.Specilatyamount = list.Revamt;

                        if (list.uin != null)
                        {
                            RevenuefortheMonth.Add(Newdata);
                        }

                    }
                }


                RQD.toRevenuespecificBreakUPTOtalDetailss = RevenuefortheMonth;
            }
            else if (Tomonth == "Sep - Oct - Nov")
            {
                string mnthname = "September";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == OLMID
                           && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                    join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                    join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                    select new
                                    {
                                        uin = rg.UIN,
                                        name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                        age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                        gender = rg.Gender,
                                        phone = rg.Phone,
                                        address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                        Description = desc,
                                        Revamt = PDD.TotalProductValue,
                                    }).ToList();

                if (listAmtvalue.Count() != 0)
                {
                    foreach (var list in listAmtvalue)
                    {
                        var Newdata = new toRevenuespecificBreakUPTOtalDetails();
                        Newdata.UIN = list.uin;
                        Newdata.Name = list.name;
                        Newdata.Age = list.age;
                        Newdata.Gender = list.gender;
                        Newdata.Phone = list.phone;
                        Newdata.Address1 = list.address;
                        Newdata.Specilaitydescription = list.Description;
                        Newdata.Specilatyamount = list.Revamt;
                        if (list.uin != null)
                        {
                            RevenuefortheMonth.Add(Newdata);
                        }

                    }
                }


                RQD.toRevenuespecificBreakUPTOtalDetailss = RevenuefortheMonth;
            }
            else if (Tomonth == "Oct - Nov - Dec")
            {
                string mnthname = "October";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == OLMID
                           && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                    join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                    join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                    select new
                                    {
                                        uin = rg.UIN,
                                        name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                        age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                        gender = rg.Gender,
                                        phone = rg.Phone,
                                        address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                        Description = desc,
                                        Revamt = PDD.TotalProductValue,
                                    }).ToList();

                if (listAmtvalue.Count() != 0)
                {
                    foreach (var list in listAmtvalue)
                    {
                        var Newdata = new toRevenuespecificBreakUPTOtalDetails();
                        Newdata.UIN = list.uin;
                        Newdata.Name = list.name;
                        Newdata.Age = list.age;
                        Newdata.Gender = list.gender;
                        Newdata.Phone = list.phone;
                        Newdata.Address1 = list.address;
                        Newdata.Specilaitydescription = list.Description;
                        Newdata.Specilatyamount = list.Revamt;

                        if (list.uin != null)
                        {
                            RevenuefortheMonth.Add(Newdata);
                        }

                    }
                }


                RQD.toRevenuespecificBreakUPTOtalDetailss = RevenuefortheMonth;
            }
            else if (Tomonth == "Nov - Dec - Jan")
            {
                string mnthname = "November";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == OLMID
                           && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                    join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                    join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                    select new
                                    {
                                        uin = rg.UIN,
                                        name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                        age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                        gender = rg.Gender,
                                        phone = rg.Phone,
                                        address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                        Description = desc,
                                        Revamt = PDD.TotalProductValue,
                                    }).ToList();

                if (listAmtvalue.Count() != 0)
                {
                    foreach (var list in listAmtvalue)
                    {
                        var Newdata = new toRevenuespecificBreakUPTOtalDetails();
                        Newdata.UIN = list.uin;
                        Newdata.Name = list.name;
                        Newdata.Age = list.age;
                        Newdata.Gender = list.gender;
                        Newdata.Phone = list.phone;
                        Newdata.Address1 = list.address;
                        Newdata.Specilaitydescription = list.Description;
                        Newdata.Specilatyamount = list.Revamt;

                        if (list.uin != null)
                        {
                            RevenuefortheMonth.Add(Newdata);
                        }

                    }
                }


                RQD.toRevenuespecificBreakUPTOtalDetailss = RevenuefortheMonth;
            }
            else if (Tomonth == "Dec - Jan - Feb")
            {
                string mnthname = "December";
                int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                var TotalDays = j + 2;

                var listAmtvalue = (from PDD in PAdetail.Where(x => x.OLMID == OLMID
                           && x.CreatedUTC.Month >= j && x.CreatedUTC.Month <= TotalDays && x.CreatedUTC.Year == Convert.ToInt32(Toyear))
                                    join Pa in Patientaccount on PDD.PAID equals Pa.PAID
                                    join rg in WYNKContext.Registration on Pa.UIN equals rg.UIN
                                    select new
                                    {
                                        uin = rg.UIN,
                                        name = rg.Name + ' ' + rg.MiddleName + ' ' + rg.LastName,
                                        age = PasswordEncodeandDecode.ToAgeString(rg.DateofBirth),
                                        gender = rg.Gender,
                                        phone = rg.Phone,
                                        address = rg.Address1 + ' ' + rg.Address2 + ' ' + rg.Address3,
                                        Description = desc,
                                        Revamt = PDD.TotalProductValue,
                                    }).ToList();

                if (listAmtvalue.Count() != 0)
                {
                    foreach (var list in listAmtvalue)
                    {
                        var Newdata = new toRevenuespecificBreakUPTOtalDetails();
                        Newdata.UIN = list.uin;
                        Newdata.Name = list.name;
                        Newdata.Age = list.age;
                        Newdata.Gender = list.gender;
                        Newdata.Phone = list.phone;
                        Newdata.Address1 = list.address;
                        Newdata.Specilaitydescription = list.Description;
                        Newdata.Specilatyamount = list.Revamt;

                        if (list.uin != null)
                        {
                            RevenuefortheMonth.Add(Newdata);
                        }

                    }
                }


                RQD.toRevenuespecificBreakUPTOtalDetailss = RevenuefortheMonth;
            }


            RQD.RevenueFromdate = frommonth + '-' + selectedyear;
            RQD.RevenueTodate = Tomonth + '-' + Toyear;


            return RQD;
        }


        public dynamic GetCountRevenueMonthFulldetails(string Monthstring, string Surgdescription, int CompanyID, string format)
        {
            var RD = new ManagementDashboardViewModel();
            if (format == "MONTH")
            {
                var monthyear = DateTime.Now.Date.ToString("yyyy-MM");
                var dm = CMPSContext.DoctorMaster.Where(x => x.CMPID == CompanyID).ToList();
                var rsdetails = WYNKContext.RevenueSummary.Where(x => x.ParentID == Convert.ToInt32(Monthstring) && x.CmpID == CompanyID && x.CreatedUTC.Date.ToString("yyyy-MM") == monthyear).ToList();
                RD.Revenuebreakupdetails = (from cm in rsdetails
                                            select new Revenuebreakupdetails
                                            {
                                                Description = cm.SpecialityDesc,
                                                Doctorname = dm.Where(x => x.DoctorID == cm.DoctorID).Select(x => x.Firstname + " " + x.LastName).FirstOrDefault(),
                                                Amount = cm.Amount,
                                                Date = cm.CreatedUTC,
                                            }).ToList();
            }
            else
            {
                var monthyear = DateTime.Now.Date.ToString("yyyy-MM-dd");
                var dm = CMPSContext.DoctorMaster.Where(x => x.CMPID == CompanyID).ToList();
                var rsdetails = WYNKContext.RevenueSummary.Where(x => x.ParentID == Convert.ToInt32(Monthstring) && x.CmpID == CompanyID && x.CreatedUTC.Date.ToString("yyyy-MM-dd") == monthyear).ToList();
                RD.Revenuebreakupdetails = (from cm in rsdetails
                                            select new Revenuebreakupdetails
                                            {
                                                Description = cm.SpecialityDesc,
                                                Doctorname = dm.Where(x => x.DoctorID == cm.DoctorID).Select(x => x.Firstname + " " + x.LastName).FirstOrDefault(),
                                                Amount = cm.Amount,
                                                Date = cm.CreatedUTC,
                                            }).ToList();
            }

            return RD.Revenuebreakupdetails;
        }
        public ManagementDashboardViewModel GetRevenuedetails(string Yearstring, string Monthstring, string Daystring, int CompanyID)
        {
            var RD = new ManagementDashboardViewModel();
            DateTime fulldate = Convert.ToDateTime(Yearstring + '/' + Monthstring + '/' + Daystring);
            var revenuefortheday = new List<Revenuefortheday>();
            var RevenuefortheMonth = new List<Revenuefortheday>();
            var Fullmonth = fulldate.ToString("yyyy-MM");
            var FFFDate = fulldate.Date.ToString("yyyy-MM-dd");


            var rs = WYNKContext.RevenueSummary.Where(x => x.CmpID == CompanyID && x.ServiceID != null && x.ParentID != null).ToList();
            var services = WYNKContext.Services.Where(x => x.CMPID == CompanyID).ToList();

            var Dayrevenueamount = (from rss in rs.Where(x => x.CreatedUTC.Date.ToString("yyyy-MM-dd") == FFFDate)
                                    group rss by new { rss.ParentID, rss.ServiceID } into g
                                    select new
                                    {
                                        Revamt = g.FirstOrDefault().Amount,
                                    }).ToList();

            var Monthrevenueamount = (from rss in rs.Where(x => x.CreatedUTC.Date.ToString("yyyy-MM") == Fullmonth)
                                      group rss by new { rss.ParentID, rss.ServiceID } into g
                                      select new
                                      {
                                          Revamt = g.FirstOrDefault().Amount,
                                      }).ToList();

            var TotalInvestigationSumForday = Dayrevenueamount.Sum(x => x.Revamt);
            var MonthTotalInvestigationSumForday = Monthrevenueamount.Sum(x => x.Revamt);
            var revenuesummary = rs.GroupBy(x => x.ParentID).Select(x => x.FirstOrDefault().ParentID).ToList();
            foreach (var Item in revenuesummary)
            {
                var RAMT = new RevenueAMT();
                var pid = (from gm in rs.Where(x => x.ParentID == Item)
                           group gm by new { gm.ParentID } into hg
                           select new
                           {
                               ParentID = hg.FirstOrDefault().ParentID,
                           }).ToList();
                foreach (var iitem in pid)
                {
                    var Newdata = new Revenuefortheday();
                    Newdata.Revenuedesc = WYNKContext.Services.Where(x => x.ID == iitem.ParentID).Select(x => x.Description).FirstOrDefault();
                    Newdata.RevenueAmount = rs.Where(x => x.ParentID == iitem.ParentID && x.CreatedUTC.Date.ToString("yyyy-MM-dd") == FFFDate).Sum(x => x.Amount);
                    Newdata.RevenueMonthAmount = rs.Where(x => x.ParentID == iitem.ParentID && x.CreatedUTC.Date.ToString("yyyy-MM") == Fullmonth).Sum(x => x.Amount);
                    Newdata.serviceid = iitem.ParentID;
                    revenuefortheday.Add(Newdata);
                }
            }
            var RRD = revenuefortheday;
            var ser = services.Where(x => !revenuefortheday.Any(y => y.serviceid == x.ID) && x.ParentID == 0).ToList();
            foreach (var iitems in ser)
            {
                var Newdata = new Revenuefortheday();
                Newdata.Revenuedesc = iitems.Description;
                Newdata.RevenueAmount = 0;
                Newdata.RevenueMonthAmount = 0;
                Newdata.serviceid = iitems.ID;
                RevenuefortheMonth.Add(Newdata);
            }
            var rrm = RevenuefortheMonth;
            var totalmethods = RRD.Concat(rrm);
            RD.TOTALMERGEDRevenue = (from r in totalmethods
                                     select new TOTALMERGEDRevenue
                                     {
                                         NewRevenueAmount = r.RevenueAmount,
                                         OLDRevenueAmount = r.RevenueMonthAmount,
                                         Revenuedesc = r.Revenuedesc,
                                         serviceid = r.serviceid,
                                     }).ToList();
            RD.TotalrevenuesumFortheDay = RD.TOTALMERGEDRevenue.Sum(x => x.NewRevenueAmount);
            RD.TotalrevenuesumFoirtheMonth = RD.TOTALMERGEDRevenue.Sum(x => x.OLDRevenueAmount);
            return RD;
        }
        public ManagementDashboardViewModel GetSpecificperiodRevenuedetails(string FromDate, string ToDate, int CompanyID)
        {
            var RD = new ManagementDashboardViewModel();

            DateTime FT;
            var appdate = DateTime.TryParseExact(FromDate.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out FT);
            {
                FT.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            }

            DateTime TT;
            var apptdate = DateTime.TryParseExact(ToDate.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out TT);
            {
                TT.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            }
            var RevenuefortheMonth = new List<Revenuefortheday>();
            var revenuefortheday = new List<Revenuefortheday>();
            var FROMDATE = FT;
            var TODATE = TT;
            var rs = WYNKContext.RevenueSummary.Where(x => x.CmpID == CompanyID && x.ServiceID != null && x.ParentID != null).ToList();
            var services = WYNKContext.Services.Where(x => x.CMPID == CompanyID).ToList();
            var revenuesummary = rs.GroupBy(x => x.ParentID).Select(x => x.FirstOrDefault().ParentID).ToList();
            foreach (var Item in revenuesummary)
            {
                var RAMT = new RevenueAMT();
                var pid = (from gm in rs.Where(x => x.ParentID == Item)
                           group gm by new { gm.ParentID } into hg
                           select new
                           {
                               ParentID = hg.FirstOrDefault().ParentID,
                           }).ToList();
                foreach (var iitem in pid)
                {
                    var Newdata = new Revenuefortheday();
                    Newdata.Revenuedesc = WYNKContext.Services.Where(x => x.ID == iitem.ParentID).Select(x => x.Description).FirstOrDefault();
                    Newdata.RevenueAmount = rs.Where(x => x.ParentID == iitem.ParentID && (x.CreatedUTC.Date >= FROMDATE.Date && x.CreatedUTC.Date <= TODATE.Date)).Sum(x => x.Amount);
                    Newdata.serviceid = iitem.ParentID;
                    Newdata.RevenueMonthAmount = rs.Where(x => x.ParentID == iitem.ParentID && (x.CreatedUTC.Date >= FROMDATE.Date && x.CreatedUTC.Date <= TODATE.Date)).Sum(x => x.Numbers);
                    revenuefortheday.Add(Newdata);
                }
            }

            var RRD = revenuefortheday;
            var ser = services.Where(x => !revenuefortheday.Any(y => y.serviceid == x.ID) && x.ParentID == 0).ToList();

            foreach (var iitems in ser)
            {
                var Newdata = new Revenuefortheday();
                Newdata.Revenuedesc = iitems.Description;
                Newdata.RevenueAmount = 0;
                Newdata.RevenueMonthAmount = 0;
                Newdata.serviceid = iitems.ID;
                RevenuefortheMonth.Add(Newdata);
            }
            var rrm = RevenuefortheMonth;
            var totalmethods = RRD.Concat(rrm);
            RD.revenuefortheday = (from r in totalmethods
                                     select new Revenuefortheday
                                     {
                                         RevenueAmount = r.RevenueAmount,
                                         RevenueMonthAmount = r.RevenueMonthAmount,
                                         Revenuedesc = r.Revenuedesc,
                                         serviceid = r.serviceid,
                                     }).ToList();
            RD.revenuefortheday = RD.revenuefortheday;
            RD.Totalrevenuesum = RD.revenuefortheday.Sum(x => x.RevenueAmount);
            RD.RFromdate = FROMDATE;
            RD.RTodate = TODATE;
            return RD;
        }
        public ManagementDashboardViewModel GetMonthREVENUEdetails(string FromDate, string ToDate, int CompanyID)
        {
            var RD = new ManagementDashboardViewModel();

            DateTime FT;
            var appdate = DateTime.TryParseExact(FromDate.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out FT);
            {
                FT.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            }

            DateTime TT;
            var apptdate = DateTime.TryParseExact(ToDate.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out TT);
            {
                TT.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            }
            var RevenuefortheMonth = new List<Revenuefortheday>();
            var revenuefortheday = new List<Revenuefortheday>();
            var revenueforthemonth = new List<Revenuefortheday>();
            var FROMDATE = FT;
            var TODATE = TT;
            var rs = WYNKContext.RevenueSummary.Where(x => x.CmpID == CompanyID && x.ServiceID != null && x.ParentID != null).ToList();
            var services = WYNKContext.Services.Where(x => x.CMPID == CompanyID).ToList();
            var revenuesummary = rs.GroupBy(x => x.ParentID).Select(x => x.FirstOrDefault().ParentID).ToList();
            foreach (var Item in revenuesummary)
            {
                var RAMT = new RevenueAMT();
                var pid = (from gm in rs.Where(x => x.ParentID == Item)
                           group gm by new { gm.ParentID } into hg
                           select new
                           {
                               ParentID = hg.FirstOrDefault().ParentID,
                           }).ToList();
                foreach (var iitem in pid)
                {
                    var Newdata = new Revenuefortheday();
                    Newdata.Revenuedesc = WYNKContext.Services.Where(x => x.ID == iitem.ParentID).Select(x => x.Description).FirstOrDefault();
                    Newdata.RevenueAmount = rs.Where(x => x.ParentID == iitem.ParentID && x.CreatedUTC.Date.ToString("yyyy-MM") == FROMDATE.Date.ToString("yyyy-MM")).Sum(x => x.Amount);
                    Newdata.serviceid = iitem.ParentID;
                    Newdata.RevenueMonthAmount = rs.Where(x => x.ParentID == iitem.ParentID && x.CreatedUTC.Date.ToString("yyyy-MM") == TODATE.Date.ToString("yyyy-MM")).Sum(x => x.Amount);
                    revenuefortheday.Add(Newdata);
                }
            }

            var RRD = revenuefortheday;            
            var ser = services.Where(x => !revenuefortheday.Any(y => y.serviceid == x.ID) && x.ParentID == 0).ToList();
            foreach (var iitems in ser)
            {
                var Newdata = new Revenuefortheday();
                Newdata.Revenuedesc = iitems.Description;
                Newdata.RevenueAmount = 0;
                Newdata.RevenueMonthAmount = 0;
                Newdata.serviceid = iitems.ID;
                RevenuefortheMonth.Add(Newdata);
            }
            var rmd = RevenuefortheMonth;
            var totalmethods = RRD.Concat(rmd);
            RD.TOTALMERGEDRevenue = (from r in totalmethods
                                     select new TOTALMERGEDRevenue
                                      {
                                          NewRevenueAmount = r.RevenueAmount,
                                          OLDRevenueAmount = r.RevenueMonthAmount,
                                          Revenuedesc = r.Revenuedesc,
                                      }).ToList();

            RD.TotalrevenuesumFortheDay = RD.TOTALMERGEDRevenue.Sum(x => x.NewRevenueAmount);
            RD.TotalrevenuesumFoirtheMonth = RD.TOTALMERGEDRevenue.Sum(x => x.OLDRevenueAmount);
            RD.RFromdate = FROMDATE.Date;
            RD.RTodate = TODATE.Date;

            return RD;
        }

        public ManagementDashboardViewModel GetQuarterlycomparisionRevenuedetails(string selecetdmonth, string selectedyear,
      string selectedtomonth, string selectedtoyear, int CompanyID)
        {
            var RD = new ManagementDashboardViewModel();
            string mnthname = selecetdmonth;
            int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
            string tomnthname = selectedtomonth;
            int t = DateTime.ParseExact(tomnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
            var FromDate = Convert.ToDateTime("01-" + j +'-'+ selectedyear).ToString("dd-MMM-yyyy");
            var ToDate = Convert.ToDateTime("01-" + t +'-'+ selectedtoyear).ToString("dd-MMM-yyyy");
            DateTime FT;
            var appdate = DateTime.TryParseExact(FromDate.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out FT);
            {
                FT.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            }
            DateTime TT;
            var apptdate = DateTime.TryParseExact(ToDate.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out TT);
            {
                TT.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            }
            var RevenuefortheMonth = new List<Revenuefortheday>();
            var revenuefortheday = new List<Revenuefortheday>();
            var revenueforthemonth = new List<Revenuefortheday>();
            var FROMDATEs = Convert.ToDateTime(FromDate);
            var fthree = FROMDATEs.AddMonths(3);
            var TODATEs = Convert.ToDateTime(ToDate);
            var thtree = TT.AddMonths(3);
            var rs = WYNKContext.RevenueSummary.Where(x => x.CmpID == CompanyID && x.ServiceID != null && x.ParentID != null).ToList();
            var services = WYNKContext.Services.Where(x => x.CMPID == CompanyID).ToList();
            var revenuesummary = rs.GroupBy(x => x.ParentID).Select(x => x.FirstOrDefault().ParentID).ToList();
            foreach (var Item in revenuesummary)
            {
                var RAMT = new RevenueAMT();
                var pid = (from gm in rs.Where(x => x.ParentID == Item)
                           group gm by new { gm.ParentID } into hg
                           select new
                           {
                               ParentID = hg.FirstOrDefault().ParentID,
                           }).ToList();
                foreach (var iitem in pid)
                {
                    var Newdata = new Revenuefortheday();
                    Newdata.Revenuedesc = WYNKContext.Services.Where(x => x.ID == iitem.ParentID).Select(x => x.Description).FirstOrDefault();
                    Newdata.RevenueAmount = rs.Where(x => x.ParentID == iitem.ParentID && (x.CreatedUTC.Date >= FROMDATEs.Date && x.CreatedUTC.Date <= fthree.Date)).Sum(x => x.Amount);
                    Newdata.serviceid = iitem.ParentID;
                    Newdata.RevenueMonthAmount = rs.Where(x => x.ParentID == iitem.ParentID && (x.CreatedUTC.Date >= TODATEs.Date && x.CreatedUTC.Date <= thtree.Date)).Sum(x => x.Amount);
                    revenuefortheday.Add(Newdata);
                }
            }

            var RRD = revenuefortheday;
            var ser = services.Where(x => !revenuefortheday.Any(y => y.serviceid == x.ID) && x.ParentID == 0).ToList();
            foreach (var iitems in ser)
            {
                var Newdata = new Revenuefortheday();
                Newdata.Revenuedesc = iitems.Description;
                Newdata.RevenueAmount = 0;
                Newdata.RevenueMonthAmount = 0;
                Newdata.serviceid = iitems.ID;
                RevenuefortheMonth.Add(Newdata);
            }
            var rmd = RevenuefortheMonth;
            var totalmethods = RRD.Concat(rmd);
            RD.TOTALMERGEDRevenue = (from r in totalmethods
                                     select new TOTALMERGEDRevenue
                                     {
                                         NewRevenueAmount = r.RevenueAmount,
                                         OLDRevenueAmount = r.RevenueMonthAmount,
                                         Revenuedesc = r.Revenuedesc,
                                     }).ToList();


            RD.TotalrevenuesumFortheDay = RD.TOTALMERGEDRevenue.Sum(x => x.NewRevenueAmount);
            RD.TotalrevenuesumFoirtheMonth = RD.TOTALMERGEDRevenue.Sum(x => x.OLDRevenueAmount);
            RD.RevenueAllFrom = selecetdmonth + " - " + selectedyear;
            RD.RevenueAllTo = selectedtomonth + " - " + selectedtoyear;
            return RD;
        }
        public ManagementDashboardViewModel GetHalfyaerlycomparisionRevenuedetails(string selecetdmonth, string selectedyear,
          string selectedtomonth, string selectedtoyear, int CompanyID)
        {
            var RD = new ManagementDashboardViewModel();
            string mnthname = selecetdmonth;
            int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
            string tomnthname = selectedtomonth;
            int t = DateTime.ParseExact(tomnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
            var FromDate = Convert.ToDateTime("01-" + j + '-' + selectedyear).ToString("dd-MMM-yyyy");
            var ToDate = Convert.ToDateTime("01-" + t + '-' + selectedtoyear).ToString("dd-MMM-yyyy");
            DateTime FT;
            var appdate = DateTime.TryParseExact(FromDate.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out FT);
            {
                FT.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            }
            DateTime TT;
            var apptdate = DateTime.TryParseExact(ToDate.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out TT);
            {
                TT.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            }
            var RevenuefortheMonth = new List<Revenuefortheday>();
            var revenuefortheday = new List<Revenuefortheday>();
            var revenueforthemonth = new List<Revenuefortheday>();
            var FROMDATEs = Convert.ToDateTime(FromDate);
            var fthree = FROMDATEs.AddMonths(6);
            var TODATEs = Convert.ToDateTime(ToDate);
            var thtree = TT.AddMonths(6);
            var rs = WYNKContext.RevenueSummary.Where(x => x.CmpID == CompanyID && x.ServiceID != null && x.ParentID != null).ToList();
            var services = WYNKContext.Services.Where(x => x.CMPID == CompanyID).ToList();
            var revenuesummary = rs.GroupBy(x => x.ParentID).Select(x => x.FirstOrDefault().ParentID).ToList();
            foreach (var Item in revenuesummary)
            {
                var RAMT = new RevenueAMT();
                var pid = (from gm in rs.Where(x => x.ParentID == Item)
                           group gm by new { gm.ParentID } into hg
                           select new
                           {
                               ParentID = hg.FirstOrDefault().ParentID,
                           }).ToList();
                foreach (var iitem in pid)
                {
                    var Newdata = new Revenuefortheday();
                    Newdata.Revenuedesc = WYNKContext.Services.Where(x => x.ID == iitem.ParentID).Select(x => x.Description).FirstOrDefault();
                    Newdata.RevenueAmount = rs.Where(x => x.ParentID == iitem.ParentID && (x.CreatedUTC.Date >= FROMDATEs.Date && x.CreatedUTC.Date <= fthree.Date)).Sum(x => x.Amount);
                    Newdata.serviceid = iitem.ParentID;
                    Newdata.RevenueMonthAmount = rs.Where(x => x.ParentID == iitem.ParentID && (x.CreatedUTC.Date >= TODATEs.Date && x.CreatedUTC.Date <= thtree.Date)).Sum(x => x.Amount);
                    revenuefortheday.Add(Newdata);
                }
            }

            var RRD = revenuefortheday;
            var ser = services.Where(x => !revenuefortheday.Any(y => y.serviceid == x.ID) && x.ParentID == 0).ToList();
            foreach (var iitems in ser)
            {
                var Newdata = new Revenuefortheday();
                Newdata.Revenuedesc = iitems.Description;
                Newdata.RevenueAmount = 0;
                Newdata.RevenueMonthAmount = 0;
                Newdata.serviceid = iitems.ID;
                RevenuefortheMonth.Add(Newdata);
            }
            var rmd = RevenuefortheMonth;
            var totalmethods = RRD.Concat(rmd);
            RD.TOTALMERGEDRevenue = (from r in totalmethods
                                     select new TOTALMERGEDRevenue
                                     {
                                         NewRevenueAmount = r.RevenueAmount,
                                         OLDRevenueAmount = r.RevenueMonthAmount,
                                         Revenuedesc = r.Revenuedesc,
                                     }).ToList();


            RD.TotalrevenuesumFortheDay = RD.TOTALMERGEDRevenue.Sum(x => x.NewRevenueAmount);
            RD.TotalrevenuesumFoirtheMonth = RD.TOTALMERGEDRevenue.Sum(x => x.OLDRevenueAmount);
            RD.RevenueAllFrom = selecetdmonth + " - " + selectedyear;
            RD.RevenueAllTo = selectedtomonth + " - " + selectedtoyear;
            return RD;
        }

        public ManagementDashboardViewModel GetAnnualcomparisionRevenuedetails(string selecetdmonth, string selectedyear,
     string selectedtomonth, string selectedtoyear, int CompanyID)
        {

            var RD = new ManagementDashboardViewModel();
            string mnthname = selecetdmonth;
            int j = DateTime.ParseExact(mnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
            string tomnthname = selectedtomonth;
            int t = DateTime.ParseExact(tomnthname, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
            var FromDate = Convert.ToDateTime("01-" + j + '-' + selectedyear).ToString("dd-MMM-yyyy");
            var ToDate = Convert.ToDateTime("01-" + t + '-' + selectedtoyear).ToString("dd-MMM-yyyy");
            DateTime FT;
            var appdate = DateTime.TryParseExact(FromDate.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out FT);
            {
                FT.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            }
            DateTime TT;
            var apptdate = DateTime.TryParseExact(ToDate.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out TT);
            {
                TT.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            }
            var RevenuefortheMonth = new List<Revenuefortheday>();
            var revenuefortheday = new List<Revenuefortheday>();
            var revenueforthemonth = new List<Revenuefortheday>();
            var FROMDATEs = Convert.ToDateTime(FromDate);
            var fthree = FROMDATEs.AddYears(1);
            var TODATEs = Convert.ToDateTime(ToDate);
            var thtree = TT.AddYears(1);
            var rs = WYNKContext.RevenueSummary.Where(x => x.CmpID == CompanyID && x.ServiceID != null && x.ParentID != null).ToList();
            var services = WYNKContext.Services.Where(x => x.CMPID == CompanyID).ToList();
            var revenuesummary = rs.GroupBy(x => x.ParentID).Select(x => x.FirstOrDefault().ParentID).ToList();
            foreach (var Item in revenuesummary)
            {
                var RAMT = new RevenueAMT();
                var pid = (from gm in rs.Where(x => x.ParentID == Item)
                           group gm by new { gm.ParentID } into hg
                           select new
                           {
                               ParentID = hg.FirstOrDefault().ParentID,
                           }).ToList();
                foreach (var iitem in pid)
                {
                    var Newdata = new Revenuefortheday();
                    Newdata.Revenuedesc = WYNKContext.Services.Where(x => x.ID == iitem.ParentID).Select(x => x.Description).FirstOrDefault();
                    Newdata.RevenueAmount = rs.Where(x => x.ParentID == iitem.ParentID && (x.CreatedUTC.Date >= FROMDATEs.Date && x.CreatedUTC.Date <= fthree.Date)).Sum(x => x.Amount);
                    Newdata.serviceid = iitem.ParentID;
                    Newdata.RevenueMonthAmount = rs.Where(x => x.ParentID == iitem.ParentID && (x.CreatedUTC.Date >= TODATEs.Date && x.CreatedUTC.Date <= thtree.Date)).Sum(x => x.Amount);
                    revenuefortheday.Add(Newdata);
                }
            }

            var RRD = revenuefortheday;
            var ser = services.Where(x => !revenuefortheday.Any(y => y.serviceid == x.ID) && x.ParentID == 0).ToList();
            foreach (var iitems in ser)
            {
                var Newdata = new Revenuefortheday();
                Newdata.Revenuedesc = iitems.Description;
                Newdata.RevenueAmount = 0;
                Newdata.RevenueMonthAmount = 0;
                Newdata.serviceid = iitems.ID;
                RevenuefortheMonth.Add(Newdata);
            }
            var rmd = RevenuefortheMonth;
            var totalmethods = RRD.Concat(rmd);
            RD.TOTALMERGEDRevenue = (from r in totalmethods
                                     select new TOTALMERGEDRevenue
                                     {
                                         NewRevenueAmount = r.RevenueAmount,
                                         OLDRevenueAmount = r.RevenueMonthAmount,
                                         Revenuedesc = r.Revenuedesc,
                                     }).ToList();


            RD.TotalrevenuesumFortheDay = RD.TOTALMERGEDRevenue.Sum(x => x.NewRevenueAmount);
            RD.TotalrevenuesumFoirtheMonth = RD.TOTALMERGEDRevenue.Sum(x => x.OLDRevenueAmount);
            RD.RevenueAllFrom = selecetdmonth + " - " + selectedyear;
            RD.RevenueAllTo = selectedtomonth + " - " + selectedtoyear;
            return RD;

        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
