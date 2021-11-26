using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class SquintTran
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }
        public int RID { get; set; }
        public Boolean IsDVOD { get; set; }
        public Boolean IsDVOS { get; set; }
        public Boolean IsDVOU { get; set; }
        public DateTime Date { get; set; }
        public int SquintType { get; set; }
        public string SquintDiagnosisDescription { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedUTC { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public Boolean IsActive { get; set; }


    }
}


