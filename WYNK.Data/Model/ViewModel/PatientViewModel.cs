using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class PatientViewModel
    {
        public RegistrationTran_Master RegistrationTran_Master { get; set; }


        public ICollection<PatientList> PatientList { get; set; }

        public int total { get; set; }



    }
}
public class PatientList
{
    public string UIN { get; set; }
    public string Name { get; set; }
    public DateTime DateofVisit { get; set; }
    public string Gender { get; set; }
    public string Address1 { get; set; }
    public string PatientVisitType { get; set; }
    public string Phone { get; set; }
    public DateTime FromDate { get; set; }

    public DateTime RDate { get; set; }
    public DateTime ToDate { get; set; }
}
