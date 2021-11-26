using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class SurgeryAssignedTran

    {
        [Key]


        public int ID { get; set; }
        public string SAID { get; set; }
        public string SurgeryID { get; set; }
        public int DoctorID { get; set; }
        public bool IsCancelled { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

    }
}




