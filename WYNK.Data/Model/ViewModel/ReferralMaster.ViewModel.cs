using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class Referral_Master
    {
        public ReferralMasterS ReferralMaster { get; set; }
        public PatientRegistrationMaster Master { get; set; }
        public ReferralPatientHDRDataModel Referral { get; set; }
    }
}
