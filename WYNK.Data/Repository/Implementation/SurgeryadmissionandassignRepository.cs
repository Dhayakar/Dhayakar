using Microsoft.EntityFrameworkCore;
using CommonMailService.cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WYNK.Data.Common;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Helpers;
using System.Globalization;

//Test

namespace WYNK.Data.Repository.Implementation
{
    class SurgeryadmissionandassignRepository : RepositoryBase<Surgeryadmissionandassign>, ISurgeryadmissionandassignRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;

        public SurgeryadmissionandassignRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public dynamic GetDoctDetails(int cmpid)
        {
            var det = new Surgeryadmissionandassign();
            det.Getdoctorrole = new List<Getdoctorrole>();


            var docmas = CMPSContext.DoctorMaster.ToList();
            var docspec = CMPSContext.DoctorSpeciality.ToList();
            var onelinemaster = CMPSContext.OneLineMaster.ToList();

            det.Getdoctorrole = (from OLM in onelinemaster
                                 join DS in docspec on OLM.OLMID equals DS.OLMID
                                 join DM in docmas on DS.DoctorID equals DM.DoctorID
                                 where (DM.IsDeleted == false && DM.IsActive == true && OLM.ParentDescription != "Optometrist" && OLM.ParentDescription != "Vision care" && DM.CMPID == cmpid)

                                 select new Getdoctorrole
                                 {

                                     Name = DM.Firstname + " " + DM.MiddleName + " " + DM.LastName,
                                     Role = OLM.ParentDescription,
                                     ID = DM.DoctorID,

                                 }).ToList();


            return det;
        }
        public dynamic Getpatient(int cmpid, string Time)
        {
            var details = new Surgeryadmissionandassign();
            details.patientdetails = new List<patientdetails>();



            var find = WYNKContext.Findings.ToList();
            var findext = WYNKContext.FindingsExt.ToList();
            var one = CMPSContext.OneLineMaster.ToList();
            var reg = WYNKContext.Registration.ToList();
            var user = CMPSContext.Users.ToList();
            var docmas = CMPSContext.DoctorMaster.ToList();
            var icdspec = WYNKContext.ICDSpecialityCode.ToList();
            TimeSpan ts = TimeSpan.Parse(Time);



            var findinglist = (from f in find.Where(x => x.CmpID == cmpid).OrderByDescending(x => x.CreatedUTC)
                               join fex in findext on
                               f.RandomUniqueID equals fex.FindingsID
                               where fex.SurgeryComplete == false && fex.Status != "1"

                               select new
                               {

                                   uin = f.UIN,
                                   Name = reg.Where(x => x.UIN == f.UIN).Select(x => x.Name + " " + x.MiddleName + " " + x.LastName).FirstOrDefault(),
                                   Age = PasswordEncodeandDecode.ToAgeString(reg.Where(x => x.UIN == f.UIN).Select(x => x.DateofBirth).FirstOrDefault()),
                                   Gender = reg.Where(x => x.UIN == f.UIN).Select(x => x.Gender).FirstOrDefault(),
                                   SurgeryAdvName = docmas.Where(e => e.EmailID == user.Where(x => x.Userid == fex.CreatedBy).Select(x => x.Username).FirstOrDefault()).Select(s => s.Firstname + " " + s.MiddleName + " " + s.LastName).FirstOrDefault(),
                                   SurgeryAdvDate = fex.CreatedUTC.Add(ts),
                                   SurgeryType = icdspec.Where(x => x.ID == fex.ICDSpecialityid).Select(x => x.SpecialityDescription).FirstOrDefault(),
                                   treatment = f.TreatmentAdvice,
                                   findingid = fex.FindingsID,
                                   status = fex.Status,
                                   Rid = f.RegistrationTranID,
                               }).ToList();




            foreach (var itm in findinglist.ToList())
            {
                var patientdetails = new patientdetails();

                patientdetails.UIN = itm.uin;
                patientdetails.Name = itm.Name;
                patientdetails.Age = itm.Age;
                patientdetails.Gender = itm.Gender;
                patientdetails.SurgeryAdvName = itm.SurgeryAdvName;
                patientdetails.SurgeryAdvDate = itm.SurgeryAdvDate;
                patientdetails.SurgeryType = itm.SurgeryType;
                patientdetails.TreatmentAdvice = itm.treatment;
                patientdetails.findingsID = itm.findingid;
                patientdetails.status = itm.status;
                patientdetails.RegID = itm.Rid;
                details.patientdetails.Add(patientdetails);



            }

            return details;
        }
        public dynamic Getdemography(string UIN, int RegID, string Time)

        {

            var patientdetail = new Surgeryadmissionandassign();
            patientdetail.Counsel = new List<Counsel>();
            patientdetail.SurgeryType = new List<SurgeryType>();
            TimeSpan ts = TimeSpan.Parse(Time);
            var cmpid = WYNKContext.RegistrationTran.Where(x => x.RegistrationTranID == RegID).Select(x => x.CmpID).FirstOrDefault();
            var docmas = CMPSContext.DoctorMaster.ToList();
            var couns = WYNKContext.Counselling.ToList();
            var icdspcode = WYNKContext.ICDSpecialityCode.ToList();
            var icdmas = WYNKContext.ICDMaster.Where(x => x.SpecialityCode != null && x.IsIOLReqd != null).ToList();
            var findings = WYNKContext.Findings.ToList();
            var FindingsExt = WYNKContext.FindingsExt.ToList();
            var PatientAssign = WYNKContext.PatientAssign.ToList();
            var patientinsurance = WYNKContext.PatientVsInsurance.Where(x => x.UIN == UIN && x.CmpID == cmpid).ToList();

            patientdetail.PolicyDetails = (from pc in patientinsurance
                                           join md in WYNKContext.InsuranceVsMiddlemen on pc.InsurancevsMiddlemenID equals md.ID
                                           join ins in WYNKContext.Insurance.Where(x => x.IsActive == true && x.CMPID == cmpid) on md.IID equals ins.ID
                                           select new PolicyDetails
                                           {
                                               Insurancemiddlemanid = md.ID,
                                               plocyid = ins.ID,
                                               InsuranceName = ins.Name,
                                               PolicyName = pc.PolicyName,
                                               PolicyNumber = pc.PolicyNo,
                                               Periodfrom = pc.PeriodFrom,
                                               periodto = pc.PeriodTo,
                                               sumassured = pc.SumAssured,
                                           }).ToList();

            var counsel = (from c in couns.Where(u => u.UIN == UIN)

                           select new
                           {
                               CreateduTC = c.CreatedUTC.Add(ts),
                               Otherrequirements = c.OtherRequirements,
                               CounsellingAdvName = docmas.Where(x => x.CreatedBy == c.CreatedBy).Select(s => s.Firstname + " " + s.MiddleName + " " + s.LastName).FirstOrDefault(),
                               Surgerydate = c.SurgeryDate,
                               CID = c.ID,
                           }).LastOrDefault();

            if (counsel != null)
            {
                var a = new Counsel();

                a.CreatedUTC = counsel.CreateduTC;
                a.OtherRequirements = counsel.Otherrequirements;
                a.CounsellingAdvName = counsel.CounsellingAdvName;
                a.CID = counsel.CID;
                a.SurgeryDate = counsel.Surgerydate;
                patientdetail.Counsel.Add(a);

            }
            else
            {
                patientdetail.Counsel = new List<Counsel>();
            }


            patientdetail.SurgeryType = (from f in findings.Where(u => u.UIN == UIN && u.RegistrationTranID == RegID)
                                         join fex in FindingsExt on
                                         f.RandomUniqueID equals fex.FindingsID
                                         where fex.SurgeryComplete == false
                                         select new SurgeryType
                                         {
                                             SurgeryTyp = fex.ICDSpecialityid,
                                             SurgeryDescriptio = fex.ICDCode,
                                             SurgeryTypname = icdspcode.Where(x => x.ID == fex.ICDSpecialityid).Select(x => x.SpecialityDescription).FirstOrDefault(),
                                             SurgeryDescriptionname = icdmas.Where(x => x.ICDCODE == fex.ICDCode).Select(x => x.ICDDESCRIPTION).FirstOrDefault(),
                                             IsOD = fex.IsOD,
                                             IsOS = fex.IsOS,
                                             IsOU = fex.IsOU,
                                             CreateUtc = fex.CreatedUTC.Add(ts),
                                             Doctorname = docmas.Where(x => x.DoctorID == PatientAssign.Where(a => a.RegistrationTranID == f.RegistrationTranID).Select(q => q.DoctorID).FirstOrDefault()).Select(x => x.Firstname + "" + x.MiddleName + "" + x.LastName).FirstOrDefault(),
                                         }).ToList();




            return patientdetail;
        }
        public dynamic Getroom(Surgeryadmissionandassign sur, string roomid, int cmpid)

        {
            var room = new Surgeryadmissionandassign();
            room.Roomdetemr = new List<Roomdetemr>();
            room.Roomoccuiped = new List<Roomoccuiped>();

            var SurgeryCostDetail = WYNKContext.SurgeryCostDetail.ToList();
            var OneLineMaster = CMPSContext.OneLineMaster.ToList();
            var Room = WYNKContext.Room.ToList();
            var RoomDetailsExtension = WYNKContext.RoomDetailsExtension.ToList();
            var RoomDetails = WYNKContext.RoomDetails.ToList();

            if (sur.Surgery_Tran.Count() > 0)
            {
                var TID = sur.Surgery_Tran.ToList();

                foreach (var item in TID.ToList())
                {

                    var Surgerydetails = item.ICDCode;

                    if (Surgerydetails != null)
                    {
                        var roomdetemr = (from su in SurgeryCostDetail.Where(u => u.ICDCode == Surgerydetails && u.RoomType == roomid && u.CMPID == cmpid)

                                          select new
                                          {
                                              surgerycost = su.SURGERYCOST,
                                              packagerate = su.PackageRate,
                                              dressingcharge = su.DressingCharge,
                                              medicationcharge = su.MedicationCharge,
                                              surgeoncharge = su.SurgeonCharge,
                                              roomtype = su.RoomType,

                                          }).ToList();

                        foreach (var itm in roomdetemr.ToList())
                        {

                            var Roomdetemr = new Roomdetemr();
                            Roomdetemr.SURGERYCOST = itm.surgerycost;
                            Roomdetemr.PackageRate = itm.packagerate;
                            Roomdetemr.DressingCharge = itm.dressingcharge;
                            Roomdetemr.MedicationCharge = itm.medicationcharge;
                            Roomdetemr.SurgeonCharge = itm.surgeoncharge;
                            Roomdetemr.RoomType = OneLineMaster.Where(x => x.OLMID == Convert.ToInt32(itm.roomtype)).Select(x => x.ParentDescription).FirstOrDefault();
                            room.Roomdetemr.Add(Roomdetemr);

                        }

                    }


                }


            }

            var Roominfo = (from r in Room.Where(u => u.RoomType == roomid && u.CMPID == cmpid)
                            join fex in RoomDetails on
                            r.ID equals fex.RoomID
                            where fex.IsActive == true
                            select new
                            {
                                roomtype = r.RoomType,
                                roomno = fex.RoomNumber,
                                bedno = fex.BedNo,
                                id = fex.ID,
                                roomid = fex.RoomID,
                                roomcost = r.RoomCost,
                                restroomtype = RoomDetailsExtension.Where(x => x.RoomDetailsID == fex.ID).Select(x => x.RestRoomType).ToList(),
                            }).ToList();

            var filterdata = Roominfo.Where(x => x.restroomtype.Count() > 0).ToList();


            if (Roominfo.Count() > 0)
            {
                foreach (var itm in filterdata.ToList())
                {
                    foreach (var itms in itm.restroomtype.ToList())
                    {
                        foreach (var items in Roominfo.ToList())
                        {
                            var Roomoccupied = WYNKContext.RoomOccupiedStatus.Where(x => x.RoomDetailsID == items.id && x.BedNo == items.bedno && x.IsOccupied == true).ToList();

                            if (Roomoccupied.Count == 0)
                            {

                                if (itm.roomtype == items.roomtype && itm.roomno == items.roomno)
                                {

                                    var Roomoccuiped = new Roomoccuiped();

                                    Roomoccuiped.RoomNumber = items.roomno;
                                    Roomoccuiped.BedNo = items.bedno;
                                    Roomoccuiped.ID = items.id;
                                    Roomoccuiped.RestroomType = Enum.GetName(typeof(RestRoomType), itms);
                                    Roomoccuiped.RoomID = items.roomid;
                                    Roomoccuiped.Status = "Vacant";
                                    Roomoccuiped.RoomCost = items.roomcost;
                                    Roomoccuiped.SID = OneLineMaster.Where(n => n.ParentDescription == "Booked").Select(i => i.OLMID).FirstOrDefault();
                                    room.Roomoccuiped.Add(Roomoccuiped);

                                }
                            }
                        }
                    }
                }
            }


            var PackageOnelineID = WYNKContext.Services.Where(x => x.Description == "Package").Select(x => x.ID).FirstOrDefault();
            room.PackageCost = WYNKContext.ServiceMaster.Where(x => x.parentDescription == Convert.ToString(PackageOnelineID) && x.RoomID == Convert.ToInt32(roomid)).Select(x => x.TotalAmount).FirstOrDefault();



            return room;
        }
        public dynamic InsertSurgeryAdmandAssign(Surgeryadmissionandassign Addsurgery, int cmpid, int TransactionId, string M_TelNo, string Category_M)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    var Admission = new Admission();
                    var SurgeryMaster = new SurgeryMaster();
                    var SurgeryAssigned = new SurgeryAssigned();
                    var attend = new AttendersPass();
                    var roomoc = WYNKContext.RoomOccupiedStatus.ToList();
                    var room = WYNKContext.Room.ToList();
                    var RoomDetails = WYNKContext.RoomDetails.ToList();
                    var Adm = WYNKContext.Admission.ToList();


                    var Roomoccupied = roomoc.Where(a => a.UIN == Addsurgery.Admission.UIN && a.CmpID == cmpid).Select(x => x.IsOccupied).LastOrDefault();

                    if (Roomoccupied == true)
                    {
                        dbContextTransaction.Rollback();
                        return new
                        {
                            Success = false,
                            Message = "This patient already exists",
                        };
                    }

                    Admission.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                    string AdmRandomUniqueID = Admission.RandomUniqueID;
                    Admission.UIN = Addsurgery.Admission.UIN;
                    Admission.RegTranID = Addsurgery.Admission.RegTranID;
                    Admission.AdmDate = Addsurgery.AdmDate;
                    Admission.CMPID = Addsurgery.Admission.CMPID;
                    Admission.AdmissionNumber = Addsurgery.AdmissionNumber;
                    Admission.CounsellingID = Addsurgery.Admission.CounsellingID;
                    Admission.IsSurgeryCompleted = false;
                    Admission.CreatedUTC = DateTime.UtcNow;
                    Admission.CreatedBy = Addsurgery.Admission.CreatedBy;
                    Admission.BillingType = Addsurgery.Admission.BillingType;
                    Admission.Category = Category_M;
                    WYNKContext.Admission.Add(Admission);
                    WYNKContext.SaveChanges();

                    SurgeryMaster.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                    string SmRandomUniqueID = SurgeryMaster.RandomUniqueID;
                    SurgeryMaster.UIN = Addsurgery.SurgeryMaster.UIN;
                    SurgeryMaster.RegistrationTranID = Admission.RegTranID;
                    SurgeryMaster.OTID = null;
                    SurgeryMaster.AdmID = AdmRandomUniqueID;
                    SurgeryMaster.DateofSurgery = Addsurgery.DateofSurgery;
                    SurgeryMaster.Remarks = Addsurgery.SurgeryMaster.Remarks;
                    SurgeryMaster.CMPID = Addsurgery.SurgeryMaster.CMPID;
                    SurgeryMaster.CreatedUTC = DateTime.UtcNow;
                    SurgeryMaster.CreatedBy = Addsurgery.Admission.CreatedBy;
                    WYNKContext.Surgery.Add(SurgeryMaster);
                    WYNKContext.SaveChanges();

                    if (Addsurgery.Surgery_Tran.Count() > 0)
                    {
                        foreach (var item in Addsurgery.Surgery_Tran.ToList())
                        {
                            var SurgeryTran = new Surgery_Tran();

                            SurgeryTran.SurgeryID = SmRandomUniqueID;
                            SurgeryTran.ICDCode = item.ICDCode;
                            SurgeryTran.IcdSpecialityCode = item.IcdSpecialityCode;
                            SurgeryTran.IsOD = item.IsOD;
                            SurgeryTran.IsOS = item.IsOS;
                            SurgeryTran.IsOU = item.IsOU;
                            SurgeryTran.CreatedUTC = DateTime.UtcNow;
                            SurgeryTran.CreatedBy = item.CreatedBy;
                            WYNKContext.SurgeryTran.AddRange(SurgeryTran);
                            WYNKContext.SaveChanges();
                        }
                    }

                    SurgeryAssigned.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                    string SaRandomUniqueID = SurgeryAssigned.RandomUniqueID;
                    SurgeryAssigned.UIN = Addsurgery.SurgeryAssigned.UIN;
                    SurgeryAssigned.SurgeryID = SmRandomUniqueID;
                    SurgeryAssigned.Admid = AdmRandomUniqueID;

                    if (Addsurgery.SurgeryDate != null)
                    {
                        SurgeryAssigned.FromTime = Addsurgery.SurgeryDate.ToString("HH:mm");
                    }
                    SurgeryAssigned.SurgeryDate = Addsurgery.SurgeryDate.Date;
                    SurgeryAssigned.FindingsExtID = WYNKContext.FindingsExt.Where(x => x.FindingsID == Addsurgery.findingsID).Select(s => s.ID).FirstOrDefault();
                    SurgeryAssigned.Status = 0;
                    SurgeryAssigned.IsRescheduled = false;
                    SurgeryAssigned.IsCancelled = false;
                    SurgeryAssigned.IsSurgeryCompleted = false;
                    SurgeryAssigned.CmpID = Addsurgery.SurgeryAssigned.CmpID;
                    SurgeryAssigned.CreatedUTC = DateTime.UtcNow;
                    SurgeryAssigned.CreatedBy = Addsurgery.Admission.CreatedBy;
                    WYNKContext.SurgeryAssigned.Add(SurgeryAssigned);
                    WYNKContext.SaveChanges();


                    if (Addsurgery.SurgeryAssignedTran.Count() > 0)
                    {
                        foreach (var item in Addsurgery.SurgeryAssignedTran.ToList())
                        {
                            var SurgeryAssignedTran = new SurgeryAssignedTran();

                            SurgeryAssignedTran.SAID = SaRandomUniqueID;
                            SurgeryAssignedTran.DoctorID = item.DoctorID;
                            SurgeryAssignedTran.IsCancelled = false;
                            SurgeryAssignedTran.SurgeryID = SmRandomUniqueID;
                            SurgeryAssignedTran.CreatedUTC = DateTime.UtcNow;
                            SurgeryAssignedTran.CreatedBy = item.CreatedBy;
                            WYNKContext.SurgeryAssignedTran.AddRange(SurgeryAssignedTran);
                            WYNKContext.SaveChanges();
                        }
                    }

                    if (Addsurgery.RoomOccupiedstatus.Count() > 0)
                    {
                        foreach (var item in Addsurgery.RoomOccupiedstatus.ToList())
                        {
                            var ros = new RoomOccupiedstatus();

                            ros.RoomID = item.RoomID;
                            ros.RoomDetailsID = item.RoomDetailsID;
                            ros.UIN = item.UIN;
                            ros.BedNo = item.BedNo;
                            ros.Status = item.Status;

                            if (item.RoomOccupationFromDate != null)
                            {
                                ros.RoomOccupationFromDate = Convert.ToDateTime(item.RoomOccupationFromDate).Date;
                            }

                            if (item.RoomOccupationFromDate != null)
                            {
                                DateTime rd = Convert.ToDateTime(item.RoomOccupationFromDate);
                                ros.RoomOccupationFromTime = rd.ToString("HH:mm");
                            }
                            ros.IsOccupied = true;
                            ros.CreatedUTC = DateTime.UtcNow;
                            ros.CmpID = cmpid;
                            ros.AdmID = AdmRandomUniqueID;
                            ros.CreatedBy = item.CreatedBy;
                            WYNKContext.RoomOccupiedStatus.AddRange(ros);
                            WYNKContext.SaveChanges();
                        }
                    }


                    var findid = WYNKContext.FindingsExt.Where(x => x.FindingsID == Addsurgery.findingsID).FirstOrDefault();


                    if (Addsurgery.findingsID != "")
                    {

                        var fid = new FindingsExt();

                        fid = WYNKContext.FindingsExt.Where(x => x.ID == findid.ID).FirstOrDefault();
                        fid.Status = "1";
                        fid.SurgeryComplete = false;
                        fid.UpdatedUTC = DateTime.UtcNow;
                        fid.UpdatedBy = Addsurgery.UpdatedBy;
                        WYNKContext.Entry(fid).State = EntityState.Modified;


                    }

                    var Date = DateTime.Now;
                    var CurrentMonth = Date.Month;
                    var FinancialYearId = WYNKContext.FinancialYear.Where(x => Convert.ToDateTime(x.FYFrom) <= Date && Convert.ToDateTime(x.FYTo) >= Date && x.CMPID == cmpid && x.IsActive == true).Select(x => x.ID).FirstOrDefault();

                    if (FinancialYearId == 0)
                    {
                        dbContextTransaction.Rollback();
                        return new
                        {
                            Success = false,
                            Message = "Financial year doesn't exists",
                        };
                    }


                    if (Addsurgery.PaymentMaster.Count > 0)
                    {
                        foreach (var item in Addsurgery.PaymentMaster.ToList())
                        {
                            var payment = new Payment_Master();

                            payment.UIN = Addsurgery.Admission.UIN;
                            payment.PaymentType = "A";
                            payment.PaymentMode = item.PaymentMode;
                            payment.InstrumentNumber = item.InstrumentNumber;
                            if (item.Instrumentdate != null)
                            {
                                payment.Instrumentdate = Convert.ToDateTime(item.Instrumentdate);
                            }
                            else
                            {
                                payment.Instrumentdate = null;
                            }
                            payment.BankBranch = item.BankBranch;
                            payment.BankName = item.BankName;

                            if (item.Expirydate != null)
                            {
                                payment.Expirydate = Convert.ToDateTime(item.Expirydate);
                            }
                            else
                            {
                                payment.Expirydate = null;
                            }

                            payment.Amount = Convert.ToDecimal(item.Amount);
                            payment.IsBilled = false;
                            payment.PaymentReferenceID = Admission.AdmID;
                            var date = DateTime.Now;
                            payment.Fyear = Convert.ToString(WYNKContext.FinancialYear.Where(x => x.ID == WYNKContext.FinancialYear.Where(b => Convert.ToDateTime(b.FYFrom) <= date && Convert.ToDateTime(b.FYTo) >= date && x.CMPID == cmpid && x.IsActive == true).Select(f => f.ID).FirstOrDefault()).Select(c => c.FYAccYear).FirstOrDefault());
                            payment.ReceiptNumber = Addsurgery.ReceiptRunningNo;
                            payment.ReceiptDate = DateTime.UtcNow;
                            payment.TransactionID = TransactionId;
                            payment.CmpID = cmpid;
                            payment.CreatedBy = Addsurgery.Admission.CreatedBy;
                            payment.CreatedUTC = DateTime.UtcNow;
                            WYNKContext.PaymentMaster.Add(payment);

                        }

                    }

                    if (Addsurgery.VehiclePasstran.Count > 0)
                    {
                        var vehiclemaster = new VehiclePassmaster();

                        vehiclemaster.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                        string VhRandomUniqueID = vehiclemaster.RandomUniqueID;
                        vehiclemaster.UIN = Admission.UIN;
                        vehiclemaster.RegistrationTranID = Admission.RegTranID;
                        vehiclemaster.AdmID = AdmRandomUniqueID;
                        vehiclemaster.CmpID = Admission.CMPID;
                        vehiclemaster.CreatedUTC = DateTime.UtcNow;
                        vehiclemaster.CreatedBy = Admission.CreatedBy;
                        WYNKContext.VehiclePassmaster.Add(vehiclemaster);


                        foreach (var item in Addsurgery.VehiclePasstran.ToList())
                        {
                            var vechicletran = new VehiclePasstran();

                            vechicletran.VecpassID = VhRandomUniqueID;
                            vechicletran.Make = item.Make;
                            vechicletran.VehicleNo = item.VehicleNo;
                            vechicletran.Type = item.Type;
                            vechicletran.CreatedBy = vehiclemaster.CreatedBy;
                            vechicletran.CreatedUTC = DateTime.UtcNow;
                            WYNKContext.VehiclePasstran.Add(vechicletran);

                        }

                    }

                    if (Addsurgery.AttendersPass.Name != null)
                    {
                        attend.RegistrationTranID = Admission.RegTranID;
                        attend.UIN = Admission.UIN;
                        attend.AdmID = AdmRandomUniqueID;
                        attend.CmpID = Admission.CMPID;
                        attend.Name = Addsurgery.AttendersPass.Name;
                        attend.Gender = Addsurgery.AttendersPass.Gender;
                        attend.Relationship = Addsurgery.AttendersPass.Relationship;
                        attend.Phone = M_TelNo;
                        attend.CreatedUTC = DateTime.UtcNow;
                        attend.CreatedBy = Admission.CreatedBy;
                        WYNKContext.AttendersPass.Add(attend);


                    }


                    if (Addsurgery.Policydata.Count > 0)
                    {
                        //attend.RegistrationTranID = Admission.RegTranID;
                        //attend.UIN = Admission.UIN;
                        //attend.AdmID = AdmRandomUniqueID;
                        //attend.CmpID = Admission.CMPID;
                        //attend.Name = Addsurgery.AttendersPass.Name;
                        //attend.Gender = Addsurgery.AttendersPass.Gender;
                        //attend.Relationship = Addsurgery.AttendersPass.Relationship;
                        //attend.Phone = M_TelNo;
                        //attend.CreatedUTC = DateTime.UtcNow;
                        //attend.CreatedBy = Admission.CreatedBy;
                        //WYNKContext.AttendersPass.Add(attend);
                    }
                    else
                    {
                        if (Addsurgery.M_Insurancecompany != null)
                        {
                            var pvi = new PatientVsInsurance();
                            pvi.SumAssured = Convert.ToDecimal(Addsurgery.PloicyM_Sumassured);
                            pvi.CmpID = cmpid;
                            pvi.UIN = Admission.UIN;
                            pvi.InsurancevsMiddlemenID = Addsurgery.M_MiddleManDetails;
                            pvi.InsuranceID = Addsurgery.M_Insurancecompany;
                            pvi.PolicyName = Addsurgery.PloicyM_PolicyName;
                            pvi.PolicyNo = Addsurgery.PloicyM_PolicyNumber;
                            DateTime PDTS;
                            var pappdates = DateTime.TryParseExact(Addsurgery.PloicyM_PolicyDate.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out PDTS);
                            {
                                PDTS.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                            }
                            pvi.PolicyTakenOn = PDTS;
                            DateTime DTS;
                            var appdates = DateTime.TryParseExact(Addsurgery.policyfrom.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DTS);
                            {
                                DTS.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                            }
                            pvi.PeriodFrom = DTS;
                            DateTime DT;
                            var appdate = DateTime.TryParseExact(Addsurgery.policyto.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DT);
                            {
                                DT.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                            }
                            pvi.PeriodTo = DT;
                            pvi.IsJointPolicy = false;
                            pvi.IsActive = true;
                            pvi.IsTransacted = false;
                            pvi.Remarks = Addsurgery.PloicyM_Remarks;
                            pvi.CreatedUTC = DateTime.UtcNow;
                            pvi.CreatedBy = Admission.CreatedBy;
                            WYNKContext.PatientVsInsurance.AddRange(pvi);
                        }
                    }

                    WYNKContext.SaveChanges();
                    dbContextTransaction.Commit();

                    var commonRepos = new CommonRepository(_Wynkcontext, _Cmpscontext);
                    var RunningNumber = commonRepos.GenerateRunningCtrlNoo(TransactionId, cmpid, "GetRunningNo");
                    if (RunningNumber == Addsurgery.AdmissionNumber)
                    {
                        commonRepos.GenerateRunningCtrlNoo(TransactionId, cmpid, "UpdateRunningNo");
                    }
                    else
                    {
                        var GetRunningNumber = commonRepos.GenerateRunningCtrlNoo(TransactionId, cmpid, "UpdateRunningNo");

                        var Invoiceno = WYNKContext.Admission.Where(x => x.AdmissionNumber == Addsurgery.AdmissionNumber).FirstOrDefault();
                        Invoiceno.InvoiceNumber = GetRunningNumber;
                        WYNKContext.Admission.UpdateRange(Invoiceno);
                        WYNKContext.SaveChanges();
                    }

                    var RecContraID = commonRepos.GettingReceiptTcID(TransactionId, cmpid);
                    var ReceiptRunningNumber = commonRepos.GenerateRunningCtrlNoo(Convert.ToInt32(RecContraID), cmpid, "GetRunningNo");
                    if (ReceiptRunningNumber == Addsurgery.ReceiptRunningNo)
                    {
                        commonRepos.GenerateRunningCtrlNoo(Convert.ToInt32(RecContraID), cmpid, "UpdateRunningNo");
                    }
                    else
                    {
                        var payments = WYNKContext.PaymentMaster.Where(x => x.ReceiptNumber == Addsurgery.ReceiptRunningNo && x.TransactionID == TransactionId).ToList();
                        payments.All(x => { x.ReceiptNumber = ReceiptRunningNumber; return true; });
                        WYNKContext.PaymentMaster.UpdateRange(payments);
                    }



                    return new
                    {
                        Success = true,
                        recepno = Addsurgery.ReceiptRunningNo,
                        uin = Addsurgery.Admission.UIN,
                        transid = TransactionId,
                        AdmitNo = Addsurgery.AdmissionNumber,
                    };
                }

                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                    string msg = ex.InnerException.Message;
                    return new { Success = false, Message = msg, grn = Addsurgery.AdmissionNumber };
                }

            }
        }

        public dynamic UpdateSurgeryAdmandAssign(Surgeryadmissionandassign Addsurgery, int cmpid, int TransactionId, string M_TelNo, string Category_M)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    var Admission = WYNKContext.Admission.Where(x => x.AdmID == Convert.ToInt32(M_TelNo)).FirstOrDefault();
                    var SurgeryMaster = new SurgeryMaster();
                    var SurgeryAssigned = new SurgeryAssigned();
                    var attend = new AttendersPass();

                    Admission.AdmDate = Admission.AdmDate;
                    Admission.UpdatedUTC = DateTime.UtcNow;
                    Admission.UpdatedBy = Addsurgery.Admission.CreatedBy;
                    WYNKContext.Admission.Update(Admission);

                    var surgery = WYNKContext.Surgery.Where(x => x.AdmID == Admission.RandomUniqueID).FirstOrDefault();
                    surgery.DateofSurgery = Addsurgery.SurgeryDate;
                    surgery.Remarks = Addsurgery.SurgeryMaster.Remarks;
                    surgery.UpdatedUTC = DateTime.UtcNow;
                    surgery.UpdatedBy = Addsurgery.Admission.CreatedBy;
                    WYNKContext.Surgery.Update(surgery);

                    var surgeryassigned = WYNKContext.SurgeryAssigned.Where(x => x.SurgeryID == surgery.RandomUniqueID).FirstOrDefault();
                    if (Addsurgery.SurgeryDate != null)
                    {
                        surgeryassigned.FromTime = Addsurgery.SurgeryDate.ToString("HH:mm");
                    }
                    surgeryassigned.SurgeryDate = Addsurgery.SurgeryDate.Date;
                    surgeryassigned.UpdatedUTC = DateTime.UtcNow;
                    surgeryassigned.UpdatedBy = Addsurgery.Admission.CreatedBy;
                    WYNKContext.SurgeryAssigned.Update(surgeryassigned);

                    var surgeryassignedtran = WYNKContext.SurgeryAssignedTran.Where(x => x.SAID == surgeryassigned.RandomUniqueID && x.SurgeryID == surgeryassigned.SurgeryID).ToList();
                    WYNKContext.SurgeryAssignedTran.RemoveRange(surgeryassignedtran);

                    if (Addsurgery.SurgeryAssignedTran.Count() > 0)
                    {
                        foreach (var item in Addsurgery.SurgeryAssignedTran.ToList())
                        {
                            var SurgeryAssignedTran = new SurgeryAssignedTran();
                            SurgeryAssignedTran.SAID = surgeryassigned.RandomUniqueID;
                            SurgeryAssignedTran.DoctorID = item.DoctorID;
                            SurgeryAssignedTran.IsCancelled = false;
                            SurgeryAssignedTran.SurgeryID = surgeryassigned.SurgeryID;
                            SurgeryAssignedTran.CreatedUTC = DateTime.UtcNow;
                            SurgeryAssignedTran.CreatedBy = item.CreatedBy;
                            WYNKContext.SurgeryAssignedTran.AddRange(SurgeryAssignedTran);
                            WYNKContext.SaveChanges();
                        }
                    }

                    if (Addsurgery.VehiclePasstran.Count > 0)
                    {
                        var VehiclepassID = WYNKContext.VehiclePassmaster.Where(x => x.RegistrationTranID == Admission.RegTranID && x.AdmID == Admission.RandomUniqueID).Select(x => x.RandomUniqueID).FirstOrDefault();

                        var Vehiclepass = WYNKContext.VehiclePassmaster.Where(x => x.RegistrationTranID == Admission.RegTranID && x.AdmID == Admission.RandomUniqueID).ToList();
                        WYNKContext.VehiclePassmaster.RemoveRange(Vehiclepass);

                        var vehiclemaster = new VehiclePassmaster();
                        vehiclemaster.RandomUniqueID = VehiclepassID;
                        string VhRandomUniqueID = vehiclemaster.RandomUniqueID;
                        vehiclemaster.UIN = Admission.UIN;
                        vehiclemaster.RegistrationTranID = Admission.RegTranID;
                        vehiclemaster.AdmID = Admission.RandomUniqueID;
                        vehiclemaster.CmpID = Admission.CMPID;
                        vehiclemaster.CreatedUTC = DateTime.UtcNow;
                        vehiclemaster.CreatedBy = Admission.CreatedBy;
                        WYNKContext.VehiclePassmaster.Add(vehiclemaster);


                        var Vehiclepasstran = WYNKContext.VehiclePasstran.Where(x =>  x.VecpassID == VehiclepassID).ToList();
                        WYNKContext.VehiclePasstran.RemoveRange(Vehiclepasstran);

                        foreach (var item in Addsurgery.VehiclePasstran.ToList())
                        {
                            var vechicletran = new VehiclePasstran();
                            vechicletran.VecpassID = VhRandomUniqueID;
                            vechicletran.Make = item.Make;
                            vechicletran.VehicleNo = item.VehicleNo;
                            vechicletran.Type = item.Type;
                            vechicletran.CreatedBy = vehiclemaster.CreatedBy;
                            vechicletran.CreatedUTC = DateTime.UtcNow;
                            WYNKContext.VehiclePasstran.Add(vechicletran);

                        }

                    }

                    if (Addsurgery.AttendersPass.Name != null)
                    {

                        var Attenderpass = WYNKContext.AttendersPass.Where(x => x.AdmID == Admission.RandomUniqueID && x.RegistrationTranID == Admission.RegTranID && x.UIN == Admission.UIN).FirstOrDefault();                        

                        Attenderpass.Name = Addsurgery.AttendersPass.Name;
                        Attenderpass.Gender = Addsurgery.AttendersPass.Gender;
                        Attenderpass.Relationship = Addsurgery.AttendersPass.Relationship;
                        Attenderpass.Phone = M_TelNo;
                        Attenderpass.UpdatedUTC = DateTime.UtcNow;
                        Attenderpass.CreatedBy = Admission.CreatedBy;
                        WYNKContext.AttendersPass.UpdateRange(Attenderpass);
                    }
                    if (Addsurgery.Policydata.Count > 0)
                    {

                    }
                    else
                    {
                        if (Addsurgery.M_Insurancecompany != null)
                        {
                            var pvi = new PatientVsInsurance();
                            pvi.SumAssured = Convert.ToDecimal(Addsurgery.PloicyM_Sumassured);
                            pvi.CmpID = cmpid;
                            pvi.UIN = Admission.UIN;
                            pvi.InsurancevsMiddlemenID = Addsurgery.M_MiddleManDetails;
                            pvi.InsuranceID = Addsurgery.M_Insurancecompany;
                            pvi.PolicyName = Addsurgery.PloicyM_PolicyName;
                            pvi.PolicyNo = Addsurgery.PloicyM_PolicyNumber;
                            DateTime PDTS;
                            var pappdates = DateTime.TryParseExact(Addsurgery.PloicyM_PolicyDate.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out PDTS);
                            {
                                PDTS.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                            }
                            pvi.PolicyTakenOn = PDTS;
                            DateTime DTS;
                            var appdates = DateTime.TryParseExact(Addsurgery.policyfrom.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DTS);
                            {
                                DTS.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                            }
                            pvi.PeriodFrom = DTS;
                            DateTime DT;
                            var appdate = DateTime.TryParseExact(Addsurgery.policyto.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DT);
                            {
                                DT.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                            }
                            pvi.PeriodTo = DT;
                            pvi.IsJointPolicy = false;
                            pvi.IsActive = true;
                            pvi.IsTransacted = false;
                            pvi.Remarks = Addsurgery.PloicyM_Remarks;
                            pvi.CreatedUTC = DateTime.UtcNow;
                            pvi.CreatedBy = Admission.CreatedBy;
                            WYNKContext.PatientVsInsurance.AddRange(pvi);
                        }
                    }

                    WYNKContext.SaveChanges();
                    dbContextTransaction.Commit();

                    return new
                    {
                        Success = true,
                        recepno = WYNKContext.PaymentMaster.Where(x =>x.UIN == Admission.UIN && x.TransactionID == TransactionId && x.CmpID == Admission.CMPID).Select(x =>x.ReceiptNumber).FirstOrDefault(),
                        uin = Admission.UIN,
                        transid = TransactionId,
                        AdmitNo = Admission.AdmissionNumber,

                    };
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                    string msg = ex.InnerException.Message;
                    return new { Success = false, Message = msg};
                }

            }


        }
        public dynamic Getadmissionprint(string AdmitNo, int? cmpid, string uin, int? tid, string Time, string recepno)
        {

            var surprint = new Surgeryadmissionandassign();
            surprint.printpaymnet = new List<printpaymnet>();
            TimeSpan ts = TimeSpan.Parse(Time);

            if (recepno == "Admission" || recepno == "Advance" || recepno == "Vehiclepass" || recepno == "attenderpass")
            {
                surprint.Address = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address1 != null ? x.Address1 : string.Empty).FirstOrDefault();
                surprint.Address1 = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address2 != null ? x.Address2 : string.Empty).FirstOrDefault();
                surprint.Address2 = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address3 != null ? x.Address3 : string.Empty).FirstOrDefault();
                surprint.phone = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Phone1).FirstOrDefault();
                surprint.web = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Website).FirstOrDefault();
                surprint.Compnayname = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.CompanyName).FirstOrDefault();
                surprint.AdmitNo = WYNKContext.Admission.Where(x => x.AdmissionNumber == AdmitNo && x.CMPID == cmpid).Select(x => x.AdmissionNumber).FirstOrDefault();
                surprint.AdmitDate = WYNKContext.Admission.Where(x => x.AdmissionNumber == AdmitNo && x.CMPID == cmpid).Select(x => x.AdmDate.Add(ts)).FirstOrDefault();
                if (recepno == "Admission")
                {
                    surprint.RoomType = WYNKContext.Room.Where(w => w.ID == WYNKContext.RoomOccupiedStatus.Where(u => u.UIN == uin && u.CmpID == cmpid && u.AdmID == WYNKContext.Admission.Where(e => e.AdmissionNumber == AdmitNo).Select(x => x.RandomUniqueID).FirstOrDefault()).Select(x => x.RoomID).FirstOrDefault()).Select(g => g.RoomType).FirstOrDefault();
                    surprint.RoomDescription = WYNKContext.Room.Where(w => w.ID == WYNKContext.RoomOccupiedStatus.Where(u => u.UIN == uin && u.CmpID == cmpid && u.AdmID == WYNKContext.Admission.Where(e => e.AdmissionNumber == AdmitNo).Select(x => x.RandomUniqueID).FirstOrDefault()).Select(x => x.RoomID).FirstOrDefault()).Select(g => g.RoomDescription).FirstOrDefault();
                    surprint.RoomNo = WYNKContext.RoomDetails.Where(w => w.ID == WYNKContext.RoomOccupiedStatus.Where(u => u.UIN == uin && u.CmpID == cmpid && u.AdmID == WYNKContext.Admission.Where(e => e.AdmissionNumber == AdmitNo).Select(x => x.RandomUniqueID).FirstOrDefault()).Select(x => x.RoomDetailsID).FirstOrDefault()).Select(g => g.RoomNumber).FirstOrDefault();
                    surprint.BedNo = WYNKContext.RoomOccupiedStatus.Where(s => s.UIN == uin && s.CmpID == cmpid && s.AdmID == WYNKContext.Admission.Where(x => x.AdmissionNumber == AdmitNo).Select(x => x.RandomUniqueID).FirstOrDefault()).Select(x => x.BedNo).FirstOrDefault();
                    surprint.RoomoccupiedDate = WYNKContext.RoomOccupiedStatus.Where(s => s.UIN == uin && s.CmpID == cmpid && s.AdmID == WYNKContext.Admission.Where(x => x.AdmissionNumber == AdmitNo).Select(x => x.RandomUniqueID).FirstOrDefault()).Select(x => x.RoomOccupationFromDate).FirstOrDefault();
                    DateTime DT = Convert.ToDateTime(WYNKContext.RoomOccupiedStatus.Where(s => s.UIN == uin && s.CmpID == cmpid && s.AdmID == WYNKContext.Admission.Where(x => x.AdmissionNumber == AdmitNo).Select(x => x.RandomUniqueID).FirstOrDefault()).Select(x => x.RoomOccupationFromTime).FirstOrDefault());
                    string time = DT.Add(ts).ToString("HH:mm");
                    surprint.Roomoccupiedfromtime = time != null ? time : "";
                }
                else if (recepno == "Advance")
                {
                    recepno = WYNKContext.PaymentMaster.Where(x => x.PaymentReferenceID == tid).Select(x => x.ReceiptNumber).FirstOrDefault();
                    tid = WYNKContext.PaymentMaster.Where(x => x.PaymentReferenceID == tid).Select(x => x.TransactionID).FirstOrDefault();

                    var cost = WYNKContext.PaymentMaster.Where(x => x.ReceiptNumber == recepno && x.CmpID == cmpid && x.UIN == uin && x.TransactionID == tid).Select(u => u.Amount).ToList();

                    if (cost.Count != 0)
                    {


                        foreach (var item in cost.ToList())
                        {
                            var value = item;

                            if (surprint.TotalAmount == 0)
                            {

                                surprint.TotalAmount = value;
                            }
                            else
                            {
                                surprint.TotalAmount = surprint.TotalAmount + value;

                            }

                        }

                    }

                    surprint.printpaymnet = (from PoTrans in WYNKContext.PaymentMaster.Where(x => x.ReceiptNumber == recepno && x.CmpID == cmpid && x.UIN == uin && x.TransactionID == tid)



                                             select new printpaymnet
                                             {

                                                 PaymentMode = PoTrans.PaymentMode,
                                                 InstrumentNumber = PoTrans.InstrumentNumber,
                                                 Instrumentdate = PoTrans.Instrumentdate,
                                                 Expirydate = PoTrans.Expirydate,
                                                 BankName = PoTrans.BankName,
                                                 BankBranch = PoTrans.BankBranch,
                                                 Amount = PoTrans.Amount,
                                                 Receiptnumber = PoTrans.ReceiptNumber,
                                                 Receiptdate = PoTrans.ReceiptDate.Value.Add(ts),
                                             }).ToList();
                }

                else if (recepno == "Vehiclepass")
                {
                    surprint.Vehiclepassdetail = WYNKContext.VehiclePasstran.Where(x => x.VecpassID == WYNKContext.VehiclePassmaster.Where(s => s.AdmID == WYNKContext.Admission.Where(w => w.AdmissionNumber == AdmitNo && w.CMPID == cmpid).Select(e => e.RandomUniqueID).FirstOrDefault()).Select(q => q.RandomUniqueID).FirstOrDefault()).ToList();
                }

                else if (recepno == "attenderpass")
                {
                    surprint.AttendersPassdetail = WYNKContext.AttendersPass.Where(x => x.AdmID == WYNKContext.Admission.Where(w => w.AdmissionNumber == AdmitNo && w.CMPID == cmpid).Select(w => w.RandomUniqueID).FirstOrDefault()).ToList();

                }

                return new
                {


                    add = surprint.Address,
                    add1 = surprint.Address1,
                    add2 = surprint.Address2,
                    ph = surprint.phone,
                    webb = surprint.web,
                    cn = surprint.Compnayname,
                    totalamt = surprint.TotalAmount,
                    Posurgerydetails = surprint.printpaymnet.ToList(),
                    Admitcardno = surprint.AdmitNo,
                    Admitdate = surprint.AdmitDate,
                    AdmitRoomType = surprint.RoomType,
                    AdmitRoomDescription = surprint.RoomDescription,
                    AdmitRoomNo = surprint.RoomNo,
                    AdmitBedNo = surprint.BedNo,
                    AdmitRoomoccupiedDate = surprint.RoomoccupiedDate,
                    AdmitRoomoccupiedfromtime = surprint.Roomoccupiedfromtime,
                    Vehiclepassdetails = surprint.Vehiclepassdetail,
                    AttendersPassdetails = surprint.AttendersPassdetail,

                };

            }
            else
            {

                surprint.Address = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address1 != null ? x.Address1 : string.Empty).FirstOrDefault();
                surprint.Address1 = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address2 != null ? x.Address2 : string.Empty).FirstOrDefault();
                surprint.Address2 = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address3 != null ? x.Address3 : string.Empty).FirstOrDefault();
                surprint.phone = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Phone1).FirstOrDefault();
                surprint.web = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Website).FirstOrDefault();
                surprint.Compnayname = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.CompanyName).FirstOrDefault();
                surprint.AdmitNo = WYNKContext.Admission.Where(x => x.AdmissionNumber == AdmitNo && x.CMPID == cmpid).Select(x => x.AdmissionNumber).FirstOrDefault();
                surprint.AdmitDate = WYNKContext.Admission.Where(x => x.AdmissionNumber == AdmitNo && x.CMPID == cmpid).Select(x => x.AdmDate.Add(ts)).FirstOrDefault();
                surprint.RoomType = WYNKContext.Room.Where(w => w.ID == WYNKContext.RoomOccupiedStatus.Where(u => u.UIN == uin && u.CmpID == cmpid && u.AdmID == WYNKContext.Admission.Where(e => e.AdmissionNumber == AdmitNo).Select(x => x.RandomUniqueID).FirstOrDefault()).Select(x => x.RoomID).FirstOrDefault()).Select(g => g.RoomType).FirstOrDefault();
                surprint.RoomDescription = WYNKContext.Room.Where(w => w.ID == WYNKContext.RoomOccupiedStatus.Where(u => u.UIN == uin && u.CmpID == cmpid && u.AdmID == WYNKContext.Admission.Where(e => e.AdmissionNumber == AdmitNo).Select(x => x.RandomUniqueID).FirstOrDefault()).Select(x => x.RoomID).FirstOrDefault()).Select(g => g.RoomDescription).FirstOrDefault();
                surprint.RoomNo = WYNKContext.RoomDetails.Where(w => w.ID == WYNKContext.RoomOccupiedStatus.Where(u => u.UIN == uin && u.CmpID == cmpid && u.AdmID == WYNKContext.Admission.Where(e => e.AdmissionNumber == AdmitNo).Select(x => x.RandomUniqueID).FirstOrDefault()).Select(x => x.RoomDetailsID).FirstOrDefault()).Select(g => g.RoomNumber).FirstOrDefault();
                surprint.BedNo = WYNKContext.RoomOccupiedStatus.Where(s => s.UIN == uin && s.CmpID == cmpid && s.AdmID == WYNKContext.Admission.Where(x => x.AdmissionNumber == AdmitNo).Select(x => x.RandomUniqueID).FirstOrDefault()).Select(x => x.BedNo).FirstOrDefault();
                surprint.RoomoccupiedDate = WYNKContext.RoomOccupiedStatus.Where(s => s.UIN == uin && s.CmpID == cmpid && s.AdmID == WYNKContext.Admission.Where(x => x.AdmissionNumber == AdmitNo).Select(x => x.RandomUniqueID).FirstOrDefault()).Select(x => x.RoomOccupationFromDate).FirstOrDefault();
                DateTime DT = Convert.ToDateTime(WYNKContext.RoomOccupiedStatus.Where(s => s.UIN == uin && s.CmpID == cmpid && s.AdmID == WYNKContext.Admission.Where(x => x.AdmissionNumber == AdmitNo).Select(x => x.RandomUniqueID).FirstOrDefault()).Select(x => x.RoomOccupationFromTime).FirstOrDefault());
                string time = DT.Add(ts).ToString("HH:mm");
                surprint.Roomoccupiedfromtime = time != null ? time : "";


                var cost = WYNKContext.PaymentMaster.Where(x => x.ReceiptNumber == recepno && x.CmpID == cmpid && x.UIN == uin && x.TransactionID == tid).Select(u => u.Amount).ToList();

                if (cost.Count != 0)
                {


                    foreach (var item in cost.ToList())
                    {
                        var value = item;

                        if (surprint.TotalAmount == 0)
                        {

                            surprint.TotalAmount = value;
                        }
                        else
                        {
                            surprint.TotalAmount = surprint.TotalAmount + value;

                        }

                    }

                }

                surprint.printpaymnet = (from PoTrans in WYNKContext.PaymentMaster.Where(x => x.ReceiptNumber == recepno && x.CmpID == cmpid && x.UIN == uin && x.TransactionID == tid)



                                         select new printpaymnet
                                         {

                                             PaymentMode = PoTrans.PaymentMode,
                                             InstrumentNumber = PoTrans.InstrumentNumber,
                                             Instrumentdate = PoTrans.Instrumentdate,
                                             Expirydate = PoTrans.Expirydate,
                                             BankName = PoTrans.BankName,
                                             BankBranch = PoTrans.BankBranch,
                                             Amount = PoTrans.Amount,
                                             Receiptnumber = PoTrans.ReceiptNumber,
                                             Receiptdate = PoTrans.ReceiptDate.Value.Add(ts),
                                         }).ToList();


                var Vehiclepass = WYNKContext.VehiclePasstran.Where(x => x.VecpassID == WYNKContext.VehiclePassmaster.Where(s => s.AdmID == WYNKContext.Admission.Where(w => w.AdmissionNumber == AdmitNo && w.CMPID == cmpid).Select(e => e.RandomUniqueID).FirstOrDefault()).Select(q => q.RandomUniqueID).FirstOrDefault()).ToList();

                var AttendersPass = WYNKContext.AttendersPass.Where(x => x.AdmID == WYNKContext.Admission.Where(w => w.AdmissionNumber == AdmitNo && w.CMPID == cmpid).Select(w => w.RandomUniqueID).FirstOrDefault()).ToList();


                return new
                {


                    add = surprint.Address,
                    add1 = surprint.Address1,
                    add2 = surprint.Address2,
                    ph = surprint.phone,
                    webb = surprint.web,
                    cn = surprint.Compnayname,
                    totalamt = surprint.TotalAmount,
                    Posurgerydetails = surprint.printpaymnet.ToList(),
                    Admitcardno = surprint.AdmitNo,
                    Admitdate = surprint.AdmitDate,
                    AdmitRoomType = surprint.RoomType,
                    AdmitRoomDescription = surprint.RoomDescription,
                    AdmitRoomNo = surprint.RoomNo,
                    AdmitBedNo = surprint.BedNo,
                    AdmitRoomoccupiedDate = surprint.RoomoccupiedDate,
                    AdmitRoomoccupiedfromtime = surprint.Roomoccupiedfromtime,
                    Vehiclepassdetails = Vehiclepass,
                    AttendersPassdetails = AttendersPass

                };

            }

        }
        public dynamic Getviewpatient(int cmpid, string Time)
        {

            var Admission = WYNKContext.Admission.ToList();
            var Surgery = WYNKContext.Surgery.ToList();
            var SurgeryTran = WYNKContext.SurgeryTran.ToList();
            var reg = WYNKContext.Registration.ToList();
            var docmas = CMPSContext.DoctorMaster.ToList();
            var SurgeryAssignedTran = WYNKContext.SurgeryAssignedTran.ToList();
            var icdspec = WYNKContext.ICDSpecialityCode.ToList();
            TimeSpan ts = TimeSpan.Parse(Time);



            var Getviewpatient = (from f in Admission.Where(x => x.CMPID == cmpid && x.DischargeID == null).OrderByDescending(x => x.CreatedUTC)
                                  join fex in Surgery on
                                  f.RandomUniqueID equals fex.AdmID
                                  join us in SurgeryTran on
                                  fex.RandomUniqueID equals us.SurgeryID
                                  join sur in SurgeryAssignedTran on
                                  fex.RandomUniqueID equals sur.SurgeryID




                                  select new
                                  {

                                      uin = f.UIN,
                                      Name = reg.Where(x => x.UIN == f.UIN).Select(x => x.Name + " " + x.MiddleName + " " + x.LastName).FirstOrDefault(),
                                      Age = PasswordEncodeandDecode.ToAgeString(reg.Where(x => x.UIN == f.UIN).Select(x => x.DateofBirth).FirstOrDefault()),
                                      Gender = reg.Where(x => x.UIN == f.UIN).Select(x => x.Gender).FirstOrDefault(),
                                      SurgeryAdvName = docmas.Where(x => x.DoctorID == sur.DoctorID).Select(x => x.Firstname + " " + x.MiddleName + " " + x.LastName).FirstOrDefault(),
                                      SurgeryAdvDate = fex.DateofSurgery.Value.Add(ts),
                                      SurgeryType = icdspec.Where(x => x.ID == us.IcdSpecialityCode).Select(x => x.SpecialityDescription).FirstOrDefault(),
                                      Rid = f.RegTranID,
                                      RandomUniqueID = f.RandomUniqueID,
                                      admission = f.AdmissionNumber,
                                      cmpid = f.CMPID,
                                      createUtc = f.CreatedUTC,
                                      AdmID = f.AdmID,
                                      DoctorID = docmas.Where(x => x.DoctorID == sur.DoctorID).Select(x => x.DoctorID).FirstOrDefault(),
                                  }).ToList();






            return Getviewpatient;
        }
        public dynamic Getviewselectpatient(string RandomUniqueID, int RegID, string Time, string uin, int AdmID)
        {

            var detail = new Surgeryadmissionandassign();
            detail.Counselview = new List<Counselview>();
            detail.AllotedViewroomdetails = new List<AllotedViewroomdetails>();
            var Adms = WYNKContext.Admission.ToList();
            var surg = WYNKContext.Surgery.ToList();
            var surgtran = WYNKContext.SurgeryTran.ToList();
            var coun = WYNKContext.Counselling.ToList();
            var surgeryassg = WYNKContext.SurgeryAssigned.ToList();
            var surgeryassgt = WYNKContext.SurgeryAssignedTran.ToList();
            var docmas = CMPSContext.DoctorMaster.ToList();
            var roomoccu = WYNKContext.RoomOccupiedStatus.ToList();
            var icdspcode = WYNKContext.ICDSpecialityCode.ToList();
            var room = WYNKContext.Room.ToList();
            var roomdetails = WYNKContext.RoomDetails.ToList();
            var roomdetailtex = WYNKContext.RoomDetailsExtension.ToList();
            var one = CMPSContext.OneLineMaster.ToList();
            var SurgeryCostDetail = WYNKContext.SurgeryCostDetail.ToList();
            var DoctorSpeciality = CMPSContext.DoctorSpeciality.ToList();
            var cmpid = WYNKContext.Admission.Where(x => x.RandomUniqueID == RandomUniqueID).OrderByDescending(x => x.CreatedUTC).FirstOrDefault();
            var icdmas = WYNKContext.ICDMaster.Where(x => x.SpecialityCode != null && x.IsIOLReqd != null).ToList();
            TimeSpan ts = TimeSpan.Parse(Time);


            detail.PolicyDetails = (from pc in WYNKContext.PatientVsInsurance.Where(x => x.UIN == uin && x.CmpID == cmpid.CMPID)
                                    select new PolicyDetails
                                    {
                                        InsuranceName = WYNKContext.Insurance.Where(x =>x.ID == pc.InsuranceID).Select(x =>x.Name).FirstOrDefault(),
                                        PolicyName = pc.PolicyName,
                                        PolicyNumber = pc.PolicyNo,
                                        Periodfrom = pc.PeriodFrom,
                                        periodto = pc.PeriodTo,
                                        sumassured = pc.SumAssured,
                                    }).ToList();

            var Policyinstruction = cmpid.Category;

            var viewcounsel = (from c in coun.Where(u => u.UIN == uin)
                               select new
                               {
                                   CreateduTC = c.CreatedUTC.Add(ts),
                                   Otherrequirements = c.OtherRequirements,
                                   CounsellingAdvName = docmas.Where(x => x.CreatedBy == c.CreatedBy).Select(s => s.Firstname + " " + s.MiddleName + " " + s.LastName).FirstOrDefault(),
                                   Surgerydate = c.SurgeryDate,
                                   CID = c.ID,
                               }).LastOrDefault();

            var Viewsurgerydetails = (from f in Adms.Where(x => x.RandomUniqueID == RandomUniqueID).OrderByDescending(x => x.CreatedUTC)
                                      join fex in surg on
                                      f.RandomUniqueID equals fex.AdmID
                                      join st in surgtran on
                                      fex.RandomUniqueID equals st.SurgeryID
                                      join sut in surgeryassgt on
                                      fex.RandomUniqueID equals sut.SurgeryID
                                      select new
                                      {
                                          ID = fex.ID,
                                          SurgeryTypname = icdspcode.Where(x => x.ID == st.IcdSpecialityCode).Select(x => x.SpecialityDescription).FirstOrDefault(),
                                          SurgeryDescriptionname = icdmas.Where(x => x.ICDCODE == st.ICDCode).Select(x => x.ICDDESCRIPTION).FirstOrDefault(),
                                          IsOD = st.IsOD,
                                          IsOS = st.IsOS,
                                          IsOU = st.IsOU,
                                          DateofSurgery = fex.DateofSurgery.Value.Add(ts),
                                          Doctorname = docmas.Where(x => x.DoctorID == sut.DoctorID).Select(x => x.Firstname + " " + x.MiddleName + " " + x.LastName).FirstOrDefault(),
                                          DoctorID = docmas.Where(x => x.DoctorID == sut.DoctorID).Select(x => x.DoctorID).FirstOrDefault(),
                                          CreateUtc = st.CreatedUTC,
                                      }).ToList();
            var Viewroomdetails = (from f in Adms.Where(x => x.RandomUniqueID == RandomUniqueID).OrderByDescending(x => x.CreatedUTC)
                                   join fex in roomoccu on
                                   f.RandomUniqueID equals fex.AdmID
                                   select new
                                   {
                                       ID = fex.RoomID,
                                       Roomtype = WYNKContext.Room.Where(x => x.ID == fex.RoomID).Select(x => x.RoomType).FirstOrDefault(),
                                       Roomnumber = roomdetails.Where(x => x.ID == fex.RoomDetailsID).Select(s => s.RoomNumber).FirstOrDefault(),
                                       Bedno = fex.BedNo,
                                       Roomcost = room.Where(x => x.ID == fex.RoomID).Select(x => x.RoomCost).FirstOrDefault(),
                                       roomocudate = fex.RoomOccupationFromDate,
                                       vacant = one.Where(x => x.OLMID == fex.Status).Select(x => x.ParentDescription).FirstOrDefault(),
                                       RoomID = roomdetailtex.Where(h => h.RoomDetailsID == roomdetails.Where(e => e.RoomNumber == roomdetails.Where(x => x.ID == fex.RoomDetailsID).Select(s => s.RoomNumber).FirstOrDefault()).Select(r => r.ID).LastOrDefault()).Select(j => j.RestRoomType).ToList(),
                                   }).ToList();
            if (Viewroomdetails.Count() > 0)
            {
                foreach (var itm in Viewroomdetails.ToList())
                {
                    foreach (var itms in itm.RoomID.ToList())
                    {
                        var AllotedViewroomdetails = new AllotedViewroomdetails();

                        AllotedViewroomdetails.ID = itm.ID;
                        AllotedViewroomdetails.Roomtype = itm.Roomtype;
                        AllotedViewroomdetails.Roomnumber = itm.Roomnumber;
                        AllotedViewroomdetails.Bedno = itm.Bedno;
                        AllotedViewroomdetails.Roomcost = itm.Roomcost;
                        AllotedViewroomdetails.roomocudate = itm.roomocudate;
                        AllotedViewroomdetails.roomresttype = Enum.GetName(typeof(RestRoomType), itms);
                        AllotedViewroomdetails.vacant = itm.vacant;
                        detail.AllotedViewroomdetails.Add(AllotedViewroomdetails);
                    }
                }
            }
            var Doctorlist = (from f in Adms.Where(x => x.RandomUniqueID == RandomUniqueID).OrderByDescending(x => x.CreatedUTC)
                              join fex in surg on
                              f.RandomUniqueID equals fex.AdmID
                              join sur in surgeryassgt on
                              fex.RandomUniqueID equals sur.SurgeryID
                              join DC in DoctorSpeciality on
                              sur.DoctorID equals DC.DoctorID
                              join onemas in one on
                              DC.OLMID equals onemas.OLMID
                              select new
                              {
                                  ID = sur.DoctorID,
                                  Name = docmas.Where(x => x.DoctorID == sur.DoctorID).Select(x => x.Firstname + " " + x.MiddleName + " " + x.LastName).FirstOrDefault(),
                                  Role = onemas.ParentDescription,
                                  checkStatusall = true,
                              }).ToList();

            var unselecteddoctors = (from OLM in one
                                     join DS in DoctorSpeciality on OLM.OLMID equals DS.OLMID
                                     join DM in docmas on DS.DoctorID equals DM.DoctorID
                                     where (DM.IsDeleted == false && DM.IsActive == true && OLM.ParentDescription != "Optometrist"
                                     && OLM.ParentDescription != "Vision care" && DM.CMPID == cmpid.CMPID)
                                     select new
                                     {
                                         ID = DM.DoctorID,
                                         Name = DM.Firstname + " " + DM.MiddleName + " " + DM.LastName,
                                         Role = OLM.ParentDescription,
                                         checkStatusall = false,
                                     }).ToList();

            var dupliactedoctors = unselecteddoctors.Where(p => Doctorlist.All(p2 => p2.ID != p.ID));

            var mergeddocotrs = dupliactedoctors.Union(Doctorlist);


            var viewprintpaymnet = (from PoTrans in WYNKContext.PaymentMaster.Where(x => x.PaymentReferenceID == AdmID && x.UIN == uin)
                                    select new
                                    {

                                        PaymentMode = PoTrans.PaymentMode,
                                        InstrumentNumber = PoTrans.InstrumentNumber,
                                        Instrumentdate = PoTrans.Instrumentdate,
                                        Expirydate = PoTrans.Expirydate,
                                        BankName = PoTrans.BankName,
                                        BankBranch = PoTrans.BankBranch,
                                        Amount = PoTrans.Amount,
                                    }).ToList();
            var viewvehiclepass = (from vp in WYNKContext.VehiclePassmaster.Where(x => x.AdmID == RandomUniqueID).OrderByDescending(x => x.CreatedUTC)
                                   join vh in WYNKContext.VehiclePasstran on
                                   vp.RandomUniqueID equals vh.VecpassID
                                   select new
                                   {
                                       Make = vh.Make,
                                       Type = vh.Type,
                                       VehicleNo = vh.VehicleNo
                                   }).ToList();
            var attendername = WYNKContext.AttendersPass.Where(x => x.AdmID == RandomUniqueID).OrderByDescending(x => x.CreatedUTC).Select(x => x.Name).FirstOrDefault();
            var attenderrelationship = WYNKContext.AttendersPass.Where(x => x.AdmID == RandomUniqueID).OrderByDescending(x => x.CreatedUTC).Select(x => x.Relationship).FirstOrDefault();
            var attendergender = WYNKContext.AttendersPass.Where(x => x.AdmID == RandomUniqueID).OrderByDescending(x => x.CreatedUTC).Select(x => x.Gender).FirstOrDefault();
            var attenderphone = WYNKContext.AttendersPass.Where(x => x.AdmID == RandomUniqueID).OrderByDescending(x => x.CreatedUTC).Select(x => x.Phone).FirstOrDefault();


            var Admdate = cmpid.AdmDate.Add(ts);
            var surgerydatetime = surg.Where(x => x.AdmID == cmpid.RandomUniqueID).Select(x => x.DateofSurgery).FirstOrDefault();
            var admnumber = cmpid.AdmID;
            var remarks = surg.Where(x => x.AdmID == cmpid.RandomUniqueID).Select(x => x.Remarks).FirstOrDefault();
            try
            {

                return new
                {
                    AdmiD = cmpid.AdmID,
                    Regtranid = cmpid.RegTranID,
                    Admdates = Admdate,
                    surgerydatetimes = surgerydatetime,
                    admnumbers = admnumber,
                    remarkss = remarks,
                    Getbillingtype = WYNKContext.Admission.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.BillingType).FirstOrDefault(),
                    //GetAdmissiondetails = ViewAdmissiondetails,
                    GetpolicyDetails = detail.PolicyDetails,
                    GetCounsellingdetails = detail.Counselview,
                    Getsurgerydetails = Viewsurgerydetails,
                    Getroomdetails = detail.AllotedViewroomdetails,
                    //Getsurgerycost = surgerycost,
                    GetDoctorlist = mergeddocotrs.OrderByDescending(x => x.checkStatusall),
                    //totalamt = Totalamt,
                    paymentsurgerydetails = viewprintpaymnet,
                    Getvehiclepass = viewvehiclepass,
                    attendernames = attendername,
                    attenderrelationships = attenderrelationship,
                    attendergenders = attendergender,
                    attenderphones = attenderphone,
                    insurancecategory = Policyinstruction,
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

    }
}
