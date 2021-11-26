using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{

    public class OpticalInvoiceMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int CMPID { get; set; }
        public int TransactionId { get; set; }
        public string InvoiceNumber { get; set; }
        public string Fyear { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? BookingDate { get; set; }
        public DateTime? DeliveryDate { get; set; }

        public decimal GrossProductValue { get; set; }
        public decimal? TotalDiscountValue { get; set; }
        public decimal? TotalTaxValue { get; set; }
        public decimal? TotalCGSTTaxValue { get; set; }
        public decimal? TotalSGSTTaxValue { get; set; }
        public decimal? TotalIGSTTaxValue { get; set; }
        public decimal NetAmount { get; set; }
        public decimal? CESSAmount { get; set; }
        public decimal? AdditionalCESSAmount { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public string RandomUniqueID { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int? CustomerID { get; set; }
    }

}
