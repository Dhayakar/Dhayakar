using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class ItemBatchTrans
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 ItemBatchTransID { get; set; }
        public string ItemBatchID { get; set; }
        public int TC { get; set; }
        public string SMID { get; set; }
        public int cmpID { get; set; }
        public string STID { get; set; }
        public int ItemID { get; set; }
        public string ItemBatchNumber { get; set; }
        public Decimal ItemBatchTransactedQty { get; set; }
        public Decimal? ReturnQty { get; set; }
        public DateTime ItemBatchExpiry { get; set; }
        public int? UOMID { get; set; }
        public string ContraItemBatchID { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public string RandomUniqueID { get; set; }

    }
}
