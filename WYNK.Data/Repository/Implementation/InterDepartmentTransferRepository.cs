using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WYNK.Data.Common;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Data.Repository.Operation;
using WYNK.Helpers;

namespace WYNK.Data.Repository.Implementation
{
    class InterDepartmentTransferRepository : RepositoryBase<InterDepartmentTransfer>, IInterDeparmentTransferRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        
        public InterDepartmentTransferRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }
        public dynamic GetStoreDetails(int ID)
        {
            var StoreDetail = new InterDepartmentTransfer();
            StoreDetail.StoreDetails = (from REF in CMPSContext.Storemasters.Where(u => u.StoreID == ID)
                                        select new StoreDetail
                                        {
                                            ID = REF.StoreID,
                                            Address1 = REF.Address1,
                                            Address2 = REF.Address2,
                                            Location = REF.StoreLocation,
                                            PhoneNo = REF.PhoneNumber,
                                        }).ToList();
            return StoreDetail;
        }
        public dynamic GetdrugDetails(int ID)
        {
            var InterDepartmentDrugDetails = new InterDepartmentDrugDetails();

            InterDepartmentDrugDetails.DrugDetails = (from Drug in WYNKContext.DrugMaster.Where(x => x.ID == ID)
                                                      select new DrugDetails
                                                      {
                                                        DrugID =Drug.ID,
                                                        Drug = Drug.Brand,
                                                        DrugGroup = WYNKContext.DrugGroup.Where(x=>x.ID == WYNKContext.DrugMaster.Where(y => y.ID == ID).Select(y=>y.GenericName).FirstOrDefault()).Select(x=>x.Description).FirstOrDefault(),
                                                        UOM=Drug.UOM,
                                                      }).ToList();

            return InterDepartmentDrugDetails;
        }
        public IEnumerable<Dropdown> GetDrugvalues(int storeID, int Cmpid)
        {
            var drug = WYNKContext.DrugMaster.Where(x=>x.Cmpid == Cmpid).ToList();
            var Datee = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            var Fyear = WYNKContext.FinancialYear.Where(x => Convert.ToDateTime(x.FYFrom) <= Datee && Convert.ToDateTime(x.FYTo) >= Datee && x.CMPID == Cmpid && x.IsActive == true).Select(x => x.ID).FirstOrDefault();
            return (from e in WYNKContext.ItemBalance.Where(x => x.StoreID == storeID && x.FYear == Fyear).GroupBy(y => y.ItemID)
                    select new Dropdown
                    {
                        Value = e.Select(x => x.ItemID).FirstOrDefault().ToString(),
                        Text = drug.Where(x => x.ID == e.Select(y => y.ItemID).FirstOrDefault()).Select(x => x.Brand).FirstOrDefault(),
                    }).ToList();
        }
        public dynamic InterDepartmentTransferDetails(int transactionCode, string GMT)
        {
            var InterDepartmentTransferDetails = new InterDepartmentTransferDetails();
            TimeSpan ts = TimeSpan.Parse(GMT);
            var storeMaster = CMPSContext.Storemasters.ToList();
            InterDepartmentTransferDetails.InterDepartmentTransferDetail = (from res in WYNKContext.StockMaster.Where(x => x.TransactionID == transactionCode)
                                                                            select new InterDepartmentTransferDetail
                                                                            {
                                                                                  FromStore = storeMaster.Where(x => x.StoreID == res.StoreID).Select(x => x.Storename).FirstOrDefault(),
                                                                                  ToStore = storeMaster.Where(x=>x.StoreID == res.ReceiverStoreID).Select(x=>x.Storename).FirstOrDefault(),                                                     
                                                                                  stockTransNo=res.DocumentNumber,
                                                                                  Date=res.DocumentDate.ToString("dd-MMM-yyyy, hh:mm tt"),
                                                                            }).ToList(); 

            return InterDepartmentTransferDetails;
        }
        public dynamic StockTransferDetails(string StockTransferNo,int CmpId, string GMT)
        {
            var StockTransferDetail = new StockTransferDetails();
            TimeSpan ts = TimeSpan.Parse(GMT);
            var SupplierStoreId = WYNKContext.StockMaster.Where(x => x.DocumentNumber == StockTransferNo).Select(x => x.StoreID).FirstOrDefault();
            var ReceiverStoreId = WYNKContext.StockMaster.Where(x => x.DocumentNumber == StockTransferNo).Select(x => x.ReceiverStoreID).FirstOrDefault();

            var StockMasterId = WYNKContext.StockMaster.Where(x => x.DocumentNumber == StockTransferNo).Select(x => x.RandomUniqueID).FirstOrDefault();

            var DrugMaster = WYNKContext.DrugMaster.ToList();
            var DrugGroup = WYNKContext.DrugGroup.ToList();

            StockTransferDetail.StockTransferNo = StockTransferNo;
            StockTransferDetail.StockTransferDate= WYNKContext.StockMaster.Where(x => x.DocumentNumber == StockTransferNo).Select(x => x.DocumentDate).FirstOrDefault();
            StockTransferDetail.StockTransferDate = StockTransferDetail.StockTransferDate + ts;
            var StockMaster = WYNKContext.StockMaster.ToList();
            var StockTrans = WYNKContext.StockTran.ToList();


            StockTransferDetail.DrugDetails = (from res in StockTrans.Where(x => x.SMID == StockMasterId)
                                               select new DrugDetails1
                                               {
                                                   Brand = DrugMaster.Where(x => x.ID == res.ItemID).Select(x => x.Brand).FirstOrDefault(),
                                                   GenericName = DrugGroup.Where(x => x.ID == DrugMaster.Where(y => y.ID == res.ItemID).Select(y => y.GenericName).FirstOrDefault()).Select(x => x.Description).FirstOrDefault(),
                                                   UOM = DrugMaster.Where(x => x.ID == res.ItemID).Select(x => x.UOM).FirstOrDefault(),
                                                   Quantity = res.ItemQty,
                                                   IsSerial = TrackingType(res.ItemID),
                                                   SerialDetails = TrackingType(res.ItemID) == true ? GettingSerialList(StockMasterId, CmpId, SupplierStoreId, ReceiverStoreId, res.ItemID) : null,
                                                   BatchDetails = TrackingType(res.ItemID) == false ? GettingBatchList(StockMasterId, res.RandomUniqueID, res.ItemID) : null,
                                               }).ToList();

            StockTransferDetail.Supplierdetail = (from res in CMPSContext.Storemasters.Where(x=>x.StoreID == SupplierStoreId)
                                            select new Supplierdetail
                                            {
                                                SupplierStore = res.Storename,
                                                SupplierAddress1=res.Address1,
                                                SupplierAddress2 = res.Address2,
                                                SupplierLocation=res.StoreLocation,
                                                SupplierPhoneNo=res.PhoneNumber,
                                            }).ToList();


            StockTransferDetail.Receiverdetail = (from res in CMPSContext.Storemasters.Where(x => x.StoreID == ReceiverStoreId)
                                                   select new Receiverdetail
                                                   {
                                                       ReceiverStore = res.Storename,
                                                       ReceiverAddress1 = res.Address1,
                                                       ReceiverAddress2 = res.Address2,
                                                       ReceiverLocation = res.StoreLocation,
                                                       ReceiverPhoneNo = res.PhoneNumber,
                                                   }).ToList();

            StockTransferDetail.CompanyDetails = CMPSContext.Company.Where(x => x.CmpID == CmpId).FirstOrDefault();

            return StockTransferDetail;
        }
        private ICollection<BatchDetailInfo> GettingBatchList(string stockMasterId, string sTID, int itemID)
        {
            var ItemBatchtrans = WYNKContext.ItemBatchTrans.ToList();
            var res = (from re in ItemBatchtrans.Where(x => x.SMID == stockMasterId && x.STID == sTID && x.ItemID == itemID)
                       select new BatchDetailInfo
                       { 
                        Batch = re.ItemBatchNumber,
                        qty =Convert.ToInt32(re.ItemBatchTransactedQty),
                        Expiry =re.ItemBatchExpiry,
                       }).ToList();
            return res;
        }
        private ICollection<SerialDetailInfo> GettingSerialList(string stockMasterId, int cmpId, int supplierStoreId, int? receiverStoreId,int ItemID)
        {
            var StockRec = WYNKContext.StockMaster.Where(x => x.RandomUniqueID == stockMasterId && x.CMPID == cmpId && x.StoreID == supplierStoreId && x.ReceiverStoreID == receiverStoreId).FirstOrDefault();
            var ItemSerial = WYNKContext.ItemSerial.ToList();

            var res = (from IT in ItemSerial.Where(x => x.IssueNo == StockRec.DocumentNumber && x.StoreID == supplierStoreId && x.ItemID == ItemID)
                       select new SerialDetailInfo
                       {
                           SerialNo = IT.SerialNo,
                       }).ToList();
            return res;
        }
        private bool? TrackingType(int itemID)
        {
            var DrugMaster = WYNKContext.DrugMaster.ToList();
            var DrugTrackerId = DrugMaster.Where(x => x.ID == itemID).Select(x => x.DrugTracker).FirstOrDefault();
            var res = Enum.GetName(typeof(TrackingType), DrugTrackerId);
            if (res == "SerialNumberBased")
            {
                return true;
            }
            else if (res == "BatchNumberBased")
            {
                return false;
            }
            else {
                return null;
            }
         }
        public dynamic AddstockDetails(SubmitTransferdetails AddTransfer, string GMT)
        {
            TimeSpan ts = TimeSpan.Parse(GMT);
            AddTransfer.InSufficientDrugs = new List<InSufficientDrug>();
            AddTransfer.InSufficientSerials = new List<SerialDetail>();
            AddTransfer.InSufficientOtherDrugs = new List<OtherDrugs>();

            List<AllotedBatch> AllotingBatchs = new List<AllotedBatch>();
            List<SerialDetail> AllotingSerial = new List<SerialDetail>();
            List<OtherDrugs> AllotingOtherDrugs = new List<OtherDrugs>();
            List<Alert> Alerts = new List<Alert>();

            var Fyear = Convert.ToString(WYNKContext.FinancialYear.Where(x => x.FYAccYear == (Convert.ToInt16(AddTransfer.FYear)) && x.CMPID == AddTransfer.Cmpid && x.IsActive == true).Select(x => x.ID).FirstOrDefault());
            var Fyear1 = Convert.ToString(WYNKContext.FinancialYear.Where(x => x.FYAccYear == (Convert.ToInt16(AddTransfer.FYear)) && x.CMPID == AddTransfer.Cmpid && x.IsActive == true).Select(x => x.FYAccYear).FirstOrDefault());

            var drugMaster = WYNKContext.DrugMaster.Where(x=>x.Cmpid == AddTransfer.Cmpid).ToList();
            var uomMaster = CMPSContext.uommaster.ToList();

            DateTime GivenDate;
            var appdate = DateTime.TryParseExact(AddTransfer.GivenDate.Trim(), "dd-MM-yyyy,HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out GivenDate);
            {
                GivenDate.ToString("dd-MM-yyyy,HH:mm", CultureInfo.InvariantCulture);
            }
            GivenDate = GivenDate - ts;

            var StoreId = AddTransfer.SupplierStoreID;

            var TransactionID = AddTransfer.TransactionId;
            var Cmpid = AddTransfer.Cmpid;
            var createdby = AddTransfer.CreatedBy;

            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
              {
                try
                {
                    string stockmasterIdentitys = "";

                    var SupplierStoreName = CMPSContext.Storemasters.Where(x => x.StoreID == AddTransfer.SupplierStoreID).Select(x => x.Storename).FirstOrDefault();
                    var ReceiverStoreName = CMPSContext.Storemasters.Where(x => x.StoreID == AddTransfer.ReceiverStoreID).Select(x => x.Storename).FirstOrDefault();

                    foreach (var MedicalPrescriptionIdDetail in AddTransfer.interTransfers.ToList())
                    {

                        var Drugid = MedicalPrescriptionIdDetail.ID;
                        var reqQuantity = Convert.ToInt32(MedicalPrescriptionIdDetail.Quantity);
                        IList<AllotedBatch> AllotingBatch = new List<AllotedBatch>();

                        var DrugTrackerId = WYNKContext.DrugMaster.Where(x => x.ID == Drugid).Select(x => x.DrugTracker).FirstOrDefault();
                        var res = Enum.GetName(typeof(TrackingType), DrugTrackerId);


                        if (res == "SerialNumberBased")
                        {
                            AddTransfer.InSufficientSerials = CheckSerials(MedicalPrescriptionIdDetail.SelectedList, Drugid, AddTransfer.SupplierStoreID, AddTransfer.Cmpid);

                            if (AddTransfer.InSufficientSerials.Count == 0)
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
                                AllotingBatch = CheckBatchQty(Drugid, AddTransfer.SupplierStoreID, qtylist.BatchNo, qtylist.ExpiryDate, qtylist.QtyTaken);

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
                                            AddTransfer.InSufficientDrugs.Add(InSufficientDrugs);
                                        }
                                    }
                                }

                            }
                        }
                        else
                        {
                            var Date = DateTime.Now;

                            var FinancialYearId = WYNKContext.FinancialYear.Where(x => Convert.ToDateTime(x.FYFrom) <= Date && Convert.ToDateTime(x.FYTo) >= Date && x.CMPID == AddTransfer.Cmpid && x.IsActive == true).Select(x => x.ID).FirstOrDefault();

                            var Itembal = WYNKContext.ItemBalance.Where(x => x.ItemID == Drugid && x.FYear == FinancialYearId && x.StoreID == AddTransfer.SupplierStoreID).Select(x => x.ClosingBalance).FirstOrDefault();

                            if (Itembal >= MedicalPrescriptionIdDetail.Quantity)
                            {
                                var OtherDrugs = new OtherDrugs();
                                OtherDrugs.DrugID = Drugid;
                                OtherDrugs.QtyTobeTaken = Convert.ToInt32(MedicalPrescriptionIdDetail.Quantity);
                                AllotingOtherDrugs.Add(OtherDrugs);
                            }
                            else
                            {
                                var OtherDrugs = new OtherDrugs();
                                OtherDrugs.Brand = WYNKContext.DrugMaster.Where(x => x.ID == Drugid).Select(x => x.Brand).FirstOrDefault();
                                OtherDrugs.AvailableQty = Convert.ToInt32(Itembal);
                                OtherDrugs.DrugID = Drugid;
                                AddTransfer.InSufficientOtherDrugs.Add(OtherDrugs);
                            }

                        }
                    }

                    if (AddTransfer.InSufficientDrugs.Count == 0 && AddTransfer.InSufficientSerials.Count == 0 && AddTransfer.InSufficientOtherDrugs.Count == 0)
                    {
                        var stockmas = AddBilling.AddstockMaster1(AddTransfer.RunningNoStock, GivenDate, AddTransfer.SupplierStoreID, AddTransfer.ReceiverStoreID, 0, AddTransfer.TransactionId, CMPSContext.TransactionType.Where(x => x.TransactionID == AddTransfer.TransactionId).Select(x => x.Rec_Issue_type).FirstOrDefault(), AddTransfer.Cmpid, AddTransfer.CreatedBy, Fyear1);
                        WYNKContext.StockMaster.Add(stockmas);

                        WYNKContext.SaveChanges();
                        stockmasterIdentitys = stockmas.RandomUniqueID;

                        if (AllotingBatchs.Count > 0)
                        {
                            foreach (var item2 in AllotingBatchs.ToList())
                            {

                                var itemBatch = new ItemBatch();
                                itemBatch = WYNKContext.ItemBatch.Where(x => x.ItemBatchNumber == item2.itemBatchNo && x.ItemID == item2.DrugId && x.StoreID == AddTransfer.SupplierStoreID && x.ItemBatchExpiry.Date == Convert.ToDateTime(item2.ExpiryDate)).FirstOrDefault();
                                itemBatch.ItemBatchBalnceQty = itemBatch.ItemBatchBalnceQty - item2.GoingToIssue;
                                itemBatch.ItemBatchissueQty = itemBatch.ItemBatchissueQty + item2.GoingToIssue;
                                itemBatch.LockedQuantity = itemBatch.LockedQuantity - Convert.ToInt32(item2.GoingToIssue);
                                WYNKContext.ItemBatch.UpdateRange(itemBatch);


                                var ItemBalance = WYNKContext.ItemBalance.Where(x => x.ItemID == item2.DrugId && x.FYear == Convert.ToInt32(Fyear) && x.StoreID == AddTransfer.SupplierStoreID && x.CmpID == AddTransfer.Cmpid).FirstOrDefault();

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
                                var Uom = uomMaster.Where(u => u.Description == WYNKContext.DrugMaster.Where(x => x.ID == item2.DrugId).Select(x => x.UOM).FirstOrDefault()).Select(x => x.id).FirstOrDefault();
                                var stockTran = WYNKContext.StockTran.Where(x => x.SMID == stockmasterIdentitys && x.ItemID == item2.DrugId).FirstOrDefault();
                                if (stockTran == null)
                                {
                                    var StkTran = new StockTran();
                                    StkTran.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                                    StkTran.SMID = stockmas.RandomUniqueID;
                                    StkTran.ItemID = item2.DrugId;
                                    var Qty = WYNKContext.StockTran.Where(x => x.SMID == stockmas.RandomUniqueID && x.ItemID == item2.DrugId).Select(x => x.ItemQty).FirstOrDefault();
                                    StkTran.ItemQty = Qty != null ? Qty + item2.GoingToIssue : item2.GoingToIssue;
                                    StkTran.UOMID = uomMaster.Where(UOM => UOM.Description == drugMaster.Where(x => x.ID == item2.DrugId).Select(x => x.UOM).FirstOrDefault()).Select(UOM => UOM.id).FirstOrDefault();
                                    StkTran.ItemRate = drugMaster.Where(x => x.ID == item2.DrugId).Select(x => x.Rate).FirstOrDefault();
                                    StkTran.ItemValue = StkTran.ItemQty * StkTran.ItemRate;
                                    StkTran.CreatedUTC = DateTime.UtcNow;
                                    StkTran.CreatedBy = AddTransfer.CreatedBy;
                                    WYNKContext.StockTran.AddRange(StkTran);
                                    WYNKContext.SaveChanges();
                            
                                    var ItemBatchID = WYNKContext.ItemBatch.Where(x => x.ItemBatchNumber == item2.itemBatchNo && x.ItemID == item2.DrugId && x.StoreID == AddTransfer.SupplierStoreID && x.ItemBatchExpiry.Date == Convert.ToDateTime(item2.ExpiryDate)).Select(x => x.RandomUniqueID).FirstOrDefault();
                                    WYNKContext.ItemBatchTrans.Add(AddBilling.AddItemBatchTrans1(item2, stockmasterIdentitys, StkTran.RandomUniqueID, ItemBatchID, null, AddTransfer.TransactionId, Uom, AddTransfer.CreatedBy, Cmpid));
                                    WYNKContext.SaveChanges();
                                }
                                else
                                {
                                    stockTran.ItemQty = stockTran.ItemQty != null ? stockTran.ItemQty + item2.GoingToIssue : item2.GoingToIssue;
                                    WYNKContext.StockTran.UpdateRange(stockTran);
                                    WYNKContext.SaveChanges();

                                    var ItemBatchID = WYNKContext.ItemBatch.Where(x => x.ItemBatchNumber == item2.itemBatchNo && x.ItemID == item2.DrugId && x.StoreID == AddTransfer.SupplierStoreID && x.ItemBatchExpiry.Date == Convert.ToDateTime(item2.ExpiryDate)).Select(x => x.RandomUniqueID).FirstOrDefault();
                                    WYNKContext.ItemBatchTrans.Add(AddBilling.AddItemBatchTrans1(item2, stockmasterIdentitys, stockTran.RandomUniqueID, ItemBatchID, null, AddTransfer.TransactionId, Uom, AddTransfer.CreatedBy, Cmpid));
                                    WYNKContext.SaveChanges();
                                }
                                WYNKContext.SaveChanges();
                            }
                        }
                        if (AllotingSerial.Count > 0)
                        {
                            var CurrentMonth = DateTime.Now.Month;

                            foreach (var item in AllotingSerial.ToList())
                            {
                                var ItemBalance = WYNKContext.ItemBalance.Where(x => x.FYear == Convert.ToInt32(Fyear) && x.ItemID == item.DrugID && x.StoreID == AddTransfer.SupplierStoreID).FirstOrDefault();

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
                                if (stockTran == null)
                                {
                                    var StkTran = new StockTran();
                                    StkTran.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                                    StkTran.SMID = stockmas.RandomUniqueID;
                                    StkTran.ItemID = Convert.ToInt32(item.DrugID);
                                    var Qty = WYNKContext.StockTran.Where(x => x.SMID == stockmas.RandomUniqueID && x.ItemID == Convert.ToInt32(item.DrugID)).Select(x => x.ItemQty).FirstOrDefault();
                                    StkTran.ItemQty = Qty != null ? Qty + 1 : 1;
                                    StkTran.UOMID = uomMaster.Where(UOM => UOM.Description == drugMaster.Where(x => x.ID == Convert.ToInt32(item.DrugID)).Select(x => x.UOM).FirstOrDefault()).Select(UOM => UOM.id).FirstOrDefault();
                                    StkTran.ItemRate = drugMaster.Where(x => x.ID == Convert.ToInt32(item.DrugID)).Select(x => x.Rate).FirstOrDefault();
                                    StkTran.ItemValue = StkTran.ItemQty * StkTran.ItemRate;
                                    StkTran.CreatedUTC = DateTime.UtcNow;
                                    StkTran.CreatedBy = AddTransfer.CreatedBy;
                                    WYNKContext.StockTran.AddRange(StkTran);
                                    WYNKContext.SaveChanges();

                                }
                                else
                                {
                                    stockTran.ItemQty = stockTran.ItemQty != null ? stockTran.ItemQty + 1 : 1;
                                    WYNKContext.StockTran.UpdateRange(stockTran);
                                    WYNKContext.SaveChanges();
                                }
                            }
                            foreach (var Serial in AllotingSerial.ToList())
                            {
                                var ItemSerial = WYNKContext.ItemSerial.Where(x => x.ItemID == Serial.DrugID && x.SerialNo == Serial.SerialNo && x.GRNNo == Serial.BillNo && x.ExpiryDate == Serial.ExpiryDate && x.StoreID == AddTransfer.SupplierStoreID && x.cmpID == AddTransfer.Cmpid).FirstOrDefault();
                                ItemSerial.IssueDate = DateTime.UtcNow;
                                ItemSerial.IssueNo = AddTransfer.RunningNoStock;
                                ItemSerial.IssueTC = AddTransfer.TransactionId;
                                WYNKContext.ItemSerial.UpdateRange(ItemSerial);

                            }

                            WYNKContext.SaveChanges();
                        }
                        if (AllotingOtherDrugs.Count > 0)
                        {
                            var CurrentMonth = DateTime.Now.Month;
                            foreach (var item in AllotingOtherDrugs.ToList())
                            {
                                var ItemBalance = WYNKContext.ItemBalance.Where(x => x.FYear == Convert.ToInt32(Fyear) && x.ItemID == item.DrugID && x.StoreID == AddTransfer.SupplierStoreID).FirstOrDefault();
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

                                if (stockTran == null)
                                {
                                    var StkTran = new StockTran();
                                    StkTran.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                                    StkTran.SMID = stockmas.RandomUniqueID;
                                    StkTran.ItemID = Convert.ToInt32(item.DrugID);
                                    var Qty = WYNKContext.StockTran.Where(x => x.SMID == stockmas.RandomUniqueID && x.ItemID == Convert.ToInt32(item.DrugID)).Select(x => x.ItemQty).FirstOrDefault();
                                    StkTran.ItemQty = Qty != null ? Qty + Convert.ToInt32(item.QtyTobeTaken) : Convert.ToInt32(item.QtyTobeTaken);
                                    StkTran.UOMID = uomMaster.Where(UOM => UOM.Description == drugMaster.Where(x => x.ID == Convert.ToInt32(item.DrugID)).Select(x => x.UOM).FirstOrDefault()).Select(UOM => UOM.id).FirstOrDefault();
                                    StkTran.ItemRate = drugMaster.Where(x => x.ID == Convert.ToInt32(item.DrugID)).Select(x => x.Rate).FirstOrDefault();
                                    StkTran.ItemValue = StkTran.ItemQty * StkTran.ItemRate;
                                    StkTran.CreatedUTC = DateTime.UtcNow;
                                    StkTran.CreatedBy = AddTransfer.CreatedBy;
                                    WYNKContext.StockTran.AddRange(StkTran);
                                    WYNKContext.SaveChanges();
                                }
                                else {
                                    stockTran.ItemQty = stockTran.ItemQty != null ? stockTran.ItemQty + Convert.ToInt32(item.QtyTobeTaken) : Convert.ToInt32(item.QtyTobeTaken);
                                    WYNKContext.StockTran.UpdateRange(stockTran);
                                    WYNKContext.SaveChanges();

                                }
                            }
                            WYNKContext.SaveChanges();
                        }

                        var commonRepos = new CommonRepository(_Wynkcontext, _Cmpscontext);
                        var RunningNumber = commonRepos.GenerateRunningCtrlNoo(AddTransfer.TransactionId, AddTransfer.Cmpid, "GetRunningNo");
                        if (RunningNumber == AddTransfer.RunningNoStock)
                        {
                            commonRepos.GenerateRunningCtrlNoo(AddTransfer.TransactionId, AddTransfer.Cmpid, "UpdateRunningNo");
                        }
                        else
                        {
                            var GetRunningNumber = commonRepos.GenerateRunningCtrlNoo(AddTransfer.TransactionId, AddTransfer.Cmpid, "GetRunningNo");

                            var stockMaster = WYNKContext.StockMaster.Where(x => x.DocumentNumber == AddTransfer.RunningNoStock).FirstOrDefault();
                            stockMaster.DocumentNumber = GetRunningNumber;
                            WYNKContext.StockMaster.UpdateRange(stockMaster);
                            WYNKContext.SaveChanges();
                        }

                        WYNKContext.SaveChanges();
                        dbContextTransaction.Commit();

                        if (WYNKContext.SaveChanges() >= 0)
                            return new
                            {
                                Success = true,
                                Message = "Saved successfully",
                                StockTransferNo = AddTransfer.RunningNoStock,
                                SupplierStoreName = SupplierStoreName,
                                ReceiverStoreName = ReceiverStoreName,
                                Date = GivenDate + ts,
                                CompanyDetails = CMPSContext.Company.Where(x => x.CmpID == AddTransfer.Cmpid).FirstOrDefault(),
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
                            OutOfStock = AddTransfer.InSufficientDrugs,
                            OutStockSerials = AddTransfer.InSufficientSerials,
                            OutOtherDrugs = AddTransfer.InSufficientOtherDrugs,
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
                        return new { Success = false, Message = msg, grn = AddTransfer.RunningNoStock };
                    }
                    else
                    {
                        ErrorLog oErrorLog = new ErrorLog();
                        oErrorLog.WriteErrorLog("Error", "Something Went Wrong");
                        return new { Success = false, Message = "Something Went Wrong" };
                    }
                }
                return new
                {
                    Success = false,
                    Message = "SomeThing Went Wrong"
                };
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
        public IList<AllotedBatch> CheckBatchQty(int DrugID, int StoreId, string BatchNo, DateTime ExpiryDate, int QtyTaken)
        {
            IList<AllotedBatch> Alloted = new List<AllotedBatch>();

            var drugGroup = WYNKContext.DrugGroup.Where(x => x.ID == WYNKContext.DrugMaster.Where(y => y.ID == DrugID).Select(y => y.GenericName).FirstOrDefault()).FirstOrDefault();
            var RetestInterval = drugGroup.RetestInterval;
            var CriticalIntervalDays = drugGroup.RetestCriticalInterval;

            var drugstockcheck = WYNKContext.ItemBatch.Where(x => x.ItemID == DrugID && x.ItemBatchNumber == BatchNo && DateTime.Now.AddDays(CriticalIntervalDays) < x.ItemBatchExpiry.Date && x.StoreID == StoreId && x.ItemBatchExpiry == ExpiryDate).OrderBy(x => x.CreatedUTC).ToList();
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
                    itemBatch = WYNKContext.ItemBatch.Where(x => x.ItemBatchNumber == res.ItemBatchNumber && x.ItemID == DrugID && x.ItemBatchExpiry == res.ItemBatchExpiry).FirstOrDefault();
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

        public dynamic GetMixedCompanyDetails(GetCompanyIds GetCompanyIds, int CmpId)
        {
            try
            {
                var res = (from Cmp in CMPSContext.Company.Where(x => x.CmpID != CmpId)
                           where GetCompanyIds.CompanyIDSs.Select(x => x.CompanyIDS).Contains(Cmp.CmpID)
                           select new
                           {
                               CmpId = Cmp.CmpID,
                               CmpDetails = Cmp.CompanyName,
                           }).ToList();

                return new
                {
                    Success = true,
                    res = res,
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

        public dynamic AddInterBranchTransfer(SubmitInterBanchdetails AddTransfer)
        {
            AddTransfer.InSufficientDrugs = new List<InSufficientDrug>();
            AddTransfer.InSufficientSerials = new List<SerialDetail>();
            AddTransfer.InSufficientOtherDrugs = new List<OtherDrugs>();

            List<AllotedBatch> AllotingBatchs = new List<AllotedBatch>();
            List<SerialDetail> AllotingSerial = new List<SerialDetail>();
            List<OtherDrugs> AllotingOtherDrugs = new List<OtherDrugs>();

            List<Alert> Alerts = new List<Alert>();

            var Datee = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            var Fyear = Convert.ToString(WYNKContext.FinancialYear.Where(x => Convert.ToDateTime(x.FYFrom) <= Datee && Convert.ToDateTime(x.FYTo) >= Datee && x.CMPID == AddTransfer.Cmpid && x.IsActive == true).Select(x => x.ID).FirstOrDefault());

            var drugMaster = WYNKContext.DrugMaster.Where(x => x.Cmpid == AddTransfer.Cmpid).ToList();
            var uomMaster = CMPSContext.uommaster.ToList();


            var StoreId = AddTransfer.SupplierStoreID;

            var TransactionID = AddTransfer.TransactionId;
            var Cmpid = AddTransfer.Cmpid;
            var createdby = AddTransfer.CreatedBy;

            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    string stockmasterIdentitys = "";
                    var SupplierStoreName = CMPSContext.Storemasters.Where(x => x.StoreID == AddTransfer.SupplierStoreID).Select(x => x.Storename).FirstOrDefault();
                    var ReceiverStoreName = CMPSContext.Company.Where(x => x.CmpID == AddTransfer.ReceiverStoreID).Select(x => x.CompanyName).FirstOrDefault();
                    foreach (var MedicalPrescriptionIdDetail in AddTransfer.interTransfers.ToList())
                    {
                        var Drugid = MedicalPrescriptionIdDetail.ID;
                        var reqQuantity = Convert.ToInt32(MedicalPrescriptionIdDetail.Quantity);
                        IList<AllotedBatch> AllotingBatch = new List<AllotedBatch>();
                        var DrugTrackerId = WYNKContext.DrugMaster.Where(x => x.ID == Drugid).Select(x => x.DrugTracker).FirstOrDefault();
                        var res = Enum.GetName(typeof(TrackingType), DrugTrackerId);
                        if (res == "SerialNumberBased")
                        {
                            AddTransfer.InSufficientSerials = CheckSerials(MedicalPrescriptionIdDetail.SelectedList, Drugid, AddTransfer.SupplierStoreID, AddTransfer.Cmpid);

                            if (AddTransfer.InSufficientSerials.Count == 0)
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
                                AllotingBatch = CheckBatchQty(Drugid, AddTransfer.SupplierStoreID, qtylist.BatchNo, qtylist.ExpiryDate, qtylist.QtyTaken);

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
                                            AddTransfer.InSufficientDrugs.Add(InSufficientDrugs);
                                        }
                                    }
                                }

                            }
                        }
                        else
                        {
                            var Date = DateTime.Now;

                            var FinancialYearId = WYNKContext.FinancialYear.Where(x => Convert.ToDateTime(x.FYFrom) <= Date && Convert.ToDateTime(x.FYTo) >= Date && x.CMPID == AddTransfer.Cmpid && x.IsActive == true).Select(x => x.ID).FirstOrDefault();

                            var Itembal = WYNKContext.ItemBalance.Where(x => x.ItemID == Drugid && x.FYear == FinancialYearId && x.StoreID == AddTransfer.SupplierStoreID).Select(x => x.ClosingBalance).FirstOrDefault();

                            if (Itembal >= MedicalPrescriptionIdDetail.Quantity)
                            {
                                var OtherDrugs = new OtherDrugs();
                                OtherDrugs.DrugID = Drugid;
                                OtherDrugs.QtyTobeTaken = Convert.ToInt32(MedicalPrescriptionIdDetail.Quantity);
                                AllotingOtherDrugs.Add(OtherDrugs);
                            }
                            else
                            {
                                var OtherDrugs = new OtherDrugs();
                                OtherDrugs.Brand = WYNKContext.DrugMaster.Where(x => x.ID == Drugid).Select(x => x.Brand).FirstOrDefault();
                                OtherDrugs.AvailableQty = Convert.ToInt32(Itembal);
                                OtherDrugs.DrugID = Drugid;
                                AddTransfer.InSufficientOtherDrugs.Add(OtherDrugs);
                            }

                        }
                    }

                    if (AddTransfer.InSufficientDrugs.Count == 0 && AddTransfer.InSufficientSerials.Count == 0 && AddTransfer.InSufficientOtherDrugs.Count == 0)
                    {
                        var stockmas = new StockMaster();
                        stockmas.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                        stockmas.TransactionID = AddTransfer.TransactionId;
                        stockmas.CMPID = AddTransfer.Cmpid;
                        stockmas.DocumentNumber = AddTransfer.RunningNoStock;
                        stockmas.DocumentDate = DateTime.UtcNow;
                        stockmas.StoreID = AddTransfer.SupplierStoreID;
                        stockmas.ReceiverBranchId = AddTransfer.ReceiverStoreID;
                        stockmas.RefNo = AddTransfer.RefNo;
                        stockmas.RefDate = AddTransfer.RefDate != null ? Convert.ToDateTime(AddTransfer.RefDate).AddDays(1) : (DateTime?)null;
                        stockmas.Remarks = AddTransfer.Remarks;
                        stockmas.TransportationMode = AddTransfer.Transportation;
                        stockmas.TransportationNo = AddTransfer.TransportationNo;
                        stockmas.TransportationDate = AddTransfer.Transportationdate != null ? Convert.ToDateTime(AddTransfer.Transportationdate).AddDays(1) : (DateTime?)null;
                        stockmas.TransporterName = AddTransfer.TransportationName;
                        stockmas.TransportCharges = AddTransfer.TransportationCharges;
                        stockmas.TransactionType = CMPSContext.TransactionType.Where(X => X.TransactionID == AddTransfer.TransactionId).Select(x => x.Rec_Issue_type).FirstOrDefault();
                        stockmas.VendorID = 0;
                        stockmas.TotalPOValue = 0;
                        stockmas.IsCancelled = false;
                        stockmas.IsDeleted = false;
                        stockmas.Status = (int)Status.Open;
                        stockmas.CreatedUTC = DateTime.UtcNow;
                        stockmas.CreatedBy = AddTransfer.CreatedBy;
                        stockmas.Fyear = Convert.ToString(WYNKContext.FinancialYear.Where(x => x.ID == 1).Select(c => c.FYAccYear).FirstOrDefault());
                        WYNKContext.StockMaster.AddRange(stockmas);

                        WYNKContext.SaveChanges();
                        stockmasterIdentitys = stockmas.RandomUniqueID;

                        if (AllotingBatchs.Count > 0)
                        {
                            foreach (var item2 in AllotingBatchs.ToList())
                            {

                                var itemBatch = new ItemBatch();
                                itemBatch = WYNKContext.ItemBatch.Where(x => x.ItemBatchNumber == item2.itemBatchNo && x.ItemID == item2.DrugId && x.StoreID == AddTransfer.SupplierStoreID && x.ItemBatchExpiry.Date == Convert.ToDateTime(item2.ExpiryDate)).FirstOrDefault();
                                itemBatch.ItemBatchBalnceQty = itemBatch.ItemBatchBalnceQty - item2.GoingToIssue;
                                itemBatch.ItemBatchissueQty = itemBatch.ItemBatchissueQty + item2.GoingToIssue;
                                itemBatch.LockedQuantity = itemBatch.LockedQuantity - Convert.ToInt32(item2.GoingToIssue);
                                WYNKContext.ItemBatch.UpdateRange(itemBatch);

                                var ItemBalance = WYNKContext.ItemBalance.Where(x => x.ItemID == item2.DrugId && x.FYear == Convert.ToInt32(Fyear) && x.StoreID == AddTransfer.SupplierStoreID && x.CmpID == AddTransfer.Cmpid).FirstOrDefault();

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
                                var Uom = uomMaster.Where(u => u.Description == WYNKContext.DrugMaster.Where(x => x.ID == item2.DrugId).Select(x => x.UOM).FirstOrDefault()).Select(x => x.id).FirstOrDefault();
                                var stockTran = WYNKContext.StockTran.Where(x => x.SMID == stockmasterIdentitys && x.ItemID == item2.DrugId).FirstOrDefault();
                                if (stockTran == null)
                                {
                                    var StkTran = new StockTran();
                                    StkTran.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                                    StkTran.SMID = stockmas.RandomUniqueID;
                                    StkTran.ItemID = item2.DrugId;
                                    var Qty = WYNKContext.StockTran.Where(x => x.SMID == stockmas.RandomUniqueID && x.ItemID == item2.DrugId).Select(x => x.ItemQty).FirstOrDefault();
                                    StkTran.ItemQty = Qty != null ? Qty + item2.GoingToIssue : item2.GoingToIssue;
                                    StkTran.UOMID = uomMaster.Where(UOM => UOM.Description == drugMaster.Where(x => x.ID == item2.DrugId).Select(x => x.UOM).FirstOrDefault()).Select(UOM => UOM.id).FirstOrDefault();
                                    StkTran.ItemRate = drugMaster.Where(x => x.ID == item2.DrugId).Select(x => x.Rate).FirstOrDefault();
                                    StkTran.ItemValue = StkTran.ItemQty * StkTran.ItemRate;
                                    StkTran.CreatedUTC = DateTime.UtcNow;
                                    StkTran.CreatedBy = AddTransfer.CreatedBy;
                                    StkTran.RecdQty = 0;
                                    StkTran.DamageQty = 0;
                                    StkTran.OtherQty = 0;
                                    StkTran.LossInTransit = 0;
                                    StkTran.Status = (int)Status.Open;
                                    WYNKContext.StockTran.AddRange(StkTran);
                                    WYNKContext.SaveChanges();
                                
                                    var ItemBatchID = WYNKContext.ItemBatch.Where(x => x.ItemBatchNumber == item2.itemBatchNo && x.ItemID == item2.DrugId && x.StoreID == AddTransfer.SupplierStoreID && x.ItemBatchExpiry.Date == Convert.ToDateTime(item2.ExpiryDate)).Select(x => x.RandomUniqueID).FirstOrDefault();
                                    WYNKContext.ItemBatchTrans.Add(AddBilling.AddItemBatchTrans1(item2, stockmasterIdentitys, StkTran.RandomUniqueID, ItemBatchID, null, AddTransfer.TransactionId, Uom, AddTransfer.CreatedBy, Cmpid));
                                    WYNKContext.SaveChanges();
                                }
                                else
                                {
                                    stockTran.ItemQty = stockTran.ItemQty != null ? stockTran.ItemQty + item2.GoingToIssue : item2.GoingToIssue;
                                    WYNKContext.StockTran.UpdateRange(stockTran);
                                    WYNKContext.SaveChanges();

                                    var ItemBatchID = WYNKContext.ItemBatch.Where(x => x.ItemBatchNumber == item2.itemBatchNo && x.ItemID == item2.DrugId && x.StoreID == AddTransfer.SupplierStoreID && x.ItemBatchExpiry.Date == Convert.ToDateTime(item2.ExpiryDate)).Select(x => x.RandomUniqueID).FirstOrDefault();
                                    WYNKContext.ItemBatchTrans.Add(AddBilling.AddItemBatchTrans1(item2, stockmasterIdentitys, stockTran.RandomUniqueID, ItemBatchID, null, AddTransfer.TransactionId, Uom, AddTransfer.CreatedBy, Cmpid));
                                    WYNKContext.SaveChanges();
                                }
                                WYNKContext.SaveChanges();
                            }
                        }
                        if (AllotingSerial.Count > 0)
                        {
                            var CurrentMonth = DateTime.Now.Month;

                            foreach (var item in AllotingSerial.ToList())
                            {
                                var ItemBalance = WYNKContext.ItemBalance.Where(x => x.FYear == Convert.ToInt32(Fyear) && x.ItemID == item.DrugID && x.StoreID == AddTransfer.SupplierStoreID).FirstOrDefault();

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
                                if (stockTran == null)
                                {
                                    var StkTran = new StockTran();
                                    StkTran.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                                    StkTran.SMID = stockmas.RandomUniqueID;
                                    StkTran.ItemID = Convert.ToInt32(item.DrugID);
                                    var Qty = WYNKContext.StockTran.Where(x => x.SMID == stockmas.RandomUniqueID && x.ItemID == Convert.ToInt32(item.DrugID)).Select(x => x.ItemQty).FirstOrDefault();
                                    StkTran.ItemQty = Qty != null ? Qty + 1 : 1;
                                    StkTran.UOMID = uomMaster.Where(UOM => UOM.Description == drugMaster.Where(x => x.ID == Convert.ToInt32(item.DrugID)).Select(x => x.UOM).FirstOrDefault()).Select(UOM => UOM.id).FirstOrDefault();
                                    StkTran.ItemRate = drugMaster.Where(x => x.ID == Convert.ToInt32(item.DrugID)).Select(x => x.Rate).FirstOrDefault();
                                    StkTran.ItemValue = StkTran.ItemQty * StkTran.ItemRate;
                                    StkTran.CreatedUTC = DateTime.UtcNow;
                                    StkTran.CreatedBy = AddTransfer.CreatedBy;
                                    StkTran.RecdQty = 0;
                                    StkTran.DamageQty = 0;
                                    StkTran.OtherQty = 0;
                                    StkTran.LossInTransit = 0;
                                    StkTran.Status = (int)Status.Open;
                                    WYNKContext.StockTran.AddRange(StkTran);
                                    WYNKContext.SaveChanges();

                                }
                                else
                                {
                                    stockTran.ItemQty = stockTran.ItemQty != null ? stockTran.ItemQty + 1 : 1;
                                    WYNKContext.StockTran.UpdateRange(stockTran);
                                    WYNKContext.SaveChanges();
                                }
                            }
                            foreach (var Serial in AllotingSerial.ToList())
                            {
                                var ItemSerial = WYNKContext.ItemSerial.Where(x => x.ItemID == Serial.DrugID && x.SerialNo == Serial.SerialNo && x.GRNNo == Serial.BillNo && x.ExpiryDate == Serial.ExpiryDate && x.StoreID == AddTransfer.SupplierStoreID && x.cmpID == AddTransfer.Cmpid).FirstOrDefault();
                                ItemSerial.IssueDate = DateTime.UtcNow;
                                ItemSerial.IssueNo = AddTransfer.RunningNoStock;
                                ItemSerial.IssueTC = AddTransfer.TransactionId;
                                WYNKContext.ItemSerial.UpdateRange(ItemSerial);
                            }

                            WYNKContext.SaveChanges();
                        }
                        if (AllotingOtherDrugs.Count > 0)
                        {
                            var CurrentMonth = DateTime.Now.Month;
                            foreach (var item in AllotingOtherDrugs.ToList())
                            {
                                var ItemBalance = WYNKContext.ItemBalance.Where(x => x.FYear == Convert.ToInt32(Fyear) && x.ItemID == item.DrugID && x.StoreID == AddTransfer.SupplierStoreID).FirstOrDefault();
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

                                if (stockTran == null)
                                {
                                    var StkTran = new StockTran();
                                    StkTran.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                                    StkTran.SMID = stockmas.RandomUniqueID;
                                    StkTran.ItemID = Convert.ToInt32(item.DrugID);
                                    var Qty = WYNKContext.StockTran.Where(x => x.SMID == stockmas.RandomUniqueID && x.ItemID == Convert.ToInt32(item.DrugID)).Select(x => x.ItemQty).FirstOrDefault();
                                    StkTran.ItemQty = Qty != null ? Qty + Convert.ToInt32(item.QtyTobeTaken) : Convert.ToInt32(item.QtyTobeTaken);
                                    StkTran.UOMID = uomMaster.Where(UOM => UOM.Description == drugMaster.Where(x => x.ID == Convert.ToInt32(item.DrugID)).Select(x => x.UOM).FirstOrDefault()).Select(UOM => UOM.id).FirstOrDefault();
                                    StkTran.ItemRate = drugMaster.Where(x => x.ID == Convert.ToInt32(item.DrugID)).Select(x => x.Rate).FirstOrDefault();
                                    StkTran.ItemValue = StkTran.ItemQty * StkTran.ItemRate;
                                    StkTran.CreatedUTC = DateTime.UtcNow;
                                    StkTran.CreatedBy = AddTransfer.CreatedBy;
                                    StkTran.RecdQty = 0;
                                    StkTran.DamageQty = 0;
                                    StkTran.OtherQty = 0;
                                    StkTran.LossInTransit = 0;
                                    StkTran.Status = (int)Status.Open;
                                    WYNKContext.StockTran.AddRange(StkTran);
                                    WYNKContext.SaveChanges();
                                }
                                else
                                {
                                    stockTran.ItemQty = stockTran.ItemQty != null ? stockTran.ItemQty + Convert.ToInt32(item.QtyTobeTaken) : Convert.ToInt32(item.QtyTobeTaken);
                                    WYNKContext.StockTran.UpdateRange(stockTran);
                                    WYNKContext.SaveChanges();
                                }
                            }
                            WYNKContext.SaveChanges();
                        }

                        var commonRepos = new CommonRepository(_Wynkcontext, _Cmpscontext);
                        var RunningNumber = commonRepos.GenerateRunningCtrlNoo(AddTransfer.TransactionId, AddTransfer.Cmpid, "GetRunningNo");
                        if (RunningNumber == AddTransfer.RunningNoStock)
                        {
                            commonRepos.GenerateRunningCtrlNoo(AddTransfer.TransactionId, AddTransfer.Cmpid, "UpdateRunningNo");
                        }
                        else
                        {
                            var GetRunningNumber = commonRepos.GenerateRunningCtrlNoo(AddTransfer.TransactionId, AddTransfer.Cmpid, "GetRunningNo");

                            var stockMaster = WYNKContext.StockMaster.Where(x => x.DocumentNumber == AddTransfer.RunningNoStock).FirstOrDefault();
                            stockMaster.DocumentNumber = GetRunningNumber;
                            WYNKContext.StockMaster.UpdateRange(stockMaster);
                            WYNKContext.SaveChanges();
                        }

                        WYNKContext.SaveChanges();
                        dbContextTransaction.Commit();

                        if (WYNKContext.SaveChanges() >= 0)
                            return new
                            {
                                Success = true,
                                CompanyDetails = CMPSContext.Company.Where(x => x.CmpID == AddTransfer.Cmpid).FirstOrDefault(),
                                SupplierStoreName = SupplierStoreName,
                                ReceiverStoreName = ReceiverStoreName,
                                StockTransferNo = AddTransfer.RunningNoStock,
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
                            OutOfStock = AddTransfer.InSufficientDrugs,
                            OutStockSerials = AddTransfer.InSufficientSerials,
                            OutOtherDrugs = AddTransfer.InSufficientOtherDrugs,
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
                        return new { Success = false, Message = msg, grn = AddTransfer.RunningNoStock };
                    }
                    else
                    {
                        ErrorLog oErrorLog = new ErrorLog();
                        oErrorLog.WriteErrorLog("Error", "Something Went Wrong");
                        return new { Success = false, Message = "Something Went Wrong" };
                    }
                }
                return new
                {
                    Success = false,
                    Message = "SomeThing Went Wrong"
                };
            }
        }

        public dynamic InterBranchIsuueDetails(int transactionCode, int Cmpid, string GMT)
        {
            var InterDepartmentTransferDetails = new InterDepartmentTransferDetails();
            var storeMaster = CMPSContext.Storemasters.ToList();
            var CompanyDet = CMPSContext.Company.ToList();
            TimeSpan ts = TimeSpan.Parse(GMT);
            InterDepartmentTransferDetails.InterDepartmentTransferDetail = (from res in WYNKContext.StockMaster.Where(x => x.TransactionID == transactionCode && x.CMPID == Cmpid && x.Status == (int)Status.Open)
                                                                            select new InterDepartmentTransferDetail
                                                                            {
                                                                                FromStore = storeMaster.Where(x => x.StoreID == res.StoreID).Select(x => x.Storename).FirstOrDefault(),
                                                                                ToStore = CompanyDet.Where(x => x.CmpID == res.ReceiverBranchId).Select(x => String.Concat(x.CompanyName, " ", x.LocationName)).FirstOrDefault(),
                                                                                stockTransNo = res.DocumentNumber,
                                                                                Date = (Convert.ToDateTime(res.DocumentDate) + ts).ToString("dd-MMM-yyyy, hh:mm tt"),
                                                                                created = (res.CreatedUTC + ts),
                                                                            }).OrderByDescending(x => x.created).ToList();

            return InterDepartmentTransferDetails;
        }

        public dynamic InterBranchIssueDetailed(string StockTransferNo, int CmpId)
        {
            var StockTransferDetail = new StockTransferDetails1();

            var OpStockMasterDet = WYNKContext.StockMaster.Where(x => x.DocumentNumber == StockTransferNo && x.CMPID == CmpId).FirstOrDefault();

            var SupplierStoreId = WYNKContext.StockMaster.Where(x => x.DocumentNumber == StockTransferNo).Select(x => x.StoreID).FirstOrDefault();
            var ReceiverStoreId = WYNKContext.StockMaster.Where(x => x.DocumentNumber == StockTransferNo).Select(x => x.ReceiverBranchId).FirstOrDefault();

            int cmp = CMPSContext.Company.AsNoTracking().Where(c => c.CmpID == CmpId).Select(c => c.ParentID).FirstOrDefault();
            if (cmp == 0)
            {
                cmp = CmpId;
            }
            var BrandMas = WYNKContext.Brand.ToList();
            var Uom = CMPSContext.uommaster.ToList();

            var UTC = CMPSContext.Setup.Where(x => x.CMPID == CmpId).Select(x => x.UTCTime).FirstOrDefault();
            TimeSpan ts = TimeSpan.Parse(UTC);

            var DrugMaster = WYNKContext.DrugMaster.ToList();
            var DrugGroup = WYNKContext.DrugGroup.ToList();

            var StockMasterId = WYNKContext.StockMaster.Where(x => x.DocumentNumber == StockTransferNo).Select(x => x.RandomUniqueID).FirstOrDefault();

            StockTransferDetail.StockTransferNo = StockTransferNo;
            StockTransferDetail.StockTransferDate = (Convert.ToDateTime(OpStockMasterDet.DocumentDate) + ts).ToString("dd-MMM-yyyy, hh:mm tt");
            StockTransferDetail.RefNo = OpStockMasterDet.RefNo;
            StockTransferDetail.RefDate = OpStockMasterDet.RefDate;
            StockTransferDetail.TransportMode = OpStockMasterDet.TransportationMode;
            StockTransferDetail.TransportationNo = OpStockMasterDet.TransportationNo;
            StockTransferDetail.Transportationdate = OpStockMasterDet.TransportationDate;
            StockTransferDetail.TransportationName = OpStockMasterDet.TransporterName;
            StockTransferDetail.TransportationCharges = OpStockMasterDet.TransportCharges;
            StockTransferDetail.Remarks = OpStockMasterDet.Remarks;
            StockTransferDetail.RandomUniqueId = OpStockMasterDet.RandomUniqueID;
            var Onelinemaster = CMPSContext.OneLineMaster.ToList();

            var StockTrans = WYNKContext.StockTran.ToList();

            StockTransferDetail.DrugDetails = (from res in StockTrans.Where(x => x.SMID == StockMasterId)
                                               select new DrugDetails1
                                               {
                                                   Brand = DrugMaster.Where(x => x.ID == res.ItemID).Select(x => x.Brand).FirstOrDefault(),
                                                   GenericName = DrugGroup.Where(x => x.ID == DrugMaster.Where(y => y.ID == res.ItemID).Select(y => y.GenericName).FirstOrDefault()).Select(x => x.Description).FirstOrDefault(),
                                                   UOM = DrugMaster.Where(x => x.ID == res.ItemID).Select(x => x.UOM).FirstOrDefault(),
                                                   Quantity = res.ItemQty,
                                                   IsSerial = TrackingType(res.ItemID),
                                                   SerialDetails = TrackingType(res.ItemID) == true ? GettingSerialList1(StockMasterId, CmpId, SupplierStoreId, ReceiverStoreId, res.ItemID) : null,
                                                   BatchDetails = TrackingType(res.ItemID) == false ? GettingBatchList(StockMasterId, res.RandomUniqueID, res.ItemID) : null,
                                               }).ToList();

            StockTransferDetail.CompanyDetails = CMPSContext.Company.Where(x => x.CmpID == CmpId).FirstOrDefault();

            return StockTransferDetail;
        }

        private ICollection<SerialDetailInfo> GettingSerialList1(string stockMasterId, int cmpId, int supplierStoreId, int? receiverStoreId, int ItemID)
        {
            var StockRec = WYNKContext.StockMaster.Where(x => x.RandomUniqueID == stockMasterId && x.CMPID == cmpId && x.StoreID == supplierStoreId && x.ReceiverBranchId == receiverStoreId).FirstOrDefault();
            var ItemSerial = WYNKContext.ItemSerial.ToList();

            var res = (from IT in ItemSerial.Where(x => x.IssueNo == StockRec.DocumentNumber && x.StoreID == supplierStoreId && x.ItemID == ItemID)
                       select new SerialDetailInfo
                       {
                           SerialNo = IT.SerialNo,
                       }).ToList();
            return res;
        }

        public dynamic GetItemListDetails(GetItemListDetails GetItemListDetail, string FromDate, string Todate, int CmpId, string GMT)
        {

            try
            {
                int cmp = CMPSContext.Company.AsNoTracking().Where(c => c.CmpID == CmpId).Select(c => c.ParentID).FirstOrDefault();
                if (cmp == 0)
                {
                    cmp = CmpId;
                }
                var BrandMas = WYNKContext.Brand.Where(x => x.cmpID == cmp).ToList();
                var Uom = CMPSContext.uommaster.ToList();

                TimeSpan ts = TimeSpan.Parse(GMT);
                var DrugMaster = WYNKContext.DrugMaster.ToList();
                var DrugGroup = WYNKContext.DrugGroup.ToList();


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
                var CompanyDet = CMPSContext.Company.ToList();
                int StatusValue = (int)Enum.Parse(typeof(Status), GetItemListDetail.Status);
                var Onelinemaster = CMPSContext.OneLineMaster.ToList();

                var res = (from OptSt in WYNKContext.StockMaster.Where(x => x.CMPID == CmpId && x.Status == StatusValue && x.TransactionID == GetItemListDetail.TransactionId && (Convert.ToDateTime(x.DocumentDate) + ts).Date >= fdate && (Convert.ToDateTime(x.DocumentDate) + ts).Date <= tdate)
                           join OptStTran in WYNKContext.StockTran on OptSt.RandomUniqueID equals OptStTran.SMID
                           where GetItemListDetail.SelectedBranches.Contains(Convert.ToInt32(OptSt.ReceiverBranchId))
                           select new
                           {
                               RandomUniqueID = OptSt.RandomUniqueID,
                               DestinationBranch = CompanyDet.Where(x => x.CmpID == OptSt.ReceiverBranchId).Select(x => x.CompanyName).FirstOrDefault(),
                               IssueNo = OptSt.DocumentNumber,
                               IssueDate = OptSt.DocumentDate + ts,
                               Brand = DrugMaster.Where(x => x.ID == OptStTran.ItemID).Select(x => x.Brand).FirstOrDefault(),
                               GenericName = DrugGroup.Where(x => x.ID == DrugMaster.Where(y => y.ID == OptStTran.ItemID).Select(y => y.GenericName).FirstOrDefault()).Select(x => x.Description).FirstOrDefault(),
                               UOM = DrugMaster.Where(x => x.ID == OptStTran.ItemID).Select(x => x.UOM).FirstOrDefault(),
                               SentQty = OptStTran.ItemQty,
                               RecdQty = OptStTran.RecdQty,
                               DamageQty = OptStTran.DamageQty,
                               LossTrsQty = OptStTran.LossInTransit,
                               OtherQty = OptStTran.OtherQty,
                               Status = Enum.GetName(typeof(Status), OptStTran.Status)
                           }).ToList();


                return new { Success = true, res = res };
            }
            catch (Exception ex)
            {
                ErrorLog oErrorLog = new ErrorLog();
                if (ex.InnerException != null)
                {
                    oErrorLog.WriteErrorLog("Error", ex.InnerException.Message);
                }
                else
                {
                    oErrorLog.WriteErrorLog("Error", ex.Message);
                }
                return new { Success = false };
            }
        }

        public dynamic IssueStatusUpdate(IssueStatusDetails IssueStatusDetail, int Cmpid)
        {
            try
            {
                var Stock = WYNKContext.StockMaster.Where(x => x.CMPID == Cmpid && x.RandomUniqueID == IssueStatusDetail.RandomUniqueId).FirstOrDefault();
                Stock.Status = (int)Status.Cancelled;
                Stock.UpdatedBy = IssueStatusDetail.CreatedBy;
                Stock.UpdatedUTC = DateTime.UtcNow;
                WYNKContext.StockMaster.UpdateRange(Stock);

                var StkTran = WYNKContext.StockTran.Where(x => x.SMID == IssueStatusDetail.RandomUniqueId).ToList();
                if (StkTran != null)
                {
                    StkTran.All(x => { x.Status = (int)Status.Cancelled; return true; });
                    WYNKContext.StockTran.UpdateRange(StkTran);
                }

                WYNKContext.SaveChanges();
                return new { Success = true };
            }
            catch (Exception ex)
            {
                ErrorLog oErrorLog = new ErrorLog();
                if (ex.InnerException != null)
                {
                    oErrorLog.WriteErrorLog("Error", ex.InnerException.Message);
                }
                else
                {
                    oErrorLog.WriteErrorLog("Error", ex.Message);
                }
                return new { Success = false };
            }
        }

        public dynamic AddstockDetails(SubmitTransferdetails AddTransfer)
        {
            throw new NotImplementedException();
        }

        public dynamic InterDepartmentTransferDetails(int transactionCode)
        {
            throw new NotImplementedException();
        }

        public dynamic StockTransferDetails(string StockTransferNo, int CmpId)
        {
            throw new NotImplementedException();
        }
    }
}
