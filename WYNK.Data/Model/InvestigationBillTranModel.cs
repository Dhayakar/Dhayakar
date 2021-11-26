using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class InvestigationBillTran
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int InvestigationBillID { get; set; }
        public string InvPrescriptionID { get; set; }
        public int InvPrescriptionTranId { get; set; }

        public int InvestigationID { get; set; }
        
        public decimal? Amount { get; set; }
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
        public int? ContraID { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int? TaxID { get; set; }

    }

}
