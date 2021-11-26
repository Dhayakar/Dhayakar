using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class SquintMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]


        public int ID { get; set; }//ICDCode
        public int? FindingsID { get; set; }
        public int? RefractionID { get; set; }
       // public int ICDSpecID { get; set; }
        public int DoctorID { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedUTC { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedUTC { get; set; }



    }
}


