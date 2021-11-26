using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class Patient_Assign
    {
        [Key]

        public int ID { get; set; }
        public int RegistrationTranID { get; set; }
        public DateTime AssignedDate { get; set; }
        public int DoctorID { get; set; }
        public DateTime? Cancelleddate { get; set; }
        
        public DateTime? ExaminationStartDate { get; set; }
        public DateTime? ExaminationCompletedDate { get; set; }
        public string CancellationReasons{ get; set; }
        public Boolean IsActive { get; set; }
        public Boolean IsCancelled { get; set; }
        public DateTime? CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public string Patientallocatestatus { get; set; }
    }
}

