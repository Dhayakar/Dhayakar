using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace WYNK.Data.Model
{
    public class PatientRegistrationMaster

    {
        [Key]
        public string MR_NO { get; set; }
        public string IP_NO { get; set; }
        public string Patient_Class { get; set; }
        public string Patient_Name { get; set; }
        public string Patient_Lastname { get; set; }
        public string SurName { get; set; }
        public string Next_Of_Kin { get; set; }
        public DateTime Date_Of_Birth { get; set; }
        public string Sex { get; set; }
        public string Door { get; set; }
        public string Street_Locality { get; set; }
        public string Pincode { get; set; }
        public string Town_City { get; set; }
        public string Taluk { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Police_Station { get; set; }
        public string POBox { get; set; }
        public string Phone { get; set; }
        public string Email_Id { get; set; }
        public string AadhaarNo { get; set; }
        public DateTime Registered_Date { get; set; }
        public DateTime Last_Visit_Date { get; set; }
        public string Base_Unit { get; set; }
        public string Last_Unit_Visited { get; set; }
        public decimal Visit_Number { get; set; }
        public DateTime? Next_Followup_Date { get; set; }
        public DateTime Sysdate { get; set; }
        public string Remarks { get; set; }
        public string OneEyed { get; set; }
        public string Diabetic { get; set; }
        public string Cardiac { get; set; }
        public string Asthamatic { get; set; }
        public string Hypertensive { get; set; }
        public string Systemic_Diseases { get; set; }
        public string Allergic { get; set; }
        public string Other_Complication { get; set; }
        public string UIN { get; set; }
        public int SiteId { get; set; }
        public string PatientName_Tamil { get; set; }
        public string PatientName_Temp { get; set; }
        public string PatientImageName { get; set; }
    }
}
