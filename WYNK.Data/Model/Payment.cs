using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class Payment_Master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string UIN { get; set; }
        public string PaymentMode { get; set; }
        public string PaymentType { get; set; }
        public string InstrumentNumber { get; set; }
        public DateTime? Instrumentdate { get; set; }
        public DateTime? Expirydate { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string ReceiptNumber { get; set; }
        public decimal Amount { get; set; }
        public DateTime? ReceiptDate { get; set; }
        
        public int? OLMID { get; set; }
        public Boolean IsBilled { get; set; }
        public Boolean? IsCancelled { get; set; }
        
        public string InVoiceNumber { get; set; }
        public DateTime? InVoiceDate { get; set; }
        public int? Paid { get; set; }
        public int? CmpID { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int? TransactionID { get; set; }
        public int? PaymentReferenceID { get; set; }

        public string Fyear { get; set; }
    }
}

