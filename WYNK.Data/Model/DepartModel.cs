using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WYNK.Data.Model
{
    public class Depart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int CMPID { get; set; }
        public string Description { get; set; }
        public string DepartmentIncharge { get; set; }
        public string DepartmentLocation { get; set; }
        public bool IsActive { get; set; }
        public Boolean IsDeleted { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }


    }
}
