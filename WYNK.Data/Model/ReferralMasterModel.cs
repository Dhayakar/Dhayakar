using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WYNK.Data.Model
{
    public class ReferralMasterS
    {
        [Key]
        public string REFERRAL_CODE { get; set; }
        public string REFERRAL_NAME { get; set; }
        public string REFERRAL_ADDRESS1 { get; set; }
        public string REFERRAL_ADDRESS2 { get; set; }
        public string REFERRAL_ADDRESS3 { get; set; }
        public string FAX { get; set; }
        public string PHONE_NO { get; set; }
        public string EMAIL_ID { get; set; }
        public string CONTACT_PERSON { get; set; }
        public string REF_TYPE { get; set; }
    }
}
