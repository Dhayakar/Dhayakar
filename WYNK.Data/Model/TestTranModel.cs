﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class TestTran
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string TestMasterID { get; set; }
        public int TestResultID { get; set; }
        public Boolean IsOD { get; set; }
        public Boolean IsOS { get; set; }
        public Boolean IsOU { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public string Remarks { get; set; }


    }
}
