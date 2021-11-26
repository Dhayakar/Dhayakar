
using System;
using System.Collections.Generic;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Helpers;
using System.Text.RegularExpressions;


namespace WYNK.Data.Repository.Implementation
{
    class GrnWoPoRepository : RepositoryBase<GrnWoPo>, IGrnWoPoRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;


        public GrnWoPoRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }


        public dynamic GetdrugDetails(int ID, int cmpid)
        {
            var DrugDetails = new GrnWoPo();
            var durggrp = WYNKContext.DrugGroup.ToList();
            var drugss = WYNKContext.DrugMaster.ToList();
            var uom = CMPSContext.uommaster.ToList();
            var TaxMaster = CMPSContext.TaxMaster.ToList();
            DrugDetails.itemorderdetails = new List<itemorderdetails>();

            var taxid = WYNKContext.DrugMaster.Where(x => x.ID == ID).Select(x => x.TaxID).FirstOrDefault();

            DrugDetails.itemorderdetails = (from REF in drugss.Where(x => x.ID == ID && x.Cmpid == cmpid)

                                            select new itemorderdetails
                                            {
                                                Drugname = REF.Brand,
                                                DrugID = Convert.ToString(REF.ID),
                                                purchaseid = uom.Where(x => x.Description == REF.UOM).Select(x => x.id).FirstOrDefault() != 0 ? uom.Where(x => x.Description == REF.UOM).Select(x => x.id).FirstOrDefault() : 0,
                                                PurchaseUOM = uom.Where(x => x.Description == REF.UOM).Select(x => x.Description).FirstOrDefault() != null ? uom.Where(x => x.Description == REF.UOM).Select(x => x.Description).FirstOrDefault() : string.Empty,
                                                UnitPrice = REF.Rate,
                                                GST = TaxMaster.Where(x => x.ID == taxid).Select(x => x.GSTPercentage).FirstOrDefault() != null ? TaxMaster.Where(x => x.ID == taxid).Select(x => x.GSTPercentage).FirstOrDefault() : 0,
                                                CESS = TaxMaster.Where(x => x.ID == taxid).Select(x => x.CESSPercentage).FirstOrDefault() != null ? TaxMaster.Where(x => x.ID == taxid).Select(x => x.CESSPercentage).FirstOrDefault() : 0,
                                                AdditionalCESS = TaxMaster.Where(x => x.ID == taxid).Select(x => x.AdditionalCESSPercentage).FirstOrDefault() != null ? TaxMaster.Where(x => x.ID == taxid).Select(x => x.AdditionalCESSPercentage).FirstOrDefault() : 0,
                                                TaxDescription = TaxMaster.Where(x => x.ID == taxid).Select(x => x.TaxDescription).FirstOrDefault() != null ? TaxMaster.Where(x => x.ID == taxid).Select(x => x.TaxDescription).FirstOrDefault() : string.Empty,
                                                CessDescription = TaxMaster.Where(x => x.ID == taxid).Select(x => x.CESSDescription).FirstOrDefault() != null ? TaxMaster.Where(x => x.ID == taxid).Select(x => x.CESSDescription).FirstOrDefault() : string.Empty,
                                                AddcessDescription = TaxMaster.Where(x => x.ID == taxid).Select(x => x.AdditionalCESSDescription).FirstOrDefault() != null ? TaxMaster.Where(x => x.ID == taxid).Select(x => x.AdditionalCESSDescription).FirstOrDefault() : string.Empty,
                                                Type = Enum.GetName(typeof(DrugCategory), REF.DrugCategory),
                                                GenericName = durggrp.Where(x => x.ID == REF.GenericName).Select(x => x.Description).FirstOrDefault() != null ? durggrp.Where(x => x.ID == REF.GenericName).Select(x => x.Description).FirstOrDefault() : string.Empty,
                                                Tracking = REF.DrugTracker,

                                            }


                                        ).ToList();


            return DrugDetails;
        }
        public dynamic UpdateGrnWoPo(GrnWoPo GrnWoPo, int cmpid, int TransactionId, string Time)
        {

            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {

                TimeSpan ts = TimeSpan.Parse(Time);

                try
                {

                    var Stkmas = new StockMaster();

                    var transcation = CMPSContext.TransactionType.ToList();

                    Stkmas.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                    string RandomUniqueID = Stkmas.RandomUniqueID;
                    Stkmas.TransactionID = GrnWoPo.StockMaster.TransactionID;
                    Stkmas.CMPID = cmpid;
                    Stkmas.Fyear = GrnWoPo.StockMaster.Fyear;
                    Stkmas.DocumentNumber = GrnWoPo.StockMaster.DocumentNumber;
                    Stkmas.DocumentDate = GrnWoPo.StockMaster.DocumentDate;

                    if (GrnWoPo.StockMaster.QtnNumber != null)
                    {
                        var list = WYNKContext.StockMaster.Where(x => x.CMPID == cmpid && x.TransactionID == TransactionId && x.QtnNumber != null).ToList();
                        var Q = list.Where(x => x.QtnNumber.Replace(" ", string.Empty).Equals(GrnWoPo.StockMaster.QtnNumber.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(y => y.SMID).FirstOrDefault();

                        if (Q != 0)
                        {
                            return new
                            {
                                Success = false,
                                Message = "Quotation number already exists"
                            };

                        }
                    }

                    Stkmas.QtnNumber = GrnWoPo.StockMaster.QtnNumber;

                    if (GrnWoPo.StockMaster.QtnDate != null)
                    {
                        var Grn = WYNKContext.StockMaster.Where(x => x.RandomUniqueID == GrnWoPo.StockMaster.RandomUniqueID).Select(x => x.QtnDate).FirstOrDefault();
                        if (Grn != null)
                        {
                            Stkmas.QtnDate = GrnWoPo.StockMaster.QtnDate.Value.Subtract(ts);
                        }
                        else
                        {
                            Stkmas.QtnDate = GrnWoPo.StockMaster.QtnDate;
                        }
                    }

                    if (GrnWoPo.StockMaster.DCNumber != null)
                    {

                        var listDC = WYNKContext.StockMaster.Where(x => x.CMPID == cmpid && x.TransactionID == TransactionId && x.DCNumber != null).ToList();
                        var D = listDC.Where(x => x.DCNumber.Replace(" ", string.Empty).Equals(GrnWoPo.StockMaster.DCNumber.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(y => y.SMID).FirstOrDefault();

                        if (D != 0)
                        {
                            return new
                            {
                                Success = false,
                                Message = "Delivery Challan number already exists"
                            };

                        }
                    }

                    Stkmas.DCNumber = GrnWoPo.StockMaster.DCNumber;

                    if (GrnWoPo.StockMaster.DCDate != null)
                    {
                        var GrnDcDate = WYNKContext.StockMaster.Where(x => x.RandomUniqueID == GrnWoPo.StockMaster.RandomUniqueID).Select(x => x.DCDate).FirstOrDefault();
                        if (GrnDcDate != null)
                        {
                            Stkmas.DCDate = GrnWoPo.StockMaster.DCDate.Value.Subtract(ts);
                        }
                        else
                        {
                            Stkmas.DCDate = GrnWoPo.StockMaster.DCDate;
                        }
                    }


                    Stkmas.StoreID = GrnWoPo.StockMaster.StoreID;
                    Stkmas.TransactionType = transcation.Where(X => X.TransactionID == TransactionId).Select(x => x.Rec_Issue_type).FirstOrDefault();
                    Stkmas.VendorID = GrnWoPo.StockMaster.VendorID;
                    Stkmas.GrossProductValue = GrnWoPo.StockMaster.GrossProductValue;
                    Stkmas.TotalDiscountValue = GrnWoPo.StockMaster.TotalDiscountValue;
                    Stkmas.TotalTaxValue = GrnWoPo.StockMaster.TotalTaxValue;
                    Stkmas.TotalCGSTTaxValue = (GrnWoPo.StockMaster.TotalTaxValue) / 2;
                    Stkmas.TotalSGSTTaxValue = (GrnWoPo.StockMaster.TotalTaxValue) / 2;
                    Stkmas.TotalCESSValue = GrnWoPo.StockMaster.TotalCESSValue;
                    Stkmas.TotalAdditionalCESSValue = GrnWoPo.StockMaster.TotalAdditionalCESSValue;
                    Stkmas.TotalPOValue = GrnWoPo.StockMaster.TotalPOValue;
                    Stkmas.TermsConditions = GrnWoPo.StockMaster.TermsConditions;
                    Stkmas.CreatedUTC = DateTime.UtcNow;
                    Stkmas.CreatedBy = GrnWoPo.StockMaster.CreatedBy;
                    WYNKContext.StockMaster.AddRange(Stkmas);

                    string STRandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                    if (GrnWoPo.Itemdetails.Count() > 0)
                    {
                        foreach (var item in GrnWoPo.Itemdetails.ToList())

                        {

                            var StkTran = new StockTran();
                            StkTran.RandomUniqueID = STRandomUniqueID;
                            StkTran.SMID = RandomUniqueID;
                            StkTran.ItemID = Convert.ToInt32(item.ItemID);
                            StkTran.ItemQty = item.ItemQty;
                            StkTran.UOMID = Convert.ToInt32(item.UOMID);
                            StkTran.ItemRate = item.ItemRate;
                            StkTran.ItemValue = item.ItemValue;
                            StkTran.DiscountPercentage = item.DiscountPercentage;
                            StkTran.DiscountAmount = item.DiscountAmount;
                            StkTran.GSTPercentage = item.GSTPercentage;
                            StkTran.GSTTaxValue = item.GSTTaxValue;
                            StkTran.CESSPercentage = item.CESSPercentage;
                            StkTran.CESSValue = item.CESSAmount;
                            StkTran.AdditionalCESSPercentage = item.AdditionalCESSPercentage;
                            StkTran.AdditionalCESSValue = item.AddCESSAmount;
                            StkTran.CGSTPercentage = (item.GSTPercentage) / 2;
                            StkTran.CGSTTaxValue = (item.GSTTaxValue) / 2;
                            StkTran.SGSTPercentage = (item.GSTPercentage) / 2;
                            StkTran.SGSTTaxValue = (item.GSTTaxValue) / 2;
                            StkTran.IsDeleted = false;
                            StkTran.CreatedUTC = DateTime.UtcNow;
                            StkTran.CreatedBy = Stkmas.CreatedBy;
                            WYNKContext.StockTran.AddRange(StkTran);
                        }

                    }

                    string BTRandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();

                    if (GrnWoPo.BatchhDetails.Count() > 0)
                    {
                        foreach (var item in GrnWoPo.BatchhDetails.ToList())
                        {
                            var itembatch = new ItemBatch();


                            var count = WYNKContext.ItemBatch.Where(x => x.ItemBatchNumber.Equals(item.BatchNo, StringComparison.OrdinalIgnoreCase) && x.ItemID == item.druggid && x.StoreID == GrnWoPo.StockMaster.StoreID && x.cmpID == cmpid && x.ItemBatchExpiry == Convert.ToDateTime(item.ExpiryDate)).ToList();
                            if (count != null && count.Count != 0)
                            {
                                count.All(x => { x.ItemBatchQty += item.BatchQuantity; x.ItemBatchBalnceQty += item.BatchQuantity; return true; });
                                WYNKContext.ItemBatch.UpdateRange(count);

                            }
                            else
                            {
                                itembatch.RandomUniqueID = BTRandomUniqueID;
                                itembatch.cmpID = cmpid;
                                itembatch.ItemID = item.druggid;
                                itembatch.ItemBatchNumber = item.BatchNo;
                                itembatch.ItemBatchQty = item.BatchQuantity;
                                itembatch.ItemBatchBalnceQty = item.BatchQuantity;
                                itembatch.ItemBatchissueQty = 0;
                                itembatch.ItemBatchExpiry = Convert.ToDateTime(item.ExpiryDate);
                                itembatch.StoreID = GrnWoPo.StockMaster.StoreID;
                                itembatch.CreatedUTC = DateTime.UtcNow;
                                itembatch.CreatedBy = Stkmas.CreatedBy;
                                WYNKContext.ItemBatch.AddRange(itembatch);
                                WYNKContext.SaveChanges();
                            }
                        }
                    }

                    if (GrnWoPo.BatchhDetails.Count() > 0)
                    {
                        foreach (var items in GrnWoPo.BatchhDetails.ToList())
                        {
                            var itembatchtrns = new ItemBatchTrans();
                            itembatchtrns.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                            itembatchtrns.cmpID = cmpid;
                            itembatchtrns.ItemBatchID = BTRandomUniqueID;
                            itembatchtrns.TC = GrnWoPo.StockMaster.TransactionID;
                            itembatchtrns.SMID = RandomUniqueID;
                            itembatchtrns.STID = STRandomUniqueID;
                            itembatchtrns.ItemID = items.druggid;
                            itembatchtrns.ItemBatchNumber = items.BatchNo;
                            itembatchtrns.ItemBatchTransactedQty = items.BatchQuantity;
                            itembatchtrns.ItemBatchExpiry = Convert.ToDateTime(items.ExpiryDate);
                            itembatchtrns.UOMID = items.uomm;
                            itembatchtrns.CreatedUTC = DateTime.UtcNow;
                            itembatchtrns.CreatedBy = Stkmas.CreatedBy;
                            WYNKContext.ItemBatchTrans.AddRange(itembatchtrns);
                        }
                    }

                    if (GrnWoPo.SerialArraywithpo.Count() > 0)
                    {
                        foreach (var item in GrnWoPo.SerialArraywithpo.ToList())
                        {
                            var Itmserial = new ItemSerial();

                            Itmserial.cmpID = cmpid;
                            Itmserial.ItemID = item.druggid;
                            Itmserial.SerialNo = item.SerialNumber;
                            Itmserial.GRNNo = GrnWoPo.StockMaster.DocumentNumber;
                            Itmserial.TC = GrnWoPo.StockMaster.TransactionID;
                            Itmserial.ExpiryDate = Convert.ToDateTime(item.ExpiryDate);
                            Itmserial.StoreID = GrnWoPo.StockMaster.StoreID;
                            Itmserial.IsCancelled = false;
                            Itmserial.CreatedUTC = DateTime.UtcNow;
                            Itmserial.CreatedBy = Stkmas.CreatedBy;
                            WYNKContext.ItemSerial.AddRange(Itmserial);
                        }
                    }

                   
                        if (GrnWoPo.Itemdetails.Count() > 0)
                        {
                            foreach (var item in GrnWoPo.Itemdetails.ToList())
                            {
                            var Date = DateTime.Now;
                            var CurrentMonth = Date.Month;
                            var FinancialYearId = WYNKContext.FinancialYear.Where(x => x.FYAccYear == Convert.ToInt32(GrnWoPo.StockMaster.Fyear) && x.CMPID == cmpid).Select(x => x.ID).FirstOrDefault();

                            var ItemBalance = WYNKContext.ItemBalance.Where(x => x.FYear == Convert.ToInt32(GrnWoPo.StockMaster.Fyear) && x.ItemID == Convert.ToInt32(item.ItemID) && x.StoreID == GrnWoPo.StockMaster.StoreID && x.CmpID == cmpid).FirstOrDefault();

                                if (ItemBalance != null)
                                {
                                    switch (CurrentMonth)
                                    {
                                        case 1:
                                            ItemBalance.Rec01 = ItemBalance.Rec01 + Convert.ToInt32(item.ItemQty);
                                            break;
                                        case 2:
                                            ItemBalance.Rec02 = ItemBalance.Rec02 + Convert.ToInt32(item.ItemQty);
                                            break;
                                        case 3:
                                            ItemBalance.Rec03 = ItemBalance.Rec03 + Convert.ToInt32(item.ItemQty);
                                            break;
                                        case 4:
                                            ItemBalance.Rec04 = ItemBalance.Rec04 + Convert.ToInt32(item.ItemQty);
                                            break;
                                        case 5:
                                            ItemBalance.Rec05 = ItemBalance.Rec05 + Convert.ToInt32(item.ItemQty);
                                            break;
                                        case 6:
                                            ItemBalance.Rec06 = ItemBalance.Rec06 + Convert.ToInt32(item.ItemQty);
                                            break;
                                        case 7:
                                            ItemBalance.Rec07 = ItemBalance.Rec07 + Convert.ToInt32(item.ItemQty);
                                            break;
                                        case 8:
                                            ItemBalance.Rec08 = ItemBalance.Rec08 + Convert.ToInt32(item.ItemQty);
                                            break;
                                        case 9:
                                            ItemBalance.Rec09 = ItemBalance.Rec09 + Convert.ToInt32(item.ItemQty);
                                            break;
                                        case 10:
                                            ItemBalance.Rec10 = ItemBalance.Rec10 + Convert.ToInt32(item.ItemQty);
                                            break;
                                        case 11:
                                            ItemBalance.Rec11 = ItemBalance.Rec11 + Convert.ToInt32(item.ItemQty);
                                            break;
                                        case 12:
                                            ItemBalance.Rec12 = ItemBalance.Rec12 + Convert.ToInt32(item.ItemQty);
                                            break;
                                    }

                                    ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Convert.ToInt32(item.ItemQty);
                                    ItemBalance.StoreID = GrnWoPo.StockMaster.StoreID;
                                    ItemBalance.UpdatedBy = Stkmas.CreatedBy;
                                    ItemBalance.UpdatedUTC = DateTime.UtcNow;
                                    WYNKContext.ItemBalance.UpdateRange(ItemBalance);
                                }
                                else
                                {
                                    var closebal = WYNKContext.ItemBalance.Where(x => x.ItemID == Convert.ToInt32(item.ItemID) && x.CmpID == cmpid).Sum(x => x.ClosingBalance);
                                    if (closebal != 0)
                                    {
                                        var storeid = WYNKContext.ItemBalance.Where(x => x.ItemID == Convert.ToInt32(item.ItemID) && x.StoreID != GrnWoPo.StockMaster.StoreID && x.FYear == FinancialYearId).Select(x => x.StoreID).FirstOrDefault();
                                        if (storeid != 0)
                                        {
                                            var ItemBalance1 = new ItemBalance();

                                            switch (CurrentMonth)
                                            {

                                                case 1:
                                                    ItemBalance1.Rec01 = Convert.ToInt32(item.ItemQty);
                                                    break;
                                                case 2:
                                                    ItemBalance1.Rec02 = Convert.ToInt32(item.ItemQty);
                                                    break;
                                                case 3:
                                                    ItemBalance1.Rec03 = Convert.ToInt32(item.ItemQty);
                                                    break;
                                                case 4:
                                                    ItemBalance1.Rec04 = Convert.ToInt32(item.ItemQty);
                                                    break;
                                                case 5:
                                                    ItemBalance1.Rec05 = Convert.ToInt32(item.ItemQty);
                                                    break;
                                                case 6:
                                                    ItemBalance1.Rec06 = Convert.ToInt32(item.ItemQty);
                                                    break;
                                                case 7:
                                                    ItemBalance1.Rec07 = Convert.ToInt32(item.ItemQty);
                                                    break;
                                                case 8:
                                                    ItemBalance1.Rec08 = Convert.ToInt32(item.ItemQty);
                                                    break;
                                                case 9:
                                                    ItemBalance1.Rec09 = Convert.ToInt32(item.ItemQty);
                                                    break;
                                                case 10:
                                                    ItemBalance1.Rec10 = Convert.ToInt32(item.ItemQty);
                                                    break;
                                                case 11:
                                                    ItemBalance1.Rec11 = Convert.ToInt32(item.ItemQty);
                                                    break;
                                                case 12:
                                                    ItemBalance1.Rec12 = Convert.ToInt32(item.ItemQty);
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
                                            ItemBalance1.ItemID = Convert.ToInt32(item.ItemID);
                                            ItemBalance1.UOMID = Convert.ToInt32(item.UOMID);
                                            ItemBalance1.FYear = FinancialYearId;
                                            ItemBalance1.OpeningBalance = 0;
                                            ItemBalance1.StoreID = GrnWoPo.StockMaster.StoreID;
                                            ItemBalance1.ClosingBalance = Convert.ToInt32(item.ItemQty);
                                            ItemBalance1.CreatedBy = Stkmas.CreatedBy;
                                            ItemBalance1.CreatedUTC = DateTime.UtcNow;
                                            ItemBalance1.CmpID = cmpid;
                                            WYNKContext.ItemBalance.AddRange(ItemBalance1);

                                        }
                                        else
                                        {
                                            var ItemBalance1 = new ItemBalance();
                                            switch (CurrentMonth)
                                            {
                                                case 1:
                                                    ItemBalance1.Rec01 = Convert.ToInt32(item.ItemQty);
                                                    break;
                                                case 2:
                                                    ItemBalance1.Rec02 = Convert.ToInt32(item.ItemQty);
                                                    break;
                                                case 3:
                                                    ItemBalance1.Rec03 = Convert.ToInt32(item.ItemQty);
                                                    break;
                                                case 4:
                                                    ItemBalance1.Rec04 = Convert.ToInt32(item.ItemQty);
                                                    break;
                                                case 5:
                                                    ItemBalance1.Rec05 = Convert.ToInt32(item.ItemQty);
                                                    break;
                                                case 6:
                                                    ItemBalance1.Rec06 = Convert.ToInt32(item.ItemQty);
                                                    break;
                                                case 7:
                                                    ItemBalance1.Rec07 = Convert.ToInt32(item.ItemQty);
                                                    break;
                                                case 8:
                                                    ItemBalance1.Rec08 = Convert.ToInt32(item.ItemQty);
                                                    break;
                                                case 9:
                                                    ItemBalance1.Rec09 = Convert.ToInt32(item.ItemQty);
                                                    break;
                                                case 10:
                                                    ItemBalance1.Rec10 = Convert.ToInt32(item.ItemQty);
                                                    break;
                                                case 11:
                                                    ItemBalance1.Rec11 = Convert.ToInt32(item.ItemQty);
                                                    break;
                                                case 12:
                                                    ItemBalance1.Rec12 = Convert.ToInt32(item.ItemQty);
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
                                            ItemBalance1.ItemID = Convert.ToInt32(item.ItemID);
                                            ItemBalance1.UOMID = Convert.ToInt32(item.UOMID);
                                            ItemBalance1.FYear = FinancialYearId;
                                            ItemBalance1.OpeningBalance = 0;
                                            ItemBalance1.StoreID = GrnWoPo.StockMaster.StoreID;
                                            ItemBalance1.ClosingBalance = Convert.ToInt32(item.ItemQty);
                                            ItemBalance1.CreatedBy = Stkmas.CreatedBy;
                                            ItemBalance1.CreatedUTC = DateTime.UtcNow;
                                            ItemBalance1.CmpID = cmpid;
                                            WYNKContext.ItemBalance.AddRange(ItemBalance1);
                                        }
                                    }
                                    else
                                    {
                                        var ItemBalance1 = new ItemBalance();
                                        switch (CurrentMonth)
                                        {
                                            case 1:
                                                ItemBalance1.Rec01 = Convert.ToInt32(item.ItemQty);
                                                break;
                                            case 2:
                                                ItemBalance1.Rec02 = Convert.ToInt32(item.ItemQty);
                                                break;
                                            case 3:
                                                ItemBalance1.Rec03 = Convert.ToInt32(item.ItemQty);
                                                break;
                                            case 4:
                                                ItemBalance1.Rec04 = Convert.ToInt32(item.ItemQty);
                                                break;
                                            case 5:
                                                ItemBalance1.Rec05 = Convert.ToInt32(item.ItemQty);
                                                break;
                                            case 6:
                                                ItemBalance1.Rec06 = Convert.ToInt32(item.ItemQty);
                                                break;
                                            case 7:
                                                ItemBalance1.Rec07 = Convert.ToInt32(item.ItemQty);
                                                break;
                                            case 8:
                                                ItemBalance1.Rec08 = Convert.ToInt32(item.ItemQty);
                                                break;
                                            case 9:
                                                ItemBalance1.Rec09 = Convert.ToInt32(item.ItemQty);
                                                break;
                                            case 10:
                                                ItemBalance1.Rec10 = Convert.ToInt32(item.ItemQty);
                                                break;
                                            case 11:
                                                ItemBalance1.Rec11 = Convert.ToInt32(item.ItemQty);
                                                break;
                                            case 12:
                                                ItemBalance1.Rec12 = Convert.ToInt32(item.ItemQty);
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
                                        ItemBalance1.ItemID = Convert.ToInt32(item.ItemID);
                                        ItemBalance1.UOMID = Convert.ToInt32(item.UOMID);
                                        ItemBalance1.FYear = FinancialYearId;
                                        ItemBalance1.OpeningBalance = 0;
                                        ItemBalance1.StoreID = GrnWoPo.StockMaster.StoreID;
                                        ItemBalance1.ClosingBalance = Convert.ToInt32(item.ItemQty);
                                        ItemBalance1.CreatedBy = Stkmas.CreatedBy;
                                        ItemBalance1.CreatedUTC = DateTime.UtcNow;
                                        ItemBalance1.CmpID = cmpid;
                                        WYNKContext.ItemBalance.AddRange(ItemBalance1);
                                    }
                                }
                            }
                        }

                    WYNKContext.SaveChanges();
                    dbContextTransaction.Commit();

                    var commonRepos = new CommonRepository(_Wynkcontext, _Cmpscontext);
                    var RunningNumber = commonRepos.GenerateRunningCtrlNoo(TransactionId, cmpid, "GetRunningNo");

                    if (RunningNumber == GrnWoPo.StockMaster.DocumentNumber)
                    {
                        commonRepos.GenerateRunningCtrlNoo(TransactionId, cmpid, "UpdateRunningNo");
                    }
                    else
                    {
                        var GetRunningNumber = commonRepos.GenerateRunningCtrlNoo(TransactionId, cmpid, "UpdateRunningNo");
                        var grn = WYNKContext.StockMaster.Where(x => x.DocumentNumber == GrnWoPo.StockMaster.DocumentNumber).FirstOrDefault();
                        grn.DocumentNumber = GetRunningNumber;
                        WYNKContext.StockMaster.UpdateRange(grn);
                        WYNKContext.SaveChanges();
                    }



                    return new
                    {
                        Success = true,
                        RandomUniqueID = Stkmas.RandomUniqueID
                    };
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                    string msg = ex.InnerException.Message;
                    return new { Success = false, Message = msg, grnwithoutpo = GrnWoPo.StockMaster.DocumentNumber };
                }
            }
        }
        public dynamic GrnWoPoprint(string RandomUniqueID, int cmpid, string Time)
        {
            var Grnprintdetails = new GrnWoPo();
            var Stockmaster = WYNKContext.StockMaster.ToList();
            var StockTran = WYNKContext.StockTran.ToList();
            var Company = CMPSContext.Company.ToList();
            var VendorMaster = CMPSContext.VendorMaster.ToList();
            var LocationMaster = CMPSContext.LocationMaster.ToList();
            var Storemasters = CMPSContext.Storemasters.ToList();
            var DrugMaster = WYNKContext.DrugMaster.ToList();
            var uommaster = CMPSContext.uommaster.ToList();
            var TaxMaster = CMPSContext.TaxMaster.ToList();
            var durggrp = WYNKContext.DrugGroup.ToList();
            var drugss = WYNKContext.DrugMaster.ToList();

            TimeSpan ts = TimeSpan.Parse(Time);
            var GrnNumber = Stockmaster.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.DocumentNumber).FirstOrDefault();
            var GrnDate = Stockmaster.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.DocumentDate.Add(ts)).FirstOrDefault();
            var QtnNumber = Stockmaster.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.QtnNumber).FirstOrDefault();
            var QtnDate = Stockmaster.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.QtnDate).FirstOrDefault() != null ? Stockmaster.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.QtnDate.Value.Add(ts)).FirstOrDefault() : (DateTime?)null;
            var DcNumber = Stockmaster.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.DCNumber).FirstOrDefault();
            var DcDate = Stockmaster.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.DCDate).FirstOrDefault() != null ? Stockmaster.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.DCDate.Value.Add(ts)).FirstOrDefault() : (DateTime?)null;
            var Tremandcond = Stockmaster.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.TermsConditions).FirstOrDefault() != null ? Stockmaster.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.TermsConditions).FirstOrDefault() : string.Empty;
            var GrossProductValue = Stockmaster.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.GrossProductValue).FirstOrDefault();
            var TotalDiscountValue = Stockmaster.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.TotalDiscountValue).FirstOrDefault();
            var TotalTaxValue = Stockmaster.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.TotalTaxValue).FirstOrDefault();
            var TotalPOValue = Stockmaster.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.TotalPOValue).FirstOrDefault();
            var TotalCESSValue = Stockmaster.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.TotalCESSValue).FirstOrDefault();
            var TotalAdditionalCESSValue = Stockmaster.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.TotalAdditionalCESSValue).FirstOrDefault();
            var VendorID = Stockmaster.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.VendorID).FirstOrDefault();
            var Name = VendorMaster.Where(x => x.ID == VendorID).Select(x => x.Name).FirstOrDefault();
            var Address1 = VendorMaster.Where(x => x.ID == VendorID).Select(x => x.Address1).FirstOrDefault() != null ? VendorMaster.Where(x => x.ID == VendorID).Select(x => x.Address1).FirstOrDefault() : string.Empty;
            var Address2 = VendorMaster.Where(x => x.ID == VendorID).Select(x => x.Address2).FirstOrDefault() != null ? VendorMaster.Where(x => x.ID == VendorID).Select(x => x.Address2).FirstOrDefault() : string.Empty;
            var LocationID = VendorMaster.Where(x => x.ID == VendorID).Select(x => x.Location).FirstOrDefault() != null ? VendorMaster.Where(x => x.ID == VendorID).Select(x => x.Location).FirstOrDefault() : 0;
            var VendorLocationID = LocationMaster.Where(x => x.ID == LocationID && x.ParentTag == "loc").Select(x => x.ParentID).FirstOrDefault() != 0 ? LocationMaster.Where(x => x.ID == LocationID && x.ParentTag == "loc").Select(x => x.ParentID).FirstOrDefault() : null;
            if (VendorLocationID != null)
            {
                var CityID = LocationMaster.Where(x => x.ID == VendorLocationID).Select(x => x.ID).FirstOrDefault() != 0 ? LocationMaster.Where(x => x.ID == VendorLocationID).Select(x => x.ID).FirstOrDefault() : 0;
                var StateID = LocationMaster.Where(x => x.ID == CityID).Select(x => x.ParentID).FirstOrDefault() != 0 ? LocationMaster.Where(x => x.ID == CityID).Select(x => x.ParentID).FirstOrDefault() : 0;
                var CountryID = LocationMaster.Where(x => x.ID == StateID).Select(x => x.ParentID).FirstOrDefault() != 0 ? LocationMaster.Where(x => x.ID == StateID).Select(x => x.ParentID).FirstOrDefault() : 0;
                Grnprintdetails.Location = LocationMaster.Where(x => x.ID == LocationID).Select(x => x.ParentDescription).FirstOrDefault() != null ? LocationMaster.Where(x => x.ID == LocationID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
                Grnprintdetails.ParentDescriptioncity = LocationMaster.Where(x => x.ID == CityID).Select(x => x.ParentDescription).FirstOrDefault() != null ? LocationMaster.Where(x => x.ID == CityID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
                Grnprintdetails.ParentDescriptionstate = LocationMaster.Where(x => x.ID == StateID).Select(x => x.ParentDescription).FirstOrDefault() != null ? LocationMaster.Where(x => x.ID == StateID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
                Grnprintdetails.ParentDescriptioncountry = LocationMaster.Where(x => x.ID == CountryID).Select(x => x.ParentDescription).FirstOrDefault() != null ? LocationMaster.Where(x => x.ID == CountryID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
            }
            else
            {
                var VendorCityID = CMPSContext.LocationMaster.Where(x => x.ID == LocationID && x.ParentTag == "City").Select(x => x.ID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == LocationID && x.ParentTag == "City").Select(x => x.ID).FirstOrDefault() : 0;
                if (VendorCityID != 0)
                {
                    var StateID = LocationMaster.Where(x => x.ID == VendorCityID).Select(x => x.ParentID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == VendorCityID).Select(x => x.ParentID).FirstOrDefault() : 0;
                    var CountryID = LocationMaster.Where(x => x.ID == StateID).Select(x => x.ParentID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == StateID).Select(x => x.ParentID).FirstOrDefault() : 0;
                    Grnprintdetails.ParentDescriptioncity = LocationMaster.Where(x => x.ID == VendorCityID).Select(x => x.ParentDescription).FirstOrDefault() != null ? LocationMaster.Where(x => x.ID == VendorCityID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
                    Grnprintdetails.ParentDescriptionstate = LocationMaster.Where(x => x.ID == StateID).Select(x => x.ParentDescription).FirstOrDefault() != null ? LocationMaster.Where(x => x.ID == StateID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
                    Grnprintdetails.ParentDescriptioncountry = LocationMaster.Where(x => x.ID == CountryID).Select(x => x.ParentDescription).FirstOrDefault() != null ? LocationMaster.Where(x => x.ID == CountryID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
                }
            }


            var PhoneNo = VendorMaster.Where(x => x.ID == VendorID).Select(x => x.PhoneNo).FirstOrDefault() != null ? VendorMaster.Where(x => x.ID == VendorID).Select(x => x.PhoneNo).FirstOrDefault() : string.Empty;
            var GSTNo = VendorMaster.Where(x => x.ID == VendorID).Select(x => x.GSTNo).FirstOrDefault() != null ? VendorMaster.Where(x => x.ID == VendorID).Select(x => x.GSTNo).FirstOrDefault() : string.Empty;
            var DLno = VendorMaster.Where(x => x.ID == VendorID).Select(x => x.DLNo).FirstOrDefault() != null ? VendorMaster.Where(x => x.ID == VendorID).Select(x => x.DLNo).FirstOrDefault() : string.Empty;
            var DLDateNo = VendorMaster.Where(x => x.ID == VendorID).Select(x => x.DLNoDate).FirstOrDefault() != null ? VendorMaster.Where(x => x.ID == VendorID).Select(x => x.DLNoDate.Value).FirstOrDefault() : (DateTime?)null;
            var Storename = Storemasters.Where(x => x.StoreID == Stockmaster.Where(f => f.RandomUniqueID == RandomUniqueID).Select(d => d.StoreID).FirstOrDefault()).Select(g => g.Storename).FirstOrDefault();
            var Compnayname = Company.Where(x => x.CmpID == cmpid).Select(x => x.CompanyName).FirstOrDefault();
            var Address = Company.Where(x => x.CmpID == cmpid).Select(x => x.Address1).FirstOrDefault() != null ? Company.Where(x => x.CmpID == cmpid).Select(x => x.Address1).FirstOrDefault() : string.Empty;
            var cAddress1 = Company.Where(x => x.CmpID == cmpid).Select(x => x.Address2).FirstOrDefault() != null ? Company.Where(x => x.CmpID == cmpid).Select(x => x.Address2).FirstOrDefault() : string.Empty;
            var cAddress2 = Company.Where(x => x.CmpID == cmpid).Select(x => x.Address3).FirstOrDefault() != null ? Company.Where(x => x.CmpID == cmpid).Select(x => x.Address3).FirstOrDefault() : string.Empty;
            var phone = Company.Where(x => x.CmpID == cmpid).Select(x => x.Phone1).FirstOrDefault() != null ? Company.Where(x => x.CmpID == cmpid).Select(x => x.Phone1).FirstOrDefault() : string.Empty;
            var web = Company.Where(x => x.CmpID == cmpid).Select(x => x.Website).FirstOrDefault() != null ? Company.Where(x => x.CmpID == cmpid).Select(x => x.Website).FirstOrDefault() : string.Empty;
            var companylocationID = Company.Where(x => x.CmpID == cmpid).Select(x => x.LocationID).FirstOrDefault() != 0 ? Company.Where(x => x.CmpID == cmpid).Select(x => x.LocationID).FirstOrDefault() : 0;
            var companylocation = Company.Where(x => x.CmpID == cmpid).Select(x => x.LocationName).FirstOrDefault() != null ? Company.Where(x => x.CmpID == cmpid).Select(x => x.LocationName).FirstOrDefault() : string.Empty;

            var GrnwithoutPOdetails = (from StockTrans in StockTran.Where(x => x.SMID == RandomUniqueID && x.IsDeleted == false)
                                       join Dur in DrugMaster on StockTrans.ItemID equals Dur.ID
                                       join UOM in uommaster on StockTrans.UOMID equals UOM.id
                                       select new
                                       {

                                           RandomUniqueID = StockTrans.RandomUniqueID,
                                           STID = StockTrans.SMID,
                                           ItemDrug = DrugMaster.Where(x => x.ID == StockTrans.ItemID).Select(x => x.Brand).FirstOrDefault(),
                                           ItemID = Convert.ToString(StockTrans.ItemID),
                                           UOMname = uommaster.Where(x => x.id == StockTrans.UOMID).Select(x => x.Description).FirstOrDefault(),
                                           UOMID = StockTrans.UOMID,
                                           ItemQty = StockTrans.ItemQty,
                                           ItemRate = StockTrans.ItemRate,
                                           ItemValue = StockTrans.ItemValue,
                                           DiscountPercentage = StockTrans.DiscountPercentage,
                                           DiscountAmount = StockTrans.DiscountAmount,
                                           GSTPercentage = StockTrans.GSTPercentage,
                                           GSTTaxValue = StockTrans.GSTTaxValue,
                                           CESSPercentage = StockTrans.CESSPercentage,
                                           CESSAmount = StockTrans.CESSValue,
                                           AdditionalCESSPercentage = StockTrans.AdditionalCESSPercentage,
                                           AddCESSAmount = StockTrans.AdditionalCESSValue,
                                           GrossAmount = StockTrans.ItemQty * StockTrans.ItemRate - StockTrans.ItemQty * StockTrans.ItemRate * StockTrans.DiscountPercentage / 100,
                                           TotalAmount = (StockTrans.ItemQty * StockTrans.ItemRate - StockTrans.ItemQty * StockTrans.ItemRate * StockTrans.DiscountPercentage / 100) +
                                           (StockTrans.GSTTaxValue) + (StockTrans.CESSValue) + (StockTrans.AdditionalCESSValue),
                                           TaxDescription = TaxMaster.Where(a => a.ID == DrugMaster.Where(x => x.ID == StockTrans.ItemID).Select(s => s.TaxID).FirstOrDefault()).Select(e => e.TaxDescription).FirstOrDefault(),
                                           CESSDescription = TaxMaster.Where(a => a.ID == DrugMaster.Where(x => x.ID == StockTrans.ItemID).Select(s => s.TaxID).FirstOrDefault()).Select(e => e.CESSDescription).FirstOrDefault(),
                                           AdditionalCESSDescription = TaxMaster.Where(a => a.ID == DrugMaster.Where(x => x.ID == StockTrans.ItemID).Select(s => s.TaxID).FirstOrDefault()).Select(e => e.AdditionalCESSDescription).FirstOrDefault(),
                                           GenericName = durggrp.Where(e => e.ID == drugss.Where(x => x.ID == StockTrans.ItemID).Select(x => x.GenericName).FirstOrDefault()).Select(d => d.Description).FirstOrDefault(),
                                           Tracking = drugss.Where(x => x.ID == StockTrans.ItemID).Select(x => x.DrugTracker).FirstOrDefault(),
                                           Type = Enum.GetName(typeof(DrugCategory), drugss.Where(x => x.ID == StockTrans.ItemID).Select(x => x.DrugCategory).FirstOrDefault()),

                                       }).ToList();




            try
            {

                return new
                {
                    Pgrnno = GrnNumber,
                    Pgrndate = GrnDate,
                    pqtnno = QtnNumber,
                    pqtndate = QtnDate,
                    pdcno = DcNumber,
                    pdcdate = DcDate,
                    pname = Name,
                    padress1 = Address1,
                    padress2 = Address2,
                    pphno = PhoneNo,
                    pgstno = GSTNo,
                    pdlno = DLno,
                    pdldate = DLDateNo,
                    PLocation = Grnprintdetails.Location,
                    Pcity = Grnprintdetails.ParentDescriptioncity,
                    Pstate = Grnprintdetails.ParentDescriptionstate,
                    Pcountry = Grnprintdetails.ParentDescriptioncountry,
                    pStorename = Storename,
                    pTremandcond = Tremandcond,
                    PAddress = Address,
                    PAddress1 = cAddress1,
                    PAddress2 = cAddress2,
                    PPhone = phone,
                    PWeb = web,
                    PCompnayname = Compnayname,
                    PComplocID = companylocationID,
                    PComplocname = companylocation,
                    PGrossProductValue = GrossProductValue,
                    PTotalDiscountValue = TotalDiscountValue,
                    PTotalTaxValue = TotalTaxValue,
                    PTotalPOValue = TotalPOValue,
                    PTotalCESSValue = TotalCESSValue,
                    PTotalAdditionalCESSValue = TotalAdditionalCESSValue,
                    Stocktransdetailss = GrnwithoutPOdetails.ToList(),
                    Gettotalamount = GrnwithoutPOdetails.Select(x => x.ItemValue).ToList().Sum(),
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
        public dynamic GrnWoPohis(int cmpid, int TransactionId, string Time)
        {
            TimeSpan ts = TimeSpan.Parse(Time);
            var Stockmaster = WYNKContext.StockMaster.ToList();
            var VendorMaster = CMPSContext.VendorMaster.ToList();
            var Storemasters = CMPSContext.Storemasters.ToList();

            var GetgrnwithoutPOdetails = (from Sm in Stockmaster.Where(x => x.CMPID == cmpid && x.IsDeleted == false && x.TransactionID == TransactionId && x.IsDeleted == false)

                                          select new
                                          {
                                              RandomUniqueID = Sm.RandomUniqueID,
                                              Grnno = Sm.DocumentNumber,
                                              Grndate = Sm.DocumentDate.Add(ts),
                                              Qtnno = Sm.QtnNumber,
                                              Qtndate = Sm.QtnDate != null ? Sm.QtnDate.Value.Add(ts) : (DateTime?)null,
                                              Dcno = Sm.DCNumber,
                                              Dcdate = Sm.DCDate != null ? Sm.DCDate.Value.Add(ts) : (DateTime?)null,
                                              Vendorname = VendorMaster.Where(x => x.ID == Sm.VendorID).Select(x => x.Name).FirstOrDefault(),
                                              Storename = Storemasters.Where(x => x.StoreID == Sm.StoreID).Select(x => x.Storename).FirstOrDefault(),
                                              Grn = int.Parse(Regex.Match(Sm.DocumentNumber, @"\d+").Value),
        }).ToList();

            return GetgrnwithoutPOdetails;


        }
        public dynamic GrnWoPobatchhistory(string RandomUniqueID, int TransactionId)
        {

            var ItemBatchTrans = WYNKContext.ItemBatchTrans.ToList();
            var DrugMaster = WYNKContext.DrugMaster.ToList();
            var DrugGroup = WYNKContext.DrugGroup.ToList();

            var GetgrnwithoutPObatchhistory = (from IBT in ItemBatchTrans.Where(x => x.SMID == RandomUniqueID && x.TC == TransactionId)

                                               select new
                                               {

                                                   ItembatchNumber = IBT.ItemBatchNumber,
                                                   ItembatchTransactedQty = IBT.ItemBatchTransactedQty,
                                                   ItembatchExpiry = IBT.ItemBatchExpiry,
                                                   Itemdrugname = DrugMaster.Where(x => x.ID == IBT.ItemID).Select(d => d.Brand).FirstOrDefault(),
                                                   Itemgenericname = DrugGroup.Where(e => e.ID == DrugMaster.Where(x => x.ID == IBT.ItemID).Select(d => d.GenericName).FirstOrDefault()).Select(t => t.Description).FirstOrDefault(),
                                               }).ToList();

            return GetgrnwithoutPObatchhistory;


        }
        public dynamic GrnWoPoserialhistory(string Pgrnno, int TransactionId)
        {

            var ItemSerial = WYNKContext.ItemSerial.ToList();
            var DrugMaster = WYNKContext.DrugMaster.ToList();
            var DrugGroup = WYNKContext.DrugGroup.ToList();

            var GetgrnwithoutPOserialhistory = (from IS in ItemSerial.Where(x => x.GRNNo == Pgrnno && x.TC == TransactionId)

                                                select new
                                                {

                                                    ItemserialNumber = IS.SerialNo,
                                                    ItemserialExpiry = IS.ExpiryDate,
                                                    Itemdrugname = DrugMaster.Where(x => x.ID == IS.ItemID).Select(d => d.Brand).FirstOrDefault(),
                                                    Itemgenericname = DrugGroup.Where(e => e.ID == DrugMaster.Where(x => x.ID == IS.ItemID).Select(d => d.GenericName).FirstOrDefault()).Select(t => t.Description).FirstOrDefault(),

                                                }).ToList();

            return GetgrnwithoutPOserialhistory;


        }

    }
}






