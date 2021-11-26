using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{


    public class SurgeryDoctor
    {
        [Key]
        public int ID { get; set; }
        public int SurgeryID { get; set; }
        public int? OTID { get; set; }
        public int? DoctorID { get; set; }
        public string DoctorDescription { get; set; }
        public int? AnaestheticID { get; set; }
        public string HospitalName { get; set; }
        public string Location { get; set; }
        public Int64? PhoneNumber { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
    }


}

  