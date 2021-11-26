using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class StockTran
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 STID { get; set; }
        public string SMID { get; set; }
        public string RandomUniqueID { get; set; }
        public int ItemID { get; set; }
        public Decimal ItemQty { get; set; }
        public int? UOMID { get; set; }
        public Decimal? ItemRate { get; set; }
        public Decimal? ItemValue { get; set; }
        public string ContraSMID { get; set; }
        public string ContraSTID { get; set; }
        public Decimal? ContraQty { get; set; }
        public Decimal? DiscountPercentage { get; set; }
        public Decimal? DiscountAmount { get; set; }
        public Decimal? GSTPercentage { get; set; }
        public Decimal? GSTTaxValue { get; set; }
        public Decimal? CGSTPercentage { get; set; }
        public Decimal? CGSTTaxValue { get; set; }
        public Decimal? SGSTPercentage { get; set; }
        public Decimal? SGSTTaxValue { get; set; }
        public Decimal? IGSTPercentage { get; set; }
        public Decimal? TotalIGSTTaxValue { get; set; }
        public Decimal? CESSPercentage { get; set; }
        public Decimal? AdditionalCESSPercentage { get; set; }
        public Decimal? CESSValue { get; set; }
        public Decimal? AdditionalCESSValue { get; set; }
        public Boolean IsDeleted { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public Decimal? GrossValue { get; set; }
        public int? Status { get; set; }
        public Decimal? LossInTransit { get; set; }
        public Decimal? DamageQty { get; set; }
        public Decimal? RecdQty { get; set; }
        public Decimal? OtherQty { get; set; }
    }
}
