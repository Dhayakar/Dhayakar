using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using System.Threading.Tasks;
using WYNK.Data.Common;
using WYNK.Data.Repository.Operation;
using WYNK.Helpers;
using System.Globalization;

namespace WYNK.Data.Repository.Implementation
{
    class BillingMasterrepository : RepositoryBase<BillingPharmacy>, IBillingMasterRepository
    {

        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        public BillingMasterrepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }
        public dynamic CurrentDateSearch(string SelectedDate, int CmpID, string Status, string GMT)
        {
            var currentmedicalprescription = new BillingPharmacy();
            currentmedicalprescription.paymenttran = new List<Payment_Master>();

            var reg = WYNKContext.Registration.Where(x => x.CMPID == CmpID).ToList();
            var medicalpres = WYNKContext.MedicalPrescription.Where(x => x.CmpID == CmpID).ToList();

            TimeSpan ts = TimeSpan.Parse(GMT);

            List<MedicalPrescription> res;


            DateTime DT;
            var appdate = DateTime.TryParseExact(SelectedDate.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DT);
            {
                DT.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            }
            var fdate = DT;


            if (Status == "Open")
            {
                res = medicalpres.Where(x => (x.PrescribedDate + ts).Date == fdate && x.CmpID == CmpID && x.Status != "Closed").ToList();
            }
            else
            {
                res = medicalpres.Where(x => (x.PrescribedDate + ts).Date == fdate && x.CmpID == CmpID && x.Status == Status).ToList();
            }


            currentmedicalprescription.DateMedicalPrescription = (from med in res
                                                                  select new DateMedicalPrescription
                                                                  {

                                                                      MedicalPrescriptionID = med.RandomUniqueID,
                                                                      UIN = med.UIN,
                                                                      RegistrationTranID = med.RegistrationTranID,
                                                                      MedicalPrescriptionNo = med.MedicalPrescriptionNo,
                                                                      PatientName = reg.Where(x => x.UIN == med.UIN).Select(x => String.Concat(x.Name + " " + (x.MiddleName != null ? x.MiddleName : " ") + " " + x.LastName)).FirstOrDefault(),
                                                                      PrescribedDoctor = med.PrescribedByName,
                                                                      Status = med.Status,
                                                                      PrescribedDate = med.PrescribedDate + ts,
                                                                      Gender = reg.Where(x => x.UIN == med.UIN).Select(x => x.Gender).FirstOrDefault(),
                                                                      Age = PasswordEncodeandDecode.ToAgeString(reg.Where(x => x.CMPID == CmpID && x.UIN == med.UIN).Select(x => x.DateofBirth).FirstOrDefault()),
                                                                  }).ToList();

            return currentmedicalprescription;

        }

        public dynamic PeriodSearch(string FromDate, string Todate, int CmpID, string Status, string GMT)
        {
            var currentmedicalprescription = new BillingPharmacy();
            currentmedicalprescription.paymenttran = new List<Payment_Master>();

            var reg = WYNKContext.Registration.Where(x => x.CMPID == CmpID).ToList();
            var medicalpres = WYNKContext.MedicalPrescription.Where(x => x.CmpID == CmpID).ToList();

            TimeSpan ts = TimeSpan.Parse(GMT);

            List<MedicalPrescription> res;

            DateTime DT;
            var appdate = DateTime.TryParseExact(FromDate.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DT);
            {
                DT.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            }
            var fdate = DT;

            DateTime DT1;
            var appdate1 = DateTime.TryParseExact(Todate.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DT1);
            {
                DT1.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            }
            var tdate = DT1;


            if (Status == "Open")
            {
                res = medicalpres.Where(x => (x.PrescribedDate + ts).Date >= fdate && (x.PrescribedDate + ts).Date <= tdate && x.CmpID == CmpID && x.Status != "Closed").ToList();
            }
            else
            {
                res = medicalpres.Where(x => (x.PrescribedDate + ts).Date >= fdate && (x.PrescribedDate + ts).Date <= tdate && x.CmpID == CmpID && x.Status == Status).ToList();
            }

            currentmedicalprescription.DateMedicalPrescription = (from med in res
                                                                  select new DateMedicalPrescription
                                                                  {

                                                                      MedicalPrescriptionID = med.RandomUniqueID,
                                                                      UIN = med.UIN,
                                                                      RegistrationTranID = med.RegistrationTranID,
                                                                      MedicalPrescriptionNo = med.MedicalPrescriptionNo,
                                                                      PatientName = reg.Where(x => x.UIN == med.UIN).Select(x => String.Concat(x.Name + " " + (x.MiddleName != null ? x.MiddleName : " ") + " " + x.LastName)).FirstOrDefault(),
                                                                      PrescribedDoctor = med.PrescribedByName,
                                                                      Status = med.Status,
                                                                      PrescribedDate = med.PrescribedDate + ts,
                                                                      Gender = reg.Where(x => x.UIN == med.UIN).Select(x => x.Gender).FirstOrDefault(),
                                                                      Age = PasswordEncodeandDecode.ToAgeString(reg.Where(x => x.CMPID == CmpID && x.UIN == med.UIN).Select(x => x.DateofBirth).FirstOrDefault()),
                                                                  }).ToList();



            return currentmedicalprescription;
        }

        public dynamic GetMedicalPrescriptionIddetails(string MedicalPrescriptionId, int StoreID, int CMPID)
        {
            var MedicalPrescription = new BillingPharmacy();

            var drugmaster = WYNKContext.DrugMaster.ToList();

            var medicalprestran = WYNKContext.MedicalPrescriptionTran.ToList();

            var taxMaster = CMPSContext.TaxMaster.ToList();

            MedicalPrescription.paymenttran = new List<Payment_Master>();

            var DrugDetails = (from med in medicalprestran.Where(x => x.MedicalPrescriptionID == MedicalPrescriptionId)
                               group med by med.DrugId into medgroup
                               select new
                               {
                                   DrugID = medgroup.Key,
                               }).ToList();

            List<MedicalPrescriptionIddetails> PrescribedMedicinelist = new List<MedicalPrescriptionIddetails>();

            foreach (var Drug in DrugDetails)
            {
                var DrugTrackerId = WYNKContext.DrugMaster.Where(x => x.ID == Drug.DrugID).Select(x => x.DrugTracker).FirstOrDefault();
                var res = Enum.GetName(typeof(TrackingType), DrugTrackerId);

                if (res == "SerialNumberBased")
                {
                    var Date = DateTime.Now;
                    var FinancialYearId = WYNKContext.FinancialYear.Where(x => x.FYFrom.Date <= Date.Date && x.FYTo.Date >= Date.Date && x.CMPID == CMPID && x.IsActive == true).Select(x => x.ID).FirstOrDefault();
                    var CheckingItemBalance = WYNKContext.ItemBalance.Where(x => x.FYear == FinancialYearId && x.ItemID == Drug.DrugID && x.StoreID == StoreID).Select(x => x.ClosingBalance).FirstOrDefault();

                    var ItemSerialList = (from ItemSerial in WYNKContext.ItemSerial.Where(x => x.ItemID == Drug.DrugID && x.IssueNo == null && x.StoreID == StoreID)
                                          select new SerialInfo
                                          {
                                              SerialNo = ItemSerial.SerialNo,
                                              ExpiryDate = ItemSerial.ExpiryDate,
                                              BillNo = ItemSerial.GRNNo,
                                          }).ToList();

                    PrescribedMedicinelist.AddRange((from med in medicalprestran.Where(x => x.MedicalPrescriptionID == MedicalPrescriptionId && x.DrugId == Drug.DrugID).GroupBy(y => y.DrugId)
                                                     select new MedicalPrescriptionIddetails
                                                     {
                                                         DrugID = Drug.DrugID,
                                                         Drug = drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.Brand).FirstOrDefault(),
                                                         Quantity = med.Sum(x => x.Quantity),
                                                         UnitPrice = drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.Rate).FirstOrDefault(),
                                                         GST = taxMaster.Where(t => t.ID == drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.GSTPercentage).FirstOrDefault(),
                                                         AddCess = taxMaster.Where(t => t.ID == drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.AdditionalCESSPercentage).FirstOrDefault(),
                                                         Cess = taxMaster.Where(t => t.ID == drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.CESSPercentage).FirstOrDefault(),
                                                         UOM = drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.UOM).FirstOrDefault(),
                                                         MedicalPrescriptionTranId = med.Select(x => x.MedicalPrescriptionTranID).FirstOrDefault(),
                                                         IsSerial = true,
                                                         SerialsInfo = CheckingItemBalance == 0 ? null : ItemSerialList,
                                                         AvailQuantity = CheckingItemBalance,
                                                         IsMedicinePrescribed = true,
                                                         IsAvailable = CheckingItemBalance == 0 ? false : true,
                                                         AddCessValue = 0,
                                                         CessValue = 0,
                                                         Reqqty = 0,
                                                         TaxDescription = taxMaster.Where(t => t.ID == drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.TaxDescription).FirstOrDefault(),
                                                         CessDescription = taxMaster.Where(t => t.ID == drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.CESSDescription).FirstOrDefault(),
                                                         AddCessDescription = taxMaster.Where(t => t.ID == drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.AdditionalCESSDescription).FirstOrDefault(),
                                                         TaxID = drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.TaxID).FirstOrDefault(),
                                                     }).ToList());


                }
                else if (res == "BatchNumberBased")
                {
                    var drugGroup = WYNKContext.DrugGroup.Where(x => x.ID == WYNKContext.DrugMaster.Where(y => y.ID == Drug.DrugID).Select(y => y.GenericName).FirstOrDefault()).FirstOrDefault();
                    var RetestInterval = drugGroup.RetestInterval;
                    var CriticalIntervalDays = drugGroup.RetestCriticalInterval;

                    var drugstockcheck = WYNKContext.ItemBatch.Where(x => x.ItemID == Drug.DrugID && DateTime.Now.AddDays(CriticalIntervalDays) < x.ItemBatchExpiry.Date && x.StoreID == StoreID).OrderBy(x => x.CreatedUTC).ToList();

                    decimal? AvailableStock = drugstockcheck.Select(x => x.ItemBatchBalnceQty - (x.LockedQuantity != null ? x.LockedQuantity : 0)).Sum();

                    var ItemBatchDetails = (from itemBatch in drugstockcheck
                                            select new BatchInfo
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


                    PrescribedMedicinelist.AddRange((from med in medicalprestran.Where(x => x.MedicalPrescriptionID == MedicalPrescriptionId && x.DrugId == Drug.DrugID).GroupBy(y => y.DrugId)
                                                     select new MedicalPrescriptionIddetails
                                                     {
                                                         DrugID = Drug.DrugID,
                                                         Drug = drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.Brand).FirstOrDefault(),
                                                         Quantity = med.Sum(x => x.Quantity),
                                                         UnitPrice = drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.Rate).FirstOrDefault(),
                                                         GST = taxMaster.Where(t => t.ID == drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.GSTPercentage).FirstOrDefault(),
                                                         AddCess = taxMaster.Where(t => t.ID == drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.AdditionalCESSPercentage).FirstOrDefault(),
                                                         Cess = taxMaster.Where(t => t.ID == drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.CESSPercentage).FirstOrDefault(),
                                                         UOM = drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.UOM).FirstOrDefault(),
                                                         MedicalPrescriptionTranId = med.Select(x => x.MedicalPrescriptionTranID).FirstOrDefault(),
                                                         BatchDetail = ItemBatchDetails,
                                                         AvailQuantity = AvailableStock,
                                                         IsSerial = false,
                                                         IsMedicinePrescribed = true,
                                                         IsAvailable = AvailableStock == 0 ? false : true,
                                                         AddCessValue = 0,
                                                         CessValue = 0,
                                                         Reqqty = 0,
                                                         TaxDescription = taxMaster.Where(t => t.ID == drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.TaxDescription).FirstOrDefault(),
                                                         CessDescription = taxMaster.Where(t => t.ID == drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.CESSDescription).FirstOrDefault(),
                                                         AddCessDescription = taxMaster.Where(t => t.ID == drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.AdditionalCESSDescription).FirstOrDefault(),
                                                         TaxID = drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.TaxID).FirstOrDefault(),
                                                     }).ToList());



                }
                else
                {
                    var Date = DateTime.Now;
                    var FinancialYearId = WYNKContext.FinancialYear.Where(x => Convert.ToDateTime(x.FYFrom) <= Date && Convert.ToDateTime(x.FYTo) >= Date && x.CMPID == CMPID && x.IsActive == true).Select(x => x.ID).FirstOrDefault();
                    var CheckingItemBalance = WYNKContext.ItemBalance.Where(x => x.FYear == FinancialYearId && x.ItemID == Drug.DrugID && x.StoreID == StoreID).Select(x => x.ClosingBalance).FirstOrDefault();

                    PrescribedMedicinelist.AddRange((from med in medicalprestran.Where(x => x.MedicalPrescriptionID == MedicalPrescriptionId && x.DrugId == Drug.DrugID).GroupBy(y => y.DrugId)
                                                     select new MedicalPrescriptionIddetails
                                                     {
                                                         DrugID = Drug.DrugID,
                                                         Drug = drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.Brand).FirstOrDefault(),
                                                         Quantity = med.Sum(x => x.Quantity),
                                                         UnitPrice = drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.Rate).FirstOrDefault(),
                                                         GST = taxMaster.Where(t => t.ID == drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.GSTPercentage).FirstOrDefault(),
                                                         AddCess = taxMaster.Where(t => t.ID == drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.AdditionalCESSPercentage).FirstOrDefault(),
                                                         Cess = taxMaster.Where(t => t.ID == drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.CESSPercentage).FirstOrDefault(),
                                                         UOM = drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.UOM).FirstOrDefault(),
                                                         MedicalPrescriptionTranId = med.Select(x => x.MedicalPrescriptionTranID).FirstOrDefault(),
                                                         AvailQuantity = CheckingItemBalance,
                                                         IsSerial = null,
                                                         IsMedicinePrescribed = true,
                                                         IsAvailable = CheckingItemBalance == 0 ? false : true,
                                                         AddCessValue = 0,
                                                         CessValue = 0,
                                                         Reqqty = 0,
                                                         TaxDescription = taxMaster.Where(t => t.ID == drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.TaxDescription).FirstOrDefault(),
                                                         CessDescription = taxMaster.Where(t => t.ID == drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.CESSDescription).FirstOrDefault(),
                                                         AddCessDescription = taxMaster.Where(t => t.ID == drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.AdditionalCESSDescription).FirstOrDefault(),
                                                         TaxID = drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.TaxID).FirstOrDefault(),
                                                     }).ToList());


                }
            }
            MedicalPrescription.MedicalPrescriptionIddetails = PrescribedMedicinelist;
            return MedicalPrescription;
        }

        public dynamic CheckMedPresQuantity(int Quantity, int DrugID, int StoreID)
        {
            IList<AllotedBatch> Alloted = new List<AllotedBatch>();
            var requestedQuantity = Quantity;
            decimal totalQuantity = 0;

            var drugGroup = WYNKContext.DrugGroup.Where(x => x.ID == WYNKContext.DrugMaster.Where(y => y.ID == DrugID).Select(y => y.GenericName).FirstOrDefault()).FirstOrDefault();
            var RetestInterval = drugGroup.RetestInterval;
            var CriticalIntervalDays = drugGroup.RetestCriticalInterval;


            var drugstockcheck = WYNKContext.ItemBatch.Where(x => x.ItemID == DrugID && DateTime.Now.AddDays(CriticalIntervalDays) < x.ItemBatchExpiry.Date && x.StoreID == StoreID).OrderBy(x => x.CreatedUTC).ToList();
            decimal? EnoughStock = drugstockcheck.Select(x => x.ItemBatchBalnceQty - (x.LockedQuantity != null ? x.LockedQuantity : 0)).Sum();
            if (EnoughStock >= requestedQuantity)
            {
                /*Alloting Batches based on Requested Quantity*/
                foreach (var res in drugstockcheck)
                {
                    if (!(res.LockedQuantity == res.ItemBatchBalnceQty) && !(res.ItemBatchBalnceQty == 0))
                    {
                        if (totalQuantity < requestedQuantity && !(totalQuantity == requestedQuantity))
                        {
                            Alloted.Add(new AllotedBatch()
                            {
                                DrugId = res.ItemID,
                                DrugName = WYNKContext.DrugMaster.Where(x => x.ID == res.ItemID).Select(x => x.Brand).FirstOrDefault(),
                                itemBatchNo = res.ItemBatchNumber,
                                balanceQty = res.ItemBatchBalnceQty - Convert.ToDecimal(res.LockedQuantity),
                                CreatedUTC = res.CreatedUTC,
                                ExpiryDate = res.ItemBatchExpiry,
                                ExpireInDays = ExpireDateCalculation(res.ItemBatchExpiry, CriticalIntervalDays, RetestInterval)
                            });
                            totalQuantity = totalQuantity + Math.Abs(res.ItemBatchBalnceQty - Convert.ToDecimal(res.LockedQuantity));
                        }
                    }
                }
                return new
                {
                    Success = true,
                    Message = "Available Batch Details",
                    ExactAlloted = Alloted,
                };
            }
            else
            {
                return new
                {
                    Success = false,
                    Message = "Available Quantity",
                    AvailableQuantity = Convert.ToDecimal(EnoughStock),
                };
            }

        }

        public IList<AllotedBatch> CheckMedPresQuantity1(int Quantity, int DrugID, int StoreId)
        {
            var MedicalPrescription = new BillingPharmacy();
            IList<AllotedBatch> Alloted = new List<AllotedBatch>();
            var requestedQuantity = Quantity;
            decimal totalQuantity = 0;
            var drugGroup = WYNKContext.DrugGroup.Where(x => x.ID == WYNKContext.DrugMaster.Where(y => y.ID == DrugID).Select(y => y.GenericName).FirstOrDefault()).FirstOrDefault();
            var RetestInterval = drugGroup.RetestInterval;
            var CriticalIntervalDays = drugGroup.RetestCriticalInterval;

            var drugstockcheck = WYNKContext.ItemBatch.Where(x => x.ItemID == DrugID && DateTime.Now.AddDays(CriticalIntervalDays) < x.ItemBatchExpiry.Date && x.StoreID == StoreId).OrderBy(x => x.CreatedUTC).ToList();
            decimal StillNeededQuantity = requestedQuantity;
            decimal? EnoughStock = drugstockcheck.Select(x => x.ItemBatchBalnceQty - (x.LockedQuantity != null ? x.LockedQuantity : 0)).Sum();
            if (EnoughStock >= requestedQuantity)
            {
                /*Alloting Batches based on Requested Quantity*/
                foreach (var res in drugstockcheck)
                {
                    if (!(StillNeededQuantity == 0))
                    {
                        if (!(res.LockedQuantity == res.ItemBatchBalnceQty) && !(res.ItemBatchBalnceQty == 0))
                        {
                            if (totalQuantity < requestedQuantity && !(totalQuantity == requestedQuantity))
                            {
                                var result = StockCheck.MedicalPrescription(res, totalQuantity, requestedQuantity, StillNeededQuantity);
                                Alloted.Add(result.AllotedBatch);
                                totalQuantity = result.totalQuantity;
                                StillNeededQuantity = result.StillNeededQuantity;
                                var itemBatch = new ItemBatch();
                                itemBatch = WYNKContext.ItemBatch.Where(x => x.ItemBatchNumber == result.AllotedBatch.itemBatchNo && x.ItemID == result.AllotedBatch.DrugId).FirstOrDefault();
                                itemBatch.LockedQuantity = Convert.ToInt32((itemBatch.LockedQuantity != null ? itemBatch.LockedQuantity : 0) + result.ActualTakenFromBatch);
                                WYNKContext.ItemBatch.UpdateRange(itemBatch);
                                WYNKContext.SaveChanges();
                            }
                        }

                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                Alloted.Add(new AllotedBatch()
                {
                    DrugId = DrugID,
                    itemBatchNo = " ",
                    balanceQty = Convert.ToDecimal(EnoughStock),
                   
                });
            }
            return Alloted;
        }

        public async Task<dynamic> AddBillingDetailsAsync(BillingPharmacy AddBill)
        {
            int REGTranId = AddBill.MedicalBillMaster.RegistrationTranID;
            string PatientUIN = AddBill.MedicalBillMaster.UIN;
            AddBill.InSufficientDrugs = new List<InSufficientDrug>();
            List<AllotedBatch> AllotingBatchs = new List<AllotedBatch>();
            var medprestran = AddBill.MedicalBillTran.MedicalPrescriptionID;
            var StoreId = AddBill.StoreID;
            try
            {
              

            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Success = false,
                Message = "SomeThing Went Wrong"
            };
        }

        public string GenerateBillNo(int rnControlCode)
        {
            var rn = CMPSContext.NumberControl.FirstOrDefault(x => x.VCID == rnControlCode);
            rn.RunningNumber += 1;
            CMPSContext.Entry(rn).State = EntityState.Modified;
            CMPSContext.SaveChanges();
            return $"{rn.Prefix}{rn.Suffix}{rn.RunningNumber}";
        }

        public dynamic GetdrugDetails(int ID, int StoreID, int cmpid)
        {
            var DrugTrackerId = WYNKContext.DrugMaster.Where(x => x.ID == ID).Select(x => x.DrugTracker).FirstOrDefault();
            var res = Enum.GetName(typeof(TrackingType), DrugTrackerId);

            if (res == "SerialNumberBased")
            {
                var Date = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));

                var FinancialYearId = WYNKContext.FinancialYear.Where(x => Convert.ToDateTime(x.FYFrom) <= Date && Convert.ToDateTime(x.FYTo) >= Date && x.IsActive == true && x.CMPID == cmpid).Select(x => x.ID).FirstOrDefault();

                var CheckingItemBalance = WYNKContext.ItemBalance.Where(x => x.FYear == FinancialYearId && x.ItemID == ID && x.CmpID == cmpid && x.StoreID == StoreID).Select(x => x.ClosingBalance).FirstOrDefault();

                if (CheckingItemBalance != 0)
                {
                    var ItemSerialList = (from ItemSerial in WYNKContext.ItemSerial.Where(x => x.ItemID == ID && x.IssueNo == null && x.StoreID == StoreID && x.cmpID == cmpid)
                                          select new SerialInfo
                                          {
                                              SerialNo = ItemSerial.SerialNo,
                                              ExpiryDate = ItemSerial.ExpiryDate,
                                              BillNo = ItemSerial.GRNNo,
                                          }).ToList();



                    var SerialsDrug = (from drug in WYNKContext.DrugMaster.Where(x => x.ID == ID)
                                       select new
                                       {
                                           DrugID = drug.ID,
                                           Brand = drug.Brand,
                                           UOM = drug.UOM,
                                           SerialList = ItemSerialList,

                                           UnitPrice = drug.Rate,
                                           GST = drug.Gst,
                                           Cess = drug.CESSPercentage,
                                           AddCess = drug.AdditionalCESSPercentage,
                                           GenericName = WYNKContext.DrugGroup.Where(x => x.ID == drug.GenericName).Select(x => x.Description).FirstOrDefault(),
                                           IsSerial = true,
                                           SerialsInfo = CheckingItemBalance == 0 ? null : ItemSerialList,
                                           AvailQuantity = CheckingItemBalance,
                                           IsMedicinePrescribed = false,
                                           IsAvailable = CheckingItemBalance == 0 ? false : true,
                                           DrugCategory = Enum.GetName(typeof(DrugCategory), drug.DrugCategory) == "ImplantDrug" ? "ImplantDrug" : "",
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

                var Date = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                var FinancialYearId = WYNKContext.FinancialYear.Where(x => Convert.ToDateTime(x.FYFrom) <= Date && Convert.ToDateTime(x.FYTo) >= Date && x.IsActive == true && x.CMPID == cmpid).Select(x => x.ID).FirstOrDefault();
                decimal? AvailableStock = WYNKContext.ItemBalance.Where(x => x.FYear == FinancialYearId && x.ItemID == ID && x.CmpID == cmpid && x.StoreID == StoreID).Select(x => x.ClosingBalance).FirstOrDefault();

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
                                      Drug = drug.Brand,
                                      UOM = drug.UOM,
                                      UnitPrice = drug.Rate,
                                      GST = drug.Gst,
                                      Cess = drug.CESSPercentage,
                                      AddCess = drug.AdditionalCESSPercentage,
                                      GenericName = WYNKContext.DrugGroup.Where(x => x.ID == drug.GenericName).Select(x => x.Description).FirstOrDefault(),
                                      AvailQuantity = AvailableStock,
                                      BatchDetail = ItemBatchDetails,
                                      IsSerial = false,
                                      IsMedicinePrescribed = false,
                                      IsAvailable = AvailableStock == 0 ? false : true,
                                      DrugCategory = Enum.GetName(typeof(DrugCategory), drug.DrugCategory) == "ImplantDrug" ? "ImplantDrug" : "",
                                  }).FirstOrDefault();

                if (BatchDrugs.IsAvailable)
                {
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
                    return new
                    {
                        Success = false,
                        Message = "Batch Details Drugs",
                        IsSerial = false,
                    };
                }
            }
            else
            {
                var Date = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));

                var FinancialYearId = WYNKContext.FinancialYear.Where(x => Convert.ToDateTime(x.FYFrom) <= Date && Convert.ToDateTime(x.FYTo) >= Date && x.IsActive == true && x.CMPID == cmpid).Select(x => x.ID).FirstOrDefault();

                var CheckingItemBalance = WYNKContext.ItemBalance.Where(x => x.FYear == FinancialYearId && x.ItemID == ID && x.CmpID == cmpid && x.StoreID == StoreID).Select(x => x.ClosingBalance).FirstOrDefault();

                if (CheckingItemBalance != 0)
                {
                    return new
                    {
                        Success = true,
                        Message = "Other Drug Details Drugs",
                        DrugID = ID,
                        AvailableQty = CheckingItemBalance,
                        GenericName = WYNKContext.DrugGroup.Where(x => x.ID == WYNKContext.DrugMaster.Where(y => y.ID == ID).Select(y => y.GenericName).FirstOrDefault()).Select(x => x.Description).FirstOrDefault(),
                        Brand = WYNKContext.DrugMaster.Where(x => x.ID == ID).Select(x => x.Brand).FirstOrDefault(),
                        UOM = WYNKContext.DrugMaster.Where(x => x.ID == ID).Select(x => x.UOM).FirstOrDefault(),
                        UnitPrice = WYNKContext.DrugMaster.Where(x => x.ID == ID).Select(x => x.Rate).FirstOrDefault(),
                        GST = WYNKContext.DrugMaster.Where(x => x.ID == ID).Select(x => x.Gst).FirstOrDefault(),
                        Cess = WYNKContext.DrugMaster.Where(x => x.ID == ID).Select(x => x.CESSPercentage).FirstOrDefault(),
                        AddCess = WYNKContext.DrugMaster.Where(x => x.ID == ID).Select(x => x.AdditionalCESSPercentage).FirstOrDefault(),
                        IsMedicinePrescribed = false,
                        IsAvailable = CheckingItemBalance == 0 ? false : true,
                        DrugCategory = Enum.GetName(typeof(DrugCategory), WYNKContext.DrugMaster.Where(x => x.ID == ID).Select(x => x.DrugCategory).FirstOrDefault()) == "ImplantDrug" ? "ImplantDrug" : "",
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

        public dynamic GetClosedDetails(string ID, int CmpID, int TC, string GMT)
        {
            var MedicalPrescription = new BillingPharmacy();
            var drugmaster = WYNKContext.DrugMaster.ToList();
            var taxMaster = CMPSContext.TaxMaster.ToList();
            var medicalbilltran = WYNKContext.MedicalBillTran.ToList();
            var OnelineMaster = CMPSContext.OneLineMaster.ToList();
            TimeSpan ts = TimeSpan.Parse(GMT);

            MedicalPrescription.GetClosedDetails = (from MB in WYNKContext.MedicalBillMaster.Where(x => x.TransactionId == TC)
                                                    join med in medicalbilltran on MB.ID equals med.MedicalBillID
                                                    where med.MedicalPrescriptionID == ID
                                                    select new GetClosedDetails
                                                    {
                                                        DrugID = med.DrugID,
                                                        Drug = drugmaster.Where(x => x.ID == med.DrugID).Select(x => x.Brand).FirstOrDefault(),
                                                        UOM = med.UOM,
                                                        Reqqty = Convert.ToInt32(med.Quantity),
                                                        UnitPrice = Convert.ToDecimal(med.ItemRate),
                                                        Amount = Convert.ToDecimal(med.ItemValue),
                                                        Discount = Convert.ToDecimal(med.DiscountPercentage),
                                                        DiscountAmount = Convert.ToDecimal(med.DiscountAmount),
                                                        GrossAmount = Convert.ToInt32(med.DiscountPercentage) != 100 ? Convert.ToDecimal(med.ItemValue - med.DiscountAmount) : 0,
                                                        GST = Convert.ToDecimal(med.GSTPercentage),
                                                        GSTValue = Convert.ToDecimal(med.GSTTaxValue),
                                                        Cess = med.CESSPercentage == null ? (decimal?)null : Convert.ToDecimal(med.CESSPercentage),
                                                        AddCess = med.AdditionalCESSPercentage == null ? (decimal?)null : Convert.ToDecimal(med.AdditionalCESSPercentage),
                                                        TotalCost = Convert.ToInt32(med.DiscountPercentage) != 100 ? Convert.ToDecimal(med.GSTTaxValue + (med.ItemValue - med.DiscountAmount + med.CESSValue + med.AdditionalCESSValue)) : 0,
                                                        CessValue = med.CESSPercentage == null ? (decimal?)null : Convert.ToDecimal(med.CESSValue),
                                                        AddCessValue = med.AdditionalCESSPercentage == null ? (decimal?)null : Convert.ToDecimal(med.AdditionalCESSValue),
                                                        TaxDescription = taxMaster.Where(t => t.ID == drugmaster.Where(x => x.ID == med.DrugID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.TaxDescription).FirstOrDefault(),
                                                        CessDescription = taxMaster.Where(t => t.ID == drugmaster.Where(x => x.ID == med.DrugID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.CESSDescription).FirstOrDefault(),
                                                        AddCessDescription = taxMaster.Where(t => t.ID == drugmaster.Where(x => x.ID == med.DrugID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.AdditionalCESSDescription).FirstOrDefault(),
                                                    }).ToList();

            MedicalPrescription.MedicalBillID = WYNKContext.MedicalBillTran.Where(x => x.MedicalPrescriptionID == ID).Select(x => x.MedicalBillID).FirstOrDefault();



            var medi = WYNKContext.PaymentMaster.Where(x => x.PaymentReferenceID == MedicalPrescription.MedicalBillID).Select(x => x.PaymentReferenceID).ToList();
            MedicalPrescription.PrintTotal = Convert.ToInt32(WYNKContext.MedicalBillMaster.Where(x => x.ID == MedicalPrescription.MedicalBillID).Select(x => x.TotalBillValue).FirstOrDefault());

            foreach (var item in medi.ToList())
            {
                MedicalPrescription.paymenttran = (from payment in WYNKContext.PaymentMaster.Where(x => x.PaymentReferenceID == item)
                                                   select new Payment_Master
                                                   {
                                                       PaymentMode = OnelineMaster.Where(x => x.OLMID == payment.OLMID).Select(x => x.ParentDescription).FirstOrDefault(),
                                                       InstrumentNumber = payment.InstrumentNumber,
                                                       Instrumentdate = payment.Instrumentdate,
                                                       BankName = payment.BankName,
                                                       BankBranch = payment.BankBranch,
                                                       Expirydate = payment.Expirydate,
                                                       Amount = payment.Amount,
                                                       PaymentReferenceID = payment.PaymentReferenceID,
                                                   }).ToList();
            }
            
            MedicalPrescription.PaidTotal = Math.Abs((MedicalPrescription.paymenttran == null ? 0 : MedicalPrescription.paymenttran.Sum(x => x.Amount)));
            MedicalPrescription.CMPDetails = CMPSContext.Company.Where(x => x.CmpID == CmpID).FirstOrDefault();
            MedicalPrescription.MedicalBillMaster = WYNKContext.MedicalBillMaster.Where(x => x.ID == WYNKContext.MedicalBillTran.Where(y => y.MedicalPrescriptionID == ID).Select(y => y.MedicalBillID).FirstOrDefault()).FirstOrDefault();

            MedicalPrescription.MedicalBillMaster.CreatedUTC = MedicalPrescription.MedicalBillMaster.CreatedUTC + ts;
            MedicalPrescription.ReceiptRunningNo = WYNKContext.PaymentMaster.Where(x => x.PaymentReferenceID == MedicalPrescription.MedicalBillID).Select(x => x.ReceiptNumber).FirstOrDefault();
            MedicalPrescription.ReceiptDatetime = WYNKContext.PaymentMaster.Where(x => x.PaymentReferenceID == MedicalPrescription.MedicalBillID).Select(x => x.ReceiptDate + ts).FirstOrDefault();
            return MedicalPrescription;
        }

        public dynamic GetPartiallyClosedDetails(string MedicalPrescriptionId, int StoreID, int CMPID)
        {
            var MedicalPrescription = new BillingPharmacy();

            MedicalPrescription.MedicalBillID = WYNKContext.MedicalBillTran.Where(x => x.MedicalPrescriptionID == MedicalPrescriptionId).Select(x => x.MedicalBillID).FirstOrDefault();

            var taxMaster = CMPSContext.TaxMaster.ToList();
            var drugmaster = WYNKContext.DrugMaster.ToList();
            MedicalPrescription.paymenttran = new List<Payment_Master>();
            var PurchasedDrugIDs = WYNKContext.MedicalBillTran.Where(x => x.MedicalPrescriptionID == MedicalPrescriptionId && x.IsMedicinePrescribed == true).Select(x => x.DrugID).ToList();
            var DrugDetails = (from med in WYNKContext.MedicalPrescriptionTran.Where(x => x.MedicalPrescriptionID == MedicalPrescriptionId)
                               where !PurchasedDrugIDs.Contains(med.DrugId)
                               group med by med.DrugId into medgroup
                               select new
                               {
                                   DrugID = medgroup.Key,
                               }).ToList();

            List<MedicalPrescriptionIddetails> PrescribedMedicinelist = new List<MedicalPrescriptionIddetails>();

            foreach (var Drug in DrugDetails)
            {
                var DrugTrackerId = WYNKContext.DrugMaster.Where(x => x.ID == Drug.DrugID).Select(x => x.DrugTracker).FirstOrDefault();
                var res = Enum.GetName(typeof(TrackingType), DrugTrackerId);

                if (res == "SerialNumberBased")
                {
                    var Date = DateTime.Now;
                    var FinancialYearId = WYNKContext.FinancialYear.Where(x => Convert.ToDateTime(x.FYFrom) <= Date && Convert.ToDateTime(x.FYTo) >= Date && x.CMPID == CMPID && x.IsActive == true).Select(x => x.ID).FirstOrDefault();
                    var CheckingItemBalance = WYNKContext.ItemBalance.Where(x => x.FYear == FinancialYearId && x.ItemID == Drug.DrugID && x.StoreID == StoreID).Select(x => x.ClosingBalance).FirstOrDefault();

                    var ItemSerialList = (from ItemSerial in WYNKContext.ItemSerial.Where(x => x.ItemID == Drug.DrugID && x.IssueNo == null && x.StoreID == StoreID)
                                          select new SerialInfo
                                          {
                                              SerialNo = ItemSerial.SerialNo,
                                          }).ToList();

                    PrescribedMedicinelist.AddRange((from med in WYNKContext.MedicalPrescriptionTran.Where(x => x.MedicalPrescriptionID == MedicalPrescriptionId && x.DrugId == Drug.DrugID).GroupBy(y => y.DrugId)
                                                     select new MedicalPrescriptionIddetails
                                                     {
                                                         DrugID = Drug.DrugID,
                                                         Drug = drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.Brand).FirstOrDefault(),
                                                         Quantity = med.Sum(x => x.Quantity),
                                                         UnitPrice = drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.Rate).FirstOrDefault(),
                                                         GST = Convert.ToInt32(drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.Gst).FirstOrDefault()),
                                                         AddCess = drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.AdditionalCESSPercentage).FirstOrDefault(),
                                                         Cess = drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.CESSPercentage).FirstOrDefault(),
                                                         UOM = drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.UOM).FirstOrDefault(),
                                                         MedicalPrescriptionTranId = med.Select(x => x.MedicalPrescriptionTranID).FirstOrDefault(),
                                                         IsSerial = true,
                                                         SerialsInfo = CheckingItemBalance == 0 ? null : ItemSerialList,
                                                         AvailQuantity = CheckingItemBalance,
                                                         IsMedicinePrescribed = true,
                                                         IsAvailable = CheckingItemBalance == 0 ? false : true,
                                                         AddCessValue = 0,
                                                         CessValue = 0,
                                                         TaxDescription = taxMaster.Where(t => t.ID == drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.TaxDescription).FirstOrDefault(),
                                                         CessDescription = taxMaster.Where(t => t.ID == drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.CESSDescription).FirstOrDefault(),
                                                         AddCessDescription = taxMaster.Where(t => t.ID == drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.AdditionalCESSDescription).FirstOrDefault(),
                                                     }).ToList());


                }
                else if (res == "BatchNumberBased")
                {
                    var drugGroup = WYNKContext.DrugGroup.Where(x => x.ID == WYNKContext.DrugMaster.Where(y => y.ID == Drug.DrugID).Select(y => y.GenericName).FirstOrDefault()).FirstOrDefault();
                    var RetestInterval = drugGroup.RetestInterval;
                    var CriticalIntervalDays = drugGroup.RetestCriticalInterval;

                    var drugstockcheck = WYNKContext.ItemBatch.Where(x => x.ItemID == Drug.DrugID && DateTime.Now.AddDays(CriticalIntervalDays) < x.ItemBatchExpiry.Date && x.StoreID == StoreID).OrderBy(x => x.CreatedUTC).ToList();

                    decimal? AvailableStock = drugstockcheck.Select(x => x.ItemBatchBalnceQty - (x.LockedQuantity != null ? x.LockedQuantity : 0)).Sum();

                    var ItemBatchDetails = (from itemBatch in drugstockcheck
                                            select new BatchInfo
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


                    PrescribedMedicinelist.AddRange((from med in WYNKContext.MedicalPrescriptionTran.Where(x => x.MedicalPrescriptionID == MedicalPrescriptionId && x.DrugId == Drug.DrugID).GroupBy(y => y.DrugId)
                                                     select new MedicalPrescriptionIddetails
                                                     {
                                                         DrugID = Drug.DrugID,
                                                         Drug = drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.Brand).FirstOrDefault(),
                                                         Quantity = med.Sum(x => x.Quantity),
                                                         UnitPrice = drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.Rate).FirstOrDefault(),
                                                         GST = Convert.ToInt32(drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.Gst).FirstOrDefault()),
                                                         AddCess = drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.AdditionalCESSPercentage).FirstOrDefault(),
                                                         Cess = drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.CESSPercentage).FirstOrDefault(),
                                                         UOM = drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.UOM).FirstOrDefault(),
                                                         MedicalPrescriptionTranId = med.Select(x => x.MedicalPrescriptionTranID).FirstOrDefault(),
                                                         BatchDetail = ItemBatchDetails,
                                                         AvailQuantity = AvailableStock,
                                                         IsSerial = false,
                                                         IsMedicinePrescribed = true,
                                                         IsAvailable = AvailableStock == 0 ? false : true,
                                                         AddCessValue = 0,
                                                         CessValue = 0,
                                                         TaxDescription = taxMaster.Where(t => t.ID == drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.TaxDescription).FirstOrDefault(),
                                                         CessDescription = taxMaster.Where(t => t.ID == drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.CESSDescription).FirstOrDefault(),
                                                         AddCessDescription = taxMaster.Where(t => t.ID == drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.AdditionalCESSDescription).FirstOrDefault(),
                                                     }).ToList());



                }
                else
                {
                    var Date = DateTime.Now;
                    var FinancialYearId = WYNKContext.FinancialYear.Where(x => Convert.ToDateTime(x.FYFrom) <= Date && Convert.ToDateTime(x.FYTo) >= Date && x.CMPID == CMPID && x.IsActive == true).Select(x => x.ID).FirstOrDefault();
                    var CheckingItemBalance = WYNKContext.ItemBalance.Where(x => x.FYear == FinancialYearId && x.ItemID == Drug.DrugID && x.StoreID == StoreID).Select(x => x.ClosingBalance).FirstOrDefault();

                    PrescribedMedicinelist.AddRange((from med in WYNKContext.MedicalPrescriptionTran.Where(x => x.MedicalPrescriptionID == MedicalPrescriptionId && x.DrugId == Drug.DrugID).GroupBy(y => y.DrugId)
                                                     select new MedicalPrescriptionIddetails
                                                     {
                                                         DrugID = Drug.DrugID,
                                                         Drug = drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.Brand).FirstOrDefault(),
                                                         Quantity = med.Sum(x => x.Quantity),
                                                         UnitPrice = drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.Rate).FirstOrDefault(),
                                                         GST = Convert.ToInt32(drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.Gst).FirstOrDefault()),
                                                         AddCess = drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.AdditionalCESSPercentage).FirstOrDefault(),
                                                         Cess = drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.CESSPercentage).FirstOrDefault(),
                                                         UOM = drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.UOM).FirstOrDefault(),
                                                         MedicalPrescriptionTranId = med.Select(x => x.MedicalPrescriptionTranID).FirstOrDefault(),
                                                         AvailQuantity = CheckingItemBalance,
                                                         IsSerial = null,
                                                         IsMedicinePrescribed = true,
                                                         IsAvailable = CheckingItemBalance == 0 ? false : true,
                                                         AddCessValue = 0,
                                                         CessValue = 0,
                                                         TaxDescription = taxMaster.Where(t => t.ID == drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.TaxDescription).FirstOrDefault(),
                                                         CessDescription = taxMaster.Where(t => t.ID == drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.CESSDescription).FirstOrDefault(),
                                                         AddCessDescription = taxMaster.Where(t => t.ID == drugmaster.Where(x => x.ID == Drug.DrugID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.AdditionalCESSDescription).FirstOrDefault(),
                                                     }).ToList());


                }
            }

            var MedicalBillAmount = WYNKContext.MedicalBillMaster.Where(x => x.ID == MedicalPrescription.MedicalBillID).Select(x => x.TotalBillValue).FirstOrDefault();
            MedicalPrescription.paymenttran = WYNKContext.PaymentMaster.Where(x => x.PaymentReferenceID == MedicalPrescription.MedicalBillID).ToList();
            MedicalPrescription.PaidTotal = Math.Abs((MedicalPrescription.paymenttran == null ? 0 : MedicalPrescription.paymenttran.Sum(x => x.Amount)));
            MedicalPrescription.RemainingAmount = Math.Abs((MedicalPrescription.paymenttran == null ? 0 : MedicalPrescription.paymenttran.Sum(x => x.Amount)) - Convert.ToDecimal(MedicalBillAmount));
            MedicalPrescription.MedicalPrescriptionIddetails = PrescribedMedicinelist;

            return MedicalPrescription;
        }

        public dynamic AddOutPatientBilling(OutPatientBillingPharmacy AddOPBill, string GMT)
        {

            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    int REGTranId = AddOPBill.MedicalBillMaster.RegistrationTranID;
                    string PatientUIN = AddOPBill.MedicalBillMaster.UIN;

                    AddOPBill.InSufficientDrugs = new List<InSufficientDrug>();
                    AddOPBill.InSufficientSerials = new List<SerialDetail>();
                    AddOPBill.InSufficientOtherDrugs = new List<OtherDrugs>();

                    List<AllotedBatch> AllotingBatchs = new List<AllotedBatch>();
                    List<SerialDetail> AllotingSerial = new List<SerialDetail>();
                    List<OtherDrugs> AllotingOtherDrugs = new List<OtherDrugs>();

                    List<Alert> Alerts = new List<Alert>();

                    var medprestran = AddOPBill.MedicalBillTran.MedicalPrescriptionID;
                    var StoreId = AddOPBill.StoreID;
                    TimeSpan ts = TimeSpan.Parse(GMT);

                    var drugMaster = WYNKContext.DrugMaster.Where(x => x.Cmpid == AddOPBill.CmpId).ToList();
                    var uommaster = CMPSContext.uommaster.ToList();

                    string MedicalBillNo = "0";

                    var Datee = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                    var Fyear = Convert.ToString(WYNKContext.FinancialYear.Where(x => x.FYAccYear == (Convert.ToInt16(AddOPBill.FYear)) && x.CMPID == AddOPBill.CmpId && x.IsActive == true).Select(x => x.ID).FirstOrDefault());
                    //var Fyear = Convert.ToString(WYNKContext.FinancialYear.Where(x => x.FYFrom <= Datee && x.FYTo >= Datee && x.CMPID == AddOPBill.CmpId && x.IsActive == true).Select(x => x.ID).FirstOrDefault());
           

                    DateTime GivenDate;
                    var appdate = DateTime.TryParseExact(AddOPBill.GivenDate.Trim(), "dd-MM-yyyy,HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out GivenDate);
                    {
                        GivenDate.ToString("dd-MM-yyyy,HH:mm", CultureInfo.InvariantCulture);
                    }
                    GivenDate = GivenDate - ts;

                    foreach (var MedicalPrescriptionIdDetail in AddOPBill.MedicalPrescriptionIddetails.ToList())
                    {
                        var Drugid = MedicalPrescriptionIdDetail.DrugID;
                        var reqQuantity = Convert.ToInt32(MedicalPrescriptionIdDetail.AvailQuantity);
                        IList<AllotedBatch> AllotingBatch = new List<AllotedBatch>();

                        var DrugTrackerId = WYNKContext.DrugMaster.Where(x => x.ID == Drugid).Select(x => x.DrugTracker).FirstOrDefault();
                        var res = Enum.GetName(typeof(TrackingType), DrugTrackerId);


                        if (res == "SerialNumberBased")
                        {
                            AddOPBill.InSufficientSerials = CheckSerials(MedicalPrescriptionIdDetail.SelectedList, Drugid, AddOPBill.StoreID, AddOPBill.CmpId);

                            if (AddOPBill.InSufficientSerials.Count == 0)
                            {
                                foreach (var item in MedicalPrescriptionIdDetail.SelectedList.ToList())
                                {

                                    var ISerial = new SerialDetail();
                                    ISerial.DrugID = Drugid;
                                    ISerial.SerialNo = item.SerialNo;
                                    ISerial.BillNo = item.BillNo;
                                    ISerial.ExpiryDate = item.ExpiryDate;
                                    AllotingSerial.Add(ISerial);
                                }
                            }

                        }
                        else if (res == "BatchNumberBased")
                        {
                            var qtylists = MedicalPrescriptionIdDetail.BatchDetail.Where(x => x.QtyTaken != 0).ToList();

                            foreach (var qtylist in qtylists.ToList())
                            {
                                AllotingBatch = CheckBatchQty(Drugid, AddOPBill.StoreID, qtylist.BatchNo, qtylist.ExpiryDate, qtylist.QtyTaken, AddOPBill.CmpId);

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
                                            AddOPBill.InSufficientDrugs.Add(InSufficientDrugs);
                                        }
                                    }
                                }

                            }
                        }
                        else
                        {
                            var Date = DateTime.Now;

                            var FinancialYearId = WYNKContext.FinancialYear.Where(x => Convert.ToDateTime(x.FYFrom) <= Date && Convert.ToDateTime(x.FYTo) >= Date && x.CMPID == AddOPBill.CmpId && x.IsActive == true).Select(x => x.ID).FirstOrDefault();

                            var Itembal = WYNKContext.ItemBalance.Where(x => x.ItemID == Drugid && x.FYear == FinancialYearId && x.StoreID == AddOPBill.StoreID && x.CmpID == AddOPBill.CmpId).Select(x => x.ClosingBalance).FirstOrDefault();

                            if (Itembal >= MedicalPrescriptionIdDetail.Reqqty)
                            {
                                var OtherDrugs = new OtherDrugs();
                                OtherDrugs.DrugID = Drugid;
                                OtherDrugs.QtyTobeTaken = MedicalPrescriptionIdDetail.Reqqty;
                                AllotingOtherDrugs.Add(OtherDrugs);
                            }
                            else
                            {
                                var OtherDrugs = new OtherDrugs();
                                OtherDrugs.Brand = WYNKContext.DrugMaster.Where(x => x.ID == Drugid).Select(x => x.Brand).FirstOrDefault();
                                OtherDrugs.AvailableQty = Convert.ToInt32(Itembal);
                                OtherDrugs.DrugID = Drugid;
                                AddOPBill.InSufficientOtherDrugs.Add(OtherDrugs);
                            }

                        }

                    }

                    /*Stock Master*/
                    string stockmasterIdentitys = "";

                    if (AddOPBill.InSufficientDrugs.Count == 0 && AddOPBill.InSufficientSerials.Count == 0 && AddOPBill.InSufficientOtherDrugs.Count == 0)
                    {

                        var stockmas = AddBilling.AddstockMaster1(AddOPBill.MedicalBillMaster.BillNo, DateTime.UtcNow, AddOPBill.StoreID, null, 0, AddOPBill.Tc, CMPSContext.TransactionType.Where(x => x.TransactionID == AddOPBill.Tc).Select(x => x.Rec_Issue_type).FirstOrDefault(), AddOPBill.CmpId, AddOPBill.CreatedBy, Fyear);
                        WYNKContext.StockMaster.Add(stockmas);

                        string cmpname = CMPSContext.Company.Where(x => x.CmpID == AddOPBill.CmpId).Select(x => x.CompanyName).FirstOrDefault();
                        string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == AddOPBill.CreatedBy).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                        string userid = Convert.ToString(AddOPBill.CreatedBy);
                        ErrorLog oErrorLogs = new ErrorLog();
                        oErrorLogs.WriteErrorLogTitle(cmpname, "OP Medical billing", "User name :", username, "User ID :", userid, "Mode : Add");
                        object names = stockmas;
                        oErrorLogs.WriteErrorLogArray("StockMaster", names);

                        WYNKContext.SaveChanges();
                        stockmasterIdentitys = stockmas.RandomUniqueID;


                        if (AllotingBatchs.Count > 0)
                        {
                            foreach (var item2 in AllotingBatchs.ToList())
                            {

                                var itemBatch = new ItemBatch();
                                itemBatch = WYNKContext.ItemBatch.Where(x => x.ItemBatchNumber == item2.itemBatchNo && x.ItemID == item2.DrugId && x.StoreID == AddOPBill.StoreID && x.ItemBatchExpiry.Date == Convert.ToDateTime(item2.ExpiryDate) && x.cmpID == AddOPBill.CmpId).FirstOrDefault();
                                itemBatch.ItemBatchBalnceQty = itemBatch.ItemBatchBalnceQty - item2.GoingToIssue;
                                itemBatch.ItemBatchissueQty = itemBatch.ItemBatchissueQty + item2.GoingToIssue;
                                itemBatch.LockedQuantity = itemBatch.LockedQuantity - Convert.ToInt32(item2.GoingToIssue);
                                WYNKContext.ItemBatch.UpdateRange(itemBatch);

                                var ItemBalance = WYNKContext.ItemBalance.Where(x => x.ItemID == item2.DrugId && x.FYear == Convert.ToInt32(Fyear) && x.StoreID == AddOPBill.StoreID && x.CmpID == AddOPBill.CmpId).FirstOrDefault();

                                var CurrentMonth = DateTime.Now.Month;
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


                                var UomMaster = CMPSContext.uommaster.ToList();
                                var Uom = UomMaster.Where(u => u.Description == WYNKContext.DrugMaster.Where(x => x.ID == item2.DrugId).Select(x => x.UOM).FirstOrDefault()).Select(x => x.id).FirstOrDefault();

                                var stockTran = WYNKContext.StockTran.Where(x => x.SMID == stockmasterIdentitys && x.ItemID == item2.DrugId).FirstOrDefault();

                                if (stockTran == null)
                                {
                                    var StkTran = new StockTran();
                                    StkTran.SMID = stockmasterIdentitys;
                                    StkTran.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                                    StkTran.ItemID = item2.DrugId;
                                    var Qty = WYNKContext.StockTran.Where(x => x.SMID == stockmasterIdentitys && x.ItemID == item2.DrugId).Select(x => x.ItemQty).FirstOrDefault();
                                    StkTran.ItemQty = Qty != null ? Qty + item2.GoingToIssue : item2.GoingToIssue;
                                    StkTran.UOMID = UomMaster.Where(UOM => UOM.Description == drugMaster.Where(x => x.ID == item2.DrugId).Select(x => x.UOM).FirstOrDefault()).Select(UOM => UOM.id).FirstOrDefault();
                                    StkTran.ItemRate = drugMaster.Where(x => x.ID == item2.DrugId).Select(x => x.Rate).FirstOrDefault();
                                    StkTran.ItemValue = StkTran.ItemQty * StkTran.ItemRate;
                                    StkTran.CreatedUTC = DateTime.UtcNow;
                                    StkTran.CreatedBy = AddOPBill.CreatedBy;
                                    WYNKContext.StockTran.AddRange(StkTran);
                                    WYNKContext.SaveChanges();

                                    var stockMas = new StockMaster();
                                    stockMas = WYNKContext.StockMaster.Where(x => x.RandomUniqueID == stockmasterIdentitys).FirstOrDefault();
                                    WYNKContext.StockMaster.UpdateRange(AddBilling.UpdateStockMaster(AddOPBill.MedicalPrescriptionIddetails.Where(x => x.DrugID == item2.DrugId).FirstOrDefault(), stockMas));

                                    var ItemBatchID = WYNKContext.ItemBatch.Where(x => x.ItemBatchNumber == item2.itemBatchNo && x.ItemID == item2.DrugId && x.StoreID == AddOPBill.StoreID && x.ItemBatchExpiry.Date == Convert.ToDateTime(item2.ExpiryDate) && x.cmpID == AddOPBill.CmpId).Select(x => x.RandomUniqueID).FirstOrDefault();
                                    WYNKContext.ItemBatchTrans.Add(AddBilling.AddItemBatchTrans1(item2, stockmasterIdentitys, StkTran.RandomUniqueID, ItemBatchID, null, AddOPBill.Tc, Uom, AddOPBill.CreatedBy, AddOPBill.CmpId));
                                    WYNKContext.SaveChanges();
                                }
                                else
                                {
                                    var ItemBatchID = WYNKContext.ItemBatch.Where(x => x.ItemBatchNumber == item2.itemBatchNo && x.ItemID == item2.DrugId && x.StoreID == AddOPBill.StoreID && x.ItemBatchExpiry.Date == Convert.ToDateTime(item2.ExpiryDate) && x.cmpID == AddOPBill.CmpId).Select(x => x.RandomUniqueID).FirstOrDefault();
                                    WYNKContext.ItemBatchTrans.Add(AddBilling.AddItemBatchTrans1(item2, stockmasterIdentitys, stockTran.RandomUniqueID, ItemBatchID, null, AddOPBill.Tc, Uom, AddOPBill.CreatedBy, AddOPBill.CmpId));

                                    stockTran.ItemQty = stockTran.ItemQty + item2.GoingToIssue;
                                    WYNKContext.StockTran.UpdateRange(stockTran);

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
                                var ItemBalance = WYNKContext.ItemBalance.Where(x => x.FYear == Convert.ToInt32(Fyear) && x.ItemID == item.DrugID && x.StoreID == AddOPBill.StoreID && x.CmpID == AddOPBill.CmpId).FirstOrDefault();

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

                                var stockTran = WYNKContext.StockTran.Where(x => x.SMID == stockmasterIdentitys && x.ItemID == item.DrugID).FirstOrDefault();
                                var Uom = uommaster.Where(u => u.Description == WYNKContext.DrugMaster.Where(x => x.ID == item.DrugID).Select(x => x.UOM).FirstOrDefault()).Select(x => x.id).FirstOrDefault();
                                if (stockTran == null)
                                {

                                    var StockTranDetail = AddOPBill.MedicalPrescriptionIddetails.Where(x => x.DrugID == item.DrugID).FirstOrDefault();

                                    var StkTran = new StockTran();
                                    StkTran.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                                    StkTran.SMID = stockmasterIdentitys;
                                    StkTran.ItemID = Convert.ToInt32(item.DrugID);
                                    var Qty = WYNKContext.StockTran.Where(x => x.SMID == stockmasterIdentitys && x.ItemID == Convert.ToInt32(item.DrugID)).Select(x => x.ItemQty).FirstOrDefault();
                                    StkTran.ItemQty = Qty != null ? Qty + 1 : 1;
                                    StkTran.UOMID = uommaster.Where(UOM => UOM.Description == drugMaster.Where(x => x.ID == item.DrugID).Select(x => x.UOM).FirstOrDefault()).Select(UOM => UOM.id).FirstOrDefault();
                                    StkTran.ItemRate = drugMaster.Where(x => x.ID == Convert.ToInt32(item.DrugID)).Select(x => x.Rate).FirstOrDefault();
                                    StkTran.ItemValue = StkTran.ItemQty * StkTran.ItemRate;
                                    StkTran.CreatedUTC = DateTime.UtcNow;
                                    StkTran.CreatedBy = AddOPBill.CreatedBy;
                                    WYNKContext.StockTran.AddRange(StkTran);
                                    WYNKContext.SaveChanges();

                                    var stockMas = WYNKContext.StockMaster.Where(x => x.RandomUniqueID == stockmasterIdentitys).FirstOrDefault();
                                    stockMas.GrossProductValue = stockMas.GrossProductValue != null ? (stockMas.GrossProductValue + StockTranDetail.GrossAmount) : 0 + StockTranDetail.GrossAmount;
                                    stockMas.TotalDiscountValue = stockMas.TotalDiscountValue != null ? (stockMas.TotalDiscountValue + StockTranDetail.DiscountAmount) : 0 + StockTranDetail.DiscountAmount;
                                    stockMas.TotalTaxValue = stockMas.TotalTaxValue != null ? (stockMas.TotalTaxValue + StockTranDetail.GSTValue + StockTranDetail.CessValue + StockTranDetail.AddCessValue) : 0 + StockTranDetail.GSTValue + StockTranDetail.CessValue + StockTranDetail.AddCessValue;
                                    stockMas.TotalCGSTTaxValue = stockMas.TotalCGSTTaxValue != null ? (stockMas.TotalCGSTTaxValue + StockTranDetail.GSTValue / 2 + StockTranDetail.CessValue / 2 + StockTranDetail.AddCessValue / 2) : 0 + StockTranDetail.GSTValue / 2 + StockTranDetail.CessValue / 2 + StockTranDetail.AddCessValue / 2;
                                    stockMas.TotalSGSTTaxValue = stockMas.TotalSGSTTaxValue != null ? (stockMas.TotalSGSTTaxValue + StockTranDetail.GSTValue / 2 + StockTranDetail.CessValue / 2 + StockTranDetail.AddCessValue / 2) : 0 + StockTranDetail.GSTValue / 2 + StockTranDetail.CessValue / 2 + StockTranDetail.AddCessValue / 2;
                                    stockMas.TotalPOValue = stockMas.TotalPOValue != null ? (stockMas.TotalPOValue + StockTranDetail.TotalCost) : 0 + StockTranDetail.TotalCost;
                                    stockMas.TotalCESSValue = stockMas.TotalCESSValue != null ? (stockMas.TotalCESSValue + StockTranDetail.CessValue) : 0 + StockTranDetail.CessValue;
                                    stockMas.TotalAdditionalCESSValue = stockMas.TotalAdditionalCESSValue != null ? (stockMas.TotalAdditionalCESSValue + StockTranDetail.AddCessValue) : 0 + StockTranDetail.AddCessValue;
                                    WYNKContext.StockMaster.UpdateRange(stockMas);
                                    WYNKContext.SaveChanges();
                                }
                                else
                                {
                                    stockTran.ItemQty = stockTran.ItemQty + 1;
                                    WYNKContext.StockTran.UpdateRange(stockTran);
                                    WYNKContext.SaveChanges();
                                }
                            }
                            foreach (var Serial in AllotingSerial.ToList())
                            {

                              
                                var ItemSerial = WYNKContext.ItemSerial.Where(x => x.ItemID == Serial.DrugID && x.SerialNo == Serial.SerialNo && x.GRNNo == Serial.BillNo && x.ExpiryDate == Serial.ExpiryDate && x.StoreID == AddOPBill.StoreID && x.cmpID == AddOPBill.CmpId).FirstOrDefault();
                                ItemSerial.IssueDate = Date;
                                ItemSerial.IssueNo = AddOPBill.MedicalBillMaster.BillNo;
                                ItemSerial.IssueTC = AddOPBill.Tc;
                                WYNKContext.ItemSerial.UpdateRange(ItemSerial);
                                WYNKContext.SaveChanges();
                               
                            }

                            WYNKContext.SaveChanges();
                        }
                        if (AllotingOtherDrugs.Count > 0)
                        {
                            var Date = DateTime.Now;
                            var CurrentMonth = Date.Month;

                            foreach (var item in AllotingOtherDrugs.ToList())
                            {
                                var ItemBalance = WYNKContext.ItemBalance.Where(x => x.FYear == Convert.ToInt32(Fyear) && x.ItemID == item.DrugID && x.StoreID == AddOPBill.StoreID && x.CmpID == AddOPBill.CmpId).FirstOrDefault();
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

                                var stockTran = WYNKContext.StockTran.Where(x => x.SMID == stockmasterIdentitys && x.ItemID == item.DrugID).FirstOrDefault();
                                var Uomlist = CMPSContext.uommaster.ToList();
                                var Uom = Uomlist.Where(u => u.Description == WYNKContext.DrugMaster.Where(x => x.ID == item.DrugID).Select(x => x.UOM).FirstOrDefault()).Select(x => x.id).FirstOrDefault();

                                if (stockTran == null)
                                {
                                    var StockTranDetail = AddOPBill.MedicalPrescriptionIddetails.Where(x => x.DrugID == item.DrugID).FirstOrDefault();

                                    var stockTrans = AddBilling.AddstockTran(StockTranDetail, stockmasterIdentitys, AddOPBill.CreatedBy);
                                    WYNKContext.StockTran.Add(stockTrans);

                                    var stockMas = WYNKContext.StockMaster.Where(x => x.RandomUniqueID == stockmasterIdentitys).FirstOrDefault();
                                    WYNKContext.StockMaster.UpdateRange(AddBilling.UpdateStockMaster(StockTranDetail, stockMas));
                                    WYNKContext.SaveChanges();
                                }
                                else
                                {
                                    stockTran.ItemQty = stockTran.ItemQty + item.QtyTobeTaken;
                                    WYNKContext.StockTran.UpdateRange(stockTran);

                                    WYNKContext.SaveChanges();
                                }
                            }
                            WYNKContext.SaveChanges();
                        }

                        var MedicalBillMaster = WYNKContext.MedicalBillMaster.Where(x => x.RegistrationTranID == REGTranId && x.UIN == PatientUIN).FirstOrDefault();
                        DateTime MedicalBillDate = DateTime.Now;

                        var MedicalBillIdentity = 0;

                        if (MedicalBillMaster == null)
                        {
                            var medicalbill = new MedicalBill_Master();
                            medicalbill.CMPID = AddOPBill.CmpId;
                            medicalbill.CreatedBy = AddOPBill.CreatedBy;
                            var billtypes = CMPSContext.Setup.Where(x => x.CMPID == AddOPBill.CmpId).Select(x => x.BillingType).FirstOrDefault();
                            if (billtypes != "Consolidated Billing")
                            {
                                medicalbill.BillNo = AddOPBill.MedicalBillMaster.BillNo;
                            }
                            else {
                                medicalbill.BillNo = "999";
                            }

                            medicalbill.UIN = AddOPBill.MedicalBillMaster.UIN;
                            medicalbill.RegistrationTranID = AddOPBill.MedicalBillMaster.RegistrationTranID;
                            medicalbill.BillDate = GivenDate;
                            medicalbill.CreatedUTC = DateTime.UtcNow;
                            medicalbill.TransactionId = AddOPBill.Tc;
                            medicalbill.Status = true;
                            medicalbill.Fyear = Convert.ToString(WYNKContext.FinancialYear.Where(x => x.ID == Convert.ToInt32(Fyear)).Select(c => c.FYAccYear).FirstOrDefault());
                            WYNKContext.MedicalBillMaster.Add(medicalbill);
                            WYNKContext.SaveChanges();

                            MedicalBillNo = medicalbill.BillNo;
                            MedicalBillDate = Convert.ToDateTime(medicalbill.CreatedUTC);
                            int MedicalBillMasterID = medicalbill.ID;
                            MedicalBillIdentity = medicalbill.ID;

                            foreach (var item1 in AddOPBill.MedicalPrescriptionIddetails.ToList())
                            {
                                var MedBillTRan = AddBilling.AddMedicalBillTran(item1, medprestran, MedicalBillIdentity, AddOPBill.CmpId, null);
                                WYNKContext.MedicalBillTran.Add(MedBillTRan);
                                WYNKContext.SaveChanges();

                                object MT = MedBillTRan;
                                oErrorLogs.WriteErrorLogArray("MedicalBillTran", MT);

                                if (item1.TaxID != null)
                                {

                                    var taxidd = CMPSContext.TaxMaster.Where(x => x.ID == item1.TaxID).ToList();
                                    var taxgrp = CMPSContext.TaxMaster.Where(x => x.ID == item1.TaxID).Select(x => x.TaxGroupId).FirstOrDefault();

                                    if (taxidd != null && taxidd.Count != 0)
                                    {
                                        taxidd.All(x => { x.istransact = false; return true; });
                                        CMPSContext.TaxMaster.UpdateRange(taxidd);
                                        CMPSContext.SaveChanges();
                                    }

                                    var now = DateTime.UtcNow;
                                    var first = new DateTime(now.Year, now.Month, 1);
                                    var last = first.AddMonths(1).AddDays(-1);
                                    var billtype1 = CMPSContext.Setup.Where(x => x.CMPID == AddOPBill.CmpId).Select(x => x.BillingType).FirstOrDefault();
                                    var taid = (from TS in WYNKContext.TaxSummary.Where(x => x.TransactionDate == last && x.CMPID == AddOPBill.CmpId && x.BillingType == "B" && x.TaxID == item1.TaxID)
                                                select new
                                                {
                                                    ret = TS.TaxID,
                                                }).ToList();

                                    if (billtype1 != "Consolidated Billing")
                                    {
                                        if (taid.Count == 0)
                                        {
                                            var tax = new Tax_Summary();
                                            tax.TaxID = item1.TaxID;
                                            tax.CMPID = AddOPBill.CmpId;
                                            tax.BillingType = "B";
                                            tax.TransactionDate = last;
                                            tax.TaxGroupID = taxgrp;
                                            tax.GrossAmount = (item1.UnitPrice * item1.Quantity) - item1.DiscountAmount;
                                            tax.TaxDescription = item1.TaxDescription;
                                            tax.CESSDescription = item1.CessDescription;
                                            tax.AddlCESSDescription = item1.AddCessDescription;
                                            tax.TaxPercentage = Convert.ToInt16(item1.GST);
                                            tax.TaxValue = item1.GSTValue;
                                            tax.Tax1Percentage = Convert.ToInt16(item1.GST / 2);
                                            tax.Tax1Value = item1.GSTValue / 2;
                                            tax.Tax2Percentage = Convert.ToInt16(item1.GST / 2);
                                            tax.Tax2Value = item1.GSTValue / 2;
                                            tax.CESSPercentage = item1.Cess != null ? item1.Cess : 0;
                                            tax.CESSValue = item1.Cess != null ? item1.CessValue : 0;
                                            tax.AddlCESSPercentage = item1.AddCess != null ? item1.AddCess : 0;
                                            tax.AddlCESSValue = item1.AddCess != null ? item1.AddCessValue : 0;
                                            tax.CreatedUTC = DateTime.UtcNow;
                                            tax.CreatedBy = AddOPBill.CreatedBy;
                                            WYNKContext.TaxSummary.AddRange(tax);
                                            WYNKContext.SaveChanges();
                                        }
                                        else
                                        {

                                            var masters = WYNKContext.TaxSummary.Where(x => x.TransactionDate == last && x.CMPID == AddOPBill.CmpId && x.BillingType == "B" && x.TaxID == item1.TaxID).LastOrDefault();
                                            masters.GrossAmount += (item1.UnitPrice * item1.Quantity) - item1.DiscountAmount;
                                            masters.TaxValue += item1.GSTValue;
                                            masters.Tax1Value += item1.GSTValue / 2;
                                            masters.Tax2Value += item1.GSTValue / 2;
                                            masters.CESSValue += item1.Cess != null ? item1.CessValue : 0;
                                            masters.AddlCESSValue += item1.AddCess != null ? item1.AddCessValue : 0;
                                            masters.UpdatedUTC = DateTime.UtcNow;
                                            masters.UpdatedBy = AddOPBill.CreatedBy;
                                            WYNKContext.TaxSummary.UpdateRange(masters);

                                        }
                                    }
                                }



                                WYNKContext.SaveChanges();
                                var medicalbills = WYNKContext.MedicalBillMaster.Where(x => x.ID == MedicalBillIdentity).FirstOrDefault();
                                medicalbills = AddBilling.UpdateMedicalBill_Master(item1, medicalbills);
                                WYNKContext.MedicalBillMaster.UpdateRange(medicalbills);
                                WYNKContext.SaveChanges();

                                var IsPatientCurrentMedication = WYNKContext.PatientCurrentMedication.Where(x => x.GenericDrugDescription == WYNKContext.DrugMaster.Where(y => y.ID == item1.DrugID && y.Cmpid == AddOPBill.CmpId).Select(y => y.Brand).FirstOrDefault() && x.ProgressStatusID == 1 && x.UIN == AddOPBill.MedicalBillMaster.UIN).FirstOrDefault();
                                if (IsPatientCurrentMedication == null)
                                {
                                    var PatCurMedication = new PatientCurrentMedication();
                                    PatCurMedication.GenericDrugDescription = WYNKContext.DrugMaster.Where(x => x.ID == item1.DrugID && x.Cmpid == AddOPBill.CmpId).Select(x => x.Brand).FirstOrDefault();
                                    PatCurMedication.RegistrationTranID = WYNKContext.RegistrationTran.OrderByDescending(x => x.UIN == AddOPBill.MedicalBillMaster.UIN).Select(x => x.RegistrationTranID).FirstOrDefault();
                                    PatCurMedication.UIN = AddOPBill.MedicalBillMaster.UIN;
                                    PatCurMedication.VisitDate = DateTime.Now;
                                    PatCurMedication.Since = DateTime.UtcNow;
                                    PatCurMedication.PrescribedBy = WYNKContext.MedicalPrescription.Where(x => x.RandomUniqueID == AddOPBill.MedicalBillTran.MedicalPrescriptionID).Select(x => x.PrescribedBy).FirstOrDefault();
                                    PatCurMedication.PrescribedByDoctorName = WYNKContext.MedicalPrescription.Where(x => x.RandomUniqueID == AddOPBill.MedicalBillTran.MedicalPrescriptionID).Select(x => x.PrescribedByName).FirstOrDefault();
                                    PatCurMedication.ProgressStatusID = 1;
                                    PatCurMedication.Cmpid = AddOPBill.CmpId;
                                    WYNKContext.PatientCurrentMedication.Add(PatCurMedication);
                                }
                                WYNKContext.SaveChanges();
                            }
                        }
                        else
                        {

                            foreach (var item1 in AddOPBill.MedicalPrescriptionIddetails.ToList())
                            {
                                var MedBillTRan = AddBilling.AddMedicalBillTran(item1, medprestran, MedicalBillMaster.ID, AddOPBill.CmpId, null);
                                WYNKContext.MedicalBillTran.Add(MedBillTRan);

                                WYNKContext.SaveChanges();

                                if (item1.TaxID != null)
                                {

                                    var taxidd = CMPSContext.TaxMaster.Where(x => x.ID == item1.TaxID).ToList();
                                    var taxgrp = CMPSContext.TaxMaster.Where(x => x.ID == item1.TaxID).Select(x => x.TaxGroupId).FirstOrDefault();

                                    if (taxidd != null && taxidd.Count != 0)
                                    {
                                        taxidd.All(x => { x.istransact = false; return true; });
                                        CMPSContext.TaxMaster.UpdateRange(taxidd);
                                        CMPSContext.SaveChanges();
                                    }

                                    var now = DateTime.UtcNow;
                                    var first = new DateTime(now.Year, now.Month, 1);
                                    var last = first.AddMonths(1).AddDays(-1);

                                    var taid = (from TS in WYNKContext.TaxSummary.Where(x => x.TransactionDate == last && x.CMPID == AddOPBill.CmpId && x.BillingType == "B" && x.TaxID == item1.TaxID)
                                                select new
                                                {
                                                    ret = TS.TaxID,
                                                }).ToList();

                                    if (taid.Count == 0)
                                    {
                                        var tax = new Tax_Summary();
                                        tax.TaxID = item1.TaxID;
                                        tax.CMPID = AddOPBill.CmpId;
                                        tax.BillingType = "B";
                                        tax.TransactionDate = last;
                                        tax.TaxGroupID = taxgrp;
                                        tax.GrossAmount = (item1.UnitPrice * item1.Quantity) - item1.DiscountAmount;
                                        tax.TaxDescription = item1.TaxDescription;
                                        tax.CESSDescription = item1.CessDescription;
                                        tax.AddlCESSDescription = item1.AddCessDescription;
                                        tax.TaxPercentage = Convert.ToInt16(item1.GST);
                                        tax.TaxValue = item1.GSTValue;
                                        tax.Tax1Percentage = Convert.ToInt16(item1.GST / 2);
                                        tax.Tax1Value = item1.GSTValue / 2;
                                        tax.Tax2Percentage = Convert.ToInt16(item1.GST / 2);
                                        tax.Tax2Value = item1.GSTValue / 2;
                                        tax.CESSPercentage = item1.Cess != null ? item1.Cess : 0;
                                        tax.CESSValue = item1.Cess != null ? item1.CessValue : 0;
                                        tax.AddlCESSPercentage = item1.AddCess != null ? item1.AddCess : 0;
                                        tax.AddlCESSValue = item1.AddCess != null ? item1.AddCessValue : 0;
                                        tax.CreatedUTC = DateTime.UtcNow;
                                        tax.CreatedBy = AddOPBill.CreatedBy;
                                        WYNKContext.TaxSummary.AddRange(tax);
                                        WYNKContext.SaveChanges();
                                    }
                                    else
                                    {

                                        var masters = WYNKContext.TaxSummary.Where(x => x.TransactionDate == last && x.CMPID == AddOPBill.CmpId && x.BillingType == "B" && x.TaxID == item1.TaxID).LastOrDefault();
                                        masters.GrossAmount += (item1.UnitPrice * item1.Quantity) - item1.DiscountAmount;
                                        masters.TaxValue += item1.GSTValue;
                                        masters.Tax1Value += item1.GSTValue / 2;
                                        masters.Tax2Value += item1.GSTValue / 2;
                                        masters.CESSValue += item1.Cess != null ? item1.CessValue : 0;
                                        masters.AddlCESSValue += item1.AddCess != null ? item1.AddCessValue : 0;
                                        masters.UpdatedUTC = DateTime.UtcNow;
                                        masters.UpdatedBy = AddOPBill.CreatedBy;
                                        WYNKContext.TaxSummary.UpdateRange(masters);

                                    }

                                }

                                var medicalbills = WYNKContext.MedicalBillMaster.Where(x => x.ID == MedicalBillMaster.ID).FirstOrDefault();
                                medicalbills = AddBilling.UpdateMedicalBill_Master(item1, medicalbills);
                                WYNKContext.MedicalBillMaster.UpdateRange(medicalbills);
                                WYNKContext.SaveChanges();
                            }
                        }

                        /*Payment Master*/
                        var billtype2 = CMPSContext.Setup.Where(x => x.CMPID == AddOPBill.CmpId).Select(x => x.BillingType).FirstOrDefault();
                        if (billtype2 != "Consolidated Billing")
                        {
                            if (AddOPBill.paymenttrans.Count > 0)
                            {
                                var MedicalBillID = WYNKContext.MedicalBillTran.Where(x => x.MedicalPrescriptionID == medprestran).Select(x => x.MedicalBillID).FirstOrDefault();
                                AddOPBill.PrintTotal = Convert.ToInt32(WYNKContext.MedicalBillMaster.Where(x => x.ID == MedicalBillID).Select(x => x.TotalBillValue).FirstOrDefault());
                                foreach (var item1 in AddOPBill.paymenttrans.ToList())
                                {
                                    var payment = new Payment_Master();
                                    payment.UIN = PatientUIN;
                                    payment.PaymentType = "O";
                                    payment.PaymentMode = item1.PaymentMode;
                                    payment.OLMID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == item1.PaymentMode).Select(x => x.OLMID).FirstOrDefault();
                                    payment.Amount = item1.Amount;
                                    payment.BankBranch = item1.BankBranch;
                                    payment.BankName = item1.BankName;
                                    payment.Fyear = Convert.ToString(WYNKContext.FinancialYear.Where(x => x.ID == WYNKContext.FinancialYear.Where(b => Convert.ToDateTime(b.FYFrom) <= DateTime.Now && Convert.ToDateTime(b.FYTo) >= DateTime.Now && x.CMPID == AddOPBill.CmpId && x.IsActive == true).Select(f => f.ID).FirstOrDefault()).Select(c => c.FYAccYear).FirstOrDefault());
                                    payment.InVoiceNumber = AddOPBill.Status != "Partially Closed" ? AddOPBill.MedicalBillMaster.BillNo : WYNKContext.PaymentMaster.Where(x => x.PaymentReferenceID == AddOPBill.MedicalBillID).Select(x => x.InVoiceNumber).FirstOrDefault();
                                    payment.InVoiceDate = DateTime.UtcNow;
                                    payment.ReceiptNumber = AddOPBill.ReceiptRunningNo;
                                    payment.ReceiptDate = DateTime.UtcNow;
                                    payment.InstrumentNumber = item1.InstrumentNumber;
                                    if (item1.Instrumentdate != null)
                                    {
                                        payment.Instrumentdate = Convert.ToDateTime(item1.Instrumentdate).AddDays(1);
                                    }
                                    else
                                    {
                                        payment.Instrumentdate = null;
                                    }
                                    if (item1.Expirydate != null)
                                    {
                                        payment.Expirydate = Convert.ToDateTime(item1.Expirydate).AddDays(1);
                                    }
                                    else
                                    {
                                        payment.Expirydate = null;
                                    }
                                    payment.PaymentReferenceID = MedicalBillID;
                                    payment.TransactionID = AddOPBill.Tc;
                                    payment.CmpID = AddOPBill.CmpId;
                                    payment.IsBilled = true;
                                    payment.CreatedUTC = DateTime.UtcNow;
                                    payment.CreatedBy = AddOPBill.CreatedBy;
                                    WYNKContext.PaymentMaster.Add(payment);
                                }

                                var res1 = AddOPBill.MedicalPrescriptionIddetails.Sum(x => x.TotalCost);
                                var res5 = AddOPBill.paymenttrans.Sum(x => x.Amount);

                                if (AddOPBill.paymenttrans.Sum(x => x.Amount) < AddOPBill.MedicalPrescriptionIddetails.Sum(x => x.TotalCost))
                                {

                                    var MedBillMast = WYNKContext.MedicalBillMaster.Where(x => x.ID == MedicalBillIdentity && x.CMPID == AddOPBill.CmpId).FirstOrDefault();
                                    MedBillMast.BillType = "Credit";
                                    MedBillMast.CreditStatus = true;
                                    MedBillMast.AmountCollected = AddOPBill.paymenttrans.Sum(x => x.Amount);
                                    WYNKContext.MedicalBillMaster.UpdateRange(MedBillMast);
                                }
                                else
                                {
                                    var MedBillMast = WYNKContext.MedicalBillMaster.Where(x => x.ID == MedicalBillIdentity && x.CMPID == AddOPBill.CmpId).FirstOrDefault();
                                    MedBillMast.BillType = "Normal";
                                    WYNKContext.MedicalBillMaster.UpdateRange(MedBillMast);
                                }

                                WYNKContext.SaveChanges();
                            }
                            else
                            {

                                var MedBillMast = WYNKContext.MedicalBillMaster.Where(x => x.ID == MedicalBillIdentity && x.CMPID == AddOPBill.CmpId).FirstOrDefault();
                                MedBillMast.BillType = "Credit";
                                MedBillMast.CreditStatus = true;
                                WYNKContext.MedicalBillMaster.UpdateRange(MedBillMast);
                                WYNKContext.SaveChanges();
                            }
                        }


                        var billtype = CMPSContext.Setup.Where(x => x.CMPID == AddOPBill.CmpId).Select(x => x.BillingType).FirstOrDefault();
                        if (billtype == "Consolidated Billing")
                        {
                            var invespa = new PatientAccount();
                            foreach (var item1 in AddOPBill.MedicalPrescriptionIddetails.ToList())
                            {
                              
                                var respa = (from pa in WYNKContext.PatientAccount.Where(x => x.UIN == PatientUIN && x.InvoiceNumber == null && x.CMPID == AddOPBill.CmpId && x.TransactionID == AddOPBill.billtranid)
                                             select new
                                             {
                                                 ret = pa.UIN.Count(),

                                             }).ToList();

                                var PAID = WYNKContext.PatientAccount.Where(x => x.UIN == PatientUIN && x.InvoiceNumber == null && x.CMPID == AddOPBill.CmpId && x.TransactionID == AddOPBill.billtranid).Select(x => x.PAID).LastOrDefault();
                                var ServiceTypeID = WYNKContext.Services.Where(x => x.Tag == "MED" && x.CMPID == AddOPBill.CmpId).Select(x => x.ID).FirstOrDefault();
                                var ServiceDesc = WYNKContext.Services.Where(x => x.Tag == "MED" && x.CMPID == AddOPBill.CmpId).Select(x => x.Description).FirstOrDefault();
                                if (respa.Count == 0)
                                {

                                    invespa.CMPID = AddOPBill.CmpId;
                                    invespa.UIN = PatientUIN;
                                    invespa.TotalProductValue = item1.GrossAmount;
                                    invespa.TotalDiscountValue = item1.DiscountAmount;
                                    invespa.TotalTaxValue = item1.GSTValue;
                                    invespa.TotalCGSTTaxValue = Convert.ToInt16(item1.GST / 2);
                                    invespa.TotalSGSTTaxValue = Convert.ToInt16(item1.GST / 2);
                                    invespa.TotalBillValue = item1.TotalCost;
                                    invespa.CESSValue = item1.Cess != null ? item1.CessValue : 0;
                                    invespa.AdditionalCESSValue = item1.AddCess != null ? item1.AddCessValue : 0;
                                    invespa.CreatedUTC = DateTime.UtcNow;
                                    invespa.CreatedBy = AddOPBill.CreatedBy;
                                    invespa.TransactionID = AddOPBill.billtranid;
                                    WYNKContext.PatientAccount.AddRange(invespa);
                                    WYNKContext.SaveChanges();

                                }

                                else
                                {


                                    var masters1 = WYNKContext.PatientAccount.Where(x => x.UIN == PatientUIN).LastOrDefault();
                                    masters1.TotalProductValue +=  item1.GrossAmount;
                                    masters1.TotalBillValue +=  item1.TotalCost;
                                    masters1.TotalDiscountValue +=  item1.DiscountAmount;
                                    masters1.CESSValue +=  item1.CessValue;
                                    masters1.AdditionalCESSValue +=  item1.AddCessValue;
                                    masters1.TotalTaxValue += item1.GSTValue;
                                    masters1.TotalCGSTTaxValue +=  (item1.GSTValue ) / 2;
                                    masters1.TotalSGSTTaxValue +=  (item1.GSTValue ) / 2;
                                    masters1.UpdatedUTC = DateTime.UtcNow;
                                    masters1.UpdatedBy = AddOPBill.CreatedBy;
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
                                    patactdt.ServiceDescription = ServiceDesc;
                                    patactdt.Description = item1.Drug;
                                    patactdt.TotalProductValue = item1.GrossAmount;
                                    patactdt.TotalDiscountValue = item1.DiscountAmount;
                                    patactdt.TotalTaxValue = item1.GSTValue;
                                    patactdt.TotalCGSTTaxValue = Convert.ToInt16(item1.GSTValue / 2);
                                    patactdt.TotalSGSTTaxValue = Convert.ToInt16(item1.GSTValue / 2);
                                    patactdt.TotalBillValue = item1.TotalCost;
                                    patactdt.CESSValue = item1.Cess != null ? item1.CessValue : 0;
                                    patactdt.AdditonalCESSValue = item1.AddCess != null ? item1.AddCessValue : 0;
                                    patactdt.CreatedUTC = DateTime.UtcNow;
                                    patactdt.CreatedBy = AddOPBill.CreatedBy;
                                    WYNKContext.PatientAccountDetail.AddRange(patactdt);
                                    WYNKContext.SaveChanges();
                                }
                                else
                                {

                                    var masters = WYNKContext.PatientAccountDetail.Where(x => x.PAID == PAID && x.ServiceTypeID == ServiceTypeID).LastOrDefault();
                                    masters.Description += ";"+item1.Drug;
                                    masters.TotalProductValue +=  item1.GrossAmount;
                                    masters.TotalBillValue +=  item1.TotalCost;
                                    masters.TotalDiscountValue +=  item1.DiscountAmount;
                                    masters.CESSValue +=  item1.CessValue;
                                    masters.AdditonalCESSValue += item1.AddCessValue;
                                    masters.TotalTaxValue +=  item1.GSTValue + item1.CessValue + item1.AddCessValue;
                                    masters.TotalCGSTTaxValue +=  (item1.GSTValue + item1.CessValue + item1.AddCessValue) / 2;
                                    masters.TotalSGSTTaxValue +=  (item1.GSTValue + item1.CessValue + item1.AddCessValue) / 2;
                                    masters.UpdatedUTC = DateTime.UtcNow;
                                    masters.UpdatedBy = AddOPBill.CreatedBy;
                                    WYNKContext.PatientAccountDetail.UpdateRange(masters);

                                }
                                var PAccDetailID = WYNKContext.PatientAccountDetail.Where(x => x.PAID == PAID && x.ServiceTypeID == ServiceTypeID).Select(x => x.PAccDetailID).LastOrDefault();

                                var paid = invespa.PAID;
                                var patactdttx = new PatientAccountDetailTax();
                                patactdttx.PAccDetailID = PAccDetailID;
                                patactdttx.ServiceTypeID = ServiceTypeID;
                                patactdttx.TaxPercentage = 0;
                                patactdttx.Description = item1.Drug;
                                patactdttx.Quantity = item1.Quantity;
                                patactdttx.TaxID = item1.TaxID;
                                patactdttx.TotalProductValue = item1.GrossAmount;
                                patactdttx.TotalDiscountValue = item1.DiscountAmount;
                                patactdttx.TotalTaxValue = item1.GSTValue;
                                patactdttx.TotalCGSTTaxValue = Convert.ToInt16(item1.GSTValue / 2);
                                patactdttx.TotalSGSTTaxValue = Convert.ToInt16(item1.GSTValue / 2);
                                patactdttx.TotalValue = item1.TotalCost;
                                patactdttx.CESSValue = item1.Cess != null ? item1.CessValue : 0;
                                patactdttx.AdditionalCESSValue = item1.AddCess != null ? item1.AddCessValue : 0;
                                patactdttx.CreatedUTC = DateTime.UtcNow;
                                patactdttx.CreatedBy = AddOPBill.CreatedBy;

                                WYNKContext.PatientAccountDetailTax.AddRange(patactdttx);


                                WYNKContext.SaveChanges();

                            }
                        }
                       

                        var cmb = WYNKContext.MedicalPrescription.Where(x => x.RandomUniqueID == medprestran).ToList();
                        cmb.All(x =>
                        {
                            x.Status = "Closed";
                            return true;
                        });
                        WYNKContext.MedicalPrescription.UpdateRange(cmb);

                        WYNKContext.SaveChanges();

                        var commonRepos = new CommonRepository(_Wynkcontext, _Cmpscontext);
                        var RunningNumber = commonRepos.GenerateRunningCtrlNoo(AddOPBill.Tc, AddOPBill.CmpId, "GetRunningNo");

                        if (RunningNumber == AddOPBill.MedicalBillMaster.BillNo)
                        {
                            commonRepos.GenerateRunningCtrlNoo(AddOPBill.Tc, AddOPBill.CmpId, "UpdateRunningNo");

                        }
                        else
                        {
                            var GetRunningNumber = commonRepos.GenerateRunningCtrlNoo(AddOPBill.Tc, AddOPBill.CmpId, "UpdateRunningNo");

                            var MedicalBillMasters = WYNKContext.MedicalBillMaster.Where(x => x.BillNo == MedicalBillNo).FirstOrDefault();
                            MedicalBillMasters.BillNo = GetRunningNumber;
                            WYNKContext.MedicalBillMaster.UpdateRange(MedicalBillMasters);

                            var StockMasters = WYNKContext.StockMaster.Where(x => x.RandomUniqueID == stockmasterIdentitys && x.TransactionID == AddOPBill.Tc).FirstOrDefault();
                            StockMasters.DocumentNumber = GetRunningNumber;
                            WYNKContext.StockMaster.UpdateRange(StockMasters);

                            if (AddOPBill.paymenttrans.Count > 0)
                            {
                                var payments = WYNKContext.PaymentMaster.Where(x => x.InVoiceNumber == MedicalBillNo && x.TransactionID == AddOPBill.Tc).ToList();
                                payments.All(x => { x.InVoiceNumber = GetRunningNumber; return true; });
                                WYNKContext.PaymentMaster.UpdateRange(payments);
                            }
                        }

                        if (AddOPBill.paymenttrans.Count > 0)
                        {
                            var RecContraID = commonRepos.GettingReceiptTcID(AddOPBill.Tc, AddOPBill.CmpId);
                            var ReceiptRunningNumber = commonRepos.GenerateRunningCtrlNoo(Convert.ToInt32(RecContraID), AddOPBill.CmpId, "GetRunningNo");

                            if (ReceiptRunningNumber == AddOPBill.ReceiptRunningNo)
                            {
                                commonRepos.GenerateRunningCtrlNoo(Convert.ToInt32(RecContraID), AddOPBill.CmpId, "UpdateRunningNo");
                            }
                            else
                            {
                                var payments = WYNKContext.PaymentMaster.Where(x => x.ReceiptNumber == AddOPBill.ReceiptRunningNo && x.TransactionID == AddOPBill.Tc).ToList();
                                payments.All(x => { x.ReceiptNumber = ReceiptRunningNumber; return true; });
                                WYNKContext.PaymentMaster.UpdateRange(payments);
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
                            ExpiresAlert = Alerts,
                            PrintTotal = AddOPBill.PrintTotal,
                            DocumentDate= GivenDate + ts,
                            CompanyDetails = CMPSContext.Company.Where(x => x.CmpID == AddOPBill.CmpId).FirstOrDefault(),
                            Stockdetail = WYNKContext.StockMaster.Where(x => x.RandomUniqueID == stockmasterIdentitys).FirstOrDefault(),
                            PaidDetails = AddOPBill.paymenttrans,
                            PaidTotal = Math.Abs((AddOPBill.paymenttrans == null ? 0 : AddOPBill.paymenttrans.Sum(x => x.Amount))),
                            ReceiptRunningNo = AddOPBill.paymenttrans.Count > 0 ? WYNKContext.PaymentMaster.Where(x => x.PaymentReferenceID == MedicalBillIdentity).Select(x => x.ReceiptNumber).FirstOrDefault() : null,
                            ReceiptRunningDate = AddOPBill.paymenttrans.Count > 0 ? WYNKContext.PaymentMaster.Where(x => x.PaymentReferenceID == MedicalBillIdentity).Select(x => x.ReceiptDate + ts).FirstOrDefault() : null,
                        };
                    }
                    else
                    {
                        foreach (var item in AllotingBatchs.ToList())
                        {
                            var itembatch = new ItemBatch();
                            itembatch = WYNKContext.ItemBatch.Where(x => x.ItemBatchNumber == item.itemBatchNo && x.ItemID == item.DrugId && x.StoreID == StoreId && x.ItemBatchExpiry == item.ExpiryDate).FirstOrDefault();
                            itembatch.LockedQuantity = itembatch.LockedQuantity - Convert.ToInt32(item.GoingToIssue);
                            WYNKContext.ItemBatch.UpdateRange(itembatch);
                            WYNKContext.SaveChanges();
                        }
                        return new
                        {
                            Success = false,
                            Message = "Out Of Stock Medicines",
                            OutOfStock = AddOPBill.InSufficientDrugs,
                            OutStockSerials = AddOPBill.InSufficientSerials,
                            OutOtherDrugs = AddOPBill.InSufficientOtherDrugs,
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
                        return new { Success = false, Message = msg, grn = AddOPBill.MedicalBillMaster.BillNo };
                    }
                    else
                    {
                        ErrorLog oErrorLog = new ErrorLog();
                        oErrorLog.WriteErrorLog("Error", ex.Message.ToString());
                        return new { Success = false };
                    }
                }
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


        public IList<SerialDetail> CheckSerials(ICollection<SerialInfo> selectedList, int drugid, int storeID, int cmpid)
        {
            IList<SerialDetail> InSufficientSerials = new List<SerialDetail>();

            foreach (var item in selectedList.ToList())
            {
                var itemSerial = WYNKContext.ItemSerial.Where(x => x.ItemID == drugid && x.StoreID == storeID && x.SerialNo == Convert.ToString(item.SerialNo) && x.GRNNo == item.BillNo && x.ExpiryDate == item.ExpiryDate && x.IssueNo == null && x.IssueDate == null && x.IssueTC == null && x.cmpID == cmpid).FirstOrDefault();

                if (itemSerial == null)
                {
                    InSufficientSerials.Add(new SerialDetail()
                    {
                        Brand = WYNKContext.DrugMaster.Where(x => x.ID == drugid).Select(x => x.Brand).FirstOrDefault(),
                        SerialNo = Convert.ToString(item.SerialNo),
                    });
                }

            }

            return InSufficientSerials;
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

        public dynamic PendingPayments(PendingPayments PendingPayments, int CmpID, string GMT)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    TimeSpan ts = TimeSpan.Parse(GMT);
                    DateTime? ReceiptDate = null;
                    if (PendingPayments.paymenttrans.Count > 0)
                    {
                        foreach (var item1 in PendingPayments.paymenttrans.ToList())
                        {
                            var payment = new Payment_Master();
                            payment.UIN = PendingPayments.PendingPayment.UIN;
                            payment.PaymentType = PendingPayments.PendingPayment.PayRef == "Refund Amount" ? "R" : "O";
                            payment.PaymentMode = item1.PaymentMode;
                            payment.OLMID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == item1.PaymentMode).Select(x => x.OLMID).FirstOrDefault();
                            payment.Fyear = Convert.ToString(WYNKContext.FinancialYear.Where(x => x.ID == WYNKContext.FinancialYear.Where(b => Convert.ToDateTime(b.FYFrom) <= DateTime.Now && Convert.ToDateTime(b.FYTo) >= DateTime.Now && x.CMPID == CmpID && x.IsActive == true).Select(f => f.ID).FirstOrDefault()).Select(c => c.FYAccYear).FirstOrDefault());
                            payment.InVoiceNumber = WYNKContext.PaymentMaster.Where(x => x.PaymentReferenceID == PendingPayments.PendingPayment.MedicalBillID).Select(x => x.InVoiceNumber).FirstOrDefault();
                            payment.InVoiceDate = DateTime.UtcNow;
                            payment.ReceiptNumber = PendingPayments.ReceiptRunningNo;
                            payment.ReceiptDate = DateTime.UtcNow + ts;
                            ReceiptDate = payment.ReceiptDate;
                            payment.Amount = item1.Amount;
                            payment.BankBranch = item1.BankBranch;
                            payment.BankName = item1.BankName;
                            payment.InstrumentNumber = item1.InstrumentNumber;
                            if (item1.Instrumentdate != null)
                            {
                                payment.Instrumentdate = Convert.ToDateTime(item1.Instrumentdate).AddDays(1);
                            }
                            else
                            {
                                payment.Instrumentdate = null;
                            }
                            if (item1.Expirydate != null)
                            {
                                payment.Expirydate = Convert.ToDateTime(item1.Expirydate).AddDays(1);
                            }
                            else
                            {
                                payment.Expirydate = null;
                            }
                            payment.PaymentReferenceID = PendingPayments.PendingPayment.MedicalBillID;
                            payment.TransactionID = PendingPayments.PendingPayment.TC;
                            payment.CmpID = PendingPayments.PendingPayment.CMPID;
                            payment.IsBilled = true;
                            payment.CreatedUTC = DateTime.UtcNow;
                            payment.CreatedBy = PendingPayments.PendingPayment.CreatedBy;
                            WYNKContext.PaymentMaster.Add(payment);
                        }
                        WYNKContext.SaveChanges();

                        var Medicalbill = WYNKContext.MedicalBillMaster.Where(x => x.CMPID == CmpID && x.ID == PendingPayments.PendingPayment.MedicalBillID).FirstOrDefault();

                        if (PendingPayments.PendingPayment.PayRef == "Refund Amount")
                        {
                            Medicalbill.CreditStatus = false;
                        }
                        else
                        {
                            if (PendingPayments.PendingPayment.CreditItemCost > (Medicalbill.AmountCollected != null ? Medicalbill.AmountCollected : 0))
                            {
                                Medicalbill.AmountCollected = Medicalbill.AmountCollected != null ? Medicalbill.AmountCollected + PendingPayments.paymenttrans.Sum(x => x.Amount) : PendingPayments.paymenttrans.Sum(x => x.Amount);

                                if (PendingPayments.PendingPayment.CreditItemCost == Medicalbill.AmountCollected)
                                {
                                    Medicalbill.CreditStatus = false;
                                }
                                else
                                {
                                    Medicalbill.CreditStatus = true;
                                }

                            }
                         
                        }


                        WYNKContext.MedicalBillMaster.UpdateRange(Medicalbill);
                        WYNKContext.SaveChanges();

                        var commonRepos = new CommonRepository(_Wynkcontext, _Cmpscontext);
                        var RunningNumber = commonRepos.GenerateRunningCtrlNoo(PendingPayments.PendingPayment.TC, CmpID, "GetRunningNo");
                        if (RunningNumber == PendingPayments.ReceiptRunningNo)
                        {
                            commonRepos.GenerateRunningCtrlNoo(PendingPayments.PendingPayment.TC, CmpID, "UpdateRunningNo");
                        }
                        else
                        {
                            var GetRunningNumber = commonRepos.GenerateRunningCtrlNoo(PendingPayments.PendingPayment.TC, CmpID, "UpdateRunningNo");
                            if (PendingPayments.paymenttrans.Count > 0)
                            {
                                var payments = WYNKContext.PaymentMaster.Where(x => x.InVoiceNumber == PendingPayments.ReceiptRunningNo && x.TransactionID == PendingPayments.PendingPayment.TC).ToList();
                                payments.All(x => { x.InVoiceNumber = GetRunningNumber; return true; });
                                WYNKContext.PaymentMaster.UpdateRange(payments);
                            }
                        }
                        WYNKContext.SaveChanges();

                        dbContextTransaction.Commit();

                        return new
                        {
                            Success = true,
                            CMPDetails = CMPSContext.Company.Where(x => x.CmpID == CmpID).FirstOrDefault(),
                            ReceiptNumber = PendingPayments.ReceiptRunningNo,
                            ReceiptDate = ReceiptDate,
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
                catch (Exception)
                {

                    return new
                    {
                        Success = false,
                    };
                }

            }
        }

        public dynamic CurrentDateSearchOPR(string SelectedDate, int CmpID, string Status, int ContraTc, string GMT)
        {
            var reg = WYNKContext.Registration.ToList();
            var medicalbill = WYNKContext.MedicalBillMaster.ToList();
            List<MedicalBill_Master> res;
            var storemaster = CMPSContext.Storemasters.ToList();
            TimeSpan ts = TimeSpan.Parse(GMT);


            DateTime DT;
            var appdate = DateTime.TryParseExact(SelectedDate.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DT);
            {
                DT.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            }
            var fdate = DT;


            if (Status == "Billed")
            {
                res = medicalbill.Where(x => (x.CreatedUTC + ts).Date == fdate && x.CMPID == CmpID && x.Status == true && x.TransactionId == ContraTc).ToList();
            }
            else
            {
                res = medicalbill.Where(x => (x.CreatedUTC + ts).Date == fdate && x.CMPID == CmpID && x.Status == false && x.TransactionId == ContraTc).ToList();
            }

            var result = (from med in res
                          join stockmas in WYNKContext.StockMaster on med.BillNo equals stockmas.DocumentNumber
                          where stockmas.CMPID == CmpID
                          select new
                          {
                              BillNo = med.BillNo,
                              BillDate = med.CreatedUTC + ts,
                              UIN = med.UIN,
                              RegistrationTranID = med.RegistrationTranID,
                              PatientName = reg.Where(x => x.UIN == med.UIN).FirstOrDefault().Name + reg.Where(x => x.UIN == med.UIN).FirstOrDefault().MiddleName + reg.Where(x => x.UIN == med.UIN).FirstOrDefault().LastName,
                              Status = med.Status,
                              Gender = reg.Where(x => x.UIN == med.UIN).Select(x => x.Gender).FirstOrDefault(),
                              Age = DateTime.Now.Date.Year - (reg.Where(x => x.UIN == med.UIN).Select(x => x.DateofBirth).FirstOrDefault()).Year,
                              StoreName = storemaster.Where(x => x.StoreID == stockmas.StoreID).Select(x => x.Storename).FirstOrDefault(),
                              StoreId = stockmas.StoreID,
                          }).ToList();

            return new
            {
                Success = true,
                Result = result,
            };

        }
        public dynamic CurrentDateSearchOPRBills(string SelectedDate, int CmpID, string Status, int Tc, string GMT)
        {
            var reg = WYNKContext.Registration.ToList();
            var medicalbill = WYNKContext.MedicalBillMaster.ToList();
            List<MedicalBill_Master> res;
            var storemaster = CMPSContext.Storemasters.ToList();
            TimeSpan ts = TimeSpan.Parse(GMT);


            DateTime DT;
            var appdate = DateTime.TryParseExact(SelectedDate.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DT);
            {
                DT.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            }
            var fdate = DT;



            res = medicalbill.Where(x => (x.CreatedUTC + ts).Date == fdate && x.CMPID == CmpID && x.Status == true && x.TransactionId == Tc).ToList();


            var result = (from med in res
                          join stockmas in WYNKContext.StockMaster on med.BillNo equals stockmas.DocumentNumber
                          where stockmas.CMPID == CmpID
                          select new
                          {
                              BillNo = med.BillNo,
                              BillDate = med.CreatedUTC + ts,
                              UIN = med.UIN,
                              RegistrationTranID = med.RegistrationTranID,
                              PatientName = reg.Where(x => x.UIN == med.UIN).FirstOrDefault().Name + reg.Where(x => x.UIN == med.UIN).FirstOrDefault().MiddleName + reg.Where(x => x.UIN == med.UIN).FirstOrDefault().LastName,
                              Status = med.Status,
                              Gender = reg.Where(x => x.UIN == med.UIN).Select(x => x.Gender).FirstOrDefault(),
                              Age = DateTime.Now.Date.Year - (reg.Where(x => x.UIN == med.UIN).Select(x => x.DateofBirth).FirstOrDefault()).Year,
                              StoreName = storemaster.Where(x => x.StoreID == stockmas.StoreID).Select(x => x.Storename).FirstOrDefault(),
                              StoreId = stockmas.StoreID,
                          }).ToList();

            return new
            {
                Success = true,
                Result = result,
            };

        }
        public dynamic PeriodSearchOPR(string FromDate, string Todate, int CmpID, string Status, int ContraTc, string GMT)
        {
            var reg = WYNKContext.Registration.ToList();
            var medicalbill = WYNKContext.MedicalBillMaster.ToList();
            List<MedicalBill_Master> res;
            TimeSpan ts = TimeSpan.Parse(GMT);
            var storemaster = CMPSContext.Storemasters.ToList();


            DateTime DT;
            var appdate = DateTime.TryParseExact(FromDate.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DT);
            {
                DT.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            }
            var fdate = DT;

            DateTime DT1;
            var appdate1 = DateTime.TryParseExact(Todate.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DT1);
            {
                DT1.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            }
            var tdate = DT1;

            if (Status == "Billed")
            {
                res = medicalbill.Where(x => (x.CreatedUTC + ts).Date >= fdate && (x.CreatedUTC + ts).Date <= tdate && x.CMPID == CmpID && x.Status == true && x.TransactionId == ContraTc).ToList();
            }
            else
            {
                res = medicalbill.Where(x => (x.CreatedUTC + ts).Date >= fdate && (x.CreatedUTC + ts).Date <= tdate && x.CMPID == CmpID && x.Status == false && x.TransactionId == ContraTc).ToList();
            }

            var result = (from med in res
                          join stockmas in WYNKContext.StockMaster on med.BillNo equals stockmas.DocumentNumber
                          where stockmas.CMPID == CmpID
                          select new
                          {
                              BillNo = med.BillNo,
                              BillDate = med.CreatedUTC + ts,
                              UIN = med.UIN,
                              RegistrationTranID = med.RegistrationTranID,
                              PatientName = reg.Where(x => x.UIN == med.UIN).FirstOrDefault().Name + reg.Where(x => x.UIN == med.UIN).FirstOrDefault().MiddleName + reg.Where(x => x.UIN == med.UIN).FirstOrDefault().LastName,
                              Status = med.Status,
                              Gender = reg.Where(x => x.UIN == med.UIN).Select(x => x.Gender).FirstOrDefault(),
                              Age = DateTime.Now.Date.Year - (reg.Where(x => x.UIN == med.UIN).Select(x => x.DateofBirth).FirstOrDefault()).Year,
                              StoreName = storemaster.Where(x => x.StoreID == stockmas.StoreID).Select(x => x.Storename).FirstOrDefault(),
                              StoreId = stockmas.StoreID,
                          }).ToList();
            return new
            {
                Success = true,
                Result = result,
            };
        }
        public dynamic PeriodSearchOPRs(string FromDate, string Todate, int CmpID, string Status, int Tc, string GMT)
        {
            var reg = WYNKContext.Registration.ToList();
            var medicalbill = WYNKContext.MedicalBillMaster.ToList();
            List<MedicalBill_Master> res;
            TimeSpan ts = TimeSpan.Parse(GMT);
            var storemaster = CMPSContext.Storemasters.ToList();

            DateTime DT;
            var appdate = DateTime.TryParseExact(FromDate.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DT);
            {
                DT.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            }
            var fdate = DT;

            DateTime DT1;
            var appdate1 = DateTime.TryParseExact(Todate.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DT1);
            {
                DT1.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            }
            var tdate = DT1;


            res = medicalbill.Where(x => (x.CreatedUTC + ts).Date >= fdate && (x.CreatedUTC + ts).Date <= tdate && x.CMPID == CmpID && x.Status == true && x.TransactionId == Tc).ToList();


            var result = (from med in res
                          join stockmas in WYNKContext.StockMaster on med.BillNo equals stockmas.DocumentNumber
                          where stockmas.CMPID == CmpID
                          select new
                          {
                              BillNo = med.BillNo,
                              BillDate = med.CreatedUTC + ts,
                              UIN = med.UIN,
                              RegistrationTranID = med.RegistrationTranID,
                              PatientName = reg.Where(x => x.UIN == med.UIN).FirstOrDefault().Name + reg.Where(x => x.UIN == med.UIN).FirstOrDefault().MiddleName + reg.Where(x => x.UIN == med.UIN).FirstOrDefault().LastName,
                              Status = med.Status,
                              Gender = reg.Where(x => x.UIN == med.UIN).Select(x => x.Gender).FirstOrDefault(),
                              Age = DateTime.Now.Date.Year - (reg.Where(x => x.UIN == med.UIN).Select(x => x.DateofBirth).FirstOrDefault()).Year,
                              StoreName = storemaster.Where(x => x.StoreID == stockmas.StoreID).Select(x => x.Storename).FirstOrDefault(),
                              StoreId = stockmas.StoreID,
                          }).ToList();
            return new
            {
                Success = true,
                Result = result,
            };
        }
        public dynamic PatientNameSearch(int CmpID, string Status, int ContraTc, string GMT, string Name)
        {
            var storemaster = CMPSContext.Storemasters.ToList();
            TimeSpan ts = TimeSpan.Parse(GMT);

            var result = (from med in WYNKContext.MedicalBillMaster.Where(x => x.CMPID == CmpID && x.Status == true && x.TransactionId == ContraTc)
                          join stockmas in WYNKContext.StockMaster on med.BillNo equals stockmas.DocumentNumber
                          join Regs in WYNKContext.Registration.Where(x => x.CMPID == CmpID) on med.UIN equals Regs.UIN
                          where stockmas.CMPID == CmpID && string.Equals(Regs.Name + (Regs.MiddleName != null ? Regs.MiddleName : "") + Regs.LastName, Name, StringComparison.OrdinalIgnoreCase)
                          select new
                          {
                              BillNo = med.BillNo,
                              BillDate = med.CreatedUTC + ts,
                              UIN = med.UIN,
                              RegistrationTranID = med.RegistrationTranID,
                              PatientName = Regs.Name + (Regs.MiddleName != null ? Regs.MiddleName : "") + Regs.LastName,
                              Status = med.Status,
                              Gender = Regs.Gender,
                              Age = DateTime.Now.Date.Year - Regs.DateofBirth.Year,
                              StoreName = storemaster.Where(x => x.StoreID == stockmas.StoreID).Select(x => x.Storename).FirstOrDefault(),
                              StoreId = stockmas.StoreID,
                          }).ToList();

            return new
            {
                Success = true,
                Result = result,
            };
        }
        public dynamic PatientNameSearchs(int CmpID, string Status, int Tc, string GMT, string Name)
        {
            var storemaster = CMPSContext.Storemasters.ToList();
            TimeSpan ts = TimeSpan.Parse(GMT);

            var result = (from med in WYNKContext.MedicalBillMaster.Where(x => x.CMPID == CmpID && x.Status == true && x.TransactionId == Tc)
                          join stockmas in WYNKContext.StockMaster on med.BillNo equals stockmas.DocumentNumber
                          join Regs in WYNKContext.Registration.Where(x => x.CMPID == CmpID) on med.UIN equals Regs.UIN
                          where stockmas.CMPID == CmpID && string.Equals(Regs.Name + (Regs.MiddleName != null ? Regs.MiddleName : "") + Regs.LastName, Name, StringComparison.OrdinalIgnoreCase)
                          select new
                          {
                              BillNo = med.BillNo,
                              BillDate = med.CreatedUTC + ts,
                              UIN = med.UIN,
                              RegistrationTranID = med.RegistrationTranID,
                              PatientName = Regs.Name + (Regs.MiddleName != null ? Regs.MiddleName : "") + Regs.LastName,
                              Status = med.Status,
                              Gender = Regs.Gender,
                              Age = DateTime.Now.Date.Year - Regs.DateofBirth.Year,
                              StoreName = storemaster.Where(x => x.StoreID == stockmas.StoreID).Select(x => x.Storename).FirstOrDefault(),
                              StoreId = stockmas.StoreID,
                          }).ToList();

            return new
            {
                Success = true,
                Result = result,
            };
        }
        public dynamic BillNoSearch(int CmpID, string Status, int ContraTc, string GMT, string BillNo)
        {
            var storemaster = CMPSContext.Storemasters.ToList();
            TimeSpan ts = TimeSpan.Parse(GMT);

            var result = (from med in WYNKContext.MedicalBillMaster.Where(x => x.CMPID == CmpID && x.Status == (Status == "Billed" ? true : false) && x.TransactionId == ContraTc && x.BillNo == BillNo)
                          join stockmas in WYNKContext.StockMaster on med.BillNo equals stockmas.DocumentNumber
                          join Regs in WYNKContext.Registration.Where(x => x.CMPID == CmpID) on med.UIN equals Regs.UIN
                          where stockmas.CMPID == CmpID
                          select new
                          {
                              BillNo = med.BillNo,
                              BillDate = med.CreatedUTC + ts,
                              UIN = med.UIN,
                              RegistrationTranID = med.RegistrationTranID,
                              PatientName = Regs.Name + (Regs.MiddleName != null ? Regs.MiddleName : " ") + Regs.LastName,
                              Status = med.Status,
                              Gender = Regs.Gender,
                              Age = DateTime.Now.Date.Year - Regs.DateofBirth.Year,
                              StoreName = storemaster.Where(x => x.StoreID == stockmas.StoreID).Select(x => x.Storename).FirstOrDefault(),
                              StoreId = stockmas.StoreID,
                          }).ToList();

            return new
            {
                Success = true,
                Result = result,
            };
        }
        public dynamic BillNoSearchs(int CmpID, string Status, int Tc, string GMT, string BillNo)
        {
            var storemaster = CMPSContext.Storemasters.ToList();
            TimeSpan ts = TimeSpan.Parse(GMT);

            var result = (from med in WYNKContext.MedicalBillMaster.Where(x => x.CMPID == CmpID && x.Status == true && x.TransactionId == Tc && x.BillNo == BillNo)
                          join stockmas in WYNKContext.StockMaster on med.BillNo equals stockmas.DocumentNumber
                          join Regs in WYNKContext.Registration.Where(x => x.CMPID == CmpID) on med.UIN equals Regs.UIN
                          where stockmas.CMPID == CmpID
                          select new
                          {
                              BillNo = med.BillNo,
                              BillDate = med.CreatedUTC + ts,
                              UIN = med.UIN,
                              RegistrationTranID = med.RegistrationTranID,
                              PatientName = Regs.Name + (Regs.MiddleName != null ? Regs.MiddleName : " ") + Regs.LastName,
                              Status = med.Status,
                              Gender = Regs.Gender,
                              Age = DateTime.Now.Date.Year - Regs.DateofBirth.Year,
                              StoreName = storemaster.Where(x => x.StoreID == stockmas.StoreID).Select(x => x.Storename).FirstOrDefault(),
                              StoreId = stockmas.StoreID,
                          }).ToList();

            return new
            {
                Success = true,
                Result = result,
            };
        }
        public dynamic ReturningBillNoDetails(string BillNo, int CmpID, int StoreID)
        {
            try
            {
                var MedicalBill = WYNKContext.MedicalBillMaster.Where(x => x.BillNo == BillNo && x.CMPID == CmpID).FirstOrDefault();
                var taxMaster = CMPSContext.TaxMaster.ToList();
                var MedicalBillMaster = WYNKContext.MedicalBillMaster.Where(x => x.CMPID == CmpID).ToList();
                var MedicalBillTran = WYNKContext.MedicalBillTran.ToList();
                var StockMaster = WYNKContext.StockMaster.Where(x => x.CMPID == CmpID).ToList();
                var drugmaster = WYNKContext.DrugMaster.Where(x => x.Cmpid == CmpID).ToList();

                var ItemDetails = (from MBM in MedicalBillMaster.Where(x => x.BillNo == BillNo && x.CMPID == CmpID)
                                   join MBT in MedicalBillTran on MBM.ID equals MBT.MedicalBillID
                                   join SM in StockMaster on MBM.BillNo equals SM.DocumentNumber
                                   select new
                                   {
                                       ItemId = MBT.DrugID,
                                       Drug = WYNKContext.DrugMaster.Where(x => x.ID == MBT.DrugID && x.Cmpid == CmpID).Select(x => x.Brand).FirstOrDefault(),
                                       UOM = MBT.UOM,
                                       Qty = MBT.Quantity,
                                       AlreadyReturnQty = MBT.ReturnQuantity != null ? MBT.ReturnQuantity : 0,
                                       ReturnQty = 0.0,
                                       UnitPrice = MBT.ItemRate,
                                       Amount = 0.0,
                                       Discount = MBT.DiscountPercentage,
                                       DiscountAmount = 0.0,
                                       GrossAmount = 0.0,
                                       TotalCost = 0.0,
                                       GST = MBT.GSTPercentage,
                                       Cess = MBT.CESSPercentage,
                                       AddCess = MBT.AdditionalCESSPercentage,
                                       GSTValue = 0.0,
                                       CessValue = 0.0,
                                       AddCessValue = 0.0,
                                       TaxID = MBT.TaxID,
                                       TaxDescription = taxMaster.Where(t => t.ID == drugmaster.Where(x => x.ID == MBT.DrugID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.TaxDescription).FirstOrDefault(),
                                       CessDescription = taxMaster.Where(t => t.ID == drugmaster.Where(x => x.ID == MBT.DrugID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.CESSDescription).FirstOrDefault(),
                                       AddCessDescription = taxMaster.Where(t => t.ID == drugmaster.Where(x => x.ID == MBT.DrugID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.AdditionalCESSDescription).FirstOrDefault(),
                                       IsSerial = GetDrugTracker(MBT.DrugID, CmpID),
                                       SerialDetails = Enum.GetName(typeof(TrackingType), WYNKContext.DrugMaster.Where(x => x.ID == MBT.DrugID && x.Cmpid == CmpID).Select(x => x.DrugTracker).FirstOrDefault()) == "SerialNumberBased" ? GetSerialDetails(BillNo, MBT.DrugID, CmpID, StoreID) : null,
                                       BatchDetails = Enum.GetName(typeof(TrackingType), WYNKContext.DrugMaster.Where(x => x.ID == MBT.DrugID && x.Cmpid == CmpID).Select(x => x.DrugTracker).FirstOrDefault()) == "BatchNumberBased" ? GetBatchDetails(SM.RandomUniqueID, MBT.DrugID, CmpID) : null,
                                       MedicalbillTranID = MBT.ID,
                                       MedicalPrescriptionTranId = MBT.MedicalPrescriptionTranId,
                                       MedicalPrescriptionID = MBT.MedicalPrescriptionID,
                                       IsMedicinePrescribed = MBT.IsMedicinePrescribed,
                                       IsReturned = MBT.Status == false ? true : false,
                                   }).ToList();


                var AlreadyPaid = WYNKContext.PaymentMaster.Where(x => x.PaymentReferenceID == MedicalBill.ID && x.PaymentType == "O").Select(x => x.Amount).ToList();
                return new
                {
                    Success = true,
                    ItemDetails = ItemDetails,
                    MedicalBillID = MedicalBill.ID,
                    BillType = MedicalBill.CreditStatus == null ? "Normal" : MedicalBill.CreditStatus == true ? "Credit" : "Normal",
                    AlreadyPaid = AlreadyPaid.Count == 0 ? 0 : AlreadyPaid.Sum(),
                    ActualBillAmount = MedicalBill.TotalBillValue,
                    ReturnedItemAmount = MedicalBill.ReturnedItemAmount == null ? 0 : MedicalBill.ReturnedItemAmount,
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

        public dynamic ClosedReturningBillNoDetails(string BillNo, int CmpID, int StoreID, string GMT)
        {
            try
            {
                var MedicalBill = WYNKContext.MedicalBillMaster.Where(x => x.BillNo == BillNo && x.CMPID == CmpID).FirstOrDefault();
                var taxMaster = CMPSContext.TaxMaster.ToList();
                var MedicalBillMaster = WYNKContext.MedicalBillMaster.ToList();
                var MedicalBillTran = WYNKContext.MedicalBillTran.ToList();
                var StockMaster = WYNKContext.StockMaster.ToList();
                var drugmaster = WYNKContext.DrugMaster.ToList();
                TimeSpan ts = TimeSpan.Parse(GMT);
                var OrgReturnBillDetails = WYNKContext.StockMaster.Where(x => x.SMID == WYNKContext.StockMaster.Where(Z => Z.DocumentNumber == BillNo && x.CMPID == CmpID).Select(Z => Z.ContraSmid).FirstOrDefault()).Select(y => new { OrgBillNo = y.DocumentNumber, Date = y.DocumentDate + ts }).FirstOrDefault();

                List<Payment_Master> paymenttrans = null;

                var ItemDetails = (from MBM in MedicalBillMaster.Where(x => x.BillNo == BillNo && x.CMPID == CmpID)
                                   join MBT in MedicalBillTran on MBM.ID equals MBT.MedicalBillID
                                   join SM in StockMaster on MBM.BillNo equals SM.DocumentNumber
                                   select new
                                   {
                                       ItemId = MBT.DrugID,
                                       Drug = WYNKContext.DrugMaster.Where(x => x.ID == MBT.DrugID && x.Cmpid == CmpID).Select(x => x.Brand).FirstOrDefault(),
                                       UOM = MBT.UOM,
                                       Qty = MBT.Quantity,
                                       AlreadyReturnQty = MBT.ReturnQuantity != null ? MBT.ReturnQuantity : 0,
                                       UnitPrice = MBT.ItemRate,
                                       Amount = MBT.Quantity * MBT.ItemRate,
                                       Discount = MBT.DiscountPercentage,
                                       DiscountAmount = MBT.DiscountAmount,
                                       GrossAmount = (MBT.Quantity * MBT.ItemRate) - MBT.DiscountAmount,
                                       TotalCost = MBT.ItemValue + MBT.GSTTaxValue + MBT.CESSValue + MBT.AdditionalCESSValue,
                                       GST = MBT.GSTPercentage,
                                       Cess = MBT.CESSPercentage,
                                       AddCess = MBT.AdditionalCESSPercentage,
                                       GSTValue = MBT.GSTTaxValue,
                                       CessValue = MBT.CESSValue,
                                       AddCessValue = MBT.AdditionalCESSValue,
                                       TaxID = MBT.TaxID,
                                       TaxDescription = taxMaster.Where(t => t.ID == drugmaster.Where(x => x.ID == MBT.DrugID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.TaxDescription).FirstOrDefault(),
                                       CessDescription = taxMaster.Where(t => t.ID == drugmaster.Where(x => x.ID == MBT.DrugID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.CESSDescription).FirstOrDefault(),
                                       AddCessDescription = taxMaster.Where(t => t.ID == drugmaster.Where(x => x.ID == MBT.DrugID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.AdditionalCESSDescription).FirstOrDefault(),
                                       IsSerial = GetDrugTracker(MBT.DrugID, CmpID),
                                       SerialDetails = Enum.GetName(typeof(TrackingType), WYNKContext.DrugMaster.Where(x => x.ID == MBT.DrugID && x.Cmpid == CmpID).Select(x => x.DrugTracker).FirstOrDefault()) == "SerialNumberBased" ? GetReturnedSerialDetails(BillNo, MBT.DrugID, CmpID, Convert.ToInt32(MedicalBill.TransactionId)) : null,
                                       BatchDetails = Enum.GetName(typeof(TrackingType), WYNKContext.DrugMaster.Where(x => x.ID == MBT.DrugID && x.Cmpid == CmpID).Select(x => x.DrugTracker).FirstOrDefault()) == "BatchNumberBased" ? GetClosedBatchDetails(SM.RandomUniqueID, MBT.DrugID, CmpID, ts) : null,
                                       IsReturned = MBT.Status == false ? true : false,
                                   }).ToList();

                var OnelineMaster = CMPSContext.OneLineMaster.ToList();
               
                var MedicalBillIds = WYNKContext.MedicalBillTran.Where(x => x.MedicalBillID == MedicalBill.ID).Select(x => x.MedicalBillID).ToList();

                paymenttrans = (from payment in WYNKContext.PaymentMaster
                                where payment.InVoiceNumber == BillNo && payment.CmpID == CmpID
                                select new Payment_Master
                                {
                                    PaymentMode = OnelineMaster.Where(x => x.OLMID == payment.OLMID).Select(x => x.ParentDescription).FirstOrDefault(),
                                    InstrumentNumber = payment.InstrumentNumber,
                                    Instrumentdate = payment.Instrumentdate,
                                    BankName = payment.BankName,
                                    BankBranch = payment.BankBranch,
                                    Expirydate = payment.Expirydate,
                                    Amount = payment.Amount,
                                    PaymentReferenceID = payment.PaymentReferenceID,
                                }).ToList();



                return new
                {
                    Success = true,
                    ItemDetails = ItemDetails,
                    MedicalBillID = MedicalBill.ID,
                    OrgReturnBillDetails = OrgReturnBillDetails,
                    paymenttran = paymenttrans,
                    paymentReceiptNo = paymenttrans.Count > 0 ? WYNKContext.PaymentMaster.Where(x => x.PaymentReferenceID == MedicalBill.ID).Select(x => x.ReceiptNumber).FirstOrDefault() : null,
                    paymentReceiptDate = paymenttrans.Count > 0 ? WYNKContext.PaymentMaster.Where(x => x.PaymentReferenceID == MedicalBill.ID).Select(x => x.ReceiptDate + ts).FirstOrDefault() : null,
                    CMPDetails = CMPSContext.Company.Where(x => x.CmpID == CmpID).FirstOrDefault(),
                };
            }
            catch (Exception)
            {
                return new
                {
                    Success = false,
                };
            }
        }

        public Boolean? GetDrugTracker(int drugID, int cmpID)
        {
            bool? IsSerial;
            var res = Enum.GetName(typeof(TrackingType), WYNKContext.DrugMaster.Where(x => x.ID == drugID && x.Cmpid == cmpID).Select(x => x.DrugTracker).FirstOrDefault());
            if (res == "SerialNumberBased")
            {
                IsSerial = true;
            }
            else if (res == "BatchNumberBased")
            {
                IsSerial = false;
            }
            else
            {
                IsSerial = null;
            }
            return IsSerial;
        }

        public object GetSerialDetails(string billNo, int drugID, int cmpID, int StoreID)
        {
            var SerialDetails = (from ItemSerial in WYNKContext.ItemSerial.Where(x => x.IssueNo == billNo && x.ItemID == drugID && x.cmpID == cmpID && x.StoreID == StoreID && x.IsReturned == null)
                                 select new
                                 {
                                     SerialNo = ItemSerial.SerialNo,
                                     ExpiryDate = ItemSerial.ExpiryDate,
                                     BillNo = billNo,
                                 }).ToList();

            return SerialDetails;
        }

        public object GetReturnedSerialDetails(string billNo, int drugID, int cmpID, int Tc)
        {
            var SerialDetails = (from ItemSerial in WYNKContext.ItemSerial.Where(x => x.GRNNo == billNo && x.ItemID == drugID && x.cmpID == cmpID && x.TC == Tc)
                                 select new
                                 {
                                     SerialNo = ItemSerial.SerialNo,
                                     ExpiryDate = ItemSerial.ExpiryDate,
                                 }).ToList();

            return SerialDetails;
        }
        public object GetBatchDetails(string sMID, int drugID, int cmpID)
        {
            var StockTranID = WYNKContext.StockTran.Where(x => x.SMID == sMID && x.ItemID == drugID).Select(x => x.RandomUniqueID).FirstOrDefault();
            var ItemBatchTrans = WYNKContext.ItemBatchTrans.ToList();
            var Batchdetails = (from Itembatch in ItemBatchTrans.Where(x => x.SMID == sMID && x.STID == StockTranID && x.ItemID == drugID && x.cmpID == cmpID)
                                select new
                                {
                                    BatchNo = Itembatch.ItemBatchNumber,
                                    ExpiryDate = Itembatch.ItemBatchExpiry,
                                    IssuedQty = Itembatch.ItemBatchTransactedQty,
                                    ReturnQty = Itembatch.ReturnQty != null ? Itembatch.ReturnQty : 0,
                                    SMID = sMID,
                                    STID = StockTranID,
                                    ItemBatchTransID = Itembatch.RandomUniqueID,
                                    QtyToReturn = 0,
                                    IsReturned = Itembatch.ItemBatchTransactedQty == Itembatch.ReturnQty ? false : true,
                                    ReturnMedicalBillDetails = Itembatch.ReturnQty != null ? GetReturnMedicalBillDetails(Itembatch.RandomUniqueID) : null,
                                }).ToList();

            return Batchdetails;
        }


        public object GetClosedBatchDetails(string sMID, int drugID, int cmpID, TimeSpan ts)
        {
            var ItemBatchTranss = WYNKContext.ItemBatchTrans.Where(x => x.SMID == sMID && x.ItemID == drugID && x.cmpID == cmpID).ToList();
            var ItemBatchTrans = WYNKContext.ItemBatchTrans.ToList();
            var ItemBatchResult = (from Itembatch in ItemBatchTranss.Where(x => x.SMID == sMID && x.ItemID == drugID && x.cmpID == cmpID)
                                   select new
                                   {
                                       BatchNo = Itembatch.ItemBatchNumber,
                                       Qty = Itembatch.ItemBatchTransactedQty,
                                       ExpiryDate = Itembatch.ItemBatchExpiry,
                                   }).ToList();
            return ItemBatchResult;
        }

        public string GetReturnMedicalBillDetails(string ItemBatchTransID)
        {
            var SMIDIds = WYNKContext.ItemBatchTrans.Where(x => x.ContraItemBatchID == ItemBatchTransID).Select(x => x.SMID).ToList();
            var BillNos = (from sm in WYNKContext.StockMaster
                           where SMIDIds.Contains(sm.RandomUniqueID)
                           select new
                           {
                               sm.DocumentNumber,
                           }).ToList();

            var DocumentNumber = string.Join(", ", BillNos.Select(x => x.DocumentNumber).ToList());
            return DocumentNumber;
        }
        public dynamic AddOutPatientBillingReturns(BillingReturns BillingReturnDetails, string GMT)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    var OriginalMedicalBillDetail = WYNKContext.MedicalBillMaster.Where(x => x.BillNo == BillingReturnDetails.BillNo && x.CMPID == BillingReturnDetails.Cmpid && x.UIN == BillingReturnDetails.UIN).FirstOrDefault();
                  
                    var FinancialYearId = WYNKContext.FinancialYear.Where(x => x.FYAccYear == (Convert.ToInt16(BillingReturnDetails.FYear)) && x.CMPID == BillingReturnDetails.Cmpid && x.IsActive == true).Select(x => x.ID).FirstOrDefault();
                    var Fyear = Convert.ToString(WYNKContext.FinancialYear.Where(x => x.ID == Convert.ToInt32(FinancialYearId)).Select(c => c.FYAccYear).FirstOrDefault());
                    TimeSpan ts = TimeSpan.Parse(GMT);
                    var CurrentMonth = DateTime.Now.Month;

                    DateTime GivenDate;
                    var appdate = DateTime.TryParseExact(BillingReturnDetails.GivenDate.Trim(), "dd-MM-yyyy,HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out GivenDate);
                    {
                        GivenDate.ToString("dd-MM-yyyy,HH:mm", CultureInfo.InvariantCulture);
                    }
                    GivenDate = GivenDate - ts;


                    var medicalbill = new MedicalBill_Master();
                    medicalbill.CMPID = BillingReturnDetails.Cmpid;
                    medicalbill.CreatedBy = BillingReturnDetails.CreatedBy;
                    medicalbill.BillNo = BillingReturnDetails.RunningNo;
                    medicalbill.BillDate = GivenDate;
                    medicalbill.UIN = BillingReturnDetails.UIN;
                    medicalbill.RegistrationTranID = OriginalMedicalBillDetail.RegistrationTranID;
                    medicalbill.CreatedUTC = DateTime.UtcNow;
                    medicalbill.TransactionId = BillingReturnDetails.TC;
                    medicalbill.Fyear = Fyear;
                    medicalbill.Status = true;
                    WYNKContext.MedicalBillMaster.Add(medicalbill);

                    var stockmas = AddBilling.AddstockMaster1(BillingReturnDetails.RunningNo, DateTime.UtcNow, BillingReturnDetails.StoreID, null, 0, BillingReturnDetails.TC, CMPSContext.TransactionType.Where(x => x.TransactionID == BillingReturnDetails.TC).Select(x => x.Rec_Issue_type).FirstOrDefault(), BillingReturnDetails.Cmpid, BillingReturnDetails.CreatedBy, Fyear);
                    stockmas.ContraSmid = WYNKContext.StockMaster.Where(x => x.DocumentNumber == BillingReturnDetails.BillNo && x.CMPID == BillingReturnDetails.Cmpid).Select(x => x.SMID).FirstOrDefault();
                    WYNKContext.StockMaster.Add(stockmas);

                    WYNKContext.SaveChanges();

                    var ReturnMedicalBillItemdetails = BillingReturnDetails.ReturnMedicalBillItemdetails.Where(x => x.ReturnQty != 0).ToList();
                    foreach (var item in ReturnMedicalBillItemdetails.ToList())
                    {

                        var UOM = WYNKContext.DrugMaster.Where(x => x.ID == item.ItemId && x.Cmpid == BillingReturnDetails.Cmpid).Select(x => x.UOM).FirstOrDefault();
                        var UOMID = CMPSContext.uommaster.Where(x => x.Description == UOM).Select(x => x.id).FirstOrDefault();

                        /*Insert MedicalBillTran && StockTran Record*/
                        var MedicalbillTran = new MedicalBill_Tran();
                        MedicalbillTran.MedicalBillID = medicalbill.ID;
                        MedicalbillTran.MedicalPrescriptionTranId = 0;
                        MedicalbillTran.DrugID = item.ItemId;
                        MedicalbillTran.Quantity = item.ReturnQty;
                        MedicalbillTran.MedicalPrescriptionID = item.MedicalPrescriptionID;
                        MedicalbillTran.MedicalBillParentID = OriginalMedicalBillDetail.ID;
                        MedicalbillTran.IsMedicinePrescribed = item.IsMedicinePrescribed;
                        MedicalbillTran.UOM = item.UOM;
                        MedicalbillTran.ItemRate = item.UnitPrice;
                        MedicalbillTran.ItemValue = item.Amount;
                        MedicalbillTran.DiscountPercentage = item.Discount;
                        MedicalbillTran.DiscountAmount = item.DiscountAmount;
                        MedicalbillTran.GSTPercentage = item.GST;
                        MedicalbillTran.GSTTaxValue = item.GSTValue;
                        MedicalbillTran.CGSTPercentage = item.GST / 2;
                        MedicalbillTran.CGSTTaxValue = item.GSTValue / 2;
                        MedicalbillTran.SGSTPercentage = item.GST / 2;
                        MedicalbillTran.SGSTTaxValue = item.GSTValue / 2;
                        MedicalbillTran.CESSPercentage = item.Cess;
                        MedicalbillTran.CESSValue = item.CessValue;
                        MedicalbillTran.AdditionalCESSPercentage = item.AddCess;
                        MedicalbillTran.AdditionalCESSValue = item.AddCessValue;
                        MedicalbillTran.TaxID = item.TaxID;
                        MedicalbillTran.MedicalBillParentTranID = item.MedicalbillTranID;
                        MedicalbillTran.Status = true;
                        MedicalbillTran.CreatedUTC = DateTime.UtcNow;
                        MedicalbillTran.CreatedBy = BillingReturnDetails.CreatedBy;
                        WYNKContext.MedicalBillTran.Add(MedicalbillTran);
                        WYNKContext.SaveChanges();


                        var stockTran = new StockTran();
                        stockTran.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                        stockTran.SMID = stockmas.RandomUniqueID;
                        stockTran.ItemID = item.ItemId;
                        stockTran.ItemQty = item.ReturnQty;
                        stockTran.ItemRate = item.UnitPrice;
                        stockTran.ItemValue = item.Amount;
                        stockTran.DiscountPercentage = item.Discount;
                        stockTran.DiscountAmount = item.DiscountAmount;
                        stockTran.CESSPercentage = item.Cess;
                        stockTran.CESSValue = item.CessValue;
                        stockTran.AdditionalCESSPercentage = item.AddCess;
                        stockTran.AdditionalCESSValue = item.AddCessValue;
                        stockTran.SGSTPercentage = item.GST / 2;
                        stockTran.CGSTPercentage = item.GST / 2;
                        stockTran.GSTPercentage = item.GST;
                        stockTran.GSTTaxValue = item.GSTValue + item.CessValue + item.AddCessValue;
                        stockTran.SGSTTaxValue = item.GSTValue / 2;
                        stockTran.CGSTTaxValue = item.GSTValue / 2;
                        stockTran.CreatedUTC = DateTime.UtcNow;
                        stockTran.CreatedBy = BillingReturnDetails.CreatedBy;
                        WYNKContext.StockTran.Add(stockTran);
                        WYNKContext.SaveChanges();
                        /*End*/

                        /*Update MedicalBillMaster Returns && StockMaster Records*/
                        var ReturnMedicalBillMaster = WYNKContext.MedicalBillMaster.Where(x => x.ID == medicalbill.ID).FirstOrDefault();
                        ReturnMedicalBillMaster.GrossProductValue = ReturnMedicalBillMaster.GrossProductValue != null ? (ReturnMedicalBillMaster.GrossProductValue + item.GrossAmount) : 0 + item.GrossAmount;
                        ReturnMedicalBillMaster.TotalDiscountValue = ReturnMedicalBillMaster.TotalDiscountValue != null ? (ReturnMedicalBillMaster.TotalDiscountValue + item.DiscountAmount) : 0 + item.DiscountAmount;
                        ReturnMedicalBillMaster.CESSValue = ReturnMedicalBillMaster.CESSValue != null ? ReturnMedicalBillMaster.CESSValue + item.CessValue : 0 + item.CessValue;
                        ReturnMedicalBillMaster.AdditionalCESSValue = ReturnMedicalBillMaster.AdditionalCESSValue != null ? ReturnMedicalBillMaster.AdditionalCESSValue + item.AddCessValue : 0 + item.AddCessValue;
                        ReturnMedicalBillMaster.TotalTaxValue = ReturnMedicalBillMaster.TotalTaxValue != null ? (ReturnMedicalBillMaster.TotalTaxValue + item.GSTValue + item.CessValue + item.AddCessValue) : 0 + item.GSTValue + item.CessValue + item.AddCessValue;
                        ReturnMedicalBillMaster.TotalCGSTTaxValue = ReturnMedicalBillMaster.TotalCGSTTaxValue != null ? (ReturnMedicalBillMaster.TotalCGSTTaxValue + item.GSTValue / 2) : 0 + (item.GSTValue + item.CessValue + item.AddCessValue) / 2;
                        ReturnMedicalBillMaster.TotalSGSTTaxValue = ReturnMedicalBillMaster.TotalSGSTTaxValue != null ? (ReturnMedicalBillMaster.TotalSGSTTaxValue + item.GSTValue / 2) : 0 + (item.GSTValue + item.CessValue + item.AddCessValue) / 2;
                        ReturnMedicalBillMaster.TotalBillValue = ReturnMedicalBillMaster.TotalBillValue != null ? (ReturnMedicalBillMaster.TotalBillValue + item.TotalCost) : 0 + item.TotalCost;
                        WYNKContext.MedicalBillMaster.UpdateRange(ReturnMedicalBillMaster);

                        var stockMas = WYNKContext.StockMaster.Where(x => x.RandomUniqueID == stockmas.RandomUniqueID).FirstOrDefault();
                        stockMas.GrossProductValue = stockMas.GrossProductValue != null ? (stockMas.GrossProductValue + item.GrossAmount) : 0 + item.GrossAmount;
                        stockMas.TotalDiscountValue = stockMas.TotalDiscountValue != null ? (stockMas.TotalDiscountValue + item.DiscountAmount) : 0 + item.DiscountAmount;
                        stockMas.TotalTaxValue = stockMas.TotalTaxValue != null ? (stockMas.TotalTaxValue + item.GSTValue + item.CessValue + item.AddCessValue) : 0 + item.GSTValue + item.CessValue + item.AddCessValue;
                        stockMas.TotalCGSTTaxValue = stockMas.TotalCGSTTaxValue != null ? (stockMas.TotalCGSTTaxValue + item.GSTValue / 2 + item.CessValue / 2 + item.AddCessValue / 2) : 0 + item.GSTValue / 2 + item.CessValue / 2 + item.AddCessValue / 2;
                        stockMas.TotalSGSTTaxValue = stockMas.TotalSGSTTaxValue != null ? (stockMas.TotalSGSTTaxValue + item.GSTValue / 2 + item.CessValue / 2 + item.AddCessValue / 2) : 0 + item.GSTValue / 2 + item.CessValue / 2 + item.AddCessValue / 2;
                        stockMas.TotalPOValue = stockMas.TotalPOValue != null ? (stockMas.TotalPOValue + item.TotalCost) : 0 + item.TotalCost;
                        stockMas.TotalCESSValue = stockMas.TotalCESSValue != null ? (stockMas.TotalCESSValue + item.CessValue) : 0 + item.CessValue;
                        stockMas.TotalAdditionalCESSValue = stockMas.TotalAdditionalCESSValue != null ? (stockMas.TotalAdditionalCESSValue + item.AddCessValue) : 0 + item.AddCessValue;
                        WYNKContext.StockMaster.UpdateRange(stockMas);
                        WYNKContext.SaveChanges();
                        /*End*/

                        /*Update Previous MedicalBillTran return Qty*/
                        var PreviousMedicalbillTran = WYNKContext.MedicalBillTran.Where(x => x.ID == item.MedicalbillTranID).FirstOrDefault();
                        PreviousMedicalbillTran.ReturnQuantity = PreviousMedicalbillTran.ReturnQuantity != null ? PreviousMedicalbillTran.ReturnQuantity + item.ReturnQty : 0 + item.ReturnQty;

                        if (PreviousMedicalbillTran.Quantity == PreviousMedicalbillTran.ReturnQuantity)
                        {
                            PreviousMedicalbillTran.Status = false;
                        }
                        else
                        {
                            PreviousMedicalbillTran.Status = true;
                        }
                        WYNKContext.MedicalBillTran.UpdateRange(PreviousMedicalbillTran);
                        WYNKContext.SaveChanges();
                        /*End*/

                        /*Checking Its serial or Batch or None Category drug*/
                        if (item.IsSerial == false)
                        {
                            var ReturningBatchDetails = item.BatchDetails.Where(x => x.QtyToReturn != 0).ToList();
                            if (ReturningBatchDetails.Count > 0)
                            {
                                foreach (var ReturningBatchDetail in ReturningBatchDetails.ToList())
                                {
                                    /*Update Original ItemBatchTrans Record with return qty*/
                                    var OrginalItemBatchTranRec = WYNKContext.ItemBatchTrans.Where(x => x.RandomUniqueID == ReturningBatchDetail.ItemBatchTransID && x.SMID == ReturningBatchDetail.SMID && x.STID == ReturningBatchDetail.STID).FirstOrDefault();
                                    OrginalItemBatchTranRec.ReturnQty = OrginalItemBatchTranRec.ReturnQty != null ? OrginalItemBatchTranRec.ReturnQty + ReturningBatchDetail.QtyToReturn : 0 + ReturningBatchDetail.QtyToReturn;
                                    WYNKContext.ItemBatchTrans.UpdateRange(OrginalItemBatchTranRec);

                                    /*Updating ItemBatch Record with return qty*/
                                    var ItemBatch = WYNKContext.ItemBatch.Where(x => x.ItemID == item.ItemId && x.StoreID == BillingReturnDetails.StoreID && x.ItemBatchNumber == ReturningBatchDetail.BatchNo && x.ItemBatchExpiry.Date == Convert.ToDateTime(ReturningBatchDetail.ExpiryDate) && x.cmpID == BillingReturnDetails.Cmpid).FirstOrDefault();
                                    if (ItemBatch != null)
                                    {
                                        ItemBatch.ItemBatchQty = ItemBatch.ItemBatchQty + ReturningBatchDetail.QtyToReturn;
                                        ItemBatch.ItemBatchBalnceQty = ItemBatch.ItemBatchBalnceQty + ReturningBatchDetail.QtyToReturn;
                                        WYNKContext.ItemBatch.UpdateRange(ItemBatch);
                                    }
                                    else
                                    {
                                        /*Incase that store having no records with this batch details*/
                                        var itemBatch = new ItemBatch();
                                        itemBatch.ItemID = item.ItemId;
                                        itemBatch.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                                        itemBatch.ItemBatchNumber = ReturningBatchDetail.BatchNo;
                                        itemBatch.ItemBatchQty = ReturningBatchDetail.QtyToReturn;
                                        itemBatch.ItemBatchBalnceQty = ReturningBatchDetail.QtyToReturn;
                                        itemBatch.ItemBatchExpiry = Convert.ToDateTime(ReturningBatchDetail.ExpiryDate);
                                        itemBatch.StoreID = BillingReturnDetails.StoreID;
                                        itemBatch.CreatedUTC = DateTime.UtcNow;
                                        itemBatch.CreatedBy = BillingReturnDetails.CreatedBy;
                                        itemBatch.cmpID = BillingReturnDetails.Cmpid;
                                        WYNKContext.ItemBatch.Add(itemBatch);
                                        WYNKContext.SaveChanges();
                                        ItemBatch = itemBatch;
                                    }

                                    /*Insert New ItemBatchTrans Record with return qty*/
                                    var itemBatchTrans = new ItemBatchTrans();
                                    itemBatchTrans.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                                    itemBatchTrans.ItemBatchID = ItemBatch.RandomUniqueID;
                                    itemBatchTrans.TC = BillingReturnDetails.TC;
                                    itemBatchTrans.SMID = stockmas.RandomUniqueID;
                                    itemBatchTrans.STID = stockTran.RandomUniqueID;
                                    itemBatchTrans.ItemID = item.ItemId;
                                    itemBatchTrans.ItemBatchNumber = ReturningBatchDetail.BatchNo;
                                    itemBatchTrans.ItemBatchTransactedQty = ReturningBatchDetail.QtyToReturn;
                                    itemBatchTrans.ItemBatchExpiry = Convert.ToDateTime(ReturningBatchDetail.ExpiryDate);
                                    itemBatchTrans.UOMID = UOMID;
                                    itemBatchTrans.CreatedUTC = DateTime.UtcNow;
                                    itemBatchTrans.CreatedBy = BillingReturnDetails.CreatedBy;
                                    itemBatchTrans.cmpID = BillingReturnDetails.Cmpid;
                                    itemBatchTrans.ContraItemBatchID = ReturningBatchDetail.ItemBatchTransID;
                                    WYNKContext.ItemBatchTrans.Add(itemBatchTrans);

                                    /*Updating ItemBalance Record with return qty*/
                                    var ItemBalance = WYNKContext.ItemBalance.Where(x => x.FYear == FinancialYearId && x.ItemID == item.ItemId && x.StoreID == BillingReturnDetails.StoreID && x.CmpID == BillingReturnDetails.Cmpid).FirstOrDefault();

                                    if (ItemBalance != null)
                                    {
                                        switch (CurrentMonth)
                                        {
                                            case 1:
                                                ItemBalance.Rec01 = ItemBalance.Rec01 + ReturningBatchDetail.QtyToReturn;
                                                ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + ReturningBatchDetail.QtyToReturn;
                                                WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                break;
                                            case 2:
                                                ItemBalance.Rec02 = ItemBalance.Rec02 + ReturningBatchDetail.QtyToReturn;
                                                ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + ReturningBatchDetail.QtyToReturn;
                                                WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                break;
                                            case 3:
                                                ItemBalance.Rec03 = ItemBalance.Rec03 + ReturningBatchDetail.QtyToReturn;
                                                ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + ReturningBatchDetail.QtyToReturn;
                                                WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                break;
                                            case 4:
                                                ItemBalance.Rec04 = ItemBalance.Rec04 + ReturningBatchDetail.QtyToReturn;
                                                ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + ReturningBatchDetail.QtyToReturn;
                                                WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                break;
                                            case 5:
                                                ItemBalance.Rec05 = ItemBalance.Rec05 + ReturningBatchDetail.QtyToReturn;
                                                ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + ReturningBatchDetail.QtyToReturn;
                                                WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                break;
                                            case 6:
                                                ItemBalance.Rec06 = ItemBalance.Rec06 + ReturningBatchDetail.QtyToReturn;
                                                ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + ReturningBatchDetail.QtyToReturn;
                                                WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                break;
                                            case 7:
                                                ItemBalance.Rec07 = ItemBalance.Rec07 + ReturningBatchDetail.QtyToReturn;
                                                ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + ReturningBatchDetail.QtyToReturn;
                                                WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                break;
                                            case 8:
                                                ItemBalance.Rec08 = ItemBalance.Rec08 + ReturningBatchDetail.QtyToReturn;
                                                ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + ReturningBatchDetail.QtyToReturn;
                                                WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                break;
                                            case 9:
                                                ItemBalance.Rec09 = ItemBalance.Rec09 + ReturningBatchDetail.QtyToReturn;
                                                ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + ReturningBatchDetail.QtyToReturn;
                                                WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                break;
                                            case 10:
                                                ItemBalance.Rec10 = ItemBalance.Rec10 + ReturningBatchDetail.QtyToReturn;
                                                ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + ReturningBatchDetail.QtyToReturn;
                                                WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                break;
                                            case 11:
                                                ItemBalance.Rec11 = ItemBalance.Rec11 + ReturningBatchDetail.QtyToReturn;
                                                ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + ReturningBatchDetail.QtyToReturn;
                                                WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                break;
                                            case 12:
                                                ItemBalance.Rec12 = ItemBalance.Rec12 + ReturningBatchDetail.QtyToReturn;
                                                ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + ReturningBatchDetail.QtyToReturn;
                                                WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        var ItemBala = new ItemBalance();
                                        switch (CurrentMonth)
                                        {
                                            case 1:
                                                ItemBala.Rec01 = ReturningBatchDetail.QtyToReturn;
                                                break;
                                            case 2:
                                                ItemBala.Rec02 = ReturningBatchDetail.QtyToReturn;
                                                break;
                                            case 3:
                                                ItemBala.Rec03 = ReturningBatchDetail.QtyToReturn;
                                                break;
                                            case 4:
                                                ItemBala.Rec04 = ReturningBatchDetail.QtyToReturn;
                                                break;
                                            case 5:
                                                ItemBala.Rec05 = ReturningBatchDetail.QtyToReturn;
                                                break;
                                            case 6:
                                                ItemBala.Rec06 = ReturningBatchDetail.QtyToReturn;
                                                break;
                                            case 7:
                                                ItemBala.Rec07 = ReturningBatchDetail.QtyToReturn;
                                                break;
                                            case 8:
                                                ItemBala.Rec08 = ReturningBatchDetail.QtyToReturn;
                                                break;
                                            case 9:
                                                ItemBala.Rec09 = ReturningBatchDetail.QtyToReturn;
                                                break;
                                            case 10:
                                                ItemBala.Rec10 = ReturningBatchDetail.QtyToReturn;
                                                break;
                                            case 11:
                                                ItemBala.Rec11 = ReturningBatchDetail.QtyToReturn;
                                                break;
                                            case 12:
                                                ItemBala.Rec12 = ReturningBatchDetail.QtyToReturn;
                                                break;
                                        }
                                        ItemBala.Iss01 = 0;
                                        ItemBala.Iss02 = 0;
                                        ItemBala.Iss03 = 0;
                                        ItemBala.Iss04 = 0;
                                        ItemBala.Iss05 = 0;
                                        ItemBala.Iss06 = 0;
                                        ItemBala.Iss07 = 0;
                                        ItemBala.Iss08 = 0;
                                        ItemBala.Iss09 = 0;
                                        ItemBala.Iss10 = 0;
                                        ItemBala.Iss11 = 0;
                                        ItemBala.Iss12 = 0;
                                        ItemBala.UOMID = UOMID;
                                        ItemBala.FYear = FinancialYearId;
                                        ItemBala.OpeningBalance = 0;
                                        ItemBala.ItemID = item.ItemId;
                                        ItemBala.StoreID = BillingReturnDetails.StoreID;
                                        ItemBala.ClosingBalance = ReturningBatchDetail.QtyToReturn;
                                        ItemBala.CreatedBy = BillingReturnDetails.CreatedBy;
                                        ItemBala.CreatedUTC = DateTime.UtcNow;
                                        ItemBala.CmpID = BillingReturnDetails.Cmpid;
                                        WYNKContext.ItemBalance.Add(ItemBala);
                                    }
                                    WYNKContext.SaveChanges();
                                }
                            }
                        }
                        else if (item.IsSerial == true)
                        {
                            if (item.SelectedList.Count > 0)
                            {
                                foreach (var SerialsDetail in item.SelectedList.ToList())
                                {

                                    var ItemSerial = new ItemSerial();
                                    ItemSerial.ItemID = item.ItemId;
                                    ItemSerial.SerialNo = SerialsDetail.SerialNo;
                                    ItemSerial.GRNNo = BillingReturnDetails.RunningNo;
                                    ItemSerial.TC = BillingReturnDetails.TC;
                                    ItemSerial.IsCancelled = false;
                                    ItemSerial.ExpiryDate = SerialsDetail.ExpiryDate;
                                    ItemSerial.CreatedUTC = DateTime.UtcNow;
                                    ItemSerial.CreatedBy = BillingReturnDetails.CreatedBy;
                                    ItemSerial.StoreID = BillingReturnDetails.StoreID;
                                    ItemSerial.cmpID = BillingReturnDetails.Cmpid;
                                    WYNKContext.ItemSerial.Add(ItemSerial);

                                    /*Updating ItemSerial is Returned*/
                                    var OriginalItemSerialRec = WYNKContext.ItemSerial.Where(x => x.SerialNo == SerialsDetail.SerialNo && x.IssueNo == SerialsDetail.BillNo && Convert.ToDateTime(x.ExpiryDate).Date == Convert.ToDateTime(SerialsDetail.ExpiryDate).Date && x.cmpID == BillingReturnDetails.Cmpid).FirstOrDefault();
                                    OriginalItemSerialRec.IsReturned = true;
                                    WYNKContext.ItemSerial.UpdateRange(OriginalItemSerialRec);
                                }
                                var ItemBalance = WYNKContext.ItemBalance.Where(x => x.FYear == FinancialYearId && x.ItemID == item.ItemId && x.StoreID == BillingReturnDetails.StoreID && x.CmpID == BillingReturnDetails.Cmpid).FirstOrDefault();
                                if (ItemBalance != null)
                                {
                                    switch (CurrentMonth)
                                    {
                                        case 1:
                                            ItemBalance.Rec01 = ItemBalance.Rec01 + item.ReturnQty;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + item.ReturnQty;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 2:
                                            ItemBalance.Rec02 = ItemBalance.Rec02 + item.ReturnQty;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + item.ReturnQty;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 3:
                                            ItemBalance.Rec03 = ItemBalance.Rec03 + item.ReturnQty;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + item.ReturnQty;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 4:
                                            ItemBalance.Rec04 = ItemBalance.Rec04 + item.ReturnQty;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + item.ReturnQty;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 5:
                                            ItemBalance.Rec05 = ItemBalance.Rec05 + item.ReturnQty;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + item.ReturnQty;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 6:
                                            ItemBalance.Rec06 = ItemBalance.Rec06 + item.ReturnQty;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + item.ReturnQty;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 7:
                                            ItemBalance.Rec07 = ItemBalance.Rec07 + item.ReturnQty;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + item.ReturnQty;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 8:
                                            ItemBalance.Rec08 = ItemBalance.Rec08 + item.ReturnQty;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + item.ReturnQty;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 9:
                                            ItemBalance.Rec09 = ItemBalance.Rec09 + item.ReturnQty;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + item.ReturnQty;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 10:
                                            ItemBalance.Rec10 = ItemBalance.Rec10 + item.ReturnQty;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + item.ReturnQty;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 11:
                                            ItemBalance.Rec11 = ItemBalance.Rec11 + item.ReturnQty;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + item.ReturnQty;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                        case 12:
                                            ItemBalance.Rec12 = ItemBalance.Rec12 + item.ReturnQty;
                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + item.ReturnQty;
                                            WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                            break;
                                    }
                                }
                                else
                                {
                                    var ItemBala = new ItemBalance();
                                    switch (CurrentMonth)
                                    {
                                        case 1:
                                            ItemBala.Rec01 = item.ReturnQty;
                                            break;
                                        case 2:
                                            ItemBala.Rec02 = item.ReturnQty;
                                            break;
                                        case 3:
                                            ItemBala.Rec03 = item.ReturnQty;
                                            break;
                                        case 4:
                                            ItemBala.Rec04 = item.ReturnQty;
                                            break;
                                        case 5:
                                            ItemBala.Rec05 = item.ReturnQty;
                                            break;
                                        case 6:
                                            ItemBala.Rec06 = item.ReturnQty;
                                            break;
                                        case 7:
                                            ItemBala.Rec07 = item.ReturnQty;
                                            break;
                                        case 8:
                                            ItemBala.Rec08 = item.ReturnQty;
                                            break;
                                        case 9:
                                            ItemBala.Rec09 = item.ReturnQty;
                                            break;
                                        case 10:
                                            ItemBala.Rec10 = item.ReturnQty;
                                            break;
                                        case 11:
                                            ItemBala.Rec11 = item.ReturnQty;
                                            break;
                                        case 12:
                                            ItemBala.Rec12 = item.ReturnQty;
                                            break;
                                    }
                                    ItemBala.Iss01 = 0;
                                    ItemBala.Iss02 = 0;
                                    ItemBala.Iss03 = 0;
                                    ItemBala.Iss04 = 0;
                                    ItemBala.Iss05 = 0;
                                    ItemBala.Iss06 = 0;
                                    ItemBala.Iss07 = 0;
                                    ItemBala.Iss08 = 0;
                                    ItemBala.Iss09 = 0;
                                    ItemBala.Iss10 = 0;
                                    ItemBala.Iss11 = 0;
                                    ItemBala.Iss12 = 0;
                                    ItemBala.UOMID = UOMID;
                                    ItemBala.FYear = FinancialYearId;
                                    ItemBala.OpeningBalance = 0;
                                    ItemBala.ItemID = item.ItemId;
                                    ItemBala.StoreID = BillingReturnDetails.StoreID;
                                    ItemBala.ClosingBalance = item.ReturnQty;
                                    ItemBala.CreatedBy = BillingReturnDetails.CreatedBy;
                                    ItemBala.CreatedUTC = DateTime.UtcNow;
                                    ItemBala.CmpID = BillingReturnDetails.Cmpid;
                                    WYNKContext.ItemBalance.Add(ItemBala);
                                }
                                WYNKContext.SaveChanges();
                            }
                        }
                        else
                        {
                            var ItemBalance = WYNKContext.ItemBalance.Where(x => x.FYear == FinancialYearId && x.ItemID == item.ItemId && x.StoreID == BillingReturnDetails.StoreID && x.CmpID == BillingReturnDetails.Cmpid).FirstOrDefault();
                            if (ItemBalance != null)
                            {
                                switch (CurrentMonth)
                                {
                                    case 1:
                                        ItemBalance.Rec01 = ItemBalance.Rec01 + item.ReturnQty;
                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + item.ReturnQty;
                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                        break;
                                    case 2:
                                        ItemBalance.Rec02 = ItemBalance.Rec02 + item.ReturnQty;
                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + item.ReturnQty;
                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                        break;
                                    case 3:
                                        ItemBalance.Rec03 = ItemBalance.Rec03 + item.ReturnQty;
                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + item.ReturnQty;
                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                        break;
                                    case 4:
                                        ItemBalance.Rec04 = ItemBalance.Rec04 + item.ReturnQty;
                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + item.ReturnQty;
                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                        break;
                                    case 5:
                                        ItemBalance.Rec05 = ItemBalance.Rec05 + item.ReturnQty;
                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + item.ReturnQty;
                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                        break;
                                    case 6:
                                        ItemBalance.Rec06 = ItemBalance.Rec06 + item.ReturnQty;
                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + item.ReturnQty;
                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                        break;
                                    case 7:
                                        ItemBalance.Rec07 = ItemBalance.Rec07 + item.ReturnQty;
                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + item.ReturnQty;
                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                        break;
                                    case 8:
                                        ItemBalance.Rec08 = ItemBalance.Rec08 + item.ReturnQty;
                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + item.ReturnQty;
                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                        break;
                                    case 9:
                                        ItemBalance.Rec09 = ItemBalance.Rec09 + item.ReturnQty;
                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + item.ReturnQty;
                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                        break;
                                    case 10:
                                        ItemBalance.Rec10 = ItemBalance.Rec10 + item.ReturnQty;
                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + item.ReturnQty;
                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                        break;
                                    case 11:
                                        ItemBalance.Rec11 = ItemBalance.Rec11 + item.ReturnQty;
                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + item.ReturnQty;
                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                        break;
                                    case 12:
                                        ItemBalance.Rec12 = ItemBalance.Rec12 + item.ReturnQty;
                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + item.ReturnQty;
                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                        break;
                                }
                            }
                            else
                            {
                                var ItemBala = new ItemBalance();
                                switch (CurrentMonth)
                                {
                                    case 1:
                                        ItemBala.Rec01 = item.ReturnQty;
                                        break;
                                    case 2:
                                        ItemBala.Rec02 = item.ReturnQty;
                                        break;
                                    case 3:
                                        ItemBala.Rec03 = item.ReturnQty;
                                        break;
                                    case 4:
                                        ItemBala.Rec04 = item.ReturnQty;
                                        break;
                                    case 5:
                                        ItemBala.Rec05 = item.ReturnQty;
                                        break;
                                    case 6:
                                        ItemBala.Rec06 = item.ReturnQty;
                                        break;
                                    case 7:
                                        ItemBala.Rec07 = item.ReturnQty;
                                        break;
                                    case 8:
                                        ItemBala.Rec08 = item.ReturnQty;
                                        break;
                                    case 9:
                                        ItemBala.Rec09 = item.ReturnQty;
                                        break;
                                    case 10:
                                        ItemBala.Rec10 = item.ReturnQty;
                                        break;
                                    case 11:
                                        ItemBala.Rec11 = item.ReturnQty;
                                        break;
                                    case 12:
                                        ItemBala.Rec12 = item.ReturnQty;
                                        break;
                                }
                                ItemBala.Iss01 = 0;
                                ItemBala.Iss02 = 0;
                                ItemBala.Iss03 = 0;
                                ItemBala.Iss04 = 0;
                                ItemBala.Iss05 = 0;
                                ItemBala.Iss06 = 0;
                                ItemBala.Iss07 = 0;
                                ItemBala.Iss08 = 0;
                                ItemBala.Iss09 = 0;
                                ItemBala.Iss10 = 0;
                                ItemBala.Iss11 = 0;
                                ItemBala.Iss12 = 0;
                                ItemBala.UOMID = UOMID;
                                ItemBala.FYear = FinancialYearId;
                                ItemBala.OpeningBalance = 0;
                                ItemBala.ItemID = item.ItemId;
                                ItemBala.StoreID = BillingReturnDetails.StoreID;
                                ItemBala.ClosingBalance = item.ReturnQty;
                                ItemBala.CreatedBy = BillingReturnDetails.CreatedBy;
                                ItemBala.CreatedUTC = DateTime.UtcNow;
                                ItemBala.CmpID = BillingReturnDetails.Cmpid;
                                WYNKContext.ItemBalance.Add(ItemBala);
                            }
                            WYNKContext.SaveChanges();
                        }
                        /*End*/
                        if (item.TaxID != null)
                        {

                            var taxidd = CMPSContext.TaxMaster.Where(x => x.ID == item.TaxID).ToList();
                            var taxgrp = CMPSContext.TaxMaster.Where(x => x.ID == item.TaxID).Select(x => x.TaxGroupId).FirstOrDefault();

                            if (taxidd != null && taxidd.Count != 0)
                            {
                                taxidd.All(x => { x.istransact = false; return true; });
                                CMPSContext.TaxMaster.UpdateRange(taxidd);
                                CMPSContext.SaveChanges();
                            }

                            var now = DateTime.UtcNow;
                            var first = new DateTime(now.Year, now.Month, 1);
                            var last = first.AddMonths(1).AddDays(-1);

                            var taid = (from TS in WYNKContext.TaxSummary.Where(x => x.TransactionDate == last && x.CMPID == BillingReturnDetails.Cmpid && x.BillingType == "R" && x.TaxID == item.TaxID)
                                        select new
                                        {
                                            ret = TS.TaxID,
                                        }).ToList();

                            if (taid.Count == 0)
                            {
                                var tax = new Tax_Summary();
                                tax.TaxID = item.TaxID;
                                tax.CMPID = BillingReturnDetails.Cmpid;
                                tax.BillingType = "R";
                                tax.TransactionDate = last;
                                tax.TaxGroupID = taxgrp;
                                tax.GrossAmount = (item.UnitPrice * item.ReturnQty) - item.DiscountAmount;
                                tax.TaxDescription = CMPSContext.TaxMaster.Where(x => x.ID == item.TaxID).Select(x => x.TaxDescription).FirstOrDefault();
                                tax.CESSDescription = CMPSContext.TaxMaster.Where(x => x.ID == item.TaxID).Select(x => x.TaxDescription).FirstOrDefault();
                                tax.AddlCESSDescription = CMPSContext.TaxMaster.Where(x => x.ID == item.TaxID).Select(x => x.TaxDescription).FirstOrDefault();
                                tax.TaxPercentage = Convert.ToInt16(item.GST);
                                tax.TaxValue = item.GSTValue;
                                tax.Tax1Percentage = Convert.ToInt16(item.GST / 2);
                                tax.Tax1Value = item.GSTValue / 2;
                                tax.Tax2Percentage = Convert.ToInt16(item.GST / 2);
                                tax.Tax2Value = item.GSTValue / 2;
                                tax.CESSPercentage = item.Cess != null ? item.Cess : 0;
                                tax.CESSValue = item.Cess != null ? item.CessValue : 0;
                                tax.AddlCESSPercentage = item.AddCess != null ? item.AddCess : 0;
                                tax.AddlCESSValue = item.AddCess != null ? item.AddCessValue : 0;
                                tax.CreatedUTC = DateTime.UtcNow;
                                tax.CreatedBy = BillingReturnDetails.CreatedBy;
                                WYNKContext.TaxSummary.AddRange(tax);
                                WYNKContext.SaveChanges();
                            }
                            else
                            {

                                var masters = WYNKContext.TaxSummary.Where(x => x.TransactionDate == last && x.CMPID == BillingReturnDetails.Cmpid && x.BillingType == "R" && x.TaxID == item.TaxID).LastOrDefault();
                                masters.GrossAmount += (item.UnitPrice * item.ReturnQty) - item.DiscountAmount;
                                masters.TaxValue += item.GSTValue;
                                masters.Tax1Value += item.GSTValue / 2;
                                masters.Tax2Value += item.GSTValue / 2;
                                masters.CESSValue += item.Cess != null ? item.CessValue : 0;
                                masters.AddlCESSValue += item.AddCess != null ? item.AddCessValue : 0;
                                masters.UpdatedUTC = DateTime.UtcNow;
                                masters.UpdatedBy = BillingReturnDetails.CreatedBy;
                                WYNKContext.TaxSummary.UpdateRange(masters);
                                WYNKContext.SaveChanges();
                            }

                        }
                    }

                    WYNKContext.SaveChanges();


                    if (BillingReturnDetails.BillType == "Credit")
                    {
                        OriginalMedicalBillDetail.ReturnedItemAmount = OriginalMedicalBillDetail.ReturnedItemAmount != null ? OriginalMedicalBillDetail.ReturnedItemAmount + medicalbill.TotalBillValue : 0 + medicalbill.TotalBillValue;
                    }
                    else if (BillingReturnDetails.BillType == "Refund")
                    {
                        OriginalMedicalBillDetail.ReturnedItemAmount = OriginalMedicalBillDetail.ReturnedItemAmount != null ? OriginalMedicalBillDetail.ReturnedItemAmount + medicalbill.TotalBillValue : 0 + medicalbill.TotalBillValue;
                        OriginalMedicalBillDetail.CreditStatus = false;
                    }

                    /*Payment Master*/
                    if (BillingReturnDetails.paymenttrans.Count > 0)
                    {

                        foreach (var item1 in BillingReturnDetails.paymenttrans.ToList())
                        {
                            var payment = new Payment_Master();
                            payment.UIN = BillingReturnDetails.UIN;
                            payment.PaymentType = BillingReturnDetails.BillType == "Refund" ? "R" : "O";
                            payment.PaymentMode = item1.PaymentMode;
                            payment.OLMID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == item1.PaymentMode).Select(x => x.OLMID).FirstOrDefault();
                            payment.Amount = item1.Amount;
                            payment.BankBranch = item1.BankBranch;
                            payment.BankName = item1.BankName;
                            payment.Fyear = Fyear;
                            payment.InVoiceNumber = BillingReturnDetails.RunningNo;
                            payment.InVoiceDate = DateTime.UtcNow;
                            payment.ReceiptNumber = BillingReturnDetails.ReceiptRunningNo;
                            payment.ReceiptDate = DateTime.UtcNow;
                            payment.InstrumentNumber = item1.InstrumentNumber;
                            if (item1.Instrumentdate != null)
                            {
                                payment.Instrumentdate = Convert.ToDateTime(item1.Instrumentdate).AddDays(1);
                            }
                            else
                            {
                                payment.Instrumentdate = null;
                            }
                            if (item1.Expirydate != null)
                            {
                                payment.Expirydate = Convert.ToDateTime(item1.Expirydate).AddDays(1);
                            }
                            else
                            {
                                payment.Expirydate = null;
                            }
                            payment.PaymentReferenceID = medicalbill.ID;
                            payment.TransactionID = BillingReturnDetails.TC;
                            payment.CmpID = BillingReturnDetails.Cmpid;
                            payment.IsBilled = true;
                            payment.CreatedUTC = DateTime.UtcNow;
                            payment.CreatedBy = BillingReturnDetails.CreatedBy;
                            WYNKContext.PaymentMaster.Add(payment);
                        }
                        WYNKContext.SaveChanges();
                    }

                    var CheckingMedicalBillTran = WYNKContext.MedicalBillTran.Where(x => x.MedicalBillID == BillingReturnDetails.MedicalBillId).All(x => x.Status == false);

                    if (CheckingMedicalBillTran)
                    {
                        var ChangeMedicalbillmaster = WYNKContext.MedicalBillMaster.Where(x => x.ID == BillingReturnDetails.MedicalBillId).FirstOrDefault();
                        ChangeMedicalbillmaster.Status = false;
                        WYNKContext.MedicalBillMaster.UpdateRange(ChangeMedicalbillmaster);
                        WYNKContext.SaveChanges();
                    }

                    var commonRepos = new CommonRepository(_Wynkcontext, _Cmpscontext);
                    var RunningNumber = commonRepos.GenerateRunningCtrlNoo(BillingReturnDetails.TC, BillingReturnDetails.Cmpid, "GetRunningNo");
                    if (RunningNumber == BillingReturnDetails.RunningNo)
                    {
                        commonRepos.GenerateRunningCtrlNoo(BillingReturnDetails.TC, BillingReturnDetails.Cmpid, "UpdateRunningNo");
                    }
                    else
                    {
                        var GetRunningNumber = commonRepos.GenerateRunningCtrlNoo(BillingReturnDetails.TC, BillingReturnDetails.Cmpid, "UpdateRunningNo");

                        var MedicalBillMasters = WYNKContext.MedicalBillMaster.Where(x => x.BillNo == BillingReturnDetails.RunningNo && x.ID == medicalbill.ID).FirstOrDefault();
                        MedicalBillMasters.BillNo = GetRunningNumber;
                        WYNKContext.MedicalBillMaster.UpdateRange(MedicalBillMasters);

                        var StockMasters = WYNKContext.StockMaster.Where(x => x.SMID == stockmas.SMID && x.TransactionID == BillingReturnDetails.TC).FirstOrDefault();
                        StockMasters.DocumentNumber = GetRunningNumber;
                        WYNKContext.StockMaster.UpdateRange(StockMasters);

                        if (BillingReturnDetails.paymenttrans.Count > 0)
                        {
                            var payments = WYNKContext.PaymentMaster.Where(x => x.InVoiceNumber == BillingReturnDetails.RunningNo && x.TransactionID == BillingReturnDetails.TC).ToList();
                            payments.All(x => { x.InVoiceNumber = GetRunningNumber; return true; });
                            WYNKContext.PaymentMaster.UpdateRange(payments);
                        }
                    }
                    WYNKContext.SaveChanges();
                    dbContextTransaction.Commit();
                    return new
                    {
                        Success = true,
                        Message = "Saved successfully",
                        CMPDetails = CMPSContext.Company.Where(x => x.CmpID == BillingReturnDetails.Cmpid).FirstOrDefault(),
                        paymentReceiptNo = BillingReturnDetails.paymenttrans.Count > 0 ? WYNKContext.PaymentMaster.Where(x => x.PaymentReferenceID == medicalbill.ID).Select(x => x.ReceiptNumber).FirstOrDefault() : null,
                        paymentReceiptDate = BillingReturnDetails.paymenttrans.Count > 0 ? WYNKContext.PaymentMaster.Where(x => x.PaymentReferenceID == medicalbill.ID).Select(x => x.ReceiptDate + ts).FirstOrDefault() : null,
                        BillNo = medicalbill.BillNo,
                    };

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    if (ex.InnerException != null)
                    {
                        ErrorLog oErrorLog = new ErrorLog();
                        oErrorLog.WriteErrorLog("Error", ex.InnerException.Message.ToString());
                        string msg = ex.InnerException.Message;
                        return new { Success = false, Message = msg, grn = BillingReturnDetails.RunningNo };
                    }
                    else
                    {
                        ErrorLog oErrorLog = new ErrorLog();
                        oErrorLog.WriteErrorLog("Error", "Something Went Wrong");
                        return new { Success = false, Message = "Something Went Wrong" };
                    }
                }
            }
        }

        public dynamic CurrentDateSearchCreditBill(DateTime SelectedDate, int CmpID, int Tc, string GMT)
        {
            var reg = WYNKContext.Registration.Where(x => x.CMPID == CmpID).ToList();
            var medicalbill = WYNKContext.MedicalBillMaster.Where(x => x.CMPID == CmpID).ToList();
            var storemaster = CMPSContext.Storemasters.Where(x => x.CmpID == CmpID).ToList();
            TimeSpan ts = TimeSpan.Parse(GMT);
            List<MedicalBill_Master> res;

            res = medicalbill.Where(x => (x.CreatedUTC + ts).Date == Convert.ToDateTime(SelectedDate.ToString("yyyy-MM-dd")) && x.CMPID == CmpID && x.Status == true && x.TransactionId == Tc && x.BillType == "Credit" && x.CreditStatus == true).ToList();

            var result = (from med in res
                          join stockmas in WYNKContext.StockMaster on med.BillNo equals stockmas.DocumentNumber
                          where stockmas.CMPID == CmpID
                          select new
                          {
                              BillNo = med.BillNo,
                              BillDate = med.CreatedUTC + ts,
                              UIN = med.UIN,
                              RegistrationTranID = med.RegistrationTranID,
                              PatientName = reg.Where(x => x.UIN == med.UIN).FirstOrDefault().Name + reg.Where(x => x.UIN == med.UIN).FirstOrDefault().MiddleName + reg.Where(x => x.UIN == med.UIN).FirstOrDefault().LastName,
                              Status = med.Status,
                              Gender = reg.Where(x => x.UIN == med.UIN).Select(x => x.Gender).FirstOrDefault(),
                              Age = DateTime.Now.Date.Year - (reg.Where(x => x.UIN == med.UIN).Select(x => x.DateofBirth).FirstOrDefault()).Year,
                              StoreName = storemaster.Where(x => x.StoreID == stockmas.StoreID).Select(x => x.Storename).FirstOrDefault(),
                              StoreId = stockmas.StoreID,
                              MedicalBillID = med.ID,
                          }).ToList();

            return new
            {
                Success = true,
                Result = result,
            };

        }

        public dynamic PeriodSearchCreditBill(DateTime FromDate, DateTime ToDate, int CmpID, int Tc, string GMT)
        {
            var reg = WYNKContext.Registration.Where(x => x.CMPID == CmpID).ToList();
            var medicalbill = WYNKContext.MedicalBillMaster.Where(x => x.CMPID == CmpID).ToList();
            var storemaster = CMPSContext.Storemasters.Where(x => x.CmpID == CmpID).ToList();
            TimeSpan ts = TimeSpan.Parse(GMT);
            List<MedicalBill_Master> res;

            res = medicalbill.Where(x => (x.CreatedUTC + ts).Date >= Convert.ToDateTime(FromDate.ToString("yyyy-MM-dd")) && (x.CreatedUTC + ts).Date <= Convert.ToDateTime(ToDate.ToString("yyyy-MM-dd")) && x.CMPID == CmpID && x.Status == true && x.TransactionId == Tc && x.BillType == "Credit" && x.CreditStatus == true).ToList();

            var result = (from med in res
                          join stockmas in WYNKContext.StockMaster on med.BillNo equals stockmas.DocumentNumber
                          where stockmas.CMPID == CmpID
                          select new
                          {
                              BillNo = med.BillNo,
                              BillDate = med.CreatedUTC + ts,
                              UIN = med.UIN,
                              RegistrationTranID = med.RegistrationTranID,
                              PatientName = reg.Where(x => x.UIN == med.UIN).FirstOrDefault().Name + reg.Where(x => x.UIN == med.UIN).FirstOrDefault().MiddleName + reg.Where(x => x.UIN == med.UIN).FirstOrDefault().LastName,
                              Status = med.Status,
                              Gender = reg.Where(x => x.UIN == med.UIN).Select(x => x.Gender).FirstOrDefault(),
                              Age = DateTime.Now.Date.Year - (reg.Where(x => x.UIN == med.UIN).Select(x => x.DateofBirth).FirstOrDefault()).Year,
                              StoreName = storemaster.Where(x => x.StoreID == stockmas.StoreID).Select(x => x.Storename).FirstOrDefault(),
                              StoreId = stockmas.StoreID,
                              MedicalBillID = med.ID,
                          }).ToList();

            return new
            {
                Success = true,
                Result = result,
            };
        }

        public dynamic GetCreditItemDetails(int MedBillID, int CmpID, int TC, string GMT)
        {
            var MedicalPrescription = new CreditBillingDetails();
            var drugmaster = WYNKContext.DrugMaster.Where(x => x.Cmpid == CmpID).ToList();
            var taxMaster = CMPSContext.TaxMaster.ToList();
            var OnelineMaster = CMPSContext.OneLineMaster.ToList();
            TimeSpan ts = TimeSpan.Parse(GMT);
            MedicalPrescription.CreditItemDetails = (from MB in WYNKContext.MedicalBillMaster.Where(x => x.TransactionId == TC && x.CMPID == CmpID && x.ID == MedBillID)
                                                     join med in WYNKContext.MedicalBillTran on MB.ID equals med.MedicalBillID
                                                     let ReturnQty = med.ReturnQuantity != null ? Convert.ToInt32(med.Quantity) == Convert.ToInt32(med.ReturnQuantity) ? Convert.ToInt32(med.ReturnQuantity) : Convert.ToInt32(med.Quantity) - Convert.ToInt32(med.ReturnQuantity) : 0
                                                     let Amount = med.ReturnQuantity != null ? Convert.ToDecimal(med.ReturnQuantity) * Convert.ToDecimal(med.ItemRate) : Convert.ToDecimal(med.Quantity) * Convert.ToDecimal(med.ItemRate)
                                                     select new CreditItemDetails
                                                     {
                                                         DrugID = med.DrugID,
                                                         Drug = drugmaster.Where(x => x.ID == med.DrugID).Select(x => x.Brand).FirstOrDefault(),
                                                         UOM = med.UOM,
                                                         Qty = Convert.ToInt32(med.Quantity),
                                                   
                                                         ReturnQty = ReturnQty,
                                                         UnitPrice = Convert.ToDecimal(med.ItemRate),
                                                        
                                                         Amount = Amount,
                                                         Discount = Convert.ToDecimal(med.DiscountPercentage),
                                                         DiscountAmount = Convert.ToDecimal(med.DiscountAmount),
                                                        
                                                         GrossAmount = med.Quantity == med.ReturnQuantity ? 0 : med.DiscountPercentage != 0 ? Convert.ToDecimal((Amount * med.DiscountPercentage) / 100) : Convert.ToDecimal(Amount),
                                                         GST = med.Quantity == med.ReturnQuantity ? 0 : med.GSTPercentage == null ? null : med.GSTPercentage,
                                                         GSTValue = med.Quantity == med.ReturnQuantity ? 0 : (med.ReturnQuantity != null ? ((Convert.ToDecimal(med.ReturnQuantity) * Convert.ToDecimal(med.ItemRate) - Convert.ToDecimal(med.DiscountAmount)) * Convert.ToDecimal(med.GSTPercentage)) / 100 : ((Convert.ToDecimal(med.Quantity) * Convert.ToDecimal(med.ItemRate) - Convert.ToDecimal(med.DiscountAmount)) * Convert.ToDecimal(med.GSTPercentage)) / 100),
                                                         Cess = med.Quantity == med.ReturnQuantity ? 0 : med.CESSPercentage == null ? null : med.CESSPercentage,
                                                         AddCess = med.Quantity == med.ReturnQuantity ? 0 : med.AdditionalCESSPercentage == null ? null : med.AdditionalCESSPercentage,
                                                         TotalCost = med.Quantity == med.ReturnQuantity ? 0 : GetTotalCreditBillCost((med.ReturnQuantity != null ? Convert.ToDecimal(med.ReturnQuantity) * Convert.ToDecimal(med.ItemRate) : Convert.ToDecimal(med.Quantity) * Convert.ToDecimal(med.ItemRate)) - Convert.ToDecimal(med.DiscountAmount),
                                                        (med.ReturnQuantity != null ? ((Convert.ToDecimal(med.ReturnQuantity) * Convert.ToDecimal(med.ItemRate) - Convert.ToDecimal(med.DiscountAmount)) * Convert.ToDecimal(med.GSTPercentage)) / 100 : ((Convert.ToDecimal(med.Quantity) * Convert.ToDecimal(med.ItemRate) - Convert.ToDecimal(med.DiscountAmount)) * Convert.ToDecimal(med.GSTPercentage)) / 100),
                                                        med.CESSPercentage == null ? (decimal?)null : (med.ReturnQuantity != null ? ((Convert.ToDecimal(med.ReturnQuantity) * Convert.ToDecimal(med.ItemRate) - Convert.ToDecimal(med.DiscountAmount)) * Convert.ToDecimal(med.CESSPercentage)) / 100 : ((Convert.ToDecimal(med.Quantity) * Convert.ToDecimal(med.ItemRate) - Convert.ToDecimal(med.DiscountAmount)) * Convert.ToDecimal(med.CESSPercentage)) / 100),
                                                        med.AdditionalCESSPercentage == null ? (decimal?)null : (med.ReturnQuantity != null ? ((Convert.ToDecimal(med.ReturnQuantity) * Convert.ToDecimal(med.ItemRate) - Convert.ToDecimal(med.DiscountAmount)) * Convert.ToDecimal(med.AdditionalCESSPercentage)) / 100 : ((Convert.ToDecimal(med.Quantity) * Convert.ToDecimal(med.ItemRate) - Convert.ToDecimal(med.DiscountAmount)) * Convert.ToDecimal(med.AdditionalCESSPercentage)) / 100)),
                                                         CessValue = med.CESSPercentage == null ? (decimal?)null : (med.ReturnQuantity != null ? ((Convert.ToDecimal(med.ReturnQuantity) * Convert.ToDecimal(med.ItemRate) - Convert.ToDecimal(med.DiscountAmount)) * Convert.ToDecimal(med.CESSPercentage)) / 100 : ((Convert.ToDecimal(med.Quantity) * Convert.ToDecimal(med.ItemRate) - Convert.ToDecimal(med.DiscountAmount)) * Convert.ToDecimal(med.CESSPercentage)) / 100),
                                                         AddCessValue = med.AdditionalCESSPercentage == null ? (decimal?)null : (med.ReturnQuantity != null ? ((Convert.ToDecimal(med.ReturnQuantity) * Convert.ToDecimal(med.ItemRate) - Convert.ToDecimal(med.DiscountAmount)) * Convert.ToDecimal(med.AdditionalCESSPercentage)) / 100 : ((Convert.ToDecimal(med.Quantity) * Convert.ToDecimal(med.ItemRate) - Convert.ToDecimal(med.DiscountAmount)) * Convert.ToDecimal(med.AdditionalCESSPercentage)) / 100),
                                                         TaxDescription = taxMaster.Where(t => t.ID == drugmaster.Where(x => x.ID == med.DrugID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.TaxDescription).FirstOrDefault(),
                                                         CessDescription = taxMaster.Where(t => t.ID == drugmaster.Where(x => x.ID == med.DrugID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.CESSDescription).FirstOrDefault(),
                                                         AddCessDescription = taxMaster.Where(t => t.ID == drugmaster.Where(x => x.ID == med.DrugID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.AdditionalCESSDescription).FirstOrDefault(),
                                                     }).ToList();
           
            var medi = WYNKContext.PaymentMaster.Where(x => x.PaymentReferenceID == MedBillID).Select(x => x.PaymentReferenceID).ToList();
            foreach (var item in medi.ToList())
            {
                MedicalPrescription.paymenttran = (from payment in WYNKContext.PaymentMaster.Where(x => x.PaymentReferenceID == item)
                                                   select new Payment_Master
                                                   {
                                                       PaymentMode = OnelineMaster.Where(x => x.OLMID == payment.OLMID).Select(x => x.ParentDescription).FirstOrDefault(),
                                                       InstrumentNumber = payment.InstrumentNumber,
                                                       Instrumentdate = payment.Instrumentdate,
                                                       BankName = payment.BankName,
                                                       BankBranch = payment.BankBranch,
                                                       Expirydate = payment.Expirydate,
                                                       Amount = payment.Amount,
                                                       PaymentReferenceID = payment.PaymentReferenceID,
                                                   }).ToList();
            }

            MedicalPrescription.CreditTotatAmount = MedicalPrescription.CreditItemDetails.Sum(x => x.TotalCost);
            MedicalPrescription.RemainingAmount = (MedicalPrescription.paymenttran == null ? MedicalPrescription.CreditItemDetails.Sum(x => x.TotalCost) : MedicalPrescription.CreditItemDetails.Sum(x => x.TotalCost) - MedicalPrescription.paymenttran.Sum(x => x.Amount));
            
            var OrgMedicalBill = WYNKContext.MedicalBillMaster.Where(x => x.ID == MedBillID).FirstOrDefault();
            MedicalPrescription.InvoiceNo = OrgMedicalBill.BillNo;
            MedicalPrescription.InvoiceDate = OrgMedicalBill.CreatedUTC + ts;
            return MedicalPrescription;
        }

        public decimal GetGrossAmountCreditBillCost(int Qty, int RtnQty, decimal UnitPrice, decimal Amount, decimal? Discount)
        {
            var res = 0;

            if (Qty > RtnQty)
            {





                return 0;
            }




            return 0;
        }

        public decimal GetTotalCreditBillCost(decimal GrossAmount, decimal? Tax, decimal? Cess, decimal? Addcess)
        {
            return (GrossAmount) + (Tax != null ? Convert.ToDecimal(Tax) : 0) + (Cess != null ? Convert.ToDecimal(Cess) : 0) + (Addcess != null ? Convert.ToDecimal(Addcess) : 0);
        }

    }

}





