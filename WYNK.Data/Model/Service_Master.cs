using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class Service_Master
    {
        [Key]
        public int ID { get; set; }
        public int CMPID { get; set; }
        public int ParentID { get; set; }
        public string Description { get; set; }
        public string Tag { get; set; }
        public DateTime CreatedUTC { get; set; }        
        public int CreatedBy { get; set; }
    }
}
