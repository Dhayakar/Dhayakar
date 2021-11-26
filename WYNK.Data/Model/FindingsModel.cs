using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
   public  class _Findings
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int RegistrationTranID { get; set; }
        public string UIN { get; set; }
        public string SlitLampODImagePath { get; set; }
        public string SlitLampOSImagePath { get; set; }
        public string IOLNCTOD { get; set; }
        public string IOLNCTOS { get; set; }
        public string IOLATOD { get; set;}
        public string IOLATOS { get; set;}
        public string FundusODImagePath { get; set;}
        public string FundusOSImagePath { get; set;}
        public string DiagnosisOthers { get; set; }
        public string TreatmentAdvice { get; set; }
        public Boolean IsSurgeryAdviced { get; set; }
        public decimal ConsultantFees { get; set; }
        public DateTime ReviewDate { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        [ForeignKey("FindingsID")]
        public virtual RegistrationTran_Master RID { get; set; }
        [ForeignKey("UIN")]
        public virtual Registration_Master UINumber { get; set; }
      

    }
}

  