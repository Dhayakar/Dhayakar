
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using CommonMailService.cs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Data.Repository.Operation;
using WYNK.Helpers;

namespace WYNK.Data.Repository.Implementation
{

    class RegistrationMasterRepository : RepositoryBase<RegistrationMasterViewModel>, IRegistrationMasterRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;


        public RegistrationMasterRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public RegistrationMasterI getBusinessRule(int CmpID, string Activedata, string RegNO, int TransactiontypeID, DateTime RGDate)
        {

            var BusinessRule = new RegistrationMasterI();
            BusinessRule.PatientAssignStatus = new List<PatientAssignStatus>();
            var VisitTypeREV = CMPSContext.OneLineMaster.AsNoTracking().Where(c => c.ParentDescription == "Review" && c.ParentTag == "TOV").Select(c => c.OLMID).FirstOrDefault();
            var VisitTypeNew = CMPSContext.OneLineMaster.AsNoTracking().Where(c => c.ParentDescription == "New" && c.ParentTag == "TOV").Select(c => c.OLMID).FirstOrDefault();
            var dateef = WYNKContext.RegistrationTran.Where(x => x.UIN == RegNO && x.CmpID == CmpID && x.PatientVisitType == Convert.ToString(VisitTypeREV)).Select(c => c.RegistrationTranID).FirstOrDefault();

            if (dateef != 0)
            {
                BusinessRule.EFMdate = WYNKContext.RegistrationTran.AsNoTracking().Where(x => x.UIN == RegNO && x.CmpID == CmpID && x.PatientVisitType == Convert.ToString(VisitTypeREV)).Select(c => c.DateofVisit).FirstOrDefault();
            }

            else
            {
                BusinessRule.EFMdate = WYNKContext.RegistrationTran.AsNoTracking().Where(x => x.UIN == RegNO && x.CmpID == CmpID && x.PatientVisitType == Convert.ToString(VisitTypeNew)).Select(c => c.DateofVisit).FirstOrDefault();
            }



            if (Activedata == "ActivedataBR")
            {

                BusinessRule.BRuleActBR = (from BR in WYNKContext.BussinessRule.Where(x => x.CMPID == CmpID && x.ModuleDescription == "Registration" && x.IsActive == true)
                                           join BRT in WYNKContext.BussinessRuleTran on BR.ID equals BRT.BRID
                                           where BRT.IsActive == true
                                           select new BusinessRuleAct
                                           {
                                               WEF = BR.WEF,
                                               From = BRT.From,
                                               TO = BRT.TO,
                                               Amount = BRT.Amount,
                                               NoofDays = BRT.NoofDays,
                                               CountOfRegTran = WYNKContext.RegistrationTran.Where(x => x.UIN == RegNO).Count() - 1,
                                               EffectiveDate = BRT.EffectiveDate,
                                           }
                                        ).AsNoTracking().ToList();

                return BusinessRule;
            }






            var regdate = WYNKContext.RegistrationTran.Where(x => x.UIN == RegNO && x.CmpID == CmpID).Select(c => c.DateofVisit).LastOrDefault();

            var surgeryID = WYNKContext.SurgeryTran.Where(x => x.SurgeryID ==
            WYNKContext.Surgery.Where(s => s.AdmID == WYNKContext.Admission.Where(c => c.IsSurgeryCompleted == true && c.DischargeID != null && c.UIN == RegNO && c.CMPID == CmpID)
            .Select(q => q.RandomUniqueID).FirstOrDefault())
            .Select(w => w.RandomUniqueID).FirstOrDefault())
            .Select(r => r.ICDCode).FirstOrDefault();

            BusinessRule.surgeryID = surgeryID;



            var Specialitycode = WYNKContext.ICDSpecialityCode.AsNoTracking().Where(x => x.ID == WYNKContext.ICDMaster.Where(z => z.ICDCODE == surgeryID).Select(c => c.SpecialityCode).FirstOrDefault()).Select(v => v.ID).FirstOrDefault();


            var BURID = WYNKContext.BussinessRuleTran.AsNoTracking().Where(x => x.CreatedUTC.Date <= regdate.Date).Select(x => x.ID).LastOrDefault();


            if (BURID != 0)
            {
                if (surgeryID != null)
                {
                    var brid1 = WYNKContext.BussinessRule.AsNoTracking().Where(x => x.ICDCODE == Convert.ToString(Specialitycode) && x.CMPID == CmpID).Select(c => c.ID).FirstOrDefault();

                    if (brid1 == 0)
                    {
                        BusinessRule.Message = "Business Rule not implemented for Surgery Speciality";
                        return BusinessRule;
                    }


                    DateTime EFFDate2 = WYNKContext.BussinessRuleTran.Where(x => x.BRID == brid1 && x.EffectiveDate <= regdate).Select(x => x.CreatedUTC).LastOrDefault();
                    var efdateYMHm1 = new DateTime(EFFDate2.Year, EFFDate2.Month, EFFDate2.Day, EFFDate2.Hour, EFFDate2.Minute, 0);
                    BusinessRule.BRule1 = (from BR in WYNKContext.BussinessRule.Where(x => x.CMPID == CmpID && x.ID == brid1 && x.IsActive == true)
                                           join BRT in WYNKContext.BussinessRuleTran on BR.ID equals BRT.BRID
                                           where new DateTime(BRT.CreatedUTC.Year, BRT.CreatedUTC.Month, BRT.CreatedUTC.Day, BRT.CreatedUTC.Hour, BRT.CreatedUTC.Minute, 0) == efdateYMHm1
                                           select new Business_Rule1
                                           {
                                               WEF = BR.WEF,
                                               From = BRT.From,
                                               TO = BRT.TO,
                                               Amount = BRT.Amount,
                                               NoofDays = BRT.NoofDays,
                                               CountOfRegTran = WYNKContext.RegistrationTran.Where(x => x.UIN == RegNO).Count() - 1,
                                               EffectiveDate = BRT.EffectiveDate,
                                           }
                                           ).AsNoTracking().ToList();

                }
                else
                {
                    var brid2 = WYNKContext.BussinessRule.AsNoTracking().Where(x => x.ModuleDescription == "Registration" && x.CMPID == CmpID).Select(c => c.ID).FirstOrDefault();
                    DateTime EFFDate3 = WYNKContext.BussinessRuleTran.AsNoTracking().Where(x => x.BRID == brid2 && x.EffectiveDate <= regdate).Select(x => x.CreatedUTC).LastOrDefault();
                    var efdateYMHm2 = new DateTime(EFFDate3.Year, EFFDate3.Month, EFFDate3.Day, EFFDate3.Hour, EFFDate3.Minute, 0);

                    BusinessRule.BRule = (from BR in WYNKContext.BussinessRule.Where(x => x.CMPID == CmpID && x.ID == brid2 && x.IsActive == true)
                                          join BRT in WYNKContext.BussinessRuleTran on BR.ID equals BRT.BRID
                                          where new DateTime(BRT.CreatedUTC.Year, BRT.CreatedUTC.Month, BRT.CreatedUTC.Day, BRT.CreatedUTC.Hour, BRT.CreatedUTC.Minute, 0) == efdateYMHm2
                                          select new Business_Rule
                                          {
                                              WEF = BR.WEF,
                                              From = BRT.From,
                                              TO = BRT.TO,
                                              Amount = BRT.Amount,
                                              NoofDays = BRT.NoofDays,
                                              CountOfRegTran = WYNKContext.RegistrationTran.Where(x => x.UIN == RegNO).Count() - 1,
                                              EffectiveDate = BRT.EffectiveDate,
                                          }
                                      ).AsNoTracking().ToList();
                }

            }

            else
            {
                BusinessRule.Message = "Business Rule not implemented";
                return BusinessRule;

            }



            return BusinessRule;
        }

        public RegistrationMasterI Getkindetail(string uin)
        {
            var kindetails = new RegistrationMasterI();
            kindetails.KinNDetails = (from KINDetails in WYNKContext.PatientKINDetails.Where(x => x.UIN == uin)

                                      select new PatientKINDetails
                                      {
                                          UIN = KINDetails.UIN,
                                          Relationship = KINDetails.Relationship,
                                          FirstName = KINDetails.FirstName,
                                          MiddleName = KINDetails.MiddleName,
                                          LastName = KINDetails.LastName,
                                          ContactNumber = KINDetails.ContactNumber,
                                          EmailID = KINDetails.EmailID,
                                          PrimaryKinId = KINDetails.PrimaryKinId,
                                      }
                                      ).AsNoTracking().ToList();
            return kindetails;
        }
        public RegistrationMasterI Deletekin(int cmpid, string uin, string Relationship)
        {
            var Dkindetails = new RegistrationMasterI();
            var kinD = WYNKContext.PatientKINDetails.Where(x => x.UIN == uin && x.CmpID == cmpid).ToList();
            WYNKContext.PatientKINDetails.RemoveRange(kinD);
            WYNKContext.SaveChanges();
            return Dkindetails;
        }
        public RegistrationMasterI ForeignNational()
        {
            var ForeignNationaldetails = new RegistrationMasterI();
            ForeignNationaldetails.ForNationaldetails = (from BRT in WYNKContext.ForeignNational
                                                         select new ForNationaldetails
                                                         {
                                                             ID = BRT.ID,
                                                             IsForeignNational = BRT.IsForeignNational,
                                                             NormalFee = BRT.NormalFee,
                                                             SuperSpecialityFee = BRT.SuperSpecialityFee,

                                                         }).AsNoTracking().ToList();
            return ForeignNationaldetails;
        }
        public RegistrationMasterI GetRegistrationExtension(string RegNO, int CmpID)
        {
            var RegistrationExtension = new RegistrationMasterI();
            RegistrationExtension.GETRegExtension = (from REGE in WYNKContext.RegistrationExtension.Where(x => x.UIN == RegNO && x.CMPID == CmpID)
                                                     select new GETRegExtension
                                                     {
                                                         Artificialeye = REGE.Artificialeye,
                                                         OU = REGE.OU,
                                                         OD = REGE.OD,
                                                         OS = REGE.OS,

                                                     }).AsNoTracking().ToList();
            return RegistrationExtension;
        }
        public RegistrationMasterI GetServiceamount(int CmpID, int docid)
        {
            var RegistrationExtension = new RegistrationMasterI();
            RegistrationExtension.Registrationcharges = WYNKContext.ServiceMaster.Where(x => x.Tag == "REG" && x.IsActive == true && x.CMPID == CmpID).Select(x => x.TotalAmount).FirstOrDefault();
            RegistrationExtension.Concharges = WYNKContext.ServiceMaster.Where(x => x.Tag == "CON" && x.IsActive == true && x.CMPID == CmpID && x.DoctorID == docid).Select(x => x.TotalAmount).FirstOrDefault();
            return RegistrationExtension;
        }
        public RegistrationMasterI GetlocationDetails(int id)
        {
            var locDetails = new RegistrationMasterI();
            try
            {
                Console.WriteLine("Entered into method");
                
                var des = CMPSContext.LocationMaster.Where(x => x.ID == id).Select(x => x.ParentDescription).FirstOrDefault();
                var stateid = CMPSContext.LocationMaster.Where(x => x.ParentDescription == des).Select(x => x.ParentID).FirstOrDefault();
                locDetails.ParentDescriptionstate = CMPSContext.LocationMaster.Where(x => x.ID == stateid).Select(x => x.ParentDescription).FirstOrDefault();
                Console.WriteLine("Middle Line Updated");
                var countryid = CMPSContext.LocationMaster.Where(x => x.ParentDescription == locDetails.ParentDescriptionstate).Select(x => x.ParentID).FirstOrDefault();
                locDetails.ParentDescriptioncountry = CMPSContext.LocationMaster.Where(x => x.ID == countryid).Select(x => x.ParentDescription).FirstOrDefault();
                locDetails.ParentDescriptioncountryID = CMPSContext.LocationMaster.Where(x => x.ID == countryid).Select(x => x.ParentID).FirstOrDefault();
                Console.WriteLine("Ending Line Updated");
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.InnerException.Message);
            }
            return locDetails;
        }
        public RegistrationMasterI GetPinCodeDetails(int location)
        {
            var PinCode = new RegistrationMasterI();
            PinCode.ParentDescriptionPinCode = CMPSContext.LocationMaster.Where(x => x.ID == location).Select(x => x.Pincode).FirstOrDefault();
            return PinCode;
        }
        public dynamic Getreviewpatient(int CmpID, string PhoneNO, string Gender)
        {
            var PhoneNONumber = WYNKContext.Registration.Where(x => x.Phone == PhoneNO && x.Gender == Gender && x.CMPID == CmpID).Select(x => x.Phone).FirstOrDefault();

            if (PhoneNONumber != null)
            {
                return new
                {
                    Success = false,
                    Message = "Phone Number already exists",
                    UIN = PhoneNONumber,
                };
            }

            return new
            {
                Success = true,

            };

        }
        public dynamic InsertregMas(RegistrationMasterI AddReg, int userid, int TransactionTypeid, int ContraTransactionid)
        {
            var registrationMaster = new Registration_Master();
            var registrationTranMaster = new RegistrationTran_Master();
            var RegistrationExtension = new RegistrationExtension();
            var ManagementMaster = new ManagementMaster();

            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {

                    var billtype = CMPSContext.Setup.Where(x => x.CMPID == AddReg.RegistrationMaster.CMPID).Select(x => x.BillingType).FirstOrDefault();
                    if (billtype == "Consolidated Billing" && AddReg.Regcharges != 0)
                    {

                        var respa = (from pa in WYNKContext.PatientAccount.Where(x => x.UIN == AddReg.RegistrationMaster.UIN && x.InvoiceNumber == null && x.CMPID == AddReg.RegistrationMaster.CMPID && x.TransactionID == AddReg.billtranid)
                                     select new
                                     {
                                         ret = pa.UIN.Count(),

                                     }).ToList();

                        var PAID = WYNKContext.PatientAccount.Where(x => x.UIN == AddReg.RegistrationMaster.UIN && x.InvoiceNumber == null && x.CMPID == AddReg.RegistrationMaster.CMPID && x.TransactionID == AddReg.billtranid).Select(x => x.PAID).LastOrDefault();
                        var childid = WYNKContext.ServiceMaster.Where(x => x.Tag == "REG" && x.IsActive == true && x.CMPID == AddReg.RegistrationMaster.CMPID).Select(x => x.ChildDescription).FirstOrDefault();
                        var ServiceTypeID = WYNKContext.Services.Where(x => x.ID == Convert.ToInt32(childid) && x.CMPID == AddReg.RegistrationMaster.CMPID).Select(x => x.ID).FirstOrDefault();
                        var childdesc = WYNKContext.Services.Where(x => x.ID == Convert.ToInt32(childid) && x.CMPID == AddReg.RegistrationMaster.CMPID).Select(x => x.Description).FirstOrDefault();
                        var invespa = new PatientAccount();

                        if (respa.Count == 0)
                        {

                            invespa.CMPID = AddReg.RegistrationMaster.CMPID;
                            invespa.UIN = AddReg.RegistrationMaster.UIN;
                            invespa.TotalProductValue = AddReg.Regcharges;
                            invespa.TotalBillValue = AddReg.Regcharges;
                            invespa.CreatedUTC = DateTime.UtcNow;
                            invespa.CreatedBy = userid;
                            invespa.TransactionID = AddReg.billtranid;
                            WYNKContext.PatientAccount.AddRange(invespa);
                            WYNKContext.SaveChanges();
                        }

                        else
                        {

                            var masters1 = WYNKContext.PatientAccount.Where(x => x.UIN == AddReg.RegistrationMaster.UIN).LastOrDefault();
                            masters1.TotalProductValue += AddReg.Regcharges;
                            masters1.TotalBillValue += AddReg.Regcharges;
                            masters1.UpdatedUTC = DateTime.UtcNow;
                            masters1.UpdatedBy = userid;
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
                        var respa1 = WYNKContext.PatientAccountDetail.Where(x => x.PAID == PAID && x.ServiceTypeID == ServiceTypeID).ToList();
                        if (respa1.Count == 0)
                        {
                            var patactdt = new PatientAccountDetail();
                            patactdt.PAID = PAID;
                            patactdt.OLMID = ServiceTypeID;
                            patactdt.ServiceTypeID = ServiceTypeID;
                            patactdt.ServiceDescription = childdesc;
                            patactdt.Description = childdesc;
                            patactdt.TotalProductValue = AddReg.Regcharges;
                            patactdt.TotalBillValue = AddReg.Regcharges;
                            patactdt.CreatedUTC = DateTime.UtcNow;
                            patactdt.CreatedBy = userid;
                            WYNKContext.PatientAccountDetail.AddRange(patactdt);
                            WYNKContext.SaveChanges();
                        }
                        else
                        {
                            var masters = WYNKContext.PatientAccountDetail.Where(x => x.PAID == PAID && x.ServiceTypeID == ServiceTypeID).LastOrDefault();
                            masters.TotalProductValue += AddReg.Regcharges;
                            masters.TotalBillValue += AddReg.Regcharges;
                            masters.UpdatedUTC = DateTime.UtcNow;
                            masters.UpdatedBy = userid;
                            WYNKContext.PatientAccountDetail.UpdateRange(masters);

                        }



                        WYNKContext.SaveChanges();

                    }

                    if (billtype == "Consolidated Billing" && AddReg.M_Consultationcharges != 0)
                    {

                        var respa = (from pa in WYNKContext.PatientAccount.Where(x => x.UIN == AddReg.RegistrationMaster.UIN && x.InvoiceNumber == null && x.CMPID == AddReg.RegistrationMaster.CMPID && x.TransactionID == AddReg.billtranid)
                                     select new
                                     {
                                         ret = pa.UIN.Count(),

                                     }).ToList();

                        var PAID = WYNKContext.PatientAccount.Where(x => x.UIN == AddReg.RegistrationMaster.UIN && x.InvoiceNumber == null && x.CMPID == AddReg.RegistrationMaster.CMPID && x.TransactionID == AddReg.billtranid).Select(x => x.PAID).LastOrDefault();
                        var childid = WYNKContext.ServiceMaster.Where(x => x.Tag == "CON" && x.IsActive == true && x.CMPID == AddReg.RegistrationMaster.CMPID).Select(x => x.ChildDescription).FirstOrDefault();
                        var ServiceTypeID = WYNKContext.Services.Where(x => x.ID == Convert.ToInt32(childid) && x.CMPID == AddReg.RegistrationMaster.CMPID).Select(x => x.ID).FirstOrDefault();
                        var childdesc = WYNKContext.Services.Where(x => x.ID == Convert.ToInt32(childid) && x.CMPID == AddReg.RegistrationMaster.CMPID).Select(x => x.Description).FirstOrDefault();

                        var invespa = new PatientAccount();

                        if (respa.Count == 0)
                        {

                            invespa.CMPID = AddReg.RegistrationMaster.CMPID;
                            invespa.UIN = AddReg.RegistrationMaster.UIN;
                            invespa.TotalProductValue = AddReg.M_Consultationcharges;
                            invespa.TotalBillValue = AddReg.M_Consultationcharges;
                            invespa.CreatedUTC = DateTime.UtcNow;
                            invespa.CreatedBy = userid;
                            invespa.TransactionID = AddReg.billtranid;
                            WYNKContext.PatientAccount.AddRange(invespa);
                            WYNKContext.SaveChanges();
                        }

                        else
                        {

                            var masters1 = WYNKContext.PatientAccount.Where(x => x.UIN == AddReg.RegistrationMaster.UIN).LastOrDefault();
                            masters1.TotalProductValue += AddReg.M_Consultationcharges;
                            masters1.TotalBillValue += AddReg.M_Consultationcharges;
                            masters1.UpdatedUTC = DateTime.UtcNow;
                            masters1.UpdatedBy = userid;
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
                        var respa1 = WYNKContext.PatientAccountDetail.Where(x => x.PAID == PAID && x.ServiceTypeID == ServiceTypeID).ToList();
                        if (respa1.Count == 0)
                        {
                            var patactdt = new PatientAccountDetail();
                            patactdt.PAID = PAID;
                            patactdt.OLMID = ServiceTypeID;
                            patactdt.ServiceTypeID = ServiceTypeID;
                            patactdt.ServiceDescription = childdesc;
                            patactdt.Description = childdesc;
                            patactdt.TotalProductValue = AddReg.M_Consultationcharges;
                            patactdt.TotalBillValue = AddReg.M_Consultationcharges;
                            patactdt.CreatedUTC = DateTime.UtcNow;
                            patactdt.CreatedBy = userid;
                            WYNKContext.PatientAccountDetail.AddRange(patactdt);
                            WYNKContext.SaveChanges();
                        }
                        else
                        {
                            var masters = WYNKContext.PatientAccountDetail.Where(x => x.PAID == PAID && x.ServiceTypeID == ServiceTypeID).LastOrDefault();
                            masters.TotalProductValue += AddReg.M_Consultationcharges;
                            masters.TotalBillValue += AddReg.M_Consultationcharges;
                            masters.UpdatedUTC = DateTime.UtcNow;
                            masters.UpdatedBy = userid;
                            WYNKContext.PatientAccountDetail.UpdateRange(masters);

                        }


                        WYNKContext.SaveChanges();

                    }

                    if (AddReg.RegistrationMaster.AadharNumber != null)
                    {
                        var AadharNumber = WYNKContext.Registration.Where(x => x.AadharNumber == AddReg.RegistrationMaster.AadharNumber).Select(x => x.UIN).FirstOrDefault();
                        if (AadharNumber != null)
                        {
                            return new
                            {
                                Success = false,
                                Message = "Aadhar Number already exists"
                            };
                        }

                    }
                    if (AddReg.RegistrationMaster.PanCardNo != null)
                    {
                        var PanCardNo = WYNKContext.Registration.Where(x => x.PanCardNo == AddReg.RegistrationMaster.PanCardNo).Select(x => x.UIN).FirstOrDefault();
                        if (PanCardNo != null)
                        {
                            return new
                            {
                                Success = false,
                                Message = "PanCard Number already exists"
                            };
                        }

                    }
                    if (AddReg.RegistrationMaster.DrivingLicenseNo != null)
                    {
                        var DrivingLicenseNo = WYNKContext.Registration.Where(x => x.DrivingLicenseNo == AddReg.RegistrationMaster.DrivingLicenseNo).Select(x => x.UIN).FirstOrDefault();
                        if (DrivingLicenseNo != null)
                        {
                            return new
                            {
                                Success = false,
                                Message = "Driving License Number already exists"
                            };
                        }

                    }
                    if (AddReg.RegistrationMaster.PassportNo != null)
                    {
                        var PassportNo = WYNKContext.Registration.Where(x => x.PassportNo == AddReg.RegistrationMaster.PassportNo).Select(x => x.UIN).FirstOrDefault();
                        if (PassportNo != null)
                        {
                            return new
                            {
                                Success = false,
                                Message = "Passport Number already exists"
                            };
                        }

                    }
                    if (AddReg.RegistrationMaster.EmailID != null)
                    {
                        var EmailID = WYNKContext.Registration.Where(x => x.EmailID == AddReg.RegistrationMaster.EmailID).Select(x => x.UIN).FirstOrDefault();
                        if (EmailID != null)
                        {
                            return new
                            {
                                Success = false,
                                Message = "EmailID  already exists"
                            };
                        }

                    }

                    if (AddReg.RegistrationMaster.RegType == "Appointment")
                    {
                        var AppReg = new Appointment();
                        AppReg = WYNKContext.Appointment.Where(x => x.UIN == AddReg.AppUIN).FirstOrDefault();
                        AppReg.IsVisited = true;
                        AppReg.UpdatedBy = userid;
                        AppReg.UpdatedUTC = DateTime.UtcNow;
                        WYNKContext.Entry(AppReg).State = EntityState.Modified;
                        WYNKContext.SaveChanges();

                    }

                    if (AddReg.RegistrationMaster.RegType == "CampRegistration")
                    {
                        var CampReg = new CampRegistration();
                        CampReg = WYNKContext.CampRegistration.Where(x => x.CampUIN == AddReg.CampUIN).FirstOrDefault();
                        CampReg.IsVisited = true;
                        CampReg.UpdatedBy = userid;
                        CampReg.UpdatedUTC = DateTime.UtcNow;
                        WYNKContext.Entry(CampReg).State = EntityState.Modified;
                        WYNKContext.SaveChanges();
                        registrationMaster.UIN = AddReg.CampUIN;
                    }
                    else
                    {
                        registrationMaster.UIN = AddReg.RegistrationMaster.UIN;
                    }


                    registrationMaster.RegType = AddReg.RegistrationMaster.RegType;



                    if (AddReg.RegistrationMaster.DateofRegistration.Date == DateTime.UtcNow.Date)
                    { registrationMaster.DateofRegistration = AddReg.RegistrationMaster.DateofRegistration; }
                    else { registrationMaster.DateofRegistration = AddReg.RegistrationMaster.DateofRegistration.AddDays(1); }



                    registrationMaster.CMPID = AddReg.RegistrationMaster.CMPID;
                    registrationMaster.Name = AddReg.RegistrationMaster.Name;
                    registrationMaster.MiddleName = AddReg.RegistrationMaster.MiddleName;
                    registrationMaster.LastName = AddReg.RegistrationMaster.LastName;
                    registrationMaster.PreferredLanguage = AddReg.RegistrationMaster.PreferredLanguage;
                    registrationMaster.MaritalStatus = AddReg.RegistrationMaster.MaritalStatus;
                    registrationMaster.Occupation = AddReg.RegistrationMaster.Occupation;
                    registrationMaster.DateofBirth = AddReg.RegistrationMaster.DateofBirth.AddDays(1);
                    registrationMaster.Gender = AddReg.RegistrationMaster.Gender;
                    registrationMaster.Address1 = AddReg.RegistrationMaster.Address1;
                    registrationMaster.Address2 = AddReg.RegistrationMaster.Address2;
                    registrationMaster.Address3 = AddReg.RegistrationMaster.Address3;
                    registrationMaster.LocationID = AddReg.RegistrationMaster.LocationID;
                    registrationMaster.FatherHusbandName = AddReg.RegistrationMaster.FatherHusbandName;
                    registrationMaster.EmailID = AddReg.RegistrationMaster.EmailID;
                    registrationMaster.Phone = AddReg.RegistrationMaster.Phone;
                    registrationMaster.AlternateMailID = AddReg.RegistrationMaster.AlternateMailID;
                    registrationMaster.AlternatePhoneNumber = AddReg.RegistrationMaster.AlternatePhoneNumber;
                    registrationMaster.TransactionID = TransactionTypeid;
                    registrationMaster.AadharNumber = AddReg.RegistrationMaster.AadharNumber;
                    registrationMaster.PanCardNo = AddReg.RegistrationMaster.PanCardNo;
                    registrationMaster.DrivingLicenseNo = AddReg.RegistrationMaster.DrivingLicenseNo;
                    registrationMaster.PassportNo = AddReg.RegistrationMaster.PassportNo;
                    registrationMaster.IsForeignNational = AddReg.RegistrationMaster.IsForeignNational;
                    registrationMaster.Insurance = AddReg.RegistrationMaster.Insurance;
                    registrationMaster.SourceofReferralID = AddReg.RegistrationMaster.SourceofReferralID;
                    registrationMaster.ReferralName = AddReg.RegistrationMaster.ReferralName;
                    registrationMaster.CreatedBy = userid;
                    registrationMaster.IsDeleted = false;
                    registrationMaster.CreatedUTC = DateTime.UtcNow;
                    registrationMaster.OLDUIN = "";
                    WYNKContext.Registration.Add(registrationMaster);

                    string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == userid).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();

                    ErrorLog oErrorLogs = new ErrorLog();
                    object namestr = registrationMaster;
                    oErrorLogs.WriteErrorLogTitle(AddReg.Companyname, "Registration", "User name :", username, "User ID :", Convert.ToString(userid), "Mode : Add");

                    var tag = CMPSContext.Users.Where(x => x.Userid == userid).Select(x => x.ReferenceTag).FirstOrDefault();

                    var usernamedetails = CMPSContext.Users.Where(x => x.Userid == userid).Select(x => x.Username).FirstOrDefault();
                    var username2 = "";
                    if (tag == "A")
                    {
                        username2 = "Admin";
                    }
                    else if (tag == "V" || tag == "O" || tag == "D")
                    {
                        username2 = CMPSContext.DoctorMaster.Where(x => x.EmailID == usernamedetails).Select(x => x.Firstname + " " + x.MiddleName + " " + x.LastName).FirstOrDefault();
                    }
                    else if (tag == "E" || tag == "R")
                    {
                        var usernameempid = CMPSContext.EmployeeCommunication.Where(x => x.EmailID == usernamedetails).Select(x => x.EmpID).FirstOrDefault();
                        username2 = CMPSContext.Employee.Where(x => x.EmployeeID == usernameempid).Select(x => x.LastName).FirstOrDefault();
                    }


                    registrationTranMaster.UIN = registrationMaster.UIN;
                    registrationTranMaster.CmpID = AddReg.RegistrationMaster.CMPID;
                    registrationTranMaster.DateofVisit = registrationMaster.DateofRegistration;
                    registrationTranMaster.TypeofVisit = AddReg.RegistrationTranMaster.TypeofVisit;
                    registrationTranMaster.Status = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Open").Select(x => x.OLMID).FirstOrDefault();
                    registrationTranMaster.StatusDateTime = registrationMaster.DateofRegistration;
                    registrationTranMaster.Remarks = AddReg.RegistrationTranMaster.Remarks;
                    registrationTranMaster.RegistrationFees = AddReg.RegistrationTranMaster.RegistrationFees;
                    if (AddReg.RegistrationMaster.RegType == "Appointment")
                    {
                        registrationTranMaster.ConsulationFees = AddReg.RegistrationTranMaster.ConsulationFees;
                    }


                    registrationMaster.IsDeleted = false;
                    registrationTranMaster.CreatedUTC = DateTime.UtcNow;
                    registrationTranMaster.CreatedBy = userid;

                    var usserid = AddReg.RegistrationTranMaster.DoctorID;
                    var reftag = CMPSContext.Users.Where(x => x.Userid == usserid).Select(x => x.ReferenceTag).FirstOrDefault();

                    if (reftag == "D" || reftag == "O")
                    {
                        var emailid = CMPSContext.Users.Where(x => x.Userid == usserid).Select(x => x.Username).FirstOrDefault();
                        var docid = CMPSContext.DoctorMaster.Where(x => x.EmailID == emailid).Select(x => x.DoctorID).FirstOrDefault();
                        registrationTranMaster.DoctorID = docid;
                    }
                    else
                    {
                        registrationTranMaster.DoctorID = AddReg.RegistrationTranMaster.DoctorID;
                    }

                    registrationTranMaster.ReviewCount = 0;
                    registrationTranMaster.CmpID = AddReg.RegistrationMaster.CMPID;
                    registrationTranMaster.PatientVisitType = Convert.ToString(CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "New").Select(x => x.OLMID).FirstOrDefault());
                    WYNKContext.RegistrationTran.Add(registrationTranMaster);
                    ErrorLog oErrorLogstran = new ErrorLog();
                    object namestr1 = registrationTranMaster;
                    oErrorLogstran.WriteErrorLogArray("RegistrationTran", namestr);
                    WYNKContext.SaveChanges();
                    var RegTranID = registrationTranMaster.RegistrationTranID;
                    var ccmpid = AddReg.RegistrationMaster.CMPID;
                    var OLMID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Assigned").Select(x => x.OLMID).FirstOrDefault();
                    var Referncetag = CMPSContext.Users.Where(x => x.Userid == userid).Select(x => x.ReferenceTag).FirstOrDefault();
                    var utctime = CMPSContext.Setup.Where(x => x.CMPID == ccmpid).Select(x => x.UTCTime).FirstOrDefault();
                    TimeSpan ts = TimeSpan.Parse(utctime);
                    if (Referncetag == "D" || Referncetag == "O")
                    {
                        var emailid = CMPSContext.Users.Where(x => x.Userid == userid).Select(x => x.Username).FirstOrDefault();
                        var docid = CMPSContext.DoctorMaster.Where(x => x.EmailID == emailid).Select(x => x.DoctorID).FirstOrDefault();

                        var Assigningtable = new Patient_Assign();
                        Assigningtable.RegistrationTranID = RegTranID;
                        Assigningtable.AssignedDate = DateTime.UtcNow + ts;
                        Assigningtable.DoctorID = docid;
                        Assigningtable.IsCancelled = false;
                        Assigningtable.CreatedUTC = DateTime.UtcNow;
                        Assigningtable.CreatedBy = userid;
                        Assigningtable.Patientallocatestatus = null;
                        var idd = WYNKContext.PatientAssign.Select(x => x.ID).LastOrDefault();
                        if (idd < 1)
                        {
                            Assigningtable.ID = 1;
                        }
                        else
                        {
                            Assigningtable.ID = idd + 1;
                        }
                        WYNKContext.PatientAssign.Add(Assigningtable);
                        var Regtrandetails = WYNKContext.RegistrationTran.Where(x => x.RegistrationTranID == RegTranID).FirstOrDefault();
                        Regtrandetails.Status = OLMID;
                        Regtrandetails.DoctorID = docid;
                        Regtrandetails.AssignedDate = DateTime.UtcNow + ts;
                        WYNKContext.RegistrationTran.UpdateRange(Regtrandetails);
                        WYNKContext.SaveChanges();


                        var workflow = new Workflowtran();
                        var CurDate = DateTime.UtcNow;
                        var times1 = CurDate.Hour + ":" + CurDate.Minute + ":" + CurDate.Second;
                        var times2 = ts;
                        var fulltime = TimeSpan.Parse(times1) + times2;
                        DateTime time = DateTime.Today.Add(fulltime);
                        var curfullh = time.Hour;
                        var curfullm = time.Minute;
                        string displayTimes = curfullh + ":" + curfullm;
                        workflow.Checkintime = displayTimes;
                        workflow.Checkouttime = "";
                        workflow.CMPID = AddReg.RegistrationMaster.CMPID;
                        workflow.Createdutc = DateTime.UtcNow;
                        if (Referncetag == "O")
                        {
                            workflow.Workflowmasterid = WYNKContext.Workflowmaster.Where(x => x.Description == "Optometrist").Select(x => x.ID).FirstOrDefault();
                            workflow.Optometrist = true;
                            workflow.Doctor = false;
                        }
                        else if (Referncetag == "D")
                        {
                            workflow.Workflowmasterid = WYNKContext.Workflowmaster.Where(x => x.Description == "Doctor").Select(x => x.ID).FirstOrDefault();
                            workflow.Optometrist = false;
                            workflow.Doctor = true;
                        }
                        workflow.status = true;
                        workflow.Reception = false;
                        workflow.Doctorid = docid;
                        workflow.Date = DateTime.UtcNow.Date;
                        workflow.Regtranid = RegTranID;
                        workflow.Createdby = userid;
                        WYNKContext.Workflowtran.Add(workflow);
                        WYNKContext.SaveChanges();


                    }
                    else if (Referncetag == "R" || Referncetag == "A")
                    {
                        var Assigningtable = new Patient_Assign();
                        Assigningtable.RegistrationTranID = RegTranID;
                        Assigningtable.AssignedDate = DateTime.UtcNow + ts;
                        Assigningtable.DoctorID = 0;
                        Assigningtable.IsCancelled = false;
                        Assigningtable.CreatedUTC = DateTime.UtcNow;
                        Assigningtable.CreatedBy = userid;
                        Assigningtable.Patientallocatestatus = "D";
                        var idd = WYNKContext.PatientAssign.Select(x => x.ID).LastOrDefault();
                        if (idd < 1)
                        {
                            Assigningtable.ID = 1;
                        }
                        else
                        {
                            Assigningtable.ID = idd + 1;
                        }
                        WYNKContext.PatientAssign.Add(Assigningtable);
                        WYNKContext.SaveChanges();
                        var Assigningtables = new Patient_Assign();
                        Assigningtables.RegistrationTranID = RegTranID;
                        Assigningtables.AssignedDate = DateTime.UtcNow + ts;
                        Assigningtables.DoctorID = 0;
                        Assigningtables.IsCancelled = false;
                        Assigningtables.CreatedUTC = DateTime.UtcNow;
                        Assigningtables.CreatedBy = userid;
                        Assigningtables.Patientallocatestatus = "O";
                        var idds = WYNKContext.PatientAssign.Select(x => x.ID).LastOrDefault();
                        if (idds < 1)
                        {
                            Assigningtables.ID = 1;
                        }
                        else
                        {
                            Assigningtables.ID = idds + 1;
                        }
                        WYNKContext.PatientAssign.Add(Assigningtables);
                        WYNKContext.SaveChanges();


                        var workflow = new Workflowtran();
                        var CurDate = DateTime.UtcNow;
                        var times1 = CurDate.Hour + ":" + CurDate.Minute + ":" + CurDate.Second;
                        var times2 = ts;
                        var fulltime = TimeSpan.Parse(times1) + times2;
                        DateTime time = DateTime.Today.Add(fulltime);
                        var curfullh = time.Hour;
                        var curfullm = time.Minute;
                        string displayTimes = curfullh + ":" + curfullm;
                        workflow.Checkintime = displayTimes;
                        workflow.Checkouttime = "";
                        workflow.CMPID = AddReg.RegistrationMaster.CMPID;
                        workflow.Createdutc = DateTime.UtcNow;
                        workflow.Workflowmasterid = WYNKContext.Workflowmaster.Where(x => x.Description == "Reception").Select(x => x.ID).FirstOrDefault();
                        workflow.Optometrist = false;
                        workflow.Reception = true;
                        workflow.Doctor = false;
                        workflow.status = true;
                        workflow.Doctorid = 0;
                        workflow.Date = DateTime.UtcNow.Date;
                        workflow.Regtranid = RegTranID;
                        workflow.Createdby = userid;
                        WYNKContext.Workflowtran.Add(workflow);
                        WYNKContext.SaveChanges();


                    }


                    if (AddReg.RegistrationExtension.Artificialeye == true)
                    {
                        RegistrationExtension.UIN = registrationMaster.UIN;
                        RegistrationExtension.Artificialeye = AddReg.RegistrationExtension.Artificialeye;
                        RegistrationExtension.OD = AddReg.RegistrationExtension.OD;
                        RegistrationExtension.OU = AddReg.RegistrationExtension.OU;
                        RegistrationExtension.OS = AddReg.RegistrationExtension.OS;
                        RegistrationExtension.CMPID = AddReg.RegistrationMaster.CMPID;
                        RegistrationExtension.CreatedUTC = DateTime.UtcNow;
                        RegistrationExtension.CreatedBy = userid;
                        WYNKContext.RegistrationExtension.Add(RegistrationExtension);
                        ErrorLog oErrorLogstranEX = new ErrorLog();
                        object namestrEX = RegistrationExtension;
                        oErrorLogstran.WriteErrorLogArray("RegistrationExtension", namestrEX);
                        WYNKContext.SaveChanges();
                    }

                    foreach (var item in AddReg.Patientlist.ToList())
                    {
                        var kindetails = new PatientKINDetails();
                        kindetails.UIN = registrationMaster.UIN;
                        kindetails.Relationship = item.Relationship;
                        kindetails.FirstName = item.FirstName;
                        kindetails.LastName = item.LastName;
                        kindetails.ContactNumber = item.ContactNumber;
                        kindetails.EmailID = item.EmailID;
                        kindetails.PrimaryKinId = item.PrimaryKinId;
                        kindetails.CmpID = AddReg.RegistrationMaster.CMPID;
                        kindetails.CreatedUTC = DateTime.UtcNow;
                        kindetails.CreatedBy = userid;
                        WYNKContext.PatientKINDetails.Add(kindetails);
                        ErrorLog oErrorLogstranKIN = new ErrorLog();
                        object namestrKIN = kindetails;
                        oErrorLogstran.WriteErrorLogArray("PatientKINDetails", namestrKIN);
                        WYNKContext.SaveChanges();
                    }
                    registrationTranMaster = WYNKContext.RegistrationTran.Where(x => x.RegistrationTranID == RegTranID).FirstOrDefault();
                    registrationTranMaster.KinConsentID = WYNKContext.PatientKINDetails.Where(x => x.UIN == registrationMaster.UIN && x.PrimaryKinId == true).Select(x => x.ID).FirstOrDefault();
                    WYNKContext.Entry(registrationTranMaster).State = EntityState.Modified;
                    WYNKContext.SaveChanges();


                    foreach (var item in AddReg.PatientVSInsurance.ToList())
                    {
                        var PVI = new PatientVsInsurance();
                        PVI.UIN = registrationMaster.UIN;
                        PVI.SumAssured = item.SumAssured;
                        PVI.InsurancevsMiddlemenID = item.InsurancevsMiddlemenID;
                        PVI.PolicyName = item.PolicyName;
                        PVI.PolicyNo = item.PolicyNo;
                        PVI.PolicyTakenOn = item.PolicyTakenOn;
                        PVI.PeriodFrom = item.PeriodFrom;
                        PVI.PeriodTo = item.PeriodTo;
                        PVI.IsJointPolicy = false;
                        PVI.IsActive = false;
                        PVI.IsTransacted = false;
                        PVI.AmountAvailed = item.AmountAvailed;
                        PVI.Remarks = item.Remarks;
                        PVI.CreatedUTC = DateTime.UtcNow;
                        PVI.CreatedBy = userid;
                        PVI.CmpID = AddReg.RegistrationMaster.CMPID;
                        WYNKContext.PatientVsInsurance.Add(PVI);
                        WYNKContext.SaveChanges();
                    }
                    foreach (var item in AddReg.PatientVSInsurance.ToList())
                    {
                        var PIT = new PatientInsuranceTran();
                        PIT.UIN = registrationMaster.UIN;
                        PIT.RegTranID = RegTranID;
                        PIT.SumAssured = item.SumAssured;
                        PIT.InsurancevsMiddlemenID = item.InsurancevsMiddlemenID;
                        PIT.PolicyName = item.PolicyName;
                        PIT.PolicyNo = item.PolicyNo;
                        PIT.PolicyTakenOn = item.PolicyTakenOn;
                        PIT.PeriodFrom = item.PeriodFrom;
                        PIT.PeriodTo = item.PeriodTo;
                        PIT.AmountAvailed = item.AmountAvailed;
                        PIT.Remarks = item.Remarks;
                        PIT.CreatedUTC = DateTime.UtcNow;
                        PIT.CreatedBy = userid;
                        PIT.CmpID = AddReg.RegistrationMaster.CMPID;
                        WYNKContext.PatientInsuranceTran.Add(PIT);
                        WYNKContext.SaveChanges();
                    }

                    foreach (var item in AddReg.PatientAssignStatus.ToList())
                    {
                        if (item.Amount == 0)
                        {
                            break;
                        }
                        else
                        {
                            var Payment = new Payment_Master();
                            Payment.UIN = registrationMaster.UIN;
                            Payment.PaymentType = "O";
                            Payment.PaymentMode = item.PaymentMode;
                            Payment.InstrumentNumber = item.InstrumentNumber;
                            Payment.Instrumentdate = item.Instrumentdate;
                            Payment.BankName = item.BankName;
                            Payment.BankBranch = item.BankBranch;
                            Payment.Expirydate = item.Expirydate;
                            Payment.Amount = item.Amount;
                            Payment.CmpID = AddReg.RegistrationMaster.CMPID;
                            Payment.IsBilled = true;
                            Payment.OLMID = null;
                            Payment.ReceiptNumber = AddReg.ContraTransactionid;
                            Payment.ReceiptDate = DateTime.UtcNow;
                            Payment.TransactionID = TransactionTypeid;
                            Payment.Fyear = Convert.ToString(WYNKContext.FinancialYear.Where(x => x.ID == WYNKContext.FinancialYear.Where(b => Convert.ToDateTime(b.FYFrom) <= DateTime.Now && Convert.ToDateTime(b.FYTo) >=
                            DateTime.Now && x.CMPID == AddReg.RegistrationMaster.CMPID && x.IsActive == true).Select(f => f.ID).FirstOrDefault()).Select(c => c.FYAccYear).FirstOrDefault());
                            Payment.CreatedUTC = DateTime.UtcNow;
                            Payment.CreatedBy = userid;
                            WYNKContext.PaymentMaster.Add(Payment);
                            WYNKContext.SaveChanges();
                        }

                    }

                    var datereglocaltime = DateTime.Now;
                    var Date1 = WYNKContext.PatientFootfall.Where(x => x.Date.Date == datereglocaltime.Date).Select(x => x.Date.Date).FirstOrDefault();
                    var Age = PasswordEncodeandDecode.ToAgeString(AddReg.RegistrationMaster.DateofBirth);
                    var result = Regex.Match(Age, @"\d+").Value;
                    var PodiatristAge = CMPSContext.Setup.Where(x => x.CMPID == AddReg.RegistrationMaster.CMPID).Select(x => x.Pediatric).FirstOrDefault();
                    var results = WYNKContext.PatientFootfall.Where(x => x.Date.Date == datereglocaltime.Date).ToList();

                    foreach (var Arrays1 in results)
                    {
                        if (Arrays1.CmpID == AddReg.RegistrationMaster.CMPID)

                        {
                            AddReg.Cmpid = AddReg.RegistrationMaster.CMPID;
                        }
                    }
                    if (datereglocaltime.Date == Date1.Date && registrationMaster.CMPID == AddReg.Cmpid)
                    {
                        ManagementMaster = WYNKContext.PatientFootfall.Where(x => x.Date == Date1 && x.CmpID == AddReg.Cmpid).FirstOrDefault();

                        if (Convert.ToInt32(PodiatristAge) > Convert.ToInt32(result))
                        {
                            if (AddReg.RegistrationExtension.Artificialeye == true)
                            {
                                ManagementMaster.NewPediatricOcular += 1;
                                ManagementMaster.NewPediatricNormal += 0;
                            }
                            else
                            {
                                ManagementMaster.NewPediatricNormal += 1;
                                ManagementMaster.NewPediatricOcular += 0;
                            }
                            ManagementMaster.NewGeneralNormal += 0;
                            ManagementMaster.NewGeneralOcular += 0;
                        }
                        else
                        {
                            if (AddReg.RegistrationExtension.Artificialeye == true)
                            {
                                ManagementMaster.NewGeneralOcular += 1;
                                ManagementMaster.NewGeneralNormal += 0;
                            }
                            else
                            {
                                ManagementMaster.NewGeneralNormal += 1;
                                ManagementMaster.NewGeneralOcular += 0;
                            }
                            ManagementMaster.NewPediatricOcular += 0;
                            ManagementMaster.NewPediatricNormal += 0;
                        }
                        if (AddReg.RegistrationTranMaster.RegistrationFees != null)
                        {
                            ManagementMaster.ConsultationNew += AddReg.RegistrationTranMaster.RegistrationFees;
                        }
                        else
                        {
                            ManagementMaster.ConsultationNew += 0;
                        }

                        ManagementMaster.NewPatients += 1;
                        ManagementMaster.UpdatedBy = userid;
                        WYNKContext.Entry(ManagementMaster).State = EntityState.Modified;
                        ErrorLog oErrorLogstranPF = new ErrorLog();
                        object namestrPF = ManagementMaster;
                        oErrorLogstran.WriteErrorLogArray("PatientFootfall", namestrPF);
                        WYNKContext.SaveChanges();
                    }
                    else
                    {

                        ManagementMaster.NewPatients = +1;
                        if (Convert.ToInt32(PodiatristAge) > Convert.ToInt32(result))
                        {
                            if (AddReg.RegistrationExtension.Artificialeye == true)
                            {
                                ManagementMaster.NewPediatricOcular = +1;
                                ManagementMaster.NewPediatricNormal = +0;
                            }
                            else
                            {
                                ManagementMaster.NewPediatricNormal = +1;
                                ManagementMaster.NewPediatricOcular = +0;
                            }
                            ManagementMaster.NewGeneralNormal = +0;
                            ManagementMaster.NewGeneralOcular = +0;
                        }
                        else
                        {
                            if (AddReg.RegistrationExtension.Artificialeye == true)
                            {
                                ManagementMaster.NewGeneralOcular = +1;
                                ManagementMaster.NewGeneralNormal = +0;
                            }
                            else
                            {
                                ManagementMaster.NewGeneralNormal = +1;
                                ManagementMaster.NewGeneralOcular = +0;
                            }
                            ManagementMaster.NewPediatricNormal = +0;
                            ManagementMaster.NewPediatricOcular = +0;
                        }

                        if (AddReg.RegistrationTranMaster.RegistrationFees != null)


                        {
                            ManagementMaster.ConsultationNew = +AddReg.RegistrationTranMaster.RegistrationFees;
                        }
                        else
                        {
                            ManagementMaster.ConsultationNew = +0;
                        }
                        ManagementMaster.ReviewGeneralNormal = +0;
                        ManagementMaster.ReviewGeneralOcular = +0;
                        ManagementMaster.ReviewPediatricNormal = +0;
                        ManagementMaster.ReviewPediatricOcular = +0;
                        ManagementMaster.ConsultationReview = +0;
                        ManagementMaster.Date = Convert.ToDateTime(datereglocaltime.ToString("yyyy-MM-dd"));
                        ManagementMaster.CreatedBy = userid;
                        ManagementMaster.CmpID = registrationMaster.CMPID;
                        WYNKContext.PatientFootfall.Add(ManagementMaster);
                        ErrorLog oErrorLogstranPF = new ErrorLog();
                        object namestrPF = ManagementMaster;
                        oErrorLogstran.WriteErrorLogArray("PatientFootfall", namestrPF);
                        WYNKContext.SaveChanges();
                    }
                    WYNKContext.SaveChanges();
                    var commonRepos = new CommonRepository(_Wynkcontext, _Cmpscontext);
                    var RunningNumber = commonRepos.GenerateRunningCtrlNoo(TransactionTypeid, AddReg.RegistrationMaster.CMPID, "GetRunningNo");
                    var RunningNumber1 = commonRepos.GenerateRunningCtrlNoo(ContraTransactionid, AddReg.RegistrationMaster.CMPID, "GetRunningNo");

                    if (RunningNumber1 == AddReg.ContraTransactionid)
                    {
                        commonRepos.GenerateRunningCtrlNoo(ContraTransactionid, AddReg.RegistrationMaster.CMPID, "UpdateRunningNo");
                    }
                    else
                    {
                        var GetRunningNumber1 = commonRepos.GenerateRunningCtrlNoo(ContraTransactionid, AddReg.RegistrationMaster.CMPID, "UpdateRunningNo");
                        var Payment1 = new Payment_Master();
                        Payment1 = WYNKContext.PaymentMaster.Where(x => x.ReceiptNumber == AddReg.ContraTransactionid).FirstOrDefault();
                        Payment1.ReceiptNumber = GetRunningNumber1;
                        WYNKContext.Entry(Payment1).State = EntityState.Modified;
                        WYNKContext.SaveChanges();
                    }

                    if (RunningNumber == AddReg.RegistrationMaster.UIN)
                    {
                        commonRepos.GenerateRunningCtrlNoo(TransactionTypeid, AddReg.RegistrationMaster.CMPID, "UpdateRunningNo");
                    }

                    else
                    {
                        var GetRunningNumber = commonRepos.GenerateRunningCtrlNoo(TransactionTypeid, AddReg.RegistrationMaster.CMPID, "UpdateRunningNo");
                        registrationMaster = WYNKContext.Registration.Where(x => x.UIN == AddReg.RegistrationMaster.UIN).FirstOrDefault();
                        registrationMaster.UIN = GetRunningNumber;
                        WYNKContext.Entry(registrationMaster).State = EntityState.Modified;
                        WYNKContext.SaveChanges();
                    }

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
                            uinss = registrationMaster.UIN,
                            namep = registrationMaster.Name + ' ' + registrationMaster.MiddleName + ' ' + registrationMaster.LastName,
                            FName = registrationMaster.FatherHusbandName,
                            age = PasswordEncodeandDecode.ToAgeString(registrationMaster.DateofBirth),
                            gender = registrationMaster.Gender,
                            dateofregistration = registrationMaster.DateofRegistration,
                            phone = registrationMaster.Phone,
                            Address1 = registrationMaster.Address1,
                            email = registrationMaster.EmailID,
                            RegistrationTranID = registrationTranMaster.RegistrationTranID,
                            ConsulationFee = AddReg.RegistrationTranMaster.RegistrationFees,
                            PAddress = CMPSContext.Company.Where(x => x.CmpID == AddReg.RegistrationMaster.CMPID).Select(x => x.Address1).FirstOrDefault(),
                            PAddress2 = CMPSContext.Company.Where(x => x.CmpID == AddReg.RegistrationMaster.CMPID).Select(x => x.Address2).FirstOrDefault(),
                            PAddress3 = CMPSContext.Company.Where(x => x.CmpID == AddReg.RegistrationMaster.CMPID).Select(x => x.Address3).FirstOrDefault(),
                            Pphone = CMPSContext.Company.Where(x => x.CmpID == AddReg.RegistrationMaster.CMPID).Select(x => x.Phone1).FirstOrDefault(),
                            Pweb = CMPSContext.Company.Where(x => x.CmpID == AddReg.RegistrationMaster.CMPID).Select(x => x.Website).FirstOrDefault(),
                            PCompnayname = AddReg.Companyname,
                            RegisteredBy = CMPSContext.Users.Where(x => x.Userid == userid).Select(x => x.Username).FirstOrDefault(),
                            ReceiptNumber = AddReg.ContraTransactionid,
                            ReceiptDate = DateTime.UtcNow,
                            ReferenceTag = CMPSContext.Users.Where(x => x.Userid == userid).Select(x => x.ReferenceTag).FirstOrDefault(),
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
                        Message = msg,
                        grn = AddReg.RegistrationMaster.UIN
                    };
                }
                return new
                {
                    Success = false,

                };
            }
        }
        public RegistrationMasterI getpayment(string UIN, string REPNo, int Cmpid)
        {
            var PaymentMaster = WYNKContext.PaymentMaster.AsNoTracking().ToList();
            var payment = new RegistrationMasterI();
            payment.paymentReMode = (from PTY in PaymentMaster.Where(x => x.UIN == UIN && x.ReceiptNumber == REPNo && x.PaymentType == "O" && x.IsBilled == true && x.CmpID == Cmpid)

                                     select new paymentReMode
                                     {
                                         PaymentMode = PTY.PaymentMode,
                                         InstrumentNumber = PTY.InstrumentNumber,
                                         Instrumentdate = PTY.Instrumentdate,
                                         BankName = PTY.BankName,
                                         BankBranch = PTY.BankBranch,
                                         Expirydate = PTY.Expirydate,
                                         Amount = PTY.Amount,
                                     }
                        ).ToList();

            payment.TOpayMode = payment.paymentReMode.Select(x => x.Amount).Sum();

            return payment;
        }
        public dynamic UpdateregMas(RegistrationMasterI AddReg, string UIN, int userid)
        {
            var registrationMaster = new Registration_Master();
            var registrationTranMaster = new RegistrationTran_Master();
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {

                    registrationMaster = WYNKContext.Registration.Where(x => x.UIN == UIN).FirstOrDefault();
                    registrationMaster.Name = AddReg.RegistrationMaster.Name;
                    registrationMaster.MiddleName = AddReg.RegistrationMaster.MiddleName;
                    registrationMaster.LastName = AddReg.RegistrationMaster.LastName;
                    registrationMaster.PreferredLanguage = AddReg.RegistrationMaster.PreferredLanguage;
                    registrationMaster.MaritalStatus = AddReg.RegistrationMaster.MaritalStatus;
                    registrationMaster.DateofBirth = AddReg.RegistrationMaster.DateofBirth;
                    registrationMaster.Gender = AddReg.RegistrationMaster.Gender;
                    registrationMaster.Address1 = AddReg.RegistrationMaster.Address1;
                    registrationMaster.Address2 = AddReg.RegistrationMaster.Address2;
                    registrationMaster.Address3 = AddReg.RegistrationMaster.Address3;
                    registrationMaster.LocationID = AddReg.RegistrationMaster.LocationID;
                    registrationMaster.FatherHusbandName = AddReg.RegistrationMaster.FatherHusbandName;
                    registrationMaster.EmailID = AddReg.RegistrationMaster.EmailID;
                    registrationMaster.Phone = AddReg.RegistrationMaster.Phone;
                    registrationMaster.AlternateMailID = AddReg.RegistrationMaster.AlternateMailID;
                    registrationMaster.AlternatePhoneNumber = AddReg.RegistrationMaster.AlternatePhoneNumber;
                    registrationMaster.AadharNumber = AddReg.RegistrationMaster.AadharNumber;

                    registrationMaster.PanCardNo = AddReg.RegistrationMaster.PanCardNo;
                    registrationMaster.DrivingLicenseNo = AddReg.RegistrationMaster.DrivingLicenseNo;
                    registrationMaster.PassportNo = AddReg.RegistrationMaster.PassportNo;
                    registrationMaster.IsForeignNational = AddReg.RegistrationMaster.IsForeignNational;

                    registrationMaster.Occupation = AddReg.RegistrationMaster.Occupation;
                    registrationMaster.SourceofReferralID = AddReg.RegistrationMaster.SourceofReferralID;
                    registrationMaster.ReferralName = AddReg.RegistrationMaster.ReferralName;
                    registrationMaster.UpdatedBy = userid;
                    registrationMaster.IsDeleted = false;
                    registrationMaster.UpdatedUTC = DateTime.UtcNow;
                    WYNKContext.Entry(registrationMaster).State = EntityState.Modified;

                    string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == userid).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                    ErrorLog oErrorLogs = new ErrorLog();
                    oErrorLogs.WriteErrorLogTitle(AddReg.Companyname, "Registration", "User name :", username, "User ID :", Convert.ToString(userid), "Mode : Update");


                    object names = registrationMaster;
                    oErrorLogs.WriteErrorLogArray("Registration", names);


                    var RegistrationExtension = new RegistrationExtension();
                    if (AddReg.RegistrationExtension.Artificialeye == true)
                    {
                        RegistrationExtension = WYNKContext.RegistrationExtension.Where(x => x.UIN == registrationMaster.UIN && x.CMPID == AddReg.RegistrationMaster.CMPID).FirstOrDefault();
                        RegistrationExtension.Artificialeye = AddReg.RegistrationExtension.Artificialeye;
                        RegistrationExtension.OD = AddReg.RegistrationExtension.OD;
                        RegistrationExtension.OU = AddReg.RegistrationExtension.OU;
                        RegistrationExtension.OS = AddReg.RegistrationExtension.OS;
                        RegistrationExtension.UpdatedUTC = DateTime.UtcNow;
                        RegistrationExtension.UpdatedBy = AddReg.RegistrationTranMaster.CreatedBy;
                        WYNKContext.Entry(RegistrationExtension).State = EntityState.Modified;

                        ErrorLog oErrorLogstranEX = new ErrorLog();
                        object namestrEX = RegistrationExtension;
                        oErrorLogs.WriteErrorLogArray("RegistrationExtension", namestrEX);

                        WYNKContext.SaveChanges();
                    }


                    var DkinD = WYNKContext.PatientKINDetails.Where(x => x.UIN == UIN && x.CmpID == AddReg.RegistrationMaster.CMPID).ToList();
                    WYNKContext.PatientKINDetails.RemoveRange(DkinD);
                    WYNKContext.SaveChanges();
                    foreach (var item in AddReg.Patientlist.ToList())
                    {
                        var kindetails = new PatientKINDetails();
                        kindetails.UIN = registrationMaster.UIN;
                        kindetails.Relationship = item.Relationship;
                        kindetails.FirstName = item.FirstName;
                        kindetails.MiddleName = item.MiddleName;
                        kindetails.LastName = item.LastName;
                        kindetails.ContactNumber = item.ContactNumber;
                        kindetails.EmailID = item.EmailID;
                        kindetails.PrimaryKinId = item.PrimaryKinId;
                        kindetails.CmpID = AddReg.RegistrationMaster.CMPID;
                        kindetails.CreatedUTC = DateTime.UtcNow;
                        kindetails.CreatedBy = userid;
                        WYNKContext.PatientKINDetails.Add(kindetails);

                        ErrorLog oErrorLogstranKIN = new ErrorLog();
                        object namestrKIN = RegistrationExtension;
                        oErrorLogs.WriteErrorLogArray("PatientKINDetails", namestrKIN);

                        WYNKContext.SaveChanges();
                    }
                    var NewID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "New").Select(x => Convert.ToString(x.OLMID)).FirstOrDefault();
                    registrationTranMaster = WYNKContext.RegistrationTran.Where(x => x.PatientVisitType == NewID && x.UIN == registrationMaster.UIN).FirstOrDefault();
                    registrationTranMaster.KinConsentID = WYNKContext.PatientKINDetails.Where(x => x.UIN == registrationMaster.UIN && x.PrimaryKinId == true).Select(x => x.ID).FirstOrDefault();
                    WYNKContext.Entry(registrationTranMaster).State = EntityState.Modified;
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
        public dynamic InsertReview(RegistrationMasterI AddReg, string UIN, int Cmpid, int ContraTransactionid, int userid, decimal ConsultationFee)

        {

            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {

                    var Duplicatetranid = WYNKContext.RegistrationTran.Where(x => x.UIN == UIN).OrderByDescending(x => x.CreatedUTC).Select(x => x.RegistrationTranID).FirstOrDefault();

                    var Status = WYNKContext.RegistrationTran.Where(x => x.UIN == UIN).Select(x => x.Status).LastOrDefault();
                    var CancelledID = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "RegStatus" && x.ParentDescription == "Cancelled").Select(x => x.OLMID).FirstOrDefault();
                    var ClosedID = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "RegStatus" && x.ParentDescription == "Closed").Select(x => x.OLMID).FirstOrDefault();
                    int billclosed = WYNKContext.PatientAccount.Where(x => x.UIN == UIN && x.CMPID == Cmpid && x.InvoiceNumber == null).Select(x => x.PAID).FirstOrDefault();

                    if (Status != CancelledID && Status != ClosedID)
                    {
                        return new
                        {
                            Success = false,
                            Message = "Patient already on the queue please check the status"
                        };
                    }
                    if (billclosed != 0)
                    {
                        return new
                        {
                            Success = false,
                            Message = "Patient Consolidated Billing Not Paid"
                        };
                    }


                    var billtype = CMPSContext.Setup.Where(x => x.CMPID == Cmpid).Select(x => x.BillingType).FirstOrDefault();
                    if (billtype == "Consolidated Billing" && AddReg.Regcharges != 0)
                    {

                        var respa = (from pa in WYNKContext.PatientAccount.Where(x => x.UIN == UIN && x.InvoiceNumber == null && x.CMPID == Cmpid)
                                     select new
                                     {
                                         ret = pa.UIN.Count(),

                                     }).ToList();

                        var PAID = WYNKContext.PatientAccount.Where(x => x.UIN == UIN && x.InvoiceNumber == null && x.CMPID == Cmpid).Select(x => x.PAID).LastOrDefault();
                        var childid = WYNKContext.ServiceMaster.Where(x => x.Tag == "REG" && x.IsActive == true && x.CMPID == AddReg.RegistrationMaster.CMPID).Select(x => x.ChildDescription).FirstOrDefault();
                        var ServiceTypeID = WYNKContext.Services.Where(x => x.ID == Convert.ToInt32(childid) && x.CMPID == AddReg.RegistrationMaster.CMPID).Select(x => x.ID).FirstOrDefault();
                        var childdesc = WYNKContext.Services.Where(x => x.ID == Convert.ToInt32(childid) && x.CMPID == AddReg.RegistrationMaster.CMPID).Select(x => x.Description).FirstOrDefault();

                        var invespa = new PatientAccount();

                        if (respa.Count == 0)
                        {

                            invespa.CMPID = Cmpid;
                            invespa.UIN = UIN;
                            invespa.TotalProductValue = AddReg.Regcharges;
                            invespa.TotalBillValue = AddReg.Regcharges;
                            invespa.CreatedUTC = DateTime.UtcNow;
                            invespa.CreatedBy = userid;
                            WYNKContext.PatientAccount.AddRange(invespa);
                            WYNKContext.SaveChanges();
                        }

                        else
                        {

                            var masters1 = WYNKContext.PatientAccount.Where(x => x.UIN == UIN).LastOrDefault();
                            masters1.TotalProductValue += AddReg.Regcharges;
                            masters1.TotalBillValue += AddReg.Regcharges;
                            masters1.UpdatedUTC = DateTime.UtcNow;
                            masters1.UpdatedBy = userid;
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
                        var respa1 = WYNKContext.PatientAccountDetail.Where(x => x.PAID == PAID && x.ServiceTypeID == ServiceTypeID).ToList();
                        if (respa1.Count == 0)
                        {
                            var patactdt = new PatientAccountDetail();
                            patactdt.PAID = PAID;
                            patactdt.OLMID = ServiceTypeID;
                            patactdt.ServiceTypeID = ServiceTypeID;
                            patactdt.ServiceDescription = childdesc;
                            patactdt.Description = childdesc;
                            patactdt.TotalProductValue = AddReg.Regcharges;
                            patactdt.TotalBillValue = AddReg.Regcharges;
                            patactdt.CreatedUTC = DateTime.UtcNow;
                            patactdt.CreatedBy = userid;
                            WYNKContext.PatientAccountDetail.AddRange(patactdt);
                            WYNKContext.SaveChanges();
                        }
                        else
                        {
                            var masters = WYNKContext.PatientAccountDetail.Where(x => x.PAID == PAID && x.ServiceTypeID == ServiceTypeID).LastOrDefault();
                            masters.TotalProductValue += AddReg.Regcharges;
                            masters.TotalBillValue += AddReg.Regcharges;
                            masters.UpdatedUTC = DateTime.UtcNow;
                            masters.UpdatedBy = userid;
                            WYNKContext.PatientAccountDetail.UpdateRange(masters);

                        }



                        WYNKContext.SaveChanges();

                    }

                    if (billtype == "Consolidated Billing" && AddReg.M_Consultationcharges != 0)
                    {

                        var respa = (from pa in WYNKContext.PatientAccount.Where(x => x.UIN == UIN && x.InvoiceNumber == null && x.CMPID == Cmpid)
                                     select new
                                     {
                                         ret = pa.UIN.Count(),

                                     }).ToList();

                        var PAID = WYNKContext.PatientAccount.Where(x => x.UIN == UIN && x.InvoiceNumber == null && x.CMPID == Cmpid).Select(x => x.PAID).LastOrDefault();
                        var childid = WYNKContext.ServiceMaster.Where(x => x.Tag == "CON" && x.IsActive == true && x.CMPID == AddReg.RegistrationMaster.CMPID).Select(x => x.ChildDescription).FirstOrDefault();
                        var ServiceTypeID = WYNKContext.Services.Where(x => x.ID == Convert.ToInt32(childid) && x.CMPID == AddReg.RegistrationMaster.CMPID).Select(x => x.ID).FirstOrDefault();
                        var childdesc = WYNKContext.Services.Where(x => x.ID == Convert.ToInt32(childid) && x.CMPID == AddReg.RegistrationMaster.CMPID).Select(x => x.Description).FirstOrDefault();

                        var invespa = new PatientAccount();

                        if (respa.Count == 0)
                        {

                            invespa.CMPID = Cmpid;
                            invespa.UIN = UIN;
                            invespa.TotalProductValue = AddReg.M_Consultationcharges;
                            invespa.TotalBillValue = AddReg.M_Consultationcharges;
                            invespa.CreatedUTC = DateTime.UtcNow;
                            invespa.CreatedBy = userid;
                            WYNKContext.PatientAccount.AddRange(invespa);
                            WYNKContext.SaveChanges();
                        }

                        else
                        {

                            var masters1 = WYNKContext.PatientAccount.Where(x => x.UIN == UIN).LastOrDefault();
                            masters1.TotalProductValue += AddReg.M_Consultationcharges;
                            masters1.TotalBillValue += AddReg.M_Consultationcharges;
                            masters1.UpdatedUTC = DateTime.UtcNow;
                            masters1.UpdatedBy = userid;
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
                        var respa1 = WYNKContext.PatientAccountDetail.Where(x => x.PAID == PAID && x.ServiceTypeID == ServiceTypeID).ToList();
                        if (respa1.Count == 0)
                        {
                            var patactdt = new PatientAccountDetail();
                            patactdt.PAID = PAID;
                            patactdt.OLMID = ServiceTypeID;
                            patactdt.ServiceTypeID = ServiceTypeID;
                            patactdt.ServiceDescription = childdesc;
                            patactdt.Description = childdesc;
                            patactdt.TotalProductValue = AddReg.M_Consultationcharges;
                            patactdt.TotalBillValue = AddReg.M_Consultationcharges;
                            patactdt.CreatedUTC = DateTime.UtcNow;
                            patactdt.CreatedBy = userid;
                            WYNKContext.PatientAccountDetail.AddRange(patactdt);
                            WYNKContext.SaveChanges();
                        }
                        else
                        {
                            var masters = WYNKContext.PatientAccountDetail.Where(x => x.PAID == PAID && x.ServiceTypeID == ServiceTypeID).LastOrDefault();
                            masters.TotalProductValue += AddReg.M_Consultationcharges;
                            masters.TotalBillValue += AddReg.M_Consultationcharges;
                            masters.UpdatedUTC = DateTime.UtcNow;
                            masters.UpdatedBy = userid;
                            WYNKContext.PatientAccountDetail.UpdateRange(masters);

                        }



                        WYNKContext.SaveChanges();

                    }


                    var registrationMaster = new Registration_Master();
                    var registrationTranMaster = new RegistrationTran_Master();
                    var ManagementMaster = new ManagementMaster();
                    var Payment1 = new Payment_Master();
                    var RegistrationExtension = new RegistrationExtension();
                    var ReviewCount = WYNKContext.RegistrationTran.Where(x => x.UIN == UIN).Select(x => x.ReviewCount).LastOrDefault();
                    if (AddReg.RegistrationTranMaster.ConsulationFees != 0)
                    {


                        if (AddReg.RegistrationMaster.RegType == "Appointment")
                        {
                            var AppReg = new Appointment();
                            AppReg = WYNKContext.Appointment.Where(x => x.UIN == AddReg.AppUIN).FirstOrDefault();
                            AppReg.IsVisited = true;
                            AppReg.UpdatedBy = userid;
                            AppReg.UpdatedUTC = DateTime.UtcNow;
                            WYNKContext.Entry(AppReg).State = EntityState.Modified;
                            WYNKContext.SaveChanges();

                        }

                        registrationMaster = WYNKContext.Registration.Where(x => x.UIN == UIN).FirstOrDefault();
                        registrationTranMaster.UIN = registrationMaster.UIN;
                        registrationTranMaster.CmpID = Cmpid;
                        registrationTranMaster.DateofVisit = AddReg.RegistrationTranMaster.DateofVisit;
                        registrationTranMaster.TypeofVisit = AddReg.RegistrationTranMaster.TypeofVisit;
                        registrationTranMaster.Status = AddReg.RegistrationTranMaster.Status;
                        registrationTranMaster.StatusDateTime = AddReg.RegistrationTranMaster.StatusDateTime;
                        registrationTranMaster.Remarks = AddReg.RegistrationTranMaster.Remarks;
                        registrationMaster.IsDeleted = false;
                        registrationTranMaster.CreatedUTC = DateTime.UtcNow;
                        registrationTranMaster.UpdatedUTC = DateTime.UtcNow;
                        registrationTranMaster.CreatedBy = AddReg.RegistrationTranMaster.CreatedBy;
                        registrationTranMaster.UpdatedBy = AddReg.RegistrationTranMaster.CreatedBy;
                        registrationTranMaster.DoctorID = 0;
                        registrationTranMaster.CmpID = Cmpid;
                        registrationTranMaster.ReviewCount = ReviewCount + 1;
                        registrationTranMaster.RegistrationFees = AddReg.RegistrationTranMaster.RegistrationFees;
                        registrationTranMaster.PatientVisitType = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Review").Select(x => Convert.ToString(x.OLMID)).FirstOrDefault();
                        WYNKContext.RegistrationTran.Add(registrationTranMaster);

                        string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == AddReg.RegistrationTranMaster.CreatedBy).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                        string userid1 = Convert.ToString(AddReg.RegistrationTranMaster.CreatedBy);
                        ErrorLog oErrorLogs = new ErrorLog();
                        oErrorLogs.WriteErrorLogTitle(AddReg.Companyname, "Registration", "User name :", username, "User ID :", userid1, "Mode : AddReview");


                        object names = registrationTranMaster;
                        oErrorLogs.WriteErrorLogArray("RegistrationTran", names);
                        foreach (var item in AddReg.PatientAssignStatus.ToList())
                        {
                            var Payment = new Payment_Master();
                            Payment.UIN = registrationMaster.UIN;
                            Payment.PaymentType = "O";
                            Payment.PaymentMode = item.PaymentMode;
                            Payment.InstrumentNumber = item.InstrumentNumber;
                            Payment.Instrumentdate = item.Instrumentdate;
                            Payment.BankName = item.BankName;
                            Payment.BankBranch = item.BankBranch;
                            Payment.Expirydate = item.Expirydate;
                            Payment.Amount = item.Amount;
                            Payment.CmpID = Cmpid;
                            Payment.IsBilled = true;
                            Payment.ReceiptNumber = AddReg.ContraTransactionid;
                            Payment.ReceiptDate = DateTime.UtcNow;
                            Payment.TransactionID = ContraTransactionid;
                            Payment.Fyear = Convert.ToString(WYNKContext.FinancialYear.Where(x => x.ID == WYNKContext.FinancialYear.Where(b => Convert.ToDateTime(b.FYFrom) <= DateTime.Now && Convert.ToDateTime(b.FYTo) >=
                            DateTime.Now && x.CMPID == Cmpid && x.IsActive == true).Select(f => f.ID).FirstOrDefault()).Select(c => c.FYAccYear).FirstOrDefault());
                            Payment.CreatedUTC = DateTime.UtcNow;
                            Payment.CreatedBy = AddReg.RegistrationTranMaster.CreatedBy;
                            WYNKContext.PaymentMaster.Add(Payment);
                            ErrorLog oErrorLogstran = new ErrorLog();
                            object namestr = Payment;
                            oErrorLogstran.WriteErrorLogArray("PaymentMaster", namestr);
                            WYNKContext.SaveChanges();
                        }

                        if (AddReg.RegistrationExtension.Artificialeye == true)
                        {
                            RegistrationExtension = WYNKContext.RegistrationExtension.Where(x => x.UIN == registrationMaster.UIN && x.CMPID == Cmpid).FirstOrDefault();
                            RegistrationExtension.Artificialeye = AddReg.RegistrationExtension.Artificialeye;
                            RegistrationExtension.OD = AddReg.RegistrationExtension.OD;
                            RegistrationExtension.OU = AddReg.RegistrationExtension.OU;
                            RegistrationExtension.OS = AddReg.RegistrationExtension.OS;
                            RegistrationExtension.UpdatedUTC = DateTime.UtcNow;
                            RegistrationExtension.UpdatedBy = AddReg.RegistrationTranMaster.CreatedBy;
                            WYNKContext.Entry(RegistrationExtension).State = EntityState.Modified;
                            ErrorLog oErrorLogstranEX = new ErrorLog();
                            object namestrEX = RegistrationExtension;
                            oErrorLogstranEX.WriteErrorLogArray("RegistrationExtension", namestrEX);
                            WYNKContext.SaveChanges();
                        }
                        var datereglocaltime = DateTime.Now;
                        var Date1 = WYNKContext.PatientFootfall.Where(x => x.Date.Date == datereglocaltime.Date).Select(x => x.Date.Date).FirstOrDefault();
                        var Age = PasswordEncodeandDecode.ToAgeString(AddReg.RegistrationMaster.DateofBirth);
                        var result = Regex.Match(Age, @"\d+").Value;
                        var PodiatristAge = CMPSContext.Setup.Where(x => x.CMPID == Cmpid).Select(x => x.Pediatric).FirstOrDefault();

                        var results = WYNKContext.PatientFootfall.Where(x => x.Date.Date == datereglocaltime.Date).ToList();
                        foreach (var Arrays1 in results)
                        {
                            if (Arrays1.CmpID == Cmpid)
                            {
                                AddReg.Cmpid = Cmpid;
                            }

                        }
                        if (datereglocaltime.Date == Date1.Date && Cmpid == AddReg.Cmpid)
                        {
                            ManagementMaster = WYNKContext.PatientFootfall.Where(x => x.Date.Date == Date1.Date && x.CmpID == Cmpid).FirstOrDefault();
                            ManagementMaster.ReviewPatients += 1;

                            if (Convert.ToInt32(PodiatristAge) > Convert.ToInt32(result))
                            {
                                if (AddReg.RegistrationExtension.Artificialeye == true)
                                {
                                    ManagementMaster.ReviewPediatricOcular += 1;
                                    ManagementMaster.ReviewPediatricNormal += 0;
                                }
                                else
                                {
                                    ManagementMaster.ReviewPediatricNormal += 1;
                                    ManagementMaster.ReviewPediatricOcular += 0;
                                }
                                ManagementMaster.ReviewGeneralNormal += 0;
                                ManagementMaster.ReviewGeneralOcular += 0;
                            }
                            else
                            {
                                if (AddReg.RegistrationExtension.Artificialeye == true)
                                {
                                    ManagementMaster.ReviewGeneralOcular += 1;
                                    ManagementMaster.ReviewGeneralNormal += 0;
                                }
                                else
                                {
                                    ManagementMaster.ReviewGeneralNormal += 1;
                                    ManagementMaster.ReviewGeneralOcular += 0;
                                }
                                ManagementMaster.ReviewPediatricOcular += 0;
                                ManagementMaster.ReviewPediatricNormal += 0;
                            }

                            if (AddReg.RegistrationTranMaster.RegistrationFees != null)
                            {
                                ManagementMaster.ConsultationReview += AddReg.RegistrationTranMaster.RegistrationFees;
                            }
                            else
                            {
                                ManagementMaster.ConsultationReview += 0;
                            }

                            ManagementMaster.UpdatedBy = AddReg.RegistrationTranMaster.CreatedBy;
                            WYNKContext.Entry(ManagementMaster).State = EntityState.Modified;
                            ErrorLog oErrorLogstranPT = new ErrorLog();
                            object namestrPT = ManagementMaster;
                            oErrorLogstranPT.WriteErrorLogArray("PatientFootfall", namestrPT);

                        }
                        else
                        {
                            ManagementMaster.ReviewPatients = +1;

                            if (Convert.ToInt32(PodiatristAge) > Convert.ToInt32(result))
                            {
                                if (AddReg.RegistrationExtension.Artificialeye == true)
                                {
                                    ManagementMaster.ReviewPediatricOcular = +1;
                                    ManagementMaster.ReviewPediatricNormal = +0;
                                }
                                else
                                {
                                    ManagementMaster.ReviewPediatricNormal = +1;
                                    ManagementMaster.ReviewPediatricOcular = +0;
                                }
                            }
                            else
                            {
                                if (AddReg.RegistrationExtension.Artificialeye == true)
                                {
                                    ManagementMaster.ReviewGeneralOcular = +1;
                                    ManagementMaster.ReviewGeneralNormal = +0;
                                }
                                else
                                {
                                    ManagementMaster.ReviewGeneralNormal = +1;
                                    ManagementMaster.ReviewGeneralOcular = +0;
                                }
                            }

                            if (AddReg.RegistrationTranMaster.RegistrationFees != null)
                            {
                                ManagementMaster.ConsultationReview = +AddReg.RegistrationTranMaster.RegistrationFees;
                            }
                            else
                            {
                                ManagementMaster.ConsultationReview = +0;
                            }

                            ManagementMaster.NewGeneralNormal = +0;
                            ManagementMaster.NewGeneralOcular = +0;
                            ManagementMaster.NewPediatricNormal = +0;
                            ManagementMaster.NewPediatricOcular = +0;
                            ManagementMaster.ConsultationNew = +0;
                            ManagementMaster.Date = Convert.ToDateTime(datereglocaltime.ToString("yyyy-MM-dd"));
                            ManagementMaster.CreatedBy = AddReg.RegistrationTranMaster.CreatedBy;
                            ManagementMaster.CmpID = Cmpid;
                            WYNKContext.PatientFootfall.Add(ManagementMaster);
                            ErrorLog oErrorLogstranPT = new ErrorLog();
                            object namestrPT = ManagementMaster;
                            oErrorLogstranPT.WriteErrorLogArray("PatientFootfall", namestrPT);
                        }
                        WYNKContext.SaveChanges();

                        if (ConsultationFee != 0 && ConsultationFee != null)
                        {
                            var commonRepos = new CommonRepository(_Wynkcontext, _Cmpscontext);
                            var RunningNumber = commonRepos.GenerateRunningCtrlNoo(ContraTransactionid, Cmpid, "GetRunningNo");
                            if (RunningNumber == AddReg.ContraTransactionid)
                            {
                                commonRepos.GenerateRunningCtrlNoo(ContraTransactionid, Cmpid, "UpdateRunningNo");
                            }
                            else
                            {
                                var GetRunningNumber1 = commonRepos.GenerateRunningCtrlNoo(ContraTransactionid, Cmpid, "UpdateRunningNo");
                                Payment1 = WYNKContext.PaymentMaster.Where(x => x.ReceiptNumber == AddReg.ContraTransactionid).FirstOrDefault();
                                Payment1.ReceiptNumber = GetRunningNumber1;
                                WYNKContext.Entry(Payment1).State = EntityState.Modified;
                                WYNKContext.SaveChanges();
                            }
                        }

                        var RegTranID = registrationTranMaster.RegistrationTranID;
                        var ccmpid = Cmpid;
                        var OLMID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Assigned").Select(x => x.OLMID).FirstOrDefault();
                        var Referncetag = CMPSContext.Users.Where(x => x.Userid == userid).Select(x => x.ReferenceTag).FirstOrDefault();
                        var utctime = CMPSContext.Setup.Where(x => x.CMPID == ccmpid).Select(x => x.UTCTime).FirstOrDefault();
                        TimeSpan ts = TimeSpan.Parse(utctime);
                        if (Referncetag == "D" || Referncetag == "O" || Referncetag == "V")
                        {
                            var emailid = CMPSContext.Users.Where(x => x.Userid == userid).Select(x => x.Username).FirstOrDefault();
                            var docid = CMPSContext.DoctorMaster.Where(x => x.EmailID == emailid).Select(x => x.DoctorID).FirstOrDefault();

                            var Assigningtable = new Patient_Assign();
                            Assigningtable.RegistrationTranID = RegTranID;
                            Assigningtable.AssignedDate = DateTime.UtcNow + ts;
                            Assigningtable.DoctorID = docid;
                            Assigningtable.IsCancelled = false;
                            Assigningtable.CreatedUTC = DateTime.UtcNow;
                            Assigningtable.CreatedBy = userid;
                            Assigningtable.Patientallocatestatus = null;
                            var idd = WYNKContext.PatientAssign.Select(x => x.ID).LastOrDefault();
                            if (idd < 1)
                            {
                                Assigningtable.ID = 1;
                            }
                            else
                            {
                                Assigningtable.ID = idd + 1;
                            }
                            WYNKContext.PatientAssign.Add(Assigningtable);
                            var Regtrandetails = WYNKContext.RegistrationTran.Where(x => x.RegistrationTranID == RegTranID).FirstOrDefault();
                            Regtrandetails.Status = OLMID;
                            Regtrandetails.DoctorID = docid;
                            Regtrandetails.AssignedDate = DateTime.UtcNow + ts;
                            WYNKContext.RegistrationTran.UpdateRange(Regtrandetails);
                            WYNKContext.SaveChanges();


                            var workflow = new Workflowtran();
                            var CurDate = DateTime.UtcNow;
                            var times1 = CurDate.Hour + ":" + CurDate.Minute + ":" + CurDate.Second;
                            var times2 = ts;
                            var fulltime = TimeSpan.Parse(times1) + times2;
                            DateTime time = DateTime.Today.Add(fulltime);
                            var curfullh = time.Hour;
                            var curfullm = time.Minute;
                            string displayTimes = curfullh + ":" + curfullm;
                            workflow.Checkintime = displayTimes;
                            workflow.Checkouttime = "";
                            workflow.CMPID = Cmpid;
                            workflow.Createdutc = DateTime.UtcNow;

                            if (Referncetag == "O")
                            {
                                workflow.Workflowmasterid = WYNKContext.Workflowmaster.Where(x => x.Description == "Optometrist").Select(x => x.ID).FirstOrDefault();
                                workflow.Optometrist = true;
                                workflow.Doctor = false;
                            }
                            else if (Referncetag == "D")
                            {
                                workflow.Workflowmasterid = WYNKContext.Workflowmaster.Where(x => x.Description == "Doctor").Select(x => x.ID).FirstOrDefault();
                                workflow.Optometrist = false;
                                workflow.Doctor = true;
                            }
                            var fid = WYNKContext.Workflowmaster.Where(x => x.Description == "Findings").Select(x => x.ID).FirstOrDefault();
                            var wtran = WYNKContext.Workflowtran.Where(x => x.Regtranid == Duplicatetranid && x.Workflowmasterid == fid).ToList();
                            wtran.All(x => { x.Checkouttime = displayTimes; x.status = false; return true; });
                            WYNKContext.Workflowtran.UpdateRange(wtran);
                            workflow.status = true;
                            workflow.Reception = false;
                            workflow.Doctorid = docid;
                            workflow.Date = DateTime.UtcNow.Date;
                            workflow.Regtranid = RegTranID;
                            workflow.Createdby = userid;
                            WYNKContext.Workflowtran.Add(workflow);
                            WYNKContext.SaveChanges();




                        }
                        else if (Referncetag == "R" || Referncetag == "A" || Referncetag == "E")
                        {
                            var Assigningtable = new Patient_Assign();
                            Assigningtable.RegistrationTranID = RegTranID;
                            Assigningtable.AssignedDate = DateTime.UtcNow + ts;
                            Assigningtable.DoctorID = 0;
                            Assigningtable.IsCancelled = false;
                            Assigningtable.CreatedUTC = DateTime.UtcNow;
                            Assigningtable.CreatedBy = userid;
                            Assigningtable.Patientallocatestatus = "D";
                            var idd = WYNKContext.PatientAssign.Select(x => x.ID).LastOrDefault();
                            if (idd < 1)
                            {
                                Assigningtable.ID = 1;
                            }
                            else
                            {
                                Assigningtable.ID = idd + 1;
                            }
                            WYNKContext.PatientAssign.Add(Assigningtable);
                            WYNKContext.SaveChanges();

                            var Assigningtables = new Patient_Assign();
                            Assigningtables.RegistrationTranID = RegTranID;
                            Assigningtables.AssignedDate = DateTime.UtcNow + ts;
                            Assigningtables.DoctorID = 0;
                            Assigningtables.IsCancelled = false;
                            Assigningtables.CreatedUTC = DateTime.UtcNow;
                            Assigningtables.CreatedBy = userid;
                            Assigningtables.Patientallocatestatus = "O";
                            var idds = WYNKContext.PatientAssign.Select(x => x.ID).LastOrDefault();
                            if (idd < 1)
                            {
                                Assigningtables.ID = 1;
                            }
                            else
                            {
                                Assigningtables.ID = idds + 1;
                            }
                            WYNKContext.PatientAssign.Add(Assigningtables);
                            WYNKContext.SaveChanges();



                            var workflow = new Workflowtran();
                            var CurDate = DateTime.UtcNow;
                            var times1 = CurDate.Hour + ":" + CurDate.Minute + ":" + CurDate.Second;
                            var times2 = ts;
                            var fulltime = TimeSpan.Parse(times1) + times2;
                            DateTime time = DateTime.Today.Add(fulltime);
                            var curfullh = time.Hour;
                            var curfullm = time.Minute;
                            string displayTimes = curfullh + ":" + curfullm;
                            var fid = WYNKContext.Workflowmaster.Where(x => x.Description == "Findings").Select(x => x.ID).FirstOrDefault();
                            var wtran = WYNKContext.Workflowtran.Where(x => x.Regtranid == Duplicatetranid && x.Workflowmasterid == fid).ToList();
                            wtran.All(x => { x.Checkouttime = displayTimes; x.status = false; return true; });
                            WYNKContext.Workflowtran.UpdateRange(wtran);
                            workflow.Checkintime = displayTimes;
                            workflow.Checkouttime = "";
                            workflow.CMPID = Cmpid;
                            workflow.Createdutc = DateTime.UtcNow;
                            workflow.Workflowmasterid = WYNKContext.Workflowmaster.Where(x => x.Description == "Reception").Select(x => x.ID).FirstOrDefault();
                            workflow.Optometrist = false;
                            workflow.Reception = true;
                            workflow.Doctor = false;
                            workflow.status = true;
                            workflow.Doctorid = 0;
                            workflow.Date = DateTime.UtcNow.Date;
                            workflow.Regtranid = RegTranID;
                            workflow.Createdby = userid;
                            WYNKContext.Workflowtran.Add(workflow);
                            WYNKContext.SaveChanges();


                        }

                    }
                    else
                    {

                        if (AddReg.RegistrationMaster.RegType == "Appointment")
                        {
                            var AppReg = new Appointment();
                            AppReg = WYNKContext.Appointment.Where(x => x.UIN == AddReg.AppUIN).FirstOrDefault();
                            AppReg.IsVisited = true;
                            AppReg.UpdatedBy = userid;
                            AppReg.UpdatedUTC = DateTime.UtcNow;
                            WYNKContext.Entry(AppReg).State = EntityState.Modified;
                            WYNKContext.SaveChanges();

                        }

                        registrationMaster = WYNKContext.Registration.Where(x => x.UIN == UIN).FirstOrDefault();
                        registrationTranMaster.UIN = registrationMaster.UIN;
                        registrationTranMaster.DateofVisit = AddReg.RegistrationTranMaster.DateofVisit;
                        registrationTranMaster.TypeofVisit = AddReg.RegistrationTranMaster.TypeofVisit;
                        registrationTranMaster.Status = AddReg.RegistrationTranMaster.Status;
                        registrationTranMaster.StatusDateTime = AddReg.RegistrationTranMaster.StatusDateTime;
                        registrationTranMaster.Remarks = AddReg.RegistrationTranMaster.Remarks;
                        registrationTranMaster.CmpID = Cmpid;
                        registrationMaster.IsDeleted = false;
                        registrationTranMaster.CreatedUTC = DateTime.UtcNow;
                        registrationTranMaster.UpdatedUTC = DateTime.UtcNow;
                        registrationTranMaster.CreatedBy = AddReg.RegistrationTranMaster.CreatedBy;
                        registrationTranMaster.UpdatedBy = AddReg.RegistrationTranMaster.CreatedBy;
                        registrationTranMaster.DoctorID = 0;
                        registrationTranMaster.ReviewCount = ReviewCount + 1;
                        registrationTranMaster.RegistrationFees = 0;
                        registrationTranMaster.CmpID = Cmpid;
                        registrationTranMaster.PatientVisitType = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Review").Select(x => Convert.ToString(x.OLMID)).FirstOrDefault();
                        WYNKContext.RegistrationTran.Add(registrationTranMaster);
                        WYNKContext.SaveChanges();

                        var RegTranID = registrationTranMaster.RegistrationTranID;
                        var ccmpid = Cmpid;
                        var OLMID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Assigned").Select(x => x.OLMID).FirstOrDefault();
                        var Referncetag = CMPSContext.Users.Where(x => x.Userid == userid).Select(x => x.ReferenceTag).FirstOrDefault();
                        var utctime = CMPSContext.Setup.Where(x => x.CMPID == ccmpid).Select(x => x.UTCTime).FirstOrDefault();
                        TimeSpan ts = TimeSpan.Parse(utctime);
                        if (Referncetag == "D" || Referncetag == "O" || Referncetag == "V")
                        {
                            var emailid = CMPSContext.Users.Where(x => x.Userid == userid).Select(x => x.Username).FirstOrDefault();
                            var docid = CMPSContext.DoctorMaster.Where(x => x.EmailID == emailid).Select(x => x.DoctorID).FirstOrDefault();

                            var Assigningtable = new Patient_Assign();
                            Assigningtable.RegistrationTranID = RegTranID;
                            Assigningtable.AssignedDate = DateTime.UtcNow + ts;
                            Assigningtable.DoctorID = docid;
                            Assigningtable.IsCancelled = false;
                            Assigningtable.CreatedUTC = DateTime.UtcNow;
                            Assigningtable.CreatedBy = userid;
                            Assigningtable.Patientallocatestatus = null;
                            var idd = WYNKContext.PatientAssign.Select(x => x.ID).LastOrDefault();
                            if (idd < 1)
                            {
                                Assigningtable.ID = 1;
                            }
                            else
                            {
                                Assigningtable.ID = idd + 1;
                            }
                            WYNKContext.PatientAssign.Add(Assigningtable);
                            var Regtrandetails = WYNKContext.RegistrationTran.Where(x => x.RegistrationTranID == RegTranID).FirstOrDefault();
                            Regtrandetails.Status = OLMID;
                            Regtrandetails.DoctorID = docid;
                            Regtrandetails.AssignedDate = DateTime.UtcNow + ts;
                            WYNKContext.RegistrationTran.UpdateRange(Regtrandetails);
                            WYNKContext.SaveChanges();

                            var workflow = new Workflowtran();
                            var CurDate = DateTime.UtcNow;
                            var times1 = CurDate.Hour + ":" + CurDate.Minute + ":" + CurDate.Second;
                            var times2 = ts;
                            var fulltime = TimeSpan.Parse(times1) + times2;
                            DateTime time = DateTime.Today.Add(fulltime);
                            var curfullh = time.Hour;
                            var curfullm = time.Minute;
                            string displayTimes = curfullh + ":" + curfullm;
                            workflow.Checkintime = displayTimes;
                            workflow.Checkouttime = "";
                            workflow.CMPID = Cmpid;
                            workflow.Createdutc = DateTime.UtcNow;
                            if (Referncetag == "O")
                            {
                                workflow.Workflowmasterid = WYNKContext.Workflowmaster.Where(x => x.Description == "Optometrist").Select(x => x.ID).FirstOrDefault();
                                workflow.Optometrist = true;
                                workflow.Doctor = false;
                            }
                            else if (Referncetag == "D")
                            {
                                workflow.Workflowmasterid = WYNKContext.Workflowmaster.Where(x => x.Description == "Doctor").Select(x => x.ID).FirstOrDefault();
                                workflow.Optometrist = false;
                                workflow.Doctor = true;
                            }
                            var fid = WYNKContext.Workflowmaster.Where(x => x.Description == "Findings").Select(x => x.ID).FirstOrDefault();
                            var wtran = WYNKContext.Workflowtran.Where(x => x.Regtranid == Duplicatetranid && x.Workflowmasterid == fid).ToList();
                            wtran.All(x => { x.Checkouttime = displayTimes; x.status = false; return true; });
                            WYNKContext.Workflowtran.UpdateRange(wtran);
                            workflow.status = true;
                            workflow.Reception = false;
                            workflow.Doctorid = docid;
                            workflow.Date = DateTime.UtcNow.Date;
                            workflow.Regtranid = RegTranID;
                            workflow.Createdby = userid;
                            WYNKContext.Workflowtran.Add(workflow);
                            WYNKContext.SaveChanges();


                        }
                        else if (Referncetag == "R" || Referncetag == "A" || Referncetag == "E")
                        {
                            var Assigningtable = new Patient_Assign();
                            Assigningtable.RegistrationTranID = RegTranID;
                            Assigningtable.AssignedDate = DateTime.UtcNow;
                            Assigningtable.DoctorID = 0;
                            Assigningtable.IsCancelled = false;
                            Assigningtable.CreatedUTC = DateTime.UtcNow;
                            Assigningtable.CreatedBy = userid;
                            Assigningtable.Patientallocatestatus = "D";
                            var idd = WYNKContext.PatientAssign.Select(x => x.ID).LastOrDefault();
                            if (idd < 1)
                            {
                                Assigningtable.ID = 1;
                            }
                            else
                            {
                                Assigningtable.ID = idd + 1;
                            }
                            WYNKContext.PatientAssign.Add(Assigningtable);
                            WYNKContext.SaveChanges();

                            var Assigningtables = new Patient_Assign();
                            Assigningtables.RegistrationTranID = RegTranID;
                            Assigningtables.AssignedDate = DateTime.UtcNow;
                            Assigningtables.DoctorID = 0;
                            Assigningtables.IsCancelled = false;
                            Assigningtables.CreatedUTC = DateTime.UtcNow;
                            Assigningtables.CreatedBy = userid;
                            Assigningtables.Patientallocatestatus = "O";
                            var idds = WYNKContext.PatientAssign.Select(x => x.ID).LastOrDefault();
                            if (idd < 1)
                            {
                                Assigningtables.ID = 1;
                            }
                            else
                            {
                                Assigningtables.ID = idds + 1;
                            }
                            WYNKContext.PatientAssign.Add(Assigningtables);
                            WYNKContext.SaveChanges();

                            var workflow = new Workflowtran();
                            var CurDate = DateTime.UtcNow;
                            var times1 = CurDate.Hour + ":" + CurDate.Minute + ":" + CurDate.Second;
                            var times2 = ts;
                            var fulltime = TimeSpan.Parse(times1) + times2;
                            DateTime time = DateTime.Today.Add(fulltime);
                            var curfullh = time.Hour;
                            var curfullm = time.Minute;
                            string displayTimes = curfullh + ":" + curfullm;
                            workflow.Checkintime = displayTimes;
                            var fid = WYNKContext.Workflowmaster.Where(x => x.Description == "Findings").Select(x => x.ID).FirstOrDefault();
                            var wtran = WYNKContext.Workflowtran.Where(x => x.Regtranid == Duplicatetranid && x.Workflowmasterid == fid).ToList();
                            wtran.All(x => { x.Checkouttime = displayTimes; x.status = false; return true; });
                            WYNKContext.Workflowtran.UpdateRange(wtran);
                            workflow.Checkouttime = "";
                            workflow.CMPID = Cmpid;
                            workflow.Createdutc = DateTime.UtcNow;
                            workflow.Workflowmasterid = WYNKContext.Workflowmaster.Where(x => x.Description == "Reception").Select(x => x.ID).FirstOrDefault();
                            workflow.Optometrist = false;
                            workflow.Reception = true;
                            workflow.Doctor = false;
                            workflow.status = true;
                            workflow.Doctorid = 0;
                            workflow.Date = DateTime.UtcNow.Date;
                            workflow.Regtranid = RegTranID;
                            workflow.Createdby = userid;
                            WYNKContext.Workflowtran.Add(workflow);
                            WYNKContext.SaveChanges();
                        }

                        string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == AddReg.RegistrationTranMaster.CreatedBy).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                        string userid1 = Convert.ToString(AddReg.RegistrationTranMaster.CreatedBy);
                        ErrorLog oErrorLogs = new ErrorLog();
                        oErrorLogs.WriteErrorLogTitle(AddReg.Companyname, "Registration", "User name :", username, "User ID :", userid1, "Mode : AddReview");


                        object names = registrationTranMaster;
                        oErrorLogs.WriteErrorLogArray("RegistrationTran", names);


                        if (AddReg.RegistrationExtension.Artificialeye == true)
                        {
                            RegistrationExtension = WYNKContext.RegistrationExtension.Where(x => x.UIN == registrationMaster.UIN && x.CMPID == Cmpid).FirstOrDefault();
                            RegistrationExtension.Artificialeye = AddReg.RegistrationExtension.Artificialeye;
                            RegistrationExtension.OD = AddReg.RegistrationExtension.OD;
                            RegistrationExtension.OU = AddReg.RegistrationExtension.OU;
                            RegistrationExtension.OS = AddReg.RegistrationExtension.OS;
                            RegistrationExtension.UpdatedUTC = DateTime.UtcNow;
                            RegistrationExtension.UpdatedBy = AddReg.RegistrationTranMaster.CreatedBy;
                            WYNKContext.Entry(RegistrationExtension).State = EntityState.Modified;
                            ErrorLog oErrorLogstranPT = new ErrorLog();
                            object namestrPT = RegistrationExtension;
                            oErrorLogstranPT.WriteErrorLogArray("RegistrationExtension", namestrPT);
                            WYNKContext.SaveChanges();
                        }
                        var datereglocaltime = DateTime.Now;
                        var Date1 = WYNKContext.PatientFootfall.Where(x => x.Date.Date == datereglocaltime.Date).Select(x => x.Date.Date).FirstOrDefault();
                        var Age = PasswordEncodeandDecode.ToAgeString(AddReg.RegistrationMaster.DateofBirth);
                        var result = Regex.Match(Age, @"\d+").Value;
                        var PodiatristAge = CMPSContext.Setup.Where(x => x.CMPID == Cmpid).Select(x => x.Pediatric).FirstOrDefault();

                        var results = WYNKContext.PatientFootfall.Where(x => x.Date.Date == datereglocaltime.Date).ToList();
                        foreach (var Arrays1 in results)
                        {
                            if (Arrays1.CmpID == Cmpid)
                            {
                                AddReg.Cmpid = Cmpid;
                            }

                        }
                        if (datereglocaltime.Date == Date1.Date && Cmpid == AddReg.Cmpid)
                        {
                            ManagementMaster = WYNKContext.PatientFootfall.Where(x => x.Date.Date == Date1.Date && x.CmpID == Cmpid).FirstOrDefault();
                            ManagementMaster.ReviewPatients += 1;

                            if (Convert.ToInt32(PodiatristAge) > Convert.ToInt32(result))
                            {
                                if (AddReg.RegistrationExtension.Artificialeye == true)
                                {
                                    ManagementMaster.ReviewPediatricOcular += 1;
                                    ManagementMaster.ReviewPediatricNormal += 0;
                                }
                                else
                                {
                                    ManagementMaster.ReviewPediatricNormal += 1;
                                    ManagementMaster.ReviewPediatricOcular += 0;
                                }
                                ManagementMaster.ReviewGeneralNormal += 0;
                                ManagementMaster.ReviewGeneralOcular += 0;
                            }
                            else
                            {
                                if (AddReg.RegistrationExtension.Artificialeye == true)
                                {
                                    ManagementMaster.ReviewGeneralOcular += 1;
                                    ManagementMaster.ReviewGeneralNormal += 0;
                                }
                                else
                                {
                                    ManagementMaster.ReviewGeneralNormal += 1;
                                    ManagementMaster.ReviewGeneralOcular += 0;
                                }
                                ManagementMaster.ReviewPediatricNormal += 0;
                                ManagementMaster.ReviewPediatricOcular += 0;
                            }

                            if (AddReg.RegistrationTranMaster.RegistrationFees != null)
                            {
                                ManagementMaster.ConsultationReview += AddReg.RegistrationTranMaster.RegistrationFees;
                            }
                            else
                            {
                                ManagementMaster.ConsultationReview += 0;
                            }

                            ManagementMaster.UpdatedBy = AddReg.RegistrationTranMaster.CreatedBy;
                            WYNKContext.Entry(ManagementMaster).State = EntityState.Modified;
                            ErrorLog oErrorLogstranPT = new ErrorLog();
                            object namestrPT = ManagementMaster;
                            oErrorLogstranPT.WriteErrorLogArray("PatientFootfall", namestrPT);
                        }
                        else
                        {
                            ManagementMaster.ReviewPatients = +1;

                            if (Convert.ToInt32(PodiatristAge) > Convert.ToInt32(result))
                            {
                                if (AddReg.RegistrationExtension.Artificialeye == true)
                                {
                                    ManagementMaster.ReviewPediatricOcular = +1;
                                    ManagementMaster.ReviewPediatricNormal = +0;
                                }
                                else
                                {
                                    ManagementMaster.ReviewPediatricNormal = +1;
                                    ManagementMaster.ReviewPediatricOcular = +0;
                                }
                            }
                            else
                            {
                                if (AddReg.RegistrationExtension.Artificialeye == true)
                                {
                                    ManagementMaster.ReviewGeneralOcular = +1;
                                    ManagementMaster.ReviewGeneralNormal = +0;
                                }
                                else
                                {
                                    ManagementMaster.ReviewGeneralNormal = +1;
                                    ManagementMaster.ReviewGeneralOcular = +0;
                                }
                            }

                            if (AddReg.RegistrationTranMaster.RegistrationFees != null)
                            {
                                ManagementMaster.ConsultationReview = +AddReg.RegistrationTranMaster.RegistrationFees;
                            }
                            else
                            {
                                ManagementMaster.ConsultationReview = +0;
                            }

                            ManagementMaster.NewGeneralNormal = +0;
                            ManagementMaster.NewGeneralOcular = +0;
                            ManagementMaster.NewPediatricNormal = +0;
                            ManagementMaster.NewPediatricOcular = +0;
                            ManagementMaster.ConsultationNew = +0;
                            ManagementMaster.Date = Convert.ToDateTime(datereglocaltime.ToString("yyyy-MM-dd"));
                            ManagementMaster.CreatedBy = AddReg.RegistrationTranMaster.CreatedBy;
                            ManagementMaster.CmpID = Cmpid;
                            WYNKContext.PatientFootfall.Add(ManagementMaster);
                            ErrorLog oErrorLogstranPT = new ErrorLog();
                            object namestrPT = ManagementMaster;
                            oErrorLogstranPT.WriteErrorLogArray("PatientFootfall", namestrPT);
                        }

                    }
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
                            uinss = registrationMaster.UIN,
                            namep = registrationMaster.Name + ' ' + registrationMaster.MiddleName + ' ' + registrationMaster.LastName,
                            FName = registrationMaster.FatherHusbandName,
                            age = PasswordEncodeandDecode.ToAgeString(registrationMaster.DateofBirth),
                            gender = registrationMaster.Gender,
                            dateofregistration = registrationMaster.DateofRegistration,
                            RegistrationTranID = registrationTranMaster.RegistrationTranID,
                            phone = registrationMaster.Phone,
                            Address1 = registrationMaster.Address1,
                            email = registrationMaster.EmailID,
                            ConsulationFee = AddReg.RegistrationTranMaster.RegistrationFees,
                            PAddress = CMPSContext.Company.Where(x => x.CmpID == Cmpid).Select(x => x.Address1).FirstOrDefault(),
                            PAddress2 = CMPSContext.Company.Where(x => x.CmpID == Cmpid).Select(x => x.Address2).FirstOrDefault(),
                            PAddress3 = CMPSContext.Company.Where(x => x.CmpID == Cmpid).Select(x => x.Address3).FirstOrDefault(),
                            Pphone = CMPSContext.Company.Where(x => x.CmpID == Cmpid).Select(x => x.Phone1).FirstOrDefault(),
                            Pweb = CMPSContext.Company.Where(x => x.CmpID == Cmpid).Select(x => x.Website).FirstOrDefault(),
                            PCompnayname = CMPSContext.Company.Where(x => x.CmpID == Cmpid).Select(x => x.CompanyName).FirstOrDefault(),
                            RegisteredBy = CMPSContext.Users.Where(x => x.Userid == userid).Select(x => x.Username).FirstOrDefault(),
                            ReceiptNumber = AddReg.ContraTransactionid,
                            ReceiptDate = DateTime.UtcNow,
                            ReferenceTag = CMPSContext.Users.Where(x => x.Userid == userid).Select(x => x.ReferenceTag).FirstOrDefault(),
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
        public dynamic DeleteRegMas(string UIN)
        {

            var Reg = new Registration_Master();
            var RegT = new RegistrationTran_Master();
            var ManagementMaster = new ManagementMaster();



            Reg = WYNKContext.Registration.Where(x => x.UIN == UIN).FirstOrDefault();
            Reg.IsDeleted = true;

            ErrorLog oErrorLogs = new ErrorLog();
            oErrorLogs.WriteErrorLogTitle("", "Registration", "User name :", "", "User ID :", "", "Mode : Delete");
            object names = Reg;
            oErrorLogs.WriteErrorLogArray("Registration", names);


            var Date1 = WYNKContext.PatientFootfall.Where(x => x.Date.Date == Reg.DateofRegistration.Date).Select(x => x.Date.Date).FirstOrDefault();
            var CmpID = WYNKContext.PatientFootfall.Where(x => x.CmpID == Reg.CMPID).Select(x => x.CmpID).FirstOrDefault();
            if (Reg.DateofRegistration.Date == Date1.Date && Reg.CMPID == CmpID)
            {
                ManagementMaster = WYNKContext.PatientFootfall.Where(x => x.Date.Date == Date1.Date && x.CmpID == CmpID).FirstOrDefault();
                ManagementMaster.NewPatients -= 1;
                WYNKContext.Entry(ManagementMaster).State = EntityState.Modified;
                ErrorLog oErrorLogstranPT1 = new ErrorLog();
                object namestrPT1 = ManagementMaster;
                oErrorLogstranPT1.WriteErrorLogArray("PatientFootfall", namestrPT1);
            }

            RegT = WYNKContext.RegistrationTran.Where(x => x.UIN == UIN).FirstOrDefault();
            RegT.IsDeleted = true;
            ErrorLog oErrorLogstranRegT = new ErrorLog();
            object namestrRegT = RegT;
            oErrorLogstranRegT.WriteErrorLogArray("RegistrationTran", namestrRegT);

            ErrorLog oErrorLogstranPT = new ErrorLog();
            object namestrPT = ManagementMaster;
            oErrorLogstranPT.WriteErrorLogArray("PatientFootfall", namestrPT);
            var Date2 = WYNKContext.PatientFootfall.Where(x => x.Date.Date == RegT.DateofVisit.Date).Select(x => x.Date.Date).FirstOrDefault();
            int CmpID1 = WYNKContext.Registration.Where(x => x.UIN == UIN).Select(x => x.CMPID).FirstOrDefault();
            int MCmpID = WYNKContext.PatientFootfall.Where(x => x.CmpID == CmpID1).Select(x => x.CmpID).FirstOrDefault();

            if (RegT.DateofVisit.Date == Date2.Date && MCmpID == CmpID1)
            {
                ManagementMaster = WYNKContext.PatientFootfall.Where(x => x.Date.Date == Date1.Date && x.CmpID == MCmpID).FirstOrDefault();
                ManagementMaster.ReviewPatients -= 1;
                WYNKContext.Entry(ManagementMaster).State = EntityState.Modified;
                ErrorLog oErrorLogstranPT2 = new ErrorLog();
                object namestrPT2 = ManagementMaster;
                oErrorLogstranPT2.WriteErrorLogArray("PatientFootfall", namestrPT2);
            }

            try
            {
                if (WYNKContext.SaveChanges() >= 0)
                {
                    ErrorLog oErrorLog = new ErrorLog();
                    oErrorLog.WriteErrorLog("Information :", "Saved Successfully");
                }
                if (WYNKContext.SaveChanges() > 0)
                    return new
                    {
                        Success = true,

                    };
            }
            catch (Exception ex)
            {
                ErrorLog oErrorLog = new ErrorLog();
                oErrorLog.WriteErrorLog("Error :", ex.InnerException.Message.ToString());
                Console.Write(ex);
            }
            return new
            {
                Success = false,

            };
        }
        public dynamic AgeOrDboChange(string type, string value, string M_MMDD, int AgeN)
        {
            var age = 0;
            var result = Regex.Match(value, @"\d+").Value;
            dynamic calculatedValue = null;
            if (type == "age")
            {


                age = Convert.ToInt16(result);


                calculatedValue = DateTime.Now.AddYears(-age).ToString("yyyy-MM-dd");

                if (M_MMDD == "Months")
                {
                    calculatedValue = DateTime.Now.AddMonths(-age).ToString("yyyy-MM-dd");
                }
                else if (M_MMDD == "Days")
                {
                    calculatedValue = DateTime.Now.AddDays(-age).ToString("yyyy-MM-dd");
                }
            }

            if (M_MMDD == "Months")
            {
                calculatedValue = DateTime.Now.AddMonths(-AgeN).ToString("yyyy-MM-dd");
            }
            else if (M_MMDD == "Days")
            {
                calculatedValue = DateTime.Now.AddDays(-AgeN).ToString("yyyy-MM-dd");
            }

            else if (type == "dbo")
            {
                var now = DateTime.Now;
                var dbo = DateTime.Parse(value);
                age = now.Year - dbo.Year;
                if (dbo > now.AddYears(-age))
                    age--;
                calculatedValue = age;
            }

            return new
            {
                calculatedValue
            };
        }
        public RegistrationMasterViewModel Getusersaccess(int userid, string userroleid, int CID)
        {
            var Getusersaccess = new RegistrationMasterViewModel();

            var Cmparray = new List<Companyarray>();
            Getusersaccess.PaymentMaster = new List<Payment_Master>();

            var roleid = CMPSContext.Users.Where(x => x.ReferenceTag == userroleid && x.CMPID == CID).FirstOrDefault();
            Getusersaccess.RoleidValue = CMPSContext.Role.Where(x => x.RoleDescription == userroleid).Select(x => x.RoleID).FirstOrDefault();

            var cmp = CMPSContext.Company.Where(x => x.IsDeleted == false).ToList();
            var car = cmp.Where(x => x.CmpID == CID).ToList();
            var parcar = cmp.Where(x => x.CmpID == CID).Select(x => x.ParentID).FirstOrDefault();
            if (parcar == 0)
            {
                var ca = cmp.Where(x => x.ParentID == CID && x.CmpID != CID).ToList();
                var mergeddata = car.Concat(ca);
                Getusersaccess.Companyarray = (from c in mergeddata
                                               select new Companyarray
                                               {
                                                   CompanyIDS = c.CmpID,
                                                   ParentID = c.ParentID,
                                               }).ToList();
            }
            else
            {
                var ca = cmp.Where(x => x.ParentID == parcar || x.CmpID == CID).ToList();
                var pca = cmp.Where(x => x.CmpID == parcar).ToList();
                var mergeddata = car.Concat(ca).Concat(pca);
                Getusersaccess.Companyarray = (from c in mergeddata
                                               group c by new { c.CmpID } into gg
                                               select new Companyarray
                                               {
                                                   CompanyIDS = gg.FirstOrDefault().CmpID,
                                                   ParentID = gg.FirstOrDefault().ParentID,
                                               }).ToList();
            }


            var parentmoduleid = CMPSContext.ModuleMaster.Where(x => x.ModuleType == "Main-Module" && x.ModuleDescription == "Menu-Workflow").Select(x => x.ModuleID).FirstOrDefault();


            var modules = (from Rv in CMPSContext.RoleVsModule
                           join mod in CMPSContext.ModuleMaster on Rv.ModuleID equals mod.ModuleID
                           where mod.ModuleType == "Form" && mod.ParentModuleid == parentmoduleid
                            && mod.IsActive == true && Rv.RoleID == Getusersaccess.RoleidValue && Rv.CmpID == CID
                            && (Rv.Add != false || Rv.Edit != false || Rv.Query != false || Rv.Print != false || Rv.Delete != false)
                           group new { mod, Rv } by new { mod.ModuleDescription, mod.Parentmoduledescription, Rv.RoleID, Rv.ModuleID } into g
                           select new
                           {
                               moduleid = g.FirstOrDefault().mod.ModuleID,
                               Parentmoduledescription = g.FirstOrDefault().mod.Parentmoduledescription,
                               moduledescription = g.FirstOrDefault().mod.ModuleDescription,
                               Add = g.FirstOrDefault().Rv.Add,
                               Update = g.FirstOrDefault().Rv.Query,
                               Edit = g.FirstOrDefault().Rv.Edit,
                               Delete = g.FirstOrDefault().Rv.Delete,
                               Tid = g.FirstOrDefault().mod.TransactionTypeID
                           }).ToList();


            Getusersaccess.Workflowaccerss = (from cp in modules
                                              orderby cp.moduleid
                                              select new Workflowaccerss
                                              {
                                                  Parentmoduledescription = cp.Parentmoduledescription,
                                                  formDescription = cp.moduledescription,
                                                  Add = cp.Add,
                                                  Update = cp.Update,
                                                  Edit = cp.Edit,
                                                  Delete = cp.Delete,
                                                  TransactionID = cp.Tid
                                              }).ToList();


            Getusersaccess.OriginalModule = new List<OriginalModule>();
            var ModuleMaster = CMPSContext.ModuleMaster.OrderBy(x => x.Menuorder).Where(y => y.IsActive == true).ToList();


            var rolemoduedata = (from rm in CMPSContext.RoleVsModule.Where(y => y.RoleID == Getusersaccess.RoleidValue && y.CmpID == CID && (y.Add == true || y.Edit == true || y.Print == true || y.Query == true || y.All == true))
                                 group new { rm } by new { rm.ModuleID } into g
                                 select new
                                 {
                                     Modid = g.FirstOrDefault().rm.ModuleID,
                                     parentid = g.FirstOrDefault().rm.ParentModuleID,
                                 }).ToList();

            var formresdata = (from v in ModuleMaster
                               join b in rolemoduedata on v.ModuleID equals b.Modid

                               select new
                               {
                                   formdesc = v.ModuleDescription,
                                   Modid = v.ModuleID,
                                   parentid = v.ParentModuleid,
                                   parentsubid = b.parentid,
                               }).ToList();

            var subresdata = (from g in ModuleMaster
                              join l in formresdata on g.ModuleID equals l.parentid
                              group new { g, l } by new { g.ModuleDescription } into p
                              select new
                              {
                                  subdesc = p.FirstOrDefault().g.ModuleDescription,
                                  modid = p.FirstOrDefault().g.ModuleID,
                                  parentmoid = p.FirstOrDefault().l.parentsubid,
                              }).ToList();

            var mainres = (from g in ModuleMaster.Where(x => x.ModuleType == "Main-Module" && x.ParentModuleid == 0)
                           join i in subresdata on g.ModuleID equals i.parentmoid
                           group new { g } by new { g.ModuleDescription } into r
                           select new
                           {
                               Maindec = r.FirstOrDefault().g.ModuleDescription,
                               modid = r.FirstOrDefault().g.ModuleID,
                           }).ToList();

            Getusersaccess.DataOriginalModule = new List<DataOriginalModule>();
            var fullacess = (from gf in mainres.Where(x => x.Maindec != "Menu-Workflow")
                             select new DataOriginalModule()
                             {
                                 MainDescription = gf.Maindec,
                                 Formvisible = ModuleMaster.Where(x => x.ModuleID == gf.modid).Select(x => x.Visible).FirstOrDefault(),
                                 DataOriginalsubModule = (from s in subresdata.Where(x => x.parentmoid == gf.modid)
                                                          select new DataOriginalsubModule()
                                                          {
                                                              Formvisible = ModuleMaster.Where(x => x.ModuleID == s.modid).Select(x => x.Visible).FirstOrDefault(),
                                                              subDescription = s.subdesc,
                                                              DataOriginalformsModule = (from a in formresdata.Where(x => x.parentid == s.modid)
                                                                                         select new DataOriginalformsModule()
                                                                                         {
                                                                                             Formvisible = ModuleMaster.Where(x => x.ModuleID == a.Modid).Select(x => x.Visible).FirstOrDefault(),
                                                                                             formDescription = a.formdesc,
                                                                                             Parentmoduledescription = ModuleMaster.Where(x => x.ModuleID == a.Modid).Select(x => x.Parentmoduledescription).FirstOrDefault(),
                                                                                             Moduletypes = ModuleMaster.Where(x => x.ModuleID == a.Modid).Select(x => x.ModuleType).FirstOrDefault(),
                                                                                             Add = CMPSContext.RoleVsModule.Where(x => x.ModuleID == a.Modid && x.CmpID == CID).Select(x => x.Add).FirstOrDefault(),
                                                                                             Edit = CMPSContext.RoleVsModule.Where(x => x.ModuleID == a.Modid && x.CmpID == CID).Select(x => x.Edit).FirstOrDefault(),
                                                                                             Update = CMPSContext.RoleVsModule.Where(x => x.ModuleID == a.Modid && x.CmpID == CID).Select(x => x.Query).FirstOrDefault(),
                                                                                             Export = CMPSContext.RoleVsModule.Where(x => x.ModuleID == a.Modid && x.CmpID == CID).Select(x => x.Print).FirstOrDefault(),
                                                                                             Delete = CMPSContext.RoleVsModule.Where(x => x.ModuleID == a.Modid && x.CmpID == CID).Select(x => x.Delete).FirstOrDefault(),
                                                                                             TransactionID = ModuleMaster.Where(x => x.ModuleID == a.Modid).Select(x => x.TransactionTypeID).FirstOrDefault(),
                                                                                         }).ToList(),
                                                          }).ToList(),
                             }).ToList();

            Getusersaccess.DataOriginalModule.AddRange(fullacess);
            var Formaccessdata = fullacess.Select(x => x.DataOriginalsubModule).ToList();
            Getusersaccess.FgformsModule = new List<FgformsModule>();
            foreach (var item in Formaccessdata)
            {
                var list = item.Select(x => x.DataOriginalformsModule).ToList();

                foreach (var thirditem in list)
                {
                    foreach (var secsitem in thirditem)
                    {
                        var forms = new FgformsModule();
                        forms.Formvisible = secsitem.Formvisible;
                        forms.formDescription = secsitem.formDescription;
                        forms.Parentmoduledescription = secsitem.Parentmoduledescription;
                        forms.Add = secsitem.Add;
                        forms.Edit = secsitem.Edit;
                        forms.Update = secsitem.Update;
                        forms.Delete = secsitem.Delete;
                        forms.Export = secsitem.Export;
                        forms.TransactionID = secsitem.TransactionID;
                        Getusersaccess.FgformsModule.Add(forms);
                    }
                }

            }

            var DashboardFormaccessdata = Getusersaccess.DataOriginalModule.Where(x => x.MainDescription == "Main-Dashboard").ToList();
            Getusersaccess.DashboardsectionModule = new List<DashboardsectionModule>();
            foreach (var item in DashboardFormaccessdata)
            {
                var list = item.DataOriginalsubModule.ToList();
                foreach (var thirditem in list)
                {
                    foreach (var secsitem in thirditem.DataOriginalformsModule)
                    {
                        var forms = new DashboardsectionModule();
                        if (secsitem.formDescription != "Management Dashboard")
                        {
                            forms.MainDescription = secsitem.formDescription;
                            forms.DescriptionPath = secsitem.Parentmoduledescription;
                            Getusersaccess.DashboardsectionModule.Add(forms);
                        }
                    }
                }
            }
            var modid = CMPSContext.Role.Where(x => x.RoleID == Getusersaccess.RoleidValue).Select(x => x.Formpath).FirstOrDefault();
            Getusersaccess.CompanyImnage = CMPSContext.ModuleMaster.Where(x => x.ModuleID == modid).Select(x => x.Parentmoduledescription).FirstOrDefault();

            return Getusersaccess;

        }



        public dynamic Getcompanylogo(int CID)
        {
            var reg = new RegistrationMasterViewModel();
            reg.PatientAssignStatus = new List<PatientAssignStatus>();
            var regs = CMPSContext.Setup.Where(x => x.CMPID == CID).Select(x => x.LogoPath).FirstOrDefault();
            if (regs != null)
            {
                var osfn = CID + ".png";
                var osfi = "/CompanyLogo/";
                var currentDir = Directory.GetCurrentDirectory();
                string path = currentDir + osfi + osfn;
                if ((File.Exists(path)))
                {
                    string imageData = Convert.ToBase64String(File.ReadAllBytes(path));
                    string source = imageData;
                    string base64 = source.Substring(source.IndexOf(',') + 1);
                    byte[] data = Convert.FromBase64String(base64);
                    reg.ProductImage = imageData;
                }
                else
                {

                }
            }
            else
            {
            }
            return reg.ProductImage;
        }
        public dynamic getsubmodules(int Modid)
        {
            var list = new RegistrationDataViewModel();
            var submodule = CMPSContext.ModuleMaster.Where(x => x.ParentModuleid == Modid).ToList();
            foreach (var secitem in submodule)
            {
                var subid = secitem.ModuleID;

            }


            return list;
        }

        public dynamic DeleteToken(string username)
        {


            string ipAddress = "";
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            ipAddress = Convert.ToString(ipHostInfo.AddressList.FirstOrDefault(address => address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork));
            var loginstatus = new RegistrationMasterViewModel();
            var userTokenid = CMPSContext.LogUser.OrderByDescending(x => x.LoguserID).Where(x => x.UserID == Convert.ToInt32(username)).Select(x => x.LoguserID).FirstOrDefault();
            var uservsroles = new LogUser();
            uservsroles = CMPSContext.LogUser.Where(x => x.LoguserID == userTokenid).FirstOrDefault();
            if (uservsroles != null)
            {
                uservsroles.LogoutDatetime = DateTime.UtcNow;
                uservsroles.IsLogged = false;
                CMPSContext.Entry(uservsroles).State = EntityState.Modified;
                CMPSContext.SaveChanges();
            }

            try
            {
                if (CMPSContext.SaveChanges() >= 0)
                {
                    ErrorLog oErrorLog = new ErrorLog();
                    oErrorLog.WriteErrorLog("Information :", "Successfully Logged out. USER ID :" + username + "  IP Address : " + ipAddress + "Date & Time : " + DateTime.UtcNow);
                    return new
                    {
                        Success = true,
                    };
                }

            }
            catch (Exception ex)
            {
                ErrorLog oErrorLog = new ErrorLog();
                oErrorLog.WriteErrorLog("Error :", ex.InnerException.Message.ToString());
                Console.Write(ex);
            }
            return new
            {
                Success = false,
                Message = "Some data are Missing"
            };
        }


        public dynamic GetUsernamepassword(string username, string password)
        {
            var loginstatus = new LoginMasterViewModel();
            var passwordss = PasswordEncodeandDecode.EncodePasswordToBase64(password);
            loginstatus.Users = CMPSContext.Users.Where(x => x.Username == username && x.Password == passwordss && x.Isactive == true).FirstOrDefault();
            if (loginstatus.Users != null)
            {
                var userTokenid = loginstatus.Users.Userid;
                var userCMPid = loginstatus.Users.CMPID;
                loginstatus.USERSCompanyID = userCMPid;
                loginstatus.Userid = loginstatus.Users.Userid;
                var companydetails = CMPSContext.Company.Where(x => x.CmpID == loginstatus.USERSCompanyID && x.IsDeleted == false).FirstOrDefault();
                loginstatus.CompanyName = companydetails.CompanyName;
                try
                {
                    if (loginstatus.Users != null)
                    {
                        if (loginstatus.Users.ReferenceTag == "A")
                        {
                            loginstatus.UserCompany = (from cm in CMPSContext.Company.Where(x => x.CmpID == loginstatus.USERSCompanyID || x.ParentID == loginstatus.USERSCompanyID)
                                                       select new UserCompany
                                                       {
                                                           CompanyNames = cm.CompanyName + "-" + cm.Address1,
                                                           CompanyIDS = cm.CmpID,
                                                       }).ToList();
                        }
                        else
                        {
                            var userdet = CMPSContext.Users.Where(x => x.Username == username && x.Password == passwordss && x.Isactive == true).Select(x => x.CMPID).ToList();
                            loginstatus.UserCompany = new List<UserCompany>();
                            foreach (var item in userdet)
                            {
                                var cmp = new UserCompany();
                                cmp.CompanyIDS = item;
                                cmp.CompanyNames = CMPSContext.Company.Where(x => x.CmpID == item).Select(x => x.CompanyName).FirstOrDefault() + "-" + CMPSContext.Company.Where(x => x.CmpID == item).Select(x => x.Address1).FirstOrDefault();
                                loginstatus.UserCompany.Add(cmp);
                            }
                        }
                    }
                    else
                    {
                        loginstatus.doctorname = "No Users are found";
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                }
            }
            else
            {
                loginstatus.doctorname = "No Users are found";
            }

            return loginstatus;
        }


        public static void Log(string logInfo)
        {
            File.AppendAllText("TestLogger.txt", logInfo);

        }
        public RegistrationMasterViewModel GetselectedPatientDetails(string ViewsStatusid, string RoleDescription, int userDoctorIDs, DateTime todaydates, int CompanyID)
        {

            var utctimes = CMPSContext.Setup.Where(x => x.CMPID == CompanyID).Select(x => x.UTCTime).FirstOrDefault();
            TimeSpan utctime = TimeSpan.Parse(utctimes);
            var reg = new RegistrationMasterViewModel();
            reg.PatientAssignStatus = new List<PatientAssignStatus>();

            var selectedtodaydate = todaydates.AddDays(1);
            if (userDoctorIDs != null)
            {
                var Getonelinemaster = CMPSContext.OneLineMaster.ToList();
                var Getdoctormaster = CMPSContext.DoctorMaster.ToList();
                var sViewsStatusid = Getonelinemaster.Where(x => x.ParentDescription == ViewsStatusid && x.ParentTag == "RegStatus").Select(x => x.OLMID).FirstOrDefault();
                var Getregistration = WYNKContext.Registration.Where(x => x.CMPID == CompanyID && x.IsDeleted == false).ToList();
                var Getregistrationtran = WYNKContext.RegistrationTran.Where(x => x.Status == sViewsStatusid && x.DateofVisit.Date >= todaydates.Date && x.DateofVisit.Date <= selectedtodaydate.Date && x.CmpID == CompanyID).ToList();
                var GetPatientassion = WYNKContext.PatientAssign.AsNoTracking().ToList();


                if (RoleDescription == "Admin" || RoleDescription == "Reception")
                {
                    reg.Patientlist = (from RM in Getregistration
                                       join RMT in Getregistrationtran on RM.UIN equals RMT.UIN
                                       orderby RM.DateofRegistration descending
                                       select new Patientlists
                                       {
                                           UIN = RM.UIN,
                                           OLDUIN = RM.OLDUIN,
                                           Name = GetConcatName(RM.Name),
                                           MName = GetConcatName(RM.MiddleName),
                                           LName = GetConcatName(RM.LastName),
                                           DOR = Convert.ToDateTime(RM.DateofRegistration + utctime),
                                           Regdate = (RM.DateofRegistration + utctime).ToString("dd-MMM-yyyy HH:mm"),
                                           Assdate = getdates((RMT.AssignedDate)),
                                           Age = PasswordEncodeandDecode.ToAgeString(RM.DateofBirth),
                                           Sex = RM.Gender,
                                           StatusID = RMT.Status,
                                           Status = Getonelinemaster.Where(x => x.OLMID == RMT.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                                           RegistrationTranID = RMT.RegistrationTranID,
                                           AssDoctorID = Getdoctormaster.Where(x => x.DoctorID == RMT.DoctorID).Select(x => x.LastName).FirstOrDefault(),
                                           AssignedDate = getdate(Convert.ToDateTime(RMT.AssignedDate)),

                                       }).ToList();

                }
                else
                {
                    reg.Patientlist = (from RM in Getregistration
                                       join RMT in Getregistrationtran on RM.UIN equals RMT.UIN
                                       join PAT in GetPatientassion on RMT.RegistrationTranID equals PAT.RegistrationTranID
                                       where PAT.DoctorID == userDoctorIDs
                                       orderby RM.DateofRegistration descending
                                       select new Patientlists
                                       {
                                           UIN = RM.UIN,
                                           OLDUIN = RM.OLDUIN,
                                           Name = GetConcatName(RM.Name),
                                           MName = GetConcatName(RM.MiddleName),
                                           LName = GetConcatName(RM.LastName),
                                           DOR = Convert.ToDateTime(RM.DateofRegistration + utctime),
                                           Regdate = (RM.DateofRegistration + utctime).ToString("dd-MMM-yyyy HH:mm"),
                                           Assdate = getdate((PAT.AssignedDate)).ToString("dd-MMM-yyyy HH:mm"),
                                           Age = PasswordEncodeandDecode.ToAgeString(RM.DateofBirth),
                                           Sex = RM.Gender,
                                           StatusID = RMT.Status,
                                           Status = Getonelinemaster.Where(x => x.OLMID == RMT.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                                           RegistrationTranID = PAT.RegistrationTranID,
                                           AssDoctorID = Getdoctormaster.Where(x => x.DoctorID == PAT.DoctorID).Select(x => x.LastName).FirstOrDefault(),
                                           AssignedDate = getdate(Convert.ToDateTime(PAT.AssignedDate)),
                                       }).ToList();
                }
            }
            return reg;
        }
        public RegistrationMasterViewModel GetselectedRevPatientDetails(int ViewsStatusid, string RoleDescription, int userDoctorIDs, DateTime todaydates, int CompanyID)

        {
            var utctimes = CMPSContext.Setup.Where(x => x.CMPID == CompanyID).Select(x => x.UTCTime).FirstOrDefault();

            TimeSpan utctime = TimeSpan.Parse(utctimes);
            var regrev = new RegistrationMasterViewModel();
            regrev.PatientAssignStatus = new List<PatientAssignStatus>();
            var registration = WYNKContext.Registration.Where(x => x.CMPID == CompanyID).ToList();
            var Patientassion = WYNKContext.PatientAssign.AsNoTracking().ToList();
            var registrationtran = WYNKContext.RegistrationTran.ToList();
            var onelinemaster = CMPSContext.OneLineMaster.ToList();
            var doctormaster = CMPSContext.DoctorMaster.ToList();
            var findings = WYNKContext.Findings.ToList();
            var Review = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Review").Select(x => x.OLMID).FirstOrDefault();
            var v = WYNKContext.Admission.Where(x => x.IsSurgeryCompleted == true).ToList();
            if (RoleDescription == "Admin" || RoleDescription == "Reception")
            {

                regrev.RevPatientlist = (from RM in registration
                                         join RMT in registrationtran.Where(x => x.CmpID == CompanyID) on RM.UIN equals RMT.UIN
                                         where v.All(a => a.UIN != RMT.UIN) && RMT.PatientVisitType == Convert.ToString(Review) && RM.IsDeleted == false && RMT.Status == ViewsStatusid
                                         && (RMT.DateofVisit.Date == todaydates.Date)
                                         orderby RMT.DateofVisit descending
                                         select new RevPatientlist
                                         {
                                             RUIN = RM.UIN,
                                             RName = GetConcatName(RM.Name),
                                             RMName = GetConcatName(RM.MiddleName),
                                             RLName = GetConcatName(RM.LastName),
                                             DOV = Convert.ToDateTime(RMT.DateofVisit + utctime),
                                             RAge = PasswordEncodeandDecode.ToAgeString(RM.DateofBirth),
                                             RSex = RM.Gender,
                                             FDate = findings.Where(x => x.UIN == RM.UIN).Select(x => x.ReviewDate).FirstOrDefault(),
                                             RStatus = onelinemaster.Where(x => x.OLMID == RMT.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                                             RegistrationTranID = RMT.RegistrationTranID,
                                             DoctorID = doctormaster.Where(x => x.DoctorID == RMT.DoctorID).Select(x => x.LastName).FirstOrDefault(),
                                             AssignedDate = getdate(Convert.ToDateTime(RMT.AssignedDate)),
                                         }).ToList();

            }
            else
            {
                regrev.RevPatientlist = (from RM in registration
                                         join RMT in registrationtran.Where(x => x.CmpID == CompanyID) on RM.UIN equals RMT.UIN
                                         join PAT in Patientassion on RMT.RegistrationTranID equals PAT.RegistrationTranID
                                         where v.All(a => a.UIN != RMT.UIN) && PAT.DoctorID == userDoctorIDs
                                         && RMT.PatientVisitType == Convert.ToString(Review) && RMT.Status == ViewsStatusid && RM.IsDeleted == false
                                          && (RMT.DateofVisit.Date == todaydates.Date)
                                         orderby RMT.DateofVisit descending
                                         select new RevPatientlist
                                         {
                                             RUIN = RM.UIN,
                                             RName = GetConcatName(RM.Name),
                                             RMName = GetConcatName(RM.MiddleName),
                                             RLName = GetConcatName(RM.LastName),
                                             DOV = Convert.ToDateTime(RMT.DateofVisit + utctime),
                                             RAge = PasswordEncodeandDecode.ToAgeString(RM.DateofBirth),
                                             RSex = RM.Gender,
                                             FDate = findings.Where(x => x.UIN == RM.UIN).Select(x => x.ReviewDate).FirstOrDefault(),
                                             RStatus = onelinemaster.Where(x => x.OLMID == RMT.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                                             RegistrationTranID = RMT.RegistrationTranID,
                                             DoctorID = doctormaster.Where(x => x.DoctorID == RMT.DoctorID).Select(x => x.LastName).FirstOrDefault(),
                                             AssignedDate = getdate(Convert.ToDateTime(RMT.AssignedDate)),
                                         }).ToList();
            }

            return regrev;
        }
        public RegistrationMasterViewModel GetfromtoRevPatientDetails(int ViewsStatusid, string RoleDescription, int userDoctorIDs, DateTime fromdate, DateTime todate, int CompanyID)

        {
            var utctimes = CMPSContext.Setup.Where(x => x.CMPID == CompanyID).Select(x => x.UTCTime).FirstOrDefault();

            TimeSpan utctime = TimeSpan.Parse(utctimes);
            var regrev = new RegistrationMasterViewModel();
            regrev.PatientAssignStatus = new List<PatientAssignStatus>();
            var registration = WYNKContext.Registration.Where(x => x.CMPID == CompanyID).ToList();
            var Patientassion = WYNKContext.PatientAssign.AsNoTracking().ToList();
            var registrationtran = WYNKContext.RegistrationTran.ToList();
            var onelinemaster = CMPSContext.OneLineMaster.ToList();
            var doctormaster = CMPSContext.DoctorMaster.ToList();
            var findings = WYNKContext.Findings.ToList();
            var v = WYNKContext.Admission.Where(x => x.IsSurgeryCompleted == true).ToList();
            var Review = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Review").Select(x => x.OLMID).FirstOrDefault();
            if (RoleDescription == "Admin" || RoleDescription == "Reception")
            {

                regrev.RevPatientlist = (from RM in registration
                                         join RMT in registrationtran.Where(x => x.CmpID == CompanyID) on RM.UIN equals RMT.UIN
                                         where v.All(a => a.UIN != RMT.UIN) && RMT.PatientVisitType == Convert.ToString(Review) && RM.IsDeleted == false && RMT.Status == ViewsStatusid
                                         && (RMT.DateofVisit.Date.Date >= fromdate.Date && RMT.DateofVisit.Date.Date <= todate.Date)
                                         orderby RMT.DateofVisit descending
                                         select new RevPatientlist
                                         {
                                             RUIN = RM.UIN,
                                             RName = GetConcatName(RM.Name),
                                             RMName = GetConcatName(RM.MiddleName),
                                             RLName = GetConcatName(RM.LastName),
                                             DOV = Convert.ToDateTime(RMT.DateofVisit + utctime),
                                             RAge = PasswordEncodeandDecode.ToAgeString(RM.DateofBirth),
                                             RSex = RM.Gender,
                                             FDate = findings.Where(x => x.UIN == RM.UIN).Select(x => x.ReviewDate).FirstOrDefault(),
                                             RStatus = onelinemaster.Where(x => x.OLMID == RMT.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                                             RegistrationTranID = RMT.RegistrationTranID,
                                             DoctorID = doctormaster.Where(x => x.DoctorID == RMT.DoctorID).Select(x => x.LastName).FirstOrDefault(),
                                             AssignedDate = getdate(Convert.ToDateTime(RMT.AssignedDate)),
                                         }).ToList();

            }
            else
            {
                regrev.RevPatientlist = (from RM in registration
                                         join RMT in registrationtran.Where(x => x.CmpID == CompanyID) on RM.UIN equals RMT.UIN
                                         join PAT in Patientassion on RMT.RegistrationTranID equals PAT.RegistrationTranID
                                         where v.All(a => a.UIN != RMT.UIN) && PAT.DoctorID == userDoctorIDs && RMT.PatientVisitType == Convert.ToString(Review) && RMT.Status == ViewsStatusid && RM.IsDeleted == false
                                        && (RMT.DateofVisit.Date.Date >= fromdate.Date && RMT.DateofVisit.Date.Date <= todate.Date)
                                         orderby RMT.DateofVisit descending
                                         select new RevPatientlist
                                         {
                                             RUIN = RM.UIN,
                                             RName = GetConcatName(RM.Name),
                                             RMName = GetConcatName(RM.MiddleName),
                                             RLName = GetConcatName(RM.LastName),
                                             DOV = Convert.ToDateTime(RMT.DateofVisit + utctime),
                                             RAge = PasswordEncodeandDecode.ToAgeString(RM.DateofBirth),
                                             RSex = RM.Gender,
                                             FDate = findings.Where(x => x.UIN == RM.UIN).Select(x => x.ReviewDate).FirstOrDefault(),
                                             RStatus = onelinemaster.Where(x => x.OLMID == RMT.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                                             RegistrationTranID = RMT.RegistrationTranID,
                                             DoctorID = doctormaster.Where(x => x.DoctorID == RMT.DoctorID).Select(x => x.LastName).FirstOrDefault(),
                                             AssignedDate = getdate(Convert.ToDateTime(RMT.AssignedDate)),
                                         }).ToList();
            }

            return regrev;
        }
        public RegistrationMasterViewModel GetfromtoPatientDetails(string ViewsStatusid, string RoleDescription, int userDoctorIDs, DateTime fromdate, DateTime todate, int Companyid)
        {
            var utctimes = CMPSContext.Setup.Where(x => x.CMPID == Companyid).Select(x => x.UTCTime).FirstOrDefault();
            TimeSpan utctime = TimeSpan.Parse(utctimes);
            var reg = new RegistrationMasterViewModel();
            reg.PatientAssignStatus = new List<PatientAssignStatus>();

            if (userDoctorIDs != null)
            {
                var Getonelinemaster = CMPSContext.OneLineMaster.ToList();

                var Getdoctormaster = CMPSContext.DoctorMaster.ToList();
                var sViewsStatusid = Getonelinemaster.Where(x => x.ParentDescription == ViewsStatusid && x.ParentTag == "RegStatus").Select(x => x.OLMID).FirstOrDefault();
                var Getregistration = WYNKContext.Registration.Where(x => x.CMPID == Companyid && x.IsDeleted == false).ToList();
                var Getregistrationtran = WYNKContext.RegistrationTran.Where(x => x.Status == sViewsStatusid && x.CmpID == Companyid && (x.DateofVisit.Date >= fromdate.Date && x.DateofVisit.Date <= todate.Date)).ToList();
                var GetPatientassion = WYNKContext.PatientAssign.AsNoTracking().ToList();

                var New = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "New").Select(x => x.OLMID).FirstOrDefault();
                if (RoleDescription == "Admin" || RoleDescription == "Reception")
                {
                    reg.Patientlist = (from RM in Getregistration
                                       join RMT in Getregistrationtran on RM.UIN equals RMT.UIN
                                       orderby RM.DateofRegistration descending
                                       select new Patientlists
                                       {
                                           UIN = RM.UIN,
                                           OLDUIN = RM.OLDUIN,
                                           Name = GetConcatName(RM.Name),
                                           MName = GetConcatName(RM.MiddleName),
                                           LName = GetConcatName(RM.LastName),
                                           DOR = Convert.ToDateTime(RM.DateofRegistration + utctime),
                                           Regdate = (RM.DateofRegistration + utctime).ToString("dd-MMM-yyyy HH:mm"),
                                           Assdate = getdates((RMT.AssignedDate)),
                                           Age = PasswordEncodeandDecode.ToAgeString(RM.DateofBirth),
                                           Sex = RM.Gender,
                                           StatusID = RMT.Status,
                                           Status = Getonelinemaster.Where(x => x.OLMID == RMT.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                                           RegistrationTranID = RMT.RegistrationTranID,
                                           AssDoctorID = Getdoctormaster.Where(x => x.DoctorID == RMT.DoctorID).Select(x => x.LastName).FirstOrDefault(),
                                           AssignedDate = getdate(Convert.ToDateTime(RMT.AssignedDate)),

                                       }).ToList();
                }
                else
                {
                    reg.Patientlist = (from RM in Getregistration
                                       join RMT in Getregistrationtran on RM.UIN equals RMT.UIN
                                       join PAT in GetPatientassion on RMT.RegistrationTranID equals PAT.RegistrationTranID
                                       where PAT.DoctorID == userDoctorIDs
                                       orderby RM.DateofRegistration descending
                                       select new Patientlists
                                       {
                                           UIN = RM.UIN,
                                           OLDUIN = RM.OLDUIN,
                                           Name = GetConcatName(RM.Name),
                                           MName = GetConcatName(RM.MiddleName),
                                           LName = GetConcatName(RM.LastName),
                                           DOR = Convert.ToDateTime(RM.DateofRegistration + utctime),
                                           Regdate = (RM.DateofRegistration + utctime).ToString("dd-MMM-yyyy HH:mm"),
                                           Assdate = getdate((PAT.AssignedDate)).ToString("dd-MMM-yyyy HH:mm"),
                                           Age = PasswordEncodeandDecode.ToAgeString(RM.DateofBirth),
                                           Sex = RM.Gender,
                                           StatusID = RMT.Status,
                                           Status = Getonelinemaster.Where(x => x.OLMID == RMT.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                                           RegistrationTranID = PAT.RegistrationTranID,
                                           AssDoctorID = Getdoctormaster.Where(x => x.DoctorID == PAT.DoctorID).Select(x => x.LastName).FirstOrDefault(),
                                           AssignedDate = getdate(Convert.ToDateTime(PAT.AssignedDate)),
                                       }).ToList();
                }
            }

            return reg;
        }
        public string ToLastName(int DID, int? Regdoctorid)
        {
            var id = Convert.ToString(DID);
            var Getdoctormaster = CMPSContext.DoctorMaster.ToList();
            if (DID == 38)
            {
                id = "";
            }
            else
            {
                id = Getdoctormaster.Where(x => x.DoctorID == Regdoctorid).Select(x => x.Firstname + ' ' + x.MiddleName + ' ' + x.LastName).FirstOrDefault();
            }
            return Convert.ToString(id);

        }


        public RegistrationMasterI GetDoctorConFee(int DoctorID, int userID, int CampID)
        {
            var confee = new RegistrationMasterI();
            var user = CMPSContext.Users.Where(x => x.CMPID == CampID).AsNoTracking().ToList();
            var DoctorMaster = CMPSContext.DoctorMaster.Where(x => x.CMPID == CampID).AsNoTracking().ToList();
            if (userID != 0)
            {
                DoctorID = DoctorMaster.Where(x => x.EmailID == user.Where(g => g.Userid == userID).Select(v => v.Username).FirstOrDefault()).Select(v => v.DoctorID).FirstOrDefault();
            }




            confee.DoctorID = DoctorID;

            confee.ConsultationFee = 0;

            var id = WYNKContext.Services.Where(x => x.Description == "ConsultationCharges" && x.CMPID == CampID && x.ParentID != 0).Select(c => c.ID).FirstOrDefault();
            var data = WYNKContext.ServiceMaster.Where(x => x.DoctorID == DoctorID && x.ChildDescription == Convert.ToString(id)).Select(c => c.TotalAmount).FirstOrDefault();

            if (data == 0)
            {
                confee.ConsultationFee = 0;
            }
            else
            {
                confee.ConsultationFee = data;
            }


            return confee;
        }
        public RegistrationMasterViewModel GetPatientDetails(string ViewsStatusid, string RoleDescription, int userDoctorIDs, int CompanyID)
        {
            var reg = new RegistrationMasterViewModel();
            reg.PatientAssignStatus = new List<PatientAssignStatus>();
            reg.PaymentMaster = new List<Payment_Master>();
            var utctimes = CMPSContext.Setup.Where(x => x.CMPID == CompanyID).Select(x => x.UTCTime).FirstOrDefault();
            reg.regapplicablevalues = Convert.ToString(CMPSContext.Setup.Where(x => x.CMPID == Convert.ToInt32(CompanyID)).Select(x => x.Registrationfeeapplicable).FirstOrDefault());
            TimeSpan utctime = TimeSpan.Parse(utctimes);
            if (userDoctorIDs != null)
            {
                var Getregistration = WYNKContext.Registration.OrderByDescending(x => x.CreatedUTC).Where(x => x.CMPID == CompanyID).ToList();
                var Getregistrationtran = WYNKContext.RegistrationTran.OrderByDescending(x => x.CreatedUTC).Where(x => x.CmpID == CompanyID).ToList();
                var GetPatientassion = WYNKContext.PatientAssign.AsNoTracking().OrderByDescending(x => x.CreatedUTC).ToList();
                var Getonelinemaster = CMPSContext.OneLineMaster.ToList();

                var Getdoctormaster = CMPSContext.DoctorMaster.ToList();
                var sViewsStatusid = Getonelinemaster.Where(x => x.ParentDescription == ViewsStatusid).Select(x => x.OLMID).FirstOrDefault();
                var New = Getonelinemaster.Where(x => x.ParentDescription == "New" && x.ParentTag == "RegStatus").Select(x => x.OLMID).FirstOrDefault();
                var closed = Getonelinemaster.Where(x => x.ParentDescription == "Closed" && x.ParentTag == "RegStatus").Select(x => x.OLMID).FirstOrDefault();
                reg.Status = Getonelinemaster.Where(x => x.OLMID == sViewsStatusid && x.ParentTag == "RegStatus").Select(x => x.ParentDescription).FirstOrDefault();
                var Allpatients = Getonelinemaster.Where(x => x.ParentDescription == "ALL" && x.ParentTag == "RegStatus").Select(x => x.OLMID).FirstOrDefault();

                if (RoleDescription == "Admin" || RoleDescription == "Reception")
                {
                    if (sViewsStatusid == Allpatients)
                    {
                        reg.Patientlist = (from RM in Getregistration
                                           join RMT in Getregistrationtran on RM.UIN equals RMT.UIN
                                           where RM.IsDeleted == false && RMT.Status != closed
                                           orderby RMT.DateofVisit descending
                                           select new Patientlists
                                           {
                                               UIN = RM.UIN,
                                               OLDUIN = RM.OLDUIN,
                                               Name = GetConcatName(RM.Name),
                                               MName = GetConcatName(RM.MiddleName),
                                               LName = GetConcatName(RM.LastName),
                                               DOR = Convert.ToDateTime(RMT.DateofVisit + utctime),
                                               Regdate = (RM.DateofRegistration + utctime).ToString("dd-MMM-yyyy HH:mm"),
                                               Assdate = getdates((RMT.AssignedDate)),
                                               Age = PasswordEncodeandDecode.ToAgeString(RM.DateofBirth),
                                               DOB = RM.DateofBirth,
                                               Sex = RM.Gender,
                                               StatusID = RMT.Status,
                                               Status = Getonelinemaster.Where(x => x.OLMID == RMT.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                                               AdminName = "Admin",
                                               RegistrationTranID = RMT.RegistrationTranID,
                                               AssDoctorID = Getdoctormaster.Where(x => x.DoctorID == RMT.DoctorID).Select(x => x.Firstname + ' ' + x.MiddleName + ' ' + x.LastName).FirstOrDefault(),
                                               ConsultationFee = RMT.ConsulationFees,
                                               AssignedDate = getdate(GetPatientassion.Where(x => x.RegistrationTranID == RMT.RegistrationTranID).Select(x => x.AssignedDate).FirstOrDefault()),
                                               Statusvisits = CMPSContext.OneLineMaster.Where(x => x.OLMID == Convert.ToInt32(RMT.PatientVisitType) && x.ParentTag == "TOV").Select(x => x.ParentDescription).FirstOrDefault(),
                                           }).ToList();
                        reg.TOTALPatientcount = reg.Patientlist.Select(x => x.UIN).Count();
                    }
                    else
                    {
                        reg.Patientlist = (from RM in Getregistration
                                           join RMT in Getregistrationtran.Where(x => x.Status == sViewsStatusid) on RM.UIN equals RMT.UIN
                                           where RMT.Status == sViewsStatusid && RM.IsDeleted == false
                                           orderby RMT.DateofVisit descending
                                           select new Patientlists
                                           {
                                               UIN = RM.UIN,
                                               OLDUIN = RM.OLDUIN,
                                               Name = GetConcatName(RM.Name),
                                               MName = GetConcatName(RM.MiddleName),
                                               LName = GetConcatName(RM.LastName),
                                               DOR = Convert.ToDateTime(RMT.DateofVisit + utctime),
                                               Regdate = (RM.DateofRegistration + utctime).ToString("dd-MMM-yyyy HH:mm"),
                                               Assdate = getdates((RMT.AssignedDate)),
                                               Age = PasswordEncodeandDecode.ToAgeString(RM.DateofBirth),
                                               DOB = RM.DateofBirth,
                                               Sex = RM.Gender,
                                               StatusID = RMT.Status,
                                               Status = Getonelinemaster.Where(x => x.OLMID == RMT.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                                               RegistrationTranID = RMT.RegistrationTranID,
                                               AdminName = "Admin",
                                               AssDoctorID = ToLastName(sViewsStatusid, RMT.DoctorID),
                                               ConsultationFee = RMT.ConsulationFees,
                                               AssignedDate = getdate(GetPatientassion.Where(x => x.RegistrationTranID == RMT.RegistrationTranID).Select(x => x.AssignedDate).FirstOrDefault()),
                                               Statusvisits = CMPSContext.OneLineMaster.Where(x => x.OLMID == Convert.ToInt32(RMT.PatientVisitType) && x.ParentTag == "TOV").Select(x => x.ParentDescription).FirstOrDefault(),
                                           }).ToList();
                        reg.TOTALPatientcount = reg.Patientlist.Select(x => x.UIN).Count();
                    }

                }
                else if (RoleDescription == "Optometrist")
                {
                    if (sViewsStatusid == Allpatients)
                    {
                        var patientlistdetails = (from RM in Getregistration
                                                  join RMT in Getregistrationtran on RM.UIN equals RMT.UIN
                                                  join PAT in GetPatientassion on RMT.RegistrationTranID equals PAT.RegistrationTranID
                                                  where (PAT.DoctorID == userDoctorIDs) && RM.IsDeleted == false
                                                  orderby RMT.DateofVisit descending
                                                  select new
                                                  {
                                                      UIN = RM.UIN,
                                                      OLDUIN = RM.OLDUIN,
                                                      Name = GetConcatName(RM.Name),
                                                      MName = GetConcatName(RM.MiddleName),
                                                      LName = GetConcatName(RM.LastName),
                                                      DOR = Convert.ToDateTime(RMT.DateofVisit + utctime),
                                                      Regdate = (RM.DateofRegistration + utctime).ToString("dd-MMM-yyyy HH:mm"),
                                                      Assdate = getdate((PAT.AssignedDate)).ToString("dd-MMM-yyyy HH:mm"),
                                                      Age = PasswordEncodeandDecode.ToAgeString(RM.DateofBirth),
                                                      Sex = RM.Gender,
                                                      DOB = RM.DateofBirth,
                                                      StatusID = RMT.Status,
                                                      Status = Getonelinemaster.Where(x => x.OLMID == RMT.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                                                      RegistrationTranID = PAT.RegistrationTranID,
                                                      AssDoctorID = Getdoctormaster.Where(x => x.DoctorID == PAT.DoctorID).Select(x => x.Firstname + ' ' + x.MiddleName + ' ' + x.LastName).FirstOrDefault(),
                                                      ConsultationFee = RMT.ConsulationFees,
                                                      AssignedDate = getdate(Convert.ToDateTime(PAT.AssignedDate)),
                                                      AdminName = "NotAdmin",
                                                      Statusvisits = CMPSContext.OneLineMaster.Where(x => x.OLMID == Convert.ToInt32(RMT.PatientVisitType) && x.ParentTag == "TOV").Select(x => x.ParentDescription).FirstOrDefault(),
                                                  }).ToList();

                        var Nonoptovalues = (from RM in Getregistration
                                             join RMT in Getregistrationtran on RM.UIN equals RMT.UIN
                                             join PAT in GetPatientassion on RMT.RegistrationTranID equals PAT.RegistrationTranID
                                             where (PAT.DoctorID == 0 && PAT.Patientallocatestatus == "O")
                                             && RM.IsDeleted == false
                                             orderby RMT.DateofVisit descending
                                             select new
                                             {
                                                 UIN = RM.UIN,
                                                 OLDUIN = RM.OLDUIN,
                                                 Name = GetConcatName(RM.Name),
                                                 MName = GetConcatName(RM.MiddleName),
                                                 LName = GetConcatName(RM.LastName),
                                                 DOR = Convert.ToDateTime(RMT.DateofVisit + utctime),
                                                 Regdate = (RM.DateofRegistration + utctime).ToString("dd-MMM-yyyy HH:mm"),
                                                 Assdate = getdate((PAT.AssignedDate)).ToString("dd-MMM-yyyy HH:mm"),
                                                 Age = PasswordEncodeandDecode.ToAgeString(RM.DateofBirth),
                                                 Sex = RM.Gender,
                                                 DOB = RM.DateofBirth,
                                                 StatusID = RMT.Status,
                                                 Status = Getonelinemaster.Where(x => x.OLMID == RMT.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                                                 RegistrationTranID = PAT.RegistrationTranID,
                                                 AssDoctorID = Getdoctormaster.Where(x => x.DoctorID == PAT.DoctorID).Select(x => x.Firstname + ' ' + x.MiddleName + ' ' + x.LastName).FirstOrDefault(),
                                                 ConsultationFee = RMT.ConsulationFees,
                                                 AssignedDate = getdate(Convert.ToDateTime(PAT.AssignedDate)),
                                                 AdminName = "NotAdmin",
                                                 Statusvisits = CMPSContext.OneLineMaster.Where(x => x.OLMID == Convert.ToInt32(RMT.PatientVisitType) && x.ParentTag == "TOV").Select(x => x.ParentDescription).FirstOrDefault(),
                                             }).ToList();

                        var cxoncatinationlist = patientlistdetails.Concat(Nonoptovalues);

                        reg.Patientlist = (from gg in patientlistdetails.GroupBy(x => x.UIN)
                                           select new Patientlists
                                           {
                                               UIN = gg.FirstOrDefault().UIN,
                                               OLDUIN = gg.FirstOrDefault().OLDUIN,
                                               Name = GetConcatName(gg.FirstOrDefault().Name),
                                               MName = GetConcatName(gg.FirstOrDefault().MName),
                                               LName = GetConcatName(gg.FirstOrDefault().LName),
                                               DOR = gg.FirstOrDefault().DOR,
                                               Regdate = gg.FirstOrDefault().Regdate,
                                               Assdate = gg.FirstOrDefault().Assdate,
                                               DOB = gg.FirstOrDefault().DOB,
                                               Age = gg.FirstOrDefault().Age,
                                               Sex = gg.FirstOrDefault().Sex,
                                               StatusID = gg.FirstOrDefault().StatusID,
                                               Status = gg.FirstOrDefault().Status,
                                               RegistrationTranID = gg.FirstOrDefault().RegistrationTranID,
                                               AssDoctorID = gg.FirstOrDefault().AssDoctorID,
                                               ConsultationFee = gg.FirstOrDefault().ConsultationFee,
                                               AssignedDate = gg.FirstOrDefault().AssignedDate,
                                               AdminName = "NotAdmin",
                                               Statusvisits = gg.FirstOrDefault().Statusvisits,
                                           }).ToList();



                        reg.TOTALPatientcount = reg.Patientlist.Select(x => x.UIN).Count();
                    }
                    else
                    {

                        var patientdetails = (from RM in Getregistration
                                              join RMT in Getregistrationtran.Where(x => x.Status == sViewsStatusid) on RM.UIN equals RMT.UIN
                                              join PAT in GetPatientassion on RMT.RegistrationTranID equals PAT.RegistrationTranID
                                              where (PAT.DoctorID == userDoctorIDs)
                                               && RM.IsDeleted == false
                                              orderby RMT.DateofVisit descending
                                              select new
                                              {
                                                  UIN = RM.UIN,
                                                  OLDUIN = RM.OLDUIN,
                                                  Name = GetConcatName(RM.Name),
                                                  MName = GetConcatName(RM.MiddleName),
                                                  LName = GetConcatName(RM.LastName),
                                                  DOR = Convert.ToDateTime(RMT.DateofVisit + utctime),
                                                  Age = PasswordEncodeandDecode.ToAgeString(RM.DateofBirth),
                                                  Regdate = (RM.DateofRegistration + utctime).ToString("dd-MMM-yyyy HH:mm"),
                                                  Assdate = getdate((PAT.AssignedDate)).ToString("dd-MMM-yyyy HH:mm"),
                                                  Sex = RM.Gender,
                                                  DOB = RM.DateofBirth,
                                                  StatusID = RMT.Status,
                                                  Status = Getonelinemaster.Where(x => x.OLMID == RMT.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                                                  RegistrationTranID = PAT.RegistrationTranID,
                                                  AssDoctorID = ToLastName(sViewsStatusid, PAT.DoctorID),
                                                  ConsultationFee = RMT.ConsulationFees,
                                                  AssignedDate = getdate(Convert.ToDateTime(PAT.AssignedDate)),
                                                  AdminName = "NotAdmin",
                                                  Statusvisits = CMPSContext.OneLineMaster.Where(x => x.OLMID == Convert.ToInt32(RMT.PatientVisitType) && x.ParentTag == "TOV").Select(x => x.ParentDescription).FirstOrDefault(),
                                              }).ToList();



                        var Nonoptovalues = (from RM in Getregistration
                                             join RMT in Getregistrationtran.Where(x => x.Status == sViewsStatusid) on RM.UIN equals RMT.UIN
                                             join PAT in GetPatientassion on RMT.RegistrationTranID equals PAT.RegistrationTranID
                                             where (PAT.DoctorID == 0 && PAT.Patientallocatestatus == "O")
                                              && RM.IsDeleted == false
                                             orderby RMT.DateofVisit descending
                                             select new
                                             {
                                                 UIN = RM.UIN,
                                                 OLDUIN = RM.OLDUIN,
                                                 Name = GetConcatName(RM.Name),
                                                 MName = GetConcatName(RM.MiddleName),
                                                 LName = GetConcatName(RM.LastName),
                                                 DOR = Convert.ToDateTime(RMT.DateofVisit + utctime),
                                                 Age = PasswordEncodeandDecode.ToAgeString(RM.DateofBirth),
                                                 Regdate = (RM.DateofRegistration + utctime).ToString("dd-MMM-yyyy HH:mm"),
                                                 Assdate = getdate((PAT.AssignedDate)).ToString("dd-MMM-yyyy HH:mm"),
                                                 Sex = RM.Gender,
                                                 DOB = RM.DateofBirth,
                                                 StatusID = RMT.Status,
                                                 Status = Getonelinemaster.Where(x => x.OLMID == RMT.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                                                 RegistrationTranID = PAT.RegistrationTranID,
                                                 AssDoctorID = ToLastName(sViewsStatusid, PAT.DoctorID),
                                                 ConsultationFee = RMT.ConsulationFees,
                                                 AssignedDate = getdate(Convert.ToDateTime(PAT.AssignedDate)),
                                                 AdminName = "NotAdmin",
                                                 Statusvisits = CMPSContext.OneLineMaster.Where(x => x.OLMID == Convert.ToInt32(RMT.PatientVisitType) && x.ParentTag == "TOV").Select(x => x.ParentDescription).FirstOrDefault(),
                                             }).ToList();

                        var cxoncatinationlist = patientdetails.Concat(Nonoptovalues);

                        reg.Patientlist = (from gg in cxoncatinationlist.GroupBy(x => x.UIN)
                                           select new Patientlists
                                           {
                                               UIN = gg.FirstOrDefault().UIN,
                                               OLDUIN = gg.FirstOrDefault().OLDUIN,
                                               Name = GetConcatName(gg.FirstOrDefault().Name),
                                               MName = GetConcatName(gg.FirstOrDefault().MName),
                                               LName = GetConcatName(gg.FirstOrDefault().LName),
                                               DOR = gg.FirstOrDefault().DOR,
                                               Regdate = gg.FirstOrDefault().Regdate,
                                               Assdate = gg.FirstOrDefault().Assdate,
                                               Age = gg.FirstOrDefault().Age,
                                               Sex = gg.FirstOrDefault().Sex,
                                               DOB = gg.FirstOrDefault().DOB,
                                               StatusID = gg.FirstOrDefault().StatusID,
                                               Status = gg.FirstOrDefault().Status,
                                               RegistrationTranID = gg.FirstOrDefault().RegistrationTranID,
                                               AssDoctorID = gg.FirstOrDefault().AssDoctorID,
                                               ConsultationFee = gg.FirstOrDefault().ConsultationFee,
                                               AssignedDate = gg.FirstOrDefault().AssignedDate,
                                               AdminName = "NotAdmin",
                                               Statusvisits = gg.FirstOrDefault().Statusvisits,
                                           }).ToList();

                        reg.TOTALPatientcount = reg.Patientlist.Select(x => x.UIN).Count();
                    }
                }
                else
                {
                    if (sViewsStatusid == Allpatients)
                    {

                        var fullpatiendetails = (from RM in Getregistration
                                                 join RMT in Getregistrationtran on RM.UIN equals RMT.UIN
                                                 join PAT in GetPatientassion on RMT.RegistrationTranID equals PAT.RegistrationTranID
                                                 where (PAT.DoctorID == userDoctorIDs)
                                                 && RM.IsDeleted == false
                                                 orderby RMT.DateofVisit descending
                                                 select new
                                                 {
                                                     UIN = RM.UIN,
                                                     OLDUIN = RM.OLDUIN,
                                                     Name = GetConcatName(RM.Name),
                                                     MName = GetConcatName(RM.MiddleName),
                                                     LName = GetConcatName(RM.LastName),
                                                     DOR = Convert.ToDateTime(RMT.DateofVisit + utctime),
                                                     Regdate = (RM.DateofRegistration + utctime).ToString("dd-MMM-yyyy HH:mm"),
                                                     Assdate = getdate((PAT.AssignedDate)).ToString("dd-MMM-yyyy HH:mm"),
                                                     Age = PasswordEncodeandDecode.ToAgeString(RM.DateofBirth),
                                                     Sex = RM.Gender,
                                                     DOB = RM.DateofBirth,
                                                     StatusID = RMT.Status,
                                                     Status = Getonelinemaster.Where(x => x.OLMID == RMT.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                                                     RegistrationTranID = PAT.RegistrationTranID,
                                                     AssDoctorID = Getdoctormaster.Where(x => x.DoctorID == PAT.DoctorID).Select(x => x.Firstname + ' ' + x.MiddleName + ' ' + x.LastName).FirstOrDefault(),
                                                     ConsultationFee = RMT.ConsulationFees,
                                                     AssignedDate = getdate(Convert.ToDateTime(PAT.AssignedDate)),
                                                     AdminName = "NotAdmin",
                                                     Statusvisits = CMPSContext.OneLineMaster.Where(x => x.OLMID == Convert.ToInt32(RMT.PatientVisitType) && x.ParentTag == "TOV").Select(x => x.ParentDescription).FirstOrDefault(),
                                                 }).ToList();


                        var Nonoptovalues = (from RM in Getregistration
                                             join RMT in Getregistrationtran on RM.UIN equals RMT.UIN
                                             join PAT in GetPatientassion on RMT.RegistrationTranID equals PAT.RegistrationTranID
                                             where (PAT.DoctorID == 0 && PAT.Patientallocatestatus != "O")
                                             && RM.IsDeleted == false
                                             orderby RMT.DateofVisit descending
                                             select new
                                             {
                                                 UIN = RM.UIN,
                                                 OLDUIN = RM.OLDUIN,
                                                 Name = GetConcatName(RM.Name),
                                                 MName = GetConcatName(RM.MiddleName),
                                                 LName = GetConcatName(RM.LastName),
                                                 DOR = Convert.ToDateTime(RMT.DateofVisit + utctime),
                                                 Regdate = (RM.DateofRegistration + utctime).ToString("dd-MMM-yyyy HH:mm"),
                                                 Assdate = getdate((PAT.AssignedDate)).ToString("dd-MMM-yyyy HH:mm"),
                                                 Age = PasswordEncodeandDecode.ToAgeString(RM.DateofBirth),
                                                 Sex = RM.Gender,
                                                 DOB = RM.DateofBirth,
                                                 StatusID = RMT.Status,
                                                 Status = Getonelinemaster.Where(x => x.OLMID == RMT.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                                                 RegistrationTranID = PAT.RegistrationTranID,
                                                 AssDoctorID = Getdoctormaster.Where(x => x.DoctorID == PAT.DoctorID).Select(x => x.Firstname + ' ' + x.MiddleName + ' ' + x.LastName).FirstOrDefault(),
                                                 ConsultationFee = RMT.ConsulationFees,
                                                 AssignedDate = getdate(Convert.ToDateTime(PAT.AssignedDate)),
                                                 AdminName = "NotAdmin",
                                                 Statusvisits = CMPSContext.OneLineMaster.Where(x => x.OLMID == Convert.ToInt32(RMT.PatientVisitType) && x.ParentTag == "TOV").Select(x => x.ParentDescription).FirstOrDefault(),
                                             }).ToList();

                        var cxoncatinationlist = fullpatiendetails.Concat(Nonoptovalues);

                        reg.Patientlist = (from gg in cxoncatinationlist.GroupBy(x => x.UIN)
                                           select new Patientlists
                                           {
                                               UIN = gg.FirstOrDefault().UIN,
                                               OLDUIN = gg.FirstOrDefault().OLDUIN,
                                               Name = GetConcatName(gg.FirstOrDefault().Name),
                                               MName = GetConcatName(gg.FirstOrDefault().MName),
                                               LName = GetConcatName(gg.FirstOrDefault().LName),
                                               DOR = gg.FirstOrDefault().DOR,
                                               Regdate = gg.FirstOrDefault().Regdate,
                                               Assdate = gg.FirstOrDefault().Assdate,
                                               Age = gg.FirstOrDefault().Age,
                                               DOB =gg.FirstOrDefault().DOB,
                                               Sex = gg.FirstOrDefault().Sex,
                                               StatusID = gg.FirstOrDefault().StatusID,
                                               Status = gg.FirstOrDefault().Status,
                                               RegistrationTranID = gg.FirstOrDefault().RegistrationTranID,
                                               AssDoctorID = gg.FirstOrDefault().AssDoctorID,
                                               ConsultationFee = gg.FirstOrDefault().ConsultationFee,
                                               AssignedDate = gg.FirstOrDefault().AssignedDate,
                                               AdminName = "NotAdmin",
                                               Statusvisits = gg.FirstOrDefault().Statusvisits,
                                           }).ToList();
                        reg.TOTALPatientcount = reg.Patientlist.Select(x => x.UIN).Count();
                    }
                    else
                    {
                        var statuspatiendetails = (from RM in Getregistration
                                                   join RMT in Getregistrationtran.Where(x => x.Status == sViewsStatusid) on RM.UIN equals RMT.UIN
                                                   join PAT in GetPatientassion on RMT.RegistrationTranID equals PAT.RegistrationTranID
                                                   where PAT.DoctorID == userDoctorIDs && RM.IsDeleted == false
                                                   orderby RMT.DateofVisit descending
                                                   select new
                                                   {
                                                       UIN = RM.UIN,
                                                       OLDUIN = RM.OLDUIN,
                                                       Name = GetConcatName(RM.Name),
                                                       MName = GetConcatName(RM.MiddleName),
                                                       LName = GetConcatName(RM.LastName),
                                                       DOR = Convert.ToDateTime(RMT.DateofVisit + utctime),
                                                       Regdate = (RM.DateofRegistration + utctime).ToString("dd-MMM-yyyy HH:mm"),
                                                       Assdate = getdate((PAT.AssignedDate)).ToString("dd-MMM-yyyy HH:mm"),
                                                       Age = PasswordEncodeandDecode.ToAgeString(RM.DateofBirth),
                                                       Sex = RM.Gender,
                                                       DOB = RM.DateofBirth,
                                                       StatusID = RMT.Status,
                                                       Status = Getonelinemaster.Where(x => x.OLMID == RMT.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                                                       RegistrationTranID = PAT.RegistrationTranID,
                                                       AssDoctorID = ToLastName(sViewsStatusid, PAT.DoctorID),
                                                       ConsultationFee = RMT.ConsulationFees,
                                                       AssignedDate = getdate(Convert.ToDateTime(PAT.AssignedDate)),
                                                       AdminName = "NotAdmin",
                                                       Statusvisits = CMPSContext.OneLineMaster.Where(x => x.OLMID == Convert.ToInt32(RMT.PatientVisitType) && x.ParentTag == "TOV").Select(x => x.ParentDescription).FirstOrDefault(),
                                                   }).ToList();

                        var Nonoptovalues = (from RM in Getregistration
                                             join RMT in Getregistrationtran.Where(x => x.Status == sViewsStatusid) on RM.UIN equals RMT.UIN
                                             join PAT in GetPatientassion on RMT.RegistrationTranID equals PAT.RegistrationTranID
                                             where (PAT.DoctorID == 0 && PAT.Patientallocatestatus != "O") && RM.IsDeleted == false
                                             orderby RMT.DateofVisit descending
                                             select new
                                             {
                                                 UIN = RM.UIN,
                                                 OLDUIN = RM.OLDUIN,
                                                 Name = GetConcatName(RM.Name),
                                                 MName = GetConcatName(RM.MiddleName),
                                                 LName = GetConcatName(RM.LastName),
                                                 DOR = Convert.ToDateTime(RMT.DateofVisit + utctime),
                                                 Regdate = (RM.DateofRegistration + utctime).ToString("dd-MMM-yyyy HH:mm"),
                                                 Assdate = getdate((PAT.AssignedDate)).ToString("dd-MMM-yyyy HH:mm"),
                                                 Age = PasswordEncodeandDecode.ToAgeString(RM.DateofBirth),
                                                 Sex = RM.Gender,
                                                 DOB = RM.DateofBirth,
                                                 StatusID = RMT.Status,
                                                 Status = Getonelinemaster.Where(x => x.OLMID == RMT.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                                                 RegistrationTranID = PAT.RegistrationTranID,
                                                 AssDoctorID = ToLastName(sViewsStatusid, PAT.DoctorID),
                                                 ConsultationFee = RMT.ConsulationFees,
                                                 AssignedDate = getdate(Convert.ToDateTime(PAT.AssignedDate)),
                                                 AdminName = "NotAdmin",
                                                 Statusvisits = CMPSContext.OneLineMaster.Where(x => x.OLMID == Convert.ToInt32(RMT.PatientVisitType) && x.ParentTag == "TOV").Select(x => x.ParentDescription).FirstOrDefault(),
                                             }).ToList();

                        var cxoncatinationlist = statuspatiendetails.Concat(Nonoptovalues);

                        reg.Patientlist = (from gg in cxoncatinationlist.GroupBy(x => x.UIN)
                                           select new Patientlists
                                           {
                                               UIN = gg.FirstOrDefault().UIN,
                                               OLDUIN = gg.FirstOrDefault().OLDUIN,
                                               Name = GetConcatName(gg.FirstOrDefault().Name),
                                               MName = GetConcatName(gg.FirstOrDefault().MName),
                                               LName = GetConcatName(gg.FirstOrDefault().LName),
                                               DOR = gg.FirstOrDefault().DOR,
                                               Regdate = gg.FirstOrDefault().Regdate,
                                               Assdate = gg.FirstOrDefault().Assdate,
                                               Age = gg.FirstOrDefault().Age,
                                               Sex = gg.FirstOrDefault().Sex,
                                               DOB = gg.FirstOrDefault().DOB,
                                               StatusID = gg.FirstOrDefault().StatusID,
                                               Status = gg.FirstOrDefault().Status,
                                               RegistrationTranID = gg.FirstOrDefault().RegistrationTranID,
                                               AssDoctorID = gg.FirstOrDefault().AssDoctorID,
                                               ConsultationFee = gg.FirstOrDefault().ConsultationFee,
                                               AssignedDate = gg.FirstOrDefault().AssignedDate,
                                               AdminName = "NotAdmin",
                                               Statusvisits = gg.FirstOrDefault().Statusvisits,
                                           }).ToList();
                        reg.TOTALPatientcount = reg.Patientlist.Select(x => x.UIN).Count();


                    }
                }
            }

            return reg;
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
        public RegistrationMasterViewModel GetRevPatientDetails(int ViewsStatusid, string RoleDescription, int userDoctorIDs, int CompanyID)

        {
            var utctimes = CMPSContext.Setup.Where(x => x.CMPID == CompanyID).Select(x => x.UTCTime).FirstOrDefault();

            TimeSpan utctime = TimeSpan.Parse(utctimes);
            var regrev = new RegistrationMasterViewModel();
            regrev.PatientAssignStatus = new List<PatientAssignStatus>();
            var registration = WYNKContext.Registration.ToList();
            var Patientassion = WYNKContext.PatientAssign.AsNoTracking().ToList();
            var registrationtran = WYNKContext.RegistrationTran.ToList();
            var onelinemaster = CMPSContext.OneLineMaster.ToList();
            var doctormaster = CMPSContext.DoctorMaster.ToList();
            var findings = WYNKContext.Findings.ToList();
            var Review = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Review").Select(x => x.OLMID).FirstOrDefault();
            var v = WYNKContext.Admission.Where(x => x.IsSurgeryCompleted == true).ToList();
            regrev.Status = CMPSContext.OneLineMaster.Where(x => x.OLMID == ViewsStatusid).Select(x => x.ParentDescription).FirstOrDefault();
            var closed = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Closed").Select(x => x.OLMID).FirstOrDefault();
            var Allpatients = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "ALL").Select(x => x.OLMID).FirstOrDefault();
            if (RoleDescription == "Admin" || RoleDescription == "Reception")
            {

                if (ViewsStatusid == Allpatients)
                {

                    regrev.RevPatientlist = (from RM in registration
                                             join RMT in registrationtran.Where(x => x.CmpID == CompanyID) on RM.UIN equals RMT.UIN
                                             where v.All(a => a.UIN != RMT.UIN) && RMT.PatientVisitType == Convert.ToString(Review)
                                             && RM.IsDeleted == false && RMT.Status != closed

                                             orderby RMT.DateofVisit descending
                                             select new RevPatientlist
                                             {
                                                 RUIN = RM.UIN,
                                                 RName = GetConcatName(RM.Name),
                                                 RMName = GetConcatName(RM.MiddleName),
                                                 RLName = GetConcatName(RM.LastName),
                                                 DOV = Convert.ToDateTime(RMT.DateofVisit + utctime),
                                                 RAge = PasswordEncodeandDecode.ToAgeString(RM.DateofBirth),
                                                 RSex = RM.Gender,
                                                 FDate = findings.Where(x => x.UIN == RM.UIN).Select(x => x.ReviewDate).FirstOrDefault(),
                                                 RStatus = onelinemaster.Where(x => x.OLMID == RMT.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                                                 RegistrationTranID = RMT.RegistrationTranID,
                                                 DoctorID = doctormaster.Where(x => x.DoctorID == RMT.DoctorID).Select(x => x.LastName).FirstOrDefault(),
                                                 AssignedDate = getdate(Convert.ToDateTime(RMT.AssignedDate)),
                                                 RAdminName = "Admin",
                                             }).ToList();
                    regrev.TOTALREVIEWPatientcount = regrev.RevPatientlist.Select(x => x.RUIN).Count();

                }

                else
                {
                    regrev.RevPatientlist = (from RM in registration
                                             join RMT in registrationtran.Where(x => x.CmpID == CompanyID) on RM.UIN equals RMT.UIN
                                             where v.All(a => a.UIN != RMT.UIN) && RMT.PatientVisitType == Convert.ToString(Review)
                                             && RMT.Status != closed && RMT.Status == ViewsStatusid && RM.IsDeleted == false

                                             orderby RMT.DateofVisit descending
                                             select new RevPatientlist
                                             {
                                                 RUIN = RM.UIN,
                                                 RName = GetConcatName(RM.Name),
                                                 RMName = GetConcatName(RM.MiddleName),
                                                 RLName = GetConcatName(RM.LastName),
                                                 DOV = Convert.ToDateTime(RMT.DateofVisit + utctime),
                                                 RAge = PasswordEncodeandDecode.ToAgeString(RM.DateofBirth),
                                                 RSex = RM.Gender,
                                                 FDate = findings.Where(x => x.UIN == RM.UIN).Select(x => x.ReviewDate).FirstOrDefault(),
                                                 RStatus = onelinemaster.Where(x => x.OLMID == RMT.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                                                 RegistrationTranID = RMT.RegistrationTranID,
                                                 DoctorID = doctormaster.Where(x => x.DoctorID == RMT.DoctorID).Select(x => x.LastName).FirstOrDefault(),
                                                 AssignedDate = getdate(Convert.ToDateTime(RMT.AssignedDate)),
                                                 RAdminName = "Admin",
                                             }).ToList();
                    regrev.TOTALREVIEWPatientcount = regrev.RevPatientlist.Select(x => x.RUIN).Count();
                }

            }
            else if (RoleDescription == "Optometrist")
            {
                if (ViewsStatusid == Allpatients)
                {
                    var allrevpatindetails = (from RM in registration
                                              join RMT in registrationtran.Where(x => x.CmpID == CompanyID) on RM.UIN equals RMT.UIN
                                              join PAT in Patientassion on RMT.RegistrationTranID equals PAT.RegistrationTranID
                                              where v.All(a => a.UIN != RMT.UIN) && (PAT.DoctorID == userDoctorIDs)
                                              && RMT.PatientVisitType == Convert.ToString(Review) && RMT.Status != closed
                                              && RM.IsDeleted == false
                                              orderby RMT.DateofVisit descending
                                              select new
                                              {
                                                  RUIN = RM.UIN,
                                                  RName = GetConcatName(RM.Name),
                                                  RMName = GetConcatName(RM.MiddleName),
                                                  RLName = GetConcatName(RM.LastName),
                                                  DOV = Convert.ToDateTime(RMT.DateofVisit + utctime),
                                                  RAge = PasswordEncodeandDecode.ToAgeString(RM.DateofBirth),
                                                  RSex = RM.Gender,
                                                  FDate = findings.Where(x => x.UIN == RM.UIN).Select(x => x.ReviewDate).FirstOrDefault(),
                                                  RStatus = onelinemaster.Where(x => x.OLMID == RMT.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                                                  RegistrationTranID = RMT.RegistrationTranID,
                                                  DoctorID = doctormaster.Where(x => x.DoctorID == RMT.DoctorID).Select(x => x.LastName).FirstOrDefault(),
                                                  AssignedDate = getdate(Convert.ToDateTime(RMT.AssignedDate)),
                                                  RAdminName = "NotAdmin",
                                              }).ToList();

                    var Nonoptovalues = (from RM in registration
                                         join RMT in registrationtran.Where(x => x.CmpID == CompanyID) on RM.UIN equals RMT.UIN
                                         join PAT in Patientassion on RMT.RegistrationTranID equals PAT.RegistrationTranID
                                         where v.All(a => a.UIN != RMT.UIN) && (PAT.DoctorID == 0 && PAT.Patientallocatestatus == "O")
                                         && RMT.PatientVisitType == Convert.ToString(Review) && RMT.Status != closed
                                         && RM.IsDeleted == false
                                         orderby RMT.DateofVisit descending
                                         select new
                                         {
                                             RUIN = RM.UIN,
                                             RName = GetConcatName(RM.Name),
                                             RMName = GetConcatName(RM.MiddleName),
                                             RLName = GetConcatName(RM.LastName),
                                             DOV = Convert.ToDateTime(RMT.DateofVisit + utctime),
                                             RAge = PasswordEncodeandDecode.ToAgeString(RM.DateofBirth),
                                             RSex = RM.Gender,
                                             FDate = findings.Where(x => x.UIN == RM.UIN).Select(x => x.ReviewDate).FirstOrDefault(),
                                             RStatus = onelinemaster.Where(x => x.OLMID == RMT.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                                             RegistrationTranID = RMT.RegistrationTranID,
                                             DoctorID = doctormaster.Where(x => x.DoctorID == RMT.DoctorID).Select(x => x.LastName).FirstOrDefault(),
                                             AssignedDate = getdate(Convert.ToDateTime(RMT.AssignedDate)),
                                             RAdminName = "NotAdmin",
                                         }).ToList();

                    var cxoncatinationlist = allrevpatindetails.Concat(Nonoptovalues);

                    regrev.RevPatientlist = (from RM in cxoncatinationlist.GroupBy(x => x.RUIN)
                                             select new RevPatientlist
                                             {
                                                 RUIN = RM.FirstOrDefault().RUIN,
                                                 RName = RM.FirstOrDefault().RName,
                                                 RMName = RM.FirstOrDefault().RMName,
                                                 RLName = RM.FirstOrDefault().RLName,
                                                 DOV = RM.FirstOrDefault().DOV,
                                                 RAge = RM.FirstOrDefault().RAge,
                                                 RSex = RM.FirstOrDefault().RSex,
                                                 FDate = RM.FirstOrDefault().FDate,
                                                 RStatus = RM.FirstOrDefault().RStatus,
                                                 RegistrationTranID = RM.FirstOrDefault().RegistrationTranID,
                                                 DoctorID = RM.FirstOrDefault().DoctorID,
                                                 AssignedDate = RM.FirstOrDefault().AssignedDate,
                                                 RAdminName = "NotAdmin",
                                             }).ToList();

                    regrev.TOTALREVIEWPatientcount = regrev.RevPatientlist.Select(x => x.RUIN).Count();
                }
                else
                {

                    var statureviewdetails = (from RM in registration
                                              join RMT in registrationtran.Where(x => x.CmpID == CompanyID) on RM.UIN equals RMT.UIN
                                              join PAT in Patientassion on RMT.RegistrationTranID equals PAT.RegistrationTranID
                                              where v.All(a => a.UIN != RMT.UIN) && (PAT.DoctorID == userDoctorIDs)
                                              && RMT.Status == ViewsStatusid && RMT.PatientVisitType == Convert.ToString(Review)
                                              && RMT.Status != closed && RM.IsDeleted == false

                                              orderby RMT.DateofVisit descending
                                              select new
                                              {
                                                  RUIN = RM.UIN,
                                                  RName = GetConcatName(RM.Name),
                                                  RMName = GetConcatName(RM.MiddleName),
                                                  RLName = GetConcatName(RM.LastName),
                                                  DOV = Convert.ToDateTime(RMT.DateofVisit + utctime),
                                                  RAge = PasswordEncodeandDecode.ToAgeString(RM.DateofBirth),
                                                  RSex = RM.Gender,
                                                  FDate = findings.Where(x => x.UIN == RM.UIN).Select(x => x.ReviewDate).FirstOrDefault(),
                                                  RStatus = onelinemaster.Where(x => x.OLMID == RMT.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                                                  RegistrationTranID = RMT.RegistrationTranID,
                                                  DoctorID = doctormaster.Where(x => x.DoctorID == RMT.DoctorID).Select(x => x.LastName).FirstOrDefault(),
                                                  AssignedDate = getdate(Convert.ToDateTime(RMT.AssignedDate)),
                                                  RAdminName = "NotAdmin",
                                              }).ToList();

                    var Nonoptovalues = (from RM in registration
                                         join RMT in registrationtran.Where(x => x.CmpID == CompanyID) on RM.UIN equals RMT.UIN
                                         join PAT in Patientassion on RMT.RegistrationTranID equals PAT.RegistrationTranID
                                         where v.All(a => a.UIN != RMT.UIN) && (PAT.DoctorID == 0 && PAT.Patientallocatestatus == "O")
                                         && RMT.Status == ViewsStatusid && RMT.PatientVisitType == Convert.ToString(Review)
                                         && RMT.Status != closed && RM.IsDeleted == false

                                         orderby RMT.DateofVisit descending
                                         select new
                                         {
                                             RUIN = RM.UIN,
                                             RName = GetConcatName(RM.Name),
                                             RMName = GetConcatName(RM.MiddleName),
                                             RLName = GetConcatName(RM.LastName),
                                             DOV = Convert.ToDateTime(RMT.DateofVisit + utctime),
                                             RAge = PasswordEncodeandDecode.ToAgeString(RM.DateofBirth),
                                             RSex = RM.Gender,
                                             FDate = findings.Where(x => x.UIN == RM.UIN).Select(x => x.ReviewDate).FirstOrDefault(),
                                             RStatus = onelinemaster.Where(x => x.OLMID == RMT.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                                             RegistrationTranID = RMT.RegistrationTranID,
                                             DoctorID = doctormaster.Where(x => x.DoctorID == RMT.DoctorID).Select(x => x.LastName).FirstOrDefault(),
                                             AssignedDate = getdate(Convert.ToDateTime(RMT.AssignedDate)),
                                             RAdminName = "NotAdmin",
                                         }).ToList();

                    var cxoncatinationlist = statureviewdetails.Concat(Nonoptovalues);

                    regrev.RevPatientlist = (from RM in cxoncatinationlist.GroupBy(x => x.RUIN)
                                             select new RevPatientlist
                                             {
                                                 RUIN = RM.FirstOrDefault().RUIN,
                                                 RName = RM.FirstOrDefault().RName,
                                                 RMName = RM.FirstOrDefault().RMName,
                                                 RLName = RM.FirstOrDefault().RLName,
                                                 DOV = RM.FirstOrDefault().DOV,
                                                 RAge = RM.FirstOrDefault().RAge,
                                                 RSex = RM.FirstOrDefault().RSex,
                                                 FDate = RM.FirstOrDefault().FDate,
                                                 RStatus = RM.FirstOrDefault().RStatus,
                                                 RegistrationTranID = RM.FirstOrDefault().RegistrationTranID,
                                                 DoctorID = RM.FirstOrDefault().DoctorID,
                                                 AssignedDate = RM.FirstOrDefault().AssignedDate,
                                                 RAdminName = "NotAdmin",
                                             }).ToList();


                    regrev.TOTALREVIEWPatientcount = regrev.RevPatientlist.Select(x => x.RUIN).Count();

                }
            }
            else
            {
                if (ViewsStatusid == Allpatients)
                {
                    var statureviewdetails = (from RM in registration
                                              join RMT in registrationtran.Where(x => x.CmpID == CompanyID) on RM.UIN equals RMT.UIN
                                              join PAT in Patientassion on RMT.RegistrationTranID equals PAT.RegistrationTranID
                                              where v.All(a => a.UIN != RMT.UIN) && (PAT.DoctorID == userDoctorIDs)
                                              && RMT.PatientVisitType == Convert.ToString(Review) && RMT.Status != closed
                                              && RM.IsDeleted == false
                                              orderby RMT.DateofVisit descending
                                              select new
                                              {
                                                  RUIN = RM.UIN,
                                                  RName = GetConcatName(RM.Name),
                                                  RMName = GetConcatName(RM.MiddleName),
                                                  RLName = GetConcatName(RM.LastName),
                                                  DOV = Convert.ToDateTime(RMT.DateofVisit + utctime),
                                                  RAge = PasswordEncodeandDecode.ToAgeString(RM.DateofBirth),
                                                  RSex = RM.Gender,
                                                  FDate = findings.Where(x => x.UIN == RM.UIN).Select(x => x.ReviewDate).FirstOrDefault(),
                                                  RStatus = onelinemaster.Where(x => x.OLMID == RMT.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                                                  RegistrationTranID = RMT.RegistrationTranID,
                                                  DoctorID = doctormaster.Where(x => x.DoctorID == RMT.DoctorID).Select(x => x.LastName).FirstOrDefault(),
                                                  AssignedDate = getdate(Convert.ToDateTime(RMT.AssignedDate)),
                                                  RAdminName = "NotAdmin",
                                              }).ToList();


                    var Nonoptovalues = (from RM in registration
                                         join RMT in registrationtran.Where(x => x.CmpID == CompanyID) on RM.UIN equals RMT.UIN
                                         join PAT in Patientassion on RMT.RegistrationTranID equals PAT.RegistrationTranID
                                         where v.All(a => a.UIN != RMT.UIN) && (PAT.Patientallocatestatus != "O" && PAT.DoctorID == 0)
                                         && RMT.PatientVisitType == Convert.ToString(Review) && RMT.Status != closed
                                         && RM.IsDeleted == false
                                         orderby RMT.DateofVisit descending
                                         select new
                                         {
                                             RUIN = RM.UIN,
                                             RName = GetConcatName(RM.Name),
                                             RMName = GetConcatName(RM.MiddleName),
                                             RLName = GetConcatName(RM.LastName),
                                             DOV = Convert.ToDateTime(RMT.DateofVisit + utctime),
                                             RAge = PasswordEncodeandDecode.ToAgeString(RM.DateofBirth),
                                             RSex = RM.Gender,
                                             FDate = findings.Where(x => x.UIN == RM.UIN).Select(x => x.ReviewDate).FirstOrDefault(),
                                             RStatus = onelinemaster.Where(x => x.OLMID == RMT.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                                             RegistrationTranID = RMT.RegistrationTranID,
                                             DoctorID = doctormaster.Where(x => x.DoctorID == RMT.DoctorID).Select(x => x.LastName).FirstOrDefault(),
                                             AssignedDate = getdate(Convert.ToDateTime(RMT.AssignedDate)),
                                             RAdminName = "NotAdmin",
                                         }).ToList();

                    var cxoncatinationlist = statureviewdetails.Concat(Nonoptovalues);

                    regrev.RevPatientlist = (from RM in cxoncatinationlist.GroupBy(x => x.RUIN)
                                             select new RevPatientlist
                                             {
                                                 RUIN = RM.FirstOrDefault().RUIN,
                                                 RName = RM.FirstOrDefault().RName,
                                                 RMName = RM.FirstOrDefault().RMName,
                                                 RLName = RM.FirstOrDefault().RLName,
                                                 DOV = RM.FirstOrDefault().DOV,
                                                 RAge = RM.FirstOrDefault().RAge,
                                                 RSex = RM.FirstOrDefault().RSex,
                                                 FDate = RM.FirstOrDefault().FDate,
                                                 RStatus = RM.FirstOrDefault().RStatus,
                                                 RegistrationTranID = RM.FirstOrDefault().RegistrationTranID,
                                                 DoctorID = RM.FirstOrDefault().DoctorID,
                                                 AssignedDate = RM.FirstOrDefault().AssignedDate,
                                                 RAdminName = "NotAdmin",
                                             }).ToList();


                    regrev.TOTALREVIEWPatientcount = regrev.RevPatientlist.Select(x => x.RUIN).Count();
                }
                else
                {
                    var ssstatureviewdetails = (from RM in registration
                                                join RMT in registrationtran.Where(x => x.CmpID == CompanyID) on RM.UIN equals RMT.UIN
                                                join PAT in Patientassion on RMT.RegistrationTranID equals PAT.RegistrationTranID
                                                where v.All(a => a.UIN != RMT.UIN) && (PAT.DoctorID == userDoctorIDs)
                                                && RMT.Status == ViewsStatusid && RMT.PatientVisitType == Convert.ToString(Review)
                                                && RMT.Status != closed && RM.IsDeleted == false

                                                orderby RMT.DateofVisit descending
                                                select new
                                                {
                                                    RUIN = RM.UIN,
                                                    RName = GetConcatName(RM.Name),
                                                    RMName = GetConcatName(RM.MiddleName),
                                                    RLName = GetConcatName(RM.LastName),
                                                    DOV = Convert.ToDateTime(RMT.DateofVisit + utctime),
                                                    RAge = PasswordEncodeandDecode.ToAgeString(RM.DateofBirth),
                                                    RSex = RM.Gender,
                                                    FDate = findings.Where(x => x.UIN == RM.UIN).Select(x => x.ReviewDate).FirstOrDefault(),
                                                    RStatus = onelinemaster.Where(x => x.OLMID == RMT.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                                                    RegistrationTranID = RMT.RegistrationTranID,
                                                    DoctorID = doctormaster.Where(x => x.DoctorID == RMT.DoctorID).Select(x => x.LastName).FirstOrDefault(),
                                                    AssignedDate = getdate(Convert.ToDateTime(RMT.AssignedDate)),
                                                    RAdminName = "NotAdmin",
                                                }).ToList();


                    var Nonoptovalues = (from RM in registration
                                         join RMT in registrationtran.Where(x => x.CmpID == CompanyID) on RM.UIN equals RMT.UIN
                                         join PAT in Patientassion on RMT.RegistrationTranID equals PAT.RegistrationTranID
                                         where v.All(a => a.UIN != RMT.UIN) && (PAT.Patientallocatestatus != "O" && PAT.DoctorID == 0)
                                         && RMT.PatientVisitType == Convert.ToString(Review) && RMT.Status != closed
                                         && RM.IsDeleted == false
                                         orderby RMT.DateofVisit descending
                                         select new
                                         {
                                             RUIN = RM.UIN,
                                             RName = GetConcatName(RM.Name),
                                             RMName = GetConcatName(RM.MiddleName),
                                             RLName = GetConcatName(RM.LastName),
                                             DOV = Convert.ToDateTime(RMT.DateofVisit + utctime),
                                             RAge = PasswordEncodeandDecode.ToAgeString(RM.DateofBirth),
                                             RSex = RM.Gender,
                                             FDate = findings.Where(x => x.UIN == RM.UIN).Select(x => x.ReviewDate).FirstOrDefault(),
                                             RStatus = onelinemaster.Where(x => x.OLMID == RMT.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                                             RegistrationTranID = RMT.RegistrationTranID,
                                             DoctorID = doctormaster.Where(x => x.DoctorID == RMT.DoctorID).Select(x => x.LastName).FirstOrDefault(),
                                             AssignedDate = getdate(Convert.ToDateTime(RMT.AssignedDate)),
                                             RAdminName = "NotAdmin",
                                         }).ToList();

                    var cxoncatinationlist = ssstatureviewdetails.Concat(Nonoptovalues);

                    regrev.RevPatientlist = (from RM in cxoncatinationlist.GroupBy(x => x.RUIN)
                                             select new RevPatientlist
                                             {
                                                 RUIN = RM.FirstOrDefault().RUIN,
                                                 RName = RM.FirstOrDefault().RName,
                                                 RMName = RM.FirstOrDefault().RMName,
                                                 RLName = RM.FirstOrDefault().RLName,
                                                 DOV = RM.FirstOrDefault().DOV,
                                                 RAge = RM.FirstOrDefault().RAge,
                                                 RSex = RM.FirstOrDefault().RSex,
                                                 FDate = RM.FirstOrDefault().FDate,
                                                 RStatus = RM.FirstOrDefault().RStatus,
                                                 RegistrationTranID = RM.FirstOrDefault().RegistrationTranID,
                                                 DoctorID = RM.FirstOrDefault().DoctorID,
                                                 AssignedDate = RM.FirstOrDefault().AssignedDate,
                                                 RAdminName = "NotAdmin",
                                             }).ToList();

                    regrev.TOTALREVIEWPatientcount = regrev.RevPatientlist.Select(x => x.RUIN).Count();

                }
            }

            return regrev;
        }
        public RegistrationMasterViewModel GetSurgeryRevPatientDetails(int ViewsStatusid, string RoleDescription, int userDoctorIDs, int CompanyID)
        {
            var utctimes = CMPSContext.Setup.Where(x => x.CMPID == CompanyID).Select(x => x.UTCTime).FirstOrDefault();

            TimeSpan utctime = TimeSpan.Parse(utctimes);
            var reg = new RegistrationMasterViewModel();
            reg.PatientAssignStatus = new List<PatientAssignStatus>();
            reg.PaymentMaster = new List<Payment_Master>();
            if (userDoctorIDs != null)
            {

                var Getregistration = WYNKContext.Registration.ToList();
                var Getregistrationtran = WYNKContext.RegistrationTran.ToList();
                var GetPatientassion = WYNKContext.PatientAssign.AsNoTracking().ToList();
                var Getonelinemaster = CMPSContext.OneLineMaster.ToList();
                var Doctormas = CMPSContext.DoctorMaster.ToList();
                var Review = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Review").Select(x => x.OLMID).FirstOrDefault();
                var Find = WYNKContext.Findings.ToList();
                var Findext = WYNKContext.FindingsExt.ToList();
                var ICDSPecial = WYNKContext.ICDSpecialityCode.ToList();
                var closed = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Closed").Select(x => x.OLMID).FirstOrDefault();
                var Allpatients = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "ALL").Select(x => x.OLMID).FirstOrDefault();
                reg.Status = CMPSContext.OneLineMaster.Where(x => x.OLMID == ViewsStatusid).Select(x => x.ParentDescription).FirstOrDefault();
                if (RoleDescription == "Admin" || RoleDescription == "Reception")
                {
                    if (ViewsStatusid == Allpatients)
                    {
                        var Patientlistss = (from Fext in Findext
                                             join F in Find.Where(x => x.CmpID == CompanyID) on Fext.FindingsID equals F.RandomUniqueID
                                             join RegTran in Getregistrationtran.Where(x => x.CmpID == CompanyID) on F.UIN equals RegTran.UIN
                                             join Reg in Getregistration on F.UIN equals Reg.UIN
                                             join ic in ICDSPecial on Fext.ICDSpecialityid equals ic.ID
                                             where Fext.Isdeleted == false && ic.IsActive == true
                                             && RegTran.PatientVisitType == Convert.ToString(Review) && RegTran.Status != closed
                                             orderby Reg.DateofRegistration descending
                                             select new
                                             {
                                                 UIN = Reg.UIN,
                                                 Name = GetConcatName(Reg.Name),
                                                 MName = GetConcatName(Reg.MiddleName),
                                                 LName = GetConcatName(Reg.LastName),
                                                 DOR = Convert.ToDateTime(RegTran.DateofVisit + utctime),
                                                 Age = PasswordEncodeandDecode.ToAgeString(Reg.DateofBirth),
                                                 Sex = Reg.Gender,
                                                 LastName = Doctormas.Where(x => x.DoctorID == RegTran.DoctorID).Select(x => x.LastName).FirstOrDefault(),
                                                 Status = Getonelinemaster.Where(x => x.OLMID == RegTran.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                                                 RegistrationTranID = RegTran.RegistrationTranID,
                                                 AssDoctorID = Doctormas.Where(x => x.DoctorID == RegTran.DoctorID).Select(x => x.LastName).FirstOrDefault(),
                                                 AssignedDate = getdate(Convert.ToDateTime(RegTran.AssignedDate)),
                                                 ReviewDate = Convert.ToDateTime(Fext.SureryReviewDate + utctime),
                                             }).ToList();


                        reg.Patientlist = (from oo in Patientlistss.GroupBy(x => x.RegistrationTranID)
                                           select new Patientlists
                                           {
                                               UIN = oo.FirstOrDefault().UIN,
                                               Name = GetConcatName(oo.FirstOrDefault().Name),
                                               MName = GetConcatName(oo.FirstOrDefault().MName),
                                               LName = GetConcatName(oo.FirstOrDefault().LName),
                                               DOR = oo.FirstOrDefault().DOR,
                                               Age = oo.FirstOrDefault().Age,
                                               Sex = oo.FirstOrDefault().Sex,
                                               LastName = oo.FirstOrDefault().LastName,
                                               Status = oo.FirstOrDefault().Status,
                                               AssDoctorID = oo.FirstOrDefault().AssDoctorID,
                                               ReviewDate = oo.FirstOrDefault().ReviewDate,
                                               RegistrationTranID = oo.FirstOrDefault().RegistrationTranID,
                                               AssignedDate = oo.FirstOrDefault().AssignedDate,
                                               AdminName = "Admin",
                                           }).ToList();

                    }
                    else
                    {
                        var Patientlistss = (from Fext in Findext
                                             join F in Find.Where(x => x.CmpID == CompanyID) on Fext.FindingsID equals F.RandomUniqueID
                                             join RegTran in Getregistrationtran.Where(x => x.CmpID == CompanyID) on F.UIN equals RegTran.UIN
                                             join Reg in Getregistration on F.UIN equals Reg.UIN
                                             join ic in ICDSPecial on Fext.ICDSpecialityid equals ic.ID
                                             where Fext.Isdeleted == false && ic.IsActive == true
                                             && RegTran.PatientVisitType == Convert.ToString(Review) && RegTran.Status == ViewsStatusid
                                             orderby Reg.DateofRegistration descending
                                             select new
                                             {
                                                 UIN = Reg.UIN,
                                                 Name = GetConcatName(Reg.Name),
                                                 MName = GetConcatName(Reg.MiddleName),
                                                 LName = GetConcatName(Reg.LastName),
                                                 DOR = Convert.ToDateTime(RegTran.DateofVisit + utctime),
                                                 Age = PasswordEncodeandDecode.ToAgeString(Reg.DateofBirth),
                                                 Sex = Reg.Gender,
                                                 LastName = Doctormas.Where(x => x.DoctorID == RegTran.DoctorID).Select(x => x.LastName).FirstOrDefault(),
                                                 Status = Getonelinemaster.Where(x => x.OLMID == RegTran.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                                                 RegistrationTranID = RegTran.RegistrationTranID,
                                                 AssDoctorID = Doctormas.Where(x => x.DoctorID == RegTran.DoctorID).Select(x => x.LastName).FirstOrDefault(),
                                                 AssignedDate = getdate(Convert.ToDateTime(RegTran.AssignedDate)),
                                                 ReviewDate = Convert.ToDateTime(Fext.SureryReviewDate + utctime),
                                             }).ToList();


                        reg.Patientlist = (from oo in Patientlistss.GroupBy(x => x.RegistrationTranID)
                                           select new Patientlists
                                           {
                                               UIN = oo.FirstOrDefault().UIN,

                                               Name = GetConcatName(oo.FirstOrDefault().Name),
                                               MName = GetConcatName(oo.FirstOrDefault().MName),
                                               LName = GetConcatName(oo.FirstOrDefault().LName),
                                               DOR = oo.FirstOrDefault().DOR,
                                               Age = oo.FirstOrDefault().Age,
                                               Sex = oo.FirstOrDefault().Sex,
                                               LastName = oo.FirstOrDefault().LastName,
                                               Status = oo.FirstOrDefault().Status,
                                               AssDoctorID = oo.FirstOrDefault().AssDoctorID,
                                               ReviewDate = oo.FirstOrDefault().ReviewDate,
                                               RegistrationTranID = oo.FirstOrDefault().RegistrationTranID,
                                               AssignedDate = oo.FirstOrDefault().AssignedDate,
                                               AdminName = "Admin",
                                           }).ToList();
                    }
                }
                else
                {
                    if (ViewsStatusid == Allpatients)
                    {
                        var Patientlistss = (from Fext in Findext
                                             join F in Find.Where(x => x.CmpID == CompanyID) on Fext.FindingsID equals F.RandomUniqueID
                                             join RegTran in Getregistrationtran.Where(x => x.CmpID == CompanyID) on F.UIN equals RegTran.UIN
                                             join Reg in Getregistration on F.UIN equals Reg.UIN
                                             join ic in ICDSPecial on Fext.ICDSpecialityid equals ic.ID
                                             join Pt in GetPatientassion on RegTran.RegistrationTranID equals Pt.RegistrationTranID
                                             where Fext.Isdeleted == false && ic.IsActive == true
                                             && RegTran.PatientVisitType == Convert.ToString(Review) && RegTran.Status != closed
                                             && Pt.DoctorID == userDoctorIDs
                                             orderby Reg.DateofRegistration descending
                                             select new
                                             {
                                                 UIN = Reg.UIN,
                                                 Name = GetConcatName(Reg.Name),
                                                 MName = GetConcatName(Reg.MiddleName),
                                                 LName = GetConcatName(Reg.LastName),
                                                 DOR = Convert.ToDateTime(RegTran.DateofVisit + utctime),
                                                 Age = PasswordEncodeandDecode.ToAgeString(Reg.DateofBirth),
                                                 Sex = Reg.Gender,
                                                 LastName = Doctormas.Where(x => x.DoctorID == RegTran.DoctorID).Select(x => x.LastName).FirstOrDefault(),
                                                 Status = Getonelinemaster.Where(x => x.OLMID == RegTran.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                                                 RegistrationTranID = RegTran.RegistrationTranID,
                                                 AssDoctorID = Doctormas.Where(x => x.DoctorID == RegTran.DoctorID).Select(x => x.LastName).FirstOrDefault(),
                                                 AssignedDate = getdate(Convert.ToDateTime(RegTran.AssignedDate)),
                                                 ReviewDate = Convert.ToDateTime(Fext.SureryReviewDate + utctime),
                                             }).ToList();


                        reg.Patientlist = (from oo in Patientlistss.GroupBy(x => x.RegistrationTranID)
                                           select new Patientlists
                                           {
                                               UIN = oo.FirstOrDefault().UIN,

                                               Name = GetConcatName(oo.FirstOrDefault().Name),
                                               MName = GetConcatName(oo.FirstOrDefault().MName),
                                               LName = GetConcatName(oo.FirstOrDefault().LName),
                                               DOR = oo.FirstOrDefault().DOR,
                                               Age = oo.FirstOrDefault().Age,
                                               Sex = oo.FirstOrDefault().Sex,
                                               LastName = oo.FirstOrDefault().LastName,
                                               Status = oo.FirstOrDefault().Status,
                                               AssDoctorID = oo.FirstOrDefault().AssDoctorID,
                                               ReviewDate = oo.FirstOrDefault().ReviewDate,
                                               RegistrationTranID = oo.FirstOrDefault().RegistrationTranID,
                                               AssignedDate = oo.FirstOrDefault().AssignedDate,
                                           }).ToList();
                    }
                    else
                    {
                        var Patientlistss = (from Fext in Findext
                                             join F in Find.Where(x => x.CmpID == CompanyID) on Fext.FindingsID equals F.RandomUniqueID
                                             join RegTran in Getregistrationtran.Where(x => x.CmpID == CompanyID) on F.UIN equals RegTran.UIN
                                             join Reg in Getregistration on F.UIN equals Reg.UIN
                                             join ic in ICDSPecial on Fext.ICDSpecialityid equals ic.ID
                                             join Pt in GetPatientassion on RegTran.RegistrationTranID equals Pt.RegistrationTranID
                                             where Fext.Isdeleted == false && ic.IsActive == true
                                             && RegTran.PatientVisitType == Convert.ToString(Review) && RegTran.Status == ViewsStatusid
                                             && Pt.DoctorID == userDoctorIDs
                                             orderby Reg.DateofRegistration descending
                                             select new
                                             {
                                                 UIN = Reg.UIN,
                                                 Name = GetConcatName(Reg.Name),
                                                 MName = GetConcatName(Reg.MiddleName),
                                                 LName = GetConcatName(Reg.LastName),
                                                 DOR = Convert.ToDateTime(RegTran.DateofVisit + utctime),
                                                 Age = PasswordEncodeandDecode.ToAgeString(Reg.DateofBirth),
                                                 Sex = Reg.Gender,
                                                 LastName = Doctormas.Where(x => x.DoctorID == RegTran.DoctorID).Select(x => x.LastName).FirstOrDefault(),
                                                 Status = Getonelinemaster.Where(x => x.OLMID == RegTran.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                                                 RegistrationTranID = RegTran.RegistrationTranID,
                                                 AssDoctorID = Doctormas.Where(x => x.DoctorID == RegTran.DoctorID).Select(x => x.LastName).FirstOrDefault(),
                                                 AssignedDate = getdate(Convert.ToDateTime(RegTran.AssignedDate)),
                                                 ReviewDate = Convert.ToDateTime(Fext.SureryReviewDate + utctime),
                                             }).ToList();


                        reg.Patientlist = (from oo in Patientlistss.GroupBy(x => x.RegistrationTranID)
                                           select new Patientlists
                                           {
                                               UIN = oo.FirstOrDefault().UIN,

                                               Name = GetConcatName(oo.FirstOrDefault().Name),
                                               MName = GetConcatName(oo.FirstOrDefault().MName),
                                               LName = GetConcatName(oo.FirstOrDefault().LName),
                                               DOR = oo.FirstOrDefault().DOR,
                                               Age = oo.FirstOrDefault().Age,
                                               Sex = oo.FirstOrDefault().Sex,
                                               LastName = oo.FirstOrDefault().LastName,
                                               Status = oo.FirstOrDefault().Status,
                                               AssDoctorID = oo.FirstOrDefault().AssDoctorID,
                                               ReviewDate = oo.FirstOrDefault().ReviewDate,
                                               RegistrationTranID = oo.FirstOrDefault().RegistrationTranID,
                                               AssignedDate = oo.FirstOrDefault().AssignedDate,
                                           }).ToList();
                    }

                }

            }

            return reg;
        }


        public dynamic InsertPatientAssign(RegistrationMasterViewModel InsertPatientAssign, int userID)
        {
            var Regtran = new RegistrationTran_Master();
            var PatientAssignn = new Patient_Assign();
            var PatientAssign = new Patient_Assign();
            var cmpid = InsertPatientAssign.Cmpid;
            var utctimes = CMPSContext.Setup.Where(x => x.CMPID == cmpid).Select(x => x.UTCTime).FirstOrDefault();
            TimeSpan utctime = TimeSpan.Parse(utctimes);

            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in InsertPatientAssign.PatientAssignStatus)
                    {

                        Regtran = WYNKContext.RegistrationTran.Where(x => x.RegistrationTranID == Convert.ToInt32(item.RegistrationTranID)).FirstOrDefault();
                        if (item.StatusID == "Open")
                        {
                            if (item.DoctorID != 0)
                            {
                                Regtran.DoctorID = item.DoctorID;
                                Regtran.Status = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Assigned").Select(x => x.OLMID).FirstOrDefault();
                            }

                            else
                            {
                                Regtran.DoctorID = 0;
                                Regtran.Status = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Assigned").Select(x => x.OLMID).FirstOrDefault();
                            }


                            Regtran.AssignedDate = System.DateTime.Now;
                        }
                        else if (item.StatusID == "Assigned")
                        {

                            if (item.DoctorID != 0)
                            {
                                Regtran.DoctorID = item.DoctorID;
                                Regtran.Status = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Reassigned").Select(x => x.OLMID).FirstOrDefault();
                            }
                            else if (item.OptometristcID != 0)
                            {
                                Regtran.Status = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Reassigned").Select(x => x.OLMID).FirstOrDefault();
                            }
                            else
                            {
                                Regtran.Status = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Reassigned").Select(x => x.OLMID).FirstOrDefault();
                            }

                            Regtran.AssignedDate = DateTime.UtcNow + utctime;
                        }
                        WYNKContext.Entry(Regtran).State = EntityState.Modified;

                    }
                    foreach (var item in InsertPatientAssign.PatientAssignStatus)
                    {

                        if (item.DoctorID != 0)
                        {

                            var details = WYNKContext.PatientAssign.OrderByDescending(x => x.CreatedUTC).Where(x => x.RegistrationTranID == Convert.ToInt32(item.RegistrationTranID) && x.Patientallocatestatus == "D").FirstOrDefault();
                            if (details != null)
                            {
                                details.Patientallocatestatus = null;
                                details.DoctorID = item.DoctorID;
                                details.RegistrationTranID = Convert.ToInt32(item.RegistrationTranID);
                                details.CreatedBy = userID;
                                details.AssignedDate = DateTime.UtcNow + utctime;
                                details.CreatedUTC = DateTime.UtcNow;
                                WYNKContext.PatientAssign.UpdateRange(details);
                                WYNKContext.SaveChanges();

                                var tran = WYNKContext.Workflowmaster.Where(x => x.Description == "Doctor").Select(x => x.ID).FirstOrDefault();
                                var CurDate = DateTime.UtcNow;
                                var workflow = WYNKContext.Workflowtran.Where(x => x.Regtranid == details.RegistrationTranID && x.CMPID == cmpid && x.Createdutc.Date == CurDate.Date).ToList();
                                var times1 = CurDate.Hour + ":" + CurDate.Minute + ":" + CurDate.Second;
                                var times2 = utctime;
                                var fulltime = TimeSpan.Parse(times1) + times2;
                                DateTime time = DateTime.Today.Add(fulltime);
                                var curfullh = time.Hour;
                                var curfullm = time.Minute;
                                string displayTimes = curfullh + ":" + curfullm;
                                foreach (var itemm in workflow)
                                {
                                    var wwfl = workflow.Where(x => x.ID == itemm.ID && x.status == true).FirstOrDefault();
                                    if (wwfl != null)
                                    {

                                        wwfl.Checkouttime = displayTimes;
                                        wwfl.status = false;
                                        wwfl.Optometrist = false;
                                        wwfl.Reception = false;
                                        wwfl.Date = DateTime.UtcNow.Date;
                                        WYNKContext.Workflowtran.UpdateRange(wwfl);
                                        WYNKContext.SaveChanges();
                                    }
                                }

                                var wflowtran = new Workflowtran();

                                wflowtran.Checkintime = displayTimes;
                                wflowtran.Regtranid = details.RegistrationTranID;
                                wflowtran.Workflowmasterid = WYNKContext.Workflowmaster.Where(x => x.Description == "Doctor").Select(x => x.ID).FirstOrDefault();
                                wflowtran.Optometrist = false;
                                wflowtran.Doctor = true;
                                wflowtran.Reception = false;
                                wflowtran.status = true;
                                wflowtran.Doctorid = item.DoctorID;
                                wflowtran.Date = DateTime.UtcNow.Date;
                                wflowtran.Createdutc = DateTime.UtcNow;
                                wflowtran.CMPID = cmpid;
                                wflowtran.Createdby = Convert.ToInt32(userID);
                                WYNKContext.Workflowtran.AddRange(wflowtran);
                                WYNKContext.SaveChanges();

                            }
                            else
                            {
                                PatientAssign.DoctorID = item.DoctorID;
                                PatientAssign.RegistrationTranID = Convert.ToInt32(item.RegistrationTranID);
                                PatientAssign.CreatedBy = userID;
                                PatientAssign.AssignedDate = DateTime.UtcNow + utctime;
                                PatientAssign.CreatedUTC = DateTime.UtcNow;
                                var idd = WYNKContext.PatientAssign.Select(x => x.ID).LastOrDefault();
                                if (idd < 1)
                                {
                                    PatientAssign.ID = 1;
                                }
                                else
                                {
                                    PatientAssign.ID = idd + 1;
                                }
                                WYNKContext.PatientAssign.Add(PatientAssign);
                                WYNKContext.SaveChanges();


                                var tran = WYNKContext.Workflowmaster.Where(x => x.Description == "Doctor").Select(x => x.ID).FirstOrDefault();
                                var CurDate = DateTime.UtcNow;
                                var workflow = WYNKContext.Workflowtran.Where(x => x.Regtranid == details.RegistrationTranID && x.CMPID == cmpid && x.Createdutc.Date == CurDate.Date).ToList();
                                var times1 = CurDate.Hour + ":" + CurDate.Minute + ":" + CurDate.Second;
                                var times2 = utctime;
                                var fulltime = TimeSpan.Parse(times1) + times2;
                                DateTime time = DateTime.Today.Add(fulltime);
                                var curfullh = time.Hour;
                                var curfullm = time.Minute;
                                string displayTimes = curfullh + ":" + curfullm;
                                foreach (var itemm in workflow)
                                {
                                    var wwfl = workflow.Where(x => x.ID == itemm.ID && x.status == true).FirstOrDefault();
                                    if (wwfl != null)
                                    {
                                        wwfl.Checkouttime = displayTimes;
                                        wwfl.status = false;
                                        wwfl.Optometrist = false;
                                        wwfl.Reception = false;
                                        wwfl.Date = DateTime.UtcNow.Date;
                                        WYNKContext.Workflowtran.UpdateRange(wwfl);
                                        WYNKContext.SaveChanges();
                                    }
                                }

                                var wflowtran = new Workflowtran();

                                wflowtran.Checkintime = displayTimes;
                                wflowtran.Regtranid = details.RegistrationTranID;
                                wflowtran.Workflowmasterid = WYNKContext.Workflowmaster.Where(x => x.Description == "Doctor").Select(x => x.ID).FirstOrDefault();
                                wflowtran.Optometrist = false;
                                wflowtran.Doctor = true;
                                wflowtran.Reception = false;
                                wflowtran.status = true;
                                wflowtran.Doctorid = item.DoctorID;
                                wflowtran.Date = DateTime.UtcNow.Date;
                                wflowtran.CMPID = cmpid;
                                wflowtran.Createdutc = DateTime.UtcNow;
                                wflowtran.Createdby = Convert.ToInt32(userID);
                                WYNKContext.Workflowtran.AddRange(wflowtran);
                                WYNKContext.SaveChanges();

                            }
                        }
                    }
                    foreach (var item in InsertPatientAssign.PatientAssignStatus)
                    {
                        if (item.OptometristcID != 0)
                        {
                            var details = WYNKContext.PatientAssign.OrderByDescending(x => x.CreatedUTC).Where(x => x.RegistrationTranID == Convert.ToInt32(item.RegistrationTranID) && x.Patientallocatestatus == "O").FirstOrDefault();
                            if (details != null)
                            {
                                details.Patientallocatestatus = null;
                                details.DoctorID = item.OptometristcID;
                                details.RegistrationTranID = Convert.ToInt32(item.RegistrationTranID);
                                details.CreatedBy = userID;
                                details.AssignedDate = DateTime.UtcNow + utctime;
                                details.CreatedUTC = DateTime.UtcNow;
                                WYNKContext.PatientAssign.UpdateRange(details);
                                WYNKContext.SaveChanges();

                                var tran = WYNKContext.Workflowmaster.Where(x => x.Description == "Optometrist").Select(x => x.ID).FirstOrDefault();
                                var CurDate = DateTime.UtcNow;
                                var workflow = WYNKContext.Workflowtran.Where(x => x.Regtranid == details.RegistrationTranID && x.CMPID == cmpid && x.Createdutc.Date == CurDate.Date).ToList();
                                var times1 = CurDate.Hour + ":" + CurDate.Minute + ":" + CurDate.Second;
                                var times2 = utctime;
                                var fulltime = TimeSpan.Parse(times1) + times2;
                                DateTime time = DateTime.Today.Add(fulltime);
                                var curfullh = time.Hour;
                                var curfullm = time.Minute;
                                string displayTimes = curfullh + ":" + curfullm;
                                foreach (var itemm in workflow)
                                {
                                    var wwfl = workflow.Where(x => x.ID == itemm.ID && x.status == true).FirstOrDefault();
                                    if (wwfl != null)
                                    {
                                        wwfl.Checkouttime = displayTimes;
                                        wwfl.Doctor = false;
                                        wwfl.Reception = false;
                                        wwfl.status = false;
                                        wwfl.Date = DateTime.UtcNow.Date;
                                        WYNKContext.Workflowtran.UpdateRange(wwfl);
                                        WYNKContext.SaveChanges();
                                    }
                                }

                                var wflowtran = new Workflowtran();
                                wflowtran.Checkintime = displayTimes;
                                wflowtran.Regtranid = details.RegistrationTranID;
                                wflowtran.Workflowmasterid = WYNKContext.Workflowmaster.Where(x => x.Description == "Optometrist").Select(x => x.ID).FirstOrDefault();
                                wflowtran.Optometrist = true;
                                wflowtran.Doctor = false;
                                wflowtran.Reception = false;
                                wflowtran.status = true;
                                wflowtran.Doctorid = item.OptometristcID;
                                wflowtran.Date = DateTime.UtcNow.Date;
                                wflowtran.CMPID = cmpid;
                                wflowtran.Createdutc = DateTime.UtcNow;
                                wflowtran.Createdby = Convert.ToInt32(userID);
                                WYNKContext.Workflowtran.AddRange(wflowtran);
                                WYNKContext.SaveChanges();

                            }
                            else
                            {
                                PatientAssignn.DoctorID = item.OptometristcID;
                                PatientAssignn.RegistrationTranID = Convert.ToInt32(item.RegistrationTranID);
                                PatientAssignn.CreatedBy = userID;
                                PatientAssignn.AssignedDate = DateTime.UtcNow + utctime;
                                PatientAssignn.CreatedUTC = DateTime.UtcNow;
                                var idd = WYNKContext.PatientAssign.Select(x => x.ID).LastOrDefault();
                                if (idd < 1)
                                {
                                    PatientAssignn.ID = 1;
                                }
                                else
                                {
                                    PatientAssignn.ID = idd + 1;
                                }
                                WYNKContext.PatientAssign.Add(PatientAssignn);
                                WYNKContext.SaveChanges();

                                var tran = WYNKContext.Workflowmaster.Where(x => x.Description == "Optometrist").Select(x => x.ID).FirstOrDefault();
                                var CurDate = DateTime.UtcNow;
                                var workflow = WYNKContext.Workflowtran.Where(x => x.Regtranid == details.RegistrationTranID && x.CMPID == cmpid && x.Createdutc.Date == CurDate.Date).ToList();
                                var times1 = CurDate.Hour + ":" + CurDate.Minute + ":" + CurDate.Second;
                                var times2 = utctime;
                                var fulltime = TimeSpan.Parse(times1) + times2;
                                DateTime time = DateTime.Today.Add(fulltime);
                                var curfullh = time.Hour;
                                var curfullm = time.Minute;
                                string displayTimes = curfullh + ":" + curfullm;
                                foreach (var itemm in workflow)
                                {
                                    var wwfl = workflow.Where(x => x.ID == itemm.ID && x.status == true).FirstOrDefault();
                                    if (wwfl != null)
                                    {
                                        wwfl.Checkouttime = displayTimes;
                                        wwfl.status = false;
                                        wwfl.Doctor = false;

                                        wwfl.Reception = false;
                                        wwfl.Date = DateTime.UtcNow.Date;
                                        WYNKContext.Workflowtran.UpdateRange(wwfl);
                                        WYNKContext.SaveChanges();
                                    }
                                }

                                var wflowtran = new Workflowtran();

                                wflowtran.Checkintime = displayTimes;
                                wflowtran.Regtranid = details.RegistrationTranID;
                                wflowtran.Workflowmasterid = WYNKContext.Workflowmaster.Where(x => x.Description == "Optometrist").Select(x => x.ID).FirstOrDefault();
                                wflowtran.Optometrist = true;
                                wflowtran.Doctor = false;
                                wflowtran.Reception = false;
                                wflowtran.status = true;
                                wflowtran.Doctorid = item.OptometristcID;
                                wflowtran.CMPID = cmpid;
                                wflowtran.Date = DateTime.UtcNow.Date;
                                wflowtran.Createdutc = DateTime.UtcNow;
                                wflowtran.Createdby = Convert.ToInt32(userID);
                                WYNKContext.Workflowtran.AddRange(wflowtran);
                                WYNKContext.SaveChanges();
                            }


                        }
                    }
                    foreach (var item in InsertPatientAssign.PatientAssignStatus)
                    {
                        if (item.VisionID != 0)
                        {
                            PatientAssignn.DoctorID = item.VisionID;
                            PatientAssignn.RegistrationTranID = Convert.ToInt32(item.RegistrationTranID);
                            PatientAssignn.CreatedBy = userID;
                            PatientAssignn.AssignedDate = DateTime.UtcNow + utctime;
                            PatientAssignn.CreatedUTC = DateTime.UtcNow;
                            var idd = WYNKContext.PatientAssign.Select(x => x.ID).LastOrDefault();
                            if (idd < 1)
                            {
                                PatientAssignn.ID = 1;
                            }
                            else
                            {
                                PatientAssignn.ID = idd + 1;
                            }
                            WYNKContext.PatientAssign.Add(PatientAssignn);
                            WYNKContext.SaveChanges();
                        }


                    }

                    dbContextTransaction.Commit();
                    if (WYNKContext.SaveChanges() >= 0)
                        return new
                        {
                            Success = true,
                            Message = "Saved successfully"
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
                    Message = "Some Data Missing "
                };
            }

        }
        public dynamic CancelPatientAssign(RegistrationMasterViewModel CancelPatientAssign)
        {
            var patientvisttype = CancelPatientAssign.PatientAssignStatus.Select(x => x.Visittype).FirstOrDefault();
            var CompanyID = CancelPatientAssign.CompanyID;
            var Regtran = new RegistrationTran_Master();
            var PatientAssignn = new Patient_Assign();
            var RRg = new RegistrationTran_Master();
            var sst = CancelPatientAssign.Stringdata;
            var Userr = CMPSContext.Users.Where(x => x.Userid == Convert.ToInt32(CancelPatientAssign.UserRole)).Select(x => x.ReferenceTag).FirstOrDefault();
            var DID = CMPSContext.Users.Where(x => x.Userid == Convert.ToInt32(CancelPatientAssign.UserRole)).Select(x => x.Userid).FirstOrDefault();
            if (patientvisttype == "New")
            {
                var Ststus = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "New").Select(x => x.OLMID).FirstOrDefault();
                Regtran.TypeofVisit = Ststus;
                Regtran.PatientVisitType = Convert.ToString(CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "New").Select(x => x.OLMID).FirstOrDefault()); ;
            }
            else if (patientvisttype == "Review")
            {
                var Ststus = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Review").Select(x => x.OLMID).FirstOrDefault();
                RRg.TypeofVisit = Ststus;
                RRg.PatientVisitType = Convert.ToString(CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Review").Select(x => x.OLMID).FirstOrDefault()); ;
            }

            if (Userr == "A" || Userr == "R")
            {

                foreach (var item in CancelPatientAssign.PatientAssignStatus)
                {
                    Regtran = WYNKContext.RegistrationTran.Where(x => x.RegistrationTranID == Convert.ToInt32(item.RegistrationTranID)).FirstOrDefault();
                    var UUin = WYNKContext.RegistrationTran.Where(x => x.RegistrationTranID == Convert.ToInt32(item.RegistrationTranID)).Select(x => x.UIN).FirstOrDefault();

                    Regtran.Status = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Cancelled").Select(x => x.OLMID).FirstOrDefault();
                    Regtran.DoctorID = Convert.ToInt32(CancelPatientAssign.UserRole);
                    Regtran.AssignedDate = DateTime.UtcNow;
                    Regtran.CancellationReasons = CancelPatientAssign.Cancellatioreasons;
                    Regtran.IsCancelled = true;
                    Regtran.Cancelleddate = DateTime.UtcNow;
                    Regtran.TypeofVisit = 0;
                    sst = "Yes";
                    WYNKContext.Entry(Regtran).State = EntityState.Modified;
                    WYNKContext.SaveChanges();
                }
            }
            else
            {
                foreach (var item in CancelPatientAssign.PatientAssignStatus)
                {
                    PatientAssignn = WYNKContext.PatientAssign.Where(x => x.RegistrationTranID == Convert.ToInt32(item.RegistrationTranID)).FirstOrDefault();
                    PatientAssignn.DoctorID = DID;
                    PatientAssignn.IsCancelled = true;
                    PatientAssignn.CancellationReasons = CancelPatientAssign.Cancellatioreasons;
                    WYNKContext.Entry(PatientAssignn).State = EntityState.Modified;
                    WYNKContext.SaveChanges();

                    var Cancelledomid = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Cancelled").Select(x => x.OLMID).FirstOrDefault();
                    Regtran = WYNKContext.RegistrationTran.Where(x => x.RegistrationTranID == Convert.ToInt32(item.RegistrationTranID)).FirstOrDefault();
                    Regtran.Status = Cancelledomid;
                    Regtran.DoctorID = DID;
                    Regtran.Cancelleddate = DateTime.UtcNow;
                    Regtran.AssignedDate = DateTime.UtcNow;
                    Regtran.CancellationReasons = CancelPatientAssign.Cancellatioreasons;
                    Regtran.IsCancelled = true;
                    sst = "Yes";
                    WYNKContext.Entry(Regtran).State = EntityState.Modified;
                    WYNKContext.SaveChanges();


                    var Dateti = DateTime.UtcNow;
                    var UUin = WYNKContext.RegistrationTran.Where(x => x.RegistrationTranID == Convert.ToInt32(item.RegistrationTranID)).Select(x => x.UIN).FirstOrDefault();
                    var ExactStstus = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Open").Select(x => x.OLMID).FirstOrDefault();
                    RRg.UIN = UUin;
                    RRg.DateofVisit = Dateti;
                    RRg.Status = ExactStstus;
                    RRg.AssignedDate = null;
                    if (patientvisttype == "New")
                    {
                        var Ststus = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "New").Select(x => x.OLMID).FirstOrDefault();
                        RRg.TypeofVisit = Ststus;
                        RRg.PatientVisitType = Convert.ToString(CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "New").Select(x => x.OLMID).FirstOrDefault()); ;
                    }
                    else if (patientvisttype == "Review")
                    {
                        var Ststus = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Review").Select(x => x.OLMID).FirstOrDefault();
                        RRg.TypeofVisit = Ststus;
                        RRg.PatientVisitType = Convert.ToString(CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Review").Select(x => x.OLMID).FirstOrDefault()); ;
                    }

                    RRg.DoctorID = DID;
                    RRg.StatusDateTime = null;
                    RRg.Remarks = null;
                    RRg.FollowupDate = null;
                    RRg.Cancelleddate = null;
                    RRg.IsDeleted = false;
                    RRg.CreatedUTC = DateTime.UtcNow;
                    RRg.CreatedBy = Convert.ToInt32(CancelPatientAssign.UserRole);
                    RRg.RegistrationFees = null;
                    RRg.CancellationReasons = null;
                    RRg.IsCancelled = false;
                    WYNKContext.RegistrationTran.Add(RRg);
                    WYNKContext.SaveChanges();


                }

            }



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
                Message = "Some Data Missing "
            };


        }
        public dynamic UploadImage(IFormFile file, string UIN)
        {

            var currentDir = Directory.GetCurrentDirectory();
            if (!Directory.Exists(currentDir + "/PatientImages/"))
                Directory.CreateDirectory(currentDir + "/PatientImages/");
            var fileName = $"{UIN}{Path.GetExtension(file.FileName)}";
            var path = $"{currentDir}/PatientImages/{fileName}";

            if ((File.Exists(path)))
                File.Delete(path);

            using (var stream = new FileStream(path, FileMode.OpenOrCreate))
            {
                file.CopyTo(stream);
                var photo = WYNKContext.Registration.Where(x => x.UIN == UIN).FirstOrDefault();
                photo.PhotoPath = fileName;

                WYNKContext.Entry(photo).State = EntityState.Modified;

                var tt = WYNKContext.SaveChanges() > 0;
                return tt;

            }

        }
        public dynamic UploadpatientVideo(IFormFile file, string UIN)
        {

            var currentDir = Directory.GetCurrentDirectory();
            if (!Directory.Exists(currentDir + "/PatientVideoConsent/"))
                Directory.CreateDirectory(currentDir + "/PatientVideoConsent/");
            var fileName = $"{UIN}{Path.GetExtension(file.FileName)}";
            var path = $"{currentDir}/PatientVideoConsent/{fileName}";

            if ((File.Exists(path)))
                File.Delete(path);

            using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate))
            {
                file.CopyTo(stream);
                var photo = WYNKContext.Registration.Where(x => x.UIN == UIN).FirstOrDefault();
                photo.Videoconsent = true;

                WYNKContext.Entry(photo).State = EntityState.Modified;

                var tt = WYNKContext.SaveChanges() > 0;
                return tt;

            }


        }
        public dynamic Uploadpatientvoice(IFormFile file, string UIN)
        {


            var currentDir = Directory.GetCurrentDirectory();
            if (!Directory.Exists(currentDir + "/PatientVoiceConsent/"))
                Directory.CreateDirectory(currentDir + "/PatientVoiceConsent/");
            var fileName = $"{UIN}{Path.GetExtension(file.FileName)}";
            var path = $"{currentDir}/PatientVoiceConsent/{fileName}";

            if ((File.Exists(path)))
                File.Delete(path);

            using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate))
            {
                file.CopyTo(stream);
                var photo = WYNKContext.Registration.Where(x => x.UIN == UIN).FirstOrDefault();
                photo.VoiceConsent = true;

                WYNKContext.Entry(photo).State = EntityState.Modified;

                var tt = WYNKContext.SaveChanges() > 0;
                return tt;

            }


        }

        public dynamic Getpatientvoice(string UIN)
        {
            var reg = new RegistrationMasterViewModel();
            reg.PatientAssignStatus = new List<PatientAssignStatus>();
            var regs = WYNKContext.Registration.Where(x => x.UIN == UIN).Select(x => x.VoiceConsent).FirstOrDefault();

            if (regs == true)
            {

                var currentDir = Directory.GetCurrentDirectory();
                string path = currentDir + "/PatientVoiceConsent/" + UIN;
                if ((File.Exists(path)))
                {
                    string imageData = Convert.ToBase64String(File.ReadAllBytes(path));
                    reg.ProductImage = imageData;
                }

            }

            return reg;
        }

        public dynamic Getpatientvideo(string UIN)
        {
            var reg = new RegistrationMasterViewModel();
            reg.PatientAssignStatus = new List<PatientAssignStatus>();
            var regs = WYNKContext.Registration.Where(x => x.UIN == UIN).Select(x => x.Videoconsent).FirstOrDefault();

            if (regs == true)
            {

                var currentDir = Directory.GetCurrentDirectory();
                string path = currentDir + "/PatientVideoConsent/" + UIN;
                if ((File.Exists(path)))
                {
                    string imageData = Convert.ToBase64String(File.ReadAllBytes(path));
                    reg.ProductImage = imageData;
                }

            }

            return reg;
        }

        public dynamic Getpatientimage(string UIN)
        {


            var reg = new RegistrationMasterViewModel();

            reg.PatientAssignStatus = new List<PatientAssignStatus>();
            var regs = WYNKContext.Registration.Where(x => x.UIN == UIN).Select(x => x.PhotoPath).FirstOrDefault();
            if (regs != null)
            {
                var osfn = regs;
                var osfi = "/PatientImages/";
                var currentDir = Directory.GetCurrentDirectory();
                string path = currentDir + osfi + osfn;
                if ((File.Exists(path)))
                {
                    string imageData = Convert.ToBase64String(File.ReadAllBytes(path));
                    string source = imageData;
                    string base64 = source.Substring(source.IndexOf(',') + 1);
                    byte[] data = Convert.FromBase64String(base64);
                    reg.ProductImage = imageData;
                }
                else
                {

                }
            }
            else
            {
            }
            return reg;
        }
        public dynamic GetThumbimage(string UIN, int CMPID)
        {
            var reg = new RegistrationMasterViewModel();
            reg.NewSummaryImages = new List<NewSummaryImages>();
            reg.NewSummaryImagesgorup = new List<NewSummaryImagesgorup>();

            reg.NewSummaryPDF = new List<NewSummaryPDF>();
            var currentDir = Directory.GetCurrentDirectory();
            var datetime = DateTime.UtcNow.Date.ToString("dd-MMM-yyyy");
            var path = $"{currentDir}/Dashboard/{CMPID}/MedicalPrescription/{UIN}/";
            var path1 = $"{currentDir}/Dashboard/{CMPID}/casesheet/{UIN}/";
            var path2 = $"{currentDir}/Dashboard/{CMPID}/OpticalPrescription/{UIN}/";
            var path3 = $"{currentDir}/Dashboard/{CMPID}/Others/{UIN}/";
            String searchFolder = path;
            String searchFolder1 = path1;
            String searchFolder2 = path2;
            String searchFolder3 = path3;

            var filters = new String[] { "jpg", "jpeg", "png", "gif", "tiff", "bmp", "svg" };
            var pdffilters = new String[] { "pdf" };

            var imagefiles = GetFilesFrom(searchFolder, filters, false);
            var pdffiles = GetFilesFrom(searchFolder, pdffilters, false);
            var imagefiles1 = GetFilesFrom(searchFolder1, filters, false);
            var pdffiles1 = GetFilesFrom(searchFolder1, pdffilters, false);
            var imagefiles2 = GetFilesFrom(searchFolder2, filters, false);
            var pdffiles2 = GetFilesFrom(searchFolder2, pdffilters, false);
            var imagefiles3 = GetFilesFrom(searchFolder3, filters, false);
            var pdffiles3 = GetFilesFrom(searchFolder3, pdffilters, false);

            if (imagefiles != null)
            {
                foreach (var item in imagefiles)
                {
                    FileInfo fi = new FileInfo(item);
                    var filename = fi.Name;
                    var list = new NewSummaryImages();
                    string imageData = Convert.ToBase64String(File.ReadAllBytes(item));
                    list.Imagedesc = "data:image/png;base64," + imageData;
                    list.ImageName = filename;
                    list.Imagecategory = "Medical Prescription";
                    reg.NewSummaryImages.Add(list);

                }
            }
            if (pdffiles != null)
            {
                foreach (var item in pdffiles)
                {
                    FileInfo fi = new FileInfo(item);
                    var filename = fi.Name;
                    var list = new NewSummaryPDF();
                    string imageData = Convert.ToBase64String(File.ReadAllBytes(item));
                    list.Imagedesc = "data:application/pdf;base64," + imageData;
                    list.ImageName = filename;
                    list.Imagecategory = "Medical Prescription";
                    reg.NewSummaryPDF.Add(list);

                }
            }



            if (imagefiles1 != null)
            {
                foreach (var item in imagefiles1)
                {
                    FileInfo fi = new FileInfo(item);
                    var filename = fi.Name;
                    var list = new NewSummaryImages();
                    string imageData = Convert.ToBase64String(File.ReadAllBytes(item));
                    list.Imagedesc = "data:image/png;base64," + imageData;
                    list.ImageName = filename;
                    list.Imagecategory = "Case Sheet";
                    reg.NewSummaryImages.Add(list);

                }
            }
            if (pdffiles1 != null)
            {
                foreach (var item in pdffiles1)
                {
                    FileInfo fi = new FileInfo(item);
                    var filename = fi.Name;
                    var list = new NewSummaryPDF();
                    string imageData = Convert.ToBase64String(File.ReadAllBytes(item));
                    list.Imagedesc = "data:application/pdf;base64," + imageData;
                    list.ImageName = filename;
                    list.Imagecategory = "Case Sheet";
                    reg.NewSummaryPDF.Add(list);

                }
            }



            if (imagefiles2 != null)
            {
                foreach (var item in imagefiles2)
                {
                    FileInfo fi = new FileInfo(item);
                    var filename = fi.Name;
                    var list = new NewSummaryImages();
                    string imageData = Convert.ToBase64String(File.ReadAllBytes(item));
                    list.Imagedesc = "data:image/png;base64," + imageData;
                    list.ImageName = filename;
                    list.Imagecategory = "Optical Prescription";
                    reg.NewSummaryImages.Add(list);

                }
            }
            if (pdffiles2 != null)
            {
                foreach (var item in pdffiles2)
                {
                    FileInfo fi = new FileInfo(item);
                    var filename = fi.Name;
                    var list = new NewSummaryPDF();
                    string imageData = Convert.ToBase64String(File.ReadAllBytes(item));
                    list.Imagedesc = "data:application/pdf;base64," + imageData;
                    list.ImageName = filename;
                    list.Imagecategory = "Optical Prescription";
                    reg.NewSummaryPDF.Add(list);

                }
            }


            if (imagefiles3 != null)
            {
                foreach (var item in imagefiles3)
                {
                    FileInfo fi = new FileInfo(item);
                    var filename = fi.Name;
                    var list = new NewSummaryImages();
                    string imageData = Convert.ToBase64String(File.ReadAllBytes(item));
                    list.Imagedesc = "data:image/png;base64," + imageData;
                    list.ImageName = filename;
                    list.Imagecategory = "Others";
                    reg.NewSummaryImages.Add(list);

                }
            }
            if (pdffiles3 != null)
            {
                foreach (var item in pdffiles3)
                {
                    FileInfo fi = new FileInfo(item);
                    var filename = fi.Name;
                    var list = new NewSummaryPDF();
                    string imageData = Convert.ToBase64String(File.ReadAllBytes(item));
                    list.Imagedesc = "data:application/pdf;base64," + imageData;
                    list.ImageName = filename;
                    list.Imagecategory = "Others";
                    reg.NewSummaryPDF.Add(list);

                }
            }

            reg.NewSummaryImagesgorup = (from cc in reg.NewSummaryImages.GroupBy(x => x.Imagecategory)
                                         select new NewSummaryImagesgorup
                                         {
                                             Imagecategory = cc.Key,
                                             Imagedesc = cc.Select(x => x.Imagedesc).ToList(),
                                             ImageName = cc.Select(x => x.ImageName).ToList(),
                                         }).ToList();




            return reg;
        }


        public static String[] GetFilesFrom(String searchFolder, String[] filters, bool isRecursive)
        {
            List<String> filesFound = new List<String>();
            var searchOption = isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            foreach (var filter in filters)
            {
                try
                {
                    filesFound.AddRange(Directory.GetFiles(searchFolder, String.Format("*.{0}", filter), searchOption));
                }
                catch (Exception ex)
                {

                }
            }
            return filesFound.ToArray();
        }



        public dynamic GetReviewThumbimage(string UIN, int CMPID)
        {
            var reg = new RegistrationMasterViewModel();

            return reg;
        }

        public dynamic GetSurgeryreviewThumbimage(string UIN, int CMPID)
        {
            var reg = new RegistrationMasterViewModel();

            return reg;
        }

        public dynamic Insertnewpass(RegistrationMasterViewModel Insertnewpasss, int UIN)
        {
            var user = new LoginMasterViewModel();
            var usersss = CMPSContext.Users.Where(x => x.Userid == UIN).Select(x => x.Username).FirstOrDefault();
            var usersmenu = CMPSContext.Users.Where(x => x.Username == usersss).ToList();
            var oldpassword = PasswordEncodeandDecode.EncodePasswordToBase64(Insertnewpasss.oldpassword);
            foreach (var item in usersmenu)
            {
                if (oldpassword == item.Password)
                {
                    var userss = CMPSContext.Users.Where(x => x.Userid == item.Userid).FirstOrDefault();
                    userss.Updatedutc = DateTime.UtcNow;
                    userss.Updatedby = null;
                    userss.Password = PasswordEncodeandDecode.EncodePasswordToBase64(Insertnewpasss.confirmpasswword);
                    CMPSContext.Entry(userss).State = EntityState.Modified;
                }
                else
                {
                    user.status = "Password Incorrect";
                    break;
                }
            }

            try
            {
                if (CMPSContext.SaveChanges() > 0)
                    return new
                    {
                        Success = true,
                        Message = "Saved successfully",

                    };

            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Success = false,
                Message = "some data are missing"
            };

        }
        public RegistrationMasterViewModel Resetpassword(string emailid)
        {
            var Resetpassword = new RegistrationMasterViewModel();

            var checkpassword = CMPSContext.Users.Where(x => x.Username == emailid).FirstOrDefault();
            var cmpid = checkpassword.CMPID;
            var cmpname = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.CompanyName).FirstOrDefault();

            if (checkpassword != null)
            {
                var password = CMPSContext.Users.Where(x => x.Username == emailid).Select(x => x.Password).FirstOrDefault();
                var decodepassword = PasswordEncodeandDecode.DecodeFrom64(password);
                EmailService.EmailSend(emailid, emailid, cmpname, password);
                Resetpassword.ResetMessage = CommonMessage.ResetMessage2;
            }
            else
            {
                Resetpassword.ResetMessage = CommonMessage.ResetMessage1;
            }

            return Resetpassword;
        }
        public RegistrationMasterViewModel GeMenuaccess(int Roletableid, int userroleID, string id)
        {
            var Menuaccessdata = new RegistrationMasterViewModel();
            if (id == "ManagementDashboard")
            {
                var MangementModuleaccess = CMPSContext.ModuleMaster.Where(x => x.ModuleDescription == "Management Dashboard").Select(x => x.ModuleID).FirstOrDefault();
                var Userroleid = CMPSContext.UserVsRole.Where(x => x.UserID == userroleID).Select(x => x.UserRoleID).FirstOrDefault();
                Menuaccessdata.ALLMenuaccessdata = CMPSContext.RoleVsModule.Where(x => x.RoleID == Roletableid && x.UserRoleID == Userroleid && x.ModuleID == MangementModuleaccess).Select(x => x.ModuleID).FirstOrDefault();

            }
            else
            {

                var Moduleaccess = CMPSContext.ModuleMaster.Where(x => x.ModuleDescription == "Access").Select(x => x.ModuleID).FirstOrDefault();
                var Userroleid = CMPSContext.UserVsRole.Where(x => x.UserID == userroleID).Select(x => x.UserRoleID).FirstOrDefault();
                Menuaccessdata.ALLMenuaccessdata = CMPSContext.RoleVsModule.Where(x => x.RoleID == Roletableid).Select(x => x.ModuleID).FirstOrDefault();

            }
            return Menuaccessdata;
        }

        public dynamic SendOTP(int OTP, string Mobilnumber)
        {

            string userid = "cmpsadmin";
            string apikey = "UMrCTzVADqibrFY4PAto";
            object mobile = Mobilnumber;
            string msgtext = "Welcome to Eye Hospital, Your OTP is : " + OTP;
            var client = new WebClient();
            var baseurl = "http://smshorizon.co.in/api/sendsms.php?user=" + userid + "&apikey=" + apikey + "&number=" + mobile + "&message=" + msgtext + "&senderid=CMPSIN&type=txt";
            var data = client.OpenRead(baseurl);
            var reader = new StreamReader(data);
            var response = reader.ReadToEnd();
            string smssucces = response;
            data.Close();
            reader.Close();
            object messagebox = "msg saved";
            object text = "tt";

            try
            {

                return new
                {
                    Success = true,
                    Message = "Saved successfully",

                };

            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Success = false,
                Message = "some data are missing"
            };
        }


        public dynamic GetMyPatientDetails(int CmpID)
        {
            var utctimes = CMPSContext.Setup.Where(x => x.CMPID == CmpID).Select(x => x.UTCTime).FirstOrDefault();

            TimeSpan utctime = TimeSpan.Parse(utctimes);
            var RR = new RegistrationMasterViewModel();
            var Getregistration = WYNKContext.Registration.ToList();
            var Getregistrationtran = WYNKContext.RegistrationTran.ToList();
            var GetPatientassion = WYNKContext.PatientAssign.AsNoTracking().ToList();
            var Getonelinemaster = CMPSContext.OneLineMaster.ToList();
            var Getdoctormaster = CMPSContext.DoctorMaster.ToList();
            var Doctormas = CMPSContext.DoctorMaster.ToList();
            var New = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "New").Select(x => x.OLMID).FirstOrDefault();
            var Review = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Review").Select(x => x.OLMID).FirstOrDefault();
            var closed = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Closed").Select(x => x.OLMID).FirstOrDefault();
            var Find = WYNKContext.Findings.ToList();
            var Findext = WYNKContext.FindingsExt.ToList();
            var ICDSPecial = WYNKContext.ICDSpecialityCode.ToList();
            var ViewsStatusid = 38;
            var NewOpen = (from RM in Getregistration.Where(x => x.CMPID == CmpID)
                           join RMT in Getregistrationtran on RM.UIN equals RMT.UIN
                           where RMT.PatientVisitType == Convert.ToString(New) && RMT.Status == ViewsStatusid

                           orderby RM.DateofRegistration descending
                           select new Patientlists
                           {
                               UIN = RM.UIN,
                               Name = GetConcatName(RM.Name),
                               MName = GetConcatName(RM.MiddleName),
                               LName = GetConcatName(RM.LastName),
                               DOR = Convert.ToDateTime(RM.DateofRegistration + utctime),
                               Age = PasswordEncodeandDecode.ToAgeString(RM.DateofBirth),
                               Sex = RM.Gender,
                               Status = Getonelinemaster.Where(x => x.OLMID == RMT.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                               RegistrationTranID = RMT.RegistrationTranID,
                               AdminName = "New",
                               AssDoctorID = ToLastName(ViewsStatusid, RMT.DoctorID),
                               AssignedDate = getdate(Convert.ToDateTime(RMT.AssignedDate)),

                           }).ToList();

            var ReviewOpen = (from RM in Getregistration.Where(x => x.CMPID == CmpID)
                              join RMT in Getregistrationtran on RM.UIN equals RMT.UIN
                              where RMT.PatientVisitType == Convert.ToString(Review) && RMT.Status == ViewsStatusid

                              orderby RM.DateofRegistration descending
                              select new Patientlists
                              {
                                  UIN = RM.UIN,
                                  Name = GetConcatName(RM.Name),
                                  MName = GetConcatName(RM.MiddleName),
                                  LName = GetConcatName(RM.LastName),
                                  DOR = Convert.ToDateTime(RM.DateofRegistration + utctime),
                                  Age = PasswordEncodeandDecode.ToAgeString(RM.DateofBirth),
                                  Sex = RM.Gender,
                                  Status = Getonelinemaster.Where(x => x.OLMID == RMT.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                                  RegistrationTranID = RMT.RegistrationTranID,
                                  AdminName = "Review",
                                  AssDoctorID = ToLastName(ViewsStatusid, RMT.DoctorID),
                                  AssignedDate = getdate(Convert.ToDateTime(RMT.AssignedDate)),

                              }).ToList();


            var SurgeryOpen = (from Fext in Findext
                               join F in Find.Where(x => x.CmpID == CmpID) on Fext.FindingsID equals F.RandomUniqueID
                               join RegTran in Getregistrationtran on F.UIN equals RegTran.UIN
                               join Reg in Getregistration.Where(x => x.CMPID == CmpID) on F.UIN equals Reg.UIN
                               join ic in ICDSPecial on Fext.ICDSpecialityid equals ic.ID
                               where Fext.Isdeleted == false && ic.IsActive == true
                               && RegTran.PatientVisitType == Convert.ToString(Review) && RegTran.Status == ViewsStatusid
                               orderby Reg.DateofRegistration descending
                               select new Patientlists
                               {
                                   UIN = Reg.UIN,
                                   Name = GetConcatName(Reg.Name),
                                   MName = GetConcatName(Reg.MiddleName),
                                   LName = GetConcatName(Reg.LastName),
                                   DOR = TimeZoneInfo.ConvertTimeFromUtc(RegTran.DateofVisit, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")),
                                   Age = PasswordEncodeandDecode.ToAgeString(Reg.DateofBirth),
                                   Sex = Reg.Gender,
                                   LastName = Doctormas.Where(x => x.DoctorID == RegTran.DoctorID).Select(x => x.LastName).FirstOrDefault(),
                                   Status = Getonelinemaster.Where(x => x.OLMID == RegTran.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                                   RegistrationTranID = RegTran.RegistrationTranID,
                                   AssDoctorID = Getdoctormaster.Where(x => x.DoctorID == RegTran.DoctorID).Select(x => x.LastName).FirstOrDefault(),
                                   AssignedDate = getdate(Convert.ToDateTime(RegTran.AssignedDate)),
                                   ReviewDate = Fext.SureryReviewDate,
                                   AdminName = "SurgeryReview",
                               }).ToList();


            var mrgedcode = NewOpen.Concat(ReviewOpen).Concat(SurgeryOpen);

            RR.MyPatientlistss = new List<MyPatientlists>();

            foreach (var item in mrgedcode.ToList())
            {
                var MM = new MyPatientlists();
                MM.UIN = item.UIN;
                MM.Name = GetConcatName(item.Name);
                MM.MName = GetConcatName(item.MiddleName);
                MM.LName = GetConcatName(item.LName);
                MM.DOR = item.DOR;
                MM.Age = item.Age;
                MM.Sex = item.Sex;
                MM.Status = item.Status;
                MM.RegistrationTranID = item.RegistrationTranID;
                MM.AssDoctorID = item.AssDoctorID;
                MM.AssignedDate = item.AssignedDate;
                MM.ReviewDate = item.ReviewDate;
                MM.AdminName = item.AdminName;
                RR.MyPatientlistss.Add(MM);
            }

            return RR;

        }


        public dynamic getdocname(int? id)
        {
            var doctorname = "0";

            if (id != 0 && id != null)
            {
                var fname = CMPSContext.DoctorMaster.Where(x => x.DoctorID == id).Select(x => x.Firstname).FirstOrDefault();
                var mname = CMPSContext.DoctorMaster.Where(x => x.DoctorID == id).Select(x => x.MiddleName).FirstOrDefault();
                var lname = CMPSContext.DoctorMaster.Where(x => x.DoctorID == id).Select(x => x.LastName).FirstOrDefault();
                doctorname = fname + mname + lname;
            }
            else
            {
                doctorname = " ";
            }


            return doctorname;
        }



        public dynamic GetAppointmentDetails(int CmpID, string Userid)
        {
            var RR = new RegistrationMasterViewModel();
            var utctimes = CMPSContext.Setup.Where(x => x.CMPID == CmpID).Select(x => x.UTCTime).FirstOrDefault();
            TimeSpan utctime = TimeSpan.Parse(utctimes);

            if (Userid == "A" || Userid == "R")
            {
                var Appointment = WYNKContext.Appointment.Where(x => x.CMPID == CmpID).ToList();
                var Testdata = (from a in Appointment.OrderByDescending(x => x.CreatedUTC)
                                join at in WYNKContext.AppointmentTrans.OrderByDescending(x => x.CreatedUTC) on a.RandomUniqueID equals at.AppointmentMasterID

                                select new
                                {
                                    APPID = a.RandomUniqueID,
                                    Name = a.PatientName,
                                    Age = PasswordEncodeandDecode.ToAgeString(Convert.ToDateTime(a.Dateofbirth)),
                                    Gender = a.Gender,
                                    Address = a.Address1 + "  " + a.Address2,
                                    Phone = a.Phone,
                                    Appointmentdate = at.AppointmentDateandTime,
                                    Appointmenttime = at.AppointmentTime,
                                    Appointmentcreateddatedate = at.CreatedUTC,
                                    APpointmentreasons = a.AppointmentReasons,
                                    Appoinytstatus = at.Isstatus,
                                    Cancelleddatetime = at.iscancelleddateandtime,
                                    docid = getdocname(at.Doctorid),
                                    Doctoruserid = at.Doctorid,
                                    bookedby = at.AppointmentBookedby,
                                }).ToList();


                RR.APpointmentdwtailss = (from c in Testdata
                                          group c by new { c.Appointmentdate, c.Name } into i
                                          select new APpointmentdwtails
                                          {
                                              APPID = i.Select(x => x.APPID).FirstOrDefault(),
                                              Name = i.Select(x => x.Name).FirstOrDefault(),
                                              Age = i.Select(x => x.Age).FirstOrDefault(),
                                              Gender = i.Select(x => x.Gender).FirstOrDefault(),
                                              Address = i.Select(x => x.Address).FirstOrDefault(),
                                              Phone = i.Select(x => x.Phone).FirstOrDefault(),
                                              Appointmentdate = i.Select(x => x.Appointmentdate).FirstOrDefault(),
                                              Apptime = i.Select(x => x.Appointmenttime).FirstOrDefault(),
                                              Appointmentcreateddatedate = i.Select(x => x.Appointmentcreateddatedate).FirstOrDefault(),
                                              APpointmentreasons = i.Select(x => x.APpointmentreasons).FirstOrDefault(),
                                              Appoinytstatus = i.Select(x => x.Appoinytstatus).FirstOrDefault(),
                                              Cancelleddatetime = i.Select(x => x.Cancelleddatetime).FirstOrDefault(),
                                              Doctorid = i.Select(x => x.docid).FirstOrDefault(),
                                              Appointmentbookedby = i.Select(x => x.bookedby).FirstOrDefault(),
                                              Doctoruserid = i.Select(x => x.Doctoruserid).FirstOrDefault(),
                                          }).ToList();


                RR.APpointmentdwtailscalender = (from c in Testdata
                                                 group c by new { c.Appointmentdate, c.Name } into i
                                                 select new APpointmentdwtailscalender
                                                 {
                                                     text = i.Select(x => x.Name).FirstOrDefault(),
                                                     startDate = i.Select(x => x.Appointmentdate).FirstOrDefault(),
                                                     endDate = ((DateTime)i.Select(x => x.Appointmentdate).FirstOrDefault()).AddMinutes(30),
                                                 }).ToList();

            }
            else
            {
                var Appointment = WYNKContext.Appointment.ToList();
                var Testdata = (from a in Appointment.OrderByDescending(x => x.CreatedUTC)
                                join at in WYNKContext.AppointmentTrans.OrderByDescending(x => x.CreatedUTC).Where(x => x.CMPID == CmpID && x.Doctorid == Convert.ToInt32(Userid)) on a.RandomUniqueID equals at.AppointmentMasterID
                                select new
                                {
                                    APPID = a.RandomUniqueID,
                                    Name = a.PatientName,
                                    Age = PasswordEncodeandDecode.ToAgeString(Convert.ToDateTime(a.Dateofbirth)),
                                    Gender = a.Gender,
                                    Address = a.Address1 + "  " + a.Address2,
                                    Phone = a.Phone,
                                    Appointmentdate = at.AppointmentDateandTime,
                                    Appointmentcreateddatedate = at.CreatedUTC,
                                    APpointmentreasons = a.AppointmentReasons,
                                    Appoinytstatus = at.Isstatus,
                                    Cancelleddatetime = at.iscancelleddateandtime,
                                    docid = getdocname(at.Doctorid),
                                    Doctoruserid = at.Doctorid,
                                    bookedby = at.AppointmentBookedby,
                                    Appointmenttime = at.AppointmentTime,
                                }).ToList();


                RR.APpointmentdwtailss = (from c in Testdata
                                          group c by new { c.Appointmentdate, c.Name } into i
                                          select new APpointmentdwtails
                                          {
                                              APPID = i.Select(x => x.APPID).FirstOrDefault(),
                                              Name = i.Select(x => x.Name).FirstOrDefault(),
                                              Age = i.Select(x => x.Age).FirstOrDefault(),
                                              Gender = i.Select(x => x.Gender).FirstOrDefault(),
                                              Address = i.Select(x => x.Address).FirstOrDefault(),
                                              Phone = i.Select(x => x.Phone).FirstOrDefault(),
                                              Apptime = i.Select(x => x.Appointmenttime).FirstOrDefault(),
                                              Appointmentdate = i.Select(x => x.Appointmentdate).FirstOrDefault(),
                                              Appointmentcreateddatedate = i.Select(x => x.Appointmentcreateddatedate).FirstOrDefault(),
                                              APpointmentreasons = i.Select(x => x.APpointmentreasons).FirstOrDefault(),
                                              Appoinytstatus = i.Select(x => x.Appoinytstatus).FirstOrDefault(),
                                              Cancelleddatetime = i.Select(x => x.Cancelleddatetime).FirstOrDefault(),
                                              Doctorid = i.Select(x => x.docid).FirstOrDefault(),
                                              Appointmentbookedby = i.Select(x => x.bookedby).FirstOrDefault(),
                                              Doctoruserid = i.Select(x => x.Doctoruserid).FirstOrDefault(),
                                          }).ToList();



            }

            return RR;
        }


        public dynamic GetAppointmentstatusPatientDetails(string CmpID, string Status, string RegNO)
        {
            var utctimes = CMPSContext.Setup.Where(x => x.CMPID == Convert.ToInt32(CmpID)).Select(x => x.UTCTime).FirstOrDefault();

            TimeSpan utctime = TimeSpan.Parse(utctimes);
            var RR = new RegistrationMasterViewModel();
            var Appointment = WYNKContext.Appointment.Where(x => x.CMPID == Convert.ToInt32(CmpID)).ToList();


            if (RegNO == "0")
            {
                Appointment = WYNKContext.Appointment.Where(x => x.CMPID == Convert.ToInt32(CmpID)).ToList();

            }
            else
            {
                Appointment = WYNKContext.Appointment.Where(x => x.CMPID == Convert.ToInt32(CmpID) && x.AppointmentdoctorID == Convert.ToInt32(RegNO)).ToList();
            }

            if (Status != "All")
            {

                RR.APpointmentdwtailss = (from a in Appointment.OrderByDescending(x => x.CreatedUTC)
                                          join at in WYNKContext.AppointmentTrans.OrderByDescending(x => x.CreatedUTC).Where(x => x.Isstatus == Status)
                                          on a.RandomUniqueID equals at.AppointmentMasterID
                                          select new APpointmentdwtails
                                          {
                                              APPID = a.RandomUniqueID,
                                              Name = a.PatientName,
                                              Age = PasswordEncodeandDecode.ToAgeString(Convert.ToDateTime(a.Dateofbirth)),
                                              Gender = a.Gender,
                                              Address = a.Address1 + "  " + a.Address2,
                                              Phone = a.Phone,
                                              Appointmentdate = at.AppointmentDateandTime,
                                              Apptime = at.AppointmentTime,
                                              Appointmentcreateddatedate = at.CreatedUTC,
                                              APpointmentreasons = a.AppointmentReasons,
                                              Appoinytstatus = at.Isstatus,
                                              Cancelleddatetime = at.iscancelleddateandtime,
                                              Doctorid = getdocname(at.Doctorid),
                                              Appointmentbookedby = at.AppointmentBookedby,
                                          }).ToList();
            }
            else
            {

                RR.APpointmentdwtailss = (from a in Appointment.OrderByDescending(x => x.CreatedUTC)
                                          join at in WYNKContext.AppointmentTrans.OrderByDescending(x => x.CreatedUTC) on a.RandomUniqueID equals at.AppointmentMasterID
                                          select new APpointmentdwtails
                                          {
                                              APPID = a.RandomUniqueID,
                                              Name = a.PatientName,
                                              Age = PasswordEncodeandDecode.ToAgeString(Convert.ToDateTime(a.Dateofbirth)),
                                              Gender = a.Gender,
                                              Address = a.Address1 + "   " + a.Address2,
                                              Phone = a.Phone,
                                              Appointmentdate = at.AppointmentDateandTime,
                                              Apptime = at.AppointmentTime,
                                              Appointmentcreateddatedate = at.CreatedUTC,
                                              APpointmentreasons = a.AppointmentReasons,
                                              Appoinytstatus = at.Isstatus,
                                              Cancelleddatetime = at.iscancelleddateandtime,
                                              Doctorid = getdocname(at.Doctorid),
                                              Appointmentbookedby = at.AppointmentBookedby,
                                          }).ToList();
            }





            return RR;
        }

        public dynamic GetAppointmentPatientDetails(int CmpID, string ID)
        {
            var utctimes = CMPSContext.Setup.Where(x => x.CMPID == CmpID).Select(x => x.UTCTime).FirstOrDefault();

            TimeSpan utctime = TimeSpan.Parse(utctimes);
            var Appointment = WYNKContext.Appointment.Where(x => x.CMPID == CmpID && x.ID == Convert.ToInt32(ID)).ToList();
            var reg = new RegistrationMasterViewModel();
            var regs = WYNKContext.Appointment.Where(x => x.CMPID == CmpID && x.ID == Convert.ToInt32(ID)).Select(x => x.Phone).FirstOrDefault();
            reg.AppointmentPtaientname = WYNKContext.Appointment.Where(x => x.CMPID == CmpID && x.ID == Convert.ToInt32(ID)).Select(x => x.PatientName).FirstOrDefault();
            reg.Appointmentgender = WYNKContext.Appointment.Where(x => x.CMPID == CmpID && x.ID == Convert.ToInt32(ID)).Select(x => x.Gender).FirstOrDefault();
            var agggee = WYNKContext.Appointment.Where(x => x.CMPID == CmpID && x.ID == Convert.ToInt32(ID)).Select(x => x.Dateofbirth).FirstOrDefault();
            reg.Appointmentage = PasswordEncodeandDecode.ToAgeString(agggee);
            var ADd1 = WYNKContext.Appointment.Where(x => x.CMPID == CmpID && x.ID == Convert.ToInt32(ID)).Select(x => x.Address1).FirstOrDefault();
            var add2 = WYNKContext.Appointment.Where(x => x.CMPID == CmpID && x.ID == Convert.ToInt32(ID)).Select(x => x.Address2).FirstOrDefault();
            reg.Appointmentaddress = ADd1 + ",  " + add2;
            reg.Appointmentphone = WYNKContext.Appointment.Where(x => x.CMPID == CmpID && x.RandomUniqueID == ID).Select(x => x.Phone).FirstOrDefault();
            reg.AppointmentDate = WYNKContext.AppointmentTrans.Where(x => x.CMPID == CmpID && x.AppointmentMasterID == ID).Select(x => x.AppointmentDateandTime).FirstOrDefault();
            reg.AppointmentReasons = WYNKContext.Appointment.Where(x => x.CMPID == CmpID && x.ID == Convert.ToInt32(ID)).Select(x => x.AppointmentReasons).FirstOrDefault();
            var APttime = WYNKContext.AppointmentTrans.Where(x => x.CMPID == CmpID && x.AppointmentMasterID == ID && x.Isstatus == "Open").Select(x => x.AppointmentTime).FirstOrDefault();
            var Pfd = WYNKContext.AppointmentTrans.Where(x => x.CMPID == CmpID && x.AppointmentMasterID == ID).Select(x => x.Doctorid).FirstOrDefault();
            if (Pfd != 0)
            {
                var fname = GetConcatName(CMPSContext.DoctorMaster.Where(x => x.DoctorID == Pfd).Select(x => x.Firstname).FirstOrDefault());
                var mname = GetConcatName(CMPSContext.DoctorMaster.Where(x => x.DoctorID == Pfd).Select(x => x.MiddleName).FirstOrDefault());
                var lname = GetConcatName(CMPSContext.DoctorMaster.Where(x => x.DoctorID == Pfd).Select(x => x.LastName).FirstOrDefault());
                reg.PreferredDoctor = fname + ' ' + mname + ' ' + lname;
            }
            else
            {
                reg.PreferredDoctor = "Not Assigned to any Doctor";
            }

            var appointmentbokedby = WYNKContext.AppointmentTrans.Where(x => x.CMPID == CmpID && x.AppointmentMasterID == ID).Select(x => x.AppointmentBookedby).FirstOrDefault();

            if (appointmentbokedby != null)
            {
                reg.Appointmentbookedby = appointmentbokedby;
            }
            else
            {
                reg.Appointmentbookedby = "No Data";
            }

            var Appointmentcreatedby = WYNKContext.AppointmentTrans.Where(x => x.CMPID == CmpID && x.AppointmentMasterID == ID).Select(x => x.CreatedBy).FirstOrDefault();
            var Createdid = CMPSContext.Users.Where(x => x.Userid == Appointmentcreatedby).Select(x => x.Username).FirstOrDefault();
            var Createdtag = CMPSContext.Users.Where(x => x.Userid == Appointmentcreatedby).Select(x => x.ReferenceTag).FirstOrDefault();

            if (Createdtag == "A")
            {
                reg.Appointmentcreatedby = "Admin" + "- " + "(" + Createdid + ")";
            }
            else if (Createdtag == "D" || Createdtag == "O" || Createdtag == "V")
            {
                var fname = GetConcatName(CMPSContext.DoctorMaster.Where(x => x.EmailID == Createdid).Select(x => x.Firstname).FirstOrDefault());
                var mname = GetConcatName(CMPSContext.DoctorMaster.Where(x => x.EmailID == Createdid).Select(x => x.MiddleName).FirstOrDefault());
                var lname = GetConcatName(CMPSContext.DoctorMaster.Where(x => x.EmailID == Createdid).Select(x => x.LastName).FirstOrDefault());
                reg.Appointmentcreatedby = fname + ' ' + mname + ' ' + lname;
            }
            else
            {
                var fjoinname = CMPSContext.EmployeeCommunication.Where(x => x.EmailID == Createdid).Select(x => x.EmpID).FirstOrDefault();
                var fname = GetConcatName(CMPSContext.Employee.Where(x => x.EmployeeID == fjoinname).Select(x => x.FirstName).FirstOrDefault());
                var mname = GetConcatName(CMPSContext.Employee.Where(x => x.EmployeeID == fjoinname).Select(x => x.MiddleName).FirstOrDefault());
                var lname = GetConcatName(CMPSContext.Employee.Where(x => x.EmployeeID == fjoinname).Select(x => x.LastName).FirstOrDefault());
                reg.Appointmentcreatedby = fname + ' ' + mname + ' ' + lname;
            }

            if (APttime != null)
            {
                reg.Appointmenttime = APttime;
            }
            else
            {
                reg.Appointmenttime = "00:00";
            }

            if (regs != null)
            {
                var osfn = regs + ".png";
                var osfi = "/Appointment/";
                var currentDir = Directory.GetCurrentDirectory();
                string path = currentDir + osfi + osfn;
                if ((File.Exists(path)))
                {
                    string imageData = Convert.ToBase64String(File.ReadAllBytes(path));
                    string source = imageData;
                    string base64 = source.Substring(source.IndexOf(',') + 1);
                    byte[] data = Convert.FromBase64String(base64);
                    reg.ProductImage = imageData;
                }
                else
                {

                }


            }
            else
            {
            }
            return reg;

        }




        public dynamic InsertAppointmentmembvers(string CMPID, RegistrationMasterViewModel APpointdata)
        {
            var model = new RegistrationMasterViewModel();

            try
            {
                var idd = WYNKContext.AppointmentTrans.OrderByDescending(x => x.CreatedUTC).Where(x => x.AppointmentMasterID == APpointdata.Appointmentmemberd).FirstOrDefault();

                if (APpointdata.AppointmentStatus == "Cancelled")
                {
                    idd.Isstatus = APpointdata.AppointmentStatus;
                    idd.iscancelledreason = APpointdata.AppointmentCancellationReasons;
                    idd.iscancelleddateandtime = DateTime.UtcNow;
                    idd.UpdatedBy = Convert.ToInt32(APpointdata.Appointmentuserid);
                    idd.UpdatedUTC = DateTime.UtcNow;
                    WYNKContext.Entry(idd).State = EntityState.Modified;


                    var phonenumber = WYNKContext.Appointment.Where(x => x.RandomUniqueID == idd.AppointmentMasterID).Select(x => x.Phone).FirstOrDefault();

                    var docid = WYNKContext.AppointmentTrans.OrderByDescending(x => x.CreatedUTC).Where(x => x.AppointmentMasterID == APpointdata.Appointmentmemberd).Select(x => x.Doctorid).FirstOrDefault();

                    var doctornumber = CMPSContext.DoctorMaster.Where(x => x.DoctorID == Convert.ToInt32(docid)).Select(x => x.Phone1).FirstOrDefault();

                    var Statusid = WYNKContext.Configuration.OrderByDescending(x => x.CreatedUTC).Where(x => x.RRDescription == "Appointment Cancelled").Select(x => x.ID).FirstOrDefault();

                    var PIDSTATSUs = WYNKContext.ConfigurationTrans.OrderByDescending(x => x.CreatedUTC).Where(x => x.ConfigurationID == Statusid).Select(x => x.NotifyPatient_SMS).FirstOrDefault();
                    var DIDSTATSUs = WYNKContext.ConfigurationTrans.OrderByDescending(x => x.CreatedUTC).Where(x => x.ConfigurationID == Statusid).Select(x => x.NotifyPatient_SMS).FirstOrDefault();

                    if (PIDSTATSUs == true)
                    {
                        string usersid = "cmpsadmin";
                        string apikey = "UMrCTzVADqibrFY4PAto";
                        object mobile = phonenumber;
                        string msgtext = "Hi your Appointment is Cancelled due to " + APpointdata.AppointmentCancellationReasons;
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

                    if (DIDSTATSUs == true)
                    {
                        string usersid = "cmpsadmin";
                        string apikey = "UMrCTzVADqibrFY4PAto";
                        object mobile = doctornumber;
                        string msgtext = "Hi Appointment is Cancelled due to " + APpointdata.AppointmentCancellationReasons;
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
                    WYNKContext.SaveChanges();

                }
                else if (APpointdata.AppointmentStatus == "Scheduled")
                {

                    idd.Isstatus = "Re-scheduled";
                    idd.iscancelledreason = null;
                    idd.AppointmentTime = null;
                    idd.iscancelleddateandtime = DateTime.UtcNow;
                    idd.UpdatedBy = Convert.ToInt32(APpointdata.Appointmentuserid);
                    idd.UpdatedUTC = DateTime.UtcNow;
                    WYNKContext.Entry(idd).State = EntityState.Modified;
                    WYNKContext.SaveChanges();

                    var id = new Appointmenttran();
                    id.AppointmentMasterID = APpointdata.Appointmentmemberd;



                    var idss = WYNKContext.AppointmentTrans.OrderByDescending(x => x.AppointmentTranID).Select(x => x.AppointmentTranID).FirstOrDefault();
                    if (idss != 0)
                    {

                        id.AppointmentTranID = idss + 1;
                    }
                    else
                    {
                        id.AppointmentTranID = 1;
                    }


                    var Appdate = WYNKContext.AppointmentTrans.Where(x => x.Isstatus == "Re-scheduled" && x.AppointmentMasterID == APpointdata.Appointmentmemberd).Select(x => x.AppointmentDateandTime).FirstOrDefault();

                    if (APpointdata.APpointmentSceduleDate != Convert.ToDateTime("01/01/0001 00:00:00"))
                    {
                        var ddate = APpointdata.APpointmentSceduleDate.AddDays(1);
                        id.AppointmentDateandTime = Convert.ToDateTime(ddate);
                    }
                    else
                    {
                        id.AppointmentDateandTime = Appdate;
                    }


                    id.CMPID = Convert.ToInt32(CMPID);
                    id.Doctorid = Convert.ToInt32(APpointdata.Appointmentuserid);

                    id.Isstatus = "Open";
                    id.iscancelledreason = null;
                    id.iscancelleddateandtime = null;
                    id.Appointmentconfirmationsenttodoctor = null;

                    var fulltime = APpointdata.Hour + ':' + APpointdata.Mnute;

                    var Hoursss = APpointdata.Hour;
                    var Minutes = APpointdata.Mnute;

                    if (fulltime != ":" || fulltime != "00:00")
                    {

                        if (APpointdata.Hour != "" && APpointdata.Hour != null)
                        {
                            Hoursss = APpointdata.Hour;
                        }
                        else
                        {
                            Hoursss = "00";
                        }
                        if (APpointdata.Mnute != "" && APpointdata.Mnute != null)
                        {
                            Minutes = APpointdata.Mnute;
                        }
                        else
                        {
                            Minutes = "00";
                        }

                        id.AppointmentTime = Hoursss + ':' + Minutes;
                    }
                    else
                    {
                        id.AppointmentTime = null;
                    }


                    id.Contraid = APpointdata.Appointmentmemberd;
                    id.CreatedUTC = DateTime.UtcNow;
                    id.CreatedBy = Convert.ToInt32(APpointdata.Appointmentuserid);

                    WYNKContext.AppointmentTrans.Add(id);

                    WYNKContext.SaveChanges();



                    var phonenumber = WYNKContext.Appointment.Where(x => x.RandomUniqueID == idd.AppointmentMasterID).Select(x => x.Phone).FirstOrDefault();


                    var docid = WYNKContext.AppointmentTrans.OrderByDescending(x => x.CreatedUTC).Where(x => x.AppointmentMasterID == APpointdata.Appointmentmemberd).Select(x => x.Doctorid).FirstOrDefault();

                    var doctornumber = CMPSContext.DoctorMaster.Where(x => x.DoctorID == Convert.ToInt32(docid)).Select(x => x.Phone1).FirstOrDefault();

                    var Statusid = WYNKContext.Configuration.OrderByDescending(x => x.CreatedUTC).Where(x => x.RRDescription == "Appointment Re-scheduled").Select(x => x.ID).FirstOrDefault();

                    var PIDSTATSUs = WYNKContext.ConfigurationTrans.OrderByDescending(x => x.CreatedUTC).Where(x => x.ConfigurationID == Statusid).Select(x => x.NotifyPatient_SMS).FirstOrDefault();
                    var DIDSTATSUs = WYNKContext.ConfigurationTrans.OrderByDescending(x => x.CreatedUTC).Where(x => x.ConfigurationID == Statusid).Select(x => x.NotifyPatient_SMS).FirstOrDefault();

                    if (PIDSTATSUs == true)
                    {
                        string usersid = "cmpsadmin";
                        string apikey = "UMrCTzVADqibrFY4PAto";
                        object mobile = phonenumber;
                        string msgtext = "Hi your Appointment is Rescheduled on " + Convert.ToDateTime(APpointdata.APpointmentSceduleDate.AddDays(1)).ToString("dd-MMM-yyyy") + ' ' + APpointdata.Hour + ':' + APpointdata.Mnute;
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

                    if (DIDSTATSUs == true)
                    {
                        string usersid = "cmpsadmin";
                        string apikey = "UMrCTzVADqibrFY4PAto";
                        object mobile = doctornumber;
                        string msgtext = "Hi your Appointment is Rescheduled on " + Convert.ToDateTime(APpointdata.APpointmentSceduleDate.AddDays(1)).ToString("dd-MMM-yyyy") + ' ' + APpointdata.Hour + ':' + APpointdata.Mnute;
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
                else if (APpointdata.AppointmentStatus == "Confirmed")
                {


                    idd.Isstatus = "Confirmed";
                    idd.iscancelledreason = null;
                    idd.iscancelleddateandtime = null;
                    idd.Appointmentconfirmationsenttodoctor = DateTime.UtcNow;
                    idd.UpdatedBy = Convert.ToInt32(APpointdata.Appointmentuserid);
                    idd.UpdatedUTC = DateTime.UtcNow;

                    var phonenumber = WYNKContext.Appointment.Where(x => x.RandomUniqueID == idd.AppointmentMasterID).Select(x => x.Phone).FirstOrDefault();


                    var docid = WYNKContext.AppointmentTrans.OrderByDescending(x => x.CreatedUTC).Where(x => x.AppointmentMasterID == APpointdata.Appointmentmemberd).Select(x => x.Doctorid).FirstOrDefault();

                    var doctornumber = CMPSContext.DoctorMaster.Where(x => x.DoctorID == Convert.ToInt32(docid)).Select(x => x.Phone1).FirstOrDefault();

                    var Statusid = WYNKContext.Configuration.OrderByDescending(x => x.CreatedUTC).Where(x => x.RRDescription == "Appointment Confirmation").Select(x => x.ID).FirstOrDefault();

                    var PIDSTATSUs = WYNKContext.ConfigurationTrans.OrderByDescending(x => x.CreatedUTC).Where(x => x.ConfigurationID == Statusid).Select(x => x.NotifyPatient_SMS).FirstOrDefault();
                    var DIDSTATSUs = WYNKContext.ConfigurationTrans.OrderByDescending(x => x.CreatedUTC).Where(x => x.ConfigurationID == Statusid).Select(x => x.NotifyPatient_SMS).FirstOrDefault();

                    if (PIDSTATSUs == true)
                    {
                        string usersid = "cmpsadmin";
                        string apikey = "UMrCTzVADqibrFY4PAto";
                        object mobile = phonenumber;
                        string msgtext = "Hi your Appointment is Confirmed";
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

                    if (DIDSTATSUs == true)
                    {
                        string usersid = "cmpsadmin";
                        string apikey = "UMrCTzVADqibrFY4PAto";
                        object mobile = doctornumber;
                        string msgtext = "Hi your Appointment is Confirmed";
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



                    WYNKContext.Entry(idd).State = EntityState.Modified;
                    WYNKContext.SaveChanges();

                }


                if (WYNKContext.SaveChanges() >= 0)
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
            };


        }



        public dynamic InsertAppointmentCancelledbymembvers(string CMPID, RegistrationMasterViewModel APpointdata)
        {
            var model = new RegistrationMasterViewModel();

            try
            {
                var appid = WYNKContext.Appointment.Where(x => x.RandomUniqueID == APpointdata.Appointmentmemberd).FirstOrDefault();

                var apptransid = WYNKContext.AppointmentTrans.Where(x => x.AppointmentMasterID == appid.RandomUniqueID).ToList();

                foreach (var item in apptransid)
                {
                    var sta = item.Isstatus;
                    if (sta != "Cancelled")
                    {


                        if (sta == "Re-scheduled")
                        {
                            var apptransids = WYNKContext.AppointmentTrans.Where(x => x.AppointmentMasterID == item.AppointmentMasterID && x.Contraid == item.AppointmentMasterID).FirstOrDefault();
                            if (APpointdata.Appointmentuserid == "A" || APpointdata.Appointmentuserid == "R")
                            {
                                var Patientdata = WYNKContext.Appointment.Where(x => x.RandomUniqueID == item.AppointmentMasterID).FirstOrDefault();

                                apptransids.Isstatus = "Cancelled";
                                apptransids.iscancelleddateandtime = DateTime.UtcNow;
                                apptransids.iscancelledreason = APpointdata.AppointmentCancellationReasons;
                                apptransids.UpdatedUTC = DateTime.UtcNow;
                                apptransids.Cancelledby = "Patient";
                                WYNKContext.Entry(apptransids).State = EntityState.Modified;
                                WYNKContext.SaveChanges();

                                var apptransidss = WYNKContext.AppointmentTrans.OrderByDescending(x => x.UpdatedUTC).Where(x => x.AppointmentMasterID == item.AppointmentMasterID && x.Contraid == item.AppointmentMasterID).FirstOrDefault();
                                var Patientphone = Patientdata.Phone;
                                var PatientCMP = Patientdata.CMPID;
                                var ompnayname = CMPSContext.Company.Where(x => x.CmpID == PatientCMP).Select(x => x.CompanyName).FirstOrDefault();
                                var Patientname = Patientdata.PatientName;
                                var Message = CMPSContext.MessagingTemplate.OrderByDescending(x => x.CreatedUTC).Where(x => x.CMPID == PatientCMP
                                && x.MsgTemplateName == "Appointment Cancelled").Select(x => x.SMSMsgDescription).FirstOrDefault();

                                var configstatus = WYNKContext.Configuration.OrderByDescending(x => x.CreatedUTC).Where(x => x.RRDescription == "Appointment Cancelled"
                                 && x.CMPID == PatientCMP).Select(x => x.ID).FirstOrDefault();

                                var statusid = WYNKContext.ConfigurationTrans.Where(x => x.ConfigurationID == configstatus).Select(x => x.NotifyPatient_SMS).FirstOrDefault();
                                var Stringmesage = Message;
                                MatchCollection allMatchResults = null;
                                var regexObj = new Regex(@"<\w*>");
                                allMatchResults = regexObj.Matches(Message);

                                string[] Seondnames = typeof(Appointmenttran).GetProperties()
                                .Select(property => property.Name)
                                .ToArray();

                                foreach (var Regitem in allMatchResults)
                                {

                                    var app = new Appointmenttran();
                                    string stringdata = Convert.ToString(Regitem);
                                    var parts = stringdata.Split(new[] { '<', '>' }, StringSplitOptions.RemoveEmptyEntries);
                                    string Realmessage = Convert.ToString(parts[0]);

                                    var allMatchResultsss = Array.Exists(Seondnames, element => element == Realmessage);
                                    object dd = "";
                                    if (allMatchResultsss == false)
                                    {
                                        dd = WYNKContext.Entry(Patientdata).CurrentValues[Realmessage];
                                    }
                                    else
                                    {
                                        dd = WYNKContext.Entry(apptransidss).CurrentValues[Realmessage];
                                    }


                                    var strinfdata = "<" + Realmessage + ">";
                                    string dataaa = Convert.ToString(dd);
                                    string constringdata = Stringmesage.Replace(strinfdata, dataaa);
                                    Stringmesage = constringdata;
                                }


                                if (statusid == true)
                                {
                                    PasswordEncodeandDecode.Sendsmsmessage(Patientphone, Stringmesage, Patientname, ompnayname);
                                }


                            }
                            else
                            {
                                var Patientdata = WYNKContext.Appointment.Where(x => x.RandomUniqueID == item.AppointmentMasterID).FirstOrDefault();
                                apptransids.Isstatus = "Cancelled";
                                apptransids.iscancelleddateandtime = DateTime.UtcNow;
                                apptransids.iscancelledreason = APpointdata.AppointmentCancellationReasons;
                                apptransids.UpdatedUTC = DateTime.UtcNow;
                                apptransids.Cancelledby = "Doctor";
                                WYNKContext.Entry(apptransids).State = EntityState.Modified;
                                WYNKContext.SaveChanges();
                                var apptransidss = WYNKContext.AppointmentTrans.OrderByDescending(x => x.UpdatedUTC).Where(x => x.AppointmentMasterID == item.AppointmentMasterID && x.Contraid == item.AppointmentMasterID).FirstOrDefault();
                                var Patientphone = Patientdata.Phone;
                                var PatientCMP = Patientdata.CMPID;
                                var ompnayname = CMPSContext.Company.Where(x => x.CmpID == PatientCMP).Select(x => x.CompanyName).FirstOrDefault();
                                var Patientname = Patientdata.PatientName;

                                var Message = CMPSContext.MessagingTemplate.OrderByDescending(x => x.CreatedUTC).Where(x => x.CMPID == PatientCMP
                                && x.MsgTemplateName == "Appointment Cancelled").Select(x => x.SMSMsgDescription).FirstOrDefault();

                                var configstatus = WYNKContext.Configuration.OrderByDescending(x => x.CreatedUTC).Where(x => x.RRDescription == "Appointment Cancelled"
                                 && x.CMPID == PatientCMP).Select(x => x.ID).FirstOrDefault();

                                var statusid = WYNKContext.ConfigurationTrans.Where(x => x.ConfigurationID == configstatus).Select(x => x.NotifyPatient_SMS).FirstOrDefault();

                                var Stringmesage = Message;
                                MatchCollection allMatchResults = null;
                                var regexObj = new Regex(@"<\w*>");
                                allMatchResults = regexObj.Matches(Message);

                                string[] Seondnames = typeof(Appointmenttran).GetProperties()
                                .Select(property => property.Name)
                                .ToArray();

                                foreach (var Regitem in allMatchResults)
                                {

                                    var app = new Appointmenttran();
                                    string stringdata = Convert.ToString(Regitem);
                                    var parts = stringdata.Split(new[] { '<', '>' }, StringSplitOptions.RemoveEmptyEntries);
                                    string Realmessage = Convert.ToString(parts[0]);

                                    var allMatchResultsss = Array.Exists(Seondnames, element => element == Realmessage);
                                    object dd = "";
                                    if (allMatchResultsss == false)
                                    {
                                        dd = WYNKContext.Entry(Patientdata).CurrentValues[Realmessage];
                                    }
                                    else
                                    {
                                        dd = WYNKContext.Entry(apptransidss).CurrentValues[Realmessage];
                                    }


                                    var strinfdata = "<" + Realmessage + ">";
                                    string dataaa = Convert.ToString(dd);
                                    string constringdata = Stringmesage.Replace(strinfdata, dataaa);
                                    Stringmesage = constringdata;
                                }


                                if (statusid == true)
                                {
                                    PasswordEncodeandDecode.Sendsmsmessage(Patientphone, Stringmesage, Patientname, ompnayname);
                                }


                            }
                        }
                        else
                        {
                            var apptransids = WYNKContext.AppointmentTrans.Where(x => x.AppointmentMasterID == item.AppointmentMasterID).FirstOrDefault();
                            if (APpointdata.Appointmentuserid == "A" || APpointdata.Appointmentuserid == "R")
                            {



                                apptransids.Isstatus = "Cancelled";
                                apptransids.iscancelleddateandtime = DateTime.UtcNow;
                                apptransids.iscancelledreason = APpointdata.AppointmentCancellationReasons;
                                apptransids.UpdatedUTC = DateTime.UtcNow;
                                apptransids.Cancelledby = "Patient";
                                WYNKContext.Entry(apptransids).State = EntityState.Modified;
                                WYNKContext.SaveChanges();
                                var Patientdata = WYNKContext.Appointment.Where(x => x.RandomUniqueID == item.AppointmentMasterID).FirstOrDefault();
                                var apptransidss = WYNKContext.AppointmentTrans.OrderByDescending(x => x.UpdatedUTC).Where(x => x.AppointmentMasterID == item.AppointmentMasterID).FirstOrDefault();
                                var Patientphone = Patientdata.Phone;
                                var PatientCMP = Patientdata.CMPID;
                                var ompnayname = CMPSContext.Company.Where(x => x.CmpID == PatientCMP).Select(x => x.CompanyName).FirstOrDefault();
                                var Patientname = Patientdata.PatientName;

                                var Message = CMPSContext.MessagingTemplate.OrderByDescending(x => x.CreatedUTC).Where(x => x.CMPID == PatientCMP
                                && x.MsgTemplateName == "Appointment Cancelled").Select(x => x.SMSMsgDescription).FirstOrDefault();

                                var configstatus = WYNKContext.Configuration.OrderByDescending(x => x.CreatedUTC).Where(x => x.RRDescription == "Appointment Cancelled"
                                 && x.CMPID == PatientCMP).Select(x => x.ID).FirstOrDefault();

                                var statusid = WYNKContext.ConfigurationTrans.Where(x => x.ConfigurationID == configstatus).Select(x => x.NotifyPatient_SMS).FirstOrDefault();
                                if (Message != null)
                                {
                                    var Stringmesage = Message;
                                    MatchCollection allMatchResults = null;
                                    var regexObj = new Regex(@"<\w*>");
                                    allMatchResults = regexObj.Matches(Message);

                                    string[] Seondnames = typeof(Appointmenttran).GetProperties()
                                    .Select(property => property.Name)
                                    .ToArray();

                                    foreach (var Regitem in allMatchResults)
                                    {

                                        var app = new Appointmenttran();
                                        string stringdata = Convert.ToString(Regitem);
                                        var parts = stringdata.Split(new[] { '<', '>' }, StringSplitOptions.RemoveEmptyEntries);
                                        string Realmessage = Convert.ToString(parts[0]);

                                        var allMatchResultsss = Array.Exists(Seondnames, element => element == Realmessage);
                                        object dd = "";
                                        if (allMatchResultsss == false)
                                        {
                                            dd = WYNKContext.Entry(Patientdata).CurrentValues[Realmessage];
                                        }
                                        else
                                        {
                                            dd = WYNKContext.Entry(apptransidss).CurrentValues[Realmessage];
                                        }


                                        var strinfdata = "<" + Realmessage + ">";
                                        string dataaa = Convert.ToString(dd);
                                        string constringdata = Stringmesage.Replace(strinfdata, dataaa);
                                        Stringmesage = constringdata;
                                    }


                                    if (statusid == true)
                                    {
                                        PasswordEncodeandDecode.Sendsmsmessage(Patientphone, Stringmesage, Patientname, ompnayname);
                                    }
                                }
                            }
                            else
                            {
                                apptransids.Isstatus = "Cancelled";
                                apptransids.iscancelleddateandtime = DateTime.UtcNow;
                                apptransids.iscancelledreason = APpointdata.AppointmentCancellationReasons;
                                apptransids.UpdatedUTC = DateTime.UtcNow;
                                apptransids.Cancelledby = "Doctor";
                                WYNKContext.Entry(apptransids).State = EntityState.Modified;
                                WYNKContext.SaveChanges();
                                var Patientdata = WYNKContext.Appointment.Where(x => x.RandomUniqueID == item.AppointmentMasterID).FirstOrDefault();
                                var apptransidss = WYNKContext.AppointmentTrans.OrderByDescending(x => x.UpdatedUTC).Where(x => x.AppointmentMasterID == item.AppointmentMasterID && x.Contraid == item.AppointmentMasterID).FirstOrDefault();
                                var Patientphone = Patientdata.Phone;
                                var PatientCMP = Patientdata.CMPID;
                                var ompnayname = CMPSContext.Company.Where(x => x.CmpID == PatientCMP).Select(x => x.CompanyName).FirstOrDefault();
                                var Patientname = Patientdata.PatientName;

                                var Message = CMPSContext.MessagingTemplate.OrderByDescending(x => x.CreatedUTC).Where(x => x.CMPID == PatientCMP
                                && x.MsgTemplateName == "Appointment Cancelled").Select(x => x.SMSMsgDescription).FirstOrDefault();

                                var configstatus = WYNKContext.Configuration.OrderByDescending(x => x.CreatedUTC).Where(x => x.RRDescription == "Appointment Cancelled"
                                 && x.CMPID == PatientCMP).Select(x => x.ID).FirstOrDefault();

                                var statusid = WYNKContext.ConfigurationTrans.Where(x => x.ConfigurationID == configstatus).Select(x => x.NotifyPatient_SMS).FirstOrDefault();
                                if (Message != null)
                                {
                                    var Stringmesage = Message;
                                    MatchCollection allMatchResults = null;
                                    var regexObj = new Regex(@"<\w*>");
                                    allMatchResults = regexObj.Matches(Message);

                                    string[] Seondnames = typeof(Appointmenttran).GetProperties()
                                    .Select(property => property.Name)
                                    .ToArray();

                                    foreach (var Regitem in allMatchResults)
                                    {

                                        var app = new Appointmenttran();
                                        string stringdata = Convert.ToString(Regitem);
                                        var parts = stringdata.Split(new[] { '<', '>' }, StringSplitOptions.RemoveEmptyEntries);
                                        string Realmessage = Convert.ToString(parts[0]);

                                        var allMatchResultsss = Array.Exists(Seondnames, element => element == Realmessage);
                                        object dd = "";
                                        if (allMatchResultsss == false)
                                        {
                                            dd = WYNKContext.Entry(Patientdata).CurrentValues[Realmessage];
                                        }
                                        else
                                        {
                                            dd = WYNKContext.Entry(apptransidss).CurrentValues[Realmessage];
                                        }


                                        var strinfdata = "<" + Realmessage + ">";
                                        string dataaa = Convert.ToString(dd);
                                        string constringdata = Stringmesage.Replace(strinfdata, dataaa);
                                        Stringmesage = constringdata;
                                    }


                                    if (statusid == true)
                                    {
                                        PasswordEncodeandDecode.Sendsmsmessage(Patientphone, Stringmesage, Patientname, ompnayname);
                                    }
                                }

                            }
                        }




                    }


                }






                if (WYNKContext.SaveChanges() >= 0)
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
            };

        }



        public dynamic Getconfirmstatus(int CmpID)
        {
            var RR = new RegistrationMasterViewModel();

            var Statusid = WYNKContext.Configuration.OrderByDescending(x => x.CreatedUTC).Where(x => x.RRDescription == "Appointment Confirmation").Select(x => x.ID).FirstOrDefault();

            var PIDSTATSUs = WYNKContext.ConfigurationTrans.OrderByDescending(x => x.CreatedUTC).Where(x => x.ConfigurationID == Statusid).Select(x => x.NotifyPatient_SMS).FirstOrDefault();

            if (PIDSTATSUs == false)
            {
                RR.Messagestatus = "Nostatusavailable";
            }
            else
            {
                RR.Messagestatus = "statusavailable";
            }

            return RR;
        }


        public dynamic GetpatientVoiceconsentdata(Consentdata Consentdata)
        {

            var dataexist = new Consentdata();
            dataexist.Totalvoiceconsentdata = new List<GetConsentdetails>();
            var fromdate = Convert.ToDateTime(Consentdata.Fromdate);
            var Todate = Convert.ToDateTime(Consentdata.Todate);
            var withoutregdate = WYNKContext.Registration.Where(x => x.CMPID == Convert.ToInt32(Consentdata.cmpid)).ToList();
            var Regdata = withoutregdate.OrderByDescending(x => x.CreatedUTC).Where(x => x.DateofRegistration.Date >= fromdate.Date && x.DateofRegistration.Date <= Todate.Date).ToList();

            var det = (from c in Regdata
                       select new GetConsentdetails
                       {
                           UIN = c.UIN,
                           Name = c.Name + " " + c.LastName,
                           Consentcreated = c.DateofRegistration,
                           Voice = getboolvalu(c.VoiceConsent),
                           video = getboolvalu(c.Videoconsent)
                       }).ToList();
            dataexist.Totalvoiceconsentdata = det;

            return dataexist;
        }

        public static string getboolvalu(bool? voice)
        {
            var d = "";
            if (voice == false)
            {
                d = "NO";
            }
            else if (voice == null)
            {
                d = "EMpty";
            }
            else
            {
                d = "Yes";
            }
            return d;
        }

        public string getusername(int userid)
        {
            var d = "";
            if (userid != null)
            {
                d = CMPSContext.Users.Where(x => x.Userid == userid).Select(x => x.Username).FirstOrDefault();
            }

            else
            {
                d = "User undefined";
            }
            return d;
        }
        public string getpatientname(string UIN)
        {
            var s = "";
            var d = WYNKContext.Registration.Where(x => x.UIN == UIN).Select(x => x.MiddleName).FirstOrDefault();

            if (d != null)
            {
                s = d;
            }
            else
            {
                s = " ";
            }
            return s;
        }
        public dynamic Getprint(int CMPID, string UIN, string Phase)
        {
            var data = new RegistrationMasterViewModel();
            var Newolmid = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "New").Select(x => x.OLMID).FirstOrDefault();
            var Reviewolmid = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Review").Select(x => x.OLMID).FirstOrDefault();
            var Companydeatils = CMPSContext.Company.Where(x => x.CmpID == CMPID).FirstOrDefault();
            var Registrationdeatils = WYNKContext.Registration.Where(x => x.UIN == UIN).FirstOrDefault();
            var registrationtrandetails = WYNKContext.RegistrationTran.OrderByDescending(x => x.CreatedUTC).Where(x => x.UIN == UIN && x.PatientVisitType == Convert.ToString(Newolmid)).Select(x => x.RegistrationTranID).FirstOrDefault();
            if (Phase == "New")
            {
                data.cmpname = Companydeatils.CompanyName;
                data.cmpaddress = Companydeatils.Address1;
                data.cmpaddress2 = Companydeatils.Address2;
                data.cmpaddress3 = Companydeatils.Address3;
                data.cmpphone = Companydeatils.Phone1;
                data.patientuin = Registrationdeatils.UIN;
                data.name = Registrationdeatils.Name;
                data.middlename = getpatientname(UIN);
                data.lastname = Registrationdeatils.LastName;
                data.age = PasswordEncodeandDecode.ToAgeString(Registrationdeatils.DateofBirth);
                data.address = Registrationdeatils.Address1;
                data.gender = Registrationdeatils.Gender;
                data.dateofregistration = Registrationdeatils.DateofRegistration;
                data.phone = Registrationdeatils.Phone;
                var usrroledata = Registrationdeatils.CreatedBy;
                data.UserRole = CMPSContext.Users.Where(x => x.Userid == usrroledata).Select(x => x.Username).FirstOrDefault();
                var cid = CMPSContext.Setup.Where(x => x.CMPID == CMPID).Select(x => x.Country).FirstOrDefault();
                data.country = CMPSContext.Country.Where(x => x.ID == Convert.ToInt32(cid)).Select(x => x.CountryName).FirstOrDefault();
                data.receiptnumber = WYNKContext.PaymentMaster.Where(x => x.UIN == UIN && x.PaymentType == "O" && x.IsBilled == true && x.CmpID == CMPID).Select(x => x.ReceiptNumber).FirstOrDefault();
                data.receiptdate = WYNKContext.PaymentMaster.Where(x => x.UIN == UIN && x.PaymentType == "O" && x.IsBilled == true && x.CmpID == CMPID).Select(x => x.ReceiptDate).FirstOrDefault();
                data.dashboardpaymentReMode = (from PTY in WYNKContext.PaymentMaster.Where(x => x.UIN == UIN
                                               && x.PaymentType == "O" && x.IsBilled == true && x.CmpID == CMPID)
                                               select new dashboardpaymentReMode
                                               {
                                                   PaymentMode = PTY.PaymentMode,
                                                   InstrumentNumber = PTY.InstrumentNumber,
                                                   Instrumentdate = PTY.Instrumentdate,
                                                   BankName = PTY.BankName,
                                                   BankBranch = PTY.BankBranch,
                                                   Expirydate = PTY.Expirydate,
                                                   Amount = PTY.Amount,
                                               }
                          ).ToList();
                data.amount = data.dashboardpaymentReMode.Sum(x => x.Amount);
            }
            else if (Phase == "Review")
            {

            }


            return data;
        }


        public dynamic Getconsultationbill(int CMPID, string UIN)
        {
            var data = new RegistrationMasterViewModel();
            var registrationtrandetails = WYNKContext.RegistrationTran.OrderByDescending(x => x.DateofVisit.Date).Where(x => x.UIN == UIN && x.ConsulationFees != null && x.DoctorID != 0).ToList();
            if (registrationtrandetails.Count() != 0)
            {
                var Companydeatils = CMPSContext.Company.Where(x => x.CmpID == CMPID).FirstOrDefault();
                var Registrationdeatils = WYNKContext.Registration.Where(x => x.UIN == UIN).FirstOrDefault();
                var dmdet = CMPSContext.DoctorMaster.AsNoTracking().ToList();
                data.COnsultationmembersdata = (from gg in registrationtrandetails.GroupBy(x => x.DateofVisit.Date)
                                                select new COnsultationmembersdata
                                                {
                                                    dov = gg.FirstOrDefault().DateofVisit,
                                                    consultant = dmdet.Where(x => x.DoctorID == Convert.ToInt32(gg.FirstOrDefault().DoctorID)).Select(x => x.LastName).FirstOrDefault(),
                                                    fees = gg.FirstOrDefault().ConsulationFees,
                                                }).ToList();
                data.totalamountconsultation = data.COnsultationmembersdata.Sum(x => x.fees);
                data.name = Registrationdeatils.Name;
                data.lastname = Registrationdeatils.LastName;
                data.UIN = Registrationdeatils.UIN;
                data.CompanyName = Companydeatils.CompanyName;
                data.cmpaddress = Companydeatils.Address1;
                data.cmpphone = Companydeatils.Phone1;
            }



            return data;
        }

        public dynamic getdate(DateTime? Date)
        {
            var data = Date;
            var anotherdata = Convert.ToString(data);

            if (anotherdata != "1/1/0001 12:00:00 AM" && anotherdata != "" && anotherdata != null)
            {
                data = Convert.ToDateTime(Date);
            }
            else
            {
                data = null;
            }
            return data;
        }
        public dynamic getdates(DateTime? Date)
        {
            var data = "";
            var anotherdata = Convert.ToString(Date);
            if (anotherdata != "1/1/0001 12:00:00 AM" && anotherdata != "" && anotherdata != null)
            {
                data = Convert.ToDateTime(Date).ToString("dd-MMM-yyyy HH:mm");
            }
            else
            {
                data = "";
            }
            return data;
        }


        public dynamic DeleteTokenbasaedonlink(string username)
        {

            try
            {
                var userstable = CMPSContext.Users.Where(x => x.Username == username).FirstOrDefault();
                string ipAddress = "";
                ErrorLog oErrorLog = new ErrorLog();
                oErrorLog.WriteErrorLog("Information :", "User trying to logout from previous session: userid : " + username + ", Date & Time : " + DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                ErrorLog oErrorLog = new ErrorLog();
                oErrorLog.WriteErrorLog("Error :", ex.InnerException.Message.ToString());
                Console.Write(ex);
            }
            return new
            {
                Success = false,
                Message = "Some data are Missing"
            };
        }

        public dynamic DeleteTokenbasaedonID(int username)
        {
            string ipAddress = "";
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            ipAddress = Convert.ToString(ipHostInfo.AddressList.FirstOrDefault(address => address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork));
            var loginstatus = new RegistrationMasterViewModel();
            var userTokenid = CMPSContext.LogUser.OrderByDescending(x => x.LoguserID).Where(x => x.UserID == Convert.ToInt32(username)).Select(x => x.LoguserID).FirstOrDefault();
            var uservsroles = new LogUser();
            uservsroles = CMPSContext.LogUser.OrderByDescending(x => x.LoginDatetime).Where(x => x.LoguserID == userTokenid).FirstOrDefault();
            if (uservsroles != null)
            {
                uservsroles.LogoutDatetime = DateTime.UtcNow;
                uservsroles.IsLogged = false;
                CMPSContext.Entry(uservsroles).State = EntityState.Modified;
                CMPSContext.SaveChanges();
            }

            try
            {
                if (CMPSContext.SaveChanges() >= 0)
                {
                    ErrorLog oErrorLog = new ErrorLog();
                    oErrorLog.WriteErrorLog("Information :", "Successfully Logged out. userid : " + username + ", Date & Time : " + DateTime.UtcNow);
                    return new
                    {
                        Success = true,
                    };
                }

            }
            catch (Exception ex)
            {
                ErrorLog oErrorLog = new ErrorLog();
                oErrorLog.WriteErrorLog("Error :", ex.InnerException.Message.ToString());
                Console.Write(ex);
            }
            return new
            {
                Success = false,
                Message = "Some data are Missing"
            };
        }

        public dynamic Deleteselectedimage(string UIN, int CMPID, string Imagename, string status)
        {

            var Regdata = new RegistrationMasterViewModel();
            var Companyid = Convert.ToInt32(CMPID);
            var datetime = DateTime.UtcNow.Date.ToString("dd-MMM-yyyy");
            var currentDir = Directory.GetCurrentDirectory();
            var patientfilenames = Imagename;
            if (!Directory.Exists(currentDir + "/PatientSummmaryFiles/" + Companyid + "/Patients/" + UIN + '/'))
                Directory.CreateDirectory(currentDir + "/PatientSummmaryFiles/" + Companyid + "/Patients/" + UIN + '/');
            var newpath = $"{currentDir}/PatientSummmaryFiles/{Companyid}/Patients/{UIN}/{patientfilenames}";
            if (File.Exists(newpath))
            {
                File.Delete(newpath);
            }
            else
            {
                Regdata.Messagestatus = "NoImage";
            }

            return Regdata;
        }
        public RegistrationMasterViewModel GetselectedsearchPatientDetails(string ViewsStatusid, string RoleDescription, int userDoctorIDs, string todaydates, int CompanyID)
        {
            var utctimes = CMPSContext.Setup.Where(x => x.CMPID == CompanyID).Select(x => x.UTCTime).FirstOrDefault();
            TimeSpan utctime = TimeSpan.Parse(utctimes);
            var reg = new RegistrationMasterViewModel();
            reg.PatientAssignStatus = new List<PatientAssignStatus>();
            if (userDoctorIDs != null)
            {
                var Getonelinemaster = CMPSContext.OneLineMaster.ToList();
                var Getdoctormaster = CMPSContext.DoctorMaster.ToList();
                var sViewsStatusid = Getonelinemaster.Where(x => x.ParentDescription == ViewsStatusid).Select(x => x.OLMID).FirstOrDefault();
                var Getregistration = WYNKContext.Registration.Where(x => x.CMPID == CompanyID && x.IsDeleted == false).ToList();
                var Getregistrationtran = WYNKContext.RegistrationTran.Where(x => x.CmpID == CompanyID && x.Status == sViewsStatusid).ToList();
                var GetPatientassion = WYNKContext.PatientAssign.AsNoTracking().ToList();


                if (RoleDescription == "Admin" || RoleDescription == "Reception")
                {
                    reg.Patientlist = (from RM in Getregistration.Where(x => x.UIN.StartsWith(Convert.ToString(todaydates), StringComparison.OrdinalIgnoreCase)
                              || x.Phone.StartsWith(Convert.ToString(todaydates)) || x.Name.StartsWith(Convert.ToString(todaydates), StringComparison.OrdinalIgnoreCase)
                              || x.MiddleName == todaydates
                              || x.AadharNumber == todaydates
                              || x.LastName.StartsWith(Convert.ToString(todaydates), StringComparison.OrdinalIgnoreCase)
                              || x.Address1.StartsWith(Convert.ToString(todaydates), StringComparison.OrdinalIgnoreCase))
                                       join RMT in Getregistrationtran on RM.UIN equals RMT.UIN
                                       orderby RM.DateofRegistration descending
                                       select new Patientlists
                                       {
                                           UIN = RM.UIN,
                                           Name = GetConcatName(RM.Name),
                                           MName = GetConcatName(RM.MiddleName),
                                           LName = GetConcatName(RM.LastName),
                                           DOR = Convert.ToDateTime(RM.DateofRegistration + utctime),
                                           Age = PasswordEncodeandDecode.ToAgeString(RM.DateofBirth),
                                           DOB = RM.DateofBirth,
                                           Sex = RM.Gender,
                                           Status = Getonelinemaster.Where(x => x.OLMID == RMT.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                                           RegistrationTranID = RMT.RegistrationTranID,
                                           AssDoctorID = Getdoctormaster.Where(x => x.DoctorID == RMT.DoctorID).Select(x => x.LastName).FirstOrDefault(),
                                           AssignedDate = getdate(Convert.ToDateTime(RMT.AssignedDate)),

                                       }).ToList();

                }
                else
                {
                    reg.Patientlist = (from RM in Getregistration.Where(x => x.UIN.StartsWith(Convert.ToString(todaydates), StringComparison.OrdinalIgnoreCase)
                              || x.Phone.StartsWith(Convert.ToString(todaydates)) || x.Name.StartsWith(Convert.ToString(todaydates), StringComparison.OrdinalIgnoreCase)
                              || x.MiddleName == todaydates
                              || x.AadharNumber == todaydates
                              || x.LastName.StartsWith(Convert.ToString(todaydates), StringComparison.OrdinalIgnoreCase)
                              || x.Address1.StartsWith(Convert.ToString(todaydates), StringComparison.OrdinalIgnoreCase))
                                       join RMT in Getregistrationtran on RM.UIN equals RMT.UIN
                                       join PAT in GetPatientassion on RMT.RegistrationTranID equals PAT.RegistrationTranID
                                       where PAT.DoctorID == userDoctorIDs
                                       orderby RM.DateofRegistration descending
                                       select new Patientlists
                                       {
                                           UIN = RM.UIN,
                                           Name = GetConcatName(RM.Name),
                                           MName = GetConcatName(RM.MiddleName),
                                           LName = GetConcatName(RM.LastName),
                                           DOR = Convert.ToDateTime(RM.DateofRegistration + utctime),
                                           Age = PasswordEncodeandDecode.ToAgeString(RM.DateofBirth),
                                           Sex = RM.Gender,
                                           DOB = RM.DateofBirth,
                                           Status = Getonelinemaster.Where(x => x.OLMID == RMT.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                                           RegistrationTranID = RMT.RegistrationTranID,
                                           AssDoctorID = Getdoctormaster.Where(x => x.DoctorID == RMT.DoctorID).Select(x => x.LastName).FirstOrDefault(),
                                           AssignedDate = getdate(Convert.ToDateTime(RMT.AssignedDate)),
                                       }).ToList();
                }


            }

            return reg;
        }

        public RegistrationMasterViewModel GetselectedsearchReviewPatientDetails(int ViewsStatusid, string RoleDescription, int userDoctorIDs, string todaydates, int CompanyID)
        {
            var utctimes = CMPSContext.Setup.Where(x => x.CMPID == CompanyID).Select(x => x.UTCTime).FirstOrDefault();
            TimeSpan utctime = TimeSpan.Parse(utctimes);
            var reg = new RegistrationMasterViewModel();
            reg.PatientAssignStatus = new List<PatientAssignStatus>();
            if (userDoctorIDs != null)
            {
                var Getregistration = WYNKContext.Registration.Where(x => x.CMPID == CompanyID).ToList();
                var Getregistrationtran = WYNKContext.RegistrationTran.ToList();
                var GetPatientassion = WYNKContext.PatientAssign.AsNoTracking().ToList();
                var Getonelinemaster = CMPSContext.OneLineMaster.ToList();
                var Getdoctormaster = CMPSContext.DoctorMaster.ToList();
                var New = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Review").Select(x => x.OLMID).FirstOrDefault();
                if (RoleDescription == "Admin" || RoleDescription == "Reception")
                {
                    reg.RevPatientlist = (from RM in Getregistration.Where(x => x.UIN.StartsWith(Convert.ToString(todaydates), StringComparison.OrdinalIgnoreCase)
                              || x.Phone.StartsWith(Convert.ToString(todaydates)) || x.Name.StartsWith(Convert.ToString(todaydates), StringComparison.OrdinalIgnoreCase)
                              || x.MiddleName == todaydates
                              || x.AadharNumber == todaydates
                              || x.LastName.StartsWith(Convert.ToString(todaydates), StringComparison.OrdinalIgnoreCase)
                              || x.Address1.StartsWith(Convert.ToString(todaydates), StringComparison.OrdinalIgnoreCase))
                                          join RMT in Getregistrationtran.Where(x => x.CmpID == CompanyID) on RM.UIN equals RMT.UIN
                                          where RMT.PatientVisitType == Convert.ToString(New) && RMT.Status == ViewsStatusid
                                          && RM.IsDeleted == false
                                          orderby RM.DateofRegistration descending
                                          select new RevPatientlist
                                          {
                                              RUIN = RM.UIN,
                                              RName = GetConcatName(RM.Name),
                                              RMName = GetConcatName(RM.MiddleName),
                                              RLName = GetConcatName(RM.LastName),
                                              DOV = Convert.ToDateTime(RMT.DateofVisit + utctime),
                                              RAge = PasswordEncodeandDecode.ToAgeString(RM.DateofBirth),
                                              RSex = RM.Gender,
                                              RStatus = Getonelinemaster.Where(x => x.OLMID == RMT.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                                              RegistrationTranID = RMT.RegistrationTranID,
                                              DoctorID = Getdoctormaster.Where(x => x.DoctorID == RMT.DoctorID).Select(x => x.LastName).FirstOrDefault(),
                                              AssignedDate = getdate(Convert.ToDateTime(RMT.AssignedDate)),

                                          }).ToList();

                }
                else
                {
                    reg.RevPatientlist = (from RM in Getregistration.Where(x => x.UIN.StartsWith(Convert.ToString(todaydates), StringComparison.OrdinalIgnoreCase)
                              || x.Phone.StartsWith(Convert.ToString(todaydates)) || x.Name.StartsWith(Convert.ToString(todaydates), StringComparison.OrdinalIgnoreCase)
                              || x.MiddleName == todaydates
                              || x.AadharNumber == todaydates
                              || x.LastName.StartsWith(Convert.ToString(todaydates), StringComparison.OrdinalIgnoreCase)
                              || x.Address1.StartsWith(Convert.ToString(todaydates), StringComparison.OrdinalIgnoreCase)
                              || x.Address2.StartsWith(Convert.ToString(todaydates), StringComparison.OrdinalIgnoreCase)
                              || x.DateofBirth == Convert.ToDateTime(todaydates))
                                          join RMT in Getregistrationtran.Where(x => x.CmpID == CompanyID) on RM.UIN equals RMT.UIN
                                          join PAT in GetPatientassion on RMT.RegistrationTranID equals PAT.RegistrationTranID
                                          where PAT.DoctorID == userDoctorIDs
                                          && RMT.Status == ViewsStatusid && RMT.PatientVisitType == Convert.ToString(New) && RM.IsDeleted == false
                                          orderby RM.DateofRegistration descending
                                          select new RevPatientlist
                                          {
                                              RUIN = RM.UIN,
                                              RName = GetConcatName(RM.Name),
                                              RMName = GetConcatName(RM.MiddleName),
                                              RLName = GetConcatName(RM.LastName),
                                              DOV = Convert.ToDateTime(RMT.DateofVisit + utctime),
                                              RAge = PasswordEncodeandDecode.ToAgeString(RM.DateofBirth),
                                              RSex = RM.Gender,
                                              RStatus = Getonelinemaster.Where(x => x.OLMID == RMT.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                                              RegistrationTranID = RMT.RegistrationTranID,
                                              DoctorID = Getdoctormaster.Where(x => x.DoctorID == RMT.DoctorID).Select(x => x.LastName).FirstOrDefault(),
                                              AssignedDate = getdate(Convert.ToDateTime(RMT.AssignedDate)),
                                          }).ToList();
                }


            }

            return reg;
        }



        public dynamic CheckPatientAvailabliity(int Regtranid, int CMPID)
        {
            var Regdata = new RegistrationMasterViewModel();
            var Tranid = Regtranid;
            var UIN = WYNKContext.RegistrationTran.Where(x => x.RegistrationTranID == Tranid && x.CmpID == CMPID).Select(x => x.UIN).FirstOrDefault();
            var Oculardetails = WYNKContext.OcularComplaints.Where(x => x.RegistrationTranID == Tranid).FirstOrDefault();
            var SystemicCondiutions = WYNKContext.PatientHistory.Where(x => x.RegistrationTranID == Tranid && x.UIN == UIN).FirstOrDefault();
            var Patientgeneral = WYNKContext.PatientGeneral.Where(x => x.UIN == UIN && x.RegistrationTranID == Tranid).FirstOrDefault();

            if (Oculardetails != null || SystemicCondiutions != null || Patientgeneral != null)
            {
                var PatientAssignStatus = WYNKContext.PatientAssign.Where(x => x.RegistrationTranID == Tranid).FirstOrDefault();
            }
            else
            {

            }
            return Regdata;
        }


        public dynamic InsertReviewfromdashboard(RegistrationMasterI AddReg, string UIN, int Cmpid, int ContraTransactionid, int userid, decimal ConsultationFee)

        {

            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    var registrationMaster = new Registration_Master();
                    var registrationTranMaster = new RegistrationTran_Master();
                    var ManagementMaster = new ManagementMaster();
                    var Payment1 = new Payment_Master();
                    var RegistrationExtension = new RegistrationExtension();
                    var onl = CMPSContext.OneLineMaster.AsNoTracking().ToList();
                    var Status = WYNKContext.RegistrationTran.Where(x => x.UIN == UIN).Select(x => x.Status).LastOrDefault();
                    var CancelledID = onl.Where(x => x.ParentTag == "RegStatus" && x.ParentDescription == "Cancelled").Select(x => x.OLMID).FirstOrDefault();
                    var ClosedID = onl.Where(x => x.ParentTag == "RegStatus" && x.ParentDescription == "Closed").Select(x => x.OLMID).FirstOrDefault();
                    if (Status != CancelledID && Status != ClosedID)
                    {
                        return new
                        {
                            Success = false,
                            Message = "Patient already on the queue please check the status"
                        };
                    }

                    var ReviewCount = WYNKContext.RegistrationTran.Where(x => x.UIN == UIN).Select(x => x.ReviewCount).LastOrDefault();
                    if (AddReg.RegistrationTranMaster.RegistrationFees != 0)
                    {

                        registrationMaster = WYNKContext.Registration.Where(x => x.UIN == UIN).FirstOrDefault();
                        registrationTranMaster.UIN = registrationMaster.UIN;
                        registrationTranMaster.CmpID = Cmpid;
                        registrationTranMaster.DateofVisit = DateTime.UtcNow;
                        registrationTranMaster.TypeofVisit = onl.Where(x => x.ParentTag == "TOV" && x.ParentDescription == "Review").Select(x => x.OLMID).FirstOrDefault();
                        registrationTranMaster.Status = onl.Where(x => x.ParentTag == "RegStatus" && x.ParentDescription == "Open").Select(x => x.OLMID).FirstOrDefault();
                        registrationTranMaster.StatusDateTime = DateTime.UtcNow;
                        registrationTranMaster.Remarks = null;
                        registrationMaster.IsDeleted = false;
                        registrationTranMaster.CreatedUTC = DateTime.UtcNow;
                        registrationTranMaster.UpdatedUTC = DateTime.UtcNow;
                        registrationTranMaster.CreatedBy = AddReg.RegistrationTranMaster.CreatedBy;
                        registrationTranMaster.UpdatedBy = AddReg.RegistrationTranMaster.CreatedBy;
                        registrationTranMaster.DoctorID = 0;
                        registrationTranMaster.ReviewCount = ReviewCount + 1;
                        registrationTranMaster.RegistrationFees = Convert.ToDecimal(0);
                        registrationTranMaster.ConsulationFees = AddReg.RegistrationTranMaster.RegistrationFees;
                        registrationTranMaster.PatientVisitType = onl.Where(x => x.ParentTag == "TOV" && x.ParentDescription == "Review").Select(x => Convert.ToString(x.OLMID)).FirstOrDefault();
                        WYNKContext.RegistrationTran.Add(registrationTranMaster);

                        string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == AddReg.RegistrationTranMaster.CreatedBy).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                        string userid1 = Convert.ToString(AddReg.RegistrationTranMaster.CreatedBy);
                        ErrorLog oErrorLogs = new ErrorLog();
                        oErrorLogs.WriteErrorLogTitle(AddReg.Companyname, "Registration", "User name :", username, "User ID :", userid1, "Mode : AddReview");


                        object names = registrationTranMaster;
                        oErrorLogs.WriteErrorLogArray("RegistrationTran", names);
                        foreach (var item in AddReg.PaymentMaster.ToList())
                        {
                            var Payment = new Payment_Master();
                            Payment.UIN = registrationMaster.UIN;
                            Payment.PaymentType = "O";
                            Payment.PaymentMode = item.PaymentMode;
                            Payment.InstrumentNumber = item.InstrumentNumber;
                            Payment.Instrumentdate = item.Instrumentdate;
                            Payment.BankName = item.BankName;
                            Payment.BankBranch = item.BankBranch;
                            Payment.Expirydate = item.Expirydate;
                            Payment.Amount = item.Amount;
                            Payment.CmpID = Cmpid;
                            Payment.IsBilled = true;
                            Payment.ReceiptNumber = AddReg.ContraTransactionid;
                            Payment.ReceiptDate = DateTime.UtcNow;
                            Payment.TransactionID = ContraTransactionid;
                            Payment.Fyear = Convert.ToString(WYNKContext.FinancialYear.Where(x => x.ID == WYNKContext.FinancialYear.Where(b => Convert.ToDateTime(b.FYFrom) <= DateTime.Now && Convert.ToDateTime(b.FYTo) >=
                            DateTime.Now && x.CMPID == Cmpid && x.IsActive == true).Select(f => f.ID).FirstOrDefault()).Select(c => c.FYAccYear).FirstOrDefault());
                            Payment.CreatedUTC = DateTime.UtcNow;
                            Payment.CreatedBy = AddReg.RegistrationTranMaster.CreatedBy;
                            WYNKContext.PaymentMaster.Add(Payment);
                            ErrorLog oErrorLogstran = new ErrorLog();
                            object namestr = Payment;
                            oErrorLogstran.WriteErrorLogArray("PaymentMaster", namestr);
                            WYNKContext.SaveChanges();
                        }

                        var datereglocaltime = DateTime.Now;
                        var Date1 = WYNKContext.PatientFootfall.Where(x => x.Date.Date == datereglocaltime.Date).Select(x => x.Date.Date).FirstOrDefault();
                        var Age = PasswordEncodeandDecode.ToAgeString(registrationMaster.DateofBirth);
                        var result = Regex.Match(Age, @"\d+").Value;
                        var PodiatristAge = CMPSContext.Setup.Where(x => x.CMPID == Cmpid).Select(x => x.Pediatric).FirstOrDefault();
                        var results = WYNKContext.PatientFootfall.Where(x => x.Date.Date == datereglocaltime.Date).ToList();
                        var art = WYNKContext.RegistrationExtension.Where(x => x.UIN == UIN && x.CMPID == Cmpid).OrderByDescending(x => x.CreatedUTC).Select(x => x.Artificialeye).FirstOrDefault();
                        foreach (var Arrays1 in results)
                        {
                            if (Arrays1.CmpID == Cmpid)
                            {
                                AddReg.Cmpid = Cmpid;
                            }

                        }
                        if (datereglocaltime.Date == Date1.Date && Cmpid == AddReg.Cmpid)
                        {
                            ManagementMaster = WYNKContext.PatientFootfall.Where(x => x.Date.Date == Date1.Date && x.CmpID == Cmpid).FirstOrDefault();
                            ManagementMaster.ReviewPatients += 1;

                            if (Convert.ToInt32(PodiatristAge) > Convert.ToInt32(result))
                            {
                                if (art == true)
                                {
                                    ManagementMaster.ReviewPediatricOcular += 1;
                                    ManagementMaster.ReviewPediatricNormal += 0;
                                }
                                else
                                {
                                    ManagementMaster.ReviewPediatricNormal += 1;
                                    ManagementMaster.ReviewPediatricOcular += 0;
                                }
                                ManagementMaster.ReviewGeneralNormal += 0;
                                ManagementMaster.ReviewGeneralOcular += 0;
                            }
                            else
                            {
                                if (art == true)
                                {
                                    ManagementMaster.ReviewGeneralOcular += 1;
                                    ManagementMaster.ReviewGeneralNormal += 0;
                                }
                                else
                                {
                                    ManagementMaster.ReviewGeneralNormal += 1;
                                    ManagementMaster.ReviewGeneralOcular += 0;
                                }
                                ManagementMaster.ReviewPediatricOcular += 0;
                                ManagementMaster.ReviewPediatricNormal += 0;
                            }

                            if (AddReg.RegistrationTranMaster.RegistrationFees != null)
                            {
                                ManagementMaster.ConsultationReview += AddReg.RegistrationTranMaster.RegistrationFees;
                            }
                            else
                            {
                                ManagementMaster.ConsultationReview += 0;
                            }

                            ManagementMaster.UpdatedBy = AddReg.RegistrationTranMaster.CreatedBy;
                            WYNKContext.Entry(ManagementMaster).State = EntityState.Modified;
                            ErrorLog oErrorLogstranPT = new ErrorLog();
                            object namestrPT = ManagementMaster;
                            oErrorLogstranPT.WriteErrorLogArray("PatientFootfall", namestrPT);

                        }
                        else
                        {
                            ManagementMaster.ReviewPatients = +1;

                            if (Convert.ToInt32(PodiatristAge) > Convert.ToInt32(result))
                            {
                                if (art == true)
                                {
                                    ManagementMaster.ReviewPediatricOcular = +1;
                                    ManagementMaster.ReviewPediatricNormal = +0;
                                }
                                else
                                {
                                    ManagementMaster.ReviewPediatricNormal = +1;
                                    ManagementMaster.ReviewPediatricOcular = +0;
                                }
                            }
                            else
                            {
                                if (art == true)
                                {
                                    ManagementMaster.ReviewGeneralOcular = +1;
                                    ManagementMaster.ReviewGeneralNormal = +0;
                                }
                                else
                                {
                                    ManagementMaster.ReviewGeneralNormal = +1;
                                    ManagementMaster.ReviewGeneralOcular = +0;
                                }
                            }
                            if (AddReg.RegistrationTranMaster.RegistrationFees != null)
                            {
                                ManagementMaster.ConsultationReview = +AddReg.RegistrationTranMaster.RegistrationFees;
                            }
                            else
                            {
                                ManagementMaster.ConsultationReview = +0;
                            }

                            ManagementMaster.NewGeneralNormal = +0;
                            ManagementMaster.NewGeneralOcular = +0;
                            ManagementMaster.NewPediatricNormal = +0;
                            ManagementMaster.NewPediatricOcular = +0;
                            ManagementMaster.ConsultationNew = +0;
                            ManagementMaster.Date = Convert.ToDateTime(datereglocaltime.ToString("yyyy-MM-dd"));
                            ManagementMaster.CreatedBy = AddReg.RegistrationTranMaster.CreatedBy;
                            ManagementMaster.CmpID = Cmpid;
                            WYNKContext.PatientFootfall.Add(ManagementMaster);
                            ErrorLog oErrorLogstranPT = new ErrorLog();
                            object namestrPT = ManagementMaster;
                            oErrorLogstranPT.WriteErrorLogArray("PatientFootfall", namestrPT);
                        }
                        WYNKContext.SaveChanges();

                        if (ConsultationFee != 0 && ConsultationFee != null)
                        {
                            var commonRepos = new CommonRepository(_Wynkcontext, _Cmpscontext);
                            var RunningNumber = commonRepos.GenerateRunningCtrlNoo(ContraTransactionid, Cmpid, "GetRunningNo");
                            if (RunningNumber == AddReg.ContraTransactionid)
                            {
                                commonRepos.GenerateRunningCtrlNoo(ContraTransactionid, Cmpid, "UpdateRunningNo");
                            }
                            else
                            {
                                var GetRunningNumber1 = commonRepos.GenerateRunningCtrlNoo(ContraTransactionid, Cmpid, "UpdateRunningNo");

                                Payment1 = WYNKContext.PaymentMaster.Where(x => x.ReceiptNumber == AddReg.ContraTransactionid).FirstOrDefault();
                                Payment1.ReceiptNumber = GetRunningNumber1;
                                WYNKContext.Entry(Payment1).State = EntityState.Modified;
                                WYNKContext.SaveChanges();
                            }
                        }

                        var RegTranID = registrationTranMaster.RegistrationTranID;
                        var ccmpid = Cmpid;
                        var OLMID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Assigned").Select(x => x.OLMID).FirstOrDefault();
                        var Referncetag = CMPSContext.Users.Where(x => x.Userid == userid).Select(x => x.ReferenceTag).FirstOrDefault();
                        var utctime = CMPSContext.Setup.Where(x => x.CMPID == ccmpid).Select(x => x.UTCTime).FirstOrDefault();
                        TimeSpan ts = TimeSpan.Parse(utctime);
                        if (Referncetag == "D" || Referncetag == "D")
                        {
                            var emailid = CMPSContext.Users.Where(x => x.Userid == userid).Select(x => x.Username).FirstOrDefault();
                            var docid = CMPSContext.DoctorMaster.Where(x => x.EmailID == emailid).Select(x => x.DoctorID).FirstOrDefault();

                            var Assigningtable = new Patient_Assign();
                            Assigningtable.RegistrationTranID = RegTranID;
                            Assigningtable.AssignedDate = DateTime.UtcNow + ts;
                            Assigningtable.DoctorID = docid;
                            Assigningtable.IsCancelled = false;
                            Assigningtable.CreatedUTC = DateTime.UtcNow;
                            Assigningtable.CreatedBy = userid;
                            Assigningtable.Patientallocatestatus = null;
                            var idd = WYNKContext.PatientAssign.Select(x => x.ID).LastOrDefault();
                            if (idd < 1)
                            {
                                Assigningtable.ID = 1;
                            }
                            else
                            {
                                Assigningtable.ID = idd + 1;
                            }
                            WYNKContext.PatientAssign.Add(Assigningtable);
                            var Regtrandetails = WYNKContext.RegistrationTran.Where(x => x.RegistrationTranID == RegTranID).FirstOrDefault();
                            Regtrandetails.Status = OLMID;
                            Regtrandetails.DoctorID = docid;
                            Regtrandetails.AssignedDate = DateTime.UtcNow + ts;
                            WYNKContext.RegistrationTran.UpdateRange(Regtrandetails);
                            WYNKContext.SaveChanges();
                        }
                        else if (Referncetag == "R" || Referncetag == "A")
                        {
                            var Assigningtable = new Patient_Assign();
                            Assigningtable.RegistrationTranID = RegTranID;
                            Assigningtable.AssignedDate = DateTime.UtcNow + ts;
                            Assigningtable.DoctorID = 0;
                            Assigningtable.IsCancelled = false;
                            Assigningtable.CreatedUTC = DateTime.UtcNow;
                            Assigningtable.CreatedBy = userid;
                            Assigningtable.Patientallocatestatus = "D";
                            var idd = WYNKContext.PatientAssign.Select(x => x.ID).LastOrDefault();
                            if (idd < 1)
                            {
                                Assigningtable.ID = 1;
                            }
                            else
                            {
                                Assigningtable.ID = idd + 1;
                            }
                            WYNKContext.PatientAssign.Add(Assigningtable);
                            WYNKContext.SaveChanges();

                            var Assigningtables = new Patient_Assign();
                            Assigningtables.RegistrationTranID = RegTranID;
                            Assigningtables.AssignedDate = DateTime.UtcNow + ts;
                            Assigningtables.DoctorID = 0;
                            Assigningtables.IsCancelled = false;
                            Assigningtables.CreatedUTC = DateTime.UtcNow;
                            Assigningtables.CreatedBy = userid;
                            Assigningtables.Patientallocatestatus = "O";
                            var idds = WYNKContext.PatientAssign.Select(x => x.ID).LastOrDefault();
                            if (idd < 1)
                            {
                                Assigningtables.ID = 1;
                            }
                            else
                            {
                                Assigningtables.ID = idds + 1;
                            }
                            WYNKContext.PatientAssign.Add(Assigningtables);
                            WYNKContext.SaveChanges();
                        }

                    }

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
                            uinss = registrationMaster.UIN,
                            namep = registrationMaster.Name + ' ' + registrationMaster.MiddleName + ' ' + registrationMaster.LastName,
                            FName = registrationMaster.FatherHusbandName,
                            age = PasswordEncodeandDecode.ToAgeString(registrationMaster.DateofBirth),
                            gender = registrationMaster.Gender,
                            dateofregistration = registrationMaster.DateofRegistration,
                            RegistrationTranID = registrationTranMaster.RegistrationTranID,
                            phone = registrationMaster.Phone,
                            Address1 = registrationMaster.Address1,
                            email = registrationMaster.EmailID,
                            ConsulationFee = AddReg.RegistrationTranMaster.RegistrationFees,
                            PAddress = CMPSContext.Company.Where(x => x.CmpID == Cmpid).Select(x => x.Address1).FirstOrDefault(),
                            PAddress2 = CMPSContext.Company.Where(x => x.CmpID == Cmpid).Select(x => x.Address2).FirstOrDefault(),
                            PAddress3 = CMPSContext.Company.Where(x => x.CmpID == Cmpid).Select(x => x.Address3).FirstOrDefault(),
                            Pphone = CMPSContext.Company.Where(x => x.CmpID == Cmpid).Select(x => x.Phone1).FirstOrDefault(),
                            Pweb = CMPSContext.Company.Where(x => x.CmpID == Cmpid).Select(x => x.Website).FirstOrDefault(),
                            PCompnayname = CMPSContext.Company.Where(x => x.CmpID == Cmpid).Select(x => x.CompanyName).FirstOrDefault(),
                            RegisteredBy = CMPSContext.Users.Where(x => x.Userid == userid).Select(x => x.Username).FirstOrDefault(),
                            ReceiptNumber = AddReg.ContraTransactionid,
                            ReceiptDate = DateTime.UtcNow,
                            ReferenceTag = CMPSContext.Users.Where(x => x.Userid == userid).Select(x => x.ReferenceTag).FirstOrDefault(),
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




        public dynamic UpdateConBilling(RegistrationMasterViewModel UpdateConBilling, int cmpPid, int DRID, int regtranid, int TransactionTypeid)
        {

            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    if (UpdateConBilling.PaymentMaster.Count() > 0)
                    {
                        foreach (var item in UpdateConBilling.PaymentMaster.ToList())
                        {
                            var pm = new Payment_Master();
                            pm.UIN = UpdateConBilling.UIN;
                            pm.PaymentType = "O";
                            pm.PaymentMode = item.PaymentMode;
                            pm.InstrumentNumber = item.InstrumentNumber;
                            pm.Instrumentdate = item.Instrumentdate;
                            pm.BankName = item.BankName;
                            pm.BankBranch = item.BankBranch;
                            pm.Expirydate = item.Expirydate;
                            pm.Amount = item.Amount;
                            pm.IsBilled = true;
                            var Datee = DateTime.Now;
                            pm.Fyear = Convert.ToString(WYNKContext.FinancialYear.Where(s => s.ID == WYNKContext.FinancialYear.Where(x => Convert.ToDateTime(x.FYFrom) <= Datee && Convert.ToDateTime(x.FYTo) >= Datee && x.CMPID == cmpPid && x.IsActive == true).Select(x => x.ID).FirstOrDefault()).Select(s => s.FYAccYear).FirstOrDefault());
                            pm.PaymentReferenceID = item.PaymentReferenceID;
                            pm.InVoiceNumber = UpdateConBilling.InVoiceNumber;
                            pm.InVoiceDate = DateTime.Now.Date;
                            pm.ReceiptNumber = UpdateConBilling.InVoiceNumber;
                            pm.ReceiptDate = DateTime.Now.Date;
                            pm.CreatedUTC = DateTime.UtcNow;
                            pm.CreatedBy = item.CreatedBy;
                            pm.TransactionID = TransactionTypeid;
                            pm.CmpID = cmpPid;
                            pm.IsCancelled = false;
                            WYNKContext.PaymentMaster.AddRange(pm);
                            ErrorLog oErrorLogspay = new ErrorLog();
                            object namepay = pm;
                            oErrorLogspay.WriteErrorLogArray("PaymentMaster", namepay);
                            WYNKContext.SaveChanges();
                        }

                    }


                    var master = WYNKContext.RegistrationTran.Where(x => x.RegistrationTranID == regtranid).FirstOrDefault();
                    if (master != null)
                    {
                        master.ConsulationFees = UpdateConBilling.PTotalAmountcon1;
                        WYNKContext.Entry(master).State = EntityState.Modified;
                        WYNKContext.SaveChanges();

                    }

                    if (UpdateConBilling.PTotalAmountcon1 != 0)
                    {

                        var conid = (from CS in WYNKContext.ConsultantionSummary.Where(x => x.DoctorID == DRID && x.CmpID == cmpPid)
                                     select new
                                     {
                                         ret = CS.DoctorID,

                                     }).ToList();

                        var pid = CMPSContext.Company.Where(x => x.CmpID == cmpPid).Select(x => x.ParentID).FirstOrDefault();

                        var consum = new ConsultantionSummary();
                        if (conid.Count == 0)
                        {

                            consum.DoctorID = DRID;
                            consum.CmpID = cmpPid;
                            consum.ParentID = pid;
                            consum.Date = DateTime.UtcNow;
                            consum.ConsultationCharges = UpdateConBilling.PTotalAmountcon1;
                            consum.CreatedUTC = DateTime.UtcNow;
                            consum.CreatedBy = Convert.ToInt32(UpdateConBilling.userroleID);
                            WYNKContext.ConsultantionSummary.AddRange(consum);
                            ErrorLog oErrorconsum = new ErrorLog();
                            object naminveconsum = consum;
                            oErrorconsum.WriteErrorLogArray("ConsultantionSummary", naminveconsum);
                            WYNKContext.SaveChanges();

                        }

                        else
                        {

                            var masterss = WYNKContext.ConsultantionSummary.Where(x => x.DoctorID == DRID && x.CmpID == cmpPid).LastOrDefault();
                            masterss.Date = DateTime.UtcNow;
                            masterss.ConsultationCharges += UpdateConBilling.PTotalAmountcon1;
                            WYNKContext.ConsultantionSummary.UpdateRange(masterss);
                            ErrorLog oErrormasterss = new ErrorLog();
                            object naminvemasterss = masterss;
                            oErrormasterss.WriteErrorLogArray("ConsultationSummary", naminvemasterss);

                        }


                        var re = WYNKContext.Findings.Where(x => x.RegistrationTranID == regtranid).ToList();

                        var revid = (from RS in WYNKContext.RevenueSummary.Where(x => x.DoctorID == DRID && x.CmpID == cmpPid && x.Date.Date == DateTime.UtcNow.Date && x.SpecialityDesc == "Consultation Fee")
                                     select new
                                     {
                                         ret = RS.DoctorID,

                                     }).ToList();
                        var revenue = new RevenueSummary();
                        if (re.Count == 1)
                        {
                            if (revid.Count == 0)
                            {

                                revenue.DoctorID = DRID;
                                revenue.CmpID = cmpPid;
                                revenue.SpecialityDesc = "Consultation Fee";
                                revenue.Date = DateTime.UtcNow.Date;
                                revenue.Numbers = 1;
                                revenue.Amount = UpdateConBilling.PTotalAmountcon1;
                                revenue.CreatedUTC = DateTime.UtcNow;
                                revenue.CreatedBy = Convert.ToInt32(UpdateConBilling.userroleID);
                                WYNKContext.RevenueSummary.AddRange(revenue);
                                WYNKContext.SaveChanges();
                            }

                            else
                            {
                                var masterre = WYNKContext.RevenueSummary.Where(x => x.DoctorID == DRID && x.CmpID == cmpPid && x.Date.Date == DateTime.Now.Date && x.SpecialityDesc == "Consultation Fee").LastOrDefault();
                                masterre.Numbers += 1;
                                masterre.Amount += UpdateConBilling.PTotalAmountcon1;
                                masterre.UpdatedUTC = DateTime.UtcNow;
                                masterre.UpdatedBy = Convert.ToInt32(UpdateConBilling.userroleID);
                                WYNKContext.RevenueSummary.UpdateRange(masterre);
                                WYNKContext.SaveChanges();
                            }
                        }
                    }

                    WYNKContext.SaveChanges();
                    dbContextTransaction.Commit();

                    var commonRepos = new CommonRepository(_Wynkcontext, _Cmpscontext);
                    var RunningNumber = commonRepos.GenerateRunningCtrlNoo(TransactionTypeid, cmpPid, "GetRunningNo");

                    if (RunningNumber == UpdateConBilling.InVoiceNumber)
                    {
                        commonRepos.GenerateRunningCtrlNoo(TransactionTypeid, cmpPid, "UpdateRunningNo");
                    }
                    else
                    {
                        var GetRunningNumber = commonRepos.GenerateRunningCtrlNoo(TransactionTypeid, cmpPid, "UpdateRunningNo");

                        var ib = WYNKContext.PaymentMaster.Where(x => x.InVoiceNumber == UpdateConBilling.InVoiceNumber).FirstOrDefault();
                        ib.InVoiceNumber = GetRunningNumber;
                        WYNKContext.PaymentMaster.UpdateRange(ib);

                        WYNKContext.SaveChanges();
                    }



                    if (WYNKContext.SaveChanges() >= 0)
                    {
                        ErrorLog oErrorLog = new ErrorLog();
                        oErrorLog.WriteErrorLog("Information :", "Saved Successfully");
                    }
                    return new
                    {

                        Success = true,
                        Message = CommonMessage.saved,
                        bill = UpdateConBilling.InVoiceNumber,
                    };
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                    ErrorLog oErrorLog = new ErrorLog();
                    oErrorLog.WriteErrorLog("Error :", ex.InnerException.Message.ToString());
                    Console.Write(ex);
                }

            }
            return new
            {
                Success = false,
                Message = CommonMessage.Missing,
            };

        }

        public dynamic Getpaymentdetailsbasedontrainid(int CMPID, int Regtranid, string uin)
        {
            var data = new Consultaionpaymentdetails();
            data.Consultationpaymentdetails = new List<Consultationpaymentdetails>();
            var paymentdetaiuls = WYNKContext.PaymentMaster.Where(x => x.UIN == uin && x.IsCancelled == false).ToList();
            foreach (var secitem in paymentdetaiuls)
            {
                var paaydet = new Consultationpaymentdetails();
                paaydet.ID = secitem.ID;
                paaydet.PaymentMode = secitem.PaymentMode;
                paaydet.PaymentType = secitem.PaymentType;
                paaydet.InstrumentNumber = secitem.InstrumentNumber;
                paaydet.Instrumentdate = secitem.Instrumentdate;
                paaydet.Expirydate = secitem.Expirydate;
                paaydet.BankName = secitem.BankName;
                paaydet.BankBranch = secitem.BankBranch;
                paaydet.Amount = secitem.Amount;
                data.Consultationpaymentdetails.Add(paaydet);
            }

            return data;
        }
        public dynamic Deletedatabasedondate(int CMPID, int id, string userid)
        {
            try
            {
                var data = WYNKContext.PaymentMaster.Where(x => x.ID == id && x.CmpID == CMPID).FirstOrDefault();
                data.IsCancelled = true;
                data.UpdatedBy = Convert.ToInt32(userid);
                data.UpdatedUTC = DateTime.UtcNow;
                WYNKContext.PaymentMaster.Update(data);
                WYNKContext.SaveChanges();

                if (WYNKContext.SaveChanges() >= 0)
                {
                    ErrorLog oErrorLog = new ErrorLog();
                    oErrorLog.WriteErrorLog("Information :", "Deleted Successfully");
                }
                return new
                {
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                ErrorLog oErrorLog = new ErrorLog();
                oErrorLog.WriteErrorLog("Consultation Error :", ex.InnerException.Message.ToString());
            }
            return new
            {
                Success = false
            };
        }
        public dynamic UpdateConsultationamount(COnsultationview Reaster, string UIN, int Cmpid, int ContraTransactionid, int userid, decimal ConsultationFee, int regtranid)
        {
            var data = new COnsultationview();
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    var rtid = WYNKContext.RegistrationTran.Where(x => x.RegistrationTranID == regtranid).FirstOrDefault();                    
                    if(rtid.ConsulationFees != ConsultationFee)
                    {
                        rtid.ConsulationFees = ConsultationFee;
                        WYNKContext.RegistrationTran.Update(rtid);
                        WYNKContext.SaveChanges();
                    }

                    var trid = WYNKContext.PaymentMaster.Where(x => x.UIN == UIN).FirstOrDefault();
                    WYNKContext.PaymentMaster.RemoveRange(WYNKContext.PaymentMaster.Where(x => x.UIN == UIN).ToList());
                    WYNKContext.SaveChanges();
                    foreach (var item in Reaster.ConsultationPaymentMastermas.ToList())
                    {
                        var payd = new Payment_Master();
                        payd.UIN = UIN;
                        payd.PaymentType = "O";
                        payd.PaymentMode = item.PaymentMode;
                        payd.InstrumentNumber = item.InstrumentNumber;
                      
                        if (item.Instrumentdate != null)
                        {
                            var dddts = Convert.ToDateTime(item.Instrumentdate).ToString("dd-MMM-yyyy");
                            DateTime DT;
                            var appdate = DateTime.TryParseExact(dddts.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DT);
                            {
                                DT.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                            }
                            payd.Instrumentdate = DT;
                        }
                        if (item.Expirydate != null)
                        {
                            var dddt = Convert.ToDateTime(item.Expirydate).ToString("dd-MMM-yyyy");
                            DateTime EDT;
                            var appsdate = DateTime.TryParseExact(dddt.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out EDT);
                            {
                                EDT.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                            }
                            payd.Expirydate = EDT;
                        }


                        payd.BankName = item.BankName;
                        payd.BankBranch = item.BankBranch;
                        payd.Amount = item.Amount;
                        payd.IsBilled = true;
                        payd.InVoiceNumber = trid.InVoiceNumber;
                        payd.InVoiceDate = DateTime.UtcNow;
                        payd.ReceiptNumber = trid.ReceiptNumber;
                        payd.ReceiptDate = DateTime.UtcNow;
                        payd.CreatedUTC = DateTime.UtcNow;
                        payd.TransactionID = trid.TransactionID;
                        payd.Fyear = trid.Fyear;
                        payd.CmpID = Cmpid;
                        payd.IsCancelled = false;
                        WYNKContext.PaymentMaster.AddRange(payd);

                    }
                    WYNKContext.SaveChanges();

                    if (WYNKContext.SaveChanges() >= 0)
                    {
                        ErrorLog oErrorLog = new ErrorLog();
                        oErrorLog.WriteErrorLog("Information :", "Consultation fee updated Successfully");
                        dbContextTransaction.Commit();
                    }
                    return new
                    {
                        Success = true,
                    };
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    ErrorLog oErrorLog = new ErrorLog();
                    oErrorLog.WriteErrorLog("Consultation Error :", ex.InnerException.Message.ToString());
                }
                return new
                {
                    Success = false
                };
            }
        }

    }
}













