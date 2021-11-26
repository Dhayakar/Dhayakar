using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class CampDashboardViewModel
    {
        public ICollection<CampSearchDetail> CampSearchDetails { get; set; }
        public List<string> CampPatFootFallIDs { get; set; }
        public List<string> CampFttranRandomUniqueIDs { get; set; }
        public List<string> SurgeryIDs { get; set; }

    }


    public class CampSearchDetail 
    {
        public int SurgeryCount { get; set; }
        public string SurgeryDescription { get; set; }
        public List<string> CampFttranRandomUniqueID { get; set; }

    }





}
