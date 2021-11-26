using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Data.Repository.Operation;
using WYNK.Helpers;


namespace WYNK.Data.Repository.Implementation
{
    class InvestigationRepository : RepositoryBase<InvestigationImage>, IInvestigationRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;

        public InvestigationRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public IEnumerable<Test> GettestDetails(int cmpid, string tag)

        {
            return (from SPM in WYNKContext.SpecialityMaster.Where(u => u.ParentTag == tag && u.IsDeleted == false && u.IsActive == true && u.CmpID == cmpid).OrderByDescending(x => x.ID)
                    select new Test
                    {
                        Desc1 = SPM.ParentDescription,
                        Code1 = SPM.ID,
                    }).ToList();
        }


        public InvestigationImage GetPatDetails(string uin, int cmpid, string GMT)

        {
            var invv = new InvestigationImage();
            invv.Investigation = new InvestigationImages();
            invv.SpecialityMaster = new SpecialityMaster();
            invv.INV = new List<InvestigationImages>();
            invv.InvImg = new List<InvImg>();
            invv.InvDet = new List<InvDet>();
            invv.PaymentMaster = new List<Payment_Master>();
            invv.PatDetails = new List<PatDetails>();
            invv.InvestigationExtn = new InvestigationExtn();
            invv.TestTran = new List<TestTran>();
            invv.TestMaster = new TestMaster();
            invv.OneLineMaster = new OneLine_Masters();

            var doct = CMPSContext.DoctorMaster.ToList();
            var invpre = WYNKContext.InvestigationPrescription.ToList();
            var use = CMPSContext.Users.ToList();
            var cmp = CMPSContext.Company.ToList();

            var doctor = (from IP in invpre.Where(x => x.Status == "Open" && x.isbilled == false && x.UIN == uin && x.CMPID == cmpid).OrderByDescending(x => x.CreatedUTC)
                         
                          join DOC in doct on IP.PrescribedBy equals DOC.DoctorID



                          select new
                          {
                              rid = IP.RegistrationTranID,
                              ipid = IP.RandomUniqueID,
                              PrescribedDate = IP.CreatedUTC,
                             
                              PrescribedBy = DOC.Firstname + " " + DOC.MiddleName + " " + DOC.LastName,
                              Remarks = IP.Remarks,
                          }

                            ).ToList();

            var admin = (from IP in invpre.Where(x => x.Status == "Open" && x.isbilled == false && x.UIN == uin && x.CMPID == cmpid).OrderByDescending(x => x.CreatedUTC)
                         join us in use on IP.CreatedBy equals us.Userid
                         join cm in cmp on us.Username equals cm.EmailID

                         select new
                         {
                             rid = IP.RegistrationTranID,
                             ipid = IP.RandomUniqueID,
                             PrescribedDate = IP.CreatedUTC,                             
                             PrescribedBy = "Admin",
                             Remarks = IP.Remarks,
                         }

                            ).ToList();



            var docadm = doctor.Concat(admin);



            invv.PatientBillDetailsim = (from res in docadm.OrderByDescending(x => x.PrescribedDate)

                                         select new PatientBillDetailsim
                                         {
                                             rid = res.rid,
                                             ipid = res.ipid,
                                             PrescribedDate = res.PrescribedDate,
                                             PrescribedBy = res.PrescribedBy,
                                             Remarks = res.Remarks,
                                         }

                            ).ToList();


            return invv;


        }



        public InvestigationImage GetUINDetails(int cid)
        {

            var invv = new InvestigationImage();
            invv.Investigation = new InvestigationImages();
            invv.INV = new List<InvestigationImages>();
            invv.InvImg = new List<InvImg>();
            invv.InvDet = new List<InvDet>();
            invv.PaymentMaster = new List<Payment_Master>();
            invv.PatDetails = new List<PatDetails>();
            invv.InvestigationExtn = new InvestigationExtn();
            invv.TestTran = new List<TestTran>();
            invv.TestMaster = new TestMaster();
            invv.SpecialityMaster = new SpecialityMaster();
            invv.OneLineMaster = new OneLine_Masters();
            var reg = WYNKContext.InvestigationPrescription.ToList();
            var regtr = WYNKContext.Registration.ToList();




            var uininv = (from RT in reg.Where(x => x.CMPID == cid && x.Status =="Open")
                          join R in regtr
                          on RT.UIN equals R.UIN


                          select new
                          {
                              UIN = R.UIN,
                              name = R.Name + " " + R.MiddleName + " " + R.LastName,
                              gender = R.Gender,
                              //rid = RT.RegistrationTranID,
                              age = PasswordEncodeandDecode.ToAgeString(R.DateofBirth),
                              addr1 = R.Address1,
                              addr2 = R.Address2,
                              addr3 = R.Address3,
                              phone = R.Phone,

                          }).ToList();            
            invv.UinDett = (from RE in uininv.GroupBy(x => x.UIN)

                            select new UinDett
                            {
                                UIN = RE.Select(x => x.UIN).FirstOrDefault(),
                                name = RE.Select(x => x.name).FirstOrDefault(),
                                gender = RE.Select(x => x.gender).FirstOrDefault(),
                                //rid = RE.Select(x => x.rid).FirstOrDefault(),
                                age = RE.Select(x => x.age).FirstOrDefault(),
                                addr1 = RE.Select(x => x.addr1).FirstOrDefault(),
                                addr2 = RE.Select(x => x.addr2).FirstOrDefault(),
                                addr3 = RE.Select(x => x.addr3).FirstOrDefault(),
                                phone = RE.Select(x => x.phone).FirstOrDefault(),

                            }).ToList();


            return invv;
        }



        public InvestigationImage GetInvpastDetails(int cmpid, string uin)

        {
            var invim = new InvestigationImage();
            invim.Investigation = new InvestigationImages();
            invim.INV = new List<InvestigationImages>();
            invim.InvImg = new List<InvImg>();
            invim.InvDet = new List<InvDet>();
            invim.PatDetails = new List<PatDetails>();
            invim.InvestigationExtn = new InvestigationExtn();
            invim.TestTran = new List<TestTran>();
            invim.TestMaster = new TestMaster();
            invim.SpecialityMaster = new SpecialityMaster();
            invim.OneLineMaster = new OneLine_Masters();
            invim.PastDetails = (from INN in WYNKContext.InvestigationImages.Where(x => x.CmpID == cmpid && x.UIN == uin).GroupBy(x => x.RegistrationTranID)

                                 select new PastDetails
                                 {
                                     uin = INN.Select(x => x.UIN).FirstOrDefault(),
                                     InvDate = INN.Select(x => x.CreatedUTC).FirstOrDefault(),
                                     ImgcapLoc = INN.Select(x => x.ImageCapturedLocation).FirstOrDefault(),
                                     ExtInt = INN.Select(x => x.ExternalInternal).FirstOrDefault(),
                                     Address1 = INN.Select(x => x.Address1).FirstOrDefault(),
                                     Address2 = INN.Select(x => x.Address2).FirstOrDefault(),
                                     ReferredBy = INN.Select(x => x.ReferredBy).FirstOrDefault(),
                                     Remarks = INN.Select(x => x.Remarks).FirstOrDefault(),

                                 }).ToList();


            return invim;


        }

        public dynamic Getpahistory(int cmpid)
        {
            var invim = new InvestigationImage();

            var registration = WYNKContext.Registration.ToList();
            var invimg = WYNKContext.InvestigationImages.ToList();

            var uininv = (from IN in invimg.Where(x => x.CmpID == cmpid)
                          join R in registration
                          on IN.UIN equals R.UIN


                          select new
                          {
                              UIN = R.UIN,
                              name = R.Name + " " + R.MiddleName + " " + R.LastName,
                              gender = R.Gender,
                              age = PasswordEncodeandDecode.ToAgeString(R.DateofBirth),
                              InvDate = IN.CreatedUTC,
                              ImgcapLoc = IN.ImageCapturedLocation,
                              ExtInt = IN.ExternalInternal,
                              Address1 = IN.Address1,
                              Address2 = IN.Address2,
                              ReferredBy = IN.ReferredBy,
                              Remarks = IN.Remarks,

                          }).ToList();
            invim.PastDetails = (from INN in uininv.GroupBy(x => x.UIN)

                                 select new PastDetails
                                 {
                                     uin = INN.Select(x => x.UIN).FirstOrDefault(),
                                     name = INN.Select(x => x.name).FirstOrDefault(),
                                     gender = INN.Select(x => x.gender).FirstOrDefault(),
                                     age = INN.Select(x => x.age).FirstOrDefault(),
                                     InvDate = INN.Select(x => x.InvDate).FirstOrDefault(),
                                     ImgcapLoc = INN.Select(x => x.ImgcapLoc).FirstOrDefault(),
                                     ExtInt = INN.Select(x => x.ExtInt).FirstOrDefault(),
                                     Address1 = INN.Select(x => x.Address1).FirstOrDefault(),
                                     Address2 = INN.Select(x => x.Address2).FirstOrDefault(),
                                     ReferredBy = INN.Select(x => x.ReferredBy).FirstOrDefault(),
                                     Remarks = INN.Select(x => x.Remarks).FirstOrDefault(),

                                 }).ToList();
            return invim;
        }



        public InvestigationImage GetInvpresDetails(string ID)

        {
            var invv = new InvestigationImage();
            invv.Investigation = new InvestigationImages();
            invv.INV = new List<InvestigationImages>();
            invv.InvImg = new List<InvImg>();
            invv.InvDet = new List<InvDet>();
            invv.PaymentMaster = new List<Payment_Master>();
            invv.PatDetails = new List<PatDetails>();
            invv.InvestigationExtn = new InvestigationExtn();
            invv.TestTran = new List<TestTran>();
            invv.TestMaster = new TestMaster();
            invv.SpecialityMaster = new SpecialityMaster();
            invv.OneLineMaster = new OneLine_Masters();
            var invpre = WYNKContext.InvestigationPrescription.ToList();
            var invtr = WYNKContext.InvestigationPrescriptionTran.ToList();
            var one = WYNKContext.Services.ToList();
            var servicemaster = WYNKContext.ServiceMaster.ToList();


            invv.PatDetails = (from IP in invpre.Where(x => x.RandomUniqueID == Convert.ToString(ID))
                               join IM in invtr
                               on IP.RandomUniqueID equals IM.IPID
                               join OLM in one
                               on IM.InvestigationID equals OLM.ID
                               

                               select new PatDetails
                               {
                                   Uin = IP.UIN,
                                   idd = IP.RandomUniqueID,
                                   DOI = IP.Dateofinvestigation,
                                   Remarks = IP.Remarks,
                                   Desc = OLM.Description,
                                   rd = IP.RegistrationTranID,                                 

                               }

                              ).ToList();


            return invv;


        }


        public InvestigationImage GetInvpresTranDetails(string ID, int NO)

        {
            var inn = new InvestigationImage();
            inn.Investigation = new InvestigationImages();
            inn.INV = new List<InvestigationImages>();
            inn.InvImg = new List<InvImg>();
            inn.InvDet = new List<InvDet>();
            inn.PatDetails = new List<PatDetails>();
            inn.InvestigationExtn = new InvestigationExtn();
            inn.TestTran = new List<TestTran>();
            inn.TestMaster = new TestMaster();
            inn.SpecialityMaster = new SpecialityMaster();
            inn.OneLineMaster = new OneLine_Masters();
            var invpr = WYNKContext.InvestigationPrescription.ToList();
            var invtr = WYNKContext.InvestigationPrescriptionTran.ToList();
            var one = WYNKContext.Services.ToList();

            inn.InvDetails = (from IP in invpr.Where(x => x.UIN == ID && x.RandomUniqueID == Convert.ToString(NO))
                              join IPT in invtr on
                              IP.RandomUniqueID equals IPT.IPID
                              join OLM in one on
                              IPT.InvestigationID equals OLM.ID

                              select new InvDetails
                              {
                                  Desc = OLM.Description,
                                  rd = IP.RegistrationTranID,
                                  Cost = IPT.Amount,

                              }

                              ).ToList();


            return inn;


        }




        public dynamic UpdateInv(InvestigationImage Investigation, string UIN, int IID, int cmpid)
        {



            Investigation.InvGroup = (from IN in Investigation.INV.GroupBy(x => x.InvestigationID)
                                      select new InvGroup
                                      {
                                          id = IN.Key,
                                          Amount = IN.Select(x => x.InvestigationAmount).FirstOrDefault(),
                                          uid = IN.Select(x => x.InvestigationDescription).FirstOrDefault(),
                                          cid = IN.Select(x => x.CmpID).FirstOrDefault(),
                                          tid = IN.Select(x => x.TaxID).FirstOrDefault(),
                                          tper = IN.Select(x => x.TaxPercentage).FirstOrDefault(),
                                          tval = IN.Select(x => x.TaxValue).FirstOrDefault(),
                                          dper = IN.Select(x => x.DiscountPercentage).FirstOrDefault(),
                                          dval = IN.Select(x => x.DiscountValue).FirstOrDefault(),
                                          toval = IN.Select(x => x.TotalValue).FirstOrDefault(),
                                      }).ToList();



            var testmas = new TestMaster();
            testmas.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
            testmas.UIN = Investigation.TestMaster.UIN;
            testmas.TestID = Investigation.TestMaster.TestID;
            testmas.TestDateTime = Investigation.TestMaster.TestDateTime;
            testmas.DiagnosisID = Investigation.TestMaster.DiagnosisID;
            if (Investigation.Cooperation != null)
            {
                testmas.Cooperation = (int)Enum.Parse(typeof(PACGoodFairPoor), Investigation.Cooperation);
            }
            if (Investigation.ReliablityOD != null)
            {
                testmas.ReliablityOD = (int)Enum.Parse(typeof(PACGoodFairPoor), Investigation.ReliablityOD);
            }
            if (Investigation.ReliablityOS != null)
            {
                testmas.ReliablityOS = (int)Enum.Parse(typeof(PACGoodFairPoor), Investigation.ReliablityOS);
            }
            testmas.Comments = Investigation.TestMaster.Comments;
            testmas.PerformedByID = Investigation.TestMaster.PerformedByID;
            testmas.PerformedBy = Investigation.TestMaster.PerformedBy;
            testmas.CreatedBy = Investigation.TestMaster.CreatedBy;
            testmas.CreatedUTC = DateTime.UtcNow;
            WYNKContext.TestMaster.AddRange(testmas);
            WYNKContext.SaveChanges();
            var ranunid = testmas.RandomUniqueID;
            var crby = testmas.CreatedBy;

            if (Investigation.TestTran.Count() > 0)
            {
                foreach (var item in Investigation.TestTran.ToList())


                {


                    var testtran = new TestTran();

                    testtran.TestMasterID = ranunid;
                    testtran.TestResultID = item.TestResultID;
                    testtran.IsOD = item.IsOD;
                    testtran.IsOS = item.IsOS;
                    testtran.IsOU = item.IsOU;
                    testtran.Remarks = item.Remarks;
                    testtran.CreatedUTC = DateTime.UtcNow;
                    testtran.CreatedBy = crby;
                    WYNKContext.TestTran.AddRange(testtran);

                }
            }
            WYNKContext.SaveChanges();

            var invesimgs = new InvestigationTran();
            if (Investigation.INV.Count() > 0)
            {
                foreach (var item1 in Investigation.InvGroup.ToList())

                {


                    invesimgs.InvestigationID = item1.id;
                    invesimgs.CmpID = item1.cid;
                    invesimgs.InvestigationTakenOn = DateTime.Now.Date;
                    invesimgs.InvestigationAmount = item1.Amount;
                    invesimgs.TaxID = item1.tid;
                    invesimgs.TaxPercentage = item1.tper;
                    invesimgs.TaxValue = item1.tval;
                    invesimgs.DiscountPercentage = item1.dper;
                    invesimgs.DiscountValue = item1.dval;
                    invesimgs.TotalValue = item1.toval;
                    invesimgs.IsBilled = false;
                    invesimgs.CreatedUTC = DateTime.Now;
                    invesimgs.CreatedBy = Investigation.uid;
                    WYNKContext.InvestigationTran.AddRange(invesimgs);

                    WYNKContext.SaveChanges();
                }
            }



            if (Investigation.INV.Count() > 0)
            {
                foreach (var item in Investigation.INV.ToList())

                {


                    var invesimg = new InvestigationImages();
                    var invid = invesimgs.ID;

                    invesimg.CmpID = item.CmpID;
                    invesimg.UIN = Investigation.Investigation.UIN;
                    invesimg.RegistrationTranID = Investigation.Investigation.RegistrationTranID;
                    invesimg.InvestigationID = item.InvestigationID;
                    invesimg.InvestigationDescription = item.InvestigationDescription;
                    invesimg.InvestigationAmount = item.InvestigationAmount;
                    invesimg.TaxID = item.TaxID;
                    invesimg.TaxPercentage = item.TaxPercentage;
                    invesimg.TaxValue = item.TaxValue;
                    invesimg.DiscountPercentage = item.DiscountPercentage;
                    invesimg.DiscountValue = item.DiscountValue;
                    invesimg.TotalValue = item.TotalValue;
                    invesimg.ImageLocation = item.ImageLocation;
                    invesimg.Remarks = item.Remarks;
                    invesimg.InvestigationTranID = invid;
                    invesimg.IsDeleted = false;
                    invesimg.CreatedUTC = DateTime.Now;
                    invesimg.CreatedBy = Investigation.uid;
                    WYNKContext.InvestigationImages.AddRange(invesimg);

                }


              
                var billtype = CMPSContext.Setup.Where(x => x.CMPID == cmpid).Select(x => x.BillingType).FirstOrDefault();

                if (billtype == "Consolidated Billing" && Investigation.TestAmount.Count >0)
                {

                    var respa = (from pa in WYNKContext.PatientAccount.Where(x => x.UIN == UIN && x.InvoiceNumber == null && x.CMPID == cmpid && x.TransactionID == Investigation.billtranid)
                                 select new
                                 {
                                     ret = pa.UIN.Count(),

                                 }).ToList();

                    var PAID = WYNKContext.PatientAccount.Where(x => x.UIN == UIN && x.InvoiceNumber == null && x.CMPID == cmpid && x.TransactionID == Investigation.billtranid).Select(x => x.PAID).LastOrDefault();
                    var childid = WYNKContext.ServiceMaster.Where(x => x.Tag == "INV" && x.IsActive == true && x.CMPID == cmpid).Select(x => x.ChildDescription).FirstOrDefault();
                    var ServiceTypeID = WYNKContext.Services.Where(x => x.ID == Convert.ToInt32(childid) && x.CMPID == cmpid).Select(x => x.ID).FirstOrDefault();
                    var childdesc = WYNKContext.Services.Where(x => x.ID == Convert.ToInt32(childid) && x.CMPID == cmpid).Select(x => x.Description).FirstOrDefault();

                    var parid = WYNKContext.ServiceMaster.Where(x => x.Tag == "INV" && x.IsActive == true && x.CMPID == cmpid).Select(x => x.parentDescription).FirstOrDefault();
                    var pid = WYNKContext.Services.Where(x => x.ID == Convert.ToInt32(parid) && x.CMPID == cmpid).Select(x => x.ID).FirstOrDefault();
                    var pardesc = WYNKContext.Services.Where(x => x.ID == Convert.ToInt32(parid) && x.CMPID == cmpid).Select(x => x.Description).FirstOrDefault();

                    var Amount = WYNKContext.ServiceMaster.Where(x => x.ChildDescription == Convert.ToString(IID) && x.CMPID == cmpid).Select(x => x.TotalAmount).FirstOrDefault();
                    if (Amount == null) {
                        Amount = 0;
                    }
                    var invname = WYNKContext.Services.Where(x => x.ID == IID).Select(x => x.Description).FirstOrDefault();
                    var invespa = new PatientAccount();

                    if (respa.Count == 0)
                    {

                        invespa.CMPID = cmpid;
                        invespa.UIN = UIN;
                        invespa.TotalProductValue = Investigation.Amount;
                        invespa.TotalDiscountValue = Investigation.DiscountAmount;
                        invespa.TotalTaxValue = Investigation.GSTAmount != null ? Investigation.GSTAmount : 0;
                        invespa.TotalCGSTTaxValue = (Investigation.GSTAmount != null ? Investigation.GSTAmount : 0)/2;
                        invespa.TotalSGSTTaxValue = (Investigation.GSTAmount != null ? Investigation.GSTAmount : 0) / 2;
                        invespa.CESSValue = Investigation.CESSAmount;
                        invespa.AdditionalCESSValue = Investigation.AdditionalCESSAmount;
                        invespa.TotalBillValue = Investigation.TotalCost;
                        invespa.CreatedUTC = DateTime.UtcNow;
                        invespa.CreatedBy = Investigation.uid;
                        invespa.TransactionID =Investigation.billtranid;
                        WYNKContext.PatientAccount.AddRange(invespa);
                        WYNKContext.SaveChanges();

                    }

                    else
                    {

                        var masters1 = WYNKContext.PatientAccount.Where(x => x.UIN == UIN).LastOrDefault();
                        masters1.TotalProductValue += Investigation.Amount != null ? Investigation.Amount : 0;
                        masters1.TotalDiscountValue += Investigation.DiscountAmount != null ? Investigation.DiscountAmount : 0;
                        masters1.TotalTaxValue += Investigation.GSTAmount != null ? Investigation.GSTAmount : 0;
                        masters1.TotalCGSTTaxValue += (Investigation.GSTAmount != null ? Investigation.GSTAmount : 0)/2;
                        masters1.TotalSGSTTaxValue += (Investigation.GSTAmount != null ? Investigation.GSTAmount : 0) / 2;
                        masters1.CESSValue += Investigation.CESSAmount != null ? Investigation.CESSAmount : 0;
                        masters1.AdditionalCESSValue += Investigation.AdditionalCESSAmount != null ? Investigation.AdditionalCESSAmount : 0;
                        masters1.TotalBillValue += Investigation.TotalCost != null ? Investigation.TotalCost : 0;
                        masters1.UpdatedUTC = DateTime.UtcNow;
                        masters1.UpdatedBy = Investigation.uid;
                        WYNKContext.PatientAccount.UpdateRange(masters1);
                        WYNKContext.SaveChanges();
                    }
                    if (respa.Count == 0)
                    {

                        PAID = invespa.PAID;
                    }
                    else
                    {

                    }
                    var respa1 = WYNKContext.PatientAccountDetail.Where(x => x.PAID == PAID && x.ServiceTypeID == pid).ToList();
                    if (respa1.Count == 0)
                    {
                        var patactdt = new PatientAccountDetail();
                        patactdt.PAID = PAID;
                        patactdt.OLMID = pid;
                        patactdt.ServiceTypeID = pid;
                        patactdt.ServiceDescription = pardesc;
                        patactdt.TotalProductValue = Investigation.Amount != null ? Investigation.Amount : 0;
                        patactdt.TotalDiscountValue = Investigation.DiscountAmount != null ? Investigation.DiscountAmount : 0;
                        patactdt.TotalTaxValue = Investigation.GSTAmount != null ? Investigation.GSTAmount : 0;
                        patactdt.TotalCGSTTaxValue = (Investigation.GSTAmount != null ? Investigation.GSTAmount : 0) / 2;
                        patactdt.TotalSGSTTaxValue = (Investigation.GSTAmount != null ? Investigation.GSTAmount : 0) / 2;
                        patactdt.CESSValue = Investigation.CESSAmount != null ? Investigation.CESSAmount : 0;
                        patactdt.AdditonalCESSValue = Investigation.AdditionalCESSAmount != null ? Investigation.AdditionalCESSAmount : 0;
                        patactdt.TotalBillValue = Investigation.TotalCost != null ? Investigation.TotalCost : 0;
                        patactdt.CreatedUTC = DateTime.UtcNow;
                        patactdt.CreatedBy = Investigation.uid;
                        WYNKContext.PatientAccountDetail.AddRange(patactdt);
                        WYNKContext.SaveChanges();
                    }
                    else
                    {
                        var masters = WYNKContext.PatientAccountDetail.Where(x => x.PAID == PAID && x.ServiceTypeID == pid).LastOrDefault();
                        masters.TotalProductValue += Investigation.Amount != null ? Investigation.Amount : 0;
                        masters.TotalDiscountValue += Investigation.DiscountAmount != null ? Investigation.DiscountAmount : 0;
                        masters.TotalTaxValue += Investigation.GSTAmount != null ? Investigation.GSTAmount : 0;
                        masters.TotalCGSTTaxValue += (Investigation.GSTAmount != null ? Investigation.GSTAmount : 0) / 2;
                        masters.TotalSGSTTaxValue += (Investigation.GSTAmount != null ? Investigation.GSTAmount : 0) / 2;
                        masters.CESSValue += Investigation.CESSAmount != null ? Investigation.CESSAmount : 0;
                        masters.AdditonalCESSValue += Investigation.AdditionalCESSAmount != null ? Investigation.AdditionalCESSAmount : 0;
                        masters.TotalBillValue += Investigation.TotalCost != null ? Investigation.TotalCost : 0;
                        masters.UpdatedUTC = DateTime.UtcNow;
                        masters.UpdatedBy = Investigation.uid;
                        WYNKContext.PatientAccountDetail.UpdateRange(masters);

                    }
                    var PAccDetailID = WYNKContext.PatientAccountDetail.Where(x => x.PAID == PAID && x.ServiceTypeID == pid).Select(x => x.PAccDetailID).LastOrDefault();

                    var paid = invespa.PAID;
                    var patactdttx = new PatientAccountDetailTax();
                    patactdttx.PAccDetailID = PAccDetailID;
                    patactdttx.ServiceTypeID = IID;
                    patactdttx.Description = invname;
                    patactdttx.TaxPercentage = 0;
                    patactdttx.TotalProductValue = Investigation.Amount != null ? Investigation.Amount : 0;
                    patactdttx.TotalValue = Convert.ToDecimal(Investigation.TotalCost) != null ? Convert.ToDecimal(Investigation.TotalCost) : 0;
                    patactdttx.TotalDiscountValue = Investigation.DiscountAmount != null ? Investigation.DiscountAmount : 0;
                    patactdttx.TotalTaxValue = Investigation.GSTAmount != null ? Investigation.GSTAmount : 0;
                    patactdttx.TotalCGSTTaxValue = (Investigation.GSTAmount != null ? Investigation.GSTAmount : 0) / 2;
                    patactdttx.TotalSGSTTaxValue = (Investigation.GSTAmount != null ? Investigation.GSTAmount : 0) / 2;
                    patactdttx.CESSValue = Investigation.CESSAmount != null ? Investigation.CESSAmount : 0;
                    patactdttx.AdditionalCESSValue = Investigation.AdditionalCESSAmount != null ? Investigation.AdditionalCESSAmount : 0;
                    patactdttx.CreatedUTC = DateTime.UtcNow;
                    patactdttx.CreatedBy = Investigation.uid;
                    WYNKContext.PatientAccountDetailTax.AddRange(patactdttx);

                }
                    WYNKContext.SaveChanges();
              
                var phonenumber = WYNKContext.Registration.Where(x => x.UIN == Investigation.Investigation.UIN).Select(x => x.Phone).FirstOrDefault();
                var Patientfirstname = WYNKContext.Registration.Where(x => x.UIN == Investigation.Investigation.UIN).Select(x => x.Name).FirstOrDefault();
                var Patientlastname = WYNKContext.Registration.Where(x => x.UIN == Investigation.Investigation.UIN).Select(x => x.LastName).FirstOrDefault();
                var Uploadeddatetime = DateTime.Now.ToString("dd-MMM-yyyy HH:mm");
                var fullname = Patientfirstname + ' ' + Patientlastname;
                var Uploadedby = "";
                var sumcount = Investigation.INV.Count();
                var Invdescription = Investigation.INV.Select(x => x.InvestigationDescription).FirstOrDefault();
                var Invremarks = Investigation.INV.Select(x => x.Remarks).FirstOrDefault();
                var Createdid = CMPSContext.Users.Where(x => x.Userid == Investigation.uid).Select(x => x.Username).FirstOrDefault();
                var Createdtag = CMPSContext.Users.Where(x => x.Userid == Investigation.uid).Select(x => x.ReferenceTag).FirstOrDefault();
                var CMPID = Investigation.INV.Select(x => x.CmpID).FirstOrDefault();
                var companyname = CMPSContext.Company.Where(x => x.CmpID == CMPID).Select(x => x.CompanyName).FirstOrDefault();
                if (Createdtag == "A")
                {
                    Uploadedby = "Admin" + " - " + "(" + Createdid + ")";
                }
                else if (Createdtag == "D" || Createdtag == "O" || Createdtag == "V")
                {
                    var fname = GetConcatName(CMPSContext.DoctorMaster.Where(x => x.EmailID == Createdid).Select(x => x.Firstname).FirstOrDefault());
                    var mname = GetConcatName(CMPSContext.DoctorMaster.Where(x => x.EmailID == Createdid).Select(x => x.MiddleName).FirstOrDefault());
                    var lname = GetConcatName(CMPSContext.DoctorMaster.Where(x => x.EmailID == Createdid).Select(x => x.LastName).FirstOrDefault());
                    Uploadedby = fname + ' ' + mname + ' ' + lname;
                }
                else
                {
                    var fjoinname = CMPSContext.EmployeeCommunication.Where(x => x.EmailID == Createdid).Select(x => x.EmpID).FirstOrDefault();
                    var fname = GetConcatName(CMPSContext.Employee.Where(x => x.EmployeeID == fjoinname).Select(x => x.FirstName).FirstOrDefault());
                    var mname = GetConcatName(CMPSContext.Employee.Where(x => x.EmployeeID == fjoinname).Select(x => x.MiddleName).FirstOrDefault());
                    var lname = GetConcatName(CMPSContext.Employee.Where(x => x.EmployeeID == fjoinname).Select(x => x.LastName).FirstOrDefault());
                    Uploadedby = fname + ' ' + mname + ' ' + lname;
                }

                if (phonenumber != null)
                {

                    string usersid = "cmpsadmin";
                    string apikey = "UMrCTzVADqibrFY4PAto";
                    object mobile = phonenumber;
                    string msgtext = Investigation.Investigation.UIN + " - " + fullname + "\n" + Uploadeddatetime + "\n" + "Under " + Invdescription + ' ' + sumcount + " Images uploaded by " + '-' + Uploadedby + "\n" + "Remarks - " + Invremarks + "\n" + companyname;
                    var client = new WebClient();
                    var baseurl = "http://smshorizon.co.in/api/sendsms.php?user=" + usersid + "&apikey=" + apikey + "&number=" + mobile + "&message=" + msgtext + "&senderid=CMPSIN&type=txt";
                    var data = client.OpenRead(baseurl);
                    var reader = new StreamReader(data);
                    var response = reader.ReadToEnd();
                    string smssucces = response;
                    data.Close();
                    reader.Close();
                    object messagebox = "msg saved";
                    object text = "tt";
                }



            }





            try
            {
                if (WYNKContext.SaveChanges() >= 0)
                {
                    ErrorLog oErrorLog = new ErrorLog();
                    oErrorLog.WriteErrorLog("Information :", "Saved Successfully");
                }
                return new
                {
                    Uin = Investigation.Investigation.UIN,

                    Success = true,
                    Message = CommonMessage.saved,
                };
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Success = false,
                Message = CommonMessage.Missing,
            };

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

        public dynamic UpdateInvestigation(InvestigationImage Investigation, string UIN, string ipid)
        {

            var inv = new InvestigationImages();

            var ids = WYNKContext.InvestigationImages.Where(x => x.UIN == UIN && x.CreatedUTC.Date == DateTime.Now.Date).ToList();

            if (ids != null && ids.Count != 0)
            {

                ids.All(x =>
                {
                    x.ExternalInternal = Investigation.Investigation.ExternalInternal; x.ImageCapturedLocation = Investigation.Investigation.ImageCapturedLocation; x.Address1 = Investigation.Investigation.Address1;
                    x.Address2 = Investigation.Investigation.Address2;                   
                    x.ReferredBy = Investigation.Investigation.ReferredBy; return true;
                });
                WYNKContext.InvestigationImages.UpdateRange(ids);
            }

            var regid = WYNKContext.InvestigationPrescription.Where(x => x.UIN == Investigation.Investigation.UIN).Select(x => x.RegistrationTranID).LastOrDefault();

            var master = WYNKContext.InvestigationPrescription.Where(x => x.RandomUniqueID == ipid).ToList();
            var uid = WYNKContext.InvestigationPrescription.Where(x => x.RandomUniqueID == ipid).Select(x => x.CreatedBy).LastOrDefault();

            if (master != null)
            {

                master.All(x => { x.Status = "Closed"; return true; });
                WYNKContext.InvestigationPrescription.UpdateRange(master);
            }

            var invext = new InvestigationExtn();
            invext.CmpID = Investigation.InvestigationExtn.CmpID;
            invext.UIN = Investigation.InvestigationExtn.UIN;
            invext.Name = Investigation.InvestigationExtn.Name;
            invext.Date = DateTime.UtcNow;
            invext.IPID = ipid;
            invext.DoctorID = uid;
            invext.Tag = false;
            invext.CreatedUTC = DateTime.UtcNow;
            invext.CreatedBy = Investigation.InvestigationExtn.CreatedBy;
            WYNKContext.InvestigationExtn.AddRange(invext);
            try
            {
                if (WYNKContext.SaveChanges() >= 0)
                    return new
                    {
                        Success = true,
                        Message = CommonMessage.saved,
                    };
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Success = false,
                Message = CommonMessage.Missing,
            };

        }


        public bool uploadImag(IFormFile file1, string desc, string uin, string id)
        {
            var inves = new InvestigationImage();
            inves.Investigation = new InvestigationImages();
            inves.INV = new List<InvestigationImages>();
            try
            {
                var ivid = WYNKContext.InvestigationImages.Where(x => x.UIN == id && x.InvestigationDescription == uin).Select(x => x.ID).LastOrDefault();
                var currentDir = Directory.GetCurrentDirectory();
                var dt = Convert.ToDateTime(DateTime.Now.Date).ToString("dd-MM-yyyy");
                var res = Directory.CreateDirectory(currentDir + '/' + uin + '/' + dt);
                var descs = desc + ivid;
                var fileName1 = $"{descs}{Path.GetExtension(file1.FileName)}";
                var path1 = $"{currentDir}/{uin}/{dt}/{fileName1}";
                var pathh = $"{currentDir}/{uin}/{dt}";
                using (var stream1 = new FileStream(path1, FileMode.Create))
                {
                    file1.CopyTo(stream1);
                    var opbio = WYNKContext.InvestigationImages.Where(x => x.UIN == id && x.InvestigationDescription == uin && x.CreatedUTC.Date == DateTime.Now.Date && x.ImageLocation == "Test").ToList();
                    if (opbio.Count() > 0)
                    {




                        foreach (var item1 in opbio.ToList())
                        {
                            item1.ImageLocation = pathh;
                            WYNKContext.Entry(item1).State = EntityState.Modified;
                            WYNKContext.SaveChanges();
                        }

                    }

                    return WYNKContext.SaveChanges() > 0;
                }

            }



            catch (Exception)
            {
                return false;
            }
        }




        public dynamic Getimage(string UIN)
        {
            var inv = new InvestigationImage();
            inv.Investigation = new InvestigationImages();
            inv.INV = new List<InvestigationImages>();
            inv.InvImg = new List<InvImg>();
            var regs = WYNKContext.InvestigationImages.Where(x => x.UIN == UIN).Select(x => x.ImageLocation).ToList();
            var res = WYNKContext.InvestigationImages.Where(x => x.UIN == UIN).Select(x => x.InvestigationDescription).ToList();

            inv.Registration = WYNKContext.Registration.Where(x => x.UIN == UIN).FirstOrDefault();
            var groups = WYNKContext.InvestigationImages.Where(x => x.UIN == UIN).OrderByDescending(x => x.CreatedUTC).GroupBy(x => x.ImageLocation);//ImageLocation
            inv.Imagedata = (from IN in groups
                             select new Imagedata
                             {
                                 idd = IN.Key,
                                 cont = IN.Count(),
                                 Descs = IN.Select(X => X.ImageLocation).FirstOrDefault(),
                                 rmrks = IN.Select(X => X.Remarks).FirstOrDefault(),
                                 ImDescs = IN.Select(X => X.InvestigationDescription).FirstOrDefault(),
                                 dttms = IN.Select(X => X.CreatedUTC).FirstOrDefault(),
                             }).ToList();
            if (inv.Imagedata != null)
            {


                var rest = inv.Imagedata.Sum(x => x.cont);
                inv.stringArray = new string[rest];

                var list1 = new List<samplelist>();

                foreach (var item1 in inv.Imagedata.ToList())

                {

                    var ress = item1.cont;

                    var list2 = new samplelist();
                    list2.Uin = ress;
                    list1.Add(list2);

                    var cv = list1.Sum(x => x.Uin);

                    var cvv = cv - item1.cont;

                    for (var inde = 0; inde < item1.cont; inde++)
                    {
                        var ivid = WYNKContext.InvestigationImages.Where(x => x.UIN == UIN).Select(x => x.ID).ToList();
                        for (var indes = 0; indes < ivid.Count; indes++)
                        {
                            var a = ivid[indes];                          
                            var testStr = item1.Descs + '/' + UIN + inde + a + ".png";
                            var path = testStr;

                            var dttms = WYNKContext.InvestigationImages.Where(x => x.ImageLocation == item1.Descs && x.UIN == UIN).Select(x => x.CreatedUTC).ToList();

                            if ((File.Exists(path)))
                            {

                                string imageData = Convert.ToBase64String(File.ReadAllBytes(path));
                                string source = imageData;
                                if (inv.stringArray[inde] != null)
                                {
                                    inv.stringArray[inde + cvv] = imageData;

                                }
                                else
                                {
                                    inv.stringArray[inde] = imageData;

                                }


                                var list4 = new InvImg();
                                list4.idm = item1.idd;
                                list4.remr = item1.rmrks;
                                list4.Desccm = item1.ImDescs;
                                list4.dttm = item1.dttms;
                                list4.imgdt = "data:image/png;base64," + imageData;
                                inv.InvImg.Add(list4);
                            }
                        }
                    }
                }

                inv.InvImgres = (from IN in inv.InvImg.GroupBy(x => x.Desccm)//Desccm
                                 select new InvImgres
                                 {
                                     Desccmre = IN.Key,
                                     remarks = IN.Select(x => x.remr).ToList(),
                                     imgdtre = IN.Select(x => x.imgdt).ToList(),
                                     imgdttm = IN.Select(x => x.dttm).ToList(),

                                 }).ToList();


            }

            else
            {
            }
            return inv;
        }


        public dynamic Getnotificationalerts(int Docid, int cmpid)
        {
            var data = new InvestigationImage();
            var inv = WYNKContext.InvestigationExtn.Where(x => x.CmpID == cmpid && x.DoctorID == Docid && x.Tag == false).FirstOrDefault();           
            if (inv != null)
            {
                data.NotificationMessage = "Investigation images uploaded against " + inv.UIN + " - " + inv.Name + ".";
                inv.Tag = true;
                WYNKContext.InvestigationExtn.UpdateRange(inv);
                WYNKContext.SaveChanges();
            }
            return data;
        }

        public dynamic GetamtDetails(int cmpid, string id)
        {
            var inv = new InvestigationImage();
            var servicemaster = WYNKContext.ServiceMaster.ToList();
            var service = WYNKContext.Services.ToList();

            inv.TestAmount = (from SM in servicemaster.Where(x => x.CMPID == cmpid && x.ChildDescription == id)
                              join SE in service on SM.ChildDescription equals Convert.ToString(SE.ID)

                              select new TestAmount
                              {
                                  Description = SE.Description,
                                  Amount = SM.TotalAmount,
                                  TotalCost = SM.TotalAmount,

                              }

                              ).ToList();
            return inv;
        }

    }
}







