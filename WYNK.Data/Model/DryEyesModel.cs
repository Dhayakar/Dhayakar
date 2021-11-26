using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class DryEyes
    {
        [Key]
        public int ID { get; set; }
        public string FindingsID { get; set; }
        public int? NorTMH { get; set; }
        public string NorTMHtxt { get; set; }
        public int? DryTMH { get; set; }
        public string DryTMHtxt { get; set; }
        public int? TBUT { get; set; }
        public string TBUTtxt { get; set; }
        public int? NiTBUT { get; set; }
        public string NiTBUTtxt { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

    }
}












