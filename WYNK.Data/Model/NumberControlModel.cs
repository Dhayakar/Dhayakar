using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class Number_Control
    {
        [Key]
        public int VCID { get; set; }
        public int TransactionID { get; set; }
        public int DepartmentID { get; set; }
        public int CmpID { get; set; }
        //public Int32 LocationID { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public string Description { get; set; }
        public bool Autonumber { get; set; }
        public Int64 RunningNumber { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
    }
}

