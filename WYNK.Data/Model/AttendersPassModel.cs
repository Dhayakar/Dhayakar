using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class AttendersPass
    {
        [Key]
        public int ID { get; set; }
        public int RegistrationTranID { get; set; }
        public string AdmID { get; set; }
        public string UIN { get; set; }
        public string Name { get; set; }
        public string Relationship { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int CmpID { get; set; }

    }
}








