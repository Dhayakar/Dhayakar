using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WYNK.Data.Model
{
    public class OcularComplaintsHistory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }

        public int OcularComplaintsID { get; set; }
        public string Description { get; set; }

        public bool ISOD { get; set; }

        public bool ISOS { get; set; }
        public bool ISOU { get; set; }
        public bool IsActive { get; set; }
        public DateTime RemovedOn { get; set; }
        
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }

        public string Reasons { get; set; }

        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

    }
}
