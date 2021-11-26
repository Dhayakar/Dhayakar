using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WYNK.Data.Model
{
   public class Connectionstring
    {
        [Key]
        public int ID { get; set; }
        public string Databasename { get; set; }
        public int CMPID { get; set; }
        public string Connectionstringpath { get; set; }
    }
}
