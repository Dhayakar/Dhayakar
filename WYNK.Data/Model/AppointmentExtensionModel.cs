using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WYNK.Data.Model
{
  public class AppointmentExtension
    {
        [Key]
        public int ID { get; set; }
        public string RandomUniqueID { get; set; }
        public int AppointmentID { get; set; }
        public int CMPID { get; set; }
        public string Path { get; set; }
        public int Count { get; set; }
    }
}
