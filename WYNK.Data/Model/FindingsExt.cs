using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class FindingsExt
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }//ICDCode
        public string ICDCode { get; set; }
        public string FindingsID { get; set; }
        public int ICDSpecialityid { get; set; }
        public Boolean IsOD { get; set; }
        public Boolean IsOS { get; set; }
        public Boolean IsOU { get; set; }
        public Boolean SurgeryComplete { get; set; }
        public Boolean Isdeleted { get; set; }
        public int? DischargeID { get; set; }
        public DateTime? SureryReviewDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedUTC { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public string Status { get; set; }


    }
}


