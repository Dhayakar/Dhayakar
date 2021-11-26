using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using System.Linq;
using WYNK.Helpers;
using System.IO;
using WYNK.Data.Repository.Operation;

namespace WYNK.Data.Repository.Implementation
{
    public class PatientHistoryRepository : RepositoryBase<PatientHistoryViewModel>, IPatientHistoryRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        

        public PatientHistoryRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public dynamic InsertPatientHistory(PatientHistoryViewModel InsertPatientHistory)
        {


            if (InsertPatientHistory.listofHistory.Count() > 0)
            {

                foreach (var item in InsertPatientHistory.listofHistory.Where(x => x.ID == 0))
                {
                    var PatientHistory = new PatientHistory();

                    var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "SystemicCondition"
                 && x.ParentDescription == item.Description).Select(x => x.OLMID).FirstOrDefault();


                    PatientHistory.Description = Desc;
                    PatientHistory.FromUTC = item.FromDate;
                    PatientHistory.CreatedUTC = DateTime.UtcNow;
                    PatientHistory.CreatedBy = Convert.ToInt32(InsertPatientHistory.USERID);
                    PatientHistory.IsActive = true;
                    PatientHistory.UIN = item.UIN;
                    PatientHistory.RegistrationTranID = WYNKContext.RegistrationTran.Where(x => x.UIN == item.UIN).OrderByDescending(x => x.CreatedUTC).Select(x => x.RegistrationTranID).FirstOrDefault();
                    WYNKContext.PatientHistory.Add(PatientHistory);
                    WYNKContext.SaveChanges();
                }

                foreach (var item in InsertPatientHistory.listofHistory.Where(x => x.ID != 0 && x.Actions == "DROP").ToList())
                {
                    var PatientHistory = new PatientHistory();
                    PatientHistory = WYNKContext.PatientHistory.Where(x => x.ID == item.ID).SingleOrDefault();
                    PatientHistory.IsActive = false;
                    PatientHistory.CreatedBy = Convert.ToInt32(InsertPatientHistory.USERID);
                    WYNKContext.PatientHistory.UpdateRange(PatientHistory);
                    WYNKContext.SaveChanges();

                }
                var itemss = InsertPatientHistory.listofHistory;

                WYNKContext.SaveChanges();
                try
                {
                    if (WYNKContext.SaveChanges() >= 0)
                        return new
                        {
                            Success = true,
                            Message = "Saved successfully",

                            Action = itemss,
                        };
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
            else
            {
                return new
                {
                    Success = false,
                    Message = "Some data are Missing"
                };
            }

        }


        public dynamic Insertfamilyhistory(PatientHistoryViewModel Insertfamilyhistory)
        {



            var patintlist = new PatientGeneralDatamodel();

            patintlist.UIN = Insertfamilyhistory.Patientgeneral.UIN;
            patintlist.RegistrationTranID = Insertfamilyhistory.Patientgeneral.RegistrationTranID;
            patintlist.VisitDate = WYNKContext.RegistrationTran.Where(x => x.RegistrationTranID == patintlist.RegistrationTranID).Select(x => x.DateofVisit).FirstOrDefault();


            if (Insertfamilyhistory.Patientgeneral.CurrentMedication == null)
            {
                patintlist.CurrentMedication = "";
            }
            else
            {
                patintlist.CurrentMedication = Insertfamilyhistory.Patientgeneral.CurrentMedication;
            }
            if (Insertfamilyhistory.Patientgeneral.Allergy == null)
            {
                patintlist.Allergy = "";
            }
            else
            {
                patintlist.Allergy = Insertfamilyhistory.Patientgeneral.Allergy;
            }

            if (Insertfamilyhistory.Patientgeneral.FamilyHistory == null)
            {
                patintlist.FamilyHistory = "";
            }
            else
            {
                patintlist.FamilyHistory = Insertfamilyhistory.Patientgeneral.FamilyHistory;
            }





            patintlist.CreatedUTC = DateTime.UtcNow;
            patintlist.CreatedBy = Convert.ToInt32(Insertfamilyhistory.USERID);
            patintlist.IsDeleted = false;
            WYNKContext.PatientGeneral.Update(patintlist);
            WYNKContext.SaveChanges();


            try
            {
                if (WYNKContext.SaveChanges() >= 0)
                    return new
                    {
                        Success = true,
                        Message = "Saved successfully"
                    };
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


        public PatientHistoryViewModel Getphdetails(string Getuin)

        {
            var patintlist = new PatientHistoryViewModel();
            var lists = new List<Oculartotalhistory>();
            patintlist.history = new List<history>();
            var Sustematiclists = new List<systematictotalhistory>();
            var currentlists = new List<CurrentMedicationtotalhistory>();
            var Allergyulists = new List<Allergytotalhistory>();
            var Familylists = new List<FamilyHistorytotalhistory>();

            var Regtranid = WYNKContext.RegistrationTran.Where(x => x.UIN == Getuin).Select(x => x.RegistrationTranID).ToList();

            foreach (var item in Regtranid)
            {
                var id = item;
                var i = 0;
                var IDDesc = "---";
                var history = (from oc in WYNKContext.OcularComplaints.Where(x => x.RegistrationTranID == id)
                               select new
                               {
                                   Description = oc.Description,
                                   FromDate = oc.CreatedUTC,
                               }).ToList();

                var Systematichistory = (from oc in WYNKContext.PatientHistory.Where(x => x.UIN == Getuin)
                                         select new
                                         {
                                             Description = oc.Description,
                                             FromDate = oc.CreatedUTC,
                                             sufferfrom = oc.FromUTC,
                                         }).ToList();

                var Currenthistory = (from oc in WYNKContext.PatientGeneral.Where(x => x.RegistrationTranID == id)
                                      select new
                                      {
                                          CurrentDescription = oc.CurrentMedication,
                                          FromDate = oc.CreatedUTC,
                                      }).ToList();

                var ALLhistory = (from oc in WYNKContext.PatientGeneral.Where(x => x.RegistrationTranID == id)
                                  select new
                                  {
                                      AllergyDescription = oc.Allergy,
                                      FromDate = oc.CreatedUTC,
                                  }).ToList();

                var Familyhistory = (from oc in WYNKContext.PatientGeneral.Where(x => x.RegistrationTranID == id)
                                     select new
                                     {
                                         FamilyDescription = oc.FamilyHistory,
                                         FromDate = oc.CreatedUTC,
                                     }).ToList();

                foreach (var liss in Systematichistory)
                {
                    var Slist = new systematictotalhistory();

                    if (Convert.ToString(liss.Description) != "")
                    {
                        Slist.Description = CMPSContext.OneLineMaster.Where(x => x.OLMID == liss.Description).Select(x => x.ParentDescription).FirstOrDefault();
                        Slist.FromDate = liss.FromDate;
                        Slist.SufferFromDate = Convert.ToDateTime(liss.sufferfrom);
                        Sustematiclists.Add(Slist);
                    }
                    else
                    {
                        Slist.Description = IDDesc;
                        Slist.FromDate = liss.FromDate;
                        Slist.SufferFromDate = Convert.ToDateTime(liss.sufferfrom);
                        Sustematiclists.Add(Slist);
                    }


                }

                foreach (var liss in Currenthistory)
                {

                    var Clist = new CurrentMedicationtotalhistory();

                    if (liss.CurrentDescription != "")
                    {
                        Clist.Description = liss.CurrentDescription;
                        Clist.FromDate = liss.FromDate;
                        currentlists.Add(Clist);
                    }
                    else
                    {
                        Clist.Description = IDDesc;
                        Clist.FromDate = liss.FromDate;
                        currentlists.Add(Clist);
                    }


                }
                foreach (var liss in ALLhistory)
                {
                    var Alist = new Allergytotalhistory();
                    if (liss.AllergyDescription != "")
                    {
                        Alist.Description = liss.AllergyDescription;
                        Alist.FromDate = liss.FromDate;
                        Allergyulists.Add(Alist);
                    }
                    else
                    {
                        Alist.Description = IDDesc;
                        Alist.FromDate = liss.FromDate;
                        Allergyulists.Add(Alist);
                    }


                }
                foreach (var liss in Familyhistory)
                {
                    var Flist = new FamilyHistorytotalhistory();

                    if (liss.FamilyDescription != "")
                    {
                        Flist.Description = liss.FamilyDescription;
                        Flist.FromDate = liss.FromDate;
                        Familylists.Add(Flist);
                    }
                    else
                    {
                        Flist.Description = IDDesc;
                        Flist.FromDate = liss.FromDate;
                        Familylists.Add(Flist);
                    }


                }

                foreach (var liss in history)
                {


                    var list = new Oculartotalhistory();

                    if (Convert.ToString(liss.Description) != "")
                    {
                        list.Description = CMPSContext.OneLineMaster.Where(x => x.OLMID == liss.Description).Select(x => x.ParentDescription).FirstOrDefault();
                        list.FromDate = liss.FromDate;
                        lists.Add(list);
                    }
                    else
                    {
                        list.Description = IDDesc;
                        list.FromDate = liss.FromDate;
                        lists.Add(list);
                    }


                }



            }

            var origiunalist = lists;
            var syslist = Sustematiclists;
            var currlist = currentlists;
            var Alllist = Allergyulists;
            var Familylist = Familylists;


            patintlist.Allergytotalhistory = (from ooc in Allergyulists.GroupBy(x => x.FromDate)
                                              select new Allergytotalhistory
                                              {
                                                  FromDate = ooc.Key,
                                                  Description = ooc.Where(x => x.FromDate == ooc.Key).Select(x => x.Description).FirstOrDefault(),
                                              }).ToList();

            patintlist.FamilyHistorytotalhistory = (from ooc in Familylists.GroupBy(x => x.FromDate)
                                                    select new FamilyHistorytotalhistory
                                                    {
                                                        FromDate = ooc.Key,
                                                        Description = ooc.Where(x => x.FromDate == ooc.Key).Select(x => x.Description).FirstOrDefault(),
                                                    }).ToList();


            patintlist.Oculartotalhistory = (from ooc in lists.GroupBy(x => x.FromDate)
                                             select new Oculartotalhistory
                                             {
                                                 FromDate = ooc.Key,
                                                 Description = ooc.Where(x => x.FromDate == ooc.Key).Select(x => x.Description).FirstOrDefault(),
                                             }).ToList();

            patintlist.systematictotalhistory = (from ooc in Sustematiclists.GroupBy(x => x.FromDate)
                                                 select new systematictotalhistory
                                                 {
                                                     FromDate = ooc.Key,
                                                     SufferFromDate = ooc.Where(x => x.FromDate == ooc.Key).Select(x => x.SufferFromDate).FirstOrDefault(),
                                                     Description = ooc.Where(x => x.FromDate == ooc.Key).Select(x => x.Description).FirstOrDefault(),
                                                 }).ToList();
            patintlist.CurrentMedicationtotalhistory = (from ooc in currentlists.GroupBy(x => x.FromDate)
                                                        select new CurrentMedicationtotalhistory
                                                        {
                                                            FromDate = ooc.Key,
                                                            Description = ooc.Where(x => x.FromDate == ooc.Key).Select(x => x.Description).FirstOrDefault(),
                                                        }).ToList();


            foreach (var Aitemss in patintlist.Allergytotalhistory)
            {
                var Ahist = new history();
                Ahist.FromDate = Aitemss.FromDate;
                Ahist.allergyDescription = Aitemss.Description;
                Ahist.OcularDescription = "---";
                Ahist.systematicDescription = "---";
                Ahist.currentDescription = "---";

                Ahist.Duration = "---";
                Ahist.familyhistoryDescription = "---";
                patintlist.history.Add(Ahist);

            }
            foreach (var Fitemss in patintlist.FamilyHistorytotalhistory)
            {
                var Fhist = new history();
                Fhist.FromDate = Fitemss.FromDate;
                Fhist.allergyDescription = "---";
                Fhist.OcularDescription = "---";
                Fhist.systematicDescription = "---";
                Fhist.currentDescription = "---";

                Fhist.Duration = "---";
                Fhist.familyhistoryDescription = Fitemss.Description;
                patintlist.history.Add(Fhist);

            }
            foreach (var Oitemss in patintlist.Oculartotalhistory)
            {
                var Ohist = new history();
                Ohist.FromDate = Oitemss.FromDate;
                Ohist.allergyDescription = "---";
                Ohist.familyhistoryDescription = "---";
                Ohist.systematicDescription = "---";
                Ohist.currentDescription = "---";

                Ohist.Duration = "---";
                Ohist.OcularDescription = Oitemss.Description;
                patintlist.history.Add(Ohist);
            }
            foreach (var Citemss in patintlist.CurrentMedicationtotalhistory)
            {
                var Chist = new history();
                Chist.FromDate = Citemss.FromDate;
                Chist.allergyDescription = "---";
                Chist.familyhistoryDescription = "---";
                Chist.systematicDescription = "---";
                Chist.OcularDescription = "---";
                Chist.Duration = "---";
                Chist.currentDescription = Citemss.Description;
                patintlist.history.Add(Chist);

            }
            foreach (var itemss in patintlist.systematictotalhistory)
            {
                var hist = new history();
                hist.FromDate = itemss.FromDate;
                hist.SufferedFromDate = itemss.SufferFromDate;
                hist.Duration = PasswordEncodeandDecode.ToAgeString(itemss.SufferFromDate);
                hist.allergyDescription = "---";
                hist.familyhistoryDescription = "---";
                hist.currentDescription = "---";
                hist.OcularDescription = "---";
                hist.systematicDescription = itemss.Description;
                patintlist.history.Add(hist);

            }



            var mergedlist = patintlist.history;

            var lisrrrr = (from b in mergedlist.OrderByDescending(x => x.FromDate)
                           select new
                           {
                               FromDate = b.FromDate,
                               SufferedFromDate = b.SufferedFromDate,
                               Duration = b.Duration,
                               OcularDescription = b.OcularDescription,
                               systematicDescription = b.systematicDescription,
                               currentDescription = b.currentDescription,
                               allergyDescription = b.allergyDescription,
                               familyhistoryDescription = b.familyhistoryDescription,
                           }).ToList();

            var Totallistvalues = new List<ITotalhistory>();

            foreach (var removeduplicate in lisrrrr)
            {
                if (removeduplicate.familyhistoryDescription == "---" && removeduplicate.allergyDescription == "---" &&
                    removeduplicate.currentDescription == "---" && removeduplicate.systematicDescription == "---" &&
                    removeduplicate.OcularDescription == "---")
                {

                }
                else
                {

                    var Highlist = new ITotalhistory();
                    Highlist.FromDate = removeduplicate.FromDate;
                    Highlist.SufferedFromDate = TOSuffereddate(removeduplicate.SufferedFromDate);
                    Highlist.Duration = removeduplicate.Duration;
                    Highlist.OcularDescription = removeduplicate.OcularDescription;
                    Highlist.systematicDescription = removeduplicate.systematicDescription;
                    Highlist.currentDescription = removeduplicate.currentDescription;
                    Highlist.allergyDescription = removeduplicate.allergyDescription;
                    Highlist.familyhistoryDescription = removeduplicate.familyhistoryDescription;
                    Totallistvalues.Add(Highlist);
                }
            }

            patintlist.ITotalhistory = Totallistvalues;

            patintlist.Idatyes = (from nn in patintlist.ITotalhistory.GroupBy(x => x.FromDate.Date)
                                  select new Idatyes
                                  {
                                      FromDate = nn.Key,
                                  }).ToList();


            return patintlist;

        }

        public dynamic TOSuffereddate(DateTime? SDATE)
        {
            var d = SDATE;

            if (Convert.ToString(SDATE) == "01-01-0001 0:00:00")
            {
                d = null;
            }
            else
            {
                d = SDATE;
            }

            return d;
        }




        public PatientHistoryViewModel GetMedicalPrescriptiondetails(string Getuin)

        {
            var MedicalList = new PatientHistoryViewModel();
            var Medical2list = new MedicalPrescriptionhistory();
            var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Getuin).Select(x => x.RegistrationTranID).FirstOrDefault();

            MedicalList.MedicalPrescriptionhistory = (from mp in WYNKContext.MedicalPrescription.Where(x => x.UIN == Getuin).OrderByDescending(x => x.CreatedUTC.Date)
                                                      select new MedicalPrescriptionhistory
                                                      {

                                                          Visitdate = mp.CreatedUTC.Date,
                                                          PrescribeDate = mp.PrescribedDate.Date,
                                                          PrescribedBY = mp.PrescribedByName,
                                                          MedicalPrescriptionNo = mp.MedicalPrescriptionNo,
                                                      }).ToList();

            return MedicalList;
        }


        public PatientHistoryViewModel GetMedicaldetails(string Getuin, string Defineddata)

        {
            var MedicalList = new PatientHistoryViewModel();
            var Medical2list = new Medicalhistory();


            var medpre = WYNKContext.MedicalPrescription.ToList();
            var medtran = WYNKContext.MedicalPrescriptionTran.ToList();
            var drugmaster = WYNKContext.DrugMaster.ToList();
            var drugrp = WYNKContext.DrugGroup.ToList();
            var onelinemasters = CMPSContext.OneLineMaster.ToList();

            var MPID = medpre.Where(x => x.UIN == Getuin && x.MedicalPrescriptionNo == Defineddata)
                .Select(x => x.RandomUniqueID).FirstOrDefault();

            MedicalList.Medicalhistory = (from mt in medtran.Where(x => x.MedicalPrescriptionID == MPID)
                                          join DM in drugmaster on mt.DrugId equals DM.ID
                                          join dg in drugrp on DM.GenericName equals dg.ID

                                          select new Medicalhistory
                                          {
                                              Drugname = DM.Brand,
                                              Generic = dg.Description,
                                              UOM = DM.UOM,
                                              Frequency = mt.Frequency,
                                              Quantity = mt.Quantity,
                                              Eye = mt.Eye,
                                              Food = mt.Food,
                                              Days = Convert.ToInt16(mt.Days),
                                              Remarks = mt.Remarks,
                                          }).ToList();
            MedicalList.MPNO = Defineddata;



            return MedicalList;
        }

        public PatientHistoryViewModel GetInvestigationPrescriptiondetails(string Getuin)
        {
            var MedicalList = new PatientHistoryViewModel();
            var Investigation = WYNKContext.InvestigationPrescription.ToList();
            var InvestigationPrescription = WYNKContext.InvestigationPrescriptionTran.ToList();
            var Oneline = CMPSContext.OneLineMaster.ToList();
            var Doctormaster = CMPSContext.DoctorMaster.ToList();

            MedicalList.InvestigationPrescriptionhistory = (from i in Investigation.Where(x => x.UIN == Getuin)
                                                            join it in InvestigationPrescription on i.RandomUniqueID equals it.IPID
                                                            join om in Oneline on it.InvestigationID equals om.OLMID
                                                            join dm in Doctormaster on i.PrescribedBy equals dm.DoctorID
                                                            select new InvestigationPrescriptionhistory
                                                            {

                                                                Amount = it.Amount,
                                                                Prescribedby = dm.LastName,
                                                                Investigationdesc = om.ParentDescription,
                                                                date = i.CreatedUTC.Date,

                                                            }).ToList();




            return MedicalList;
        }

        public PatientHistoryViewModel GetInvestigationdetails(string Getuin, int CID)
        {
            var MedicalList = new PatientHistoryViewModel();
            var Investigation = WYNKContext.InvestigationPrescription.ToList();
            var InvestigationPrescription = WYNKContext.InvestigationPrescriptionTran.ToList();
            var Oneline = CMPSContext.OneLineMaster.ToList();
            var Doctormaster = CMPSContext.DoctorMaster.ToList();
            var INVimages = WYNKContext.InvestigationImages.ToList();
            var invtran = WYNKContext.InvestigationTran.ToList();
            var user = CMPSContext.Users.ToList();
            var docmas = CMPSContext.DoctorMaster.ToList();
            var userrole = CMPSContext.UserVsRole.ToList();
            var cmp = CMPSContext.Company.ToList();

            var Investigationhistoryss = (from im in INVimages.Where(x => x.UIN == Getuin && x.CmpID == CID).OrderByDescending(x => x.CreatedUTC.Date)
                                          join us in user on
                                          im.CreatedBy equals us.Userid
                                          join doc in docmas on
                                          us.Username equals doc.EmailID
                                          join usr in userrole on
                                          us.Username equals usr.UserName
                                          select new
                                          {
                                              invamount = im.TotalValue,
                                              visistdate = im.CreatedUTC.Date,
                                              exterinternal = im.ExternalInternal,
                                              prescribedby = doc.Firstname + " " + doc.MiddleName + " " + doc.LastName,
                                          }).ToList();





            var Investigationhistorysss = (from im in INVimages.Where(x => x.UIN == Getuin && x.CmpID == CID).OrderByDescending(x => x.CreatedUTC.Date)
                                           join us in user on
                                           im.CreatedBy equals us.Userid
                                           join cm in cmp on
                                           us.Username equals cm.EmailID

                                           select new
                                           {
                                               invamount = im.TotalValue,
                                               visistdate = im.CreatedUTC.Date,
                                               exterinternal = im.ExternalInternal,
                                               prescribedby = "Admin",
                                           }).ToList();



            var res = Investigationhistoryss.Concat(Investigationhistorysss);


            MedicalList.Investigationhistory = (from ims in res.GroupBy(x => x.visistdate.Date)
                                                select new Investigationhistory
                                                {
                                                    visistdate = ims.Key,
                                                    invamount = ims.Sum(x => x.invamount),
                                                    exterinternal = ims.Where(x => x.visistdate == ims.Key).Select(x => x.exterinternal).FirstOrDefault(),
                                                    prescribedby = ims.Where(x => x.visistdate == ims.Key).Select(x => x.prescribedby).FirstOrDefault(),
                                                }).ToList();






            return MedicalList;
        }

        public dynamic GetInvestigationImagedetails(DateTime Dataid, string Getuin)
        {
            var inv = new InvestigationImage();
            inv.Investigation = new InvestigationImages();
            inv.INV = new List<InvestigationImages>();
            inv.InvImg = new List<InvImg>();
            var regs = WYNKContext.InvestigationImages.Where(x => x.UIN == Getuin && x.CreatedUTC.Date == Dataid.Date).Select(x => x.ImageLocation).ToList();
            var res = WYNKContext.InvestigationImages.Where(x => x.UIN == Getuin && x.CreatedUTC.Date == Dataid.Date).Select(x => x.InvestigationDescription).ToList();
            var groups = WYNKContext.InvestigationImages.Where(x => x.UIN == Getuin && x.CreatedUTC.Date == Dataid.Date).OrderByDescending(x => x.CreatedUTC).GroupBy(x => x.InvestigationID);

            inv.Imagedata = (from IN in groups
                             select new Imagedata
                             {
                                 cont = IN.Count(),
                                 Descs = IN.Select(X => X.ImageLocation).FirstOrDefault(),
                                 rmrks = IN.Select(X => X.Remarks).FirstOrDefault(),
                                 ImDescs = IN.Select(X => X.InvestigationDescription).FirstOrDefault(),
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
                        var ivid = WYNKContext.InvestigationImages.Where(x => x.UIN == Getuin && x.CreatedUTC.Date == Dataid.Date).Select(x => x.ID).ToList();
                        for (var indes = 0; indes < ivid.Count; indes++)
                        {
                            var a = ivid[indes];

                            var testStr = item1.Descs + '/' + Getuin + inde + a + ".png";
                            var path = testStr;

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
                                list4.Desccm = item1.ImDescs;
                                list4.remr = item1.rmrks;
                                list4.imgdt = "data:image/png;base64," + imageData;
                                inv.InvImg.Add(list4);


                            }

                        }

                    }


                }


                inv.InvImgres = (from IN in inv.InvImg.GroupBy(x => x.Desccm)
                                 select new InvImgres
                                 {
                                     Desccmre = IN.Key,
                                     remarks = IN.Select(x => x.remr).FirstOrDefault(),
                                     imgdtre = IN.Select(x => x.imgdt).ToList(),

                                 }).ToList();

            }

            var testmas = WYNKContext.TestMaster.ToList();
            var testtran = WYNKContext.TestTran.ToList();
            var specialitymas = WYNKContext.SpecialityMaster.ToList();
            var services = WYNKContext.Services.ToList();
            var onelinemaster = CMPSContext.OneLineMaster.ToList();
            var randomid = WYNKContext.TestMaster.Where(x => x.TestDateTime.Value.Date == Dataid.Date).OrderByDescending(x => x.CreatedUTC).Select(x => x.RandomUniqueID).FirstOrDefault();

            inv.TestHis = (from TM in testmas.Where(x => x.UIN == Getuin && x.TestDateTime.Value.Date == Dataid.Date).GroupBy(x => x.TestID)
                                   select new TestHis
                                   {
                                       Testid = TM.Key,
                                       Testname = services.Where(x => x.ID == TM.Key).Select(x => x.Description).FirstOrDefault(),
                                       TestDate = TM.Select(x => x.TestDateTime).FirstOrDefault(),
                                       Diagnosis = onelinemaster.Where(x => x.OLMID == TM.Select(u => u.DiagnosisID).FirstOrDefault()).Select(x => x.ParentDescription).FirstOrDefault(),
                                       Cooperate = TM.Select(x => Enum.GetName(typeof(PACGoodFairPoor), x.Cooperation)).FirstOrDefault(),
                                       ReliableOD = TM.Select(x => Enum.GetName(typeof(PACGoodFairPoor), x.ReliablityOD)).FirstOrDefault(),
                                       ReliableOS = TM.Select(x => Enum.GetName(typeof(PACGoodFairPoor), x.ReliablityOS)).FirstOrDefault(),
                                       comments = TM.Select(x => x.Comments).FirstOrDefault(),
                                       performedby = TM.Select(x => x.PerformedBy).FirstOrDefault(),
                                       //  Desc = specialitymas.Where(x => x.ID == TT.TestResultID).Select(x => x.ParentDescription).FirstOrDefault(),



                                       TestReshis = (from TT in testtran.Where(x => x.TestMasterID == TM.Select(u => u.RandomUniqueID).FirstOrDefault())
                                                     select new TestReshis
                                                     {
                                                         Desc = specialitymas.Where(x => x.ID == TT.TestResultID).Select(x => x.ParentDescription).FirstOrDefault(),
                                                         od = TT.IsOD,
                                                         os = TT.IsOS,
                                                         ou = TT.IsOU,
                                                         remarks = TT.Remarks,
                                                     }).ToList()
                                   }).ToList();

            return inv;
        }




        public PatientHistoryViewModel Getrefractiondetails(string Getuin)
        {


            var refractionList = new PatientHistoryViewModel();

            refractionList.refractiondetails = new List<refractiondetails>();


            var r = (from REF in WYNKContext.Refraction.Where(u => u.UIN == Getuin)
                     group REF by REF.RegistrationTranID into g

                     select new
                     {
                         RegistrationTranID = g.Key,
                         visitdate = g.Select(x => x.CreatedUTC).FirstOrDefault(),
                     }).ToList();



            refractionList.refractiondetails = (from R in r

                                                select new refractiondetails
                                                {

                                                    visitDate = R.visitdate,
                                                    Rid = R.RegistrationTranID,

                                                }).ToList();
            return refractionList;

        }

        public dynamic Getrefractiondetails1(int RID)

        {
            var refractionget = new PatientHistoryViewModel();
            refractionget.refractiondetails1 = new List<refractiondetails1>();
            refractionget.refractionHistorycv = new List<refractionHistorycv>();
            refractionget.refractionHistoryiop = new List<refractionHistoryiop>();
            refractionget.refractionVisualacuity = new List<refractionVisualacuity>();
            refractionget.refractionHistoryrefraction = new List<refractionHistoryrefraction>();
            refractionget.refractionHisacceptance = new List<refractionHisacceptance>();
            refractionget.refractionHisamsler = new List<refractionHisamsler>();
            refractionget.SquintTraninfo1 = new List<SquintTraninfo1>();
            var onelinemaster = CMPSContext.OneLineMaster.ToList();

            var qry = from va in WYNKContext.Refraction.Where(u => u.RegistrationTranID == RID && u.Description == "PGP")
                      select new
                      {

                          Descriptionn = va.Description,
                          Ocularr = va.Ocular,
                          DistSphh = va.DistSph,
                          NearCyll = va.NearCyl,
                          PinAxiss = va.PinAxis,
                          Addd = va.Add,
                          Detailss = va.Details,
                          createdby = va.CreatedBy,
                          date = va.CreatedUTC,
                      };


            refractionget.refractiondetails1 = (from res in qry.GroupBy(x => x.createdby)
                                                select new refractiondetails1
                                                {
                                                    Date = res.Select(x => x.date).FirstOrDefault(),
                                                    Description = res.Where(x => x.Ocularr == "OD").Select(x => x.Descriptionn).FirstOrDefault(),
                                                    DistSph = res.Where(x => x.Ocularr == "OD").Select(x => x.DistSphh).FirstOrDefault(),
                                                    NearCyl = res.Where(x => x.Ocularr == "OD").Select(x => x.NearCyll).FirstOrDefault(),
                                                    PinAxis = res.Where(x => x.Ocularr == "OD").Select(x => x.PinAxiss).FirstOrDefault(),
                                                    Add = res.Where(x => x.Ocularr == "OD").Select(x => x.Addd).FirstOrDefault(),
                                                    DistSph1 = res.Where(x => x.Ocularr == "OS").Select(x => x.DistSphh).FirstOrDefault(),
                                                    NearCyl1 = res.Where(x => x.Ocularr == "OS").Select(x => x.NearCyll).FirstOrDefault(),
                                                    PinAxis1 = res.Where(x => x.Ocularr == "OS").Select(x => x.PinAxiss).FirstOrDefault(),
                                                    Add1 = res.Where(x => x.Ocularr == "OS").Select(x => x.Addd).FirstOrDefault(),
                                                    Details = res.Where(x => x.Ocularr == null).Select(x => x.Detailss).FirstOrDefault(),
                                                }).ToList();



            var cv = from va in WYNKContext.Refraction.Where(u => u.RegistrationTranID == RID && u.Description == "Color Vision")
                     select new
                     {
                         Descriptionn = va.Description,
                         Ocularr = va.Ocular,
                         cvnormal = va.CV_normal,
                         cvdefective = va.CV_defective,
                         desc = va.Desc_Text,
                         createdby = va.CreatedBy,
                         date = va.CreatedUTC,
                     };


            refractionget.refractionHistorycv = (from res in cv.GroupBy(x => x.createdby)
                                                 select new refractionHistorycv
                                                 {

                                                     Date = res.Select(x => x.date).FirstOrDefault(),
                                                     Description = res.Where(x => x.Ocularr == "OD").Select(x => x.Descriptionn).FirstOrDefault(),
                                                     CV_normal = res.Where(x => x.Ocularr == "OD").Select(x => x.cvnormal).FirstOrDefault(),
                                                     CV_defective = res.Where(x => x.Ocularr == "OD").Select(x => x.cvdefective).FirstOrDefault(),
                                                     Desc_Text = res.Where(x => x.Ocularr == "OD").Select(x => x.desc).FirstOrDefault(),
                                                     CV_normal1 = res.Where(x => x.Ocularr == "OS").Select(x => x.cvnormal).FirstOrDefault(),
                                                     CV_defective1 = res.Where(x => x.Ocularr == "OS").Select(x => x.cvdefective).FirstOrDefault(),
                                                     Desc_Text1 = res.Where(x => x.Ocularr == "OS").Select(x => x.desc).FirstOrDefault(),
                                                 }).ToList();




            var iop = from va in WYNKContext.Refraction.Where(u => u.RegistrationTranID == RID && u.Description == "Intra Ocular Pressure")
                      select new
                      {
                          VDate = va.CreatedUTC,
                          Descriptionn = va.Description,
                          Ocularr = va.Ocular,
                          IolaT = va.Iolat,
                          LoLnCT = va.Iolnct,
                          createdby = va.CreatedBy,
                          time = va.Time,

                      };


            refractionget.refractionHistoryiop = (from res in iop.GroupBy(x => x.createdby)
                                                  select new refractionHistoryiop
                                                  {
                                                      Date = res.Select(x => x.VDate).FirstOrDefault(),
                                                      Description = res.Where(x => x.Ocularr == "OD").Select(x => x.Descriptionn).FirstOrDefault(),
                                                      Iolat = res.Where(x => x.Ocularr == "OD").Select(x => x.IolaT).FirstOrDefault(),
                                                      Iolat1 = res.Where(x => x.Ocularr == "OS").Select(x => x.IolaT).FirstOrDefault(),
                                                      Iolnct = res.Where(x => x.Ocularr == "OD").Select(x => x.LoLnCT).FirstOrDefault(),
                                                      Iolnct1 = res.Where(x => x.Ocularr == "OS").Select(x => x.LoLnCT).FirstOrDefault(),
                                                      Time = res.Where(x => x.Ocularr == "OS").Select(x => x.time).FirstOrDefault(),
                                                  }).ToList();



            var visa = from va in WYNKContext.Refraction.Where(u => u.RegistrationTranID == RID && u.Description == "Visual Acuity")
                       select new
                       {
                           VDate = va.CreatedUTC,
                           VDescription = va.Description,
                           SubCategory = onelinemaster.Where(x => x.OLMID == va.SubCategory).Select(x => x.ParentDescription).FirstOrDefault(),
                           VOcular = va.Ocular,
                           DistSph = onelinemaster.Where(x => x.OLMID == Convert.ToInt32(va.DistSph)).Select(x => x.ParentDescription).FirstOrDefault(),
                           NearCyl = onelinemaster.Where(x => x.OLMID == Convert.ToInt32(va.NearCyl)).Select(x => x.ParentDescription).FirstOrDefault(),
                           VPowerGlass = va.PowerGlass,
                           NVDESC = va.N_V_DESC,
                           PinAxis = onelinemaster.Where(x => x.OLMID == Convert.ToInt32(va.PinAxis)).Select(x => x.ParentDescription).FirstOrDefault(),
                           createdby = va.CreatedBy,
                           Ocularr = va.Ocular,
                       };


            refractionget.refractionVisualacuity = (from res in visa.GroupBy(x => x.createdby)
                                                    select new refractionVisualacuity
                                                    {
                                                        Description = res.Where(x => x.Ocularr == "OD").Select(x => x.VDescription).FirstOrDefault(),
                                                        SubCategory = res.Where(x => x.Ocularr == "OD").Select(x => x.SubCategory).FirstOrDefault(),
                                                        DistSph = res.Where(x => x.Ocularr == "OD").Select(x => x.DistSph).FirstOrDefault(),
                                                        NearCyl = res.Where(x => x.Ocularr == "OD").Select(x => x.NearCyl).FirstOrDefault(),
                                                        PinAxis = res.Where(x => x.Ocularr == "OD").Select(x => x.PinAxis).FirstOrDefault(),
                                                        N_V_DESC = res.Where(x => x.Ocularr == "OD").Select(x => x.NVDESC).FirstOrDefault(),
                                                        PowerGlass = res.Where(x => x.Ocularr == "OD").Select(x => x.VPowerGlass).FirstOrDefault(),
                                                        DistSphh = res.Where(x => x.Ocularr == "OS").Select(x => x.DistSph).FirstOrDefault(),
                                                        NearCyll = res.Where(x => x.Ocularr == "OS").Select(x => x.NearCyl).FirstOrDefault(),
                                                        PinAxiss = res.Where(x => x.Ocularr == "OS").Select(x => x.PinAxis).FirstOrDefault(),
                                                        N_V_DESCC = res.Where(x => x.Ocularr == "OS").Select(x => x.NVDESC).FirstOrDefault(),
                                                        PowerGlasss = res.Where(x => x.Ocularr == "OS").Select(x => x.VPowerGlass).FirstOrDefault(),
                                                        Date = res.Select(x => x.VDate).FirstOrDefault(),
                                                    }).ToList();



            var Re = from va in WYNKContext.Refraction.Where(u => u.RegistrationTranID == RID && u.Description == "Refraction")
                     select new
                     {
                         VDate = va.CreatedUTC,
                         Descriptionn = va.Description,
                         Ocularr = va.Ocular,
                         createdby = va.CreatedBy,
                         disp = va.DistSph,
                         near = va.NearCyl,
                         pin = va.PinAxis,
                         typee = va.Type,
                         SubCategory = onelinemaster.Where(x => x.OLMID == va.SubCategory).Select(x => x.ParentDescription).FirstOrDefault(),
                     };


            refractionget.refractionHistoryrefraction = (from res in Re.GroupBy(x => x.createdby)
                                                         select new refractionHistoryrefraction
                                                         {

                                                             Date = res.Select(x => x.VDate).FirstOrDefault(),
                                                             Description = res.Where(x => x.Ocularr == "OD").Select(x => x.Descriptionn).FirstOrDefault(),
                                                             SubCategory = res.Where(x => x.Ocularr == "OD").Select(x => x.SubCategory).FirstOrDefault(),
                                                             DistSph = res.Where(x => x.Ocularr == "OD" && x.typee == 133).Select(x => x.disp).FirstOrDefault(),
                                                             NearCyl = res.Where(x => x.Ocularr == "OD" && x.typee == 133).Select(x => x.near).FirstOrDefault(),
                                                             PinAxis = res.Where(x => x.Ocularr == "OD" && x.typee == 133).Select(x => x.pin).FirstOrDefault(),
                                                             DistSph1 = res.Where(x => x.Ocularr == "OS" && x.typee == 133).Select(x => x.disp).FirstOrDefault(),
                                                             NearCyl1 = res.Where(x => x.Ocularr == "OS" && x.typee == 133).Select(x => x.near).FirstOrDefault(),
                                                             PinAxis1 = res.Where(x => x.Ocularr == "OS" && x.typee == 133).Select(x => x.pin).FirstOrDefault(),
                                                             DistSph2 = res.Where(x => x.Ocularr == "OD" && x.typee == 132).Select(x => x.disp).FirstOrDefault(),
                                                             NearCyl2 = res.Where(x => x.Ocularr == "OD" && x.typee == 132).Select(x => x.near).FirstOrDefault(),
                                                             PinAxis2 = res.Where(x => x.Ocularr == "OD" && x.typee == 132).Select(x => x.pin).FirstOrDefault(),
                                                             DistSph3 = res.Where(x => x.Ocularr == "OS" && x.typee == 132).Select(x => x.disp).FirstOrDefault(),
                                                             NearCyl3 = res.Where(x => x.Ocularr == "OS" && x.typee == 132).Select(x => x.near).FirstOrDefault(),
                                                             PinAxis3 = res.Where(x => x.Ocularr == "OS" && x.typee == 132).Select(x => x.pin).FirstOrDefault(),
                                                         }).ToList();

            var Acc = from va in WYNKContext.Refraction.Where(u => u.RegistrationTranID == RID && u.Description == "Acceptance")


                      select new
                      {
                          VDate = va.CreatedUTC,
                          Descriptionn = va.Description,
                          Ocularr = va.Ocular,
                          createdby = va.CreatedBy,
                          disp = va.DistSph,
                          near = va.NearCyl,
                          pin = va.PinAxis,
                          add = onelinemaster.Where(x => x.OLMID == Convert.ToInt32(va.Add)).Select(x => x.ParentDescription).FirstOrDefault(),
                          remarkss = va.Remarks,
                          typee = va.Type,
                          pd = va.PD,

                      };


            refractionget.refractionHisacceptance = (from res in Acc.GroupBy(x => x.createdby)
                                                     select new refractionHisacceptance
                                                     {

                                                         Date = res.Select(x => x.VDate).FirstOrDefault(),
                                                         Description = res.Where(x => x.Ocularr == "OD").Select(x => x.Descriptionn).FirstOrDefault(),
                                                         DistSph = res.Where(x => x.Ocularr == "OD" && x.typee == 134).Select(x => x.disp).FirstOrDefault(),
                                                         NearCyl = res.Where(x => x.Ocularr == "OD" && x.typee == 134).Select(x => x.near).FirstOrDefault(),
                                                         PinAxis = res.Where(x => x.Ocularr == "OD" && x.typee == 134).Select(x => x.pin).FirstOrDefault(),
                                                         Add = res.Where(x => x.Ocularr == "OD" && x.typee == 134).Select(x => x.add).FirstOrDefault(),

                                                         DistSph1 = res.Where(x => x.Ocularr == "OS" && x.typee == 134).Select(x => x.disp).FirstOrDefault(),
                                                         NearCyl1 = res.Where(x => x.Ocularr == "OS" && x.typee == 134).Select(x => x.near).FirstOrDefault(),
                                                         PinAxis1 = res.Where(x => x.Ocularr == "OS" && x.typee == 134).Select(x => x.pin).FirstOrDefault(),
                                                         Add1 = res.Where(x => x.Ocularr == "OS" && x.typee == 134).Select(x => x.add).FirstOrDefault(),

                                                         DistSph2 = res.Where(x => x.Ocularr == "OD" && x.typee == 135).Select(x => x.disp).FirstOrDefault(),
                                                         Add2 = res.Where(x => x.Ocularr == "OD" && x.typee == 135).Select(x => x.add).FirstOrDefault(),

                                                         DistSph3 = res.Where(x => x.Ocularr == "OS" && x.typee == 135).Select(x => x.disp).FirstOrDefault(),
                                                         Add3 = res.Where(x => x.Ocularr == "OS" && x.typee == 135).Select(x => x.add).FirstOrDefault(),

                                                         Remarks = res.Where(x => x.Ocularr == null).Select(x => x.remarkss).FirstOrDefault(),

                                                         PD = res.Where(x => x.typee == null).Select(x => x.pd).FirstOrDefault(),

                                                     }).ToList();

            var Ams = from va in WYNKContext.Refraction.Where(u => u.RegistrationTranID == RID && u.Description == "Amsler")


                      select new
                      {
                          VDate = va.CreatedUTC,
                          Descriptionn = va.Description,
                          Ocularr = va.Ocular,
                          createdby = va.CreatedBy,
                          anod = va.A_n_OD,
                          aabnod = va.A_abn_OD,
                          anos = va.A_n_OS,
                          aabnos = va.A_abn_OS,
                          desctext = va.Desc_Text,
                      };

            refractionget.refractionHisamsler = (from res in Ams.GroupBy(x => x.createdby)
                                                 select new refractionHisamsler
                                                 {

                                                     Date = res.Select(x => x.VDate).FirstOrDefault(),
                                                     Description = res.Where(x => x.Ocularr == "OD").Select(x => x.Descriptionn).FirstOrDefault(),
                                                     A_n_OD = res.Where(x => x.Ocularr == "OD").Select(x => x.anod).FirstOrDefault(),
                                                     A_abn_OD = res.Where(x => x.Ocularr == "OD").Select(x => x.aabnod).FirstOrDefault(),
                                                     A_n_OS = res.Where(x => x.Ocularr == "OS").Select(x => x.anos).FirstOrDefault(),
                                                     A_abn_OS = res.Where(x => x.Ocularr == "OS").Select(x => x.aabnos).FirstOrDefault(),
                                                     Desc_Text = res.Where(x => x.Ocularr == "OD").Select(x => x.desctext).FirstOrDefault(),
                                                     Desc_Text1 = res.Where(x => x.Ocularr == "OS").Select(x => x.desctext).FirstOrDefault(),

                                                 }).ToList();


            var t = WYNKContext.Refraction.Where(x => x.RegistrationTranID == RID).Select(x => x.ID).LastOrDefault();
            var g = WYNKContext.SquintMaster.Where(x => x.RefractionID == t).Select(x => x.ID).LastOrDefault();

            


            return refractionget;



        }

        public PatientHistoryViewModel Getopitcaldetails(string Getuin)

        {

            var opthistory = new PatientHistoryViewModel();


            var doctormaster = CMPSContext.DoctorMaster.ToList();
            var registration = WYNKContext.Registration.ToList();
            var user = CMPSContext.Users.ToList();

            var groups = (from REF in WYNKContext.OpticalPrescription.Where(u => u.UIN == Getuin)
                          join us in user on
                          REF.CreatedBy equals us.Userid
                          join doc in doctormaster on
                          us.Username equals doc.EmailID


                          select new
                          {

                              prescribedby = doc.LastName,
                              prescribeddate = REF.CreatedUTC,
                              rid = REF.RegistrationTranID,


                          }).ToList();



            opthistory.opticalhistory1 = (from res in groups.GroupBy(x => x.rid)
                                          select new opticalhistory1
                                          {
                                              Prescribedby1 = res.Select(x => x.prescribedby).FirstOrDefault(),
                                              prescribeddate1 = res.Select(x => x.prescribeddate).FirstOrDefault(),
                                              regid1 = res.Select(x => x.rid).FirstOrDefault(),

                                          }).ToList();

            return opthistory;

        }

        public dynamic Getopticalprescription(int RID)

        {
            var opticalpreget = new PatientHistoryViewModel();
            opticalpreget.OpticalpreHisfnalpre = new List<OpticalpreHisfnalpre>();
            var onelinemaster = CMPSContext.OneLineMaster.ToList();




            var fin = from va in WYNKContext.OpticalPrescription.Where(u => u.RegistrationTranID == RID)


                      select new
                      {
                          VDate = va.CreatedUTC,
                          Ocularr = va.Ocular,
                          createdby = va.CreatedBy,
                          disp = va.DistSph,
                          near = va.NearCyl,
                          pin = va.PinAxis,
                          add = onelinemaster.Where(x => x.OLMID == Convert.ToInt32(va.Add)).Select(x => x.ParentDescription).FirstOrDefault(),
                          remarkss = va.Remarks,
                          typee = va.Type,
                          pd = va.PD,
                          mpdod = va.MPDOD,
                          mpdos = va.MPDOS

                      };


            opticalpreget.OpticalpreHisfnalpre = (from res in fin.GroupBy(x => x.createdby)
                                                  select new OpticalpreHisfnalpre
                                                  {

                                                      Date = res.Select(x => x.VDate).FirstOrDefault(),
                                                      DistSph = res.Where(x => x.Ocularr == "OD" && x.typee == 134).Select(x => x.disp).FirstOrDefault(),
                                                      NearCyl = res.Where(x => x.Ocularr == "OD" && x.typee == 134).Select(x => x.near).FirstOrDefault(),
                                                      PinAxis = res.Where(x => x.Ocularr == "OD" && x.typee == 134).Select(x => x.pin).FirstOrDefault(),
                                                      Add = res.Where(x => x.Ocularr == "OD" && x.typee == 134).Select(x => x.add).FirstOrDefault(),

                                                      DistSph1 = res.Where(x => x.Ocularr == "OS" && x.typee == 134).Select(x => x.disp).FirstOrDefault(),
                                                      NearCyl1 = res.Where(x => x.Ocularr == "OS" && x.typee == 134).Select(x => x.near).FirstOrDefault(),
                                                      PinAxis1 = res.Where(x => x.Ocularr == "OS" && x.typee == 134).Select(x => x.pin).FirstOrDefault(),
                                                      Add1 = res.Where(x => x.Ocularr == "OS" && x.typee == 134).Select(x => x.add).FirstOrDefault(),

                                                      DistSph2 = res.Where(x => x.Ocularr == "OD" && x.typee == 135).Select(x => x.disp).FirstOrDefault(),
                                                      Add2 = res.Where(x => x.Ocularr == "OD" && x.typee == 135).Select(x => x.add).FirstOrDefault(),

                                                      DistSph3 = res.Where(x => x.Ocularr == "OS" && x.typee == 135).Select(x => x.disp).FirstOrDefault(),
                                                      Add3 = res.Where(x => x.Ocularr == "OS" && x.typee == 135).Select(x => x.add).FirstOrDefault(),

                                                      Remarks = res.Where(x => x.Ocularr == null).Select(x => x.remarkss).FirstOrDefault(),

                                                      PD = res.Where(x => x.typee == null).Select(x => x.pd).FirstOrDefault(),
                                                      MPD = res.Where(x => x.typee == null).Select(x => x.mpdod).FirstOrDefault(),
                                                      MPD1 = res.Where(x => x.typee == null).Select(x => x.mpdos).FirstOrDefault(),


                                                  }).ToList();








            return opticalpreget;



        }



        public dynamic DeletesystemicCondition(int ID, string Reasons)
        {


            var M_systemiccondDel = new PatientHistory();
            M_systemiccondDel = WYNKContext.PatientHistory.Where(x => x.ID == ID && x.IsActive == true).SingleOrDefault();


            M_systemiccondDel.IsActive = false;
            M_systemiccondDel.Reasons = Reasons;


            WYNKContext.PatientHistory.UpdateRange(M_systemiccondDel);
            WYNKContext.SaveChanges();



            try
            {
                if (WYNKContext.SaveChanges() >= 0)
                    return new
                    {
                        Success = true,
                        Message = "Delete successfully"
                    };
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



        public dynamic updateSystemicConditions(PatientHistoryViewModels PatientHistoryViewModels, int ID)
        {
            try
            {
                var PatientHistory = new PatientHistory();
                PatientHistory = WYNKContext.PatientHistory.Where(x => x.ID == ID).SingleOrDefault();
                var item2 = PatientHistoryViewModels.listofHistory;
                var IID = item2.Select(x => x.ID).FirstOrDefault();
                var OLm = new OneLine_Masters();
                var CID = WYNKContext.PatientHistory.Where(x => x.ID == IID).Select(x => x.Description).FirstOrDefault();
                OLm = CMPSContext.OneLineMaster.Where(x => x.OLMID == CID).FirstOrDefault();
                string DDESC = item2.Select(x => x.Description).FirstOrDefault();
                OLm.ParentDescription = DDESC;

                CMPSContext.OneLineMaster.UpdateRange(OLm);
                CMPSContext.SaveChanges();


                var disc = item2.Select(x => x.Description).FirstOrDefault();

                var Desc = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "SystemicCondition"
                && x.ParentDescription == disc).Select(x => x.OLMID).FirstOrDefault();

                PatientHistory.Description = Desc;
                PatientHistory.FromUTC = item2.Select(x => x.FromDate).FirstOrDefault();
                PatientHistory.UpdatedUTC = DateTime.UtcNow;
                PatientHistory.UpdatedBy = PatientHistoryViewModels.USERID;
                PatientHistory.UIN = item2.Select(x => x.UIN).FirstOrDefault();
                WYNKContext.PatientHistory.UpdateRange(PatientHistory);
                WYNKContext.SaveChanges();
                try
                {
                    if (CMPSContext.SaveChanges() >= 0)
                        return new
                        {
                            Success = true,
                            Message = "update successfully"
                        };
                }
                catch (Exception ex)
                {
                    Console.Write(ex);
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


        public dynamic SubmitCurrentMedication(PatientCurrentMed PatientCurrentMedication, string UIN, int cmpid)
        {
            try
            {
                var PatCurrentMedication = WYNKContext.PatientCurrentMedication.Where(x => x.UIN == UIN);
                if (PatCurrentMedication != null)
                {
                    WYNKContext.PatientCurrentMedication.RemoveRange(WYNKContext.PatientCurrentMedication.Where(x => x.UIN == UIN).ToList());
                    WYNKContext.SaveChanges();
                }


                foreach (var item in PatientCurrentMedication.CurrentMedications.ToList())
                {
                    var PatCurMedication = new PatientCurrentMedication();
                    PatCurMedication.GenericDrugDescription = item.GenericDescription;
                    PatCurMedication.RegistrationTranID = WYNKContext.RegistrationTran.Where(x => x.UIN == UIN).Select(x => x.RegistrationTranID).LastOrDefault();
                    PatCurMedication.UIN = UIN;
                    PatCurMedication.VisitDate = DateTime.Now;
                    if (item.Eye == "OD")
                    {
                        PatCurMedication.IsOD = true;
                    }
                    else if (item.Eye == "OS")
                    {
                        PatCurMedication.IsOS = true;
                    }
                    else
                    {
                        PatCurMedication.IsOU = true;
                    }
                    PatCurMedication.Since = Convert.ToDateTime(item.YearMonths);
                    PatCurMedication.ProgressStatusID = Convert.ToInt32(item.Status);

                    PatCurMedication.Frequency = item.FrequencyID;
                    PatCurMedication.PrescribedByDoctorName = item.PrescribedDoctor;
                    PatCurMedication.Cmpid = cmpid;
                    WYNKContext.PatientCurrentMedication.Add(PatCurMedication);
                }
                WYNKContext.SaveChanges();


                return new
                {
                    Success = true,
                    Message = "Saved successfully",
                };

            }
            catch (Exception ex)
            {

                if (ex.InnerException != null)
                {
                    ErrorLog oErrorLog = new ErrorLog();
                    oErrorLog.WriteErrorLog("Error :", ex.InnerException.Message.ToString());
                }
                else 
                {
                    ErrorLog oErrorLog = new ErrorLog();
                    oErrorLog.WriteErrorLog("Error :", ex.Message.ToString());
                }

                return new
                {
                    Success = false,
                    Message = ex.Message
                };


            }
        }

        public dynamic GetCurrentMedication(string UIN, List<ListCompanyIds> cmpid)
        {
            try
            {
                var onelinemaster = CMPSContext.OneLineMaster.ToList();

                var currentMedication = (from res in WYNKContext.PatientCurrentMedication.Where(x => x.UIN == UIN)
                                         where cmpid.Select(x => x.CompanyIDS).Contains(res.Cmpid)
                                         select new CurrentMedication
                                         {
                                             GenericDescription = res.GenericDrugDescription,
                                             Eye = getEyeDetail(res.IsOD, res.IsOS, res.IsOU),
                                             FrequencyID = Convert.ToInt32(res.Frequency),
                                             Since = res.Since != null ?SinceCalculation(res.Since) : " ",
                                             Frequency = Convert.ToString(res.Frequency),
                                             Status = Convert.ToString(res.ProgressStatusID),
                                             YearMonths = res.Since != null ? Convert.ToDateTime(res.Since).ToString("yyy \\/ MMM") : " ",
                                             FromDB = true,
                                             Disabled = true,
                                             PrescribedDoctor = res.PrescribedByDoctorName,
                                             Remarks = res.Remarks
                                         }).ToList();


                currentMedication.Reverse();

                if (currentMedication.Count >= 1)
                {
                    return new
                    {
                        Success = true,
                        currentMedicationList = currentMedication,
                    };
                }
                else
                {
                    return new
                    {
                        Success = false,
                        Message = "No Current Medication data"
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
                Message = "Something Went Wrong"
            };
        }

        public string getEyeDetail(bool? IsOD, bool? IsOS, bool? IsOU)
        {
            string res = "";

            if (Convert.ToBoolean(IsOD))
            {
                res = Enum.GetName(typeof(Ocular), 2);
            }
            else if (Convert.ToBoolean(IsOS))
            {
                res = Enum.GetName(typeof(Ocular), 1);
            }
            else if (Convert.ToBoolean(IsOU))
            {
                res = Enum.GetName(typeof(Ocular), 0);
            }
            else 
            {
                res = null;
            }
            return res;
        }

        public string SinceCalculation(DateTime? Since)
        {
            DateTime today = DateTime.Today;

            int months = today.Month - Convert.ToDateTime(Since).Month;
            int years = today.Year - Convert.ToDateTime(Since).Year;

            if (today.Day < Convert.ToDateTime(Since).Day)
            {
                months--;
            }

            if (months < 0)
            {
                years--;
                months += 12;
            }

            if (months == 0 && years == 0)
            {
                return "Current Month";

            }
            else if (years == 0)
            {
                return string.Format("{0} month{1}",
                         months, (months == 1) ? "" : "s"
                       );
            }
            else
            {
                return string.Format("{0} year{1}, {2} month{3}",
                               years, (years == 1) ? "" : "s",
                               months, (months == 1) ? "" : "s"
                             );
            }
        }

        public dynamic InsertSurgeryHistory(SurgeryHistoryViewModel SurgeryHistoryViewModel)
        {
            try
            {

                string cmpname = CMPSContext.Company.Where(x => x.CmpID == SurgeryHistoryViewModel.Cmpid).Select(x => x.CompanyName).FirstOrDefault();
                string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == SurgeryHistoryViewModel.CreatedBy).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                string userid = Convert.ToString(SurgeryHistoryViewModel.CreatedBy);
                ErrorLog oErrorLogs = new ErrorLog();
                oErrorLogs.WriteErrorLogTitle(cmpname, "Surgery History", "User name :", username, "User ID :", userid, "Mode : Add");

                foreach (var item in SurgeryHistoryViewModel.SurgeryHistoryDetails.ToList())
                {
                    var SurgeryHistory = new SurgeryHistory();
                    SurgeryHistory.Cmpid = item.Cmpid;
                    SurgeryHistory.UIN = item.UIN;
                    SurgeryHistory.DateofSurgery = item.DateOfSurgery !=null ? Convert.ToDateTime(item.DateOfSurgery).AddDays(1) : (DateTime?)null;
                    SurgeryHistory.TypeofSurgery = item.TypeofSurgery;
                    SurgeryHistory.SurgeonName = item.SurgeonName;
                    SurgeryHistory.HospitalOrClinic = item.HospitalClinic;
                    SurgeryHistory.Eye = item.Eye;
                    SurgeryHistory.IsDeleted = false;
                    SurgeryHistory.SurgeryDone = "External";
                    SurgeryHistory.CreatedUTC = DateTime.UtcNow;
                    SurgeryHistory.CreatedBy = SurgeryHistoryViewModel.CreatedBy;
                    WYNKContext.SurgeryHistory.Add(SurgeryHistory);


                    object names = SurgeryHistory;
                    oErrorLogs.WriteErrorLogArray("SurgeryHistory", names);
                }

                WYNKContext.SaveChanges();

                return new
                {
                    Success = true,
                    Message = "Save Successfully",
                    SurgeryHistoryList = (from Sh in WYNKContext.SurgeryHistory.Where(x => x.UIN == SurgeryHistoryViewModel.SurgeryHistoryDetails[0].UIN && x.IsDeleted == false)
                                          where SurgeryHistoryViewModel.ListCmpid.Select(x => x.CompanyIDS).Contains(Sh.Cmpid)
                                          orderby Sh.DateofSurgery
                                          select new
                                          {
                                              Cmpid = Sh.Cmpid,
                                              UIN = Sh.UIN,
                                              DateOfSurgery = Sh.DateofSurgery,
                                              TypeofSurgery = Sh.TypeofSurgery,
                                              SurgeonName = Sh.SurgeonName,
                                              HospitalClinic = Sh.HospitalOrClinic,
                                              Eye = Sh.Eye,
                                              SurgeryDone = Sh.SurgeryDone,
                                              Status = "Saved",
                                              RemovedReason = Sh.RemovedReason,
                                              Remarks = Sh.Remarks,
                                              SurgeryId = Sh.SurgeryID,
                                              ID = Sh.ID,
                                          }).ToList(),
                };


            }
            catch (Exception ex)
            {

                return new
                {
                    Success = false,
                    Message = "SomeThing Went Wrong"
                };
            }

        }

        public dynamic getSurgeryHistory(List<ListCompanyIds> cmpid, string UIN)
        {
            try
            {
                var SHL = (from Sh in WYNKContext.SurgeryHistory.Where(x => x.UIN == UIN && x.IsDeleted == false)
                           where cmpid.Select(x => x.CompanyIDS).Contains(Sh.Cmpid)
                           orderby Sh.DateofSurgery
                           select new
                           {
                               Cmpid = Sh.Cmpid,
                               UIN = Sh.UIN,
                               DateOfSurgery = Sh.DateofSurgery,
                               TypeofSurgery = Sh.TypeofSurgery,
                               SurgeonName = Sh.SurgeonName,
                               HospitalClinic = Sh.HospitalOrClinic,
                               Eye = Sh.Eye,
                               SurgeryDone = Sh.SurgeryDone,
                               Status = "Saved",
                               RemovedReason = Sh.RemovedReason,
                               Remarks = Sh.Remarks,
                               SurgeryId = Sh.SurgeryID,
                               ID = Sh.ID,
                           }).ToList();



                return new
                {
                    Success = true,
                    Message = "Save Successfully",
                    SurgeryHistoryList = SHL,
                };
            }
            catch (Exception ex)
            {

                return new
                {
                    Success = false,
                    Message = "SomeThing Went Wrong"
                };
            }
        }

        public dynamic GetSurgeryOtNotes(string SurgeryId, int Cmpid)
        {
            try
            {
                var DocMast = CMPSContext.DoctorMaster.Where(x => x.CMPID == Cmpid).ToList();
                var DrugMast = WYNKContext.DrugMaster.Where(x => x.Cmpid == Cmpid).ToList();
                var IoMId = WYNKContext.IOMaster.Where(x => x.Surgeryid == SurgeryId && x.CMPID == Cmpid).Select(x => x.ID).FirstOrDefault();

                var Iotran = (from Iotrans in WYNKContext.IOTran.Where(x => x.IOMID == IoMId)
                              select new
                              {
                                  Description = Iotrans.OTNotesDescription,
                              }).ToList();

                var SurgeryDoctor = string.Join(",", WYNKContext.SurgeryAssignedTran
                                                .Where(surgAssTran => surgAssTran.SurgeryID == SurgeryId)
                                                .Select(surgAssTran => DocMast
                                                    .Where(DM => DM.DoctorID == surgAssTran.DoctorID)
                                                    .Select(DM => DM.Firstname + (DM.MiddleName != null ? DM.MiddleName : "") + DM.LastName).FirstOrDefault()));


                var DrugUsed = (from Surgery in WYNKContext.Surgery.Where(x => x.RandomUniqueID == SurgeryId && x.CMPID == Cmpid)
                                join stockMaster in WYNKContext.StockMaster on Surgery.SMID equals stockMaster.RandomUniqueID
                                join stockTran in WYNKContext.StockTran on stockMaster.RandomUniqueID equals stockTran.SMID
                                select new
                                {
                                    Brand = WYNKContext.DrugMaster.Where(x => x.Cmpid == Cmpid && x.ID == stockTran.ItemID).Select(x=>x.Brand).FirstOrDefault(),
                                    Generic = WYNKContext.DrugGroup.Where(x => x.Cmpid == Cmpid && x.ID == WYNKContext.DrugMaster.Where(y => y.Cmpid == Cmpid && y.ID == stockTran.ItemID).Select(y => y.GenericName).FirstOrDefault()).Select(x => x.Description).FirstOrDefault(),
                                    Qty = stockTran.ItemQty,
                                    UOM = WYNKContext.DrugMaster.Where(x => x.Cmpid == Cmpid && x.ID == stockTran.ItemID).Select(x => x.UOM).FirstOrDefault(),
                                }).ToList();


                var IsDischarge = WYNKContext.SurgeryDischarge.Where(x => x.SurgeryID == SurgeryId).FirstOrDefault();
                dynamic DischargeDrugUsed = null;
                if (IsDischarge != null) {

                  DischargeDrugUsed = (from MedPres in WYNKContext.MedicalPrescription.Where(x => x.SurgeryID == SurgeryId)
                                            join MedPresTran in WYNKContext.MedicalPrescriptionTran on MedPres.RandomUniqueID equals MedPresTran.MedicalPrescriptionID
                                            select new
                                            {
                                                Brand = WYNKContext.DrugMaster.Where(x => x.Cmpid == Cmpid && x.ID == MedPresTran.DrugId).Select(x => x.Brand).FirstOrDefault(),
                                                Generic = WYNKContext.DrugGroup.Where(x => x.Cmpid == Cmpid && x.ID == WYNKContext.DrugMaster.Where(y => y.Cmpid == Cmpid && y.ID == MedPresTran.DrugId).Select(y => y.GenericName).FirstOrDefault()).Select(x => x.Description).FirstOrDefault(),
                                                Qty = MedPresTran.Quantity,
                                                UOM = WYNKContext.DrugMaster.Where(x => x.Cmpid == Cmpid && x.ID == MedPresTran.DrugId).Select(x => x.UOM).FirstOrDefault(),
                                            }).ToList();

                }


                return new
                {
                    Success = true,
                    Iotran = Iotran,
                    DrugUsed= DrugUsed,
                    DischargeDrugUsed = DischargeDrugUsed,
                    SurgeryDoctor = SurgeryDoctor
                };
            }
            catch (Exception)
            {

                return new
                {
                    Success = false,
                };
            }


        }




        public dynamic InsertAllergy(PatientHistoryViewModel InsertAllergy, string uin)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {


                    


                    if (InsertAllergy.AllergyTran != null)
                    {
                        if (InsertAllergy.AllergyTran.Count() > 0)
                        {

                            foreach (var item in InsertAllergy.AllergyTran.ToList())
                            {
                                var allergy = new AllergyTran();
                                var ID = item.ID;

                                if (ID != 0)
                                {
                                    allergy = WYNKContext.AllergyTran.Where(x => x.ID == item.ID).FirstOrDefault();
                                    allergy.CmpID = item.CmpID;
                                    allergy.UIN = item.UIN;
                                    var rid = WYNKContext.RegistrationTran.Where(x => x.UIN == item.UIN).Select(x => x.RegistrationTranID).LastOrDefault();
                                    allergy.RegID = rid;
                                    allergy.Description = item.Description;
                                    allergy.Type = item.Type;
                                    allergy.FromUTC = item.FromUTC;
                                    allergy.IsActive = true;
                                    allergy.Period = item.Period;
                                    allergy.Since = item.Since;
                                    allergy.UpdatedUTC = DateTime.UtcNow;
                                    allergy.UpdatedBy = item.UpdatedBy;
                                    WYNKContext.AllergyTran.UpdateRange(allergy);
                                    string cmpname = CMPSContext.Company.Where(x => x.CmpID == item.CmpID).Select(x => x.CompanyName).FirstOrDefault();
                                    string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == item.CreatedBy).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                                    string userid = Convert.ToString(item.CreatedBy);
                                    ErrorLog oErrorLogs = new ErrorLog();
                                    oErrorLogs.WriteErrorLogTitle(cmpname, "AllergyTran", "User name :", username, "User ID :", userid, "Mode : Update");
                                    object names = allergy;
                                    oErrorLogs.WriteErrorLogArray("AllergyTran", names);
                                    WYNKContext.SaveChanges();

                                }
                                else
                                {

                                    allergy.CmpID = item.CmpID;
                                    allergy.UIN = item.UIN;
                                    var rid = WYNKContext.RegistrationTran.Where(x => x.UIN == item.UIN).Select(x => x.RegistrationTranID).LastOrDefault();
                                    allergy.RegID = rid;
                                    allergy.Description = item.Description;
                                    allergy.Type = item.Type;
                                    allergy.FromUTC = item.FromUTC;
                                    allergy.IsActive = true;
                                    allergy.Period = item.Period;
                                    allergy.Since = item.Since;
                                    allergy.CreatedUTC = DateTime.UtcNow;
                                    allergy.CreatedBy = item.CreatedBy;
                                    WYNKContext.AllergyTran.AddRange(allergy);
                                    string cmpname = CMPSContext.Company.Where(x => x.CmpID == item.CmpID).Select(x => x.CompanyName).FirstOrDefault();
                                    string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == item.CreatedBy).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                                    string userid = Convert.ToString(item.CreatedBy);
                                    ErrorLog oErrorLogs = new ErrorLog();
                                    oErrorLogs.WriteErrorLogTitle(cmpname, "AllergyTran", "User name :", username, "User ID :", userid, "Mode : Sumbit");
                                    object names = allergy;
                                    oErrorLogs.WriteErrorLogArray("AllergyTran", names);
                                    WYNKContext.SaveChanges();


                                }


                            }
                        }
                    }


                    var page = new PatientGeneralDatamodel();

                    var rrid = WYNKContext.RegistrationTran.Where(x => x.UIN == uin).OrderByDescending(x => x.CreatedUTC).Select(x => x.RegistrationTranID).FirstOrDefault();
                    page.UIN = uin;
                    page.RegistrationTranID = rrid;
                    page.VisitDate = DateTime.UtcNow;
                    page.Allergy = InsertAllergy.Patientgeneral.Allergy;
                    page.FamilyHistory = "NULL";
                    page.IsDeleted = false;
                    page.CreatedBy = 1;
                    page.CreatedUTC = DateTime.UtcNow;
                    WYNKContext.PatientGeneral.Add(page);
                    WYNKContext.SaveChanges();

                    dbContextTransaction.Commit();

                    if (WYNKContext.SaveChanges() > 0)
                    {
                        ErrorLog oErrorLog = new ErrorLog();
                        oErrorLog.WriteErrorLog("Information :", "Saved Successfully");
                    }


                    return new
                    {
                        Success = true,
                        data = GetallergyDetailsrecent(uin)
                    };
                }

                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                    ErrorLog oErrorLog = new ErrorLog();
                    oErrorLog.WriteErrorLog("Error :", ex.InnerException.Message.ToString());
                }
                return new
                {
                    Success = false,
                };
            }
        }


        public dynamic InsertPAC(PatientHistoryViewModel InsertPAC)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {

                    var regid = WYNKContext.PACExamination.Where(x => x.RegistrationTranID == (WYNKContext.RegistrationTran.Where(u => u.UIN == InsertPAC.PACExamination.UIN).OrderByDescending(u => u.CreatedUTC).Select(u => u.RegistrationTranID).FirstOrDefault())).ToList();
                    if (regid.Count == 0)
                    {

                        var PACExam = new PACExamination();




                        PACExam.CMPID = InsertPAC.PACExamination.CMPID;
                        PACExam.UIN = InsertPAC.PACExamination.UIN;
                        PACExam.RegistrationTranID = WYNKContext.RegistrationTran.Where(x => x.UIN == InsertPAC.PACExamination.UIN).OrderByDescending(x => x.CreatedUTC).Select(x => x.RegistrationTranID).FirstOrDefault();
                        PACExam.PulseHeartRate = InsertPAC.PACExamination.PulseHeartRate;
                        PACExam.BloodPressure = InsertPAC.PACExamination.BloodPressure;
                        PACExam.Respiration = InsertPAC.PACExamination.Respiration;
                        PACExam.temperature = InsertPAC.PACExamination.temperature;
                        PACExam.Weight = InsertPAC.PACExamination.Weight;
                        PACExam.Height = InsertPAC.PACExamination.Height;
                        PACExam.WtUnit = InsertPAC.PACExamination.WtUnit;
                        PACExam.HtUnit = InsertPAC.PACExamination.HtUnit;
                        PACExam.BMI = InsertPAC.PACExamination.BMI;
                        PACExam.TempUnit = InsertPAC.PACExamination.TempUnit;
                        PACExam.CreatedUTC = DateTime.UtcNow;
                        PACExam.CreatedBy = InsertPAC.PACExamination.CreatedBy;
                        WYNKContext.PACExamination.AddRange(PACExam);
                        WYNKContext.SaveChanges();
                    }

                    else
                    {
                        var masters = WYNKContext.PACExamination.Where(x => x.RegistrationTranID == (WYNKContext.RegistrationTran.Where(u => u.UIN == InsertPAC.PACExamination.UIN).OrderByDescending(u => u.CreatedUTC).Select(u => u.RegistrationTranID).FirstOrDefault())).FirstOrDefault();
                        masters.CMPID = InsertPAC.PACExamination.CMPID;
                        masters.UIN = InsertPAC.PACExamination.UIN;
                        masters.RegistrationTranID = WYNKContext.RegistrationTran.Where(x => x.UIN == InsertPAC.PACExamination.UIN).OrderByDescending(x => x.CreatedUTC).Select(x => x.RegistrationTranID).FirstOrDefault();
                        masters.PulseHeartRate = InsertPAC.PACExamination.PulseHeartRate;
                        masters.BloodPressure = InsertPAC.PACExamination.BloodPressure;
                        masters.Respiration = InsertPAC.PACExamination.Respiration;
                        masters.temperature = InsertPAC.PACExamination.temperature;
                        masters.Weight = InsertPAC.PACExamination.Weight;
                        masters.Height = InsertPAC.PACExamination.Height;
                        masters.WtUnit = InsertPAC.PACExamination.WtUnit;
                        masters.HtUnit = InsertPAC.PACExamination.HtUnit;
                        masters.BMI = InsertPAC.PACExamination.BMI;
                        masters.TempUnit = InsertPAC.PACExamination.TempUnit;
                        masters.UpdatedUTC = DateTime.UtcNow;
                        masters.UpdatedBy = InsertPAC.PACExamination.CreatedBy;
                        WYNKContext.PACExamination.UpdateRange(masters);
                        WYNKContext.SaveChanges();
                    }


                        dbContextTransaction.Commit();


                    if (WYNKContext.SaveChanges() >= 0)
                    {
                        ErrorLog oErrorLog = new ErrorLog();
                        oErrorLog.WriteErrorLog("Information :", "Saved Successfully");
                    }


                    return new
                    {
                        Success = true,
                    };
                }

                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                    ErrorLog oErrorLog = new ErrorLog();
                    oErrorLog.WriteErrorLog("Error :", ex.InnerException.Message.ToString());
                }
                return new
                {
                    Success = false,
                };
            }
        }


        public dynamic InsertBH(PatientHistoryViewModel InsertBH)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {

                    var regid = WYNKContext.BirthHistory.Where(x => x.RegistrationTranID == (WYNKContext.RegistrationTran.Where(u => u.UIN == InsertBH.BirthHistory.UIN).OrderByDescending(u => u.CreatedUTC).Select(u => u.RegistrationTranID).FirstOrDefault())).ToList();
                    if (regid.Count == 0)
                    {

                        var BH = new BirthHistory();




                        BH.CMPID = InsertBH.BirthHistory.CMPID;
                        BH.UIN = InsertBH.BirthHistory.UIN;
                        BH.RegistrationTranID = WYNKContext.RegistrationTran.Where(x => x.UIN == InsertBH.BirthHistory.UIN).OrderByDescending(x => x.CreatedUTC).Select(x => x.RegistrationTranID).FirstOrDefault();
                        BH.DeliveryType = InsertBH.BirthHistory.DeliveryType;
                        BH.Weight = InsertBH.BirthHistory.Weight;
                        BH.BloodTransplant = InsertBH.BirthHistory.BloodTransplant;
                        BH.OxygenTherapy = InsertBH.BirthHistory.OxygenTherapy;
                        BH.criticalillness = InsertBH.BirthHistory.criticalillness;
                        BH.Vaccination = InsertBH.BirthHistory.Vaccination;
                        BH.GrowthDev = InsertBH.BirthHistory.GrowthDev;
                        BH.Unit = InsertBH.BirthHistory.Unit;
                        BH.CreatedUTC = DateTime.UtcNow;
                        BH.CreatedBy = InsertBH.BirthHistory.CreatedBy;
                        WYNKContext.BirthHistory.AddRange(BH);
                        WYNKContext.SaveChanges();
                    }

                    else
                    {
                        var masters = WYNKContext.BirthHistory.Where(x => x.RegistrationTranID == (WYNKContext.RegistrationTran.Where(u => u.UIN == InsertBH.BirthHistory.UIN).OrderByDescending(u => u.CreatedUTC).Select(u => u.RegistrationTranID).FirstOrDefault())).FirstOrDefault();
                        masters.CMPID = InsertBH.BirthHistory.CMPID;
                        masters.UIN = InsertBH.BirthHistory.UIN;
                        masters.RegistrationTranID = WYNKContext.RegistrationTran.Where(x => x.UIN == InsertBH.BirthHistory.UIN).OrderByDescending(x => x.CreatedUTC).Select(x => x.RegistrationTranID).FirstOrDefault();
                        masters.DeliveryType = InsertBH.BirthHistory.DeliveryType;
                        masters.Weight = InsertBH.BirthHistory.Weight;
                        masters.BloodTransplant = InsertBH.BirthHistory.BloodTransplant;
                        masters.OxygenTherapy = InsertBH.BirthHistory.OxygenTherapy;
                        masters.criticalillness = InsertBH.BirthHistory.criticalillness;
                        masters.Vaccination = InsertBH.BirthHistory.Vaccination;
                        masters.GrowthDev = InsertBH.BirthHistory.GrowthDev;
                        masters.Unit = InsertBH.BirthHistory.Unit;
                        masters.UpdatedUTC = DateTime.UtcNow;
                        masters.UpdatedBy = InsertBH.BirthHistory.CreatedBy;
                        WYNKContext.BirthHistory.UpdateRange(masters);
                        WYNKContext.SaveChanges();
                    }


                    dbContextTransaction.Commit();


                    if (WYNKContext.SaveChanges() >= 0)
                    {
                        ErrorLog oErrorLog = new ErrorLog();
                        oErrorLog.WriteErrorLog("Information :", "Saved Successfully");
                    }


                    return new
                    {
                        Success = true,
                    };
                }

                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                    ErrorLog oErrorLog = new ErrorLog();
                    oErrorLog.WriteErrorLog("Error :", ex.InnerException.Message.ToString());
                }
                return new
                {
                    Success = false,
                };
            }
        }

        public dynamic getpacxam(string uin, int CmpID)
        {
            var getData = new PatientHistoryViewModel();

            var PACExam = WYNKContext.PACExamination.ToList();
            var regid = WYNKContext.PACExamination.Where(x => x.UIN == uin).OrderByDescending(x => x.CreatedUTC).Select(x => x.RegistrationTranID).FirstOrDefault();


            getData.getPACExamDetails = (from PACE in PACExam.Where(x => x.RegistrationTranID == regid && x.CMPID == CmpID)


                                     select new getPACExamDetails
                                     {
                                         PACExamID = PACE.PACExamID,
                                         PulseHeartRate = PACE.PulseHeartRate,
                                         Respiration = PACE.Respiration,
                                         BloodPressure = PACE.BloodPressure,
                                         temperature = PACE.temperature,
                                         Weight = PACE.Weight,
                                         Height = PACE.Height,
                                         Weightunit = PACE.WtUnit,
                                         Heightunit = PACE.HtUnit,
                                         BMI = PACE.BMI,
                                         }).ToList();






            return getData;

        }

        public dynamic getbh(string uin, int CmpID)
        {
            var getData = new PatientHistoryViewModel();

            var birhis = WYNKContext.BirthHistory.ToList();
            var regid = WYNKContext.BirthHistory.Where(x => x.UIN == uin).OrderByDescending(x => x.CreatedUTC).Select(x => x.RegistrationTranID).FirstOrDefault();


            getData.getbhDetails = (from BH in birhis.Where(x => x.RegistrationTranID == regid && x.CMPID == CmpID)


                                         select new getbhDetails
                                         {
                                             delivery = BH.DeliveryType,
                                             weight = BH.Weight,
                                             transplant = BH.BloodTransplant,
                                             therapy = BH.OxygenTherapy,
                                             illness = BH.criticalillness,
                                             vac = BH.Vaccination,
                                             gad = BH.GrowthDev,
                                             unit = BH.Unit,

                                         }).ToList();






            return getData;

        }


        public dynamic gethistorypacxam(string uin, int CmpID, string gmt)
        {
            var getData = new PatientHistoryViewModel();

            var PACExam = WYNKContext.PACExamination.ToList();
            TimeSpan ts = TimeSpan.Parse(gmt);

            getData.getPACExamhistoryDetails = (from PACE in PACExam.Where(x => x.UIN == uin && x.CMPID == CmpID).OrderByDescending(x=>x.CreatedUTC)


                                         select new getPACExamhistoryDetails
                                         {
                                             PulseHeartRate = PACE.PulseHeartRate,
                                             Respiration = PACE.Respiration,
                                             BloodPressure = PACE.BloodPressure,
                                             temperature = Convert.ToString(PACE.temperature) + " " + PACE.TempUnit,
                                             Weight = Convert.ToString(PACE.Weight) + " " + PACE.WtUnit,
                                             Height = Convert.ToString(PACE.Height) + " " + PACE.HtUnit,
                                             BMI = PACE.BMI,
                                             Dates = PACE.CreatedUTC.Add(ts),
                                         }).ToList();






            return getData;

        }

        public dynamic gethistorybh(string uin, int CmpID, string gmt)
        {
            var getData = new PatientHistoryViewModel();

            var Birthhis = WYNKContext.BirthHistory.ToList();
            TimeSpan ts = TimeSpan.Parse(gmt);

            getData.getPACExamhistoryDetailsbh = (from BH in Birthhis.Where(x => x.UIN == uin && x.CMPID == CmpID).OrderByDescending(x => x.CreatedUTC)


                                                select new getPACExamhistoryDetailsbh
                                                {
                                                    delivery = BH.DeliveryType,
                                                    weight = BH.Weight,
                                                    transplant = BH.BloodTransplant,
                                                    therapy = BH.OxygenTherapy,
                                                    illness = BH.criticalillness,
                                                    Dates = BH.CreatedUTC.Add(ts),
                                                    vac = BH.Vaccination,
                                                    gad = BH.GrowthDev,
                                                    unit = BH.Unit,
                                                }).ToList();






            return getData;

        }

        public dynamic gethistoryal(string uin, int CmpID, string gmt)
        {
            var getData = new PatientHistoryViewModel();

            var PatientGeneral = WYNKContext.PatientGeneral.ToList();
            TimeSpan ts = TimeSpan.Parse(gmt);


            getData.getallergyDetails = (from PG in PatientGeneral.Where(x => x.UIN == uin).OrderByDescending(x => x.CreatedUTC).GroupBy(x => x.RegistrationTranID)


                                                  select new getallergyDetails
                                                  {
                                                      date = PG.Select(x => x.CreatedUTC.Add(ts)).FirstOrDefault(),
                                                      desc = PG.Select(x => x.Allergy).FirstOrDefault(),
                                                      
                                                  }).ToList();






            return getData;

        }


        public dynamic GetallergyDetailsrecent(string uin)
        {
            var Details = new PatientHistoryViewModel();
            Details.AllergyTranrecent = new List<AllergyTranrecent>();

            

            var nondrug = (from REF in WYNKContext.AllergyTran.Where(x => x.IsActive == true && x.UIN == uin && x.Type != 4).OrderByDescending(x => x.CreatedUTC)
                                         select new
                                         {
                                             ID = REF.ID,
                                             Descriptionname = WYNKContext.allergy.Where(x => x.ID == REF.Description).Select(x => x.ParentDescription).FirstOrDefault(),
                                             Description = REF.Description,
                                             Typename = WYNKContext.allergy.Where(x => x.ID == REF.Type).Select(x => x.ParentDescription).FirstOrDefault(),
                                             Type = REF.Type,
                                             UIN = REF.UIN,
                                             RegID = REF.RegID,
                                             FromUTC = REF.FromUTC,
                                             IsActive = REF.IsActive,
                                             Since = REF.Since,
                                             Period = REF.Period,
                                             CmpID = REF.CmpID,
                                         }).ToList();

            var drug = (from REF in WYNKContext.AllergyTran.Where(x => x.IsActive == true && x.UIN == uin && x.Type == 4).OrderByDescending(x => x.CreatedUTC)
                           select new
                           {
                               ID = REF.ID,
                               Descriptionname = WYNKContext.DrugGroup.Where(x => x.ID == REF.Description).Select(x => x.Description).FirstOrDefault(),
                               Description = REF.Description,
                               Typename = WYNKContext.allergy.Where(x => x.ID == REF.Type).Select(x => x.ParentDescription).FirstOrDefault(),
                               Type = REF.Type,
                               UIN = REF.UIN,
                               RegID = REF.RegID,
                               FromUTC = REF.FromUTC,
                               IsActive = REF.IsActive,
                               Since = REF.Since,
                               Period = REF.Period,
                               CmpID = REF.CmpID,
                           }).ToList();

            var result = nondrug.Concat(drug);

            Details.AllergyTranrecent = (from REF in result
                                         select new AllergyTranrecent
                                         {
                                             ID = REF.ID,
                                             Descriptionname = REF.Descriptionname,
                                             Description = REF.Description,
                                             Typename = REF.Typename,
                                             Type = REF.Type,
                                             UIN = REF.UIN,
                                             RegID = REF.RegID,
                                             FromUTC = REF.FromUTC,
                                             IsActive = REF.IsActive,
                                             Since = REF.Since,
                                             Period = REF.Period,
                                             CmpID = REF.CmpID,
                                         }).ToList();

            Details.Patientgeneral = WYNKContext.PatientGeneral.OrderByDescending(x => x.CreatedUTC).FirstOrDefault(x => x.UIN == uin);

            if (Details.Patientgeneral == null)
            {

                Details.Patientgeneral = new PatientGeneralDatamodel();

            }


            return Details;
        }
        public dynamic GetallergyDetailshistory(string uin)
        {
            var Details = new PatientHistoryViewModel();
            Details.AllergyTranhistory = new List<AllergyTranhistory>();



            Details.AllergyTranhistory = (from REF in WYNKContext.AllergyTran.Where(x => x.UIN == uin && x.IsActive == true).OrderByDescending(x => x.CreatedUTC)


                                          select new AllergyTranhistory
                                          {
                                              Descriptionname = WYNKContext.allergy.Where(x => x.ID == REF.Description).Select(x => x.ParentDescription).FirstOrDefault(),
                                              Typename = WYNKContext.allergy.Where(x => x.ID == REF.Type).Select(x => x.ParentDescription).FirstOrDefault(),
                                              UIN = REF.UIN,
                                              FromUTC = REF.FromUTC,

                                          }


                                        ).ToList();


            return Details;
        }
        public dynamic DeleteFDDTSyringe(string uin, int ID, int Cmpid)
        {
            try
            {
                var SYRINGEFDDT = WYNKContext.SYRINGEFDDT.Where(x => x.ID == ID && x.UIN == uin && x.CMPID == Cmpid).FirstOrDefault();
                SYRINGEFDDT.IsActive = false;
                WYNKContext.SYRINGEFDDT.UpdateRange(SYRINGEFDDT);
                WYNKContext.SaveChanges();

                return new
                {
                    Success = true,
                };
            }
            catch (Exception)
            {
                return new
                {
                    Success = false,
                };
            }
        }
        public dynamic DeleteAllergy(string uin, int ID, int Cmpid)
        {
            try
            {
                var AllergyTran = WYNKContext.AllergyTran.Where(x => x.ID == ID && x.UIN == uin && x.CmpID == Cmpid).FirstOrDefault();
                AllergyTran.IsActive = false;
                WYNKContext.AllergyTran.UpdateRange(AllergyTran);
                WYNKContext.SaveChanges();
                return new
                {
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            return new
            {
                Success = false,
            };
        }
        public dynamic GetRemovedAllergyDetails(string uin)
        {

            try
            {
                var Details = new PatientHistoryViewModel();
                Details.AllergyTranhistory = new List<AllergyTranhistory>();

                Details.AllergyTranhistory = (from REF in WYNKContext.AllergyTran.Where(x => x.UIN == uin && x.IsActive == false).OrderByDescending(x => x.CreatedUTC)


                                              select new AllergyTranhistory
                                              {
                                                  Descriptionname = WYNKContext.allergy.Where(x => x.ID == REF.Description).Select(x => x.ParentDescription).FirstOrDefault(),
                                                  Typename = WYNKContext.allergy.Where(x => x.ID == REF.Type).Select(x => x.ParentDescription).FirstOrDefault(),
                                                  UIN = REF.UIN,
                                                  FromUTC = REF.FromUTC,

                                              }).ToList();

                if (Details.AllergyTranhistory.Count > 0)
                {
                    return new
                    {
                        Success = "Data Found",
                        data = Details,
                    };

                }
                else
                {
                    return new
                    {
                        Success = "No Data Found",
                    };

                }
            }
            catch (Exception)
            {

                return new
                {
                    Success = false,
                };
            }

        }

        public dynamic DeleteSurgeryHistory(SurgeryHistoryViewModel DeleteSurgeryHistory, int SurHisID)
        {
            try
            {
                var SurgHis = WYNKContext.SurgeryHistory.Where(x => x.Cmpid == DeleteSurgeryHistory.Cmpid && x.ID == SurHisID).FirstOrDefault();
                SurgHis.RemovedReason = DeleteSurgeryHistory.RemovedReasons;
                SurgHis.IsDeleted = true;
                SurgHis.UpdatedBy = DeleteSurgeryHistory.CreatedBy;
                SurgHis.UpdatedUTC = DateTime.UtcNow;
                WYNKContext.SurgeryHistory.UpdateRange(SurgHis);
                WYNKContext.SaveChanges();

                return new
                {
                    Success = true,
                };
            }
            catch (Exception)
            {
                return new
                {
                    Success = false,
                };
            }
        }
    }
}
