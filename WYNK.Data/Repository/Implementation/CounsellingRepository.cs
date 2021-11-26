using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Helpers;

namespace WYNK.Data.Repository.Implementation
{
    class CounsellingRepository : RepositoryBase<Counselling_Master>, ICounsellingRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;


        public CounsellingRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }



        public Counselling_Master getConcerntextfile(int CompanyID)
        {
            var Registrationmaster = new RegistrationMasterViewModel();
            var Counselmaster = new Counselling_Master();

            string[] lines;
            var list = new List<string>();
            var osfi = "/ConcernPages/" + CompanyID + '/';
            var osfn = "Counselling.text";
            var currentDir = Directory.GetCurrentDirectory();
            string path = currentDir + osfi + osfn;

            if (File.Exists(path))
            {
                var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        list.Add(line);
                    }
                }
                lines = list.ToArray();
                Counselmaster.TOtalLines = lines;
            }
            else
            {
                Counselmaster.ErrorMessaGE = "No Concern";
            }



            var newlist = new CounsellingChecklist();

            Counselmaster.CounsellingchecklistDetails = (from cc in WYNKContext.CounsellingChecklist.Where(x => x.IsDeleted == false)
                                                         select new ConslgDetails
                                                         {
                                                             Description = cc.Description,
                                                             ID = cc.ID,
                                                         }).ToList();



            return Counselmaster;
        }
        public Counselling_Master getsearchdetails(int CompanyID)
        {
            var Serachcounselpatients = new Counselling_Master();
            var UUINDETAILS = new List<UINDETAILS>();
            var UINID = WYNKContext.FindingsExt.Where(x => x.Isdeleted == false && x.SurgeryComplete == false).ToList();

            foreach (var item in UINID)
            {
                var newlist = new UINDETAILS();
                var id1 = item.FindingsID;
                if (item.CreatedBy != 0)
                {
                    newlist.SurgeryAdvicedby = "Admin";
                }
                else
                {
                    newlist.SurgeryAdvicedby = CMPSContext.DoctorMaster.Where(x => x.DoctorID == item.CreatedBy && x.CMPID == CompanyID).Select(x => x.LastName).FirstOrDefault();
                }
                var adviceddate = item.CreatedUTC.Date;
                var uinid1 = WYNKContext.Findings.Where(x => x.RandomUniqueID == id1 && x.CmpID == CompanyID).Select(x => x.UIN).FirstOrDefault();
                newlist.UIN = uinid1;
                newlist.Name = WYNKContext.Registration.Where(x => x.UIN == uinid1 && x.CMPID == CompanyID).Select(x => x.Name).FirstOrDefault();
                newlist.Gender = WYNKContext.Registration.Where(x => x.UIN == uinid1 && x.CMPID == CompanyID).Select(x => x.Gender).FirstOrDefault();
                var dupage = WYNKContext.Registration.Where(x => x.UIN == uinid1 && x.CMPID == CompanyID).Select(x => x.DateofBirth).FirstOrDefault();
                var agee = PasswordEncodeandDecode.ToAgeString(dupage);
                newlist.Age = agee;
                newlist.SurgeryAdviceddate = adviceddate;
                newlist.SpecialityDescriptiobn = WYNKContext.ICDSpecialityCode.Where(x => x.ID == item.ICDSpecialityid).Select(x => x.SpecialityDescription).FirstOrDefault();
                UUINDETAILS.Add(newlist);
            }

            var groupuindeetails = (from U in UUINDETAILS.GroupBy(x => x.UIN)
                                    select new UINDETAILS
                                    {
                                        UIN = U.FirstOrDefault().UIN,
                                        Name = U.FirstOrDefault().Name,
                                        Gender = U.FirstOrDefault().Gender,
                                        Age = U.FirstOrDefault().Age,
                                        SurgeryAdvicedby = U.FirstOrDefault().SurgeryAdvicedby,
                                        SurgeryAdviceddate = U.FirstOrDefault().SurgeryAdviceddate,
                                        SpecialityDescriptiobn = U.FirstOrDefault().SpecialityDescriptiobn,
                                    }).ToList();

            Serachcounselpatients.UINDETAILS = groupuindeetails;
            return Serachcounselpatients;
        }


        public Counselling_Master GetParticularLens(string Specdesc)
        {
            var Serachcounselpatients = new Counselling_Master();
            var Decss = WYNKContext.ICDSpecialityCode.Where(x => x.SpecialityDescription == Specdesc).Select(x => x.ID).ToList();
            Serachcounselpatients.Lensdetails = new List<Lensdetails>();

            foreach (var item in Decss)
            {
                var onelist = new Lensdetails();
                onelist.Lensdesc = WYNKContext.ICDMaster.Where(x => x.SpecialityCode == item).Select(x => x.ICDDESCRIPTION).FirstOrDefault();
                Serachcounselpatients.Lensdetails.Add(onelist);
            }


            return Serachcounselpatients;
        }



        public dynamic getuintotaldatahistory(int CompanyID, string UIN)
        {
         
            var Dummy = new Counselling_Master();

            Dummy.PatientCounsellingDtls = new PatientCounsellingDetails();
            Dummy.PaymentdetailsAdvance = new List<PaymentdetailsAdvance>();
        
            var onelinemaster = CMPSContext.OneLineMaster.ToList();
            DateTime now = DateTime.Now;
            var online = CMPSContext.OneLineMaster.ToList();
            var TranID = WYNKContext.RegistrationTran.Where(x => x.UIN == UIN).OrderByDescending(x => x.CreatedUTC).Select(x => x.RegistrationTranID).FirstOrDefault();
            var ocul = WYNKContext.OcularComplaints.Where(x => x.RegistrationTranID == TranID).ToList();
            if (ocul != null)
            {
                Dummy.ComplaintsDetails = ocul.Where(x => x.RegistrationTranID == TranID && x.IsDeleted == false)
         .OrderByDescending(x => x.CreatedUTC).Select(s => new ComplaintsDetailss
         {
             ID = s.ID,
             Description = online.Where(x => x.OLMID == s.Description).Select(x => x.ParentDescription).FirstOrDefault(),
             ISOD = s.ISOD,
             ISOU = s.ISOU,
             ISOS = s.ISOS
         }).ToList();
            }
            var ph = WYNKContext.PatientHistory.Where(X => X.UIN == UIN).ToList();
            if (ph.Count() != 0)
            {
                Dummy.PatientHistorys = ph.Where(x => x.IsActive == true && x.PatientUIN.UIN == UIN).Select(s => new PatientHistoryDetailss
                {
                    ID = s.ID,
                    Description = online.Where(x => x.OLMID == s.Description).Select(x => x.ParentDescription).FirstOrDefault(),
                    FromUTC = s.FromUTC,
                    Duration = MonthDifference(Convert.ToDateTime(s.FromUTC), now) + " Months"
                }).ToList();
            }
            Dummy.cmed = WYNKContext.PatientGeneral.Where(x => x.UIN == UIN).OrderByDescending(x => x.CreatedUTC).Select(x => x.CurrentMedication).FirstOrDefault();
            Dummy.all = WYNKContext.PatientGeneral.Where(x => x.UIN == UIN).OrderByDescending(x => x.CreatedUTC).Select(x => x.Allergy).FirstOrDefault();
            Dummy.fhis = WYNKContext.PatientGeneral.Where(x => x.UIN == UIN).OrderByDescending(x => x.CreatedUTC).Select(x => x.FamilyHistory).FirstOrDefault();
            return Dummy;

        }



        private static int MonthDifference(DateTime lValue, DateTime rValue)
        {
            return Math.Abs((lValue.Month - rValue.Month) + 12 * (lValue.Year - rValue.Year));
        }

        public Counselling_Master GetCnsDetail()
        {
            var dls = new Counselling_Master();
            dls.ConslgDetails = (from get in WYNKContext.CounsellingChecklist.Where(x => x.IsDeleted == false)
                                 select new ConslgDetails
                                 {

                                     ID = get.ID,
                                     Description = get.Description,
                                     Type1 = get.Type.ToString(),
                                     IsDeleted = get.IsDeleted,
                                     CreatedDate = get.CreatedUTC,

                                     Type = Enum.GetName(typeof(Typess), get.Type),

                                 }).ToList();

            return dls;


        }
        public dynamic InsertCouns(Counselling_Master couns)
        {
            var cnse = new CounsellingChecklist();
            cnse.Description = couns.Cps.Description;
            cnse.Type = couns.Cps.Type;
            cnse.CreatedBy = couns.Cps.CreatedBy;
            cnse.CreatedUTC = DateTime.UtcNow;
            WYNKContext.CounsellingChecklist.Add(cnse);
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
                Message = "Some Data Missing"

            };

        }
        public dynamic deletecns(int? ID)
        {

            var clst = WYNKContext.CounsellingChecklist.Where(x => x.ID == ID).ToList();
            if (clst != null)

            {

                clst.All(x => { x.IsDeleted = true; return true; });
                WYNKContext.CounsellingChecklist.UpdateRange(clst);

            }
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
                Message = "Some Data Missing"
            };
        }
        public dynamic UpdateCouns(Counselling_Master cps, int ID)
        {
            var cts = new CounsellingChecklist();
            cts = WYNKContext.CounsellingChecklist.Where(x => x.ID == ID).FirstOrDefault();
            cts.Description = cps.Cps.Description;
            cts.Type = cps.Cps.Type;
            cts.UpdatedBy = cps.Cps.UpdatedBy;
            cts.UpdatedUTC = DateTime.UtcNow;
            WYNKContext.CounsellingChecklist.UpdateRange(cts);
            WYNKContext.SaveChanges();
            try
            {
                if (WYNKContext.SaveChanges() >= 0)
                    return new
                    {
                        Success = true,
                        Message = "Updated successfully"
                    };
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            return new
            {
                Success = false,
                Message = "Some Data Missing"

            };

        }


        public dynamic InsertCounsellingData(savingCounsellingdetails InsertCounselling)
        {
            var InsertCounsellingdatass = new Counselling_Master();
            var CounsellingChecklistPatientdata = new CounsellingExtensionModel();
            var CounsellingPatientdata = new CounsellingModel();
            var Paylistdetails = InsertCounselling.PaymentdetailsAdvance.ToList();

            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {

                try
                {


                    if (InsertCounselling.Reasons != null)
                    {

                        CounsellingPatientdata.CmpID = Convert.ToInt32(InsertCounselling.compnayid);
                        CounsellingPatientdata.TCID = Convert.ToInt32(InsertCounselling.Transactionid);
                        CounsellingPatientdata.UIN = InsertCounselling.UINSS;
                        CounsellingPatientdata.Reasons = InsertCounselling.Reasons;
                        CounsellingPatientdata.CreatedUTC = DateTime.Now;
                        CounsellingPatientdata.CreatedBy = Convert.ToInt32(InsertCounselling.Userid);
                        WYNKContext.Counselling.Add(CounsellingPatientdata);
                        WYNKContext.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    else
                    {
                        foreach (var item in Paylistdetails)
                        {
                            var Paymentmaster = new Payment_Master();
                            Paymentmaster.UIN = InsertCounselling.PatientCounsellingDtls.UIN;
                            Paymentmaster.PaymentType = "O";
                            Paymentmaster.PaymentMode = item.PaymentMode;
                            Paymentmaster.InstrumentNumber = item.InstrumentNumber;
                            Paymentmaster.Instrumentdate = item.Instrumentdate;
                            Paymentmaster.BankName = item.BankName;
                            Paymentmaster.BankBranch = item.BankBranch;
                            Paymentmaster.Expirydate = item.Expirydate;
                            Paymentmaster.Amount = Convert.ToDecimal(item.Amount);
                            Paymentmaster.IsBilled = false;
                            Paymentmaster.InVoiceNumber = null;
                            Paymentmaster.InVoiceDate = null;
                            Paymentmaster.Paid = null;
                            Paymentmaster.InVoiceDate = null;
                            Paymentmaster.CreatedUTC = DateTime.UtcNow;
                            Paymentmaster.UpdatedUTC = null;
                            Paymentmaster.CreatedBy = Convert.ToInt32(InsertCounselling.Userid);
                            Paymentmaster.UpdatedBy = null;
                            Paymentmaster.ReceiptNumber = "";
                            Paymentmaster.TransactionID = Convert.ToInt32(InsertCounselling.Transactionid);
                            Paymentmaster.CmpID = Convert.ToInt32(InsertCounselling.compnayid);
                            Paymentmaster.PaymentReferenceID = null;
                            Paymentmaster.Fyear = "0";
                            Paymentmaster.ReceiptDate = DateTime.UtcNow;
                            WYNKContext.PaymentMaster.Add(Paymentmaster);
                            WYNKContext.SaveChanges();


                        }

                        var totalsum = InsertCounselling.PaymentdetailsAdvance.Sum(x => x.Amount);


                        CounsellingPatientdata.CmpID = Convert.ToInt32(InsertCounselling.compnayid);
                        CounsellingPatientdata.TCID = Convert.ToInt32(InsertCounselling.Transactionid);
                        CounsellingPatientdata.UIN = InsertCounselling.PatientCounsellingDtls.UIN;
                        CounsellingPatientdata.LensID = InsertCounselling.PatientCounsellingDtls.Typeoflens;
                       
                        CounsellingPatientdata.Progonosis = InsertCounselling.PatientCounsellingDtls.Pgognosis;
                        CounsellingPatientdata.Schedule = InsertCounselling.PatientCounsellingDtls.Scheduleddate;
                        CounsellingPatientdata.OtherRequirements = InsertCounselling.PatientCounsellingDtls.Remarks;
                        CounsellingPatientdata.DateOfConfirmation = InsertCounselling.PatientCounsellingDtls.Scheduleddate;
                 
                        CounsellingPatientdata.SurgeryDate = InsertCounselling.PatientCounsellingDtls.Scheduleddate;
                        CounsellingPatientdata.DateOfArrival = DateTime.UtcNow;
                        CounsellingPatientdata.PaymentDetails = Convert.ToDecimal(totalsum);
                        CounsellingPatientdata.RoomID = Convert.ToInt32(InsertCounselling.PatientCounsellingDtls.RoomID);
                        CounsellingPatientdata.CreatedUTC = DateTime.UtcNow;
                        CounsellingPatientdata.CreatedBy = Convert.ToInt32(InsertCounselling.Userid);
                        WYNKContext.Counselling.Add(CounsellingPatientdata);
                        WYNKContext.SaveChanges();

                        var CID = WYNKContext.Counselling.Where(x => x.UIN == InsertCounselling.PatientCounsellingDtls.UIN).OrderByDescending(x => x.CreatedUTC).Select(x => x.ID).FirstOrDefault();

                        foreach (var checkitem in InsertCounselling.Checklistdetailsss)
                        {
                            var checkextensiontable = new CounsellingExtensionModel();
                            checkextensiontable.CID = CID;
                            checkextensiontable.CCID = WYNKContext.CounsellingChecklist.Where(x => x.Description == checkitem.ItemDescription).Select(x => x.ID).FirstOrDefault();
                            checkextensiontable.CheckListDoneOn = DateTime.UtcNow;
                            checkextensiontable.CheckedBy = Convert.ToInt32(InsertCounselling.Userid);
                            checkextensiontable.CreatedUTC = DateTime.UtcNow;
                            checkextensiontable.UpdatedUTC = null;
                            checkextensiontable.CreatedBy = Convert.ToInt32(InsertCounselling.Userid);
                            checkextensiontable.UpdatedBy = null;
                            WYNKContext.CounsellingExtension.Add(checkextensiontable);
                            WYNKContext.SaveChanges();

                        }


                        var roomoccupieddetais = new RoomOccupiedstatus();

                        roomoccupieddetais.RoomID = Convert.ToInt32(InsertCounselling.PatientCounsellingDtls.RoomID);
                        roomoccupieddetais.RoomDetailsID = 0;
                        roomoccupieddetais.UIN = InsertCounselling.PatientCounsellingDtls.UIN;
                        roomoccupieddetais.BedNo = null;
                     
                        roomoccupieddetais.RoomOccupationToDate = null;
                        roomoccupieddetais.RoomOccupationFromTime = null;
                        roomoccupieddetais.RoomOccupationToTime = null;
                        roomoccupieddetais.VacatedOn = DateTime.UtcNow;
                    
                        roomoccupieddetais.Status = Convert.ToInt32(InsertCounselling.PatientCounsellingDtls.Roomstatus);
                        roomoccupieddetais.CreatedUTC = DateTime.UtcNow;
                        roomoccupieddetais.CreatedBy = Convert.ToInt32(InsertCounselling.Userid);
                        roomoccupieddetais.UpdatedUTC = null;
                        roomoccupieddetais.UpdatedBy = null;
                        WYNKContext.RoomOccupiedStatus.Add(roomoccupieddetais);
                        WYNKContext.SaveChanges();

                        var Finid = WYNKContext.Findings.Where(x => x.UIN == InsertCounselling.PatientCounsellingDtls.UIN).Select(x => x.RandomUniqueID).FirstOrDefault();
                        var FinExtid = WYNKContext.FindingsExt.Where(x => x.FindingsID == Finid).Select(x => x.ID).FirstOrDefault();

                        dbContextTransaction.Commit();
                    }



                    if (WYNKContext.SaveChanges() >= 0)
                        return new
                        {
                            Success = true,
                            Message = "Saved successfully",


                        };
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                }
                return new
                {
                    Success = false,
                    Message = "some data are missing"
                };
            }


        }


        public dynamic getSurgeonDetails(SURGDETAILS SUrgeondsesss)
        {
            var Doctorciounselingdetails = new Counselling_Master();
            Doctorciounselingdetails.SurgeonDoctordetails = new List<SurgeonDoctordetails>();

            foreach (var item in SUrgeondsesss.surgeondetails.ToList())
            {
                var det = new SurgeonDoctordetails();
                var Doctordetails = CMPSContext.DoctorMaster.Where(x => x.DoctorID == Convert.ToInt32(item.IDS)).Select(x => x.LastName).FirstOrDefault();
                det.Doctornames = Doctordetails;
                var desg = CMPSContext.DoctorSpeciality.Where(x => x.DoctorID == Convert.ToInt32(item.IDS) && x.IsActive == true).Select(x => x.OLMID).FirstOrDefault();
                det.Designation = CMPSContext.OneLineMaster.Where(x => x.OLMID == desg).Select(x => x.ParentDescription).FirstOrDefault();
                Doctorciounselingdetails.SurgeonDoctordetails.Add(det);
            }



            return Doctorciounselingdetails;
        }

        public dynamic getanesthetistDetails(SURGDETAILS Anesthesesss)
        {
            var Doctorciounselingdetails = new Counselling_Master();
            var save = new savingCounsellingdetails();
            Doctorciounselingdetails.SurgeonDoctordetails = new List<SurgeonDoctordetails>();
            Doctorciounselingdetails.AnesthetistDoctordetails = new List<Ane3sthetistDoctordetails>();
            Doctorciounselingdetails.PaymentdetailsAdvance = new List<PaymentdetailsAdvance>();
            Doctorciounselingdetails.PatientCounsellingDtls = new PatientCounsellingDetails();
            foreach (var item in Anesthesesss.surgeondetails.ToList())
            {
                var det = new SurgeonDoctordetails();

                var Doctordetails = CMPSContext.DoctorMaster.Where(x => x.DoctorID == Convert.ToInt32(item.IDS)).Select(x => x.LastName).FirstOrDefault();
                det.Doctornames = Doctordetails;
                var desg = CMPSContext.DoctorSpeciality.Where(x => x.DoctorID == Convert.ToInt32(item.IDS) && x.IsActive == true).Select(x => x.OLMID).FirstOrDefault();
                det.Designation = CMPSContext.OneLineMaster.Where(x => x.OLMID == desg).Select(x => x.ParentDescription).FirstOrDefault();
                Doctorciounselingdetails.SurgeonDoctordetails.Add(det);
            }

            foreach (var item in Anesthesesss.anesthetistdetails.ToList())
            {
                var det = new Ane3sthetistDoctordetails();

                var Doctordetails = CMPSContext.DoctorMaster.Where(x => x.DoctorID == Convert.ToInt32(item.IDS)).Select(x => x.LastName).FirstOrDefault();
                det.Doctornames = Doctordetails;
                var desg = CMPSContext.DoctorSpeciality.Where(x => x.DoctorID == Convert.ToInt32(item.IDS) && x.IsActive == true).Select(x => x.OLMID).FirstOrDefault();
                det.Designation = CMPSContext.OneLineMaster.Where(x => x.OLMID == desg).Select(x => x.ParentDescription).FirstOrDefault();
                Doctorciounselingdetails.AnesthetistDoctordetails.Add(det);
            }

            var list1 = Doctorciounselingdetails.SurgeonDoctordetails;
            var list2 = Doctorciounselingdetails.AnesthetistDoctordetails;


            var a = (from c in list1
                     select new
                     {
                         Doctorname = c.Doctornames,
                         desg = c.Designation
                     }).ToList();
            var bb = (from v in list2
                      select new
                      {
                          Doctorname = v.Doctornames,
                          desg = v.Designation
                      }).ToList();


            var Mergedlist = a.Concat(bb);

            Doctorciounselingdetails.Mergeddetails = (from cc in Mergedlist
                                                      select new Mergeddetails
                                                      {
                                                          Doctornames = cc.Doctorname,
                                                          Designation = cc.desg,
                                                      }).ToList();


            return Doctorciounselingdetails;
        }

    }
}














