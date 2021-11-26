using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class OrbitalDisorders
    {
        [Key]
        public int ID { get; set; }
        public string FindingsID { get; set; }
        public int? PositionofGlobeRE { get; set; }
        public int? PositionofGlobeLE { get; set; }
        public string BaseReadingRE { get; set; }
        public string BaseReadingLE { get; set; }
        public string ReadingsfortheeyeRE { get; set; }
        public string ReadingsfortheeyeLE { get; set; }
        public string FingerInsinuationTestRE { get; set; }
        public string FingerInsinuationTestLE { get; set; }
        public int? LocationRE { get; set; }
        public int? LocationLE { get; set; }
        public int? ShapeRE { get; set; }
        public int? ShapeLE { get; set; }
        public int? TextureRE { get; set; }
        public int? TextureLE { get; set; }
        public int? SizeRE { get; set; }
        public int? SizeLE { get; set; }
        public string Remarks { get; set; }
        public int? GradeofThyroidEyeDiseaseLE { get; set; }
        public int? GradeofThyroidEyeDiseaseRE { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public string LocationTextRE { get; set; }
        public string LocationTextLE { get; set; }
        public string ShapeTextRE { get; set; }
        public string ShapeTextLE { get; set; }
        public string TextureTextRE { get; set; }
        public string TextureTextLE { get; set; }
        public string SizeTextRE { get; set; }
        public string SizeTextLE { get; set; }
        public string SizeTextREVer { get; set; }
        public string SizeTextLEVer { get; set; }
        public int? FingerInsinuationRE { get; set; }
        public int? FingerInsinuationLE { get; set; }


    }
}
