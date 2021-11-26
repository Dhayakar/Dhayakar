using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{ 
    public class OpticalBillRegisterViewModel
    {

        public OpticalInvoiceMaster OpticalInvoiceMaster { get; set; }
        public OpticalInvoiceTran OpticalInvoiceTran { get; set; }
        public ICollection<getOpticalBillRegDet> getOpticalBillRegisterDetail { get; set; }
        public ICollection<getOpticalBillRegtaxsummary> getOpticalBillRegTaxsummary { get; set; }
        public decimal? Totalsum { get; set; }
        public decimal? TTAXAMOUNT { get; set; }
        
        public decimal? TotalsumAmount { get; set; }
        public decimal? TotalsumDiscountAmount { get; set; }
        public decimal? TotalsumGrossAmount { get; set; }
        public decimal? TotalsumNetAmount { get; set; }
        public decimal? TAXTotalProductValue { get; set; }
        public decimal? TAXCESSAmount { get; set; }
        public decimal? TAXAddCESSAmount { get; set; }
        public decimal? TAXTaxPayable { get; set; }
        public decimal? TAXGSTTaxValue { get; set; }
        public decimal? TAXTaxableTurnover { get; set; }
    }

    public class getOpticalBillRegtaxsummary 
    {
        
        public string TaxDescription { get; set; }
        public string CESSDescription { get; set; }
        public string AdditionalCESSDescription { get; set; }
        public int? TaxID { get; set; }
        public decimal? TotalProductValue { get; set; }
        public decimal? GSTTaxValue { get; set; }
        public decimal? CESSAmount { get; set; }
        public decimal? AddCESSAmount { get; set; }
        public decimal? TaxPayable { get; set; }
        public decimal? TaxableTurnover { get; set; }
        
    }

    public class getOpticalBillRegDet
    {
   
        public string BillNo        { get; set; }
        public DateTime? BillDate      { get; set; }
        public string PatientName   { get; set; }
        public string Brand         { get; set; }
        public string Description   { get; set; }
        public string UOM           { get; set; }
        public decimal Quantity      { get; set; }
        public decimal Amount        { get; set; }
        public decimal DiscountPerc  { get; set; }
        public decimal DiscountAmount{ get; set; }
        public string TaxDescription    { get; set; }
        public string CessDescription   { get; set; }
        public string AddCessDescription { get; set; }
        public decimal? TaxPerc  { get; set; }
        public decimal? CessPerc  { get; set; }
        public decimal? AddCessPerc  { get; set; }
        public decimal? TaxValue  { get; set; }
        public decimal? CessValue   { get; set; }
        public decimal? AddCessValue { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal? NetAmount { get; set; }
        public string CompanyName { get; set; }
        public string CAddress1 { get; set; }     
    }

}
