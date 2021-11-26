using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WYNK.Data.Model
{
   public class MessageTemplate
    {
        [Key]
        public int ID { get; set; }
        public string MsgTemplateName { get; set; }
        public string SMSMsgDescription { get; set; }
        public string EMAILSubject { get; set; }
        
        public string EMAILMsgDescription { get; set; }
        public string WHATSAPPMsgDescription { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int CMPID { get; set; }
    }
}
