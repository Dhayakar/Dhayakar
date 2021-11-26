using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class ItemBalance
    {
        [Key]
        public int ID { get; set; }
        public int ItemID { get; set; }
        public int? UOMID { get; set; }
        public int FYear { get; set; }
        public decimal OpeningBalance { get; set; }
        public int StoreID { get; set; }
        public decimal Rec01 { get; set; }
        public decimal Rec02 { get; set; }
        public decimal Rec03 { get; set; }
        public decimal Rec04 { get; set; }
        public decimal Rec05 { get; set; }
        public decimal Rec06 { get; set; }
        public decimal Rec07 { get; set; }
        public decimal Rec08 { get; set; }
        public decimal Rec09 { get; set; }
        public decimal Rec10 { get; set; }
        public decimal Rec11 { get; set; }
        public decimal Rec12 { get; set; }
        public decimal Iss01 { get; set; }
        public decimal Iss02 { get; set; }
        public decimal Iss03 { get; set; }
        public decimal Iss04 { get; set; }
        public decimal Iss05 { get; set; }
        public decimal Iss06 { get; set; }
        public decimal Iss07 { get; set; }
        public decimal Iss08 { get; set; }
        public decimal Iss09 { get; set; }
        public decimal Iss10 { get; set; }
        public decimal Iss11 { get; set; }
        public decimal Iss12 { get; set; }
        public decimal ClosingBalance { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int CmpID { get; set; }


        public Decimal? LossInTransit { get; set; }
        public Decimal? DamageQty { get; set; }
        public Decimal? OtherQty { get; set; }
    }
}
