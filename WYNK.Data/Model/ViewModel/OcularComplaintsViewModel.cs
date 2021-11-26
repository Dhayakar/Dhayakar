using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{


    public class OcularComplaintsViewModel
    {
        public string familyhistory { get; set; }
        public string Allergy { get; set; }

        public ICollection<OcularComplaintsNew> OcularComplaintsNew { get; set; }
        public ICollection<systemicconditionsNew> systemicconditionsNew { get; set; }
        public ICollection<OcularComplaintsWithReasons> listofcomplaints { get; set; }
        public ICollection<GetDeletedOcularComplaintsNew> GetDeletedOcularComplaintsNew { get; set; }
        public ICollection<CurrentMedication> CurrentMedications { get; set; }
        public string Userid { get; set; }
        public string Role { get; set; }
        public string Registrationtranid { get; set; }

        public string Submitstatus { get; set; }
        public string Messagestatus { get; set; }
        public string CMPID { get; set; }
        public string usid { get; set; }
        public OneLine_Masters OneLineMaster { get; set; }
    }

    public class OcularComplaintsWithReasons
    {

        public int ID { get; set; }
        public string Description { get; set; }

        public int TransID { get; set; }

        public bool ISOD { get; set; }

        public string Reasons { get; set; }

        public string Actions { get; set; }

        public bool ISOS { get; set; }
        public bool ISOU { get; set; }

        public string FromDate { get; set; }
        public string Totalmonths { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }

        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }



    }


    public class OcularComplaintsNew
    {

        public int ID { get; set; }
        public string Description { get; set; }
        public bool ISOD { get; set; }
        public string Reasons { get; set; }
        public string Actions { get; set; }
        public bool ISOS { get; set; }
        public bool ISOU { get; set; }
        public DateTime FromDate { get; set; }
        public string Totalmonths { get; set; }
        public string CMPID { get; set; }
        public string Doctoruserid { get; set; }
        public string Remarks { get; set; }
    }


    public class systemicconditionsNew
    {

        public int ID { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public string Reasons { get; set; }
        public string Actions { get; set; }
        public DateTime FromDate { get; set; }
        public string Totalmonths { get; set; }
        public string CMPID { get; set; }
        public string Doctoruserid { get; set; }
    }

    public class GetOcularComplaintsNew
    {

        public int ID { get; set; }
        public string Description { get; set; }
        public bool ISOD { get; set; }
        public string Reasons { get; set; }
        public string Actions { get; set; }
        public bool ISOS { get; set; }
        public bool ISOU { get; set; }
        public DateTime FromDate { get; set; }
        public string Totalmonths { get; set; }
        public string CMPID { get; set; }
        public string Doctoruserid { get; set; }
    }

    public class GetDeletedOcularComplaintsNew
    {

        public int ID { get; set; }
        public string Description { get; set; }
        public bool ISOD { get; set; }
        public string Reasons { get; set; }
        public bool ISOS { get; set; }
        public bool ISOU { get; set; }
        public DateTime? DeletedDate { get; set; }

    }
}
