using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class doctorsearchviemodel
    {
        public ICollection<Doctorsearchpatients> Doctorsearchpatients { get; set; }
    }
    public class Doctorsearchpatients
    {
        public int? DoctorID { get; set; }
        public int DoctorSpecialityID { get; set; }
        public string CMPID { get; set; }
        public string DoctorName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Title { get; set; }
        public DateTime? DateofBirth { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string LocationID { get; set; }
        public string Designation { get; set; }
        public string RegistrationNumber { get; set; }
        public string EngagementType { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string EmailID { get; set; }
        public string Speciality { get; set; }
        public string City { get; set; }

        public string DOCTORTAG { get; set; }
        public int roleid { get; set; }
        public Boolean Status { get; set; }
    }

}
