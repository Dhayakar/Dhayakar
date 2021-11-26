using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WYNK.Data.Model
{
   public class Workflowmaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int CMPID { get; set; }
        public string Description { get; set; }
        public int TrayOrder { get; set; }
        public DateTime Createdutc { get; set; }
        public int Createdby { get; set; }
    }
}
