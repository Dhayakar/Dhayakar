using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class Opinionmasterref
    {
        public OpinionMaster OpinionMaster { get; set; }
        public Registerationtrans RegistrationTran { get; set; }
        public OneLine_Masters OneLineMasters { get; set; }
        public Doctor_Master Doctormaster { get; set; }

        public string UIN { get; set; }

        public ICollection<OPINIONLIST> OPINIONDETAILS { get; set; }

    }


    public class OPINIONLIST
    {
        public string OPUIN { get; set; }
        public string PName { get; set; }
        public DateTime? DOV { get; set; }
        public Int16 OAge { get; set; }
        public string OSex { get; set; }
        public DateTime? FODate { get; set; }
        public string OStatus { get; set; }
        public string DOCTORID { get; set; }

        public DateTime sentdate { get; set; }

        public DateTime respondeddate { get; set; }


    }


}
