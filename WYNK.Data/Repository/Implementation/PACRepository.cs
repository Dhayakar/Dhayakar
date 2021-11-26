using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WYNK.Data.Common;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Helpers;


namespace WYNK.Data.Repository.Implementation
{
    class PACRepository : RepositoryBase<PACViewM>, IPACRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        

        public PACRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }


        public PACViewM getBMIrange(decimal ID)
        {
            var getData = new PACViewM();
            var M_BMI = WYNKContext.BMI.ToList();

            getData.getBMI = (from BMI in M_BMI.Where(x => x.FromRange <= ID && x.ToRange >= ID)

                              select new getBMI
                              {
                                  BMICharacter = BMI.Category,
                                  BMIID = BMI.ID
                              }).ToList();



            return getData;
        }
        public PACViewM getPatientDrugtherapy(string ID, int CMPID)
        {
            var getData = new PACViewM();
            var MedicalPrescription = WYNKContext.MedicalPrescription.ToList();
            var MedicalPrescriptionTran = WYNKContext.MedicalPrescriptionTran.ToList();

            var DrugMaster = WYNKContext.DrugMaster.ToList();
            var druggroup = WYNKContext.DrugGroup.ToList();

            getData.getPDT = (from
                               Drug in DrugMaster
                              join DrugGrp in druggroup on Drug.GenericName equals DrugGrp.ID

                              select new getPDT
                              {
                                  GenericName = DrugGrp.Description,
                                  DrugName = Drug.Brand,
                                  DrugID = Drug.ID,
                                  GenericID = Convert.ToInt32(DrugGrp.ID),

                              }

                             ).ToList();

            return getData;

        }
        public dynamic InserData(PACViewM PACHistory)

        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    var PACHistoryModel = new PACHistoryModel();
                    PACHistoryModel.ADMID = PACHistory.PACHistoryModel.ADMID;
                    PACHistoryModel.CMPID = PACHistory.PACHistoryModel.CMPID;
                    PACHistoryModel.UIN = PACHistory.PACHistoryModel.UIN;
                    PACHistoryModel.Previous_Operation = PACHistory.PACHistoryModel.Previous_Operation;
                    PACHistoryModel.ColdCoughFever = PACHistory.PACHistoryModel.ColdCoughFever;
                    PACHistoryModel.PalpitationsBreathless = PACHistory.PACHistoryModel.PalpitationsBreathless;
                    PACHistoryModel.Orthopnoea_PND = PACHistory.PACHistoryModel.Orthopnoea_PND;
                    PACHistoryModel.CPD_Asthma = PACHistory.PACHistoryModel.CPD_Asthma;
                    PACHistoryModel.Unconsciousness = PACHistory.PACHistoryModel.Unconsciousness;
                    PACHistoryModel.Psychiatric_illness = PACHistory.PACHistoryModel.Psychiatric_illness;
                    PACHistoryModel.Renal_Disorders = PACHistory.PACHistoryModel.Renal_Disorders;
                    PACHistoryModel.Jaundice = PACHistory.PACHistoryModel.Jaundice;
                    PACHistoryModel.HTN_IHD_CHD_DM = PACHistory.PACHistoryModel.HTN_IHD_CHD_DM;
                    PACHistoryModel.Bleeding_Disorders = PACHistory.PACHistoryModel.Bleeding_Disorders;
                    PACHistoryModel.Exercise_Tolerance = PACHistory.PACHistoryModel.Exercise_Tolerance;
                    if (PACHistory.PACHistoryModel.Family_Ho == " ")
                    {
                        PACHistoryModel.Family_Ho = "     ";
                    }
                    else
                    {
                        PACHistoryModel.Family_Ho = PACHistory.PACHistoryModel.Family_Ho;
                    }

                    if (PACHistory.PACHistoryModel.Menstrual == null)
                    {
                        PACHistoryModel.Menstrual = "     ";
                    }
                    else
                    {
                        PACHistoryModel.Menstrual = PACHistory.PACHistoryModel.Menstrual;
                    }

                    PACHistoryModel.Last_Meal_Time = PACHistory.PACHistoryModel.Last_Meal_Time;
                    PACHistoryModel.Smoking_Tobacco = PACHistory.PACHistoryModel.Smoking_Tobacco;
                    PACHistoryModel.Alcohol_habituation = PACHistory.PACHistoryModel.Alcohol_habituation;
                    PACHistoryModel.Drug_Addictions = PACHistory.PACHistoryModel.Drug_Addictions;
                    PACHistoryModel.Known_allergies = PACHistory.PACHistoryModel.Known_allergies;
                    PACHistoryModel.Remarks = PACHistory.PACHistoryModel.Remarks;
                    PACHistoryModel.CreatedUTC = DateTime.UtcNow;
                    PACHistoryModel.CreatedBy = PACHistory.PACHistoryModel.CreatedBy;
                    PACHistoryModel.DoctorID = PACHistory.DoctorID;

                    WYNKContext.PACHistory.AddRange(PACHistoryModel);
                    WYNKContext.SaveChanges();

                    if (PACHistory.pushData.Count() > 0)
                    {
                        foreach (var PACHistoryTran in PACHistory.pushData.ToList())
                        {
                            var PACHTran = new PACHistoryTranModel();
                            PACHTran.PACHistoryID = PACHistoryModel.PACHistoryID;
                            PACHTran.DrugID = PACHistoryTran.DrugID;
                            PACHTran.Dossage = PACHistoryTran.Dosage;
                            PACHTran.Frequency = PACHistoryTran.frequency;
                            PACHTran.Drug_Description = PACHistoryTran.DrugDescription;
                            PACHTran.CreatedUTC = DateTime.UtcNow;
                            PACHTran.CreatedBy = PACHistoryModel.CreatedBy;
                            WYNKContext.PACHistoryTran.AddRange(PACHTran);
                        }
                    }
                    WYNKContext.SaveChanges();

                    dbContextTransaction.Commit();

                    try
                    {
                        if (WYNKContext.SaveChanges() >= 0)
                            return new
                            {
                                Success = true,
                                Message = "Saved successfully",
                                PACID = PACHistoryModel.PACHistoryID
                            };
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex);
                    }


                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                }
                return new
                {
                    Success = false,
                    Message = "Some data are Missing"

                };
            }
        }
        public dynamic InserExamData(PACViewM PACExamination)

        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    var PACExam = new PACExamination();
                    PACExam.ADMID = PACExamination.PACExamination.ADMID;
                    PACExam.CMPID = PACExamination.PACExamination.CMPID;
                    PACExam.UIN = PACExamination.PACExamination.UIN;
                    PACExam.PulseHeartRate = PACExamination.PACExamination.PulseHeartRate;
                    PACExam.BloodPressure = PACExamination.PACExamination.BloodPressure;
                    PACExam.Respiration = PACExamination.PACExamination.Respiration;
                    PACExam.temperature = PACExamination.PACExamination.temperature;
                    PACExam.Spine = PACExamination.PACExamination.Spine;
                    PACExam.Weight = PACExamination.PACExamination.Weight;
                    PACExam.Height = PACExamination.PACExamination.Height;
                    PACExam.BMI = PACExamination.PACExamination.BMI;
                    PACExam.NStatus = PACExamination.PACExamination.NStatus;
                    PACExam.Icterus = PACExamination.PACExamination.Icterus;
                    PACExam.Cyanosis = PACExamination.PACExamination.Cyanosis;
                    PACExam.Clubbing = PACExamination.PACExamination.Clubbing;
                    PACExam.LAP = PACExamination.PACExamination.LAP;
                    PACExam.BHT = PACExamination.PACExamination.BHT;
                    PACExam.Oedema = PACExamination.PACExamination.Oedema;
                    PACExam.JVP = PACExamination.PACExamination.JVP;
                    PACExam.Veins = PACExamination.PACExamination.Veins;
                    PACExam.GC = PACExamination.PACExamination.GC;
                    PACExam.Pallor = PACExamination.PACExamination.Pallor;
                    PACExam.Thyromentaldist = PACExamination.PACExamination.Thyromentaldist;
                    PACExam.MouthOpening = PACExamination.PACExamination.MouthOpening;
                    PACExam.LooseMissedTeeth = PACExamination.PACExamination.LooseMissedTeeth;
                    PACExam.MallampattiClass = PACExamination.PACExamination.MallampattiClass;
                    PACExam.NasalPatency = PACExamination.PACExamination.NasalPatency;
                    PACExam.ArtificialTeeth = PACExamination.PACExamination.ArtificialTeeth;
                    PACExam.OralHygiene = PACExamination.PACExamination.OralHygiene;
                    PACExam.TMJoint = PACExamination.PACExamination.TMJoint;
                    PACExam.UpperRespiratory = PACExamination.PACExamination.UpperRespiratory;
                    PACExam.LowerRespiratory = PACExamination.PACExamination.LowerRespiratory;
                    PACExam.CVS = PACExamination.PACExamination.CVS;
                    PACExam.CNS = PACExamination.PACExamination.CNS;
                    PACExam.PerAbdomen = PACExamination.PACExamination.PerAbdomen;
                    PACExam.Neck = PACExamination.PACExamination.Neck;
                    PACExam.CreatedUTC = DateTime.UtcNow;
                    PACExam.CreatedBy = PACExamination.PACExamination.CreatedBy;
                    PACExam.DoctorID = PACExamination.DoctorID;
                    WYNKContext.PACExamination.AddRange(PACExam);
                    WYNKContext.SaveChanges();


                    WYNKContext.SaveChanges();
                    dbContextTransaction.Commit();

                    try
                    {
                        if (WYNKContext.SaveChanges() >= 0)
                            return new
                            {
                                Success = true,
                                Message = "Saved successfully",
                                PacexamID = PACExam.PACExamID
                            };
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex);
                    }

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                }
                return new
                {
                    Success = false,
                    Message = "Some data are Missing"

                };
            }
        }
        public dynamic InserInvData(PACViewM PACInvestigation)

        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {

                    var PACInv = new PACInvestigation();
                    PACInv.ADMID = PACInvestigation.PACInvestigation.ADMID;
                    PACInv.CMPID = PACInvestigation.PACInvestigation.CMPID;
                    PACInv.UIN = PACInvestigation.PACInvestigation.UIN;
                    PACInv.Hb_PCV = PACInvestigation.PACInvestigation.Hb_PCV;
                    PACInv.TLC = PACInvestigation.PACInvestigation.TLC;
                    PACInv.PLCount = PACInvestigation.PACInvestigation.PLCount;
                    PACInv.BLGlu = PACInvestigation.PACInvestigation.BLGlu;
                    PACInv.SUricAcid = PACInvestigation.PACInvestigation.SUricAcid;
                    PACInv.T3 = PACInvestigation.PACInvestigation.T3;
                    PACInv.T4 = PACInvestigation.PACInvestigation.T4;
                    PACInv.TSH = PACInvestigation.PACInvestigation.TSH;
                    PACInv.TSH = PACInvestigation.PACInvestigation.TSH;
                    PACInv.SElectNAPlus = PACInvestigation.PACInvestigation.SElectNAPlus;
                    PACInv.SElectKPlus = PACInvestigation.PACInvestigation.SElectKPlus;
                    PACInv.SElectCAPlus = PACInvestigation.PACInvestigation.SElectCAPlus;
                    PACInv.UrineRMALB = PACInvestigation.PACInvestigation.UrineRMALB;
                    PACInv.UrineRMSugar = PACInvestigation.PACInvestigation.UrineRMSugar;
                    PACInv.UrineRMKET = PACInvestigation.PACInvestigation.UrineRMKET;
                    PACInv.UrineRMWBC = PACInvestigation.PACInvestigation.UrineRMWBC;
                    PACInv.UrineRMRBC = PACInvestigation.PACInvestigation.UrineRMRBC;
                    PACInv.BlGroup = PACInvestigation.PACInvestigation.BlGroup;
                    PACInv.BUrea = PACInvestigation.PACInvestigation.BUrea;
                    PACInv.HBSAG = PACInvestigation.PACInvestigation.HBSAG;
                    PACInv.HIV = PACInvestigation.PACInvestigation.HIV;
                    PACInv.XRay = PACInvestigation.PACInvestigation.XRay;
                    PACInv.ECG = PACInvestigation.PACInvestigation.ECG;
                    PACInv.ECHO = PACInvestigation.PACInvestigation.ECHO;
                    PACInv.ABG = PACInvestigation.PACInvestigation.ABG;
                    PACInv.SpecialInvestigation = PACInvestigation.PACInvestigation.SpecialInvestigation;
                    PACInv.Creatinine = PACInvestigation.PACInvestigation.Creatinine;
                    PACInv.DLC = PACInvestigation.PACInvestigation.DLC;
                    PACInv.PT = PACInvestigation.PACInvestigation.PT;
                    PACInv.PTTK = PACInvestigation.PACInvestigation.PTTK;
                    PACInv.SBil = PACInvestigation.PACInvestigation.SBil;
                    PACInv.SGOTSGPT = PACInvestigation.PACInvestigation.SGOTSGPT;
                    PACInv.ALKPOGGT = PACInvestigation.PACInvestigation.ALKPOGGT;
                    PACInv.SProtein = PACInvestigation.PACInvestigation.SProtein;
                    PACInv.UnfitReasons = PACInvestigation.PACInvestigation.UnfitReasons;
                    PACInv.Reviewedby1 = PACInvestigation.PACInvestigation.Reviewedby1;
                    PACInv.Reviewedby2 = PACInvestigation.PACInvestigation.Reviewedby2;
                    PACInv.Reviewedby3 = PACInvestigation.PACInvestigation.Reviewedby3;
                    PACInv.Reviewedon1 = PACInvestigation.PACInvestigation.Reviewedon1;
                    PACInv.Reviewedon2 = PACInvestigation.PACInvestigation.Reviewedon2;
                    PACInv.Reviewedon3 = PACInvestigation.PACInvestigation.Reviewedon3;
                    PACInv.Remarks1 = PACInvestigation.PACInvestigation.Remarks1;
                    PACInv.Remarks2 = PACInvestigation.PACInvestigation.Remarks2;
                    PACInv.Remarks3 = PACInvestigation.PACInvestigation.Remarks3;
                    PACInv.PlanGA = PACInvestigation.PACInvestigation.PlanGA;
                    PACInv.CreatedUTC = DateTime.UtcNow;
                    PACInv.CreatedBy = PACInvestigation.PACInvestigation.CreatedBy;
                    PACInv.DoctorID = PACInvestigation.DoctorID;
                    WYNKContext.PACInvestigation.AddRange(PACInv);
                    WYNKContext.SaveChanges();

                    dbContextTransaction.Commit();

                    try
                    {
                        if (WYNKContext.SaveChanges() >= 0)
                            return new
                            {
                                Success = true,
                                Message = "Saved successfully",
                                PACIVID = PACInv.PACInvestigationID
                            };
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex);
                    }

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                }
                return new
                {
                    Success = false,
                    Message = "Some data are Missing"

                };
            }
        }
        public dynamic PreOperativeInsert(PACViewM PreOperativeInstruction)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    var PACPreOp = new PACPreOperativeInstruction();
                    PACPreOp.ADMID = PreOperativeInstruction.PACPreOperativeInstruction.ADMID;
                    PACPreOp.CMPID = PreOperativeInstruction.PACPreOperativeInstruction.CMPID;
                    PACPreOp.UIN = PreOperativeInstruction.PACPreOperativeInstruction.UIN;
                    PACPreOp.NilOralyAfterTime = PreOperativeInstruction.PACPreOperativeInstruction.NilOralyAfterTime;
                    PACPreOp.NilOralyAfterDate = PreOperativeInstruction.PACPreOperativeInstruction.NilOralyAfterDate;
                    PACPreOp.ArrangeFor = PreOperativeInstruction.PACPreOperativeInstruction.ArrangeFor;
                    PACPreOp.BloodProduct = PreOperativeInstruction.PACPreOperativeInstruction.BloodProduct;
                    PACPreOp.ShiftedOTTime = PreOperativeInstruction.PACPreOperativeInstruction.ShiftedOTTime;
                    PACPreOp.ShiftedOTDate = PreOperativeInstruction.PACPreOperativeInstruction.ShiftedOTDate;
                    PACPreOp.SpecialInstructions = PreOperativeInstruction.PACPreOperativeInstruction.SpecialInstructions;
                    PACPreOp.PreMedications = PreOperativeInstruction.PACPreOperativeInstruction.PreMedications;
                    PACPreOp.CreatedUTC = DateTime.UtcNow;
                    PACPreOp.CreatedBy = PreOperativeInstruction.PACPreOperativeInstruction.CreatedBy;
                    PACPreOp.DoctorID = PreOperativeInstruction.DoctorID;
                    WYNKContext.PACPreOperativeInstruction.AddRange(PACPreOp);
                    WYNKContext.SaveChanges();

                    dbContextTransaction.Commit();

                    try
                    {
                        if (WYNKContext.SaveChanges() >= 0)
                            return new
                            {
                                Success = true,
                                Message = "Saved successfully",
                                PPOID = PACPreOp.PACPreOperativeID
                            };
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex);
                    }

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                }
                return new
                {
                    Success = false,
                    Message = "Some data are Missing"

                };
            }
        }
        public PACViewM getPACHistory(string UIN, int ADMID, int ID)
        {
            var getData = new PACViewM();
            var M_PACHistory = WYNKContext.PACHistory.ToList();
            var M_PACHistoryTran = WYNKContext.PACHistoryTran.ToList();

            getData.getPACHistoryDet = (from PACH in M_PACHistory.Where(x => x.CMPID == ID && x.ADMID == ADMID && x.UIN == UIN)


                                        select new getPACHistoryDet
                                        {
                                            PACHistoryID = PACH.PACHistoryID,
                                            Previous_Operation = PACH.Previous_Operation,
                                            ColdCoughFever = PACH.ColdCoughFever,
                                            PalpitationsBreathless = PACH.PalpitationsBreathless,
                                            Orthopnoea_PND = PACH.Orthopnoea_PND,
                                            CPD_Asthma = PACH.CPD_Asthma,
                                            Unconsciousness = PACH.Unconsciousness,
                                            Psychiatric_illness = PACH.Psychiatric_illness,
                                            Renal_Disorders = PACH.Renal_Disorders,
                                            Jaundice = PACH.Jaundice,
                                            HTN_IHD_CHD_DM = PACH.HTN_IHD_CHD_DM,
                                            Bleeding_Disorders = PACH.Bleeding_Disorders,
                                            Exercise_Tolerance = Enum.GetName(typeof(PACGoodFairPoor), PACH.Exercise_Tolerance),
                                            Menstrual = PACH.Menstrual,
                                            Family_Ho = PACH.Family_Ho,
                                            Last_Meal_Time = PACH.Last_Meal_Time,
                                            Smoking_Tobacco = PACH.Smoking_Tobacco,
                                            Alcohol_habituation = PACH.Alcohol_habituation,
                                            Drug_Addictions = PACH.Drug_Addictions,
                                            Remarks = PACH.Remarks,
                                            DoctorID = CMPSContext.DoctorMaster.Where(x => x.DoctorID == PACH.DoctorID).Select(x => x.Firstname + " " + x.MiddleName + " " + x.LastName).FirstOrDefault(),
                                        }).ToList();



            getData.getPACHistoryTranDet = (from PACH in M_PACHistory.Where(x => x.CMPID == ID && x.ADMID == ADMID && x.UIN == UIN)
                                            join PACHT in M_PACHistoryTran on PACH.PACHistoryID equals PACHT.PACHistoryID

                                            select new getPACHistoryTranDet
                                            {

                                                Dosage = PACHT.Dossage,
                                                frequency = PACHT.Frequency,
                                                DrugDescription = PACHT.Drug_Description,
                                                DrugName = WYNKContext.DrugMaster.Where(x => x.ID == PACHT.DrugID).Select(x => x.Brand).FirstOrDefault(),
                                                GenericName = WYNKContext.DrugGroup.Where(y => y.ID == WYNKContext.DrugMaster.Where(x => x.ID == PACHT.DrugID).Select(x => x.GenericName).FirstOrDefault()).Select(z => z.Description).FirstOrDefault(),
                                                DrugID = PACHT.DrugID

                                            }).ToList();




            return getData;

        }
        public PACViewM getPACExamination(string UIN, int ADMID, int ID)
        {
            var getData = new PACViewM();

            var PACExam = WYNKContext.PACExamination.ToList();


            getData.getPACExamDet = (from PACE in PACExam.Where(x => x.CMPID == ID && x.ADMID == ADMID && x.UIN == UIN)


                                     select new getPACExamDet
                                     {
                                         PACExamID = PACE.PACExamID,
                                         PulseHeartRate = PACE.PulseHeartRate,
                                         Respiration = PACE.Respiration,
                                         BloodPressure = PACE.BloodPressure,
                                         temperature = PACE.temperature,
                                         GC = Enum.GetName(typeof(PACGoodFairPoor), PACE.GC),
                                         Weight = PACE.Weight,
                                         Height = PACE.Height,
                                         BMI = PACE.BMI,
                                         NStatus = Enum.GetName(typeof(NStatus), PACE.NStatus),
                                         Pallor = PACE.Pallor,
                                         Icterus = PACE.Icterus,
                                         Cyanosis = PACE.Cyanosis,
                                         Clubbing = PACE.Clubbing,
                                         LAP = PACE.LAP,
                                         Oedema = PACE.Oedema,
                                         JVP = PACE.JVP,
                                         Veins = Enum.GetName(typeof(PACGoodFairPoor), PACE.Veins),
                                         Spine = PACE.Spine,
                                         BHT = PACE.BHT,
                                         TMJoint = PACE.TMJoint,
                                         MouthOpening = PACE.MouthOpening,
                                         LooseMissedTeeth = PACE.LooseMissedTeeth,
                                         MallampattiClass = PACE.MallampattiClass,
                                         Thyromentaldist = PACE.Thyromentaldist,
                                         OralHygiene = Enum.GetName(typeof(PACGoodFairPoor), PACE.OralHygiene),
                                         ArtificialTeeth = PACE.ArtificialTeeth,
                                         NasalPatency = PACE.NasalPatency,
                                         UpperRespiratory = PACE.UpperRespiratory,
                                         LowerRespiratory = PACE.LowerRespiratory,
                                         CVS = PACE.CVS,
                                         CNS = PACE.CNS,
                                         PerAbdomen = PACE.PerAbdomen,
                                         Neck = PACE.Neck,
                                         DoctorID = CMPSContext.DoctorMaster.Where(x => x.DoctorID == PACE.DoctorID).Select(x => x.Firstname + " " + x.MiddleName + " " + x.LastName).FirstOrDefault(),
                                     }).ToList();






            return getData;

        }
        public PACViewM getPACInvestigation(string UIN, int ADMID, int ID)
        {
            var getData = new PACViewM();

            var PACINV = WYNKContext.PACInvestigation.ToList();


            getData.getPACInv = (from PACI in PACINV.Where(x => x.CMPID == ID && x.ADMID == ADMID && x.UIN == UIN)


                                 select new getPACInv
                                 {
                                     PACInvestigationID = PACI.PACInvestigationID,
                                     Hb_PCV = PACI.Hb_PCV,
                                     PLCount = PACI.PLCount,
                                     BLGlu = PACI.BLGlu,
                                     SUricAcid = PACI.SUricAcid,
                                     T3 = PACI.T3,
                                     T4 = PACI.T4,
                                     TSH = PACI.TSH,
                                     SElectCAPlus = PACI.SElectCAPlus,
                                     SElectKPlus = PACI.SElectKPlus,
                                     SElectNAPlus = PACI.SElectNAPlus,
                                     UrineRMALB = PACI.UrineRMALB,
                                     UrineRMSugar = PACI.UrineRMSugar,
                                     UrineRMKET = PACI.UrineRMKET,
                                     UrineRMWBC = PACI.UrineRMWBC,
                                     UrineRMRBC = PACI.UrineRMRBC,
                                     XRay = PACI.XRay,
                                     ECG = PACI.ECG,
                                     ECHO = PACI.ECHO,
                                     ABG = PACI.ABG,
                                     SpecialInvestigation = PACI.SpecialInvestigation,
                                     TLC = PACI.TLC,
                                     BlGroup = PACI.BlGroup,
                                     BUrea = PACI.BUrea,
                                     Creatinine = PACI.Creatinine,
                                     HBSAG = PACI.HBSAG,
                                     HIV = PACI.HIV,
                                     DLC = PACI.DLC,
                                     PT = PACI.PT,
                                     PTTK = PACI.PTTK,
                                     SBil = PACI.SBil,
                                     SGOTSGPT = PACI.SGOTSGPT,
                                     ALKPOGGT = PACI.ALKPOGGT,
                                     SProtein = PACI.SProtein,
                                     UnfitReasons = PACI.UnfitReasons,
                                     Reviewedby1 = PACI.Reviewedby1,
                                     Reviewedby2 = PACI.Reviewedby2,
                                     Reviewedby3 = PACI.Reviewedby3,
                                     Reviewedon1 = PACI.Reviewedon1,
                                     Reviewedon2 = PACI.Reviewedon2,
                                     Reviewedon3 = PACI.Reviewedon3,
                                     Remarks1 = PACI.Remarks1,
                                     Remarks2 = PACI.Remarks2,
                                     Remarks3 = PACI.Remarks3,
                                     PlanGA = PACI.PlanGA,
                                     DoctorID = CMPSContext.DoctorMaster.Where(x => x.DoctorID == PACI.DoctorID).Select(x => x.Firstname + " " + x.MiddleName + " " + x.LastName).FirstOrDefault(),
                                 }).ToList();






            return getData;

        }
        public PACViewM getPACPreOperative(string UIN, int ADMID, int ID)
        {
            var getData = new PACViewM();

            var PACPOI = WYNKContext.PACPreOperativeInstruction.ToList();


            getData.getPACPreOperative = (from PACI in PACPOI.Where(x => x.CMPID == ID && x.ADMID == ADMID && x.UIN == UIN)


                                          select new getPACPreOperative
                                          {
                                              PACPreOperativeID = PACI.PACPreOperativeID,
                                              NilOralyAfterTime = PACI.NilOralyAfterTime,
                                              NilOralyAfterDate = PACI.NilOralyAfterDate,
                                              ArrangeFor = PACI.ArrangeFor,
                                              BloodProduct = PACI.BloodProduct,
                                              ShiftedOTDate = PACI.ShiftedOTDate,
                                              ShiftedOTTime = PACI.ShiftedOTTime,
                                              SpecialInstructions = PACI.SpecialInstructions,
                                              PreMedications = PACI.PreMedications,
                                              DoctorID = CMPSContext.DoctorMaster.Where(x => x.DoctorID == PACI.DoctorID).Select(x => x.Firstname + " " + x.MiddleName + " " + x.LastName).FirstOrDefault(),
                                          }).ToList();






            return getData;

        }
        public dynamic UpdateData(PACViewM PACHistory, int PACHID)

        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    var PACHistoryModel = new PACHistoryModel();

                    PACHistoryModel = WYNKContext.PACHistory.Where(x => x.PACHistoryID == PACHID).FirstOrDefault();
                    PACHistoryModel.ADMID = PACHistory.PACHistoryModel.ADMID;
                    PACHistoryModel.CMPID = PACHistory.PACHistoryModel.CMPID;
                    PACHistoryModel.UIN = PACHistory.PACHistoryModel.UIN;
                    PACHistoryModel.Previous_Operation = PACHistory.PACHistoryModel.Previous_Operation;
                    PACHistoryModel.ColdCoughFever = PACHistory.PACHistoryModel.ColdCoughFever;
                    PACHistoryModel.PalpitationsBreathless = PACHistory.PACHistoryModel.PalpitationsBreathless;
                    PACHistoryModel.Orthopnoea_PND = PACHistory.PACHistoryModel.Orthopnoea_PND;
                    PACHistoryModel.CPD_Asthma = PACHistory.PACHistoryModel.CPD_Asthma;
                    PACHistoryModel.Unconsciousness = PACHistory.PACHistoryModel.Unconsciousness;
                    PACHistoryModel.Psychiatric_illness = PACHistory.PACHistoryModel.Psychiatric_illness;
                    PACHistoryModel.Renal_Disorders = PACHistory.PACHistoryModel.Renal_Disorders;
                    PACHistoryModel.Jaundice = PACHistory.PACHistoryModel.Jaundice;
                    PACHistoryModel.HTN_IHD_CHD_DM = PACHistory.PACHistoryModel.HTN_IHD_CHD_DM;
                    PACHistoryModel.Bleeding_Disorders = PACHistory.PACHistoryModel.Bleeding_Disorders;
                    PACHistoryModel.Exercise_Tolerance = PACHistory.PACHistoryModel.Exercise_Tolerance;
                    if (PACHistory.PACHistoryModel.Family_Ho == " ")
                    {
                        PACHistoryModel.Family_Ho = "     ";
                    }
                    else
                    {
                        PACHistoryModel.Family_Ho = PACHistory.PACHistoryModel.Family_Ho;
                    }

                    if (PACHistory.PACHistoryModel.Menstrual == null)
                    {
                        PACHistoryModel.Menstrual = "     ";
                    }
                    else
                    {
                        PACHistoryModel.Menstrual = PACHistory.PACHistoryModel.Menstrual;
                    }


                    PACHistoryModel.Last_Meal_Time = PACHistory.PACHistoryModel.Last_Meal_Time;
                    PACHistoryModel.Smoking_Tobacco = PACHistory.PACHistoryModel.Smoking_Tobacco;
                    PACHistoryModel.Alcohol_habituation = PACHistory.PACHistoryModel.Alcohol_habituation;
                    PACHistoryModel.Drug_Addictions = PACHistory.PACHistoryModel.Drug_Addictions;
                    PACHistoryModel.Known_allergies = PACHistory.PACHistoryModel.Known_allergies;
                    PACHistoryModel.Remarks = PACHistory.PACHistoryModel.Remarks;
                    PACHistoryModel.DoctorID = PACHistory.DoctorID;
                    PACHistoryModel.UpdatedUTC = DateTime.UtcNow;
                    PACHistoryModel.UpdatedBy = PACHistory.PACHistoryModel.UpdatedBy;
                    WYNKContext.PACHistory.UpdateRange(PACHistoryModel);
                    WYNKContext.SaveChanges();



                    if (PACHistory.pushData.Count() > 0)
                    {
                        var ids = WYNKContext.PACHistoryTran.Where(x => x.PACHistoryID == PACHID).ToList();
                        foreach (var item in ids.ToList())
                        {
                            if (ids != null && ids.Count != 0)
                            {
                                ids.All(x => { x.PACHistoryID = item.PACHistoryID; return true; });
                                var idsss = WYNKContext.PACHistoryTran.Where(x => x.PACHistoryID == PACHID).ToList();
                                if (idsss.Count != 0)
                                {
                                    WYNKContext.PACHistoryTran.RemoveRange(ids);
                                    WYNKContext.SaveChanges();
                                }
                            }
                        }

                        var ids1 = WYNKContext.PACHistoryTran.Where(x => x.PACHistoryID == PACHID).ToList();

                        if (ids1.Count == 0)
                        {
                            if (PACHistory.pushData.Count() > 0)
                            {
                                foreach (var PACHistoryTran in PACHistory.pushData.ToList())
                                {
                                    var PACHTran = new PACHistoryTranModel();
                                    PACHTran.PACHistoryID = PACHistoryModel.PACHistoryID;
                                    PACHTran.DrugID = PACHistoryTran.DrugID;
                                    PACHTran.Dossage = PACHistoryTran.Dosage;
                                    PACHTran.Frequency = PACHistoryTran.frequency;
                                    PACHTran.Drug_Description = PACHistoryTran.DrugDescription;
                                    PACHTran.CreatedUTC = DateTime.UtcNow;
                                    PACHTran.CreatedBy = PACHistoryModel.CreatedBy;
                                    WYNKContext.PACHistoryTran.AddRange(PACHTran);
                                }
                            }
                        }
                    }



                    WYNKContext.SaveChanges();

                    dbContextTransaction.Commit();
                    try
                    {
                        if (WYNKContext.SaveChanges() >= 0)
                            return new
                            {
                                Success = true,
                                Message = "Updated Sucessfully",
                                PACID = PACHistoryModel.PACHistoryID
                            };
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex);
                    }
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                }
                return new
                {
                    Success = false,
                    Message = "Some Data Is Missing"
                };
            }

        }
        public dynamic UpdateExamData(PACViewM PACExamination, int PACHID)

        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {


                    var PACExam = new PACExamination();

                    PACExam = WYNKContext.PACExamination.Where(x => x.PACExamID == PACHID).FirstOrDefault();
                    PACExam.ADMID = PACExamination.PACExamination.ADMID;
                    PACExam.CMPID = PACExamination.PACExamination.CMPID;
                    PACExam.UIN = PACExamination.PACExamination.UIN;
                    PACExam.PulseHeartRate = PACExamination.PACExamination.PulseHeartRate;
                    PACExam.BloodPressure = PACExamination.PACExamination.BloodPressure;
                    PACExam.Respiration = PACExamination.PACExamination.Respiration;
                    PACExam.temperature = PACExamination.PACExamination.temperature;
                    PACExam.Spine = PACExamination.PACExamination.Spine;
                    PACExam.Weight = PACExamination.PACExamination.Weight;
                    PACExam.Height = PACExamination.PACExamination.Height;
                    PACExam.BMI = PACExamination.PACExamination.BMI;
                    PACExam.NStatus = PACExamination.PACExamination.NStatus;
                    PACExam.Icterus = PACExamination.PACExamination.Icterus;
                    PACExam.Cyanosis = PACExamination.PACExamination.Cyanosis;
                    PACExam.Clubbing = PACExamination.PACExamination.Clubbing;
                    PACExam.LAP = PACExamination.PACExamination.LAP;
                    PACExam.BHT = PACExamination.PACExamination.BHT;
                    PACExam.Oedema = PACExamination.PACExamination.Oedema;
                    PACExam.JVP = PACExamination.PACExamination.JVP;
                    PACExam.Veins = PACExamination.PACExamination.Veins;
                    PACExam.GC = PACExamination.PACExamination.GC;
                    PACExam.Pallor = PACExamination.PACExamination.Pallor;
                    PACExam.Thyromentaldist = PACExamination.PACExamination.Thyromentaldist;
                    PACExam.MouthOpening = PACExamination.PACExamination.MouthOpening;
                    PACExam.LooseMissedTeeth = PACExamination.PACExamination.LooseMissedTeeth;
                    PACExam.MallampattiClass = PACExamination.PACExamination.MallampattiClass;
                    PACExam.NasalPatency = PACExamination.PACExamination.NasalPatency;
                    PACExam.ArtificialTeeth = PACExamination.PACExamination.ArtificialTeeth;
                    PACExam.OralHygiene = PACExamination.PACExamination.OralHygiene;
                    PACExam.TMJoint = PACExamination.PACExamination.TMJoint;
                    PACExam.UpperRespiratory = PACExamination.PACExamination.UpperRespiratory;
                    PACExam.LowerRespiratory = PACExamination.PACExamination.LowerRespiratory;
                    PACExam.CVS = PACExamination.PACExamination.CVS;
                    PACExam.CNS = PACExamination.PACExamination.CNS;
                    PACExam.PerAbdomen = PACExamination.PACExamination.PerAbdomen;
                    PACExam.Neck = PACExamination.PACExamination.Neck;
                    PACExam.DoctorID = PACExamination.DoctorID;
                    PACExam.UpdatedUTC = DateTime.UtcNow;
                    PACExam.UpdatedBy = PACExamination.PACExamination.UpdatedBy;

                    WYNKContext.PACExamination.UpdateRange(PACExam);
                    WYNKContext.SaveChanges();

                    dbContextTransaction.Commit();
                    try
                    {
                        if (WYNKContext.SaveChanges() >= 0)
                            return new
                            {
                                Success = true,
                                Message = "Updated Sucessfully",
                                PacexamID = PACExam.PACExamID
                            };
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex);
                    }
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                }
                return new
                {
                    Success = false,
                    Message = "Some Data Is Missing"
                };
            }

        }
        public dynamic UpdateInvData(PACViewM PACInvestigation, int PACHID)

        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {


                    var PACInv = new PACInvestigation();
                    PACInv = WYNKContext.PACInvestigation.Where(x => x.PACInvestigationID == PACHID).FirstOrDefault();
                    PACInv.ADMID = PACInvestigation.PACInvestigation.ADMID;
                    PACInv.CMPID = PACInvestigation.PACInvestigation.CMPID;
                    PACInv.UIN = PACInvestigation.PACInvestigation.UIN;
                    PACInv.Hb_PCV = PACInvestigation.PACInvestigation.Hb_PCV;
                    PACInv.TLC = PACInvestigation.PACInvestigation.TLC;
                    PACInv.PLCount = PACInvestigation.PACInvestigation.PLCount;
                    PACInv.BLGlu = PACInvestigation.PACInvestigation.BLGlu;
                    PACInv.SUricAcid = PACInvestigation.PACInvestigation.SUricAcid;
                    PACInv.T3 = PACInvestigation.PACInvestigation.T3;
                    PACInv.T4 = PACInvestigation.PACInvestigation.T4;
                    PACInv.TSH = PACInvestigation.PACInvestigation.TSH;
                    PACInv.TSH = PACInvestigation.PACInvestigation.TSH;
                    PACInv.SElectNAPlus = PACInvestigation.PACInvestigation.SElectNAPlus;
                    PACInv.SElectKPlus = PACInvestigation.PACInvestigation.SElectKPlus;
                    PACInv.SElectCAPlus = PACInvestigation.PACInvestigation.SElectCAPlus;
                    PACInv.UrineRMALB = PACInvestigation.PACInvestigation.UrineRMALB;
                    PACInv.UrineRMSugar = PACInvestigation.PACInvestigation.UrineRMSugar;
                    PACInv.UrineRMKET = PACInvestigation.PACInvestigation.UrineRMKET;
                    PACInv.UrineRMWBC = PACInvestigation.PACInvestigation.UrineRMWBC;
                    PACInv.UrineRMRBC = PACInvestigation.PACInvestigation.UrineRMRBC;
                    PACInv.BlGroup = PACInvestigation.PACInvestigation.BlGroup;
                    PACInv.BUrea = PACInvestigation.PACInvestigation.BUrea;
                    PACInv.HBSAG = PACInvestigation.PACInvestigation.HBSAG;
                    PACInv.HIV = PACInvestigation.PACInvestigation.HIV;
                    PACInv.XRay = PACInvestigation.PACInvestigation.XRay;
                    PACInv.ECG = PACInvestigation.PACInvestigation.ECG;
                    PACInv.ECHO = PACInvestigation.PACInvestigation.ECHO;
                    PACInv.ABG = PACInvestigation.PACInvestigation.ABG;
                    PACInv.SpecialInvestigation = PACInvestigation.PACInvestigation.SpecialInvestigation;
                    PACInv.Creatinine = PACInvestigation.PACInvestigation.Creatinine;
                    PACInv.DLC = PACInvestigation.PACInvestigation.DLC;
                    PACInv.PT = PACInvestigation.PACInvestigation.PT;
                    PACInv.PTTK = PACInvestigation.PACInvestigation.PTTK;
                    PACInv.SBil = PACInvestigation.PACInvestigation.SBil;
                    PACInv.SGOTSGPT = PACInvestigation.PACInvestigation.SGOTSGPT;
                    PACInv.ALKPOGGT = PACInvestigation.PACInvestigation.ALKPOGGT;
                    PACInv.SProtein = PACInvestigation.PACInvestigation.SProtein;
                    PACInv.UnfitReasons = PACInvestigation.PACInvestigation.UnfitReasons;
                    PACInv.Reviewedby1 = PACInvestigation.PACInvestigation.Reviewedby1;
                    PACInv.Reviewedby2 = PACInvestigation.PACInvestigation.Reviewedby2;
                    PACInv.Reviewedby3 = PACInvestigation.PACInvestigation.Reviewedby3;
                    PACInv.Reviewedon1 = PACInvestigation.PACInvestigation.Reviewedon1;
                    PACInv.Reviewedon2 = PACInvestigation.PACInvestigation.Reviewedon2;
                    PACInv.Reviewedon3 = PACInvestigation.PACInvestigation.Reviewedon3;
                    PACInv.Remarks1 = PACInvestigation.PACInvestigation.Remarks1;
                    PACInv.Remarks2 = PACInvestigation.PACInvestigation.Remarks2;
                    PACInv.Remarks3 = PACInvestigation.PACInvestigation.Remarks3;
                    PACInv.PlanGA = PACInvestigation.PACInvestigation.PlanGA;
                    PACInv.DoctorID = PACInvestigation.DoctorID;
                    PACInv.UpdatedUTC = DateTime.UtcNow;
                    PACInv.UpdatedBy = PACInvestigation.PACInvestigation.UpdatedBy;

                    WYNKContext.PACInvestigation.UpdateRange(PACInv);
                    WYNKContext.SaveChanges();

                    dbContextTransaction.Commit();
                    try
                    {
                        if (WYNKContext.SaveChanges() >= 0)
                            return new
                            {
                                Success = true,
                                Message = "Updated Sucessfully",
                                PACIVID = PACInv.PACInvestigationID
                            };
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex);
                    }
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                }
                return new
                {
                    Success = false,
                    Message = "Some Data Is Missing"
                };
            }

        }
        public dynamic UpdatePreOperativeInstructions(PACViewM PACPreOperative, int PACPOID)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {

                    var PreOperative = new PACPreOperativeInstruction();
                    PreOperative = WYNKContext.PACPreOperativeInstruction.Where(x => x.PACPreOperativeID == PACPOID).FirstOrDefault();
                    PreOperative.ADMID = PACPreOperative.PACPreOperativeInstruction.ADMID;
                    PreOperative.CMPID = PACPreOperative.PACPreOperativeInstruction.CMPID;
                    PreOperative.UIN = PACPreOperative.PACPreOperativeInstruction.UIN;
                    PreOperative.NilOralyAfterTime = PACPreOperative.PACPreOperativeInstruction.NilOralyAfterTime;
                    PreOperative.NilOralyAfterDate = PACPreOperative.PACPreOperativeInstruction.NilOralyAfterDate;
                    PreOperative.ArrangeFor = PACPreOperative.PACPreOperativeInstruction.ArrangeFor;
                    PreOperative.BloodProduct = PACPreOperative.PACPreOperativeInstruction.BloodProduct;
                    PreOperative.ShiftedOTTime = PACPreOperative.PACPreOperativeInstruction.ShiftedOTTime;
                    PreOperative.ShiftedOTDate = PACPreOperative.PACPreOperativeInstruction.ShiftedOTDate;
                    PreOperative.SpecialInstructions = PACPreOperative.PACPreOperativeInstruction.SpecialInstructions;
                    PreOperative.PreMedications = PACPreOperative.PACPreOperativeInstruction.PreMedications;
                    PreOperative.DoctorID = PACPreOperative.DoctorID;
                    PreOperative.UpdatedUTC = DateTime.UtcNow;
                    PreOperative.UpdatedBy = PACPreOperative.PACPreOperativeInstruction.UpdatedBy;

                    WYNKContext.PACPreOperativeInstruction.UpdateRange(PreOperative);
                    WYNKContext.SaveChanges();

                    dbContextTransaction.Commit();
                    try
                    {
                        if (WYNKContext.SaveChanges() >= 0)
                            return new
                            {
                                Success = true,
                                Message = "Updated Sucessfully",
                                PPOID = PreOperative.PACPreOperativeID
                            };
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex);
                    }
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                }
                return new
                {
                    Success = false,
                    Message = "Some Data Is Missing"
                };
            }

        }
        public dynamic Beforeinductionofanaesthesia(int CMPID, int SAID)
        {

            var det = new PACViewM();
            det.Beforeinductionoana = new List<Beforeinductionoana>();

            var oneline = CMPSContext.OneLineMaster.ToList();
            var ssc = WYNKContext.SurgerySafetyCheckList.ToList();

            var OnelineMaster = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Before induction of anaesthesia" && x.ParentTag == "SSCType" && x.IsActive == true).Select(x => x.OLMID).FirstOrDefault();
            var r = WYNKContext.SscResponse.Where(x => x.SAID == Convert.ToString(SAID) && x.OLMID == OnelineMaster).Select(c => c.SAID).FirstOrDefault();

           



            return det;


        }
        public dynamic Insertbeforeana(PACViewM Addbefore)
        {


            try
            {
                var reg = 0;

                if (reg == 0)
                {

                    var SscResponseTran = new SscResponseTran();


                    if (Addbefore.Beforeinductionoana.Count() > 0)
                    {
                        foreach (var item in Addbefore.Beforeinductionoana.ToList())

                        {
                            var ss = new SscResponse();
                            ss.SSCID = item.SSCID;
                            ss.CmpID = Addbefore.Cmpid;

                            if (item.Yes)
                            {
                                ss.Response = 1;
                            }
                            else if (item.No)
                            {

                                ss.Response = 0;
                            }
                            else
                            {
                                ss.Response = 2;
                            }

                            ss.Description = item.Description;
                            ss.IsActive = true;
                            ss.IsDeleted = false;
                            ss.CreatedUTC = DateTime.UtcNow;
                            ss.CreatedBy = Addbefore.CreatedBy;
                            ss.OLMID = item.OLMID;
                            WYNKContext.SscResponse.AddRange(ss);
                            WYNKContext.SaveChanges();

                        }


                        SscResponseTran.OLMID = Addbefore.Beforeinductionoana.Select(x => x.OLMID).FirstOrDefault();
                        SscResponseTran.SAID = Addbefore.SAID;
                        SscResponseTran.STAFFID = Addbefore.DoctorID;
                        SscResponseTran.CreatedUTC = DateTime.UtcNow;
                        SscResponseTran.CreatedBy = Addbefore.CreatedBy;
                        WYNKContext.SscResponseTran.Add(SscResponseTran);
                        WYNKContext.SaveChanges();

                    }
                }

                else
                {

                    if (Addbefore.Beforeinductionoana.Count() > 0)
                    {
                        var SscResponseTranup = new SscResponseTran();

                        foreach (var item in Addbefore.Beforeinductionoana.ToList())

                        {
                            var ss = new SscResponse();

                            var ID = item.SSCRID;

                            if (ID != 0)
                            {
                                ss = WYNKContext.SscResponse.Where(x => x.SSCRID == ID).FirstOrDefault();
                                ss.SSCID = item.SSCID;
                                ss.CmpID = Addbefore.Cmpid;

                                if (item.Yes)
                                {
                                    ss.Response = 1;
                                }
                                else if (item.No)
                                {

                                    ss.Response = 0;
                                }
                                else
                                {
                                    ss.Response = 2;
                                }

                                ss.Description = item.Description;
                                ss.IsActive = true;
                                ss.IsDeleted = false;
                                ss.UpdatedUTC = DateTime.UtcNow;
                                ss.OLMID = item.OLMID;
                                ss.UpdatedBy = Addbefore.CreatedBy;
                                WYNKContext.SscResponse.UpdateRange(ss);
                                WYNKContext.SaveChanges();
                            }

                        }


                        var scid = Addbefore.Beforeinductionoana.Select(x => x.ID).FirstOrDefault();

                        SscResponseTranup = WYNKContext.SscResponseTran.Where(x => x.ID == scid).FirstOrDefault();

                        SscResponseTranup.OLMID = Addbefore.Beforeinductionoana.Select(x => x.OLMID).FirstOrDefault();
                        SscResponseTranup.SAID = Addbefore.SAID;
                        SscResponseTranup.STAFFID = Addbefore.DoctorID;
                        SscResponseTranup.UpdatedUTC = DateTime.UtcNow;
                        SscResponseTranup.UpdatedBy = Addbefore.CreatedBy;
                        WYNKContext.SscResponseTran.UpdateRange(SscResponseTranup);
                        WYNKContext.SaveChanges();


                    }

                }


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
        public dynamic Beforeinductionofanaesthesiaprint(int OLD, int SAID, int cmpid)
        {

            var det = new PACViewM();
            det.Beforeinductionoanaprint = new List<Beforeinductionoanaprint>();
            var ssc = WYNKContext.SurgerySafetyCheckList.ToList();
            var DM = CMPSContext.DoctorMaster.ToList();

            det.Address = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address1).FirstOrDefault();
            det.Address1 = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address2).FirstOrDefault();
            det.Address2 = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address2).FirstOrDefault();
            det.phone = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Phone1).FirstOrDefault();
            det.web = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Website).FirstOrDefault();
            det.Compnayname = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.CompanyName).FirstOrDefault();


            return det;


        }
        public dynamic PACHISTORYPRINT(int PACID, int cmpid)
        {
            var getDataprint = new PACViewM();
            var M_PACHistory = WYNKContext.PACHistory.ToList();
            var M_PACHistoryTran = WYNKContext.PACHistoryTran.ToList();



            getDataprint.Address = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address1).FirstOrDefault();
            getDataprint.Address1 = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address2).FirstOrDefault();
            getDataprint.Address2 = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address2).FirstOrDefault();
            getDataprint.phone = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Phone1).FirstOrDefault();
            getDataprint.web = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Website).FirstOrDefault();
            getDataprint.Compnayname = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.CompanyName).FirstOrDefault();


            getDataprint.getPACHistoryDetprint = (from PACH in M_PACHistory.Where(x => x.PACHistoryID == PACID && x.CMPID == cmpid)


                                                  select new getPACHistoryDetprint
                                                  {
                                                      Previous_Operation = PACH.Previous_Operation == true ? "Y" : "N",
                                                      ColdCoughFever = PACH.ColdCoughFever == true ? "Y" : "N",
                                                      PalpitationsBreathless = PACH.PalpitationsBreathless == true ? "Y" : "N",
                                                      Orthopnoea_PND = PACH.Orthopnoea_PND == true ? "Y" : "N",
                                                      CPD_Asthma = PACH.CPD_Asthma == true ? "Y" : "N",
                                                      Unconsciousness = PACH.Unconsciousness == true ? "Y" : "N",
                                                      Psychiatric_illness = PACH.Psychiatric_illness == true ? "Y" : "N",
                                                      Renal_Disorders = PACH.Renal_Disorders == true ? "Y" : "N",
                                                      Jaundice = PACH.Jaundice == true ? "Y" : "N",
                                                      HTN_IHD_CHD_DM = PACH.HTN_IHD_CHD_DM == true ? "Y" : "N",
                                                      Bleeding_Disorders = PACH.Bleeding_Disorders == true ? "Y" : "N",
                                                      Exercise_Tolerance = Enum.GetName(typeof(PACGoodFairPoor), PACH.Exercise_Tolerance) == "Poor" ? "Poor" : Enum.GetName(typeof(PACGoodFairPoor), PACH.Exercise_Tolerance) == "Good" ? "Good" : "Fair",
                                                      Menstrual = PACH.Menstrual,
                                                      Family_Ho = PACH.Family_Ho,
                                                      Last_Meal_Time = PACH.Last_Meal_Time,
                                                      Smoking_Tobacco = PACH.Smoking_Tobacco == true ? "Y" : "N",
                                                      Alcohol_habituation = PACH.Alcohol_habituation == true ? "Y" : "N",
                                                      Drug_Addictions = PACH.Drug_Addictions,
                                                      Remarks = PACH.Remarks,
                                                      Knownallergies = PACH.Known_allergies,
                                                      DoctorID = CMPSContext.DoctorMaster.Where(x => x.DoctorID == PACH.DoctorID).Select(x => x.Firstname + " " + x.MiddleName + " " + x.LastName).FirstOrDefault(),
                                                      Create = PACH.CreatedUTC,
                                                  }).ToList();


            getDataprint.getPACHistoryTranDetprint = (from PACH in M_PACHistoryTran.Where(x => x.PACHistoryID == PACID)

                                                      select new getPACHistoryTranDetprint
                                                      {
                                                          Dosage = PACH.Dossage,
                                                          frequency = PACH.Frequency,
                                                          DrugDescription = PACH.Drug_Description,
                                                          DrugName = WYNKContext.DrugMaster.Where(x => x.ID == PACH.DrugID).Select(x => x.Brand).FirstOrDefault(),
                                                          GenericName = WYNKContext.DrugGroup.Where(y => y.ID == WYNKContext.DrugMaster.Where(x => x.ID == PACH.DrugID).Select(x => x.GenericName).FirstOrDefault()).Select(z => z.Description).FirstOrDefault()
                                                      }).ToList();




            return getDataprint;

        }
        public dynamic PACPREOPINSPRINT(int PPOID, int cmpid)
        {
            var getDataprintpre = new PACViewM();
            var PACPreOperativeInstruction = WYNKContext.PACPreOperativeInstruction.ToList();

            getDataprintpre.Address = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address1).FirstOrDefault();
            getDataprintpre.Address1 = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address2).FirstOrDefault();
            getDataprintpre.Address2 = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address2).FirstOrDefault();
            getDataprintpre.phone = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Phone1).FirstOrDefault();
            getDataprintpre.web = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Website).FirstOrDefault();
            getDataprintpre.Compnayname = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.CompanyName).FirstOrDefault();


            getDataprintpre.getPACPreOperativeprint = (from PACI in PACPreOperativeInstruction.Where(x => x.PACPreOperativeID == PPOID && x.CMPID == cmpid)


                                                       select new getPACPreOperativeprint
                                                       {
                                                           NilOralyAfterTime = PACI.NilOralyAfterTime,
                                                           NilOralyAfterDate = PACI.NilOralyAfterDate,
                                                           ArrangeFor = PACI.ArrangeFor,
                                                           BloodProduct = PACI.BloodProduct,
                                                           ShiftedOTDate = PACI.ShiftedOTDate,
                                                           ShiftedOTTime = PACI.ShiftedOTTime,
                                                           SpecialInstructions = PACI.SpecialInstructions,
                                                           PreMedications = PACI.PreMedications,
                                                           createutc = PACI.CreatedUTC,
                                                           DoctorID = CMPSContext.DoctorMaster.Where(x => x.DoctorID == PACI.DoctorID).Select(x => x.Firstname + " " + x.MiddleName + " " + x.LastName).FirstOrDefault(),

                                                       }).ToList();




            return getDataprintpre;

        }
        public dynamic getPACExaminationprint(int PacexamID, int cmpid)
        {
            var getDataExamination = new PACViewM();

            getDataExamination.Address = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address1).FirstOrDefault();
            getDataExamination.Address1 = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address2).FirstOrDefault();
            getDataExamination.Address2 = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address2).FirstOrDefault();
            getDataExamination.phone = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Phone1).FirstOrDefault();
            getDataExamination.web = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Website).FirstOrDefault();
            getDataExamination.Compnayname = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.CompanyName).FirstOrDefault();

            var PACExam = WYNKContext.PACExamination.ToList();


            getDataExamination.getPACExamDetprint = (from PACE in PACExam.Where(x => x.PACExamID == PacexamID && x.CMPID == cmpid)


                                                     select new getPACExamDetprint
                                                     {

                                                         PulseHeartRate = PACE.PulseHeartRate,
                                                         Respiration = PACE.Respiration,
                                                         BloodPressure = PACE.BloodPressure,
                                                         temperature = PACE.temperature,
                                                         GC = Enum.GetName(typeof(PACGoodFairPoor), PACE.GC),
                                                         Weight = PACE.Weight,
                                                         Height = PACE.Height,
                                                         BMI = PACE.BMI,
                                                         NStatus = Enum.GetName(typeof(NStatus), PACE.NStatus),
                                                         Pallor = PACE.Pallor == true ? "Y" : "N",
                                                         Icterus = PACE.Icterus == true ? "Y" : "N",
                                                         Cyanosis = PACE.Cyanosis == true ? "Y" : "N",
                                                         Clubbing = PACE.Clubbing,
                                                         LAP = PACE.LAP == true ? "Y" : "N",
                                                         Oedema = PACE.Oedema == true ? "Y" : "N",
                                                         JVP = PACE.JVP,
                                                         Veins = Enum.GetName(typeof(PACGoodFairPoor), PACE.Veins),
                                                         Spine = PACE.Spine,
                                                         BHT = PACE.BHT,
                                                         TMJoint = PACE.TMJoint,
                                                         MouthOpening = PACE.MouthOpening,
                                                         LooseMissedTeeth = PACE.LooseMissedTeeth == true ? "Y" : "N",
                                                         MallampattiClass = PACE.MallampattiClass,
                                                         Thyromentaldist = PACE.Thyromentaldist,
                                                         OralHygiene = Enum.GetName(typeof(PACGoodFairPoor), PACE.OralHygiene),
                                                         ArtificialTeeth = PACE.ArtificialTeeth == true ? "Y" : "N",
                                                         NasalPatency = PACE.NasalPatency,
                                                         UpperRespiratory = PACE.UpperRespiratory,
                                                         LowerRespiratory = PACE.LowerRespiratory,
                                                         CVS = PACE.CVS,
                                                         CNS = PACE.CNS,
                                                         PerAbdomen = PACE.PerAbdomen,
                                                         Neck = PACE.Neck,
                                                         DoctorID = CMPSContext.DoctorMaster.Where(x => x.DoctorID == PACE.DoctorID).Select(x => x.Firstname + " " + x.MiddleName + " " + x.LastName).FirstOrDefault(),
                                                         CREATE = PACE.CreatedUTC

                                                     }).ToList();






            return getDataExamination;

        }
        public dynamic getPACInvestigationprint(int PACIVID, int cmpid)
        {
            var getData = new PACViewM();


            getData.Address = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address1).FirstOrDefault();
            getData.Address1 = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address2).FirstOrDefault();
            getData.Address2 = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address2).FirstOrDefault();
            getData.phone = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Phone1).FirstOrDefault();
            getData.web = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Website).FirstOrDefault();
            getData.Compnayname = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.CompanyName).FirstOrDefault();

            var PACINV = WYNKContext.PACInvestigation.ToList();


            getData.getPACInvprint = (from PACI in PACINV.Where(x => x.PACInvestigationID == PACIVID && x.CMPID == cmpid)


                                      select new getPACInvprint
                                      {

                                          Hb_PCV = PACI.Hb_PCV,
                                          PLCount = PACI.PLCount,
                                          BLGlu = PACI.BLGlu,
                                          SUricAcid = PACI.SUricAcid,
                                          T3 = PACI.T3,
                                          T4 = PACI.T4,
                                          TSH = PACI.TSH,
                                          SElectCAPlus = PACI.SElectCAPlus,
                                          SElectKPlus = PACI.SElectKPlus,
                                          SElectNAPlus = PACI.SElectNAPlus,
                                          UrineRMALB = PACI.UrineRMALB,
                                          UrineRMSugar = PACI.UrineRMSugar,
                                          UrineRMKET = PACI.UrineRMKET,
                                          UrineRMWBC = PACI.UrineRMWBC,
                                          UrineRMRBC = PACI.UrineRMRBC,
                                          XRay = PACI.XRay,
                                          ECG = PACI.ECG,
                                          ECHO = PACI.ECHO,
                                          ABG = PACI.ABG,
                                          SpecialInvestigation = PACI.SpecialInvestigation,
                                          TLC = PACI.TLC,
                                          BlGroup = PACI.BlGroup,
                                          BUrea = PACI.BUrea,
                                          Creatinine = PACI.Creatinine,
                                          HBSAG = PACI.HBSAG,
                                          HIV = PACI.HIV,
                                          DLC = PACI.DLC,
                                          PT = PACI.PT,
                                          PTTK = PACI.PTTK,
                                          SBil = PACI.SBil,
                                          SGOTSGPT = PACI.SGOTSGPT,
                                          ALKPOGGT = PACI.ALKPOGGT,
                                          SProtein = PACI.SProtein,
                                          UnfitReasons = PACI.UnfitReasons,
                                          Reviewedby1 = PACI.Reviewedby1,
                                          Reviewedby2 = PACI.Reviewedby2,
                                          Reviewedby3 = PACI.Reviewedby3,
                                          Reviewedon1 = PACI.Reviewedon1,
                                          Reviewedon2 = PACI.Reviewedon2,
                                          Reviewedon3 = PACI.Reviewedon3,
                                          Remarks1 = PACI.Remarks1,
                                          Remarks2 = PACI.Remarks2,
                                          Remarks3 = PACI.Remarks3,
                                          PlanGA = PACI.PlanGA,
                                          DoctorID = CMPSContext.DoctorMaster.Where(x => x.DoctorID == PACI.DoctorID).Select(x => x.Firstname + " " + x.MiddleName + " " + x.LastName).FirstOrDefault(),
                                          create = PACI.CreatedUTC

                                      }).ToList();






            return getData;

        }


    }
}
