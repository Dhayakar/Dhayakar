using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class TransactionType
    {
        [Key]
        public int TransactionID { get; set; }
        public string Description { get; set; }
        public int? ContraTransactionid { get; set; }
        public int? RecPayContra { get; set; }
        public string Rec_Issue_type { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

    }
}
