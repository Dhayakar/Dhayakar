using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class AdvancePaymentViewModel
    {
        public ICollection<paymenttran> paymenttran { get; set; }
        public Payment_Master payment { get; set; }
        public ICollection<ADVreP> ADVreP { get; set; }
        public ICollection<ADVrePEdit> ADVrePEdit { get; set; }
        public ICollection<ADVrePF> ADVrePF { get; set; }
        public int uid { get; set; }
    }
    public class ADVrePF
    {
        public string PaymentMode { get; set; }
        public string InstrumentNumber { get; set; }
        public DateTime? Instrumentdate { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public DateTime? Expirydate { get; set; }
        public decimal Amount { get; set; }
        public string PAddress { get; set; }
        public string PAddress2 { get; set; }
        public string PAddress3 { get; set; }
        public string Pphone { get; set; }
        public string Pweb { get; set; }
        public string PCompnayname { get; set; }
    }
  


    public class paymenttran
    {
    public string PaymentMode  { get; set; }
    public string InstrumentNumber { get; set; }
    public DateTime? Instrumentdate { get; set; }
    public string BankName{ get; set; }
    public string BankBranch { get; set; }
    public DateTime? Expirydate { get; set; }
    public string Amount { get; set; }
    public DateTime Date { get; set; }
    }

    public class ADVreP
    {
        public string ReceiptNumber { get; set; }
        public DateTime? ReceiptDate { get; set; }
        public DateTime? CreatedUTC { get; set; }
    }
    public class ADVrePEdit
    {
        public string PaymentMode { get; set; }
        public string InstrumentNumber { get; set; }
        public DateTime? Instrumentdate { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public DateTime? Expirydate { get; set; }
        public decimal Amount { get; set; }
        public decimal TAmount { get; set; }
    }
    
}

