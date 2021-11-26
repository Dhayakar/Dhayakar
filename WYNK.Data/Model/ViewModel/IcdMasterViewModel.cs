using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
   
    public class ICDMaster
    {


        public Icd_Master icdcodemasters { get; set; }
        public ICDExtenstion ICDExtenstion { get; set; }
        public SurgeryCostDetails SurgeryCostDetails { get; set; }

        public OneLine_Masters onelinemaster { get; set; }

        public ICollection<DoctorSpecialitydetails> DoctorSpecialitydetails { get; set; }
        public ICollection<ICDExtenstionDOC> ICDExtenstionDOC { get; set; }
        
    }

    public class DoctorSpecialitydetails
    {
        public string ParentDescription { get; set; }
        public int? DID { get; set; }
    }

    public class ICDExtenstionDOC
    {
       public int DOCID { get; set; }
        public int Doctorname { get; set; }
        public string DoctorSpeciality { get; set; }
        public decimal SurgeonCharges { get; set; }
    }
}
