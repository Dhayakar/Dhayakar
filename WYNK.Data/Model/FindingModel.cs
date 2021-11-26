using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class Finding
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string RandomUniqueID { get; set; }
        public int CmpID { get; set; }
        public int RegistrationTranID { get; set; }
        public string UIN { get; set; }
        public string SlitLampODImagePath { get; set; }
        public string SlitLampOSImagePath { get; set; }
        public string IOLNCTOD { get; set; }
        public string IOLNCTOS { get; set; }
        public string IOLATOD { get; set; }
        public string IOLATOS { get; set; }
        public string FundusODImagePath { get; set; }
        public string FundusOSImagePath { get; set; }
        public string DiagnosisOthers { get; set; }
        public string ProposedSurgeryPeriod { get; set; }

        public string TreatmentAdvice { get; set; }
        public Boolean IsSurgeryAdviced { get; set; }
        public DateTime? ReviewDate { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public string FilePath { get; set; }
        public int? Category { get; set; }
        public string DistSphod { get; set; }
        public string NearCylod { get; set; }
        public string N_V_DESCod { get; set; }
        public string PinAxisod { get; set; }
        public string DistSphos { get; set; }
        public string NearCylos { get; set; }
        public string N_V_DESCos { get; set; }
        public string PinAxisos { get; set; }
        public string IOLNCTbdOD { get; set; }
        public string IOLNCTbdOS { get; set; }
        public string IOLATbdOD { get; set; }
        public string IOLATbdOS { get; set; }
        public string PgDtls { get; set; }
        public string PgSphOD { get; set; }
        public string PgCylOD { get; set; }
        public string PgAxisOD { get; set; }
        public string PgAddOD { get; set; }
        public string PgSphOS { get; set; }
        public string PgCylOS { get; set; }
        public string PgAxisOS { get; set; }
        public string PgAddOS { get; set; }
        public int? Instrument { get; set; }
        public string RDySphOD { get; set; }
        public string RDyCylOD { get; set; }
        public string RDyAxisOD { get; set; }
        public string RDySphOS { get; set; }
        public string RDyCylOS { get; set; }
        public string RDyAxisOS { get; set; }
        public string RWtSphOD { get; set; }
        public string RWtCylOD { get; set; }
        public string RWtAxisOD { get; set; }
        public string RWtSphOS { get; set; }
        public string RWtCylOS { get; set; }
        public string RWtAxisOS { get; set; }
        public string PGlassOD { get; set; }
        public string PGlassOS { get; set; }
        public string TimeNCT { get; set; }
        public string TimeAT { get; set; }
        public Decimal? Consultation { get; set; }
        public int? Categoryos { get; set; }
        public Boolean IsBilled { get; set; }
        public string PFC { get; set; }
    }
}


