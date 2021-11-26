using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
   public  class UniversityViewModel
    { 
            public University University_M { get; set; }
            public UniversityExt UniversityExt_M { get; set; }
           public ICollection<getdetails> getdetail { get; set; }
          public ICollection<getdetailss> getdetails1 { get; set; }
        public ICollection<getdetailscollege> getdetailscol { get; set; }
        public ICollection<getdetailsloc> getdetailsloc { get; set; }
        public ICollection<getdetailsuniversity> getdetails { get; set; }
        public ICollection<getdetailstab> getdetailstab { get; set; }
        public int uid { get; set; }
    }
    public class getdetailstab
    {
        public string loc1;
        public string university;
        public string college;
    }
    public class getdetails
    {

        public string universityDescription;
        public int id;
    }
    public class getdetailss
    {
        public string universityDescription1;
        public int id1;
    }
    public class getdetailscollege
    {
        public string college;
        
    }
    public class getdetailsloc
    {
        public string location;
        public int id;
    }
    public class getdetailsuniversity
    {
        public string location;
        public string location1;
        public string university;
        public string college;
    }
}
