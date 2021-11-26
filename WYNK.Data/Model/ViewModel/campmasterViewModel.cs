using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
  public class campmasterViewModel
    {
        public Camporganization_Model Camp_OrganizationModel { get; set; }
        public ICollection<campognizationdetails> campognizationdetails { get; set; }
        public ICollection<campmasterdetails> campmasterdetails { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationID { get; set; }
        
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Location { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ContactPerson { get; set; }
        public string Phone { get; set; }
        public string EMailID { get; set; }
        public string Website { get; set; }
        public string OrganizationType { get; set; }
        public string IsActive { get; set; }

        public string CampMastreID { get; set; }
        public string CampMastreName { get; set; }
        public DateTime CampMastrefrom { get; set; }
        public DateTime CampMastreto { get; set; }
        public string CampMastreorganisedby { get; set; }
        public string CampMastrestate { get; set; }
        public string CampMastrecity { get; set; }
        public string CampMastrelocation { get; set; }
        public string CampMastrecountry { get; set; }



    }
    public class campmasterdetails
    {
        public int CampMastreID { get; set; }
        public string CampMastreName { get; set; }
        public string CampMastrefrom { get; set; }
        public string CampMastreto { get; set; }
        public string CampMastreorganisedby { get; set; }
        public string CampMastrestate { get; set; }
        public string CampMastrecity { get; set; }
        public string CampMastrelocation { get; set; }
        public string CampMastrecountry { get; set; }
        public bool Isactive { get; set; }
        public string Normalactive { get; set; }
    }
    public class campognizationdetails
    {
        public int OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Location { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ContactPerson { get; set; }
        public Int64 Phone { get; set; }
        public string EMailID { get; set; }
        public string Website { get; set; }
        public bool Isactive { get; set; }
        public string OrganizationType { get; set; }
        public string Normalactive { get; set; }
    }

    }
