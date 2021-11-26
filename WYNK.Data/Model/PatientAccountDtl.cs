using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class PatientAccountDetail
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PAccDetailID { get; set; }
        public int PAID { get; set; }
        public int OLMID { get; set; }
        public int? ServiceTypeID { get; set; }
        public string ServiceDescription { get; set; }
        public string Description { get; set; }
        public Decimal? TotalProductValue { get; set; }
        public Decimal? TotalDiscountValue { get; set; }
        public Decimal? TotalTaxValue { get; set; }
        public Decimal? TotalCGSTTaxValue { get; set; }
        public Decimal? TotalSGSTTaxValue { get; set; }
        public Decimal? TotalIGSTTaxValue { get; set; }
        public Decimal? TotalBillValue { get; set; }
        public Decimal? CESSValue { get; set; }
        public Decimal? AdditonalCESSValue { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

    }
}

