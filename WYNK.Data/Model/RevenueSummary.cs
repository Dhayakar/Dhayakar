using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class RevenueSummary
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int CmpID { get; set; }
        public int? DoctorID { get; set; }
        public int? SpecialityID { get; set; }
        public string SpecialityDesc { get; set; }
        public DateTime Date { get; set; }
        public int? Numbers { get; set; }
        public Decimal? Amount { get; set; }       
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int? ServiceID { get; set; }

        public int? ParentID { get; set; }

    }
}












