using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class ICDMastersearchviemodel
    {
        public ICollection<IcdMastersearch> IcdMastersearch { get; set; }
    }

    public class IcdMastersearch
    {
        public string ICDGroup { get; set; }
        public string ICDCode { get; set; }
        public string Special { get; set; }
        public Boolean? IOLReqd { get; set; }
        public string ICDDescription { get; set; }

    }
}
