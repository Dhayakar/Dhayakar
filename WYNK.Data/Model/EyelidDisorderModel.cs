using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class EyelidDisorder
    {
        [Key]
        public int ID { get; set; }
        public string FindingsID { get; set; }
        public string UpperRE { get; set; }
        public string UpperLE { get; set; }
        public string LowerRE { get; set; }
        public string LowerLE { get; set; }
        public string BothRE { get; set; }
        public string BothLE { get; set; }
        public string PosLidRE { get; set; }
        public string PosLidLE { get; set; }
        public string LashesRE { get; set; }
        public string LashesLE { get; set; }
        public Decimal? IntDstRE { get; set; }
        public Decimal? IntDstLE { get; set; }
        public Decimal? LPSRE { get; set; }
        public Decimal? LPSLE { get; set; }
        public string POSRE { get; set; }
        public string POSLE { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

    }
}

