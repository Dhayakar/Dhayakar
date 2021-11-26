using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Data.Repository.Operation;
using WYNK.Helpers;

namespace WYNK.Data.Repository.Implementation
{
    class InsuranceRepository : RepositoryBase<InsuranceViewModel>, IInsuranceRepository
    {

        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;


        public InsuranceRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public InsuranceViewModel GetPinCodeDetails(int location)
        {

            var PinCode = new InsuranceViewModel();
            PinCode.ParentDescriptionPinCode = CMPSContext.LocationMaster.Where(x => x.ID == location).Select(x => x.Pincode).FirstOrDefault();


            return PinCode;

        }


        public InsuranceViewModel GetlocationDetails(int id)
        {
            var locDetails = new InsuranceViewModel();
            var v = CMPSContext.LocationMaster.Where(x => x.ID == id).Select(x => x.ParentDescription).FirstOrDefault();
            var stateid = CMPSContext.LocationMaster.Where(x => x.ParentDescription == v).Select(x => x.ParentID).FirstOrDefault();
            locDetails.ParentDescriptionstate = CMPSContext.LocationMaster.Where(x => x.ID == stateid).Select(x => x.ParentDescription).FirstOrDefault();
            var countryid = CMPSContext.LocationMaster.Where(x => x.ParentDescription == locDetails.ParentDescriptionstate).Select(x => x.ParentID).FirstOrDefault();
            locDetails.ParentDescriptioncountry = CMPSContext.LocationMaster.Where(x => x.ID == countryid).Select(x => x.ParentDescription).FirstOrDefault();
            return locDetails;
        }
        public dynamic InsertInsurance(InsuranceViewModel AddInsurance)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    var Ins = new Insurance();
                    Ins.Name = AddInsurance.Insurance.Name;
                    Ins.Address1 = AddInsurance.Insurance.Address1;
                    Ins.Address2 = AddInsurance.Insurance.Address2;
                    Ins.Address3 = AddInsurance.Insurance.Address3;
                    Ins.LocationId = AddInsurance.Insurance.LocationId;
                    Ins.Pincode = AddInsurance.Insurance.Pincode;
                    Ins.InsuranceCategory = AddInsurance.Insurance.InsuranceCategory;
                    Ins.CreatedUTC = DateTime.UtcNow;
                    Ins.CreatedBy = AddInsurance.Insurance.CreatedBy;
                    Ins.CMPID = AddInsurance.Insurance.CMPID;
                    Ins.IsActive = true;
                    WYNKContext.Insurance.Add(Ins);
                    WYNKContext.SaveChanges();

                    var IID = Ins.ID;
                    if (AddInsurance.Insurance.InsuranceCategory == 0)
                    {
                        var InsVSmid = new InsuranceVsMiddlemen();
                        InsVSmid.IID = IID;
                        InsVSmid.IsActive = true;
                        InsVSmid.CreatedBy = AddInsurance.Insurance.CreatedBy;
                        InsVSmid.CreatedUTC = DateTime.UtcNow;
                        WYNKContext.InsuranceVsMiddlemen.AddRange(InsVSmid);
                        WYNKContext.SaveChanges();
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


        public dynamic UpdateInsurance(InsuranceViewModel InsuranceUpdate, int ID)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    var Ins = new Insurance();
                    Ins = WYNKContext.Insurance.Where(x => x.ID == ID).FirstOrDefault();
                    Ins.Name = InsuranceUpdate.Insurance.Name;
                    Ins.Address1 = InsuranceUpdate.Insurance.Address1;
                    Ins.Address2 = InsuranceUpdate.Insurance.Address2;
                    Ins.Address3 = InsuranceUpdate.Insurance.Address3;
                    Ins.LocationId = InsuranceUpdate.Insurance.LocationId;
                    Ins.Pincode = InsuranceUpdate.Insurance.Pincode;
                    Ins.InsuranceCategory = InsuranceUpdate.Insurance.InsuranceCategory;
                    Ins.UpdatedUTC = DateTime.UtcNow;
                    Ins.UpdatedBy = InsuranceUpdate.Insurance.CreatedBy;
                    Ins.IsActive = InsuranceUpdate.Insurance.IsActive;

                    WYNKContext.Entry(Ins).State = EntityState.Modified;
                    WYNKContext.SaveChanges();


                    var InsVSmid = new InsuranceVsMiddlemen();
                    if (InsuranceUpdate.Insurance.InsuranceCategory == 0)
                    {
                        InsVSmid = WYNKContext.InsuranceVsMiddlemen.Where(x => x.IID == ID).FirstOrDefault();
                        InsVSmid.IsActive = InsuranceUpdate.Insurance.IsActive;
                        InsVSmid.UpdatedBy = InsuranceUpdate.Insurance.CreatedBy;
                        InsVSmid.UpdatedUTC = DateTime.UtcNow;
                        WYNKContext.Entry(InsVSmid).State = EntityState.Modified;
                        WYNKContext.SaveChanges();
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


        //////////////////////////////////////////////////////////////////////////////////

        public InsuranceViewModel GetPreAuthorizationdtls(int cmpid)
        {

            var GetPreAuthorization = new InsuranceViewModel();

            var Admission = WYNKContext.Admission.ToList();
            var REG = WYNKContext.Registration.ToList();
            
            GetPreAuthorization.patientPreAuthorizationdtls = (from R in Admission.Where(x=>x.CMPID==cmpid && x.isbilled==false)
                                                               join CM in REG on R.UIN equals CM.UIN
                                                               select new patientPreAuthorizationdtls
                                                               {
                                                                AdmID = R.AdmID,
                                                                uin = R.UIN,
                                                                name = CM.Name + " " + CM.MiddleName + " " + CM.LastName,
                                                                gender = CM.Gender,
                                                                age = PasswordEncodeandDecode.ToAgeString(CM.DateofBirth),
                                                               }).ToList();

            return GetPreAuthorization;
        }


        public dynamic InsertPreAuthorization(InsuranceViewModel AddPreAuthorization, int ID)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    var PAI=new preAuthorizationInsurance();


                    if (ID != 0)
                    {
                        PAI = WYNKContext.preAuthorizationInsurance.Where(x => x.ID == ID).FirstOrDefault();
                        PAI.Healthinsurancecard = AddPreAuthorization.preAuthorizationInsurance.Healthinsurancecard;
                        PAI.AmtApproval = AddPreAuthorization.preAuthorizationInsurance.AmtApproval;
                        PAI.insuranceCardNo = AddPreAuthorization.preAuthorizationInsurance.insuranceCardNo;
                        PAI.CardValidity = AddPreAuthorization.preAuthorizationInsurance.CardValidity.Date.AddDays(1);
                        PAI.preAuthorizationDate = AddPreAuthorization.preAuthorizationInsurance.preAuthorizationDate.AddDays(1);
                        PAI.policyNo = AddPreAuthorization.preAuthorizationInsurance.policyNo;
                        PAI.status = AddPreAuthorization.preAuthorizationInsurance.status;
                        PAI.ModeOFCommunication = AddPreAuthorization.preAuthorizationInsurance.ModeOFCommunication;
                        PAI.RequestSentOnDate = AddPreAuthorization.preAuthorizationInsurance.RequestSentOnDate;
                        PAI.Remarks = AddPreAuthorization.preAuthorizationInsurance.Remarks;
                        PAI.UpdatedUTC = DateTime.UtcNow;
                        PAI.UpdatedBy = AddPreAuthorization.preAuthorizationInsurance.CreatedBy;
                        WYNKContext.Entry(PAI).State = EntityState.Modified;

                        string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == AddPreAuthorization.preAuthorizationInsurance.CreatedBy).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                        ErrorLog oErrorLogs = new ErrorLog();
                        object names = PAI;
                        oErrorLogs.WriteErrorLogTitle(AddPreAuthorization.Companyname, "pre-Authorization", "User name :", username, "User ID :", Convert.ToString(AddPreAuthorization.preAuthorizationInsurance.CreatedBy), "Mode : Update");
                        oErrorLogs.WriteErrorLogArray("pre-Authorization", names);

                        WYNKContext.SaveChanges();
                    }

                    else
                    {
                        PAI.UIN = AddPreAuthorization.preAuthorizationInsurance.UIN;
                        PAI.Healthinsurancecard = AddPreAuthorization.preAuthorizationInsurance.Healthinsurancecard;
                        PAI.AmtApproval = AddPreAuthorization.preAuthorizationInsurance.AmtApproval;
                        PAI.insuranceCardNo = AddPreAuthorization.preAuthorizationInsurance.insuranceCardNo;
                        PAI.CardValidity = AddPreAuthorization.preAuthorizationInsurance.CardValidity.Date.AddDays(1);
                        PAI.preAuthorizationDate = AddPreAuthorization.preAuthorizationInsurance.preAuthorizationDate.AddDays(1);
                        PAI.policyNo = AddPreAuthorization.preAuthorizationInsurance.policyNo;
                        PAI.status = AddPreAuthorization.preAuthorizationInsurance.status;
                        PAI.ModeOFCommunication = AddPreAuthorization.preAuthorizationInsurance.ModeOFCommunication;
                        PAI.RequestSentOnDate = AddPreAuthorization.preAuthorizationInsurance.RequestSentOnDate;
                        PAI.Remarks = AddPreAuthorization.preAuthorizationInsurance.Remarks;
                        PAI.isdelete = false;
                        PAI.CmpID = AddPreAuthorization.preAuthorizationInsurance.CmpID;
                        PAI.CreatedBy = AddPreAuthorization.preAuthorizationInsurance.CreatedBy;
                        PAI.CreatedUTC = DateTime.UtcNow;
                        WYNKContext.preAuthorizationInsurance.Add(PAI);
                     
                        string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == AddPreAuthorization.preAuthorizationInsurance.CreatedBy).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                        ErrorLog oErrorLogs = new ErrorLog();
                        object namestr = PAI;
                        oErrorLogs.WriteErrorLogTitle(AddPreAuthorization.Companyname, "pre-Authorization", "User name :", username, "User ID :", Convert.ToString(AddPreAuthorization.preAuthorizationInsurance.CreatedBy), "Mode : Add");
                        oErrorLogs.WriteErrorLogArray("pre-Authorization", namestr);

                        WYNKContext.SaveChanges();
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

        public InsuranceViewModel GetPreAUZupdate(int cmpid)
        {

             var GetPreAuthorizationupdate = new InsuranceViewModel();

            var PREAUZINC = WYNKContext.preAuthorizationInsurance.ToList();
            var REG = WYNKContext.Registration.ToList();
            var utctime = CMPSContext.Setup.Where(x => x.CMPID == cmpid).Select(x => x.UTCTime).FirstOrDefault();
            TimeSpan ts = TimeSpan.Parse(utctime);
            
            GetPreAuthorizationupdate.PreAuthorizationupdate = (from R in PREAUZINC.Where(x => x.CmpID == cmpid && x.isdelete == false)
                                                               join CM in REG on R.UIN equals CM.UIN
                                                               select new PreAuthorizationupdate
                                                               {
                                                                   ID = R.ID,
                                                                   uin = R.UIN,
                                                                   name = CM.Name + " " + CM.MiddleName + " " + CM.LastName,
                                                                   gender = CM.Gender,
                                                                   age = PasswordEncodeandDecode.ToAgeString(CM.DateofBirth),
                                                                   Healthinsurancecard=R.Healthinsurancecard,
                                                                   AmtApproval=R.AmtApproval,
                                                                   insuranceCardNo=R.insuranceCardNo,
                                                                   CardValidity=R.CardValidity + ts,
                                                                   preAuthorizationDate =R.preAuthorizationDate + ts,
                                                                   policyNo=R.policyNo,
                                                                   status=R.status,
                                                                   ModeOFCommunication=R.ModeOFCommunication,
                                                                   RequestSentOnDate=R.RequestSentOnDate + ts,
                                                                   Remarks=R.Remarks,
                                                               }).ToList();

            return GetPreAuthorizationupdate;
        }



        public dynamic DeletePreA(InsuranceViewModel Insurancedelete, int ID)
        {
            using (var dbContextTransaction = CMPSContext.Database.BeginTransaction())
            {
                try
                {
                    var Description = new preAuthorizationInsurance();
                    Description = WYNKContext.preAuthorizationInsurance.Where(x => x.ID == ID).FirstOrDefault();
                    Description.isdelete = true;
                    WYNKContext.Entry(Description).State = EntityState.Modified;
                    WYNKContext.SaveChanges();
                    dbContextTransaction.Commit();
                    if (WYNKContext.SaveChanges() >= 0)
                        return new
                        {
                            Success = true,

                        };
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                }

            }
            return new
            {
                Success = false,

            };
        }


        //////////////////////////////////////////EstimateTracking////////////////////////////////////////

        public InsuranceViewModel GetEstimateTrackingdtls(int cmpid)
        {

            var GetPreAuthorization = new InsuranceViewModel();

            var preAuthorization = WYNKContext.preAuthorizationInsurance.ToList();

            var REG = WYNKContext.Registration.ToList();
            var utctime = CMPSContext.Setup.Where(x => x.CMPID == cmpid).Select(x => x.UTCTime).FirstOrDefault();
            TimeSpan ts = TimeSpan.Parse(utctime);

            GetPreAuthorization.patientPreAuthorizationdtls = (from R in preAuthorization.Where(x => x.CmpID == cmpid && x.isdelete == false)
                                                               join CM in REG on R.UIN equals CM.UIN
                                                               select new patientPreAuthorizationdtls
                                                               {
                                                                   ID = R.ID,
                                                                   AmtApproval = R.AmtApproval,
                                                                   uin = R.UIN,
                                                                   name = CM.Name + " " + CM.MiddleName + " " + CM.LastName,
                                                                   gender = CM.Gender,
                                                                   age = PasswordEncodeandDecode.ToAgeString(CM.DateofBirth),
                                                                   Healthinsurancecard = R.Healthinsurancecard,
                                                                   insuranceCardNo = R.insuranceCardNo,
                                                                   CardValidity = R.CardValidity + ts,
                                                                   preAuthorizationDate = R.preAuthorizationDate + ts,
                                                                   policyNo = R.policyNo,
                                                                   status = R.status,
                                                                   ModeOFCommunication = R.ModeOFCommunication,
                                                                   RequestSentOnDate = R.RequestSentOnDate + ts,
                                                                   Remarks = R.Remarks,
                                                               }).ToList();

            return GetPreAuthorization;
        }

        public dynamic InsertEstimateTracking(InsuranceViewModel AddEstimateTracking)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    var ET = new EstimateTracking();
                        ET.UIN = AddEstimateTracking.EstimateTracking.UIN;
                        ET.PreAID = AddEstimateTracking.EstimateTracking.PreAID;
                        ET.ApprovedDate = AddEstimateTracking.EstimateTracking.ApprovedDate.AddDays(1); ;
                        ET.ApprovedLimit = AddEstimateTracking.EstimateTracking.ApprovedLimit;
                        ET.ClaimNO = AddEstimateTracking.EstimateTracking.ClaimNO;
                        ET.status = AddEstimateTracking.EstimateTracking.status;
                        ET.ModeOFCommunication = AddEstimateTracking.EstimateTracking.ModeOFCommunication;
                        ET.Remarks = AddEstimateTracking.EstimateTracking.Remarks;
                        ET.isdelete = false;
                        ET.CmpID = AddEstimateTracking.EstimateTracking.CmpID;
                        ET.CreatedBy = AddEstimateTracking.EstimateTracking.CreatedBy;
                        ET.CreatedUTC = DateTime.UtcNow;
                        WYNKContext.EstimateTracking.Add(ET);
                        string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == AddEstimateTracking.EstimateTracking.CreatedBy).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                        ErrorLog oErrorLogs = new ErrorLog();
                        object namestr = ET;
                        oErrorLogs.WriteErrorLogTitle(AddEstimateTracking.Companyname, "Estimate Tracking", "User name :", username, "User ID :", Convert.ToString(AddEstimateTracking.EstimateTracking.CreatedBy), "Mode : Add");
                        oErrorLogs.WriteErrorLogArray("Estimate Tracking", namestr);

                        WYNKContext.SaveChanges();



                    var perAstatus = WYNKContext.preAuthorizationInsurance.Where(x => x.ID == AddEstimateTracking.EstimateTracking.PreAID).FirstOrDefault();
                    perAstatus.status = AddEstimateTracking.EstimateTracking.status;
                    perAstatus.UpdatedBy = AddEstimateTracking.EstimateTracking.CreatedBy;
                    perAstatus.UpdatedUTC = DateTime.UtcNow;
                    WYNKContext.preAuthorizationInsurance.UpdateRange(perAstatus);
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

        public InsuranceViewModel GetETDetails(int cmpid)
        {

            var GetEstimateTracking = new InsuranceViewModel();

            var EstimateTracking = WYNKContext.EstimateTracking.ToList();

            var REG = WYNKContext.Registration.ToList();
            var utctime = CMPSContext.Setup.Where(x => x.CMPID == cmpid).Select(x => x.UTCTime).FirstOrDefault();
            TimeSpan ts = TimeSpan.Parse(utctime);
            GetEstimateTracking.EstimateTrackingtls = (from R in EstimateTracking.Where(x => x.CmpID == cmpid && x.isdelete == false)
                                                               join CM in REG on R.UIN equals CM.UIN
                                                               select new EstimateTrackingdtls
                                                               {
                                                                   ID = R.ID,
                                                                   ApprovedLimit = R.ApprovedLimit,
                                                                   uin = R.UIN,
                                                                   name = CM.Name + " " + CM.MiddleName + " " + CM.LastName,
                                                                   gender = CM.Gender,
                                                                   age = PasswordEncodeandDecode.ToAgeString(CM.DateofBirth),
                                                                   ClaimNO = R.ClaimNO,
                                                                   status=R.status,
                                                                   ApprovedDate=R.ApprovedDate + ts,
                                                               }).ToList();

            return GetEstimateTracking;
        }

    }
}
