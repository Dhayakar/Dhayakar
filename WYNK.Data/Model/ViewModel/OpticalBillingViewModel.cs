using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class OpticalBilling
    {
        public ICollection<GetOpticaldetails> GetOpticaldetails { get; set; }
        public ICollection<GetOpticaldetailsfull> GetOpticaldetailsfull { get; set; }
        public ICollection<GetOpticaldetailsfullcheck> GetOpticaldetailsfullcheck { get; set; }
        public ICollection<Availablestock> Availablestock { get; set; }
        public ICollection<GetOpticaldetailsfullbefore> GetOpticaldetailsfullbefore { get; set; }
        public OpticalInvoiceMaster OpticalInvoiceMaster { get; set; }
        public OpticalInvoiceTran OpticalInvoiceTran { get; set; }
        public OpticalStockMaster OpticalStockMaster { get; set; }
        public OpticalStockTran OpticalStockTran { get; set; }
        public ICollection<Payment_Master> PaymentMaster { get; set; }
        public decimal? advance { get; set; }
        public string UIN { get; set; }
        public string CustomerName { get; set; }

        public string GSTIN { get; set; }
        public string Address { get; set; }
        public string Mobileno { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string companyAddress { get; set; }
        public string phone { get; set; }
        public string web { get; set; }
        public string GstNo { get; set; }
        public string Compnayname { get; set; }
        public string companyAddress1 { get; set; }
        public string companyAddress2 { get; set; }
        public decimal TotalAmountprinting { get; set; }
        public ICollection<opticalbillingtran> opticalbillingtran { get; set; }
        public ICollection<printpaymnetbilling> printpaymnetbilling { get; set; }
        public string ReceiptRunningNo { get; set; }
        public int billtranid { get; set; }

    }

    public class printpaymnetbilling
    {

        public string PaymentMode { get; set; }
        public string InstrumentNumber { get; set; }
        public DateTime? Instrumentdate { get; set; }
        public DateTime? Expirydate { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public decimal Amount { get; set; }
        public string Receiptnumber { get; set; }
        public DateTime? Receiptdate { get; set; }

    }
    public class opticalbillingtran
    {
        public string LTname { get; set; }
        public string UOMname { get; set; }
        public string Type { get; set; }
        public string Brand { get; set; }
        public string Index { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public decimal OrderedQty { get; set; }
        public decimal? Price { get; set; }
        public decimal? Value { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? GSTPercentage { get; set; }
        public decimal? GSTTaxValue { get; set; }
        public decimal? CESSPercentage { get; set; }
        public decimal? CESSAmount { get; set; }
        public decimal? GrossValue { get; set; }
        public decimal? Totalamount { get; set; }
        public decimal? AdditionalCESSPercentage { get; set; }
        public decimal? AddCESSPerAmt { get; set; }
        public string TaxDescription { get; set; }
        public string CessDescription { get; set; }
        public string AddcessDescription { get; set; }




    }


    public class GetOpticaldetails
    {
        public string RandomUniqueID { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string UIN { get; set; }
        public string CustomerName { get; set; }
        public string CustomermiddleName { get; set; }
        public string CustomerlastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Location { get; set; }
        public string city { get; set; }
        public string State { get; set; }
        public string country { get; set; }
        public string Mobileno { get; set; }
        public int cuid { get; set; }



    }
    public class GetOpticaldetailsfull
    {
        public string COID { get; set; }
        public int COTID { get; set; }
        public int LTID { get; set; }
        public string LTname { get; set; }
        public int UOMID { get; set; }
        public string UOMname { get; set; }
        public string Type { get; set; }
        public string Brand { get; set; }
        public string Index { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public decimal OrderedQty { get; set; }
        public decimal? Price { get; set; }
        public decimal? Value { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? GSTPercentage { get; set; }
        public decimal? GSTTaxValue { get; set; }
        public decimal? CESSPercentage { get; set; }
        public decimal? CESSAmount { get; set; }
        public decimal? GrossValue { get; set; }
        public decimal? Totalamount { get; set; }
        public decimal? AdditionalCESSPercentage { get; set; }
        public decimal? AddCESSPerAmt { get; set; }
        public string TaxDescription { get; set; }
        public string CessDescription { get; set; }
        public string AddcessDescription { get; set; }
        public int? TaxID { get; set; }

    }
    public class GetOpticaldetailsfullcheck
    {
        public string COID { get; set; }
        public int COTID { get; set; }
        public int LTID { get; set; }
        public string LTname { get; set; }
        public int UOMID { get; set; }
        public string UOMname { get; set; }
        public string Type { get; set; }
        public string Brand { get; set; }
        public string Index { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public decimal OrderedQty { get; set; }
        public decimal? Price { get; set; }
        public decimal? Value { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? GSTPercentage { get; set; }
        public decimal? GSTTaxValue { get; set; }
        public decimal? CESSPercentage { get; set; }
        public decimal? CESSAmount { get; set; }
        public decimal? GrossValue { get; set; }
        public decimal? Totalamount { get; set; }
        public decimal? AdditionalCESSPercentage { get; set; }
        public decimal? AddCESSPerAmt { get; set; }
        public string TaxDescription { get; set; }
        public string CessDescription { get; set; }
        public string AddcessDescription { get; set; }
        public string Stockcheck { get; set; }
        public int? TaxID { get; set; }

    }
    public class Availablestock
    {
        public int Closingbalance { get; set; }
        public string Storename { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Brand { get; set; }
        public string uom { get; set; }
    }
    public class GetOpticaldetailsfullbefore
    {
        public string COID { get; set; }
        public int COTID { get; set; }
        public int LTID { get; set; }
        public string LTname { get; set; }
        public int UOMID { get; set; }
        public string UOMname { get; set; }
        public string Type { get; set; }
        public string Brand { get; set; }
        public string Index { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public decimal OrderedQty { get; set; }
        public decimal? Price { get; set; }
        public decimal? Value { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? GSTPercentage { get; set; }
        public decimal? GSTTaxValue { get; set; }
        public decimal? CESSPercentage { get; set; }
        public decimal? CESSAmount { get; set; }
        public decimal? GrossValue { get; set; }
        public decimal? Totalamount { get; set; }
        public decimal? AdditionalCESSPercentage { get; set; }
        public decimal? AddCESSPerAmt { get; set; }
        public string TaxDescription { get; set; }
        public string CessDescription { get; set; }
        public string AddcessDescription { get; set; }
        public string Stockcheck { get; set; }

    }

}


