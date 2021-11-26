using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using CommonMailService.cs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Helpers;

namespace WYNK.Data.Repository.Implementation
{
    class PatientVsInsuranceRepository : RepositoryBase<PatientVsInsuranceViewModel>, IPatientVsInsuranceRepository
    {

        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        

        public PatientVsInsuranceRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public PatientVsInsuranceViewModel getRegData(int CmpID,string Name)
        {
            var registration = WYNKContext.Registration.Where(x=>x.CMPID==CmpID).ToList();
            var PatientAccount = WYNKContext.PatientAccount.ToList();
            var Company = CMPSContext.Company.ToList();
            var PatientVsInsurance = new PatientVsInsuranceViewModel();
            int cmpid = CMPSContext.Company.Where(c => c.CmpID == CmpID).Select(c => c.ParentID).FirstOrDefault();
            if (cmpid == 0)
            {
                cmpid = CmpID;
            }
            if (Name==null) 
            {
                PatientVsInsurance.PatVsInsRegdetail = (from REG in registration.
                                                    Where(x => x.UIN.StartsWith(Convert.ToString(Name), StringComparison.OrdinalIgnoreCase)
                                                    || x.Phone.StartsWith(Convert.ToString(Name))
                                                    || x.Name.StartsWith(Convert.ToString(Name), StringComparison.OrdinalIgnoreCase)
                                                    || x.LastName.StartsWith(Convert.ToString(Name), StringComparison.OrdinalIgnoreCase))
                                                        join cmp in Company.Where(x => x.ParentID == cmpid || x.CmpID == cmpid) on REG.CMPID equals cmp.CmpID
                                                        where REG.IsDeleted == false && REG.Insurance == true
                                                        select new PatVsInsRegdetail
                                                        {
                                                            UIN = REG.UIN,
                                                            DateofRegistration = TimeZoneInfo.ConvertTimeFromUtc(REG.DateofRegistration, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")),
                                                            Name = REG.Name,
                                                            MiddleName = REG.MiddleName,
                                                            LastName = REG.LastName,
                                                            DateofBirth = REG.DateofBirth,
                                                            Age = PasswordEncodeandDecode.ToAgeString(REG.DateofBirth),
                                                            Gender = REG.Gender,
                                                            Phone = REG.Phone,
                                                        }).ToList();
            }
            else 
            {
                PatientVsInsurance.PatVsInsRegdetail = (from REG in registration    
                                                        where REG.IsDeleted == false && REG.Insurance == true
                                                        select new PatVsInsRegdetail
                                                        {
                                                            UIN = REG.UIN,
                                                            DateofRegistration = TimeZoneInfo.ConvertTimeFromUtc(REG.DateofRegistration, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")),
                                                            Name = REG.Name,
                                                            MiddleName = REG.MiddleName,
                                                            LastName = REG.LastName,
                                                            DateofBirth = REG.DateofBirth,
                                                            Age = PasswordEncodeandDecode.ToAgeString(REG.DateofBirth),
                                                            Gender = REG.Gender,
                                                            Phone = REG.Phone,
                                                        }).ToList();
            }

            
            return PatientVsInsurance;
        }
        public PatientVsInsuranceViewModel getJointPolicyData(int CmpID, string UIN)
        {
            var PatientKIN = WYNKContext.PatientKINDetails.ToList();
            
            var PatientVsInsurance = new PatientVsInsuranceViewModel();
            PatientVsInsurance.JointPolicyKINRdetail = (from KIN in PatientKIN.Where(u => u.UIN == UIN)
                                                        select new JointPolicyKINRdetail
                                                        {
                                                            ID = KIN.ID,
                                                            Relationship = KIN.Relationship,
                                                            FirstName = KIN.FirstName,
                                                            MiddleName = KIN.MiddleName,
                                                            LastName = KIN.LastName,
                                                            ContactNumber = KIN.ContactNumber,
                                                            EmailID = KIN.EmailID,
                                                    }).ToList();
            return PatientVsInsurance;
        }
        public dynamic InsertPatientVsInsurance(PatientVsInsuranceViewModel AddPatientVsInsurance, int UserID, int CmpID)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    var PVI = new PatientVsInsurance();
                    PVI.UIN = AddPatientVsInsurance.PatientVsInsurance.UIN;
                    PVI.SumAssured = AddPatientVsInsurance.PatientVsInsurance.SumAssured;
                    PVI.InsurancevsMiddlemenID= AddPatientVsInsurance.PatientVsInsurance.InsurancevsMiddlemenID;
                    PVI.PolicyName = AddPatientVsInsurance.PatientVsInsurance.PolicyName;
                    PVI.PolicyNo = AddPatientVsInsurance.PatientVsInsurance.PolicyNo;
                    PVI.PolicyTakenOn = AddPatientVsInsurance.PatientVsInsurance.PolicyTakenOn;
                    PVI.PeriodFrom = AddPatientVsInsurance.PatientVsInsurance.PeriodFrom;
                    PVI.PeriodTo = AddPatientVsInsurance.PatientVsInsurance.PeriodTo;
                    PVI.IsJointPolicy = AddPatientVsInsurance.PatientVsInsurance.IsJointPolicy;
                    PVI.AmountAvailed = AddPatientVsInsurance.PatientVsInsurance.AmountAvailed;
                    PVI.Remarks = AddPatientVsInsurance.PatientVsInsurance.Remarks;
                    PVI.CreatedUTC = DateTime.UtcNow;
                    PVI.CreatedBy = UserID;
                    PVI.CmpID = CmpID;
                    WYNKContext.PatientVsInsurance.Add(PVI);
                    WYNKContext.SaveChanges();

                    var PAINSID = PVI.PAINSID;
                    foreach (var item in AddPatientVsInsurance.JoindataKIN.ToList())
                    {
                        var JointFamilyDetails = new JointFamilyDetails();
                        JointFamilyDetails.PAINSID = PAINSID;
                        JointFamilyDetails.SumAssured = item.Amount;
                        JointFamilyDetails.ParentID = item.ID;
                        JointFamilyDetails.IsDeleted = false;
                        JointFamilyDetails.CreatedBy = UserID;
                        JointFamilyDetails.CreatedUTC = DateTime.UtcNow;
                        WYNKContext.JointFamilyDetails.AddRange(JointFamilyDetails);
                        WYNKContext.SaveChanges();
                    }


                    if (AddPatientVsInsurance.InsImageTranArray.Count() > 0)
                    {
                        foreach (var items in AddPatientVsInsurance.InsImageTranArray.ToList())
                        {

                            var InsimgTran = new InsuranceImageTran();
                            InsimgTran.PAINSID = PAINSID;
                            InsimgTran.CmpID = CmpID;
                            InsimgTran.CreatedUTC = DateTime.UtcNow;
                            InsimgTran.CreatedBy = UserID;
                            WYNKContext.InsuranceImageTran.Add(InsimgTran);
                            WYNKContext.SaveChanges();
                        }
                    }





                    dbContextTransaction.Commit();
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
                    Message = "Some data are Missing"
                };
            }
        }
        public PatientVsInsuranceViewModel getPatientVsInsuranceData(int CmpID)
        {
            var PatientVsInsurance = WYNKContext.PatientVsInsurance.ToList();
            var PatientKIN = WYNKContext.PatientKINDetails.ToList();
            var getPatientVsInsurance = new PatientVsInsuranceViewModel();
            getPatientVsInsurance.getPatVsIns = (from PVI in PatientVsInsurance.Where(x=>x.CmpID==CmpID && x.IsActive==false)
                                                    select new getPatVsIns
                                                    {
                                                        PAINSID= PVI.PAINSID,
                                                        UIN = PVI.UIN,
                                                        Name=WYNKContext.Registration.Where(x=>x.UIN== PVI.UIN).Select(x=>x.Name).FirstOrDefault(),
                                                        MiddleName= WYNKContext.Registration.Where(x => x.UIN == PVI.UIN).Select(x => x.MiddleName).FirstOrDefault(),
                                                        LastName = WYNKContext.Registration.Where(x => x.UIN == PVI.UIN).Select(x => x.LastName).FirstOrDefault(),
                                                        Gender = WYNKContext.Registration.Where(x => x.UIN == PVI.UIN).Select(x => x.Gender).FirstOrDefault(),
                                                        Age = PasswordEncodeandDecode.ToAgeString(WYNKContext.Registration.Where(x => x.UIN == PVI.UIN).Select(x => x.DateofBirth).FirstOrDefault()),
                                                        InsurancevsMiddlemenID =  PVI.InsurancevsMiddlemenID,
                                                        PolicyName = PVI.PolicyName,
                                                        PolicyNo = PVI.PolicyNo,
                                                        PolicyTakenOn = PVI.PolicyTakenOn,
                                                        PeriodFrom = PVI.PeriodFrom,
                                                        PeriodTo = PVI.PeriodTo,
                                                        IsJointPolicy = PVI.IsJointPolicy,
                                                        SumAssured = PVI.SumAssured,
                                                        AmountAvailed = PVI.AmountAvailed,
                                                        IsActive = PVI.IsActive,
                                                        Remarks = PVI.Remarks,
                                                        
                                                    }).ToList();
           
            return getPatientVsInsurance;
        }

        public dynamic UpdatePatientVsInsurance(PatientVsInsuranceViewModel PatientVsInsuranceUpdate, int PAINSID)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    var PVI = new PatientVsInsurance();
                    PVI = WYNKContext.PatientVsInsurance.Where(x => x.PAINSID == PAINSID).FirstOrDefault();
                    PVI.IsActive = PatientVsInsuranceUpdate.PatientVsInsurance.IsActive;
                    WYNKContext.Entry(PVI).State = EntityState.Modified;
                    WYNKContext.SaveChanges();     
                    dbContextTransaction.Commit();
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
                    Message = "Some data are Missing"
                };
            }
        }



        public bool uploadinsimage(IFormFile file1, string desc, string uin, string id)
        {
            var fnd = new Findings();
            fnd.SQI = new List<SquintImage>();
            try
            {
                var currentDir = Directory.GetCurrentDirectory();
                var dt = Convert.ToDateTime(DateTime.Now.Date).ToString("dd-MM-yyyy");

                var res = Directory.CreateDirectory(currentDir + '/' + uin + '/' + dt);
                var fileName1 = $"{desc}{Path.GetExtension(file1.FileName)}";
                var path1 = $"{currentDir}/{uin}/{dt}/{fileName1}";

                var pathh = $"{currentDir}/{uin}/{dt}";

                using (var stream1 = new FileStream(path1, FileMode.Create))
                {
                    file1.CopyTo(stream1);
                    var PAINSID = WYNKContext.PatientVsInsurance.Where(x => x.UIN == id).Select(x => x.PAINSID).LastOrDefault();
                    var IMGid = WYNKContext.InsuranceImageTran.Where(x => x.PAINSID == PAINSID).ToList();
                   
                    if (IMGid.Count() > 0)
                    {
                        foreach (var item1 in IMGid.ToList())
                        {
                            item1.InsuranceImagePath = pathh;
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


    }
}
