using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WYNK.Data.Common;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;

using WYNK.Data.Repository.Operation;
using WYNK.Helpers;

namespace WYNK.Data.Repository.Implementation
{
    class MedicalPrescriptionRepository : RepositoryBase<Medical_Prescription>, IMedicalPrescriptionRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;

        public MedicalPrescriptionRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }


        public IEnumerable<Dropdown> GetBrandDetails(int id)
        {
            var brnd = WYNKContext.DrugMaster.Where(x => x.ID == id).Select(x => x.Manufacturer).LastOrDefault();

            return CMPSContext.OneLineMaster.Where(x => x.OLMID == brnd).Select(x => new Dropdown { Text = x.ParentDescription, Value = x.OLMID.ToString() }).OrderBy(x => x.Text).ToList();

        }


        public Medical_Prescription GetStockDetails(int quantity, int drugid, int cmpid)
        {
            var medicalprescription = new Medical_Prescription();

            var itembal = WYNKContext.ItemBalance.ToList();
            decimal d = new decimal(quantity);

            var Datee = DateTime.Now;
            var fid = (WYNKContext.FinancialYear.Where(s => s.ID == WYNKContext.FinancialYear.Where(x => Convert.ToDateTime(x.FYFrom) <= Datee && Convert.ToDateTime(x.FYTo) >= Datee && x.CMPID == cmpid && x.IsActive == true).Select(x => x.ID).FirstOrDefault()).Select(s => s.ID).FirstOrDefault());
            medicalprescription.ACtStock = (from IB in itembal.Where(x => x.ItemID == drugid && x.ClosingBalance >= d && x.CmpID == cmpid && x.FYear == fid)
                                            select new ACtStock
                                            {
                                                quantity = IB.ClosingBalance,



                                            }).ToList();


            var StockDetail = WYNKContext.ItemBalance.Where(x => x.ItemID == drugid && x.ClosingBalance >= d && x.CmpID == cmpid && x.FYear == fid).LastOrDefault();
            var Restock = BusinessLayer.chestock(StockDetail);
            medicalprescription.Restock = Restock.quantity;
            return medicalprescription;
        }


        public Medical_Prescription GetAltdtls(int drugid, int cmpid)
        {

            var medicalprescription = new Medical_Prescription();
            var dname = WYNKContext.DrugMaster.Where(x => x.ID == drugid).Select(x => x.Brand).FirstOrDefault();
            var dgrp = WYNKContext.DrugMaster.Where(x => x.ID == drugid).Select(x => x.DrugGroup).FirstOrDefault();

            var drug = WYNKContext.DrugMaster.ToList();
            var druggrp = WYNKContext.DrugGroup.ToList();
            var altmed = WYNKContext.AlternativeDrug.ToList();

            medicalprescription.Altdrg = (from AD in altmed.Where(u => u.IsDeleted == false && u.IsActive == true && u.DrugID == drugid)
                                          join DG in drug
                                          on AD.DrugMappingID equals DG.ID
                                          join DRGP in druggrp
                                          on DG.GenericName equals DRGP.ID

                                          select new Altdrg
                                          {
                                              drugid = DG.ID,
                                              brand = DG.Brand,
                                              genid = DRGP.ID,
                                              genname = DRGP.Description,
                                              drgform = DG.DrugGroup,
                                              uom = DG.UOM,
                                          }).ToList();


            return medicalprescription;
        }


        public Medical_Prescription GetStockNo(int drugid, int cmpid)
        {
            var medicalprescription = new Medical_Prescription();

            var itembal = WYNKContext.ItemBalance.ToList();

            var Datee = DateTime.Now;
            var fid = (WYNKContext.FinancialYear.Where(s => s.ID == WYNKContext.FinancialYear.Where(x => Convert.ToDateTime(x.FYFrom) <= Datee && Convert.ToDateTime(x.FYTo) >= Datee && x.CMPID == cmpid && x.IsActive == true).Select(x => x.ID).FirstOrDefault()).Select(s => s.ID).FirstOrDefault());
            medicalprescription.ACtStockno = (from IB in itembal.Where(x => x.ItemID == drugid && x.CmpID == cmpid && x.FYear == fid)
                                              select new ACtStockno
                                              {
                                                  quantity = IB.ClosingBalance,

                                              }).ToList();


            return medicalprescription;
        }

        public Medical_Prescription Checkmedicine(string drugname, int cmpid)
        {
            var medicalprescription = new Medical_Prescription();

            var c = WYNKContext.SpecialityMaster.Where(x => x.IsActive == true && x.IsDeleted == false && x.CmpID == cmpid && x.ParentDescription.Equals(Convert.ToString(drugname), StringComparison.OrdinalIgnoreCase)).Select(y => y.ID).FirstOrDefault();

            if (c != 0)
            {
                medicalprescription.status = true;
            }
            return medicalprescription;

        }

        public Medical_Prescription Checkdform(string drugname, string dform, int cmpid)
        {
            var medicalprescription = new Medical_Prescription();

            var c = WYNKContext.SpecialityMaster.Where(x => x.IsActive == true && x.IsDeleted == false && x.CmpID == cmpid && x.ParentDescription.Replace(" ", string.Empty).ToLower().Contains(drugname)
            && x.ParentTag.Replace(" ", string.Empty).ToLower().Contains(dform)).Select(y => y.ID).FirstOrDefault();

            if (c != 0)
            {
                medicalprescription.status = true;
            }
            return medicalprescription;

        }

        public Medical_Prescription getdform(string ptag, int cmpid)
        {
            var medicalprescription = new Medical_Prescription();

            medicalprescription.drform = WYNKContext.SpecialityMaster.Where(x => x.IsActive == true && x.IsDeleted == false && x.CmpID == cmpid && x.ParentTag.Replace(" ", string.Empty).ToLower().Contains(ptag)).Select(y => y.ParentDescription).FirstOrDefault();
            return medicalprescription;

        }
        public Medical_Prescription Checkfqy(string fyname)
        {
            var medicalprescription = new Medical_Prescription();

            var c = CMPSContext.OneLineMaster.Where(x => x.IsActive == true && x.IsDeleted == false && x.ParentTag == "FY" && x.ParentDescription.Equals(Convert.ToString(fyname), StringComparison.OrdinalIgnoreCase)).Select(y => y.OLMID).FirstOrDefault();

            if (c != 0)
            {
                medicalprescription.statusf = true;
            }
            return medicalprescription;

        }

        public Medical_Prescription Checkfod(string fdname)
        {
            var medicalprescription = new Medical_Prescription();

            var c = CMPSContext.OneLineMaster.Where(x => x.IsActive == true && x.IsDeleted == false && x.ParentTag == "FD" && x.ParentDescription.Equals(Convert.ToString(fdname), StringComparison.OrdinalIgnoreCase)).Select(y => y.OLMID).FirstOrDefault();

            if (c != 0)
            {
                medicalprescription.statusfd = true;
            }
            return medicalprescription;

        }

        public Medical_Prescription Getallergyinfo(int Drugid, string UIN, int cmpid)
        {

            var medicalprescription = new Medical_Prescription();

            var drug = WYNKContext.DrugGroup.ToList();
            var alltran = WYNKContext.AllergyTran.ToList();


            medicalprescription.alldet = (from AT in alltran.Where(u => u.UIN == UIN && u.CmpID == cmpid && u.Description == Drugid && u.IsActive == true)
                                          join DG in drug on AT.Description equals DG.ID


                                          select new alldet
                                          {
                                              dname = DG.Description,


                                          }).ToList();


            return medicalprescription;
        }

        public Medical_Prescription GetPatientDetails(string UIN, int rid)
        {

            var medicalprescription = new Medical_Prescription();
            medicalprescription.MPT = new List<MedicalPrescriptionTran>();
            medicalprescription.MedicalPrescription = new MedicalPrescription();
            medicalprescription.MedicalPrescriptionTran = new MedicalPrescriptionTran();
            medicalprescription.DrugInfo = new List<DrugInfo>();
            medicalprescription.UploadMedicalPrescription = new List<UploadMedicalPrescription>();
            medicalprescription.InvImg = new List<InvImg>();


            var med = WYNKContext.MedicalPrescription.ToList();
            var reg = WYNKContext.MedicalPrescription.Where(x => x.UIN == UIN).Select(x => x.RegistrationTranID).LastOrDefault();
            var id = WYNKContext.MedicalPrescription.Where(x => x.RegistrationTranID == reg).Select(x => x.MedicalPrescriptionID).LastOrDefault();
            medicalprescription.MedicalPrescription.MedicalPrescriptionNo = WYNKContext.MedicalPrescription.Where(u => u.UIN == UIN).OrderByDescending(x => x.CreatedUTC).Select(x => x.MedicalPrescriptionNo).FirstOrDefault();

            medicalprescription.MedBill = (from MP in med.Where(u => u.MedicalPrescriptionID == id && u.CmpID == rid)


                                           select new MedBill
                                           {
                                               Bill = MP.MedicalPrescriptionNo,
                                               BillDate = MP.PrescribedDate.Date.ToString("dd-MMM-yyyy"),
                                               DocName = MP.PrescribedByName,


                                           }).ToList();


            return medicalprescription;
        }
        public Medical_Prescription GetHistoryDetails(string UIN, int rid, string gmt)
        {

            var medicalprescription = new Medical_Prescription();

            var med = WYNKContext.MedicalPrescription.ToList();
            var Company = CMPSContext.Company.ToList();
            int cmpid = CMPSContext.Company.Where(c => c.CmpID == rid).Select(c => c.ParentID).FirstOrDefault();
            if (cmpid == 0)
            {
                cmpid = rid;
            }

            TimeSpan ts = TimeSpan.Parse(gmt);

            medicalprescription.MedBills = (from MP in med.Where(u => u.UIN == UIN && u.DischargeID == null && u.SurgeryID == null).OrderByDescending(x => x.CreatedUTC)
                                            join cmp in Company.Where(x => x.ParentID == cmpid || x.CmpID == cmpid) on MP.CmpID equals cmp.CmpID
                                            select new MedBills
                                            {
                                                Bill = MP.MedicalPrescriptionNo,
                                                Id = MP.RandomUniqueID,
                                                BillDate = MP.PrescribedDate.Add(ts),
                                                DocName = MP.PrescribedByName,
                                                CmpName = cmp.CompanyName + "-" + cmp.Address1,
                                            }).ToList();


            return medicalprescription;
        }



        public Medical_Prescription GetHistoryDetailsm(string UIN, int rid, string gmt)
        {

            var medicalprescription = new Medical_Prescription();

            var med = WYNKContext.MedicalPrescription.ToList();
            var Company = CMPSContext.Company.ToList();
            int cmpid = CMPSContext.Company.Where(c => c.CmpID == rid).Select(c => c.ParentID).FirstOrDefault();
            if (cmpid == 0)
            {
                cmpid = rid;
            }

            TimeSpan ts = TimeSpan.Parse(gmt);

            medicalprescription.MedBills = (from MP in med.Where(u => u.UIN == UIN).OrderByDescending(x => x.CreatedUTC)
                                            join cmp in Company.Where(x => x.ParentID == cmpid || x.CmpID == cmpid) on MP.CmpID equals cmp.CmpID
                                            select new MedBills
                                            {
                                                Bill = MP.MedicalPrescriptionNo,
                                                Id = MP.RandomUniqueID,
                                                BillDate = MP.PrescribedDate.Add(ts),
                                                DocName = MP.PrescribedByName,
                                                CmpName = cmp.CompanyName + "-" + cmp.Address1,
                                            }).ToList();


            return medicalprescription;
        }




        public Medical_Prescription GetUINDetails(int cid)
        {

            var medicalprescription = new Medical_Prescription();
            medicalprescription.MPT = new List<MedicalPrescriptionTran>();
            medicalprescription.MedicalPrescription = new MedicalPrescription();
            medicalprescription.MedicalPrescriptionTran = new MedicalPrescriptionTran();
            medicalprescription.DrugInfo = new List<DrugInfo>();

            var med = WYNKContext.MedicalPrescription.ToList();
            var reg = WYNKContext.Registration.ToList();



            var meddt = (from MP in med.Where(x => x.CmpID == cid).GroupBy(x => x.UIN)


                         select new
                         {
                             uin = MP.Select(x => x.UIN).FirstOrDefault(),

                         }).ToList();


            medicalprescription.UinDet = (from M in meddt
                                          join R in reg
                                          on M.uin equals R.UIN

                                          select new UinDet
                                          {
                                              UIN = R.UIN,
                                              name = R.Name + " " + R.MiddleName + " " + R.LastName,
                                              gender = R.Gender,
                                              age = PasswordEncodeandDecode.ToAgeString(R.DateofBirth),
                                              addr1 = R.Address1,
                                              addr2 = R.Address2,
                                              addr3 = R.Address3,
                                              phone = R.Phone,

                                          }).ToList();


            return medicalprescription;
        }


        public Medical_Prescription GetMedicineDetails(string ID, DateTime presdate, int rid)

        {

            var medicalprescription = new Medical_Prescription();

            medicalprescription.MPT = new List<MedicalPrescriptionTran>();
            medicalprescription.MedicalPrescription = new MedicalPrescription();
            medicalprescription.MedicalPrescriptionTran = new MedicalPrescriptionTran();
            medicalprescription.DrugInfo = new List<DrugInfo>();

            var reg = WYNKContext.MedicalPrescription.Where(x => x.UIN == ID).Select(x => x.RegistrationTranID).LastOrDefault();

            var id = WYNKContext.MedicalPrescription.Where(x => x.RegistrationTranID == reg).Select(x => x.MedicalPrescriptionID).LastOrDefault();

            medicalprescription.Registration = WYNKContext.Registration.FirstOrDefault(x => x.UIN == ID);


            var med = WYNKContext.MedicalPrescription.ToList();
            var medtran = WYNKContext.MedicalPrescriptionTran.ToList();
            var drug = WYNKContext.DrugMaster.ToList();
            var drugrp = WYNKContext.DrugGroup.ToList();

            var pres = WYNKContext.MedicalPrescription.Where(x => x.UIN == ID).Select(x => x.MedicalPrescriptionID).LastOrDefault();

            medicalprescription.MedData = (from MP in med.Where(u => u.MedicalPrescriptionID == pres && u.PrescribedDate.Date == presdate.Date)
                                           join MPT in medtran
                                           on MP.RandomUniqueID equals MPT.MedicalPrescriptionID
                                           where MP.CmpID == rid


                                           select new MedData
                                           {
                                               DrugName = MPT.Brand,
                                               Eye = MPT.Eye,
                                               Dose = MPT.Dossage,
                                               Freq = MPT.Frequency,
                                               Foodd = MPT.Food,
                                               Tdays = Convert.ToInt32(MPT.Days),
                                               Quant = MPT.Quantity,

                                           }).ToList();

            return medicalprescription;
        }
        public Medical_Prescription GetAllMedicineDetails(string rid)

        {

            var medicalprescription = new Medical_Prescription();

            medicalprescription.MPT = new List<MedicalPrescriptionTran>();
            medicalprescription.MedicalPrescription = new MedicalPrescription();
            medicalprescription.MedicalPrescriptionTran = new MedicalPrescriptionTran();
            medicalprescription.DrugInfo = new List<DrugInfo>();


            var med = WYNKContext.MedicalPrescription.ToList();
            var medtran = WYNKContext.MedicalPrescriptionTran.ToList();
            var drug = WYNKContext.DrugMaster.ToList();
            var drugrp = WYNKContext.DrugGroup.ToList();


            medicalprescription.MedDatas = (from MP in med.Where(x => x.RandomUniqueID == rid)
                                            join MPT in medtran
                                            on MP.RandomUniqueID equals MPT.MedicalPrescriptionID


                                            select new MedDatas
                                            {
                                                DrugName = MPT.Brand,
                                                Eye = MPT.Eye,
                                                Dose = MPT.Dossage,
                                                Freq = MPT.Frequency,
                                                Foodd = MPT.Food,
                                                Tdays = Convert.ToInt32(MPT.Days),
                                                Quant = MPT.Quantity,
                                                uom = MPT.DrugForm,
                                            }).ToList();

            var cid = WYNKContext.MedicalPrescription.Where(x => x.RandomUniqueID == rid).Select(x => x.CmpID).FirstOrDefault();

            var regid = WYNKContext.MedicalPrescription.Where(x => x.RandomUniqueID == rid).Select(x => x.RegistrationTranID).FirstOrDefault();

            var uin = WYNKContext.RegistrationTran.Where(x => x.RegistrationTranID == regid).Select(x => x.UIN).FirstOrDefault();

            medicalprescription.Registration = WYNKContext.Registration.Where(x => x.UIN == uin).FirstOrDefault();

            medicalprescription.rpage = DateTime.Now.Year - medicalprescription.Registration.DateofBirth.Year;


            medicalprescription.rCAddress = CMPSContext.Company.Where(x => x.CmpID == cid).Select(x => x.Address1).FirstOrDefault();

            medicalprescription.rCphone = CMPSContext.Company.Where(x => x.CmpID == cid).Select(x => x.Phone1).FirstOrDefault();

            medicalprescription.rCweb = CMPSContext.Company.Where(x => x.CmpID == cid).Select(x => x.Website).FirstOrDefault();

            medicalprescription.rCname = CMPSContext.Company.Where(x => x.CmpID == cid).Select(x => x.CompanyName).FirstOrDefault();
            medicalprescription.allergy = WYNKContext.PatientGeneral.Where(x => x.RegistrationTranID == regid).Select(x => x.Allergy).FirstOrDefault();
            medicalprescription.remsrks = WYNKContext.MedicalPrescriptionTran.Where(x => x.MedicalPrescriptionID == rid).Select(x => x.Remarks).FirstOrDefault();
            medicalprescription.reviewdates = WYNKContext.Findings.Where(x => x.RegistrationTranID == regid).Select(x => x.ReviewDate).FirstOrDefault();
            medicalprescription.docnamelast = WYNKContext.MedicalPrescription.Where(x => x.RandomUniqueID == rid).Select(x => x.PrescribedByName).FirstOrDefault();
            var docidss = WYNKContext.MedicalPrescription.Where(x => x.RandomUniqueID == rid).Select(x => x.PrescribedBy).FirstOrDefault();
            medicalprescription.docno = CMPSContext.DoctorMaster.Where(x => x.DoctorID == docidss).Select(x => x.RegistrationNumber).FirstOrDefault();

            medicalprescription.regname = WYNKContext.Registration.Where(x => x.UIN == uin).Select(x => x.Name).FirstOrDefault();

            medicalprescription.regnamemid = WYNKContext.Registration.Where(x => x.UIN == uin).Select(x => x.MiddleName).FirstOrDefault();
            medicalprescription.regnamelast = WYNKContext.Registration.Where(x => x.UIN == uin).Select(x => x.LastName).FirstOrDefault();


            return medicalprescription;
        }

        public Medical_Prescription GetAllMedicineDetailsm(string rid)

        {

            var medicalprescription = new Medical_Prescription();

            medicalprescription.MPT = new List<MedicalPrescriptionTran>();
            medicalprescription.MedicalPrescription = new MedicalPrescription();
            medicalprescription.MedicalPrescriptionTran = new MedicalPrescriptionTran();
            medicalprescription.DrugInfo = new List<DrugInfo>();


            var med = WYNKContext.MedicalPrescription.ToList();
            var medtran = WYNKContext.MedicalPrescriptionTran.ToList();
            var Specmas = WYNKContext.SpecialityMaster.ToList();

            medicalprescription.MedDatas = (from MP in med.Where(x => x.RandomUniqueID == rid)
                                            join MPT in medtran
                                            on MP.RandomUniqueID equals MPT.MedicalPrescriptionID
                                            join SM in Specmas
                                            on MPT.DrugId equals SM.ID


                                            select new MedDatas
                                            {
                                                DrugName = SM.ParentDescription,
                                                Eye = MPT.Eye,
                                                Dose = MPT.Dossage,
                                                Freq = MPT.Frequency,
                                                Foodd = MPT.Food,
                                                Tdays = Convert.ToInt32(MPT.Days),
                                                Quant = MPT.Quantity,
                                                uom = MPT.DrugForm,
                                            }).ToList();

            var cid = WYNKContext.MedicalPrescription.Where(x => x.RandomUniqueID == rid).Select(x => x.CmpID).FirstOrDefault();

            var regid = WYNKContext.MedicalPrescription.Where(x => x.RandomUniqueID == rid).Select(x => x.RegistrationTranID).FirstOrDefault();

            var uin = WYNKContext.RegistrationTran.Where(x => x.RegistrationTranID == regid).Select(x => x.UIN).FirstOrDefault();

            medicalprescription.Registration = WYNKContext.Registration.Where(x => x.UIN == uin).FirstOrDefault();

            medicalprescription.rpage = DateTime.Now.Year - medicalprescription.Registration.DateofBirth.Year;


            medicalprescription.rCAddress = CMPSContext.Company.Where(x => x.CmpID == cid).Select(x => x.Address1).FirstOrDefault();

            medicalprescription.rCphone = CMPSContext.Company.Where(x => x.CmpID == cid).Select(x => x.Phone1).FirstOrDefault();

            medicalprescription.rCweb = CMPSContext.Company.Where(x => x.CmpID == cid).Select(x => x.Website).FirstOrDefault();

            medicalprescription.rCname = CMPSContext.Company.Where(x => x.CmpID == cid).Select(x => x.CompanyName).FirstOrDefault();
            medicalprescription.allergy = WYNKContext.PatientGeneral.Where(x => x.RegistrationTranID == regid).Select(x => x.Allergy).FirstOrDefault();
            medicalprescription.remsrks = WYNKContext.MedicalPrescriptionTran.Where(x => x.MedicalPrescriptionID == rid).Select(x => x.Remarks).FirstOrDefault();
            medicalprescription.reviewdates = WYNKContext.Findings.Where(x => x.RegistrationTranID == regid).Select(x => x.ReviewDate).FirstOrDefault();
            medicalprescription.docnamelast = WYNKContext.MedicalPrescription.Where(x => x.RandomUniqueID == rid).Select(x => x.PrescribedByName).FirstOrDefault();
            var docidss = WYNKContext.MedicalPrescription.Where(x => x.RandomUniqueID == rid).Select(x => x.PrescribedBy).FirstOrDefault();
            medicalprescription.docno = CMPSContext.DoctorMaster.Where(x => x.DoctorID == docidss).Select(x => x.RegistrationNumber).FirstOrDefault();

            medicalprescription.regname = WYNKContext.Registration.Where(x => x.UIN == uin).Select(x => x.Name).FirstOrDefault();

            medicalprescription.regnamemid = WYNKContext.Registration.Where(x => x.UIN == uin).Select(x => x.MiddleName).FirstOrDefault();
            medicalprescription.regnamelast = WYNKContext.Registration.Where(x => x.UIN == uin).Select(x => x.LastName).FirstOrDefault();


            return medicalprescription;
        }

        public Medical_Prescription TapperedDetailsdelete(int drugid, int DoctorId, int cmpid)

        {

            var medicalprescription = new Medical_Prescription();

            medicalprescription.MPT = new List<MedicalPrescriptionTran>();
            medicalprescription.MedicalPrescription = new MedicalPrescription();
            medicalprescription.MedicalPrescriptionTran = new MedicalPrescriptionTran();
            medicalprescription.DrugInfo = new List<DrugInfo>();

            var drugrp = WYNKContext.DrugGroup.ToList();
            var onelinemasterss = CMPSContext.OneLineMaster.ToList();
            var drugtapper = WYNKContext.DrugTappering.ToList();

            var master = WYNKContext.DrugTappering.Where(x => x.DrugID == drugid && x.DoctorID == DoctorId && x.CmpID == cmpid).ToList();
            if (master != null)
            {

                WYNKContext.DrugTappering.RemoveRange(master);
                WYNKContext.SaveChanges();
            }


            return medicalprescription;
        }


        public Medical_Prescription ViewTempDetailsdel(string Drugname, int DoctorId, int cmpid)

        {

            var medicalprescription = new Medical_Prescription();

            medicalprescription.MPT = new List<MedicalPrescriptionTran>();
            medicalprescription.MedicalPrescription = new MedicalPrescription();
            medicalprescription.MedicalPrescriptionTran = new MedicalPrescriptionTran();
            medicalprescription.DrugInfo = new List<DrugInfo>();

            var master = WYNKContext.DrugTemplate.Where(x => x.Description == Drugname && x.DoctorID == DoctorId && x.CmpID == cmpid).ToList();
            if (master != null)
            {

                WYNKContext.DrugTemplate.RemoveRange(master);
                WYNKContext.SaveChanges();
            }


            return medicalprescription;
        }


        public Medical_Prescription GetTapperingDetails(int medid, int docid, int cmpid)

        {

            var medicalprescription = new Medical_Prescription();

            medicalprescription.MPT = new List<MedicalPrescriptionTran>();
            medicalprescription.MedicalPrescription = new MedicalPrescription();
            medicalprescription.MedicalPrescriptionTran = new MedicalPrescriptionTran();
            medicalprescription.DrugInfo = new List<DrugInfo>();

            var drugrp = WYNKContext.DrugGroup.ToList();
            var onelinemasterss = CMPSContext.OneLineMaster.ToList();
            var drugtapper = WYNKContext.DrugTappering.ToList();


            medicalprescription.TapperData = (from DT in drugtapper.Where(u => u.DrugID == medid && u.DoctorID == docid && u.CmpID == cmpid).OrderByDescending(x => x.ID)
                                              join dg in drugrp on DT.DrugID equals dg.ID

                                              select new TapperData
                                              {
                                                  ICD_DESCRIPTION = WYNKContext.DrugMaster.Where(x => x.GenericName == medid).Select(x => x.Brand).FirstOrDefault(),
                                                  Brand = dg.Description,
                                                  UOM = WYNKContext.DrugMaster.Where(x => x.GenericName == medid).Select(x => x.UOM).FirstOrDefault(),
                                                  DrugID = WYNKContext.DrugMaster.Where(x => x.GenericName == medid).Select(x => x.ID).FirstOrDefault(),
                                                  medicineid = Convert.ToInt32(dg.ID),
                                                  Frequency = DT.Frequency,
                                                  Quantity = DT.Quantity,
                                                  Food = DT.Food,
                                                  Days = DT.Days,
                                                  sideeffect = dg.SideEffects,
                                                  precaution = dg.Precautions,
                                                  overdose = dg.Overdose,
                                                  dform = WYNKContext.DrugMaster.Where(x => x.GenericName == medid).Select(x => x.DrugGroup).FirstOrDefault(),
                                                  manufac = onelinemasterss.Where(x => x.OLMID == WYNKContext.DrugMaster.Where(s => s.GenericName == medid).Select(s => s.Manufacturer).FirstOrDefault()).Select(x => x.ParentDescription).FirstOrDefault(),

                                              }).ToList();



            return medicalprescription;
        }


        public Medical_Prescription TapperedDetails(int DoctorId, int cmpid)

        {

            var medicalprescription = new Medical_Prescription();

            medicalprescription.MPT = new List<MedicalPrescriptionTran>();
            medicalprescription.MedicalPrescription = new MedicalPrescription();
            medicalprescription.MedicalPrescriptionTran = new MedicalPrescriptionTran();
            medicalprescription.DrugInfo = new List<DrugInfo>();

            var drugrp = WYNKContext.DrugGroup.ToList();
            var drugtapper = WYNKContext.DrugTappering.ToList();


            var res = (from DT in drugtapper.Where(u => u.DoctorID == DoctorId && u.CmpID == cmpid)
                       join dg in drugrp on DT.DrugID equals dg.ID

                       select new
                       {
                           drugid = DT.DrugID,
                           drugname = dg.Description,


                       }).ToList();


            medicalprescription.TapDrug = (from R in res.GroupBy(x => x.drugid)
                                           select new TapDrug
                                           {
                                               drugid = R.Select(x => x.drugid).LastOrDefault(),
                                               drugname = R.Select(x => x.drugname).LastOrDefault(),

                                           }).ToList();



            return medicalprescription;
        }

        public Medical_Prescription TapperedDetailsall(int drugid, int DoctorId, int cmpid)

        {

            var medicalprescription = new Medical_Prescription();

            medicalprescription.MPT = new List<MedicalPrescriptionTran>();
            medicalprescription.MedicalPrescription = new MedicalPrescription();
            medicalprescription.MedicalPrescriptionTran = new MedicalPrescriptionTran();
            medicalprescription.DrugInfo = new List<DrugInfo>();

            var drugrp = WYNKContext.DrugGroup.ToList();
            var drugtapper = WYNKContext.DrugTappering.ToList();




            medicalprescription.Tapdetails = (from DT in drugtapper.Where(u => u.DrugID == drugid && u.DoctorID == DoctorId && u.CmpID == cmpid).OrderByDescending(x => x.Days)
                                              join dg in drugrp on DT.DrugID equals dg.ID
                                              select new Tapdetails
                                              {
                                                  drugname = dg.Description,
                                                  freq = DT.Frequency,
                                                  days = DT.Days,
                                                  food = DT.Food,
                                                  quant = DT.Quantity,

                                              }).ToList();



            return medicalprescription;
        }

        public dynamic UpdateFreq(Medical_Prescription MedicalPrescription)
        {



            var olm = new OneLine_Masters();

            olm.ParentDescription = MedicalPrescription.OneLineMaster.ParentDescription;
            olm.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Frequency").Select(x => x.OLMID).FirstOrDefault();
            olm.ParentTag = "FY";
            olm.IsActive = true;
            olm.IsDeleted = false;
            olm.CreatedUTC = DateTime.UtcNow;
            olm.CreatedBy = MedicalPrescription.OneLineMaster.CreatedBy;

            CMPSContext.OneLineMaster.Add(olm);


            try
            {
                if (CMPSContext.SaveChanges() >= 0)
                    return new
                    {

                        Success = true,
                        Message = CommonMessage.saved,
                    };
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Success = false,
                Message = CommonMessage.Missing,
            };



        }


        public dynamic UpdateFood(Medical_Prescription MedicalPrescription)
        {



            var olm = new OneLine_Masters();

            olm.ParentDescription = MedicalPrescription.OneLineMaster.ParentDescription;
            olm.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Drug Instruction").Select(x => x.OLMID).FirstOrDefault();
            olm.ParentTag = "FD";
            olm.IsActive = true;
            olm.IsDeleted = false;
            olm.CreatedUTC = DateTime.UtcNow;
            olm.CreatedBy = MedicalPrescription.OneLineMaster.CreatedBy;
            CMPSContext.OneLineMaster.Add(olm);


            try
            {
                if (CMPSContext.SaveChanges() >= 0)
                    return new
                    {

                        Success = true,
                        Message = CommonMessage.saved,
                    };
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Success = false,
                Message = CommonMessage.Missing,
            };



        }



        public dynamic UpdateMedicalPrescription(Medical_Prescription MedicalPrescription, int cmpPid, int TransactionTypeid, string cmpname, string dcname)
        {

            try
            {

                var MedPres = new MedicalPrescription();

                var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == MedicalPrescription.MedicalPrescription.UIN && x.CmpID == cmpPid).Select(x => x.RegistrationTranID).LastOrDefault();
                var docid = WYNKContext.PatientAssign.Where(x => x.RegistrationTranID == regid).Select(x => x.DoctorID).FirstOrDefault();
                var docname = CMPSContext.DoctorMaster.Where(x => x.DoctorID == docid).Select(x => x.LastName).FirstOrDefault();

                MedPres.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                MedPres.RegistrationTranID = regid;
                MedPres.UIN = MedicalPrescription.MedicalPrescription.UIN;
                MedPres.MedicalPrescriptionNo = MedicalPrescription.MedicalPrescription.MedicalPrescriptionNo;
                var Datee = DateTime.Now;
                MedPres.Fyear = Convert.ToString(WYNKContext.FinancialYear.Where(s => s.ID == WYNKContext.FinancialYear.Where(x => Convert.ToDateTime(x.FYFrom) <= Datee && Convert.ToDateTime(x.FYTo) >= Datee && x.CMPID == cmpPid && x.IsActive == true).Select(x => x.ID).FirstOrDefault()).Select(s => s.FYAccYear).FirstOrDefault());
                MedPres.PrescribedBy = MedicalPrescription.MedicalPrescription.PrescribedBy;
                MedPres.PrescribedByName = MedicalPrescription.MedicalPrescription.PrescribedByName;
                MedPres.PrescribedDate = DateTime.UtcNow;
                MedPres.CmpID = MedicalPrescription.MedicalPrescription.CmpID;
                MedPres.Status = "Open";
                MedPres.IsDeleted = false;
                MedPres.CreatedUTC = DateTime.UtcNow;
                MedPres.CreatedBy = MedicalPrescription.MedicalPrescription.CreatedBy;
                MedPres.Transactionid = MedicalPrescription.MedicalPrescription.Transactionid;

                WYNKContext.MedicalPrescription.Add(MedPres);
                WYNKContext.SaveChanges();

                string userid = Convert.ToString(MedicalPrescription.MedicalPrescription.CreatedBy);
                ErrorLog oErrorLogs = new ErrorLog();
                oErrorLogs.WriteErrorLogTitle(cmpname, "Medical Prescription", "User name :", dcname, "User ID :", userid, "Mode : Submit");

                object names = MedPres;
                oErrorLogs.WriteErrorLogArray("MedicalPrescription", names);
                var ranunid = MedPres.RandomUniqueID;
                var crby = MedPres.CreatedBy;

                if (MedicalPrescription.MPT.Count() > 0)
                {
                    foreach (var item in MedicalPrescription.MPT.ToList())


                    {


                        var MedPrescTran = new MedicalPrescriptionTran();

                        MedPrescTran.MedicalPrescriptionID = ranunid;
                        MedPrescTran.DrugId = item.DrugId;
                        MedPrescTran.Dossage = item.Dossage;
                        MedPrescTran.Frequency = item.Frequency;
                        MedPrescTran.Quantity = item.Quantity;
                        MedPrescTran.Eye = item.Eye;
                        MedPrescTran.Food = item.Food;
                        MedPrescTran.Days = item.Days;
                        MedPrescTran.Brand = item.Brand;
                        MedPrescTran.DrugForm = item.DrugForm;
                        MedPrescTran.Remarks = MedicalPrescription.MedicalPrescriptionTran.Remarks;
                        MedPrescTran.CreatedUTC = DateTime.UtcNow;
                        MedPrescTran.CreatedBy = crby;
                        WYNKContext.MedicalPrescriptionTran.AddRange(MedPrescTran);

                        ErrorLog oErrorLogstran = new ErrorLog();
                        object namestr = MedPrescTran;
                        oErrorLogstran.WriteErrorLogArray("MedicalPrescriptionTran", namestr);
                    }
                }

                WYNKContext.SaveChanges();

                var commonRepos = new CommonRepository(_Wynkcontext, _Cmpscontext);
                var RunningNumber = commonRepos.GenerateRunningCtrlNoo(TransactionTypeid, cmpPid, "GetRunningNo");

                if (RunningNumber == MedicalPrescription.MedicalPrescription.MedicalPrescriptionNo)
                {
                    commonRepos.GenerateRunningCtrlNoo(TransactionTypeid, cmpPid, "UpdateRunningNo");
                }
                else
                {
                    var GetRunningNumber = commonRepos.GenerateRunningCtrlNoo(TransactionTypeid, cmpPid, "UpdateRunningNo");

                    var MedicalPres = WYNKContext.MedicalPrescription.Where(x => x.MedicalPrescriptionNo == MedicalPrescription.MedicalPrescription.MedicalPrescriptionNo).FirstOrDefault();
                    MedicalPres.MedicalPrescriptionNo = GetRunningNumber;
                    WYNKContext.MedicalPrescription.UpdateRange(MedicalPres);

                    WYNKContext.SaveChanges();
                }



                if (WYNKContext.SaveChanges() >= 0)
                {
                    ErrorLog oErrorLog = new ErrorLog();
                    oErrorLog.WriteErrorLog("Information :", "Saved Successfully");
                }
                MedicalPrescription.regid = WYNKContext.MedicalPrescription.Where(x => x.UIN == MedicalPrescription.MedicalPrescription.UIN).Select(x => x.RegistrationTranID).FirstOrDefault();

                MedicalPrescription.medid = WYNKContext.MedicalPrescription.Where(x => x.RegistrationTranID == MedicalPrescription.regid).Select(x => x.MedicalPrescriptionID).LastOrDefault();




                var medicalprescription = new Medical_Prescription();



                var regidss = WYNKContext.RegistrationTran.Where(x => x.UIN == MedicalPrescription.MedicalPrescription.UIN && x.CmpID == cmpPid).Select(x => x.RegistrationTranID).LastOrDefault();

                var docnamelast = WYNKContext.MedicalPrescription.Where(x => x.RegistrationTranID == regid).Select(x => x.PrescribedByName).LastOrDefault();

                var docidss = WYNKContext.MedicalPrescription.Where(x => x.RegistrationTranID == regid).Select(x => x.PrescribedBy).LastOrDefault();


                var docno = CMPSContext.DoctorMaster.Where(x => x.DoctorID == docidss).Select(x => x.RegistrationNumber).FirstOrDefault();



                var medi1 = WYNKContext.MedicalPrescription.Where(x => x.UIN == MedicalPrescription.MedicalPrescription.UIN && x.CmpID == cmpPid).Select(x => x.RandomUniqueID).LastOrDefault();


                var remsrks = WYNKContext.MedicalPrescriptionTran.Where(x => x.MedicalPrescriptionID == medi1).Select(x => x.Remarks).FirstOrDefault();

                var regids = WYNKContext.RegistrationTran.Where(x => x.UIN == MedicalPrescription.MedicalPrescription.UIN && x.CmpID == cmpPid).Select(x => x.UIN).FirstOrDefault();
                var reviewdates = WYNKContext.Findings.Where(x => x.UIN == regids).Select(x => x.ReviewDate).FirstOrDefault();
                var regname = WYNKContext.Registration.Where(x => x.UIN == regids).Select(x => x.Name).FirstOrDefault();

                var regnamemid = WYNKContext.Registration.Where(x => x.UIN == regids).Select(x => x.MiddleName).FirstOrDefault();
                var regnamelast = WYNKContext.Registration.Where(x => x.UIN == regids).Select(x => x.LastName).FirstOrDefault();

                var genders = WYNKContext.Registration.Where(x => x.UIN == regids).Select(x => x.Gender).FirstOrDefault();

                var allergy = WYNKContext.PatientGeneral.Where(x => x.UIN == regids).Select(x => x.Allergy).FirstOrDefault();


                var CAddress = CMPSContext.Company.Where(x => x.CmpID == MedicalPrescription.MedicalPrescription.CmpID).Select(x => x.Address1).FirstOrDefault();

                var Cphone = CMPSContext.Company.Where(x => x.CmpID == MedicalPrescription.MedicalPrescription.CmpID).Select(x => x.Phone1).FirstOrDefault();

                var Cweb = CMPSContext.Company.Where(x => x.CmpID == MedicalPrescription.MedicalPrescription.CmpID).Select(x => x.Website).FirstOrDefault();

                var Cname = CMPSContext.Company.Where(x => x.CmpID == MedicalPrescription.MedicalPrescription.CmpID).Select(x => x.CompanyName).FirstOrDefault();

                var medtran = WYNKContext.MedicalPrescriptionTran.ToList();
                var drug = WYNKContext.DrugMaster.ToList();
                var onelinemasterss = CMPSContext.OneLineMaster.ToList();
                var druggroup = WYNKContext.DrugGroup.ToList();


                medicalprescription.Patientlist = (from MPT in WYNKContext.MedicalPrescriptionTran.Where(u => u.MedicalPrescriptionID == medi1)
                                                   select new Patientlist
                                                   {
                                                       Brand = MPT.Brand,
                                                       Dossage = MPT.Dossage,
                                                       Frequency = MPT.Frequency,
                                                       Food = MPT.Food,
                                                       Dayss = Convert.ToInt32(MPT.Days),
                                                       Quant = MPT.Quantity,
                                                       PrescribedDate = MPT.CreatedUTC.Date,
                                                       Remarks = MPT.Remarks,
                                                       Eye = MPT.Eye,
                                                       uom = MPT.DrugForm,

                                                   }).ToList();

                return new
                {

                    reguin = regids,
                    regnames = regname,
                    regnamesmid = regnamemid,
                    regnameslast = regnamelast,
                    aller = allergy,
                    gend = genders,
                    remarks = remsrks,
                    rdate = reviewdates,
                    coname = Cname,
                    caddrs = CAddress,
                    cphone = Cphone,
                    cwebb = Cweb,
                    dcnum = docno,
                    dname3 = docnamelast,
                    Uin = medicalprescription.Patientlist.ToList(),
                    Success = true,
                    Message = CommonMessage.saved,
                };
            }
            catch (Exception ex)
            {
                ErrorLog oErrorLog = new ErrorLog();
                oErrorLog.WriteErrorLog("Error :", ex.InnerException.Message.ToString());
                Console.Write(ex);
                string msg = ex.InnerException.Message;
                return new { Success = false, Message = msg, grn = MedicalPrescription.MedicalPrescription.MedicalPrescriptionNo };



            }



        }



        public dynamic UpdateMedicalPrescriptionnew(Medical_Prescription MedicalPrescription, int cmpPid, int TransactionTypeid, string cmpname, string dcname)
        {

            try
            {

                var MedPres = new MedicalPrescription();

                var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == MedicalPrescription.MedicalPrescription.UIN && x.CmpID == cmpPid).Select(x => x.RegistrationTranID).LastOrDefault();
                var docid = WYNKContext.PatientAssign.Where(x => x.RegistrationTranID == regid).Select(x => x.DoctorID).FirstOrDefault();
                var docname = CMPSContext.DoctorMaster.Where(x => x.DoctorID == docid).Select(x => x.LastName).FirstOrDefault();

                MedPres.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                MedPres.RegistrationTranID = regid;
                MedPres.UIN = MedicalPrescription.MedicalPrescription.UIN;
                MedPres.MedicalPrescriptionNo = MedicalPrescription.MedicalPrescription.MedicalPrescriptionNo;
                var Datee = DateTime.Now;
                MedPres.Fyear = Convert.ToString(WYNKContext.FinancialYear.Where(s => s.ID == WYNKContext.FinancialYear.Where(x => Convert.ToDateTime(x.FYFrom) <= Datee && Convert.ToDateTime(x.FYTo) >= Datee && x.CMPID == cmpPid && x.IsActive == true).Select(x => x.ID).FirstOrDefault()).Select(s => s.FYAccYear).FirstOrDefault());
                MedPres.PrescribedBy = MedicalPrescription.MedicalPrescription.PrescribedBy;
                MedPres.PrescribedByName = MedicalPrescription.MedicalPrescription.PrescribedByName;
                MedPres.PrescribedDate = DateTime.UtcNow;
                MedPres.CmpID = MedicalPrescription.MedicalPrescription.CmpID;
                MedPres.Status = "Open";
                MedPres.IsDeleted = false;
                MedPres.CreatedUTC = DateTime.UtcNow;
                MedPres.CreatedBy = MedicalPrescription.MedicalPrescription.CreatedBy;
                MedPres.Transactionid = MedicalPrescription.MedicalPrescription.Transactionid;
                MedPres.Tag = "N";
                WYNKContext.MedicalPrescription.Add(MedPres);
                WYNKContext.SaveChanges();

                string userid = Convert.ToString(MedicalPrescription.MedicalPrescription.CreatedBy);
                ErrorLog oErrorLogs = new ErrorLog();
                oErrorLogs.WriteErrorLogTitle(cmpname, "Medical Prescription", "User name :", dcname, "User ID :", userid, "Mode : Submit");

                object names = MedPres;
                oErrorLogs.WriteErrorLogArray("MedicalPrescription", names);
                var ranunid = MedPres.RandomUniqueID;
                var crby = MedPres.CreatedBy;

                if (MedicalPrescription.MPT.Count() > 0)
                {
                    foreach (var item in MedicalPrescription.MPT.ToList())

                    {


                        var MedPrescTran = new MedicalPrescriptionTran();



                        MedPrescTran.MedicalPrescriptionID = ranunid;
                        MedPrescTran.DrugId = WYNKContext.SpecialityMaster.Where(x => x.ParentDescription == item.Brand && x.CmpID == cmpPid && x.IsActive == true && x.IsDeleted == false).Select(x => x.ID).FirstOrDefault();
                        MedPrescTran.Dossage = item.Dossage;
                        MedPrescTran.Frequency = item.Frequency;
                        MedPrescTran.Quantity = item.Quantity;
                        MedPrescTran.Eye = item.Eye;
                        MedPrescTran.Food = item.Food;
                        MedPrescTran.Days = item.Days;
                        MedPrescTran.Brand = item.Brand;
                        MedPrescTran.DrugForm = item.DrugForm;
                        MedPrescTran.Remarks = MedicalPrescription.MedicalPrescriptionTran.Remarks;
                        MedPrescTran.CreatedUTC = DateTime.UtcNow;
                        MedPrescTran.CreatedBy = crby;
                        WYNKContext.MedicalPrescriptionTran.AddRange(MedPrescTran);

                        ErrorLog oErrorLogstran = new ErrorLog();
                        object namestr = MedPrescTran;
                        oErrorLogstran.WriteErrorLogArray("MedicalPrescriptionTran", namestr);
                    }
                }

                WYNKContext.SaveChanges();

                var commonRepos = new CommonRepository(_Wynkcontext, _Cmpscontext);
                var RunningNumber = commonRepos.GenerateRunningCtrlNoo(TransactionTypeid, cmpPid, "GetRunningNo");

                if (RunningNumber == MedicalPrescription.MedicalPrescription.MedicalPrescriptionNo)
                {
                    commonRepos.GenerateRunningCtrlNoo(TransactionTypeid, cmpPid, "UpdateRunningNo");
                }
                else
                {
                    var GetRunningNumber = commonRepos.GenerateRunningCtrlNoo(TransactionTypeid, cmpPid, "UpdateRunningNo");

                    var MedicalPres = WYNKContext.MedicalPrescription.Where(x => x.MedicalPrescriptionNo == MedicalPrescription.MedicalPrescription.MedicalPrescriptionNo).FirstOrDefault();
                    MedicalPres.MedicalPrescriptionNo = GetRunningNumber;
                    WYNKContext.MedicalPrescription.UpdateRange(MedicalPres);

                    WYNKContext.SaveChanges();
                }



                if (WYNKContext.SaveChanges() >= 0)
                {
                    ErrorLog oErrorLog = new ErrorLog();
                    oErrorLog.WriteErrorLog("Information :", "Saved Successfully");
                }
                MedicalPrescription.regid = WYNKContext.MedicalPrescription.Where(x => x.UIN == MedicalPrescription.MedicalPrescription.UIN).Select(x => x.RegistrationTranID).FirstOrDefault();

                MedicalPrescription.medid = WYNKContext.MedicalPrescription.Where(x => x.RegistrationTranID == MedicalPrescription.regid).Select(x => x.MedicalPrescriptionID).LastOrDefault();




                var medicalprescription = new Medical_Prescription();



                var regidss = WYNKContext.RegistrationTran.Where(x => x.UIN == MedicalPrescription.MedicalPrescription.UIN && x.CmpID == cmpPid).Select(x => x.RegistrationTranID).LastOrDefault();

                var docnamelast = WYNKContext.MedicalPrescription.Where(x => x.RegistrationTranID == regid).Select(x => x.PrescribedByName).LastOrDefault();

                var docidss = WYNKContext.MedicalPrescription.Where(x => x.RegistrationTranID == regid).Select(x => x.PrescribedBy).LastOrDefault();

                var docno = CMPSContext.DoctorMaster.Where(x => x.DoctorID == docidss).Select(x => x.RegistrationNumber).FirstOrDefault();



                var medi1 = WYNKContext.MedicalPrescription.Where(x => x.UIN == MedicalPrescription.MedicalPrescription.UIN && x.CmpID == cmpPid).Select(x => x.RandomUniqueID).LastOrDefault();


                var remsrks = WYNKContext.MedicalPrescriptionTran.Where(x => x.MedicalPrescriptionID == medi1).Select(x => x.Remarks).FirstOrDefault();

                var regids = WYNKContext.RegistrationTran.Where(x => x.UIN == MedicalPrescription.MedicalPrescription.UIN && x.CmpID == cmpPid).Select(x => x.UIN).FirstOrDefault();
                var reviewdates = WYNKContext.Findings.Where(x => x.UIN == regids).Select(x => x.ReviewDate).FirstOrDefault();
                var regname = WYNKContext.Registration.Where(x => x.UIN == regids).Select(x => x.Name).FirstOrDefault();

                var regnamemid = WYNKContext.Registration.Where(x => x.UIN == regids).Select(x => x.MiddleName).FirstOrDefault();
                var regnamelast = WYNKContext.Registration.Where(x => x.UIN == regids).Select(x => x.LastName).FirstOrDefault();

                var genders = WYNKContext.Registration.Where(x => x.UIN == regids).Select(x => x.Gender).FirstOrDefault();

                var allergy = WYNKContext.PatientGeneral.Where(x => x.UIN == regids).Select(x => x.Allergy).FirstOrDefault();


                var CAddress = CMPSContext.Company.Where(x => x.CmpID == MedicalPrescription.MedicalPrescription.CmpID).Select(x => x.Address1).FirstOrDefault();

                var Cphone = CMPSContext.Company.Where(x => x.CmpID == MedicalPrescription.MedicalPrescription.CmpID).Select(x => x.Phone1).FirstOrDefault();

                var Cweb = CMPSContext.Company.Where(x => x.CmpID == MedicalPrescription.MedicalPrescription.CmpID).Select(x => x.Website).FirstOrDefault();

                var Cname = CMPSContext.Company.Where(x => x.CmpID == MedicalPrescription.MedicalPrescription.CmpID).Select(x => x.CompanyName).FirstOrDefault();

                var medtran = WYNKContext.MedicalPrescriptionTran.ToList();
                var drug = WYNKContext.DrugMaster.ToList();
                var onelinemasterss = CMPSContext.OneLineMaster.ToList();
                var druggroup = WYNKContext.DrugGroup.ToList();
                var SpecialityMas = WYNKContext.SpecialityMaster.ToList();


                medicalprescription.Patientlist = (from MPT in WYNKContext.MedicalPrescriptionTran.Where(u => u.MedicalPrescriptionID == medi1)
                                                   join SM in SpecialityMas on MPT.DrugId equals SM.ID
                                                   select new Patientlist
                                                   {
                                                       Brand = SM.ParentDescription,
                                                       Dossage = MPT.Dossage,
                                                       Frequency = MPT.Frequency,
                                                       Food = MPT.Food,
                                                       Dayss = Convert.ToInt32(MPT.Days),
                                                       Quant = MPT.Quantity,
                                                       PrescribedDate = MPT.CreatedUTC.Date,
                                                       Remarks = MPT.Remarks,
                                                       Eye = MPT.Eye,
                                                       uom = MPT.DrugForm,

                                                   }).ToList();





                return new
                {

                    reguin = regids,
                    regnames = regname,
                    regnamesmid = regnamemid,
                    regnameslast = regnamelast,
                    aller = allergy,
                    gend = genders,
                    remarks = remsrks,
                    rdate = reviewdates,
                    coname = Cname,
                    caddrs = CAddress,
                    cphone = Cphone,
                    cwebb = Cweb,
                    dcnum = docno,
                    dname3 = docnamelast,
                    Uin = medicalprescription.Patientlist.ToList(),
                    Success = true,
                    Message = CommonMessage.saved,
                };
            }
            catch (Exception ex)
            {
                ErrorLog oErrorLog = new ErrorLog();
                oErrorLog.WriteErrorLog("Error :", ex.InnerException.Message.ToString());
                Console.Write(ex);
                string msg = ex.InnerException.Message;
                return new { Success = false, Message = msg, grn = MedicalPrescription.MedicalPrescription.MedicalPrescriptionNo };



            }



        }



        public dynamic SaveTemplate(Medical_Prescription MedicalPrescription)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {

                try
                {


                    if (MedicalPrescription.tdesc != null)
                    {


                        var c = WYNKContext.DrugTemplate.Where(x => x.Description.Equals(Convert.ToString(MedicalPrescription.tdesc), StringComparison.OrdinalIgnoreCase)).Select(y => y.ID).FirstOrDefault();

                        if (c != 0)
                        {
                            return new
                            {
                                Success = false,
                                Message = "Description already Exists"
                            };

                        }

                    }



                    if (MedicalPrescription.MPT.Count >= 1)
                    {
                        foreach (var item in MedicalPrescription.MPT.ToList())
                        {
                            var template = new DrugTemplate();
                            template.DoctorID = MedicalPrescription.did;
                            template.CmpID = MedicalPrescription.cmpid;
                            template.Description = MedicalPrescription.tdesc;
                            template.Frequency = item.Frequency;
                            template.DrugID = item.DrugId;
                            template.Brand = item.Brand;
                            template.Days = Convert.ToInt32(item.Days);
                            template.Food = item.Food;
                            template.Quantity = Convert.ToInt32(item.Quantity);
                            template.CreatedUTC = DateTime.UtcNow;
                            template.CreatedBy = MedicalPrescription.uid;
                            WYNKContext.DrugTemplate.Add(template);
                        }
                        WYNKContext.SaveChanges();
                        dbContextTransaction.Commit();
                        return new
                        {
                            Success = true,
                            Message = CommonMessage.saved,
                        };
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
                    Message = CommonMessage.Missing,
                };
            }
        }


        public dynamic SaveTemplatem(Medical_Prescription MedicalPrescription)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {

                try
                {


                    if (MedicalPrescription.tdesc != null)
                    {


                        var c = WYNKContext.DrugTemplate.Where(x => x.Description.Equals(Convert.ToString(MedicalPrescription.tdesc), StringComparison.OrdinalIgnoreCase)).Select(y => y.ID).FirstOrDefault();

                        if (c != 0)
                        {
                            return new
                            {
                                Success = false,
                                Message = "Description already Exists"
                            };

                        }

                    }



                    if (MedicalPrescription.MPT.Count >= 1)
                    {
                        foreach (var item in MedicalPrescription.MPT.ToList())
                        {
                            var template = new DrugTemplate();
                            template.DoctorID = MedicalPrescription.did;
                            template.CmpID = MedicalPrescription.cmpid;
                            template.Description = MedicalPrescription.tdesc;
                            template.Frequency = item.Frequency;
                            template.Brand = item.Brand;
                            template.DrugID = WYNKContext.SpecialityMaster.Where(x => x.ParentDescription == item.Brand && x.CmpID == MedicalPrescription.cmpid && x.IsActive == true && x.IsDeleted == false).Select(x => x.ID).FirstOrDefault();
                            template.Days = Convert.ToInt32(item.Days);
                            template.Food = item.Food;
                            template.Quantity = Convert.ToInt32(item.Quantity);
                            template.CreatedUTC = DateTime.UtcNow;
                            template.CreatedBy = MedicalPrescription.uid;
                            WYNKContext.DrugTemplate.Add(template);
                        }
                        WYNKContext.SaveChanges();
                        dbContextTransaction.Commit();
                        return new
                        {
                            Success = true,
                            Message = CommonMessage.saved,
                        };
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
                    Message = CommonMessage.Missing,
                };
            }
        }




        public dynamic SaveTappering(Medical_Prescription MedicalPrescription, int medid, int docid)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {

                try
                {


                    var ids = WYNKContext.DrugTappering.Where(x => x.DrugID == medid && x.DoctorID == docid).ToList();


                    if (ids != null)
                    {

                        WYNKContext.DrugTappering.RemoveRange(ids);
                    }



                    if (MedicalPrescription.MPT.Count >= 1)
                    {
                        foreach (var item in MedicalPrescription.MPT.ToList())
                        {
                            var template = new DrugTappering();
                            template.DoctorID = MedicalPrescription.did;
                            template.CmpID = MedicalPrescription.cmpid;
                            template.Frequency = item.Frequency;
                            template.DrugID = medid;
                            template.Days = Convert.ToInt32(item.Days);
                            template.Food = item.Food;
                            template.Quantity = Convert.ToInt32(item.Quantity);
                            template.CreatedUTC = DateTime.UtcNow;
                            template.CreatedBy = MedicalPrescription.uid;
                            WYNKContext.DrugTappering.Add(template);
                        }
                        WYNKContext.SaveChanges();
                        dbContextTransaction.Commit();
                        return new
                        {
                            Success = true,
                            Message = CommonMessage.saved,
                        };
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
                    Message = CommonMessage.Missing,
                };
            }
        }




        public dynamic OverrideTemplate(Medical_Prescription MedicalPrescription)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {

                try
                {


                    var ids = WYNKContext.DrugTemplate.Where(x => x.Description == MedicalPrescription.tdesc).ToList();


                    if (ids != null)
                    {

                        WYNKContext.DrugTemplate.RemoveRange(ids);
                    }



                    if (MedicalPrescription.MPT.Count >= 1)
                    {
                        foreach (var item in MedicalPrescription.MPT.ToList())
                        {
                            var template = new DrugTemplate();
                            template.DoctorID = MedicalPrescription.did;
                            template.Description = MedicalPrescription.tdesc;
                            template.CmpID = MedicalPrescription.cmpid;
                            template.Frequency = item.Frequency;
                            template.DrugID = item.DrugId;
                            template.Days = Convert.ToInt32(item.Days);
                            template.Food = item.Food;
                            template.Quantity = Convert.ToInt32(item.Quantity);
                            template.CreatedUTC = DateTime.UtcNow;
                            template.CreatedBy = MedicalPrescription.uid;
                            WYNKContext.DrugTemplate.Add(template);
                        }
                        WYNKContext.SaveChanges();
                        dbContextTransaction.Commit();
                        return new
                        {
                            Success = true,
                            Message = CommonMessage.saved,
                        };
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
                    Message = CommonMessage.Missing,
                };
            }
        }



        public bool uploadImag(IFormFile file, string uin)
        {
            try
            {
                var currentDir = Directory.GetCurrentDirectory();
                if (!Directory.Exists(currentDir + "/MedicalPrescription/"))
                    Directory.CreateDirectory(currentDir + "/MedicalPrescription/");
                var fileName = $"{uin}{Path.GetExtension(file.FileName)}";
                var path = $"{currentDir}/MedicalPrescription/{fileName}";

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                    var pat = WYNKContext.MedicalPrescription.Where(x => x.UIN == uin).LastOrDefault();
                    pat.ImagePath = fileName;
                    WYNKContext.Entry(pat).State = EntityState.Modified;
                    return WYNKContext.SaveChanges() > 0;
                }
            }
            catch (Exception)
            {
                return false;
            }





        }




        public Medical_Prescription getDrug(int cmpid)
        {
            var medicalprescriptionn = new Medical_Prescription();
            medicalprescriptionn.MPT = new List<MedicalPrescriptionTran>();
            medicalprescriptionn.DrugInfo = new List<DrugInfo>();
            medicalprescriptionn.UploadMedicalPrescription = new List<UploadMedicalPrescription>();
            medicalprescriptionn.InvImg = new List<InvImg>();

            medicalprescriptionn.MedicalPrescription = new MedicalPrescription();
            medicalprescriptionn.MedicalPrescriptionTran = new MedicalPrescriptionTran();
            medicalprescriptionn.OneLineMaster = new OneLine_Masters();
            medicalprescriptionn.Indentdata = new IndentViewModel();
            var oneline = CMPSContext.OneLineMaster.ToList();
            medicalprescriptionn.DrugssDetail = (from drug in WYNKContext.DrugMaster.Where(u => u.IsDeleted == false && u.IsActive == true && u.Cmpid == cmpid)

                                                 select new DrugssDetail
                                                 {
                                                     ID = drug.ID,
                                                     mediid = Convert.ToInt32(drug.GenericName),
                                                     DrugGroup = drug.DrugGroup,
                                                     Brand = drug.Brand,
                                                     Manufacturer = oneline.Where(x => x.OLMID == Convert.ToInt32(drug.Manufacturer)).Select(x => x.ParentDescription).FirstOrDefault(),
                                                     MedicineName = WYNKContext.DrugGroup.Where(x => x.ID == drug.GenericName).Select(x => x.Description).FirstOrDefault(),
                                                     sideeffect = WYNKContext.DrugGroup.Where(x => x.ID == drug.GenericName).Select(x => x.SideEffects).FirstOrDefault(),
                                                     precaution = WYNKContext.DrugGroup.Where(x => x.ID == drug.GenericName).Select(x => x.Precautions).FirstOrDefault(),
                                                     overdose = WYNKContext.DrugGroup.Where(x => x.ID == drug.GenericName).Select(x => x.Overdose).FirstOrDefault(),
                                                     uomName = drug.UOM,
                                                 }).ToList();

            medicalprescriptionn.Drugssonly = (from o in medicalprescriptionn.DrugssDetail

                                               select new Drugssonly
                                               {
                                                   Description = o.Brand,
                                                   IDD = o.ID

                                               }).ToList();

            return medicalprescriptionn;
        }

        public dynamic Updatepres(Medical_Prescription MedicalPrescription, string UIN)
        {







            var rid = WYNKContext.RegistrationTran.Where(x => x.UIN == UIN).OrderByDescending(x => x.CreatedUTC).Select(x => x.RegistrationTranID).FirstOrDefault();

            if (MedicalPrescription.UploadMedicalPrescription.Count() > 0)
            {
                foreach (var item1 in MedicalPrescription.UploadMedicalPrescription.ToList())

                {
                    var presupload = new UploadMedicalPrescription();

                    presupload.CmpID = item1.CmpID;
                    presupload.UIN = UIN;
                    presupload.RegistrationTranID = rid;
                    presupload.Remarks = item1.Remarks;
                    presupload.CreatedUTC = DateTime.Now;
                    presupload.CreatedBy = item1.CreatedBy;
                    WYNKContext.UploadMedicalPrescription.AddRange(presupload);

                }
                WYNKContext.SaveChanges();
            }








            try
            {
                if (WYNKContext.SaveChanges() >= 0)
                {
                    ErrorLog oErrorLog = new ErrorLog();
                    oErrorLog.WriteErrorLog("Information :", "Saved Successfully");
                }
                return new
                {
                    Uin = UIN,

                    Success = true,
                    Message = CommonMessage.saved,
                };
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Success = false,
                Message = CommonMessage.Missing,
            };

        }

        public bool uploadImag(IFormFile file1, string desc, string uin, string id)
        {
            var medpres = new Medical_Prescription();
            medpres.UploadMedicalPrescription = new List<UploadMedicalPrescription>();
            try
            {
                var ivid = WYNKContext.UploadMedicalPrescription.Where(x => x.UIN == id).Select(x => x.ID).LastOrDefault();
                var currentDir = Directory.GetCurrentDirectory();
                var dt = Convert.ToDateTime(DateTime.Now.Date).ToString("dd-MM-yyyy");
                var res = Directory.CreateDirectory(currentDir + '/' + uin + '/' + dt);
                var descs = desc + ivid;
                var fileName1 = $"{descs}{Path.GetExtension(file1.FileName)}";
                var path1 = $"{currentDir}/{uin}/{dt}/{fileName1}";
                var pathh = $"{currentDir}/{uin}/{dt}";
                using (var stream1 = new FileStream(path1, FileMode.Create))
                {
                    file1.CopyTo(stream1);
                    var opbio = WYNKContext.UploadMedicalPrescription.Where(x => x.UIN == id && x.CreatedUTC.Date == DateTime.Now.Date && x.Path == null).ToList();
                    if (opbio.Count() > 0)
                    {




                        foreach (var item1 in opbio.ToList())
                        {
                            item1.Path = pathh;
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

        public dynamic Getimage(string UIN)
        {
            var medpres = new Medical_Prescription();
            medpres.InvImg = new List<InvImg>();
            medpres.UploadMedicalPrescription = new List<UploadMedicalPrescription>();
            var regs = WYNKContext.UploadMedicalPrescription.Where(x => x.UIN == UIN).Select(x => x.Path).ToList();

            medpres.Registration = WYNKContext.Registration.Where(x => x.UIN == UIN).FirstOrDefault();
            var groups = WYNKContext.UploadMedicalPrescription.Where(x => x.UIN == UIN).OrderByDescending(x => x.CreatedUTC).GroupBy(x => x.Path);
            medpres.Imagedata = (from IN in groups
                                 select new Imagedata
                                 {
                                     idd = IN.Key,
                                     cont = IN.Count(),
                                     Descs = IN.Select(X => X.Path).FirstOrDefault(),
                                     rmrks = IN.Select(X => X.Remarks).FirstOrDefault(),
                                     dttms = IN.Select(X => X.CreatedUTC).FirstOrDefault(),
                                 }).ToList();
            if (medpres.Imagedata != null)
            {


                var rest = medpres.Imagedata.Sum(x => x.cont);
                medpres.stringArray = new string[rest];

                var list1 = new List<samplelist>();

                foreach (var item1 in medpres.Imagedata.ToList())

                {

                    var ress = item1.cont;

                    var list2 = new samplelist();
                    list2.Uin = ress;
                    list1.Add(list2);

                    var cv = list1.Sum(x => x.Uin);

                    var cvv = cv - item1.cont;

                    for (var inde = 0; inde < item1.cont; inde++)
                    {
                        var ivid = WYNKContext.UploadMedicalPrescription.Where(x => x.UIN == UIN).Select(x => x.ID).ToList();
                        for (var indes = 0; indes < ivid.Count; indes++)
                        {
                            var a = ivid[indes];
                            var testStr = item1.Descs + '/' + UIN + inde + a + ".png";
                            var path = testStr;

                            var dttms = WYNKContext.UploadMedicalPrescription.Where(x => x.Path == item1.Descs && x.UIN == UIN).Select(x => x.CreatedUTC).ToList();

                            if ((File.Exists(path)))
                            {

                                string imageData = Convert.ToBase64String(File.ReadAllBytes(path));
                                string source = imageData;
                                if (medpres.stringArray[inde] != null)
                                {
                                    medpres.stringArray[inde + cvv] = imageData;

                                }
                                else
                                {
                                    medpres.stringArray[inde] = imageData;

                                }


                                var list4 = new InvImg();
                                list4.idm = item1.idd;
                                list4.remr = item1.rmrks;
                                list4.Desccm = item1.ImDescs;
                                list4.dttm = item1.dttms;
                                list4.imgdt = "data:image/png;base64," + imageData;
                                medpres.InvImg.Add(list4);
                            }
                        }
                    }
                }

                medpres.InvImgres = (from IN in medpres.InvImg.GroupBy(x => x.Desccm)
                                     select new InvImgres
                                     {
                                         Desccmre = IN.Key,
                                         remarks = IN.Select(x => x.remr).ToList(),
                                         imgdtre = IN.Select(x => x.imgdt).ToList(),
                                         imgdttm = IN.Select(x => x.dttm).ToList(),

                                     }).ToList();


            }

            else
            {
            }
            return medpres;
        }


    }
}






