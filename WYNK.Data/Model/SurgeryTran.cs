using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class Surgery_Tran
    {
        [Key]
        public int ID { get; set; }
        public string SurgeryID { get; set; }
        public string ICDCode { get; set; }
        public decimal? SurgeryAmount { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? SurgeryDiscount { get; set; }
        public decimal? NetSurgeryAmount { get; set; }
        public bool IsOD { get; set; }
        public bool IsOS { get; set; }
        public bool IsOU { get; set; }
        public int IcdSpecialityCode { get; set; }
    }
}

