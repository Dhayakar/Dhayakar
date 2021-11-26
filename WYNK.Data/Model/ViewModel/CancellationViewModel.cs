using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class CancellationViewM
    {
        public ICollection<getPatientcancelledDet> getPatientcancelledDet { get; set; }
        public ICollection<getDoctorcancelledDet> getDoctorcancelledDet { get; set; }
        public ICollection<getpatientDoctorcancelledDet> getpatientDoctorcancelledDet { get; set; }

        public int frontdeskcount {get;set;}

        public string Companyname { get; set; }

        public string Fromdates { get; set; }

        public string Todated { get; set; }

        public ICollection<Doctorcount> Doctorcounts { get; set; }
        public ICollection<Optocount> Optocounts { get; set; }
        public ICollection<Visioncount> Visioncounts { get; set; }

    }


    public class Doctorcount
    {

        public DateTime Date { get; set; }
        public int Frontdeskcacelled { get; set; }
        public int Doccount { get; set; }
        public int Optocount { get; set; }
        public int Vsioncount { get; set; }

    }

    public class Optocount
    {

        public int UIN { get; set; }

    }
    public class Visioncount
    {

        public int UIN { get; set; }

    }


    public class getPatientcancelledDet
    {

        public string UIN { get; set; }
        public string FName { get; set; }
        public string MName { get; set; }
        public string lName { get; set; }

        //public string PatientName { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public DateTime CancelledDateTime { get; set; }
        public string CancelledReasons { get; set; }
        public string Doctorname { get; set; }
    }

    public class getDoctorcancelledDet
    {

        public string UIN { get; set; }
        public string PatientName { get; set; }

        public string Age { get; set; }
        public string Gender { get; set; }
        public DateTime? CancelledDateTime { get; set; }
        public string CancelledReasons { get; set; }

    }
    public class getpatientDoctorcancelledDet
    {

        public string UIN { get; set; }
        public string PatientName { get; set; }
        public string Gender { get; set; }
        public DateTime patientCancelledDateTime { get; set; }
        public DateTime? DoctorCancelledDateTime { get; set; }
        public string CancelledReasons { get; set; }

    }
}
