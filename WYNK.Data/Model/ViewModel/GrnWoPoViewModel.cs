using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class GrnWoPo
    {
        public StockMaster StockMaster { get; set; }
        public ICollection<itemorderdetails> itemorderdetails { get; set; }
        public ICollection<BatchhDetails> BatchhDetails { get; set; }
        public ICollection<Itemdetails> Itemdetails { get; set; }
        public ICollection<SerialArraywithpo> SerialArraywithpo { get; set; }
        public string Location { get; set; }
        public string ParentDescriptioncity { get; set; }
        public string ParentDescriptionstate { get; set; }
        public string ParentDescriptioncountry { get; set; }
    }

    public class itemorderdetails
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
        public string Type { get; set; }
        public string GenericName { get; set; }
        public int? Tracking { get; set; }
        public decimal ItemQty { get; set; }
        public decimal ItemValue { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal GSTTaxValue { get; set; }
        public decimal CESSAmount { get; set; }
        public decimal AddCESSAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal GrossAmount { get; set; }


    }
    public class BatchhDetails
    {
        public string DrugName { get; set; }
        public int druggid { get; set; }
        public int BatchQuantity { get; set; }
        public int uomm { get; set; }
        public string BatchNo { get; set; }
        public string ExpiryDate { get; set; }

    }
    public class SerialArraywithpo
    {

        public string DrugName { get; set; }
        public int druggid { get; set; }
        public string SerialNumber { get; set; }
        public string GRNNumber { get; set; }
        public int TC { get; set; }
        public int Store { get; set; }
        public string ExpiryDate { get; set; }
        public int CreatedBy { get; set; }

    }
    public class Itemdetails
    {

        public string ItemDrug { get; set; }
        public string ItemID { get; set; }
        public string UOMname { get; set; }
        public string UOMID { get; set; }
        public int ItemRate { get; set; }
        public decimal ItemQty { get; set; }
        public decimal ItemValue { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal GSTPercentage { get; set; }
        public decimal GSTTaxValue { get; set; }
        public decimal CESSPercentage { get; set; }
        public decimal CESSAmount { get; set; }
        public decimal AdditionalCESSPercentage { get; set; }
        public decimal AddCESSAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public string TaxDescription { get; set; }
        public string CESSDescription { get; set; }
        public string AdditionalCESSDescription { get; set; }
        public string RandomUniqueID { get; set; }
        public string Type { get; set; }
        public string GenericName { get; set; }
        public int Tracking { get; set; }

    }
}



