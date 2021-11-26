using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
   public class Configure
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }        
        public int? CMPID { get; set; }
        public string RRDescription { get; set; }
        public Int16 RRAdvDays { get; set; }
        public Int16 FrequencyperDay { get; set; }
        public string HostEmailID { get; set; }
        public string HostPassword { get; set; }
        public string Phonenumber { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public Boolean SendSMS { get; set; }
        public Boolean SendEmail { get; set; }

    }
}


