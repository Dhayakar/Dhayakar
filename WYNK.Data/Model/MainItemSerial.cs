using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class MainItemSerial
    {

        [Key]
        public int ID { get; set; }
        public int ItemID { get; set; }
        public int SerialNo { get; set; }
        public string GRNNo { get; set; }
        public int TC { get; set; }
        public string IssueNo { get; set; }
        public DateTime? IssueDate { get; set; }
        public int? IssueTC { get; set; }
        public Boolean IsCancelled { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int? StoreID { get; set; }
    }
}
