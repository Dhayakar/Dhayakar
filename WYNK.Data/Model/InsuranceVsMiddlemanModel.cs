﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
   public class InsuranceVsMiddlemen
    {
        [Key]
        public int ID { get; set; }
        public int IID { get; set; }
        public int? MiddleMenID { get; set; }
        public decimal Amount { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

    }
}
