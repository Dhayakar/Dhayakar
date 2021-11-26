using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class Vmmedicine
    {
        public ICollection<Medicine> SerialDrugs { get; set; }
        public ICollection<Medicine> BatchDrugs { get; set; }
    }

    public class Medicine
    {
        public string Brand { get; set; }
        public string StoreName { get; set; }
        public string ItemBatchNumber { get; set; }
        public string UOM { get; set; }
        public decimal ItemBatchBalanceQty { get; set; }
        public decimal ItemBatchQty { get; set; }
        public DateTime ItemBatchExpiry { get; set; }
    }
}
