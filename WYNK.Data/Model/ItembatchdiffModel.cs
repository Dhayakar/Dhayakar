using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class itembatchdiff
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Sno { get; set; }
        public int Cmpid { get; set; }
        public string Smid { get; set; }
        public int Itemid { get; set; }
        public string ItembatchId { get; set; }
        public string itembatchno { get; set; }
        public string serialno { get; set; }
        public int Diffquantity { get; set; }
        public string reasons { get; set; }
        public int issuestoreid { get; set; }
        public int issueuserid { get; set; }
        public DateTime issuedateandtime { get; set; }
        public int recstoreid { get; set; }
        public int recuserid { get; set; }
        public DateTime recdateandtime { get; set; }
    }
}
