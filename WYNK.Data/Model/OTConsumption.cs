using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class OTConsumption
    {
        [Key]
        public int ID { get; set; }
        public int TransactionID { get; set; }
        public int SMID { get; set; }
        public DateTime Date { get; set; }
        public int OTHID { get; set; }
        public int Storeid { get; set; }
    //    public string OT_Notes { get; set; }
        public string UIN { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy  { get; set; }
        public int?  UpdatedBy  { get; set; }
    }
}
