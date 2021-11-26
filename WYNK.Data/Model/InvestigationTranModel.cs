using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class InvestigationTran
    {
        [Key]
        public int ID { get; set; }
        public int CmpID { get; set; }
        public int InvestigationID { get; set; }
        public DateTime? InvestigationTakenOn { get; set; }
        public Decimal? InvestigationAmount { get; set; }
        public Boolean IsBilled { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int? PAID { get; set; }
        public int? TaxPercentage { get; set; }
        public int? TaxID { get; set; }
        public Decimal? TaxValue { get; set; }
        public Decimal? DiscountPercentage { get; set; }
        public Decimal? DiscountValue { get; set; }
        public Decimal? TotalValue { get; set; }

    }
}
