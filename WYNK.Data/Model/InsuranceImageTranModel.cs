using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
   public class InsuranceImageTran
    {
        [Key]
        public int ID { get; set; }
        public int PAINSID { get; set; }
        public string InsuranceImagePath { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int CmpID { get; set; }
        

    }
}
