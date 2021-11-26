using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class CampRegistrationTran
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int RegistrationTranID { get; set; }
        public string CampUIN { get; set; }
        public int CmpID { get; set; }
        public DateTime DateofVisit { get; set; }
        public DateTime? Cancelleddate { get; set; }
        public int? TypeofVisit { get; set; }
        public int Status { get; set; }
        public DateTime? StatusDateTime { get; set; }
        public DateTime? FollowupDate { get; set; }
        public string Remarks { get; set; }
        public Boolean IsDeleted { get; set; }
        public Boolean? IsCancelled { get; set; }
        public string CancellationReasons { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int? DoctorID { get; set; }
        public string PatientVisitType { get; set; }
        public DateTime? AssignedDate { get; set; }
        public int? KinConsentID { get; set; }
        public int? ReviewCount { get; set; }
        

    }
}

