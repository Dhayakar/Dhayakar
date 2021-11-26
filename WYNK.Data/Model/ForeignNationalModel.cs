using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
   public class ForeignNational
    {
        [Key]
        public int ID { get; set; }
        public bool IsForeignNational { get; set; }
        public decimal? NormalFee { get; set; }
        public decimal? SuperSpecialityFee { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int CmpID { get; set; }
        public bool IsActive { get; set; }



    }
}
