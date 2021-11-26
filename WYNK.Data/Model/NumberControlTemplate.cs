using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class Number_Control_Template
    {
        [Key]
        public int ID { get; set; }
        public int TransactionID { get; set; }    
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public string Description { get; set; }
        public Boolean IsActive { get; set; }
        public int RunningNumber { get; set; }
        
    }
}

