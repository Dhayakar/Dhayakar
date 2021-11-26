using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{

    public class MedicalBill_Master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int CMPID { get; set; }
        public int PAID { get; set; }
        public int? TransactionId { get; set; }
        public int RegistrationTranID { get; set; }
        public string UIN { get; set; }
        public string BillNo { get; set; }
        public DateTime? BillDate { get; set; }
        public decimal? GrossProductValue { get; set; }
        public decimal? TotalDiscountValue { get; set; }
        public decimal? CESSValue { get; set; }
        public decimal? AdditionalCESSValue { get; set; }
        public decimal? TotalTaxValue { get; set; }
        public decimal? TotalCGSTTaxValue { get; set; }
        public decimal? TotalSGSTTaxValue { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public decimal? TotalBillValue { get; set; }
        public int CreatedBy { get; set; }
         public int? UpdatedBy { get; set; }
        public string Fyear { get; set; }
        public Boolean Status { get; set; }
        public string BillType { get; set; }
        public decimal? AmountCollected { get; set; }
        public bool? CreditStatus { get; set; }
        public decimal? ReturnedItemAmount { get; set; }
    }

}
