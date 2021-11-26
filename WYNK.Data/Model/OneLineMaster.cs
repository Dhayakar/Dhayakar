using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class OneLine_Masters
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OLMID { get; set; }
        public string ParentDescription { get; set; }
        public int? ParentID { get; set; }
        public string ParentTag { get; set; }
        public bool IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public decimal? Amount { get; set; }
        public string Code { get; set; }


    }

    public class COUN
    {
        public int id { get; set; }
    }
}
