using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class OpticalPrescriptionmaster
    {
        [Key]
        public int ID { get; set; }
        public string UIN { get; set; }
        public int? RegistrationID { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateofBirth { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public DateTime PrescriptionDate { get; set; }
        public int CMPID { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int? CustomerMasterID { get; set; }
       
    }
}
















