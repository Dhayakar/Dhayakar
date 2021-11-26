using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WYNK.Data.Model
{
    public class Speciality_trans
    {
        [Key]
        public int Sno { get; set; }
        public int DoctorID { get; set; }
        public int OLMID { get; set; }
    }
}
