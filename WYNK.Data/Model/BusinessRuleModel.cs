using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
   public  class BusinessRule
    {
        [Key]
        public int ID { get; set; }
        public int ModuleID { get; set; }
        public string ModuleDescription { get; set; }
        public string BRBasedOn { get; set; }
        public int CMPID { get; set; }
        public Boolean IsActive { get; set; }
        public string ICDCODE { get; set; }
        public Boolean IsAllSurgery { get; set; }
        public Boolean IsTransacted { get; set; }
        public DateTime WEF { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
     

    }
}

  