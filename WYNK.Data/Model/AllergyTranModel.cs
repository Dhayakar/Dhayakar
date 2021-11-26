using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class AllergyTran
    {
        [Key]
        public int ID { get; set; }
        public int CmpID { get; set; }
        public string UIN { get; set; }
        public int Description { get; set; }
        public int Type { get; set; }
        public DateTime FromUTC { get; set; }
        public Boolean IsActive { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int RegID { get; set; }
        public string Period { get; set; }
        public int Since { get; set; }

    }
}
