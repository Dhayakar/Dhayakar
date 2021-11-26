using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class SurgeryMasterViewModel
    {
        public Registration_Master RegistrationMaster { get; set; }
        public RegistrationTran_Master RegistrationTranMaster { get; set; }
        public OneLine_Masters OneLineMaster { get; set; }
        public Doctor_Master DoctorMaster { get; set; }
        public Icd_Master IcdMaster { get; set; }
        public ICollection<surgeryitemdetails> surgeryalldetails { get; set; }
    }
    
}
