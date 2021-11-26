using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class SurgerySafetyCheckList
    {
        [Key]
        public int SSCID { get; set; }
        public int CmpID { get; set; }
        public string Question { get; set; }
        public string Questiontowhom { get; set; }
        public int SSCGroupDescription { get; set; }
        public int? DefaultValue { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

    }
}