using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class OperationTheatre
    {
        [Key]
        public int OTID { get; set; }
        public string OTDescription { get; set; }
        //public string OTType { get; set; }
        public string OTAddress1 { get; set; }
        public string OTAddress2 { get; set; }
        public string OTAddress3 { get; set; }
        public string OTLocation { get; set; }
        public decimal? OTCharge { get; set; }
        public decimal? OTDiscount { get; set; }
        public decimal? GST { get; set; }
        public decimal? CGST { get; set; }
        public decimal? SGST { get; set; }
        public Boolean IsActive { get; set; }
        public Boolean IsDeleted { get; set; }

        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int CMPID { get; set; }

    }
}












