using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class Meterialview
    {
        public ICollection<Meterials> Meterials { get; set; }
        public ICollection<Meterials2> Meterials2 { get; set; }
        public ICollection<Meterials4> Meterials4 { get; set; }
        public ICollection<Meterials5> Meterials5 { get; set; }
        public ICollection<PushArray> PushArray { get; set; }
        public ICollection<BatchNumbers> BatchNumbers { get; set; }
        public StockmasterModel StockmasterModel { get; set; }
        public StockTranModel StockTranModel { get; set; }
        public ItemBatchModel ItemBatchModel { get; set; }
        public ItemBatchTranModel ItemBatchTranModel { get; set; }
        public ICollection<OpticalMaterial> OpticalMaterial { get; set; }
        public ICollection<Opticamreturndetails> Opticamreturndetails { get; set; }
        public ICollection<Opticamreturnsubmitdetails> Opticamreturnsubmitdetails { get; set; }
        public ICollection<OpticalMaterialHistoryDetails> OpticalMaterialHistoryDetails { get; set; }
        public string storename { get; set; }
        public bool Yearstatus { get; set; }
        public string storekeeper { get; set; }
        public string storelocation { get; set; }
        public string storeAddress { get; set; }
        public string vendorname { get; set; }
        public string vendoradd { get; set; }
        public string vendoradd2 { get; set; }
        public string vendorlocation { get; set; }
        public string vendorcity { get; set; }
        public string vendorstate { get; set; }
        public int vendorID { get; set; }
        public int Storeid { get; set; }
        public string vendorcountry { get; set; }
        public string vendorgst { get; set; }
        public string opticalGrnnumber { get; set; }
        public DateTime opticalGrndate { get; set; }
        public int CompanyID { get; set; }
        public string NewDocnumber { get; set; }
        public int TransactiomnID { get; set; }
        public int UserID { get; set; }
        public string ponumber { get; set; }
        public string podate { get; set; }
        public string Remarks { get; set; }
        public string GRN { get; set; }
        public int vendorid { get; set; }
        public int storeiud { get; set; }
        public DateTime Issuedate { get; set; }
    }

    public class OpticalMaterialHistoryDetails
    {
        public string Docnumber { get; set; }
        public decimal ItemQty { get; set; }
        public DateTime? DocDate { get; set; }
        public decimal? ItemValue { get; set; }
        public string Vendor { get; set; }
        public string Brand { get; set; }
        public string Brandtype { get; set; }
        public string Store { get; set; }
    }
    public class Opticamreturnsubmitdetails
    {
        public string Item { get; set; }
        public string type { get; set; }
        public string model { get; set; }
        public string color { get; set; }
        public string itemqty { get; set; }
        public string ClosingQty { get; set; }
        public string itemrate { get; set; }
        public string itemvalue { get; set; }
        public int Returnqty { get; set; }
        public int ItemID { get; set; }
        public int? UOM { get; set; }
        public string STID { get; set; }
    }


    public class Opticamreturndetails
    {
        public int ItemID { get; set; }
        public long STID { get; set; }
        public int? UOM { get; set; }
        public int Returnqty { get; set; }
        public string Item { get; set; }
        public string Brand { get; set; }
        public string type { get; set; }
        public string model { get; set; }
        public string color { get; set; }
        public decimal? itemqty { get; set; }
        public decimal? ClosingQty { get; set; }
        public decimal? itemrate { get; set; }
        public decimal? itemvalue { get; set; }
        public string storename { get; set; }
        public string storekeeper { get; set; }
        public string storelocation { get; set; }
        public string storeAddress { get; set; }
        public string vendorname { get; set; }
        public string vendoradd { get; set; }
        public string vendoradd2 { get; set; }
        public string vendorlocation { get; set; }
        public string vendorcity { get; set; }
        public string vendorstate { get; set; }
        public string vendorcountry { get; set; }
        public string vendorgst { get; set; }
        public string ponumber { get; set; }
        public string podate { get; set; }

    }

    public class OpticalMaterial
    {

        public string RnNumber { get; set; }
        public decimal Tqty { get; set; }
        public DateTime? RnDate { get; set; }
        public string Vendor { get; set; }
    }




    public class BatchNumbers
    {

        public string itemvalue { get; set; }
        public string Drug { get; set; }
    }

    public class Meterials
    {
        public string ReciptNumber { get; set; }
        public string StoreName { get; set; }
        public string VendorName { get; set; }
        public string Brand { get; set; }
        public int StoreID { get; set; }
        public long? PoID { get; set; }
        public int UOMID { get; set; }


    }

    public class Meterials2
    {
        public string ReciptNo { get; set; }
        public DateTime RecipDate { get; set; }
        public string VendorN { get; set; }
        public string Adress1 { get; set; }
        public string Adress2 { get; set; }
        public int? Location { get; set; }
        public string GSTNO { get; set; }
        public string MobileNo { get; set; }
        public int VendorId { get; set; }
        public bool IsActive { get; set; }

    }

    public class Meterials4
    {

        public string DrugName1 { get; set; }
        public decimal RecivedQty1 { get; set; }
        public decimal ConsumedQty1 { get; set; }
        public decimal? ConsumedQty2 { get; set; }
        public decimal BalanceQty1 { get; set; }
        public decimal QtyIssued { get; set; }
        public decimal? BalanceQty2 { get; set; }
        public string BatchList1 { get; set; }
        public DateTime Expired { get; set; }
        public decimal Rate { get; set; }
        public decimal? GrossproductValue { get; set; }
        public decimal? TotalDiscountValue { get; set; }
        public decimal? TotalTaxValue { get; set; }
        public decimal? TotalSGSTTaxValue { get; set; }
        public decimal? TotalCGSTTaxValue { get; set; }
        public decimal? TotalPOValue { get; set; }
        public decimal? CGSTPercentage { get; set; }
        public decimal? SGSTPercentage { get; set; }
        public decimal? IGSTPercentage { get; set; }
        public decimal? GSTPercentage { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public string UOM { get; set; }
        public string GenericName { get; set; }
        public Int64 ItemBatchID { get; set; }
        public decimal? GSTTaxValue { get; set; }
        public decimal? CGSTTaxValue { get; set; }
        public decimal? SGSTTaxValue { get; set; }
        public decimal? IGSTTaxValue { get; set; }
        public decimal? DescountAmopunt { get; set; }
    }

    public class Meterials5
    {
        public string DrugName1 { get; set; }
        public decimal RecivedQty1 { get; set; }
        public decimal? ConsumedQty1 { get; set; }
        public decimal? ConsumedQty2 { get; set; }
        public decimal? BalanceQty1 { get; set; }
        public decimal? BalanceQty2 { get; set; }
        public decimal QtyIssued { get; set; }
        public string BatchList1 { get; set; }
        public DateTime Expired { get; set; }
        public decimal Rate { get; set; }
        public decimal? GrossproductValue { get; set; }
        public decimal? TotalDiscountValue { get; set; }
        public decimal? TotalTaxValue { get; set; }
        public decimal? TotalSGSTTaxValue { get; set; }
        public decimal? TotalCGSTTaxValue { get; set; }
        public decimal? TotalPOValue { get; set; }
        public decimal? CGSTPercentage { get; set; }
        public decimal? SGSTPercentage { get; set; }
        public decimal? IGSTPercentage { get; set; }
        public decimal? GSTPercentage { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public string UOM { get; set; }
        public string GenericName { get; set; }
        public decimal? TotalIGSTTaxValue { get; set; }
        public int Locked { get; set; }
        public decimal? GSTTaxValue { get; set; }
        public decimal? CGSTTaxValue { get; set; }
        public decimal? SGSTTaxValue { get; set; }
        public decimal? IGSTTaxValue { get; set; }
        public decimal? DescountAmopunt { get; set; }
    }

    public class PushArray
    {
        public string DrugName1 { get; set; }
        public decimal RecivedQty1 { get; set; }
        public decimal ConsumedQty1 { get; set; }
        public decimal ConsumedQty2 { get; set; }
        public decimal BalanceQty1 { get; set; }
        public decimal? BalanceQty2 { get; set; }
        public decimal QtyIssued { get; set; }
        public string BatchList1 { get; set; }
        public DateTime Expired { get; set; }
        public decimal Rate { get; set; }
        public decimal? GrossproductValue { get; set; }
        public decimal? TotalDiscountValue { get; set; }
        public decimal? TotalTaxValue { get; set; }
        public decimal? TotalSGSTTaxValue { get; set; }
        public decimal? TotalCGSTTaxValue { get; set; }
        public decimal? TotalPOValue { get; set; }
        public decimal? CGSTPercentage { get; set; }
        public decimal? SGSTPercentage { get; set; }
        public decimal? IGSTPercentage { get; set; }
        public decimal? GSTPercentage { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public string UOM { get; set; }
        public string GenericName { get; set; }
        public decimal? TotalIGSTTaxValue { get; set; }
        public int Locked { get; set; }
        public decimal? GSTTaxValue { get; set; }
        public decimal? CGSTTaxValue { get; set; }
        public decimal? SGSTTaxValue { get; set; }
        public decimal? IGSTTaxValue { get; set; }
        public decimal? DescountAmopunt { get; set; }
    }

}
