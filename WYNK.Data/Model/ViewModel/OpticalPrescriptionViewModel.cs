using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class OpticalPrescriptionview
    {

        public ICollection<opfinalprescri> opfinalprescri { get; set; }
        public ICollection<opticprescription> opticprescription { get; set; }
        public ICollection<opticprescriptionprint> opticprescriptionprint { get; set; }

        public string Address { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string phone { get; set; }
        public string web { get; set; }
        public string Compnayname { get; set; }
        public string Description { get; set; }
        public string UIN { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string docname { get; set; }
        public string docreg { get; set; }

    }

    public class opfinalprescri
    {
        public string UIN { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public DateTime PrescriptionDate { get; set; }
        public string DateofBirth { get; set; }
        public int? RegistrationTranID { get; set; }
        public string Doctorname { get; set; }
        public string Role { get; set; }
    }

    public class opticprescription
    {
        public int? Type { get; set; }
        public string Ocular { get; set; }
        public string DistSph { get; set; }
        public string NearCyl { get; set; }
        public string PinAxis { get; set; }
        public string Add { get; set; }
        public string Remarks { get; set; }
        public string PD { get; set; }
        public string MPDOD { get; set; }
        public string MPDOS { get; set; }

    }

    public class opticprescriptionprint
    {
        public int? Type { get; set; }
        public string Ocular { get; set; }
        public string DistSph { get; set; }
        public string NearCyl { get; set; }
        public string PinAxis { get; set; }
        public string Add { get; set; }
        public string Remarks { get; set; }
        public string PD { get; set; }
        public string MPDOD { get; set; }
        public string MPDOS { get; set; }
    }



}


