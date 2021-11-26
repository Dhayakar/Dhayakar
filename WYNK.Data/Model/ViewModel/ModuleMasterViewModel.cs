using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class ModuleMasterViewM
    {
        public ModuleMaster ModuleMaster { get; set; }
        public string Description { get; set; }
        public string Tag { get; set; }
        public int CMPUID { get; set; }
        public ICollection<getparentName> getparentName { get; set; }
        public ICollection<Maindetails> Maindetails { get; set; }
    }


    public class Maindetails
    {
        public int Value { get; set; }
        public string Text { get; set; }
    }



    public class getparentName
    {

        public int ModuleID1 { get; set; }
        public string ModuleDescription1 { get; set; }
        public string ModuleType1 { get; set; }
        public decimal ParentModuleid2 { get; set; }
        public string ParentModule1 { get; set; }
        public string Parentmoduledescription1 { get; set; }

    }

}
