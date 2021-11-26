using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace WYNK.Data.Model
{
    public class UomMaster
    {
        [Key]
        public int id { get; set; }
        public string Description { get; set; }



    }
}
