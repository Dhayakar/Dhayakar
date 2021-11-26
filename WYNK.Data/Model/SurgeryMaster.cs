using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
   public  class Surgery_Master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int FindingsID { get; set; }
        public string UIN { get; set; }
        public string Ocular { get; set; }
        public DateTime? DateofAdmission { get; set; }
        public int  SurgeryID { get; set; }
        public DateTime? DateofSurgery { get; set; }
        public DateTime? SurgeryPerformedDate { get; set;}
        public decimal GrossAmount { get; set;}
        public decimal Discount { get; set;}
        public decimal NetAmount { get; set;}
        public string TreatmentAdvice { get; set; }
        public string Remarks { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        [ForeignKey("FindingsID")]
        public virtual _Findings IDNumber { get; set; }
        [ForeignKey("UIN")]
        public virtual Registration_Master UINumber { get; set; }
        [ForeignKey("SurgeryID")]
        public virtual OneLine_Masters OLMId { get; set; }

    }
}

  