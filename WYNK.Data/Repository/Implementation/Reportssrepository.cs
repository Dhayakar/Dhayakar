using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Helpers;

namespace WYNK.Data.Repository.Implementation
{
    class Reportssrepository : RepositoryBase<Vreportss>, IReportssrepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        

        public Reportssrepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }
        public Vreportss Todaysearch(string FromDate, string ToDate, int CompanyID)
        {

            DateTime fdata;
            DateTime tdata;
            var appdate = DateTime.TryParseExact(FromDate.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fdata);
            {
                fdata.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            }

            var apptodate = DateTime.TryParseExact(ToDate.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out tdata);
            {
                tdata.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            }
            var reg = WYNKContext.Registration.ToList();
            var regisrtattran = WYNKContext.RegistrationTran.ToList();
            var Online = CMPSContext.OneLineMaster.ToList();
            var To = new Vreportss();
            To.Mreportss2 = new List<Mreportss2>();

            var SRPMD = (from ADM in WYNKContext.Admission.Where(x => x.AdmDate.Date >= fdata && x.AdmDate.Date <= tdata
                                        && x.IsSurgeryCompleted == true)
                         join RG in reg.Where(x => x.CMPID == CompanyID) on ADM.UIN equals RG.UIN
                         select new SURGERYREVIEWPatientMonthtilldatepopulationdetails
                         {
                             UINCount = ADM.UIN,
                         }).ToList();

            var cmp1 = CMPSContext.Company.Where(x => x.CmpID == CompanyID).ToList();
            var cmp2 = CMPSContext.Company.Where(x => x.ParentID == CompanyID).ToList();
            var mergedcompanies = cmp1.Concat(cmp2);

            var pdata = WYNKContext.PatientFootfall.OrderByDescending(x => x.Date).Where(x => x.Date.Date >= fdata && x.Date.Date <= tdata).ToList();

            var totaldata = (from b in pdata
                             where mergedcompanies.Select(x =>x.CmpID).Contains(b.CmpID)
                             select b);

            To.Mreportss2 = (from s in totaldata
                             group s by new { s.Date.Date} into i
                             select new Mreportss2
                             {
                                 Datess = i.Select(x => x.Date.Date).FirstOrDefault(),
                                 Maindates = i.Select(x => x.Date.Date).FirstOrDefault().ToString("dd-MMM-yyyy"),
                                 NewPatients = i.Select(x => x.NewPatients).Sum(),
                                 ReviewPatients = i.Select(x => x.ReviewPatients).Sum(),
                                 Surgeryreviewpatients = 0,
                                 Cumulativetotal = i.Select(x => x.NewPatients).Sum() + i.Select(x => x.ReviewPatients).Sum() + SRPMD.Count(),

                                 RevenueNewPatients = i.Select(x => x.ConsultationNew).Sum(),
                                 revenueReviewPatients = i.Select(x => x.ConsultationReview).Sum(),
                                 RevenueCumulativetotal = i.Select(x => x.ConsultationNew).Sum() + i.Select(x => x.ConsultationReview).Sum(),

                                 NewGeneralNormal = i.Select(x => x.NewGeneralNormal).Sum(),
                                 NewGeneralOcular = i.Select(x => x.NewGeneralOcular).Sum(),
                                 NewPediatricNormal = i.Select(x => x.NewPediatricNormal).Sum(),
                                 NewPediatricOcular = i.Select(x => x.NewPediatricOcular).Sum(),

                                 ReviewGeneralNormal = i.Select(x => x.ReviewGeneralNormal).Sum(),
                                 ReviewGeneralOcular = i.Select(x => x.ReviewGeneralOcular).Sum(),
                                 ReviewPediatricNormal = i.Select(x => x.ReviewPediatricNormal).Sum(),
                                 ReviewPediatricOcular = i.Select(x => x.ReviewPediatricOcular).Sum(),

                                 SReviewGeneralNormal = 0,
                                 SReviewGeneralOcular = 0,
                                 SReviewPediatricNormal = 0,
                                 SReviewPediatricOcular = 0,

                             }).ToList();





            To.totalReveuewAmountwhole = To.Mreportss2.Sum(x => x.RevenueCumulativetotal);
            To.Patientcumulativetotal = To.Mreportss2.Sum(x => x.Cumulativetotal);
            To.NewReveuewAmountwhole = To.Mreportss2.Sum(x => x.RevenueNewPatients);
            To.RevenueReveuewAmountwhole = To.Mreportss2.Sum(x => x.revenueReviewPatients);


            To.NewGeneralNormal = To.Mreportss2.Sum(x => x.NewGeneralNormal);
            To.NewGeneralOcular = To.Mreportss2.Sum(x => x.NewGeneralOcular);
            To.NewPediatricNormal = To.Mreportss2.Sum(x => x.NewPediatricNormal);
            To.NewPediatricOcular = To.Mreportss2.Sum(x => x.NewPediatricOcular);

            To.ReviewGeneralNormal = To.Mreportss2.Sum(x => x.ReviewGeneralNormal);
            To.ReviewGeneralOcular = To.Mreportss2.Sum(x => x.ReviewGeneralOcular);
            To.ReviewPediatricNormal = To.Mreportss2.Sum(x => x.ReviewPediatricNormal);
            To.ReviewPediatricOcular = To.Mreportss2.Sum(x => x.ReviewPediatricOcular);

            To.SReviewGeneralNormal = To.Mreportss2.Sum(x => x.SReviewGeneralNormal);
            To.SReviewGeneralOcular = To.Mreportss2.Sum(x => x.SReviewGeneralOcular);
            To.SReviewPediatricNormal = To.Mreportss2.Sum(x => x.SReviewPediatricNormal);
            To.SReviewPediatricOcular = To.Mreportss2.Sum(x => x.SReviewPediatricOcular);

            To.TotalNew = To.Mreportss2.Sum(x => x.NewPatients);
            To.TotalReview = To.Mreportss2.Sum(x => x.ReviewPatients);
            To.TotalSurgery = To.Mreportss2.Sum(x => x.Surgeryreviewpatients);

            To.FromdateDate = FromDate;
            To.TodateDate = ToDate;
            return To;
        }


        public static int Getcountvalue(int? Data)
        {
            var d = Data;

            if (Data != null)
            {
                d = Data;
            }
            else
            {
                d = 0;
            }

            return Convert.ToInt32(d);
        }
        public Vreportss Getnewgeneralocular(DateTime date, string Generalocular, string cmpid, string CMPbranch)
        {
            var To = new Vreportss();
            var CompanyID = 0;

            var cmp1 = CMPSContext.Company.Where(x => x.CompanyName == cmpid).Select(x => x.CmpID).FirstOrDefault();

            var orgcompanydet = CMPSContext.Company.Where(x => x.CmpID == cmp1 || x.ParentID == cmp1).ToList();

            foreach (var item in orgcompanydet)
            {
                if (item.Address1 == CMPbranch)
                {
                    CompanyID = item.CmpID;
                    To.COmpanyName = CMPSContext.Company.Where(x => x.CmpID == CompanyID).Select(x => x.CompanyName).FirstOrDefault();
                    break;
                }
                else
                {
                    continue;
                }

            }
            var reg = WYNKContext.Registration.Where(x => x.CMPID == CompanyID).ToList();
            var regisrtattran = WYNKContext.RegistrationTran.ToList();
            var OLMid = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "New").Select(x => x.OLMID).FirstOrDefault();
            

            To.COmpanyName = CMPSContext.Company.Where(x => x.CmpID == CompanyID).Select(x => x.CompanyName).FirstOrDefault();
            var Sreportdetails = (from rg in reg
                                  join r in WYNKContext.RegistrationTran.Where(x => x.PatientVisitType == Convert.ToString(OLMid)) on rg.UIN equals r.UIN
                                  where (r.DateofVisit.Date == date && rg.CMPID == CompanyID)
                                  select new
                                  {
                                      UIN = rg.UIN,
                                      Dob = Getdb(rg.DateofBirth, CompanyID)
                                  }).ToList();
            To.UINdetailss = new List<UINdetails>();

            To.Mreportsdetailss = new List<Mreportsdetails>();


            foreach (var ss in Sreportdetails)
            {
                var uu = new UINdetails();

                var PUIn = WYNKContext.Registration.Where(x => x.UIN == ss.UIN).Select(x => x.DateofBirth.Date).FirstOrDefault();
                var PodiatristAge = CMPSContext.Setup.Where(x => x.CMPID == CompanyID).Select(x => x.Pediatric).FirstOrDefault();
                var Age = PasswordEncodeandDecode.ToAgeString(PUIn);
                var result = Regex.Match(Age, @"\d+").Value;

                if (Convert.ToInt32(result) >= Convert.ToInt32(PodiatristAge))
                {

                    var exuin = WYNKContext.RegistrationExtension.Where(x => x.UIN == ss.UIN).Select(x => x.UIN).FirstOrDefault();

                    if (exuin != null)
                    {
                        uu.UIN = ss.UIN;

                        To.UINdetailss.Add(uu);
                    }



                }


            }


            foreach (var item in To.UINdetailss)
            {


                var Report = new Mreportsdetails();

                Report.UIN = item.UIN;
                var FName = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Name).FirstOrDefault();
                var MName = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.MiddleName).FirstOrDefault());
                var LName = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.LastName).FirstOrDefault();
                Report.Name = FName + MName + LName;
                Report.Age = PasswordEncodeandDecode.ToAgeString(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.DateofBirth).FirstOrDefault());
                Report.Gender = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Gender).FirstOrDefault();
                Report.Phone = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Phone).FirstOrDefault();
                var A1 = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Address1).FirstOrDefault());
                var A2 = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Address2).FirstOrDefault());
                var A3 = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Address3).FirstOrDefault());
                Report.Address = A1 + A2 + A3;
                Report.PatientType = "New General Ocular";
                Report.ConsultationAmount = WYNKContext.RegistrationTran.Where(x => x.UIN == item.UIN).Select(x => x.ConsulationFees).FirstOrDefault();
                var Reye = WYNKContext.RegistrationExtension.Where(x => x.UIN == item.UIN).Select(x => x.OD).FirstOrDefault();
                var Leye = WYNKContext.RegistrationExtension.Where(x => x.UIN == item.UIN).Select(x => x.OS).FirstOrDefault();
                var Beye = WYNKContext.RegistrationExtension.Where(x => x.UIN == item.UIN).Select(x => x.OU).FirstOrDefault();

                if (Reye == true)
                {
                    Report.EYE = "Right Eye (OD)";
                }

                if (Leye == true)
                {
                    Report.EYE = "Left Eye (OS)";
                }

                if (Beye == true)
                {
                    Report.EYE = "Both Eyes (OU)";
                }
                To.Mreportsdetailss.Add(Report);


            }

            To.Date = date.ToString("dd-MMM-yyyy");
            return To;
        }
        public Vreportss GetnewgeneralNormal(DateTime date, string Generalocular, string cmpid, string CMPbranch)
        {

            
            var To = new Vreportss();
            var CompanyID = 0;

            var cmp1 = CMPSContext.Company.Where(x => x.CompanyName == cmpid).Select(x => x.CmpID).FirstOrDefault();

            var orgcompanydet = CMPSContext.Company.Where(x => x.CmpID == cmp1 || x.ParentID == cmp1).ToList();

            foreach(var item in orgcompanydet)
            {
                if(item.Address1 == CMPbranch)
                {
                    CompanyID = item.CmpID;
                    To.COmpanyName = CMPSContext.Company.Where(x => x.CmpID == CompanyID).Select(x => x.CompanyName).FirstOrDefault();
                    break;
                }
                else
                {
                    continue;
                }
                
            }
            var reg = WYNKContext.Registration.Where(x => x.CMPID == CompanyID).ToList();
            var OLMid = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "New").Select(x => x.OLMID).FirstOrDefault();
            To.COmpanyName = CMPSContext.Company.Where(x => x.CmpID == CompanyID).Select(x => x.CompanyName).FirstOrDefault();
            var Sreportdetails = (from rg in reg
                                  join r in WYNKContext.RegistrationTran.Where(x => x.PatientVisitType == Convert.ToString(OLMid)) on rg.UIN equals r.UIN
                                  where (r.DateofVisit.Date == date && rg.CMPID == CompanyID)
                                  select new
                                  {
                                      UIN = rg.UIN,
                                      Dob = Getdb(rg.DateofBirth, CompanyID)
                                  }).ToList();
            To.UINdetailss = new List<UINdetails>();


            foreach (var ss in Sreportdetails)
            {
                var uu = new UINdetails();

                var PUIn = WYNKContext.Registration.Where(x => x.UIN == ss.UIN).Select(x => x.DateofBirth.Date).FirstOrDefault();
                var PodiatristAge = CMPSContext.Setup.Where(x => x.CMPID == CompanyID).Select(x => x.Pediatric).FirstOrDefault();
                var Age = PasswordEncodeandDecode.ToAgeString(PUIn);
                var result = Regex.Match(Age, @"\d+").Value;

                if (Convert.ToInt32(result) >= Convert.ToInt32(PodiatristAge))
                {

                    var exuin = WYNKContext.RegistrationExtension.Where(x => x.UIN == ss.UIN).Select(x => x.UIN).FirstOrDefault();

                    if (exuin == null)
                    {
                        uu.UIN = ss.UIN;

                        To.UINdetailss.Add(uu);
                    }

                }


            }


            To.Mreportsdetailss = new List<Mreportsdetails>();


            foreach (var item in To.UINdetailss)
            {


                var Report = new Mreportsdetails();

                Report.UIN = item.UIN;
                var FName = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Name).FirstOrDefault();
                var MName = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.MiddleName).FirstOrDefault());
                var LName = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.LastName).FirstOrDefault();
                Report.Name = FName + MName + LName;
                Report.Age = PasswordEncodeandDecode.ToAgeString(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.DateofBirth).FirstOrDefault());
                Report.Gender = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Gender).FirstOrDefault();
                Report.Phone = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Phone).FirstOrDefault();
                var A1 = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Address1).FirstOrDefault());
                var A2 = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Address2).FirstOrDefault());
                var A3 = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Address3).FirstOrDefault());
                Report.Address = A1 + A2 + A3;
                Report.ConsultationAmount = WYNKContext.RegistrationTran.Where(x => x.UIN == item.UIN).Select(x => x.ConsulationFees).FirstOrDefault();
                Report.PatientType = "New General Normal";
                To.Mreportsdetailss.Add(Report);


            }

            To.Date = date.ToString("dd-MMM-yyyy");
            return To;
        }
        public Vreportss GetnewpediatricNormal(DateTime date, string Generalocular, string cmpid, string CMPbranch)
        {
            
            var To = new Vreportss();
            var CompanyID = 0;
            var cmp1 = CMPSContext.Company.Where(x => x.CompanyName == cmpid).Select(x => x.CmpID).FirstOrDefault();
            var orgcompanydet = CMPSContext.Company.Where(x => x.CmpID == cmp1 || x.ParentID == cmp1).ToList();

            foreach (var item in orgcompanydet)
            {
                if (item.Address1 == CMPbranch)
                {
                    CompanyID = item.CmpID;
                    To.COmpanyName = CMPSContext.Company.Where(x => x.CmpID == CompanyID).Select(x => x.CompanyName).FirstOrDefault();
                    break;
                }
                else
                {
                    continue;
                }

            }
            var reg = WYNKContext.Registration.Where(x => x.CMPID == CompanyID).ToList();
            To.COmpanyName = CMPSContext.Company.Where(x => x.CmpID == CompanyID).Select(x => x.CompanyName).FirstOrDefault();
            var OLMid = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "New").Select(x => x.OLMID).FirstOrDefault();

            var Sreportdetails = (from rg in reg
                                  join r in WYNKContext.RegistrationTran.Where(x => x.PatientVisitType == Convert.ToString(OLMid)) on rg.UIN equals r.UIN
                                  where (r.DateofVisit.Date == date && rg.CMPID == CompanyID)
                                  select new
                                  {
                                      UIN = rg.UIN,
                                  }).ToList();
            To.UINdetailss = new List<UINdetails>();


            foreach (var ss in Sreportdetails)
            {
                var uu = new UINdetails();


                var PUIn = WYNKContext.Registration.Where(x => x.UIN == ss.UIN).Select(x => x.DateofBirth.Date).FirstOrDefault();
                var PodiatristAge = CMPSContext.Setup.Where(x => x.CMPID == CompanyID).Select(x => x.Pediatric).FirstOrDefault();
                var Age = PasswordEncodeandDecode.ToAgeString(PUIn);
                var result = Regex.Match(Age, @"\d+").Value;

                if (Convert.ToInt32(result) <= Convert.ToInt32(PodiatristAge))
                {
                    var exuin = WYNKContext.RegistrationExtension.Where(x => x.UIN == ss.UIN).Select(x => x.UIN).FirstOrDefault();

                    if (exuin == null)
                    {
                        uu.UIN = ss.UIN;

                        To.UINdetailss.Add(uu);
                    }

                }




            }


            To.Mreportsdetailss = new List<Mreportsdetails>();


            foreach (var item in To.UINdetailss)
            {


                var Report = new Mreportsdetails();

                Report.UIN = item.UIN;
                var FName = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Name).FirstOrDefault();
                var MName = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.MiddleName).FirstOrDefault());
                var LName = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.LastName).FirstOrDefault();
                Report.Name = FName + MName + LName;
                Report.ConsultationAmount = WYNKContext.RegistrationTran.Where(x => x.UIN == item.UIN).Select(x => x.ConsulationFees).FirstOrDefault();
                Report.Age = PasswordEncodeandDecode.ToAgeString(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.DateofBirth).FirstOrDefault());
                Report.Gender = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Gender).FirstOrDefault();
                Report.Phone = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Phone).FirstOrDefault();
                var A1 = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Address1).FirstOrDefault());
                var A2 = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Address2).FirstOrDefault());
                var A3 = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Address3).FirstOrDefault());
                Report.Address = A1 + A2 + A3;
                Report.PatientType = "New Pediatric Normal";
                To.Mreportsdetailss.Add(Report);


            }

            To.Date = date.ToString("dd-MMM-yyyy");



            return To;
        }
        public Vreportss Getnewpediatricocular(DateTime date, string Generalocular, string cmpid, string CMPbranch)
        {
            var To = new Vreportss();
            var CompanyID = 0;
            var cmp1 = CMPSContext.Company.Where(x => x.CompanyName == cmpid).Select(x => x.CmpID).FirstOrDefault();
            var orgcompanydet = CMPSContext.Company.Where(x => x.CmpID == cmp1 || x.ParentID == cmp1).ToList();

            foreach (var item in orgcompanydet)
            {
                if (item.Address1 == CMPbranch)
                {
                    CompanyID = item.CmpID;
                    To.COmpanyName = CMPSContext.Company.Where(x => x.CmpID == CompanyID).Select(x => x.CompanyName).FirstOrDefault();
                    break;
                }
                else
                {
                    continue;
                }

            }
            var reg = WYNKContext.Registration.Where(x => x.CMPID == CompanyID).ToList();
            
            To.COmpanyName = CMPSContext.Company.Where(x => x.CmpID == CompanyID).Select(x => x.CompanyName).FirstOrDefault();

            var OLMid = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "New").Select(x => x.OLMID).FirstOrDefault();

            var Sreportdetails = (from rg in reg
                                  join r in WYNKContext.RegistrationTran.Where(x => x.PatientVisitType == Convert.ToString(OLMid)) on rg.UIN equals r.UIN
                                  where (r.DateofVisit.Date == date && rg.CMPID == CompanyID)
                                  select new
                                  {
                                      UIN = rg.UIN,
                                      Dob = GetPdb(rg.DateofBirth, CompanyID)
                                  }).ToList();
            To.UINdetailss = new List<UINdetails>();


            foreach (var ss in Sreportdetails)
            {
                var uu = new UINdetails();

                var PUIn = WYNKContext.Registration.Where(x => x.UIN == ss.UIN).Select(x => x.DateofBirth.Date).FirstOrDefault();
                var PodiatristAge = CMPSContext.Setup.Where(x => x.CMPID == CompanyID).Select(x => x.Pediatric).FirstOrDefault();
                var Age = PasswordEncodeandDecode.ToAgeString(PUIn);
                var result = Regex.Match(Age, @"\d+").Value;

                if (Convert.ToInt32(result) <= Convert.ToInt32(PodiatristAge))
                {

                    var exuin = WYNKContext.RegistrationExtension.Where(x => x.UIN == ss.UIN).Select(x => x.UIN).FirstOrDefault();

                    if (exuin != null)
                    {
                        uu.UIN = ss.UIN;

                        To.UINdetailss.Add(uu);
                    }



                }

            }

            To.Mreportsdetailss = new List<Mreportsdetails>();


            foreach (var item in To.UINdetailss)
            {


                var Report = new Mreportsdetails();

                Report.UIN = item.UIN;
                var FName = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Name).FirstOrDefault();
                var MName = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.MiddleName).FirstOrDefault());
                var LName = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.LastName).FirstOrDefault();
                Report.Name = FName + MName + LName;
                Report.ConsultationAmount = WYNKContext.RegistrationTran.Where(x => x.UIN == item.UIN).Select(x => x.ConsulationFees).FirstOrDefault();
                Report.Age = PasswordEncodeandDecode.ToAgeString(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.DateofBirth).FirstOrDefault());
                Report.Gender = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Gender).FirstOrDefault();
                Report.Phone = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Phone).FirstOrDefault();
                var A1 = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Address1).FirstOrDefault());
                var A2 = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Address2).FirstOrDefault());
                var A3 = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Address3).FirstOrDefault());
                Report.Address = A1 + A2 + A3;
                Report.PatientType = "New Pediatric Ocular";
                var Reye = WYNKContext.RegistrationExtension.Where(x => x.UIN == item.UIN).Select(x => x.OD).FirstOrDefault();
                var Leye = WYNKContext.RegistrationExtension.Where(x => x.UIN == item.UIN).Select(x => x.OS).FirstOrDefault();
                var Beye = WYNKContext.RegistrationExtension.Where(x => x.UIN == item.UIN).Select(x => x.OU).FirstOrDefault();
                if (Reye == true)
                {
                    Report.EYE = "Right Eye (OD)";
                }

                if (Leye == true)
                {
                    Report.EYE = "Left Eye (OS)";
                }

                if (Beye == true)
                {
                    Report.EYE = "Both Eyes (OU)";
                }

                To.Mreportsdetailss.Add(Report);


            }

            To.Date = date.ToString("dd-MMM-yyyy");

            return To;
        }
        public Vreportss Getnewtotal(DateTime date, string Generalocular, string cmpid, string CMPbranch)
        {
            var To = new Vreportss();
            var CompanyID = 0;
            var cmp1 = CMPSContext.Company.Where(x => x.CompanyName == cmpid).Select(x => x.CmpID).FirstOrDefault();
            var orgcompanydet = CMPSContext.Company.Where(x => x.CmpID == cmp1 || x.ParentID == cmp1).ToList();

            foreach (var item in orgcompanydet)
            {
                if (item.Address1 == CMPbranch)
                {
                    CompanyID = item.CmpID;
                    To.COmpanyName = CMPSContext.Company.Where(x => x.CmpID == CompanyID).Select(x => x.CompanyName).FirstOrDefault();
                    break;
                }
                else
                {
                    continue;
                }

            }
            var reg = WYNKContext.Registration.Where(x => x.CMPID == CompanyID).ToList();
            
            var OLMid = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "New").Select(x => x.OLMID).FirstOrDefault();
            To.COmpanyName = CMPSContext.Company.Where(x => x.CmpID == CompanyID).Select(x => x.CompanyName).FirstOrDefault();
            var reportdetails = (from r in reg
                                 join rg in WYNKContext.RegistrationTran on r.UIN equals rg.UIN
                                 where (rg.DateofVisit.Date == date && rg.PatientVisitType == Convert.ToString(OLMid))
                                 select new
                                 {
                                     UIN = r.UIN,

                                 }).ToList();

            To.Mreportsdetailss = new List<Mreportsdetails>();


            foreach (var item in reportdetails)
            {
                var Report = new Mreportsdetails();

                Report.UIN = item.UIN;
                var FName = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Name).FirstOrDefault();
                var MName = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.MiddleName).FirstOrDefault());
                var LName = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.LastName).FirstOrDefault();
                Report.Name = FName + MName + LName;
                Report.Age = PasswordEncodeandDecode.ToAgeString(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.DateofBirth).FirstOrDefault());
                Report.Gender = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Gender).FirstOrDefault();
                Report.Phone = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Phone).FirstOrDefault();
                var A1 = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Address1).FirstOrDefault());
                var A2 = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Address2).FirstOrDefault());
                var A3 = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Address3).FirstOrDefault());
                Report.Address = A1 + A2 + A3;
                Report.ConsultationAmount = WYNKContext.RegistrationTran.Where(x => x.UIN == item.UIN).Select(x => x.ConsulationFees).FirstOrDefault();
                Report.PatientType = "New Pediatric Ocular";
                var Reye = WYNKContext.RegistrationExtension.Where(x => x.UIN == item.UIN).Select(x => x.OD).FirstOrDefault();
                var Leye = WYNKContext.RegistrationExtension.Where(x => x.UIN == item.UIN).Select(x => x.OS).FirstOrDefault();
                var Beye = WYNKContext.RegistrationExtension.Where(x => x.UIN == item.UIN).Select(x => x.OU).FirstOrDefault();

                if (Reye == true)
                {
                    Report.EYE = "Right Eye (OD)";
                }

                if (Leye == true)
                {
                    Report.EYE = "Left Eye (OS)";
                }

                if (Beye == true)
                {
                    Report.EYE = "Both Eyes (OU)";
                }

                if (Reye == false && Leye == false && Beye == false)
                {
                    Report.EYE = "-- No Ocular -- ";
                }

                To.Mreportsdetailss.Add(Report);

            }

            To.Date = date.ToString("dd-MMM-yyyy");

            return To;
        }



        public Vreportss GetReviewgeneralocular(DateTime date, string Generalocular, string cmpid, string CMPbranch)
        {
            var To = new Vreportss();
            var CompanyID = 0;
            var cmp1 = CMPSContext.Company.Where(x => x.CompanyName == cmpid).Select(x => x.CmpID).FirstOrDefault();
            var orgcompanydet = CMPSContext.Company.Where(x => x.CmpID == cmp1 || x.ParentID == cmp1).ToList();

            foreach (var item in orgcompanydet)
            {
                if (item.Address1 == CMPbranch)
                {
                    CompanyID = item.CmpID;
                    To.COmpanyName = CMPSContext.Company.Where(x => x.CmpID == CompanyID).Select(x => x.CompanyName).FirstOrDefault();
                    break;
                }
                else
                {
                    continue;
                }

            }
            var reg = WYNKContext.Registration.Where(x => x.CMPID == CompanyID).ToList();
            var regisrtattran = WYNKContext.RegistrationTran.ToList();
            var OLMid = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Review").Select(x => x.OLMID).FirstOrDefault();
            

            To.COmpanyName = CMPSContext.Company.Where(x => x.CmpID == CompanyID).Select(x => x.CompanyName).FirstOrDefault();
            var Sreportdetails = (from rg in reg
                                  join r in WYNKContext.RegistrationTran.Where(x => x.PatientVisitType == Convert.ToString(OLMid)) on rg.UIN equals r.UIN
                                  where (r.DateofVisit.Date == date && rg.CMPID == CompanyID)
                                  select new
                                  {
                                      UIN = rg.UIN,
                                      Dob = Getdb(rg.DateofBirth, CompanyID)
                                  }).ToList();
            To.UINdetailss = new List<UINdetails>();

            To.Mreportsdetailss = new List<Mreportsdetails>();


            foreach (var ss in Sreportdetails)
            {
                var uu = new UINdetails();

                var PUIn = WYNKContext.Registration.Where(x => x.UIN == ss.UIN).Select(x => x.DateofBirth.Date).FirstOrDefault();
                var PodiatristAge = CMPSContext.Setup.Where(x => x.CMPID == CompanyID).Select(x => x.Pediatric).FirstOrDefault();
                var Age = PasswordEncodeandDecode.ToAgeString(PUIn);
                var result = Regex.Match(Age, @"\d+").Value;

                if (Convert.ToInt32(result) >= Convert.ToInt32(PodiatristAge))
                {

                    var exuin = WYNKContext.RegistrationExtension.Where(x => x.UIN == ss.UIN).Select(x => x.UIN).FirstOrDefault();

                    if (exuin != null)
                    {
                        uu.UIN = ss.UIN;

                        To.UINdetailss.Add(uu);
                    }



                }


            }


            foreach (var item in To.UINdetailss)
            {


                var Report = new Mreportsdetails();

                Report.UIN = item.UIN;
                var FName = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Name).FirstOrDefault();
                var MName = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.MiddleName).FirstOrDefault());
                var LName = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.LastName).FirstOrDefault();
                Report.Name = FName + MName + LName;
                Report.Age = PasswordEncodeandDecode.ToAgeString(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.DateofBirth).FirstOrDefault());
                Report.Gender = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Gender).FirstOrDefault();
                Report.Phone = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Phone).FirstOrDefault();
                var A1 = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Address1).FirstOrDefault());
                var A2 = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Address2).FirstOrDefault());
                var A3 = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Address3).FirstOrDefault());
                Report.Address = A1 + A2 + A3;
                Report.PatientType = "Review General Ocular";
                var Reye = WYNKContext.RegistrationExtension.Where(x => x.UIN == item.UIN).Select(x => x.OD).FirstOrDefault();
                var Leye = WYNKContext.RegistrationExtension.Where(x => x.UIN == item.UIN).Select(x => x.OS).FirstOrDefault();
                var Beye = WYNKContext.RegistrationExtension.Where(x => x.UIN == item.UIN).Select(x => x.OU).FirstOrDefault();
                Report.ConsultationAmount = WYNKContext.RegistrationTran.Where(x => x.UIN == item.UIN).Select(x => x.ConsulationFees).FirstOrDefault();
                if (Reye == true)
                {
                    Report.EYE = "Right Eye (OD)";
                }

                if (Leye == true)
                {
                    Report.EYE = "Left Eye (OS)";
                }

                if (Beye == true)
                {
                    Report.EYE = "Both Eyes (OU)";
                }
                To.Mreportsdetailss.Add(Report);


            }

            To.Date = date.ToString("dd-MMM-yyyy");
            return To;
        }
        public Vreportss GetReviewgeneralNormal(DateTime date, string Generalocular, string cmpid, string CMPbranch)
        {
            var To = new Vreportss();
            var CompanyID = 0;
            var cmp1 = CMPSContext.Company.Where(x => x.CompanyName == cmpid).Select(x => x.CmpID).FirstOrDefault();
            var orgcompanydet = CMPSContext.Company.Where(x => x.CmpID == cmp1 || x.ParentID == cmp1).ToList();

            foreach (var item in orgcompanydet)
            {
                if (item.Address1 == CMPbranch)
                {
                    CompanyID = item.CmpID;
                    To.COmpanyName = CMPSContext.Company.Where(x => x.CmpID == CompanyID).Select(x => x.CompanyName).FirstOrDefault();
                    break;
                }
                else
                {
                    continue;
                }

            }
            var reg = WYNKContext.Registration.Where(x => x.CMPID == CompanyID).ToList();
            
            var OLMid = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Review").Select(x => x.OLMID).FirstOrDefault();
            To.COmpanyName = CMPSContext.Company.Where(x => x.CmpID == CompanyID).Select(x => x.CompanyName).FirstOrDefault();
            var Sreportdetails = (from rg in reg
                                  join r in WYNKContext.RegistrationTran.Where(x => x.PatientVisitType == Convert.ToString(OLMid)) on rg.UIN equals r.UIN
                                  where (r.DateofVisit.Date == date && rg.CMPID == CompanyID)
                                  select new
                                  {
                                      UIN = rg.UIN,
                                      Dob = Getdb(rg.DateofBirth, CompanyID)
                                  }).ToList();
            To.UINdetailss = new List<UINdetails>();


            foreach (var ss in Sreportdetails)
            {
                var uu = new UINdetails();

                var PUIn = WYNKContext.Registration.Where(x => x.UIN == ss.UIN).Select(x => x.DateofBirth.Date).FirstOrDefault();
                var PodiatristAge = CMPSContext.Setup.Where(x => x.CMPID == CompanyID).Select(x => x.Pediatric).FirstOrDefault();
                var Age = PasswordEncodeandDecode.ToAgeString(PUIn);
                var result = Regex.Match(Age, @"\d+").Value;

                if (Convert.ToInt32(result) >= Convert.ToInt32(PodiatristAge))
                {

                    var exuin = WYNKContext.RegistrationExtension.Where(x => x.UIN == ss.UIN).Select(x => x.UIN).FirstOrDefault();

                    if (exuin == null)
                    {
                        uu.UIN = ss.UIN;

                        To.UINdetailss.Add(uu);
                    }

                }


            }


            To.Mreportsdetailss = new List<Mreportsdetails>();


            foreach (var item in To.UINdetailss)
            {


                var Report = new Mreportsdetails();

                Report.UIN = item.UIN;
                var FName = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Name).FirstOrDefault();
                var MName = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.MiddleName).FirstOrDefault());
                var LName = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.LastName).FirstOrDefault();
                Report.Name = FName + MName + LName;
                Report.Age = PasswordEncodeandDecode.ToAgeString(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.DateofBirth).FirstOrDefault());
                Report.Gender = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Gender).FirstOrDefault();
                Report.Phone = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Phone).FirstOrDefault();
                var A1 = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Address1).FirstOrDefault());
                var A2 = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Address2).FirstOrDefault());
                var A3 = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Address3).FirstOrDefault());
                Report.Address = A1 + A2 + A3;
                Report.ConsultationAmount = WYNKContext.RegistrationTran.Where(x => x.UIN == item.UIN).Select(x => x.ConsulationFees).FirstOrDefault();
                Report.PatientType = "Review General Normal";
                To.Mreportsdetailss.Add(Report);


            }

            To.Date = date.ToString("dd-MMM-yyyy");
            return To;
        }
        public Vreportss GetReviewpediatricnormal(DateTime date, string Generalocular, string cmpid, string CMPbranch)
        {
            var To = new Vreportss();
            var CompanyID = 0;
            var cmp1 = CMPSContext.Company.Where(x => x.CompanyName == cmpid).Select(x => x.CmpID).FirstOrDefault();
            var orgcompanydet = CMPSContext.Company.Where(x => x.CmpID == cmp1 || x.ParentID == cmp1).ToList();

            foreach (var item in orgcompanydet)
            {
                if (item.Address1 == CMPbranch)
                {
                    CompanyID = item.CmpID;
                    To.COmpanyName = CMPSContext.Company.Where(x => x.CmpID == CompanyID).Select(x => x.CompanyName).FirstOrDefault();
                    break;
                }
                else
                {
                    continue;
                }

            }
            var reg = WYNKContext.Registration.Where(x => x.CMPID == CompanyID).ToList();
            
            To.COmpanyName = CMPSContext.Company.Where(x => x.CmpID == CompanyID).Select(x => x.CompanyName).FirstOrDefault();
            var OLMid = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Review").Select(x => x.OLMID).FirstOrDefault();

            var Sreportdetails = (from rg in reg
                                  join r in WYNKContext.RegistrationTran.Where(x => x.PatientVisitType == Convert.ToString(OLMid)) on rg.UIN equals r.UIN
                                  where (r.DateofVisit.Date == date && rg.CMPID == CompanyID)
                                  select new
                                  {
                                      UIN = rg.UIN,
                                  }).ToList();
            To.UINdetailss = new List<UINdetails>();


            foreach (var ss in Sreportdetails)
            {
                var uu = new UINdetails();

                var PUIn = WYNKContext.Registration.Where(x => x.UIN == ss.UIN).Select(x => x.DateofBirth.Date).FirstOrDefault();
                var PodiatristAge = CMPSContext.Setup.Where(x => x.CMPID == CompanyID).Select(x => x.Pediatric).FirstOrDefault();
                var Age = PasswordEncodeandDecode.ToAgeString(PUIn);
                var result = Regex.Match(Age, @"\d+").Value;

                if (Convert.ToInt32(result) <= Convert.ToInt32(PodiatristAge))
                {
                    var exuin = WYNKContext.RegistrationExtension.Where(x => x.UIN == ss.UIN).Select(x => x.UIN).FirstOrDefault();

                    if (exuin == null)
                    {
                        uu.UIN = ss.UIN;

                        To.UINdetailss.Add(uu);
                    }

                }




            }


            To.Mreportsdetailss = new List<Mreportsdetails>();


            foreach (var item in To.UINdetailss)
            {


                var Report = new Mreportsdetails();

                Report.UIN = item.UIN;
                var FName = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Name).FirstOrDefault();
                var MName = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.MiddleName).FirstOrDefault());
                var LName = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.LastName).FirstOrDefault();
                Report.Name = FName + MName + LName;
                Report.Age = PasswordEncodeandDecode.ToAgeString(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.DateofBirth).FirstOrDefault());
                Report.Gender = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Gender).FirstOrDefault();
                Report.Phone = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Phone).FirstOrDefault();
                Report.ConsultationAmount = WYNKContext.RegistrationTran.Where(x => x.UIN == item.UIN).Select(x => x.ConsulationFees).FirstOrDefault();
                var A1 = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Address1).FirstOrDefault());
                var A2 = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Address2).FirstOrDefault());
                var A3 = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Address3).FirstOrDefault());
                Report.Address = A1 + A2 + A3;
                Report.PatientType = "Review Pediatric Normal";
                To.Mreportsdetailss.Add(Report);


            }

            To.Date = date.ToString("dd-MMM-yyyy");



            return To;
        }
        public Vreportss GetReviewpediatricocular(DateTime date, string Generalocular, string cmpid, string CMPbranch)
        {
            var To = new Vreportss();
            var CompanyID = 0;
            var cmp1 = CMPSContext.Company.Where(x => x.CompanyName == cmpid).Select(x => x.CmpID).FirstOrDefault();
            var orgcompanydet = CMPSContext.Company.Where(x => x.CmpID == cmp1 || x.ParentID == cmp1).ToList();

            foreach (var item in orgcompanydet)
            {
                if (item.Address1 == CMPbranch)
                {
                    CompanyID = item.CmpID;
                    To.COmpanyName = CMPSContext.Company.Where(x => x.CmpID == CompanyID).Select(x => x.CompanyName).FirstOrDefault();
                    break;
                }
                else
                {
                    continue;
                }

            }
            var reg = WYNKContext.Registration.Where(x => x.CMPID == CompanyID).ToList();
            

            To.COmpanyName = CMPSContext.Company.Where(x => x.CmpID == CompanyID).Select(x => x.CompanyName).FirstOrDefault();
            var OLMid = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Review").Select(x => x.OLMID).FirstOrDefault();

            var Sreportdetails = (from rg in reg
                                  join r in WYNKContext.RegistrationTran.Where(x => x.PatientVisitType == Convert.ToString(OLMid)) on rg.UIN equals r.UIN
                                  where (r.DateofVisit.Date == date && rg.CMPID == CompanyID)
                                  select new
                                  {
                                      UIN = rg.UIN,
                                      Dob = GetPdb(rg.DateofBirth, CompanyID)
                                  }).ToList();
            To.UINdetailss = new List<UINdetails>();


            foreach (var ss in Sreportdetails)
            {
                var uu = new UINdetails();

                var PUIn = WYNKContext.Registration.Where(x => x.UIN == ss.UIN).Select(x => x.DateofBirth.Date).FirstOrDefault();
                var PodiatristAge = CMPSContext.Setup.Where(x => x.CMPID == CompanyID).Select(x => x.Pediatric).FirstOrDefault();
                var Age = PasswordEncodeandDecode.ToAgeString(PUIn);
                var result = Regex.Match(Age, @"\d+").Value;

                if (Convert.ToInt32(result) <= Convert.ToInt32(PodiatristAge))
                {

                    var exuin = WYNKContext.RegistrationExtension.Where(x => x.UIN == ss.UIN).Select(x => x.UIN).FirstOrDefault();

                    if (exuin != null)
                    {
                        uu.UIN = ss.UIN;

                        To.UINdetailss.Add(uu);
                    }



                }

            }

            To.Mreportsdetailss = new List<Mreportsdetails>();


            foreach (var item in To.UINdetailss)
            {


                var Report = new Mreportsdetails();

                Report.UIN = item.UIN;
                var FName = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Name).FirstOrDefault();
                var MName = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.MiddleName).FirstOrDefault());
                var LName = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.LastName).FirstOrDefault();
                Report.Name = FName + MName + LName;
                Report.ConsultationAmount = WYNKContext.RegistrationTran.Where(x => x.UIN == item.UIN).Select(x => x.ConsulationFees).FirstOrDefault();
                Report.Age = PasswordEncodeandDecode.ToAgeString(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.DateofBirth).FirstOrDefault());
                Report.Gender = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Gender).FirstOrDefault();
                Report.Phone = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Phone).FirstOrDefault();
                var A1 = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Address1).FirstOrDefault());
                var A2 = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Address2).FirstOrDefault());
                var A3 = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Address3).FirstOrDefault());
                Report.Address = A1 + A2 + A3;
                Report.PatientType = "Review Pediatric Ocular";
                var Reye = WYNKContext.RegistrationExtension.Where(x => x.UIN == item.UIN).Select(x => x.OD).FirstOrDefault();
                var Leye = WYNKContext.RegistrationExtension.Where(x => x.UIN == item.UIN).Select(x => x.OS).FirstOrDefault();
                var Beye = WYNKContext.RegistrationExtension.Where(x => x.UIN == item.UIN).Select(x => x.OU).FirstOrDefault();
                if (Reye == true)
                {
                    Report.EYE = "Right Eye (OD)";
                }

                if (Leye == true)
                {
                    Report.EYE = "Left Eye (OS)";
                }

                if (Beye == true)
                {
                    Report.EYE = "Both Eyes (OU)";
                }

                To.Mreportsdetailss.Add(Report);


            }

            To.Date = date.ToString("dd-MMM-yyyy");

            return To;
        }
        public Vreportss Getviewtotal(DateTime date, string Generalocular, string cmpid, string CMPbranch)
        {
            var To = new Vreportss();
            var CompanyID = 0;
            var cmp1 = CMPSContext.Company.Where(x => x.CompanyName == cmpid).Select(x => x.CmpID).FirstOrDefault();
            var orgcompanydet = CMPSContext.Company.Where(x => x.CmpID == cmp1 || x.ParentID == cmp1).ToList();

            foreach (var item in orgcompanydet)
            {
                if (item.Address1 == CMPbranch)
                {
                    CompanyID = item.CmpID;
                    To.COmpanyName = CMPSContext.Company.Where(x => x.CmpID == CompanyID).Select(x => x.CompanyName).FirstOrDefault();
                    break;
                }
                else
                {
                    continue;
                }

            }
            var reg = WYNKContext.Registration.Where(x => x.CMPID == CompanyID).ToList();
            
            var OLMid = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Review").Select(x => x.OLMID).FirstOrDefault();
            To.COmpanyName = CMPSContext.Company.Where(x => x.CmpID == CompanyID).Select(x => x.CompanyName).FirstOrDefault();
            var reportdetails = (from r in reg
                                 join rg in WYNKContext.RegistrationTran on r.UIN equals rg.UIN
                                 where (rg.DateofVisit.Date == date && rg.PatientVisitType == Convert.ToString(OLMid))
                                 select new
                                 {
                                     UIN = r.UIN,

                                 }).ToList();

            To.Mreportsdetailss = new List<Mreportsdetails>();


            foreach (var item in reportdetails)
            {
                var Report = new Mreportsdetails();

                Report.UIN = item.UIN;
                var FName = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Name).FirstOrDefault();
                var MName = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.MiddleName).FirstOrDefault());
                var LName = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.LastName).FirstOrDefault();
                Report.Name = FName + MName + LName;
                Report.Age = PasswordEncodeandDecode.ToAgeString(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.DateofBirth).FirstOrDefault());
                Report.Gender = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Gender).FirstOrDefault();
                Report.Phone = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Phone).FirstOrDefault();
                var A1 = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Address1).FirstOrDefault());
                var A2 = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Address2).FirstOrDefault());
                var A3 = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Address3).FirstOrDefault());
                Report.Address = A1 + A2 + A3;
                Report.ConsultationAmount = WYNKContext.RegistrationTran.Where(x => x.UIN == item.UIN).Select(x => x.ConsulationFees).FirstOrDefault();
                Report.PatientType = "New Pediatric Ocular";
                var Reye = WYNKContext.RegistrationExtension.Where(x => x.UIN == item.UIN).Select(x => x.OD).FirstOrDefault();
                var Leye = WYNKContext.RegistrationExtension.Where(x => x.UIN == item.UIN).Select(x => x.OS).FirstOrDefault();
                var Beye = WYNKContext.RegistrationExtension.Where(x => x.UIN == item.UIN).Select(x => x.OU).FirstOrDefault();

                if (Reye == true)
                {
                    Report.EYE = "Right Eye (OD)";
                }

                if (Leye == true)
                {
                    Report.EYE = "Left Eye (OS)";
                }

                if (Beye == true)
                {
                    Report.EYE = "Both Eyes (OU)";
                }

                if (Reye == false && Leye == false && Beye == false)
                {
                    Report.EYE = "-- No Ocular -- ";
                }

                To.Mreportsdetailss.Add(Report);

            }

            To.Date = date.ToString("dd-MMM-yyyy");

            return To;
        }

        public Vreportss GetnewCumulativetotal(DateTime date, string Generalocular, string cmpid, string CMPbranch)
        {
            var To = new Vreportss();
            var CompanyID = 0;
            var cmp1 = CMPSContext.Company.Where(x => x.CompanyName == cmpid).Select(x => x.CmpID).FirstOrDefault();
            var orgcompanydet = CMPSContext.Company.Where(x => x.CmpID == cmp1 || x.ParentID == cmp1).ToList();

            foreach (var item in orgcompanydet)
            {
                if (item.Address1 == CMPbranch)
                {
                    CompanyID = item.CmpID;
                    To.COmpanyName = CMPSContext.Company.Where(x => x.CmpID == CompanyID).Select(x => x.CompanyName).FirstOrDefault();
                    break;
                }
                else
                {
                    continue;
                }

            }
            var reg = WYNKContext.Registration.Where(x => x.CMPID == CompanyID).ToList();
            
            var OLMid = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Review").Select(x => x.OLMID).FirstOrDefault();
            var nOLMid = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "New").Select(x => x.OLMID).FirstOrDefault();
            To.COmpanyName = CMPSContext.Company.Where(x => x.CmpID == CompanyID).Select(x => x.CompanyName).FirstOrDefault();
            var reportdetails = (from r in reg
                                 join rg in WYNKContext.RegistrationTran on r.UIN equals rg.UIN
                                 where (rg.DateofVisit.Date == date && rg.PatientVisitType == Convert.ToString(OLMid))
                                 select new
                                 {
                                     UIN = r.UIN,

                                 }).ToList();

            var sreportdetails = (from r in reg
                                  join rg in WYNKContext.RegistrationTran on r.UIN equals rg.UIN
                                  where (rg.DateofVisit.Date == date && rg.PatientVisitType == Convert.ToString(nOLMid))
                                  select new
                                  {
                                      UIN = r.UIN,

                                  }).ToList();


            var Mergeddata = reportdetails.Concat(sreportdetails);

            To.Mreportsdetailss = new List<Mreportsdetails>();


            foreach (var item in Mergeddata)
            {
                var Report = new Mreportsdetails();

                Report.UIN = item.UIN;
                var FName = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Name).FirstOrDefault();
                var MName = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.MiddleName).FirstOrDefault());
                var LName = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.LastName).FirstOrDefault();
                Report.Name = FName + MName + LName;
                Report.Age = PasswordEncodeandDecode.ToAgeString(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.DateofBirth).FirstOrDefault());
                Report.Gender = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Gender).FirstOrDefault();
                Report.Phone = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Phone).FirstOrDefault();
                var A1 = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Address1).FirstOrDefault());
                var A2 = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Address2).FirstOrDefault());
                var A3 = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Address3).FirstOrDefault());
                Report.Address = A1 + A2 + A3;
                Report.ConsultationAmount = WYNKContext.RegistrationTran.Where(x => x.UIN == item.UIN).Select(x => x.ConsulationFees).FirstOrDefault();
                Report.PatientType = "New Pediatric Ocular";
                var Reye = WYNKContext.RegistrationExtension.Where(x => x.UIN == item.UIN).Select(x => x.OD).FirstOrDefault();
                var Leye = WYNKContext.RegistrationExtension.Where(x => x.UIN == item.UIN).Select(x => x.OS).FirstOrDefault();
                var Beye = WYNKContext.RegistrationExtension.Where(x => x.UIN == item.UIN).Select(x => x.OU).FirstOrDefault();

                if (Reye == true)
                {
                    Report.EYE = "Right Eye (OD)";
                }

                if (Leye == true)
                {
                    Report.EYE = "Left Eye (OS)";
                }

                if (Beye == true)
                {
                    Report.EYE = "Both Eyes (OU)";
                }

                if (Reye == false && Leye == false && Beye == false)
                {
                    Report.EYE = "-- No Ocular -- ";
                }

                To.Mreportsdetailss.Add(Report);

            }

            To.Date = date.ToString("dd-MMM-yyyy");

            return To;
        }


        public string GetPediatric(DateTime Name, int CompanyID)
        {
            var PodiatristAge = CMPSContext.Setup.Where(x => x.CMPID == CompanyID).Select(x => x.Pediatric).FirstOrDefault();
            var Age = PasswordEncodeandDecode.ToAgeString(Name);
            var result = Regex.Match(Age, @"\d+").Value;
            var S = " ";
            var G = Name;
            if (Convert.ToInt32(result) >= Convert.ToInt32(PodiatristAge))
            {
                S = Convert.ToString(Name);
            }

            return S;
        }
        public string Getdb(DateTime Name, int CompanyID)
        {
            var PodiatristAge = CMPSContext.Setup.Where(x => x.CMPID == CompanyID).Select(x => x.Pediatric).FirstOrDefault();
            var Age = PasswordEncodeandDecode.ToAgeString(Name);
            var result = Regex.Match(Age, @"\d+").Value;
            var S = " ";
            var G = Name;
            if (Convert.ToInt32(result) >= Convert.ToInt32(PodiatristAge))
            {
                S = Convert.ToString(Name);
            }

            return S;
        }
        public string GetPdb(DateTime Name, int CompanyID)
        {
            var PodiatristAge = CMPSContext.Setup.Where(x => x.CMPID == CompanyID).Select(x => x.Pediatric).FirstOrDefault();
            var Age = PasswordEncodeandDecode.ToAgeString(Name);
            var result = Regex.Match(Age, @"\d+").Value;
            var S = " ";
            var G = Name;
            if (Convert.ToInt32(result) <= Convert.ToInt32(PodiatristAge))
            {
                S = Convert.ToString(Name);
            }

            return S;
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




        public Vreportss Getnewvisits(string date, string Newcount, int CompanyID)
        {
            var reg = WYNKContext.Registration.ToList();

            var Fulldate = Convert.ToDateTime(date);

            var regisrtattran = WYNKContext.RegistrationTran.ToList();
            var Online = CMPSContext.OneLineMaster.ToList();
            var To = new Vreportss();
            To.Mreportsdetailss = new List<Mreportsdetails>();
            var OLMID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "New").Select(x => x.OLMID).FirstOrDefault();

            var Listdetails = (from s in reg.Where(x => x.CMPID == CompanyID)
                               join d in regisrtattran.Where(x => x.DateofVisit.Date == Fulldate.Date && x.PatientVisitType == Convert.ToString(OLMID)) on s.UIN equals d.UIN

                               select new
                               {
                                   UIN = s.UIN,
                                   Name = s.Name + ' ' + s.MiddleName + ' ' + s.LastName,
                                   Age = PasswordEncodeandDecode.ToAgeString(s.DateofBirth),
                                   Genderss = s.Gender,
                                   address = s.Address1 + ' ' + s.Address2 + ' ' + s.Address3,
                                   phone = s.Phone,
                               }).ToList();

            foreach (var item in Listdetails)
            {
                var Newdata = new Mreportsdetails();
                Newdata.UIN = item.UIN;
                Newdata.Name = item.Name;
                Newdata.Age = item.Age;
                Newdata.Gender = item.Genderss;
                Newdata.Phone = item.phone;
                Newdata.Address = item.address;
                string Agenumeric = new String(item.Age.Where(Char.IsDigit).ToArray());
                if (Convert.ToInt32(Agenumeric) <= 18)
                {
                    Newdata.PatientType = "Pediatric";
                }
                else
                {
                    Newdata.PatientType = "General";
                }

                To.Mreportsdetailss.Add(Newdata);

            }
            To.Date = date;


            return To;
        }

        public Vreportss Getreviewvisits(string date, string Newcount, int CompanyID)
        {
            var reg = WYNKContext.Registration.ToList();

            var Fulldate = Convert.ToDateTime(date);

            var regisrtattran = WYNKContext.RegistrationTran.ToList();
            var Online = CMPSContext.OneLineMaster.ToList();
            var To = new Vreportss();
            To.Mreportsdetailss = new List<Mreportsdetails>();
            var OLMID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Review").Select(x => x.OLMID).FirstOrDefault();

            var Listdetails = (from s in reg.Where(x => x.CMPID == CompanyID)
                               join d in regisrtattran.Where(x => x.DateofVisit.Date == Fulldate.Date && x.PatientVisitType == Convert.ToString(OLMID)) on s.UIN equals d.UIN

                               select new
                               {
                                   UIN = s.UIN,
                                   Name = s.Name + ' ' + s.MiddleName + ' ' + s.LastName,
                                   Age = PasswordEncodeandDecode.ToAgeString(s.DateofBirth),
                                   Genderss = s.Gender,
                                   address = s.Address1 + ' ' + s.Address2 + ' ' + s.Address3,
                                   phone = s.Phone,
                               }).ToList();

            foreach (var item in Listdetails)
            {
                var Newdata = new Mreportsdetails();
                Newdata.UIN = item.UIN;
                Newdata.Name = item.Name;
                Newdata.Age = item.Age;
                Newdata.Gender = item.Genderss;
                Newdata.Phone = item.phone;
                Newdata.Address = item.address;
                string Agenumeric = new String(item.Age.Where(Char.IsDigit).ToArray());
                if (Convert.ToInt32(Agenumeric) <= 18)
                {
                    Newdata.PatientType = "Pediatric";
                }
                else
                {
                    Newdata.PatientType = "General";
                }

                To.Mreportsdetailss.Add(Newdata);

            }
            To.Date = date;


            return To;
        }

        public Vreportss Getsurgreviewvisits(string date, string Newcount, int CompanyID)
        {
            var reg = WYNKContext.Registration.ToList();

            var Fulldate = Convert.ToDateTime(date);

            var regisrtattran = WYNKContext.RegistrationTran.ToList();
            var Online = CMPSContext.OneLineMaster.ToList();
            var To = new Vreportss();
            To.Mreportsdetailss = new List<Mreportsdetails>();
            var OLMID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Review").Select(x => x.OLMID).FirstOrDefault();

            var Listdetails = (from s in reg.Where(x => x.CMPID == CompanyID)
                               join ss in WYNKContext.Admission.Where(x => x.IsSurgeryCompleted == true && x.AdmDate.Date == Fulldate.Date) on s.UIN equals ss.UIN
                               select new
                               {
                                   UIN = s.UIN,
                                   Name = s.Name + ' ' + s.MiddleName + ' ' + s.LastName,
                                   Age = PasswordEncodeandDecode.ToAgeString(s.DateofBirth),
                                   Genderss = s.Gender,
                                   address = s.Address1 + ' ' + s.Address2 + ' ' + s.Address3,
                                   phone = s.Phone,
                               }).ToList();

            foreach (var item in Listdetails)
            {
                var Newdata = new Mreportsdetails();
                Newdata.UIN = item.UIN;
                Newdata.Name = item.Name;
                Newdata.Age = item.Age;
                Newdata.Gender = item.Genderss;
                Newdata.Phone = item.phone;
                Newdata.Address = item.address;
                string Agenumeric = new String(item.Age.Where(Char.IsDigit).ToArray());
                if (Convert.ToInt32(Agenumeric) <= 18)
                {
                    Newdata.PatientType = "Pediatric";
                }
                else
                {
                    Newdata.PatientType = "General";
                }

                To.Mreportsdetailss.Add(Newdata);

            }
            To.Date = date;


            return To;
        }


        public Vreportss GetBranchwisedetails(string date, int CompanyID)
        {
            var reg = WYNKContext.Registration.ToList();
            var regisrtattran = WYNKContext.RegistrationTran.ToList();
            var Online = CMPSContext.OneLineMaster.ToList();
            var To = new Vreportss();
            To.Mreportssbranch = new List<Mreportssbranch>();
            var cmp1 = CMPSContext.Company.Where(x => x.CmpID == CompanyID).ToList();
            var cmp2 = CMPSContext.Company.Where(x => x.ParentID == CompanyID).ToList();
            var mergedcompanies = cmp1.Concat(cmp2);

            var pdata = WYNKContext.PatientFootfall.OrderByDescending(x => x.Date).Where(x => x.Date.Date == Convert.ToDateTime(date).Date).ToList();

            var totaldata = (from b in pdata
                             where mergedcompanies.Select(x => x.CmpID).Contains(b.CmpID)
                             select b);

            To.Mreportssbranch = (from s in totaldata
                             group s by new { s.CmpID } into i
                             select new Mreportssbranch
                             {
                                 Companybranch = CMPSContext.Company.Where(x =>x.CmpID == i.Select(u =>u.CmpID).FirstOrDefault()).Select(x =>x.Address1).FirstOrDefault(),
                                 CompanyName = CMPSContext.Company.Where(x => x.CmpID == i.Select(u => u.CmpID).FirstOrDefault()).Select(x => x.CompanyName).FirstOrDefault(),
                                 Datess = i.Select(x => x.Date).FirstOrDefault(),
                                 NewPatients = i.Select(x => x.NewPatients).FirstOrDefault(),
                                 ReviewPatients = i.Select(x => x.ReviewPatients).FirstOrDefault(),
                                 Surgeryreviewpatients = 0,
                                 Cumulativetotal = i.Select(x => x.NewPatients).FirstOrDefault() + i.Select(x => x.ReviewPatients).FirstOrDefault() +0,

                                 RevenueNewPatients = i.Select(x => x.ConsultationNew).FirstOrDefault(),
                                 revenueReviewPatients = i.Select(x => x.ConsultationReview).FirstOrDefault(),
                                 RevenueCumulativetotal = i.Select(x => x.ConsultationNew).FirstOrDefault() + i.Select(x => x.ConsultationReview).FirstOrDefault(),

                                 NewGeneralNormal = i.Select(x => x.NewGeneralNormal).FirstOrDefault(),
                                 NewGeneralOcular = i.Select(x => x.NewGeneralOcular).FirstOrDefault(),
                                 NewPediatricNormal = i.Select(x => x.NewPediatricNormal).FirstOrDefault(),
                                 NewPediatricOcular = i.Select(x => x.NewPediatricOcular).FirstOrDefault(),

                                 ReviewGeneralNormal = i.Select(x => x.ReviewGeneralNormal).FirstOrDefault(),
                                 ReviewGeneralOcular = i.Select(x => x.ReviewGeneralOcular).FirstOrDefault(),
                                 ReviewPediatricNormal = i.Select(x => x.ReviewPediatricNormal).FirstOrDefault(),
                                 ReviewPediatricOcular = i.Select(x => x.ReviewPediatricOcular).FirstOrDefault(),

                                 SReviewGeneralNormal = 0,
                                 SReviewGeneralOcular = 0,
                                 SReviewPediatricNormal = 0,
                                 SReviewPediatricOcular = 0,

                             }).ToList();





            To.totalReveuewAmountwhole = To.Mreportssbranch.Sum(x => x.RevenueCumulativetotal);
            To.Patientcumulativetotal = To.Mreportssbranch.Sum(x => x.Cumulativetotal);
            To.NewReveuewAmountwhole = To.Mreportssbranch.Sum(x => x.RevenueNewPatients);
            To.RevenueReveuewAmountwhole = To.Mreportssbranch.Sum(x => x.revenueReviewPatients);


            To.NewGeneralNormal = To.Mreportssbranch.Sum(x => x.NewGeneralNormal);
            To.NewGeneralOcular = To.Mreportssbranch.Sum(x => x.NewGeneralOcular);
            To.NewPediatricNormal = To.Mreportssbranch.Sum(x => x.NewPediatricNormal);
            To.NewPediatricOcular = To.Mreportssbranch.Sum(x => x.NewPediatricOcular);

            To.ReviewGeneralNormal = To.Mreportssbranch.Sum(x => x.ReviewGeneralNormal);
            To.ReviewGeneralOcular = To.Mreportssbranch.Sum(x => x.ReviewGeneralOcular);
            To.ReviewPediatricNormal = To.Mreportssbranch.Sum(x => x.ReviewPediatricNormal);
            To.ReviewPediatricOcular = To.Mreportssbranch.Sum(x => x.ReviewPediatricOcular);

            To.SReviewGeneralNormal = To.Mreportssbranch.Sum(x => x.SReviewGeneralNormal);
            To.SReviewGeneralOcular = To.Mreportssbranch.Sum(x => x.SReviewGeneralOcular);
            To.SReviewPediatricNormal = To.Mreportssbranch.Sum(x => x.SReviewPediatricNormal);
            To.SReviewPediatricOcular = To.Mreportssbranch.Sum(x => x.SReviewPediatricOcular);

            To.TotalNew = To.Mreportssbranch.Sum(x => x.NewPatients);
            To.TotalReview = To.Mreportssbranch.Sum(x => x.ReviewPatients);
            To.TotalSurgery = To.Mreportssbranch.Sum(x => x.Surgeryreviewpatients);

            To.FromdateDate =date;
            return To;
        }



    }
}




















