using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class SscResponse
    {
        [Key]
        public int SSCRID { get; set; }
        public int SSCID { get; set; }
        public int CmpID { get; set; }
        public int OLMID { get; set; }
        public string SAID { get; set; }
        public int Response { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

    }
}