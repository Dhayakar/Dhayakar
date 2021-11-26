using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class SchirmerTest
    {
        [Key]
        public int ID { get; set; }
        public int CmpID { get; set; }
        public int RID { get; set; }
        public DateTime VisitDatetime { get; set; }
        public string Ocular { get; set; }
        public string Time { get; set; }
        public string Tearsecretion { get; set; }
        public int Examinedby { get; set; }
        public Boolean IsActive { get; set; }
        public string Remarks { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

    }
}
