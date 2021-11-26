using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class SurgeryViewModel
    {
        public SurgeryMaster SurgeryMaster { get; set; }
        public Surgery_Tran SurgeryTran { get; set; }
        public SurgeryDoctor SurgeryDoctor { get; set; }
        public Admission Admission { get; set; }
        public IntraOperative Intraoperative { get; set; }
        public PreOperative PreOperative { get; set; }


    }
    
}
