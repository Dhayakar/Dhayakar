using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class PatientGeneralDatamodel
    {
        [Key]
        public int RunningID { get; set; }
        public string UIN { get; set; }
        public int RegistrationTranID { get; set; }
        public DateTime? VisitDate { get; set; }
        public string CurrentMedication { get; set; }
        public string Allergy { get; set; }
        public string FamilyHistory { get; set; }
        public Boolean IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
