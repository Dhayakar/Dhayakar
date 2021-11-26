using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Helpers;

namespace WYNK.Data.Repository.Implementation
{
    class SurgeryRepository : RepositoryBase<SurgeryViewModel>, ISurgeryRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        
        public SurgeryRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }


        public dynamic InsertSurgeryMas(SurgeryViewModel Surgery)

        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {

                    var Admission = new Admission();
                    var SurgeryMaster = new SurgeryMaster();
                    var SurgeryTran = new Surgery_Tran();
                    var SurgeryDoctor = new SurgeryDoctor();
                    var IntraOperative = new IntraOperative();
                    var PatientAct = new PatientAccount();
                    var PatientActDet = new PatientAccountDetail();
                    var ManagementMaster = new ManagementMaster();

                    Admission.AdmDate = Surgery.Admission.AdmDate;
                    Admission.UIN = Surgery.Admission.UIN;
                    Admission.CMPID = Surgery.Admission.CMPID;
                    Admission.RegTranID = Surgery.Admission.RegTranID;
                    Admission.IsSurgeryCompleted = false;
                    Admission.CreatedUTC = DateTime.UtcNow;
                    Admission.CreatedBy = Surgery.Admission.CreatedBy;
                    WYNKContext.Admission.Add(Admission);
                    WYNKContext.SaveChanges();

                    var AdmID = Admission.AdmID;

                    SurgeryMaster.UIN = Surgery.SurgeryMaster.UIN;
                    SurgeryMaster.RegistrationTranID = Surgery.SurgeryMaster.RegistrationTranID;
                    SurgeryMaster.OTID = Surgery.SurgeryMaster.OTID;
                    SurgeryMaster.Remarks = Surgery.SurgeryMaster.Remarks;
                    SurgeryMaster.SurgeryCost = Surgery.SurgeryMaster.SurgeryCost;
                    SurgeryMaster.SurgeryPerformedInHouse = Surgery.SurgeryMaster.SurgeryPerformedInHouse;
                    SurgeryMaster.CMPID = Surgery.SurgeryMaster.CMPID;
                    SurgeryMaster.CreatedUTC = DateTime.UtcNow;
                    SurgeryMaster.CreatedBy = Surgery.Admission.CreatedBy;
                    WYNKContext.Surgery.Add(SurgeryMaster);
                    WYNKContext.SaveChanges();

                 
                    PatientAct.CMPID = Surgery.SurgeryMaster.CMPID;
                    PatientAct.UIN = Surgery.SurgeryMaster.UIN;
                    PatientAct.TotalProductValue = Surgery.SurgeryMaster.SurgeryCost;
                    PatientAct.TotalBillValue = Surgery.SurgeryMaster.SurgeryCost;
                    PatientAct.CreatedUTC = DateTime.UtcNow;
                    PatientAct.CreatedBy = Surgery.Admission.CreatedBy;
                    WYNKContext.PatientAccount.Add(PatientAct);
                    WYNKContext.SaveChanges();

                    var PAID = PatientAct.PAID;

                    PatientActDet.PAID = PAID;
                    PatientActDet.TotalProductValue = Surgery.SurgeryMaster.SurgeryCost;
                    PatientActDet.TotalBillValue = Surgery.SurgeryMaster.SurgeryCost;
                    PatientActDet.OLMID = (CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Surgery Cost").Select(x => x.OLMID).FirstOrDefault());
                    PatientActDet.ServiceTypeID = (CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Surgery Cost").Select(x => x.OLMID).FirstOrDefault());
                    PatientActDet.ServiceDescription = "Surgery Cost";
                    PatientActDet.CreatedUTC = DateTime.UtcNow;
                    PatientActDet.CreatedBy = Surgery.Admission.CreatedBy;
                    WYNKContext.PatientAccountDetail.Add(PatientActDet);
                    WYNKContext.SaveChanges();


                    var SurgeryID = SurgeryMaster.ID;

                    SurgeryTran.ICDCode = Surgery.SurgeryTran.ICDCode;
                    SurgeryTran.CreatedUTC = DateTime.UtcNow;
                    SurgeryTran.CreatedBy = Surgery.Admission.CreatedBy;
                    WYNKContext.SurgeryTran.Add(SurgeryTran);

                    SurgeryDoctor.SurgeryID = SurgeryID;
                    SurgeryDoctor.OTID = Surgery.SurgeryDoctor.OTID;
                    SurgeryDoctor.DoctorID = Surgery.SurgeryDoctor.DoctorID;
                    SurgeryDoctor.AnaestheticID = Surgery.SurgeryDoctor.AnaestheticID;
                    SurgeryDoctor.HospitalName = Surgery.SurgeryDoctor.HospitalName;
                    SurgeryDoctor.Location = Surgery.SurgeryDoctor.Location;
                    SurgeryDoctor.PhoneNumber = Surgery.SurgeryDoctor.PhoneNumber;
                    SurgeryDoctor.CreatedUTC = DateTime.UtcNow;
                    SurgeryDoctor.CreatedBy = Surgery.Admission.CreatedBy;
                    WYNKContext.SurgeryDoctor.Add(SurgeryDoctor);
                    WYNKContext.SaveChanges();
                    dbContextTransaction.Commit();

                    if (WYNKContext.SaveChanges() >= 0)
                        return new
                        {
                            Success = true,
                            Message = CommonMessage.saved
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
                Message = CommonMessage.Missing
            };
        }


        public dynamic InsertIntraoperative(SurgeryViewModel IntraOP)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    var IntraOperative = new IntraOperative();
                    IntraOperative.UIN = IntraOP.Intraoperative.UIN;
                    IntraOperative.IODate = IntraOP.Intraoperative.IODate;
                    IntraOperative.AntibioticGivenBy = IntraOP.Intraoperative.AntibioticGivenBy;
                    IntraOperative.CaseSheetPreparedBy = IntraOP.Intraoperative.CaseSheetPreparedBy;
                    IntraOperative.Instruction = IntraOP.Intraoperative.Instruction;
                    IntraOperative.Note = IntraOP.Intraoperative.Note;
                    IntraOperative.CMPID = IntraOP.Intraoperative.CMPID;
                    IntraOperative.CreatedUTC = DateTime.UtcNow;
                    IntraOperative.CreatedBy = IntraOP.Intraoperative.CreatedBy;
                    WYNKContext.IntraOperative.Add(IntraOperative);

                    dbContextTransaction.Commit();
                    if (WYNKContext.SaveChanges() >= 0)
                        return new
                        {
                            Success = true,
                            Message = CommonMessage.saved
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
                Message = CommonMessage.Missing
            };

        }



        public dynamic InsertPreoperative(SurgeryViewModel PreOP)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    var Preoperative = new PreOperative();
                    Preoperative.UIN = PreOP.PreOperative.UIN;
                    Preoperative.PreOpDate = PreOP.PreOperative.PreOpDate;
                    Preoperative.AntibioticGivenBy = PreOP.PreOperative.AntibioticGivenBy;
                    Preoperative.CaseSheetPreparedBy = PreOP.PreOperative.CaseSheetPreparedBy;
                    Preoperative.Instruction = PreOP.PreOperative.Instruction;
                    Preoperative.Note = PreOP.PreOperative.Note;
                    Preoperative.CMPID = PreOP.PreOperative.CMPID;
                    Preoperative.CreatedUTC = DateTime.UtcNow;
                    Preoperative.CreatedBy = PreOP.PreOperative.CreatedBy;
                    WYNKContext.PreOperative.Add(Preoperative);
                    dbContextTransaction.Commit();

                    if (WYNKContext.SaveChanges() >= 0)
                        return new
                        {
                            Success = true,
                            Message = CommonMessage.saved
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
                Message = CommonMessage.Missing
            };

        }



        public dynamic UpdateSurgeryMas(SurgeryViewModel UpdateSurgery, int M_AdmId, int M_surId)

        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    var Admission = new Admission();
                    var SurgeryMaster = new SurgeryMaster();
                    var SurgeryTran = new Surgery_Tran();
                    var SurgeryDoctor = new SurgeryDoctor();
                    var IntraOperative = new IntraOperative();

                    Admission = WYNKContext.Admission.Where(x => x.AdmID == M_AdmId).FirstOrDefault();
                    SurgeryMaster = WYNKContext.Surgery.Where(x => x.ID == M_surId).FirstOrDefault();
                    SurgeryDoctor = WYNKContext.SurgeryDoctor.Where(x => x.SurgeryID == M_surId).FirstOrDefault();

                    Admission.AdmDate = UpdateSurgery.Admission.AdmDate;
                    Admission.UIN = UpdateSurgery.Admission.UIN;
                    Admission.RegTranID = UpdateSurgery.Admission.RegTranID;
                    Admission.IsSurgeryCompleted = UpdateSurgery.Admission.IsSurgeryCompleted;
                    Admission.UpdatedUTC = DateTime.UtcNow;
                    Admission.UpdatedBy = UpdateSurgery.SurgeryMaster.UpdatedBy; 

                    WYNKContext.Entry(Admission).State = EntityState.Modified;

                    var AdmID = Admission.AdmID;

                    SurgeryMaster.UIN = UpdateSurgery.SurgeryMaster.UIN;
                    SurgeryMaster.RegistrationTranID = UpdateSurgery.SurgeryMaster.RegistrationTranID;
                    SurgeryMaster.OTID = UpdateSurgery.SurgeryMaster.OTID;
                    SurgeryMaster.Remarks = UpdateSurgery.SurgeryMaster.Remarks;
                    SurgeryMaster.ReviewDate = UpdateSurgery.SurgeryMaster.ReviewDate.Value.AddDays(1);
                    SurgeryMaster.SurgeryCost = UpdateSurgery.SurgeryMaster.SurgeryCost;
                    SurgeryMaster.UpdatedUTC = DateTime.UtcNow;
                    SurgeryMaster.UpdatedBy = UpdateSurgery.SurgeryMaster.UpdatedBy;
                    WYNKContext.Entry(SurgeryMaster).State = EntityState.Modified;

                    var SurgeryID = SurgeryMaster.ID;

                    SurgeryTran.ICDCode = UpdateSurgery.SurgeryTran.ICDCode;
                    SurgeryTran.UpdatedUTC = DateTime.UtcNow;
                    SurgeryTran.UpdatedBy = UpdateSurgery.SurgeryMaster.UpdatedBy;
                    WYNKContext.Entry(SurgeryTran).State = EntityState.Modified;


                    SurgeryDoctor.SurgeryID = SurgeryID;
                    SurgeryDoctor.OTID = UpdateSurgery.SurgeryDoctor.OTID;
                    SurgeryDoctor.DoctorID = UpdateSurgery.SurgeryDoctor.DoctorID;
                    SurgeryDoctor.AnaestheticID = UpdateSurgery.SurgeryDoctor.AnaestheticID;
                    SurgeryDoctor.UpdatedUTC = DateTime.UtcNow;
                    SurgeryDoctor.UpdatedBy = UpdateSurgery.SurgeryMaster.UpdatedBy;
                    WYNKContext.Entry(SurgeryDoctor).State = EntityState.Modified;
                    dbContextTransaction.Commit();

                    if (WYNKContext.SaveChanges() >= 0)
                        return new
                        {
                            Success = true,
                            Message = CommonMessage.saved
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
                Message = CommonMessage.Missing
            };
        }





    }

}