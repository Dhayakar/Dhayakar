using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
   public class PatientQueueViewModel
    {
       public ICollection<Patientassignquedata> Patientassignquedata { get; set; }
        public int TotalNewCOuntdata { get; set; }
        public int TotalAwaitingforoptometristCOuntdata { get; set; }
    }

    public class subforms
    {
        public string UIN { get; set; }
        public string Name { get; set; }
        public string MName { get; set; }
        public string Lname { get; set; }
        public Boolean Reception { get; set; }
        public Boolean optometrist { get; set; }
        public string checintime { get; set; }
        public string Gender { get; set; }
        public string Age { get; set; }
        public string checouttime { get; set; }
        public DateTime regdatetime { get; set; }
        public decimal? fees { get; set; }
        public string Patientstatus { get; set; }
        public string Doctorname { get; set; }
        public ICollection<Patientcheckdetails> Patientcheckdetails { get; set; }
    }

    public class Patientcheckdetails
    {
        public string checkintime { get; set; }
        public string checouttime { get; set; }
        public string waitingtime { get; set; }
        public string date { get; set; }
        public DateTime Createdutc { get; set; }
        public string Role { get; set; }

    }

    public class Patientassignquedata
    {
        public string Description { get; set; }
        public ICollection<subforms> subforms { get; set; }
    }

}
