using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class FundusImage
    {
        [Key]


        public int ID { get; set; }
        public string FindingsID { get; set; }
        public string Eye { get; set; }
        public string FundusODImagePath { get; set; }
        public string FundusOSImagePath { get; set; }
        public DateTime UploadedOn { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public string UIN { get; set; }

    }
}