using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WYNK.Data.Model
{
    public class CampPatientFootDoctor
    {
        [Key]
        public string RandomUniqueID { get; set; }
        public string CampFRTID { get; set; }
        public int DoctorID { get; set; }
        public string SurgeryID { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int Createdby { get; set; }
        public int? Updatedby { get; set; }

    }
}
