using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class MedicalPrescription
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MedicalPrescriptionID { get; set; }
        public string RandomUniqueID { get; set; }
        public int CmpID { get; set; }
        public int RegistrationTranID { get; set; }
        public string UIN { get; set; }
        public string MedicalPrescriptionNo { get; set; }
        public int PrescribedBy { get; set; }
        public string PrescribedByName { get; set; }
        public DateTime PrescribedDate { get; set; }
        public string Status { get; set; }
        public string Tag { get; set; }

        public Boolean IsDeleted { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }


        public string SurgeryID { get; set; }
        public int? DischargeID { get; set; }
        public int? Transactionid { get; set; }
        public string ImagePath { get; set; }
        public string Fyear { get; set; }

    }
}


