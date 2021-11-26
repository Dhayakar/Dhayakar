
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Data.Repository.Operation;
using WYNK.Helpers;

namespace WYNK.Data.Repository.Implementation
{
    class GrnRepository : RepositoryBase<Grn>, IGrnRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;


        public GrnRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public Grn GetPoDetails(int id)

        {

            var gr = new Grn();

            gr.BatchDetails = new List<BatchDetails>();
            gr.ItemDetails = new List<ItemDetails>();
            gr.StockMaster = new StockMaster();
            gr.STableArray = new List<STableArray>();
            gr.SerialArray = new List<SerialArray>();
            var purchaseordr = WYNKContext.PurchaseOrder.AsNoTracking().ToList();
           
            var vendor = CMPSContext.VendorMaster.AsNoTracking().ToList();
            var locmas = CMPSContext.LocationMaster.AsNoTracking().ToList();

            gr.PurchaseDetails = (from PO in purchaseordr.Where(u => u.POStatus == "Open" && u.IsCancelled == false && u.CMPID == id)
                                  join VM in vendor on PO.VendorID equals VM.ID
                                  join OLM in locmas on VM.Location equals OLM.ID
                                  select new PurchaseDetails
                                  {
                                      POId = PO.POID,
                                      PONumber = PO.PONumber,
                                      PODate = PO.PODate.Date,
                                      QuotationNumber = PO.QuotationNumber,
                                      QuotationDate = PO.QuotationDate,
                                      DeliveryName = PO.DeliveryName,
                                      DeliveryAddress1 = PO.DeliveryAddress1,
                                      DeliveryAddress2 = PO.DeliveryAddress2,
                                      DeliveryAddress3 = PO.DeliveryAddress3,
                                      RandomUniqueID = PO.RandomUniqueID,
                                      DeliveryLocation = OLM.ParentDescription,
                                      VendorId = VM.ID,
                                      SupplierName = VM.Name,
                                      Address1 = VM.Address1,
                                      Address2 = VM.Address2,
                                      Location = OLM.ParentDescription,
                                      scity = locmas.Where(x => x.ID == locmas.Where(u => u.ID == VM.Location).Select(u => u.ParentID).FirstOrDefault()).Select(x => x.ParentDescription).FirstOrDefault(),
                                      Phone = VM.PhoneNo,
                                      GST = VM.GSTNo,
                                      SCityID = Convert.ToString(locmas.Where(x => x.ID == Convert.ToInt16(VM.Location)).Select(x => x.ParentID).FirstOrDefault()),
                                  }).ToList();
            return gr;
        }


        public Grn GetGrnDetails(int id, int tid, string Getloctime)

        {

            var gr = new Grn();

            gr.BatchDetails = new List<BatchDetails>();
            gr.ItemDetails = new List<ItemDetails>();
            gr.StockMaster = new StockMaster();
            gr.STableArray = new List<STableArray>();
            gr.SerialArray = new List<SerialArray>();

            var purchaseordr = WYNKContext.PurchaseOrder.AsNoTracking().ToList();            
            var vendor = CMPSContext.VendorMaster.AsNoTracking().ToList();
            var stkmas = WYNKContext.StockMaster.AsNoTracking().ToList();
            var stomas = CMPSContext.Storemasters.AsNoTracking().ToList();
            var LocationMaster = CMPSContext.LocationMaster.AsNoTracking().ToList();

            gr.GrnDetails = (from SM in stkmas.Where(x => x.CMPID == id && x.TransactionID == tid)
                             join VM in vendor on SM.VendorID equals VM.ID
                             join STM in stomas on SM.StoreID equals STM.StoreID
                             join PO in purchaseordr on SM.POID equals PO.POID
                             join OLM in LocationMaster on PO.DeliveryLocationID equals OLM.ID

                             select new GrnDetails
                             {
                                 smid = SM.SMID,
                                 GrNo = SM.DocumentNumber,
                                 GrDt = SM.DocumentDate,
                                 poid = PO.PONumber,
                                 podt = PO.PODate,
                                 qnum = PO.QuotationNumber,
                                 qudt = PO.QuotationDate,
                                 store = STM.Storename,
                                 vendor = VM.Name,
                                 GpValue = SM.GrossProductValue,
                                 TdValue = SM.TotalDiscountValue,
                                 TtValue = SM.TotalTaxValue,
                                 TcValue = SM.TotalCGSTTaxValue,
                                 TsValue = SM.TotalSGSTTaxValue,
                                 TpValue = SM.TotalPOValue,
                                 Address11 = VM.Address1,
                                 Address22 = VM.Address2,
                                 Locationn = LocationMaster.Where(x => x.ID == VM.Location).Select(x => x.ParentDescription).FirstOrDefault(),
                                 Scity1 = LocationMaster.Where(x => x.ID == LocationMaster.Where(u => u.ID == VM.Location).Select(u => u.ParentID).FirstOrDefault()).Select(x => x.ParentDescription).FirstOrDefault(),
                                 SCityID1 = Convert.ToString(LocationMaster.Where(x => x.ID == Convert.ToInt16(VM.Location)).Select(x => x.ParentID).FirstOrDefault()),
                                 Phonee = VM.PhoneNo,
                                 GSTt = VM.GSTNo,
                                 Deliverycity = LocationMaster.Where(x => x.ID == LocationMaster.Where(u => u.ID == PO.DeliveryLocationID).Select(u => u.ParentID).FirstOrDefault()).Select(x => x.ParentDescription).FirstOrDefault(),
                                 DeliveryCityID = Convert.ToString(LocationMaster.Where(x => x.ID == Convert.ToInt16(PO.DeliveryLocationID)).Select(x => x.ParentID).FirstOrDefault()),
                                 DeliveryNamee = PO.DeliveryName,
                                 DeliveryAddress11 = PO.DeliveryAddress1,
                                 DeliveryAddress22 = PO.DeliveryAddress2,
                                 DeliveryAddress33 = PO.DeliveryAddress3,
                                 DeliveryLocationn = OLM.ParentDescription,
                                 Terms = SM.TermsConditions,
                             }).ToList();
            return gr;
        }

        public Grn GetItemDetails(string ID, int storeID, int CmpID)

        {

            var grr = new Grn();

            grr.BatchDetails = new List<BatchDetails>();
            grr.ItemDetails = new List<ItemDetails>();
            grr.StockMaster = new StockMaster();
            grr.STableArray = new List<STableArray>();
            grr.SerialArray = new List<SerialArray>();

            var purchaseordr = WYNKContext.PurchaseOrder.AsNoTracking().ToList();
            var purchaseordrtran = WYNKContext.PurchaseOrderTrans.AsNoTracking().ToList();
            var drug = WYNKContext.DrugMaster.AsNoTracking().ToList();
           
            var uom = CMPSContext.uommaster.AsNoTracking().ToList();
            var taxMaster = CMPSContext.TaxMaster.AsNoTracking().ToList();
            var DrugGroup = WYNKContext.DrugGroup.AsNoTracking().ToList();
            var ItemBalance = WYNKContext.ItemBalance.AsNoTracking().ToList();

            var Date = DateTime.Now;
           
            var FinancialYearId = WYNKContext.FinancialYear.Where(x => Convert.ToDateTime(x.FYFrom) <= Date && Convert.ToDateTime(x.FYTo) >= Date && x.CMPID == CmpID && x.IsActive == true).Select(x => x.ID).FirstOrDefault();


            grr.ItemDetails = (from PO in purchaseordr.Where(u => u.PONumber == ID)
                               join POT in purchaseordrtran.Where(u => u.ItemQty != u.PORecdQty) on PO.RandomUniqueID equals POT.RandomUniqueID
                               join DRG in drug on POT.ItemID equals DRG.ID
                               join UOMM in uom on DRG.UOM equals UOMM.Description
                              

                               select new ItemDetails
                               {
                                   DrugName = DRG.Brand,
                                   DrgId = DRG.ID,
                                   poid = POT.RandomUniqueID,
                                   uom = UOMM.id,
                                   BaseUOM = DRG.UOM,
                                   PurchaseUOM = DRG.UOM,
                                   Quantity = POT.ItemQty,
                                   ReceivedQuantity = POT.PORecdQty,
                                   Rate = POT.ItemRate,
                                   GST = POT.GSTPercentage,
                                   GSTValue = POT.GSTTaxValue,
                                   Discount = POT.DiscountPercentage,
                                   DiscountValue = POT.DiscountAmount,
                                   CESS = POT.CESSPercentage,
                                   CESSAmount = POT.CESSAmount,
                                   AdditionalCESS = POT.AdditionalCESSPercentage,
                                   AdditionalCESSAmount = POT.AddCESSAmount,
                                   Type = Enum.GetName(typeof(DrugCategory), DRG.DrugCategory),
                                   DrugTracker = Enum.GetName(typeof(TrackingType), DRG.DrugTracker),
                                   TaxDescription = taxMaster.Where(t => t.ID == drug.Where(x => x.ID == DRG.ID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.TaxDescription).FirstOrDefault(),
                                   CESSDescription = taxMaster.Where(t => t.ID == drug.Where(x => x.ID == DRG.ID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.CESSDescription).FirstOrDefault(),
                                   AdditionalCESSDescription = taxMaster.Where(t => t.ID == drug.Where(x => x.ID == DRG.ID).Select(x => x.TaxID).FirstOrDefault()).Select(t => t.AdditionalCESSDescription).FirstOrDefault(),
                                   GenericName = DrugGroup.Where(x => x.ID == DRG.GenericName).Select(x => x.Description).FirstOrDefault(),
                                   ClosingBalance = Convert.ToInt32(ItemBalance.Where(x => x.ItemID == DRG.ID && x.FYear == FinancialYearId && x.StoreID == storeID).Select(x => x.ClosingBalance).FirstOrDefault()),
                               }).ToList();

            return grr;
        }



        public Grn GetGrnItemDetails(string ID, int cmpPid)

        {

            var grr = new Grn();
            grr.BatchDetails = new List<BatchDetails>();
            grr.ItemDetails = new List<ItemDetails>();
            grr.StockMaster = new StockMaster();
            grr.STableArray = new List<STableArray>();
            grr.SerialArray = new List<SerialArray>();

            var drug = WYNKContext.DrugMaster.AsNoTracking().ToList();
            var uom = CMPSContext.uommaster.AsNoTracking().ToList();
            var stkmas = WYNKContext.StockMaster.AsNoTracking().ToList();
            var stktran = WYNKContext.StockTran.AsNoTracking().ToList();
            
            grr.GrnitemDetails = (from SM in stkmas.Where(u => u.DocumentNumber == ID && u.CMPID == cmpPid)
                                  join SMT in stktran on SM.RandomUniqueID equals SMT.SMID
                                  join DRG in drug on SMT.ItemID equals DRG.ID
                                  join UOMM in uom on DRG.UOM equals UOMM.Description

                                  select new GrnitemDetails
                                  {
                                      SMID = SMT.SMID,
                                      DrgQuan = SMT.ItemQty,
                                      DrugName = DRG.Brand,
                                      Uom = DRG.UOM,
                                      rate = SMT.ItemRate,
                                      value = SMT.ItemValue,
                                      Discount = SMT.DiscountPercentage,
                                      DiscountValue = SMT.DiscountAmount,
                                      GST = SMT.GSTPercentage,
                                      GSTValue = SMT.GSTTaxValue,                                      
                                      TotalCost = SMT.ItemValue - SMT.DiscountAmount,
                                  }).ToList();

            grr.Totalvalue = grr.GrnitemDetails.Select(x => x.value).Sum();
            grr.TotalDiscountValue = grr.GrnitemDetails.Select(x => x.DiscountValue).Sum();
            grr.TotalTotalCost = grr.GrnitemDetails.Select(x => x.TotalCost).Sum();

            return grr;
        }



        public dynamic UpdateGRN(Grn GRN, int SID, int cmpPid, int TransactionTypeid)
        {

            var ItemBal = WYNKContext.ItemBalance.ToList();
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {                    
                    var Stkmas = new StockMaster();
                    Stkmas.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                    string RandomUniqueID = Stkmas.RandomUniqueID;
                    Stkmas.TransactionID = GRN.StockMaster.TransactionID;
                    Stkmas.CMPID = GRN.StockMaster.CMPID;
                    //Stkmas.Fyear = Convert.ToString(WYNKContext.FinancialYear.Where(x => x.ID == WYNKContext.FinancialYear.Where(b => Convert.ToDateTime(b.FYFrom) <= DateTime.Now && Convert.ToDateTime(b.FYTo) >= DateTime.Now && x.CMPID == cmpPid && x.IsActive == true).Select(f => f.ID).FirstOrDefault()).Select(c => c.FYAccYear).FirstOrDefault());
                    Stkmas.Fyear = GRN.StockMaster.Fyear;
                    Stkmas.DocumentNumber = GRN.StockMaster.DocumentNumber;
                    Stkmas.DocumentDate = GRN.StockMaster.DocumentDate.Date;
                    Stkmas.POID = GRN.StockMaster.POID;
                    Stkmas.StoreID = GRN.StockMaster.StoreID;
                    Stkmas.TransactionType = "R";
                    Stkmas.VendorID = GRN.StockMaster.VendorID;
                    Stkmas.GrossProductValue = GRN.StockMaster.GrossProductValue;
                    Stkmas.TotalDiscountValue = GRN.StockMaster.TotalDiscountValue;
                    Stkmas.TotalTaxValue = GRN.StockMaster.TotalTaxValue;
                    Stkmas.TotalCGSTTaxValue = (GRN.StockMaster.TotalTaxValue) / 2;
                    Stkmas.TotalSGSTTaxValue = (GRN.StockMaster.TotalTaxValue) / 2;
                    Stkmas.TotalCESSValue = GRN.StockMaster.TotalCESSValue;
                    Stkmas.TotalAdditionalCESSValue = GRN.StockMaster.TotalAdditionalCESSValue;
                    Stkmas.TotalPOValue = GRN.StockMaster.TotalPOValue;
                    Stkmas.TermsConditions = GRN.StockMaster.TermsConditions;
                    Stkmas.CreatedUTC = DateTime.UtcNow;
                    Stkmas.CreatedBy = GRN.StockMaster.CreatedBy;
                    WYNKContext.StockMaster.Add(Stkmas);                    
                    string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == GRN.StockMaster.CreatedBy).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                    string userid = Convert.ToString(GRN.StockMaster.CreatedBy);
                    ErrorLog oErrorLogs = new ErrorLog();
                    oErrorLogs.WriteErrorLogTitle(GRN.Companyname, "GRN", "User name :", username, "User ID :", Convert.ToString(userid), "Mode : Add");
                    object names = Stkmas;
                    oErrorLogs.WriteErrorLogArray("StockMaster", names);
                    WYNKContext.SaveChanges();
                    var crtby = Stkmas.CreatedBy;
                    var trid = Stkmas.TransactionID;

                    string STRandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                    if (GRN.ItemDetails.Count() > 0)
                    {
                        foreach (var item in GRN.ItemDetails.ToList())

                        {
                            var StkTran = new StockTran();
                            StkTran.RandomUniqueID = STRandomUniqueID;
                            StkTran.SMID = RandomUniqueID;
                            StkTran.ItemID = item.DrgId;
                            StkTran.ItemQty = item.ActualQuantity;
                            StkTran.UOMID = item.uom;
                            StkTran.ItemRate = item.Rate;
                            StkTran.ItemValue = item.Value;
                            StkTran.DiscountPercentage = item.Discount;
                            StkTran.DiscountAmount = item.DiscountValue;
                            StkTran.GSTPercentage = item.GST;
                            StkTran.GSTTaxValue = item.GSTValue;
                            StkTran.CGSTPercentage = (item.GST) / 2;
                            StkTran.CGSTTaxValue = (item.GSTValue) / 2;
                            StkTran.SGSTPercentage = (item.GST) / 2;
                            StkTran.SGSTTaxValue = (item.GSTValue) / 2;
                            StkTran.CESSPercentage = item.CESS;
                            StkTran.CESSValue = item.CESSAmount;
                            StkTran.AdditionalCESSPercentage = item.AdditionalCESS;
                            StkTran.AdditionalCESSValue = item.AdditionalCESSAmount;
                            StkTran.CreatedUTC = DateTime.UtcNow;
                            StkTran.CreatedBy = crtby;
                            WYNKContext.StockTran.AddRange(StkTran);
                            ErrorLog oErrorLogstran = new ErrorLog();
                            object namestr = StkTran;
                            oErrorLogstran.WriteErrorLogArray("StockTran", StkTran);
                            WYNKContext.SaveChanges();
                        }
                    }
                    ///////////////////////////////////////////////////////////Item Batch/////////////////////////////////////////////////////////////////////////////
                    var noninpland = GRN.BatchDetails.Select(x => x.BatchNo).FirstOrDefault();
                    string BTRandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                    if (GRN.BatchDetails.Count() > 0 && noninpland != null)
                    {
                        foreach (var item in GRN.BatchDetails.ToList())
                        {
                         
                            var ids = WYNKContext.ItemBatch.Where(x => x.ItemBatchNumber == item.BatchNo && x.ItemID == item.druggid && x.StoreID == SID).ToList();
                            if (ids != null && ids.Count != 0)
                            {

                                ids.All(x => { x.ItemBatchQty += item.BatchQuantity; x.ItemBatchBalnceQty += item.BatchQuantity; return true; });
                                WYNKContext.ItemBatch.UpdateRange(ids);
                                ErrorLog oErrorLogstran = new ErrorLog();
                                object namestr = ids;
                                oErrorLogstran.WriteErrorLogArray("ItemBatch", ids);
                            }
                        }
                        foreach (var item in GRN.BatchDetails.ToList())
                        {
                            var BatDet = new ItemBatch();
                            var idss = WYNKContext.ItemBatch.Where(x => x.ItemBatchNumber != item.BatchNo || x.ItemBatchNumber == item.BatchNo).ToList();
                            var ids1 = WYNKContext.ItemBatch.Where(x => x.ItemBatchNumber == item.BatchNo && x.ItemID == item.druggid && x.StoreID == SID).ToList();
                            if (idss.Count == 0)
                            {

                                BatDet.RandomUniqueID = BTRandomUniqueID;
                                BatDet.ItemID = item.druggid;
                                BatDet.ItemBatchNumber = item.BatchNo;
                                BatDet.ItemBatchQty = item.BatchQuantity;
                                BatDet.ItemBatchissueQty = 0;
                                BatDet.ItemBatchBalnceQty = item.BatchQuantity;
                                BatDet.StoreID = SID;
                                BatDet.cmpID = cmpPid;
                                BatDet.CreatedBy = crtby;
                                BatDet.CreatedUTC = DateTime.UtcNow;
                                BatDet.ItemBatchExpiry = item.ExpiryDate;
                                WYNKContext.ItemBatch.AddRange(BatDet);
                                ErrorLog oErrorLogstran = new ErrorLog();
                                object namestr = BatDet;
                                oErrorLogstran.WriteErrorLogArray("ItemBatch", BatDet);
                            }
                            if (ids1.Count == 0)
                            {
                                BatDet.RandomUniqueID = BTRandomUniqueID;
                                BatDet.ItemID = item.druggid;
                                BatDet.ItemBatchNumber = item.BatchNo;
                                BatDet.ItemBatchQty = item.BatchQuantity;
                                BatDet.ItemBatchissueQty = 0;
                                BatDet.ItemBatchBalnceQty = item.BatchQuantity;
                                BatDet.StoreID = SID;
                                BatDet.cmpID = cmpPid;
                                BatDet.CreatedBy = crtby;
                                BatDet.CreatedUTC = DateTime.UtcNow;
                                BatDet.ItemBatchExpiry = item.ExpiryDate;
                                WYNKContext.ItemBatch.AddRange(BatDet);
                                ErrorLog oErrorLogstran = new ErrorLog();
                                object namestr = BatDet;
                                oErrorLogstran.WriteErrorLogArray("ItemBatch", BatDet);

                            }
                        }

                        WYNKContext.SaveChanges();

                    }
                    var noninpland1 = GRN.BatchDetails.Select(x => x.BatchNo).FirstOrDefault();
                    if (GRN.BatchDetails.Count() > 0 && noninpland1 != null)
                    {
                        foreach (var item in GRN.BatchDetails.ToList())

                        {
                            var BatDets = new ItemBatchTrans();
                            var smid = Stkmas.RandomUniqueID;
                            var ibid = WYNKContext.ItemBatch.Where(x => x.ItemID == item.druggid).ToList();
                            var stid = WYNKContext.StockTran.Where(x => x.SMID == smid && x.ItemID == item.druggid).ToList();
                            var newstid = (from res in stid select res.STID).FirstOrDefault();
                            var idsss = WYNKContext.ItemBatch.Where(x => x.ItemBatchNumber == item.BatchNo && x.ItemID == item.druggid).ToList();
                            if (idsss != null)
                            {
                                foreach (var item1 in idsss)
                                {
                                    BatDets.ItemBatchID = BTRandomUniqueID;
                                    BatDets.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                                    BatDets.TC = trid;
                                    BatDets.STID = STRandomUniqueID;
                                    BatDets.SMID = smid;
                                    BatDets.cmpID = cmpPid;
                                    BatDets.ItemID = item.druggid;
                                    BatDets.ItemBatchNumber = item.BatchNo;
                                    BatDets.ItemBatchTransactedQty = item.BatchQuantity;
                                    BatDets.UOMID = item.uomm;
                                    BatDets.CreatedUTC = DateTime.UtcNow;
                                    BatDets.CreatedBy = crtby;
                                    BatDets.ItemBatchExpiry = item.ExpiryDate;
                                    WYNKContext.ItemBatchTrans.AddRange(BatDets);
                                    ErrorLog oErrorLogstran = new ErrorLog();
                                    object namestr = BatDets;
                                    oErrorLogstran.WriteErrorLogArray("ItemBatchTrans", BatDets);
                                }
                            }
                        }
                    }
                    ///////////////////////////////////////////////////////////Item Serial//////////////////////////////////////////////////////////
                    var planned = GRN.STableArray.Select(x => x.GRNNumber).FirstOrDefault();
                    if (GRN.STableArray.Count > 0 && planned != null)
                    {
                        foreach (var Arrays in GRN.STableArray.ToList())
                        {
                            var SerialItem = new ItemSerial();
                            var itemid = WYNKContext.DrugMaster.Where(x => x.Brand == Arrays.DrugName).Select(x => x.ID).FirstOrDefault();
                            SerialItem.ItemID = itemid;
                            SerialItem.SerialNo = Convert.ToString(Arrays.SerialNumber);
                            SerialItem.GRNNo = GRN.StockMaster.DocumentNumber;
                            SerialItem.TC = Arrays.TC;
                            SerialItem.cmpID = cmpPid;
                            SerialItem.CreatedUTC = DateTime.UtcNow;
                            SerialItem.CreatedBy = Arrays.CreatedBy;
                            SerialItem.ExpiryDate = Arrays.ExpiryDate1;
                            SerialItem.StoreID = SID;
                            SerialItem.IssueDate = null;
                            WYNKContext.ItemSerial.AddRange(SerialItem);
                            ErrorLog oErrorLogstran = new ErrorLog();
                            object namestr = SerialItem;
                            oErrorLogstran.WriteErrorLogArray("ItemSerial", SerialItem);
                            WYNKContext.SaveChanges();
                        }
                    }

                    /////////////////////////////////////////////Item balance/////////////////////////////////////////////////////////                    

                    


                        if (GRN.ItemDetails.Count() > 0)
                        {
                            foreach (var item in GRN.ItemDetails.ToList())
                            {
                            var Date = DateTime.Now;
                            var CurrentMonth = Date.Month;
                            var FinancialYearId = WYNKContext.FinancialYear.Where(x => x.FYAccYear == Convert.ToInt32(GRN.StockMaster.Fyear) && x.CMPID == cmpPid).Select(x => x.ID).FirstOrDefault();

                            var ItemBalance = WYNKContext.ItemBalance.Where(x => x.FYear == Convert.ToInt32(GRN.StockMaster.Fyear) && x.ItemID == item.DrgId && x.StoreID == SID && x.CmpID == cmpPid).FirstOrDefault();

                                if (ItemBalance != null)
                                {
                                    ////////////////////////////////////Update method/////////////////////////////////////////
                                    switch (CurrentMonth)
                                    {
                                        case 1:
                                            ItemBalance.Rec01 = ItemBalance.Rec01 + Convert.ToInt16(item.ActualQuantity);

                                            break;
                                        case 2:
                                            ItemBalance.Rec02 = ItemBalance.Rec02 + Convert.ToInt16(item.ActualQuantity);

                                            break;
                                        case 3:
                                            ItemBalance.Rec03 = ItemBalance.Rec03 + Convert.ToInt16(item.ActualQuantity);

                                            break;
                                        case 4:
                                            ItemBalance.Rec04 = ItemBalance.Rec04 + Convert.ToInt16(item.ActualQuantity);

                                            break;
                                        case 5:
                                            ItemBalance.Rec05 = ItemBalance.Rec05 + Convert.ToInt16(item.ActualQuantity);

                                            break;
                                        case 6:
                                            ItemBalance.Rec06 = ItemBalance.Rec06 + Convert.ToInt16(item.ActualQuantity);

                                            break;
                                        case 7:
                                            ItemBalance.Rec07 = ItemBalance.Rec07 + Convert.ToInt16(item.ActualQuantity);

                                            break;
                                        case 8:
                                            ItemBalance.Rec08 = ItemBalance.Rec08 + Convert.ToInt16(item.ActualQuantity);

                                            break;
                                        case 9:
                                            ItemBalance.Rec09 = ItemBalance.Rec09 + Convert.ToInt16(item.ActualQuantity);

                                            break;
                                        case 10:
                                            ItemBalance.Rec10 = ItemBalance.Rec10 + Convert.ToInt16(item.ActualQuantity);

                                            break;
                                        case 11:
                                            ItemBalance.Rec11 = ItemBalance.Rec11 + Convert.ToInt16(item.ActualQuantity);

                                            break;
                                        case 12:
                                            ItemBalance.Rec12 = ItemBalance.Rec12 + Convert.ToInt16(item.ActualQuantity);

                                            break;

                                    }

                                    ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Convert.ToInt16(item.ActualQuantity);
                                    ItemBalance.StoreID = SID;
                                    ItemBalance.UpdatedBy = crtby;
                                    ItemBalance.UpdatedUTC = DateTime.UtcNow;
                                    WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                    ErrorLog oErrorLogstran = new ErrorLog();
                                    object namestr = ItemBalance;
                                    oErrorLogstran.WriteErrorLogArray("ItemBalance", ItemBalance);
                                    WYNKContext.SaveChanges();
                                }

                                else
                                {

                                    ////////////////////////////////////Insert method/////////////////////////////////////////
                                    var closebal = ItemBal.Where(x => x.ItemID == item.DrgId && x.CmpID == cmpPid).Sum(x => x.ClosingBalance);

                                    if (closebal != 0)
                                    {

                                        var storeid = ItemBal.Where(x => x.ItemID == item.DrgId && x.StoreID != SID && x.FYear == FinancialYearId).Select(x => x.StoreID).FirstOrDefault();

                                        if (storeid != 0)
                                        {
                                            ////////////////////////////////////New storeID/////////////////////////////////////////
                                            var ItemBalance1 = new ItemBalance();

                                            switch (CurrentMonth)
                                            {

                                                case 1:
                                                    ItemBalance1.Rec01 = Convert.ToInt16(item.ActualQuantity);
                                                    break;
                                                case 2:
                                                    ItemBalance1.Rec02 = Convert.ToInt16(item.ActualQuantity);
                                                    break;
                                                case 3:
                                                    ItemBalance1.Rec03 = Convert.ToInt16(item.ActualQuantity);
                                                    break;
                                                case 4:
                                                    ItemBalance1.Rec04 = Convert.ToInt16(item.ActualQuantity);
                                                    break;
                                                case 5:
                                                    ItemBalance1.Rec05 = Convert.ToInt16(item.ActualQuantity);
                                                    break;
                                                case 6:
                                                    ItemBalance1.Rec06 = Convert.ToInt16(item.ActualQuantity);
                                                    break;
                                                case 7:
                                                    ItemBalance1.Rec07 = Convert.ToInt16(item.ActualQuantity);
                                                    break;
                                                case 8:
                                                    ItemBalance1.Rec08 = Convert.ToInt16(item.ActualQuantity);
                                                    break;
                                                case 9:
                                                    ItemBalance1.Rec09 = Convert.ToInt16(item.ActualQuantity);
                                                    break;
                                                case 10:
                                                    ItemBalance1.Rec10 = Convert.ToInt16(item.ActualQuantity);
                                                    break;
                                                case 11:
                                                    ItemBalance1.Rec11 = Convert.ToInt16(item.ActualQuantity);
                                                    break;
                                                case 12:
                                                    ItemBalance1.Rec12 = Convert.ToInt16(item.ActualQuantity);
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
                                            ItemBalance1.UOMID = item.uom;
                                            ItemBalance1.FYear = FinancialYearId;
                                            ItemBalance1.OpeningBalance = 0;
                                            ItemBalance1.StoreID = SID;
                                            ItemBalance1.ClosingBalance = Convert.ToInt16(item.ActualQuantity);
                                            ItemBalance1.CreatedBy = crtby;
                                            ItemBalance1.CreatedUTC = DateTime.UtcNow;
                                            ItemBalance1.CmpID = cmpPid;
                                            WYNKContext.ItemBalance.Add(ItemBalance1);
                                            ErrorLog oErrorLogstran = new ErrorLog();
                                            object namestr = ItemBalance1;
                                            oErrorLogstran.WriteErrorLogArray("ItemBalance", ItemBalance1);
                                            WYNKContext.SaveChanges();
                                        }
                                        else
                                        {

                                            //////////////////////////////////// Financial Year/////////////////////////////////////////
                                            var ItemBalance1 = new ItemBalance();

                                            switch (CurrentMonth)
                                            {
                                                case 1:
                                                    ItemBalance1.Rec01 = Convert.ToInt16(item.ActualQuantity);

                                                    break;
                                                case 2:
                                                    ItemBalance1.Rec02 = Convert.ToInt16(item.ActualQuantity);

                                                    break;
                                                case 3:
                                                    ItemBalance1.Rec03 = Convert.ToInt16(item.ActualQuantity);

                                                    break;
                                                case 4:
                                                    ItemBalance1.Rec04 = Convert.ToInt16(item.ActualQuantity);

                                                    break;
                                                case 5:
                                                    ItemBalance1.Rec05 = Convert.ToInt16(item.ActualQuantity);

                                                    break;
                                                case 6:
                                                    ItemBalance1.Rec06 = Convert.ToInt16(item.ActualQuantity);

                                                    break;
                                                case 7:
                                                    ItemBalance1.Rec07 = Convert.ToInt16(item.ActualQuantity);

                                                    break;
                                                case 8:
                                                    ItemBalance1.Rec08 = Convert.ToInt16(item.ActualQuantity);

                                                    break;
                                                case 9:
                                                    ItemBalance1.Rec09 = Convert.ToInt16(item.ActualQuantity);

                                                    break;
                                                case 10:
                                                    ItemBalance1.Rec10 = Convert.ToInt16(item.ActualQuantity);

                                                    break;
                                                case 11:
                                                    ItemBalance1.Rec11 = Convert.ToInt16(item.ActualQuantity);

                                                    break;
                                                case 12:
                                                    ItemBalance1.Rec12 = Convert.ToInt16(item.ActualQuantity);

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
                                            ItemBalance1.ItemID = item.DrgId;
                                            ItemBalance1.UOMID = item.uom;
                                            ItemBalance1.FYear = FinancialYearId;
                                            ItemBalance1.OpeningBalance = 0;
                                            ItemBalance1.StoreID = SID;
                                            ItemBalance1.ClosingBalance = Convert.ToInt16(item.ActualQuantity);
                                            ItemBalance1.CreatedBy = crtby;
                                            ItemBalance1.CreatedUTC = DateTime.UtcNow;
                                            ItemBalance1.CmpID = cmpPid;
                                            WYNKContext.ItemBalance.Add(ItemBalance1);
                                            ErrorLog oErrorLogstran = new ErrorLog();
                                            object namestr = ItemBalance1;
                                            oErrorLogstran.WriteErrorLogArray("ItemBalance", ItemBalance1);
                                            WYNKContext.SaveChanges();
                                        }

                                    }
                                    else
                                    {
                                        ////////////////////////////////////New drugID/////////////////////////////////////////
                                        var ItemBalance1 = new ItemBalance();

                                        switch (CurrentMonth)
                                        {

                                            case 1:
                                                ItemBalance1.Rec01 = Convert.ToInt16(item.ActualQuantity);

                                                break;
                                            case 2:
                                                ItemBalance1.Rec02 = Convert.ToInt16(item.ActualQuantity);

                                                break;
                                            case 3:
                                                ItemBalance1.Rec03 = Convert.ToInt16(item.ActualQuantity);

                                                break;
                                            case 4:
                                                ItemBalance1.Rec04 = Convert.ToInt16(item.ActualQuantity);

                                                break;
                                            case 5:
                                                ItemBalance1.Rec05 = Convert.ToInt16(item.ActualQuantity);


                                                break;
                                            case 6:
                                                ItemBalance1.Rec06 = Convert.ToInt16(item.ActualQuantity);

                                                break;
                                            case 7:
                                                ItemBalance1.Rec07 = Convert.ToInt16(item.ActualQuantity);

                                                break;
                                            case 8:
                                                ItemBalance1.Rec08 = Convert.ToInt16(item.ActualQuantity);

                                                break;
                                            case 9:
                                                ItemBalance1.Rec09 = Convert.ToInt16(item.ActualQuantity);

                                                break;
                                            case 10:
                                                ItemBalance1.Rec10 = Convert.ToInt16(item.ActualQuantity);

                                                break;
                                            case 11:
                                                ItemBalance1.Rec11 = Convert.ToInt16(item.ActualQuantity);

                                                break;
                                            case 12:
                                                ItemBalance1.Rec12 = Convert.ToInt16(item.ActualQuantity);

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
                                        ItemBalance1.ItemID = item.DrgId;
                                        ItemBalance1.UOMID = item.uom;
                                        ItemBalance1.FYear = FinancialYearId;
                                        ItemBalance1.OpeningBalance = 0;
                                        ItemBalance1.StoreID = SID;
                                        ItemBalance1.ClosingBalance = Convert.ToInt16(item.ActualQuantity);
                                        ItemBalance1.CreatedBy = crtby;
                                        ItemBalance1.CreatedUTC = DateTime.UtcNow;
                                        ItemBalance1.CmpID = cmpPid;
                                        WYNKContext.ItemBalance.Add(ItemBalance1);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = ItemBalance1;
                                        oErrorLogstran.WriteErrorLogArray("ItemBalance", ItemBalance1);
                                        WYNKContext.SaveChanges();
                                    }
                                }
                            }
                        }
                    //}
                    /////////////////////////////////////////////Purchase Order Tran/////////////////////////////////////////////////////////
                    if (GRN.ItemDetails.Count() > 0)
                    {
                        foreach (var item in GRN.ItemDetails.ToList())

                        {

                            var pot = new PurchaseOrderTrans();

                            var ids = WYNKContext.PurchaseOrderTrans.Where(x => x.RandomUniqueID == item.poid && x.ItemID == item.DrgId).ToList();

                            var match = false;
                            if (ids != null)
                            {

                                ids.All(x =>
                                {
                                    x.PORecdQty = item.ActualQuantity + item.ReceivedQuantity;
                                    return true;
                                });
                                WYNKContext.PurchaseOrderTrans.UpdateRange(ids);
                                ErrorLog oErrorLogstran = new ErrorLog();
                                object namestr = ids;
                                oErrorLogstran.WriteErrorLogArray("PurchaseOrderTrans", ids);
                                WYNKContext.SaveChanges();

                            }
                            var idss = WYNKContext.PurchaseOrderTrans.Where(x => x.RandomUniqueID == item.poid).Select(x => x.ItemQty).ToList();
                            var idssrece = WYNKContext.PurchaseOrderTrans.Where(x => x.RandomUniqueID == item.poid).Select(x => x.PORecdQty).ToList();


                            if (idss.SequenceEqual(idssrece))
                            {
                                match = true;
                            }
                            else
                            {
                                match = false;
                            }

                            if (match == true)
                            {
                                var POtablesid = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == item.poid).ToList();
                                if (POtablesid != null)
                                {

                                    POtablesid.All(x =>
                                    {
                                        x.POStatus = "Closed";
                                        return true;
                                    });
                                    WYNKContext.PurchaseOrder.UpdateRange(POtablesid);
                                    ErrorLog oErrorLogstran = new ErrorLog();
                                    object namestr = POtablesid;
                                    oErrorLogstran.WriteErrorLogArray("PurchaseOrder", POtablesid);
                                    WYNKContext.SaveChanges();
                                }

                            }
                            else
                            {

                            }

                        }
                        WYNKContext.SaveChanges();
                    }

                    /////////////////////////////////////////////Running Number/////////////////////////////////////////////////////////
                    var commonRepos = new CommonRepository(_Wynkcontext, _Cmpscontext);
                    var RunningNumber = commonRepos.GenerateRunningCtrlNoo(TransactionTypeid, cmpPid, "GetRunningNo");

                    if (RunningNumber == GRN.StockMaster.DocumentNumber)
                    {
                        commonRepos.GenerateRunningCtrlNoo(TransactionTypeid, cmpPid, "UpdateRunningNo");
                    }
                    else
                    {
                        var GetRunningNumber = commonRepos.GenerateRunningCtrlNoo(TransactionTypeid, cmpPid, "UpdateRunningNo");

                        var grn = WYNKContext.StockMaster.Where(x => x.DocumentNumber == GRN.StockMaster.DocumentNumber).FirstOrDefault();
                        grn.DocumentNumber = GetRunningNumber;
                        WYNKContext.StockMaster.UpdateRange(grn);

                        WYNKContext.SaveChanges();
                    }

                    if (CMPSContext.SaveChanges() >= 0)
                    {
                        ErrorLog oErrorLog = new ErrorLog();
                        oErrorLog.WriteErrorLog("Information :", "Saved Successfully");
                    }

                    dbContextTransaction.Commit();

                    if (WYNKContext.SaveChanges() >= 0)
                        return new
                        {
                            Success = true,
                            Message = CommonMessage.saved,
                            GRNNO = GRN.StockMaster.DocumentNumber,
                            PAddress = CMPSContext.Company.Where(x => x.CmpID == cmpPid).Select(x => x.Address1).FirstOrDefault(),
                            PAddress2 = CMPSContext.Company.Where(x => x.CmpID == cmpPid).Select(x => x.Address2).FirstOrDefault(),
                            Pphone = CMPSContext.Company.Where(x => x.CmpID == cmpPid).Select(x => x.Phone1).FirstOrDefault(),
                            Pweb = CMPSContext.Company.Where(x => x.CmpID == cmpPid).Select(x => x.Website).FirstOrDefault(),
                            PCompnayname = CMPSContext.Company.Where(x => x.CmpID == cmpPid).Select(x => x.CompanyName).FirstOrDefault(),

                        };
                }
                catch (Exception ex)
                {
                    ErrorLog oErrorLog = new ErrorLog();
                    oErrorLog.WriteErrorLog("Error :", ex.InnerException.Message.ToString());
                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                    string msg = ex.InnerException.Message;
                    return new { Success = false, Message = msg, grn = GRN.StockMaster.DocumentNumber };
                }


            }
            return new
            {
                Success = false,
                Message = CommonMessage.Missing,
            };

        }




        ////////////////////////////////////////get itemserial/////////////////////////////////////////////////////////////////
        public Grn GetGrnSerialDetails(string gnno)

        {

            var grr = new Grn();
            var ItemSerial = WYNKContext.ItemSerial.AsNoTracking().ToList();
            var DrugMaster = WYNKContext.DrugMaster.AsNoTracking().ToList();

            grr.GetGrnSerialDetails = (from IS in ItemSerial.Where(u => u.GRNNo == gnno)


                                       select new GetGrnSerialDetails
                                       {
                                           Drugname = DrugMaster.Where(x => x.ID == IS.ItemID).Select(x => x.Brand).FirstOrDefault(),
                                           SerialNo = Convert.ToString(IS.SerialNo),
                                           ExpiryDate = IS.ExpiryDate
                                       }).ToList();

            return grr;
        }



        public dynamic CheckRNC(int cmpid, int tranid)
        {
            var Date = DateTime.Now;            
            var FinancialYearId = WYNKContext.FinancialYear.Where(x => Convert.ToDateTime(x.FYFrom) <= Date && Convert.ToDateTime(x.FYTo) >= Date && x.CMPID == cmpid && x.IsActive == true).Select(x => x.ID).FirstOrDefault();
            var fromdate = WYNKContext.FinancialYear.Where(x => x.ID == FinancialYearId).Select(x => x.FYFrom).FirstOrDefault();
            var todate = WYNKContext.FinancialYear.Where(x => x.ID == FinancialYearId).Select(x => x.FYTo).FirstOrDefault();
            var grr = new Grn();
            var stockmas = WYNKContext.StockMaster.ToList();
            var numbercontrol = CMPSContext.NumberControl.ToList();
            var NCRN = CMPSContext.NumberControl.Where(x => x.TransactionID == tranid && x.IsActive == true &&
                    x.Autonumber == true && x.CmpID == cmpid).Select(x => x.RunningNumber).FirstOrDefault();
            NCRN += 1;


            grr.numbercon = (from ADM in stockmas.Where(x => x.CreatedUTC.Date >= fromdate.Date && x.CreatedUTC.Date <= todate.Date)
                             select new numbercon
                             {
                                 DocNo = stockmas.Where(c => c.DocumentNumber == numbercontrol.Where(x => x.TransactionID == tranid && x.IsActive == true &&
                                 x.Autonumber == true && x.CmpID == cmpid).Select(x => x.Prefix + NCRN + x.Suffix).FirstOrDefault()).Select(x => x.DocumentNumber).FirstOrDefault(),
                             }).FirstOrDefault();

            try
            {
                if (grr.numbercon.DocNo != null)
                {
                    return new
                    {
                        Success = true,
                        Message = " DocumentNumber already exists"
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




    }
}







