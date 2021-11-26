using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class QualificationMasterViewModel
    {
        public Qualification qualification { get; set; }
        public QualificationExt qualificationExt { get; set; }
        public ICollection<getdetails> getdetail { get; set; }
        public ICollection<getdetails1> getdetailss { get; set; }
        public ICollection<getdetailsLoc> getdetailsloc { get; set; }
        public int uid { get; set; }
        public class getdetails
        {
            public string Description;
            public string QualExtDesc;
            public string QualExtAns;
            public string idd;
            public bool IsActive;
            public int ide;
        }
        public class getdetails1
        {
            public string Description;
            public int idd {get; set; }
    }
        public class getdetailsLoc
        {
            internal IEnumerable<getdetailsLoc> getdetailsloc;

            public string location { get; set; }
        }
    }
}