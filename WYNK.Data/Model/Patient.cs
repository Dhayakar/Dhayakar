using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model
{
    public class Patient
    {
        public int Id { get; set; }
        public string Patient_Name { get; set; }
        public string Last_Name { get; set; }
        public string Next_Of_Kin { get; set; }
        public string Age { get; set; }
        public string Dob { get; set; }
        public string Language_Translate { get; set; }
        public string Mobile_No { get; set; }
        public string Alternate_No { get; set; }
        public string Aadhaar_No { get; set; }
        public string Gender { get; set; }
        public string Normal_Referral { get; set; }
        public string Allocation { get; set; }
        public string Purpose_Of_Visit { get; set; }
        public string Base_Unit { get; set; }
        public string MR_Location { get; set; }
        //[NotMapped]
        //public IFormFile UserImage { get; set; }
    }
}
