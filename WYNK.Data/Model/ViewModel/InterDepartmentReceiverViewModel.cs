using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class InterDepartmentReceiver
    {
        public string StoreName { get; set; }
        public ICollection<ReceivedDetails> ReceivedDetail { get; set; }
        public ICollection<ReceivedDetails1> ReceivedDetail1 { get; set; }
    }

    public class ReceivedDetails
    {
        public DateTime Date { get; set; }
        public string StockTransferNo { get; set; }
        public string SentFromStore { get; set; }
        public string StoreKeeper { get; set; }
        public string ReceivedStore { get; set; }
        public string Receivedby { get; set; }
    }

    public class ReceivedDetails1
    {
        public DateTime Date { get; set; }
        public string Datestring { get; set; }
        public string StockTransferNo { get; set; }
        public string Organization { get; set; }
        public string Branch { get; set; }
        public string SentFromStore { get; set; }
        public string StoreKeeper { get; set; }
        public string ReceivedStore { get; set; }
        public string Receivedby { get; set; }
        public string RandomUniqueId { get; set; }
    }

    public class InterDepartmentStockDetails
    {
        public string RecName { get; set; }
        public int CreatedBy { get; set; }
        public int TransactionID { get; set; }
        public int storeId { get; set; }
        public long? SentSmid { get; set; }
        public int? SenderstoreId { get; set; }
        public int? SenderUserId { get; set; }
        public DateTime? Senderdatetime { get; set; }
        public string Recdatetime { get; set; }
        public string FYear { get; set; }
        public int cmpid { get; set; }
        public string RunningNoStock { get; set; }
        public ICollection<StockTransferItemDetail> ItemDetails { get; set; }
        public List<fullItemsReceivedDetail> fullItemsReceivedDetails { get; set; }
    }

    public class fullItemsReceivedDetail 
    {
        public int ItemId { get; set; }
        public string DrugName { get; set; }
        public string GenericName { get; set; }
        public string UOM { get; set; }
        public string SMID { get; set; }
        public string STID { get; set; }
        public bool? IsSerial { get; set; }
        public string BatchSerial { get; set; }
        public string ItemBatchID { get; set; }
        public int SentQuantity { get; set; }
        public int? RecQuantity { get; set; }
        public int? Difference { get; set; }
        public DateTime? BatchExpiry { get; set; }
        public string Reasons { get; set; }
    }

    public class StockTransferItemDetail
    {
        public int ItemId { get; set; }
        public string DrugName { get; set; }
        public string GenericName { get; set; }
        public string UOM { get; set; }
        public int TotalQuantity { get; set; }
        public string SMID { get; set; }
        public string STID { get; set; }
        public bool? IsSerial { get; set; }
        public ICollection<ReceivedBatchDetail> ItemReceivedBatchDetails { get; set; }
        public ICollection<ReceivedSerialDetail> ItemReceivedSerialDetails { get; set; }
        public List<ReceivedOtherDetail> ItemReceivedOtherDetails { get; set; }

    //    public ReceivedOtherDetail ItemReceivedOtherDetails { get; set; }
    }


    public class ReceivedBatchDetail
    {
        public string ItemBatchID { get; set; }
        public string ItemBatchNo { get; set; }
        public int SentQuantity { get; set; }
      //  public int? RecQuantity { get; set; }
        public int? Difference { get; set; }
        public DateTime BatchExpiry { get; set; }
        public string Reasons { get; set; }
    }

    public class ReceivedSerialDetail
    {
        public string SerialNo { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string Reasons { get; set; }
        public int? Difference { get; set; }

    }



    public class ReceivedOtherDetail
    {
        public string Reasons { get; set; }
        public int? Difference { get; set; }
        public int? SentQty { get; set; }
        public int? Recqty { get; set; }

    }

    public class ViewRecStockDetails
    {
        public DateTime? Recdatetime { get; set; }
        public string RecName { get; set; }
        public string RunningNoStock { get; set; }
        public object CompanyDetails { get; set; }
        public ICollection<StockTransferItemDetail> ItemDetails { get; set; }
        public List<fullItemsReceivedDetail> fullItemsReceivedDetails { get; set; }

    }
    public class fullItemsReceivedDetail1
    {
        public int ItemId { get; set; }
        public string DrugName { get; set; }
        public string GenericName { get; set; }
        public string UOM { get; set; }
        public string SMID { get; set; }
        public string STID { get; set; }
        public bool? IsSerial { get; set; }
        public string BatchSerial { get; set; }
        public string ItemBatchID { get; set; }
        public DateTime? BatchExpiry { get; set; }
        public decimal SentQuantity { get; set; }
        public decimal DamageQty { get; set; }
        public decimal ReceivedQty { get; set; }
        public decimal LossInTransit { get; set; }
        public decimal OtherQty { get; set; }
    }
    public class InterDepartmentStockDetails1
    {
        public string RandomUniqueId { get; set; }
        public string RefNo { get; set; }
        public DateTime? RefDate { get; set; }
        public ICollection<StockTransferItemDetail> ItemDetails { get; set; }

        public List<fullItemsReceivedDetail1> fullItemsReceivedDetails { get; set; }
    }

    public class PrintInterBranchReceipts
    {
        public string ReceiptNo { get; set; }
        public DateTime? ReceiptDate { get; set; }
        public string IssueNo { get; set; }
        public DateTime? IssueDate { get; set; }
        public List<PrintReceiptItemDetail> PrintReceiptItemDetails { get; set; }
    }

    public class PrintReceiptItemDetail
    {
        public string Brand { get; set; }
        public string GenericName { get; set; }
        public string UOM { get; set; }
        public decimal QtySent { get; set; }
        public decimal QtyRecd { get; set; }

    }

    public class SubmitReceiptDetails
    {
        public string RandomUniqueId { get; set; }
        public int storeId { get; set; }
        public int cmpid { get; set; }
        public int CreatedBy { get; set; }
        public int TransactionID { get; set; }
        public int DamageTcID { get; set; }
        public int LossInTransitTcID { get; set; }
        public int OthersTcID { get; set; }
        public List<fullItemsReceivedDetail1> fullItemsReceivedDetails1 { get; set; }

        public string TcRunningNo { get; set; }
        public string DamageRunningNo { get; set; }
        public string LossInTransitRunningNo { get; set; }
        public string OthersRunningNo { get; set; }

        public string Receiptdate { get; set; }
        public string Fyear { get; set; }


    }

}
