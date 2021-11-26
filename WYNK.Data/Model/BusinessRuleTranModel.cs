using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
   public  class BusinessRuleTran
    {
        [Key]
        public int ID { get; set; }
        public int BRID { get; set; }
        public int? From { get; set; }
        public int? TO { get; set; }
        public decimal Amount { get; set; }
        public int NoofDays { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public DateTime? EffectiveDate { get; set; }

   }
}

  