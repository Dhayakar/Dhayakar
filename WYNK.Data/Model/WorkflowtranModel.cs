using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
  public class Workflowtran
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int Workflowmasterid { get; set; }
        public int Regtranid { get; set; }
        public Boolean Reception { get; set; }
        public Boolean Optometrist { get; set; }
        public Boolean Doctor { get; set; }
        public string Checkintime { get; set; }
        public string Checkouttime { get; set; }
        public DateTime Date { get; set; }
        public int CMPID { get; set; }
        public DateTime Createdutc { get; set; }
        public int Createdby { get; set; }
        public int Doctorid { get; set; }
        public Boolean status { get; set; }
    }
}
