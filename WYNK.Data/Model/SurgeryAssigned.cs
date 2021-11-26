using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class SurgeryAssigned
    {
        [Key]


        public int SAID { get; set; }
        public string SurgeryID { get; set; }
        public string UIN { get; set; }
        public int CmpID { get; set; }
        public DateTime SurgeryDate { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public int? OTBID { get; set; }
        public int Status { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public bool IsRescheduled { get; set; }
        public int? Cancel_Rescheduled_description { get; set; }
        public bool IsCancelled { get; set; }
        public int? FindingsExtID { get; set; }
        public string ReasonForCancellation { get; set; }
        public bool? IsSurgeryCompleted { get; set; }
        public DateTime? SurgeryCompletedDate { get; set; }
        public bool? IsResurgeryneeded { get; set; }
        public string ReasonForResurgery { get; set; }
        public string Admid { get; set; }
        public string RandomUniqueID { get; set; }
    }
}