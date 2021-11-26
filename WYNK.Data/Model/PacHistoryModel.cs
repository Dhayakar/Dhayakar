using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace WYNK.Data.Model
{
    public class PACHistoryModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PACHistoryID { get; set; }
        public int CMPID { get; set; }
        public int ADMID { get; set; }
        public string UIN { get; set; }
        public bool Previous_Operation { get; set; }
        public bool ColdCoughFever { get; set; }
        public bool Orthopnoea_PND { get; set; }
        public bool CPD_Asthma { get; set; }
        public bool Unconsciousness { get; set; }
        public bool Psychiatric_illness { get; set; }
        public bool PalpitationsBreathless { get; set; }
        public bool Renal_Disorders { get; set; }
        public bool Jaundice { get; set; }
        public bool HTN_IHD_CHD_DM { get; set; }
        public bool Bleeding_Disorders { get; set; }
        public int Exercise_Tolerance { get; set; }
        public string Family_Ho { get; set; }
        public string Menstrual { get; set; }
        public string Last_Meal_Time { get; set; }
        public bool Smoking_Tobacco { get; set; }
        public bool Alcohol_habituation { get; set; }
        public string Drug_Addictions { get; set; }
        public string Known_allergies { get; set; }
        public string Remarks { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public int DoctorID { get; set; }

    }
}
