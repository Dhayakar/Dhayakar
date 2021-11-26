using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WYNK.Data.Model
{
  public  class Appointment
    {

        [Key]
        public int ID { get; set; }
        public string RandomUniqueID { get; set; }
        public int CMPID { get; set; }        
        public string PatientName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public decimal ConsulationFees { get; set; }
        public string PatientImage { get; set; }
        public DateTime Dateofbirth { get; set; }
        public string UIN { get; set; }
        public string Gender { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Locationname { get; set; }
        public string Phone { get; set; }
        public int AppointmentdoctorID { get; set; }
        public string AppointmentReasons { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public bool? IsVisited { get; set; }
    }
}
