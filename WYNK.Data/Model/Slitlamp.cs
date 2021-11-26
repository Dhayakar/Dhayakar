using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class SlitLamp
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public Int64 RunningID { get; set; }

        public int SlitLampID { get; set; }

        public int SlitLampLineItemID { get; set; }

        public string FindingID { get; set; }

        public Boolean IsOD { get; set; }

        public Boolean IsOS { get; set; }

        public Boolean IsOU { get; set; }

        public DateTime? RemovedDate { get; set; }

        public Boolean IsRemoved { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedUTC { get; set; }
        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedUTC { get; set; }
        public string Description { get; set; }
        public int? SlitProperty { get; set; }
        public string BasicDescriptionRE { get; set; }
        public string BasicDescriptionLE { get; set; }




    }
}


