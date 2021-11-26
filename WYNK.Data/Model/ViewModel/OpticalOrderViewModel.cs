
using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data;


namespace WYNK.Data.Model.ViewModel
{

    public class OpticalOrderView
    {
    
        public OpticalOrder OpticalOrder { get; set; }
        public OpticalOrderTran OpticalOrderTran { get; set; }
        public ICollection<VendorDetails> VendorDetails { get; set; }
        public ICollection<VendorDetails1> VendorDetails1 { get; set; }
        public string ParentDescriptioncity { get; set; }
        public string ParentDescriptionstate { get; set; }
        public string ParentDescriptioncountry { get; set; }
        public string Companyname { get; set; }
        public ICollection<paymentReMode1> paymentReMode1 { get; set; }
        public decimal TOpayMode1 { get; set; }

        public decimal? Totalsum { get; set; }
        public ICollection<Payment_Master> paymenttran { get; set; }
        public ICollection<OpticalOrderdetails> OpticalOrderdetails { get; set; }
        public ICollection<OPticalDetails> OPticalDetails { get; set; }
        public string ReceiptRunningNo { get; set; }
        public ICollection<OpticalOrderUpdate> OpticalOrderUpdate { get; set; }
        public ICollection<OpticalbindingDet> OpticalbindingDet { get; set; }

        public ICollection<OpticalbindingDetStatus> OpticalbindingDetStatus { get; set; }


    }



    



    public class paymentReMode1
    {
        public string PaymentMode { get; set; }
        public string InstrumentNumber { get; set; }
        public DateTime? Instrumentdate { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public DateTime? Expirydate { get; set; }
        public decimal Amount { get; set; }
        public string InVoiceNumber { get; set; }
        public DateTime? InVoiceDate { get; set; }
    }
    public class OpticalbindingDetStatus
    {
        public decimal Quantity { get; set; }
    }
    public class OpticalbindingDet
    {
        public string LensName { get; set; }

        public string Index { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public string Type { get; set; }
        public string Brand { get; set; }






        public int LMID { get; set; }
        public string UOMDescription { get; set; }
        public int ID { get; set; }
        public decimal Prize { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }
        public decimal? Discount { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? GST { get; set; }
        public decimal? GrossAmount { get; set; }
        public decimal? GSTAmount { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? IGST { get; set; }
        public decimal? IGSTAmount { get; set; }
        public decimal? CESS { get; set; }
        public decimal? CESSAmount { get; set; }
        public decimal? AdditionalCESS { get; set; }
        public decimal? AdditionalCESSAmount { get; set; }


        public string CompanyName { get; set; }
        public string CAddress1 { get; set; }
        public string CAddress2 { get; set; }
        public string CAddress3 { get; set; }
        public string CPhone1 { get; set; }
        public string CWebsite { get; set; }

        public string TaxDescription { get; set; }
        public string CESSDescription { get; set; }
        public string AdditionalCESSDescription { get; set; }

    }
    public class OpticalOrderUpdate
    {
        public string ID { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string RefNo { get; set; }
        public DateTime? RefDate { get; set; }
        public string VendorName { get; set; }
        public string PhoneNo { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string GSTNo { get; set; }

        public int VendorID { get; set; }
        public string DeliveryName { get; set; }
        public DateTime? DeliveryDate { get; set; }

        public string DeliveryAddress1 { get; set; }
        public string DeliveryAddress2 { get; set; }
        public string DeliveryAddress3 { get; set; }
        public Int16 Validity { get; set; }
        public string TermsAndConditions { get; set; }
        public string DeliveryLocationName { get; set; }
        public string Dcity { get; set; }
        public string Status { get; set; }

    }

    public class VendorDetails
    {
        public int ID { get; set; }
        public string VendorName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Location { get; set; }
        public string City { get; set; }
        public string PhoneNo { get; set; }
        public string GSTNo { get; set; }

        public string CompanyName { get; set; }
        public string CAddress1 { get; set; }
        public string CAddress2 { get; set; }
        public string CAddress3 { get; set; }
        public string CLocationID { get; set; }
        public string CCity { get; set; }
    }
    public class VendorDetails1
    {
        public int ID { get; set; }
        public string VendorName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Location { get; set; }
        public string City { get; set; }
        public string PhoneNo { get; set; }
        public string GSTNo { get; set; }

    }
    public class OpticalOrderdetails
    {
        public string LensName { get; set; }
        public string UOMDescription { get; set; }
        public decimal Prize { get; set; }
        public Int16? GST { get; set; }
        public string LMID { get; set; }
        public int LTID { get; set; }
        public Int16? IGST { get; set; }
        public decimal? CESS { get; set; }
        public decimal? AdditionalCESS { get; set; }
        public string TaxDescription { get; set; }
        public string CESSDescription { get; set; }
        public string AdditionalCESSDescription { get; set; }


    }

    public class OPticalDetails
    {

        public string LensName { get; set; }
        public string LMID { get; set; }
        public int LTID { get; set; }
        public string UOMDescription { get; set; }
        public int ID { get; set; }
        public int uomid { get; set; }
        public int Prize { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal GST { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal GSTAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal IGST { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal CESS { get; set; }
        public decimal CESSAmount { get; set; }
        public decimal AdditionalCESS { get; set; }
        public decimal AdditionalCESSAmount { get; set; }




    }



}






