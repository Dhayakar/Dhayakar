using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class Employee_Master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeID { get; set; }
        public int CMPID { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime Dateofbirth { get; set; }
        public int Gender { get; set; }
        public Boolean PhysicallyChallenged { get; set; }
        public string MaritalStatus { get; set; }
        public string Category { get; set; }
        public int DeptCode { get; set; }
        public string AadhaarNo { get; set; }
        public string PANNo { get; set; }
        public string FatherHusbandName { get; set; }
        public string ReasonForResignation { get; set; }
        public DateTime DOJ { get; set; }
        public DateTime? DOR { get; set; }
        public string Designation { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactNo { get; set; }
        public string BloodGroup { get; set; }
        public Boolean IsDeleted { get; set; }
        public Boolean IsActive { get; set; }
        public DateTime? UpdatedUTC { get; set; }

        public DateTime CreatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public string PhotoPath { get; set; }
    }


}
