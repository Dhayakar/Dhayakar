using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class OpticalMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int OpticalPrescriptionID { get; set; }
        public int RegTranID { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? BookingDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public Decimal GrossProductValue { get; set; }
        public Decimal? TotalDiscountValue { get; set; }
        public Decimal? TotalTaxValue { get; set; }
        public Decimal? TotalCGSTTaxValue { get; set; }
        public Decimal? TotalSGSTTaxValue { get; set; }
        public Decimal? TotalIGSTTaxValue { get; set; }
        public Decimal NetAmount { get; set; }
        public int CMPID { get; set; }
        public int TransactionId { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }


    }
}


