using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class OpticalTran
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int OID { get; set; }
        public int? LensID { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public int? UOMID { get; set; }
        public Decimal Price { get; set; }
        public Decimal ProductAmount { get; set; }
        public int? TaxID { get; set; }
        public Decimal? DiscountAmount { get; set; }
        public int? TaxPercentage { get; set; }
        public Decimal? TaxAmount { get; set; }
        public Decimal NetAmount { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }


    }
}


