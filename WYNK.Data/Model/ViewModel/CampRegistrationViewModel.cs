using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
   
    public class CampRegistrationViewModel
    {
        public CampRegistration CampRegistration { get; set; }
        public CampRegistrationTran CampRegistrationTran { get; set; }
        public CampRegistrationExtension CampRegistrationExtension { get; set; }
        public string Companyname { get; set; }
        public ICollection<CampPatientKIN> CampPatientKIN { get; set; }
        public ICollection<CampPatientKINDetails> CampPatientKINDetails { get; set; }
        public ICollection<GETCampRegExtension> GETCampRegExtension { get; set; }
    }

    public class CampPatientKIN
    {
        public int ID { get; set; }
        public string Relationship { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string ContactNumber { get; set; }
        public string EmailID { get; set; }
    }

    public class GETCampRegExtension
    {
        public string UIN { get; set; }
        public int CMPID { get; set; }
        public bool Artificialeye { get; set; }
        public bool OD { get; set; }
        public bool OU { get; set; }
        public bool OS { get; set; }
    }

}

