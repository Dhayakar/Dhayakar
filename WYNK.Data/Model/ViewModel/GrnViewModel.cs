using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class Grn
    {

        public PurchaseOrder PurchaseOrder { get; set; }
        public ICollection<PurchaseOrderTrans> PurchaseOrderTrans { get; set; }
        public Vendor_Masters VendorMaster { get; set; }
        public OneLine_Masters OneLineMaster { get; set; }
        public UomMaster UomMaster { get; set; }
        public StockMaster StockMaster { get; set; }
        public ICollection<StockTran> StockTran { get; set; }
        public ICollection<ItemBatch> ItemBatch { get; set; }
        public ICollection<PurchaseDetails> PurchaseDetails { get; set; }
        public ICollection<ItemDetails> ItemDetails { get; set; }
        public ICollection<BatchDetails> BatchDetails { get; set; }
       
       // public ICollection<RcdDet> RcdDet { get; set; }
       public ICollection<GrnDetails> GrnDetails { get; set; }
        public ICollection<GrnitemDetails> GrnitemDetails { get; set; }
        public ICollection<SerialArray> SerialArray { get; set; }
        public ICollection<STableArray> STableArray { get; set; }
        public ICollection<ItembalanceArry> ItembalanceArry { get; set; }
        public ICollection<GetGrnSerialDetails> GetGrnSerialDetails { get; set; }
        public decimal? Totalvalue { get; set; }
        public decimal? TotalDiscountValue { get; set; }
        public decimal? TotalTotalCost { get; set; }
        public numbercon numbercon { get; set; }
        public string Companyname { get; set; }
    }


}


public class numbercon
{
    public string DocNo { get; set; }

}
public class GetGrnSerialDetails
{
    public string Drugname { get; set; }
    public string SerialNo { get; set; }
    public DateTime? ExpiryDate { get; set; }
}

public class ItembalanceArry
    {
    public int ActualQty { get; set; }
    public string DrugName { get; set; }
    public string UOM { get; set; }
    public int DrugID { get; set; }
    public int UOMID { get; set; }
    }

public class SerialArray
{
    public string DrugName { get; set; }
    public int SerialNumber { get; set; }
    public string GRNNumber { get; set; }
    public int TC { get; set; }
    public string UOM { get; set; }
    public int Store { get; set; }
    public int CreatedBy { get; set; }
    public DateTime? ExpiryDate1 { get; set; }
    public int ID { get; set; }
}

public class STableArray
{
    public string DrugName { get; set; }
    public int SerialNumber { get; set; }
    public string GRNNumber { get; set; }
    public int TC { get; set; }
    public string UOM { get; set; }
    public int Store { get; set; }
    public int CreatedBy { get; set; }
    public DateTime? ExpiryDate1 { get; set; }
    public int ID { get; set; }

}

public class PurchaseDetails

{
    public Int64 POId { get; set; }
    public string PONumber { get; set; }
    public DateTime PODate { get; set; }
    public string QuotationNumber { get; set; }
    public DateTime? QuotationDate { get; set; }
    public int VendorId { get; set; }
    public string SupplierName { get; set; }
    public string Address1 { get; set; }
    public string Address2 { get; set; }
    public string Location { get; set; }
    public string Phone { get; set; }
    public string GST { get; set; }
    public string SCityID { get; set; }
    public string scity { get; set; }
    
    public string DeliveryName { get; set; }
    public string DeliveryAddress1 { get; set; }
    public string DeliveryAddress2 { get; set; }
    public string DeliveryAddress3 { get; set; }
    public string RandomUniqueID { get; set; }
    public string DeliveryLocation { get; set; }


}

public class ItemDetails

{
    public string DrugName { get; set; }
    public int DrgId { get; set; }
    public string poid { get; set; }
    public int uom { get; set; }
    public string BaseUOM { get; set; }
    public string PurchaseUOM { get; set; }
    public Decimal Quantity { get; set; }
    public Decimal ReceivedQuantity { get; set; }
    public Decimal ActualQuantity { get; set; }
    public Decimal Rate { get; set; }
    public Decimal Value { get; set; }
    public Decimal GST { get; set; }
    public Decimal GSTValue { get; set; }
    public Decimal Discount { get; set; }
    public Decimal DiscountValue { get; set; }
    public Decimal GrossAmount { get; set; }
    public Decimal TotalCost { get; set; }
    public string Type { get; set; }
    public string DrugTracker { get; set; }
    public string typess { get; set; }


    public Decimal? CESS { get; set; }
    public Decimal? CESSAmount { get; set; }
    public Decimal? AdditionalCESS { get; set; }
    public Decimal? AdditionalCESSAmount { get; set; }

    public string TaxDescription { get; set; }
    public string CESSDescription { get; set; }
    public string AdditionalCESSDescription { get; set; }

    public string GenericName { get; set; }

    public int ClosingBalance { get; set; }
    
}

public class BatchDetails

{
    public string DrugName { get; set; }
    public Int64 Std { get; set; }
    public int BatchQuantity { get; set; }
    public int druggid { get; set; }
    public int stid { get; set; }
    public int uomm { get; set; }
    public string BatchNo { get; set; }
    public DateTime ExpiryDate { get; set; }

}

//public class RcdDet

//{
//    public Decimal quantity { get; set; }

//}


public class GrnDetails
{
    public Int64 smid { get; set; }
    public string GrNo { get; set; }
    public DateTime GrDt { get; set; }
    public string poid { get; set; }
    public DateTime podt { get; set; }
    public string qnum { get; set; }
    public DateTime? qudt { get; set; }
    public string store { get; set; }
    public string vendor { get; set; }
    public Decimal? GpValue { get; set; }
    public Decimal? TdValue { get; set; }
    public Decimal? TtValue { get; set; }
    public Decimal? TcValue { get; set; }
    public Decimal? TsValue { get; set; }
    public Decimal? TpValue { get; set; }
    public string Address11 { get; set; }
    public string Address22 { get; set; }
    public string Locationn { get; set; }
    public string Phonee { get; set; }
    public string GSTt { get; set; }
    public string DeliveryNamee { get; set; }
    public string DeliveryAddress11 { get; set; }
    public string DeliveryAddress22 { get; set; }
    public string DeliveryAddress33 { get; set; }
    public string DeliveryLocationn { get; set; }
    public string Terms { get; set; }
    public string SCityID1 { get; set; }
    public string Scity1 { get; set; }
    public string Deliverycity { get; set; }
    public string DeliveryCityID { get; set; }

}

public class GrnitemDetails

{

    public string SMID { get; set; }
    public string DrugName { get; set; }
    public Decimal? DrgQuan { get; set; }
    public string Uom { get; set; }
    public Decimal? rate { get; set; }
    public Decimal? value { get; set; }
    public Decimal? Discount { get; set; }
    public Decimal? DiscountValue { get; set; }
    public Decimal? GST { get; set; }
    public Decimal? GSTValue { get; set; }
    public Decimal? TotalCost { get; set; }

}