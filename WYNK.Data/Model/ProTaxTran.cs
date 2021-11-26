using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WYNK.Data.Model
{
   public class ProTaxTran
    {
        [Key]
        public int ID { get; set; }
        public int PTID { get; set; }
        public decimal From { get; set; }
        public decimal To { get; set; }
        public decimal PTAmount { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }


    }
}
