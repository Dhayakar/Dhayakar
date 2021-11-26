﻿
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class VehiclePassmaster
    {
        [Key]
        public int ID { get; set; }
        public int RegistrationTranID { get; set; }
        public string UIN { get; set; }
        public string AdmID { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int CmpID { get; set; }
        public string RandomUniqueID { get; set; }

    }
}









