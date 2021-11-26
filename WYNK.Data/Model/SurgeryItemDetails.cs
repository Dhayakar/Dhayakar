using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WYNK.Data.Model
{
    public class surgeryitemdetails
    {
        [Key]
        public int id { get; set; }
        public string surgdescription { get; set; }
        public int amount { get; set; }
        public int discount { get; set; }
        public int totalamt { get; set; }



    }
}
