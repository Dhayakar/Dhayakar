using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class OpticalInvoiceTran
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string OID { get; set; }
        public string COID { get; set; }
        public int COTID { get; set; }
        public int LensID { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public int? UOMID { get; set; }
        public int? TaxID { get; set; }
        public decimal? Amount { get; set; }
        public decimal? itemValue { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? GSTPercentage { get; set; }
        public decimal? GSTTaxValue { get; set; }
        public decimal? CGSTPercentage { get; set; }
        public decimal? CGSTTaxValue { get; set; }
        public decimal? SGSTPercentage { get; set; }
        public decimal? SGSTTaxValue { get; set; }
        public decimal? IGSTPercentage { get; set; }
        public decimal? IGSTTaxValue { get; set; }
        public decimal? CESSPercentage { get; set; }
        public decimal? CESSAmount { get; set; }
        public decimal? AdditionalCESSPercentage { get; set; }
        public decimal? AdditionalCESSAmount { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
    }

}
