using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class Tax_Summary
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int? TaxID { get; set; }
        public int CMPID { get; set; }
        public string BillingType { get; set; }
        public DateTime TransactionDate { get; set; }
        public int TaxGroupID { get; set; }
        public decimal? GrossAmount { get; set; }
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
        public int? UpdatedBy { get; set; }
    }
}

