
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Data.Repository.Operation;
using WYNK.Helpers;

namespace WYNK.Data.Repository.Implementation
{

    class CampRegistrationRepository : RepositoryBase<CampRegistrationViewModel>, ICampRegistrationRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;


        public CampRegistrationRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }
        public CampRegistrationViewModel GetCampkindetail(string uin)
        {
            var Campkindetails = new CampRegistrationViewModel();
            Campkindetails.CampPatientKINDetails = (from KINDetails in WYNKContext.CampPatientKINDetails.Where(x => x.CampUIN == uin)

                                      select new CampPatientKINDetails
                                      {
                                         
                                          CampUIN = KINDetails.CampUIN,
                                          Relationship = KINDetails.Relationship,
                                          FirstName = KINDetails.FirstName,
                                          MiddleName = KINDetails.MiddleName,
                                          LastName = KINDetails.LastName,
                                          ContactNumber = KINDetails.ContactNumber,
                                          EmailID = KINDetails.EmailID,
                                          PrimaryKinId = KINDetails.PrimaryKinId,
                                      }
                                      ).AsNoTracking().ToList();
            return Campkindetails;
        }
        public CampRegistrationViewModel GetCampRegistrationExtension(string CampRegNO, int CmpID)
        {
            var CampRegistrationExtension = new CampRegistrationViewModel();
            CampRegistrationExtension.GETCampRegExtension = (from REGE in WYNKContext.CampRegistrationExtension.Where(x => x.CampUIN == CampRegNO && x.CMPID == CmpID)
                                                     select new GETCampRegExtension
                                                     {
                                                         Artificialeye = REGE.Artificialeye,
                                                         OU = REGE.OU,
                                                         OD = REGE.OD,
                                                         OS = REGE.OS,

                                                     }).AsNoTracking().ToList();
            return CampRegistrationExtension;
        }


        public dynamic InsertCampReg(CampRegistrationViewModel CampRegistration, int userid, int Transactionid)
        {
            var CampReg = new CampRegistration();
            var CampRegTran = new CampRegistrationTran();
    

            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    CampReg.CampUIN = CampRegistration.CampRegistration.CampUIN;
                    CampReg.RegType = CampRegistration.CampRegistration.RegType;
                    CampReg.DateofRegistration = CampRegistration.CampRegistration.DateofRegistration;
                    CampReg.CMPID = CampRegistration.CampRegistration.CMPID;
                    CampReg.Name = CampRegistration.CampRegistration.Name;
                    CampReg.MiddleName = CampRegistration.CampRegistration.MiddleName;
                    CampReg.LastName = CampRegistration.CampRegistration.LastName;
                    CampReg.PreferredLanguage = CampRegistration.CampRegistration.PreferredLanguage;
                    CampReg.MaritalStatus = CampRegistration.CampRegistration.MaritalStatus;
                    CampReg.Occupation = CampRegistration.CampRegistration.Occupation;
                    CampReg.DateofBirth = CampRegistration.CampRegistration.DateofBirth.AddDays(1);
                    CampReg.Gender = CampRegistration.CampRegistration.Gender;
                    CampReg.Address1 = CampRegistration.CampRegistration.Address1;
                    CampReg.Address2 = CampRegistration.CampRegistration.Address2;
                    CampReg.Address3 = CampRegistration.CampRegistration.Address3;
                    CampReg.LocationID = CampRegistration.CampRegistration.LocationID;
                    CampReg.FatherHusbandName = CampRegistration.CampRegistration.FatherHusbandName;
                    CampReg.EmailID = CampRegistration.CampRegistration.EmailID;
                    CampReg.Phone = CampRegistration.CampRegistration.Phone;
                    CampReg.AlternateMailID = CampRegistration.CampRegistration.AlternateMailID;
                    CampReg.AlternatePhoneNumber = CampRegistration.CampRegistration.AlternatePhoneNumber;
                    CampReg.TransactionID = Transactionid;
                    CampReg.AadharNumber = CampRegistration.CampRegistration.AadharNumber;
                    CampReg.PanCardNo = CampRegistration.CampRegistration.PanCardNo;
                    CampReg.DrivingLicenseNo = CampRegistration.CampRegistration.DrivingLicenseNo;
                    CampReg.PassportNo = CampRegistration.CampRegistration.PassportNo;
                    CampReg.IsForeignNational = CampRegistration.CampRegistration.IsForeignNational;
                    CampReg.Insurance = CampRegistration.CampRegistration.Insurance;
                    CampReg.SourceofReferralID = CampRegistration.CampRegistration.SourceofReferralID;
                    CampReg.ReferralName = CampRegistration.CampRegistration.ReferralName;
                    CampReg.CampID = CampRegistration.CampRegistration.CampID;
                    CampReg.ICDSpecialityId = CampRegistration.CampRegistration.ICDSpecialityId;
                    CampReg.TreatmentAdvice = CampRegistration.CampRegistration.TreatmentAdvice;
                    CampReg.CreatedBy = userid;
                    CampReg.IsDeleted = false;
                    CampReg.CreatedUTC = DateTime.UtcNow;
                    CampReg.IsVisited = false;
                    CampReg.IsSurgeryDone = false;
                    WYNKContext.CampRegistration.Add(CampReg);

                    //////////////////////////CampregistrationTran///////////////////////////////////////
                    CampRegTran.CampUIN = CampRegistration.CampRegistration.CampUIN;
                    CampRegTran.CmpID = CampRegistration.CampRegistration.CMPID;
                    CampRegTran.DateofVisit = CampRegistration.CampRegistration.DateofRegistration;
                    CampRegTran.TypeofVisit = CampRegistration.CampRegistrationTran.TypeofVisit;
                    CampRegTran.Status = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Open").Select(x => x.OLMID).FirstOrDefault();
                    CampRegTran.StatusDateTime = CampRegistration.CampRegistration.DateofRegistration;
                    CampRegTran.Remarks = CampRegistration.CampRegistrationTran.Remarks;
                    CampRegTran.CreatedUTC = DateTime.UtcNow;
                    CampRegTran.CreatedBy = userid;

  
                    CampRegTran.DoctorID = 0;
                    CampRegTran.ReviewCount = 0;
                    CampRegTran.CmpID = CampRegistration.CampRegistration.CMPID;
                    CampRegTran.PatientVisitType = Convert.ToString(CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "New").Select(x => x.OLMID).FirstOrDefault());
                    WYNKContext.CampRegistrationTran.Add(CampRegTran);
                

                    //////////////////////////CampRegistrationExtension///////////////////////////////////////
                    var CampRegistrationExtension = new CampRegistrationExtension();
                    if (CampRegistration.CampRegistrationExtension.Artificialeye == true)
                    {
                        CampRegistrationExtension.CampUIN = CampRegistration.CampRegistration.CampUIN;
                        CampRegistrationExtension.Artificialeye = CampRegistration.CampRegistrationExtension.Artificialeye;
                        CampRegistrationExtension.OD = CampRegistration.CampRegistrationExtension.OD;
                        CampRegistrationExtension.OU = CampRegistration.CampRegistrationExtension.OU;
                        CampRegistrationExtension.OS = CampRegistration.CampRegistrationExtension.OS;
                        CampRegistrationExtension.CMPID = CampRegistration.CampRegistration.CMPID;
                        CampRegistrationExtension.CreatedUTC = DateTime.UtcNow;
                        CampRegistrationExtension.CreatedBy = userid;
                        WYNKContext.CampRegistrationExtension.Add(CampRegistrationExtension);
                        ErrorLog oErrorLogstranEX = new ErrorLog();
                        object namestrEX = CampRegistrationExtension;
                        oErrorLogstranEX.WriteErrorLogArray("CampRegistrationExtension", namestrEX);

                    }
                    //////////////////////////kindetails///////////////////////////////////////
                    foreach (var item in CampRegistration.CampPatientKIN.ToList())
                    {
                        var kindetails = new CampPatientKINDetails();
                        kindetails.CampUIN = CampRegistration.CampRegistration.CampUIN;
                        kindetails.Relationship = item.Relationship;
                        kindetails.FirstName = item.FirstName;
                        kindetails.LastName = item.LastName;
                        kindetails.ContactNumber = item.ContactNumber;
                        kindetails.EmailID = item.EmailID;
                        kindetails.PrimaryKinId = false;
                        kindetails.CmpID = CampRegistration.CampRegistration.CMPID;
                        kindetails.CreatedUTC = DateTime.UtcNow;
                        kindetails.CreatedBy = userid;
                        WYNKContext.CampPatientKINDetails.Add(kindetails);
                        ErrorLog oErrorLogstranKIN = new ErrorLog();
                        object namestrKIN = kindetails;
                        oErrorLogstranKIN.WriteErrorLogArray("PatientKINDetails", namestrKIN);
                    }

                    var CampPatientFootfall = new CampPatientFootfall();
                    //////////////////////////PatientFootfall open///////////////////////////////////////
                    var datereglocaltime = DateTime.Now;
                    var Date1 = WYNKContext.CampPatientFootfall.Where(x => x.Date.Date == datereglocaltime.Date && x.CmpID == CampRegistration.CampRegistration.CMPID && x.CampID == CampRegistration.CampRegistration.CampID).Select(x => x.Date.Date).FirstOrDefault();
                    
                    //////////////////////////podiatrist NewPatient///////////////////////////////////////
                    var Age = PasswordEncodeandDecode.ToAgeString(CampRegistration.CampRegistration.DateofBirth);
                    var result = Regex.Match(Age, @"\d+").Value;
                    var PodiatristAge = CMPSContext.Setup.Where(x => x.CMPID == CampRegistration.CampRegistration.CMPID).Select(x => x.Pediatric).FirstOrDefault();

                    //////////////////////////New Patient///////////////////////////////////////
                    if (datereglocaltime.Date == Date1.Date)
                    {
                        CampPatientFootfall = WYNKContext.CampPatientFootfall.Where(x => x.Date == Date1 && x.CmpID == CampRegistration.CampRegistration.CMPID && x.CampID == CampRegistration.CampRegistration.CampID).FirstOrDefault();
                        /////////////////////////////////////Camp-Surgery////////////////////////////////////////////////

                        if (CampRegistration.CampRegistration.RegType == "Camp-Surgery") 
                        {
                            CampPatientFootfall.SurgeryAdvisedPatient += 1;
                        }
                        else
                        {
                            CampPatientFootfall.SurgeryAdvisedPatient += 0;
                        }
                        //////////////////////////podiatrist NewPatient///////////////////////////////////////
                        if (Convert.ToInt32(PodiatristAge) > Convert.ToInt32(result))
                        {
                            ////////////////////////// Ocular(Artificialeye)///////////////////////////////////////
                            if (CampRegistration.CampRegistrationExtension.Artificialeye == true)
                            {
                                CampPatientFootfall.PaediatricOcular += 1;
                                CampPatientFootfall.PaediatricNormal += 0;
                            }
                            else
                            {
                                CampPatientFootfall.PaediatricNormal += 1;
                                CampPatientFootfall.PaediatricOcular += 0;
                            }
                            CampPatientFootfall.GeneralNormal += 0;
                            CampPatientFootfall.GeneralOcular += 0;
                        }
                        else
                        {
                            ////////////////////////// Ocular(Artificialeye)///////////////////////////////////////
                            if (CampRegistration.CampRegistrationExtension.Artificialeye == true)
                            {
                                CampPatientFootfall.GeneralOcular += 1;
                                CampPatientFootfall.GeneralNormal += 0;
                            }
                            else
                            {
                                CampPatientFootfall.GeneralNormal += 1;
                                CampPatientFootfall.GeneralOcular += 0;
                            }
                            CampPatientFootfall.PaediatricOcular += 0;
                            CampPatientFootfall.PaediatricNormal += 0;
                        }
              

                        CampPatientFootfall.PatientCount += 1;
                        CampPatientFootfall.Updatedby = userid;
                        WYNKContext.Entry(CampPatientFootfall).State = EntityState.Modified;
                        ErrorLog oErrorLogstranPF = new ErrorLog();
                        object namestrPF = CampPatientFootfall;
                        oErrorLogstranPF.WriteErrorLogArray("CampPatientFootfall", namestrPF);
                        WYNKContext.SaveChanges();
                    }
                    else
                    {
                        CampPatientFootfall.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                        CampPatientFootfall.PatientCount = +1;

                        /////////////////////////////////////Camp-Surgery////////////////////////////////////////////////

                        if (CampRegistration.CampRegistration.RegType == "Camp-Surgery")
                        {
                            CampPatientFootfall.SurgeryAdvisedPatient = +1;
                        }
                        else
                        {
                            CampPatientFootfall.SurgeryAdvisedPatient = +0;
                        }

                        //////////////////////////podiatrist NewPatient///////////////////////////////////////
                        if (Convert.ToInt32(PodiatristAge) > Convert.ToInt32(result))
                        {
                            ////////////////////////// Ocular(Artificialeye)///////////////////////////////////////
                            if (CampRegistration.CampRegistrationExtension.Artificialeye == true)
                            {
                                CampPatientFootfall.PaediatricOcular = +1;
                                CampPatientFootfall.PaediatricNormal = +0;
                            }
                            else
                            {
                                CampPatientFootfall.PaediatricNormal = +1;
                                CampPatientFootfall.PaediatricOcular = +0;
                            }
                            CampPatientFootfall.GeneralNormal = +0;
                            CampPatientFootfall.GeneralOcular = +0;
                        }
                        else
                        {
                            ////////////////////////// Ocular(Artificialeye)///////////////////////////////////////
                            if (CampRegistration.CampRegistrationExtension.Artificialeye == true)
                            {
                                CampPatientFootfall.GeneralOcular = +1;
                                CampPatientFootfall.GeneralNormal = +0;
                            }
                            else
                            {
                                CampPatientFootfall.GeneralNormal = +1;
                                CampPatientFootfall.GeneralOcular = +0;
                            }
                            CampPatientFootfall.PaediatricNormal = +0;
                            CampPatientFootfall.PaediatricOcular = +0;
                        }

                        CampPatientFootfall.Date = Convert.ToDateTime(datereglocaltime.ToString("yyyy-MM-dd"));
                        CampPatientFootfall.Createdby = userid;
                        CampPatientFootfall.CreatedUTC = DateTime.UtcNow;
                        CampPatientFootfall.CmpID = CampRegistration.CampRegistration.CMPID;
                        CampPatientFootfall.CampID = CampRegistration.CampRegistration.CampID;
                        WYNKContext.CampPatientFootfall.Add(CampPatientFootfall);
                        ErrorLog oErrorLogstranPF = new ErrorLog();
                        object namestrPF = CampPatientFootfall;
                        oErrorLogstranPF.WriteErrorLogArray("CampPatientFootfall", namestrPF);
                        WYNKContext.SaveChanges();
                    }
                    //////////////////////////PatientFootfall end///////////////////////////////////////
                    WYNKContext.SaveChanges();

                    ///////////////////////////GenerateRunningCtrlNoo1(update)//////////////////////////////////
                    var commonRepos = new CommonRepository(_Wynkcontext, _Cmpscontext);
                    var RunningNumber = commonRepos.GenerateRunningCtrlNoo(Transactionid, CampRegistration.CampRegistration.CMPID, "GetRunningNo");
                    if (RunningNumber == CampRegistration.CampRegistration.CampUIN)
                    {
                        commonRepos.GenerateRunningCtrlNoo(Transactionid, CampRegistration.CampRegistration.CMPID, "UpdateRunningNo");
                    }

                    else
                    {
                        var GetRunningNumber = commonRepos.GenerateRunningCtrlNoo(Transactionid, CampRegistration.CampRegistration.CMPID, "UpdateRunningNo");
                        ///////////////////////////registrationMaster//////////////////////////////////
                        CampReg = WYNKContext.CampRegistration.Where(x => x.CampUIN == CampRegistration.CampRegistration.CampUIN).FirstOrDefault();
                        CampReg.CampUIN = GetRunningNumber;
                        WYNKContext.Entry(RunningNumber).State = EntityState.Modified;
                        WYNKContext.SaveChanges();
                    }

                    string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == userid).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                    ErrorLog oErrorLogs = new ErrorLog();
                    object namestr = CampRegistration;
                    oErrorLogs.WriteErrorLogTitle(CampRegistration.Companyname, "CampRegistration", "User name :", username, "User ID :", Convert.ToString(userid), "Mode : Add");

                    WYNKContext.SaveChanges();
                    if (WYNKContext.SaveChanges() >= 0)
                    {
                        ErrorLog oErrorLog = new ErrorLog();
                        oErrorLog.WriteErrorLog("Information :", "Saved Successfully");
                    }
                    dbContextTransaction.Commit();

                    if (WYNKContext.SaveChanges() >= 0)
                        return new
                        {
                            Success = true,
                            Cuin = CampRegistration.CampRegistration.CampUIN,
                            Cname = CampRegistration.CampRegistration.Name + ' ' + CampRegistration.CampRegistration.MiddleName + ' ' + CampRegistration.CampRegistration.LastName,
                            PAddress = CMPSContext.Company.Where(x => x.CmpID == CampRegistration.CampRegistration.CMPID).Select(x => x.Address1).FirstOrDefault(),
                            PAddress2 = CMPSContext.Company.Where(x => x.CmpID == CampRegistration.CampRegistration.CMPID).Select(x => x.Address2).FirstOrDefault(),
                            PAddress3 = CMPSContext.Company.Where(x => x.CmpID == CampRegistration.CampRegistration.CMPID).Select(x => x.Address3).FirstOrDefault(),
                            Pphone = CMPSContext.Company.Where(x => x.CmpID == CampRegistration.CampRegistration.CMPID).Select(x => x.Phone1).FirstOrDefault(),
                            Pweb = CMPSContext.Company.Where(x => x.CmpID == CampRegistration.CampRegistration.CMPID).Select(x => x.Website).FirstOrDefault(),
                            PCompnayname = CampRegistration.Companyname,
                            RegisteredBy = CMPSContext.Users.Where(x => x.Userid == userid).Select(x => x.Username).FirstOrDefault(),
                            DateofRegistration = CampRegistration.CampRegistration.DateofRegistration,
                            CampName = WYNKContext.CAMP.Where(x => x.CampID == CampRegistration.CampRegistration.CampID).Select(c => c.CampName).FirstOrDefault(),
                        };

                }

                catch (Exception ex)

                {
                    ErrorLog oErrorLog = new ErrorLog();
                    oErrorLog.WriteErrorLog("Error :", ex.InnerException.Message.ToString());
                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                    string msg = ex.InnerException.Message;
                    return new
                    {
                        Success = false,
                       
                    };
                }
                return new
                {
                    Success = false,

                };
            }
        }


        public dynamic UdateCampReg(CampRegistrationViewModel CampRegistrationUpdate, int CmpID, int userid, string Campuin)
        {
            var CampReg = new CampRegistration();
            var CampRegTran = new CampRegistrationTran();
            
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {


                    CampReg = WYNKContext.CampRegistration.Where(x => x.CampUIN == Campuin && x.CMPID == CmpID).FirstOrDefault();
                    CampReg.RegType = CampRegistrationUpdate.CampRegistration.RegType;
                    CampReg.DateofRegistration = CampRegistrationUpdate.CampRegistration.DateofRegistration;
                    CampReg.CMPID = CampRegistrationUpdate.CampRegistration.CMPID;
                    CampReg.Name = CampRegistrationUpdate.CampRegistration.Name;
                    CampReg.MiddleName = CampRegistrationUpdate.CampRegistration.MiddleName;
                    CampReg.LastName = CampRegistrationUpdate.CampRegistration.LastName;
                    CampReg.PreferredLanguage = CampRegistrationUpdate.CampRegistration.PreferredLanguage;
                    CampReg.MaritalStatus = CampRegistrationUpdate.CampRegistration.MaritalStatus;
                    CampReg.Occupation = CampRegistrationUpdate.CampRegistration.Occupation;
                    CampReg.DateofBirth = CampRegistrationUpdate.CampRegistration.DateofBirth.AddDays(1);
                    CampReg.Gender = CampRegistrationUpdate.CampRegistration.Gender;
                    CampReg.Address1 = CampRegistrationUpdate.CampRegistration.Address1;
                    CampReg.Address2 = CampRegistrationUpdate.CampRegistration.Address2;
                    CampReg.Address3 = CampRegistrationUpdate.CampRegistration.Address3;
                    CampReg.LocationID = CampRegistrationUpdate.CampRegistration.LocationID;
                    CampReg.FatherHusbandName = CampRegistrationUpdate.CampRegistration.FatherHusbandName;
                    CampReg.EmailID = CampRegistrationUpdate.CampRegistration.EmailID;
                    CampReg.Phone = CampRegistrationUpdate.CampRegistration.Phone;
                    CampReg.AlternateMailID = CampRegistrationUpdate.CampRegistration.AlternateMailID;
                    CampReg.AlternatePhoneNumber = CampRegistrationUpdate.CampRegistration.AlternatePhoneNumber;
                    CampReg.AadharNumber = CampRegistrationUpdate.CampRegistration.AadharNumber;
                    CampReg.PanCardNo = CampRegistrationUpdate.CampRegistration.PanCardNo;
                    CampReg.DrivingLicenseNo = CampRegistrationUpdate.CampRegistration.DrivingLicenseNo;
                    CampReg.PassportNo = CampRegistrationUpdate.CampRegistration.PassportNo;
                    CampReg.IsForeignNational = CampRegistrationUpdate.CampRegistration.IsForeignNational;
                    CampReg.SourceofReferralID = CampRegistrationUpdate.CampRegistration.SourceofReferralID;
                    CampReg.ReferralName = CampRegistrationUpdate.CampRegistration.ReferralName;
                    CampReg.CampID = CampRegistrationUpdate.CampRegistration.CampID;
                    CampReg.ICDSpecialityId = CampRegistrationUpdate.CampRegistration.ICDSpecialityId;
                    CampReg.TreatmentAdvice = CampRegistrationUpdate.CampRegistration.TreatmentAdvice;
                    CampReg.UpdatedBy = userid;
                    CampReg.UpdatedUTC = DateTime.UtcNow;
                   
                    WYNKContext.UpdateRange(CampReg);
                    WYNKContext.SaveChanges();

                    var CampRegistrationExtension = new CampRegistrationExtension();
                    if (CampRegistrationUpdate.CampRegistrationExtension.Artificialeye == true)
                    {
                        CampRegistrationExtension = WYNKContext.CampRegistrationExtension.Where(x => x.CampUIN == Campuin && x.CMPID == CmpID).FirstOrDefault();
                        CampRegistrationExtension.Artificialeye = CampRegistrationUpdate.CampRegistrationExtension.Artificialeye;
                        CampRegistrationExtension.OD = CampRegistrationUpdate.CampRegistrationExtension.OD;
                        CampRegistrationExtension.OU = CampRegistrationUpdate.CampRegistrationExtension.OU;
                        CampRegistrationExtension.OS = CampRegistrationUpdate.CampRegistrationExtension.OS;
                        CampRegistrationExtension.UpdatedUTC = DateTime.UtcNow;
                        CampRegistrationExtension.UpdatedBy = userid;
                        WYNKContext.Entry(CampRegistrationExtension).State = EntityState.Modified;

                        ErrorLog oErrorLogstranEX = new ErrorLog();
                        object namestrEX = CampRegistrationExtension;
                        oErrorLogstranEX.WriteErrorLogArray("CampRegistrationExtension", namestrEX);

                        WYNKContext.SaveChanges();
                    }


                    //////////////////////////kindetails///////////////////////////////////////
                    var DkinD = WYNKContext.CampPatientKINDetails.Where(x => x.CampUIN == Campuin && x.CmpID ==CmpID).ToList();
                    WYNKContext.CampPatientKINDetails.RemoveRange(DkinD);
                    WYNKContext.SaveChanges();
                    var kindetails = new CampPatientKINDetails();
                    foreach (var item in CampRegistrationUpdate.CampPatientKIN.ToList())
                    {
                        kindetails.CampUIN = Campuin;
                        kindetails.Relationship = item.Relationship;
                        kindetails.FirstName = item.FirstName;
                        kindetails.LastName = item.LastName;
                        kindetails.ContactNumber = item.ContactNumber;
                        kindetails.EmailID = item.EmailID;
                        kindetails.PrimaryKinId = false;
                        kindetails.CmpID = CmpID;
                        kindetails.CreatedUTC = DateTime.UtcNow;
                        kindetails.CreatedBy = userid;
                        WYNKContext.CampPatientKINDetails.Add(kindetails);
                        ErrorLog oErrorLogstranKIN = new ErrorLog();
                        object namestrKIN = kindetails;
                        oErrorLogstranKIN.WriteErrorLogArray("PatientKINDetails", namestrKIN);

                       
                    }

                   








                    WYNKContext.SaveChanges();



                    if (WYNKContext.SaveChanges() >= 0)
                    {
                        ErrorLog oErrorLog = new ErrorLog();
                        oErrorLog.WriteErrorLog("Information :", "Saved Successfully");
                    }
                    dbContextTransaction.Commit();

                    if (WYNKContext.SaveChanges() >= 0)
                        return new
                        {
                            Success = true,
                        };

                }
                catch (Exception ex)
                {
                    ErrorLog oErrorLog = new ErrorLog();
                    oErrorLog.WriteErrorLog("Error :", ex.InnerException.Message.ToString());
                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                }
                return new
                {
                    Success = false,
                };
            }
        }




        public dynamic DeleteCampReg(string Campuin, int CMPID, int USERID, string Companyname)
        {
            try
            {
                var CampRegistrationdelete = WYNKContext.CampRegistration.Where(x => x.CampUIN == Campuin).FirstOrDefault();
                if (CampRegistrationdelete != null)
                {
                    CampRegistrationdelete.IsDeleted = true;
                    WYNKContext.CampRegistration.Update(CampRegistrationdelete);


                    string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == USERID).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                    string userid = Convert.ToString(USERID);
                    ErrorLog oErrorLogs = new ErrorLog();
                    oErrorLogs.WriteErrorLogTitle(Companyname, "Donor", "User name :", username, "User ID :", userid, "Mode : Delete");
                    object names = CampRegistrationdelete;
                    oErrorLogs.WriteErrorLogArray("Donor", names);

                    WYNKContext.SaveChanges();

                }
                if (WYNKContext.SaveChanges() >= 0)
                {
                    ErrorLog oErrorLog = new ErrorLog();
                    oErrorLog.WriteErrorLog("Information :", "Delete Successfully");
                    return new
                    {
                        Success = true,
                        Message = "Save successfully"
                    };
                }
            }
            catch (Exception ex)
            {
                ErrorLog oErrorLog = new ErrorLog();
                oErrorLog.WriteErrorLog("Error :", ex.InnerException.Message.ToString());
            }
            return new
            {
                Success = false,
                Message = "Something Went Wrong"
            };

        }


    }
}













