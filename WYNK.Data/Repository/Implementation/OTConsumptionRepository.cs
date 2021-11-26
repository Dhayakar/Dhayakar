using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WYNK.Data.Common;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Data.Repository.Operation;
using WYNK.Helpers;
using System.Transactions;
using System.Text.RegularExpressions;

namespace WYNK.Data.Repository.Implementation
{
    class OTConsumptionRepository : RepositoryBase<OTConsumptionViewModel>, IOTConsumptionRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;


        public OTConsumptionRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public string ExpireDateCalculation(DateTime ItemBatchExpiry, int CriticalIntervalTime, int RetestIntervalTime)
        {
            var IntervalDays = CriticalIntervalTime + RetestIntervalTime;
            var DaysDifference = Math.Ceiling((ItemBatchExpiry - DateTime.Now).TotalDays);
            if (DaysDifference <= IntervalDays)
            {
                return Convert.ToString(DaysDifference);
            }
            else
            {
                return " ";
            }
        }

        public string CriticalIntervalCalculation(DateTime ItemBatchExpiry, int CriticalIntervalTime, int RetestIntervalTime)
        {
            var IntervalDays = CriticalIntervalTime + RetestIntervalTime;
            var DaysDifference = Math.Ceiling((ItemBatchExpiry - DateTime.Now).TotalDays);
            var CriticalIntervalDays = DaysDifference - CriticalIntervalTime;

            return Convert.ToString(CriticalIntervalDays);
        }

        public DateTime CriticalIntervalDate(DateTime ItemBatchExpiry, int CriticalIntervalDays)
        {
            var CriticalIntervalDate = ItemBatchExpiry.AddDays(-CriticalIntervalDays);

            return CriticalIntervalDate;
        }

        public string RetestIntervalCalculation(DateTime ItemBatchExpiry, int CriticalIntervalTime, int RetestIntervalTime)
        {
            var IntervalDays = CriticalIntervalTime + RetestIntervalTime;
            var DaysDifference = Math.Ceiling((ItemBatchExpiry - DateTime.Now).TotalDays);
            var CriticalIntervalDays = DaysDifference - CriticalIntervalTime;

            var RetestIntervalDays = CriticalIntervalDays - RetestIntervalTime;

            var result = RetestIntervalDays <= 0 ? 0 : RetestIntervalDays;

            return result == 0 ? "Re-test days in progress" : Convert.ToString(result);
        }

        public DateTime RetestIntervalDate(DateTime ItemBatchExpiry, int Days)
        {
            var CriticalIntervalDate = ItemBatchExpiry.AddDays(-Days);

            return CriticalIntervalDate;
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
                return string.Format("{0}year{1}", years, (years == 1) ? "" : "s");
            }
            else if (years == 0 && months != 0 && days == 0)
            {
                return string.Format("{0}month{1}", months, (months == 1) ? "" : "s");
            }
            else if (years == 0 && months == 0 && days != 0)

            {
                return string.Format("{0}day{1}", days, (days == 1) ? "" : "s");
            }
            else if (years != 0 && months != 0 && days != 0)
            {
                years = dt.Year - dob.Year;
                return string.Format("{0}year{1}", years, (years == 1) ? "" : "s");
            }
            else if (years != 0 && months == 0 && days != 0)
            {

                years = dt.Year - dob.Year;
                return string.Format("{0}year{1}", years, (years == 1) ? "" : "s");
            }
            else if (years != 0 && months != 0 && days == 0)
            {
                years = dt.Year - dob.Year;
                return string.Format("{0}year{1}", years, (years == 1) ? "" : "s");
            }
            else if (years == 0 && months != 0 && days != 0)
            {
                return string.Format("{0}month{1}", months, (months == 1) ? "" : "s");
            }
            else if (years == 0 && months == 0 && days == 0)
            {
                return string.Format("{0}day{1}", days, (days == 1) ? "" : "s");
            }
            return string.Format("{0} year{1}, {2} month{3} and {4} day{5}",
                                 years, (years == 1) ? "" : "s",
                                 months, (months == 1) ? "" : "s",
                                 days, (days == 1) ? "" : "s");
        }

        public dynamic SurgeryAssignedList(int cmpid, string GMTTIME)
        {
            var icdmaster = WYNKContext.ICDSpecialityCode.ToList();
            var Reg = WYNKContext.Registration.ToList();

            TimeSpan ts = TimeSpan.Parse(GMTTIME);

            var SurAssign = WYNKContext.SurgeryAssigned.ToList();

            return (from e in SurAssign.Where(x => x.CmpID == cmpid && x.IsCancelled == false && x.IsSurgeryCompleted == false)
                    select new
                    {
                        ID = e.RandomUniqueID,
                        UIN = e.UIN,
                        AdmDate = WYNKContext.Admission.Where(adm => adm.RandomUniqueID == WYNKContext.Surgery.Where(x => x.RandomUniqueID == e.SurgeryID).Select(x => x.AdmID).FirstOrDefault()).Select(x => x.AdmDate).FirstOrDefault() + ts,
                        SurgeryDate = WYNKContext.Surgery.Where(x => x.RandomUniqueID == e.SurgeryID).Select(x => x.DateofSurgery).FirstOrDefault() + ts,
                        SurgeryID = e.SurgeryID,
                        Surgery = string.Join(",", WYNKContext.SurgeryTran
                                                .Where(surgTran => surgTran.SurgeryID == e.SurgeryID)
                                                .Select(surgTran => icdmaster
                                                    .Where(icd => icd.ID == surgTran.IcdSpecialityCode)
                                                    .Select(icd => icd.SpecialityDescription).FirstOrDefault())),
                        //SurgeryPackageCost = WYNKContext.SurgeryCostDetail.Where(x => x.ICDCode == WYNKContext.SurgeryTran.Where(st => st.SurgeryID == e.SurgeryID).Select(st => st.ICDCode).FirstOrDefault() && x.CMPID == cmpid).Select(x => x.PackageRate).FirstOrDefault(),
                        //SurgeryPackageCost = WYNKContext.SurgeryTran.Where(st => st.SurgeryID == e.SurgeryID).Select(st => st.IcdSpecialityCode).FirstOrDefault(),
                        Name = Reg.Where(x => x.UIN == e.UIN).Select(x => String.Concat(x.Name + " " + (x.MiddleName != null ? x.MiddleName : " ") + " " + x.LastName)).FirstOrDefault(),
                        Age = ToAgeString(Reg.Where(x => x.UIN == e.UIN).Select(x => x.DateofBirth).FirstOrDefault()),
                        Gender = Reg.Where(x => x.UIN == e.UIN).Select(x => x.Gender).FirstOrDefault(),
                        SurgeryProcedure = (from IO in WYNKContext.IOProcedureTemplate.Where(x => x.IsActive == true && x.ICDSpecialityCode == WYNKContext.SurgeryTran.Where(y => y.SurgeryID == e.SurgeryID).Select(y => y.IcdSpecialityCode).FirstOrDefault())
                                            group IO by IO.SurgeryDescription into IOgroup
                                            select new
                                            {
                                                Text = IOgroup.Key,
                                                Value = IOgroup.Select(y => y.ICDSpecialityCode).FirstOrDefault(),
                                            }).ToList(),
                        OperationTheatreName = WYNKContext.OperationTheatre.Where(ot => ot.OTID == WYNKContext.Surgery.Where(x => x.RandomUniqueID == e.SurgeryID).Select(x => x.OTID).FirstOrDefault()).Select(ot => ot.OTDescription).FirstOrDefault(),
                        IsIOLReqd = WYNKContext.ICDMaster.Where(x => x.ICDCODE == WYNKContext.SurgeryTran.Where(st => st.SurgeryID == e.SurgeryID).Select(st => st.ICDCode).FirstOrDefault()).Select(x => x.IsIOLReqd).FirstOrDefault(),
                        FindExt = e.FindingsExtID != null ? GetFindingExt(e.FindingsExtID) : null,
                        BillingType = WYNKContext.Admission.Where(adm => adm.RandomUniqueID == WYNKContext.Surgery.Where(x => x.RandomUniqueID == e.SurgeryID).Select(x => x.AdmID).FirstOrDefault()).Select(x => x.BillingType).FirstOrDefault(),
                        AdmRandomId = WYNKContext.Admission.Where(adm => adm.RandomUniqueID == WYNKContext.Surgery.Where(x => x.RandomUniqueID == e.SurgeryID).Select(x => x.AdmID).FirstOrDefault()).Select(x => x.RandomUniqueID).FirstOrDefault(),
                    }).ToList();
        }

        public object GetFindingExt(int? ID)
        {
            var DoctorMaster = CMPSContext.DoctorMaster.ToList();
            var Users = CMPSContext.Users.ToList();
            var FindingsRec = WYNKContext.FindingsExt.Where(x => x.ID == ID).ToList();
            var FinExts = (from FinExt in FindingsRec
                           select new
                           {
                               Diagnosed = FinExt.CreatedUTC,
                               SurgeryAdviceby = DoctorMaster.Where(x => x.EmailID == Users.Where(u => u.Userid == FinExt.CreatedBy).Select(u => u.Username).FirstOrDefault()).Select(x => x.Firstname + x.MiddleName + x.LastName).FirstOrDefault(),
                           }).ToList();
            return FinExts;
        }

        public dynamic AssignedDoctors(string SAID, int cmpid)
        {
            var DocSpec = CMPSContext.DoctorSpeciality.ToList();
            var OnelineMaster = CMPSContext.OneLineMaster.ToList();

            var AssignedDoctorIDs = WYNKContext.SurgeryAssignedTran.Where(x => x.SAID == SAID).Select(x => x.DoctorID).ToList();

            var result = (from e in CMPSContext.DoctorMaster
                          join ds in CMPSContext.DoctorSpeciality on e.DoctorID equals ds.DoctorID
                          where AssignedDoctorIDs.Contains(e.DoctorID)
                          select new
                          {
                              ID = e.DoctorID,
                              Name = String.Concat(e.LastName, ' ', e.MiddleName, ' ', e.Firstname),
                              Specialization = CMPSContext.OneLineMaster.Where(x => x.OLMID == ds.OLMID).Select(x => x.ParentDescription).FirstOrDefault()
                          }).ToList();

            return result;
        }

        public dynamic GetdrugDetails(int ID, int StoreID, int cmpid)
        {

            var DrugTrackerId = WYNKContext.DrugMaster.Where(x => x.ID == ID).Select(x => x.DrugTracker).FirstOrDefault();
            var res = Enum.GetName(typeof(TrackingType), DrugTrackerId);

            if (res == "SerialNumberBased")
            {
                var Date = DateTime.Now;
                var FinancialYearId = WYNKContext.FinancialYear.Where(x => Convert.ToDateTime(x.FYFrom) <= Date && Convert.ToDateTime(x.FYTo) >= Date && x.IsActive == true && x.CMPID == cmpid).Select(x => x.ID).FirstOrDefault();
                var CheckingItemBalance = WYNKContext.ItemBalance.Where(x => x.FYear == FinancialYearId && x.ItemID == ID && x.StoreID == StoreID && x.CmpID == cmpid).Select(x => x.ClosingBalance).FirstOrDefault();

                if (CheckingItemBalance != 0)
                {
                    var ItemSerialList = (from ItemSerial in WYNKContext.ItemSerial.Where(x => x.ItemID == ID && x.IssueNo == null && x.StoreID == StoreID && x.cmpID == cmpid)
                                          select new
                                          {
                                              SerialNo = ItemSerial.SerialNo,
                                          }).ToList();



                    var SerialsDrug = (from drug in WYNKContext.DrugMaster.Where(x => x.ID == ID)
                                       select new
                                       {
                                           DrugID = drug.ID,
                                           Brand = drug.Brand,
                                           UOM = drug.UOM,
                                           GenericName = WYNKContext.DrugGroup.Where(x => x.ID == drug.GenericName).Select(x => x.Description).FirstOrDefault(),
                                           IsSerial = true,
                                           SerialList = ItemSerialList,
                                           DrugCategory = Enum.GetName(typeof(DrugCategory), drug.DrugCategory) == "ImplantDrug" ? "ImplantDrug" : "",
                                           Rate = drug.Rate,
                                       }).FirstOrDefault();



                    return new
                    {
                        Success = true,
                        Message = "Serial Details Drugs",
                        IsSerial = true,
                        Items = SerialsDrug,
                    };

                }
                else
                {
                    return new
                    {
                        Success = false,
                        Message = "Serial Details Drugs",
                        IsSerial = true,
                    };
                }
            }
            else if (res == "BatchNumberBased")
            {
                var drugGroup = WYNKContext.DrugGroup.Where(x => x.ID == WYNKContext.DrugMaster.Where(y => y.ID == ID).Select(y => y.GenericName).FirstOrDefault()).FirstOrDefault();
                var RetestInterval = drugGroup.RetestInterval;
                var CriticalIntervalDays = drugGroup.RetestCriticalInterval;

                var drugstockcheck = WYNKContext.ItemBatch.Where(x => x.ItemID == ID && DateTime.Now.AddDays(CriticalIntervalDays) < x.ItemBatchExpiry.Date && x.StoreID == StoreID && x.cmpID == cmpid).OrderBy(x => x.CreatedUTC).ToList();

                var Date = DateTime.Now;
                var FinancialYearId = WYNKContext.FinancialYear.Where(x => Convert.ToDateTime(x.FYFrom) <= Date && Convert.ToDateTime(x.FYTo) >= Date && x.IsActive == true && x.CMPID == cmpid).Select(x => x.ID).FirstOrDefault();
                decimal? AvailableStock = WYNKContext.ItemBalance.Where(x => x.FYear == FinancialYearId && x.ItemID == ID && x.CmpID == cmpid).Select(x => x.ClosingBalance).FirstOrDefault();

                var ItemBatchDetails = (from itemBatch in drugstockcheck
                                        select new
                                        {
                                            BatchNo = itemBatch.ItemBatchNumber,
                                            TotalQty = itemBatch.ItemBatchQty,
                                            BalanceQty = itemBatch.ItemBatchBalnceQty - (itemBatch.LockedQuantity != null ? itemBatch.LockedQuantity : 0),
                                            ExpiryDate = itemBatch.ItemBatchExpiry,
                                            QtyTaken = 0,
                                            CriticalIntervalDay = CriticalIntervalCalculation(itemBatch.ItemBatchExpiry, CriticalIntervalDays, RetestInterval),
                                            CriticalIntervalDate = CriticalIntervalDate(itemBatch.ItemBatchExpiry, CriticalIntervalDays),
                                            RetestIntervalDate = RetestIntervalDate(itemBatch.ItemBatchExpiry, (CriticalIntervalDays + RetestInterval)),
                                            RetestIntervalDays = RetestIntervalCalculation(itemBatch.ItemBatchExpiry, CriticalIntervalDays, RetestInterval),
                                            ExpireInDays = ExpireDateCalculation(itemBatch.ItemBatchExpiry, CriticalIntervalDays, RetestInterval)
                                        }).ToList();

                var BatchDrugs = (from drug in WYNKContext.DrugMaster.Where(x => x.ID == ID)
                                  select new
                                  {
                                      DrugID = drug.ID,
                                      Brand = drug.Brand,
                                      UOM = drug.UOM,
                                      GenericName = WYNKContext.DrugGroup.Where(x => x.ID == drug.GenericName).Select(x => x.Description).FirstOrDefault(),
                                      IsSerial = false,
                                      AvailableStock = AvailableStock,
                                      BatchDetails = ItemBatchDetails,
                                      DrugCategory = Enum.GetName(typeof(DrugCategory), drug.DrugCategory) == "ImplantDrug" ? "ImplantDrug" : "",
                                      Rate = drug.Rate,
                                  }).FirstOrDefault();

                return new
                {
                    Success = true,
                    Message = "Batch Details Drugs",
                    IsSerial = false,
                    Items = BatchDrugs,
                };

            }
            else
            {
                var Date = DateTime.Now;

                var FinancialYearId = WYNKContext.FinancialYear.Where(x => Convert.ToDateTime(x.FYFrom) <= Date && Convert.ToDateTime(x.FYTo) >= Date).Select(x => x.ID).FirstOrDefault();

                var CheckingItemBalance = WYNKContext.ItemBalance.Where(x => x.FYear == FinancialYearId && x.ItemID == ID && x.StoreID == StoreID).Select(x => x.ClosingBalance).FirstOrDefault();

                if (CheckingItemBalance != 0)
                {
                    return new
                    {
                        Success = true,
                        Message = "Other Drug Details Drugs",
                        IsSerial = false,
                        DrugID = ID,
                        AvailableQty = CheckingItemBalance,
                        Brand = WYNKContext.DrugMaster.Where(x => x.ID == ID).Select(x => x.Brand).FirstOrDefault(),
                        UOM = WYNKContext.DrugMaster.Where(x => x.ID == ID).Select(x => x.UOM).FirstOrDefault(),
                        DrugCategory = Enum.GetName(typeof(DrugCategory), WYNKContext.DrugMaster.Where(x => x.ID == ID).Select(x => x.DrugCategory).FirstOrDefault()) == "ImplantDrug" ? "ImplantDrug" : "",
                        Rate = WYNKContext.DrugMaster.Where(x => x.ID == ID).Select(x => x.Rate).FirstOrDefault(),
                    };
                }
                else
                {

                    return new
                    {
                        Success = false,
                        Message = "Other Drug Details Drugs",
                        IsSerial = false,
                    };

                }

            }
        }


        public IList<AllotedBatch> CheckBatchQty(int DrugID, int StoreId, string BatchNo, DateTime ExpiryDate, int QtyTaken, int cmpid)
        {
            IList<AllotedBatch> Alloted = new List<AllotedBatch>();

            var drugGroup = WYNKContext.DrugGroup.Where(x => x.ID == WYNKContext.DrugMaster.Where(y => y.ID == DrugID).Select(y => y.GenericName).FirstOrDefault()).FirstOrDefault();
            var RetestInterval = drugGroup.RetestInterval;
            var CriticalIntervalDays = drugGroup.RetestCriticalInterval;

            var drugstockcheck = WYNKContext.ItemBatch.Where(x => x.ItemID == DrugID && x.ItemBatchNumber == BatchNo && x.cmpID == cmpid && DateTime.Now.AddDays(CriticalIntervalDays) < x.ItemBatchExpiry.Date && x.StoreID == StoreId && x.ItemBatchExpiry == ExpiryDate).OrderBy(x => x.CreatedUTC).ToList();
            decimal? EnoughStock = drugstockcheck.Select(x => x.ItemBatchBalnceQty - (x.LockedQuantity != null ? x.LockedQuantity : 0)).Sum();

            if (EnoughStock >= QtyTaken)
            {
                foreach (var res in drugstockcheck)
                {
                    Alloted.Add(new AllotedBatch()
                    {
                        DrugId = DrugID,
                        balanceQty = res.ItemBatchBalnceQty - Convert.ToDecimal(res.LockedQuantity),
                        CreatedUTC = res.CreatedUTC,
                        ExpiryDate = res.ItemBatchExpiry,
                        GoingToIssue = QtyTaken,
                        itemBatchNo = res.ItemBatchNumber,
                    });
                    var itemBatch = new ItemBatch();
                    itemBatch = WYNKContext.ItemBatch.Where(x => x.ItemBatchNumber == res.ItemBatchNumber && x.ItemID == DrugID && x.StoreID == StoreId && x.cmpID == cmpid && x.ItemBatchExpiry == res.ItemBatchExpiry).FirstOrDefault();
                    itemBatch.LockedQuantity = Convert.ToInt32((itemBatch.LockedQuantity != null ? itemBatch.LockedQuantity : 0) + QtyTaken);
                    WYNKContext.ItemBatch.UpdateRange(itemBatch);
                    WYNKContext.SaveChanges();
                }
            }
            else
            {
                Alloted.Add(new AllotedBatch()
                {
                    DrugId = DrugID,
                    itemBatchNo = BatchNo,
                    balanceQty = Convert.ToDecimal(EnoughStock),
                    ExpiryDate = ExpiryDate,
                });

            }
            return Alloted;
        }

        public dynamic SubmitOT(OTConsumptionDetails OTConsumptionDetails)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {

                try
                {
                    OTConsumptionDetails.InSufficientDrugs = new List<InSufficientDrug>();
                    OTConsumptionDetails.InSufficientSerials = new List<SerialDetail>();
                    OTConsumptionDetails.InSufficientOtherDrugs = new List<OtherDrugs>();
                    List<AllotedBatch> AllotingBatchs = new List<AllotedBatch>();
                    List<SerialDetail> AllotingSerial = new List<SerialDetail>();
                    List<OtherDrugs> AllotingOtherDrugs = new List<OtherDrugs>();

                    List<AllotedAnaesBatch> AllotingAnaesBatchs = new List<AllotedAnaesBatch>();
                    List<AnaesSerialDetail> AllotingAnaesSerial = new List<AnaesSerialDetail>();
                    List<AnaesOtherDrugs> AllotingAnaesOtherDrugs = new List<AnaesOtherDrugs>();

                    List<Alert> Alerts = new List<Alert>();

                    var Datee = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                    var Fyear = Convert.ToString(WYNKContext.FinancialYear.Where(x => Convert.ToDateTime(x.FYFrom) <= Datee && Convert.ToDateTime(x.FYTo) >= Datee && x.CMPID == OTConsumptionDetails.Cmpid && x.IsActive == true).Select(x => x.ID).FirstOrDefault());

                    var uommaster = CMPSContext.uommaster.ToList();


                    if (OTConsumptionDetails.OTItemDetails.Count > 0)
                    {
                        foreach (var Item in OTConsumptionDetails.OTItemDetails.ToList())
                        {
                            var Drugid = Item.DrugID;
                            var reqQuantity = Item.Quantity;
                            IList<AllotedBatch> AllotingBatch = new List<AllotedBatch>();

                            var DrugTrackerId = WYNKContext.DrugMaster.Where(x => x.ID == Drugid && x.Cmpid == OTConsumptionDetails.Cmpid).Select(x => x.DrugTracker).FirstOrDefault();
                            var res = Enum.GetName(typeof(TrackingType), DrugTrackerId);

                            if (res == "SerialNumberBased")
                            {
                                OTConsumptionDetails.InSufficientSerials = CheckSerials(Item.SelectedList, Drugid, Convert.ToInt32(OTConsumptionDetails.storeID), OTConsumptionDetails.Cmpid);

                                if (OTConsumptionDetails.InSufficientSerials.Count == 0)
                                {
                                    foreach (var item in Item.SelectedList.ToList())
                                    {

                                        var ISerial = new SerialDetail();
                                        ISerial.DrugID = Drugid;
                                        ISerial.SerialNo = Convert.ToString(item);
                                        AllotingSerial.Add(ISerial);
                                    }
                                }

                            }
                            else if (res == "BatchNumberBased")
                            {
                                var qtylists = Item.BatchInfo.Where(x => x.QtyTaken != 0).ToList();

                                foreach (var qtylist in qtylists.ToList())
                                {
                                    AllotingBatch = CheckBatchQty(Drugid, Convert.ToInt32(OTConsumptionDetails.storeID), qtylist.BatchNo, qtylist.ExpiryDate, qtylist.QtyTaken, OTConsumptionDetails.Cmpid);

                                    if (AllotingBatch.Sum(x => x.GoingToIssue) >= qtylist.QtyTaken)
                                    {
                                        AllotingBatchs.AddRange(AllotingBatch);
                                    }
                                    else
                                    {
                                        foreach (var item in AllotingBatch.ToList())
                                        {
                                            if (item.GoingToIssue == 0)
                                            {
                                                var InSufficientDrugs = new InSufficientDrug();
                                                InSufficientDrugs.DrugId = item.DrugId;
                                                InSufficientDrugs.DrugName = WYNKContext.DrugMaster.Where(x => x.ID == Drugid).Select(x => x.Brand).FirstOrDefault();
                                                InSufficientDrugs.BatchNumber = item.itemBatchNo;
                                                InSufficientDrugs.ExpiryDate = item.ExpiryDate;
                                                InSufficientDrugs.BalanceQuantity = item.balanceQty;
                                                OTConsumptionDetails.InSufficientDrugs.Add(InSufficientDrugs);
                                            }
                                        }
                                    }

                                }
                            }
                            else
                            {
                                var Date = DateTime.Now;

                                var Itembal = WYNKContext.ItemBalance.Where(x => x.ItemID == Drugid && x.FYear == Convert.ToInt32(Fyear) && x.StoreID == OTConsumptionDetails.storeID).Select(x => x.ClosingBalance).FirstOrDefault();

                                if (Itembal >= Item.Quantity)
                                {
                                    var OtherDrugs = new OtherDrugs();
                                    OtherDrugs.DrugID = Drugid;
                                    OtherDrugs.QtyTobeTaken = Item.Quantity;
                                    AllotingOtherDrugs.Add(OtherDrugs);
                                }
                                else
                                {
                                    var OtherDrugs = new OtherDrugs();
                                    OtherDrugs.Brand = WYNKContext.DrugMaster.Where(x => x.ID == Drugid).Select(x => x.Brand).FirstOrDefault();
                                    OtherDrugs.AvailableQty = Convert.ToInt32(Itembal);
                                    OtherDrugs.DrugID = Drugid;
                                    OTConsumptionDetails.InSufficientOtherDrugs.Add(OtherDrugs);
                                }

                            }

                        }
                        /*Stock Master*/
                        var StockMaster = WYNKContext.StockMaster.Where(x => x.DocumentNumber == OTConsumptionDetails.RunningNo).FirstOrDefault();
                        string stockmasterIdentity = "";
                        if (OTConsumptionDetails.InSufficientDrugs.Count == 0 && OTConsumptionDetails.InSufficientSerials.Count == 0 && OTConsumptionDetails.InSufficientOtherDrugs.Count == 0)
                        {

                            var stockmas = AddBilling.AddstockMaster1(OTConsumptionDetails.RunningNo, DateTime.Now, Convert.ToInt32(OTConsumptionDetails.storeID), null, 0, OTConsumptionDetails.Tc, CMPSContext.TransactionType.Where(x => x.TransactionID == OTConsumptionDetails.Tc).Select(x => x.Rec_Issue_type).FirstOrDefault(), OTConsumptionDetails.Cmpid, OTConsumptionDetails.CreatedBy, Fyear);
                            WYNKContext.StockMaster.Add(stockmas);


                            string cmpname = CMPSContext.Company.Where(x => x.CmpID == OTConsumptionDetails.Cmpid).Select(x => x.CompanyName).FirstOrDefault();
                            string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == OTConsumptionDetails.CreatedBy).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                            string userid = Convert.ToString(OTConsumptionDetails.CreatedBy);
                            ErrorLog oErrorLogs = new ErrorLog();
                            oErrorLogs.WriteErrorLogTitle(cmpname, "Intra Operative", "User name :", username, "User ID :", userid, "Mode : Add");
                            object names = stockmas;
                            oErrorLogs.WriteErrorLogArray("StockMaster", names);

                            WYNKContext.SaveChanges();
                            stockmasterIdentity = stockmas.RandomUniqueID;

                            if (AllotingBatchs.Count > 0)
                            {
                                foreach (var item2 in AllotingBatchs.ToList())
                                {

                                    var itemBatch = new ItemBatch();
                                    itemBatch = WYNKContext.ItemBatch.Where(x => x.ItemBatchNumber == item2.itemBatchNo && x.ItemID == item2.DrugId && x.StoreID == OTConsumptionDetails.storeID && x.ItemBatchExpiry.Date == Convert.ToDateTime(item2.ExpiryDate) && x.cmpID == OTConsumptionDetails.Cmpid).FirstOrDefault();
                                    itemBatch.ItemBatchBalnceQty = itemBatch.ItemBatchBalnceQty - item2.GoingToIssue;
                                    itemBatch.ItemBatchissueQty = itemBatch.ItemBatchissueQty + item2.GoingToIssue;
                                    itemBatch.LockedQuantity = itemBatch.LockedQuantity - Convert.ToInt32(item2.GoingToIssue);
                                    WYNKContext.ItemBatch.UpdateRange(itemBatch);

                                    var CurrentMonth = DateTime.Now.Month;
                                    var ItemBalance = WYNKContext.ItemBalance.Where(x => x.ItemID == item2.DrugId && x.FYear == Convert.ToInt32(Fyear) && x.StoreID == OTConsumptionDetails.storeID && x.CmpID == OTConsumptionDetails.Cmpid).FirstOrDefault();
                                    switch (CurrentMonth)
                                    {
                                        case 1:
                                            ItemBalance.Iss01 = ItemBalance.Iss01 + item2.GoingToIssue;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance - item2.GoingToIssue;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 2:
                                            ItemBalance.Iss02 = ItemBalance.Iss02 + item2.GoingToIssue;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance - item2.GoingToIssue;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 3:
                                            ItemBalance.Iss03 = ItemBalance.Iss03 + item2.GoingToIssue;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance - item2.GoingToIssue;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 4:
                                            ItemBalance.Iss04 = ItemBalance.Iss04 + item2.GoingToIssue;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + 1;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 5:
                                            ItemBalance.Iss05 = ItemBalance.Iss05 + item2.GoingToIssue;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance - item2.GoingToIssue;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 6:
                                            ItemBalance.Iss06 = ItemBalance.Iss06 + item2.GoingToIssue;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance - item2.GoingToIssue;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 7:
                                            ItemBalance.Iss07 = ItemBalance.Iss07 + item2.GoingToIssue;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance - item2.GoingToIssue;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 8:
                                            ItemBalance.Iss08 = ItemBalance.Iss08 + item2.GoingToIssue;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance - item2.GoingToIssue;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 9:
                                            ItemBalance.Iss09 = ItemBalance.Iss09 + item2.GoingToIssue;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance - item2.GoingToIssue;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 10:
                                            ItemBalance.Iss10 = ItemBalance.Iss10 + item2.GoingToIssue;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance - item2.GoingToIssue;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 11:
                                            ItemBalance.Iss11 = ItemBalance.Iss11 + item2.GoingToIssue;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance - item2.GoingToIssue;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 12:
                                            ItemBalance.Iss12 = ItemBalance.Iss12 + item2.GoingToIssue;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance - item2.GoingToIssue;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                    }

                                    object iB1 = itemBatch;
                                    oErrorLogs.WriteErrorLogArray("ItemBatch", iB1);


                                    var drugGroup = WYNKContext.DrugGroup.Where(x => x.ID == WYNKContext.DrugMaster.Where(y => y.ID == item2.DrugId).Select(y => y.GenericName).FirstOrDefault()).FirstOrDefault();
                                    var IntervalDays = drugGroup.RetestInterval + drugGroup.RetestCriticalInterval;
                                    var DaysDifference = Math.Ceiling((itemBatch.ItemBatchExpiry - DateTime.Now).TotalDays);
                                    if (DaysDifference <= IntervalDays)
                                    {
                                        var alert = new Alert();
                                        alert.DrugName = WYNKContext.DrugMaster.Where(y => y.ID == item2.DrugId).Select(y => y.Brand).FirstOrDefault();
                                        alert.BatchNo = item2.itemBatchNo;
                                        alert.ExpiresInDays = Convert.ToInt32(DaysDifference);
                                        Alerts.Add(alert);
                                    }

                                    var Uom = uommaster.Where(u => u.Description == WYNKContext.DrugMaster.Where(x => x.ID == item2.DrugId).Select(x => x.UOM).FirstOrDefault()).Select(x => x.id).FirstOrDefault();

                                    var stockTran = WYNKContext.StockTran.Where(x => x.SMID == stockmas.RandomUniqueID && x.ItemID == item2.DrugId).FirstOrDefault();

                                    if (stockTran == null)
                                    {
                                        var stockTrans = new StockTran();
                                        stockTrans.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                                        stockTrans.SMID = stockmasterIdentity;
                                        stockTrans.ItemID = item2.DrugId;
                                        stockTrans.ItemQty = item2.GoingToIssue;
                                        stockTrans.ItemRate = WYNKContext.DrugMaster.Where(x => x.ID == item2.DrugId).Select(x => x.Rate).FirstOrDefault();
                                        stockTrans.ItemValue = stockTrans.ItemRate * stockTrans.ItemQty;
                                        stockTrans.UOMID = Uom;
                                        stockTrans.IsDeleted = false;
                                        stockTrans.CreatedBy = OTConsumptionDetails.CreatedBy;
                                        stockTrans.CreatedUTC = DateTime.UtcNow;
                                        WYNKContext.StockTran.Add(stockTrans);

                                        object ST = stockTrans;
                                        oErrorLogs.WriteErrorLogArray("StockTran", ST);

                                        var stockMas = WYNKContext.StockMaster.Where(x => x.RandomUniqueID == stockmasterIdentity).FirstOrDefault();
                                        stockMas.TotalPOValue = stockMas.TotalPOValue != null ? (stockMas.TotalPOValue + stockTrans.ItemValue) : 0 + stockTrans.ItemValue;
                                        WYNKContext.StockMaster.UpdateRange(stockMas);

                                        object StockMaster1 = stockMas;
                                        oErrorLogs.WriteErrorLogArray("StockMaster", StockMaster);

                                        WYNKContext.SaveChanges();

                                        var ItemBatchID = WYNKContext.ItemBatch.Where(x => x.ItemBatchNumber == item2.itemBatchNo && x.ItemID == item2.DrugId && x.StoreID == OTConsumptionDetails.storeID && x.ItemBatchExpiry.Date == Convert.ToDateTime(item2.ExpiryDate) && x.cmpID == OTConsumptionDetails.Cmpid).Select(x => x.RandomUniqueID).FirstOrDefault();
                                        WYNKContext.ItemBatchTrans.Add(AddBilling.AddItemBatchTrans1(item2, stockmasterIdentity, stockTrans.RandomUniqueID, ItemBatchID, null, OTConsumptionDetails.Tc, Uom, OTConsumptionDetails.CreatedBy, OTConsumptionDetails.Cmpid));
                                        WYNKContext.SaveChanges();
                                    }
                                    else
                                    {
                                        var ItemBatchID = WYNKContext.ItemBatch.Where(x => x.ItemBatchNumber == item2.itemBatchNo && x.ItemID == item2.DrugId && x.StoreID == OTConsumptionDetails.storeID && x.ItemBatchExpiry.Date == Convert.ToDateTime(item2.ExpiryDate) && x.cmpID == OTConsumptionDetails.Cmpid).Select(x => x.RandomUniqueID).FirstOrDefault();
                                        WYNKContext.ItemBatchTrans.Add(AddBilling.AddItemBatchTrans1(item2, stockmasterIdentity, stockTran.RandomUniqueID, ItemBatchID, null, OTConsumptionDetails.Tc, Uom, OTConsumptionDetails.CreatedBy, OTConsumptionDetails.Cmpid));

                                        stockTran.ItemQty = stockTran.ItemQty + item2.GoingToIssue;
                                        WYNKContext.StockTran.UpdateRange(stockTran);

                                        object ST = stockTran;
                                        oErrorLogs.WriteErrorLogArray("StockTran", ST);
                                        WYNKContext.SaveChanges();
                                    }
                                    WYNKContext.SaveChanges();
                                }
                            }
                            if (AllotingSerial.Count > 0)
                            {
                                var Date = DateTime.Now;
                                var CurrentMonth = Date.Month;

                                foreach (var item in AllotingSerial.ToList())
                                {

                                    var ItemBalance = WYNKContext.ItemBalance.Where(x => x.FYear == Convert.ToInt32(Fyear) && x.ItemID == item.DrugID && x.StoreID == OTConsumptionDetails.storeID && x.CmpID == OTConsumptionDetails.Cmpid).FirstOrDefault();
                                    switch (CurrentMonth)
                                    {
                                        case 1:
                                            ItemBalance.Iss01 = ItemBalance.Iss01 + 1;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance - 1;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 2:
                                            ItemBalance.Iss02 = ItemBalance.Iss02 + 1;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance - 1;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 3:
                                            ItemBalance.Iss03 = ItemBalance.Iss03 + 1;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance - 1;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 4:
                                            ItemBalance.Iss04 = ItemBalance.Iss04 + 1;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance - 1;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 5:
                                            ItemBalance.Iss05 = ItemBalance.Iss05 + 1;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance - 1;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 6:
                                            ItemBalance.Iss06 = ItemBalance.Iss06 + 1;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance - 1;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 7:
                                            ItemBalance.Iss07 = ItemBalance.Iss07 + 1;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance - 1;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 8:
                                            ItemBalance.Iss08 = ItemBalance.Iss08 + 1;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance - 1;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 9:
                                            ItemBalance.Iss09 = ItemBalance.Iss09 + 1;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance - 1;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 10:
                                            ItemBalance.Iss10 = ItemBalance.Iss10 + 1;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance - 1;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 11:
                                            ItemBalance.Iss11 = ItemBalance.Iss11 + 1;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance - 1;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 12:
                                            ItemBalance.Iss12 = ItemBalance.Iss12 + 1;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance - 1;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                    }

                                    var stockTrans = WYNKContext.StockTran.Where(x => x.SMID == stockmasterIdentity && x.ItemID == item.DrugID).FirstOrDefault();
                                    var Uom = uommaster.Where(u => u.Description == WYNKContext.DrugMaster.Where(x => x.ID == item.DrugID).Select(x => x.UOM).FirstOrDefault()).Select(x => x.id).FirstOrDefault();
                                    if (stockTrans == null)
                                    {
                                        var stockTran = new StockTran();
                                        stockTran.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                                        stockTran.SMID = stockmasterIdentity;
                                        stockTran.ItemID = Convert.ToInt32(item.DrugID);
                                        stockTran.ItemQty = 1;
                                        stockTran.ItemRate = WYNKContext.DrugMaster.Where(x => x.ID == item.DrugID).Select(x => x.Rate).FirstOrDefault();
                                        stockTran.ItemValue = stockTran.ItemRate * stockTran.ItemQty;
                                        stockTran.UOMID = Uom;
                                        stockTran.IsDeleted = false;
                                        stockTran.CreatedBy = OTConsumptionDetails.CreatedBy;
                                        stockTran.CreatedUTC = DateTime.UtcNow;
                                        WYNKContext.StockTran.Add(stockTran);

                                        object ST = stockTran;
                                        oErrorLogs.WriteErrorLogArray("StockTran", ST);

                                        var stockMas = new StockMaster();
                                        stockMas = WYNKContext.StockMaster.Where(x => x.RandomUniqueID == stockmasterIdentity).FirstOrDefault();
                                        stockMas.TotalPOValue = stockMas.TotalPOValue != null ? (stockMas.TotalPOValue + stockTran.ItemRate) : 0 + stockTran.ItemRate;
                                        WYNKContext.StockMaster.UpdateRange(stockMas);
                                        WYNKContext.SaveChanges();
                                    }
                                    else
                                    {
                                        stockTrans.SMID = stockmasterIdentity;
                                        stockTrans.ItemID = Convert.ToInt32(item.DrugID);
                                        stockTrans.ItemQty = stockTrans.ItemQty + 1;
                                        stockTrans.ItemRate = WYNKContext.DrugMaster.Where(x => x.ID == item.DrugID).Select(x => x.Rate).FirstOrDefault();
                                        stockTrans.ItemValue = stockTrans.ItemRate * stockTrans.ItemQty;
                                        stockTrans.IsDeleted = false;
                                        stockTrans.CreatedBy = OTConsumptionDetails.CreatedBy;
                                        stockTrans.CreatedUTC = DateTime.UtcNow;
                                        WYNKContext.StockTran.UpdateRange(stockTrans);

                                        object ST = stockTrans;
                                        oErrorLogs.WriteErrorLogArray("StockTran", ST);

                                        var stockMas = new StockMaster();
                                        stockMas = WYNKContext.StockMaster.Where(x => x.RandomUniqueID == stockmasterIdentity).FirstOrDefault();
                                        stockMas.TotalPOValue = stockMas.TotalPOValue != null ? (stockMas.TotalPOValue + stockTrans.ItemRate) : 0 + stockTrans.ItemRate;
                                        WYNKContext.StockMaster.UpdateRange(stockMas);
                                        WYNKContext.SaveChanges();
                                    }
                                    WYNKContext.SaveChanges();
                                }

                                WYNKContext.SaveChanges();

                                foreach (var Serial in AllotingSerial.ToList())
                                {

                                    var ItemSerial = WYNKContext.ItemSerial.Where(x => x.ItemID == Serial.DrugID && x.SerialNo == Serial.SerialNo && x.StoreID == OTConsumptionDetails.storeID && x.cmpID == OTConsumptionDetails.Cmpid).FirstOrDefault();
                                    ItemSerial.IssueDate = Date;
                                    ItemSerial.IssueNo = OTConsumptionDetails.RunningNo;
                                    ItemSerial.IssueTC = OTConsumptionDetails.Tc;
                                    WYNKContext.ItemSerial.UpdateRange(ItemSerial);
                                    WYNKContext.SaveChanges();
                                }
                            }
                            if (AllotingOtherDrugs.Count > 0)
                            {
                                var Date = DateTime.Now;
                                var CurrentMonth = Date.Month;

                                foreach (var item in AllotingOtherDrugs.ToList())
                                {
                                    var ItemBalance = WYNKContext.ItemBalance.Where(x => x.FYear == Convert.ToInt32(Fyear) && x.ItemID == item.DrugID && x.StoreID == OTConsumptionDetails.storeID && x.CmpID == OTConsumptionDetails.Cmpid).FirstOrDefault();

                                    switch (CurrentMonth)
                                    {
                                        case 1:
                                            ItemBalance.Iss01 = ItemBalance.Iss01 + Convert.ToInt32(item.QtyTobeTaken);
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance - Convert.ToInt32(item.QtyTobeTaken);
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 2:
                                            ItemBalance.Iss02 = ItemBalance.Iss02 + Convert.ToInt32(item.QtyTobeTaken);
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance - Convert.ToInt32(item.QtyTobeTaken);
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 3:
                                            ItemBalance.Iss03 = ItemBalance.Iss03 + Convert.ToInt32(item.QtyTobeTaken);
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance - Convert.ToInt32(item.QtyTobeTaken);
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 4:
                                            ItemBalance.Iss04 = ItemBalance.Iss04 + Convert.ToInt32(item.QtyTobeTaken);
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance - Convert.ToInt32(item.QtyTobeTaken);
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 5:
                                            ItemBalance.Iss05 = ItemBalance.Iss05 + Convert.ToInt32(item.QtyTobeTaken);
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance - Convert.ToInt32(item.QtyTobeTaken);
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 6:
                                            ItemBalance.Iss06 = ItemBalance.Iss06 + Convert.ToInt32(item.QtyTobeTaken);
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance - Convert.ToInt32(item.QtyTobeTaken);
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 7:
                                            ItemBalance.Iss07 = ItemBalance.Iss07 + Convert.ToInt32(item.QtyTobeTaken);
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance - Convert.ToInt32(item.QtyTobeTaken);
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 8:
                                            ItemBalance.Iss08 = ItemBalance.Iss08 + Convert.ToInt32(item.QtyTobeTaken);
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance - Convert.ToInt32(item.QtyTobeTaken);
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 9:
                                            ItemBalance.Iss09 = ItemBalance.Iss09 + Convert.ToInt32(item.QtyTobeTaken);
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance - Convert.ToInt32(item.QtyTobeTaken);
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 10:
                                            ItemBalance.Iss10 = ItemBalance.Iss10 + Convert.ToInt32(item.QtyTobeTaken);
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance - Convert.ToInt32(item.QtyTobeTaken);
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 11:
                                            ItemBalance.Iss11 = ItemBalance.Iss11 + Convert.ToInt32(item.QtyTobeTaken);
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance - Convert.ToInt32(item.QtyTobeTaken);
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 12:
                                            ItemBalance.Iss12 = ItemBalance.Iss12 + Convert.ToInt32(item.QtyTobeTaken);
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance - Convert.ToInt32(item.QtyTobeTaken);
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;

                                    }

                                    var stockTrans = WYNKContext.StockTran.Where(x => x.SMID == stockmasterIdentity && x.ItemID == item.DrugID).FirstOrDefault();
                                    var Uom = uommaster.Where(u => u.Description == WYNKContext.DrugMaster.Where(x => x.ID == item.DrugID).Select(x => x.UOM).FirstOrDefault()).Select(x => x.id).FirstOrDefault();

                                    if (stockTrans == null)
                                    {
                                        var stockTran = new StockTran();
                                        stockTran.SMID = stockmasterIdentity;
                                        stockTran.ItemID = Convert.ToInt32(item.DrugID);
                                        stockTran.ItemQty = Convert.ToDecimal(item.QtyTobeTaken);
                                        stockTran.ItemRate = WYNKContext.DrugMaster.Where(x => x.ID == item.DrugID).Select(x => x.Rate).FirstOrDefault();
                                        stockTran.ItemValue = stockTran.ItemRate * stockTran.ItemQty;
                                        stockTran.UOMID = Uom;
                                        stockTran.IsDeleted = false;
                                        stockTran.CreatedBy = OTConsumptionDetails.CreatedBy;
                                        stockTran.CreatedUTC = DateTime.UtcNow;
                                        WYNKContext.StockTran.Add(stockTran);

                                        object ST = stockTran;
                                        oErrorLogs.WriteErrorLogArray("StockTran", ST);

                                        var stockMas = WYNKContext.StockMaster.Where(x => x.RandomUniqueID == stockmasterIdentity).FirstOrDefault();
                                        stockMas.TotalPOValue = stockMas.TotalPOValue != null ? (stockMas.TotalPOValue + (stockTran.ItemRate * stockTran.ItemQty)) : 0 + stockTran.ItemRate;
                                        WYNKContext.StockMaster.UpdateRange(stockMas);
                                        WYNKContext.SaveChanges();

                                    }
                                    else
                                    {
                                        stockTrans.ItemQty = stockTrans.ItemQty + item.QtyTobeTaken;
                                        WYNKContext.StockTran.UpdateRange(stockTrans);

                                        object ST = stockTrans;
                                        oErrorLogs.WriteErrorLogArray("StockTran", ST);
                                        WYNKContext.SaveChanges();
                                    }


                                }


                            }

                            var SurgeryID = WYNKContext.SurgeryAssigned.Where(x => x.RandomUniqueID == OTConsumptionDetails.SAID).Select(x => x.SurgeryID).FirstOrDefault();

                            var Surgery = WYNKContext.Surgery.Where(x => x.RandomUniqueID == SurgeryID).FirstOrDefault();
                            Surgery.SMID = stockmasterIdentity;
                            WYNKContext.Surgery.UpdateRange(Surgery);

                            var IcdSpecialityCode = WYNKContext.SurgeryTran.Where(x => x.SurgeryID == SurgeryID).Select(x => x.IcdSpecialityCode).FirstOrDefault();

                            var IcdSpecialityDescription = WYNKContext.ICDSpecialityCode.Where(x => x.ID == IcdSpecialityCode).Select(x => x.SpecialityDescription).FirstOrDefault();

                            if (OTConsumptionDetails.SurgeryPerformedDoctors.Count > 0)
                            {
                                /* Check Whether it is Camp or Normal Registraion */
                                var IsCamp = WYNKContext.Registration.Where(x => x.UIN == OTConsumptionDetails.UIN && x.CMPID == OTConsumptionDetails.Cmpid).Select(x => x.RegType).FirstOrDefault();

                                if (IsCamp == "CampRegistration")
                                {
                                    /* Camp Registraion */
                                    var Camp = WYNKContext.CampRegistration.Where(x => x.CampUIN == OTConsumptionDetails.UIN && x.CMPID == OTConsumptionDetails.Cmpid).FirstOrDefault();
                                    int CampIds = Camp.CampID;

                                    /* update Camp Registraion table IsSurgeryDone Column */
                                    var CampReg = WYNKContext.CampRegistration.Where(x => x.CampUIN == OTConsumptionDetails.UIN && x.CMPID == OTConsumptionDetails.Cmpid).FirstOrDefault();
                                    CampReg.IsSurgeryDone = true;
                                    WYNKContext.CampRegistration.UpdateRange(CampReg);


                                    var Age = PasswordEncodeandDecode.ToAgeString(WYNKContext.Registration.Where(x => x.UIN == OTConsumptionDetails.UIN && x.CMPID == OTConsumptionDetails.Cmpid).Select(x => x.DateofBirth).FirstOrDefault());
                                    var result = Regex.Match(Age, @"\d+").Value;
                                    var PaediatricAge = CMPSContext.Setup.Where(x => x.CMPID == OTConsumptionDetails.Cmpid).Select(x => x.Pediatric).FirstOrDefault();
                                    var IsArtificialeye = WYNKContext.RegistrationExtension.Where(x => x.UIN == OTConsumptionDetails.UIN && x.CMPID == OTConsumptionDetails.Cmpid).FirstOrDefault();

                                    /* update CampPatientFootfall table SurgeryUnderWent Column */
                                    var CampPatientFootfall = WYNKContext.CampPatientFootfall.Where(x => x.Date == Camp.DateofRegistration.Date && x.CmpID == OTConsumptionDetails.Cmpid && x.CampID == CampIds).FirstOrDefault();
                                    CampPatientFootfall.SurgeryUnderwentPatient = CampPatientFootfall.SurgeryUnderwentPatient + 1;
                                    CampPatientFootfall.Updatedby = OTConsumptionDetails.CreatedBy;
                                    CampPatientFootfall.UpdatedUTC = DateTime.UtcNow;
                                    WYNKContext.CampPatientFootfall.UpdateRange(CampPatientFootfall);
                                    WYNKContext.SaveChanges();

                                    /* Insert CampPatientFootfallTran table SurgeryUnderWent Column */
                                    var CampPatientFootfallTran = new CampPatientFootfallTran();
                                    CampPatientFootfallTran.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                                    CampPatientFootfallTran.CAMPFFId = CampPatientFootfall.RandomUniqueID;
                                    CampPatientFootfallTran.CmpID = OTConsumptionDetails.Cmpid;
                                    CampPatientFootfallTran.DateofSurgery = OTConsumptionDetails.SurgeryDateTime.Date;
                                    CampPatientFootfallTran.SpecialityID = IcdSpecialityCode;
                                    CampPatientFootfallTran.SpecialityDesc = IcdSpecialityDescription;
                                    if (Convert.ToInt32(PaediatricAge) > Convert.ToInt32(result))
                                    {
                                        if (IsArtificialeye != null)
                                        {
                                            CampPatientFootfall.PaediatricOcular = 1;
                                            CampPatientFootfall.PaediatricNormal = 0;
                                        }
                                        else
                                        {
                                            CampPatientFootfall.PaediatricNormal = 1;
                                            CampPatientFootfall.PaediatricOcular = 0;
                                        }
                                        CampPatientFootfall.GeneralNormal = 0;
                                        CampPatientFootfall.GeneralOcular = 0;
                                    }
                                    else
                                    {
                                        if (IsArtificialeye != null)
                                        {
                                            CampPatientFootfall.GeneralOcular = 1;
                                            CampPatientFootfall.GeneralNormal = 0;
                                        }
                                        else
                                        {
                                            CampPatientFootfall.GeneralNormal = 1;
                                            CampPatientFootfall.GeneralOcular = 0;
                                        }
                                        CampPatientFootfall.PaediatricOcular = 0;
                                        CampPatientFootfall.PaediatricNormal = 0;
                                    }
                                    CampPatientFootfallTran.CreatedUTC = DateTime.UtcNow;
                                    CampPatientFootfallTran.Createdby = OTConsumptionDetails.CreatedBy;
                                    WYNKContext.CampPatientFootfallTran.AddRange(CampPatientFootfallTran);
                                    WYNKContext.SaveChanges();

                                    foreach (var SurgeryPerformedDoctor in OTConsumptionDetails.SurgeryPerformedDoctors.ToList())
                                    {
                                        var CampPatientFootDoctor = new CampPatientFootDoctor();
                                        CampPatientFootDoctor.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                                        CampPatientFootDoctor.CampFRTID = CampPatientFootfallTran.RandomUniqueID;
                                        CampPatientFootDoctor.DoctorID = SurgeryPerformedDoctor.ID;
                                        CampPatientFootDoctor.SurgeryID = SurgeryID;
                                        CampPatientFootDoctor.CreatedUTC = DateTime.UtcNow;
                                        CampPatientFootDoctor.Createdby = OTConsumptionDetails.CreatedBy;
                                        WYNKContext.CampPatientFootDoctor.AddRange(CampPatientFootDoctor);
                                    }
                                    WYNKContext.SaveChanges();
                                }
                                else
                                {
                                    foreach (var SurgeryPerformedDoctor in OTConsumptionDetails.SurgeryPerformedDoctors.ToList())
                                    {
                                        /* Normal Registraion */
                                        var revenues = WYNKContext.RevenueSummary.Where(x => x.DoctorID == SurgeryPerformedDoctor.ID && x.CmpID == OTConsumptionDetails.Cmpid && x.Date.Date == DateTime.UtcNow.Date && x.SpecialityID == IcdSpecialityCode).FirstOrDefault();
                                        if (revenues == null)
                                        {
                                            var revenue = new RevenueSummary();
                                            revenue.DoctorID = SurgeryPerformedDoctor.ID;
                                            revenue.CmpID = OTConsumptionDetails.Cmpid;
                                            revenue.SpecialityID = IcdSpecialityCode;
                                            revenue.SpecialityDesc = IcdSpecialityDescription;
                                            revenue.Date = DateTime.UtcNow.Date;
                                            revenue.Numbers = 1;
                                            revenue.Amount = OTConsumptionDetails.SurgeryPackageCost;
                                            revenue.CreatedUTC = DateTime.UtcNow;
                                            revenue.CreatedBy = OTConsumptionDetails.CreatedBy;
                                            WYNKContext.RevenueSummary.AddRange(revenue);
                                            WYNKContext.SaveChanges();
                                        }
                                        else
                                        {
                                            revenues.Numbers += 1;
                                            revenues.Amount += OTConsumptionDetails.SurgeryPackageCost;
                                            revenues.UpdatedUTC = DateTime.UtcNow;
                                            revenues.UpdatedBy = OTConsumptionDetails.CreatedBy;
                                            WYNKContext.RevenueSummary.UpdateRange(revenues);
                                            WYNKContext.SaveChanges();
                                        }
                                    }
                                }
                            }

                            WYNKContext.SaveChanges();

                            var SurgeryAssigned = WYNKContext.SurgeryAssigned.Where(x => x.RandomUniqueID == OTConsumptionDetails.SAID).FirstOrDefault();
                            SurgeryAssigned.FromTime = OTConsumptionDetails.FromTime;
                            SurgeryAssigned.ToTime = OTConsumptionDetails.ToTime;
                            SurgeryAssigned.IsSurgeryCompleted = true;
                            WYNKContext.SurgeryAssigned.UpdateRange(SurgeryAssigned);

                            object SA = SurgeryAssigned;
                            oErrorLogs.WriteErrorLogArray("SurgeryAssigned", SA);

                            WYNKContext.SaveChanges();

                            var AdmID = WYNKContext.SurgeryAssigned.Where(x => x.RandomUniqueID == OTConsumptionDetails.SAID).Select(x => x.Admid).FirstOrDefault();

                            var Admission = WYNKContext.Admission.Where(x => x.RandomUniqueID == AdmID).FirstOrDefault();
                            Admission.IsSurgeryCompleted = true;
                            WYNKContext.Admission.UpdateRange(Admission);

                            object AD = Admission;
                            oErrorLogs.WriteErrorLogArray("Admission", AD);

                            WYNKContext.SaveChanges();

                            if (OTConsumptionDetails.OtImages.Count >= 1)
                            {
                                foreach (var item in OTConsumptionDetails.OtImages.ToList())
                                {
                                    var otImagePath = new OtImagepath();
                                    otImagePath.SurgeryID = SurgeryID;
                                    otImagePath.CreatedUTC = DateTime.UtcNow;
                                    otImagePath.CreatedBy = OTConsumptionDetails.CreatedBy;
                                    WYNKContext.OtImagepath.Add(otImagePath);

                                    object ot = otImagePath;
                                    oErrorLogs.WriteErrorLogArray("OtImagepath", ot);

                                    WYNKContext.SaveChanges();
                                }
                            }

                            foreach (var BeforeSkinIncision in OTConsumptionDetails.BeforeSkinIncisions.ToList())
                            {
                                var SSC = new SscResponse();
                                SSC.SSCID = BeforeSkinIncision.Sscid;
                                SSC.SAID = OTConsumptionDetails.SAID;
                                SSC.CmpID = OTConsumptionDetails.Cmpid;
                                SSC.OLMID = BeforeSkinIncision.OLMID;
                                if (BeforeSkinIncision.Yes)
                                {
                                    SSC.Response = 1;
                                }
                                else if (BeforeSkinIncision.No)
                                {

                                    SSC.Response = 0;
                                }
                                else
                                {
                                    SSC.Response = 2;
                                }
                                SSC.Description = BeforeSkinIncision.Description;
                                SSC.IsActive = true;
                                SSC.IsDeleted = false;
                                SSC.CreatedUTC = DateTime.UtcNow;
                                SSC.CreatedBy = OTConsumptionDetails.CreatedBy;
                                WYNKContext.SscResponse.Add(SSC);

                                object SSCS = SSC;
                                oErrorLogs.WriteErrorLogArray("SscResponse", SSCS);


                            }

                            foreach (var patientLeaves in OTConsumptionDetails.BeforePatientLeaves.ToList())
                            {
                                var SSC = new SscResponse();
                                SSC.SSCID = patientLeaves.Sscid;
                                SSC.SAID = OTConsumptionDetails.SAID;
                                SSC.CmpID = OTConsumptionDetails.Cmpid;
                                SSC.OLMID = patientLeaves.OLMID;
                                if (patientLeaves.Yes)
                                {
                                    SSC.Response = 1;
                                }
                                else if (patientLeaves.No)
                                {

                                    SSC.Response = 0;
                                }
                                else
                                {
                                    SSC.Response = 2;
                                }
                                SSC.Description = patientLeaves.Description;
                                SSC.IsActive = true;
                                SSC.IsDeleted = false;
                                SSC.CreatedUTC = DateTime.UtcNow;
                                SSC.CreatedBy = OTConsumptionDetails.CreatedBy;
                                WYNKContext.SscResponse.Add(SSC);

                                object SSCS = SSC;
                                oErrorLogs.WriteErrorLogArray("SscResponse", SSCS);


                            }


                            var Monitoring = new Monitoring();
                            Monitoring.CMPID = OTConsumptionDetails.Cmpid;
                            Monitoring.SAID = OTConsumptionDetails.SAID;
                            Monitoring.UIN = OTConsumptionDetails.UIN;
                            Monitoring.ECG = OTConsumptionDetails.Monitoring.ECG != null ? Convert.ToBoolean(OTConsumptionDetails.Monitoring.ECG) : (Boolean?)null;
                            Monitoring.ECGLeadsDesc = OTConsumptionDetails.Monitoring.ECGLeadsDesc;
                            Monitoring.SpO2 = OTConsumptionDetails.Monitoring.SpO2 != null ? Convert.ToBoolean(OTConsumptionDetails.Monitoring.SpO2) : (Boolean?)null;
                            Monitoring.SpO2SiteDesc = OTConsumptionDetails.Monitoring.SpO2SiteDesc;
                            Monitoring.Temp = OTConsumptionDetails.Monitoring.Temp;
                            Monitoring.TempDesc = OTConsumptionDetails.Monitoring.TempDesc;
                            Monitoring.FiO2 = OTConsumptionDetails.Monitoring.FiO2 != null ? Convert.ToBoolean(OTConsumptionDetails.Monitoring.FiO2) : (Boolean?)null;
                            Monitoring.ETCO2 = OTConsumptionDetails.Monitoring.ETCO2;
                            Monitoring.ETCO2Desc = OTConsumptionDetails.Monitoring.ETCO2Desc;
                            Monitoring.InspETAA = OTConsumptionDetails.Monitoring.InspETAA != null ? Convert.ToBoolean(OTConsumptionDetails.Monitoring.InspETAA) : (Boolean?)null;
                            Monitoring.AirwayPress = OTConsumptionDetails.Monitoring.AirwayPress != null ? Convert.ToBoolean(OTConsumptionDetails.Monitoring.AirwayPress) : (Boolean?)null;
                            Monitoring.NIBP = OTConsumptionDetails.Monitoring.NIBP != null ? Convert.ToBoolean(OTConsumptionDetails.Monitoring.NIBP) : (Boolean?)null;
                            Monitoring.IBP = OTConsumptionDetails.Monitoring.IBP != null ? Convert.ToBoolean(OTConsumptionDetails.Monitoring.IBP) : (Boolean?)null;
                            Monitoring.IBPSiteDesc = OTConsumptionDetails.Monitoring.IBPSiteDesc;
                            Monitoring.CVP = OTConsumptionDetails.Monitoring.CVP != null ? Convert.ToBoolean(OTConsumptionDetails.Monitoring.CVP) : (Boolean?)null;
                            Monitoring.CVPSiteDesc = OTConsumptionDetails.Monitoring.CVPSiteDesc;
                            Monitoring.PCWP = OTConsumptionDetails.Monitoring.PCWP != null ? Convert.ToBoolean(OTConsumptionDetails.Monitoring.PCWP) : (Boolean?)null;
                            Monitoring.Resp = OTConsumptionDetails.Monitoring.Resp != null ? Convert.ToBoolean(OTConsumptionDetails.Monitoring.Resp) : (Boolean?)null;
                            Monitoring.RespSource = OTConsumptionDetails.Monitoring.RespSource;
                            Monitoring.BloodLoss = OTConsumptionDetails.Monitoring.BloodLoss != null ? Convert.ToBoolean(OTConsumptionDetails.Monitoring.BloodLoss) : (Boolean?)null;
                            Monitoring.UrineOutput = OTConsumptionDetails.Monitoring.UrineOutput != null ? Convert.ToBoolean(OTConsumptionDetails.Monitoring.UrineOutput) : (Boolean?)null;
                            Monitoring.CreatedUTC = DateTime.UtcNow;
                            Monitoring.CreatedBy = OTConsumptionDetails.CreatedBy;
                            WYNKContext.Monitoring.Add(Monitoring);


                            object Monitori = Monitoring;
                            oErrorLogs.WriteErrorLogArray("Monitoring", Monitori);

                            WYNKContext.SaveChanges();


                            var IoMaster = new IOMaster();
                            IoMaster.SurgeryPerformedDate = OTConsumptionDetails.SurgeryDateTime;
                            IoMaster.CMPID = OTConsumptionDetails.Cmpid;
                            IoMaster.Surgeryid = SurgeryID;
                            IoMaster.ADMID = WYNKContext.Surgery.Where(sur => sur.RandomUniqueID == WYNKContext.SurgeryAssigned.Where(x => x.RandomUniqueID == OTConsumptionDetails.SAID).Select(x => x.SurgeryID).FirstOrDefault()).Select(sur => sur.AdmID).FirstOrDefault();
                            IoMaster.AddlNotes = OTConsumptionDetails.OTNotes;
                            IoMaster.CreatedUTC = DateTime.UtcNow;
                            IoMaster.CreatedBy = OTConsumptionDetails.CreatedBy;
                            WYNKContext.IOMaster.Add(IoMaster);

                            WYNKContext.SaveChanges();

                            var SurgeryHistory = new SurgeryHistory();
                            SurgeryHistory.Cmpid = OTConsumptionDetails.Cmpid;
                            SurgeryHistory.SurgeryID = SurgeryID;
                            SurgeryHistory.UIN = OTConsumptionDetails.UIN;
                            SurgeryHistory.DateofSurgery = DateTime.UtcNow;
                            SurgeryHistory.TypeofSurgery = WYNKContext.ICDSpecialityCode.Where(s => s.ID == WYNKContext.SurgeryTran.Where(x => x.SurgeryID == SurgeryID).Select(x => x.IcdSpecialityCode).FirstOrDefault()).Select(s => s.SpecialityDescription).FirstOrDefault();
                            SurgeryHistory.Eye = WYNKContext.SurgeryTran.Where(x => x.SurgeryID == SurgeryID).Select(x => x.IsOD != false ? "OD" : x.IsOS != false ? "OS" : "OU").FirstOrDefault();
                            SurgeryHistory.SurgeryDone = "Internal";
                            SurgeryHistory.IsDeleted = false;
                            SurgeryHistory.CreatedUTC = DateTime.UtcNow;
                            SurgeryHistory.CreatedBy = OTConsumptionDetails.CreatedBy;
                            WYNKContext.SurgeryHistory.Add(SurgeryHistory);


                            WYNKContext.SaveChanges();



                            foreach (var IOnotes in OTConsumptionDetails.IntraOperativeNotes.ToList())
                            {
                                var Iotran = new IOTran();
                                Iotran.IOMID = IoMaster.ID;
                                Iotran.IOTemplateID = IOnotes.IOProcedureTempID == null ? IOnotes.IOProcedureTempID : Convert.ToInt32(IOnotes.IOProcedureTempID);
                                Iotran.IOTemplateTranID = IOnotes.IOTempTranID == null ? IOnotes.IOTempTranID : Convert.ToInt32(IOnotes.IOTempTranID);
                                if (IOnotes.UserInputType != "None")
                                {
                                    Iotran.OTNotesDescription = IOnotes.OTNotesDescription + " " + IOnotes.GivenInputValue;
                                }
                                else
                                {
                                    Iotran.OTNotesDescription = IOnotes.OTNotesDescription;
                                }
                                Iotran.CreatedUTC = DateTime.UtcNow;
                                Iotran.CreatedBy = OTConsumptionDetails.CreatedBy;
                                WYNKContext.IOTran.Add(Iotran);


                                object Iotra = Iotran;
                                oErrorLogs.WriteErrorLogArray("IOTran", Iotra);
                                WYNKContext.SaveChanges();
                            }
                            WYNKContext.SaveChanges();

                            var PatAcc = WYNKContext.PatientAccount.Where(x => x.UIN == OTConsumptionDetails.UIN && x.InvoiceNumber == null && x.InvoiceDate == null && x.TransactionID == OTConsumptionDetails.billtranid).FirstOrDefault();

                            var ServiceTypeID = WYNKContext.Services.Where(x => x.Description == "Surgery Cost" && x.CMPID == OTConsumptionDetails.Cmpid).Select(x => x.ID).FirstOrDefault();

                            // var Amount = WYNKContext.ServiceMaster.Where(x => x.ChildDescription == Convert.ToString(ServiceTypeID) && x.Icdcode == IcdSpecialityCode).Select(x => x.TotalAmount).FirstOrDefault();

                            if (OTConsumptionDetails.BillingType == "Pkg")
                            {
                                var Amount = OTConsumptionDetails.SurgeryPackageCost;
                                if (PatAcc == null)
                                {
                                    var PatientAccount = new PatientAccount();

                                    PatientAccount.UIN = OTConsumptionDetails.UIN;
                                    PatientAccount.TotalProductValue = Amount;
                                    PatientAccount.TotalBillValue = Amount;
                                    PatientAccount.Description = OTConsumptionDetails.LensDescription;
                                    PatientAccount.CreatedUTC = DateTime.UtcNow;
                                    PatientAccount.CreatedBy = OTConsumptionDetails.CreatedBy;
                                    PatientAccount.CMPID = OTConsumptionDetails.Cmpid;
                                    PatientAccount.TransactionID = OTConsumptionDetails.billtranid;
                                    WYNKContext.PatientAccount.Add(PatientAccount);

                                    object PatientAccoun = PatientAccount;
                                    oErrorLogs.WriteErrorLogArray("PatientAccount", PatientAccoun);

                                    WYNKContext.SaveChanges();

                                    var PatientAccountDetail = new PatientAccountDetail();
                                    PatientAccountDetail.PAID = PatientAccount.PAID;
                                    PatientAccountDetail.OLMID = IcdSpecialityCode;
                                    PatientAccountDetail.ServiceTypeID = OTConsumptionDetails.SurgeryServiceTypeID;
                                    PatientAccountDetail.ServiceDescription = "Surgery Cost";
                                    PatientAccountDetail.TotalProductValue = Amount;
                                    PatientAccountDetail.TotalBillValue = Amount;
                                    PatientAccountDetail.CreatedUTC = DateTime.UtcNow;
                                    PatientAccountDetail.CreatedBy = OTConsumptionDetails.CreatedBy;
                                    WYNKContext.PatientAccountDetail.Add(PatientAccountDetail);

                                    object PatientAccountDetai = PatientAccountDetail;
                                    oErrorLogs.WriteErrorLogArray("PatientAccountDetail", PatientAccountDetai);

                                    WYNKContext.SaveChanges();
                                }
                                else
                                {
                                    PatAcc.TotalProductValue = PatAcc.TotalProductValue != null ? PatAcc.TotalProductValue + OTConsumptionDetails.SurgeryPackageCost : 0 + OTConsumptionDetails.SurgeryPackageCost;
                                    PatAcc.TotalBillValue = PatAcc.TotalBillValue != null ? PatAcc.TotalBillValue + OTConsumptionDetails.SurgeryPackageCost : 0 + OTConsumptionDetails.SurgeryPackageCost;
                                    PatAcc.CreatedUTC = DateTime.UtcNow;
                                    PatAcc.Description = OTConsumptionDetails.LensDescription;
                                    PatAcc.UpdatedBy = OTConsumptionDetails.CreatedBy;
                                    WYNKContext.PatientAccount.UpdateRange(PatAcc);

                                    object PatAc = PatAcc;
                                    oErrorLogs.WriteErrorLogArray("PatientAccount", PatAc);

                                    WYNKContext.SaveChanges();

                                    var PatientAccountDetail = new PatientAccountDetail();
                                    PatientAccountDetail.PAID = PatAcc.PAID;
                                    PatientAccountDetail.OLMID = IcdSpecialityCode;
                                    PatientAccountDetail.ServiceTypeID = OTConsumptionDetails.SurgeryServiceTypeID;
                                    PatientAccountDetail.ServiceDescription = "Surgery Cost";
                                    PatientAccountDetail.TotalProductValue = Amount;
                                    PatientAccountDetail.TotalBillValue = Amount;
                                    PatientAccountDetail.CreatedUTC = DateTime.UtcNow;
                                    PatientAccountDetail.CreatedBy = OTConsumptionDetails.CreatedBy;
                                    WYNKContext.PatientAccountDetail.Add(PatientAccountDetail);

                                    object PatientAccountDetai = PatientAccountDetail;
                                    oErrorLogs.WriteErrorLogArray("PatientAccountDetail", PatientAccountDetai);

                                    WYNKContext.SaveChanges();
                                }
                            }
                            else
                            {
                                if (PatAcc == null)
                                {
                                    var PatientAccount = new PatientAccount();

                                    PatientAccount.UIN = OTConsumptionDetails.UIN;
                                    PatientAccount.TotalProductValue = OTConsumptionDetails.SurgeryPackageCost;
                                    PatientAccount.TotalBillValue = OTConsumptionDetails.SurgeryPackageCost;
                                    PatientAccount.Description = OTConsumptionDetails.LensDescription;
                                    PatientAccount.CreatedUTC = DateTime.UtcNow;
                                    PatientAccount.CreatedBy = OTConsumptionDetails.CreatedBy;
                                    PatientAccount.CMPID = OTConsumptionDetails.Cmpid;
                                    PatientAccount.TransactionID = OTConsumptionDetails.billtranid;
                                    WYNKContext.PatientAccount.Add(PatientAccount);
                                    WYNKContext.SaveChanges();

                                    var PatientAccountDetail = new PatientAccountDetail();
                                    PatientAccountDetail.PAID = PatientAccount.PAID;
                                    PatientAccountDetail.OLMID = IcdSpecialityCode;
                                    PatientAccountDetail.ServiceTypeID = OTConsumptionDetails.SurgeryServiceTypeID;
                                    PatientAccountDetail.ServiceDescription = "Surgery Cost";
                                    PatientAccountDetail.TotalProductValue = OTConsumptionDetails.SurgeryPackageCost;
                                    PatientAccountDetail.TotalBillValue = OTConsumptionDetails.SurgeryPackageCost;
                                    PatientAccountDetail.CreatedUTC = DateTime.UtcNow;
                                    PatientAccountDetail.CreatedBy = OTConsumptionDetails.CreatedBy;
                                    WYNKContext.PatientAccountDetail.Add(PatientAccountDetail);
                                    WYNKContext.SaveChanges();

                                    var PatAccDetailTax = new PatientAccountDetailTax();
                                    PatAccDetailTax.PAccDetailID = PatientAccountDetail.PAccDetailID;
                                    PatAccDetailTax.ServiceTypeID = OTConsumptionDetails.SurgeryServiceTypeID;
                                    PatAccDetailTax.TotalValue = OTConsumptionDetails.SurgeryPackageCost;
                                    PatAccDetailTax.CreatedUTC = DateTime.UtcNow;
                                    PatAccDetailTax.CreatedBy = OTConsumptionDetails.CreatedBy;
                                    WYNKContext.PatientAccountDetailTax.Add(PatAccDetailTax);
                                    WYNKContext.SaveChanges();


                                    var PatAccDetailTax1 = new PatientAccountDetailTax();
                                    PatAccDetailTax1.PAccDetailID = PatientAccountDetail.PAccDetailID;
                                    PatAccDetailTax1.ServiceTypeID = OTConsumptionDetails.OtServiceID;
                                    PatAccDetailTax1.TotalValue = OTConsumptionDetails.OTtheatreCost;
                                    PatAccDetailTax1.CreatedUTC = DateTime.UtcNow;
                                    PatAccDetailTax1.CreatedBy = OTConsumptionDetails.CreatedBy;
                                    WYNKContext.PatientAccountDetailTax.Add(PatAccDetailTax1);

                                    PatientAccountDetail.TotalBillValue = PatientAccountDetail.TotalBillValue + OTConsumptionDetails.OTtheatreCost;
                                    WYNKContext.PatientAccountDetail.UpdateRange(PatientAccountDetail);

                                    PatientAccount.TotalBillValue = PatientAccount.TotalBillValue + OTConsumptionDetails.OTtheatreCost;
                                    WYNKContext.PatientAccount.UpdateRange(PatientAccount);

                                    WYNKContext.SaveChanges();
                                }
                                else
                                {
                                    PatAcc.TotalProductValue = PatAcc.TotalProductValue != null ? PatAcc.TotalProductValue + OTConsumptionDetails.SurgeryPackageCost : 0 + OTConsumptionDetails.SurgeryPackageCost;
                                    PatAcc.TotalBillValue = PatAcc.TotalBillValue != null ? PatAcc.TotalBillValue + OTConsumptionDetails.SurgeryPackageCost : 0 + OTConsumptionDetails.SurgeryPackageCost;
                                    PatAcc.CreatedUTC = DateTime.UtcNow;
                                    PatAcc.Description = OTConsumptionDetails.LensDescription;
                                    PatAcc.UpdatedBy = OTConsumptionDetails.CreatedBy;
                                    WYNKContext.PatientAccount.UpdateRange(PatAcc);
                                    WYNKContext.SaveChanges();

                                    var PatientAccountDetail = new PatientAccountDetail();
                                    PatientAccountDetail.PAID = PatAcc.PAID;
                                    PatientAccountDetail.OLMID = IcdSpecialityCode;
                                    PatientAccountDetail.ServiceTypeID = OTConsumptionDetails.SurgeryServiceTypeID;
                                    PatientAccountDetail.ServiceDescription = "Surgery Cost";
                                    PatientAccountDetail.TotalProductValue = OTConsumptionDetails.SurgeryPackageCost;
                                    PatientAccountDetail.TotalBillValue = OTConsumptionDetails.SurgeryPackageCost;
                                    PatientAccountDetail.CreatedUTC = DateTime.UtcNow;
                                    PatientAccountDetail.CreatedBy = OTConsumptionDetails.CreatedBy;
                                    WYNKContext.PatientAccountDetail.Add(PatientAccountDetail);
                                    WYNKContext.SaveChanges();

                                    var PatAccDetailTax = new PatientAccountDetailTax();
                                    PatAccDetailTax.PAccDetailID = PatientAccountDetail.PAccDetailID;
                                    PatAccDetailTax.ServiceTypeID = OTConsumptionDetails.SurgeryServiceTypeID;
                                    PatAccDetailTax.TotalValue = OTConsumptionDetails.SurgeryPackageCost;
                                    PatAccDetailTax.CreatedUTC = DateTime.UtcNow;
                                    PatAccDetailTax.CreatedBy = OTConsumptionDetails.CreatedBy;
                                    WYNKContext.PatientAccountDetailTax.Add(PatAccDetailTax);
                                    WYNKContext.SaveChanges();


                                    var PatAccDetailTax1 = new PatientAccountDetailTax();
                                    PatAccDetailTax1.PAccDetailID = PatientAccountDetail.PAccDetailID;
                                    PatAccDetailTax1.ServiceTypeID = OTConsumptionDetails.OtServiceID;
                                    PatAccDetailTax1.TotalValue = OTConsumptionDetails.OTtheatreCost;
                                    PatAccDetailTax1.CreatedUTC = DateTime.UtcNow;
                                    PatAccDetailTax1.CreatedBy = OTConsumptionDetails.CreatedBy;
                                    WYNKContext.PatientAccountDetailTax.Add(PatAccDetailTax1);

                                    PatientAccountDetail.TotalBillValue = PatientAccountDetail.TotalBillValue + OTConsumptionDetails.OTtheatreCost;
                                    WYNKContext.PatientAccountDetail.UpdateRange(PatientAccountDetail);

                                    PatAcc.TotalBillValue = PatAcc.TotalBillValue + OTConsumptionDetails.OTtheatreCost;
                                    WYNKContext.PatientAccount.UpdateRange(PatAcc);

                                    WYNKContext.SaveChanges();
                                }
                            }


                            WYNKContext.SaveChanges();

                            var commonRepos = new CommonRepository(_Wynkcontext, _Cmpscontext);
                            var RunningNumber = commonRepos.GenerateRunningCtrlNoo(OTConsumptionDetails.Tc, OTConsumptionDetails.Cmpid, "GetRunningNo");

                            if (RunningNumber == OTConsumptionDetails.RunningNo)
                            {
                                commonRepos.GenerateRunningCtrlNoo(OTConsumptionDetails.Tc, OTConsumptionDetails.Cmpid, "UpdateRunningNo");
                            }
                            else
                            {
                                var GetRunningNumber = commonRepos.GenerateRunningCtrlNoo(OTConsumptionDetails.Tc, OTConsumptionDetails.Cmpid, "UpdateRunningNo");

                                var StockMasters = WYNKContext.StockMaster.Where(x => x.RandomUniqueID == stockmasterIdentity).FirstOrDefault();
                                StockMasters.DocumentNumber = GetRunningNumber;
                                WYNKContext.StockMaster.UpdateRange(StockMasters);
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
                                SurgeryID = SurgeryID,
                            };

                        }
                        else
                        {
                            foreach (var item in AllotingBatchs.ToList())
                            {
                                var itembatch = new ItemBatch();
                                itembatch = WYNKContext.ItemBatch.Where(x => x.ItemBatchNumber == item.itemBatchNo && x.ItemID == item.DrugId && x.StoreID == OTConsumptionDetails.storeID && x.ItemBatchExpiry == item.ExpiryDate).FirstOrDefault();
                                itembatch.LockedQuantity = itembatch.LockedQuantity - Convert.ToInt32(item.GoingToIssue);
                                WYNKContext.ItemBatch.UpdateRange(itembatch);

                                ErrorLog oErrorLogstran = new ErrorLog();
                                object itembatc = itembatch;
                                oErrorLogstran.WriteErrorLogArray("itembatch", itembatc);

                                WYNKContext.SaveChanges();
                            }
                            return new
                            {
                                Success = false,
                                Message = "Out Of Stock Medicines",
                                OutOfStock = OTConsumptionDetails.InSufficientDrugs,
                                OutStockSerials = OTConsumptionDetails.InSufficientSerials,
                                OutOtherDrugs = OTConsumptionDetails.InSufficientOtherDrugs,
                            };
                        }
                    }
                    else
                    {

                        string cmpname = CMPSContext.Company.Where(x => x.CmpID == OTConsumptionDetails.Cmpid).Select(x => x.CompanyName).FirstOrDefault();
                        string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == OTConsumptionDetails.CreatedBy).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                        string userid = Convert.ToString(OTConsumptionDetails.CreatedBy);
                        ErrorLog oErrorLogs = new ErrorLog();
                        oErrorLogs.WriteErrorLogTitle(cmpname, "Intra Operative", "User name :", username, "User ID :", userid, "Mode : Add");

                        var SurgeryID = WYNKContext.SurgeryAssigned.Where(x => x.RandomUniqueID == OTConsumptionDetails.SAID).Select(x => x.SurgeryID).FirstOrDefault();


                        var IcdSpecialityCode = WYNKContext.SurgeryTran.Where(x => x.SurgeryID == SurgeryID).Select(x => x.IcdSpecialityCode).FirstOrDefault();

                        var IcdSpecialityDescription = WYNKContext.ICDSpecialityCode.Where(x => x.ID == IcdSpecialityCode).Select(x => x.SpecialityDescription).FirstOrDefault();

                        var ServiceTypeID = WYNKContext.Services.Where(x => x.Description == "Surgery Cost" && x.CMPID == OTConsumptionDetails.Cmpid).Select(x => x.ID).FirstOrDefault();
                        //var Amount = WYNKContext.ServiceMaster.Where(x => x.ChildDescription == Convert.ToString(ServiceTypeID) && x.Icdcode == IcdSpecialityCode).Select(x => x.TotalAmount).FirstOrDefault();
                        //var Amount = 0;
                        //if (Amount == null)
                        //{
                        //    Amount = 0;
                        //}
                        if (OTConsumptionDetails.SurgeryPerformedDoctors.Count > 0)
                        {
                            /* Check Whether it is Camp or Normal Registraion */
                            var IsCamp = WYNKContext.Registration.Where(x => x.UIN == OTConsumptionDetails.UIN && x.CMPID == OTConsumptionDetails.Cmpid).Select(x => x.RegType).FirstOrDefault();

                            if (IsCamp == "CampRegistration")
                            {
                                /* Camp Registraion */
                                var Camp = WYNKContext.CampRegistration.Where(x => x.CampUIN == OTConsumptionDetails.UIN && x.CMPID == OTConsumptionDetails.Cmpid).FirstOrDefault();
                                int CampIds = Camp.CampID;

                                /* update Camp Registraion table IsSurgeryDone Column */
                                var CampReg = WYNKContext.CampRegistration.Where(x => x.CampUIN == OTConsumptionDetails.UIN && x.CMPID == OTConsumptionDetails.Cmpid).FirstOrDefault();
                                CampReg.IsSurgeryDone = true;
                                WYNKContext.CampRegistration.UpdateRange(CampReg);


                                var Age = PasswordEncodeandDecode.ToAgeString(WYNKContext.Registration.Where(x => x.UIN == OTConsumptionDetails.UIN && x.CMPID == OTConsumptionDetails.Cmpid).Select(x => x.DateofBirth).FirstOrDefault());
                                var result = Regex.Match(Age, @"\d+").Value;
                                var PaediatricAge = CMPSContext.Setup.Where(x => x.CMPID == OTConsumptionDetails.Cmpid).Select(x => x.Pediatric).FirstOrDefault();
                                var IsArtificialeye = WYNKContext.RegistrationExtension.Where(x => x.UIN == OTConsumptionDetails.UIN && x.CMPID == OTConsumptionDetails.Cmpid).FirstOrDefault();

                                /* update CampPatientFootfall table SurgeryUnderWent Column */
                                var CampPatientFootfall = WYNKContext.CampPatientFootfall.Where(x => x.Date == Camp.DateofRegistration.Date && x.CmpID == OTConsumptionDetails.Cmpid && x.CampID == CampIds).FirstOrDefault();
                                CampPatientFootfall.SurgeryUnderwentPatient = CampPatientFootfall.SurgeryUnderwentPatient + 1;
                                CampPatientFootfall.Updatedby = OTConsumptionDetails.CreatedBy;
                                CampPatientFootfall.UpdatedUTC = DateTime.UtcNow;
                                WYNKContext.CampPatientFootfall.UpdateRange(CampPatientFootfall);
                                WYNKContext.SaveChanges();

                                /* Insert CampPatientFootfallTran table SurgeryUnderWent Column */
                                var CampPatientFootfallTran = new CampPatientFootfallTran();
                                CampPatientFootfallTran.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                                CampPatientFootfallTran.CAMPFFId = CampPatientFootfall.RandomUniqueID;
                                CampPatientFootfallTran.CmpID = OTConsumptionDetails.Cmpid;
                                CampPatientFootfallTran.DateofSurgery = OTConsumptionDetails.SurgeryDateTime.Date;
                                CampPatientFootfallTran.SpecialityID = IcdSpecialityCode;
                                CampPatientFootfallTran.SpecialityDesc = IcdSpecialityDescription;
                                if (Convert.ToInt32(PaediatricAge) > Convert.ToInt32(result))
                                {
                                    if (IsArtificialeye != null)
                                    {
                                        CampPatientFootfall.PaediatricOcular = 1;
                                        CampPatientFootfall.PaediatricNormal = 0;
                                    }
                                    else
                                    {
                                        CampPatientFootfall.PaediatricNormal = 1;
                                        CampPatientFootfall.PaediatricOcular = 0;
                                    }
                                    CampPatientFootfall.GeneralNormal = 0;
                                    CampPatientFootfall.GeneralOcular = 0;
                                }
                                else
                                {
                                    if (IsArtificialeye != null)
                                    {
                                        CampPatientFootfall.GeneralOcular = 1;
                                        CampPatientFootfall.GeneralNormal = 0;
                                    }
                                    else
                                    {
                                        CampPatientFootfall.GeneralNormal = 1;
                                        CampPatientFootfall.GeneralOcular = 0;
                                    }
                                    CampPatientFootfall.PaediatricOcular = 0;
                                    CampPatientFootfall.PaediatricNormal = 0;
                                }
                                CampPatientFootfallTran.CreatedUTC = DateTime.UtcNow;
                                CampPatientFootfallTran.Createdby = OTConsumptionDetails.CreatedBy;
                                WYNKContext.CampPatientFootfallTran.AddRange(CampPatientFootfallTran);
                                WYNKContext.SaveChanges();

                                foreach (var SurgeryPerformedDoctor in OTConsumptionDetails.SurgeryPerformedDoctors.ToList())
                                {
                                    var CampPatientFootDoctor = new CampPatientFootDoctor();
                                    CampPatientFootDoctor.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                                    CampPatientFootDoctor.CampFRTID = CampPatientFootfallTran.RandomUniqueID;
                                    CampPatientFootDoctor.DoctorID = SurgeryPerformedDoctor.ID;
                                    CampPatientFootDoctor.SurgeryID = SurgeryID;
                                    CampPatientFootDoctor.CreatedUTC = DateTime.UtcNow;
                                    CampPatientFootDoctor.Createdby = OTConsumptionDetails.CreatedBy;
                                    WYNKContext.CampPatientFootDoctor.AddRange(CampPatientFootDoctor);
                                }
                                WYNKContext.SaveChanges();
                            }
                            else
                            {
                                foreach (var SurgeryPerformedDoctor in OTConsumptionDetails.SurgeryPerformedDoctors.ToList())
                                {
                                    /* Normal Registraion */
                                    var revenues = WYNKContext.RevenueSummary.Where(x => x.DoctorID == SurgeryPerformedDoctor.ID && x.CmpID == OTConsumptionDetails.Cmpid && x.Date.Date == DateTime.UtcNow.Date && x.SpecialityID == IcdSpecialityCode).FirstOrDefault();
                                    if (revenues == null)
                                    {
                                        var revenue = new RevenueSummary();
                                        revenue.DoctorID = SurgeryPerformedDoctor.ID;
                                        revenue.CmpID = OTConsumptionDetails.Cmpid;
                                        revenue.SpecialityID = IcdSpecialityCode;
                                        revenue.SpecialityDesc = IcdSpecialityDescription;
                                        revenue.Date = DateTime.UtcNow.Date;
                                        revenue.Numbers = 1;
                                        revenue.Amount = OTConsumptionDetails.SurgeryPackageCost;
                                        revenue.CreatedUTC = DateTime.UtcNow;
                                        revenue.CreatedBy = OTConsumptionDetails.CreatedBy;
                                        WYNKContext.RevenueSummary.AddRange(revenue);
                                        WYNKContext.SaveChanges();
                                    }
                                    else
                                    {
                                        revenues.Numbers += 1;
                                        revenues.Amount += OTConsumptionDetails.SurgeryPackageCost;
                                        revenues.UpdatedUTC = DateTime.UtcNow;
                                        revenues.UpdatedBy = OTConsumptionDetails.CreatedBy;
                                        WYNKContext.RevenueSummary.UpdateRange(revenues);
                                        WYNKContext.SaveChanges();
                                    }
                                }
                            }
                        }

                        WYNKContext.SaveChanges();


                        var SurgeryAssigned = WYNKContext.SurgeryAssigned.Where(x => x.RandomUniqueID == OTConsumptionDetails.SAID).FirstOrDefault();
                        SurgeryAssigned.FromTime = OTConsumptionDetails.FromTime;
                        SurgeryAssigned.ToTime = OTConsumptionDetails.ToTime;
                        SurgeryAssigned.IsSurgeryCompleted = true;
                        WYNKContext.SurgeryAssigned.UpdateRange(SurgeryAssigned);

                        object SA = SurgeryAssigned;
                        oErrorLogs.WriteErrorLogArray("SurgeryAssigned", SA);

                        WYNKContext.SaveChanges();

                        var AdmID = WYNKContext.SurgeryAssigned.Where(x => x.RandomUniqueID == OTConsumptionDetails.SAID).Select(x => x.Admid).FirstOrDefault();

                        var Admission = WYNKContext.Admission.Where(x => x.RandomUniqueID == AdmID).FirstOrDefault();
                        Admission.IsSurgeryCompleted = true;
                        WYNKContext.Admission.UpdateRange(Admission);

                        object AD = Admission;
                        oErrorLogs.WriteErrorLogArray("Admission", AD);

                        WYNKContext.SaveChanges();

                        if (OTConsumptionDetails.OtImages.Count >= 1)
                        {
                            foreach (var item in OTConsumptionDetails.OtImages.ToList())
                            {
                                var otImagePath = new OtImagepath();
                                otImagePath.SurgeryID = SurgeryID;
                                otImagePath.CreatedUTC = DateTime.UtcNow;
                                otImagePath.CreatedBy = OTConsumptionDetails.CreatedBy;
                                WYNKContext.OtImagepath.Add(otImagePath);

                                object ot = otImagePath;
                                oErrorLogs.WriteErrorLogArray("OtImagepath", ot);

                                WYNKContext.SaveChanges();
                            }
                        }

                        foreach (var BeforeSkinIncision in OTConsumptionDetails.BeforeSkinIncisions.ToList())
                        {
                            var SSC = new SscResponse();
                            SSC.SSCID = BeforeSkinIncision.Sscid;
                            SSC.SAID = OTConsumptionDetails.SAID;
                            SSC.CmpID = OTConsumptionDetails.Cmpid;
                            SSC.OLMID = BeforeSkinIncision.OLMID;
                            if (BeforeSkinIncision.Yes)
                            {
                                SSC.Response = 1;
                            }
                            else if (BeforeSkinIncision.No)
                            {

                                SSC.Response = 0;
                            }
                            else
                            {
                                SSC.Response = 2;
                            }
                            SSC.Description = BeforeSkinIncision.Description;
                            SSC.IsActive = true;
                            SSC.IsDeleted = false;
                            SSC.CreatedUTC = DateTime.UtcNow;
                            SSC.CreatedBy = OTConsumptionDetails.CreatedBy;
                            WYNKContext.SscResponse.Add(SSC);

                            object SSCS = SSC;
                            oErrorLogs.WriteErrorLogArray("SscResponse", SSCS);


                        }

                        foreach (var patientLeaves in OTConsumptionDetails.BeforePatientLeaves.ToList())
                        {
                            var SSC = new SscResponse();
                            SSC.SSCID = patientLeaves.Sscid;
                            SSC.SAID = OTConsumptionDetails.SAID;
                            SSC.CmpID = OTConsumptionDetails.Cmpid;
                            SSC.OLMID = patientLeaves.OLMID;
                            if (patientLeaves.Yes)
                            {
                                SSC.Response = 1;
                            }
                            else if (patientLeaves.No)
                            {

                                SSC.Response = 0;
                            }
                            else
                            {
                                SSC.Response = 2;
                            }
                            SSC.Description = patientLeaves.Description;
                            SSC.IsActive = true;
                            SSC.IsDeleted = false;
                            SSC.CreatedUTC = DateTime.UtcNow;
                            SSC.CreatedBy = OTConsumptionDetails.CreatedBy;
                            WYNKContext.SscResponse.Add(SSC);

                            object SSCS = SSC;
                            oErrorLogs.WriteErrorLogArray("SscResponse", SSCS);


                        }


                        var Monitoring = new Monitoring();
                        Monitoring.CMPID = OTConsumptionDetails.Cmpid;
                        Monitoring.SAID = OTConsumptionDetails.SAID;
                        Monitoring.UIN = OTConsumptionDetails.UIN;
                        Monitoring.ECG = OTConsumptionDetails.Monitoring.ECG != null ? Convert.ToBoolean(OTConsumptionDetails.Monitoring.ECG) : (Boolean?)null;
                        Monitoring.ECGLeadsDesc = OTConsumptionDetails.Monitoring.ECGLeadsDesc;
                        Monitoring.SpO2 = OTConsumptionDetails.Monitoring.SpO2 != null ? Convert.ToBoolean(OTConsumptionDetails.Monitoring.SpO2) : (Boolean?)null;
                        Monitoring.SpO2SiteDesc = OTConsumptionDetails.Monitoring.SpO2SiteDesc;
                        Monitoring.Temp = OTConsumptionDetails.Monitoring.Temp;
                        Monitoring.TempDesc = OTConsumptionDetails.Monitoring.TempDesc;
                        Monitoring.FiO2 = OTConsumptionDetails.Monitoring.FiO2 != null ? Convert.ToBoolean(OTConsumptionDetails.Monitoring.FiO2) : (Boolean?)null;
                        Monitoring.ETCO2 = OTConsumptionDetails.Monitoring.ETCO2;
                        Monitoring.ETCO2Desc = OTConsumptionDetails.Monitoring.ETCO2Desc;
                        Monitoring.InspETAA = OTConsumptionDetails.Monitoring.InspETAA != null ? Convert.ToBoolean(OTConsumptionDetails.Monitoring.InspETAA) : (Boolean?)null;
                        Monitoring.AirwayPress = OTConsumptionDetails.Monitoring.AirwayPress != null ? Convert.ToBoolean(OTConsumptionDetails.Monitoring.AirwayPress) : (Boolean?)null;
                        Monitoring.NIBP = OTConsumptionDetails.Monitoring.NIBP != null ? Convert.ToBoolean(OTConsumptionDetails.Monitoring.NIBP) : (Boolean?)null;
                        Monitoring.IBP = OTConsumptionDetails.Monitoring.IBP != null ? Convert.ToBoolean(OTConsumptionDetails.Monitoring.IBP) : (Boolean?)null;
                        Monitoring.IBPSiteDesc = OTConsumptionDetails.Monitoring.IBPSiteDesc;
                        Monitoring.CVP = OTConsumptionDetails.Monitoring.CVP != null ? Convert.ToBoolean(OTConsumptionDetails.Monitoring.CVP) : (Boolean?)null;
                        Monitoring.CVPSiteDesc = OTConsumptionDetails.Monitoring.CVPSiteDesc;
                        Monitoring.PCWP = OTConsumptionDetails.Monitoring.PCWP != null ? Convert.ToBoolean(OTConsumptionDetails.Monitoring.PCWP) : (Boolean?)null;
                        Monitoring.Resp = OTConsumptionDetails.Monitoring.Resp != null ? Convert.ToBoolean(OTConsumptionDetails.Monitoring.Resp) : (Boolean?)null;
                        Monitoring.RespSource = OTConsumptionDetails.Monitoring.RespSource;
                        Monitoring.BloodLoss = OTConsumptionDetails.Monitoring.BloodLoss != null ? Convert.ToBoolean(OTConsumptionDetails.Monitoring.BloodLoss) : (Boolean?)null;
                        Monitoring.UrineOutput = OTConsumptionDetails.Monitoring.UrineOutput != null ? Convert.ToBoolean(OTConsumptionDetails.Monitoring.UrineOutput) : (Boolean?)null;
                        Monitoring.CreatedUTC = DateTime.UtcNow;
                        Monitoring.CreatedBy = OTConsumptionDetails.CreatedBy;
                        WYNKContext.Monitoring.Add(Monitoring);


                        object Monitori = Monitoring;
                        oErrorLogs.WriteErrorLogArray("Monitoring", Monitori);

                        WYNKContext.SaveChanges();


                        var IoMaster = new IOMaster();
                        IoMaster.SurgeryPerformedDate = OTConsumptionDetails.SurgeryDateTime;
                        IoMaster.CMPID = OTConsumptionDetails.Cmpid;
                        IoMaster.Surgeryid = SurgeryID;
                        IoMaster.ADMID = WYNKContext.Surgery.Where(sur => sur.RandomUniqueID == WYNKContext.SurgeryAssigned.Where(x => x.RandomUniqueID == OTConsumptionDetails.SAID).Select(x => x.SurgeryID).FirstOrDefault()).Select(sur => sur.AdmID).FirstOrDefault();
                        IoMaster.AddlNotes = OTConsumptionDetails.OTNotes;
                        IoMaster.CreatedUTC = DateTime.UtcNow;
                        IoMaster.CreatedBy = OTConsumptionDetails.CreatedBy;
                        WYNKContext.IOMaster.Add(IoMaster);

                        WYNKContext.SaveChanges();

                        var SurgeryHistory = new SurgeryHistory();
                        SurgeryHistory.Cmpid = OTConsumptionDetails.Cmpid;
                        SurgeryHistory.SurgeryID = SurgeryID;
                        SurgeryHistory.UIN = OTConsumptionDetails.UIN;
                        SurgeryHistory.DateofSurgery = DateTime.UtcNow;
                        SurgeryHistory.TypeofSurgery = WYNKContext.ICDSpecialityCode.Where(s => s.ID == WYNKContext.SurgeryTran.Where(x => x.SurgeryID == SurgeryID).Select(x => x.IcdSpecialityCode).FirstOrDefault()).Select(s => s.SpecialityDescription).FirstOrDefault();
                        SurgeryHistory.Eye = WYNKContext.SurgeryTran.Where(x => x.SurgeryID == SurgeryID).Select(x => x.IsOD != false ? "OD" : x.IsOS != false ? "OS" : "OU").FirstOrDefault();
                        SurgeryHistory.SurgeryDone = "Internal";
                        SurgeryHistory.IsDeleted = false;
                        SurgeryHistory.CreatedUTC = DateTime.UtcNow;
                        SurgeryHistory.CreatedBy = OTConsumptionDetails.CreatedBy;
                        WYNKContext.SurgeryHistory.Add(SurgeryHistory);


                        WYNKContext.SaveChanges();



                        foreach (var IOnotes in OTConsumptionDetails.IntraOperativeNotes.ToList())
                        {
                            var Iotran = new IOTran();
                            Iotran.IOMID = IoMaster.ID;
                            Iotran.IOTemplateID = IOnotes.IOProcedureTempID == null ? IOnotes.IOProcedureTempID : Convert.ToInt32(IOnotes.IOProcedureTempID);
                            Iotran.IOTemplateTranID = IOnotes.IOTempTranID == null ? IOnotes.IOTempTranID : Convert.ToInt32(IOnotes.IOTempTranID);
                            if (IOnotes.UserInputType != "None")
                            {
                                Iotran.OTNotesDescription = IOnotes.OTNotesDescription + " " + IOnotes.GivenInputValue;
                            }
                            else
                            {
                                Iotran.OTNotesDescription = IOnotes.OTNotesDescription;
                            }
                            Iotran.CreatedUTC = DateTime.UtcNow;
                            Iotran.CreatedBy = OTConsumptionDetails.CreatedBy;
                            WYNKContext.IOTran.Add(Iotran);


                            object Iotra = Iotran;
                            oErrorLogs.WriteErrorLogArray("IOTran", Iotra);
                            WYNKContext.SaveChanges();
                        }
                        WYNKContext.SaveChanges();

                        var PatAcc = WYNKContext.PatientAccount.Where(x => x.UIN == OTConsumptionDetails.UIN && x.InvoiceNumber == null && x.InvoiceDate == null).FirstOrDefault();
                        var Olmid = CMPSContext.OneLineMaster.Where(x => x.ParentTag == "Services" && x.ParentDescription == "Surgery Cost").FirstOrDefault();

                        if (OTConsumptionDetails.BillingType == "Pkg")
                        {
                            var Amount = OTConsumptionDetails.SurgeryPackageCost;
                            if (PatAcc == null)
                            {
                                var PatientAccount = new PatientAccount();

                                PatientAccount.UIN = OTConsumptionDetails.UIN;
                                PatientAccount.TotalProductValue = Amount;
                                PatientAccount.TotalBillValue = Amount;
                                PatientAccount.Description = OTConsumptionDetails.LensDescription;
                                PatientAccount.CreatedUTC = DateTime.UtcNow;
                                PatientAccount.CreatedBy = OTConsumptionDetails.CreatedBy;
                                PatientAccount.CMPID = OTConsumptionDetails.Cmpid;
                                PatientAccount.TransactionID = OTConsumptionDetails.billtranid;
                                WYNKContext.PatientAccount.Add(PatientAccount);

                                object PatientAccoun = PatientAccount;
                                oErrorLogs.WriteErrorLogArray("PatientAccount", PatientAccoun);

                                WYNKContext.SaveChanges();

                                var PatientAccountDetail = new PatientAccountDetail();
                                PatientAccountDetail.PAID = PatientAccount.PAID;
                                PatientAccountDetail.OLMID = IcdSpecialityCode;
                                PatientAccountDetail.ServiceTypeID = OTConsumptionDetails.SurgeryServiceTypeID;
                                PatientAccountDetail.ServiceDescription = "Surgery Cost";
                                PatientAccountDetail.TotalProductValue = Amount;
                                PatientAccountDetail.TotalBillValue = Amount;
                                PatientAccountDetail.CreatedUTC = DateTime.UtcNow;
                                PatientAccountDetail.CreatedBy = OTConsumptionDetails.CreatedBy;
                                WYNKContext.PatientAccountDetail.Add(PatientAccountDetail);

                                object PatientAccountDetai = PatientAccountDetail;
                                oErrorLogs.WriteErrorLogArray("PatientAccountDetail", PatientAccountDetai);

                                WYNKContext.SaveChanges();
                            }
                            else
                            {
                                PatAcc.TotalProductValue = PatAcc.TotalProductValue != null ? PatAcc.TotalProductValue + OTConsumptionDetails.SurgeryPackageCost : 0 + OTConsumptionDetails.SurgeryPackageCost;
                                PatAcc.TotalBillValue = PatAcc.TotalBillValue != null ? PatAcc.TotalBillValue + OTConsumptionDetails.SurgeryPackageCost : 0 + OTConsumptionDetails.SurgeryPackageCost;
                                PatAcc.CreatedUTC = DateTime.UtcNow;
                                PatAcc.Description = OTConsumptionDetails.LensDescription;
                                PatAcc.UpdatedBy = OTConsumptionDetails.CreatedBy;
                                WYNKContext.PatientAccount.UpdateRange(PatAcc);

                                object PatAc = PatAcc;
                                oErrorLogs.WriteErrorLogArray("PatientAccount", PatAc);

                                WYNKContext.SaveChanges();

                                var PatientAccountDetail = new PatientAccountDetail();
                                PatientAccountDetail.PAID = PatAcc.PAID;
                                PatientAccountDetail.OLMID = IcdSpecialityCode;
                                PatientAccountDetail.ServiceTypeID = OTConsumptionDetails.SurgeryServiceTypeID;
                                PatientAccountDetail.ServiceDescription = "Surgery Cost";
                                PatientAccountDetail.TotalProductValue = Amount;
                                PatientAccountDetail.TotalBillValue = Amount;
                                PatientAccountDetail.CreatedUTC = DateTime.UtcNow;
                                PatientAccountDetail.CreatedBy = OTConsumptionDetails.CreatedBy;
                                WYNKContext.PatientAccountDetail.Add(PatientAccountDetail);

                                object PatientAccountDetai = PatientAccountDetail;
                                oErrorLogs.WriteErrorLogArray("PatientAccountDetail", PatientAccountDetai);

                                WYNKContext.SaveChanges();
                            }
                        }
                        else
                        {
                            if (PatAcc == null)
                            {
                                var PatientAccount = new PatientAccount();

                                PatientAccount.UIN = OTConsumptionDetails.UIN;
                                PatientAccount.TotalProductValue = OTConsumptionDetails.SurgeryPackageCost;
                                PatientAccount.TotalBillValue = OTConsumptionDetails.SurgeryPackageCost;
                                PatientAccount.Description = OTConsumptionDetails.LensDescription;
                                PatientAccount.CreatedUTC = DateTime.UtcNow;
                                PatientAccount.CreatedBy = OTConsumptionDetails.CreatedBy;
                                PatientAccount.CMPID = OTConsumptionDetails.Cmpid;
                                PatientAccount.TransactionID = OTConsumptionDetails.billtranid;
                                WYNKContext.PatientAccount.Add(PatientAccount);
                                WYNKContext.SaveChanges();

                                var PatientAccountDetail = new PatientAccountDetail();
                                PatientAccountDetail.PAID = PatientAccount.PAID;
                                PatientAccountDetail.OLMID = IcdSpecialityCode;
                                PatientAccountDetail.ServiceTypeID = OTConsumptionDetails.SurgeryServiceTypeID;
                                PatientAccountDetail.ServiceDescription = "Surgery Cost";
                                PatientAccountDetail.TotalProductValue = OTConsumptionDetails.SurgeryPackageCost;
                                PatientAccountDetail.TotalBillValue = OTConsumptionDetails.SurgeryPackageCost;
                                PatientAccountDetail.CreatedUTC = DateTime.UtcNow;
                                PatientAccountDetail.CreatedBy = OTConsumptionDetails.CreatedBy;
                                WYNKContext.PatientAccountDetail.Add(PatientAccountDetail);
                                WYNKContext.SaveChanges();

                                var PatAccDetailTax = new PatientAccountDetailTax();
                                PatAccDetailTax.PAccDetailID = PatientAccountDetail.PAccDetailID;
                                PatAccDetailTax.ServiceTypeID = OTConsumptionDetails.SurgeryServiceTypeID;
                                PatAccDetailTax.TotalValue = OTConsumptionDetails.SurgeryPackageCost;
                                PatAccDetailTax.CreatedUTC = DateTime.UtcNow;
                                PatAccDetailTax.CreatedBy = OTConsumptionDetails.CreatedBy;
                                WYNKContext.PatientAccountDetailTax.Add(PatAccDetailTax);
                                WYNKContext.SaveChanges();


                                var PatAccDetailTax1 = new PatientAccountDetailTax();
                                PatAccDetailTax1.PAccDetailID = PatientAccountDetail.PAccDetailID;
                                PatAccDetailTax1.ServiceTypeID = OTConsumptionDetails.OtServiceID;
                                PatAccDetailTax1.TotalValue = OTConsumptionDetails.OTtheatreCost;
                                PatAccDetailTax1.CreatedUTC = DateTime.UtcNow;
                                PatAccDetailTax1.CreatedBy = OTConsumptionDetails.CreatedBy;
                                WYNKContext.PatientAccountDetailTax.Add(PatAccDetailTax1);

                                PatientAccountDetail.TotalBillValue = PatientAccountDetail.TotalBillValue + OTConsumptionDetails.OTtheatreCost;
                                WYNKContext.PatientAccountDetail.UpdateRange(PatientAccountDetail);

                                PatientAccount.TotalBillValue = PatientAccount.TotalBillValue + OTConsumptionDetails.OTtheatreCost;
                                WYNKContext.PatientAccount.UpdateRange(PatientAccount);

                                WYNKContext.SaveChanges();
                            }
                            else
                            {
                                PatAcc.TotalProductValue = PatAcc.TotalProductValue != null ? PatAcc.TotalProductValue + OTConsumptionDetails.SurgeryPackageCost : 0 + OTConsumptionDetails.SurgeryPackageCost;
                                PatAcc.TotalBillValue = PatAcc.TotalBillValue != null ? PatAcc.TotalBillValue + OTConsumptionDetails.SurgeryPackageCost : 0 + OTConsumptionDetails.SurgeryPackageCost;
                                PatAcc.CreatedUTC = DateTime.UtcNow;
                                PatAcc.Description = OTConsumptionDetails.LensDescription;
                                PatAcc.UpdatedBy = OTConsumptionDetails.CreatedBy;
                                WYNKContext.PatientAccount.UpdateRange(PatAcc);
                                WYNKContext.SaveChanges();

                                var PatientAccountDetail = new PatientAccountDetail();
                                PatientAccountDetail.PAID = PatAcc.PAID;
                                PatientAccountDetail.OLMID = IcdSpecialityCode;
                                PatientAccountDetail.ServiceTypeID = OTConsumptionDetails.SurgeryServiceTypeID;
                                PatientAccountDetail.ServiceDescription = "Surgery Cost";
                                PatientAccountDetail.TotalProductValue = OTConsumptionDetails.SurgeryPackageCost;
                                PatientAccountDetail.TotalBillValue = OTConsumptionDetails.SurgeryPackageCost;
                                PatientAccountDetail.CreatedUTC = DateTime.UtcNow;
                                PatientAccountDetail.CreatedBy = OTConsumptionDetails.CreatedBy;
                                WYNKContext.PatientAccountDetail.Add(PatientAccountDetail);
                                WYNKContext.SaveChanges();

                                var PatAccDetailTax = new PatientAccountDetailTax();
                                PatAccDetailTax.PAccDetailID = PatientAccountDetail.PAccDetailID;
                                PatAccDetailTax.ServiceTypeID = OTConsumptionDetails.SurgeryServiceTypeID;
                                PatAccDetailTax.TotalValue = OTConsumptionDetails.SurgeryPackageCost;
                                PatAccDetailTax.CreatedUTC = DateTime.UtcNow;
                                PatAccDetailTax.CreatedBy = OTConsumptionDetails.CreatedBy;
                                WYNKContext.PatientAccountDetailTax.Add(PatAccDetailTax);
                                WYNKContext.SaveChanges();


                                var PatAccDetailTax1 = new PatientAccountDetailTax();
                                PatAccDetailTax1.PAccDetailID = PatientAccountDetail.PAccDetailID;
                                PatAccDetailTax1.ServiceTypeID = OTConsumptionDetails.OtServiceID;
                                PatAccDetailTax1.TotalValue = OTConsumptionDetails.OTtheatreCost;
                                PatAccDetailTax1.CreatedUTC = DateTime.UtcNow;
                                PatAccDetailTax1.CreatedBy = OTConsumptionDetails.CreatedBy;
                                WYNKContext.PatientAccountDetailTax.Add(PatAccDetailTax1);

                                PatientAccountDetail.TotalBillValue = PatientAccountDetail.TotalBillValue + OTConsumptionDetails.OTtheatreCost;
                                WYNKContext.PatientAccountDetail.UpdateRange(PatientAccountDetail);

                                PatAcc.TotalBillValue = PatAcc.TotalBillValue + OTConsumptionDetails.OTtheatreCost;
                                WYNKContext.PatientAccount.UpdateRange(PatAcc);

                                WYNKContext.SaveChanges();
                            }
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
                            SurgeryID = SurgeryID,
                        };

                    }

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    if (ex.InnerException != null)
                    {
                        ErrorLog oErrorLog = new ErrorLog();
                        oErrorLog.WriteErrorLog("Error", ex.InnerException.Message.ToString());
                        string msg = ex.InnerException.Message;
                        return new { Success = false, Message = msg };
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

        public IList<SerialDetail> CheckSerials(ICollection<string> selectedList, int drugid, int storeID, int cmpid)
        {
            IList<SerialDetail> InSufficientSerials = new List<SerialDetail>();

            foreach (var item in selectedList.ToList())
            {
                var itemSerial = WYNKContext.ItemSerial.Where(x => x.ItemID == drugid && x.StoreID == storeID && x.SerialNo == Convert.ToString(item) && x.IssueNo == null && x.IssueDate == null && x.IssueTC == null && x.cmpID == cmpid).FirstOrDefault();

                if (itemSerial == null)
                {
                    InSufficientSerials.Add(new SerialDetail()
                    {
                        Brand = WYNKContext.DrugMaster.Where(x => x.ID == drugid).Select(x => x.Brand).FirstOrDefault(),
                        SerialNo = Convert.ToString(item),
                    });
                }

            }

            return InSufficientSerials;
        }

        public dynamic BeforeSkinIncisionDetails(int CMPID)
        {

            var OnelineMaster = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Before skin incision" && x.ParentTag == "SSCType" && x.IsActive == true).Select(x => x.OLMID).FirstOrDefault();

            var BeforeIncisionQuestions = (from e in WYNKContext.SurgerySafetyCheckList.Where(x => x.SSCGroupDescription == OnelineMaster && x.IsActive == true)
                                           select new
                                           {
                                               OLMID = OnelineMaster,
                                               Sscid = e.SSCID,
                                               Questions = e.Question,
                                               ToWhom = e.Questiontowhom,
                                               Description = "",
                                               DisabledYes = e.DefaultValue != null ? Enum.GetName(typeof(Answers), e.DefaultValue) == "Yes" ? false : true : false,
                                               DisabledNo = e.DefaultValue != null ? Enum.GetName(typeof(Answers), e.DefaultValue) == "No" ? false : true : false,
                                               DisabledNA = e.DefaultValue != null ? Enum.GetName(typeof(Answers), e.DefaultValue) == "NA" ? false : true : false,
                                               Yes = e.DefaultValue != null ? Enum.GetName(typeof(Answers), e.DefaultValue) == "Yes" ? true : false : false,
                                               No = e.DefaultValue != null ? Enum.GetName(typeof(Answers), e.DefaultValue) == "No" ? true : false : false,
                                               NA = e.DefaultValue != null ? Enum.GetName(typeof(Answers), e.DefaultValue) == "NA" ? true : false : false,
                                           }).ToList();


            return BeforeIncisionQuestions;


        }

        public dynamic BeforePatientLeavesOperating(int CMPID)
        {
            var OnelineMaster = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Before patient leaves operating room" && x.ParentTag == "SSCType" && x.IsActive == true).Select(x => x.OLMID).FirstOrDefault();

            var BeforePatientLeavesOperating = (from e in WYNKContext.SurgerySafetyCheckList.Where(x => x.SSCGroupDescription == OnelineMaster && x.IsActive == true && x.CmpID == CMPID)
                                                select new
                                                {
                                                    OLMID = OnelineMaster,
                                                    Questions = e.Question,
                                                    Sscid = e.SSCID,
                                                    ToWhom = e.Questiontowhom,
                                                    Description = "",
                                                    DisabledYes = e.DefaultValue != null ? Enum.GetName(typeof(Answers), e.DefaultValue) == "Yes" ? false : true : false,
                                                    DisabledNo = e.DefaultValue != null ? Enum.GetName(typeof(Answers), e.DefaultValue) == "No" ? false : true : false,
                                                    DisabledNA = e.DefaultValue != null ? Enum.GetName(typeof(Answers), e.DefaultValue) == "NA" ? false : true : false,
                                                    Yes = e.DefaultValue != null ? Enum.GetName(typeof(Answers), e.DefaultValue) == "Yes" ? true : false : false,
                                                    No = e.DefaultValue != null ? Enum.GetName(typeof(Answers), e.DefaultValue) == "No" ? true : false : false,
                                                    NA = e.DefaultValue != null ? Enum.GetName(typeof(Answers), e.DefaultValue) == "NA" ? true : false : false,
                                                }).ToList();


            return BeforePatientLeavesOperating;
        }

        public dynamic OtNoteslist(OTConsumptionDetails otSurgeryDesc, int value)
        {
            var ResData = (from IO in WYNKContext.IOProcedureTemplate.Where(x => x.IsActive == true && x.ICDSpecialityCode == value && x.SurgeryDescription == otSurgeryDesc.OTSurgeryDescription)
                           group IO by IO.SurgeryDescription into IOgroup
                           select new
                           {
                               SurgeryDescObj = (from IOt in WYNKContext.IOProcedureTemplate.Where(x => x.IsActive == true && x.ICDSpecialityCode == value)
                                                 join IOtrans in WYNKContext.IOTemplateTran on IOt.ID equals IOtrans.IOTemplateID
                                                 where (IOgroup.Select(x => x.ID)).Contains(IOtrans.IOTemplateID)
                                                 select new
                                                 {
                                                     IOProcedureTempID = IOt.ID,
                                                     IOTempTranID = IOtrans.ID,
                                                     OTNotesDescription = IOtrans.OTNotesDescription,
                                                     UserInputType = IOtrans.UserInputType,
                                                     InputValue = IOtrans.UserInputType == "User selection" ? SeparationOtNotes(IOtrans.InputValue) : null,
                                                 }).ToList()

                           }).ToList();

            return ResData;
        }

        public IList<string> SeparationOtNotes(string Value)
        {
            return Value.Split(',').ToList<string>();

        }

        public bool uploadImagslod(IFormFile file1, string SurgeryID, int ImageCount)
        {
            var fnd = new Findings();
            fnd.SlitLampImage = new List<SlitLampImage>();
            try
            {
                var Foldername = "Intra-operative Images";
                var currentDir = Directory.GetCurrentDirectory();
                var dt = Convert.ToDateTime(DateTime.Now.Date).ToString("dd-MM-yyyy");

                var res = Directory.CreateDirectory(currentDir + '/' + Foldername + '/' + dt + '/' + SurgeryID);
                var fileName1 = $"{SurgeryID}({ImageCount}){Path.GetExtension(file1.FileName)}";
                var path1 = $"{currentDir}/{Foldername}/{dt}/{SurgeryID}/{fileName1}";

                var pathh = $"{currentDir}/{Foldername}/{dt}/{SurgeryID}";

                using (var stream1 = new FileStream(path1, FileMode.Create))
                {
                    file1.CopyTo(stream1);
                    var opbio = WYNKContext.OtImagepath.Where(x => x.SurgeryID == SurgeryID).ToList();
                    if (opbio.Count() > 0)
                    {
                        foreach (var item1 in opbio.ToList())
                        {
                            item1.Imagepath = pathh;
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

        public dynamic GetconsentDetails(int cmpid)
        {

            string[] lines;
            var list = new List<string>();
            var osfi = "/ConcernPages/";
            var osfn = "Intra_operative.text";
            var currentDir = Directory.GetCurrentDirectory();
            string path = currentDir + osfi + cmpid + '/' + osfn;


            if (File.Exists(path))
            {
                var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        list.Add(line);
                    }
                }
                lines = list.ToArray();
                return new
                {
                    Success = true,
                    TOtalLines = lines,
                };
            }
            else
            {
                return new
                {
                    Success = false,
                };
            }

        }

        public dynamic UnbilledList(int cmpid, string GMTTIME)
        {
            var icdmaster = WYNKContext.ICDSpecialityCode.ToList();
            var Reg = WYNKContext.Registration.ToList();
            var SurgeryAssigned = WYNKContext.SurgeryAssigned.ToList();

            TimeSpan ts = TimeSpan.Parse(GMTTIME);

            return (from e in SurgeryAssigned.Where(x => x.CmpID == cmpid && x.IsCancelled == false && x.IsSurgeryCompleted == true)
                    select new
                    {
                        ID = e.RandomUniqueID,
                        UIN = e.UIN,
                        SurgeryDate = WYNKContext.Surgery.Where(x => x.RandomUniqueID == e.SurgeryID).Select(x => x.DateofSurgery).FirstOrDefault() + ts,
                        SurgeryID = e.SurgeryID,
                        Surgery = string.Join(",", WYNKContext.SurgeryTran
                                                .Where(surgTran => surgTran.SurgeryID == e.SurgeryID)
                                                .Select(surgTran => icdmaster
                                                    .Where(icd => icd.ID == surgTran.IcdSpecialityCode)
                                                    .Select(icd => icd.SpecialityDescription).FirstOrDefault())),
                        Name = Reg.Where(x => x.UIN == e.UIN).Select(x => String.Concat(x.Name + " " + (x.MiddleName != null ? x.MiddleName : " ") + " " + x.LastName)).FirstOrDefault(),
                        Age = ToAgeString(Reg.Where(x => x.UIN == e.UIN).Select(x => x.DateofBirth).FirstOrDefault()),
                        Gender = Reg.Where(x => x.UIN == e.UIN).Select(x => x.Gender).FirstOrDefault(),
                        SurgeryProcedure = (from IO in WYNKContext.IOProcedureTemplate.Where(x => x.IsActive == true && x.ICDSpecialityCode == WYNKContext.SurgeryTran.Where(y => y.SurgeryID == e.SurgeryID).Select(y => y.IcdSpecialityCode).FirstOrDefault())
                                            group IO by IO.SurgeryDescription into IOgroup
                                            select new
                                            {
                                                Text = IOgroup.Key,
                                                Value = IOgroup.Select(y => y.ICDSpecialityCode).FirstOrDefault(),
                                            }).ToList(),
                        OperationTheatreName = WYNKContext.OperationTheatre.Where(ot => ot.OTID == WYNKContext.Surgery.Where(x => x.RandomUniqueID == e.SurgeryID).Select(x => x.OTID).FirstOrDefault()).Select(ot => ot.OTDescription).FirstOrDefault(),
                        FromTime = WYNKContext.SurgeryAssigned.Where(x => x.SAID == e.SAID).Select(x => x.FromTime).FirstOrDefault(),
                        ToTime = WYNKContext.SurgeryAssigned.Where(x => x.SAID == e.SAID).Select(x => x.ToTime).FirstOrDefault(),
                        FindExt = e.FindingsExtID != null ? GetFindingExt(e.FindingsExtID) : null,
                    }).ToList();
        }

        public dynamic IoDrugDetails(string SAID, int cmpid)
        {
            try
            {
                var UOM = CMPSContext.uommaster.ToList();
                var DrugMaster = WYNKContext.DrugMaster.ToList();
                var DrugGroup = WYNKContext.DrugGroup.ToList();
                var OnelineMaster = CMPSContext.OneLineMaster.ToList();
                var AssignedDoctorIDs = WYNKContext.SurgeryAssignedTran.Where(x => x.SAID == SAID).Select(x => x.DoctorID).ToList();
                var Surgery = WYNKContext.Surgery.Where(Sur => Sur.RandomUniqueID == WYNKContext.SurgeryAssignedTran.Where(x => x.SAID == SAID).Select(x => x.SurgeryID).FirstOrDefault() && Sur.CMPID == cmpid).FirstOrDefault();
                var Doctordetails = (from e in CMPSContext.DoctorMaster
                                     join ds in CMPSContext.DoctorSpeciality on e.DoctorID equals ds.DoctorID
                                     where AssignedDoctorIDs.Contains(e.DoctorID)
                                     select new
                                     {
                                         ID = e.DoctorID,
                                         Name = String.Concat(e.LastName, ' ', e.MiddleName, ' ', e.Firstname),
                                         Specialization = CMPSContext.OneLineMaster.Where(x => x.OLMID == ds.OLMID).Select(x => x.ParentDescription).FirstOrDefault()
                                     }).ToList();
                var DrugUsedDetail = (from Mst in WYNKContext.StockTran.Where(x => x.SMID == Surgery.SMID)
                                      select new
                                      {
                                          Brand = DrugMaster.Where(x => x.ID == Mst.ItemID).Select(x => x.Brand).FirstOrDefault(),
                                          GenericName = DrugGroup.Where(x => x.ID == DrugMaster.Where(d => d.ID == Mst.ItemID).Select(d => d.GenericName).FirstOrDefault()).Select(x => x.Description).FirstOrDefault(),
                                          Quantity = Mst.ItemQty,
                                          UOM = UOM.Where(x => x.id == Mst.UOMID).Select(x => x.Description).FirstOrDefault(),
                                      }).ToList();
                var Store = WYNKContext.StockMaster.Where(x => x.RandomUniqueID == Surgery.SMID && x.CMPID == cmpid).Select(x => x.StoreID).FirstOrDefault();
                var AddOtNotes = WYNKContext.IOMaster.Where(x => x.Surgeryid == Surgery.RandomUniqueID && x.CMPID == cmpid).Select(x => x.AddlNotes).FirstOrDefault();
                var BeforeSkinIncisionID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Before skin incision" && x.ParentTag == "SSCType").Select(x => x.OLMID).FirstOrDefault();
                var BeforePatientLeavesID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Before patient leaves operating room" && x.ParentTag == "SSCType").Select(x => x.OLMID).FirstOrDefault();
                var BeforeSkinList = (from SSC in WYNKContext.SscResponse.Where(x => x.SAID == SAID && x.OLMID == BeforeSkinIncisionID && x.CmpID == cmpid)
                                      select new
                                      {
                                          Questions = WYNKContext.SurgerySafetyCheckList.Where(x => x.SSCID == SSC.SSCID && x.CmpID == cmpid).Select(x => x.Question).FirstOrDefault(),
                                          ToWhom = WYNKContext.SurgerySafetyCheckList.Where(x => x.SSCID == SSC.SSCID && x.CmpID == cmpid).Select(x => x.Questiontowhom).FirstOrDefault(),
                                          Description = SSC.Description,
                                          DisabledYes = true,
                                          DisabledNo = true,
                                          DisabledNA = true,
                                          Yes = Enum.GetName(typeof(Answers), SSC.Response) == "Yes" ? true : false,
                                          No = Enum.GetName(typeof(Answers), SSC.Response) == "No" ? true : false,
                                          NA = Enum.GetName(typeof(Answers), SSC.Response) == "NA" ? true : false,
                                      }).ToList();
                var BeforePatientLeavesList = (from SSC in WYNKContext.SscResponse.Where(x => x.SAID == SAID && x.OLMID == BeforePatientLeavesID && x.CmpID == cmpid)
                                               select new
                                               {
                                                   Questions = WYNKContext.SurgerySafetyCheckList.Where(x => x.SSCID == SSC.SSCID && x.CmpID == cmpid).Select(x => x.Question).FirstOrDefault(),
                                                   ToWhom = WYNKContext.SurgerySafetyCheckList.Where(x => x.SSCID == SSC.SSCID && x.CmpID == cmpid).Select(x => x.Questiontowhom).FirstOrDefault(),
                                                   Description = SSC.Description,
                                                   DisabledYes = true,
                                                   DisabledNo = true,
                                                   DisabledNA = true,
                                                   Yes = Enum.GetName(typeof(Answers), SSC.Response) == "Yes" ? true : false,
                                                   No = Enum.GetName(typeof(Answers), SSC.Response) == "No" ? true : false,
                                                   NA = Enum.GetName(typeof(Answers), SSC.Response) == "NA" ? true : false,
                                               }).ToList();
                var Monitoring = WYNKContext.Monitoring.Where(x => x.SAID == SAID).FirstOrDefault();
                var IOMasterId = WYNKContext.IOMaster.Where(x => x.Surgeryid == Surgery.RandomUniqueID).Select(x => x.ID).FirstOrDefault();
                var SurProcedureDesc = WYNKContext.IOProcedureTemplate.Where(Io => Io.ID == WYNKContext.IOTran.Where(x => x.IOMID == IOMasterId).Select(x => x.IOTemplateID).FirstOrDefault()).Select(Io => Io.SurgeryDescription).FirstOrDefault();
                var IotranList = (from IoTran in WYNKContext.IOTran.Where(x => x.IOMID == IOMasterId)
                                  select new
                                  {
                                      IOProcedureTempID = IoTran.IOTemplateID,
                                      IOTempTranID = IoTran.IOTemplateTranID,
                                      OTNotesDescription = IoTran.OTNotesDescription,
                                  }).ToList();
                var OtImageDirectory = WYNKContext.OtImagepath.Where(x => x.SurgeryID == Surgery.RandomUniqueID).Select(x => x.Imagepath).FirstOrDefault();
                var ImageFiles = new List<string>();

                if (OtImageDirectory != null)
                {
                    var ImageInfo = Directory.GetFiles(OtImageDirectory);

                    if (ImageInfo.Length > 0)
                    {
                        foreach (var item in ImageInfo.ToList())
                        {
                            if (File.Exists(item))
                            {
                                string imageData = Convert.ToBase64String(File.ReadAllBytes(item));
                                ImageFiles.Add("data:image/png;base64," + imageData);
                            }
                        }
                    }
                }
                var Discharged = WYNKContext.Admission.Where(x => x.RandomUniqueID == Surgery.AdmID).Select(x => x.DischargeID).FirstOrDefault();
                bool IsDischarge = Discharged != null ? true : false;

                return new
                {
                    Success = true,
                    Doctordetails = Doctordetails,
                    DrugUsedDetail = DrugUsedDetail,
                    Store = Store.ToString(),
                    AddOtNotes = AddOtNotes,
                    BeforeSkinList = BeforeSkinList,
                    BeforePatientLeavesList = BeforePatientLeavesList,
                    Monitoring = Monitoring,
                    SurgeryProcedure = SurProcedureDesc,
                    IotranList = IotranList,
                    SurgeryDetails = Surgery,
                    ImageFiles = ImageFiles,
                    IsDischarge = IsDischarge,
                };
            }
            catch (Exception ex) { }
            return new
            {
                Success = false,
                Message = "SomeThing Went Wrong"
            };
        }

        public dynamic UpdateOT(UpdateOTConsumptionDetails UpdateOTConsumptionDetails)
        {
            try
            {
                var IoMaster = WYNKContext.IOMaster.Where(x => x.ADMID == UpdateOTConsumptionDetails.AdmID && x.Surgeryid == UpdateOTConsumptionDetails.SurgeryId && x.CMPID == UpdateOTConsumptionDetails.Cmpid).FirstOrDefault();

                WYNKContext.IOTran.RemoveRange(WYNKContext.IOTran.Where(x => x.IOMID == IoMaster.ID).ToList());



                string cmpname = CMPSContext.Company.Where(x => x.CmpID == UpdateOTConsumptionDetails.Cmpid).Select(x => x.CompanyName).FirstOrDefault();
                string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == UpdateOTConsumptionDetails.CreatedBy).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                string userid = Convert.ToString(UpdateOTConsumptionDetails.CreatedBy);
                ErrorLog oErrorLogs = new ErrorLog();
                oErrorLogs.WriteErrorLogTitle(cmpname, "Intra Operative", "User name :", username, "User ID :", userid, "Mode : Edit");



                foreach (var IOnotes in UpdateOTConsumptionDetails.IntraOperativeNotes.ToList())
                {
                    var Iotran = new IOTran();
                    Iotran.IOMID = IoMaster.ID;
                    Iotran.IOTemplateID = IOnotes.IOProcedureTempID == null ? IOnotes.IOProcedureTempID : Convert.ToInt32(IOnotes.IOProcedureTempID);
                    Iotran.IOTemplateTranID = IOnotes.IOTempTranID == null ? IOnotes.IOTempTranID : Convert.ToInt32(IOnotes.IOTempTranID);
                    if (IOnotes.UserInputType != "None")
                    {
                        Iotran.OTNotesDescription = IOnotes.OTNotesDescription + " " + IOnotes.GivenInputValue;
                    }
                    else
                    {
                        Iotran.OTNotesDescription = IOnotes.OTNotesDescription;
                    }
                    Iotran.CreatedUTC = DateTime.UtcNow;
                    Iotran.CreatedBy = UpdateOTConsumptionDetails.CreatedBy;
                    WYNKContext.IOTran.Add(Iotran);
                    object names = Iotran;
                    oErrorLogs.WriteErrorLogArray("IOTran", names);
                }
                WYNKContext.SaveChanges();
                return new
                {
                    Success = true,
                    Message = "Saved successfully",
                };
            }
            catch (Exception ex)
            {
                ErrorLog oErrorLog = new ErrorLog();
                oErrorLog.WriteErrorLog("Error :", ex.InnerException.Message.ToString());
            }
            return new
            {
                Success = false,
                Message = "SomeThing Went Wrong"
            };
        }

        public dynamic GetSurgeryPackageCost(string BillingType, string AdmRandomId, int CmpID)
        {
            try
            {
                var Adm = WYNKContext.Admission.Where(x => x.RandomUniqueID == AdmRandomId && x.CMPID == CmpID).FirstOrDefault();
                var RoomOccupiedID = WYNKContext.RoomOccupiedStatus.Where(x => x.AdmID == AdmRandomId && x.CmpID == CmpID).Select(x => x.RoomID).FirstOrDefault();
                var RoomTypeId = WYNKContext.Room.Where(x => x.ID == RoomOccupiedID).Select(x => x.RoomType).FirstOrDefault();
                var Surgery = WYNKContext.Surgery.Where(x => x.AdmID == AdmRandomId).FirstOrDefault();
                var SurgeryTran = WYNKContext.SurgeryTran.Where(x => x.SurgeryID == Surgery.RandomUniqueID).FirstOrDefault();
                decimal? PackageCost = null;
                int? ServiceTypeID = null;
                if (BillingType == "Pkg")
                {
                    ServiceTypeID = Convert.ToInt32(WYNKContext.ServiceMaster.Where(x => x.Tag == "Pkg" && x.Icdcode == SurgeryTran.ICDCode && x.IcdSpecialityCode == SurgeryTran.IcdSpecialityCode && x.RoomID == Convert.ToInt32(RoomTypeId) && x.CMPID == CmpID).Select(x => x.ChildDescription).FirstOrDefault());
                    PackageCost = WYNKContext.ServiceMaster.Where(x => x.Tag == "Pkg" && x.Icdcode == SurgeryTran.ICDCode && x.IcdSpecialityCode == SurgeryTran.IcdSpecialityCode  && x.RoomID == Convert.ToInt32(RoomTypeId) && x.CMPID == CmpID).Select(x => x.TotalAmount).FirstOrDefault();
                }
                else
                {
                    ServiceTypeID =Convert.ToInt32(WYNKContext.ServiceMaster.Where(x => x.Tag == "IPkg" && x.Icdcode == SurgeryTran.ICDCode && x.IcdSpecialityCode == SurgeryTran.IcdSpecialityCode && x.RoomID == Convert.ToInt32(RoomTypeId) && x.CMPID == CmpID).Select(x => x.ChildDescription).FirstOrDefault());
                    PackageCost = WYNKContext.ServiceMaster.Where(x => x.Tag == "IPkg" && x.Icdcode == SurgeryTran.ICDCode && x.IcdSpecialityCode == SurgeryTran.IcdSpecialityCode && x.RoomID == Convert.ToInt32(RoomTypeId) && x.CMPID == CmpID).Select(x => x.TotalAmount).FirstOrDefault();
                }
                return new
                {
                    Success = true,
                    PackageCost = PackageCost,
                    SurgeryServiceTypeID = ServiceTypeID == null ? 0 : ServiceTypeID,
                };

            }
            catch (Exception ex)
            {
                return new
                {
                    Success = false,
                };
            }
        }
    }
}













