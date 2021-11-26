using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WYNK.Data.Model
{
   public class Consentextension
    {
        [Key]
        public int ID { get; set; }
        public int CMPID { get; set; }
        public string ConsentDescription { get; set; }
        public string Consentpath { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedUTC { get; set; }
        public int Createdby { get; set; }
    }
}
