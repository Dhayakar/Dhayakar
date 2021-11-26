using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
   public class University
    {


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UniversityCode { get; set; }
        public int CMPID { get; set; }
        public string UniversityDescription { get; set; }
        public int LocationId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime ?UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int ?UpdatedBy { get; set; }
    }
}
