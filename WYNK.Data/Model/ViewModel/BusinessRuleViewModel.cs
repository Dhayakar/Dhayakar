using System;
using System.Collections.Generic;
using System.Text;


namespace WYNK.Data.Model.ViewModel
{
    public class BusinessRuleViewModel
    {

        public BusinessRule BusinessRule { get; set; }
        public BusinessRuleTran BusinessRuleTran { get; set; }
        public ICollection<BusinessRT> BusinessRT { get; set; }
        public ICollection<BusinessRT1> BusinessRT1 { get; set; }
        public string ModuleDescription { get; set; }
      
        
        public ICollection<IcdSPYarray> IcdSPYarray { get; set; }
        public ICollection<icdspyModuleDescription> icdspyModuleDescription { get; set; }

    }
    public class icdspyModuleDescription
    {
        public string ICDCODE { get; set; }
        

    }
    public class IcdSPYarray
    {
        public string ICDSPYDescription { get; set; }
        public int id { get; set; }
       
    }
    public class BusinessRT
    {
        public int BRID { get; set; }
        public int From { get; set; }
        public int TO { get; set; }
        public decimal Amount { get; set; }
        public int NODays { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public DateTime? EffectiveDate { get; set; }
    }


    public class BusinessRT1
    {
        public DateTime? WEF { get; set; }
        public int BRID { get; set; }
        public int? From { get; set; }
        public int? TO { get; set; }
        public decimal Amount { get; set; }
        public int NODays { get; set; }
        public int CountOfRegTran { get; set; }

    }
}
