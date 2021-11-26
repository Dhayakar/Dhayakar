using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
   public  class Icd_Master
    {
        [Key]
        public string ICDCODE { get; set; }
        public string ICDDESCRIPTION { get; set; }
        public string ICDTYPECODE { get; set; }
        public int ICDGroup { get; set; }
        public int? SpecialityCode { get; set; }
        public Boolean? IsIOLReqd { get; set; }
        public Boolean IsActive { get; set; }
        public Boolean? IsDeleted { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
    }


}



