using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class BillingPharmacy
    {
        public ICollection<UINSearchDetails> UINSearchDetails { get; set; }
        public ICollection<DateMedicalPrescription> DateMedicalPrescription { get; set; }
        public ICollection<MedicalPrescriptionIddetails> MedicalPrescriptionIddetails { get; set; }
        public ICollection<Payment_Master> paymenttran { get; set; }
        public Payment_Master payment { get; set; }
        public MedicalBill_Master MedicalBillMaster { get; set; }
        public MedicalBill_Tran MedicalBillTran { get; set; }
        public ICollection<GetClosedDetails> GetClosedDetails { get; set; }
        public ICollection<ExtraDrugDetails> ExtraDrugDetails { get; set; }

        public ICollection<CurrentStockQuantity> CurrentStockQuantity { get; set; }

        public ICollection<CurrentStockQuantityCheck> exactbatch { get; set; }

        public ICollection<AllotedBatch> ExactAlloted { get; set; }

        public ICollection<InSufficientDrug> InSufficientDrugs { get; set; }


        public int? MedicalBillID { get; set; }
        public int StoreID { get; set; }
        public int PrintTotal { get; set; }
        public decimal? RemainingAmount { get; set; }
        public decimal? PaidTotal { get; set; }
        public object CMPDetails { get; set; }
        public string ReceiptRunningNo { get; set; }
        public DateTime? ReceiptDatetime { get; set; }
    }


    public class PendingPayments
    {

        public ICollection<Payment_Master> paymenttrans { get; set; }
        public PendingPayment PendingPayment { get; set; }
        public string  ReceiptRunningNo { get; set; }

}

    public class PendingPayment
    {
        public string UIN { get; set; }
        public string PayRef { get; set; }
        public int MedicalBillID { get; set; }
        public int TC { get; set; }
        public int CMPID { get; set; }
        public int CreatedBy { get; set; }
        public decimal CreditItemCost { get; set; }
    }



    public class OutPatientBillingPharmacy
    {
        public MedicalBill_Master MedicalBillMaster { get; set; }
        public MedicalBill_Tran MedicalBillTran { get; set; }
        public ICollection<MedicalPrescriptionIddetails> MedicalPrescriptionIddetails { get; set; }
        public int StoreID { get; set; }
        public int CmpId { get; set; }
        public int Tc { get; set; }
        public int CreatedBy { get; set; }
        public ICollection<Payment_Master> paymenttrans { get; set; }
        public ICollection<Alert> Alerts { get; set; }
        public int? PrintTotal { get; set; }
        public string Status { get; set; }

        public ICollection<InSufficientDrug> InSufficientDrugs { get; set; }
        public ICollection<SerialDetail> InSufficientSerials { get; set; }

        public ICollection<OtherDrugs> InSufficientOtherDrugs { get; set; }

        public int? MedicalBillID { get; set; }

        public string ReceiptRunningNo { get; set; }
        public int billtranid { get; set; }

        public string GivenDate { get; set; }
        public string FYear { get; set; }
    }

    public class Alert
    {
        public string DrugName { get; set; }
        public string BatchNo { get; set; }
        public int ExpiresInDays { get; set; }
    }

    public class InSufficientDrug
    {
        public int DrugId { get; set; }
        public decimal? BalanceQuantity { get; set; }
        public string DrugName { get; set; }

        public string BatchNumber { get; set; }

        public DateTime ExpiryDate { get; set; }
    }

    public class CurrentStockQuantity
    {
        public int DrugId { get; set; }
        public string BatchNumber { get; set; }
        public decimal BalanceQuantity { get; set; }
        public string ItemBatchExpiry { get; set; }
    }


    public class StockCheckModel
    {
        public decimal ActualTakenFromBatch { get; set; }
        public decimal totalQuantity { get; set; }
        public decimal StillNeededQuantity { get; set; }
        public AllotedBatch AllotedBatch { get; set; }
    }

    public class CurrentStockQuantityCheck
    {
        public int DrugId { get; set; }
        public string itemBatchNo { get; set; }
        public decimal balanceQty { get; set; }
        public string ExpiryDate { get; set; }
        public DateTime CreatedUTC { get; set; }
        public int? LockedQuantity { get; set; }

    }


    public class AllotedBatch
    {
        public int DrugId { get; set; }
        public string DrugName { get; set; }
        public string itemBatchNo { get; set; }
        public decimal balanceQty { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime? CreatedUTC { get; set; }
        public decimal GoingToIssue { get; set; }
        public string ExpireInDays { get; set; }

    }

    public class MedicalBillRegisterDetails
    {
        public ICollection<getRegDet> getRegisterDetail { get; set; }
    }

    public class getRegDet
    {

        public string BillNo { get; set; }
        public string BillDate { get; set; }
        public string PatientName { get; set; }
        //  public ICollection<MedicalBillDetails> ItemDetails { get; set; }
        public string Drug { get; set; }

        public string UOM { get; set; }
        public decimal Quantity { get; set; }
        public decimal? Amount { get; set; }

        public decimal? DiscountPerc { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? GrossAmount { get; set; }


        public string TaxDescription { get; set; }
        public string CessDescription { get; set; }
        public string AddCessDescription { get; set; }

        public decimal? TaxPerc { get; set; }
        public decimal? CessPerc { get; set; }
        public decimal? AddCessPerc { get; set; }

        public decimal? TaxValue { get; set; }
        public decimal? CessValue { get; set; }
        public decimal? AddCessValue { get; set; }
        public decimal? NetAmount { get; set; }
    }

    public class MedicalBillDetails
    {
        public string Drug { get; set; }

        public string UOM { get; set; }
        public decimal Quantity { get; set; }
        public decimal? Amount { get; set; }

        public decimal? DiscountPerc { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? GrossAmount { get; set; }


        public string TaxDescription { get; set; }
        public string CessDescription { get; set; }
        public string AddCessDescription { get; set; }

        public decimal? TaxPerc { get; set; }
        public decimal? CessPerc { get; set; }
        public decimal? AddCessPerc { get; set; }

        public decimal? TaxValue { get; set; }
        public decimal? CessValue { get; set; }
        public decimal? AddCessValue { get; set; }
        public decimal? NetAmount { get; set; }

    }

    public class MedicalPrescriptionIddetails
    {
        public int DrugID { get; set; }
        public string Drug { get; set; }

        public int? Quantity { get; set; }
        public string UOM { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal? GST { get; set; }
        public decimal? Cess { get; set; }
        public decimal? AddCess { get; set; }
        public decimal? CessValue { get; set; }
        public decimal? AddCessValue { get; set; }
        public decimal GSTValue { get; set; }
        public decimal Discount { get; set; }
        public Boolean IsMedicinePrescribed { get; set; }
        public Boolean IsAvailable { get; set; }
        public decimal Amount { get; set; }
        public decimal DiscountAmount { get; set; }
        //public double DiscountAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal TotalCost { get; set; }
        public decimal ItemRate { get; set; }
        public int MedicalPrescriptionTranId { get; set; }


        public int Reqqty { get; set; }
        public decimal? AvailQuantity { get; set; }
        public Boolean? IsSerial { get; set; }
        public ICollection<BatchInfo> BatchDetail { get; set; }
        public ICollection<SerialInfo> SerialsInfo { get; set; }


        public ICollection<SerialInfo> SelectedList { get; set; }
        public string TaxDescription { get; set; }
        public string CessDescription { get; set; }
        public string AddCessDescription { get; set; }

        public int? TaxID { get; set; }
    }


    public class BatchInfo
    {
        public string BatchNo { get; set; }
        public decimal TotalQty { get; set; }
        public decimal? BalanceQty { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int QtyTaken { get; set; }
        public string CriticalIntervalDay { get; set; }
        public DateTime CriticalIntervalDate { get; set; }
        public DateTime RetestIntervalDate { get; set; }
        public string RetestIntervalDays { get; set; }
        public string ExpireInDays { get; set; }
    }


    public class SerialInfo
    {
        public string SerialNo { get; set; }
        public string BillNo { get; set; }
        public DateTime? ExpiryDate { get; set; } 
    }

    public class GetClosedDetails
    {
        public int DrugID { get; set; }
        public string Drug { get; set; }
        public int Reqqty { get; set; }
        public string UOM { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal GST { get; set; }
        public decimal GSTValue { get; set; }
        public decimal Discount { get; set; }
        public decimal Amount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal? Cess { get; set; }
        public decimal? AddCess { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal TotalCost { get; set; }
        //public decimal ItemRate { get; set; }
        public decimal? CessValue { get; set; }
        public decimal? AddCessValue { get; set; }
        public string CessDescription { get; set; }
        public string AddCessDescription { get; set; }
        public string TaxDescription { get; set; }
    }


    public class UINSearchDetails
    {
        public string PatientName { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string PrescribedDoctor { get; set; }
        public DateTime PrescribedDate { get; set; }
        public int RegistrationTranID { get; set; }

    }

    public class DateMedicalPrescription
    {
        public string MedicalPrescriptionID { get; set; }
        public string UIN { get; set; }
        public int RegistrationTranID { get; set; }

        public string MedicalPrescriptionNo { get; set; }
        public string PatientName { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string PrescribedDoctor { get; set; }
        public DateTime? PrescribedDate { get; set; }

        public string Status { get; set; }


    }


    public class ExtraDrugDetails
    {

        public int DrugID { get; set; }
        public string Drug { get; set; }

        public int? Quantity { get; set; }
        public string UOM { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? GST { get; set; }
        public decimal? GSTValue { get; set; }
        public decimal? Discount { get; set; }
        public Boolean IsMedicinePrescribed { get; set; }
        public decimal? Amount { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? GrossAmount { get; set; }
        public decimal? TotalCost { get; set; }

        public decimal? ItemRate { get; set; }
        public decimal? Cess { get; set; }
        public decimal? AddCess { get; set; }
    }



    public class BillingReturns
    {
        public int TC { get; set; }
        public int CreatedBy { get; set; }
        public int Cmpid { get; set; }
        public int StoreID { get; set; }
        public int MedicalBillId { get; set; }
        public string BillNo { get; set; }
        public string ReceiptRunningNo { get; set; }
        public string RunningNo { get; set; }
        public string UIN { get; set; }
        public string BillType { get; set; }
        public string GivenDate { get; set; }
        public string FYear { get; set; }
        public ICollection<ReturnMedicalBillItemdetail> ReturnMedicalBillItemdetails { get; set; }
        public ICollection<Payment_Master> paymenttrans { get; set; }

    }

    public class ReturnMedicalBillItemdetail
    {
        public int ItemId { get; set; }
        public string Drug { get; set; }
        public string UOM { get; set; }
        public int Qty { get; set; }
        public int AlreadyReturnQty { get; set; }
        public int ReturnQty { get; set; }
        public int UnitPrice { get; set; }
        public int Amount { get; set; }
        public decimal Discount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal? GST { get; set; }
        public decimal? Cess { get; set; }
        public decimal? AddCess { get; set; }
        public decimal TotalCost { get; set; }
        public decimal? GSTValue { get; set; }
        public decimal? CessValue { get; set; }
        public decimal? AddCessValue { get; set; }
        public int? TaxID { get; set; }
        public int MedicalbillTranID { get; set; }
        public int MedicalPrescriptionTranId { get; set; }
        public string MedicalPrescriptionID { get; set; }
        public Boolean IsMedicinePrescribed { get; set; }
        public Boolean? IsSerial { get; set; }
        public ICollection<ReturnBatchDetails> BatchDetails { get; set; }
        //  public ICollection<SerialInfo> SerialDetails { get; set; }
        public ICollection<ReturnSerialDetails> SelectedList { get; set; }
        //  public int? SelectedBatchQty { get; set; }
    }

    public class ReturnBatchDetails
    {
        public string BatchNo { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public decimal IssuedQty { get; set; }
        public decimal QtyToReturn { get; set; }
        public decimal ReturnQty { get; set; }
        public string SMID { get; set; }
        public string STID { get; set; }
        public string ItemBatchTransID { get; set; }
    }

    public class ReturnSerialDetails
    {
        public string SerialNo { get; set; }
        public string BillNo { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }


    //public class ClosedReturnBatchDetails
    //{
    //    public string BatchNo { get; set; }
    //    public decimal Qty { get; set; }
    //    public string BillNo { get; set; }
    //    public DateTime BillDate { get; set; }
    //}

    public class CreditItemDetails
    {
        public int DrugID { get; set; }
        public string Drug { get; set; }
        public int Qty { get; set; }
        public int ReturnQty { get; set; }
        public string UOM { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal? GST { get; set; }
        public decimal? GSTValue { get; set; }
        public decimal? Cess { get; set; }
        public decimal? CessValue { get; set; }
        public decimal? AddCess { get; set; }
        public decimal? AddCessValue { get; set; }
        public decimal TotalCost { get; set; }
        public string CessDescription { get; set; }
        public string AddCessDescription { get; set; }
        public string TaxDescription { get; set; }
    }

    public class CreditBillingDetails 
    {
        //public int? MedicalBillID { get; set; }
        public string  InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public ICollection<CreditItemDetails> CreditItemDetails { get; set; }
        public ICollection<Payment_Master> paymenttran { get; set; }
        public decimal? RemainingAmount { get; set; }
        public decimal? CreditTotatAmount { get; set; }
      //  public decimal? PaidTotal { get; set; }
        public object CMPDetails { get; set; }
    }

}

