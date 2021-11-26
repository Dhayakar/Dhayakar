using Microsoft.EntityFrameworkCore;
using CommonMailService.cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Data.Repository.Operation;
using WYNK.Helpers;



namespace WYNK.Data.Repository.Implementation
{
    class SurgeryDischargeRepository : RepositoryBase<SurgeryDischarge_Master>, ISurgeryDischargeRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
       

        public SurgeryDischargeRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public SurgeryDischarge_Master getSystemicOcularDetails(string UIN, int RegistrationTranId, int CMPID)
        {
            var surgeryDet = new SurgeryDischarge_Master();
            var OneLineMaster = CMPSContext.OneLineMaster.ToList();
            var drugMaster = WYNKContext.DrugMaster.ToList();
            var drugGroupMaster = WYNKContext.DrugGroup.ToList();

            var registration = WYNKContext.Registration.ToList();
            var registrationtran = WYNKContext.RegistrationTran.ToList();
            var patienthistory = WYNKContext.PatientHistory.ToList();
            var patientgeneral = WYNKContext.PatientGeneral.ToList();
            var findings = WYNKContext.Findings.ToList();
            var diagnosis = WYNKContext.Diagnosis.ToList();
            var onelinemaster = CMPSContext.OneLineMaster.ToList();


            surgeryDet.MedicalPrescriptionTranDetails = new List<MedicalPrescriptionTran>();

            surgeryDet.SystemicConditionDetails = (from patientHistory in WYNKContext.PatientHistory.Where(x => x.UIN == UIN && x.IsActive == true)
                                                   select new SystemicConditions
                                                   {
                                                       Description = onelinemaster.Where(x => x.OLMID == patientHistory.Description).Select(x => x.ParentDescription).FirstOrDefault(),
                                                       FromDate = Convert.ToDateTime(patientHistory.FromUTC),
                                                       ToMonths = Math.Abs(((DateTime.Now.Year - Convert.ToDateTime(patientHistory.FromUTC).Year) * 12) + Convert.ToDateTime(patientHistory.FromUTC).Month - DateTime.Now.Month)
                                                   }).ToList();

            surgeryDet.OcularConditionsDetails = (from REG in registration.Where(u => u.UIN == UIN)
                                                  join REGT in registrationtran on REG.UIN equals REGT.UIN
                                                  join Fid in findings on REGT.RegistrationTranID equals Fid.RegistrationTranID
                                                  join Dia in diagnosis on Fid.RandomUniqueID equals Dia.FindingsID

                                                  select new OcularConditions
                                                  {
                                                      Code = Dia.DiagnosisId,
                                                      Description = onelinemaster.Where(x=>x.OLMID == Dia.DiagnosisId).Select(x=>x.ParentDescription).FirstOrDefault(),
                                                      OD = Dia.IsOD,
                                                      OS = Dia.IsOS,
                                                      OU = Dia.IsOU,

                                                  }).ToList();

                 surgeryDet.PatientAllergys = (from AllergyTrans in WYNKContext.AllergyTran.Where(x => x.UIN == UIN && x.IsActive == true)
                                                   select new PatientAllergy
                                                   {
                                                       AllergyType = WYNKContext.allergy.Where(x => x.ID == AllergyTrans.Type).Select(x => x.ParentDescription).FirstOrDefault(),
                                                       Description = WYNKContext.allergy.Where(x=>x.ID == AllergyTrans.Description).Select(x=>x.ParentDescription).FirstOrDefault(),          
                                                   }).ToList();

            return surgeryDet;
        }
        public SurgeryDischarge_Master getSurgeryIDDischargeDetails(string UIN, int RegistrationTranId,int CMPID,int TC)
        {
            var surgeryDet = new SurgeryDischarge_Master();

            var test = WYNKContext.Findings.Where(x => x.RegistrationTranID == RegistrationTranId).OrderByDescending(x => x.CreatedUTC).Select(x => x.ID).FirstOrDefault();

            var OneLineMaster = CMPSContext.OneLineMaster.ToList();
            var drugMaster = WYNKContext.DrugMaster.ToList();
            var drugGroupMaster = WYNKContext.DrugGroup.ToList();

            var registration = WYNKContext.Registration.ToList();
            var registrationtran = WYNKContext.RegistrationTran.ToList();
            var patienthistory = WYNKContext.PatientHistory.ToList();
            var patientgeneral = WYNKContext.PatientGeneral.ToList();
            var findingID = WYNKContext.Findings.Where(x => x.RegistrationTranID == RegistrationTranId).OrderByDescending(x => x.CreatedUTC).Select(x=>x.RandomUniqueID).FirstOrDefault();
            var finding = WYNKContext.Findings.Where(x => x.CmpID == CMPID).ToList();
            var diagnosis = WYNKContext.Diagnosis.ToList();
            var onelinemaster = CMPSContext.OneLineMaster.ToList();


            surgeryDet.MedicalPrescriptionTranDetails = new List<MedicalPrescriptionTran>();

            surgeryDet.SystemicConditionDetails = (from patientHistory in WYNKContext.PatientHistory.Where(x => x.UIN == UIN)

                                                   select new SystemicConditions
                                                   {
                                                       Description = onelinemaster.Where(x =>x.OLMID == patientHistory.Description).Select(x =>x.ParentDescription).FirstOrDefault(), 
                                                       FromDate = Convert.ToDateTime(patientHistory.FromUTC),
                                                       ToMonths = Math.Abs(((DateTime.Now.Year - Convert.ToDateTime(patientHistory.FromUTC).Year) * 12) + Convert.ToDateTime(patientHistory.FromUTC).Month - DateTime.Now.Month)
                                                   }

                                                     ).ToList();

            surgeryDet.OcularConditionsDetails = (from Fid in finding.Where(x=>x.RandomUniqueID == findingID)
                                                  join Dia in diagnosis on Fid.RandomUniqueID equals Dia.FindingsID
                                                  join One in OneLineMaster on Dia.DiagnosisId equals One.OLMID
                                                  orderby Fid.CreatedUTC descending
                                                  select new OcularConditions
                                                  {
                                                      Code = Dia.DiagnosisId,
                                                      Description = One.ParentDescription,
                                                      OD = Dia.IsOD,
                                                      OS = Dia.IsOS,
                                                      OU = Dia.IsOU,
                                                  }).ToList();

            surgeryDet.PatientAllergys = (from AllergyTrans in WYNKContext.AllergyTran.Where(x => x.UIN == UIN && x.IsActive == true)
                                          select new PatientAllergy
                                          {
                                              AllergyType = WYNKContext.allergy.Where(x => x.ID == AllergyTrans.Type).Select(x => x.ParentDescription).FirstOrDefault(),
                                              Description = WYNKContext.allergy.Where(x => x.ID == AllergyTrans.Description).Select(x => x.ParentDescription).FirstOrDefault(),
                                          }).ToList();


            surgeryDet.PrescribedBy = WYNKContext.MedicalPrescription.Where(x => x.RegistrationTranID == RegistrationTranId && x.Transactionid == TC).Select(x => x.PrescribedBy).FirstOrDefault();

            surgeryDet.PrescribedMedicalPrescriptionTran = (from medpres in WYNKContext.MedicalPrescription.Where(x => x.RegistrationTranID == RegistrationTranId && x.Transactionid == TC).Select(x => x.RandomUniqueID)
                                                            join medprestran in WYNKContext.MedicalPrescriptionTran on medpres equals medprestran.MedicalPrescriptionID

                                                            select new PrescribedMedicalPrescriptionTran
                                                            {
                                                                MedicalPrescriptionTranID = medprestran.MedicalPrescriptionTranID,
                                                                Brand = drugMaster.Where(x => x.ID == medprestran.DrugId).Select(x => x.Brand).FirstOrDefault(),
                                                                DrugForm = drugMaster.Where(x => x.ID == medprestran.DrugId).Select(x => x.DrugGroup).FirstOrDefault(),
                                                                Frequency = medprestran.Frequency,
                                                                Quantity = Convert.ToInt32(medprestran.Quantity),
                                                                Eye = medprestran.Eye,
                                                                Food = medprestran.Food,
                                                                Days = Convert.ToInt32(medprestran.Days),
                                                                Remarks = medprestran.Remarks,
                                                                UOM= drugMaster.Where(x => x.ID == medprestran.DrugId).Select(x => x.UOM).FirstOrDefault(),
                                                            }

                                                          ).ToList();

            var res = WYNKContext.Findings.Where(x => x.RegistrationTranID == RegistrationTranId).Select(x => x.RandomUniqueID).LastOrDefault();
            surgeryDet.ReviewDate = WYNKContext.FindingsExt.Where(x => x.FindingsID == res).Select(x => x.SureryReviewDate).FirstOrDefault();


            surgeryDet.CMPID = CMPSContext.Company.Where(x => x.CmpID == CMPID).FirstOrDefault();

            return surgeryDet;
        }
        public dynamic SurgerDischargeDetails(SurgeryDischarge_Master SurgerDischargeDetails,int Tc)
        {
            int DischargeId;
            string MedPrescriptionId ="";

            if (SurgerDischargeDetails != null)
            {
                using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
                {
                    try
                    {
                        var surgeryDischarge = new Surgery_Discharge();
                        surgeryDischarge.CMPID = SurgerDischargeDetails.SurgeryDischargeMaster.CMPID;
                        surgeryDischarge.TransactionId = Tc;
                        surgeryDischarge.AdmissionID = SurgerDischargeDetails.SurgeryDischargeMaster.AdmissionID;
                        surgeryDischarge.SurgeryID = SurgerDischargeDetails.SurgeryDischargeMaster.SurgeryID;
                        surgeryDischarge.RegTranID = SurgerDischargeDetails.SurgeryDischargeMaster.RegTranID;
                        surgeryDischarge.TreatmentAdvice = SurgerDischargeDetails.SurgeryDischargeMaster.TreatmentAdvice;
                        surgeryDischarge.DischargeType = SurgerDischargeDetails.SurgeryDischargeMaster.DischargeType;
                        surgeryDischarge.DischargeDate = SurgerDischargeDetails.SurgeryDischargeMaster.DischargeDate;
                        surgeryDischarge.CreatedBy = SurgerDischargeDetails.MedPrescription.Createdby;
                        surgeryDischarge.CreatedUTC = DateTime.UtcNow;
                        WYNKContext.SurgeryDischarge.Add(surgeryDischarge);


                        string cmpname = CMPSContext.Company.Where(x => x.CmpID == SurgerDischargeDetails.SurgeryDischargeMaster.CMPID).Select(x => x.CompanyName).FirstOrDefault();
                        string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == SurgerDischargeDetails.SurgeryDischargeMaster.CreatedBy).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                        string userid = Convert.ToString(SurgerDischargeDetails.SurgeryDischargeMaster.CreatedBy);
                        ErrorLog oErrorLogs = new ErrorLog();
                        oErrorLogs.WriteErrorLogTitle(cmpname, "Discharge", "User name :", username, "User ID :", userid, "Mode : Add");
                        object names = surgeryDischarge;
                        oErrorLogs.WriteErrorLogArray("SurgeryDischarge", names);


                        WYNKContext.SaveChanges();

                        DischargeId = surgeryDischarge.ID;

                        var AdmIds = WYNKContext.Admission.Where(x => x.RandomUniqueID == SurgerDischargeDetails.SurgeryDischargeMaster.AdmissionID).FirstOrDefault();
                        if (AdmIds != null)
                        {
                            AdmIds.DischargeID = DischargeId;
                            WYNKContext.Admission.UpdateRange(AdmIds);
                            WYNKContext.SaveChanges();


                            var roomOcupStatus = WYNKContext.RoomOccupiedStatus.Where(x => x.AdmID == SurgerDischargeDetails.SurgeryDischargeMaster.AdmissionID && x.CmpID == SurgerDischargeDetails.SurgeryDischargeMaster.CMPID).FirstOrDefault();
                            roomOcupStatus.VacatedOn = SurgerDischargeDetails.SurgeryDischargeMaster.DischargeDate;
                            roomOcupStatus.IsOccupied = false;
                            WYNKContext.RoomOccupiedStatus.UpdateRange(roomOcupStatus);
                            WYNKContext.SaveChanges();
                        }

                        if (SurgerDischargeDetails.MedicalPrescriptionTranDetails.Count > 0)
                        {
                            var medicalprescriptionDetails = new MedicalPrescription();
                            medicalprescriptionDetails.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                            medicalprescriptionDetails.CmpID = SurgerDischargeDetails.SurgeryDischargeMaster.CMPID;
                            medicalprescriptionDetails.RegistrationTranID = SurgerDischargeDetails.SurgeryDischargeMaster.RegTranID;
                            medicalprescriptionDetails.UIN = SurgerDischargeDetails.MedPrescription.UIN;
                            medicalprescriptionDetails.MedicalPrescriptionNo = SurgerDischargeDetails.RunningNo;
                            medicalprescriptionDetails.PrescribedBy = Convert.ToInt32(SurgerDischargeDetails.MedPrescription.Prescribedby);
                            var docDetails = CMPSContext.DoctorMaster.Where(x => x.DoctorID == SurgerDischargeDetails.MedPrescription.Prescribedby && x.IsActive == true).FirstOrDefault();
                            medicalprescriptionDetails.PrescribedByName = docDetails.Firstname + " " + docDetails.MiddleName + " " + docDetails.LastName;
                            medicalprescriptionDetails.Status = "Open";
                            medicalprescriptionDetails.PrescribedDate = DateTime.UtcNow;
                            medicalprescriptionDetails.DischargeID = DischargeId;
                            medicalprescriptionDetails.SurgeryID = SurgerDischargeDetails.SurgeryDischargeMaster.SurgeryID;
                            medicalprescriptionDetails.CreatedUTC = DateTime.UtcNow;
                            medicalprescriptionDetails.CreatedBy = SurgerDischargeDetails.MedPrescription.Createdby;
                            medicalprescriptionDetails.Transactionid = Tc;
                            medicalprescriptionDetails.Fyear = SurgerDischargeDetails.FYear;


                            WYNKContext.MedicalPrescription.Add(medicalprescriptionDetails);
                            object medicalprescriptionDetai = medicalprescriptionDetails;
                            oErrorLogs.WriteErrorLogArray("MedicalPrescription", medicalprescriptionDetai);

                            WYNKContext.SaveChanges();

                            MedPrescriptionId = medicalprescriptionDetails.RandomUniqueID;

                            foreach (var item in SurgerDischargeDetails.MedicalPrescriptionTranDetails.ToList())
                            {
                                var MedPrescTran = new MedicalPrescriptionTran();
                                MedPrescTran.MedicalPrescriptionID = MedPrescriptionId;
                                MedPrescTran.DrugId = item.DrugId;
                                MedPrescTran.Frequency = item.Frequency;
                                MedPrescTran.Quantity = item.Quantity;
                                MedPrescTran.Eye = item.Eye;
                                MedPrescTran.Food = item.Food;
                                MedPrescTran.Days = item.Days;
                                MedPrescTran.CreatedUTC = DateTime.Now;
                                MedPrescTran.CreatedBy = SurgerDischargeDetails.MedPrescription.Createdby;
                                WYNKContext.MedicalPrescriptionTran.Add(MedPrescTran);
                                ErrorLog oErrorLogstran = new ErrorLog();
                                object namestr = MedPrescTran;
                                oErrorLogstran.WriteErrorLogArray("MedicalPrescriptionTran", namestr);
                            }
                            WYNKContext.SaveChanges();
                        }

                        var FindingsID = WYNKContext.Findings.Where(x => x.RegistrationTranID == SurgerDischargeDetails.SurgeryDischargeMaster.RegTranID).Select(x => x.RandomUniqueID).FirstOrDefault();
                        var FindingsExt = WYNKContext.FindingsExt.Where(x => x.FindingsID == FindingsID).FirstOrDefault();

                        if (FindingsExt != null)
                        {
                            FindingsExt.SureryReviewDate = SurgerDischargeDetails.MedPrescription.ReviewDate != null ? Convert.ToDateTime(SurgerDischargeDetails.MedPrescription.ReviewDate).AddDays(1) : (DateTime?)null;
                            FindingsExt.DischargeID = DischargeId;
                            WYNKContext.FindingsExt.UpdateRange(FindingsExt);
                        }


                        var commonRepos = new CommonRepository(_Wynkcontext, _Cmpscontext);
                        var RunningNumber = commonRepos.GenerateRunningCtrlNoo(Tc, SurgerDischargeDetails.SurgeryDischargeMaster.CMPID, "GetRunningNo");
                        if (RunningNumber == SurgerDischargeDetails.RunningNo)
                        {
                            commonRepos.GenerateRunningCtrlNoo(Tc, SurgerDischargeDetails.SurgeryDischargeMaster.CMPID, "UpdateRunningNo");
                        }
                        else
                        {
                            var GetRunningNumber = commonRepos.GenerateRunningCtrlNoo(Tc, SurgerDischargeDetails.SurgeryDischargeMaster.CMPID, "UpdateRunningNo");
                            var MedicalPrescription = WYNKContext.MedicalPrescription.Where(x => x.RandomUniqueID == MedPrescriptionId).FirstOrDefault();
                            MedicalPrescription.MedicalPrescriptionNo = GetRunningNumber;
                            WYNKContext.MedicalPrescription.UpdateRange(MedicalPrescription);
                        }

                        WYNKContext.SaveChanges();

                        if (WYNKContext.SaveChanges() >= 0)
                        {
                            ErrorLog oErrorLog = new ErrorLog();
                            oErrorLog.WriteErrorLog("Information :", "Saved Successfully");
                        }

                        dbContextTransaction.Commit();

                        return new
                        {

                            Success = true,
                            Message = "Saved successfully",
                            CompanyDetails = CMPSContext.Company.Where(x => x.CmpID == SurgerDischargeDetails.SurgeryDischargeMaster.CMPID).FirstOrDefault(),
                        };
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        if (ex.InnerException != null)
                        {
                            ErrorLog oErrorLog = new ErrorLog();
                            oErrorLog.WriteErrorLog("Error", ex.InnerException.Message.ToString());
                            string msg = ex.InnerException.Message;
                            return new { Success = false, Message = msg, grn = SurgerDischargeDetails.RunningNo };
                        }
                        else
                        {
                            ErrorLog oErrorLog = new ErrorLog();
                            oErrorLog.WriteErrorLog("Error", ex.Message.ToString());
                            return new { Success = false, Message = ex.Message.ToString() };
                        }
                    }
                }
            }
            else
            {
                return new
                {
                    Success = false,
                };
            }
        }
        public dynamic IsSurgeryComplete(int RegistrationTranId)
        {
            string Message;
            bool Success;
            try
            {
                var SurComplete = WYNKContext.Admission.Where(x => x.RegTranID == RegistrationTranId).Select(x => x.IsSurgeryCompleted).FirstOrDefault();

                if (SurComplete == true)
                    return new
                    {
                        Success = true,
                        Message = "Surgery Completed"
                    };

                else
                    return new
                    {
                        Success = false,
                        Message = "Surgery Not Completed"
                    };

            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Success = false,
                Message = "Not Found"
            };

        }
        public dynamic SaveTemplate(SaveTemplate SaveTemplate)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    var res = WYNKContext.DrugTemplate.Where(x => x.Description == SaveTemplate.Description).Select(x => x.ID).FirstOrDefault();

                        if(res != 0)
                        {
                            var DeleteDescription = WYNKContext.DrugTemplate.Where(x => x.Description == SaveTemplate.Description).ToList();
                            WYNKContext.DrugTemplate.RemoveRange(DeleteDescription);
                         }

                        foreach (var item in SaveTemplate.MedicalPrescriptionTranDetails.ToList())
                         {
                            var DrugTemplate = new DrugTemplate();
                            DrugTemplate.DoctorID = SaveTemplate.DoctorID;
                            DrugTemplate.Description = SaveTemplate.Description;
                            DrugTemplate.Frequency = item.Frequency;
                            DrugTemplate.DrugID = item.DrugId;
                            DrugTemplate.Days = Convert.ToInt32(item.Days);
                            DrugTemplate.Food = item.Food;
                            DrugTemplate.Quantity = Convert.ToInt32(item.Quantity);
                            DrugTemplate.CreatedUTC = DateTime.UtcNow;
                            DrugTemplate.CreatedBy = SaveTemplate.Createdby;
                           WYNKContext.DrugTemplate.Add(DrugTemplate);
                         }
                        WYNKContext.SaveChanges();
                        dbContextTransaction.Commit();
                        return new
                        {
                            Success = true,
                            Message = "Template Saved"
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
                    Message = "Something Went Wrong"
                };
            }
        }
        public dynamic TemplateDetails(int DoctorId, int cmpid)
        {
            var SurDistemp = new SurDisTemp();

            var DrugMaster = WYNKContext.DrugMaster.ToList();
            var DrugGroup = WYNKContext.DrugGroup.ToList();

            SurDistemp.tempDesc = (from res in WYNKContext.DrugTemplate.Where(x => x.DoctorID == DoctorId && x.CmpID == cmpid).GroupBy(x=>x.Description)
                                   select new tempDesc
                                   {
                                    TempDesc = res.Select(x=>x.Description).FirstOrDefault(),
                                    tempDetails= (from res1 in WYNKContext.DrugTemplate.Where(x => x.DoctorID == DoctorId && x.CmpID == cmpid && x.Description == res.Select(z => z.Description).FirstOrDefault())
                                                  select new tempDetails
                                                  {
                                                      DrugID = res1.DrugID,
                                                      DrugDesc = DrugMaster.Where(x => x.ID == res1.DrugID).Select(x => x.Brand).FirstOrDefault(),
                                                      GenericName = res1.Brand,
                                                      UOM = DrugMaster.Where(x => x.ID == res1.DrugID).Select(x => x.UOM).FirstOrDefault(),
                                                      sideeffect = DrugGroup.Where(x => x.ID == DrugMaster.Where(y => y.ID == res1.DrugID).Select(y => y.GenericName).FirstOrDefault()).Select(x => x.SideEffects).FirstOrDefault(),
                                                      precaution = DrugGroup.Where(x => x.ID == DrugMaster.Where(y => y.ID == res1.DrugID).Select(y => y.GenericName).FirstOrDefault()).Select(x => x.Precautions).FirstOrDefault(),
                                                      overdose = DrugGroup.Where(x => x.ID == DrugMaster.Where(y => y.ID == res1.DrugID).Select(y => y.GenericName).FirstOrDefault()).Select(x => x.Overdose).FirstOrDefault(),
                                                      frequency = res1.Frequency,
                                                      Days = res1.Days,
                                                      Food = res1.Food,
                                                      Quantity = res1.Quantity
                                                  }).ToList()
                                   }).ToList();
            return SurDistemp;
        }
        public dynamic ViewTemplateDetails(string Desc, int DoctorID, int cmpid)
        {
            var DoctorTempDetails = new DoctorTempDetails();
            var drugmaster = WYNKContext.DrugMaster.ToList();
            var DrugGroup = WYNKContext.DrugGroup.ToList();


            DoctorTempDetails.tempDetails = (from res in WYNKContext.DrugTemplate.Where(x => x.DoctorID == DoctorID && x.Description == Desc && x.CmpID == cmpid)
                                             select new  tempDetails
                                             {
                                                 DrugID = res.DrugID,
                                                 DrugDesc =drugmaster.Where(x=>x.ID == res.DrugID).Select(x=>x.Brand).FirstOrDefault(),
                                                 GenericName = res.Brand,
                                                 UOM = drugmaster.Where(x => x.ID == res.DrugID).Select(x => x.UOM).FirstOrDefault(),
                                                 sideeffect = DrugGroup.Where(x => x.ID == drugmaster.Where(y => y.ID == res.DrugID).Select(y => y.GenericName).FirstOrDefault()).Select(x => x.SideEffects).FirstOrDefault(),
                                                 precaution = DrugGroup.Where(x => x.ID == drugmaster.Where(y => y.ID == res.DrugID).Select(y => y.GenericName).FirstOrDefault()).Select(x => x.Precautions).FirstOrDefault(),
                                                 overdose = DrugGroup.Where(x => x.ID == drugmaster.Where(y => y.ID == res.DrugID).Select(y => y.GenericName).FirstOrDefault()).Select(x => x.Overdose).FirstOrDefault(),
                                                 frequency = res.Frequency,
                                                 Days=res.Days,
                                                 Food=res.Food,
                                                 Quantity=res.Quantity
                                             }).ToList();



            return DoctorTempDetails;
        }

        public dynamic ViewTemplateDetailsm(string Desc, int DoctorID, int cmpid)
        {
            var DoctorTempDetails = new DoctorTempDetails();
            var drugmaster = WYNKContext.SpecialityMaster.ToList();
            var DrugGroup = WYNKContext.DrugGroup.ToList();


            DoctorTempDetails.tempDetails = (from res in WYNKContext.DrugTemplate.Where(x => x.DoctorID == DoctorID && x.Description == Desc && x.CmpID == cmpid)
                                             select new tempDetails
                                             {
                                                 DrugID = res.DrugID,
                                                 GenericName = drugmaster.Where(x => x.ID == res.DrugID).Select(x => x.ParentDescription).FirstOrDefault(),
                                                 frequency = res.Frequency,
                                                 Days = res.Days,
                                                 Food = res.Food,
                                                 Quantity = res.Quantity
                                             }).ToList();



            return DoctorTempDetails;
        }
        public dynamic TemplateDescription(string Desc)
        {
            try
            {
                var TemplateDescription = WYNKContext.DrugTemplate.Where(x => x.Description == Desc).Select(x => x.ID).FirstOrDefault();

                if (TemplateDescription != 0)
                {
                    return new
                    {
                        Success = false,
                        Message = "Already Template Name Exist"
                    };
                 
                }
                else
                {
                    return new
                    {
                        Success = true,
                        Message = "Template Name Does Not Exist"
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

        public dynamic Isbilled(string AdmissionID)
        {
            string Message;
            bool Success;
            try
            {
                var Isbilled = WYNKContext.Admission.Where(x => x.RandomUniqueID == AdmissionID && x.isbilled == true && x.Paid != null).FirstOrDefault();

                if (Isbilled != null)
                    return new
                    {
                        Success = true,
                        Message = "Billed"
                    };

                else
                    return new
                    {
                        Success = false,
                        Message = "Not Billed"
                    };

            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Success = false,
                Message = "Not Found"
            };
        }
    }
}













