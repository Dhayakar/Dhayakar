using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class Brand
    {
        [Key]
        public int ID { get; set; }
        public string Description { get; set; }
        public string BrandType { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public Boolean IsActive { get; set; }
        public int cmpID { get; set; }
        public Boolean IsDeleted { get; set; }

    }
}
