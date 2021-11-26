using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class ColorVision
    {
        [Key]
        public int ID { get; set; }
        public int RegistrationTranID { get; set; }
        public string Description { get; set; }
        public string Ocular { get; set; }
        public string CV_normal { get; set; }
        public string CV_defective { get; set; }
        public string Desc_Text { get; set; }
        public DateTime Date { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

    }
}
















