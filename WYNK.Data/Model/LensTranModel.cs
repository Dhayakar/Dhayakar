using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WYNK.Data.Model
{
   public class LensTransModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int LMID { get; set; }
        public string LensOption { get; set; }
        public string Index { get; set; }
        public string Model { get; set; }
        public string Size { get; set; }
        public string Colour { get; set; }
        public int Brand { get; set; }
        public decimal Prize { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int? UpdatedBy { get; set; }
        public int CreatedBy { get; set; }
        public decimal? CESSAmount { get; set; }
        public decimal? ADDCESSAmount { get; set; }
        public int? UOMID { get; set; }
        public string HSNNo { get; set; }
    }
}
