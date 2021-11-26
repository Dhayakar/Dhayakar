using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model
{
    class dmmedicine
    {
        public string Brand { get; set; }
        public string ItemBatchNumber { get; set; }
        public string UOM { get; set; }
        public decimal ItemBatchBalanceQty { get; set; }
        public decimal ItemBatchQty { get; set; }
        public decimal ItemBatchissueQty { get; set; }
        public DateTime ItemBatchExpiry { get; set; }


    }
}
