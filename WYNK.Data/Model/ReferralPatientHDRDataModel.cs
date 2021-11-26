using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WYNK.Data.Model
{
    public class ReferralPatientHDRDataModel
    {
        [Key]
        public string Referral_Case_Number { get; set; }
        public string MR_NO { get; set; }
        public DateTime Visit_Date { get; set; }
        public string Reference_No { get; set; }
        public DateTime Reference_Date { get; set; }
        public string Referral_Code { get; set; }
        public string Referred_To { get; set; }
        public string Referred_To_Doc { get; set; }
        public string Module_Code { get; set; }
        public DateTime Sysdate { get; set; }
        public string UIN { get; set; }
        public int SiteId { get; set; }
    }
}
