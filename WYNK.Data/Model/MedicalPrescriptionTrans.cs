using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class MedicalPrescriptionTran
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MedicalPrescriptionTranID { get; set; }
        public string MedicalPrescriptionID { get; set; }
        public int DrugId { get; set; }
        public string Dossage { get; set; }
        public string Frequency { get; set; }
        public int? Quantity { get; set; }
        public string Eye { get; set; }
        public string Food { get; set; }
        public Int16? Days { get; set; }
        //public DateTime? FromDate { get; set; }
        //public DateTime? ToDate { get; set; }
        public string Remarks { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public string Brand { get; set; }
        public string DrugForm { get; set; }


        //[ForeignKey("MedicalPrescriptionID")]
        //public virtual MedicalPrescription MedicalPrescription { get; set; }

    }
}
