using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class ItemBatch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 ItemBatchID { get; set; }
        public int ItemID { get; set; }
        public string ItemBatchNumber { get; set; }
        public Decimal ItemBatchQty { get; set; }
        public Decimal? ItemBatchissueQty { get; set; }
        public Decimal ItemBatchBalnceQty { get; set; }
        public DateTime ItemBatchExpiry { get; set; }
        public int? LockedQuantity { get; set; }
        public int? StoreID { get; set; }
        public int cmpID { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }

        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public string RandomUniqueID { get; set; }

    }
}
