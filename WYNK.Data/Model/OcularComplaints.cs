using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WYNK.Data.Model
{
    public class OcularComplaints
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }
        public int Description { get; set; }

        public bool ISOD { get; set; }
        public int? RegistrationTranID { get; set; }


        public bool ISOS { get; set; }
        public bool ISOU { get; set; }

        public string Reasons { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? FromUTC { get; set; }

        public string Remarks { get; set; }

        public int CreatedBy { get; set; }
        // public int? UpdatedBy { get; set; }

        [ForeignKey("RegistrationTranID")]
        public virtual RegistrationTran_Master RegistrationTransID { get; set; }



    }
}
