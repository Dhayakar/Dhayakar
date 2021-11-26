using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class SurgeryHistory
    {
        [Key]
        public int ID { get; set; }
        public int Cmpid { get; set; }
        public string SurgeryID { get; set; }
        public string UIN { get; set; }
        public DateTime? DateofSurgery { get; set; }
        public string TypeofSurgery { get; set; }
        public string SurgeonName { get; set; }
        public string HospitalOrClinic { get; set; }
        public string Eye { get; set; }
        public Boolean IsDeleted { get; set; }
        public string RemovedReason { get; set; }
        public string Remarks { get; set; }
        public string SurgeryDone { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
