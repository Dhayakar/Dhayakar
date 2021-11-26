using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
   public class PatientVsInsurance
    {
        [Key]
        public int PAINSID { get; set; }
        public int CmpID { get; set; }
        public string UIN { get; set; }
        public int InsurancevsMiddlemenID { get; set; }
        public int? InsuranceID { get; set; }
        public string PolicyName { get; set; }
        public string PolicyNo { get; set; }
        public DateTime? PolicyTakenOn { get; set; }
        public DateTime? PeriodFrom { get; set; }
        public DateTime? PeriodTo { get; set; }
        public bool IsJointPolicy { get; set; }
        public decimal SumAssured { get; set; }
        public decimal? AmountAvailed { get; set; }
        public bool IsActive { get; set; }
        public bool IsTransacted { get; set; }
        public string Remarks { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
