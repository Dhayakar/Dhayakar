using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class PatientVsInsuranceViewModel
    {
        public PatientVsInsurance PatientVsInsurance { get; set; }
        public JointFamilyDetails JointFamilyDetails { get; set; }
        public InsuranceImageTran InsuranceImageTran { get; set; }
        public ICollection<PatVsInsRegdetail> PatVsInsRegdetail { get; set; }
        public ICollection<JointPolicyKINRdetail> JointPolicyKINRdetail { get; set; }
        public ICollection<JoindataKIN> JoindataKIN { get; set; }
        public ICollection<getPatVsIns> getPatVsIns { get; set; }


        public ICollection<InsImageTranArray> InsImageTranArray { get; set; }


    }
    public class InsImageTranArray
    {
        public int ID { get; set; }
        public int PAINSID { get; set; }
        public string InsuranceImagePath { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int CmpID { get; set; }


    }
    public class JoindataKIN
    {
        public int ID { get; set; }
        public decimal Amount { get; set; }
        public string Relationship { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string ContactNumber { get; set; }
        public string EmailID { get; set; }    
    }
    public class getPatVsIns
    {
        public int PAINSID { get; set; }
        public string UIN { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateofBirth { get; set; }
        public string Gender { get; set; }
        public string Age { get; set; }
        public int InsurancevsMiddlemenID { get; set; }
        public string PolicyName { get; set; }
        public string PolicyNo { get; set; }
        public DateTime? PolicyTakenOn { get; set; }
        public DateTime? PeriodFrom { get; set; }
        public DateTime? PeriodTo { get; set; }
        public bool IsJointPolicy { get; set; }
        public decimal SumAssured { get; set; }
        public decimal? AmountAvailed { get; set; }
        public bool IsActive { get; set; }
        public string Remarks { get; set; }
       

        
    }

    public class PatVsInsRegdetail
    {
        public string UIN { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateofBirth { get; set; }
        public DateTime DateofRegistration { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
    }
    public class JointPolicyKINRdetail
    {
        public int ID { get; set; }
        public string Relationship { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string ContactNumber { get; set; }
        public string EmailID { get; set; }
       
        
    }
}
