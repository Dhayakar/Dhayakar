using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class FindingsMaster
    {


       
        public OneLine_Masters OneLineMaster { get; set; }
        public ICollection<info> info { get; set; }
    }

    


    public class info
    {
        public int OLMID { get; set; }
        public string ParentDescription { get; set; }
        public int OLMID1 { get; set; }
        public string ParentDescription1 { get; set; }
        public int OLMID2 { get; set; }
        public string ParentDescription2 { get; set; }

    }



}
