using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace WYNK.Data.Model
{
    public class PACExamination
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PACExamID { get; set; }
        public int CMPID { get; set; }
        public int? ADMID { get; set; }//RegistrationTranID
        public int RegistrationTranID { get; set; }
        public string UIN { get; set; }
        public decimal PulseHeartRate { get; set; }
        public decimal Respiration { get; set; }
        public string BloodPressure { get; set; }
        public decimal temperature { get; set; }
        public Int16 GC { get; set; }
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public decimal BMI { get; set; }
        public Int16 NStatus { get; set; }
        public bool Pallor { get; set; }
        public bool Icterus { get; set; }
        public bool Cyanosis { get; set; }
        public string Clubbing { get; set; }
        public bool LAP { get; set; }
        public bool Oedema { get; set; }
        public string JVP { get; set; }
        public Int16 Veins { get; set; }
        public string Spine { get; set; }
        public Int16 BHT { get; set; }
        public string TMJoint { get; set; }
        public Int16 MouthOpening { get; set; }
        public bool LooseMissedTeeth { get; set; }
        public string MallampattiClass { get; set; }
        public Int16 Thyromentaldist { get; set; }
        public bool ArtificialTeeth { get; set; }
        public string NasalPatency { get; set; }
        public Int16 OralHygiene { get; set; }
        public string UpperRespiratory { get; set; }
        public string LowerRespiratory { get; set; }
        public string CVS { get; set; }
        public string CNS { get; set; }
        public string PerAbdomen { get; set; }
        public string Neck { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int? DoctorID { get; set; }
        public string HtUnit { get; set; }
        public string WtUnit { get; set; }
        public string TempUnit { get; set; }

    }
}
