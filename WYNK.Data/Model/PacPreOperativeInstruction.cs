using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WYNK.Data.Model
{
    public class PACPreOperativeInstruction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PACPreOperativeID { get; set; }
        public int CMPID { get; set; }
        public int ADMID { get; set; }
        public string UIN { get; set; }
        public string NilOralyAfterTime { get; set; }
        public DateTime? NilOralyAfterDate { get; set; }
        public string ArrangeFor { get; set; }
        public string BloodProduct { get; set; }
        public string ShiftedOTTime { get; set; }
        public DateTime? ShiftedOTDate { get; set; }
        public string SpecialInstructions { get; set; }
        public string PreMedications { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int DoctorID { get; set; }
    }
}
