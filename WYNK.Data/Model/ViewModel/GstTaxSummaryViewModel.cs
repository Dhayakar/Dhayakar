using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class GstTaxSummaryViewM
    { 
        public ICollection<getGSTSummary1> getGSTSummary1 { get; set; }
        public ICollection<getIGSTsummary1> getIGSTsummary1 { get; set; }
        public ICollection<getGSTSummary> getGSTSummary { get; set; }
        public ICollection<getIGSTsummary> getIGSTsummary { get; set; }
        public PatientAccountDetailTax PatientAccountDetailTax { get; set; }


    }

    public class getGSTSummary
    {
        public int? Tax1 { get; set; }
        public decimal? TotTurnOver { get; set; }
        public decimal? TotGST { get; set; }
        public decimal? TotCGST { get; set; }
        public decimal? TotSGST { get; set; }
    }


    public class getIGSTsummary
    {
        public int? Tax2 { get; set; }
        public decimal? TotTurnOver1 { get; set; }
        public decimal? TotalIGST { get; set; }
    }



    public class getGSTSummary1
    {
      
        //public int PAccDetailTaxID { get; set; }
        //public int PAccDetailID { get; set; }
        //public int ServiceTypeID { get; set; }
        //public int TaxID { get; set; }
        public int? TaxPercentage { get; set; }

        public decimal? TotalTurnOver { get; set; }
        public decimal? TotalProductValue { get; set; }
        public decimal TotalDiscountValue { get; set; }
        public decimal TotalTaxValue { get; set; }
        public decimal? TotalGSTTaxValue { get; set; }
        public decimal? TotalCGSTTaxValue { get; set; }
        public decimal? TotalSGSTTaxValue { get; set; }
        public decimal TotalIGSTTaxValue { get; set; }

        //public decimal TotalValue { get; set; }
        //public DateTime CreatedUTC { get; set; }
        //public DateTime UpdatedUTC { get; set; }
        //public DateTime CreatedBy { get; set; }
        //public DateTime UpdatedBy { get; set; }
    }

    public class getIGSTsummary1
    {

        public int? TaxPercentage { get; set; }

        public decimal? TotalTurnOver { get; set; }
        public decimal TotalProductValue { get; set; }
        public decimal TotalDiscountValue { get; set; }
        public decimal TotalTaxValue { get; set; }

        public decimal? TotalIGSTTaxValue { get; set; }

    }


}
