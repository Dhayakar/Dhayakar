
using System;
using System.Collections.Generic;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Helpers;

using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace WYNK.Data.Repository.Implementation
{
    public class HelpRepository : RepositoryBase<Help>, IHelpRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
       

        public HelpRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public string ToAgeString(DateTime dob)
        {
            DateTime dt = DateTime.Today;
            int days = dt.Day - dob.Day;
            if (days < 0)
            {
                dt = dt.AddMonths(-1);
                days += DateTime.DaysInMonth(dt.Year, dt.Month);
               
            }
            int months = dt.Month - dob.Month;
            if (months < 0)
            {
                dt = dt.AddYears(-1);
                months += 12;
               

            }
            int years = dt.Year - dob.Year;
            if (years != 0 && months == 0 && days == 0)

            {
                years = dt.Year - dob.Year;
                return string.Format("{0} yr{1}", years, (years == 1) ? "" : "s");
            }
            else if (years == 0 && months != 0 && days == 0)
            {
                return string.Format("{0} month{1}", months, (months == 1) ? "" : "s");
            }
            else if (years == 0 && months == 0 && days != 0)

            {
                return string.Format("{0} day{1}", days, (days == 1) ? "" : "s");
            }
            else if (years != 0 && months != 0 && days != 0)
            {
                years = dt.Year - dob.Year;
                return string.Format("{0} yr{1}", years, (years == 1) ? "" : "s");
            }
            else if (years != 0 && months == 0 && days != 0)
            {

                years = dt.Year - dob.Year;
                return string.Format("{0} yr{1}", years, (years == 1) ? "" : "s");
            }
            else if (years != 0 && months != 0 && days == 0)
            {
                years = dt.Year - dob.Year;
                return string.Format("{0} yr{1}", years, (years == 1) ? "" : "s");
            }
            else if (years == 0 && months != 0 && days != 0)
            {
                return string.Format("{0} month{1}", months, (months == 1) ? "" : "s");
            }
            else if (years == 0 && months == 0 && days == 0)
            {
                return string.Format("{0} day{1}", days, (days == 1) ? "" : "s");
            }
            return string.Format("{0} yr{1}, {2} month{3} and {4} day{5}",
                                 years, (years == 1) ? "" : "s",
                                 months, (months == 1) ? "" : "s",
                                 days, (days == 1) ? "" : "s");
        }



        public dynamic GetAppointmentDetails(int CMPID, string Searchvalue)
        {
            var getData = new Help();

          
            var rg = WYNKContext.Registration.Where(x => x.CMPID == CMPID).ToList();
          

            getData.getAPpointmentdetailsss = (from REG in rg.Where(x => x.UIN.StartsWith(Convert.ToString(Searchvalue), StringComparison.OrdinalIgnoreCase)
      || x.Phone.StartsWith(Convert.ToString(Searchvalue)) || x.Name.StartsWith(Convert.ToString(Searchvalue), StringComparison.OrdinalIgnoreCase)
      || x.MiddleName == Searchvalue
      || x.AadharNumber == Searchvalue
      || x.LastName.StartsWith(Convert.ToString(Searchvalue), StringComparison.OrdinalIgnoreCase))
                                               select new getAPpointmentdetails
                                               {
                                                   UIN = REG.UIN,
                                                   Name = REG.Name,
                                                   DateofBirth = REG.DateofBirth,
                                                   Gender = REG.Gender,
                                                   Address1 = REG.Address1,
                                                   Address2 = REG.Address2,
                                                   Phone = REG.Phone,
                                               
                                               }).ToList();

          
            return getData;

        }


        public Help SlitLamp(int ID)
        {
            var getData = new Help();

            getData.getSlitLampDet = (from det in CMPSContext.OneLineMaster.Where(x => x.ParentTag == "SLIT" && x.IsDeleted == false && x.IsActive == true)
                                      select new getSlitLampDet
                                      {
                                          SlitID = det.OLMID,
                                          SlitDescription = det.ParentDescription,
                                          SlitIsActive = det.IsActive
                                      }).ToList();

            return getData;

        }
        public Help Fundus(int ID)
        {
            var getData = new Help();

            getData.FundusDet = (from det in CMPSContext.OneLineMaster.Where(x => x.ParentTag == "Fun" && x.IsDeleted == false && x.IsActive == true)
                                 select new FundusDet
                                 {
                                     FundusID = det.OLMID,
                                     FundusDescription = det.ParentDescription,
                                     FundusIsActive = det.IsActive
                                 }).ToList();

            return getData;
        }
        public Help getBusinessRule(int Cmpid,int ID)
        {
            var Help = new Help();
            var BussR = WYNKContext.BussinessRule.Where(x => x.CMPID == Cmpid).ToList();
            var icdspy = WYNKContext.ICDSpecialityCode.Where(x => x.IsActive == true).ToList();           
            var ModuleMaster = CMPSContext.ModuleMaster.ToList();


            Help.getBusinessRule2 = (from BRT in WYNKContext.BussinessRule.Where(x => x.CMPID == Cmpid)
                                     select new getBusinessRule2
                                    {
                                        
                                        ModuleDescription = BRT.ModuleDescription,
                                        EffectiveDate = BRT.WEF,
                                        ID = BRT.ID,
                                        icdspy=icdspy.Where(x=>x.ID== Convert.ToInt32( BRT.ICDCODE)).Select(c=>c.SpecialityDescription).FirstOrDefault(),
                                        ModuleID = ModuleMaster.Where(X => X.ModuleDescription == BRT.ModuleDescription).Select(x => x.ModuleID).FirstOrDefault(),
                                    }).ToList();



            var getBusRule = (from BRT in WYNKContext.BussinessRuleTran.Where(x => x.BRID == ID)                             
                              select new 
                                    {
           
                                        ModuleDescription = BussR.Where(x=>x.ID==ID).Select(c=>c.ModuleDescription).FirstOrDefault(),
                                        EffectiveDate = new DateTime(BRT.CreatedUTC.Year, BRT.CreatedUTC.Month, BRT.CreatedUTC.Day, BRT.CreatedUTC.Hour, BRT.CreatedUTC.Minute, 0),
                                        CreatedUTC = BRT.CreatedUTC,
                                        ID = ID,
                                        ModuleID = ModuleMaster.Where(X => X.ModuleDescription == BussR.Where(x => x.ID == ID).Select(c => c.ModuleDescription).FirstOrDefault()).Select(x => x.ModuleID).FirstOrDefault(),
                                    }).ToList();
          Help.getBusinessRule1 = (from BRT in getBusRule.GroupBy(x => x.EffectiveDate)
                                 select new getBusinessRule1
                                 {
                                    DrugID = BRT.Key,
                                    ModuleDescription = BRT.Select(v=>v.ModuleDescription).FirstOrDefault(),
                                    EffectiveDate = BRT.Select(x => x.EffectiveDate).FirstOrDefault(),
                                     CreatedUTC = BRT.Select(x => x.CreatedUTC).FirstOrDefault(),
                                     ID = BRT.Select(x => x.ID).FirstOrDefault(),
                                    ModuleID = ModuleMaster.Where(X => X.ModuleDescription == BRT.Select(x => x.ModuleDescription).FirstOrDefault()).Select(x => x.ModuleID).FirstOrDefault(),
                                 }).ToList();

            return Help;
        }


        public Help getInsurance()
        {
            var Help = new Help();
            var LocationMaster = CMPSContext.LocationMaster.ToList();
            Help.getInsurance = (from INS in WYNKContext.Insurance.Where(x => x.IsActive == false)

                                 select new getInsurance
                                 {
                                     ID = INS.ID,
                                     Name = INS.Name,
                                     Address1 = INS.Address1,
                                     Address2 = INS.Address2,
                                     Address3 = INS.Address3,
                                     LocationId = INS.LocationId,
                                     LocationName = LocationMaster.Where(X => X.ID == Convert.ToInt16(INS.LocationId)).Select(x => x.ParentDescription).FirstOrDefault(),
                                     City = Convert.ToString(LocationMaster.Where(x => x.ID == Convert.ToInt16(INS.LocationId)).Select(x => x.ParentID).FirstOrDefault()),
                                     Pincode = INS.Pincode,
                                     InsuranceCategory = Enum.GetName(typeof(InsuranceCategory), (INS.InsuranceCategory)),
                                     IsActive = INS.IsActive,
                                 }).OrderByDescending(x => x.ID).ToList();

            return Help;
        }


        public Help getTranTypeDetails(int id)
        {
            var getData = new Help();

            var Trantype = CMPSContext.TransactionType.ToList();


            getData.getTrantypeDetails = (from details in CMPSContext.TransactionType
                                          select new getTrantypeDetails
                                          {
                                              TransactionID = details.TransactionID,
                                              Description = details.Description,
                                              ContraTransactionid = details.ContraTransactionid,
                                              ContraTransaction = Trantype.Where(x => x.TransactionID == details.ContraTransactionid).Select(x => x.Description).FirstOrDefault(),
                                              RecPayTransactionid = details.RecPayContra,
                                              RecPayTransaction = Trantype.Where(x => x.TransactionID == details.RecPayContra).Select(x => x.Description).FirstOrDefault(),
                                              Rec_Issue_type = details.Rec_Issue_type,
                                          }).ToList();

            return getData;
        }

        public Help getSurgery()

        {
            var help = new Help();
            help.SurgeryDT = (from REG in WYNKContext.Registration.Where(x => x.IsDeleted == false)
                              join REGT in WYNKContext.RegistrationTran on REG.UIN equals REGT.UIN
                              join PH in WYNKContext.PatientHistory on REG.UIN equals PH.UIN
                              join PG in WYNKContext.PatientGeneral on REG.UIN equals PG.UIN
                              join Fid in WYNKContext.Findings on REG.UIN equals Fid.UIN
                              join Dia in WYNKContext.Diagnosis on Fid.RandomUniqueID equals Dia.FindingsID
                              join One in CMPSContext.OneLineMaster on Dia.DiagnosisId equals One.OLMID
                              join Find1 in WYNKContext.Findings.Where(x => x.IsSurgeryAdviced == false) on REG.UIN equals Find1.UIN
                              select new Surgerydetail
                              {
                                  UIN = REG.UIN,
                                  Name = REG.Name,
                                  DateofBirth = REG.DateofBirth,
                                  Gender = REG.Gender,
                                  Allergy = PG.Allergy,

                              }).ToList();
            return help;
        }

        public Help GetDetailsNumCol(int CmpID)
        {
            var help = new Help();
            help.NumColdetail = new List<NumCol>();

            help.NumColdetail = (from NumCol in CMPSContext.NumberControl.Where(x => x.IsDeleted == false && x.CmpID == CmpID && x.RunningNumber != 0)

                                 select new NumCol
                                 {
                                     VCID = NumCol.VCID,
                                     TransactionID = NumCol.TransactionID,
                                     DepartmentID = NumCol.DepartmentID,
                                     CmpID = NumCol.CmpID,                                     
                                     Prefix = NumCol.Prefix,
                                     Suffix = NumCol.Suffix,
                                     Description = NumCol.Description,                                     
                                     RunningNumber = NumCol.RunningNumber,
                                     EffectiveFrom = NumCol.EffectiveFrom,
                                     EffectiveTo = NumCol.EffectiveTo,
                                     Autonumber = Enum.GetName(typeof(ISactive), NumCol.Autonumber),
                                     IsActive = Enum.GetName(typeof(ISactive), NumCol.IsActive),
                                     IsDeleted = Enum.GetName(typeof(ISactive), NumCol.IsDeleted),                                     
                                 }

                ).ToList();
            return help;

        }

        public Help getSurgery1(string name, int id)

        {
            var registration = WYNKContext.Registration.ToList();
            var registrationtran = WYNKContext.RegistrationTran.ToList();
            var patienthistory = WYNKContext.PatientHistory.ToList();
            var patientgeneral = WYNKContext.PatientGeneral.ToList();
            var findings = WYNKContext.Findings.ToList();
            var diagnosis = WYNKContext.Diagnosis.ToList();
            var onelinemaster = CMPSContext.OneLineMaster.ToList();
            var Admission = WYNKContext.Admission.ToList();
            var surgery = WYNKContext.Surgery.ToList();
            var Operation = WYNKContext.OperationTheatre.ToList();
            var surgerydoc = WYNKContext.SurgeryDoctor.ToList();            
            var Doctormas = CMPSContext.DoctorMaster.ToList();
            var Surgerytran = WYNKContext.SurgeryTran.ToList();
            var icdmas = WYNKContext.ICDMaster.ToList();
            var Intraoperative = WYNKContext.IntraOperative.ToList();
            var PreOperative = WYNKContext.PreOperative.ToList();
            var FindingsExt = WYNKContext.FindingsExt.ToList();
            var Findings = WYNKContext.Findings.ToList();
            var OLM = CMPSContext.OneLineMaster.ToList();
            var help = new Help();
            help.Intraoperative = new IntraOperative();
            help.PreOperative = new PreOperative();

            help.SurgeryDT = (from REG in registration
                              join REGT in registrationtran on REG.UIN equals REGT.UIN
                              join Find in Findings on REGT.RegistrationTranID equals Find.RegistrationTranID
                              join FindExt in FindingsExt on Find.RandomUniqueID equals FindExt.FindingsID
                            
                              where REG.CMPID == id
                              select new Surgerydetail
                              {
                                  UIN = REG.UIN,
                                  Name = REG.Name,
                                  DateofBirth = REG.DateofBirth,
                                  Age = ToAgeString(REG.DateofBirth),
                                  Gender = REG.Gender,
                                  Allergy = patientgeneral.Where(x => x.RegistrationTranID == Find.RegistrationTranID).Select(x => x.Allergy).FirstOrDefault(),
                                  Regtranid = REGT.RegistrationTranID,                              

                              }).ToList();

            help.SurgeryDT1 = (from REG in registration.Where(u => u.UIN == name || u.Phone == name || u.Name == name)
                               join REGT in registrationtran on REG.UIN equals REGT.UIN
                               join Fid in findings on REGT.RegistrationTranID equals Fid.RegistrationTranID
                               join Dia in diagnosis on Fid.RandomUniqueID equals Dia.FindingsID
                               join One in onelinemaster on Dia.DiagnosisId equals One.OLMID
                               where REG.CMPID == id                               
                               select new Surgerydetail1
                               {
                                   DiagnosisId = Dia.DiagnosisId,
                                   ParentDescription = One.ParentDescription,
                                   IsOD = Dia.IsOD,
                                   IsOS = Dia.IsOS,
                                   IsOU = Dia.IsOU,

                               }).ToList();

            help.SurgeryDT2 = (from REG in registration.Where(u => u.UIN == name || u.Phone == name || u.Name == name)
                               join PH in patienthistory on REG.UIN equals PH.UIN
                               where REG.CMPID == id
                               
                               select new Surgerydetail2
                               {

                                   Description = OLM.Where(x => x.OLMID == PH.Description).Select(x => x.ParentDescription).FirstOrDefault(),
                                   FromUTC = Convert.ToDateTime(PH.FromUTC),
                                   Months = ((DateTime.Now.Year - Convert.ToDateTime(PH.FromUTC).Year) * 12) + Convert.ToDateTime(PH.FromUTC).Month - DateTime.Now.Month

                               }).ToList();


            help.SurgeryDT4 = (from REG in registration.Where(u => u.UIN == name || u.Phone == name || u.Name == name)
                               join Int in Intraoperative on REG.UIN equals Int.UIN
                               where REG.CMPID == id

                               select new Surgerydetail4
                               {
                                   UIN = Int.UIN,
                                   IODate = Int.IODate,
                                   AntibioticGivenBy = Int.AntibioticGivenBy,
                                   CaseSheetPreparedBy = Int.CaseSheetPreparedBy,
                                   Note = Int.Note,
                                   Instruction = Int.Instruction,
                               }).OrderByDescending(x => x.IODate).ToList();

            help.SurgeryDT5 = (from REG in registration.Where(u => u.UIN == name || u.Phone == name || u.Name == name)
                               join Pre in PreOperative on REG.UIN equals Pre.UIN
                               where REG.CMPID == id

                               select new Surgerydetail5
                               {
                                   UIN = Pre.UIN,
                                   PreOpDate = Pre.PreOpDate,
                                   AntibioticGivenBy = Pre.AntibioticGivenBy,
                                   CaseSheetPreparedBy = Pre.CaseSheetPreparedBy,
                                   Note = Pre.Note,
                                   Instruction = Pre.Instruction,
                               }).OrderByDescending(x => x.PreOpDate).ToList();


            return help;
        }

        public Help GetComplaints(int RegistrationTranID)
        {
            var getcomplaints = new Help();          
            var oneline = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "OcularComplaints").ToList();
            var Ocl = WYNKContext.OcularComplaints.ToList();
            var uin = WYNKContext.RegistrationTran.Where(x => x.RegistrationTranID == RegistrationTranID).Select(x => x.UIN).FirstOrDefault();

            var trandetail = WYNKContext.RegistrationTran.Where(x => x.UIN == uin).Select(x => x.RegistrationTranID).ToList();

            foreach (var item in trandetail)
            {
                getcomplaints.ComplaintsDetails = Ocl.Where(x => x.RegistrationTranID == item && x.IsDeleted == false).Select(s => new ComplaintsDetails
                {
                    ID = s.ID,
                    Description = oneline.Where(x => x.OLMID == s.Description).Select(x => x.ParentDescription).FirstOrDefault(),
                    ISOD = s.ISOD,
                    ISOU = s.ISOU,
                    ISOS = s.ISOS,
                    Fromdate = s.FromUTC,

                }).ToList();
            }


            return getcomplaints;

        }

        public Help GetComplaintsHistory(string UIN)
        {
            var getcomplaints = new Help();
            
            var oneline = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "OcularComplaints").ToList();
            var trandetail = WYNKContext.RegistrationTran.Where(x => x.UIN == UIN).Select(x => x.RegistrationTranID).ToList();
            getcomplaints.ComplaintsDetails = new List<ComplaintsDetails>();
            foreach (var item in trandetail)
            {
                var Ocl = WYNKContext.OcularComplaints.Where(x => x.RegistrationTranID == item && x.IsDeleted == false).ToList();

                foreach(var seclist in Ocl)
                {
                    var listdata = new ComplaintsDetails();

                    listdata.ID = seclist.ID;
                    listdata.Description = oneline.Where(x => x.OLMID == seclist.Description).Select(x => x.ParentDescription).FirstOrDefault();
                    listdata.ISOD = seclist.ISOD;
                    listdata.ISOU = seclist.ISOU;
                    listdata.ISOS = seclist.ISOS;
                    listdata.Fromdate = seclist.FromUTC;
                    listdata.Disabled = true;
                    listdata.Remarks = seclist.Remarks;
                    getcomplaints.ComplaintsDetails.Add(listdata);
                }
            }

            getcomplaints.cmed = WYNKContext.PatientGeneral.Where(x => x.UIN == UIN).OrderByDescending(x => x.CreatedUTC).Select(x => x.CurrentMedication).FirstOrDefault();
            getcomplaints.all = WYNKContext.PatientGeneral.Where(x => x.UIN == UIN).OrderByDescending(x => x.CreatedUTC).Select(x => x.Allergy).FirstOrDefault();
            getcomplaints.fhis = WYNKContext.PatientGeneral.Where(x => x.UIN == UIN).OrderByDescending(x => x.CreatedUTC).Select(x => x.FamilyHistory).FirstOrDefault();

            return getcomplaints;



        }


        public Help GetComplaintsHistoryWithstring(string UIN, string d)
        {
            var getcomplaints = new Help();
           
            var oneline = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "OcularComplaints").ToList();
           
            var trandetail = WYNKContext.RegistrationTran.Where(x => x.UIN == UIN).Select(x => x.RegistrationTranID).ToList();
            foreach (var item in trandetail)
            {
                var Ocl = WYNKContext.OcularComplaints.Where(x => x.RegistrationTranID == item && x.IsDeleted == false).ToList();
                getcomplaints.ComplaintsDetails = Ocl.Select(s => new ComplaintsDetails
                {
                    ID = s.ID,
                    Description = oneline.Where(x => x.OLMID == s.Description).Select(x => x.ParentDescription).FirstOrDefault(),
                    ISOD = s.ISOD,
                    ISOU = s.ISOU,
                    ISOS = s.ISOS,
                    Fromdate = s.FromUTC,
                    Remarks = s.Remarks,
                    Disabled = true,
                }).ToList();
            }


            getcomplaints.cmed = WYNKContext.PatientGeneral.Where(x => x.UIN == UIN).OrderByDescending(x => x.CreatedUTC).Select(x => x.CurrentMedication).FirstOrDefault();
            getcomplaints.all = WYNKContext.PatientGeneral.Where(x => x.UIN == UIN).OrderByDescending(x => x.CreatedUTC).Select(x => x.Allergy).FirstOrDefault();
            getcomplaints.fhis = WYNKContext.PatientGeneral.Where(x => x.UIN == UIN).OrderByDescending(x => x.CreatedUTC).Select(x => x.FamilyHistory).FirstOrDefault();

            return getcomplaints;



        }
        public Help GetPatientHistory(string UIN)
        {
            var getpatientHistory = new Help();
            DateTime now = DateTime.Now;

            var OLM = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "SystemicCondition").ToList();
            var PH = WYNKContext.PatientHistory.Where(x => x.IsActive == true && x.PatientUIN.UIN == UIN).ToList();

            getpatientHistory.PatientHistorys = PH.Select(s => new PatientHistoryDetails
            {
                ID = s.ID,
                Description = OLM.Where(x => x.OLMID == s.Description).Select(x => x.ParentDescription).FirstOrDefault(),
                FromUTC = s.FromUTC,
                Duration = MonthDifference(Convert.ToDateTime(s.FromUTC), now) + " Months",
                Disabled = true,
                Remarks = s.Remarks,

            }).ToList();

            return getpatientHistory;
        }

        public Help GetPatientHistoryWithstring(string UIN, string d)
        {
            var getpatientHistory = new Help();
            DateTime now = DateTime.Now;
            var dd = UIN + '/' + d;
            var OLM = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "SystemicCondition").ToList();
            var PH = WYNKContext.PatientHistory.Where(x => x.IsActive == true && x.PatientUIN.UIN == dd).ToList();

            getpatientHistory.PatientHistorys = PH.Select(s => new PatientHistoryDetails
            {
                ID = s.ID,
                Description = OLM.Where(x => x.OLMID == s.Description).Select(x => x.ParentDescription).FirstOrDefault(),
                FromUTC = s.FromUTC,
                Duration = MonthDifference(Convert.ToDateTime(s.FromUTC), now) + " Months",
                 Disabled = true,
                Remarks = s.Remarks,
        }).ToList();

            return getpatientHistory;
        }


        private static int MonthDifference(DateTime lValue, DateTime rValue)
        {
            return Math.Abs((lValue.Month - rValue.Month) + 12 * (lValue.Year - rValue.Year));
        }

        public Help getFinalbilling1(string name, int ID)
        {
            var PatientAccount = WYNKContext.PatientAccount.ToList();
            var registration = WYNKContext.Registration.Where(x => x.CMPID == ID).ToList();
           
            var help = new Help();
            help.FBdetail = (from REG in registration.Where(x => x.UIN.StartsWith(Convert.ToString(name), StringComparison.OrdinalIgnoreCase)
                              || x.Phone.StartsWith(Convert.ToString(name))
                              || x.Name.StartsWith(Convert.ToString(name), StringComparison.OrdinalIgnoreCase)
                              || x.MiddleName == name
                              || x.AadharNumber == name
                              || x.LastName.StartsWith(Convert.ToString(name), StringComparison.OrdinalIgnoreCase))
                             join PA in PatientAccount on REG.UIN equals PA.UIN
                             where PA.CMPID == ID && PA.InvoiceNumber == null
                             select new FBdetail
                             {
                                 UIN = REG.UIN,
                                 DateofRegistration = TimeZoneInfo.ConvertTimeFromUtc(REG.DateofRegistration, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")),
                                 Name = REG.Name,
                                 MiddleName = REG.MiddleName,
                                 LastName = REG.LastName,
                                 DateofBirth = REG.DateofBirth,
                                 Age = ToAgeString(REG.DateofBirth),
                                 Gender = REG.Gender,
                                 Address1 = REG.Address1,
                                 Phone = REG.Phone,
                                 PAddress = CMPSContext.Company.Where(x => x.CmpID == ID).Select(x => x.Address1).FirstOrDefault(),
                                 PAddress2 = CMPSContext.Company.Where(x => x.CmpID == ID).Select(x => x.Address2).FirstOrDefault(),
                                 PAddress3 = CMPSContext.Company.Where(x => x.CmpID == ID).Select(x => x.Address3).FirstOrDefault(),
                                 Pphone = CMPSContext.Company.Where(x => x.CmpID == ID).Select(x => x.Phone1).FirstOrDefault(),
                                 Pweb = CMPSContext.Company.Where(x => x.CmpID == ID).Select(x => x.Website).FirstOrDefault(),
                                 PCompnayname = CMPSContext.Company.Where(x => x.CmpID == ID).Select(x => x.CompanyName).FirstOrDefault(),
                                 PAINSID = WYNKContext.PatientVsInsurance.Where(x => x.UIN == REG.UIN && x.IsActive == false && x.IsTransacted == false && x.CmpID == ID).Select(x => x.PAINSID).FirstOrDefault(),
                                 SumAssured = WYNKContext.PatientVsInsurance.Where(x => x.UIN == REG.UIN && x.IsActive == false && x.IsTransacted == false).Select(x => x.SumAssured).FirstOrDefault(),

                             }).ToList();
            return help;
        }

        public Help getFinalbilling(string name, int ID)
        {
            var PatientAccount = WYNKContext.PatientAccount.ToList();
            var registration = WYNKContext.Registration.Where(x => x.CMPID == ID).ToList();
          
            var help = new Help();
            help.FBdetail = (from REG in registration
                             join PA in PatientAccount on REG.UIN equals PA.UIN
                             where PA.CMPID == ID && PA.InvoiceNumber == null
                             select new FBdetail
                             {
                                 UIN = REG.UIN,
                                 DateofRegistration = TimeZoneInfo.ConvertTimeFromUtc(REG.DateofRegistration, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")),
                                 Name = REG.Name,
                                 MiddleName = REG.MiddleName,
                                 LastName = REG.LastName,
                                 DateofBirth = REG.DateofBirth,
                                 Age = ToAgeString(REG.DateofBirth),
                                 Gender = REG.Gender,
                                 Address1 = REG.Address1,
                                 Phone = REG.Phone,
                                 PAddress = CMPSContext.Company.Where(x => x.CmpID == ID).Select(x => x.Address1).FirstOrDefault(),
                                 PAddress2 = CMPSContext.Company.Where(x => x.CmpID == ID).Select(x => x.Address2).FirstOrDefault(),
                                 PAddress3 = CMPSContext.Company.Where(x => x.CmpID == ID).Select(x => x.Address3).FirstOrDefault(),
                                 Pphone = CMPSContext.Company.Where(x => x.CmpID == ID).Select(x => x.Phone1).FirstOrDefault(),
                                 Pweb = CMPSContext.Company.Where(x => x.CmpID == ID).Select(x => x.Website).FirstOrDefault(),
                                 PCompnayname = CMPSContext.Company.Where(x => x.CmpID == ID).Select(x => x.CompanyName).FirstOrDefault(),
                                 PAINSID = WYNKContext.PatientVsInsurance.Where(x => x.UIN == REG.UIN && x.IsActive == false && x.IsTransacted == false && x.CmpID == ID).Select(x => x.PAINSID).FirstOrDefault(),
                                 SumAssured = WYNKContext.PatientVsInsurance.Where(x => x.UIN == REG.UIN && x.IsActive == false && x.IsTransacted == false).Select(x => x.SumAssured).FirstOrDefault(),
                             }).ToList();
            return help;
        }

        public Help getRegCode(string name, int ID)

        {

            var onelinemaster = CMPSContext.OneLineMaster.AsNoTracking().ToList();
            var registration = WYNKContext.Registration.AsNoTracking().ToList();
            var regisrtattran = WYNKContext.RegistrationTran.AsNoTracking().ToList();
            var LocationMaster = CMPSContext.LocationMaster.AsNoTracking().ToList();
            var Company = CMPSContext.Company.AsNoTracking().ToList();
            var help = new Help();
            
            int cmpid = CMPSContext.Company.AsNoTracking().Where(c => c.CmpID == ID).Select(c => c.ParentID).FirstOrDefault();
            
            var VisitType = CMPSContext.OneLineMaster.AsNoTracking().Where(c => c.ParentDescription == "New"  && c.ParentTag== "TOV").Select(c => c.OLMID).FirstOrDefault();
            var VisitTypeReview = CMPSContext.OneLineMaster.AsNoTracking().Where(c => c.ParentDescription == "Review" && c.ParentTag == "TOV").Select(c => c.OLMID).FirstOrDefault();
          
            if (cmpid == 0)
            {
                cmpid = ID;
            }
          
            help.PatientAssignStatus = new List<PatientAssignStatus>();
            help.Regdetail = (from REG in registration.Where(x => x.UIN.StartsWith(Convert.ToString(name), StringComparison.OrdinalIgnoreCase)
                              || x.OLDUIN.StartsWith(Convert.ToString(name), StringComparison.OrdinalIgnoreCase)
                              || x.Phone.StartsWith(Convert.ToString(name)) 
                              || x.Name.StartsWith(Convert.ToString(name), StringComparison.OrdinalIgnoreCase)
                              || x.MiddleName == name
                              || x.AadharNumber == name
                              || x.LastName.StartsWith(Convert.ToString(name), StringComparison.OrdinalIgnoreCase))
                              join REGT in regisrtattran on REG.UIN equals REGT.UIN
                              join cmp in Company.Where(x => x.ParentID == cmpid || x.CmpID == cmpid) on REG.CMPID equals cmp.CmpID
                              where REGT.PatientVisitType == Convert.ToString(VisitType) && REG.IsDeleted == false
                              select new Regdetail
                              {
                                  UIN = REG.UIN,
                                  OLDUIN = REG.OLDUIN,
                                  DateofRegistration = TimeZoneInfo.ConvertTimeFromUtc(REG.DateofRegistration, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")),
                                  Name = REG.Name,
                                  DateofBirth = REG.DateofBirth,
                                  Age = ToAgeString(REG.DateofBirth),
                                  Gender = REG.Gender,
                                  Address1 = REG.Address1,
                                  Address2 = REG.Address2,
                                  Address3 = REG.Address3,
                                  LocationName = LocationMaster.Where(X => X.ID == Convert.ToInt16(REG.LocationID)).Select(x => x.ParentDescription).FirstOrDefault(),
                                  City = Convert.ToString(LocationMaster.Where(x => x.ID == Convert.ToInt16(REG.LocationID)).Select(x => x.ParentID).FirstOrDefault()),
                                  FatherHusbandName = REG.FatherHusbandName,
                                  EmailID = REG.EmailID,
                                  Phone = REG.Phone,
                                  AadharNumber = REG.AadharNumber,
                                  PanCardNo = REG.PanCardNo,
                                  PassportNo = REG.PassportNo,
                                  DrivingLicenseNo = REG.DrivingLicenseNo,
                                  IsForeignNational = Convert.ToString(REG.IsForeignNational),
                                  Occupation = REG.Occupation,
                                  SourceofReferralID = onelinemaster.Where(x => x.OLMID == REG.SourceofReferralID).Select(x => x.ParentDescription).FirstOrDefault(),
                                  ReferralPhone = REG.ReferralName,
                                  DateofVisit = regisrtattran.Where(a => a.PatientVisitType == Convert.ToString(VisitTypeReview) && a.UIN == name).Select(x => x.DateofVisit).LastOrDefault(), //REGT.DateofVisit,
                                  TypeofVisit = onelinemaster.Where(x => x.OLMID == REGT.TypeofVisit).Select(x => x.ParentDescription).FirstOrDefault(),
                                  Status = onelinemaster.Where(x => x.OLMID == REGT.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                                  Remarks = REGT.Remarks,
                                  MiddleName = REG.MiddleName,
                                  LastName = REG.LastName,
                                  AlternateMailID = REG.AlternateMailID,
                                  AlternatePhoneNumber = REG.AlternatePhoneNumber,
                                  MaritalStatus = onelinemaster.Where(x => x.OLMID == REG.MaritalStatus).Select(x => x.ParentDescription).FirstOrDefault(),
                                  ConsulationFees = REGT.RegistrationFees,
                                  PreferredLanguage = REG.PreferredLanguage,
                                  CmpAddress = cmp.Address1,
                                  DateofReg = REG.DateofRegistration,
                                  PAddress = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address1).FirstOrDefault(),
                                  PAddress2 = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address2).FirstOrDefault(),
                                  PAddress3 = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address3).FirstOrDefault(),
                                  Pphone = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Phone1).FirstOrDefault(),
                                  Pweb = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Website).FirstOrDefault(),
                                  PCompnayname = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.CompanyName).FirstOrDefault(),
                                  RegisteredBy = CMPSContext.Users.Where(x => x.Userid == REGT.CreatedBy).Select(x => x.Username).FirstOrDefault(),
                                  ReceiptNumber = WYNKContext.PaymentMaster.Where(x => x.UIN == REG.UIN && x.CmpID == cmpid).Select(x => x.ReceiptNumber).FirstOrDefault(),
                                  ReceiptDate = WYNKContext.PaymentMaster.Where(x => x.UIN == REG.UIN && x.CmpID == cmpid).Select(x => x.ReceiptDate).FirstOrDefault(),

                              }).OrderByDescending(x => x.DateofRegistration).ToList();

            return help;
        }



        public Help getCmpRegCode(string name, int ID)

        {

            var onelinemaster = CMPSContext.OneLineMaster.AsNoTracking().ToList();
            var Cmpregistration = WYNKContext.CampRegistration.Where(x => x.CMPID == ID).AsNoTracking().ToList();
            var Campregisrtationtran = WYNKContext.CampRegistrationTran.Where(x => x.CmpID == ID).AsNoTracking().ToList();
            var LocationMaster = CMPSContext.LocationMaster.AsNoTracking().ToList();
            var Company = CMPSContext.Company.AsNoTracking().ToList();
            var CAMP = WYNKContext.CAMP.AsNoTracking().ToList();
            var ICDSpeciality = WYNKContext.ICDSpecialityCode.AsNoTracking().ToList();
            var help = new Help();
            int cmpid = CMPSContext.Company.AsNoTracking().Where(c => c.CmpID == ID).Select(c => c.ParentID).FirstOrDefault();
            var VisitType = CMPSContext.OneLineMaster.AsNoTracking().Where(c => c.ParentDescription == "New" && c.ParentTag == "TOV").Select(c => c.OLMID).FirstOrDefault();
            var VisitTypeReview = CMPSContext.OneLineMaster.AsNoTracking().Where(c => c.ParentDescription == "Review" && c.ParentTag == "TOV").Select(c => c.OLMID).FirstOrDefault();
            if (cmpid == 0)
            {
                cmpid = ID;
            }
            
            help.Regdetail = (from REG in Cmpregistration.Where(x => x.CampUIN.StartsWith(Convert.ToString(name), StringComparison.OrdinalIgnoreCase)
                              || x.Phone.StartsWith(Convert.ToString(name))
                              || x.Name.StartsWith(Convert.ToString(name), StringComparison.OrdinalIgnoreCase)
                              || x.MiddleName == name
                              || x.LastName.StartsWith(Convert.ToString(name), StringComparison.OrdinalIgnoreCase))
                              join REGT in Campregisrtationtran on REG.CampUIN equals REGT.CampUIN
                              join cmp in Company.Where(x => x.ParentID == cmpid || x.CmpID == cmpid) on REG.CMPID equals cmp.CmpID
                              where REGT.PatientVisitType == Convert.ToString(VisitType) && REG.IsDeleted == false && REG.IsVisited == false
                              select new Regdetail
                              {
                                  UIN = REG.CampUIN,
                                  RegType= REG.RegType,
                                  DateofRegistration = TimeZoneInfo.ConvertTimeFromUtc(REG.DateofRegistration, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")),
                                  Name = REG.Name,
                                  DateofBirth = REG.DateofBirth,
                                  Age = ToAgeString(REG.DateofBirth),
                                  Gender = REG.Gender,
                                  Address1 = REG.Address1,
                                  Address2 = REG.Address2,
                                  Address3 = REG.Address3,
                                  LocationName = LocationMaster.Where(X => X.ID == Convert.ToInt16(REG.LocationID)).Select(x => x.ParentDescription).FirstOrDefault(),
                                  City = Convert.ToString(LocationMaster.Where(x => x.ID == Convert.ToInt16(REG.LocationID)).Select(x => x.ParentID).FirstOrDefault()),
                                  FatherHusbandName = REG.FatherHusbandName,
                                  EmailID = REG.EmailID,
                                  Phone = REG.Phone,
                                  AadharNumber = REG.AadharNumber,
                                  PanCardNo = REG.PanCardNo,
                                  PassportNo = REG.PassportNo,
                                  DrivingLicenseNo = REG.DrivingLicenseNo,
                                  IsForeignNational = Convert.ToString(REG.IsForeignNational),
                                  Occupation = REG.Occupation,
                                  SourceofReferralID = onelinemaster.Where(x => x.OLMID == REG.SourceofReferralID).Select(x => x.ParentDescription).FirstOrDefault(),
                                  ReferralPhone = REG.ReferralName,
                                  DateofVisit = Campregisrtationtran.Where(a => a.PatientVisitType == Convert.ToString(VisitTypeReview) && a.CampUIN == name).Select(x => x.DateofVisit).LastOrDefault(), //REGT.DateofVisit,
                                  TypeofVisit = onelinemaster.Where(x => x.OLMID == REGT.TypeofVisit).Select(x => x.ParentDescription).FirstOrDefault(),
                                  Status = onelinemaster.Where(x => x.OLMID == REGT.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                                  Remarks = REGT.Remarks,
                                  MiddleName = REG.MiddleName,
                                  LastName = REG.LastName,
                                  AlternateMailID = REG.AlternateMailID,
                                  TreatmentAdvice = REG.TreatmentAdvice,
                                  AlternatePhoneNumber = REG.AlternatePhoneNumber,
                                  CampName = CAMP.Where(x => x.CampID == REG.CampID).Select(c => c.CampName).FirstOrDefault(),
                                  SurgeryName = ICDSpeciality.Where(x => x.ID == REG.ICDSpecialityId).Select(c => c.SpecialityDescription).FirstOrDefault(),
                                  MaritalStatus = onelinemaster.Where(x => x.OLMID == REG.MaritalStatus).Select(x => x.ParentDescription).FirstOrDefault(),
  
                              }).OrderByDescending(x => x.DateofRegistration).ToList();

            return help;
        }

        public Help getAppRegCode(string name, int ID)

        {
            var help = new Help();
            var Appointment = WYNKContext.Appointment.Where(x => x.CMPID == ID && x.IsVisited == false).AsNoTracking().ToList();
          var AppointmentTrans = WYNKContext.AppointmentTrans.Where(x => x.CMPID == ID).AsNoTracking().ToList();

            help.Appdetail = (from app in Appointment
                              join apptrn in AppointmentTrans on app.RandomUniqueID equals apptrn.AppointmentMasterID
                              select new Appdetail
                              {
                                  RandomUniqueID = app.RandomUniqueID,
                                  PatientName = app.PatientName,
                                  MiddleName = app.MiddleName,
                                  LastName = app.LastName,
                                  Dateofbirth = app.Dateofbirth,
                                  Age = ToAgeString(app.Dateofbirth),
                                  UIN = app.UIN,
                                  Gender = app.Gender,
                                  Address1 = app.Address1,
                                  Address2 = app.Address2,
                                  Phone = app.Phone,
                                  ConsulationFees = app.ConsulationFees,
                              }).OrderByDescending(x => x.CreatedUTC).ToList();

            return help;
        }
        public Help getAdvpay(string name, int ID)

        {
            var registration = WYNKContext.Registration.Where(x => x.CMPID == ID).ToList();           
            var PatientAccount = WYNKContext.PatientAccount.ToList();
            var help = new Help();
            help.Advpaydetail = (from REG in registration.Where(x => x.UIN.StartsWith(Convert.ToString(name), StringComparison.OrdinalIgnoreCase)
                              || x.Phone.StartsWith(Convert.ToString(name)) || x.Name.StartsWith(Convert.ToString(name), StringComparison.OrdinalIgnoreCase)                              
                              || x.MiddleName == name
                              || x.AadharNumber == name                              
                              || x.LastName.StartsWith(Convert.ToString(name), StringComparison.OrdinalIgnoreCase))
                                 join PA in PatientAccount on REG.UIN equals PA.UIN
                                 where PA.CMPID == ID && PA.InvoiceNumber == null

                                 select new Advpaydetail
                                 {
                                     UIN = REG.UIN,
                                     Name = REG.Name,
                                     MiddleName = REG.MiddleName,
                                     LastName = REG.LastName,
                                     Age = ToAgeString(REG.DateofBirth),
                                     Gender = REG.Gender,
                                 }).ToList();
            return help;
        }





        public Help getDashhelpCode1()

        {
            var registration = WYNKContext.Registration.ToList();
            var registrationtran = WYNKContext.RegistrationTran.ToList();
            var onelinemaster = CMPSContext.OneLineMaster.ToList();

            var help = new Help();
            help.Regdetail = (from REG in registration
                              join REGT in registrationtran on REG.UIN equals REGT.UIN
                              select new Regdetail
                              {
                                  UIN = REG.UIN,
                                  DateofRegistration = TimeZoneInfo.ConvertTimeFromUtc(REG.DateofRegistration, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")),
                                  Name = REG.Name,
                                  DateofBirth = REG.DateofBirth,
                                  Age = ToAgeString(REG.DateofBirth),
                                  Gender = REG.Gender,
                                  Address1 = REG.Address1,
                                  Address2 = REG.Address2,
                                  Address3 = REG.Address3,
                                  FatherHusbandName = REG.FatherHusbandName,
                                  EmailID = REG.EmailID,
                                  Phone = REG.Phone,
                                  AadharNumber = REG.AadharNumber,
                                  Occupation = REG.Occupation,
                                  SourceofReferralID = onelinemaster.Where(x => x.OLMID == REG.SourceofReferralID).Select(x => x.ParentDescription).FirstOrDefault(),
                                  ReferralPhone = REG.ReferralName,
                                  DateofVisit = REGT.DateofVisit,
                                  TypeofVisit = onelinemaster.Where(x => x.OLMID == REGT.TypeofVisit).Select(x => x.ParentDescription).FirstOrDefault(),
                                  Status = onelinemaster.Where(x => x.OLMID == REGT.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                                  Remarks = REGT.Remarks,
                              }).ToList();
            return help;
        }



        public Help getRegCode1()

        {
            var registration = WYNKContext.Registration.ToList();
            var registrationtran = WYNKContext.RegistrationTran.ToList();
            var onelinemaster = CMPSContext.OneLineMaster.ToList();
            var VisitType = CMPSContext.OneLineMaster.AsNoTracking().Where(c => c.ParentDescription == "New" && c.ParentTag == "TOV").Select(c => c.OLMID).FirstOrDefault();
          
            var help = new Help();
            help.Regdetail = (from REG in registration.Where(x => x.IsDeleted == false)
                              join REGT in registrationtran on REG.UIN equals REGT.UIN
                              where REGT.PatientVisitType == Convert.ToString(VisitType)

                              select new Regdetail
                              {
                                  UIN = REG.UIN,
                                  DateofRegistration = TimeZoneInfo.ConvertTimeFromUtc(REG.DateofRegistration, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")),
                                  Name = REG.Name,
                                  DateofBirth = REG.DateofBirth,
                                  Age = ToAgeString(REG.DateofBirth),
                                  Gender = REG.Gender,
                                  Address1 = REG.Address1,
                                  Address2 = REG.Address2,
                                  Address3 = REG.Address3,                                  
                                  EmailID = REG.EmailID,
                                  Phone = REG.Phone,                                  
                                  Occupation = REG.Occupation,
                                  SourceofReferralID = onelinemaster.Where(x => x.OLMID == REG.SourceofReferralID).Select(x => x.ParentDescription).FirstOrDefault(),
                                  ReferralPhone = REG.ReferralName,
                                  DateofVisit = REGT.DateofVisit,
                                  TypeofVisit = onelinemaster.Where(x => x.OLMID == REGT.TypeofVisit).Select(x => x.ParentDescription).FirstOrDefault(),
                                  Status = onelinemaster.Where(x => x.OLMID == REGT.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                                  Remarks = REGT.Remarks,

                              }).ToList();
            return help;
        }

        public Help getSurgeryCode1()

        {
            var help = new Help();
            help.Surgerydetail = (from REG in WYNKContext.Registration.Where(x => x.IsDeleted == false)
                                  join REGT in WYNKContext.RegistrationTran on REG.UIN equals REGT.UIN
                                  join one in WYNKContext.Findings.Where(x => x.IsSurgeryAdviced == false) on REG.UIN equals one.UIN
                                  select new Surgerydetails
                                  {
                                      UIN = REG.UIN,
                                      Name = REG.Name,
                                      DateofBirth = REG.DateofBirth,
                                      Gender = REG.Gender,
                                      Address1 = REG.Address1,
                                      Address2 = REG.Address2,
                                      Address3 = REG.Address3,

                                  }).ToList();
            return help;
        }

        public Help getSurgeryCode(string name)

        {
            var help = new Help();
            help.Surgerydetail = (from REG in WYNKContext.Registration.Where(u => u.UIN == name)
                                  join REGT in WYNKContext.RegistrationTran on REG.UIN equals REGT.UIN
                                  join one in WYNKContext.Findings.Where(x => x.IsSurgeryAdviced == false) on REG.UIN equals one.UIN
                                  select new Surgerydetails
                                  {
                                      UIN = REG.UIN,
                                      Name = REG.Name,
                                      DateofBirth = REG.DateofBirth,
                                      Gender = REG.Gender,
                                      Address1 = REG.Address1,
                                      Address2 = REG.Address2,
                                      Address3 = REG.Address3,

                                  }).ToList();
            return help;
        }

        public Help getCode1(int ICDGROUPCODE)
        {
            var help = new Help();

            var icdmaster = WYNKContext.ICDMaster.ToList();
            var onelinemaster = CMPSContext.OneLineMaster.ToList();
            var surgerycost = WYNKContext.SurgeryCostDetail.ToList();
            var ICDSpecialityCode = WYNKContext.ICDSpecialityCode.ToList();
            var room = WYNKContext.Room.ToList();

            help.IcdDetail = (from ICD in icdmaster.Where(x => x.IsDeleted == false && x.ICDGroup == ICDGROUPCODE)
                              join SC in surgerycost on ICD.ICDCODE equals SC.ICDCode

                              select new IcdDetail
                              {
                                  Code = ICD.ICDCODE,
                                  description = ICD.ICDDESCRIPTION,
                                  speciality = ICDSpecialityCode.Where(x => x.ID == Convert.ToInt32(ICD.SpecialityCode)).Select(x => x.SpecialityDescription).FirstOrDefault(),
                                  icdgroup = onelinemaster.Where(x => x.OLMID == Convert.ToInt32(ICD.ICDGroup)).Select(x => x.ParentDescription).FirstOrDefault(),
                                  Status = ICD.IsActive == true ? "Active" : "InActive",
                                  IsIOLReqd = ICD.IsIOLReqd,                                  
                                  roomtype = SC.RoomType != "" ? room.Where(x => x.ID == Convert.ToInt32(SC.RoomType)).Select(x => x.RoomDescription).FirstOrDefault() : string.Empty,
                              }
                             ).ToList();

            return help;
        }

        public Help getUserDoc()
        {
            var help = new Help();
            var us = (from o in CMPSContext.Users select o.ReferenceID).ToList();
            help.UserDoc = (from c in CMPSContext.DoctorMaster
                            where !us.Contains(c.DoctorID)
                            select new UserDoc
                            {
                                DoctorID = c.DoctorID,
                                DoctorName = c.LastName,
                                RegistrationNumber = c.RegistrationNumber,
                            }
              ).ToList();
            return help;
        }

        public Help getUserEmp()
        {
            var help = new Help();
            var us = (from o in CMPSContext.Users select o.ReferenceID).ToList();
            help.UserEmp = (from e in CMPSContext.Employee
                            where !us.Contains(e.EmployeeID)
                            select new UserEmp
                            {
                                EmployeeID = e.EmployeeID,
                                EmployeeName = e.FirstName,
                                DOJ = Convert.ToString(e.DOJ),
                            }

            ).ToList();


            return help;
        }

        public Help deleteDoc()
        {
            var help = new Help();

            help.docdetail1 = (from doc in CMPSContext.DoctorMaster.Where(x => x.IsDeleted == false)                                   

                               select new docdetail1
                               {                                   
                                   DoctorID = doc.DoctorID,
                                   DoctorName = doc.LastName,

                                   Address1 = doc.Address1,

                                   Address2 = doc.Address2,
                                   Address3 = doc.Address3,

                                   LocationID = CMPSContext.OneLineMaster.Where(x => x.OLMID == Convert.ToInt64(doc.LocationID)).Select(x => x.ParentDescription).FirstOrDefault(),
                                   Designation = doc.Designation,
                                   RegistrationNumber = doc.RegistrationNumber,
                                   EngagementType = CMPSContext.OneLineMaster.Where(x => x.OLMID == Convert.ToInt64(doc.EngagementType)).Select(x => x.ParentDescription).FirstOrDefault(),
                                   Phone1 = doc.Phone1,
                                   Phone2 = doc.Phone2,
                                   EmailID = doc.EmailID,

                               }


                ).ToList();


            return help;
        }

        public string Getlocationname(string Locname)
        {
            var Data = "";

            if (Locname != null)
            {
                Data = CMPSContext.LocationMaster.Where(x => x.ID == Convert.ToInt32(Locname)).Select(x => x.ParentDescription).FirstOrDefault();
            }
            else
            {
                Data = null;
            }

            return Data;

        }


        public Help getDoc(int Name)
        {
            var help = new Help();        
            var LocationMaster = CMPSContext.LocationMaster.ToList();

            help.docdetail = (from doc in CMPSContext.DoctorMaster.Where(x => x.CMPID == Name && x.IsDeleted == false)
                              join ds in CMPSContext.DoctorSpeciality on doc.DoctorID equals ds.DoctorID
                              join om in CMPSContext.OneLineMaster on ds.OLMID equals om.OLMID
                              select new docdetail
                              {

                                  DoctorID = doc.DoctorID,
                                  DoctorName = String.Concat(doc.Firstname + " " + (doc.MiddleName != null ? doc.MiddleName : " ") + " " + doc.LastName),
                                  FirstName = doc.Firstname,
                                  MiddleName = doc.MiddleName,
                                  LastName = doc.LastName,
                                  Title = doc.Title,
                                  DateofBirth = doc.DateofBirth,
                                  Gender = doc.Gender,
                                  LocationID = doc.LocationID != null ? doc.LocationID.ToString() : null,                                  
                                  City = doc.LocationID != null ? Convert.ToString(LocationMaster.Where(x => x.ID == Convert.ToInt16(doc.LocationID)).Select(x => x.ParentID).FirstOrDefault()) : null,
                                
                                  Address1 = doc.Address1,
                                  Address2 = doc.Address2,
                                  Address3 = doc.Address3,
                                  Designation = doc.Designation,
                                  RegistrationNumber = doc.RegistrationNumber,
                                  EngagementType = CMPSContext.OneLineMaster.Where(x => x.OLMID == Convert.ToInt64(doc.EngagementType)).Select(x => x.ParentDescription).FirstOrDefault(),
                                  Phone1 = doc.Phone1,
                                  Phone2 = doc.Phone2,
                                  EmailID = doc.EmailID,
                                  Speciality = CMPSContext.OneLineMaster.Where(x => x.OLMID == ds.OLMID).Select(x => x.ParentDescription).FirstOrDefault(),
                                  DoctorSpecialityID = CMPSContext.DoctorSpeciality.Where(x => x.OLMID == ds.OLMID && x.DoctorID == doc.DoctorID).Select(x => x.DoctorSpecialityID).FirstOrDefault(),
                                  Status = doc.IsActive,
                              }

                ).ToList();




            return help;

        }


        public Help getDoc1(string name)
        {
            var help = new Help();

            help.docdetail = (from doc in CMPSContext.DoctorMaster.Where(u => u.LastName == name && u.IsDeleted == false)
                                
                              join ds in CMPSContext.DoctorSpeciality.Where(x => x.IsActive == true) on doc.DoctorID equals ds.DoctorID
                              join om in CMPSContext.OneLineMaster on ds.OLMID equals om.OLMID

                              select new docdetail
                              {
                                
                                  DoctorID = doc.DoctorID,
                                  DoctorName = doc.LastName,
                                  FirstName = doc.Firstname,
                                  MiddleName = doc.MiddleName,
                                  Title = doc.Title,
                                  DateofBirth = doc.DateofBirth,
                                  Gender = doc.Gender,
                                  Address1 = doc.Address1,
                                  Address2 = doc.Address2,
                                  Address3 = doc.Address3,
                                  Designation = doc.Designation,
                                  LocationID = CMPSContext.OneLineMaster.Where(x => x.OLMID == Convert.ToInt64(doc.LocationID)).Select(x => x.ParentDescription).FirstOrDefault(),

                                  RegistrationNumber = doc.RegistrationNumber,
                                  EngagementType = CMPSContext.OneLineMaster.Where(x => x.OLMID == Convert.ToInt64(doc.EngagementType)).Select(x => x.ParentDescription).FirstOrDefault(),
                                  Phone1 = doc.Phone1,
                                  Phone2 = doc.Phone2,
                                  EmailID = doc.EmailID,
                                  Speciality = CMPSContext.OneLineMaster.Where(x => x.OLMID == ds.OLMID).Select(x => x.ParentDescription).FirstOrDefault(),
                                  DoctorSpecialityID = CMPSContext.DoctorSpeciality.Where(x => x.OLMID == ds.OLMID).Select(x => x.DoctorSpecialityID).FirstOrDefault(),
                              }





                ).ToList();


            return help;

        }

        public Help getEmployeeAll(int name)
        {
            var help = new Help();
            var users = CMPSContext.Users.ToList();
            var usersvsrole = CMPSContext.UserVsRole.ToList();
           
            var empcommun = CMPSContext.EmployeeCommunication.ToList();
            help.EmployeeDetail = (from emp in CMPSContext.Employee.Where(u => u.IsDeleted == false && u.CMPID == name)
                                   join empc in empcommun.Where(x => x.IsDeleted == false) on emp.EmployeeID equals empc.EmpID
                                   select new EmployeeDetail
                                   {

                                       EmployeeID = emp.EmployeeID,
                                       Title = emp.Title,
                                       FirstName = emp.FirstName,
                                       MiddleName = emp.MiddleName,
                                       LastName = emp.LastName,
                                       Dateofbirth = emp.Dateofbirth,
                                       Gender = emp.Gender.ToString(),
                                       PhysicallyChallenged = emp.PhysicallyChallenged,
                                       MaritalStatus = emp.MaritalStatus,
                                       Category = emp.Category,
                                       DeptCode = CMPSContext.Department.Where(x => x.ID == emp.DeptCode).Select(x => x.Description).FirstOrDefault(),
                                       AadhaarNo = emp.AadhaarNo,
                                       PANNo = emp.PANNo,
                                       FatherHusbandName = emp.FatherHusbandName,
                                       ReasonForResignation = emp.ReasonForResignation,
                                       DOJ = emp.DOJ,
                                       DOR = emp.DOR,
                                       Designation = emp.Designation,
                                       EmergencyContactName = emp.EmergencyContactName,
                                       EmergencyContactNo = emp.EmergencyContactNo,
                                       BloodGroup = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == emp.BloodGroup).Select(x => x.ParentDescription).FirstOrDefault(),
                                       Status = emp.IsActive,
                                       EmproleID = usersvsrole.Where(x => x.UserID == users.Where(y => y.ReferenceID == emp.EmployeeID).Select(y => y.Userid).FirstOrDefault()).Select(x => x.RoleID).FirstOrDefault(),
                                       PresentAddress1 = empc.PresentAddress1,
                                       PresentAddress2 = empc.PresentAddress2,
                                       PresentLocationID = CMPSContext.LocationMaster.Where(x => x.ID == empc.PresentLocationID).Select(x => x.ParentDescription).FirstOrDefault(),
                                       PresentCity = Convert.ToString(CMPSContext.LocationMaster.Where(x => x.ID == empc.PresentLocationID).Select(x => x.ParentID).FirstOrDefault()),
                                       PresentLandmark = empc.PresentLandmark,
                                       PermanentAddress1 = empc.PermanentAddress1,
                                       PermanentAddress2 = empc.PermanentAddress2,
                                       PermanentLocationID = CMPSContext.LocationMaster.Where(x => x.ID == empc.PermanentLocationID).Select(x => x.ParentDescription).FirstOrDefault(),
                                       PermanentCity = Convert.ToString(CMPSContext.LocationMaster.Where(x => x.ID == empc.PermanentLocationID).Select(x => x.ParentID).FirstOrDefault()),
                                       PermanentLandmark = empc.PermanentLandmark,
                                       Phone = empc.Phone,
                                       MobileNumber1 = empc.MobileNumber1,
                                       MobileNumber2 = empc.MobileNumber2,
                                       EmailID = empc.EmailID,
                                       AlternateEmailID = empc.AlternateEmailID,

                                   }

                   ).ToList();




            return help;
        }
        public Help getEmployee(int empid)
        {
            var help = new Help();

            return help;
        }

        public Help getDrug(int Cmpid)
        {
            var help = new Help();

            var onelineMaster = CMPSContext.OneLineMaster.ToList();
            var TaxMaster = CMPSContext.TaxMaster.ToList();


            help.DrugDetail = (from drug in WYNKContext.DrugMaster.Where(u => u.IsDeleted == false && u.Cmpid == Cmpid)
                               select new DrugDetail
                               {
                                   ID = drug.ID,
                                   DrugGroup = drug.DrugGroup,
                                   Brand = drug.Brand,
                                   Manufacturer = onelineMaster.Where(x => x.OLMID == Convert.ToInt32(drug.Manufacturer)).Select(x => x.ParentDescription).FirstOrDefault(),
                                   UOM = drug.UOM,
                                   MedicineName = WYNKContext.DrugGroup.Where(x => x.ID == drug.GenericName).Select(x => x.Description).FirstOrDefault(),
                                   SideEffects = WYNKContext.DrugGroup.Where(x => x.ID == drug.GenericName).Select(x => x.SideEffects).FirstOrDefault(),
                                   Precautionss = WYNKContext.DrugGroup.Where(x => x.ID == drug.GenericName).Select(x => x.Precautions).FirstOrDefault(),
                                   Overdose = WYNKContext.DrugGroup.Where(x => x.ID == drug.GenericName).Select(x => x.Overdose).FirstOrDefault(),
                                   Rate = drug.Rate,
                                   HSNNo = drug.HSNNo,
                                   TaxID = drug.TaxID,
                                   GST = TaxMaster.Where(x => x.ID == drug.TaxID).Select(x => x.GSTPercentage).FirstOrDefault(),
                                   CGST = TaxMaster.Where(x => x.ID == drug.TaxID).Select(x => x.CGSTPercentage).FirstOrDefault(),
                                   SGST = TaxMaster.Where(x => x.ID == drug.TaxID).Select(x => x.SGSTPercentage).FirstOrDefault(),
                                   CESSPercentage = TaxMaster.Where(x => x.ID == drug.TaxID).Select(x => x.CESSPercentage).FirstOrDefault(),
                                   AdditionalCESSPercentage = TaxMaster.Where(x => x.ID == drug.TaxID).Select(x => x.AdditionalCESSPercentage).FirstOrDefault(),
                                   Status = drug.IsActive == true ? "Active" : "Inactive",
                                   DrugCategory = Enum.GetName(typeof(DrugCategory), drug.DrugCategory),
                                   TrackingType = Enum.GetName(typeof(TrackingType), drug.DrugTracker),
                                   Power = drug.Power,
                                   Aconstant = drug.Aconstant,
                                   ModelNo = drug.ModelNo,
                                   OpticDia = drug.OpticDia,
                                   Length = drug.Length,
                                   DrugComposition = drug.DrugComposition,
                                   DrugSubDescription = drug.DrugSubDescription
                               }).ToList();
            return help;
        }


        public Help getDrugGroupDesc(int Id)
        {

            var help = new Help();

            var onelinemaster = CMPSContext.OneLineMaster.ToList();
            var dg = WYNKContext.DrugGroup.ToList();

            var drugGroup = WYNKContext.DrugMaster.Where(u => u.ID == Id).Select(u => u.GenericName).FirstOrDefault();

            help.DrugGroupDetail = (from drug in WYNKContext.DrugMaster.Where(u => u.GenericName == drugGroup)

                                    select new DrugGroupDetail
                                    {
                                        Brand = drug.Brand,
                                        Manufacturer = onelinemaster.Where(x => x.OLMID == drug.Manufacturer).Select(x => x.ParentDescription).FirstOrDefault(),
                                        MedicineName = dg.Where(x => x.ID == drug.GenericName).Select(x => x.Description).FirstOrDefault(),
                                    }

                                  ).ToList();


            return help;

        }



        public Help getRevReg(string UIN)

        {
            var registration = WYNKContext.Registration.ToList();
            var registrationtran = WYNKContext.RegistrationTran.ToList();
            var onelinemaster = CMPSContext.OneLineMaster.ToList();
       
            var VisitTypeReview = CMPSContext.OneLineMaster.AsNoTracking().Where(c => c.ParentDescription == "Review" && c.ParentTag == "TOV").Select(c => c.OLMID).FirstOrDefault();

            var help = new Help();
            help.Regdetail = (from REG in registration.Where(x => x.IsDeleted == false && x.UIN == UIN)
                              join REGT in registrationtran on REG.UIN equals REGT.UIN
                              where REGT.PatientVisitType == Convert.ToString(VisitTypeReview)
                              select new Regdetail
                              {
                                  UIN = REG.UIN,
                                  Name = REG.Name,
                                  Gender = REG.Gender,
                                  Phone = REG.Phone,
                                  DateofBirth = REG.DateofBirth,
                                  DateofVisit = TimeZoneInfo.ConvertTimeFromUtc(REGT.DateofVisit, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")),
                                  TypeofVisit = onelinemaster.Where(x => x.OLMID == REGT.TypeofVisit).Select(x => x.ParentDescription).FirstOrDefault(),
                                  Status = onelinemaster.Where(x => x.OLMID == REGT.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                                  Remarks = REGT.Remarks,
                                  Amount = REGT.ConsulationFees,
                              }).ToList();
            return help;
        }

        public Help getVendor(int name)
        {
            var help = new Help();
            help.Vendordetail = new List<VendorMas>();

            help.Vendordetail = (from VenMas in CMPSContext.VendorMaster.Where(x => x.IsDeleted == false && x.CmpID == name)


                                 select new VendorMas
                                 {
                                     ID = VenMas.ID,
                                     VendorCode = VenMas.VendorCode,
                                     VendorCategory = Enum.GetName(typeof(Vendor), VenMas.VendorCategory),
                                     Name = VenMas.Name,
                                     Address1 = VenMas.Address1,
                                     Address2 = VenMas.Address2,
                                     Address3 = VenMas.Address3,
                                    
                                     LocationID = CMPSContext.LocationMaster.Where(X => X.ID == Convert.ToInt16(VenMas.Location)).Select(x => x.ParentDescription).FirstOrDefault(),
                                     City = Convert.ToString(CMPSContext.LocationMaster.Where(x => x.ID == Convert.ToInt16(VenMas.Location)).Select(x => x.ParentID).FirstOrDefault()),
                                     DLNo = VenMas.DLNo,
                                     DLNo1 = VenMas.DLNo1,
                                     DLNoDate = VenMas.DLNoDate,
                                     DLNoDate1 = VenMas.DLNoDate1,
                                     Landmark = VenMas.Landmark,
                                     FSSAINo = VenMas.FSSAINo,
                                     GSTNo = VenMas.GSTNo,
                                     MobileNo = VenMas.MobileNo,
                                     PhoneNo = VenMas.PhoneNo,
                                     ContactPerson = VenMas.ContactPerson,
                                     CreditDays = VenMas.CreditDays,
                                     Creditlimit = VenMas.Creditlimit,
                                     Leadtime = VenMas.Leadtime,
                                     IsActive = Enum.GetName(typeof(ISactive), VenMas.IsActive),
                                 }).OrderByDescending(x => x.DLNoDate).ToList();

            return help;

        }
        public Help getVendor1(string name, int id)
        {
            var help = new Help();
            help.Vendordetail = new List<VendorMas>();
            help.Vendordetail = (from VenMas in CMPSContext.VendorMaster.Where(x => x.VendorCode.ToString() == name && x.IsDeleted == false && x.CmpID == id || x.Name == name && x.IsDeleted == false && x.CmpID == id || x.PhoneNo == name && x.IsDeleted == false && x.CmpID == id || x.MobileNo == name && x.IsDeleted == false && x.CmpID == id)

                                 select new VendorMas
                                 {
                                     ID = VenMas.ID,
                                     VendorCode = VenMas.VendorCode,
                                     VendorCategory = Enum.GetName(typeof(Vendor), VenMas.VendorCategory),
                                     Name = VenMas.Name,
                                     Address1 = VenMas.Address1,
                                     Address2 = VenMas.Address2,
                                     Address3 = VenMas.Address3,
                                     LocationID = CMPSContext.LocationMaster.Where(X => X.ID == Convert.ToInt16(VenMas.Location)).Select(x => x.ParentDescription).FirstOrDefault(),
                                     City = Convert.ToString(CMPSContext.LocationMaster.Where(x => x.ID == Convert.ToInt16(VenMas.Location)).Select(x => x.ParentID).FirstOrDefault()),
                                     DLNo = VenMas.DLNo,
                                     DLNo1 = VenMas.DLNo1,
                                     DLNoDate = VenMas.DLNoDate,
                                     DLNoDate1 = VenMas.DLNoDate1,
                                     Landmark = VenMas.Landmark,
                                     FSSAINo = VenMas.FSSAINo,
                                     GSTNo = VenMas.GSTNo,
                                     MobileNo = VenMas.MobileNo,
                                     PhoneNo = VenMas.PhoneNo,
                                     ContactPerson = VenMas.ContactPerson,
                                     CreditDays = VenMas.CreditDays,
                                     Creditlimit = VenMas.Creditlimit,
                                     Leadtime = VenMas.Leadtime,
                                     IsActive = Enum.GetName(typeof(ISactive), VenMas.IsActive),
                                 }
                ).ToList();


            return help;

        }


        public Help getStoremaster(int Name)
        {
            var help = new Help();
            help.Storedetail = new List<StoreMaster>();

            help.Storedetail = (from StoreMas in CMPSContext.Storemasters.Where(x => x.IsDelete == false && x.CmpID == Name)

                                select new StoreMaster
                                {
                                    StoreID = StoreMas.StoreID,
                                    Storename = StoreMas.Storename,
                                    StoreLocation = StoreMas.StoreLocation,
                                    Address1 = StoreMas.Address1,
                                    Address2 = StoreMas.Address2,
                                    StoreKeeper = StoreMas.StoreKeeper,
                                    CmpID = StoreMas.CmpID,
                                    Catgtype = StoreMas.CatgType,
                                    IsActive = StoreMas.IsActive == true ? "Yes" : "No",

                                }

                ).ToList();


            return help;

        }


        public Help getStoremaster1(string Name, int id)
        {
            var help = new Help();
            help.Storedetail = new List<StoreMaster>();

            help.Storedetail = (from StoreMas in CMPSContext.Storemasters.Where(x => x.StoreKeeper == Name && x.IsDelete == false && x.CmpID == id || x.Storename == Name && x.IsDelete == false && x.CmpID == id || x.StoreLocation == Name && x.IsDelete == false && x.CmpID == id)



                                select new StoreMaster
                                {
                                    StoreID = StoreMas.StoreID,
                                    Storename = StoreMas.Storename,
                                    StoreLocation = StoreMas.StoreLocation,
                                    Address1 = StoreMas.Address1,
                                    Address2 = StoreMas.Address2,
                                    StoreKeeper = StoreMas.StoreKeeper,
                                    CmpID = StoreMas.CmpID,
                                    IsActive = StoreMas.IsActive == true ? "Yes" : "No",
                                }

                ).ToList();


            return help;

        }



        public Help getOperationTheatre(int id)
        {
            var help = new Help();
            help.OperationTheatreDetails = new List<OperationTheatreDetails>();
            var oneliemaster = CMPSContext.OneLineMaster.ToList();

            help.OperationTheatreDetails = (from operation in WYNKContext.OperationTheatre.Where(x => x.IsDeleted == false && x.CMPID == id)

                                            select new OperationTheatreDetails
                                            {
                                                OTID = operation.OTID,
                                                OTDescription = operation.OTDescription,
                                                OTAddress1 = operation.OTAddress1,
                                                OTAddress2 = operation.OTAddress2,
                                                OTAddress3 = operation.OTAddress3,
                                                OTLocation = oneliemaster.Where(x => x.OLMID == Convert.ToInt64(operation.OTLocation)).Select(x => x.ParentDescription).FirstOrDefault(),
                                                OTCharge = operation.OTCharge,
                                                GST = operation.GST,
                                                SGST = operation.SGST,
                                                CGST = operation.CGST,
                                                IsActive = operation.IsActive,
                                                IsActiveName = operation.IsActive == true ? "Yes" : "No"
                                            }

                ).ToList();


            return help;

        }

        public Help getOperationTheatreextension(int id)
        {
            var help = new Help();
            help.OperationExtensionDetails = new List<OperationExtensionDetails>();
         
            help.OperationExtensionDetails = (from operationext in WYNKContext.OperationTheatreExtension.Where(x => x.IsDeleted == false && x.OTID == id)

                                              select new OperationExtensionDetails
                                              {

                                                  ID = operationext.ID,
                                                  ICDCODE = operationext.ICDCODE,
                                                  ICDSPECIALITY = Convert.ToInt32(operationext.SpecialityDescription),
                                                  OTID = operationext.OTID,

                                              }).ToList();
            return help;

        }
        public Help getOperationTheatre1(string Name, int id)
        {
            var help = new Help();
            help.OperationTheatreDetails = new List<OperationTheatreDetails>();

            var oneliemaster = CMPSContext.OneLineMaster.ToList();

            help.OperationTheatreDetails = (from operation in WYNKContext.OperationTheatre.Where(x => x.OTDescription == Name && x.IsDeleted == false && x.CMPID == id || x.OTLocation == Name && x.IsDeleted == false && x.CMPID == id)



                                            select new OperationTheatreDetails
                                            {
                                                OTID = operation.OTID,
                                                OTDescription = operation.OTDescription,                                                
                                                OTAddress1 = operation.OTAddress1,
                                                OTAddress2 = operation.OTAddress2,
                                                OTAddress3 = operation.OTAddress3,
                                                OTLocation = oneliemaster.Where(x => x.OLMID == Convert.ToInt64(operation.OTLocation)).Select(x => x.ParentDescription).FirstOrDefault(),
                                                OTCharge = operation.OTCharge,
                                                GST = operation.GST,
                                                SGST = operation.SGST,
                                                CGST = operation.CGST,
                                                IsActive = operation.IsActive,
                                                IsActiveName = operation.IsActive == true ? "Yes" : "No"
                                            }

                ).ToList();


            return help;

        }


        public Help getSurgeryDischargeDetails(int id,string GMT)
        {

            var registration = WYNKContext.Registration.ToList();
            var SurgDetail = WYNKContext.Surgery.ToList();
            var SurgTranDetail = WYNKContext.SurgeryTran.ToList();
            var Admission = WYNKContext.Admission.ToList();
         
            var icdcode = WYNKContext.ICDMaster.ToList();
            TimeSpan ts = TimeSpan.Parse(GMT);

            var help = new Help();

            help.SurgeryDischarge = (from admission in Admission.Where(x => x.DischargeID == null && x.CMPID == id)
                                     join surgDetail in SurgDetail on admission.RegTranID equals surgDetail.RegistrationTranID
                                     join surgTranDetail in SurgTranDetail on surgDetail.RandomUniqueID equals surgTranDetail.SurgeryID
                                     select new SurgeryDischargedetail
                                     {
                                         UIN = admission.UIN,
                                         Name = registration.Where(x => x.UIN == admission.UIN).Select(reg => String.Concat(reg.Name + " " + (reg.MiddleName != null ? reg.MiddleName : " ") + " " + reg.LastName)).FirstOrDefault(),
                                         DOB = registration.Where(x => x.UIN == admission.UIN).Select(x => x.DateofBirth).FirstOrDefault(),
                                         Age = ToAgeString(registration.Where(x => x.UIN == admission.UIN).Select(x => x.DateofBirth).FirstOrDefault()),
                                         Gender = registration.Where(x => x.UIN == admission.UIN).Select(x => x.Gender).FirstOrDefault(),
                                         SurgeryDate = Convert.ToDateTime(surgDetail.DateofSurgery + ts),
                                         AdmissionDate = admission.AdmDate + ts,
                                         ocular = surgTranDetail.IsOD ? "Right Eye" : surgTranDetail.IsOS ? "Left Eye" : "Both Eyes",
                                         RegistrationTranId = surgDetail.RegistrationTranID,
                                         SurID = surgDetail.RandomUniqueID,
                                         AdmissionID = admission.RandomUniqueID,
                                         Surgery = WYNKContext.ICDSpecialityCode.Where(x => x.ID == surgTranDetail.IcdSpecialityCode).Select(x => x.SpecialityDescription).FirstOrDefault(),
                                         SurgeryDescription = icdcode.Where(x => x.ICDCODE == surgTranDetail.ICDCode).Select(x => x.ICDDESCRIPTION).FirstOrDefault(),
                                         SurgeonDetails = GetSurgeonDetails(surgDetail.RandomUniqueID),
                                     }).ToList();

            return help;

        }





        public Help DischargedPatientDetails(int id, string GMT)
        {
            var help = new Help();
            var icdcode = WYNKContext.ICDMaster.ToList();
            var PatientGeneral = WYNKContext.PatientGeneral.ToList();
            var SD = WYNKContext.SurgeryDischarge.Where(x => x.CMPID == id).ToList();
            var Surgery = WYNKContext.Surgery.ToList();
            var Stran = WYNKContext.SurgeryTran.ToList();
            var Adm = WYNKContext.Admission.Where(x => x.DischargeID != null).ToList();
            var Reg = WYNKContext.Registration.ToList();
            TimeSpan ts = TimeSpan.Parse(GMT);


            help.SurgeryPatientDischargedDetails = (from surDischarge in SD
                                                    join Sur in Surgery on surDischarge.SurgeryID equals Sur.RandomUniqueID
                                                    join SurTran in Stran on Sur.RandomUniqueID equals SurTran.SurgeryID
                                                    join admission in Adm on surDischarge.AdmissionID equals admission.RandomUniqueID
                                                    join reg in Reg on admission.UIN equals reg.UIN


                                                    select new SurgeryPatientDischargedDetails
                                                    {
                                                        UIN = reg.UIN,
                                                        RegistrationTranId = surDischarge.RegTranID,
                                                        SurgeryId = surDischarge.SurgeryID,
                                                        Name = String.Concat(reg.Name + " " + (reg.MiddleName != null ? reg.MiddleName : " ") + " " + reg.LastName),
                                                        DischargeDate = surDischarge.DischargeDate + ts,
                                                        SurgeryDate = Convert.ToDateTime(Sur.DateofSurgery + ts),
                                                        AdmissionDate = admission.AdmDate + ts,
                                                        TreatmentAdvice = surDischarge.TreatmentAdvice,
                                                        DischargeType = surDischarge.DischargeType,
                                                        AdmissionId = surDischarge.AdmissionID,
                                                        TransactionId = surDischarge.TransactionId,
                                                        CMPID = surDischarge.CMPID,
                                                        DOB = reg.DateofBirth,
                                                        Age = ToAgeString(reg.DateofBirth),
                                                        Gender = reg.Gender,
                                                        Allergy = PatientGeneral.OrderByDescending(x => x.UIN == reg.UIN).Select(x => x.Allergy).FirstOrDefault(),
                                                        ocular = SurTran.IsOD ? "Right Eye" : SurTran.IsOS ? "Left Eye" : "Both Eyes",
                                                        Surgery = WYNKContext.ICDSpecialityCode.Where(x => x.ID == SurTran.IcdSpecialityCode).Select(x => x.SpecialityDescription).FirstOrDefault(),
                                                        SurgeryDescription = icdcode.Where(x => x.ICDCODE == SurTran.ICDCode).Select(x => x.ICDDESCRIPTION).FirstOrDefault(),
                                                        SurgeonDetails = GetSurgeonDetails(surDischarge.SurgeryID),
                                                    }).ToList();


            return help;
        }

        public ICollection<SurgeonDetail> GetSurgeonDetails(string SurgeryID)
        {

            var DoctorIDs = WYNKContext.SurgeryAssignedTran.Where(x => x.SurgeryID == SurgeryID).Select(x => x.DoctorID).ToList();

            var res = (from Dm in CMPSContext.DoctorMaster
                       where DoctorIDs.Contains(Dm.DoctorID)
                       select new SurgeonDetail
                       {
                           Name = Dm.Firstname + (Dm.MiddleName != null ? Dm.MiddleName: "") + Dm.LastName,
                           Specialization = string.Join(",", CMPSContext.DoctorSpeciality
                                                .Where(DocSpec => DocSpec.DoctorID == Dm.DoctorID)
                                                .Select(DocSpec => CMPSContext.OneLineMaster
                                                    .Where(oneLine => oneLine.OLMID == DocSpec.OLMID)
                                                    .Select(oneLine => oneLine.ParentDescription).FirstOrDefault())),

                       }).ToList();

            return res;
        }

        public Help PreviousSurgeryDetails(string UIN, int id)
        {
            var help = new Help();
            var reg = WYNKContext.Registration.ToList();
            var DoctorMaster = CMPSContext.DoctorMaster.ToList();
            var icd = WYNKContext.ICDMaster.ToList();

            help.PreviousSurgeryDetails = (from surgery in WYNKContext.Surgery.Where(x => x.UIN == UIN && x.CMPID == id)
                                           join surgeryDoctor in WYNKContext.SurgeryDoctor on surgery.ID equals surgeryDoctor.SurgeryID
                                           join surgeryTran in WYNKContext.SurgeryTran on surgery.RandomUniqueID equals surgeryTran.SurgeryID
                                           join surgeryDischarge in WYNKContext.SurgeryDischarge on surgery.RandomUniqueID equals surgeryDischarge.SurgeryID

                                           select new PreviousSurgeryDetails
                                           {
                                               Name = reg.Where(x => x.UIN == UIN).Select(x => x.Name).FirstOrDefault(),
                                             
                                               Surgeon = DoctorMaster.Where(x => x.DoctorID == surgeryDoctor.DoctorID).Select(x => x.LastName).FirstOrDefault(),
                                               SurgeryDescription = icd.Where(x => x.ICDCODE == surgeryTran.ICDCode).Select(x => x.ICDDESCRIPTION).FirstOrDefault(),
                                           }
                                           ).ToList();


            return help;
        }



        public Help getpostoperative(string name)

        {
            var registration = WYNKContext.Registration.ToList();
            var registrationtran = WYNKContext.RegistrationTran.ToList();
            var patientgeneral = WYNKContext.PatientGeneral.ToList();
            var surgery = WYNKContext.Surgery.ToList();
            var Operation = WYNKContext.OperationTheatre.ToList();
            var surgerydoc = WYNKContext.SurgeryDoctor.ToList();
            var Doctormas = CMPSContext.DoctorMaster.ToList();
            var Surgerytran = WYNKContext.SurgeryTran.ToList();
            var icdmas = WYNKContext.ICDMaster.ToList();
            var postop = WYNKContext.PostOperative.ToList();
            var admission = WYNKContext.Admission.ToList();

            var help = new Help();



            help.Postoper = (from REG in registration.Where(u => u.UIN == name || u.Phone == name)
                             join REGT in registrationtran on REG.UIN equals REGT.UIN
                             join pg in patientgeneral on REGT.UIN equals pg.UIN
                             join sur in surgery on REGT.RegistrationTranID equals sur.RegistrationTranID
                             join surtran in Surgerytran on sur.RandomUniqueID equals surtran.SurgeryID
                             join surdoc in surgerydoc on sur.ID equals surdoc.SurgeryID
                             join adm in admission on REGT.RegistrationTranID equals adm.RegTranID
                             where adm.IsSurgeryCompleted == true

                             select new Postoper
                             {
                                 UIN = REG.UIN,
                                 Name = REG.Name,
                                 DateofBirth = REG.DateofBirth,
                                 Phone = REG.Phone,
                                 Age = ToAgeString(REG.DateofBirth),
                                 Gender = REG.Gender,
                                 Allergy = pg.Allergy,                                 
                                 ReviewDate = sur.ReviewDate,                                 
                                 OTID = Operation.Where(x => x.OTID == sur.OTID).Select(x => x.OTDescription).FirstOrDefault(),
                                 Description = icdmas.Where(x => x.ICDCODE == surtran.ICDCode).Select(x => x.ICDDESCRIPTION).FirstOrDefault(),
                                 Doctorname = Doctormas.Where(x => x.DoctorID == surdoc.DoctorID).Select(x => x.LastName).FirstOrDefault(),
                                 Anaestheticname = Doctormas.Where(x => x.DoctorID == surdoc.AnaestheticID).Select(x => x.LastName).FirstOrDefault(),

                                 SID = sur.ID,
                             }).LastOrDefault();



            help.Postoper3 = (from REG in registration.Where(u => u.UIN == name || u.Phone == name)
                              join REGT in registrationtran on REG.UIN equals REGT.UIN
                              join sur in surgery on REGT.RegistrationTranID equals sur.RegistrationTranID
                              join surt in Surgerytran on sur.RandomUniqueID equals surt.SurgeryID
                              join surdoc in surgerydoc on sur.ID equals surdoc.SurgeryID
                              join post in postop on sur.ID equals post.SurgeryID
                              select new Postoper3
                              {

                                  Description = icdmas.Where(x => x.ICDCODE == surt.ICDCode).Select(x => x.ICDDESCRIPTION).FirstOrDefault(),
                                  Doctorname = Doctormas.Where(x => x.DoctorID == surdoc.DoctorID).Select(x => x.LastName).FirstOrDefault(),
                                  ComplicationDetails = post.ComplicationDetails,
                                  TreatmentAdvive = post.TreatmentAdvive,
                                  PostOperativeDate = post.PostOperativeDate,
                                  ReviewDate = post.ReviewDate,
                                  ID = post.ID,
                              }).OrderByDescending(x => x.PostOperativeDate).ToList();
            return help;



        }

        public Help getpostoperative1(string name, string phone)

        {
            var registration = WYNKContext.Registration.ToList();
            var registrationtran = WYNKContext.RegistrationTran.ToList();
            var patienthistory = WYNKContext.PatientHistory.ToList();
            var findings = WYNKContext.Findings.ToList();
            var diagnosis = WYNKContext.Diagnosis.ToList();
            var onelinemaster = CMPSContext.OneLineMaster.ToList();




            var help = new Help();



            help.Postoper1 = (from REG in registration.Where(u => u.UIN == name || u.Phone == phone)
                              join REGT in registrationtran on REG.UIN equals REGT.UIN
                              join Fid in findings on REGT.RegistrationTranID equals Fid.RegistrationTranID
                              join Dia in diagnosis on Fid.RandomUniqueID equals Dia.FindingsID
                              join One in onelinemaster on Dia.DiagnosisId equals One.OLMID
                              select new Postoper1
                              {
                                  DiagnosisId = Dia.DiagnosisId,
                                  ParentDescription = One.ParentDescription,
                                  IsOD = Dia.IsOD,
                                  IsOS = Dia.IsOS,
                                  IsOU = Dia.IsOU,
                              }).ToList();

            help.Postoper2 = (from REG in registration.Where(u => u.UIN == name || u.Phone == phone)
                              join PH in patienthistory on REG.UIN equals PH.UIN
                              select new Postoper2
                              {
                                  Description = onelinemaster.Where(x => x.OLMID == PH.Description).Select(x => x.ParentDescription).FirstOrDefault(),
                                  FromUTC = Convert.ToDateTime(PH.FromUTC),
                                  Months = ((DateTime.Now.Year - Convert.ToDateTime(PH.FromUTC).Year) * 12) +Convert.ToDateTime(PH.FromUTC).Month - DateTime.Now.Month,
                              }).ToList();



            return help;
        }

        public Help getpatientlatest3(string name)
        {
            var surgery = WYNKContext.Surgery.ToList();
            var admission = WYNKContext.Admission.ToList();
            var postop = WYNKContext.PostOperative.ToList();
        

            var help = new Help();

            help.Postoperativelast = (from Adm in admission.Where(u => u.UIN == name)
                                      join Surg in surgery on Adm.UIN equals Surg.UIN
                                      join post in postop on Surg.ID equals post.SurgeryID

                                      select new Postoperativelast
                                      {

                                          PostOperativeDate = post.PostOperativeDate,
                                          ComplicationDetails = post.ComplicationDetails,
                                          TreatmentAdvive = post.TreatmentAdvive,
                                          ReviewDate = post.ReviewDate

                                      }).LastOrDefault();






            return help;



        }
        public Help getpatientlatest1(string name)
        {
            var registration = WYNKContext.Registration.ToList();
            var preop = WYNKContext.PreOperative.ToList();


            var help = new Help();

            help.preoperativelat = (from Reg in registration.Where(u => u.UIN == name)
                                    join pre in preop on Reg.UIN equals pre.UIN

                                    select new preoperativelat
                                    {

                                        PreOpDate = pre.PreOpDate,
                                        AntibioticGivenBy = pre.AntibioticGivenBy,
                                        CaseSheetPreparedBy = pre.CaseSheetPreparedBy,
                                        Instruction = pre.Instruction,
                                        Note = pre.Note

                                    }).LastOrDefault();






            return help;



        }
        public Help getpatientlatest(string name)
        {

            var surgery = WYNKContext.Surgery.ToList();
            var Operation = WYNKContext.OperationTheatre.ToList();
            var surgerydoc = WYNKContext.SurgeryDoctor.ToList();
            var Doctormas = CMPSContext.DoctorMaster.ToList();
            var Surgerytran = WYNKContext.SurgeryTran.ToList();
            var icdmas = WYNKContext.ICDMaster.ToList();
            var admission = WYNKContext.Admission.ToList();


            var help = new Help();



            help.latestadminandsur = (from Adm in admission.Where(u => u.UIN == name)
                                      join Surg in surgery on Adm.RegTranID equals Surg.RegistrationTranID
                                      join surtran in Surgerytran on Surg.RandomUniqueID equals surtran.SurgeryID
                                      join surdoct in surgerydoc on Surg.ID equals surdoct.SurgeryID

                                      select new latestadminandsur
                                      {
                                          AdmDate = Adm.AdmDate,                                          
                                          Description = icdmas.Where(x => x.ICDCODE == surtran.ICDCode).Select(x => x.ICDDESCRIPTION).FirstOrDefault(),
                                          Surgeonname = Doctormas.Where(x => x.DoctorID == surdoct.DoctorID).Select(x => x.LastName).FirstOrDefault(),
                                          OTID = Operation.Where(x => x.OTID == Surg.OTID).Select(x => x.OTDescription).FirstOrDefault(),
                                          Anaestheticname = Doctormas.Where(x => x.DoctorID == surdoct.AnaestheticID).Select(x => x.LastName).FirstOrDefault(),
                                        
                                          SurgeryCost = Surg.SurgeryCost,
                                          Review = Surg.ReviewDate,
                                          IsSurgeryCompleted = Adm.IsSurgeryCompleted,
                                          Remarks = Surg.Remarks
                                      }).LastOrDefault();


            return help;



        }
        public Help getpatientlatest2(string name)
        {
            var registration = WYNKContext.Registration.ToList();
            var surgery = WYNKContext.Surgery.ToList();
            var surgerydoc = WYNKContext.SurgeryDoctor.ToList();
            var Doctormas = CMPSContext.DoctorMaster.ToList();
            var Surgerytran = WYNKContext.SurgeryTran.ToList();
            var icdmas = WYNKContext.ICDMaster.ToList();
            var Intraop = WYNKContext.IntraOperative.ToList();


            var help = new Help();

            help.Intraoperativelat = (from Reg in registration.Where(u => u.UIN == name)
                                      join Surg in surgery on Reg.UIN equals Surg.UIN
                                      join surtran in Surgerytran on Surg.RandomUniqueID equals surtran.SurgeryID
                                      join surdoct in surgerydoc on Surg.ID equals surdoct.SurgeryID
                                      join intra in Intraop on Surg.UIN equals intra.UIN

                                      select new Intraoperativelat
                                      {
                                          Description = icdmas.Where(x => x.ICDCODE == surtran.ICDCode).Select(x => x.ICDDESCRIPTION).FirstOrDefault(),
                                          Surgeonname = Doctormas.Where(x => x.DoctorID == surdoct.DoctorID).Select(x => x.LastName).FirstOrDefault(),
                                          postoperativeins = intra.Note
                                      }).LastOrDefault();

            return help;



        }


        public Help getInvPre(string uin, string IPID, int Cmpid)
        {
            var InvestigationPre = WYNKContext.InvestigationPrescription.ToList();
            var InvestigationPreT = WYNKContext.InvestigationPrescriptionTran.ToList();
            var Oneline = CMPSContext.OneLineMaster.ToList();
            var DoctorMaster = CMPSContext.DoctorMaster.ToList();
            var services = WYNKContext.Services.ToList();            
            var v1 = InvestigationPre.Where(x => x.RandomUniqueID == IPID ).Select(x => x.PrescribedBy).FirstOrDefault();

            var help = new Help();

            help.InvPre1 = (from IP in InvestigationPreT.Where(k => k.IPID == IPID)
                            select new InvPre
                            {
                                PrescribedBy = DoctorMaster.Where(x => x.DoctorID == v1).Select(x => x.Firstname + " " + x.MiddleName + " " + x.LastName).FirstOrDefault(),
                                DRNumber = DoctorMaster.Where(x => x.DoctorID == v1).Select(x => x.RegistrationNumber).FirstOrDefault(),                                
                                InvestigationID = services.Where(x => x.ID == IP.InvestigationID).Select(x => x.Description).FirstOrDefault(),
                                Amount = IP.Amount,

                            }

                            ).ToList();


            help.DRNAME = help.InvPre1.Select(x => x.PrescribedBy).FirstOrDefault();
            help.DRID = help.InvPre1.Select(x => x.DRNumber).FirstOrDefault();
            help.P1Address = CMPSContext.Company.Where(x => x.CmpID == Cmpid).Select(x => x.Address1).FirstOrDefault();
            help.P1Address2 = CMPSContext.Company.Where(x => x.CmpID == Cmpid).Select(x => x.Address2).FirstOrDefault();
            help.P1phone = CMPSContext.Company.Where(x => x.CmpID == Cmpid).Select(x => x.Phone1).FirstOrDefault();
            help.P1web = CMPSContext.Company.Where(x => x.CmpID == Cmpid).Select(x => x.Website).FirstOrDefault();
            help.P1Compnayname = CMPSContext.Company.Where(x => x.CmpID == Cmpid).Select(x => x.CompanyName).FirstOrDefault();

            help.InvPre2 = (from IP1 in InvestigationPre.Where(k => k.UIN == uin && k.CMPID == Cmpid)
                            select new InvPre1
                            {
                                IPID = IP1.RandomUniqueID,
                                Dateofinvestigation1 = IP1.CreatedUTC,
                                PrescribedBy1 = DoctorMaster.Where(x => x.DoctorID == IP1.PrescribedBy).Select(x => x.Firstname + " " + x.MiddleName + " " + x.LastName).FirstOrDefault(),
                                DRNumber = DoctorMaster.Where(x => x.DoctorID == IP1.PrescribedBy).Select(x => x.RegistrationNumber).FirstOrDefault(),
                                Remarks1 = IP1.Remarks,
                            }

                        ).OrderByDescending(x => x.Dateofinvestigation1).ToList();


            return help;
        }
        public Help getModuleMasterDetails(int id)
        {
            var getData = new Help();
            var ModuleMas = CMPSContext.ModuleMaster.ToList();
            getData.getModuleMasDetails = (from details in CMPSContext.ModuleMaster
                                           select new getModuleMasDetails
                                           {
                                               ModuleID = details.ModuleID,
                                               ModuleDescription = details.ModuleDescription,
                                               TransactionID = details.TransactionTypeID,
                                               ModuleType = details.ModuleType,
                                               ParentModuleid1 = details.ParentModuleid,
                                               ParentModule = ModuleMas.Where(x => x.ModuleID == details.ParentModuleid).Select(x => x.ModuleDescription).FirstOrDefault(),
                                               Parentmoduledescription = details.Parentmoduledescription,
                                               IsActive = details.IsActive,
                                               isvisible = details.Visible,
                                               Tag = details.Tag,
                                           }).OrderBy(x => x.ModuleDescription).ToList();
            return getData;
        }

        public Help PopupSearch(int CompanyID)
        {
            var Vendor = CMPSContext.VendorMaster.ToList();
            var Store = CMPSContext.Storemasters.ToList();
            var Stock = WYNKContext.StockMaster.ToList();
          
            var PopupSearch = new Help();
            PopupSearch.helpmaterialreturn = new List<helpmaterialreturn>();
            PopupSearch.helpmaterialreturn = (from ST in Stock.Where(x => x.TransactionType == "R" && x.CMPID == CompanyID)
                                              join SM in Store
                                              on ST.StoreID equals SM.StoreID
                                              join VM in Vendor
                                              on ST.VendorID equals VM.ID

                                              select new helpmaterialreturn
                                              {
                                                  ReciptNumber = ST.DocumentNumber,
                                                  StoreName = SM.Storename,
                                                  VendorName = VM.Name,                                              
                                                  StoreID = ST.StoreID,
                                                  PoID = ST.POID,

                                              }).ToList();
            return PopupSearch;
        }


        public Help getCompanyName(string CompanyID)
        {
            var help = new Help();

            var C1 = CMPSContext.Company.Where(x => x.CmpID == Convert.ToInt32(CompanyID)).ToList();
            var C2 = CMPSContext.Company.Where(x => x.ParentID == Convert.ToInt32(CompanyID)).ToList();
            var mergedcompanies = C1.Concat(C2);

            help.getCompName1 = (from details in mergedcompanies.Where(x => x.IsDeleted == false)

                                 select new getCompName1
                                 {
                                     companyName = details.CompanyName,
                                     CmpID = details.CmpID,
                                     ParentID = details.ParentID
                                 }).ToList();

            help.getCompDetails1 = (from Det in mergedcompanies.Where(x => x.IsDeleted == false)


                                    select new getCompDetails1
                                    {
                                        CmpID = Det.CmpID,
                                        CompanyName = Det.CompanyName,
                                        Address1 = Det.Address1,
                                        Address2 = Det.Address2,
                                        Address3 = Det.Address3,
                                        LocationName = Det.LocationName,
                                        LocationID = Det.LocationID,
                                        City = Convert.ToString(CMPSContext.LocationMaster.Where(x => x.ID == Convert.ToInt16(Det.LocationID)).Select(x => x.ParentID).FirstOrDefault()),
                                        ContactPerson = Det.ContactPerson,
                                        GSTNo = Det.GSTNo,
                                        ParentID = Det.ParentID,
                                        Phone1 = Det.Phone1,
                                        Phone2 = Det.Phone2,
                                        EmailID = Det.EmailID,
                                        Website = Det.Website,
                                        CompanyGroup = help.getCompName1.Where(x => x.CmpID == Det.ParentID).Select(x => x.companyName).FirstOrDefault(),
                                        CompanyCategory = Det.ParentID == 0 ? "Main" : "Branch",


                                    }).ToList();



            return help;
        }


        public Help getDrugGroupDetails(int Cmpid)
        {
            var getData = new Help();
            var Onelinemaster = CMPSContext.OneLineMaster.ToList();
            getData.getDrugGroupDetails = (from details in WYNKContext.DrugGroup.Where(x => x.IsDeleted == false && x.Cmpid == Cmpid)
                                           select new getDrugGroupDetails
                                           {
                                               ID = details.ID,
                                               Description = details.Description,
                                               DrugFormID = details.DrugFormID,
                                               DrugFormName = Onelinemaster.Where(x => x.OLMID == details.DrugFormID).Select(x => x.ParentDescription).FirstOrDefault(),
                                               RetestInterval = details.RetestInterval,
                                               RetestCriticalInterval = details.RetestCriticalInterval,
                                               SideEffects = details.SideEffects,
                                               Precautions = details.Precautions,
                                               Overdose = details.Overdose,
                                               IsStepDown = details.IsStepDown
                                           }).OrderByDescending(x => x.ID).ToList();
            return getData;
        }



        public Help PopupSearch2(string OptionType)
        {
            var LensMas = WYNKContext.Lensmaster.ToList();
            var LensTarn = WYNKContext.Lenstrans.ToList();
            var TaxMas = CMPSContext.TaxMaster.ToList();
            var BrandMas = WYNKContext.Brand.ToList();
            var PopupSearch2 = new Help();
            PopupSearch2.GetDataArraysss = new List<GetDataArraysss>();
            PopupSearch2.GetDataArraysss = (from LM in LensMas.Where(x => x.LensType == OptionType)
                                            join LT in LensTarn on LM.RandomUniqueID equals LT.LMID
                                            join BM in BrandMas on LT.Brand equals BM.ID
                                            select new GetDataArraysss
                                            {
                                                TID = LT.ID,
                                                ID = LM.ID,
                                                Brand = BM.Description,

                                                Description = LM.Description,
                                                Index = LT.Index,
                                                Color = LT.Colour,
                                                Size = LT.Size,
                                                Price = LT.Prize,
                                                Model = LT.Model,
                                                GST = TaxMas.Where(x => x.ID == LM.TaxID).Select(x => x.GSTPercentage).FirstOrDefault(),
                                                CESS = LT.CESSAmount,
                                                AddCess = LT.ADDCESSAmount,
                                                TaxIDD = LM.TaxID,

                                            }).ToList();
            return PopupSearch2;
        }


        public Help PopupSearch1(int val)
        {
            var treatment = WYNKContext.TreatmentMaster.ToList();
            var splode = WYNKContext.ICDSpecialityCode.ToList();

            var PopupSearch1 = new Help();
            PopupSearch1.GetDataArrayss = new List<GetDataArrayss>();
            PopupSearch1.GetDataArrayss = (from TM in treatment.Where(x => x.ICD_SPECIALITY_ID == val && x.IsDeleted != true)
                                           join SM in splode on
                                           TM.ICD_SPECIALITY_ID equals SM.ID

                                           select new GetDataArrayss
                                           {
                                               ID = TM.ID,
                                               SpecialityID1 = SM.SpecialityDescription,
                                               Description1 = TM.TreatmentDescription,
                                               IsActive1 = TM.IsActive,

                                           }).ToList();
            return PopupSearch1;
        }

        public Help CustomerMasterDetails(int Cmpid)
        {
            var Customer = new Help();

            var Location = CMPSContext.LocationMaster.ToList();

            Customer.CustomerDetails = (from Cust in WYNKContext.CustomerMaster.Where(x => x.CmpID == Cmpid && x.IsDeleted == false)
                                        orderby Cust.CreatedUTC descending
                                        select new CustomerDetails
                                        {
                                            ID = Cust.ID,
                                           
                                            UIN = Cust.UIN,
                                            Name = Cust.Name,
                                            Address1 = Cust.Address1,
                                            Address2 = Cust.Address2,
                                            Address3 = Cust.Address3,
                                            MidleName = Cust.MiddleName,
                                            LastName = Cust.LastName,
                                            MobileNo = Cust.MobileNo,
                                            PhoneNo = Cust.PhoneNo,
                                            GSTNo = Cust.GSTNo,
                                            ContactPerson = Cust.ContactPerson,
                                            LocationName = Location.Where(x => x.ID == Cust.Location).Select(x => x.ParentDescription).FirstOrDefault(),
                                            City = Convert.ToString(Location.Where(x => x.ID == Cust.Location).Select(x => x.ParentID).FirstOrDefault()),
                                        }).ToList();



            return Customer;
        }


        public Help DonorDetails(int Cmpid)
        {
            var Donor = new Help();

            var Location = CMPSContext.LocationMaster.ToList();

            Donor.DonorDetails = (from Cust in WYNKContext.Donor.Where(x => x.CmpID == Cmpid && x.IsDeleted == false)
                                        select new DonorDetails
                                        {
                                            ID = Cust.ID,
                                            Name = Cust.Name,
                                            DonorType = Cust.DonorType,
                                            Address1 = Cust.Address1,
                                            Address2 = Cust.Address2,
                                            Address3 = Cust.Address3,
                                            MidleName = Cust.MiddleName,
                                            LastName = Cust.LastName,
                                            MobileNo = Cust.MobileNo,
                                            PhoneNo = Cust.PhoneNo,
                                            GSTNo = Cust.GSTNo,
                                            ContactPerson = Cust.ContactPerson,
                                            LocationName = Location.Where(x => x.ID == Cust.Location).Select(x => x.ParentDescription).FirstOrDefault(),
                                            City = Convert.ToString(Location.Where(x => x.ID == Cust.Location).Select(x => x.ParentID).FirstOrDefault()),
                                        }).ToList();



            return Donor;
        }



        public Help CustomerOrder(int CMPID)
        {
            var CustomerOrder = new Help();
            var LensMas = WYNKContext.Lensmaster.ToList();
            var LensTarn = WYNKContext.Lenstrans.ToList();
            var TaxMas = CMPSContext.TaxMaster.ToList();
            var BrandMas = WYNKContext.Brand.ToList();
            var UOM = CMPSContext.uommaster.ToList();
            var Onelinemaster = CMPSContext.OneLineMaster.ToList();

            int cmp = CMPSContext.Company.AsNoTracking().Where(c => c.CmpID == CMPID).Select(c => c.ParentID).FirstOrDefault();
            if (cmp == 0)
            {
                cmp = CMPID;
            }

            CustomerOrder.OfferDetails = (from LM in LensMas.Where(x => x.CMPID == cmp)
                                          join LT in LensTarn.Where(x => x.IsActive == true) on LM.RandomUniqueID equals LT.LMID
                                          join BM in BrandMas on LT.Brand equals BM.ID
                                          select new OfferDetail
                                          {
                                              LTID = LT.ID,
                                              LMID = LM.ID,
                                              Brand = BM.Description,
                                           
                                              Description = LT.Description,
                                              
                                              Index = LT.Index != "" ? Onelinemaster.Where(x => x.OLMID == Convert.ToInt32(LT.Index)).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty,
                                              Color = LT.Colour,
                                              Size = LT.Size,
                                              Price = LT.Prize,
                                              Model = LT.Model,
                                              GST = TaxMas.Where(x => x.ID == LT.TaxID).Select(x => x.GSTPercentage).FirstOrDefault(),
                                              CESS = TaxMas.Where(x => x.ID == LT.TaxID).Select(x => x.CESSPercentage).FirstOrDefault(),
                                              AddCess = TaxMas.Where(x => x.ID == LT.TaxID).Select(x => x.AdditionalCESSPercentage).FirstOrDefault(),
                                              GSTDesc = TaxMas.Where(x => x.ID == LT.TaxID).Select(x => x.TaxDescription).FirstOrDefault(),
                                              CESSDesc = TaxMas.Where(x => x.ID == LT.TaxID).Select(x => x.CESSDescription).FirstOrDefault(),
                                              AddCessDesc = TaxMas.Where(x => x.ID == LT.TaxID).Select(x => x.AdditionalCESSDescription).FirstOrDefault(),
                                              TaxIDD = LT.TaxID,
                                              Type = LM.LensType,
                                              HSNNo = LT.HSNNo,
                                              UOM = UOM.Where(x => x.id == LT.UOMID).Select(x => x.Description).FirstOrDefault(),
                                              uomid = LT.UOMID,
                                              Sph = LT.Sph,
                                              Cyl = LT.Cyl,
                                              Axis = LT.Axis,
                                              Add = LT.Add,
                                          }).ToList();
            return CustomerOrder;
        }

        public Help RoomSearch(int CompanyID)
        {
            var M_Room = WYNKContext.Room.ToList();
            var oneline = CMPSContext.OneLineMaster.AsNoTracking().Where(x => x.ParentTag == "RoomType").ToList();

            var Help = new Help();

            Help.helpRoom = (from Room in M_Room.Where(x => x.CMPID == CompanyID)
                             select new helpRoom
                             {
                                 ID = Room.ID,
                                 RoomType = oneline.Where(x => x.OLMID == Convert.ToInt32(Room.RoomType)).Select(x => x.ParentDescription).FirstOrDefault(),
                                 RoomDescription = Room.RoomDescription,
                                 RoomCost = Room.RoomCost,
                                 NoofRooms = Room.NoofRooms,
                                 NoofBed = Room.NoofBed,
                                 IsActive = Room.IsActive
                             }).ToList();

            return Help;
        }



        public Help StoreSearch()
        {
           
            var Help = new Help();

            Help.Storeroom = (from s in CMPSContext.Storemasters.Where(x => x.IsActive == true)
                              select new storedetailsDetails
                              {
                                  ID = s.StoreID,
                                  Name = s.Storename,
                                  LocationName = s.StoreLocation,
                                  Address1 = s.Address1,
                                  Address2 = s.Address2,
                                  Address3 = s.StoreKeeper,
                                  PhoneNo = s.PhoneNumber,

                              }).ToList();

            return Help;
        }



        public Help searchStoreSearch()
        {

            var Help = new Help();
            var op = WYNKContext.OperationTheatre.ToList();

            var store = CMPSContext.Storemasters.ToList();
            var OtIndent = WYNKContext.OTIndent.ToList();
          
            Help.Getindentsearchdetails = (from s in OtIndent

                                           select new Getindentsearchdetails
                                           {
                                               OperationtheatreIDD = s.ID,
                                               Operationtheatre = op.Where(x => x.OTID == s.OTID).Select(x => x.OTDescription).FirstOrDefault(),
                                               Indentraisedby = s.IndentRaisedBy,
                                               Indentdate = s.OTIndentDate,
                                               storename = store.Where(x => x.StoreID == s.StoreID).Select(x => x.Storename).FirstOrDefault(),
                                               Address1 = store.Where(x => x.StoreID == s.StoreID).Select(x => x.Address1).FirstOrDefault(),
                                               Address2 = store.Where(x => x.StoreID == s.StoreID).Select(x => x.Address2).FirstOrDefault(),
                                              
                                               status = s.IsCancelled,
                                              
                                           }).ToList();

            return Help;
        }

        public Help getSearchdetailsofindent(int id)
        {

            var Help = new Help();
            var op = WYNKContext.OperationTheatre.ToList();
            var store = CMPSContext.Storemasters.ToList();
            var drug = WYNKContext.DrugMaster.ToList();
            var druggrp = WYNKContext.DrugGroup.ToList();           
            var OtIndent = WYNKContext.OTIndent.ToList();
            var Olm = CMPSContext.OneLineMaster.ToList();
            var Otindenttran = WYNKContext.OTIndentTran.ToList();

            Help.Getindentsearchtotaldetails = new List<Getindentsearchtotaldetails>();

            var otid = OtIndent.Where(x => x.ID == id).Select(x => x.OTID).FirstOrDefault();
            var stid = OtIndent.Where(x => x.ID == id).Select(x => x.StoreID).FirstOrDefault();
            Help.Operationtheatre = op.Where(x => x.OTID == otid).Select(x => x.OTDescription).FirstOrDefault();
            Help.Indentraisedby = OtIndent.Where(x => x.ID == id).Select(x => x.IndentRaisedBy).FirstOrDefault();
            Help.Indentdate = OtIndent.Where(x => x.ID == id).Select(x => x.OTIndentDate).FirstOrDefault();
            Help.storename = store.Where(x => x.StoreID == stid).Select(x => x.Storename).FirstOrDefault();
            Help.Address1 = store.Where(x => x.StoreID == stid).Select(x => x.Address1).FirstOrDefault();
            Help.Address2 = store.Where(x => x.StoreID == stid).Select(x => x.Address2).FirstOrDefault();
            Help.Storekeeprr = store.Where(x => x.StoreID == stid).Select(x => x.StoreKeeper).FirstOrDefault();
            Help.Storenumber = store.Where(x => x.StoreID == stid).Select(x => x.PhoneNumber).FirstOrDefault();
            Help.status = OtIndent.Where(x => x.ID == id).Select(x => x.IsCancelled).FirstOrDefault();
            Help.Indewntnumber = OtIndent.Where(x => x.ID == id).Select(x => x.IndentRunningNumber).FirstOrDefault();
            foreach (var item in Otindenttran.Where(x => x.OTIndentID == id).ToList())
            {
                var Searchdata = new Getindentsearchtotaldetails();                
                var genericid = drug.Where(x => x.ID == item.DrugID).Select(x => x.GenericName).FirstOrDefault();                
                var manufactureid = drug.Where(x => x.ID == item.DrugID).Select(x => x.Manufacturer).FirstOrDefault();
                Searchdata.Brand = drug.Where(x => x.ID == item.DrugID).Select(x => x.Brand).FirstOrDefault();
                Searchdata.Generic = druggrp.Where(x => x.ID == genericid).Select(x => x.Description).FirstOrDefault();
                Searchdata.Manufacture = Olm.Where(x => x.OLMID == manufactureid).Select(x => x.ParentDescription).FirstOrDefault();
                Searchdata.druggroup = drug.Where(x => x.ID == item.DrugID).Select(x => x.DrugGroup).FirstOrDefault();
                Searchdata.uom = drug.Where(x => x.ID == item.DrugID).Select(x => x.UOM).FirstOrDefault();

                var tranID = Otindenttran.Where(x => x.ID == item.ID).Select(x => x.ID).FirstOrDefault();
                var rty = Otindenttran.Where(x => x.ID == tranID).Select(x => x.ReceivedQty).FirstOrDefault();
                Searchdata.Rqty = GetNullvalue(Convert.ToInt32(rty));
                Searchdata.Iqty = Otindenttran.Where(x => x.ID == tranID).Select(x => x.IndentQty).FirstOrDefault();
                Help.Getindentsearchtotaldetails.Add(Searchdata);
            }

            foreach (var iitem in Help.Getindentsearchtotaldetails)
            {
                if (iitem.Rqty != null && iitem.Rqty != 0)
                {
                    Help.Yesstatus = "YESRQTYISNULL";
                    break;
                }
            }



            return Help;
        }

        public dynamic GetNullvalue(int data)
        {
            var nulldata = data;
            if (nulldata == null)
            {
                nulldata = 0;
            }
            else
            {
                nulldata = data;
            }


            return nulldata;

        }



        public Help getTaxData(int id)
        {
            var getData = new Help();


            getData.getTaxDet = (from details in CMPSContext.TaxMaster.Where(x => x.IsActive == true)

                                 select new getTaxDetS
                                 {
                                     ID = details.ID,
                                     TaxDescription = details.TaxDescription,
                                     GSTPercentage = details.GSTPercentage,
                                     SGSTPercentage = details.SGSTPercentage,
                                     CGSTPercentage = details.CGSTPercentage,
                                     IGSTPercentage = details.IGSTPercentage,
                                     CESSDescription = details.CESSDescription,
                                     CESSPercentage = details.CESSPercentage,
                                     AdditionalCESSDescription = details.AdditionalCESSDescription,
                                     AdditionalCESSPercentage = details.AdditionalCESSPercentage,
                                     TaxGroupId = Enum.GetName(typeof(TaxGroupId), details.TaxGroupId),
                                     IsActive = details.IsActive,
                                     istransact = details.istransact,
                                 }).OrderByDescending(x => x.ID).ToList();
            return getData;
        }

        public Help GetConfigureDetail()
        {
            var configuredetail = new Help();
            configuredetail.ConfigureDetail = (from get in WYNKContext.Configuration
                                               select new ConfigureDetails
                                               {
                                                   id = get.ID,
                                                   RRDescription = get.RRDescription,
                                                   RRAdvdays = get.RRAdvDays,
                                                   FrequencyperDay = get.FrequencyperDay,
                                                   HostEmailID = get.HostEmailID,
                                                   HostPassword = PasswordEncodeandDecode.DecodeFrom64(get.HostPassword),
                                                   Phonenumber = get.Phonenumber,
                                                   SendSMS = get.SendSMS,
                                                   SendEmail = get.SendEmail,

                                               }).ToList();



            return configuredetail;

        }


        public Help BMISearch(int ID)
        {

            var getData = new Help();
            var M_BMI = WYNKContext.BMI.ToList();

            getData.BMIDetails = (from BMI in M_BMI
                                  select new BMIDetails
                                  {
                                      ID = BMI.ID,
                                      Category = BMI.Category,
                                      FromRange = BMI.FromRange,
                                      ToRange = BMI.ToRange
                                  }
                                  ).ToList();


            return getData;

        }
        public Help PACSearch(int ID)
        {
            var getData = new Help();

            var M_Admission = WYNKContext.Admission.ToList();
            var M_Reg = WYNKContext.Registration.ToList();
            var M_RegTran = WYNKContext.RegistrationTran.ToList();
            var M_PatientGeneral = WYNKContext.PatientGeneral.ToList();

            var Surgery = WYNKContext.Surgery.ToList();
            var SurgeryTran = WYNKContext.SurgeryTran.ToList();
            var icdspeccode = WYNKContext.ICDSpecialityCode.ToList();
            var SurgeryAssigned = WYNKContext.SurgeryAssigned.ToList();

            getData.getPACDetail = (from Admsn in M_Admission.Where(x => x.CMPID == ID && x.IsSurgeryCompleted == false)
                                    join regTran in M_RegTran on Admsn.RegTranID equals regTran.RegistrationTranID
                                    join reg in M_Reg on regTran.UIN equals reg.UIN
                                    join su in Surgery on Admsn.RandomUniqueID equals su.AdmID
                                    join SA in SurgeryAssigned on su.RandomUniqueID equals SA.SurgeryID


                                    select new getPACDetail
                                    {
                                        AdmID = Admsn.AdmID,
                                        PatientName = String.Concat(reg.Name + " " + reg.MiddleName + " " + reg.LastName),
                                        Age = ToAgeString(reg.DateofBirth),
                                        Gender = reg.Gender,
                                        UIN = Admsn.UIN,
                                        FamilyHo = M_PatientGeneral.Where(x => x.UIN == Admsn.UIN).Select(x => x.FamilyHistory).FirstOrDefault(),
                                        KnownAllergy = M_PatientGeneral.Where(x => x.UIN == Admsn.UIN).Select(x => x.Allergy).FirstOrDefault(),
                                        Surgerycode = Surgery.Where(x => x.UIN == Admsn.UIN).Select(x => x.ID).FirstOrDefault(),
                                        SAID = SA.SAID

                                    }).ToList();

            getData.SurgeryTypeDet = (from sur in Surgery.Where(x => x.ID == getData.getPACDetail.Select(y => y.Surgerycode).FirstOrDefault())
                                      join surtran in SurgeryTran on sur.RandomUniqueID equals surtran.SurgeryID
                                      join icd in icdspeccode on surtran.IcdSpecialityCode equals icd.ID

                                      select new SurgeryTypeDet
                                      {
                                          surgerytype = icd.SpecialityDescription,

                                      }).ToList();





            return getData;
        }

        public dynamic getIOProcedureTempDetails(int ICDSpecialityCode)
        {
            try
            {

                var res = (from IO in WYNKContext.IOProcedureTemplate.Where(x => x.IsActive == true && x.ICDSpecialityCode == ICDSpecialityCode)
                           group IO by IO.SurgeryDescription into IOgroup
                           select new
                           {
                               icdspeciality = WYNKContext.ICDSpecialityCode.Where(x => x.ID == IOgroup.Select(y => y.ICDSpecialityCode).FirstOrDefault()).Select(x => x.SpecialityDescription).FirstOrDefault(),
                               icdspecialityID = Convert.ToString(IOgroup.Select(y => y.ICDSpecialityCode).FirstOrDefault()),
                               SurgeryDescription = IOgroup.Key,
                               SurgeryDescObj = (from IOt in WYNKContext.IOProcedureTemplate.Where(x => x.IsActive == true && x.ICDSpecialityCode == ICDSpecialityCode)
                                                 join IOtrans in WYNKContext.IOTemplateTran.Where(x => x.IsActive == true) on IOt.ID equals IOtrans.IOTemplateID
                                                 where (IOgroup.Select(x => x.ID)).Contains(IOtrans.IOTemplateID)
                                                 select new
                                                 {
                                                     ID = IOt.ID,
                                                     icdSpecialityDesc = WYNKContext.ICDSpecialityCode.Where(x => x.ID == IOgroup.Select(y => y.ICDSpecialityCode).FirstOrDefault()).Select(x => x.SpecialityDescription).FirstOrDefault(),
                                                     icdSpecialityCode = ICDSpecialityCode,
                                                     SurgeryDescription = IOgroup.Key,
                                                     OTNotesDescription = IOtrans.OTNotesDescription,
                                                     UserInputType = IOtrans.UserInputType,
                                                     InputValue = IOtrans.InputValue,
                                                     Status = IOt.IsActive,
                                                 }).ToList()


                           }).ToList();


                return new
                {
                    Success = true,
                    IOData = res
                };


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
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        }


        public Help GetRoomTransferDetails()
        {
            var RoomOccupied = WYNKContext.RoomOccupiedStatus.ToList();
            var RoomDetails = WYNKContext.RoomDetails.ToList();
            var Registrations = WYNKContext.Registration.ToList();
           
            var One = CMPSContext.OneLineMaster.ToList();
           
            var STS = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Booked").Select(x => x.OLMID).FirstOrDefault();
           
            var GetRoomTransferDetails = new Help();
            GetRoomTransferDetails.GetRoomTransfer = new List<GetRoomTransfer>();
            GetRoomTransferDetails.GetRoomTransfer = (from RD in RoomDetails.Where(x => x.IsRoomVacant == true && x.IsActive == true)
                                                      join RO in RoomOccupied.Where(x => x.Status == STS) on RD.ID equals RO.RoomDetailsID
                                                      join ON in One.Where(x => x.OLMID == STS) on RO.Status equals ON.OLMID
                                                      join REG in Registrations on RO.UIN equals REG.UIN
                                                      select new GetRoomTransfer
                                                      {
                                                          UIN = RO.UIN,
                                                          DateofBirth = REG.DateofBirth,
                                                          Age = DateTime.Now.Date.Year - REG.DateofBirth.Year,
                                                          PatientName = REG.Name,
                                                          Gender = REG.Gender,
                                                          FromRoomDate = RO.RoomOccupationFromDate,
                                                          FromRoomTime = RO.RoomOccupationFromTime,
                                                          Phone = REG.Phone,

                                                      }).ToList();
            return GetRoomTransferDetails;
        }



        public Help getPatientInsurance(DateTime FromDate, DateTime ToDate, string Insurancetype, int CMPID)
        {
            var getgetPatientInsuranceData = new Help();


            var Registration = WYNKContext.Registration.ToList();
            bool Insurancetype1 = false;

            if (Insurancetype == "Insurance") { Insurancetype1 = true; }
            else if (Insurancetype == "Private") { Insurancetype1 = false; }
            else if (Insurancetype == "Both") { }


            if (Insurancetype != "Both")
            {
                getgetPatientInsuranceData.getPatientInsurancedetail = (from RegInc in Registration.Where(x => x.DateofRegistration.Date >= FromDate.Date && x.DateofRegistration.Date <= ToDate.Date
                                            && x.CMPID == CMPID && x.IsDeleted == false && x.Insurance == Insurancetype1)

                                                                        select new getPatientInsurancedetail
                                                                        {
                                                                            UIN = RegInc.UIN,
                                                                            DateofRegistration = RegInc.DateofRegistration,
                                                                            Name = RegInc.Name,
                                                                            MName = RegInc.MiddleName,
                                                                            LName = RegInc.LastName,
                                                                            Age = ToAgeString(RegInc.DateofBirth),                                                                            
                                                                            Gender = RegInc.Gender,
                                                                            Address1 = RegInc.Address1,
                                                                            Phone = RegInc.Phone,


                                                                        }).ToList();
            }
            else
            {
                getgetPatientInsuranceData.getPatientInsurancedetail = (from RegInc in Registration.Where(x => x.DateofRegistration.Date >= FromDate.Date && x.DateofRegistration.Date <= ToDate.Date
                                                                         && x.CMPID == CMPID && x.IsDeleted == false)
                                                                        select new getPatientInsurancedetail
                                                                        {
                                                                            UIN = RegInc.UIN,
                                                                            DateofRegistration = RegInc.DateofRegistration,
                                                                            Name = RegInc.Name,
                                                                            Age = ToAgeString(RegInc.DateofBirth),                                                                            
                                                                            Gender = RegInc.Gender,
                                                                            Address1 = RegInc.Address1,
                                                                            Phone = RegInc.Phone,


                                                                        }).ToList();
            }





            return getgetPatientInsuranceData;
        }

      
        public Help getMedicalRegisters(DateTime FromDate, DateTime ToDate, int CMPID)
        {
            var getMedicalRegistersData = new Help();


            var MedicalBillMaster = WYNKContext.MedicalBillMaster.ToList();
            var MedicalBillTran = WYNKContext.MedicalBillTran.ToList();
            var Registration = WYNKContext.Registration.ToList();
         

            getMedicalRegistersData.getMedicalRegistersdetail = (from MEDBILL in MedicalBillMaster.
                                                                 Where(x => x.CreatedUTC.Date >= FromDate.Date && x.CreatedUTC.Date <= ToDate.Date
                                                                 && x.CMPID == CMPID)
                                                                 select new getMedicalRegistersdetail
                                                                 {
                                                                     BillNo = MEDBILL.BillNo,
                                                                     BillDate = MEDBILL.CreatedUTC,
                                                                     UIN = MEDBILL.UIN,
                                                                     Name = Registration.Where(x => x.UIN == MEDBILL.UIN).Select(x => x.Name).FirstOrDefault(),
                                                                     MName = Registration.Where(x => x.UIN == MEDBILL.UIN).Select(x => x.MiddleName).FirstOrDefault(),
                                                                     LName = Registration.Where(x => x.UIN == MEDBILL.UIN).Select(x => x.LastName).FirstOrDefault(),
                                                                     GrossProductValue = MEDBILL.GrossProductValue,
                                                                     TotalDiscountValue = MEDBILL.TotalDiscountValue,
                                                                     TotalTaxValue = MEDBILL.TotalTaxValue,
                                                                     TotalBillValue = MEDBILL.TotalBillValue,
                                                                 }).ToList();

            getMedicalRegistersData.getDrugdetail = (from MEDBILL in MedicalBillMaster.Where(x => x.CMPID == CMPID)
                                                     join MEDBILLT in MedicalBillTran on MEDBILL.ID equals MEDBILLT.MedicalBillID
                                                     select new getDrugdetail
                                                     {
                                                         DrugID = MEDBILLT.DrugID,
                                                         Quantity = MEDBILLT.Quantity,
                                                         UOM = MEDBILLT.UOM,
                                                         ItemRate = MEDBILLT.ItemRate,
                                                         ItemValue = MEDBILLT.ItemValue,
                                                         DiscountAmount = MEDBILLT.DiscountAmount,
                                                         GSTTaxValue = MEDBILLT.GSTTaxValue,
                                                         CESSValue = MEDBILLT.CESSValue,
                                                         AdditionalCESSValue = MEDBILLT.AdditionalCESSValue,                                                         
                                                     }).ToList();
            getMedicalRegistersData.getDrugdetail1 = (from MEDBILLT in getMedicalRegistersData.getDrugdetail.GroupBy(x => x.DrugID)

                                                      select new getDrugdetail1
                                                      {
                                                          DrugID = MEDBILLT.Key,
                                                          Quantity = MEDBILLT.Select(x => x.Quantity).Sum(),
                                                          UOM = MEDBILLT.Select(x => x.UOM).FirstOrDefault(),
                                                          ItemRate = MEDBILLT.Select(x => x.ItemRate).Sum(),
                                                          ItemValue = MEDBILLT.Select(x => x.ItemValue).Sum(),
                                                          DiscountAmount = MEDBILLT.Select(x => x.DiscountAmount).Sum(),
                                                          GSTTaxValue = MEDBILLT.Select(x => x.GSTTaxValue).Sum(),
                                                          CESSValue = MEDBILLT.Select(x => x.CESSValue).Sum(),
                                                          AdditionalCESSValue = MEDBILLT.Select(x => x.AdditionalCESSValue).Sum(),                                                          
                                                      }).ToList();




            return getMedicalRegistersData;
        }
        
        public decimal? getvalue(decimal? data)
        {
            var Zero = 0;
            var nulldata = data;
            if (nulldata != null)
            {
                return nulldata;
            }
            return Zero;
        }

        public Help getInvReg(string FromDate, string ToDate, int CMPID)
        {
            var getInvRegData = new Help();
            var OneLineMas = CMPSContext.OneLineMaster.ToList();            
            var InvPreTran = WYNKContext.InvestigationPrescriptionTran.ToList();
            var InvBillMaster = WYNKContext.InvestigationBillMaster.ToList();
            var InvBillTran = WYNKContext.InvestigationBillTran.ToList();
            var Reg = WYNKContext.Registration.ToList();
            var Tax = CMPSContext.TaxMaster.ToList();            
            var CompanyMaster = CMPSContext.Company.ToList();
           
            DateTime DT;
            var appdate = DateTime.TryParseExact(FromDate.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DT);
            {
                DT.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            }
            var fdate = DT;

            DateTime DT1;
            var appdate1 = DateTime.TryParseExact(ToDate.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DT1);
            {
                DT1.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            }
            var tdate = DT1;



            getInvRegData.getInvRegdetail = (from Invm in InvBillMaster.Where(x => x.CreatedUTC.Date >= fdate
                                             && x.CreatedUTC.Date <= tdate && x.CMPID == CMPID && x.BillNo != "0")
                                             join Invt in InvBillTran on Invm.ID equals Invt.InvestigationBillID
                                             orderby Invm.CreatedUTC
                                             select new getInvRegdetail
                                             {
                                                 InvoiceNo = Invm.BillNo,
                                                 InvoiceDate = Invm.CreatedUTC,
                                                 PatName = Reg.Where(x => x.UIN == Invm.UIN).Select(x => x.Name + x.MiddleName + x.LastName).FirstOrDefault(),
                                                 InvDescription = OneLineMas.Where(r => r.OLMID == InvPreTran.Where(x => x.ID == InvBillTran.Where(W => W.InvestigationBillID == Invm.ID).Select(q => q.InvPrescriptionTranId).FirstOrDefault()).Select(c => c.InvestigationID).FirstOrDefault()).Select(V => V.ParentDescription).FirstOrDefault(),
                                                 Amount = InvBillTran.Where(x => x.InvestigationBillID == Invm.ID).Select(x => x.Amount).FirstOrDefault(),
                                                 DiscountPercentage = getvalue(Invt.DiscountPercentage),
                                                 DiscountAmount = getvalue(Invt.DiscountAmount),
                                                 GrossAmount = getvalue(Invt.Amount),
                                                 GST = getvalue(Invt.GSTPercentage),
                                                 IGST = getvalue(Invt.IGSTPercentage),
                                                 CESS = getvalue(Invt.CESSPercentage),
                                                 AdditionalCESS = getvalue(Invt.AdditionalCESSPercentage),
                                                 TaxDescription = CMPSContext.TaxMaster.Where(x => x.ID == Invt.TaxID).Select(t => t.TaxDescription).FirstOrDefault(),
                                                 CESSDescription = CMPSContext.TaxMaster.Where(x => x.ID == Invt.TaxID).Select(t => t.CESSDescription).FirstOrDefault(),
                                                 AdditionalCESSDescription = CMPSContext.TaxMaster.Where(x => x.ID == Invt.TaxID).Select(t => t.AdditionalCESSDescription).FirstOrDefault(),
                                                 GSTAmount = getvalue(Invt.GSTTaxValue),
                                                 CESSAmount = getvalue(Invt.CESSAmount),
                                                 AddlCESSAmount = getvalue(Invt.AdditionalCESSAmount),
                                                 NetAmount = getvalue(Invm.TotalBillValue),
                                                 CompanyName = CompanyMaster.Where(x => x.CmpID == CMPID).Select(x => x.CompanyName).FirstOrDefault(),
                                               
                                             }).ToList();
            getInvRegData.TRate = getInvRegData.getInvRegdetail.Select(x => x.Amount).Sum();
            getInvRegData.TTAXAMOUNT = getInvRegData.getInvRegdetail.Select(x => x.GSTAmount + x.CESSAmount + x.AddlCESSAmount).Sum();
            getInvRegData.DiscountAmtsum = getInvRegData.getInvRegdetail.Select(x => x.DiscountAmount).Sum();
            getInvRegData.GrossAmountsum = getInvRegData.getInvRegdetail.Select(x => x.GrossAmount).Sum();
            getInvRegData.TotalAmountsum = getInvRegData.getInvRegdetail.Select(x => x.NetAmount).Sum();


            getInvRegData.getInvBillRegTaxsummary = (from Invm in InvBillMaster.Where(x => Convert.ToDateTime(x.CreatedUTC).Date >= Convert.ToDateTime(fdate)
                                                    && Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(tdate) && x.CMPID == CMPID)
                                                     join Invt in InvBillTran on Invm.ID equals Invt.InvestigationBillID
                                                     group Invt by Invt.TaxID into g

                                                     select new getInvBillRegtaxsummary
                                                     {
                                                         TaxID = g.Key,
                                                         TaxDescription = Tax.Where(x => x.ID == g.Select(s => s.TaxID).FirstOrDefault()).Select(x => x.TaxDescription).FirstOrDefault(),

                                                         CESSDescription = Tax.Where(x => x.ID == g.Select(s => s.TaxID).FirstOrDefault()).Select(x => x.CESSDescription).FirstOrDefault(),
                                                         AdditionalCESSDescription = Tax.Where(x => x.ID == g.Select(s => s.TaxID).FirstOrDefault()).Select(x => x.AdditionalCESSDescription).FirstOrDefault(),


                                                         TotalProductValue = g.Select(x => (x.Amount + x.GSTTaxValue + x.CESSAmount + x.AdditionalCESSAmount) - (x.DiscountAmount)).Sum(),
                                                         GSTTaxValue = g.Select(x => x.GSTTaxValue).Sum(),
                                                         CESSAmount = g.Select(x => x.CESSAmount).Sum(),
                                                         AddCESSAmount = g.Select(x => x.AdditionalCESSAmount).Sum(),
                                                         TaxPayable = g.Select(x => x.GSTTaxValue + x.CESSAmount + x.AdditionalCESSAmount).Sum(),
                                                         TaxableTurnover = g.Select(x => (x.Amount) - (x.DiscountAmount)).Sum(),
                                                     }).ToList();

            getInvRegData.TAXTotalProductValue = getInvRegData.getInvBillRegTaxsummary.Select(x => x.TotalProductValue).Sum();
            getInvRegData.TAXCESSAmount = getInvRegData.getInvBillRegTaxsummary.Select(x => x.CESSAmount).Sum();
            getInvRegData.TAXAddCESSAmount = getInvRegData.getInvBillRegTaxsummary.Select(x => x.AddCESSAmount).Sum();
            getInvRegData.TAXTaxPayable = getInvRegData.getInvBillRegTaxsummary.Select(x => x.TaxPayable).Sum();
            getInvRegData.TAXGSTTaxValue = getInvRegData.getInvBillRegTaxsummary.Select(x => x.GSTTaxValue).Sum();
            getInvRegData.TAXTaxableTurnover = getInvRegData.getInvBillRegTaxsummary.Select(x => x.TaxableTurnover).Sum();

            return getInvRegData;
        }



        public Help getCampSurgerypatients(int CampID, DateTime FromDate, DateTime ToDate, int CMPID)
        {

            var getCampSurgerypatients = new Help();
            var Cmpregistration = WYNKContext.CampRegistration.Where(x=>x.CMPID == CMPID).AsNoTracking().ToList();
        
            var CAMP = WYNKContext.CAMP.AsNoTracking().ToList();
            var ICDSP = WYNKContext.ICDSpecialityCode.AsNoTracking().ToList();
            var fdate = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd");
            var tdate = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd");
         
            if (CampID == 0) 
            {
                getCampSurgerypatients.CampSurgerypatients = (from REG in Cmpregistration.Where(x => Convert.ToDateTime(x.CreatedUTC).Date >= Convert.ToDateTime(fdate)
                                                    && Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(tdate) && x.CMPID == CMPID && x.IsDeleted == false && x.RegType == "Camp-Surgery")
                                                   
                                                       orderby REG.CreatedUTC
                                                     select new CampSurgerypatients
                                                     {
                                                         UIN = REG.CampUIN,
                                                         RegType = REG.TreatmentAdvice,
                                                         DateofRegistration = TimeZoneInfo.ConvertTimeFromUtc(REG.DateofRegistration, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")),
                                                         Name = REG.Name + " " + REG.MiddleName + " " + REG.LastName,
                                                         DateofBirth = REG.DateofBirth,
                                                         Age = ToAgeString(REG.DateofBirth),
                                                         Gender = REG.Gender,
                                                         MiddleName = REG.MiddleName,
                                                         LastName = REG.LastName,
                                                         CampName = CAMP.Where(x => x.CampID == REG.CampID).Select(c => c.CampName).FirstOrDefault(),
                                                         ICDSpeciality= ICDSP.Where(x=>x.ID== REG.ICDSpecialityId).Select(v=>v.SpecialityDescription).FirstOrDefault(),

                                                     }).ToList();
                 getCampSurgerypatients.CampwiseSurgeryCount = (from REG in Cmpregistration.Where(x => Convert.ToDateTime(x.CreatedUTC).Date >= Convert.ToDateTime(fdate)
                                              && Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(tdate) && x.CMPID == CMPID && x.IsDeleted == false &&  x.RegType == "Camp-Surgery")
                                          
                                                orderby REG.CreatedUTC
                                              group REG by new
                                              {
                                                  CampName = REG.CampID,
                                                  ICDSpeciality = ICDSP.Where(x => x.ID == REG.ICDSpecialityId).Select(v => v.SpecialityDescription).FirstOrDefault(),
                                                  Surgerycount = REG.ICDSpecialityId
                                              } into grp
                                              select new CampwiseSurgeryCount
                                              {
                                                  CampName = CAMP.Where(x => x.CampID == grp.Key.CampName).Select(c=>c.CampName).FirstOrDefault(),
                                                  ICDSpeciality = grp.Key.ICDSpeciality,
                                                  Surgerycount = Cmpregistration.Where(x => x.ICDSpecialityId == grp.Key.Surgerycount && x.CampID == grp.Key.CampName && x.RegType == "Camp-Surgery" && Convert.ToDateTime(x.CreatedUTC).Date >= Convert.ToDateTime(fdate) && Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(tdate) && x.IsDeleted == false).Count(),
                                              }).ToList();
            } 
            else {
                getCampSurgerypatients.CampSurgerypatients = (from REG in Cmpregistration.Where(x => Convert.ToDateTime(x.CreatedUTC).Date >= Convert.ToDateTime(fdate)
                                                           && Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(tdate) && x.CMPID == CMPID && x.IsDeleted == false && x.RegType == "Camp-Surgery" && x.CampID == CampID)
                                                        
                                                            orderby REG.CreatedUTC
                                                     select new CampSurgerypatients
                                                     {
                                                         UIN = REG.CampUIN,
                                                         RegType = REG.TreatmentAdvice,
                                                         DateofRegistration = TimeZoneInfo.ConvertTimeFromUtc(REG.DateofRegistration, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")),
                                                         Name = REG.Name + " " + REG.MiddleName + " " + REG.LastName,
                                                         DateofBirth = REG.DateofBirth,
                                                         Age = ToAgeString(REG.DateofBirth),
                                                         Gender = REG.Gender,
                                                         MiddleName = REG.MiddleName,
                                                         LastName = REG.LastName,
                                                         CampName = CAMP.Where(x => x.CampID == REG.CampID).Select(c => c.CampName).FirstOrDefault(),
                                                         ICDSpeciality = ICDSP.Where(x => x.ID == REG.ICDSpecialityId).Select(v => v.SpecialityDescription).FirstOrDefault(),
                                                     }).ToList();

                getCampSurgerypatients.CampwiseSurgeryCount = (from REG in Cmpregistration.Where(x => Convert.ToDateTime(x.CreatedUTC).Date >= Convert.ToDateTime(fdate)
                                                && Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(tdate) && x.CMPID == CMPID && x.IsDeleted == false && x.RegType == "Camp-Surgery" && x.CampID == CampID)
                                                             
                                                               orderby REG.CreatedUTC
                                                               group REG by new
                                                               {
                                                                   CampName = REG.CampID,
                                                                   ICDSpeciality = ICDSP.Where(x => x.ID == REG.ICDSpecialityId).Select(v => v.SpecialityDescription).FirstOrDefault(),
                                                                   Surgerycount = REG.ICDSpecialityId
                                                               } into grp
                                                               select new CampwiseSurgeryCount
                                                               {
                                                                   CampName = CAMP.Where(x => x.CampID == grp.Key.CampName).Select(c => c.CampName).FirstOrDefault(),
                                                                   ICDSpeciality = grp.Key.ICDSpeciality,
                                                                   Surgerycount = Cmpregistration.Where(x => x.ICDSpecialityId == grp.Key.Surgerycount && x.CampID == grp.Key.CampName && x.RegType == "Camp-Surgery" && Convert.ToDateTime(x.CreatedUTC).Date >= Convert.ToDateTime(fdate) && Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(tdate) && x.IsDeleted == false).Count(),
                                                               }).ToList();

            }

            return getCampSurgerypatients;
        }




        public Help getCampScreenedpatients(int CampID, DateTime FromDate, DateTime ToDate, int CMPID)
        {

            var getCampScreenedpatients = new Help();
            var Cmpregistration = WYNKContext.CampRegistration.Where(x => x.CMPID == CMPID).AsNoTracking().ToList();          
            var CAMP = WYNKContext.CAMP.AsNoTracking().ToList();
            var fdate = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd");
            var tdate = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd");

            if (CampID == 0)
            {
                getCampScreenedpatients.Regdetail = (from REG in Cmpregistration.Where(x => Convert.ToDateTime(x.CreatedUTC).Date >= Convert.ToDateTime(fdate)
                                                    && Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(tdate) && x.CMPID == CMPID && x.IsDeleted == false)
                                                   
                                                     orderby REG.CreatedUTC
                                                     select new Regdetail
                                                     {
                                                         UIN = REG.CampUIN,
                                                         RegType = REG.TreatmentAdvice,
                                                         DateofRegistration = TimeZoneInfo.ConvertTimeFromUtc(REG.DateofRegistration, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")),
                                                         Name = REG.Name + " " + REG.MiddleName + " " + REG.LastName,
                                                         DateofBirth = REG.DateofBirth,
                                                         Age = ToAgeString(REG.DateofBirth),
                                                         Gender = REG.Gender,
                                                         Address1 = REG.Address1,
                                                         Address2 = REG.Address2,
                                                         Address3 = REG.Address3,
                                                         EmailID = REG.EmailID,
                                                         Phone = REG.Phone,                                                     
                                                         MiddleName = REG.MiddleName,
                                                         LastName = REG.LastName,
                                                         CampName = CAMP.Where(x => x.CampID == REG.CampID).Select(c => c.CampName).FirstOrDefault(),
                                                     }).ToList();


               var Campname = (from REG in Cmpregistration.Where(x => Convert.ToDateTime(x.CreatedUTC).Date >= Convert.ToDateTime(fdate) && Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(tdate) && x.CMPID == CMPID && x.IsDeleted == false)
                         
                               orderby REG.CreatedUTC
                                                                select new 
                                                                {
                                                                    CampName = CAMP.Where(x => x.CampID == REG.CampID).Select(c => c.CampName).FirstOrDefault(),
                                                                    Male = Cmpregistration.Where(x => x.Gender == "Male" && x.CampID == REG.CampID && x.CMPID==CMPID && Convert.ToDateTime(x.CreatedUTC).Date >= Convert.ToDateTime(fdate) && Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(tdate)  && x.IsDeleted == false).Select(e => e.Gender).Count(),
                                                                    Female = Cmpregistration.Where(x => x.Gender == "Female" && x.CampID == REG.CampID && x.CMPID == CMPID && Convert.ToDateTime(x.CreatedUTC).Date >= Convert.ToDateTime(fdate) && Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(tdate) && x.IsDeleted == false).Select(e => e.Gender).Count(),
                                                                    Transgender = Cmpregistration.Where(x => x.Gender == "Transgender" && x.CampID == REG.CampID && x.CMPID == CMPID && Convert.ToDateTime(x.CreatedUTC).Date >= Convert.ToDateTime(fdate) && Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(tdate) && x.IsDeleted == false).Select(e => e.Gender).Count(),
                                                                }).ToList();



                getCampScreenedpatients.CampwiseSummaryCount = (from REG in Campname.GroupBy(x=>x.CampName)
                                                                select new CampwiseSummaryCount
                                                                {
                                                                    CampName = REG.Key,
                                                                    Male = REG.Select(x=>x.Male).FirstOrDefault(),
                                                                    Female = REG.Select(x => x.Female).FirstOrDefault(),
                                                                    Transgender = REG.Select(x => x.Transgender).FirstOrDefault(),
                                                                }).ToList();
            } 
            else
            {
                getCampScreenedpatients.Regdetail = (from REG in Cmpregistration.Where(x => Convert.ToDateTime(x.CreatedUTC).Date >= Convert.ToDateTime(fdate)
                                          && Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(tdate) && x.CMPID == CMPID && x.IsDeleted == false && x.CampID == CampID)
                                                
                                                     orderby REG.CreatedUTC
                                                     select new Regdetail
                                                     {
                                                         UIN = REG.CampUIN,
                                                         RegType = REG.TreatmentAdvice,
                                                         DateofRegistration = TimeZoneInfo.ConvertTimeFromUtc(REG.DateofRegistration, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")),
                                                         Name = REG.Name + " " + REG.MiddleName + " " + REG.LastName,
                                                         DateofBirth = REG.DateofBirth,
                                                         Age = ToAgeString(REG.DateofBirth),
                                                         Gender = REG.Gender,
                                                         Address1 = REG.Address1,
                                                         Address2 = REG.Address2,
                                                         Address3 = REG.Address3,
                                                         EmailID = REG.EmailID,
                                                         Phone = REG.Phone,                                                       
                                                         MiddleName = REG.MiddleName,
                                                         LastName = REG.LastName,
                                                         CampName = CAMP.Where(x => x.CampID == REG.CampID).Select(c => c.CampName).FirstOrDefault(),
                                                     }).ToList();



                var Campname1 = (from REG in Cmpregistration.Where(x => Convert.ToDateTime(x.CreatedUTC).Date >= Convert.ToDateTime(fdate)
                                     && Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(tdate) && x.CMPID == CMPID && x.IsDeleted == false && x.CampID == CampID)                                
                                 orderby REG.CreatedUTC
                                select new
                                {
                                    CampName = CAMP.Where(x => x.CampID == REG.CampID).Select(c => c.CampName).FirstOrDefault(),
                                    Male = Cmpregistration.Where(x => x.Gender == "Male" && x.CampID == REG.CampID && x.CMPID == CMPID && Convert.ToDateTime(x.CreatedUTC).Date >= Convert.ToDateTime(fdate) && Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(tdate) && x.IsDeleted == false).Select(e => e.Gender).Count(),
                                    Female = Cmpregistration.Where(x => x.Gender == "Female" && x.CampID == REG.CampID && x.CMPID == CMPID && Convert.ToDateTime(x.CreatedUTC).Date >= Convert.ToDateTime(fdate) && Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(tdate) && x.IsDeleted == false).Select(e => e.Gender).Count(),
                                    Transgender = Cmpregistration.Where(x => x.Gender == "Transgender" && x.CampID == REG.CampID && x.CMPID == CMPID && Convert.ToDateTime(x.CreatedUTC).Date >= Convert.ToDateTime(fdate) && Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(tdate) && x.IsDeleted == false).Select(e => e.Gender).Count(),
                                }).ToList();



                getCampScreenedpatients.CampwiseSummaryCount = (from REG in Campname1.GroupBy(x => x.CampName)
                                                                select new CampwiseSummaryCount
                                                                {
                                                                    CampName = REG.Key,
                                                                    Male = REG.Select(x => x.Male).FirstOrDefault(),
                                                                    Female = REG.Select(x => x.Female).FirstOrDefault(),
                                                                    Transgender = REG.Select(x => x.Transgender).FirstOrDefault(),
                                                                }).ToList();





            }


            return getCampScreenedpatients;
        }





        public Help getCampSurgeryunderwentpatients(int CampID, DateTime FromDate, DateTime ToDate, int CMPID)
        {

            var getCampSurgeryunderwentpatients = new Help();
            var Cmpregistration = WYNKContext.CampRegistration.Where(x => x.CMPID == CMPID).AsNoTracking().ToList();           
            var CAMP = WYNKContext.CAMP.AsNoTracking().ToList();
            var ICDSP = WYNKContext.ICDSpecialityCode.AsNoTracking().ToList();
            var fdate = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd");
            var tdate = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd");
        
            if (CampID == 0)
            {
                getCampSurgeryunderwentpatients.CampSurgeryunderwentpatients = (from REG in Cmpregistration.Where(x => Convert.ToDateTime(x.CreatedUTC).Date >= Convert.ToDateTime(fdate)
                                                    && Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(tdate) && x.CMPID == CMPID && x.IsDeleted == false && x.IsSurgeryDone == true)
                                                          
                                                                orderby REG.CreatedUTC
                                                              select new CampSurgeryunderwentpatients
                                                              {
                                                                  UIN = REG.CampUIN,
                                                                  RegType = REG.TreatmentAdvice,
                                                                  DateofRegistration = TimeZoneInfo.ConvertTimeFromUtc(REG.DateofRegistration, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")),
                                                                  Name = REG.Name + " " + REG.MiddleName + " " + REG.LastName,
                                                                  DateofBirth = REG.DateofBirth,
                                                                  Age = ToAgeString(REG.DateofBirth),
                                                                  Gender = REG.Gender,
                                                                  MiddleName = REG.MiddleName,
                                                                  LastName = REG.LastName,
                                                                  CampName = CAMP.Where(x => x.CampID == REG.CampID).Select(c => c.CampName).FirstOrDefault(),
                                                                  ICDSpeciality = ICDSP.Where(x => x.ID == REG.ICDSpecialityId).Select(v => v.SpecialityDescription).FirstOrDefault(),

                                                              }).ToList();
                getCampSurgeryunderwentpatients.CampwiseSurgeryunderwentCount = (from REG in Cmpregistration.Where(x => Convert.ToDateTime(x.CreatedUTC).Date >= Convert.ToDateTime(fdate)
                                                             && Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(tdate) && x.CMPID == CMPID && x.IsDeleted == false && x.IsSurgeryDone == true)
                                                            
                                                               orderby REG.CreatedUTC
                                                               group REG by new
                                                               {
                                                                   CampName = REG.CampID,
                                                                   ICDSpeciality = ICDSP.Where(x => x.ID == REG.ICDSpecialityId).Select(v => v.SpecialityDescription).FirstOrDefault(),
                                                                   Surgerycount = REG.ICDSpecialityId
                                                               } into grp
                                                               select new CampwiseSurgeryunderwentCount
                                                               {
                                                                   CampName = CAMP.Where(x => x.CampID == grp.Key.CampName).Select(c => c.CampName).FirstOrDefault(),
                                                                   ICDSpeciality = grp.Key.ICDSpeciality,
                                                                   Surgerycount = Cmpregistration.Where(x => x.ICDSpecialityId == grp.Key.Surgerycount && x.CampID == grp.Key.CampName && Convert.ToDateTime(x.CreatedUTC).Date >= Convert.ToDateTime(fdate) && Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(tdate) && x.IsDeleted == false).Count(),
                                                               }).ToList();
            }
            else
            {
                getCampSurgeryunderwentpatients.CampSurgeryunderwentpatients = (from REG in Cmpregistration.Where(x => Convert.ToDateTime(x.CreatedUTC).Date >= Convert.ToDateTime(fdate)
                                          && Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(tdate) && x.CMPID == CMPID && x.IsDeleted == false && x.IsSurgeryDone == true && x.CampID == CampID)
                                                           
                                                                                orderby REG.CreatedUTC
                                                              select new CampSurgeryunderwentpatients
                                                              {
                                                                  UIN = REG.CampUIN,
                                                                  RegType = REG.TreatmentAdvice,
                                                                  DateofRegistration = TimeZoneInfo.ConvertTimeFromUtc(REG.DateofRegistration, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")),
                                                                  Name = REG.Name + " " + REG.MiddleName + " " + REG.LastName,
                                                                  DateofBirth = REG.DateofBirth,
                                                                  Age = ToAgeString(REG.DateofBirth),
                                                                  Gender = REG.Gender,
                                                                  MiddleName = REG.MiddleName,
                                                                  LastName = REG.LastName,
                                                                  CampName = CAMP.Where(x => x.CampID == REG.CampID).Select(c => c.CampName).FirstOrDefault(),
                                                                  ICDSpeciality = ICDSP.Where(x => x.ID == REG.ICDSpecialityId).Select(v => v.SpecialityDescription).FirstOrDefault(),
                                                              }).ToList();

                getCampSurgeryunderwentpatients.CampwiseSurgeryunderwentCount = (from REG in Cmpregistration.Where(x => Convert.ToDateTime(x.CreatedUTC).Date >= Convert.ToDateTime(fdate)
                                                && Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(tdate) && x.CMPID == CMPID && x.IsDeleted == false && x.IsSurgeryDone == true && x.CampID == CampID)
                                                           
                                                                                 orderby REG.CreatedUTC
                                                               group REG by new
                                                               {
                                                                   CampName = REG.CampID,
                                                                   ICDSpeciality = ICDSP.Where(x => x.ID == REG.ICDSpecialityId).Select(v => v.SpecialityDescription).FirstOrDefault(),
                                                                   Surgerycount = REG.ICDSpecialityId
                                                               } into grp
                                                               select new CampwiseSurgeryunderwentCount
                                                               {
                                                                   CampName = CAMP.Where(x => x.CampID == grp.Key.CampName).Select(c => c.CampName).FirstOrDefault(),
                                                                   ICDSpeciality = grp.Key.ICDSpeciality,
                                                                   Surgerycount = Cmpregistration.Where(x => x.ICDSpecialityId == grp.Key.Surgerycount && x.CampID == grp.Key.CampName && Convert.ToDateTime(x.CreatedUTC).Date >= Convert.ToDateTime(fdate) && Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(tdate) && x.IsDeleted == false).Count(),
                                                               }).ToList();

            }













            return getCampSurgeryunderwentpatients;
        }


        public Help GetPendingCampSurgeryadvisedPatient(int CampID, DateTime FromDate, DateTime ToDate, int CMPID)
        {

            var GetPendingCampSurgeryadvisedPatient = new Help();
            var Cmpregistration = WYNKContext.CampRegistration.Where(x => x.CMPID == CMPID).AsNoTracking().ToList();           
            var CAMP = WYNKContext.CAMP.AsNoTracking().ToList();
            var ICDSP = WYNKContext.ICDSpecialityCode.AsNoTracking().ToList();
            var fdate = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd");
            var tdate = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd");
        
            if (CampID == 0)
            {
                GetPendingCampSurgeryadvisedPatient.CampSurgeryunderwentpatients = (from REG in Cmpregistration.Where(x => Convert.ToDateTime(x.CreatedUTC).Date >= Convert.ToDateTime(fdate)
                                                    && Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(tdate) && x.CMPID == CMPID && x.IsDeleted == false && x.IsVisited == false && x.RegType == "Camp-Surgery")
                                                                             
                                                                                    orderby REG.CreatedUTC
                                                                                    select new CampSurgeryunderwentpatients
                                                                                    {
                                                                                        UIN = REG.CampUIN,
                                                                                        RegType = REG.TreatmentAdvice,
                                                                                        DateofRegistration = TimeZoneInfo.ConvertTimeFromUtc(REG.DateofRegistration, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")),
                                                                                        Name = REG.Name + " " + REG.MiddleName + " " + REG.LastName,
                                                                                        DateofBirth = REG.DateofBirth,
                                                                                        Age = ToAgeString(REG.DateofBirth),
                                                                                        Gender = REG.Gender,
                                                                                        MiddleName = REG.MiddleName,
                                                                                        LastName = REG.LastName,
                                                                                        CampName = CAMP.Where(x => x.CampID == REG.CampID).Select(c => c.CampName).FirstOrDefault(),
                                                                                        ICDSpeciality = ICDSP.Where(x => x.ID == REG.ICDSpecialityId).Select(v => v.SpecialityDescription).FirstOrDefault(),

                                                                                    }).ToList();
                GetPendingCampSurgeryadvisedPatient.CampwiseSurgeryunderwentCount = (from REG in Cmpregistration.Where(x => Convert.ToDateTime(x.CreatedUTC).Date >= Convert.ToDateTime(fdate)
                                                             && Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(tdate) && x.CMPID == CMPID && x.IsDeleted == false && x.IsVisited == false && x.RegType == "Camp-Surgery")                                                                              
                                                                                     orderby REG.CreatedUTC
                                                                                     group REG by new
                                                                                     {
                                                                                         CampName = REG.CampID,
                                                                                         ICDSpeciality = ICDSP.Where(x => x.ID == REG.ICDSpecialityId).Select(v => v.SpecialityDescription).FirstOrDefault(),
                                                                                         Surgerycount = REG.ICDSpecialityId
                                                                                     } into grp
                                                                                     select new CampwiseSurgeryunderwentCount
                                                                                     {
                                                                                         CampName = CAMP.Where(x => x.CampID == grp.Key.CampName).Select(c => c.CampName).FirstOrDefault(),
                                                                                         ICDSpeciality = grp.Key.ICDSpeciality,
                                                                                         Surgerycount = Cmpregistration.Where(x => x.ICDSpecialityId == grp.Key.Surgerycount && x.CampID == grp.Key.CampName && x.RegType == "Camp-Surgery" && Convert.ToDateTime(x.CreatedUTC).Date >= Convert.ToDateTime(fdate) && Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(tdate) && x.IsDeleted == false).Count(),
                                                                                     }).ToList();
            }
            else
            {
                GetPendingCampSurgeryadvisedPatient.CampSurgeryunderwentpatients = (from REG in Cmpregistration.Where(x => Convert.ToDateTime(x.CreatedUTC).Date >= Convert.ToDateTime(fdate)
                                          && Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(tdate) && x.CMPID == CMPID && x.IsDeleted == false && x.IsVisited == false && x.CampID == CampID && x.RegType == "Camp-Surgery")
                                                                                
                                                                                    orderby REG.CreatedUTC
                                                                                    select new CampSurgeryunderwentpatients
                                                                                    {
                                                                                        UIN = REG.CampUIN,
                                                                                        RegType = REG.TreatmentAdvice,
                                                                                        DateofRegistration = TimeZoneInfo.ConvertTimeFromUtc(REG.DateofRegistration, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")),
                                                                                        Name = REG.Name + " " + REG.MiddleName + " " + REG.LastName,
                                                                                        DateofBirth = REG.DateofBirth,
                                                                                        Age = ToAgeString(REG.DateofBirth),
                                                                                        Gender = REG.Gender,
                                                                                        MiddleName = REG.MiddleName,
                                                                                        LastName = REG.LastName,
                                                                                        CampName = CAMP.Where(x => x.CampID == REG.CampID).Select(c => c.CampName).FirstOrDefault(),
                                                                                        ICDSpeciality = ICDSP.Where(x => x.ID == REG.ICDSpecialityId).Select(v => v.SpecialityDescription).FirstOrDefault(),
                                                                                    }).ToList();

                GetPendingCampSurgeryadvisedPatient.CampwiseSurgeryunderwentCount = (from REG in Cmpregistration.Where(x => Convert.ToDateTime(x.CreatedUTC).Date >= Convert.ToDateTime(fdate)
                                                && Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(tdate) && x.CMPID == CMPID && x.IsDeleted == false && x.IsVisited == false && x.CampID == CampID && x.RegType == "Camp-Surgery")
                                                                              
                                                                                     orderby REG.CreatedUTC
                                                                                     group REG by new
                                                                                     {
                                                                                         CampName = REG.CampID,
                                                                                         ICDSpeciality = ICDSP.Where(x => x.ID == REG.ICDSpecialityId).Select(v => v.SpecialityDescription).FirstOrDefault(),
                                                                                         Surgerycount = REG.ICDSpecialityId
                                                                                     } into grp
                                                                                     select new CampwiseSurgeryunderwentCount
                                                                                     {
                                                                                         CampName = CAMP.Where(x => x.CampID == grp.Key.CampName).Select(c => c.CampName).FirstOrDefault(),
                                                                                         ICDSpeciality = grp.Key.ICDSpeciality,
                                                                                         Surgerycount = Cmpregistration.Where(x => x.ICDSpecialityId == grp.Key.Surgerycount && x.CampID == grp.Key.CampName && x.RegType == "Camp-Surgery" && Convert.ToDateTime(x.CreatedUTC).Date >= Convert.ToDateTime(fdate) && Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(tdate) && x.IsDeleted == false).Count(),
                                                                                     }).ToList();

            }


            return GetPendingCampSurgeryadvisedPatient;
        }


        public Help getCamptoHospitalvisitedpatients(int CampID, DateTime FromDate, DateTime ToDate, int CMPID)
        {

            var getCamptoHospitalvisitedpatients = new Help();
            var Cmpregistration = WYNKContext.CampRegistration.Where(x => x.CMPID == CMPID).AsNoTracking().ToList();            
            var CAMP = WYNKContext.CAMP.AsNoTracking().ToList();
            var ICDSP = WYNKContext.ICDSpecialityCode.AsNoTracking().ToList();
            var fdate = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd");
            var tdate = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd");
      
            if (CampID == 0)
            {
                getCamptoHospitalvisitedpatients.CampSurgeryunderwentpatients = (from REG in Cmpregistration.Where(x => Convert.ToDateTime(x.CreatedUTC).Date >= Convert.ToDateTime(fdate)
                                                    && Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(tdate) && x.CMPID == CMPID && x.IsDeleted == false && x.IsVisited == true)
                                                                               
                                                                                 orderby REG.CreatedUTC
                                                                                 select new CampSurgeryunderwentpatients
                                                                                 {
                                                                                     UIN = REG.CampUIN,
                                                                                     RegType = REG.TreatmentAdvice,
                                                                                     DateofRegistration = TimeZoneInfo.ConvertTimeFromUtc(REG.DateofRegistration, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")),
                                                                                     Name = REG.Name + " " + REG.MiddleName + " " + REG.LastName,
                                                                                     DateofBirth = REG.DateofBirth,
                                                                                     Age = ToAgeString(REG.DateofBirth),
                                                                                     Gender = REG.Gender,
                                                                                     MiddleName = REG.MiddleName,
                                                                                     LastName = REG.LastName,
                                                                                     CampName = CAMP.Where(x => x.CampID == REG.CampID).Select(c => c.CampName).FirstOrDefault(),
                                                                                     ICDSpeciality = ICDSP.Where(x => x.ID == REG.ICDSpecialityId).Select(v => v.SpecialityDescription).FirstOrDefault(),

                                                                                 }).ToList();
                getCamptoHospitalvisitedpatients.CampwiseSurgeryunderwentCount = (from REG in Cmpregistration.Where(x => Convert.ToDateTime(x.CreatedUTC).Date >= Convert.ToDateTime(fdate)
                                                             && Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(tdate) && x.CMPID == CMPID && x.IsDeleted == false && x.IsVisited == true && x.RegType == "Camp-Surgery")
                                                                              
                                                                                  orderby REG.CreatedUTC
                                                                                  group REG by new
                                                                                  {
                                                                                      CampName = REG.CampID,
                                                                                      ICDSpeciality = ICDSP.Where(x => x.ID == REG.ICDSpecialityId).Select(v => v.SpecialityDescription).FirstOrDefault(),
                                                                                      Surgerycount = REG.ICDSpecialityId
                                                                                  } into grp
                                                                                  select new CampwiseSurgeryunderwentCount
                                                                                  {
                                                                                      CampName = CAMP.Where(x => x.CampID == grp.Key.CampName).Select(c => c.CampName).FirstOrDefault(),
                                                                                      ICDSpeciality = grp.Key.ICDSpeciality,
                                                                                      Surgerycount = Cmpregistration.Where(x => x.ICDSpecialityId == grp.Key.Surgerycount && x.CampID == grp.Key.CampName && x.RegType == "Camp-Surgery" && Convert.ToDateTime(x.CreatedUTC).Date >= Convert.ToDateTime(fdate) && Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(tdate) && x.IsDeleted == false).Count(),
                                                                                  }).ToList();
            }
            else
            {
                getCamptoHospitalvisitedpatients.CampSurgeryunderwentpatients = (from REG in Cmpregistration.Where(x => Convert.ToDateTime(x.CreatedUTC).Date >= Convert.ToDateTime(fdate)
                                          && Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(tdate) && x.CMPID == CMPID && x.IsDeleted == false && x.IsVisited == true && x.CampID == CampID)
                                                                          
                                                                                 orderby REG.CreatedUTC
                                                                                 select new CampSurgeryunderwentpatients
                                                                                 {
                                                                                     UIN = REG.CampUIN,
                                                                                     RegType = REG.TreatmentAdvice,
                                                                                     DateofRegistration = TimeZoneInfo.ConvertTimeFromUtc(REG.DateofRegistration, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")),
                                                                                     Name = REG.Name + " " + REG.MiddleName + " " + REG.LastName,
                                                                                     DateofBirth = REG.DateofBirth,
                                                                                     Age = ToAgeString(REG.DateofBirth),
                                                                                     Gender = REG.Gender,
                                                                                     MiddleName = REG.MiddleName,
                                                                                     LastName = REG.LastName,
                                                                                     CampName = CAMP.Where(x => x.CampID == REG.CampID).Select(c => c.CampName).FirstOrDefault(),
                                                                                     ICDSpeciality = ICDSP.Where(x => x.ID == REG.ICDSpecialityId).Select(v => v.SpecialityDescription).FirstOrDefault(),
                                                                                 }).ToList();

                getCamptoHospitalvisitedpatients.CampwiseSurgeryunderwentCount = (from REG in Cmpregistration.Where(x => Convert.ToDateTime(x.CreatedUTC).Date >= Convert.ToDateTime(fdate)
                                                && Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(tdate) && x.CMPID == CMPID && x.IsDeleted == false && x.IsVisited == true && x.CampID == CampID && x.RegType == "Camp-Surgery")
                                                                            
                                                                                  orderby REG.CreatedUTC
                                                                                  group REG by new
                                                                                  {
                                                                                      CampName = REG.CampID,
                                                                                      ICDSpeciality = ICDSP.Where(x => x.ID == REG.ICDSpecialityId).Select(v => v.SpecialityDescription).FirstOrDefault(),
                                                                                      Surgerycount = REG.ICDSpecialityId
                                                                                  } into grp
                                                                                  select new CampwiseSurgeryunderwentCount
                                                                                  {
                                                                                      CampName = CAMP.Where(x => x.CampID == grp.Key.CampName).Select(c => c.CampName).FirstOrDefault(),
                                                                                      ICDSpeciality = grp.Key.ICDSpeciality,
                                                                                      Surgerycount = Cmpregistration.Where(x => x.ICDSpecialityId == grp.Key.Surgerycount && x.CampID == grp.Key.CampName && x.RegType == "Camp-Surgery" && Convert.ToDateTime(x.CreatedUTC).Date >= Convert.ToDateTime(fdate) && Convert.ToDateTime(x.CreatedUTC).Date <= Convert.ToDateTime(tdate) && x.IsDeleted == false).Count(),
                                                                                  }).ToList();

            }


            return getCamptoHospitalvisitedpatients;
        }
        public Help GetConsultationSummary(DateTime Fromdate, DateTime Todate, int Cmpid)
        {

            var Regtran = WYNKContext.RegistrationTran.ToList();
            var cosumary = WYNKContext.ConsultantionSummary.ToList();

            var consum = new Help();

            var patlist = (from RT in Regtran.Where(x => x.DateofVisit.Date >= Fromdate.Date && x.DateofVisit.Date <= Todate.Date && x.CmpID == Cmpid && x.ConsulationFees != null)
                           select new
                           {
                               uin = RT.UIN,
                               date = RT.DateofVisit.Date,
                               amnt = RT.ConsulationFees,

                           }).ToList();

            consum.Copat = (from res in patlist.GroupBy(x => x.date)
                            select new Copat
                            {
                                NoOfPat = res.Count(),
                                Date = res.Select(x => x.date).FirstOrDefault(),
                                Charges = res.Sum(x => x.amnt),

                            }).ToList();

            consum.CAddress = CMPSContext.Company.Where(x => x.CmpID == Cmpid).Select(x => x.Address1).FirstOrDefault();

            consum.Cphone = CMPSContext.Company.Where(x => x.CmpID == Cmpid).Select(x => x.Phone1).FirstOrDefault();

            consum.Cweb = CMPSContext.Company.Where(x => x.CmpID == Cmpid).Select(x => x.Website).FirstOrDefault();

            consum.Cname = CMPSContext.Company.Where(x => x.CmpID == Cmpid).Select(x => x.CompanyName).FirstOrDefault();

            return consum;

        }
        public Help GetPatientDetails(DateTime vdate, int CompanyID)
        {

            var Regtran = WYNKContext.RegistrationTran.ToList();
            var cosumary = WYNKContext.ConsultantionSummary.ToList();
            var docmas = CMPSContext.DoctorMaster.ToList();
            var registration = WYNKContext.Registration.ToList();
            var user = CMPSContext.Users.ToList();

            var consum = new Help();


            consum.FullDet = (from fnd in Regtran.Where(x => x.DateofVisit.Date == vdate.Date && x.CmpID == CompanyID && x.ConsulationFees != null)
                       
                              join REG in registration on fnd.DateofVisit equals REG.DateofRegistration

                              select new FullDet
                              {
                                  Dttime = fnd.DateofVisit,
                                  uin = fnd.UIN,
                                  name = REG.Name + " " + REG.MiddleName + " " + REG.LastName,                                 
                                  Charges = fnd.ConsulationFees,

                              }).ToList();

            return consum;

        }

        public dynamic getopticalMaterialdetails()
        {
            return (from lt in WYNKContext.Lenstrans
                    join b in WYNKContext.Brand on lt.Brand equals b.ID
                    join ob in WYNKContext.OpticalBalance.Where(x => x.ClosingBalance > 0) on lt.ID equals ob.LTID
                    select new serviceDropdown
                    {
                        Text = b.Description,
                        Value = b.ID.ToString(),
                    }).OrderBy(x => x.Text).ToList();
        }

        public dynamic InvestPresc(int cmpid)
        {
            var PatientAccount = WYNKContext.PatientAccount.ToList();
            var registration = WYNKContext.Registration.Where(x => x.CMPID == cmpid).ToList();

            var help = new Help();
            help.FBdetail = (from REG in registration
                             join PA in PatientAccount on REG.UIN equals PA.UIN
                             where PA.CMPID == cmpid && PA.InvoiceNumber == null
                             select new FBdetail
                             {
                                 UIN = REG.UIN,
                                 DateofRegistration = TimeZoneInfo.ConvertTimeFromUtc(REG.DateofRegistration, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")),
                                 Name = REG.Name,
                                 MiddleName = REG.MiddleName,
                                 LastName = REG.LastName,
                                 DateofBirth = REG.DateofBirth,
                                 Age = ToAgeString(REG.DateofBirth),
                                 Gender = REG.Gender,
                                 Address1 = REG.Address1,
                                 Phone = REG.Phone,
                                 PAddress = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address1).FirstOrDefault(),
                                 PAddress2 = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address2).FirstOrDefault(),
                                 PAddress3 = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address3).FirstOrDefault(),
                                 Pphone = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Phone1).FirstOrDefault(),
                                 Pweb = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Website).FirstOrDefault(),
                                 PCompnayname = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.CompanyName).FirstOrDefault(),
                                 PAINSID = WYNKContext.PatientVsInsurance.Where(x => x.UIN == REG.UIN && x.IsActive == false && x.IsTransacted == false && x.CmpID == cmpid).Select(x => x.PAINSID).FirstOrDefault(),
                                 SumAssured = WYNKContext.PatientVsInsurance.Where(x => x.UIN == REG.UIN && x.IsActive == false && x.IsTransacted == false).Select(x => x.SumAssured).FirstOrDefault(),

                             }).ToList();
            return help;
        }
    }
}
