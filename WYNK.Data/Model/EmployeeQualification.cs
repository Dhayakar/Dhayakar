using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class EmployeeQualification
    {
        [Key]

        public int ID { get; set; }
        public int Cmpid { get; set; }
        public int QualExtCode { get; set; }
        public int EmpID { get; set; }
        public int UniversityExtCode { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string YearofPass { get; set; }
        public decimal Percentage { get; set; }
        public Boolean IsDeleted { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }


    }
}

