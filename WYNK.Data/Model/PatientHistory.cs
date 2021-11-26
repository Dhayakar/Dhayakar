using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WYNK.Data.Model
{
    public class PatientHistory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }
        public int Description { get; set; }
        public int RegistrationTranID { get; set; }
        public DateTime? FromUTC { get; set; }

        public string Remarks { get; set; }
        public string UIN { get; set; }

        public bool IsActive { get; set; }
        public string Reasons { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
       public int CreatedBy { get; set; }
       public int UpdatedBy { get; set; }

        [ForeignKey("UIN")]
        public virtual Registration_Master PatientUIN { get; set; }
    }
}
