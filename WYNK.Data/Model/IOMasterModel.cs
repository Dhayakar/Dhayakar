using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class IOMaster
    {
        [Key]
        public int ID { get; set; }
        public DateTime SurgeryPerformedDate { get; set; }
        public int CMPID { get; set; }
        public string ADMID { get; set; }
        public string Surgeryid { get; set; }
        public string AddlNotes { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
    }
}












