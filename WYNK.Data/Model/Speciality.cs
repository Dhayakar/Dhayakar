using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class Speciality_master
    {
        [Key]

        public int DoctorSpecialityID { get; set; }

        public int? DoctorID { get; set; }
        public int OLMID { get; set; }
        public Boolean IsActive { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        [ForeignKey("DoctorID")]
        public Doctor_Master Doctormasters { get; set; }

    }
}
