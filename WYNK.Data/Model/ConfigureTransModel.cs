using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class ConfigureTrans
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int ConfigurationID { get; set; }
        public string Frequencytime { get; set; }


        public bool? NotifyDoctor_SMS { get; set; }
        public bool? NotifyDoctor_Mail { get; set; }
        public bool? NotifyPatient_SMS { get; set; }
        public bool? NotifyPatient_Mail { get; set; }
        public bool? NotifyPatient_Whatsapp { get; set; }
        public bool? NotifyDoctor_Whatsapp { get; set; }
        
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
    }
}


