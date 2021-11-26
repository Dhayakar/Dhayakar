using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class Doctor_Master
    {
        [Key]

        public int DoctorID { get; set; }
        public int CMPID { get; set; }
        
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string LocationID { get; set; }
        public string Title { get; set; }
        public string Firstname { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Gender { get; set; }
        public DateTime? DateofBirth { get; set; }
        public string RegistrationNumber { get; set; }
        public string Designation { get; set; }
        public int? EngagementType { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string EmailID { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public string Photopath { get; set; }
        public int? RoleID { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public string StaffIdentification { get; set; }


    }


}
