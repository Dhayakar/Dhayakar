using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Helpers;

namespace WYNK.Data.Repository.Operation
{
    public static class AddBilling
    {
        public static MedicalBill_Tran AddMedicalBillTran(MedicalPrescriptionIddetails item, string medprestran,int getmedicalBillMasterId,int CmpId,int? MedicalBillParentID)
        {
            var medicalbilltran = new MedicalBill_Tran();
            medicalbilltran.DrugID = item.DrugID;
            medicalbilltran.Quantity = item.Reqqty;
            medicalbilltran.MedicalPrescriptionID = medprestran;
            medicalbilltran.MedicalBillID = getmedicalBillMasterId;
            medicalbilltran.UOM = item.UOM;
            medicalbilltran.MedicalBillParentID = MedicalBillParentID;
            medicalbilltran.ItemValue = item.Amount;
            medicalbilltran.ItemRate = item.UnitPrice;
            medicalbilltran.IsMedicinePrescribed = item.IsMedicinePrescribed;
            medicalbilltran.GSTPercentage = item.GST;
            medicalbilltran.SGSTPercentage = item.GST / 2;
            medicalbilltran.CGSTPercentage = item.GST / 2;
            medicalbilltran.GSTTaxValue = item.GSTValue;
            medicalbilltran.SGSTTaxValue = item.GSTValue / 2;
            medicalbilltran.CGSTTaxValue = item.GSTValue / 2;
            medicalbilltran.CESSPercentage = item.Cess;
            medicalbilltran.CESSValue = item.CessValue;
            medicalbilltran.AdditionalCESSPercentage = item.AddCess;
            medicalbilltran.AdditionalCESSValue = item.AddCessValue;
            medicalbilltran.DiscountPercentage = item.Discount;
            medicalbilltran.DiscountAmount = item.DiscountAmount;
            medicalbilltran.MedicalPrescriptionTranId = item.MedicalPrescriptionTranId;
            medicalbilltran.TaxID = item.TaxID;
            medicalbilltran.CreatedBy = CmpId;
            medicalbilltran.CreatedUTC = DateTime.UtcNow;          
            medicalbilltran.Status = true;          
            return medicalbilltran;
        }

        public static PatientAccountDetail AddPatientAccountDetail(MedicalPrescriptionIddetails item, int MedicalChargesServicesID,int PaidId)
        {
            var PatientAccountDetail = new PatientAccountDetail();
            PatientAccountDetail.PAID = PaidId;
            PatientAccountDetail.OLMID = MedicalChargesServicesID;
            PatientAccountDetail.ServiceTypeID = MedicalChargesServicesID;
            PatientAccountDetail.ServiceDescription = "Medical Charges";
            PatientAccountDetail.TotalProductValue = PatientAccountDetail.TotalProductValue != null ? (PatientAccountDetail.TotalProductValue + item.Amount) : 0 + item.Amount;
            PatientAccountDetail.TotalDiscountValue = PatientAccountDetail.TotalDiscountValue != null ? (PatientAccountDetail.TotalDiscountValue + item.DiscountAmount) : 0 + item.DiscountAmount;
            PatientAccountDetail.TotalTaxValue = PatientAccountDetail.TotalTaxValue != null ? (PatientAccountDetail.TotalTaxValue + item.GSTValue) : 0 + item.GSTValue;
            PatientAccountDetail.TotalCGSTTaxValue = PatientAccountDetail.TotalCGSTTaxValue != null ? (PatientAccountDetail.TotalCGSTTaxValue + item.GSTValue / 2) : 0 + item.GSTValue / 2;
            PatientAccountDetail.TotalSGSTTaxValue = PatientAccountDetail.TotalSGSTTaxValue != null ? (PatientAccountDetail.TotalSGSTTaxValue + item.GSTValue / 2) : 0 + item.GSTValue / 2;
            PatientAccountDetail.TotalBillValue = PatientAccountDetail.TotalBillValue != null ? (PatientAccountDetail.TotalBillValue + item.TotalCost) : 0 + item.TotalCost;
            PatientAccountDetail.CreatedUTC = DateTime.UtcNow;
            return PatientAccountDetail;
        }

        public static StockTran AddstockTran(MedicalPrescriptionIddetails item,string stockmasterIdentity,int CreatedBy)
        {
            var stockTran = new StockTran();
            stockTran.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
            stockTran.SMID = stockmasterIdentity;
            stockTran.ItemID = item.DrugID;
            stockTran.ItemQty = Convert.ToInt32(item.Reqqty);
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
            stockTran.CreatedBy = CreatedBy;
            return stockTran;
        }


        //public static ItemBatchTrans AddItemBatchTrans(AllotedBatch item1,long stockmasterIdentity,long STID,long ItemBatchID,int Tc,int CreatedBy)
        //{
        //    var itemBatchTrans = new ItemBatchTrans();
        //    itemBatchTrans.ItemBatchID = ItemBatchID;
        //    itemBatchTrans.TC = Tc;
        //    itemBatchTrans.SMID = stockmasterIdentity;
        //    itemBatchTrans.STID = STID;
        //    itemBatchTrans.ItemID = item1.DrugId;
        //    itemBatchTrans.ItemBatchNumber = item1.itemBatchNo;
        //    itemBatchTrans.ItemBatchTransactedQty = item1.GoingToIssue;
        //    itemBatchTrans.ItemBatchExpiry = Convert.ToDateTime(item1.ExpiryDate);
        //    itemBatchTrans.UOMID = 5;
        //    itemBatchTrans.ContraItemBatchID = 7;
        //    itemBatchTrans.CreatedUTC = DateTime.UtcNow;
        //    itemBatchTrans.CreatedBy = CreatedBy;
        //    return itemBatchTrans;
        //}


        //public static StockMaster AddstockMaster (string MedicalBillNo, DateTime MedicalBillDate,int StoreId,int MedicalBillIdentity)
        //{
        //    var stockmas = new StockMaster();
        //    stockmas.CMPID = 1058;
        //    stockmas.TransactionID = 4;
        //    stockmas.DocumentNumber = MedicalBillNo;
        //    stockmas.DocumentDate = MedicalBillDate;
        //    stockmas.StoreID = StoreId;
        //    stockmas.TransactionType = "I";
        //    stockmas.VendorID = MedicalBillIdentity;
        //    stockmas.CreatedBy = 1;
        //    stockmas.CreatedUTC = DateTime.UtcNow;
        //    return stockmas;
        //}

        //public static PatientAccountDetailTax AddPatientAccountDetailTax (MedicalPrescriptionIddetails item,int PAccDetailID,int? TaxID)
        //{
        //    var PatientAccountDetailTax = new PatientAccountDetailTax();
        //    PatientAccountDetailTax.PAccDetailID = PAccDetailID;
        //    PatientAccountDetailTax.ServiceTypeID = item.DrugID;
        //    PatientAccountDetailTax.Description = "Medical Charges";
        //    PatientAccountDetailTax.TaxID = TaxID;
        //    PatientAccountDetailTax.TaxPercentage = Convert.ToInt32(item.GST);
        //    PatientAccountDetailTax.TotalProductValue = item.Amount;
        //    PatientAccountDetailTax.TotalDiscountValue = item.DiscountAmount;
        //    PatientAccountDetailTax.TotalTaxValue = item.GSTValue;
        //    PatientAccountDetailTax.TotalCGSTTaxValue = item.GSTValue / 2;
        //    PatientAccountDetailTax.TotalSGSTTaxValue = item.GSTValue / 2;
        //    PatientAccountDetailTax.TotalValue = item.TotalCost;
        //    PatientAccountDetailTax.CreatedUTC = DateTime.Now;
        //    PatientAccountDetailTax.CreatedBy = 1;
        //    return PatientAccountDetailTax;
        //}


        //public static PatientAccount UpdatePatientAccount (MedicalPrescriptionIddetails item, PatientAccount PatientAccounts)
        //{
        //    PatientAccounts.TotalProductValue = PatientAccounts.TotalProductValue != null ? (PatientAccounts.TotalProductValue + item.Amount) : 0 + item.Amount;
        //    PatientAccounts.TotalDiscountValue = PatientAccounts.TotalDiscountValue != null ? (PatientAccounts.TotalDiscountValue + item.DiscountAmount) : 0 + item.DiscountAmount;
        //    PatientAccounts.TotalTaxValue = PatientAccounts.TotalTaxValue != null ? (PatientAccounts.TotalTaxValue + item.GSTValue) : 0 + item.GSTValue;
        //    PatientAccounts.TotalCGSTTaxValue = PatientAccounts.TotalCGSTTaxValue != null ? (PatientAccounts.TotalCGSTTaxValue + item.GSTValue / 2) : 0 + item.GSTValue / 2;
        //    PatientAccounts.TotalSGSTTaxValue = PatientAccounts.TotalSGSTTaxValue != null ? (PatientAccounts.TotalSGSTTaxValue + item.GSTValue / 2) : 0 + item.GSTValue / 2;
        //    PatientAccounts.TotalBillValue = PatientAccounts.TotalBillValue != null ? (PatientAccounts.TotalBillValue + item.TotalCost) : 0 + item.TotalCost;
        //    PatientAccounts.UpdatedBy = 1;
        //    return PatientAccounts;
        //}


        //public static PatientAccountDetail UpdatePatientAccountDetail(MedicalPrescriptionIddetails item, PatientAccountDetail PatientAccountDetail)
        //{
        //    PatientAccountDetail.TotalProductValue = PatientAccountDetail.TotalProductValue != null ? (PatientAccountDetail.TotalProductValue + item.Amount) : 0 + item.Amount;
        //    PatientAccountDetail.TotalDiscountValue = PatientAccountDetail.TotalDiscountValue != null ? (PatientAccountDetail.TotalDiscountValue + item.DiscountAmount) : 0 + item.DiscountAmount;
        //    PatientAccountDetail.TotalTaxValue = PatientAccountDetail.TotalTaxValue != null ? (PatientAccountDetail.TotalTaxValue + item.GSTValue) : 0 + item.GSTValue;
        //    PatientAccountDetail.TotalCGSTTaxValue = PatientAccountDetail.TotalCGSTTaxValue != null ? (PatientAccountDetail.TotalCGSTTaxValue + item.GSTValue / 2) : 0 + item.GSTValue / 2;
        //    PatientAccountDetail.TotalSGSTTaxValue = PatientAccountDetail.TotalSGSTTaxValue != null ? (PatientAccountDetail.TotalSGSTTaxValue + item.GSTValue / 2) : 0 + item.GSTValue / 2;
        //    PatientAccountDetail.TotalBillValue = PatientAccountDetail.TotalBillValue != null ? (PatientAccountDetail.TotalBillValue + item.TotalCost) : 0 + item.TotalCost;
        //    return PatientAccountDetail;
        //}


        public static MedicalBill_Master UpdateMedicalBill_Master(MedicalPrescriptionIddetails item, MedicalBill_Master medicalbills)
        {
            medicalbills.GrossProductValue = medicalbills.GrossProductValue != null ? (medicalbills.GrossProductValue + item.GrossAmount) : 0 + item.GrossAmount;
            medicalbills.TotalDiscountValue = medicalbills.TotalDiscountValue != null ? (medicalbills.TotalDiscountValue + item.DiscountAmount) : 0 + item.DiscountAmount;
            medicalbills.CESSValue = medicalbills.CESSValue != null ? medicalbills.CESSValue + item.CessValue : 0 + item.CessValue;
            medicalbills.AdditionalCESSValue = medicalbills.AdditionalCESSValue != null ? medicalbills.AdditionalCESSValue + item.AddCessValue : 0 + item.AddCessValue;
            medicalbills.TotalTaxValue = medicalbills.TotalTaxValue != null ? (medicalbills.TotalTaxValue + item.GSTValue + item.CessValue + item.AddCessValue) : 0 + item.GSTValue + item.CessValue + item.AddCessValue;
            medicalbills.TotalCGSTTaxValue = medicalbills.TotalCGSTTaxValue != null ? (medicalbills.TotalCGSTTaxValue + item.GSTValue / 2) : 0 + (item.GSTValue + item.CessValue + item.AddCessValue) / 2;
            medicalbills.TotalSGSTTaxValue = medicalbills.TotalSGSTTaxValue != null ? (medicalbills.TotalSGSTTaxValue + item.GSTValue / 2) : 0 + (item.GSTValue + item.CessValue + item.AddCessValue) / 2;
            medicalbills.TotalBillValue = medicalbills.TotalBillValue != null ? (medicalbills.TotalBillValue + item.TotalCost) : 0 + item.TotalCost;
            return medicalbills;
        }


        public static StockMaster UpdateStockMaster(MedicalPrescriptionIddetails item, StockMaster stockMas)
        {
            stockMas.GrossProductValue = stockMas.GrossProductValue != null ? (stockMas.GrossProductValue + item.GrossAmount) : 0 + item.GrossAmount;
            stockMas.TotalDiscountValue = stockMas.TotalDiscountValue != null ? (stockMas.TotalDiscountValue + item.DiscountAmount) : 0 + item.DiscountAmount;
            stockMas.TotalTaxValue = stockMas.TotalTaxValue != null ? (stockMas.TotalTaxValue + item.GSTValue + item.CessValue + item.AddCessValue) : 0 + item.GSTValue + item.CessValue + item.AddCessValue;
            stockMas.TotalCGSTTaxValue = stockMas.TotalCGSTTaxValue != null ? (stockMas.TotalCGSTTaxValue + item.GSTValue / 2 + item.CessValue / 2 + item.AddCessValue / 2) : 0 + item.GSTValue / 2 + item.CessValue / 2 + item.AddCessValue / 2;
            stockMas.TotalSGSTTaxValue = stockMas.TotalSGSTTaxValue != null ? (stockMas.TotalSGSTTaxValue + item.GSTValue / 2 + item.CessValue / 2 + item.AddCessValue / 2) : 0 + item.GSTValue / 2 + item.CessValue / 2 + item.AddCessValue / 2;
            stockMas.TotalPOValue = stockMas.TotalPOValue != null ? (stockMas.TotalPOValue + item.TotalCost) : 0 + item.TotalCost;
            stockMas.TotalCESSValue = stockMas.TotalCESSValue != null ? (stockMas.TotalCESSValue + item.CessValue) : 0 + item.CessValue;
            stockMas.TotalAdditionalCESSValue = stockMas.TotalAdditionalCESSValue != null ? (stockMas.TotalAdditionalCESSValue + item.AddCessValue) : 0 + item.AddCessValue;
            return stockMas;
        }


        public static StockTran updateStockTran(StockTran stockTran,decimal GoingToIssue)
        {
            stockTran.ItemQty = stockTran.ItemQty + GoingToIssue;

            return stockTran;
        }

        public static StockMaster AddstockMaster1(string MedicalBillNo, DateTime MedicalBillDate, int StoreId, int? ReceiverStoreId, int MedicalBillIdentity, int TransactionID, string TransactionType,int CMPID,int CreatedBy,string Fyear)
        {
            var stockmas = new StockMaster();
            stockmas.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
            stockmas.CMPID = CMPID;
            stockmas.TransactionID = TransactionID;
            stockmas.DocumentNumber = MedicalBillNo;
            stockmas.DocumentDate = MedicalBillDate;
            stockmas.StoreID = StoreId;
            stockmas.ReceiverStoreID = ReceiverStoreId;
            stockmas.TransactionType = TransactionType;
            stockmas.VendorID = MedicalBillIdentity;
            stockmas.CreatedBy = CreatedBy;
            stockmas.CreatedUTC = DateTime.UtcNow;
            stockmas.Fyear = Fyear;
            return stockmas;
        }


        public static StockTran AddstockTran1(string stockmasterIdentity ,int DrugID,decimal Quantity,object ContraSMID, object ContraSTID,int Createdby)
        {
            var stockTran = new StockTran();
            stockTran.SMID = stockmasterIdentity;
            stockTran.ItemID = DrugID;
            stockTran.ItemQty = Quantity;
            stockTran.ContraSMID = Convert.ToString(ContraSMID);
            stockTran.ContraSTID = Convert.ToString(ContraSTID);
            stockTran.CreatedUTC = DateTime.UtcNow;
            stockTran.CreatedBy = Createdby;
            return stockTran;
        }


        public static ItemBatch itemBatch(int storeID ,int itemId, string ItemBatchNumber, Decimal ItemBatchQty, DateTime ItemBatchExpiry, int CreatedBy)
        {
            var itembatch = new ItemBatch();
            itembatch.ItemID = itemId;
            itembatch.ItemBatchNumber = ItemBatchNumber;
            itembatch.ItemBatchQty = ItemBatchQty;
            itembatch.ItemBatchissueQty = 0;
            itembatch.ItemBatchBalnceQty = ItemBatchQty;
            itembatch.ItemBatchExpiry = ItemBatchExpiry;
            itembatch.StoreID = storeID;
            itembatch.CreatedUTC = DateTime.UtcNow;
            itembatch.CreatedBy = CreatedBy;
            return itembatch;
        }

        public static ItemBatchTrans AddItemBatchTrans1(AllotedBatch item1, string stockmasterIdentity, string STID, string ItemBatchID,string ContraItemBatchID,int TransactionID, int? UOMID,int CreatedBy, int CMPID)
        {
            var itemBatchTrans = new ItemBatchTrans();
            itemBatchTrans.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
            itemBatchTrans.ItemBatchID = ItemBatchID;
            itemBatchTrans.TC = TransactionID;
            itemBatchTrans.SMID = stockmasterIdentity;
            itemBatchTrans.STID = STID;
            itemBatchTrans.ItemID = item1.DrugId;
            itemBatchTrans.ItemBatchNumber = item1.itemBatchNo;
            itemBatchTrans.ItemBatchTransactedQty = item1.GoingToIssue;
            itemBatchTrans.ItemBatchExpiry = Convert.ToDateTime(item1.ExpiryDate);
            itemBatchTrans.UOMID = UOMID;
            itemBatchTrans.ContraItemBatchID = ContraItemBatchID;
            itemBatchTrans.CreatedUTC = DateTime.UtcNow;
            itemBatchTrans.CreatedBy = CreatedBy;
            itemBatchTrans.cmpID = CMPID;
            return itemBatchTrans;
        }


        public static MainStockMaster AddMainstockMaster(string MedicalBillNo, DateTime MedicalBillDate, int StoreId, int? ReceiverStoreId, int MedicalBillIdentity, int TransactionID, string TransactionType, int CMPID, int CreatedBy,string Fyear)
        {
            var stockmas = new MainStockMaster();
            stockmas.CMPID = CMPID;
            stockmas.TransactionID = TransactionID;
            stockmas.DocumentNumber = MedicalBillNo;
            stockmas.DocumentDate = MedicalBillDate;
            stockmas.StoreID = StoreId;
            stockmas.ReceiverStoreID = ReceiverStoreId;
            stockmas.TransactionType = TransactionType;
            stockmas.VendorID = MedicalBillIdentity;
            stockmas.CreatedBy = CreatedBy;
            stockmas.CreatedUTC = DateTime.UtcNow;
            stockmas.Fyear = Fyear;
            return stockmas;
        }


        public static MainItemBatchTrans AddMainItemBatchTrans(AllotedBatch item1, long stockmasterIdentity, long STID, long ItemBatchID, Int64? ContraItemBatchID, int TransactionID, int? UOMID, int CreatedBy)
        {
            var itemBatchTrans = new MainItemBatchTrans();
            itemBatchTrans.ItemBatchID = ItemBatchID;
            itemBatchTrans.TC = TransactionID;
            itemBatchTrans.SMID = stockmasterIdentity;
            itemBatchTrans.STID = STID;
            itemBatchTrans.ItemID = item1.DrugId;
            itemBatchTrans.ItemBatchNumber = item1.itemBatchNo;
            itemBatchTrans.ItemBatchTransactedQty = item1.GoingToIssue;
            itemBatchTrans.ItemBatchExpiry = Convert.ToDateTime(item1.ExpiryDate);
            itemBatchTrans.UOMID = UOMID;
            itemBatchTrans.ContraItemBatchID = ContraItemBatchID;
            itemBatchTrans.CreatedUTC = DateTime.UtcNow;
            itemBatchTrans.CreatedBy = CreatedBy;
            return itemBatchTrans;
        }

        public static MainItemBatchTrans AddMainItemBatchTrans1(AllotedAnaesBatch item1, long stockmasterIdentity, long STID, long ItemBatchID, Int64? ContraItemBatchID, int TransactionID, int? UOMID, int CreatedBy)
        {
            var itemBatchTrans = new MainItemBatchTrans();
            itemBatchTrans.ItemBatchID = ItemBatchID;
            itemBatchTrans.TC = TransactionID;
            itemBatchTrans.SMID = stockmasterIdentity;
            itemBatchTrans.STID = STID;
            itemBatchTrans.ItemID = item1.DrugId;
            itemBatchTrans.ItemBatchNumber = item1.itemBatchNo;
            itemBatchTrans.ItemBatchTransactedQty = item1.GoingToIssue;
            itemBatchTrans.ItemBatchExpiry = Convert.ToDateTime(item1.ExpiryDate);
            itemBatchTrans.UOMID = UOMID;
            itemBatchTrans.ContraItemBatchID = ContraItemBatchID;
            itemBatchTrans.CreatedUTC = DateTime.UtcNow;
            itemBatchTrans.CreatedBy = CreatedBy;
            return itemBatchTrans;
        }

    }
}
