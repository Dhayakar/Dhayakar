using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WYNK.Data.Model
{
    public class LensTranModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string LMID { get; set; }
        public string Description { get; set; }
        public string Index { get; set; }
        public string Model { get; set; }
        public string Size { get; set; }
        public string Colour { get; set; }
        public int Brand { get; set; }
        public decimal Prize { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public decimal? CESSAmount { get; set; }
        public decimal? ADDCESSAmount { get; set; }
        public int? UOMID { get; set; }
        public string HSNNo { get; set; }
        public decimal? GST { get; set; }
        public int? TaxID { get; set; }
        public int? FrameShapeID { get; set; }
        public int? FrameTypeID { get; set; }
        public int? FrameWidthID { get; set; }
        public int? FrameStyleID { get; set; }
        public Boolean IsActive { get; set; }
        public string Sph { get; set; }
        public string Cyl { get; set; }
        public string Axis { get; set; }
        public string Add { get; set; }
        public decimal Costprice { get; set; }
        public Boolean Sptaxinclusive { get; set; }
    }
}
