using System;
using System.Collections.Generic;
using System.Text;


namespace WYNK.Data.Model.ViewModel
{
    public class CompanyMasterView
    {
        public Companymas companyMaster { get; set; }
        //  public ModuleMasterTemplate MMT { get; set; }

        public ICollection<getCompModules> MMT { get; set; }
        public ICollection<getCompName> getCompName { get; set; }
        public ICollection<getCompDetails> getCompDetails { get; set; }

        public ICollection<getCompModuless> getCompModuless { get; set; }
        public ICollection<getNumbercontrolModules> getNumbercontrolModules { get; set; }


        public string company { get; set; }
        public int ParentID { get; set; }
        public string companygroup { get; set; }
        public string CheckoutTime { get; set; }
        public string BufferTime { get; set; }

    }


    public class getCompModuless
    {
        public string ModuleDesc { get; set; }
    }



    public class getNumbercontrolModules
    {
        public string Desc { get; set; }
        public int RunningNumber { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
    }

    public class getCompModules
    {
        public string ModuleName { get; set; }
        public int ModuleD { get; set; }

    }
    public class getCompName
    {
        public string companyName { get; set; }
        public string CompanyCategory { get; set; }
        public int CmpID { get; set; }
        public string Location { get; set; }
        public int ParentID { get; set; }
        
    }
    public class getCompDetails
    {
        public int CmpID { get; set; }
        public string CompanyName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string LocationName { get; set; }
        public int LocationID { get; set; }
        public string GSTNo { get; set; }
        public string ContactPerson { get; set; }
        public int ParentID { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string EmailID { get; set; }
        public string Website { get; set; }
        public string CompanyGroup { get; set; }
    }





}
