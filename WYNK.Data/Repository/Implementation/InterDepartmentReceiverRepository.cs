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
    class InterDepartmentReceiverRepository : RepositoryBase<InterDepartmentReceiver>, IInterDeparmentReceiverRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;

        public InterDepartmentReceiverRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }
        public dynamic GetstoreDropdownvalues(int cmpid)
        {
            var storemasters = CMPSContext.Storemasters.Where(x=>x.CmpID == cmpid).ToList();
            return (from e in WYNKContext.StockMaster.Where(x => x.CMPID == cmpid && x.ReceiverStoreID != null).GroupBy(z => z.ReceiverStoreID)
                       select new Dropdown
                       {
                           Value = e.Select(x => x.ReceiverStoreID).FirstOrDefault().ToString(),
                           Text = storemasters.Where(x => x.StoreID == e.Select(y => y.ReceiverStoreID).FirstOrDefault()).Select(x => x.Storename).FirstOrDefault(),
                       }).ToList();
        }
        public dynamic GetStoreDetails(int ID,int IssueCode,int cmpid)
        {
            var StoreDetail = new InterDepartmentReceiver();
            var UTC = CMPSContext.Setup.Where(x => x.CMPID == cmpid).Select(x=>x.UTCTime).FirstOrDefault();
            TimeSpan ts = TimeSpan.Parse(UTC);
            var storeMaster = CMPSContext.Storemasters.Where(x=>x.CmpID == cmpid).ToList();
            StoreDetail.StoreName = storeMaster.Where(u => u.StoreID == ID).Select(x => x.Storename).FirstOrDefault();
            StoreDetail.ReceivedDetail = (from Stock in WYNKContext.StockMaster.Where(x => x.TransactionID == IssueCode && x.ReceiverStoreID == ID && x.CMPID == cmpid && x.ContraSmid == null)
                                          select new ReceivedDetails
                                          {
                                           Date = Stock.DocumentDate + ts,
                                           StockTransferNo=Stock.DocumentNumber,
                                           SentFromStore= storeMaster.Where(u => u.StoreID == Stock.StoreID).Select(x => x.Storename).FirstOrDefault(), 
                                           StoreKeeper = storeMaster.Where(u => u.StoreID == Stock.StoreID).Select(x => x.StoreKeeper).FirstOrDefault(),
                                          }).ToList();
            return StoreDetail;
        }

        public dynamic GetStoreDetails1(int IssueCode, int cmpid)
        {
            var StoreDetail = new InterDepartmentReceiver();
            var storeMaster = CMPSContext.Storemasters.ToList();
            var Cmp = CMPSContext.Company.ToList();
            var UTC = CMPSContext.Setup.Where(x => x.CMPID == cmpid).Select(x => x.UTCTime).FirstOrDefault();
            TimeSpan ts = TimeSpan.Parse(UTC);
            StoreDetail.ReceivedDetail1 = (from Stock in WYNKContext.StockMaster.Where(x => x.TransactionID == IssueCode && x.ReceiverBranchId == cmpid && x.Status == (int)Status.Open)
                                          select new ReceivedDetails1
                                          {
                                              RandomUniqueId = Stock.RandomUniqueID,
                                              Datestring = (Convert.ToDateTime(Stock.DocumentDate) + ts).ToString("dd-MMM-yyyy, hh:mm tt"),
                                              Date = Convert.ToDateTime(Stock.DocumentDate),
                                              StockTransferNo = Stock.DocumentNumber,
                                              SentFromStore = storeMaster.Where(u => u.StoreID == Stock.StoreID).Select(x => x.Storename).FirstOrDefault(),
                                              StoreKeeper = storeMaster.Where(u => u.StoreID == Stock.StoreID).Select(x => x.StoreKeeper).FirstOrDefault(),
                                              Organization = Cmp.Where(x => x.CmpID == Stock.CMPID).Select(x => x.CompanyName).FirstOrDefault(),
                                              Branch = Cmp.Where(x => x.CmpID == Stock.CMPID).Select(x => x.LocationName).FirstOrDefault(),
                                          }).ToList();
            return StoreDetail;
        }
        public dynamic GetRecDetails(int ID, int RecCode, int cmpid)
        {
            var StoreDetail = new InterDepartmentReceiver();
            var UTC = CMPSContext.Setup.Where(x => x.CMPID == cmpid).Select(x => x.UTCTime).FirstOrDefault();
            TimeSpan ts = TimeSpan.Parse(UTC);
            var storeMaster = CMPSContext.Storemasters.Where(x => x.CmpID == cmpid).ToList();
            StoreDetail.StoreName = storeMaster.Where(u => u.StoreID == ID).Select(x => x.Storename).FirstOrDefault();
            StoreDetail.ReceivedDetail = (from Stock in WYNKContext.StockMaster.Where(x => x.TransactionID == RecCode && x.CMPID == cmpid && x.StoreID == ID)
                                          select new ReceivedDetails
                                          {
                                              Date = Stock.DocumentDate + ts,
                                              StockTransferNo = Stock.DocumentNumber,
                                              ReceivedStore = storeMaster.Where(u => u.StoreID == Stock.StoreID).Select(x => x.Storename).FirstOrDefault(),
                                              Receivedby = Stock.ISSrecdby,
                                          }).ToList();
            return StoreDetail;
        }
        public dynamic GetStockTransferDetails(string StockTransferNo, int cmpid)
        {
            var StockDetails = new InterDepartmentStockDetails();
            var StockMaster = WYNKContext.StockMaster.Where(x => x.DocumentNumber == StockTransferNo && x.CMPID == cmpid).FirstOrDefault();
            var DrugMaster = WYNKContext.DrugMaster.Where(x=>x.Cmpid == cmpid).ToList();
            var DrugGroup = WYNKContext.DrugGroup.ToList();
            var StockTran = WYNKContext.StockTran.ToList();
            StockDetails.SenderstoreId = StockMaster.StoreID;
            StockDetails.Senderdatetime = StockMaster.CreatedUTC;
            StockDetails.SenderUserId = StockMaster.CreatedBy;
            StockDetails.SentSmid = StockMaster.SMID;
            StockDetails.ItemDetails = (from ST in StockTran.Where(x => x.SMID == StockMaster.RandomUniqueID)
                                        select new StockTransferItemDetail
                                        {
                                            ItemId = ST.ItemID,
                                            DrugName = DrugMaster.Where(x => x.ID == ST.ItemID).Select(x => x.Brand).FirstOrDefault(),
                                            GenericName = DrugGroup.Where(dg => dg.ID == DrugMaster.Where(y => y.ID == ST.ItemID).Select(y => y.GenericName).FirstOrDefault()).Select(dg =>dg.Description).FirstOrDefault(),
                                            UOM =  DrugMaster.Where(y => y.ID == ST.ItemID).Select(y => y.UOM).FirstOrDefault(),
                                            TotalQuantity = Convert.ToInt32(ST.ItemQty),
                                            SMID = ST.SMID,
                                            STID = ST.RandomUniqueID,
                                            IsSerial = TrackingType(ST.ItemID),
                                            ItemReceivedBatchDetails = TrackingType(ST.ItemID) == false ? GetReceivedBatchDetails(ST.SMID, ST.RandomUniqueID, ST.ItemID) : null,
                                            ItemReceivedSerialDetails = TrackingType(ST.ItemID) == true ? GetReceivedSerialDetails(ST.SMID, ST.STID, ST.ItemID, cmpid, StockMaster.StoreID, StockMaster.ReceiverStoreID) : null,
                                            ItemReceivedOtherDetails = TrackingType(ST.ItemID) == null ? GetReceivedOtherDetails(Convert.ToInt32(ST.ItemQty)):null,
                                        }).ToList();

            StockDetails.fullItemsReceivedDetails = new List<fullItemsReceivedDetail>();
            foreach (var item in StockDetails.ItemDetails.ToList())
            {

               if(item.ItemReceivedBatchDetails != null) 
                {
                    foreach (var IB in item.ItemReceivedBatchDetails.ToList())
                    {
                        StockDetails.fullItemsReceivedDetails.Add(new fullItemsReceivedDetail()
                        {
                            ItemId = item.ItemId,
                            DrugName = item.DrugName,
                            GenericName = item.GenericName,
                            UOM = item.UOM,
                            SMID = item.SMID,
                            STID = item.STID,
                            IsSerial = item.IsSerial,
                            BatchSerial = IB.ItemBatchNo,
                            ItemBatchID = IB.ItemBatchID,
                            SentQuantity = IB.SentQuantity,
                            RecQuantity = IB.SentQuantity,
                            Difference=0,
                            BatchExpiry = IB.BatchExpiry,
                            Reasons = IB.Reasons,
                        });
                    }
                }

                if (item.ItemReceivedSerialDetails !=null)
                {
                    foreach (var IS in item.ItemReceivedSerialDetails.ToList())
                    {
                        StockDetails.fullItemsReceivedDetails.Add(new fullItemsReceivedDetail()
                        {
                            ItemId = item.ItemId,
                            DrugName = item.DrugName,
                            GenericName = item.GenericName,
                            SMID = item.SMID,
                            STID = item.STID,
                            IsSerial = item.IsSerial,
                            BatchSerial = IS.SerialNo,
                            BatchExpiry = IS.ExpiryDate,
                            SentQuantity = 1,
                            RecQuantity = 1,
                            Difference = 0,
                            Reasons = IS.Reasons,
                        });
                    }
                }


                if (item.ItemReceivedOtherDetails != null)
                {
                    foreach (var IO in item.ItemReceivedOtherDetails.ToList())
                    {
                        StockDetails.fullItemsReceivedDetails.Add(new fullItemsReceivedDetail()
                        {
                            ItemId = item.ItemId,
                            DrugName = item.DrugName,
                            GenericName = item.GenericName,
                            SMID = item.SMID,
                            STID = item.STID,
                            IsSerial = item.IsSerial,
                            SentQuantity = Convert.ToInt32(IO.SentQty),
                            RecQuantity = IO.SentQty,
                            Difference = 0,
                            Reasons = "",
                        });
                    }
                }
            }
            return StockDetails;
        }
        public dynamic GetRecStockTransferDetails(string StockTransferNo, int cmpid, string GMT)
        {
            var StockDetails = new ViewRecStockDetails();
            TimeSpan ts = TimeSpan.Parse(GMT);
            var StockMaster = WYNKContext.StockMaster.Where(x => x.DocumentNumber == StockTransferNo && x.CMPID == cmpid).FirstOrDefault();
            var DrugMaster = WYNKContext.DrugMaster.Where(x => x.Cmpid == cmpid).ToList();
            var DrugGroup = WYNKContext.DrugGroup.ToList();
            var StockTran = WYNKContext.StockTran.ToList();
            StockDetails.Recdatetime = StockMaster.DocumentDate + ts;
            StockDetails.RecName = StockMaster.ISSrecdby;
            StockDetails.RunningNoStock = StockMaster.DocumentNumber;
            StockDetails.CompanyDetails = CMPSContext.Company.Where(x => x.CmpID == cmpid).FirstOrDefault();

            StockDetails.ItemDetails = (from ST in StockTran.Where(x => x.SMID == StockMaster.RandomUniqueID)
                                        select new StockTransferItemDetail
                                        {
                                            ItemId = ST.ItemID,
                                            DrugName = DrugMaster.Where(x => x.ID == ST.ItemID).Select(x => x.Brand).FirstOrDefault(),
                                            GenericName = DrugGroup.Where(dg => dg.ID == DrugMaster.Where(y => y.ID == ST.ItemID).Select(y => y.GenericName).FirstOrDefault()).Select(dg => dg.Description).FirstOrDefault(),
                                            TotalQuantity = Convert.ToInt32(ST.ItemQty),
                                            SMID = ST.SMID,
                                            STID = ST.RandomUniqueID,
                                            IsSerial = TrackingType(ST.ItemID),
                                            ItemReceivedBatchDetails = TrackingType(ST.ItemID) == false ? GetReceivedBatchDetails(ST.SMID, ST.RandomUniqueID, ST.ItemID) : null,
                                            ItemReceivedSerialDetails = TrackingType(ST.ItemID) == true ? GetReceivedSerialDetails(ST.SMID, ST.STID, ST.ItemID, cmpid, StockMaster.StoreID, StockMaster.ReceiverStoreID) : null,
                                            ItemReceivedOtherDetails = TrackingType(ST.ItemID) == null ? GetReceivedOtherDetails(Convert.ToInt32(ST.ItemQty)) : null,
                                        }).ToList();

            StockDetails.fullItemsReceivedDetails = new List<fullItemsReceivedDetail>();

            foreach (var item in StockDetails.ItemDetails.ToList())
            {

                if (item.ItemReceivedBatchDetails != null)
                {
                    foreach (var IB in item.ItemReceivedBatchDetails.ToList())
                    {
                        StockDetails.fullItemsReceivedDetails.Add(new fullItemsReceivedDetail()
                        {
                            ItemId = item.ItemId,
                            DrugName = item.DrugName,
                            GenericName = item.GenericName,
                            SMID = item.SMID,
                            STID = item.STID,
                            IsSerial = item.IsSerial,
                            BatchSerial = IB.ItemBatchNo,
                            ItemBatchID = IB.ItemBatchID,
                            SentQuantity = IB.SentQuantity,
                            RecQuantity = IB.SentQuantity,
                            Difference = 0,
                            BatchExpiry = IB.BatchExpiry,
                            Reasons = IB.Reasons,
                        });
                    }
                }

                if (item.ItemReceivedSerialDetails != null)
                {
                    foreach (var IS in item.ItemReceivedSerialDetails.ToList())
                    {
                        StockDetails.fullItemsReceivedDetails.Add(new fullItemsReceivedDetail()
                        {
                            ItemId = item.ItemId,
                            DrugName = item.DrugName,
                            GenericName = item.GenericName,
                            SMID = item.SMID,
                            STID = item.STID,
                            IsSerial = item.IsSerial,
                            BatchSerial = IS.SerialNo,
                            BatchExpiry = IS.ExpiryDate,
                            SentQuantity = 1,
                            RecQuantity = 1,
                            Difference = 0,
                            Reasons = IS.Reasons,
                        });
                    }
                }


                if (item.ItemReceivedOtherDetails != null)
                {
                    foreach (var IO in item.ItemReceivedOtherDetails.ToList())
                    {
                        StockDetails.fullItemsReceivedDetails.Add(new fullItemsReceivedDetail()
                        {
                            ItemId = item.ItemId,
                            DrugName = item.DrugName,
                            GenericName = item.GenericName,
                            SMID = item.SMID,
                            STID = item.STID,
                            IsSerial = item.IsSerial,
                            SentQuantity = Convert.ToInt32(IO.SentQty),
                            RecQuantity = IO.SentQty,
                            Difference = 0,
                            Reasons = "",
                        });
                    }
                }
            }

            return StockDetails;
        }
        private ICollection<ReceivedBatchDetail> GetReceivedBatchDetails(string stockMasterId, string sTID, int itemID)
        {
            var ItemBatchtrans = WYNKContext.ItemBatchTrans.ToList();
            var res = (from re in ItemBatchtrans.Where(x => x.SMID == stockMasterId && x.STID == sTID && x.ItemID == itemID)
                       select new ReceivedBatchDetail
                       {
                           ItemBatchID = re.ItemBatchID,
                           ItemBatchNo = re.ItemBatchNumber,
                           SentQuantity = Convert.ToInt32(re.ItemBatchTransactedQty),
                           BatchExpiry = re.ItemBatchExpiry,
                       }).ToList();
            return res;
        }
        private ICollection<ReceivedSerialDetail> GetReceivedSerialDetails(string stockMasterId, long sTID, int itemID,int cmpId, int supplierStoreId, int? ReceiverStoreID)
        {
            var StockRec = WYNKContext.StockMaster.Where(x => x.RandomUniqueID == stockMasterId && x.CMPID == cmpId && x.StoreID == supplierStoreId && x.ReceiverStoreID == ReceiverStoreID).FirstOrDefault();
            var ItemSerial = WYNKContext.ItemSerial.Where(x=>x.cmpID == cmpId).ToList();
            var res = (from IT in ItemSerial.Where(x => x.IssueNo == StockRec.DocumentNumber && x.StoreID == supplierStoreId && x.ItemID == itemID)
                       select new ReceivedSerialDetail
                       {
                           SerialNo = IT.SerialNo,
                           ExpiryDate = IT.ExpiryDate,
                       }).ToList();
            return res;
        }
        private List<ReceivedOtherDetail> GetReceivedOtherDetails(int qty)
        {
            var ReceivedOtherDetail = new List<ReceivedOtherDetail>();
            var res = new ReceivedOtherDetail
                       {
                        SentQty = qty,
                        Difference = 0,
                        Recqty = 0,
                        Reasons = " ",
                       };
            ReceivedOtherDetail.Add(res);
            return ReceivedOtherDetail;
        }
        public dynamic AddReceivedStockDetails(InterDepartmentStockDetails AddStock)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {

                try
                {
                    var SenderstoreId = AddStock.SenderstoreId;
                    var SenderUserId = AddStock.SenderUserId;
                    var Senderdatetime = AddStock.Senderdatetime;

                    var TransactionID = AddStock.TransactionID;
                    var Cmpid = AddStock.cmpid;
                    var createdby = AddStock.CreatedBy;
                    var CurrentMonth = DateTime.Now.Month;
                    var DrugMaster = WYNKContext.DrugMaster.Where(x => x.Cmpid == AddStock.cmpid).ToList();

                    var UTC = CMPSContext.Setup.Where(x => x.CMPID == Cmpid).Select(x => x.UTCTime).FirstOrDefault();
                    TimeSpan ts = TimeSpan.Parse(UTC);


                    DateTime GivenDate;
                    var appdate = DateTime.TryParseExact(AddStock.Recdatetime.Trim(), "dd-MM-yyyy,HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out GivenDate);
                    {
                        GivenDate.ToString("dd-MM-yyyy,HH:mm", CultureInfo.InvariantCulture);
                    }
                    GivenDate = GivenDate - ts;


                    if (AddStock.fullItemsReceivedDetails.Count >= 1)
                    {
                        var Fyear = Convert.ToString(WYNKContext.FinancialYear.Where(x => x.FYAccYear == (Convert.ToInt16(AddStock.FYear)) && x.CMPID == Cmpid && x.IsActive == true).Select(x => x.ID).FirstOrDefault());
                        var Fyear1 = Convert.ToString(WYNKContext.FinancialYear.Where(x => x.FYAccYear == (Convert.ToInt16(AddStock.FYear)) && x.CMPID == Cmpid && x.IsActive == true).Select(x => x.FYAccYear).FirstOrDefault());

                        var stockmas = AddBilling.AddstockMaster1(AddStock.RunningNoStock, GivenDate, AddStock.storeId, null, 0, TransactionID, CMPSContext.TransactionType.Where(x => x.TransactionID == AddStock.TransactionID).Select(x => x.Rec_Issue_type).FirstOrDefault(), AddStock.cmpid, createdby, Fyear1);
                        stockmas.ISSrecdby = AddStock.RecName;
                        WYNKContext.StockMaster.Add(stockmas);
                        WYNKContext.SaveChanges();

                        foreach (var item in AddStock.fullItemsReceivedDetails.ToList())
                        {
                            var Uom = CMPSContext.uommaster.Where(u => u.Description == DrugMaster.Where(x => x.ID == item.ItemId).Select(x => x.UOM).FirstOrDefault()).Select(x => x.id).FirstOrDefault();
                            if (item.IsSerial == false)
                            {
                             
                                if (item.Difference != 0 && item.Difference != null)
                                {
                                    var itembatchDiff = new itembatchdiff();
                                    itembatchDiff.Cmpid = Cmpid;
                                    itembatchDiff.Smid = stockmas.RandomUniqueID;
                                    itembatchDiff.Itemid = item.ItemId;
                                    itembatchDiff.ItembatchId = item.ItemBatchID;
                                    itembatchDiff.itembatchno = item.BatchSerial;
                                    itembatchDiff.Diffquantity = Convert.ToInt32(item.Difference);
                                    itembatchDiff.reasons = item.Reasons;
                                    itembatchDiff.issuestoreid = Convert.ToInt32(SenderstoreId);
                                    itembatchDiff.issueuserid = Convert.ToInt32(SenderUserId);
                                    itembatchDiff.issuedateandtime = Convert.ToDateTime(Senderdatetime);
                                    itembatchDiff.recstoreid = AddStock.storeId;
                                    itembatchDiff.recuserid = AddStock.CreatedBy;
                                    itembatchDiff.recdateandtime = DateTime.Now;
                                    WYNKContext.itembatchdiff.Add(itembatchDiff);
                                    WYNKContext.SaveChanges();
                                }

                                    if (item.RecQuantity > 0)
                                    {
                                        var ItemBatch = WYNKContext.ItemBatch.Where(ib => ib.ItemID == item.ItemId && ib.ItemBatchNumber == item.BatchSerial && ib.StoreID == AddStock.storeId && ib.ItemBatchExpiry == item.BatchExpiry).FirstOrDefault();
                                        if (ItemBatch != null)
                                        {
                                            
                                            ItemBatch.ItemBatchQty = ItemBatch.ItemBatchQty + Convert.ToDecimal(item.RecQuantity);
                                            ItemBatch.ItemBatchBalnceQty = ItemBatch.ItemBatchBalnceQty + Convert.ToDecimal(item.RecQuantity);
                                            ItemBatch.UpdatedBy = createdby;
                                            ItemBatch.UpdatedUTC = DateTime.UtcNow;
                                            WYNKContext.ItemBatch.UpdateRange(ItemBatch);
                                            WYNKContext.SaveChanges();
                                            var ItemBalance = WYNKContext.ItemBalance.Where(x => x.ItemID == item.ItemId && x.FYear == Convert.ToInt32(Fyear) && x.StoreID == AddStock.storeId && x.CmpID == AddStock.cmpid).FirstOrDefault();
                                            if (ItemBalance != null)
                                            {
                                                switch (CurrentMonth)
                                                {
                                                    case 1:
                                                        ItemBalance.Rec01 = ItemBalance.Rec01 + Convert.ToDecimal(item.RecQuantity);
                                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Convert.ToDecimal(item.RecQuantity);
                                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                        break;
                                                    case 2:
                                                        ItemBalance.Rec02 = ItemBalance.Iss02 + Convert.ToDecimal(item.RecQuantity);
                                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Convert.ToDecimal(item.RecQuantity);
                                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                        break;
                                                    case 3:
                                                        ItemBalance.Rec03 = ItemBalance.Iss03 + Convert.ToDecimal(item.RecQuantity);
                                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Convert.ToDecimal(item.RecQuantity);
                                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                        break;
                                                    case 4:
                                                        ItemBalance.Rec04 = ItemBalance.Rec04 + Convert.ToDecimal(item.RecQuantity);
                                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Convert.ToDecimal(item.RecQuantity);
                                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                        break;
                                                    case 5:
                                                        ItemBalance.Rec05 = ItemBalance.Rec05 + Convert.ToDecimal(item.RecQuantity);
                                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Convert.ToDecimal(item.RecQuantity);
                                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                        break;
                                                    case 6:
                                                        ItemBalance.Rec06 = ItemBalance.Rec06 + Convert.ToDecimal(item.RecQuantity);
                                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Convert.ToDecimal(item.RecQuantity);
                                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                        break;
                                                    case 7:
                                                        ItemBalance.Rec07 = ItemBalance.Rec07 + Convert.ToDecimal(item.RecQuantity);
                                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Convert.ToDecimal(item.RecQuantity);
                                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                        break;
                                                    case 8:
                                                        ItemBalance.Rec08 = ItemBalance.Rec08 + Convert.ToDecimal(item.RecQuantity);
                                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Convert.ToDecimal(item.RecQuantity);
                                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                        break;
                                                    case 9:
                                                        ItemBalance.Rec09 = ItemBalance.Rec09 + Convert.ToDecimal(item.RecQuantity);
                                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Convert.ToDecimal(item.RecQuantity);
                                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                        break;
                                                    case 10:
                                                        ItemBalance.Rec10 = ItemBalance.Rec10 + Convert.ToDecimal(item.RecQuantity);
                                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Convert.ToDecimal(item.RecQuantity);
                                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                        break;
                                                    case 11:
                                                        ItemBalance.Rec11 = ItemBalance.Rec11 + Convert.ToDecimal(item.RecQuantity);
                                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Convert.ToDecimal(item.RecQuantity);
                                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                        break;
                                                    case 12:
                                                        ItemBalance.Rec12 = ItemBalance.Rec12 + Convert.ToDecimal(item.RecQuantity);
                                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Convert.ToDecimal(item.RecQuantity);
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
                                                        ItemBala.Rec01 = Convert.ToDecimal(item.RecQuantity);
                                                        break;
                                                    case 2:
                                                        ItemBala.Rec02 = Convert.ToDecimal(item.RecQuantity);
                                                        break;
                                                    case 3:
                                                        ItemBala.Rec03 = Convert.ToDecimal(item.RecQuantity);
                                                        break;
                                                    case 4:
                                                        ItemBala.Rec04 = Convert.ToDecimal(item.RecQuantity);
                                                        break;
                                                    case 5:
                                                        ItemBala.Rec05 = Convert.ToDecimal(item.RecQuantity);
                                                        break;
                                                    case 6:
                                                        ItemBala.Rec06 = Convert.ToDecimal(item.RecQuantity);
                                                        break;
                                                    case 7:
                                                        ItemBala.Rec07 = Convert.ToDecimal(item.RecQuantity);
                                                        break;
                                                    case 8:
                                                        ItemBala.Rec08 = Convert.ToDecimal(item.RecQuantity);
                                                        break;
                                                    case 9:
                                                        ItemBala.Rec09 = Convert.ToDecimal(item.RecQuantity);
                                                        break;
                                                    case 10:
                                                        ItemBala.Rec10 = Convert.ToDecimal(item.RecQuantity);
                                                        break;
                                                    case 11:
                                                        ItemBala.Rec11 = Convert.ToDecimal(item.RecQuantity);
                                                        break;
                                                    case 12:
                                                        ItemBala.Rec12 = Convert.ToDecimal(item.RecQuantity);
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
                                                ItemBala.UOMID = Uom;
                                                ItemBala.FYear = Convert.ToInt32(Fyear);
                                                ItemBala.OpeningBalance = 0;
                                                ItemBala.StoreID = AddStock.storeId;
                                                ItemBala.ClosingBalance = Convert.ToDecimal(item.RecQuantity);
                                                ItemBala.CreatedBy = createdby;
                                                ItemBala.CreatedUTC = DateTime.UtcNow;
                                                ItemBala.CmpID = Cmpid;
                                                WYNKContext.ItemBalance.Add(ItemBala);
                                            }

                                            WYNKContext.SaveChanges();
                                            var StockTrans = WYNKContext.StockTran.Where(x => x.SMID == stockmas.RandomUniqueID && x.ItemID == item.ItemId).FirstOrDefault();
                                            if (StockTrans == null)
                                            {
                                                var stockTran = new StockTran();
                                                stockTran.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                                                stockTran.SMID = stockmas.RandomUniqueID;
                                                stockTran.ItemID = item.ItemId;
                                                stockTran.ItemQty = Convert.ToDecimal(item.RecQuantity);
                                                stockTran.CreatedUTC = DateTime.UtcNow;
                                                stockTran.CreatedBy = createdby;

                                                stockTran.ContraSMID = item.SMID;
                                                stockTran.ContraSTID = item.STID;
                                                stockTran.ContraQty = WYNKContext.StockTran.Where(x => x.SMID == item.SMID && x.RandomUniqueID == item.STID && x.ItemID == item.ItemId).Select(x=>x.ItemQty).FirstOrDefault();

                                                WYNKContext.StockTran.Add(stockTran);
                                                WYNKContext.SaveChanges();

                                                var itemBatchTrans = new ItemBatchTrans();
                                                itemBatchTrans.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                                                itemBatchTrans.ItemBatchID = ItemBatch.RandomUniqueID;
                                                itemBatchTrans.TC = TransactionID;
                                                itemBatchTrans.SMID = stockmas.RandomUniqueID;
                                                itemBatchTrans.STID = stockTran.RandomUniqueID;
                                                itemBatchTrans.ItemID = item.ItemId;
                                                itemBatchTrans.ItemBatchNumber = ItemBatch.ItemBatchNumber;
                                                itemBatchTrans.ItemBatchTransactedQty = Convert.ToDecimal(item.RecQuantity);
                                                itemBatchTrans.ItemBatchExpiry = Convert.ToDateTime(item.BatchExpiry);
                                                itemBatchTrans.UOMID = Uom;
                                                itemBatchTrans.ContraItemBatchID = item.ItemBatchID;
                                                itemBatchTrans.CreatedUTC = DateTime.UtcNow;
                                                itemBatchTrans.CreatedBy = createdby;
                                                itemBatchTrans.cmpID = Cmpid;
                                                WYNKContext.ItemBatchTrans.Add(itemBatchTrans);
                                                WYNKContext.SaveChanges();
                                            }
                                            else
                                            {
                                                StockTrans.ItemQty = StockTrans.ItemQty != null ? StockTrans.ItemQty + Convert.ToDecimal(item.RecQuantity) : Convert.ToDecimal(item.RecQuantity);
                                                StockTrans.ContraSMID = item.SMID;
                                                StockTrans.ContraSTID = item.STID;
                                                WYNKContext.StockTran.UpdateRange(StockTrans);
                                                WYNKContext.SaveChanges();


                                                var itemBatchTrans = new ItemBatchTrans();
                                                itemBatchTrans.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                                                itemBatchTrans.ItemBatchID = ItemBatch.RandomUniqueID;
                                                itemBatchTrans.TC = TransactionID;
                                                itemBatchTrans.SMID = stockmas.RandomUniqueID;
                                                itemBatchTrans.STID = StockTrans.RandomUniqueID;
                                                itemBatchTrans.ItemID = item.ItemId;
                                                itemBatchTrans.ItemBatchNumber = ItemBatch.ItemBatchNumber;
                                                itemBatchTrans.ItemBatchTransactedQty = Convert.ToDecimal(item.RecQuantity);
                                                itemBatchTrans.ItemBatchExpiry = Convert.ToDateTime(item.BatchExpiry);
                                                itemBatchTrans.UOMID = Uom;
                                                itemBatchTrans.ContraItemBatchID = item.ItemBatchID;
                                                itemBatchTrans.CreatedUTC = DateTime.UtcNow;
                                                itemBatchTrans.CreatedBy = createdby;
                                                itemBatchTrans.cmpID = Cmpid;
                                                WYNKContext.ItemBatchTrans.Add(itemBatchTrans);
                                                WYNKContext.SaveChanges();
                                            }
                                        }
                                        else
                                        {
                                            /*If  batch does not exist means insert batch  */

                                            var itemBatch = new ItemBatch();
                                            itemBatch.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                                            itemBatch.ItemID = item.ItemId;
                                            itemBatch.ItemBatchNumber = item.BatchSerial;
                                            itemBatch.ItemBatchQty = Convert.ToDecimal(item.RecQuantity);
                                            itemBatch.ItemBatchBalnceQty = Convert.ToDecimal(item.RecQuantity);
                                            itemBatch.ItemBatchExpiry = Convert.ToDateTime(item.BatchExpiry);
                                            itemBatch.StoreID = AddStock.storeId;
                                            itemBatch.CreatedUTC = DateTime.UtcNow;
                                            itemBatch.CreatedBy = createdby;
                                            itemBatch.cmpID = Cmpid;
                                            WYNKContext.ItemBatch.Add(itemBatch);
                                            WYNKContext.SaveChanges();


                                            var ItemBalance = WYNKContext.ItemBalance.Where(x => x.ItemID == item.ItemId && x.FYear == Convert.ToInt32(Fyear) && x.StoreID == AddStock.storeId && x.CmpID == AddStock.cmpid).FirstOrDefault();
                                            if (ItemBalance != null)
                                            {

                                                switch (CurrentMonth)
                                                {
                                                    case 1:
                                                        ItemBalance.Rec01 = ItemBalance.Rec01 + Convert.ToDecimal(item.RecQuantity);
                                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Convert.ToDecimal(item.RecQuantity);
                                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                        break;
                                                    case 2:
                                                        ItemBalance.Rec02 = ItemBalance.Iss02 + Convert.ToDecimal(item.RecQuantity);
                                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Convert.ToDecimal(item.RecQuantity);
                                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                        break;
                                                    case 3:
                                                        ItemBalance.Rec03 = ItemBalance.Iss03 + Convert.ToDecimal(item.RecQuantity);
                                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Convert.ToDecimal(item.RecQuantity);
                                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                        break;
                                                    case 4:
                                                        ItemBalance.Rec04 = ItemBalance.Rec04 + Convert.ToDecimal(item.RecQuantity);
                                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Convert.ToDecimal(item.RecQuantity);
                                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                        break;
                                                    case 5:
                                                        ItemBalance.Rec05 = ItemBalance.Rec05 + Convert.ToDecimal(item.RecQuantity);
                                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Convert.ToDecimal(item.RecQuantity);
                                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                        break;
                                                    case 6:
                                                        ItemBalance.Rec06 = ItemBalance.Rec06 + Convert.ToDecimal(item.RecQuantity);
                                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Convert.ToDecimal(item.RecQuantity);
                                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                        break;
                                                    case 7:
                                                        ItemBalance.Rec07 = ItemBalance.Rec07 + Convert.ToDecimal(item.RecQuantity);
                                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Convert.ToDecimal(item.RecQuantity);
                                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                        break;
                                                    case 8:
                                                        ItemBalance.Rec08 = ItemBalance.Rec08 + Convert.ToDecimal(item.RecQuantity);
                                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Convert.ToDecimal(item.RecQuantity);
                                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                        break;
                                                    case 9:
                                                        ItemBalance.Rec09 = ItemBalance.Rec09 + Convert.ToDecimal(item.RecQuantity);
                                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Convert.ToDecimal(item.RecQuantity);
                                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                        break;
                                                    case 10:
                                                        ItemBalance.Rec10 = ItemBalance.Rec10 + Convert.ToDecimal(item.RecQuantity);
                                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Convert.ToDecimal(item.RecQuantity);
                                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                        break;
                                                    case 11:
                                                        ItemBalance.Rec11 = ItemBalance.Rec11 + Convert.ToDecimal(item.RecQuantity);
                                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Convert.ToDecimal(item.RecQuantity);
                                                        WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                        break;
                                                    case 12:
                                                        ItemBalance.Rec12 = ItemBalance.Rec12 + Convert.ToDecimal(item.RecQuantity);
                                                        ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Convert.ToDecimal(item.RecQuantity);
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
                                                        ItemBala.Rec01 = Convert.ToDecimal(item.RecQuantity);
                                                        break;
                                                    case 2:
                                                        ItemBala.Rec02 = Convert.ToDecimal(item.RecQuantity);
                                                        break;
                                                    case 3:
                                                        ItemBala.Rec03 = Convert.ToDecimal(item.RecQuantity);
                                                        break;
                                                    case 4:
                                                        ItemBala.Rec04 = Convert.ToDecimal(item.RecQuantity);
                                                        break;
                                                    case 5:
                                                        ItemBala.Rec05 = Convert.ToDecimal(item.RecQuantity);
                                                        break;
                                                    case 6:
                                                        ItemBala.Rec06 = Convert.ToDecimal(item.RecQuantity);
                                                        break;
                                                    case 7:
                                                        ItemBala.Rec07 = Convert.ToDecimal(item.RecQuantity);
                                                        break;
                                                    case 8:
                                                        ItemBala.Rec08 = Convert.ToDecimal(item.RecQuantity);
                                                        break;
                                                    case 9:
                                                        ItemBala.Rec09 = Convert.ToDecimal(item.RecQuantity);
                                                        break;
                                                    case 10:
                                                        ItemBala.Rec10 = Convert.ToDecimal(item.RecQuantity);
                                                        break;
                                                    case 11:
                                                        ItemBala.Rec11 = Convert.ToDecimal(item.RecQuantity);
                                                        break;
                                                    case 12:
                                                        ItemBala.Rec12 = Convert.ToDecimal(item.RecQuantity);
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
                                                ItemBala.UOMID = Uom;
                                                ItemBala.FYear = Convert.ToInt32(Fyear);
                                                ItemBala.OpeningBalance = 0;
                                                ItemBala.ItemID = item.ItemId;
                                                ItemBala.StoreID = AddStock.storeId;
                                                ItemBala.ClosingBalance = Convert.ToDecimal(item.RecQuantity);
                                                ItemBala.CreatedBy = createdby;
                                                ItemBala.CreatedUTC = DateTime.UtcNow;
                                                ItemBala.CmpID = Cmpid;
                                                WYNKContext.ItemBalance.Add(ItemBala);
                                            }
                                             WYNKContext.SaveChanges();

                                            var StockTrans = WYNKContext.StockTran.Where(x => x.SMID == stockmas.RandomUniqueID && x.ItemID == item.ItemId).FirstOrDefault();
                                            if (StockTrans == null)
                                            {
                                                var stockTran = new StockTran();
                                                stockTran.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                                                stockTran.SMID = stockmas.RandomUniqueID;
                                                stockTran.ItemID = item.ItemId;
                                                stockTran.ItemQty = Convert.ToDecimal(item.RecQuantity);
                                                stockTran.ContraSMID = item.SMID;
                                                stockTran.ContraSTID = item.STID;
                                                stockTran.ContraQty = WYNKContext.StockTran.Where(x => x.SMID == item.SMID && x.RandomUniqueID == item.STID && x.ItemID == item.ItemId).Select(x => x.ItemQty).FirstOrDefault();
                                                stockTran.CreatedUTC = DateTime.UtcNow;
                                                stockTran.CreatedBy = createdby;
                                                WYNKContext.StockTran.Add(stockTran);
                                                WYNKContext.SaveChanges();

                                                var itemBatchTrans = new ItemBatchTrans();
                                                itemBatchTrans.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                                                itemBatchTrans.ItemBatchID = itemBatch.RandomUniqueID;
                                                itemBatchTrans.TC = TransactionID;
                                                itemBatchTrans.SMID = stockmas.RandomUniqueID;
                                                itemBatchTrans.STID = stockTran.RandomUniqueID;
                                                itemBatchTrans.ItemID = item.ItemId;
                                                itemBatchTrans.ItemBatchNumber = itemBatch.ItemBatchNumber;
                                                itemBatchTrans.ItemBatchTransactedQty = Convert.ToDecimal(item.RecQuantity);
                                                itemBatchTrans.ItemBatchExpiry = Convert.ToDateTime(item.BatchExpiry);
                                                itemBatchTrans.UOMID = Uom;
                                                itemBatchTrans.ContraItemBatchID = item.ItemBatchID;
                                                itemBatchTrans.CreatedUTC = DateTime.UtcNow;
                                                itemBatchTrans.CreatedBy = createdby;
                                                itemBatchTrans.cmpID = Cmpid;
                                                WYNKContext.ItemBatchTrans.Add(itemBatchTrans);
                                                WYNKContext.SaveChanges();
                                            }
                                            else
                                            {
                                                StockTrans.ItemQty = StockTrans.ItemQty != null ? StockTrans.ItemQty + Convert.ToDecimal(item.RecQuantity) : Convert.ToDecimal(item.RecQuantity);
                                                WYNKContext.StockTran.UpdateRange(StockTrans);
                                                WYNKContext.SaveChanges();


                                                var itemBatchTrans = new ItemBatchTrans();
                                                itemBatchTrans.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                                                itemBatchTrans.ItemBatchID = itemBatch.RandomUniqueID;
                                                itemBatchTrans.TC = TransactionID;
                                                itemBatchTrans.SMID = stockmas.RandomUniqueID;
                                                itemBatchTrans.STID = StockTrans.RandomUniqueID;
                                                itemBatchTrans.ItemID = item.ItemId;
                                                itemBatchTrans.ItemBatchNumber = itemBatch.ItemBatchNumber;
                                                itemBatchTrans.ItemBatchTransactedQty = Convert.ToDecimal(item.RecQuantity);
                                                itemBatchTrans.ItemBatchExpiry =Convert.ToDateTime(item.BatchExpiry);
                                                itemBatchTrans.UOMID = Uom;
                                                itemBatchTrans.ContraItemBatchID = item.ItemBatchID;
                                                itemBatchTrans.CreatedUTC = DateTime.UtcNow;
                                                itemBatchTrans.CreatedBy = createdby;
                                                itemBatchTrans.cmpID = Cmpid;
                                                WYNKContext.ItemBatchTrans.Add(itemBatchTrans);
                                                WYNKContext.SaveChanges();
                                            }
                                        }
                                    }
                          
                            }
                            else if (item.IsSerial == true)
                            {
                                    if (item.Difference != 0 && item.Difference != null)
                                    {
                                        var itembatchDiff = new itembatchdiff();
                                        itembatchDiff.Cmpid = Cmpid;
                                        itembatchDiff.Smid = stockmas.RandomUniqueID;
                                        itembatchDiff.Itemid = item.ItemId;
                                        itembatchDiff.serialno = item.BatchSerial;
                                        itembatchDiff.Diffquantity = Convert.ToInt32(item.Difference);
                                        itembatchDiff.reasons = item.Reasons;
                                        itembatchDiff.issuestoreid = Convert.ToInt32(SenderstoreId);
                                        itembatchDiff.issueuserid = Convert.ToInt32(SenderUserId);
                                        itembatchDiff.issuedateandtime = Convert.ToDateTime(Senderdatetime);
                                        itembatchDiff.recstoreid = AddStock.storeId;
                                        itembatchDiff.recuserid = AddStock.CreatedBy;
                                        itembatchDiff.recdateandtime = DateTime.Now;
                                        WYNKContext.itembatchdiff.Add(itembatchDiff);
                                        WYNKContext.SaveChanges();
                                     }
                                    if (item.Difference == 0 || item.Difference == null)
                                    {
                                        var ItemSerial = new ItemSerial();
                                        ItemSerial.ItemID = item.ItemId;
                                        ItemSerial.SerialNo = item.BatchSerial;
                                        ItemSerial.GRNNo = AddStock.RunningNoStock;
                                        ItemSerial.TC = TransactionID;
                                        ItemSerial.IsCancelled = false;
                                        ItemSerial.ExpiryDate = item.BatchExpiry;
                                        ItemSerial.StoreID = AddStock.storeId;
                                        ItemSerial.CreatedBy = createdby;
                                        ItemSerial.cmpID = Cmpid;
                                        ItemSerial.CreatedUTC = DateTime.UtcNow;
                                        WYNKContext.ItemSerial.Add(ItemSerial);
                                        WYNKContext.SaveChanges();

                                        var ItemBalance = WYNKContext.ItemBalance.Where(x => x.ItemID == item.ItemId && x.FYear == Convert.ToInt32(Fyear) && x.StoreID == AddStock.storeId && x.CmpID == AddStock.cmpid).FirstOrDefault();
                                        if (ItemBalance != null)
                                        {
                                            switch (CurrentMonth)
                                            {
                                                case 1:
                                                    ItemBalance.Rec01 = ItemBalance.Rec01 + 1;
                                                    ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + 1;
                                                    WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                    break;
                                                case 2:
                                                    ItemBalance.Rec02 = ItemBalance.Iss02 + 1;
                                                    ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + 1;
                                                    WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                    break;
                                                case 3:
                                                    ItemBalance.Rec03 = ItemBalance.Iss03 + 1;
                                                    ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + 1;
                                                    WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                    break;
                                                case 4:
                                                    ItemBalance.Rec04 = ItemBalance.Rec04 + 1;
                                                    ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + 1;
                                                    WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                    break;
                                                case 5:
                                                    ItemBalance.Rec05 = ItemBalance.Rec05 + 1;
                                                    ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + 1;
                                                    WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                    break;
                                                case 6:
                                                    ItemBalance.Rec06 = ItemBalance.Rec06 + 1;
                                                    ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + 1;
                                                    WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                    break;
                                                case 7:
                                                    ItemBalance.Rec07 = ItemBalance.Rec07 + 1;
                                                    ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + 1;
                                                    WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                    break;
                                                case 8:
                                                    ItemBalance.Rec08 = ItemBalance.Rec08 + 1;
                                                    ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + 1;
                                                    WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                    break;
                                                case 9:
                                                    ItemBalance.Rec09 = ItemBalance.Rec09 + 1;
                                                    ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + 1;
                                                    WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                    break;
                                                case 10:
                                                    ItemBalance.Rec10 = ItemBalance.Rec10 + 1;
                                                    ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + 1;
                                                    WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                    break;
                                                case 11:
                                                    ItemBalance.Rec11 = ItemBalance.Rec11 + 1;
                                                    ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + 1;
                                                    WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                    break;
                                                case 12:
                                                    ItemBalance.Rec12 = ItemBalance.Rec12 + 1;
                                                    ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + 1;
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
                                                    ItemBala.Rec01 = 1;
                                                    break;
                                                case 2:
                                                    ItemBala.Rec02 = 1;
                                                    break;
                                                case 3:
                                                    ItemBala.Rec03 = 1;
                                                    break;
                                                case 4:
                                                    ItemBala.Rec04 = 1;
                                                    break;
                                                case 5:
                                                    ItemBala.Rec05 = 1;
                                                    break;
                                                case 6:
                                                    ItemBala.Rec06 = 1;
                                                    break;
                                                case 7:
                                                    ItemBala.Rec07 = 1;
                                                    break;
                                                case 8:
                                                    ItemBala.Rec08 = 1;
                                                    break;
                                                case 9:
                                                    ItemBala.Rec09 = 1;
                                                    break;
                                                case 10:
                                                    ItemBala.Rec10 = 1;
                                                    break;
                                                case 11:
                                                    ItemBala.Rec11 = 1;
                                                    break;
                                                case 12:
                                                    ItemBala.Rec12 = 1;
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
                                            ItemBala.UOMID = Uom;
                                            ItemBala.FYear = Convert.ToInt32(Fyear);
                                            ItemBala.OpeningBalance = 0;
                                            ItemBala.ItemID = item.ItemId;
                                            ItemBala.StoreID = AddStock.storeId;
                                            ItemBala.ClosingBalance = 1;
                                            ItemBala.CreatedBy = createdby;
                                            ItemBala.CreatedUTC = DateTime.UtcNow;
                                            ItemBala.CmpID = Cmpid;
                                            WYNKContext.ItemBalance.Add(ItemBala);
                                        }
                                        WYNKContext.SaveChanges();
                                        var StockTrans = WYNKContext.StockTran.Where(x => x.SMID == stockmas.RandomUniqueID && x.ItemID == item.ItemId).FirstOrDefault();
                                        if (StockTrans == null)
                                        {
                                            var stockTran = new StockTran();
                                            stockTran.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                                            stockTran.SMID = stockmas.RandomUniqueID;
                                            stockTran.ItemID = item.ItemId;
                                            stockTran.ItemQty = 1;
                                            stockTran.ContraSMID = item.SMID;
                                            stockTran.ContraSTID = item.STID;
                                            stockTran.ContraQty = WYNKContext.StockTran.Where(x => x.SMID == item.SMID && x.RandomUniqueID == item.STID && x.ItemID == item.ItemId).Select(x => x.ItemQty).FirstOrDefault();
                                            stockTran.CreatedUTC = DateTime.UtcNow;
                                            stockTran.CreatedBy = createdby;
                                            WYNKContext.StockTran.Add(stockTran);
                                            WYNKContext.SaveChanges();
                                        }
                                        else
                                        {
                                            StockTrans.ItemQty = StockTrans.ItemQty != null ? StockTrans.ItemQty + 1 : 1;
                                            WYNKContext.StockTran.UpdateRange(StockTrans);
                                            WYNKContext.SaveChanges();
                                        }
                                    }
                            }
                            else
                            {
                                    if (item.Difference != 0 && item.Difference != null)
                                    {
                                        var itembatchDiff = new itembatchdiff();
                                        itembatchDiff.Cmpid = Cmpid;
                                        itembatchDiff.Smid = stockmas.RandomUniqueID;
                                        itembatchDiff.Itemid = item.ItemId;
                                        itembatchDiff.Diffquantity = Convert.ToInt32(item.Difference);
                                        itembatchDiff.reasons = item.Reasons;
                                        itembatchDiff.issuestoreid = Convert.ToInt32(SenderstoreId);
                                        itembatchDiff.issueuserid = Convert.ToInt32(SenderUserId);
                                        itembatchDiff.issuedateandtime = Convert.ToDateTime(Senderdatetime);
                                        itembatchDiff.recstoreid = AddStock.storeId;
                                        itembatchDiff.recuserid = AddStock.CreatedBy;
                                        itembatchDiff.recdateandtime = DateTime.Now;
                                        WYNKContext.itembatchdiff.Add(itembatchDiff);
                                        WYNKContext.SaveChanges();
                                    }
                                    if (item.RecQuantity > 0)
                                    {
                                        var ItemBalance = WYNKContext.ItemBalance.Where(x => x.ItemID == item.ItemId && x.FYear == Convert.ToInt32(Fyear) && x.StoreID == AddStock.storeId && x.CmpID == AddStock.cmpid).FirstOrDefault();
                                        if (ItemBalance != null)
                                        {
                                            switch (CurrentMonth)
                                            {
                                                case 1:
                                                    ItemBalance.Rec01 = ItemBalance.Rec01 + Convert.ToDecimal(item.RecQuantity);
                                                    ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Convert.ToDecimal(item.RecQuantity);
                                                    WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                    break;
                                                case 2:
                                                    ItemBalance.Rec02 = ItemBalance.Iss02 + Convert.ToDecimal(item.RecQuantity);
                                                    ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Convert.ToDecimal(item.RecQuantity);
                                                    WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                    break;
                                                case 3:
                                                    ItemBalance.Rec03 = ItemBalance.Iss03 + Convert.ToDecimal(item.RecQuantity);
                                                    ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Convert.ToDecimal(item.RecQuantity);
                                                    WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                    break;
                                                case 4:
                                                    ItemBalance.Rec04 = ItemBalance.Rec04 + Convert.ToDecimal(item.RecQuantity);
                                                    ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Convert.ToDecimal(item.RecQuantity);
                                                    WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                    break;
                                                case 5:
                                                    ItemBalance.Rec05 = ItemBalance.Rec05 + Convert.ToDecimal(item.RecQuantity);
                                                    ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Convert.ToDecimal(item.RecQuantity);
                                                    WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                    break;
                                                case 6:
                                                    ItemBalance.Rec06 = ItemBalance.Rec06 + Convert.ToDecimal(item.RecQuantity);
                                                    ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Convert.ToDecimal(item.RecQuantity);
                                                    WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                    break;
                                                case 7:
                                                    ItemBalance.Rec07 = ItemBalance.Rec07 + Convert.ToDecimal(item.RecQuantity);
                                                    ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Convert.ToDecimal(item.RecQuantity);
                                                    WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                    break;
                                                case 8:
                                                    ItemBalance.Rec08 = ItemBalance.Rec08 + Convert.ToDecimal(item.RecQuantity);
                                                    ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Convert.ToDecimal(item.RecQuantity);
                                                    WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                    break;
                                                case 9:
                                                    ItemBalance.Rec09 = ItemBalance.Rec09 + Convert.ToDecimal(item.RecQuantity);
                                                    ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Convert.ToDecimal(item.RecQuantity);
                                                    WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                    break;
                                                case 10:
                                                    ItemBalance.Rec10 = ItemBalance.Rec10 + Convert.ToDecimal(item.RecQuantity);
                                                    ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Convert.ToDecimal(item.RecQuantity);
                                                    WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                    break;
                                                case 11:
                                                    ItemBalance.Rec11 = ItemBalance.Rec11 + Convert.ToDecimal(item.RecQuantity);
                                                    ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Convert.ToDecimal(item.RecQuantity);
                                                    WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                                    break;
                                                case 12:
                                                    ItemBalance.Rec12 = ItemBalance.Rec12 + Convert.ToDecimal(item.RecQuantity);
                                                    ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Convert.ToDecimal(item.RecQuantity);
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
                                                    ItemBala.Rec01 = Convert.ToDecimal(item.RecQuantity);
                                                    break;
                                                case 2:
                                                    ItemBala.Rec02 = Convert.ToDecimal(item.RecQuantity);
                                                    break;
                                                case 3:
                                                    ItemBala.Rec03 = Convert.ToDecimal(item.RecQuantity);
                                                    break;
                                                case 4:
                                                    ItemBala.Rec04 = Convert.ToDecimal(item.RecQuantity);
                                                    break;
                                                case 5:
                                                    ItemBala.Rec05 = Convert.ToDecimal(item.RecQuantity);
                                                    break;
                                                case 6:
                                                    ItemBala.Rec06 = Convert.ToDecimal(item.RecQuantity);
                                                    break;
                                                case 7:
                                                    ItemBala.Rec07 = Convert.ToDecimal(item.RecQuantity);
                                                    break;
                                                case 8:
                                                    ItemBala.Rec08 = Convert.ToDecimal(item.RecQuantity);
                                                    break;
                                                case 9:
                                                    ItemBala.Rec09 = Convert.ToDecimal(item.RecQuantity);
                                                    break;
                                                case 10:
                                                    ItemBala.Rec10 = Convert.ToDecimal(item.RecQuantity);
                                                    break;
                                                case 11:
                                                    ItemBala.Rec11 = Convert.ToDecimal(item.RecQuantity);
                                                    break;
                                                case 12:
                                                    ItemBala.Rec12 = Convert.ToDecimal(item.RecQuantity);
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
                                            ItemBala.UOMID = Uom;
                                            ItemBala.FYear = Convert.ToInt32(Fyear);
                                            ItemBala.OpeningBalance = 0;
                                            ItemBala.StoreID = AddStock.storeId;
                                            ItemBala.ItemID = item.ItemId;
                                            ItemBala.ClosingBalance = Convert.ToDecimal(item.RecQuantity);
                                            ItemBala.CreatedBy = createdby;
                                            ItemBala.CreatedUTC = DateTime.UtcNow;
                                            ItemBala.CmpID = Cmpid;
                                            WYNKContext.ItemBalance.Add(ItemBala);
                                            WYNKContext.SaveChanges();
                                        }

                                        var StockTrans = WYNKContext.StockTran.Where(x => x.SMID == stockmas.RandomUniqueID && x.ItemID == item.ItemId).FirstOrDefault();
                                        if (StockTrans == null)
                                        {
                                            var stockTran = new StockTran();
                                            stockTran.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                                            stockTran.SMID = stockmas.RandomUniqueID;
                                            stockTran.ItemID = item.ItemId;
                                            stockTran.ItemQty = Convert.ToDecimal(item.RecQuantity);
                                            stockTran.ContraSMID = item.SMID;
                                            stockTran.ContraSTID = item.STID;
                                            stockTran.ContraQty = WYNKContext.StockTran.Where(x => x.SMID == item.SMID && x.RandomUniqueID == item.STID && x.ItemID == item.ItemId).Select(x => x.ItemQty).FirstOrDefault();
                                            stockTran.CreatedUTC = DateTime.UtcNow;
                                            stockTran.CreatedBy = createdby;
                                            WYNKContext.StockTran.Add(stockTran);
                                            WYNKContext.SaveChanges();
                                        }
                                        else
                                        {
                                            StockTrans.ItemQty = StockTrans.ItemQty != null ? StockTrans.ItemQty + Convert.ToDecimal(item.RecQuantity) : Convert.ToDecimal(item.RecQuantity);
                                            WYNKContext.StockTran.UpdateRange(StockTrans);
                                             WYNKContext.SaveChanges();
                                         }
                                    }
                            }
                        }

                        var SentStockMaster = WYNKContext.StockMaster.Where(x => x.SMID == AddStock.SentSmid && x.CMPID == AddStock.cmpid).FirstOrDefault();
                        SentStockMaster.ContraSmid = stockmas.SMID;
                        WYNKContext.UpdateRange(SentStockMaster);

                        WYNKContext.SaveChanges();

                        var commonRepos = new CommonRepository(_Wynkcontext, _Cmpscontext);
                        var RunningNumber = commonRepos.GenerateRunningCtrlNoo(AddStock.TransactionID, AddStock.cmpid, "GetRunningNo");
                        if (RunningNumber == AddStock.RunningNoStock)
                        {
                            commonRepos.GenerateRunningCtrlNoo(AddStock.TransactionID, AddStock.cmpid, "UpdateRunningNo");
                        }
                        else
                        {
                            var GetRunningNumber = commonRepos.GenerateRunningCtrlNoo(AddStock.TransactionID, AddStock.cmpid, "GetRunningNo");

                            var stockMaster = WYNKContext.StockMaster.Where(x => x.DocumentNumber == AddStock.RunningNoStock).FirstOrDefault();
                            stockMaster.DocumentNumber = GetRunningNumber;
                            WYNKContext.StockMaster.UpdateRange(stockMaster);
                        }
                        WYNKContext.SaveChanges();
                        dbContextTransaction.Commit();
                        return new
                        {
                            Success = true,
                            Message = "Save SuccessFully",
                            StockReceiptsNo = AddStock.RunningNoStock,
                            CompanyDetails = CMPSContext.Company.Where(x => x.CmpID == AddStock.cmpid).FirstOrDefault(),
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
                        return new { Success = false, Message = msg, grn = AddStock.RunningNoStock };
                    }
                    else
                    {
                        ErrorLog oErrorLog = new ErrorLog();
                        oErrorLog.WriteErrorLog("Error", ex.Message.ToString());
                        return new { Success = false };
                    }
                }
                return new { Success = false };
            }
        }
        public dynamic GetInterDepartmentTransferNo(int InterDepRecNo)
        {
            return (from e in CMPSContext.TransactionType.Where(x => x.TransactionID == InterDepRecNo)
                    select new 
                    {
                        TransValue = e.ContraTransactionid,
                    }).FirstOrDefault();
        }

        public dynamic GetInterDepartmentTransferNo1(int InterDepRecNo)
        {

            var ContraID = CMPSContext.TransactionType.Where(x => x.TransactionID == InterDepRecNo).Select(x => x.ContraTransactionid).FirstOrDefault();

            if (ContraID != null)
            {
                var res = (from e in CMPSContext.TransactionType.Where(x => x.ContraTransactionid == ContraID)
                           select new
                           {
                               TransactionID = e.TransactionID,
                               Description = e.Description,
                           }).ToList();

                var IssueTcID = ContraID;
                var DamageTcID = res.Where(x => x.Description == "Damage").Select(x => x.TransactionID).FirstOrDefault();
                var LossInTransitTcID = res.Where(x => x.Description == "Loss in Transit").Select(x => x.TransactionID).FirstOrDefault();
                var OthersTcID = res.Where(x => x.Description == "Others").Select(x => x.TransactionID).FirstOrDefault();
                var CusTcID = res.Where(x => x.Description == "Customer Order Cancellation").Select(x => x.TransactionID).FirstOrDefault();

                return new
                {
                    Success = true,
                    IssueTcID = IssueTcID,
                    DamageTcID = DamageTcID,
                    LossInTransitTcID = LossInTransitTcID,
                    OthersTcID = OthersTcID,
                    CusTcID = CusTcID,
                };
            }
            else
            {
                return new { Success = false, Message = "Contra ID Not defeined" };
            }
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
            else
            {
                return null;
            }
        }
        public dynamic GetStockTransferDetails1(InterDepartmentStockDetails1 InterOpStkDetails, int cmpid)
        {
            var StockDetails = new InterDepartmentStockDetails1();
            var StockMaster = WYNKContext.StockMaster.Where(x => x.RandomUniqueID == InterOpStkDetails.RandomUniqueId && x.ReceiverBranchId == cmpid && x.Status == (int)Status.Open).FirstOrDefault();
            StockDetails.RefDate = StockMaster.RefDate;
            StockDetails.RefNo = StockMaster.RefNo;
            var DrugMaster = WYNKContext.DrugMaster.Where(x => x.Cmpid == cmpid).ToList();
            var DrugGroup = WYNKContext.DrugGroup.ToList();
            var Uom = CMPSContext.uommaster.ToList();
            var Onelinemaster = CMPSContext.OneLineMaster.ToList();
            var StockTran = WYNKContext.StockTran.Where(x => x.SMID == InterOpStkDetails.RandomUniqueId).ToList();

            StockDetails.ItemDetails = (from OPSTK in StockTran
                                        select new StockTransferItemDetail
                                        {
                                            ItemId = OPSTK.ItemID,
                                            DrugName = DrugMaster.Where(x => x.ID == OPSTK.ItemID).Select(x => x.Brand).FirstOrDefault(),
                                            GenericName = DrugGroup.Where(dg => dg.ID == DrugMaster.Where(y => y.ID == OPSTK.ItemID).Select(y => y.GenericName).FirstOrDefault()).Select(dg => dg.Description).FirstOrDefault(),
                                            UOM = DrugMaster.Where(y => y.ID == OPSTK.ItemID).Select(y => y.UOM).FirstOrDefault(),
                                            TotalQuantity = Convert.ToInt32(OPSTK.ItemQty),
                                            SMID = OPSTK.SMID,
                                            STID = OPSTK.RandomUniqueID,
                                            IsSerial = TrackingType(OPSTK.ItemID),
                                            ItemReceivedBatchDetails = TrackingType(OPSTK.ItemID) == false ? GetReceivedBatchDetails(OPSTK.SMID, OPSTK.RandomUniqueID, OPSTK.ItemID) : null,
                                            ItemReceivedSerialDetails = TrackingType(OPSTK.ItemID) == true ? GetReceivedSerialDetails(OPSTK.SMID, OPSTK.STID, OPSTK.ItemID, cmpid, StockMaster.StoreID, StockMaster.ReceiverStoreID) : null,
                                            ItemReceivedOtherDetails = TrackingType(OPSTK.ItemID) == null ? GetReceivedOtherDetails(Convert.ToInt32(OPSTK.ItemQty)) : null,
                                        }).ToList();

            StockDetails.fullItemsReceivedDetails = new List<fullItemsReceivedDetail1>();
            foreach (var item in StockDetails.ItemDetails.ToList())
            {

                if (item.ItemReceivedBatchDetails != null)
                {
                    foreach (var IB in item.ItemReceivedBatchDetails.ToList())
                    {
                        StockDetails.fullItemsReceivedDetails.Add(new fullItemsReceivedDetail1()
                        {
                            ItemId = item.ItemId,
                            DrugName = item.DrugName,
                            GenericName = item.GenericName,
                            UOM = item.UOM,
                            SMID = item.SMID,
                            STID = item.STID,
                            IsSerial = item.IsSerial,
                            BatchSerial = IB.ItemBatchNo,
                            ItemBatchID = IB.ItemBatchID,
                            SentQuantity = IB.SentQuantity,
                            BatchExpiry = IB.BatchExpiry,
                            ReceivedQty = IB.SentQuantity,
                            DamageQty = 0,
                            LossInTransit = 0,
                            OtherQty =0,
                        });
                    }
                }

                if (item.ItemReceivedSerialDetails != null)
                {
                    foreach (var IS in item.ItemReceivedSerialDetails.ToList())
                    {
                        StockDetails.fullItemsReceivedDetails.Add(new fullItemsReceivedDetail1()
                        {
                            ItemId = item.ItemId,
                            DrugName = item.DrugName,
                            GenericName = item.GenericName,
                            UOM = item.UOM,
                            SMID = item.SMID,
                            STID = item.STID,
                            IsSerial = item.IsSerial,
                            BatchSerial = IS.SerialNo,
                            BatchExpiry = IS.ExpiryDate,
                            SentQuantity = 1,
                            ReceivedQty = 1,
                            DamageQty = 0,
                            LossInTransit = 0,
                            OtherQty = 0,
                        });
                    }
                }


                if (item.ItemReceivedOtherDetails != null)
                {
                    foreach (var IO in item.ItemReceivedOtherDetails.ToList())
                    {
                        StockDetails.fullItemsReceivedDetails.Add(new fullItemsReceivedDetail1()
                        {
                            ItemId = item.ItemId,
                            DrugName = item.DrugName,
                            GenericName = item.GenericName,
                            UOM = item.UOM,
                            SMID = item.SMID,
                            STID = item.STID,
                            IsSerial = item.IsSerial,
                            SentQuantity = Convert.ToInt32(IO.SentQty),
                            ReceivedQty = Convert.ToDecimal(IO.SentQty),
                            DamageQty = 0,
                            LossInTransit = 0,
                            OtherQty =0,
                        });
                    }
                }
            }

            return StockDetails;
        }


        public dynamic AddReceivedStockDetails1(SubmitReceiptDetails AddStock)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {

                try
                {
                    Boolean IsReceivedFound = false;
                    Boolean IsDamageFound = false;
                    Boolean IsLossInTransitFound = false;
                    Boolean IsOthersFound = false;
                    var Date = DateTime.Now;
                    var CurrentMonth = Date.Month;
                    var DrugMaster = WYNKContext.DrugMaster.Where(x => x.Cmpid == AddStock.cmpid).ToList();
                    var DrugGroup = WYNKContext.DrugGroup.ToList();
                    var FinancialYearId = WYNKContext.FinancialYear.Where(x => x.CMPID == AddStock.cmpid && x.IsActive == true && x.FYAccYear == Convert.ToInt32(AddStock.Fyear)).Select(x => x.ID).FirstOrDefault();
                    if (FinancialYearId == 0)
                    {
                        return new
                        {
                            Success = false,
                            Message = "Financial year doesn't exists",
                        };
                    }

                    var RecdDetails = new PrintInterBranchReceipts();
                    var DamageDetails = new PrintInterBranchReceipts();
                    var LossInTransitDetails = new PrintInterBranchReceipts();
                    var OtherDetails = new PrintInterBranchReceipts();

                    var Uom = CMPSContext.uommaster.ToList();
                    var UTC = CMPSContext.Setup.Where(x => x.CMPID == AddStock.cmpid).Select(x => x.UTCTime).FirstOrDefault();
                    TimeSpan ts = TimeSpan.Parse(UTC);
                    var OrgStockMas = WYNKContext.StockMaster.Where(x => x.RandomUniqueID == AddStock.RandomUniqueId).FirstOrDefault();

                    DateTime Receiptdate;
                    var appdate = DateTime.TryParseExact(AddStock.Receiptdate.Trim(), "dd-MMM-yyyy,HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out Receiptdate);
                    {
                        Receiptdate.ToString("dd-MM-yyyy,HH:mm", CultureInfo.InvariantCulture);
                    }

                    Receiptdate = Receiptdate - ts;

                    if (FinancialYearId == 0)
                    {
                        return new
                        {
                            Success = false,
                            Message = "Financial year doesn't exists",
                        };
                    }

                    if (AddStock.fullItemsReceivedDetails1.Count > 0)
                    {
                        foreach (var item in AddStock.fullItemsReceivedDetails1.ToList())
                        {
                            if (item.ReceivedQty > 0)
                            {
                                var Stock = WYNKContext.StockMaster.Where(x => x.TransactionID == AddStock.TransactionID && x.DocumentNumber == AddStock.TcRunningNo && x.CMPID == AddStock.cmpid).FirstOrDefault();
                                if (Stock == null)
                                {
                                    /*Insertion in  OpticalStockMaster*/
                                    var StkMaster = new StockMaster();
                                    StkMaster.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                                    StkMaster.TransactionID = AddStock.TransactionID;
                                    StkMaster.CMPID = AddStock.cmpid;
                                    StkMaster.DocumentNumber = AddStock.TcRunningNo;
                                    StkMaster.DocumentDate = Receiptdate;
                                    StkMaster.StoreID = AddStock.storeId;
                                    StkMaster.TransactionType = CMPSContext.TransactionType.Where(X => X.TransactionID == AddStock.TransactionID).Select(x => x.Rec_Issue_type).FirstOrDefault();
                                    StkMaster.VendorID = 0;
                                    StkMaster.TotalPOValue = 0;
                                    StkMaster.IsCancelled = false;
                                    StkMaster.IsDeleted = false;
                                    StkMaster.Fyear = Convert.ToString(WYNKContext.FinancialYear.Where(x => x.ID == FinancialYearId).Select(c => c.FYAccYear).FirstOrDefault());
                                    var FID = WYNKContext.FinancialYear.Where(b => Convert.ToDateTime(b.FYFrom) <= DateTime.Now && Convert.ToDateTime(b.FYTo) >= DateTime.Now && b.CMPID == AddStock.cmpid && b.IsActive == true).Select(f => f.ID).FirstOrDefault();
                                    StkMaster.Fyear = Convert.ToString(WYNKContext.FinancialYear.Where(x => x.ID == FID).Select(c => c.FYAccYear).FirstOrDefault());
                                    StkMaster.CreatedUTC = DateTime.UtcNow;
                                    StkMaster.CreatedBy = AddStock.CreatedBy;
                                    WYNKContext.StockMaster.AddRange(StkMaster);
                                    Stock = StkMaster;
                                }
                                /*Insertion in  StockTran*/
                                var Stktrans = new StockTran();
                                Stktrans.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber(); 
                                Stktrans.SMID = Stock.RandomUniqueID;
                                Stktrans.ItemID = item.ItemId;
                                Stktrans.ItemQty = item.ReceivedQty;
                                Stktrans.UOMID = CMPSContext.uommaster.Where(x => x.Description == item.UOM).Select(x => x.id).FirstOrDefault();
                                Stktrans.ContraSMID =item.SMID;
                                Stktrans.ContraSTID =item.STID;
                                Stktrans.IsDeleted = false;
                                Stktrans.CreatedUTC = DateTime.UtcNow;
                                Stktrans.CreatedBy = AddStock.CreatedBy;
                                WYNKContext.StockTran.AddRange(Stktrans);

                                /*OpticalBalance*/
                                var ItemBalance = WYNKContext.ItemBalance.Where(x => x.FYear == FinancialYearId && x.ItemID == item.ItemId && x.StoreID == AddStock.storeId && x.CmpID == AddStock.cmpid).FirstOrDefault();
                                if (ItemBalance != null)
                                {
                                    switch (CurrentMonth)
                                    {
                                        case 1:
                                            ItemBalance.Rec01 = ItemBalance.Rec01 + Convert.ToInt32(item.ReceivedQty);
                                            break;
                                        case 2:
                                            ItemBalance.Rec02 = ItemBalance.Rec02 + Convert.ToInt32(item.ReceivedQty);
                                            break;
                                        case 3:
                                            ItemBalance.Rec03 = ItemBalance.Rec03 + Convert.ToInt32(item.ReceivedQty);
                                            break;
                                        case 4:
                                            ItemBalance.Rec04 = ItemBalance.Rec04 + Convert.ToInt32(item.ReceivedQty);
                                            break;
                                        case 5:
                                            ItemBalance.Rec05 = ItemBalance.Rec05 + Convert.ToInt32(item.ReceivedQty);
                                            break;
                                        case 6:
                                            ItemBalance.Rec06 = ItemBalance.Rec06 + Convert.ToInt32(item.ReceivedQty);
                                            break;
                                        case 7:
                                            ItemBalance.Rec07 = ItemBalance.Rec07 + Convert.ToInt32(item.ReceivedQty);
                                            break;
                                        case 8:
                                            ItemBalance.Rec08 = ItemBalance.Rec08 + Convert.ToInt32(item.ReceivedQty);
                                            break;
                                        case 9:
                                            ItemBalance.Rec09 = ItemBalance.Rec09 + Convert.ToInt32(item.ReceivedQty);
                                            break;
                                        case 10:
                                            ItemBalance.Rec10 = ItemBalance.Rec10 + Convert.ToInt32(item.ReceivedQty);
                                            break;
                                        case 11:
                                            ItemBalance.Rec11 = ItemBalance.Rec11 + Convert.ToInt32(item.ReceivedQty);
                                            break;
                                        case 12:
                                            ItemBalance.Rec12 = ItemBalance.Rec12 + Convert.ToInt32(item.ReceivedQty);
                                            break;
                                    }
                                    ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Convert.ToInt32(item.ReceivedQty);
                                    ItemBalance.StoreID = AddStock.storeId;
                                    ItemBalance.UpdatedBy = AddStock.CreatedBy;
                                    ItemBalance.UpdatedUTC = DateTime.UtcNow;
                                    WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                }
                                else
                                {
                                    var ItemBalance1 = new ItemBalance();
                                    switch (CurrentMonth)
                                    {
                                        case 1:
                                            ItemBalance1.Rec01 = Convert.ToInt32(item.ReceivedQty);
                                            break;
                                        case 2:
                                            ItemBalance1.Rec02 = Convert.ToInt32(item.ReceivedQty);
                                            break;
                                        case 3:
                                            ItemBalance1.Rec03 = Convert.ToInt32(item.ReceivedQty);
                                            break;
                                        case 4:
                                            ItemBalance1.Rec04 = Convert.ToInt32(item.ReceivedQty);
                                            break;
                                        case 5:
                                            ItemBalance1.Rec05 = Convert.ToInt32(item.ReceivedQty);
                                            break;
                                        case 6:
                                            ItemBalance1.Rec06 = Convert.ToInt32(item.ReceivedQty);
                                            break;
                                        case 7:
                                            ItemBalance1.Rec07 = Convert.ToInt32(item.ReceivedQty);
                                            break;
                                        case 8:
                                            ItemBalance1.Rec08 = Convert.ToInt32(item.ReceivedQty);
                                            break;
                                        case 9:
                                            ItemBalance1.Rec09 = Convert.ToInt32(item.ReceivedQty);
                                            break;
                                        case 10:
                                            ItemBalance1.Rec10 = Convert.ToInt32(item.ReceivedQty);
                                            break;
                                        case 11:
                                            ItemBalance1.Rec11 = Convert.ToInt32(item.ReceivedQty);
                                            break;
                                        case 12:
                                            ItemBalance1.Rec12 = Convert.ToInt32(item.ReceivedQty);
                                            break;

                                    }

                                    ItemBalance1.Iss01 = 0;
                                    ItemBalance1.Iss02 = 0;
                                    ItemBalance1.Iss03 = 0;
                                    ItemBalance1.Iss04 = 0;
                                    ItemBalance1.Iss05 = 0;
                                    ItemBalance1.Iss06 = 0;
                                    ItemBalance1.Iss07 = 0;
                                    ItemBalance1.Iss08 = 0;
                                    ItemBalance1.Iss09 = 0;
                                    ItemBalance1.Iss10 = 0;
                                    ItemBalance1.Iss11 = 0;
                                    ItemBalance1.Iss12 = 0;
                                    ItemBalance1.ItemID = item.ItemId;
                                    ItemBalance1.UOMID = CMPSContext.uommaster.Where(x => x.Description == item.UOM).Select(x => x.id).FirstOrDefault();
                                    ItemBalance1.FYear = FinancialYearId;
                                    ItemBalance1.OpeningBalance = 0;
                                    ItemBalance1.StoreID = AddStock.storeId;
                                    ItemBalance1.ClosingBalance = Convert.ToInt32(item.ReceivedQty);
                                    ItemBalance1.CreatedBy = AddStock.CreatedBy;
                                    ItemBalance1.CreatedUTC = DateTime.UtcNow;
                                    ItemBalance1.CmpID = AddStock.cmpid;
                                    WYNKContext.ItemBalance.AddRange(ItemBalance1);
                                }


                               // var StockTran = WYNKContext.StockTran.Where(x => x.STID == item.STID && x.RandomUniqueID == AddStock.RandomUniqueId).FirstOrDefault();
                                var StockTran = WYNKContext.StockTran.Where(x => x.SMID == AddStock.RandomUniqueId).FirstOrDefault();
                                StockTran.RecdQty = item.ReceivedQty;
                                WYNKContext.StockTran.UpdateRange(StockTran);
                                WYNKContext.SaveChanges();
                                IsReceivedFound = true;


                                RecdDetails.ReceiptNo = Stock.DocumentNumber;
                                RecdDetails.ReceiptDate = Stock.DocumentDate;
                                RecdDetails.IssueNo = OrgStockMas.DocumentNumber;
                                RecdDetails.IssueDate = OrgStockMas.DocumentDate;

                                if (RecdDetails.PrintReceiptItemDetails == null)
                                {
                                    RecdDetails.PrintReceiptItemDetails = new List<PrintReceiptItemDetail>();
                                }
                                RecdDetails.PrintReceiptItemDetails.Add(new PrintReceiptItemDetail()
                                {
                                    Brand = DrugMaster.Where(x => x.ID == item.ItemId).Select(x => x.Brand).FirstOrDefault(),
                                    GenericName = DrugGroup.Where(dg => dg.ID == DrugMaster.Where(y => y.ID == item.ItemId).Select(y => y.GenericName).FirstOrDefault()).Select(dg => dg.Description).FirstOrDefault(),
                                    UOM = item.UOM,
                                    QtySent = item.SentQuantity,
                                    QtyRecd = item.ReceivedQty,
                                });

                            }
                            if (item.DamageQty > 0)
                            {
                                var Stock = WYNKContext.StockMaster.Where(x => x.TransactionID == AddStock.DamageTcID && x.DocumentNumber == AddStock.DamageRunningNo && x.CMPID == AddStock.cmpid).FirstOrDefault();
                                if (Stock == null)
                                {
                                    /*Insertion in  OpticalStockMaster*/
                                    var StkMaster = new StockMaster();
                                    StkMaster.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                                    StkMaster.TransactionID = AddStock.DamageTcID;
                                    StkMaster.CMPID = AddStock.cmpid;
                                    StkMaster.DocumentNumber = AddStock.DamageRunningNo;
                                    StkMaster.DocumentDate = Receiptdate;
                                    StkMaster.StoreID = AddStock.storeId;
                                    StkMaster.TransactionType = CMPSContext.TransactionType.Where(X => X.TransactionID == AddStock.TransactionID).Select(x => x.Rec_Issue_type).FirstOrDefault();
                                    StkMaster.VendorID = 0;
                                    StkMaster.TotalPOValue = 0;
                                    StkMaster.IsCancelled = false;
                                    StkMaster.IsDeleted = false;
                                    StkMaster.Fyear = Convert.ToString(WYNKContext.FinancialYear.Where(x => x.ID == FinancialYearId).Select(c => c.FYAccYear).FirstOrDefault());
                                    var FID = WYNKContext.FinancialYear.Where(b => Convert.ToDateTime(b.FYFrom) <= DateTime.Now && Convert.ToDateTime(b.FYTo) >= DateTime.Now && b.CMPID == AddStock.cmpid && b.IsActive == true).Select(f => f.ID).FirstOrDefault();
                                    StkMaster.Fyear = Convert.ToString(WYNKContext.FinancialYear.Where(x => x.ID == FID).Select(c => c.FYAccYear).FirstOrDefault());
                                    StkMaster.CreatedUTC = DateTime.UtcNow;
                                    StkMaster.CreatedBy = AddStock.CreatedBy;
                                    WYNKContext.StockMaster.AddRange(StkMaster);
                                    Stock = StkMaster;
                                }
                                /*Insertion in  StockTran*/
                                var Stktrans = new StockTran();
                                Stktrans.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                                Stktrans.SMID = Stock.RandomUniqueID;
                                Stktrans.ItemID = item.ItemId;
                                Stktrans.ItemQty = item.DamageQty;
                                Stktrans.UOMID = CMPSContext.uommaster.Where(x => x.Description == item.UOM).Select(x => x.id).FirstOrDefault();
                                Stktrans.ContraSMID = item.SMID;
                                Stktrans.ContraSTID = item.STID;
                                Stktrans.IsDeleted = false;
                                Stktrans.CreatedUTC = DateTime.UtcNow;
                                Stktrans.CreatedBy = AddStock.CreatedBy;
                                WYNKContext.StockTran.AddRange(Stktrans);


                                /*OpticalBalance*/
                                var ItemBalance = WYNKContext.ItemBalance.Where(x => x.FYear == FinancialYearId && x.ItemID == item.ItemId && x.StoreID == AddStock.storeId && x.CmpID == AddStock.cmpid).FirstOrDefault();
                                if (ItemBalance != null)
                                {
                                    ItemBalance.DamageQty = ItemBalance.DamageQty == null ? 0 + item.DamageQty : ItemBalance.DamageQty + item.DamageQty;
                                    ItemBalance.StoreID = AddStock.storeId;
                                    ItemBalance.UpdatedBy = AddStock.CreatedBy;
                                    ItemBalance.UpdatedUTC = DateTime.UtcNow;
                                    WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                }
                                else
                                {
                                    var ItemBalance1 = new ItemBalance();
                                    ItemBalance1.Iss01 = 0;
                                    ItemBalance1.Iss02 = 0;
                                    ItemBalance1.Iss03 = 0;
                                    ItemBalance1.Iss04 = 0;
                                    ItemBalance1.Iss05 = 0;
                                    ItemBalance1.Iss06 = 0;
                                    ItemBalance1.Iss07 = 0;
                                    ItemBalance1.Iss08 = 0;
                                    ItemBalance1.Iss09 = 0;
                                    ItemBalance1.Iss10 = 0;
                                    ItemBalance1.Iss11 = 0;
                                    ItemBalance1.Iss12 = 0;
                                    ItemBalance1.Rec01 = 0;
                                    ItemBalance1.Rec02 = 0;
                                    ItemBalance1.Rec03 = 0;
                                    ItemBalance1.Rec04 = 0;
                                    ItemBalance1.Rec05 = 0;
                                    ItemBalance1.Rec06 = 0;
                                    ItemBalance1.Rec07 = 0;
                                    ItemBalance1.Rec07 = 0;
                                    ItemBalance1.Rec08 = 0;
                                    ItemBalance1.Rec09 = 0;
                                    ItemBalance1.Rec10 = 0;
                                    ItemBalance1.Rec11 = 0;
                                    ItemBalance1.Rec12 = 0;
                                    ItemBalance1.ItemID = item.ItemId;
                                    ItemBalance1.UOMID = CMPSContext.uommaster.Where(x => x.Description == item.UOM).Select(x => x.id).FirstOrDefault();
                                    ItemBalance1.FYear = FinancialYearId;
                                    ItemBalance1.OpeningBalance = 0;
                                    ItemBalance1.StoreID = AddStock.storeId;
                                    ItemBalance1.ClosingBalance = 0;
                                    ItemBalance1.DamageQty = item.DamageQty;
                                    ItemBalance1.CreatedBy = AddStock.CreatedBy;
                                    ItemBalance1.CreatedUTC = DateTime.UtcNow;
                                    ItemBalance1.CmpID = AddStock.cmpid;
                                    WYNKContext.ItemBalance.AddRange(ItemBalance1);
                                }


                                var StockTran = WYNKContext.StockTran.Where(x => x.STID == item.ItemId && x.RandomUniqueID == AddStock.RandomUniqueId).FirstOrDefault();
                                StockTran.DamageQty = item.DamageQty;
                                WYNKContext.StockTran.UpdateRange(StockTran);
                                WYNKContext.SaveChanges();
                                IsDamageFound = true;


                                DamageDetails.ReceiptNo = Stock.DocumentNumber;
                                DamageDetails.ReceiptDate = Stock.DocumentDate;
                                DamageDetails.IssueNo = OrgStockMas.DocumentNumber;
                                DamageDetails.IssueDate = OrgStockMas.DocumentDate;

                                if (DamageDetails.PrintReceiptItemDetails == null)
                                {
                                    DamageDetails.PrintReceiptItemDetails = new List<PrintReceiptItemDetail>();
                                }
                                DamageDetails.PrintReceiptItemDetails.Add(new PrintReceiptItemDetail()
                                {
                                    Brand = DrugMaster.Where(x => x.ID == item.ItemId).Select(x => x.Brand).FirstOrDefault(),
                                    GenericName = DrugGroup.Where(dg => dg.ID == DrugMaster.Where(y => y.ID == item.ItemId).Select(y => y.GenericName).FirstOrDefault()).Select(dg => dg.Description).FirstOrDefault(),
                                    UOM = item.UOM,
                                    QtySent = item.SentQuantity,
                                    QtyRecd = item.ReceivedQty,
                                });
                            }
                            if (item.LossInTransit > 0)
                            {
                                var Stock = WYNKContext.StockMaster.Where(x => x.TransactionID == AddStock.LossInTransitTcID && x.DocumentNumber == AddStock.LossInTransitRunningNo && x.CMPID == AddStock.cmpid).FirstOrDefault();
                                if (Stock == null)
                                {
                                    /*Insertion in  OpticalStockMaster*/
                                    var StkMaster = new StockMaster();
                                    StkMaster.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                                    StkMaster.TransactionID = AddStock.LossInTransitTcID;
                                    StkMaster.CMPID = AddStock.cmpid;
                                    StkMaster.DocumentNumber = AddStock.LossInTransitRunningNo;
                                    StkMaster.DocumentDate = Receiptdate;
                                    StkMaster.StoreID = AddStock.storeId;
                                    StkMaster.TransactionType = CMPSContext.TransactionType.Where(X => X.TransactionID == AddStock.TransactionID).Select(x => x.Rec_Issue_type).FirstOrDefault();
                                    StkMaster.VendorID = 0;
                                    StkMaster.TotalPOValue = 0;
                                    StkMaster.IsCancelled = false;
                                    StkMaster.IsDeleted = false;
                                    StkMaster.Fyear = Convert.ToString(WYNKContext.FinancialYear.Where(x => x.ID == FinancialYearId).Select(c => c.FYAccYear).FirstOrDefault());
                                    var FID = WYNKContext.FinancialYear.Where(b => Convert.ToDateTime(b.FYFrom) <= DateTime.Now && Convert.ToDateTime(b.FYTo) >= DateTime.Now && b.CMPID == AddStock.cmpid && b.IsActive == true).Select(f => f.ID).FirstOrDefault();
                                    StkMaster.Fyear = Convert.ToString(WYNKContext.FinancialYear.Where(x => x.ID == FID).Select(c => c.FYAccYear).FirstOrDefault());
                                    StkMaster.CreatedUTC = DateTime.UtcNow;
                                    StkMaster.CreatedBy = AddStock.CreatedBy;
                                    WYNKContext.StockMaster.AddRange(StkMaster);
                                    Stock = StkMaster;
                                }
                                /*Insertion in  OpticalStockTran*/
                                var Stktrans = new StockTran();
                                Stktrans.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                                Stktrans.SMID = Stock.RandomUniqueID;
                                Stktrans.ItemID = item.ItemId;
                                Stktrans.ItemQty = item.LossInTransit;
                                Stktrans.UOMID = CMPSContext.uommaster.Where(x => x.Description == item.UOM).Select(x => x.id).FirstOrDefault();
                                Stktrans.ContraSMID = item.SMID;
                                Stktrans.ContraSTID = item.STID;
                                Stktrans.IsDeleted = false;
                                Stktrans.CreatedUTC = DateTime.UtcNow;
                                Stktrans.CreatedBy = AddStock.CreatedBy;
                                WYNKContext.StockTran.AddRange(Stktrans);


                                /*OpticalBalance*/
                                var ItemBalance = WYNKContext.ItemBalance.Where(x => x.FYear == FinancialYearId && x.ItemID == item.ItemId && x.StoreID == AddStock.storeId && x.CmpID == AddStock.cmpid).FirstOrDefault();
                                if (ItemBalance != null)
                                {
                                    ItemBalance.LossInTransit = ItemBalance.LossInTransit == null ? 0 + item.LossInTransit : ItemBalance.LossInTransit + item.LossInTransit;
                                    ItemBalance.StoreID = AddStock.storeId;
                                    ItemBalance.UpdatedBy = AddStock.CreatedBy;
                                    ItemBalance.UpdatedUTC = DateTime.UtcNow;
                                    WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                }
                                else
                                {
                                    var ItemBalance1 = new ItemBalance();
                                    ItemBalance1.Iss01 = 0;
                                    ItemBalance1.Iss02 = 0;
                                    ItemBalance1.Iss03 = 0;
                                    ItemBalance1.Iss04 = 0;
                                    ItemBalance1.Iss05 = 0;
                                    ItemBalance1.Iss06 = 0;
                                    ItemBalance1.Iss07 = 0;
                                    ItemBalance1.Iss08 = 0;
                                    ItemBalance1.Iss09 = 0;
                                    ItemBalance1.Iss10 = 0;
                                    ItemBalance1.Iss11 = 0;
                                    ItemBalance1.Iss12 = 0;
                                    ItemBalance1.Rec01 = 0;
                                    ItemBalance1.Rec02 = 0;
                                    ItemBalance1.Rec03 = 0;
                                    ItemBalance1.Rec04 = 0;
                                    ItemBalance1.Rec05 = 0;
                                    ItemBalance1.Rec06 = 0;
                                    ItemBalance1.Rec07 = 0;
                                    ItemBalance1.Rec07 = 0;
                                    ItemBalance1.Rec08 = 0;
                                    ItemBalance1.Rec09 = 0;
                                    ItemBalance1.Rec10 = 0;
                                    ItemBalance1.Rec11 = 0;
                                    ItemBalance1.Rec12 = 0;
                                    ItemBalance1.ItemID = item.ItemId;
                                    ItemBalance1.UOMID = CMPSContext.uommaster.Where(x => x.Description == item.UOM).Select(x => x.id).FirstOrDefault();
                                    ItemBalance1.FYear = FinancialYearId;
                                    ItemBalance1.OpeningBalance = 0;
                                    ItemBalance1.StoreID = AddStock.storeId;
                                    ItemBalance1.ClosingBalance = 0;
                                    ItemBalance1.LossInTransit = item.LossInTransit;
                                    ItemBalance1.CreatedBy = AddStock.CreatedBy;
                                    ItemBalance1.CreatedUTC = DateTime.UtcNow;
                                    ItemBalance1.CmpID = AddStock.cmpid;
                                    WYNKContext.ItemBalance.AddRange(ItemBalance1);
                                }

                                var OrginalStockTran = WYNKContext.StockTran.Where(x => x.SMID == AddStock.RandomUniqueId).FirstOrDefault();
                                OrginalStockTran.LossInTransit = item.LossInTransit;
                                WYNKContext.StockTran.UpdateRange(OrginalStockTran);
                                WYNKContext.SaveChanges();
                                IsLossInTransitFound = true;

                                LossInTransitDetails.ReceiptNo = Stock.DocumentNumber;
                                LossInTransitDetails.ReceiptDate = Stock.DocumentDate;
                                LossInTransitDetails.IssueNo = OrgStockMas.DocumentNumber;
                                LossInTransitDetails.IssueDate = OrgStockMas.DocumentDate;

                                if (LossInTransitDetails.PrintReceiptItemDetails == null)
                                {
                                    LossInTransitDetails.PrintReceiptItemDetails = new List<PrintReceiptItemDetail>();
                                }
                                LossInTransitDetails.PrintReceiptItemDetails.Add(new PrintReceiptItemDetail()
                                {
                                    Brand = DrugMaster.Where(x => x.ID == item.ItemId).Select(x => x.Brand).FirstOrDefault(),
                                    GenericName = DrugGroup.Where(dg => dg.ID == DrugMaster.Where(y => y.ID == item.ItemId).Select(y => y.GenericName).FirstOrDefault()).Select(dg => dg.Description).FirstOrDefault(),
                                    UOM = item.UOM,
                                    QtySent = item.SentQuantity,
                                    QtyRecd = item.ReceivedQty,
                                });
                            }
                            if (item.OtherQty > 0)
                            {
                                var Stock = WYNKContext.StockMaster.Where(x => x.TransactionID == AddStock.OthersTcID && x.DocumentNumber == AddStock.OthersRunningNo && x.CMPID == AddStock.cmpid).FirstOrDefault();
                                if (Stock == null)
                                {
                                    /*Insertion in  OpticalStockMaster*/
                                    var StkMaster = new StockMaster();
                                    StkMaster.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                                    StkMaster.TransactionID = AddStock.OthersTcID;
                                    StkMaster.CMPID = AddStock.cmpid;
                                    StkMaster.DocumentNumber = AddStock.OthersRunningNo;
                                    StkMaster.DocumentDate = Receiptdate;
                                    StkMaster.StoreID = AddStock.storeId;
                                    StkMaster.TransactionType = CMPSContext.TransactionType.Where(X => X.TransactionID == AddStock.TransactionID).Select(x => x.Rec_Issue_type).FirstOrDefault();
                                    StkMaster.VendorID = 0;
                                    StkMaster.TotalPOValue = 0;
                                    StkMaster.IsCancelled = false;
                                    StkMaster.IsDeleted = false;
                                    StkMaster.Fyear = Convert.ToString(WYNKContext.FinancialYear.Where(x => x.ID == FinancialYearId).Select(c => c.FYAccYear).FirstOrDefault());
                                    var FID = WYNKContext.FinancialYear.Where(b => Convert.ToDateTime(b.FYFrom) <= DateTime.Now && Convert.ToDateTime(b.FYTo) >= DateTime.Now && b.CMPID == AddStock.cmpid && b.IsActive == true).Select(f => f.ID).FirstOrDefault();
                                    StkMaster.Fyear = Convert.ToString(WYNKContext.FinancialYear.Where(x => x.ID == FID).Select(c => c.FYAccYear).FirstOrDefault());
                                    StkMaster.CreatedUTC = DateTime.UtcNow;
                                    StkMaster.CreatedBy = AddStock.CreatedBy;
                                    WYNKContext.StockMaster.AddRange(StkMaster);
                                    Stock = StkMaster;
                                }
                                /*Insertion in  OpticalStockTran*/
                                var Stktrans = new StockTran();
                                Stktrans.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                                Stktrans.SMID = Stock.RandomUniqueID;
                                Stktrans.ItemID = item.ItemId;
                                Stktrans.ItemQty = item.OtherQty;
                                Stktrans.UOMID = CMPSContext.uommaster.Where(x => x.Description == item.UOM).Select(x => x.id).FirstOrDefault();
                                Stktrans.ContraSMID = item.SMID;
                                Stktrans.ContraSTID = item.STID;
                                Stktrans.IsDeleted = false;
                                Stktrans.CreatedUTC = DateTime.UtcNow;
                                Stktrans.CreatedBy = AddStock.CreatedBy;
                                WYNKContext.StockTran.AddRange(Stktrans);

                                /*OpticalBalance*/
                                var ItemBalance = WYNKContext.ItemBalance.Where(x => x.FYear == FinancialYearId && x.ItemID == item.ItemId && x.StoreID == AddStock.storeId && x.CmpID == AddStock.cmpid).FirstOrDefault();
                                if (ItemBalance != null)
                                {
                                    ItemBalance.OtherQty = ItemBalance.OtherQty == null ? 0 + item.OtherQty : ItemBalance.OtherQty + item.OtherQty;
                                    ItemBalance.StoreID = AddStock.storeId;
                                    ItemBalance.UpdatedBy = AddStock.CreatedBy;
                                    ItemBalance.UpdatedUTC = DateTime.UtcNow;
                                    WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                }
                                else
                                {
                                    var ItemBalance1 = new ItemBalance();
                                    ItemBalance1.Iss01 = 0;
                                    ItemBalance1.Iss02 = 0;
                                    ItemBalance1.Iss03 = 0;
                                    ItemBalance1.Iss04 = 0;
                                    ItemBalance1.Iss05 = 0;
                                    ItemBalance1.Iss06 = 0;
                                    ItemBalance1.Iss07 = 0;
                                    ItemBalance1.Iss08 = 0;
                                    ItemBalance1.Iss09 = 0;
                                    ItemBalance1.Iss10 = 0;
                                    ItemBalance1.Iss11 = 0;
                                    ItemBalance1.Iss12 = 0;
                                    ItemBalance1.Rec01 = 0;
                                    ItemBalance1.Rec02 = 0;
                                    ItemBalance1.Rec03 = 0;
                                    ItemBalance1.Rec04 = 0;
                                    ItemBalance1.Rec05 = 0;
                                    ItemBalance1.Rec06 = 0;
                                    ItemBalance1.Rec07 = 0;
                                    ItemBalance1.Rec07 = 0;
                                    ItemBalance1.Rec08 = 0;
                                    ItemBalance1.Rec09 = 0;
                                    ItemBalance1.Rec10 = 0;
                                    ItemBalance1.Rec11 = 0;
                                    ItemBalance1.Rec12 = 0;
                                    ItemBalance1.ItemID = item.ItemId;
                                    ItemBalance1.UOMID = CMPSContext.uommaster.Where(x => x.Description == item.UOM).Select(x => x.id).FirstOrDefault();
                                    ItemBalance1.FYear = FinancialYearId;
                                    ItemBalance1.OpeningBalance = 0;
                                    ItemBalance1.StoreID = AddStock.storeId;
                                    ItemBalance1.ClosingBalance = 0;
                                    ItemBalance1.OtherQty = item.OtherQty;
                                    ItemBalance1.CreatedBy = AddStock.CreatedBy;
                                    ItemBalance1.CreatedUTC = DateTime.UtcNow;
                                    ItemBalance1.CmpID = AddStock.cmpid;
                                    WYNKContext.ItemBalance.AddRange(ItemBalance1);
                                }

                                var OrginalStockTran = WYNKContext.StockTran.Where(x => x.SMID == AddStock.RandomUniqueId).FirstOrDefault();
                                OrginalStockTran.OtherQty = item.OtherQty;
                                WYNKContext.StockTran.UpdateRange(OrginalStockTran);
                                WYNKContext.SaveChanges();
                                IsOthersFound = true;

                                OtherDetails.ReceiptNo = Stock.DocumentNumber;
                                OtherDetails.ReceiptDate = Stock.DocumentDate;
                                OtherDetails.IssueNo = OrgStockMas.DocumentNumber;
                                OtherDetails.IssueDate = OrgStockMas.DocumentDate;

                                if (OtherDetails.PrintReceiptItemDetails == null)
                                {
                                    OtherDetails.PrintReceiptItemDetails = new List<PrintReceiptItemDetail>();
                                }
                                OtherDetails.PrintReceiptItemDetails.Add(new PrintReceiptItemDetail()
                                {
                                    Brand = DrugMaster.Where(x => x.ID == item.ItemId).Select(x => x.Brand).FirstOrDefault(),
                                    GenericName = DrugGroup.Where(dg => dg.ID == DrugMaster.Where(y => y.ID == item.ItemId).Select(y => y.GenericName).FirstOrDefault()).Select(dg => dg.Description).FirstOrDefault(),
                                    UOM = item.UOM,
                                    QtySent = item.SentQuantity,
                                    QtyRecd = item.OtherQty,
                                });
                            }

                            var OrginalStockTrans = WYNKContext.StockTran.Where(x => x.SMID == AddStock.RandomUniqueId).FirstOrDefault();
                            OrginalStockTrans.Status = (int)Status.Closed;
                            WYNKContext.StockTran.UpdateRange(OrginalStockTrans);
                            WYNKContext.SaveChanges();
                        }

                        var OrginalStockMasters = WYNKContext.StockMaster.Where(x => x.RandomUniqueID == AddStock.RandomUniqueId).FirstOrDefault();
                        OrginalStockMasters.Status = (int)Status.Closed;
                        WYNKContext.StockMaster.UpdateRange(OrginalStockMasters);

                        WYNKContext.SaveChanges();


                    }

                    var commonRepos = new CommonRepository(_Wynkcontext, _Cmpscontext);
                    if (IsReceivedFound)
                    {
                        var RunningNumber = commonRepos.GenerateRunningCtrlNoo(AddStock.TransactionID, AddStock.cmpid, "GetRunningNo");

                        if (RunningNumber == AddStock.TcRunningNo)
                        {
                            commonRepos.GenerateRunningCtrlNoo(AddStock.TransactionID, AddStock.cmpid, "UpdateRunningNo");
                        }
                        else
                        {
                            var GetRunningNumber = commonRepos.GenerateRunningCtrlNoo(AddStock.TransactionID, AddStock.cmpid, "UpdateRunningNo");

                            var StockMasterss = WYNKContext.StockMaster.Where(x => x.DocumentNumber == AddStock.TcRunningNo && x.TransactionID == AddStock.TransactionID).FirstOrDefault();
                            StockMasterss.DocumentNumber = GetRunningNumber;
                            WYNKContext.StockMaster.UpdateRange(StockMasterss);
                            WYNKContext.SaveChanges();

                            AddStock.TcRunningNo = GetRunningNumber;
                        }
                    }
                    if (IsDamageFound)
                    {
                        var RunningNumber = commonRepos.GenerateRunningCtrlNoo(AddStock.DamageTcID, AddStock.cmpid, "GetRunningNo");

                        if (RunningNumber == AddStock.DamageRunningNo)
                        {
                            commonRepos.GenerateRunningCtrlNoo(AddStock.DamageTcID, AddStock.cmpid, "UpdateRunningNo");
                        }
                        else
                        {
                            var GetRunningNumber = commonRepos.GenerateRunningCtrlNoo(AddStock.DamageTcID, AddStock.cmpid, "UpdateRunningNo");

                            var StockMasterss = WYNKContext.StockMaster.Where(x => x.DocumentNumber == AddStock.DamageRunningNo && x.TransactionID == AddStock.DamageTcID).FirstOrDefault();
                            StockMasterss.DocumentNumber = GetRunningNumber;
                            WYNKContext.StockMaster.UpdateRange(StockMasterss);
                            WYNKContext.SaveChanges();

                            AddStock.DamageRunningNo = GetRunningNumber;
                        }
                    }
                    if (IsLossInTransitFound)
                    {
                        var RunningNumber = commonRepos.GenerateRunningCtrlNoo(AddStock.LossInTransitTcID, AddStock.cmpid, "GetRunningNo");

                        if (RunningNumber == AddStock.LossInTransitRunningNo)
                        {
                            commonRepos.GenerateRunningCtrlNoo(AddStock.LossInTransitTcID, AddStock.cmpid, "UpdateRunningNo");
                        }
                        else
                        {
                            var GetRunningNumber = commonRepos.GenerateRunningCtrlNoo(AddStock.LossInTransitTcID, AddStock.cmpid, "UpdateRunningNo");

                            var StockMasterss = WYNKContext.StockMaster.Where(x => x.DocumentNumber == AddStock.LossInTransitRunningNo && x.TransactionID == AddStock.LossInTransitTcID).FirstOrDefault();
                            StockMasterss.DocumentNumber = GetRunningNumber;
                            WYNKContext.StockMaster.UpdateRange(StockMasterss);
                            WYNKContext.SaveChanges();

                            AddStock.OthersRunningNo = GetRunningNumber;
                        }
                    }
                    if (IsOthersFound)
                    {
                        var RunningNumber = commonRepos.GenerateRunningCtrlNoo(AddStock.OthersTcID, AddStock.cmpid, "GetRunningNo");

                        if (RunningNumber == AddStock.OthersRunningNo)
                        {
                            commonRepos.GenerateRunningCtrlNoo(AddStock.OthersTcID, AddStock.cmpid, "UpdateRunningNo");
                        }
                        else
                        {
                            var GetRunningNumber = commonRepos.GenerateRunningCtrlNoo(AddStock.OthersTcID, AddStock.cmpid, "UpdateRunningNo");

                            var StockMasterss = WYNKContext.StockMaster.Where(x => x.DocumentNumber == AddStock.OthersRunningNo && x.TransactionID == AddStock.OthersTcID).FirstOrDefault();
                            StockMasterss.DocumentNumber = GetRunningNumber;
                            WYNKContext.StockMaster.UpdateRange(StockMasterss);
                            WYNKContext.SaveChanges();

                            AddStock.OthersRunningNo = GetRunningNumber;
                        }
                    }


                    if (WYNKContext.SaveChanges() >= 0)
                    {
                        ErrorLog oErrorLog = new ErrorLog();
                        oErrorLog.WriteErrorLog("Information :", "Saved Successfully");
                    }

                  //  dbContextTransaction.Commit();

                    return new
                    {
                        Success = true,
                        CompanyDetails = CMPSContext.Company.Where(x => x.CmpID == AddStock.cmpid).FirstOrDefault(),
                        RecdDetails = RecdDetails.ReceiptNo == null ? null : RecdDetails,
                        DamageDetails = DamageDetails.ReceiptNo == null ? null : DamageDetails,
                        LossInTransitDetails = LossInTransitDetails.ReceiptNo == null ? null : LossInTransitDetails,
                        OtherDetails = OtherDetails.ReceiptNo == null ? null : OtherDetails,
                        ReceiptDate = (Receiptdate + ts).ToString("dd-MMM-yyyy, hh:mm tt"),
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
                        return new { Success = false };
                        //   return new { Success = false, Message = msg, grn = AddStock.RunningNoStock };
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






    }
}
