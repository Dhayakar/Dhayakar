using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{

    public class CustomerOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string RandomUniqueID { get; set; }
        public int CmpID { get; set; }
        public int? CusID { get; set; }
        public int TCID { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public string RefNo { get; set; }
        public DateTime? RefDate { get; set; }
        public DateTime? Deliverydate { get; set; }
        public string Remarks { get; set; }
        public int? ContraOrderID { get; set; }
        public Boolean IsCancelled { get; set; }
        public DateTime? CancelledDate { get; set; }
        public string CancelledReasons { get; set; }
        public string OrderStatus { get; set; }
        public decimal? GrossProductValue { get; set; }
        public decimal? TotalDiscountValue { get; set; }
        public decimal? TotalTaxvalue { get; set; }
        public decimal? TotalProductValue { get; set; }
        public decimal? TotalCGSTTaxValue { get; set; }
        public decimal? TotalSGSTTaxValue { get; set; }
        public decimal? TotalIGSTTaxValue { get; set; }
        public decimal? TotalCESSValue { get; set; }
        public decimal? TotalAddCESSValue { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public string Fyear { get; set; }
        public string OpticalPrescriptionPath { get; set; }
    }

}
