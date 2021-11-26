using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{ 
    public class UserVsRoleViewModel
    {
        public User_Role User_Role { get; set; }
        public ICollection<GetEmployeeDetails> GetEmployeeDetails { get; set; }
        public ICollection<GetEmployeeDetails1> GetEmployeeDetails1 { get; set; }
        public ICollection<GetDoctorDetails> GetDoctorDetails { get; set; }
        public ICollection<GetDoctorDetails1> GetDoctorDetails1 { get; set; }
 
        
    }
 
        public class GetDoctorDetails1
        {
        public int userId { get; set; }
        public string Title { get; set; }
        public string Firstname { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Phone1 { get; set; }
        public string EmailID { get; set; }
        public bool Status { get; set; }     
        }

    public class GetDoctorDetails
    {
        public int userId { get; set; }
        public string Title { get; set; }
        public string Firstname { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Phone1 { get; set; }
        public string EmailID { get; set; }
        public bool Status { get; set; }
    }
    public class GetEmployeeDetails
    {
        
        public int userId { get; set; }
        public string Title { get; set; }
        public string Firstname { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string EmailID { get; set; }
        public string Phone1 { get; set; }
        public bool Status { get; set; }

    }
    public class GetEmployeeDetails1
    {

        public int userId { get; set; }
        public string Title { get; set; }
        public string Firstname { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string EmailID { get; set; }
        public string Phone1 { get; set; }
        public bool Status { get; set; }

    }
}
