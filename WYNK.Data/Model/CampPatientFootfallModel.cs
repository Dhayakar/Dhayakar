using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WYNK.Data.Model
{
    public class CampPatientFootfall
    {
        [Key]
        public string RandomUniqueID { get; set; }
        public int CmpID { get; set; }
        public int CampID { get; set; }
        public DateTime Date { get; set; }
        public int PatientCount { get; set; }
        public int SurgeryAdvisedPatient { get; set; }
        public int SurgeryUnderwentPatient { get; set; }
        public int? GeneralNormal { get; set; }
        public int? GeneralOcular { get; set; }
        public int? PaediatricNormal { get; set; }
        public int? PaediatricOcular { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int Createdby { get; set; }
        public int? Updatedby { get; set; }
    }
}
