using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class CampRegistration
    {
        [Key]
        public string CampUIN { get; set; }
        public int CMPID { get; set; }
        public string RegType { get; set; }
        public DateTime DateofRegistration { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string AlternatePhoneNumber { get; set; }
        public string AlternateMailID { get; set; }
        public DateTime DateofBirth { get; set; }
        public string Gender { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public int LocationID { get; set; }
        public string FatherHusbandName { get; set; }
        public string Occupation { get; set; }
        public string Phone { get; set; }
        public string AadharNumber { get; set; }
        public int? TransactionID { get; set; }
        public string PanCardNo { get; set; }
        public string DrivingLicenseNo { get; set; }
        public string PassportNo { get; set; }
        public int? IsForeignNational { get; set; }
        public string EmailID { get; set; }
        public int SourceofReferralID { get; set; }
        public string ReferralName { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public string PhotoPath { get; set; }
        public string BiometricPath { get; set; }
        public string PreferredLanguage { get; set; }
        public int? MaritalStatus { get; set; }
        public bool Insurance { get; set; }
        public bool? Videoconsent { get; set; }
        public bool? VoiceConsent { get; set; }
        public bool? IsVisited     { get; set; }
        public bool IsSurgeryDone { get; set; }
        public string ICDCode      { get; set; }
        public int? ICDSpecialityId { get; set; }
        public string DiagnosisOthers { get; set; }
        public string TreatmentAdvice { get; set; }
        public int CampID { get; set; }


    }
}

