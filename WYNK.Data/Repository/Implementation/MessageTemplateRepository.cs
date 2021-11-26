using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using RestSharp;
using CommonMailService.cs;
using System;
using System.Collections.Generic;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Helpers;

namespace WYNK.Data.Repository.Implementation
{
    class MessageTemplateRepository : RepositoryBase<MessagingTemplate>, IMessagingTemplateRepository
    {

        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        

        public MessageTemplateRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public dynamic InsertMessagingTemplate(MessagingTemplate MT)
        {
            var RuleMaster = new MessageTemplate();


            RuleMaster.CMPID = Convert.ToInt32(MT.MsgTemplateCMPID);
            RuleMaster.CreatedBy = Convert.ToInt32(MT.MsgTemplateCreatedby);
            RuleMaster.CreatedUTC = DateTime.UtcNow;
            RuleMaster.EMAILMsgDescription = MT.MsgTemplateMAILDESCR;
            RuleMaster.MsgTemplateName = MT.MsgTemplateName;
            RuleMaster.WHATSAPPMsgDescription = MT.MsgTemplateWHATSAPPDESCRIP;
            RuleMaster.SMSMsgDescription = MT.MsgTemplateSMSDESC;
            RuleMaster.EMAILSubject = MT.MsgTemplateSUBJECT;

            var IDD = CMPSContext.MessagingTemplate.OrderByDescending(x => x.ID).Select(x => x.ID).FirstOrDefault();
            if (IDD != 0)
            {
                RuleMaster.ID = IDD + 1;
            }
            else
            {
                RuleMaster.ID = 1;
            }
            CMPSContext.MessagingTemplate.Add(RuleMaster);
            CMPSContext.SaveChanges();

            try
            {
                if (CMPSContext.SaveChanges() >= 0)
                {
                    return new
                    {
                        Success = true,
                    };
                }

            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Success = false,
                Message = "Some data are Missing"
            };
        }


        public MessagingTemplate GetDBColumns(string Statuscolumn)
        {
            var MMT = new MessagingTemplate();


            if (Statuscolumn == "Appointment Confirmation" || Statuscolumn == "Appointment Cancelled" || Statuscolumn == "Appointment Re-Scheduled")
            {
                var names = typeof(Appointment).GetProperties()
          .Select(property => property.Name)
          .ToArray();

                var Seondnames = typeof(Appointmenttran).GetProperties()
          .Select(property => property.Name)
          .ToArray();



                var MMlist = new List<GetColumns>();


                foreach (var item in names)
                {
                    var MMtNew = new GetColumns();
                    MMtNew.ColumnDescription = item;

                    MMlist.Add(MMtNew);

                }

                foreach (var item in Seondnames)
                {
                    var MMtNew = new GetColumns();
                    MMtNew.ColumnDescription = item;

                    MMlist.Add(MMtNew);

                }

                MMT.GetColumns = MMlist;
            }
            else if (Statuscolumn == "New Patients" || Statuscolumn == "Review Patients" || Statuscolumn == "Surgery-Review Patients")
            {
                var names = typeof(Registration_Master).GetProperties()
          .Select(property => property.Name)
          .ToArray();

                var MMlist = new List<GetColumns>();


                foreach (var item in names)
                {
                    var MMtNew = new GetColumns();
                    MMtNew.ColumnDescription = item;

                    MMlist.Add(MMtNew);

                }

                MMT.GetColumns = MMlist;
            }




            return MMT;
        }


        public dynamic GetAadhaarOTP(string Aadhaatrnumber)
        {
            var MT = new MessagingTemplate();


            var client = new RestClient("https://sandbox.aadhaarapi.io/api/v1/aadhaar-v2/generate-otp");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
      
            request.AddParameter("application/json", "{\n\t\"id_number\": \"" + Aadhaatrnumber + "\"\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            var gg = response.Content;

            if (gg != "")
            {
                string source = gg;
                dynamic datas = JObject.Parse(source);
                var ffs = datas.data.client_id;

                MT.GETDET = ffs;
            }
            return MT;
        }



        public dynamic SubmitAadhaarOTP(string OTP, string ClientID)
        {
            var MT = new MessagingTemplate();

            var OOtp = Convert.ToInt32(OTP);

            var client = new RestClient("https://sandbox.aadhaarapi.io/api/v1/aadhaar-v2/submit-otp");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJqdGkiOiJkNTU1NzNhMS04MzMyLTRkMDgtYWY2NC1kN2JjMWQ1MDNkMWUiLCJmcmVzaCI6ZmFsc2UsImlhdCI6MTU5MTI2NDU2NCwidXNlcl9jbGFpbXMiOnsic2NvcGVzIjpbInJlYWQiXX0sImV4cCI6MTU5MjU2MDU2NCwibmJmIjoxNTkxMjY0NTY0LCJpZGVudGl0eSI6ImRldi5kaW5ha2FyQGFhZGhhYXJhcGkuaW8iLCJ0eXBlIjoiYWNjZXNzIn0.bC7w03hi4O4UPhuRhbqdSYFj-sCN3h7dzmEbXnrIxq8");
            request.AddParameter("application/json", "{\n\t\"client_id\": \"" + ClientID + "\",\n\t\"otp\": \"" + OOtp + "\"\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);

            var gg = response.Content;

            if (gg != "")
            {

                string source = gg;
                dynamic datas = JObject.Parse(source);
                var fnames = datas.data.full_name;
                var genders = datas.data.gender;
                var dobs = datas.data.dob;
                var profileimages = datas.data.profile_image;
                var addressss = datas.data.address;
                MT.FULLNAME = fnames;
                MT.GENDER = genders;
                MT.DOB = dobs;
                MT.Address = datas.data.address.house + " , " + datas.data.address.loc;
                MT.PRofioleimage = profileimages;
                MT.COUNYTRY = datas.data.address.country;
                MT.dist = datas.data.address.dist;
                MT.state = datas.data.address.state;
                MT.city = datas.data.address.po;

            }




            return MT;
        }



        //////////////////////////pan///////////////
        ///
        public dynamic Panverification(string pan, int amount)
        {
            var MT = new MessagingTemplate();

            var cstoptions = new Stripe.CustomerCreateOptions
            {
                Description = "First Customer",
                Name = "Jenny Rosen",
                Address = new Stripe.AddressOptions
                {
                    Line1 = "510 Townsend St",
                    PostalCode = "98140",
                    City = "San Francisco",
                    State = "CA",
                    Country = "US",
                },
                Email = "Jacob@gmail.com",
                Source = pan,
            };
            var service = new Stripe.CustomerService();
         Stripe.Customer stripecust =  service.Create(cstoptions);

            double amountt = Convert.ToDouble(amount);
            var chanrgeoptions = new Stripe.ChargeCreateOptions
            {

                Amount = amount*100,
                Currency = "USD",
                ReceiptEmail = "Jacob@gmail.com",
                Customer = stripecust.Id,
                Description = "txn_1HezIcJAJfZb9HEBRULLJiOg", //Optional  
            };            
            var chargeservice = new Stripe.ChargeService();
            Stripe.Charge charge = chargeservice.Create(chanrgeoptions);

            return MT;
        }


        public MessagingTemplate Gettemplates(int cmpid)
        {
            var MMT = new MessagingTemplate();
            MMT.GetColumns = (from c in CMPSContext.MessagingTemplate.Where(x => x.CMPID == cmpid && x.MsgTemplateName == "New Patients")
                              select new GetColumns
                              {
                                  ID = c.ID,
                                  ColumnDescription = c.SMSMsgDescription,
                                  mailColumnDescription = c.EMAILMsgDescription,
                                  whatsappColumnDescription = c.WHATSAPPMsgDescription,
                              }).ToList();

            return MMT;
        }

        public MessagingTemplate Getformattemplates(int cmpid, string format)
        {
            var MMT = new MessagingTemplate();
            if (format == "All")
            {



                MMT.GetColumns = (from c in CMPSContext.MessagingTemplate.Where(x => x.CMPID == cmpid)
                                  select new GetColumns
                                  {
                                      ID = c.ID,
                                      ColumnDescription = c.SMSMsgDescription,
                                      mailColumnDescription = c.EMAILMsgDescription,
                                      whatsappColumnDescription = c.WHATSAPPMsgDescription,
                                      selected = false
                                  }).ToList();
            }
            else if (format == "New")
            {

                MMT.GetColumns = (from c in CMPSContext.MessagingTemplate.Where(x => x.CMPID == cmpid && x.MsgTemplateName == "New Patients")
                                  select new GetColumns
                                  {
                                      ID = c.ID,
                                      ColumnDescription = c.SMSMsgDescription,
                                      mailColumnDescription = c.EMAILMsgDescription,
                                      whatsappColumnDescription = c.WHATSAPPMsgDescription,
                                      selected = false
                                  }).ToList();
            }
            else if (format == "Review")
            {

                MMT.GetColumns = (from c in CMPSContext.MessagingTemplate.Where(x => x.CMPID == cmpid && x.MsgTemplateName == "Review Patients")
                                  select new GetColumns
                                  {
                                      ID = c.ID,
                                      ColumnDescription = c.SMSMsgDescription,
                                      mailColumnDescription = c.EMAILMsgDescription,
                                      whatsappColumnDescription = c.WHATSAPPMsgDescription,
                                      selected = false
                                  }).ToList();
            }
            else if (format == "Surgery")
            {

                MMT.GetColumns = (from c in CMPSContext.MessagingTemplate.Where(x => x.CMPID == cmpid && x.MsgTemplateName == "Surgery-Review Patients")
                                  select new GetColumns
                                  {
                                      ID = c.ID,
                                      ColumnDescription = c.SMSMsgDescription,
                                      mailColumnDescription = c.EMAILMsgDescription,
                                      whatsappColumnDescription = c.WHATSAPPMsgDescription,
                                      selected = false
                                  }).ToList();
            }



            return MMT;
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
        public MessagingTemplate Getperioddetails(int cmpid, string periodtype, string visisttype)
        {
            var MMT = new MessagingTemplate();
            MMT.Getpdetailss = new List<Getpdetails>();
            var newvi = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "New").Select(x => x.OLMID).FirstOrDefault();
            var Renew = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Review").Select(x => x.OLMID).FirstOrDefault();
            var closed = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Closed").Select(x => x.OLMID).FirstOrDefault();
            if (visisttype == "New")
            {

                if (periodtype == "week")
                {
                    var adddays = DateTime.Now.Month;
                    var tilldate = adddays;
                    MMT.Getpdetailss = (from c in WYNKContext.Registration
                                        join r in WYNKContext.RegistrationTran on c.UIN equals r.UIN
                                        where c.CMPID == cmpid && r.DateofVisit.Month == tilldate && r.PatientVisitType == Convert.ToString(newvi)
                                        group c by new { c.Name, c.Phone } into i
                                        select new Getpdetails
                                        {
                                            UIN = i.Select(x => x.UIN).FirstOrDefault(),
                                            fname = GetConcatName(i.Select(x => x.Name).FirstOrDefault()),
                                            mname = GetConcatName(i.Select(x => x.MiddleName).FirstOrDefault()),
                                            lname = GetConcatName(i.Select(x => x.LastName).FirstOrDefault()),
                                            emails = i.Select(x => x.EmailID).FirstOrDefault(),
                                            phonenumber = i.Select(x => x.Phone).FirstOrDefault(),
                                            selected = false,
                                        }).ToList();
                }
                else if (periodtype == "month")
                {
                    var adddays = DateTime.Now.AddDays(-30);
                    var tilldate = adddays;
                    MMT.Getpdetailss = (from c in WYNKContext.Registration
                                        join r in WYNKContext.RegistrationTran on c.UIN equals r.UIN
                                        where c.CMPID == cmpid && r.DateofVisit.Date >= tilldate.Date && r.DateofVisit.Date <= DateTime.Now.Date && r.PatientVisitType == Convert.ToString(newvi)
                                        group c by new { c.Name, c.Phone } into i
                                        select new Getpdetails
                                        {
                                            fname = GetConcatName(i.Select(x => x.Name).FirstOrDefault()),
                                            mname = GetConcatName(i.Select(x => x.MiddleName).FirstOrDefault()),
                                            lname = GetConcatName(i.Select(x => x.LastName).FirstOrDefault()),
                                            emails = i.Select(x => x.EmailID).FirstOrDefault(),
                                            phonenumber = i.Select(x => x.Phone).FirstOrDefault(),
                                            selected = false,
                                            UIN = i.Select(x => x.UIN).FirstOrDefault(),
                                        }).ToList();
                }
                else if (periodtype == "quarter")
                {
                    var adddays = DateTime.Now.AddDays(-90);
                    var tilldate = adddays;
                    MMT.Getpdetailss = (from c in WYNKContext.Registration
                                        join r in WYNKContext.RegistrationTran on c.UIN equals r.UIN
                                        where c.CMPID == cmpid && r.DateofVisit.Date >= tilldate.Date && r.DateofVisit.Date <= DateTime.Now.Date && r.PatientVisitType == Convert.ToString(newvi)
                                        group c by new { c.Name, c.Phone } into i
                                        select new Getpdetails
                                        {
                                            fname = GetConcatName(i.Select(x => x.Name).FirstOrDefault()),
                                            mname = GetConcatName(i.Select(x => x.MiddleName).FirstOrDefault()),
                                            lname = GetConcatName(i.Select(x => x.LastName).FirstOrDefault()),
                                            emails = i.Select(x => x.EmailID).FirstOrDefault(),
                                            phonenumber = i.Select(x => x.Phone).FirstOrDefault(),
                                            selected = false,
                                            UIN = i.Select(x => x.UIN).FirstOrDefault(),
                                        }).ToList();
                }
            }
            else if (visisttype == "Review")
            {

                if (periodtype == "week")
                {
                    var adddays = DateTime.Now.Month;
                    var tilldate = adddays;
                    MMT.Getpdetailss = (from c in WYNKContext.Registration
                                        join r in WYNKContext.RegistrationTran on c.UIN equals r.UIN
                                        where c.CMPID == cmpid && r.DateofVisit.Month == tilldate && r.PatientVisitType == Convert.ToString(Renew)
                                        group c by new { c.Name, c.Phone } into i
                                        select new Getpdetails
                                        {
                                            fname = GetConcatName(i.Select(x => x.Name).FirstOrDefault()),
                                            mname = GetConcatName(i.Select(x => x.MiddleName).FirstOrDefault()),
                                            lname = GetConcatName(i.Select(x => x.LastName).FirstOrDefault()),
                                            emails = i.Select(x => x.EmailID).FirstOrDefault(),
                                            phonenumber = i.Select(x => x.Phone).FirstOrDefault(),
                                            selected = false,
                                            UIN = i.Select(x => x.UIN).FirstOrDefault(),
                                        }).ToList();
                }
                else if (periodtype == "month")
                {
                    var adddays = DateTime.Now.AddDays(-30);
                    var tilldate = adddays;
                    MMT.Getpdetailss = (from c in WYNKContext.Registration
                                        join r in WYNKContext.RegistrationTran on c.UIN equals r.UIN
                                        where c.CMPID == cmpid && r.DateofVisit.Date >= tilldate.Date && r.DateofVisit.Date <= DateTime.Now.Date && r.PatientVisitType == Convert.ToString(Renew)
                                        group c by new { c.Name, c.Phone } into i
                                        select new Getpdetails
                                        {
                                            fname = GetConcatName(i.Select(x => x.Name).FirstOrDefault()),
                                            mname = GetConcatName(i.Select(x => x.MiddleName).FirstOrDefault()),
                                            lname = GetConcatName(i.Select(x => x.LastName).FirstOrDefault()),
                                            emails = i.Select(x => x.EmailID).FirstOrDefault(),
                                            phonenumber = i.Select(x => x.Phone).FirstOrDefault(),
                                            selected = false,
                                            UIN = i.Select(x => x.UIN).FirstOrDefault(),
                                        }).ToList();
                }
                else if (periodtype == "quarter")
                {
                    var adddays = DateTime.Now.AddDays(-90);
                    var tilldate = adddays;
                    MMT.Getpdetailss = (from c in WYNKContext.Registration
                                        join r in WYNKContext.RegistrationTran on c.UIN equals r.UIN
                                        where c.CMPID == cmpid && r.DateofVisit.Date >= tilldate.Date && r.DateofVisit.Date <= DateTime.Now.Date && r.PatientVisitType == Convert.ToString(Renew)
                                        group c by new { c.Name, c.Phone } into i
                                        select new Getpdetails
                                        {
                                            fname = GetConcatName(i.Select(x => x.Name).FirstOrDefault()),
                                            mname = GetConcatName(i.Select(x => x.MiddleName).FirstOrDefault()),
                                            lname = GetConcatName(i.Select(x => x.LastName).FirstOrDefault()),
                                            emails = i.Select(x => x.EmailID).FirstOrDefault(),
                                            phonenumber = i.Select(x => x.Phone).FirstOrDefault(),
                                            selected = false,
                                            UIN = i.Select(x => x.UIN).FirstOrDefault(),
                                        }).ToList();
                }
            }
            else if (visisttype == "Surgery")
            {

                if (periodtype == "week")
                {
                    var adddays = DateTime.Now.Month;
                   
                    MMT.Getpdetailss = (from Fext in WYNKContext.FindingsExt
                                        join F in WYNKContext.Findings.Where(x => x.CmpID == cmpid) on Fext.FindingsID equals F.RandomUniqueID
                                        join RegTran in WYNKContext.RegistrationTran on F.UIN equals RegTran.UIN
                                        join c in WYNKContext.Registration.Where(x => x.CMPID == cmpid) on F.UIN equals c.UIN
                                        where Fext.Isdeleted == false && RegTran.PatientVisitType == Convert.ToString(Renew)
                                        && RegTran.Status != closed && RegTran.DateofVisit.Month == adddays
                                        orderby c.DateofRegistration descending
                                        group c by new { c.Name, c.Phone } into i
                                        select new Getpdetails
                                        {
                                            fname = GetConcatName(i.Select(x => x.Name).FirstOrDefault()),
                                            mname = GetConcatName(i.Select(x => x.MiddleName).FirstOrDefault()),
                                            lname = GetConcatName(i.Select(x => x.LastName).FirstOrDefault()),
                                            emails = i.Select(x => x.EmailID).FirstOrDefault(),
                                            phonenumber = i.Select(x => x.Phone).FirstOrDefault(),
                                            selected = false,
                                            UIN = i.Select(x => x.UIN).FirstOrDefault(),
                                        }).ToList();
                }
                else if (periodtype == "month")
                {
                    var adddays = DateTime.Now.AddDays(-30);
                    var tilldate = adddays;
                    MMT.Getpdetailss = (from Fext in WYNKContext.FindingsExt
                                        join F in WYNKContext.Findings.Where(x => x.CmpID == cmpid) on Fext.FindingsID equals F.RandomUniqueID
                                        join RegTran in WYNKContext.RegistrationTran on F.UIN equals RegTran.UIN
                                        join c in WYNKContext.Registration.Where(x => x.CMPID == cmpid) on F.UIN equals c.UIN
                                        where Fext.Isdeleted == false && RegTran.PatientVisitType == Convert.ToString(Renew)
                                        && RegTran.Status != closed && RegTran.DateofVisit.Date >= tilldate.Date && RegTran.DateofVisit.Date <= DateTime.Now.Date
                                        orderby c.DateofRegistration descending
                                        group c by new { c.Name, c.Phone } into i
                                        select new Getpdetails
                                        {
                                            fname = GetConcatName(i.Select(x => x.Name).FirstOrDefault()),
                                            mname = GetConcatName(i.Select(x => x.MiddleName).FirstOrDefault()),
                                            lname = GetConcatName(i.Select(x => x.LastName).FirstOrDefault()),
                                            emails = i.Select(x => x.EmailID).FirstOrDefault(),
                                            phonenumber = i.Select(x => x.Phone).FirstOrDefault(),
                                            selected = false,
                                            UIN = i.Select(x => x.UIN).FirstOrDefault(),
                                        }).ToList();
                }
                else if (periodtype == "quarter")
                {
                    var adddays = DateTime.Now.AddDays(-90);
                    var tilldate = adddays;
                    MMT.Getpdetailss = (from Fext in WYNKContext.FindingsExt
                                        join F in WYNKContext.Findings.Where(x => x.CmpID == cmpid) on Fext.FindingsID equals F.RandomUniqueID
                                        join RegTran in WYNKContext.RegistrationTran on F.UIN equals RegTran.UIN
                                        join c in WYNKContext.Registration.Where(x => x.CMPID == cmpid) on F.UIN equals c.UIN
                                        where Fext.Isdeleted == false && RegTran.PatientVisitType == Convert.ToString(Renew)
                                        && RegTran.Status != closed && RegTran.DateofVisit.Date >= tilldate.Date && RegTran.DateofVisit.Date <= DateTime.Now.Date
                                        orderby c.DateofRegistration descending
                                        group c by new { c.Name, c.Phone } into i
                                        select new Getpdetails
                                        {
                                            fname = GetConcatName(i.Select(x => x.Name).FirstOrDefault()),
                                            mname = GetConcatName(i.Select(x => x.MiddleName).FirstOrDefault()),
                                            lname = GetConcatName(i.Select(x => x.LastName).FirstOrDefault()),
                                            emails = i.Select(x => x.EmailID).FirstOrDefault(),
                                            phonenumber = i.Select(x => x.Phone).FirstOrDefault(),
                                            selected = false,
                                            UIN = i.Select(x => x.UIN).FirstOrDefault(),
                                        }).ToList();
                }
            }

            else if (visisttype == "Pediatric")
            {
                var Peduatrciage = CMPSContext.Setup.Where(x => x.CMPID == cmpid).Select(x => x.Pediatric).FirstOrDefault();

                if (periodtype == "week")
                {
                    var adddays = DateTime.Now.Month;
                    var tilldate = adddays;
                    var dddays = DateTime.Now.AddYears(- Convert.ToInt32(Peduatrciage));
                    MMT.Getpdetailss = (from c in WYNKContext.Registration
                                        join r in WYNKContext.RegistrationTran on c.UIN equals r.UIN
                                        where c.CMPID == cmpid && c.DateofBirth.Date >= dddays.Date && c.DateofBirth.Date <= DateTime.Now.Date
                                        && r.DateofVisit.Month == tilldate
                                        group c by new { c.Name, c.Phone } into i
                                        select new Getpdetails
                                        {
                                            UIN = i.Select(x => x.UIN).FirstOrDefault(),
                                            fname = GetConcatName(i.Select(x => x.Name).FirstOrDefault()),
                                            mname = GetConcatName(i.Select(x => x.MiddleName).FirstOrDefault()),
                                            lname = GetConcatName(i.Select(x => x.LastName).FirstOrDefault()),
                                            emails = i.Select(x => x.EmailID).FirstOrDefault(),
                                            phonenumber = i.Select(x => x.Phone).FirstOrDefault(),
                                            selected = false,
                                        }).ToList();
                }
                else if (periodtype == "month")
                {
                    var adddays = DateTime.Now.AddDays(-30);
                    var tilldate = adddays;
                 
                    var dddays = DateTime.Now.AddYears(-Convert.ToInt32(Peduatrciage));
                    MMT.Getpdetailss = (from c in WYNKContext.Registration
                                        join r in WYNKContext.RegistrationTran on c.UIN equals r.UIN
                                        where c.CMPID == cmpid && c.DateofBirth.Date >= dddays.Date && c.DateofBirth.Date <= DateTime.Now.Date
                                       && r.DateofVisit.Date >= tilldate.Date && r.DateofVisit.Date <= DateTime.Now.Date
                                        group c by new { c.Name, c.Phone } into i
                                        select new Getpdetails
                                        {
                                            UIN = i.Select(x => x.UIN).FirstOrDefault(),
                                            fname = GetConcatName(i.Select(x => x.Name).FirstOrDefault()),
                                            mname = GetConcatName(i.Select(x => x.MiddleName).FirstOrDefault()),
                                            lname = GetConcatName(i.Select(x => x.LastName).FirstOrDefault()),
                                            emails = i.Select(x => x.EmailID).FirstOrDefault(),
                                            phonenumber = i.Select(x => x.Phone).FirstOrDefault(),
                                            selected = false,
                                        }).ToList();
                }
                else if (periodtype == "quarter")
                {
                    var adddays = DateTime.Now.AddDays(-90);
                    var tilldate = adddays;
                  
                    var dddays = DateTime.Now.AddYears(-Convert.ToInt32(Peduatrciage));
                    MMT.Getpdetailss = (from c in WYNKContext.Registration
                                        join r in WYNKContext.RegistrationTran on c.UIN equals r.UIN
                                        where c.CMPID == cmpid && c.DateofBirth.Date >= dddays.Date && c.DateofBirth.Date <= DateTime.Now.Date
                                       && r.DateofVisit.Date >= tilldate.Date && r.DateofVisit.Date <= DateTime.Now.Date
                                        group c by new { c.Name, c.Phone } into i
                                        select new Getpdetails
                                        {
                                            UIN = i.Select(x => x.UIN).FirstOrDefault(),
                                            fname = GetConcatName(i.Select(x => x.Name).FirstOrDefault()),
                                            mname = GetConcatName(i.Select(x => x.MiddleName).FirstOrDefault()),
                                            lname = GetConcatName(i.Select(x => x.LastName).FirstOrDefault()),
                                            emails = i.Select(x => x.EmailID).FirstOrDefault(),
                                            phonenumber = i.Select(x => x.Phone).FirstOrDefault(),
                                            selected = false,
                                        }).ToList();
                }
            }

            return MMT;
        }

        public MessagingTemplate Getspecificperioddetails(int cmpid, string fromdate, string todate, string visisttype)
        {
            var MMT = new MessagingTemplate();
            MMT.Getpdetailss = new List<Getpdetails>();
            var newvi = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "New").Select(x => x.OLMID).FirstOrDefault();
            var Renew = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Review").Select(x => x.OLMID).FirstOrDefault();
            var closed = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Closed").Select(x => x.OLMID).FirstOrDefault();

            if (visisttype == "New")
            {
                var fadte = Convert.ToDateTime(fromdate);
                var tadte = Convert.ToDateTime(fromdate);

                MMT.Getpdetailss = (from c in WYNKContext.Registration
                                    join r in WYNKContext.RegistrationTran on c.UIN equals r.UIN
                                    where c.CMPID == cmpid && r.DateofVisit.Date >= fadte.Date && r.DateofVisit.Date <= tadte.Date
                                    && r.PatientVisitType == Convert.ToString(newvi)
                                    group c by new { c.Name, c.Phone } into i
                                    select new Getpdetails
                                    {
                                        fname = GetConcatName(i.Select(x => x.Name).FirstOrDefault()),
                                        mname = GetConcatName(i.Select(x => x.MiddleName).FirstOrDefault()),
                                        lname = GetConcatName(i.Select(x => x.LastName).FirstOrDefault()),
                                        emails = i.Select(x => x.EmailID).FirstOrDefault(),
                                        phonenumber = i.Select(x => x.Phone).FirstOrDefault(),
                                        selected = false,
                                        UIN = i.Select(x => x.UIN).FirstOrDefault(),
                                    }).ToList();
            }
            else if (visisttype == "Review")
            {
                var fadte = Convert.ToDateTime(fromdate);
                var tadte = Convert.ToDateTime(fromdate);

                MMT.Getpdetailss = (from c in WYNKContext.Registration
                                    join r in WYNKContext.RegistrationTran on c.UIN equals r.UIN
                                    where c.CMPID == cmpid && r.DateofVisit.Date >= fadte.Date && r.DateofVisit.Date <= tadte.Date
                                    && r.PatientVisitType == Convert.ToString(Renew)
                                    group c by new { c.Name, c.Phone } into i
                                    select new Getpdetails
                                    {
                                        fname = GetConcatName(i.Select(x => x.Name).FirstOrDefault()),
                                        mname = GetConcatName(i.Select(x => x.MiddleName).FirstOrDefault()),
                                        lname = GetConcatName(i.Select(x => x.LastName).FirstOrDefault()),
                                        emails = i.Select(x => x.EmailID).FirstOrDefault(),
                                        phonenumber = i.Select(x => x.Phone).FirstOrDefault(),
                                        selected = false,
                                        UIN = i.Select(x => x.UIN).FirstOrDefault(),
                                    }).ToList();
            }
            else if (visisttype == "Surgery")
            {
                var fadte = Convert.ToDateTime(fromdate);
                var tadte = Convert.ToDateTime(fromdate);
                MMT.Getpdetailss = (from Fext in WYNKContext.FindingsExt
                                    join F in WYNKContext.Findings.Where(x => x.CmpID == cmpid) on Fext.FindingsID equals F.RandomUniqueID
                                    join RegTran in WYNKContext.RegistrationTran on F.UIN equals RegTran.UIN
                                    join c in WYNKContext.Registration.Where(x => x.CMPID == cmpid) on F.UIN equals c.UIN
                                    where Fext.Isdeleted == false && RegTran.PatientVisitType == Convert.ToString(Renew)
                                    && RegTran.Status != closed && RegTran.DateofVisit.Date >= fadte.Date && RegTran.DateofVisit.Date <= tadte.Date
                                    orderby c.DateofRegistration descending
                                    group c by new { c.Name, c.Phone } into i
                                    select new Getpdetails
                                    {
                                        fname = GetConcatName(i.Select(x => x.Name).FirstOrDefault()),
                                        mname = GetConcatName(i.Select(x => x.MiddleName).FirstOrDefault()),
                                        lname = GetConcatName(i.Select(x => x.LastName).FirstOrDefault()),
                                        emails = i.Select(x => x.EmailID).FirstOrDefault(),
                                        phonenumber = i.Select(x => x.Phone).FirstOrDefault(),
                                        selected = false,
                                        UIN = i.Select(x => x.UIN).FirstOrDefault(),
                                    }).ToList();
            }

            return MMT;
        }



        public MessagingTemplate Updatesmstemplate(int cmpid, string Desc, string id)
        {
            var MMT = new MessagingTemplate();
            var defaultid = CMPSContext.MessagingTemplate.Where(x => x.ID == Convert.ToInt32(id) && x.CMPID == cmpid).FirstOrDefault();
            defaultid.SMSMsgDescription = Desc;
            CMPSContext.Entry(defaultid).State = EntityState.Modified;
            CMPSContext.SaveChanges();
            MMT.status = "TRUE";

            return MMT;
        }

        public MessagingTemplate Updatemailtemplate(int cmpid, string Desc, string id)
        {
            var MMT = new MessagingTemplate();
            var defaultid = CMPSContext.MessagingTemplate.Where(x => x.ID == Convert.ToInt32(id) && x.CMPID == cmpid).FirstOrDefault();
            defaultid.EMAILMsgDescription = Desc;
            CMPSContext.Entry(defaultid).State = EntityState.Modified;
            CMPSContext.SaveChanges();
            MMT.status = "TRUE";

            return MMT;
        }

        public MessagingTemplate Updatewhatsapptemplate(int cmpid, string Desc, string id)
        {
            var MMT = new MessagingTemplate();
            var defaultid = CMPSContext.MessagingTemplate.Where(x => x.ID == Convert.ToInt32(id) && x.CMPID == cmpid).FirstOrDefault();
            defaultid.WHATSAPPMsgDescription = Desc;
            CMPSContext.Entry(defaultid).State = EntityState.Modified;
            CMPSContext.SaveChanges();
            MMT.status = "TRUE";

            return MMT;
        }





        public dynamic InsertBulsmstemplateTemplate(MessagingTemplate MT)
        {

            var Patientlists = MT.ParientChecklistdetailsss;
            var SMSPatientlists = MT.PatientChecklistDetailssssssss.Select(x =>x.ItemDescription).FirstOrDefault();
            var EMALPatientlists = MT.PatientChecklistDetailssssssssmail.Select(x => x.ItemDescription).FirstOrDefault();
            var WHATSAPPPatientlists = MT.PatientChecklistDetailsssssssswhatsapp.Select(x => x.ItemDescription).FirstOrDefault();

            foreach(var item in Patientlists)
            {
                var phonenumber = item.phonenumber;
                var name = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.Name).FirstOrDefault());
                    var mnamename = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.MiddleName).FirstOrDefault());
                    var lastname = GetConcatName(WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.LastName).FirstOrDefault());

                var fullname = name + mnamename + lastname;

                var CMPID = WYNKContext.Registration.Where(x => x.UIN == item.UIN).Select(x => x.CMPID).FirstOrDefault();
                var cmpname = CMPSContext.Company.Where(x => x.CmpID == CMPID).Select(x => x.CompanyName).FirstOrDefault();

                var sendsms = PasswordEncodeandDecode.Sendsmsmessage(phonenumber, SMSPatientlists, fullname,cmpname);

                if(item.email != null)
                {           
                    var sendmail = EmailService.EmailSend(item.email, fullname, cmpname, EMALPatientlists);
                }

               
            }


            try
            {
                if (CMPSContext.SaveChanges() >= 0)
                {
                    return new
                    {
                        Success = true,
                    };
                }

            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Success = false,
                Message = "Some data are Missing"
            };
        }

    }
}
