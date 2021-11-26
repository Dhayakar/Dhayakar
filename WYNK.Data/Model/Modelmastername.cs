using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class Modelmastername
    {
        [Key]

        public int ModuleID { get; set; }
        public string ModuleDescription { get; set; }
        public string ModuleType { get; set; }
        public decimal ParentModuleid { get; set; }
        public string Parentmoduledescription { get; set; }
        public int CmpID { get; set; }


    }


}

