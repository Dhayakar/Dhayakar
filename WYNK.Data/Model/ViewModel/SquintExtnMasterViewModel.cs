
using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class SquintExtnMasterViewModel
    {

        public  Squint_ExtnMaster SquintExtnMaster { get; set; }
        public SpecialityMaster SpecialityMaster { get; set; }
        public ICollection<SQEMaster> SQMaster { get; set; }
   
        public string MastersName { get; set; }

    }

    public class SQEMaster
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public string PTag { get; set; }
        public int? PID { get; set; }
        public bool IsActive { get; set; }
        public string IsActive1 { get; set; }
        
    }
}




