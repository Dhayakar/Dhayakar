using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
   public class commonreceipt
    {
        public ICollection<getrecp> getrecpp { get; set; }
    }
    public class getrecp
    {
        public string name { get; set; }
        public string PaymentModee { get; set; }
        public string InstrumentNumberr { get; set; }
        public DateTime? Instrumentdatee { get; set; }
        public string BankNamee { get; set; }
        public string BankBranchh { get; set; }
        public DateTime? Expirydatee { get; set; }
        public decimal Amountt { get; set; }
        public DateTime? Datee { get; set; }
        public string ReceiptNumberr { get; set; }
        public string UIN { get; set; }
        public int amount { get; set; }
        public string phone { get; set; }
        public DateTime? date1 { get; set; }
        public string description { get; set; }

    }
}
