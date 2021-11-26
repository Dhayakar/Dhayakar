using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class Admission
    {
        [Key]
        public int AdmID { get; set; }
        public string UIN { get; set; }
        public int CMPID { get; set; }
        public int RegTranID { get; set; }
        public DateTime AdmDate { get; set; }
        public string ReferredBy { get; set; }
        public Boolean? IsSurgeryCompleted { get; set; }
        public int? DischargeID { get; set; }
        public int? CounsellingID { get; set; }
        public int? AdmissionType { get; set; }
        public Boolean isbilled { get; set; }
        public string InvoiceNumber { get; set; }
        public string AdmissionNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public int? Paid { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public string RandomUniqueID { get; set; }
        public string BillingType { get; set; }
        public string Category { get; set; }



    }
}

