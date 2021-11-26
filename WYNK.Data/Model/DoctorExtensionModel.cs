using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WYNK.Data.Model
{
    public class DoctorExtension
    {
        [Key]
        public int ID { get; set; }
        public int CMPID { get; set; }
        public int DoctorID { get; set; }
        public int? MaxFSPatients { get; set; }
        public int? MaxSSPatient { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public string Day { get; set; }
        public TimeSpan FSFromTime { get; set; }
        public TimeSpan FSToTime { get; set; }
        public TimeSpan SSFromTime { get; set; }
        public TimeSpan SSToTime { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public bool IsDeleted { get; set; }
    }
}
