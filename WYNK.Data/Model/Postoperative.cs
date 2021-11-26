using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class Postoperative
    {
        [Key]
        public int ID { get; set; }
        public int SurgeryID { get; set; }
        public DateTime PostOperativeDate { get; set; }
        public string ComplicationDetails { get; set; }
        public string TreatmentAdvive { get; set; }
        public DateTime? ReviewDate { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }


    }
}




