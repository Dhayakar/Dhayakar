using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class InvestigationPrescription
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int IPID { get; set; }
        public string INVPRESNO { get; set; }
        public bool isbilled { get; set; }
        public int CMPID { get; set; }
        public string UIN { get; set; }
        public int RegistrationTranID { get; set; }
        public DateTime? Dateofinvestigation { get; set; }
        public int? PrescribedBy { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public Boolean IsDeleted { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public string Fyear { get; set; }
        public string RandomUniqueID { get; set; }
        public string Tag { get; set; }

    }
}

