using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class OverTime
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int16 ID { get; set; }
        public int CMPID { get; set; }
        public int EmpID { get; set; }
        //public TimeSpan FromTime { get; set; }
        //public TimeSpan ToTime { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public decimal NoofHours { get; set; }
        public Decimal OTperHour { get; set; }
        public Decimal? TotalOTAmount { get; set; }
        public DateTime OTDate { get; set; }
        public Boolean IsProcessed { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public Boolean IsDeleted { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

    }
 }
