using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
   public  class Surgery_Discharge
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int CMPID { get; set; }
        public int? TransactionId { get; set; }
        public string AdmissionID { get; set; }
        public string SurgeryID { get; set; }
        public int RegTranID { get; set; }
        public string TreatmentAdvice { get; set; }
        public string DischargeType { get; set; }
        public DateTime DischargeDate { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
    }
}

  