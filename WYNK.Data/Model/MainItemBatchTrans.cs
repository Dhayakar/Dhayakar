using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class MainItemBatchTrans
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 ItemBatchTransID { get; set; }
        public Int64 ItemBatchID { get; set; }
        public int TC { get; set; }
        public Int64 SMID { get; set; }
        public Int64 STID { get; set; }
        public int ItemID { get; set; }
        public string ItemBatchNumber { get; set; }
        public Decimal ItemBatchTransactedQty { get; set; }
        public DateTime ItemBatchExpiry { get; set; }
        public int? UOMID { get; set; }
        public Int64? ContraItemBatchID { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

    }
}
