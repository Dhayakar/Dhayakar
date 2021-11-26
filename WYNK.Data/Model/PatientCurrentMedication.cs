using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class PatientCurrentMedication
    {
        [Key]
        public int ID { get; set; }
        public int RegistrationTranID { get; set; }
        public string UIN { get; set; }
        public string Remarks { get; set; }
        public DateTime VisitDate { get; set; }
        public string GenericDrugDescription { get; set; }
        public Boolean? IsOD { get; set; }
        public Boolean? IsOS { get; set; }
        public Boolean? IsOU { get; set; }
        public DateTime? Since { get; set; }
        public int ProgressStatusID { get; set; }
        public int? PrescribedBy { get; set; }
        public string PrescribedByDoctorName { get; set; }
        public int? Frequency { get; set; }
        public int Cmpid { get; set; }
    }
}

