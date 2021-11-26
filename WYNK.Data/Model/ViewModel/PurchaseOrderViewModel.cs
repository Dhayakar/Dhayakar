using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data;


namespace WYNK.Data.Model.ViewModel
{
    public class purchaseorderview
    {
        public Drug_Master DrugMaster { get; set; }
        public Vendor_Masters VendorMasters { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }
        public ICollection<purchasedetails> purchasedetails { get; set; }
        public ICollection<Vendername> Vendername { get; set; }
        public ICollection<purchaseorderdetails> purchaseorderdetails { get; set; }
        public ICollection<purchaseorderhist5> purchaseorderhist5 { get; set; }
        public ICollection<purchaseorderhist> purchaseorderhist { get; set; }
        public ICollection<purchaseorderhist1> purchaseorderhist1 { get; set; }
        public ICollection<purchaseorderhist2> purchaseorderhist2 { get; set; }
        public ICollection<purchaseorderhist3> purchaseorderhist3 { get; set; }
        public ICollection<purchaseorderhist4> purchaseorderhist4 { get; set; }
        public ICollection<purchasedetailsDelete> purchasedetailsDelete { get; set; }
        public string LocationName { get; set; }
        public string DeliveryParentDescriptioncity { get; set; }
        public string DeliveryParentDescriptionstate { get; set; }
        public string DeliveryParentDescriptioncountry { get; set; }
        public string Location { get; set; }
        public string ParentDescriptioncity { get; set; }
        public string ParentDescriptionstate { get; set; }
        public string ParentDescriptioncountry { get; set; }
        public string ParentDescriptionstatee { get; set; }
        public string ParentDescriptioncountryy { get; set; }

    }

    public class purchaseorderhist5
    {
        public string RandomUniqueID { get; set; }
        public string PONumber { get; set; }
        public DateTime PODate { get; set; }
        public string QuotationNumber { get; set; }
        public DateTime? QuotationDate { get; set; }
        public string VendorName { get; set; }
        public string DeliveryName { get; set; }
        public string Status { get; set; }
        public int VendorID { get; set; }
        public int? DeliverylocationID { get; set; }
    }
    public class purchasedetailsDelete
    {

        public Int64 ID { get; set; }
        public bool IsDeleted { get; set; }

    }
    public class purchasedetails
    {

        public string ItemDrug { get; set; }
        public string ItemID { get; set; }
        public string UOMname { get; set; }
        public string UOMID { get; set; }
        public decimal ItemRate { get; set; }
        public decimal ItemQty { get; set; }
        public decimal ItemValue { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal GSTPercentage { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal GSTTaxValue { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal IGST { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal CESSPercentage { get; set; }
        public decimal CESSAmount { get; set; }
        public decimal AdditionalCESSPercentage { get; set; }
        public decimal AddCESSAmount { get; set; }
        public Int64 POTranID { get; set; }
        public string RandomUniqueID { get; set; }

    }
    public class purchaseorderhist
    {
        public string RandomUniqueID { get; set; }
        public string PONumber { get; set; }
        public DateTime PODate { get; set; }
        public string QuotationNumber { get; set; }
        public DateTime? QuotationDate { get; set; }
        public string VendorName { get; set; }
        public string DeliveryName { get; set; }
        public string Status { get; set; }
        public int VendorID { get; set; }
        public int? DeliverylocationID { get; set; }

    }

    public class purchaseorderhist1
    {
        public string RandomUniqueID { get; set; }
        public string PONumber { get; set; }
        public DateTime PODate { get; set; }
        public string QuotationNumber { get; set; }
        public DateTime? QuotationDate { get; set; }
        public string VendorName { get; set; }
        public string DeliveryName { get; set; }
        public string Status { get; set; }
        public int VendorID { get; set; }
        public int? DeliverylocationID { get; set; }
    }
    public class purchaseorderhist2
    {
        public string RandomUniqueID { get; set; }
        public string PONumber { get; set; }
        public DateTime PODate { get; set; }
        public string QuotationNumber { get; set; }
        public DateTime? QuotationDate { get; set; }
        public string VendorName { get; set; }
        public string DeliveryName { get; set; }
        public string Status { get; set; }
        public int VendorID { get; set; }
        public int? DeliverylocationID { get; set; }
    }
    public class purchaseorderhist3
    {
        public string RandomUniqueID { get; set; }
        public string PONumber { get; set; }
        public DateTime PODate { get; set; }
        public string QuotationNumber { get; set; }
        public DateTime? QuotationDate { get; set; }
        public string VendorName { get; set; }
        public string DeliveryName { get; set; }
        public string Status { get; set; }
        public int VendorID { get; set; }
        public int? DeliverylocationID { get; set; }

    }
    public class purchaseorderhist4
    {
        public string RandomUniqueID { get; set; }
        public string PONumber { get; set; }
        public DateTime PODate { get; set; }
        public string QuotationNumber { get; set; }
        public DateTime? QuotationDate { get; set; }
        public string VendorName { get; set; }
        public string DeliveryName { get; set; }
        public string Status { get; set; }
        public int VendorID { get; set; }
        public int? DeliverylocationID { get; set; }
    }
    public class Vendername
    {
        public int ID { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Location { get; set; }
        public string City { get; set; }
        public string PhoneNo { get; set; }
        public string GSTNo { get; set; }
        public string DLNo { get; set; }
        public DateTime? DLNoDate { get; set; }

    }
    public class purchaseorderdetails
    {
        public string Drugname { get; set; }
        public string PurchaseUOM { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal? GST { get; set; }
        public string DrugID { get; set; }
        public int purchaseid { get; set; }
        public decimal? CESS { get; set; }
        public decimal? AdditionalCESS { get; set; }
        public string TaxDescription { get; set; }
        public string CessDescription { get; set; }
        public string AddcessDescription { get; set; }
        public decimal ItemQty { get; set; }
        public decimal ItemValue { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal GSTTaxValue { get; set; }
        public decimal CESSAmount { get; set; }
        public decimal AddCESSAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal GrossAmount { get; set; }

    }




}







