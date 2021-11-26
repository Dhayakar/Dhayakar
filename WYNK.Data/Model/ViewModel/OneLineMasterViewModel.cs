
using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class OneLineMasterViewModel
    {

        public OneLine_Masters OneLineMaster { get; set; }
        public SpecialityMaster SpecialityMaster { get; set; }
        public ICollection<OLMMaster> OLMaster { get; set; }
        public ICollection<STATE> state { get; set; }
        public ICollection<CITY> city { get; set; }
        public ICollection<LOCATION> location { get; set; }
        public List<detailss> detailss { get; set; }
        public string MastersName { get; set; }
        public string NV1 { get; set; }
        public string NV2 { get; set; }
        public decimal? Amountid { get; set; }
    }
}

public class detailss
{
    public List<string> Details { get; set; }

    public static implicit operator List<object>(detailss v)
    {
        throw new NotImplementedException();
    }
}
public class COUNTRY
{
    public string country { get; set; }
    public string state { get; set; }
    public string city { get; set; }
    public string location { get; set; }
}
public class STATE
{
    public string state { get; set; }
}
public class CITY
{
    public string city { get; set; }
}
public class LOCATION
{
    public string location { get; set; }
}
public class OLMMaster
{


    public int POLMID { get; set; }
    public string PDescription { get; set; }
    public int? PID { get; set; }
    public string PTag { get; set; }
    public string PCode { get; set; }
    public bool pIsActive { get; set; }
    
    public decimal? PAmount { get; set; }



}