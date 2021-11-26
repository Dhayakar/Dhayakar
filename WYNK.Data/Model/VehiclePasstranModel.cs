

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class VehiclePasstran
    {
        [Key]
        public int ID { get; set; }
        public string VecpassID { get; set; }
        public string Make { get; set; }
        public string VehicleNo { get; set; }
        public string Type { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

    }
}








