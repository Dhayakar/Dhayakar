using System;
using System.Collections.Generic;
using System.Text;


namespace WYNK.Data.Model.ViewModel
{
    public class LocationMasterViewModel
    {
        public LocationMaster LocMaster { get; set; }

        public ICollection<Country> Country { get; set; }
        public ICollection<State> State { get; set; }
        public ICollection<City> City { get; set; }
        public ICollection<location> location { get; set; }
        public ICollection<statedetails> statedetails { get; set; }
        public ICollection<citydetails> citydetails { get; set; }
        public ICollection<loactiondetails> loactiondetails { get; set; }
        //public int CID { get; set; }
    }




    public class Country
    {
        public int ID { get; set; }
        public string ParentDescription { get; set; }
    }


    public class State
    {
        public int ID { get; set; }
        public string ParentDescriptionstate { get; set; }
    }


    public class City
    {
        public int ID { get; set; }
        public string ParentDescriptioncity { get; set; }
    }

    public class location
    {
        public int ID { get; set; }
        public string ParentDescriptionlocation { get; set; }
        public int? Pincode { get; set; }
    }

    public class statedetails
    {
        public string Countryname { get; set; }
        public string Statename { get; set; }
        public int? StateID { get; set; }
        public int CountryID { get; set; }

    }

    public class citydetails
    {
        public string Countryname { get; set; }
        public string Statename { get; set; }
        public string Cityname { get; set; }
        public int? CityID { get; set; }
        public int CountryID { get; set; }
        public int? StateID { get; set; }
    }

    public class loactiondetails
    {
        public string Countryname { get; set; }
        public string Statename { get; set; }
        public string Cityname { get; set; }
        public int? CityID { get; set; }
        public int CountryID { get; set; }
        public int? StateID { get; set; }
        public string Loactionname { get; set; }
        public int? Pincode { get; set; }
        public int LoactionID { get; set; }
        public Boolean disab { get; set; }
    }

}
