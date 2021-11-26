using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class StockTranModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 STID { get; set; }
        public Int64 SMID { get; set; }
        public int? ItemID { get; set; }
        public Decimal? ItemQty { get; set; }
        public int? UOMID { get; set; }
        public Decimal? ItemRate { get; set; }
        public Decimal? ItemValue { get; set; }
        public int? ContraSMID { get; set; }
        public int? ContraSTID { get; set; }
        public Decimal? DiscountPercentage { get; set; }
        public Decimal? DiscountAmount { get; set; }
        public Decimal? GSTPercentage { get; set; }
        public Decimal? GSTTaxValue { get; set; }
        public Decimal? CGSTPercentage { get; set; }
        public Decimal? CGSTTaxValue { get; set; }
        public Decimal? SGSTPercentage { get; set; }
        public Decimal? SGSTTaxValue { get; set; }
        public Boolean IsDeleted { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public string DrugName { get; set; }

    }
}
