using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class TaxSummaryViewM
    { 
   
        public Tax_Summary TaxSummary { get; set; }

        public ICollection<getTaxSummaryDet> getTaxSummaryDet { get; set; }
        public ICollection<getTaxSummaryDet1> getTaxSummaryDet1 { get; set; }
        public decimal? TTaxValue  { get; set; }
        public decimal? TTax1Value { get; set; }
        public decimal? TTax2Value  { get; set; }
        public decimal? TCESSValue  { get; set; }
        public decimal? TAddlCESSValue { get; set; }
        public decimal? TTaxValueR { get; set; }
        public decimal? TTax1ValueR { get; set; }
        public decimal? TTax2ValueR { get; set; }
        public decimal? TCESSValueR { get; set; }
        public decimal? TAddlCESSValueR { get; set; }
        public decimal? TGrossAmount { get; set; }
        public decimal? TTaxPayable { get; set; }
        public decimal? TGrossAmountR { get; set; }
        public decimal? TTaxPayableR { get; set; }

    }


    public class getTaxSummaryDet
    {

        public int? TaxID { get; set; }
        public int CMPID { get; set; }
        public string BillingType { get; set; }
        public DateTime TransactionDate { get; set; }
        public bool TaxGroupID { get; set; }
        public decimal? GrossAmount { get; set; }
        public decimal? TaxPayable { get; set; }
        
        public string TaxDescription { get; set; }
        public string CESSDescription { get; set; }
        public string AddlCESSDescription { get; set; }
        public string CompanyName { get; set; }
        public Int16? TaxPercentage { get; set; }
        public Int16? Tax1Percentage { get; set; }
        public Int16? Tax2Percentage { get; set; }
        public decimal? CESSPercentage { get; set; }
        public decimal? AddlCESSPercentage { get; set; }
        public decimal? TaxValue { get; set; }
        public decimal? Tax1Value { get; set; }
        public decimal? Tax2Value { get; set; }
        public decimal? CESSValue { get; set; }
        public decimal? AddlCESSValue { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
    }

    public class getTaxSummaryDet1
    {

        public int? TaxID { get; set; }
        public int CMPID { get; set; }
        public string BillingType { get; set; }
        public DateTime TransactionDate { get; set; }
        public bool TaxGroupID { get; set; }
        public decimal? GrossAmount { get; set; }
        public decimal? TaxPayable { get; set; }
        public string TaxDescription { get; set; }
        public string CESSDescription { get; set; }
        public string AddlCESSDescription { get; set; }
        public Int16? TaxPercentage { get; set; }
        public Int16? Tax1Percentage { get; set; }
        public Int16? Tax2Percentage { get; set; }
        public decimal? CESSPercentage { get; set; }
        public decimal? AddlCESSPercentage { get; set; }
        public decimal? TaxValue { get; set; }
        public decimal? Tax1Value { get; set; }
        public decimal? Tax2Value { get; set; }
        public decimal? CESSValue { get; set; }
        public decimal? AddlCESSValue { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
    }




}
