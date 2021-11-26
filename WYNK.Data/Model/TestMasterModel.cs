using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class TestMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string RandomUniqueID { get; set; }
        public string UIN { get; set; }
        public int TestID { get; set; }
        public DateTime? TestDateTime { get; set; }
        public int? DiagnosisID { get; set; }
        public int? Cooperation { get; set; }
        public int? ReliablityOD { get; set; }
        public int? ReliablityOS { get; set; }
        public int? Treatment { get; set; }
        public string Comments { get; set; }
        public int? PerformedByID { get; set; }
        public string PerformedBy { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }


    }
}
